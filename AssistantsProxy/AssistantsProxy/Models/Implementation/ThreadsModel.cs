using AssistantsProxy.Schema;
using Azure.Storage.Blobs;

namespace AssistantsProxy.Models.Implementation
{
    public class ThreadsModel : IThreadsModel
    {
        private readonly BlobContainerClient _containerClient;
        private const string ContainerName = "threads";
        private readonly MessagesModel _messagesModel;

        public ThreadsModel(IConfiguration configuration)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);

            _messagesModel = new MessagesModel(configuration);
        }

        public Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams, string? bearerToken)
        {
            // TODO

            throw new NotImplementedException();
        }

        public async Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams, string? bearerToken)
        {
            var newThread = new AssistantThread
            {
                Object = "thread",
                Id = $"thread_{Guid.NewGuid()}",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Metadata = threadCreateParams?.Metadata
            };

            await _containerClient.UploadBlobAsync(newThread.Id, new BinaryData(newThread));

            if (threadCreateParams != null && threadCreateParams.Messages != null)
            {
                // TODO: this could be done in a single shot, rather than message at a time

                foreach (var message in threadCreateParams.Messages)
                {
                    await _messagesModel.CreateAsync(newThread.Id, message, bearerToken);
                }
            }

            return newThread;
        }

        public async Task DeleteAsync(string threadId, string? bearerToken)
        {
            await _messagesModel.DeleteMessages(threadId);
            await _containerClient.DeleteBlobAsync(threadId);
        }

        public Task<AssistantThread?> RetrieveAsync(string threadId, string? bearerToken)
        {
            return BlobStorageHelpers.DownloadAsync<AssistantThread>(_containerClient, threadId);
        }

        public async Task<AssistantThread?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams, string? bearerToken)
        {
            var assistantThread = await BlobStorageHelpers.DownloadAsync<AssistantThread>(_containerClient, threadId);

            if (assistantThread == null)
            {
                return null;
            }

            assistantThread.Metadata = threadUpdateParams.Metadata;

            var blobClient = _containerClient.GetBlobClient(threadId);
            await blobClient.UploadAsync(new BinaryData(assistantThread), true);

            if (threadUpdateParams != null && threadUpdateParams.Messages != null)
            {
                // TODO: delete the currtent set of messages
            }

            return assistantThread;
        }
    }
}
