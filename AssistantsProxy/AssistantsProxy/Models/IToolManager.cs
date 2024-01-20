using AssistantsProxy.Schema;

namespace AssistantsProxy.Models
{
    public interface IToolManager
    {
        Task<IList<RequiredActionFunctionToolCall>> CreateRendezvousAndEnqueueServerToolsAsync(string threadId, string runId, IList<RequiredActionFunctionToolCall> toolCalls);
        Task<Rendezvous?> UpdateRendezvousAndCheckForCompletionAsync(string threadId, string runId, RunSubmitToolOutputsParams runSubmitToolOutputsParams);
        Task DeleteRendezvousAsync(string threadId, string runId);
    }
}
