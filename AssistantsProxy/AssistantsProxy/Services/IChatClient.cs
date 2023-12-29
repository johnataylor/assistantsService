using AssistantsProxy.Schema;
using Azure.AI.OpenAI;

namespace AssistantsProxy.Services
{
    public interface IChatClient
    {
        public Task<ThreadMessage> CallAsync(ChatCompletionsOptions prompt);
    }
}
