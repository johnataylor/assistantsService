using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IRunsUpdate
    {
        Task SetInProgressAsync(string runId);
        Task SetCompletedAsync(string threadId, string runId);
        Task SetRequiresActionAsync(string threadId, string runId, IList<RequiredActionFunctionToolCall> toolCalls);
    }
}
