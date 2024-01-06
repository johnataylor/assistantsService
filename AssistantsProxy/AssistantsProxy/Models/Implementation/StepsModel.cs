using AssistantsProxy.Schema;
using Azure.Storage.Blobs;

namespace AssistantsProxy.Models.Implementation
{
    public class StepsModel : IStepsModel
    {
        private readonly BlobContainerClient _containerClient;
        private const string ContainerName = "steps";

        public StepsModel(IConfiguration configuration)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
        }

        public async Task<AssistantList<RunStep>?> ListAsync(string threadId, string runId, string? bearerToken)
        {
            // TODO check the thread and run exist

            var blobName = GetBlobName(threadId, runId);

            var threadRunSteps = await BlobStorageHelpers.DownloadAsync<List<RunStep>>(_containerClient, blobName) ?? new List<RunStep>();

            var assistantList = new AssistantList<RunStep>
            {
                Data = threadRunSteps.ToArray()
            };

            return assistantList;
        }

        public async Task<RunStep?> RetrieveAsync(string threadId, string runId, string stepId, string? bearerToken)
        {
            // TODO check the thread and run exist

            var blobName = GetBlobName(threadId, runId);

            var runSteps = await BlobStorageHelpers.DownloadAsync<List<RunStep>>(_containerClient, blobName) ?? new List<RunStep>();

            var runStep = runSteps.FirstOrDefault(item => item.Id == stepId);

            return runStep;
        }

        internal Task AddMessageCreationStepAsync(string threadId, string runId, string assistantId, string messageId)
        {
            var stepDetails = new MessageCreationStepDetails
            {
                MessageCreation = new MessageCreation { MessageId = messageId },
            };

            return AddStepAsync(threadId, runId, assistantId, "message_creation", "completed", stepDetails);
        }
        internal Task AddFunctionToolCallsStepAsync(string threadId, string runId, string assistantId, FunctionToolCall[] toolCalls)
        {
            var stepDetails = new ToolCallsStepDetails
            {
                ToolCalls = toolCalls,
            };

            return AddStepAsync(threadId, runId, assistantId, "tool_calls", "in_progress", stepDetails);
        }

        private async Task AddStepAsync(string threadId, string runId, string assistantId, string type, string status, StepDetailsBase stepDetails)
        {
            var newRunStep = new RunStep
            {
                Object = "thread.run.step",
                Id = $"step_{Guid.NewGuid()}",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                StepDetails = stepDetails,
                ThreadId = threadId,
                RunId = runId,
                AssistantId = assistantId,
                Type = type,
                Status = status
            };

            var blobName = GetBlobName(threadId, runId);
            var blobClient = _containerClient.GetBlobClient(blobName);

            var runSteps = await blobClient.ExistsAsync()
                    ?
                await BlobStorageHelpers.DownloadAsync<List<RunStep>>(_containerClient, blobName) ?? new List<RunStep>()
                    :
                new List<RunStep>();

            runSteps.Add(newRunStep);

            await blobClient.UploadAsync(new BinaryData(runSteps), true);
        }

        private string GetBlobName(string threadId, string runId) => $"{threadId}_{runId}_steps";
    }
}
