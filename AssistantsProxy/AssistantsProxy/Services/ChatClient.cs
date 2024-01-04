using AssistantsProxy.Schema;
using Azure.AI.OpenAI;

namespace AssistantsProxy.Services
{
    public class ChatClient : IChatClient
    {
        private readonly OpenAIClient _client;
        private readonly string _deploymentOrModelName;

        public ChatClient(IConfiguration configuration)
        {
            _deploymentOrModelName = configuration["DeploymentOrModelName"] ?? "gpt-4-1106-preview";
            var openAIKey = configuration["OpenAIKey"] ?? throw new ArgumentException("missing configuration for OpenAIKey");
            _client = new OpenAIClient(openAIKey);
        }

        public async Task<ThreadMessage> CallAsync(ChatCompletionsOptions chatCompletionsOptions)
        {
            chatCompletionsOptions.DeploymentName = _deploymentOrModelName;
            var chatComplations = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

            if (chatComplations.GetRawResponse().IsError)
            {
                throw new Exception("error on call to GPT");
            }

            var chatChoice = chatComplations.Value.Choices[0] ?? throw new Exception("what?! we have no choice!");

            if (chatChoice.FinishReason == CompletionsFinishReason.FunctionCall)
            {
                // TODO implement Function handling

                throw new NotImplementedException("FunctionCall not implemented");
            }
            else
            {
                return new ThreadMessage
                {
                    Content = new[]
                    {
                        new MessageContent { Text = new MessageContentText { Value = chatChoice.Message.Content } }
                    },
                    Role = "assistant"
                };
            }
        }
    }
}
