using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IStepsModel
    {
        Task<RunStep?> RetrieveAsync(string threadId, string runId, string stepId, string? bearerToken);
        Task<AssistantList<RunStep>?> ListAsync(string threadId, string runId, string? bearerToken);
    }
}
