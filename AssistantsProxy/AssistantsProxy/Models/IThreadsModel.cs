using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IThreadsModel
    {
        Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams, string? bearerToken);
        Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams, string? bearerToken);
        Task<AssistantThread?> RetrieveAsync(string threadId, string? bearerToken);
        Task<AssistantThread?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams, string? bearerToken);
        Task DeleteAsync(string threadId, string? bearerToken);
    }
}