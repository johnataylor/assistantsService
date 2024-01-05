using AssistantsProxy.Schema;
using Azure.AI.OpenAI;

namespace AssistantsProxy.Services
{
    public static class PromptFactory
    {
        public static ChatCompletionsOptions Create(Assistant assistant, AssistantThread asssitantThread, ThreadRun run, ThreadMessage[] currentMessages)
        {
            var tools = assistant.Tools ?? throw new ArgumentNullException(nameof(assistant.Tools));

            var options = new ChatCompletionsOptions();

            if (assistant.Instructions != null)
            {
                options.Messages.Add(new ChatRequestSystemMessage(assistant.Instructions));
            }

            //foreach (var tool in tools)
            //{
            //    options.Functions.Add(CreateFunctionDefinition(tool));
            //}

            AddChatRequestMessages(options.Messages, currentMessages);

            return options;
        }

        private static FunctionDefinition CreateFunctionDefinition(AssistantToolsFunction tool)
        {
            return new FunctionDefinition();
        }

        private static void AddChatRequestMessages(IList<ChatRequestMessage> chatRequestMessages, ThreadMessage[] currentMessages)
        {
            foreach (var message in currentMessages)
            {
                switch (message.Role)
                {
                    case "user":
                        if (message.Content != null)
                        {
                            foreach (var content in message.Content)
                            {
                                var value = content?.Text?.Value;
                                if (value != null)
                                {
                                    chatRequestMessages.Add(new ChatRequestUserMessage(value));
                                }
                            }
                        }
                        break;
                    case "assistant":
                        if (message.Content != null)
                        {
                            foreach (var content in message.Content)
                            {
                                var value = content?.Text?.Value;
                                if (value != null)
                                {
                                    chatRequestMessages.Add(new ChatRequestAssistantMessage(value));
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
