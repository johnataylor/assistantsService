using AssistantsProxy.Schema;
using Azure.AI.OpenAI;

namespace AssistantsProxy.Services
{
    public interface IChatClient
    {
        public Task<CallResultBase> CallAsync(ChatCompletionsOptions prompt);
    }
}
