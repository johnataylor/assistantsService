using AssistantsProxy.Schema;
using Azure.Storage.Blobs;
using System.Text.Json;

namespace AssistantsProxy.Models.Implementation
{
    public class AssistantsModel : IAssistantsModel
    {
        private readonly BlobContainerClient _containerClient;
        private const string ContainerName = "assistants";

        public AssistantsModel(IConfiguration configuration)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
        }

        public async Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams, string? bearerToken)
        {
            var assistant = new Assistant
            {
                Object = "assistant",
                Id = $"asst_{Guid.NewGuid()}",
                Name = assistantCreateParams.Name,
                Instructions = assistantCreateParams.Instructions,
                Tools = assistantCreateParams.Tools,
                Model = assistantCreateParams.Model,
                CreateAt = DateTime.UtcNow.Ticks        // TODO - unix timestamp apparently
            };

            await _containerClient.UploadBlobAsync(assistant.Id, BinaryData.FromString(JsonSerializer.Serialize(assistant)));

            return assistant;
        }

        public Task DeleteAsync(string assistantId, string? bearerToken)
        {
            return _containerClient.DeleteBlobAsync(assistantId);
        }

        public Task<AssistantList<Assistant>?> ListAsync(string? bearerToken)
        {
            // TODO - this doesn't yet support paging/listing

            return BlobStorageHelpers.ListAsync<Assistant>(_containerClient);
        }

        public Task<Assistant?> RetrieveAsync(string assistantId, string? bearerToken)
        {
            return BlobStorageHelpers.DownloadAsync<Assistant>(_containerClient, assistantId);
        }

        public Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams, string? bearerToken)
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
