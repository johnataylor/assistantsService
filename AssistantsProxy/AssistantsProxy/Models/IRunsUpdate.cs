using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IRunsUpdate
    {
        Task SetInProgressAsync(string runId);
        Task SetCompletedAsync(string runId);
        Task SetRequiresActionAsync(string runId, IList<RequiredActionFunctionToolCall> toolCalls);
    }
}
