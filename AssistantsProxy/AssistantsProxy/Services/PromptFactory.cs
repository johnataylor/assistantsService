using AssistantsProxy.Schema;
using Azure.AI.OpenAI;

namespace AssistantsProxy.Services
{
    public static class PromptFactory
    {
        public static ChatCompletionsOptions Create(Assistant? assistant, AssistantThread? asssitantThread, ThreadRun? run, ThreadMessage[]? currentMessages)
        {
            var options = new ChatCompletionsOptions();

            return options;
        }
    }
}
