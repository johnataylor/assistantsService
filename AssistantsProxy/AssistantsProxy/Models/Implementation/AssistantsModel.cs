using AssistantsProxy.Schema;
using Azure.Storage.Blobs;

namespace AssistantsProxy.Models.Implementation
{
    public class AssistantsModel : IAssistantsModel
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<AssistantsModel> _logger;
        private const string ContainerName = "assistants";

        public AssistantsModel(IConfiguration configuration, ILogger<AssistantsModel> logger)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
            _logger = logger;
        }

        public async Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams, string? bearerToken)
        {
            var newAssistant = new Assistant
            {
                Id = $"asst_{Guid.NewGuid()}",
                Object = "assistant",
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Model = assistantCreateParams.Model,
                Instructions = assistantCreateParams.Instructions,
                Name = assistantCreateParams.Name,
                Description = assistantCreateParams.Description,
                Metadata = assistantCreateParams.Metadata,
                Tools = assistantCreateParams.Tools,
                FileIds = assistantCreateParams.FileIds
            };

            await _containerClient.UploadBlobAsync(newAssistant.Id, new BinaryData(newAssistant));

            _logger.LogInformation($"Create Assistant {newAssistant.Id}");

            return newAssistant;
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

        public async Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams, string? bearerToken)
        {
            // TODO: add appropriate eTag checks and retries for concurrency control

            var assistant = await BlobStorageHelpers.DownloadAsync<Assistant>(_containerClient, assistantId);

            if (assistant == null)
            {
                return null;
            }

            assistant.Model = assistantUpdateParams.Model ?? assistant.Model;
            assistant.Instructions = assistantUpdateParams.Instructions ?? assistant.Instructions;
            assistant.Name = assistantUpdateParams.Name ?? assistant.Name;
            assistant.Description = assistantUpdateParams.Description ?? assistant.Description;
            assistant.Metadata = assistantUpdateParams.Metadata ?? assistant.Metadata;
            assistant.Tools = assistantUpdateParams.Tools ?? assistant.Tools;
            assistant.FileIds = assistantUpdateParams.FileIds ?? assistant.FileIds;

            var blobClient = _containerClient.GetBlobClient(assistantId);
            await blobClient.UploadAsync(new BinaryData(assistant), true);

            return assistant;
        }
    }
}
