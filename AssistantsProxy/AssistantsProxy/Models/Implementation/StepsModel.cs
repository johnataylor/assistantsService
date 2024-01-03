using AssistantsProxy.Schema;

namespace AssistantsProxy.Models.Implementation
{
    public class StepsModel : IStepsModel
    {
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
