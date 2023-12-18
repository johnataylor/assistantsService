using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IAssistantsModel
    {
        Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams, string? bearerToken);
        Task<AssistantList<Assistant>?> ListAsync(string? bearerToken);
        Task<Assistant?> RetrieveAsync(string assistantId, string? bearerToken);
        Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams, string? bearerToken);
        Task DeleteAsync(string assistantId, string? bearerToken);
    }
}
