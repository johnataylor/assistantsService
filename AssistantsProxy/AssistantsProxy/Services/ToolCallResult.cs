using AssistantsProxy.Schema;

namespace AssistantsProxy.Services
{
    public class ToolCallResult : CallResultBase
    {
        public ToolCallResult(IList<RequiredActionFunctionToolCall> toolCalls)
        {
            ToolCalls = toolCalls;
        }

        public IList<RequiredActionFunctionToolCall> ToolCalls { get; init; }
    }
}
