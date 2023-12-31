using AssistantsProxy.Schema;
using AssistantsProxy.Services;
using Azure.Storage.Blobs;

namespace AssistantsProxy.Models.Implementation
{
    public class RunsModel : IRunsModel
    {
        private readonly BlobContainerClient _containerClient;
        private readonly IRunsWorkQueue<RunsWorkItemValue> _queue;

        private const string ContainerName = "runs";

        public RunsModel(IConfiguration configuration, IRunsWorkQueue<RunsWorkItemValue> queue)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
            _queue = queue;
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
                CreateAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await _containerClient.UploadBlobAsync(newThreadRun.Id, new BinaryData(newThreadRun));

            await _queue.EnqueueAsync(new RunsWorkItemValue(assistantId, threadId, newThreadRun.Id));

            return newThreadRun;
        }

        public Task<ThreadRun?> RetrieveAsync(string threadId, string runId, string? bearerToken)
        {
            return BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);
        }

        public Task<ThreadRun?> UpdateAsync(string threadId, string runId, RunUpdateParams runUpdateParams, string? bearerToken)
        {
            throw new NotImplementedException();
        }
        public Task<ThreadRun?> CancelAsync(string threadId, string runId, string? bearerToken)
        {
            // delete the run

            throw new NotImplementedException();
        }

        internal async Task SetStatus(string runId, string status)
        {
            var threadRun = await BlobStorageHelpers.DownloadAsync<ThreadRun>(_containerClient, runId);

            threadRun = threadRun ?? throw new ArgumentNullException(nameof(threadRun));

            threadRun.Status = status;

            var blobClient = _containerClient.GetBlobClient(runId);
            await blobClient.UploadAsync(new BinaryData(threadRun), true);
        }
    }
}
