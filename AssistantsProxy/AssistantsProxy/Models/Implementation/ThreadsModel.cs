using AssistantsProxy.Schema;
using Azure.Storage.Blobs;
using System.Text.Json;

namespace AssistantsProxy.Models.Implementation
{
    public class ThreadsModel : IThreadsModel
    {
        private readonly BlobContainerClient _containerClient;
        private const string ContainerName = "threads";

        public ThreadsModel(IConfiguration configuration)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
        }

        public Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams, string? bearerToken)
        {
            // TODO

            throw new NotImplementedException();
        }

        public async Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams, string? bearerToken)
        {
            var thread = new AssistantThread
            {
                Object = "thread",
                Id = $"thread_{Guid.NewGuid()}",
                CreateAt = DateTime.UtcNow.Ticks        // TODO - unix timestamp apparently
            };

            await _containerClient.UploadBlobAsync(thread.Id, BinaryData.FromString(JsonSerializer.Serialize(thread)));

            return thread;
        }

        public Task DeleteAsync(string threadId, string? bearerToken)
        {
            return _containerClient.DeleteBlobAsync(threadId);
        }

        public Task<AssistantThread?> RetrieveAsync(string threadId, string? bearerToken)
        {
            return BlobStorageHelpers.DownloadAsync<AssistantThread>(_containerClient, threadId);
        }

        public Task<AssistantThread?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams, string? bearerToken)
        {
            // TODO

            // load
            // update
            // save
            // return

            throw new NotImplementedException();
        }
    }
}
