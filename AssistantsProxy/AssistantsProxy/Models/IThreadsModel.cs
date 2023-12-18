using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IThreadsModel
    {
        Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams, string? bearerToken);
        Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams, string? bearerToken);
        Task<Assistant?> RetrieveAsync(string threadId, string? bearerToken);
        Task<Assistant?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams, string? bearerToken);
        Task DeleteAsync(string threadId, string? bearerToken);
    }
}