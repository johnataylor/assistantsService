using AssistantsProxy.Schema;
using AssistantsProxy.Services;
using Azure.Storage.Blobs;

namespace AssistantsProxy.Models.Implementation
{
    public class ToolManager : IToolManager
    {
        private readonly BlobContainerClient _containerClient;
        private readonly IWorkItemQueue<RetrievalWorkItem> _retrievalQueue;
        private readonly ILogger<MessagesModel> _logger;
        private const string ContainerName = "rendezvous";
        private static readonly HashSet<string> _serverToolNames = new HashSet<string> { "retrieval", "code_interpreter" };

        public ToolManager(IConfiguration configuration, IWorkItemQueue<RetrievalWorkItem> retrievalQueue, ILogger<MessagesModel> logger)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
            _retrievalQueue = retrievalQueue;
            _logger = logger;
        }

        public async Task<IList<RequiredActionFunctionToolCall>> CreateRendezvousAndEnqueueServerToolsAsync(string threadId, string runId, IList<RequiredActionFunctionToolCall> toolCalls)
        {
            var rendezvousItems = new List<RendezvousItem>();

            var clientToolCalls = new List<RequiredActionFunctionToolCall>();

            foreach (var toolCall in toolCalls)
            {
                var rendezvousItem = new RendezvousItem { ToolCallId = toolCall.Id };

                if (_serverToolNames.Contains(toolCall.Function.Name))
                {
                    rendezvousItem.ServerToolCallFunction = toolCall.Function;

                    switch (toolCall.Function.Name)
                    {
                        case "retrieval":
                            await _retrievalQueue.EnqueueAsync(new RetrievalWorkItem
                            {
                                ThreadId = threadId,
                                RunId = runId,
                                Id = toolCall.Id,
                                Function = toolCall.Function
                            });
                            break;
                        case "code_interpreter":
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    clientToolCalls.Add(toolCall);
                }

                rendezvousItems.Add(rendezvousItem);
            }

            var rendezvous = new Rendezvous
            {
                Items = rendezvousItems.ToArray()
            };

            // overwrite blob because we are retrying... TODO: check this reasoning
            var blobClient = _containerClient.GetBlobClient(GetBlobName(runId));
            await blobClient.UploadAsync(new BinaryData(rendezvous), true);

            return clientToolCalls;
        }

        public Task DeleteRendezvousAsync(string threadId, string runId)
        {
            return Task.CompletedTask;
        }

        public async Task<Rendezvous?> UpdateRendezvousAndCheckForCompletionAsync(string threadId, string runId, RunSubmitToolOutputsParams runSubmitToolOutputsParams)
        {
            var rendezvous = await BlobStorageHelpers.DownloadAsync<Rendezvous>(_containerClient, GetBlobName(runId));

            foreach (var toolOutput in runSubmitToolOutputsParams.ToolOutputs!)
            {
                var rendezvousToolOutput = rendezvous?.Items.FirstOrDefault(rt => rt.ToolCallId == toolOutput.ToolCallId);
                if (rendezvousToolOutput != null)
                {
                    rendezvousToolOutput.Output = toolOutput.Output;
                }
            }

            var blobClient = _containerClient.GetBlobClient(GetBlobName(runId));
            await blobClient.UploadAsync(new BinaryData(rendezvous), true);

            if (rendezvous?.Items != null && rendezvous.Items.Any(rt => rt.Output == null))
            {
                return null;
            }

            return rendezvous;
        }

        private string GetBlobName(string runId) => $"{runId}_rendezvous";
    }
}
