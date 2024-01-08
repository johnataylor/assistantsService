using AssistantsProxy.Schema;

namespace AssistantsProxy.Services
{
    public class ToollCallResult : CallResultBase
    {
        public ToollCallResult(IList<RequiredActionFunctionToolCall> toolCalls)
        {
            ToolCalls = toolCalls;
        }

        public IList<RequiredActionFunctionToolCall> ToolCalls { get; init; }
    }
}
