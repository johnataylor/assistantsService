using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IMessagesModel
    {
        Task<ThreadMessage?> CreateAsync(string threadId, MessageCreateParams messageCreateParams, string? bearerToken);
        Task<ThreadMessage?> RetrieveAsync(string threadId, string messageId, string? bearerToken);
        Task<ThreadMessage?> UpdateAsync(string threadId, string messageId, MessageUpdateParams messageUpdateParams, string? bearerToken);
        Task<AssistantList<ThreadMessage>?> ListAsync(string threadId, string? bearerToken);
    }
}
