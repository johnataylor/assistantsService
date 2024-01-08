﻿using AssistantsProxy.Schema;
using Azure.AI.OpenAI;

namespace AssistantsProxy.Services
{
    public static class PromptFactory
    {
        // TODO: the contract here is a work in progress, for example, not all of the assistant and run structures are required to make the options

        public static ChatCompletionsOptions Create(Assistant assistant, AssistantThread asssitantThread, ThreadRun run, ThreadMessage[] currentMessages, RunSubmitToolOutputsParams? toolOutputs)
        {
            var options = new ChatCompletionsOptions();

            // Instructuions and Tools on this Run can override the values on the Assistant
            var instructions = run.Instructions ?? assistant.Instructions;
            var tools = run.Tools ?? assistant.Tools;

            if (instructions != null)
            {
                options.Messages.Add(new ChatRequestSystemMessage(assistant.Instructions));
            }

            AddChatRequestMessages(options.Messages, currentMessages);
            AddChatRequestTools(options.Tools, tools);

            return options;
        }

        private static void AddChatRequestTools(IList<ChatCompletionsToolDefinition> chatRequestFunctions, AssistantToolsBase[]? tools)
        {
            if (tools != null)
            {
                foreach (var tool in tools)
                {
                    if (tool is AssistantToolsFunction assistantToolsFunction && assistantToolsFunction.Function != null)
                    {
                        var parametersJson = assistantToolsFunction.Function.Parameters?.ToJsonString() ?? "{}";

                        var chatCompletionsFunctionToolDefinition = new ChatCompletionsFunctionToolDefinition
                        {
                            Name = assistantToolsFunction.Function.Name,
                            Description = assistantToolsFunction.Function.Description,
                            Parameters = BinaryData.FromString(parametersJson),
                        };

                        chatRequestFunctions.Add(chatCompletionsFunctionToolDefinition);
                    }
                    else if (tool is AssistantToolsCode assistantToolsCode)
                    {
                        throw new NotImplementedException();
                    }
                    else if (tool is AssistantToolsRetrieval assistantToolsRetrieval)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
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
