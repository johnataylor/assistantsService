using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IRunsModel
    {
        Task<ThreadRun?> CreateAsync(string threadId, RunCreateParams runCreateParams, string? bearerToken);
        Task<ThreadRun?> RetrieveAsync(string threadId, string runId, string? bearerToken);
        Task<ThreadRun?> UpdateAsync(string threadId, string runId, RunUpdateParams runUpdateParams, string? bearerToken);
        Task<ThreadRun?> CancelAsync(string threadId, string runId, string? bearerToken);
    }
}
