using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IStepsUpdate
    {
        Task AddMessageCreationStepAsync(string threadId, string runId, string assistantId, string messageId);
        Task AddFunctionToolCallsStepAsync(string threadId, string runId, string assistantId, FunctionToolCall[] toolCalls);
        Task UpdateFunctionToolCallsStepAsync(string threadId, string runId, Rendezvous rendezvous);
    }
}
