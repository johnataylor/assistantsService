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

        public async Task<CallResultBase> CallAsync(ChatCompletionsOptions chatCompletionsOptions)
        {
            chatCompletionsOptions.DeploymentName = _deploymentOrModelName;
            var chatComplations = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

            if (chatComplations.GetRawResponse().IsError)
            {
                throw new Exception("error on call to GPT");
            }

            var chatChoice = chatComplations.Value.Choices[0] ?? throw new Exception("no choice!");

            if (chatChoice.FinishReason == CompletionsFinishReason.Stopped)
            {
                return new MessageCallResult(chatChoice.Message.Content);
            }
            else if (chatChoice.FinishReason == CompletionsFinishReason.ToolCalls)
            {
                var toolCalls = new List<RequiredActionFunctionToolCall>();

                foreach (var chatCompletionToolCall in chatChoice.Message.ToolCalls)
                {
                    if (chatCompletionToolCall is ChatCompletionsFunctionToolCall chatCompletionsFunctionToolCall)
                    {
                        var functionName = chatCompletionsFunctionToolCall.Name;
                        var arguments = chatCompletionsFunctionToolCall.Arguments;

                        var newToolCall = new RequiredActionFunctionToolCall
                        {
                            Id = Guid.NewGuid().ToString(),
                            Function = new RequiredActionFunctionToolCallFunction
                            {
                                Name = functionName,
                                Arguments = arguments
                            },
                            Type = "function"
                        };

                        toolCalls.Add(newToolCall);
                    }
                }

                return new ToollCallResult(toolCalls);
            }
            else if (chatChoice.FinishReason == CompletionsFinishReason.FunctionCall)
            {
                throw new Exception("FinishReason: function call is deprecated");
            }
            else if (chatChoice.FinishReason == CompletionsFinishReason.TokenLimitReached)
            {
                throw new Exception("FinishReason: token limit reached");
            }
            else if (chatChoice.FinishReason == CompletionsFinishReason.ContentFiltered)
            {
                throw new Exception("FinishReason: content filtered");
            }
            else
            {
                throw new Exception("FinishReason: unrecognized");
            }
        }
    }
}
