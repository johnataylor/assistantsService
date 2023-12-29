using AssistantsProxy.Schema;
using Azure.AI.OpenAI;

namespace AssistantsProxy.Services
{
    public class ChatClient : IChatClient
    {
        public ChatClient(IConfiguration configuration)
        {
        }

        public Task<ThreadMessage> CallAsync(ChatCompletionsOptions prompt)
        {
            throw new NotImplementedException();
        }
    }
}
