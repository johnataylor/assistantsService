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

        public Task<AssistantList<RunStep>?> ListAsync(string threadId, string runId, string? bearerToken)
        {
            throw new NotImplementedException();
        }

        public Task<RunStep?> RetrieveAsync(string threadId, string runId, string stepId, string? bearerToken)
        {
            throw new NotImplementedException();
        }
    }
}
