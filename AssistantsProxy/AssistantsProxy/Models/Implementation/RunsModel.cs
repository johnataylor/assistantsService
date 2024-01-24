using AssistantsProxy.Hubs;
using AssistantsProxy.Schema;
using AssistantsProxy.Services;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.SignalR;

namespace AssistantsProxy.Models.Implementation
{
    public class RunsModel : IRunsModel, IRunsUpdate
    {
        private readonly BlobContainerClient _containerClient;
        private readonly IWorkItemQueue<RunsWorkItemValue> _queue;
        private readonly IToolManager _toolManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<RunsModel> _logger;
        private const string ContainerName = "runs";

        public RunsModel(IConfiguration configuration, IWorkItemQueue<RunsWorkItemValue> queue, IToolManager toolManager, IHubContext<NotificationHub> hubContext, ILogger<RunsModel> logger)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
            _queue = queue;
            _toolManager = toolManager;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<ThreadRun?> CreateAsync(string threadId, RunCreateParams runCreateParams, string? bearerToken)
        {
            // validate threadId and assistantId

            var assistantId = runCreateParams.AssistantId ?? throw new ArgumentNullException("runCreateParams.AssistantId");

            var newThreadRun = new ThreadRun
            {
                Object = "assistant.run",
                Id = $"run_{Guid.NewGuid()}",
                Status = "queued",
                AssistantId = assistantId,
                ThreadId = threadId,
                Model = runCreateParams.Model,
                Instructions = runCreateParams.Instructions,
                Tools = runCreateParams.Tools,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _containerClient.UploadBlobAsync(newThreadRun.Id, new BinaryData(newThreadRun));

            await _queue.EnqueueAsync(new RunsWorkItemValue(assistantId, threadId, newThreadRun.Id, null));

            _logger.LogInformation($"Create Run {newThreadRun.Id}");

            return newThreadRun;
        }

        public Task<ThreadRun?> RetrieveAsync(string threadId, string runId, string? bearerToken)
        {
            return BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);
        }

        public Task<ThreadRun?> UpdateAsync(string threadId, string runId, RunUpdateParams runUpdateParams, string? bearerToken)
        {
            // TODO: and add appropriate eTag checks and retries for concurrency control

            throw new NotImplementedException();
        }
        public async Task<ThreadRun?> CancelAsync(string threadId, string runId, string? bearerToken)
        {
            // TODO: add appropriate eTag checks and retries for concurrency control

            var threadRun = await BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);
            threadRun = threadRun ?? throw new ArgumentNullException(nameof(threadRun));

            threadRun.Status = "cancelled";
            threadRun.CancelledAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var blobClient = _containerClient.GetBlobClient(runId);
            await blobClient.UploadAsync(new BinaryData(threadRun), true);

            return threadRun;
        }
        public async Task<ThreadRun?> SubmitToolsOutputs(string threadId, string runId, RunSubmitToolOutputsParams runSubmitToolOutputsParams, string? bearerToken)
        {
            // TODO: add appropriate eTag checks and retries for concurrency control

            var threadRun = await BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);
            threadRun = threadRun ?? throw new ArgumentNullException(nameof(threadRun));

            // possibly split this method for internal and external updates (they have different validation)

            //ValidateToolOutputIds(threadRun, runSubmitToolOutputsParams);

            var rendezvous = await _toolManager.UpdateRendezvousAndCheckForCompletionAsync(threadId, runId, runSubmitToolOutputsParams);

            if (rendezvous != null)
            {
                await _queue.EnqueueAsync(new RunsWorkItemValue(threadRun.AssistantId, threadId, runId, rendezvous));

                await _toolManager.DeleteRendezvousAsync(threadId, runId);
            }

            threadRun.Status = "in_progress";
            
            // presumably the client should see null at this stage - however, we are also using this state to drive the backend
            //threadRun.RequiredAction = null;

            var blobClient = _containerClient.GetBlobClient(runId);
            await blobClient.UploadAsync(new BinaryData(threadRun), true);

            return threadRun;
        }
        public async Task SetCompletedAsync(string threadId, string runId)
        {
            // TODO: add appropriate eTag checks and retries for concurrency control

            var threadRun = await BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);
            threadRun = threadRun ?? throw new ArgumentNullException(nameof(threadRun));

            threadRun.Status = "completed";
            threadRun.CompletedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var blobClient = _containerClient.GetBlobClient(runId);
            await blobClient.UploadAsync(new BinaryData(threadRun), true);

            await _hubContext.Clients.All.SendAsync("notification", new Notification { ThreadId = threadId, RunId = runId, Status = threadRun.Status });
        }
        public async Task SetInProgressAsync(string runId)
        {
            // TODO: add appropriate eTag checks and retries for concurrency control

            var threadRun = await BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);
            threadRun = threadRun ?? throw new ArgumentNullException(nameof(threadRun));

            threadRun.Status = "in_progress";

            var blobClient = _containerClient.GetBlobClient(runId);
            await blobClient.UploadAsync(new BinaryData(threadRun), true);
        }

        public async Task SetRequiresActionAsync(string threadId, string runId, IList<RequiredActionFunctionToolCall> toolCalls)
        {
            // TODO: add appropriate eTag checks and retries for concurrency control

            var clientToolCalls = await _toolManager.CreateRendezvousAndEnqueueServerToolsAsync(threadId, runId, toolCalls);

            if (!clientToolCalls.Any())
            {
                return;
            }

            var threadRun = await BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);
            threadRun = threadRun ?? throw new ArgumentNullException(nameof(threadRun));

            threadRun.Status = "requires_action";
            threadRun.RequiredAction = new RequiredAction
            {
                Type = "submit_tool_outputs",
                SubmitToolOutputs = new SubmitToolOutputs { ToolCalls = clientToolCalls.ToArray() }
            };

            var blobClient = _containerClient.GetBlobClient(runId);
            await blobClient.UploadAsync(new BinaryData(threadRun), true);

            await _hubContext.Clients.All.SendAsync("notification", new Notification { ThreadId = threadId, RunId = runId, Status = threadRun.Status });
        }

        private void ValidateToolOutputIds(ThreadRun? threadRun, RunSubmitToolOutputsParams runSubmitToolOutputsParams)
        {
            var toolCalls = threadRun?.RequiredAction?.SubmitToolOutputs?.ToolCalls ?? new RequiredActionFunctionToolCall[0] { };
            var expectedIds = new HashSet<string>(toolCalls.Select(toolCall => toolCall?.Id ?? string.Empty));

            var toolOutputs = runSubmitToolOutputsParams.ToolOutputs ?? new ToolOutput[0] { };
            var submittedIds = new HashSet<string>(toolOutputs.Select(toolOutput => toolOutput?.ToolCallId ?? string.Empty));

            foreach (var expectedId in expectedIds)
            {
                if (!submittedIds.Contains(expectedId))
                {
                    throw new Exception("missing expected id in tool outputs");
                }
            }
            foreach (var submittedId in submittedIds)
            {
                if (!expectedIds.Contains(submittedId))
                {
                    throw new Exception("unexpected if in tool outpouts");
                }
            }
        }
    }
}
