using AssistantsProxy.Models;
using AssistantsProxy.Schema;
using Azure.AI.OpenAI;
using System.Reflection;

namespace AssistantsProxy.Services
{
    public static class PromptFactory
    {
        // TODO: the contract here is a work in progress, for example, not all of the assistant and run structures are required to make the options

        public static ChatCompletionsOptions Create(Assistant assistant, ThreadRun run, ThreadMessage[] currentMessages, Rendezvous? rendezvous)
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
            AddToolCallsAndOutputs(options.Messages, run, rendezvous);
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
                    else if (tool is AssistantToolsRetrieval)
                    {
                        chatRequestFunctions.Add(ServerToolDefinitions.RetrievalToolDefintion);
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

        private static void AddToolCallsAndOutputs(IList<ChatRequestMessage> chatRequestMessages, ThreadRun run, Rendezvous? rendezvous)
        {
            if (rendezvous != null && rendezvous.Items != null)
            {
                // create a single assistant message including all the tools - both the client tools and the server tools

                var assistantMessage = new ChatRequestAssistantMessage(string.Empty);

                // client tools
                if (run?.RequiredAction?.SubmitToolOutputs?.ToolCalls != null)
                {
                    foreach (var toolCall in run.RequiredAction.SubmitToolOutputs.ToolCalls)
                    {
                        if (toolCall.Function != null && toolCall.Function.Name != null && toolCall.Function.Arguments != null)
                        {
                            var toolCallObj = new ChatCompletionsFunctionToolCall(toolCall.Id, toolCall.Function.Name, toolCall.Function.Arguments);

                            // this is a work around - the "type" property appears to be null if this constructor has been used
                            // OpenAI fails the call if the "type" property is null - it should be "function"
                            // there appears to be a fix in the works: https://github.com/Azure/azure-sdk-for-net/commit/fb422f67e1c3f8ed86304c3fc6e1f8df8a5e8dcd
                            toolCallObj.GetType().InvokeMember("Type",
                                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty,
                                Type.DefaultBinder, toolCallObj, new object[] { "function" });

                            assistantMessage.ToolCalls.Add(toolCallObj);
                        }
                    }
                }

                // server tools
                foreach (var rendezvousItem in rendezvous.Items)
                {
                    var function = rendezvousItem.ServerToolCallFunction;

                    if (function != null && function.Name == "retrieval")
                    {
                        var toolCallObj = new ChatCompletionsFunctionToolCall(rendezvousItem.ToolCallId, function.Name, function.Arguments);

                        // see comment above
                        toolCallObj.GetType().InvokeMember("Type",
                            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty,
                            Type.DefaultBinder, toolCallObj, new object[] { "function" });

                        assistantMessage.ToolCalls.Add(toolCallObj);
                    }
                }

                chatRequestMessages.Add(assistantMessage);

                // and then for each tool call output we have, add a tool message

                foreach (var rendezvousItem in rendezvous.Items)
                {
                    chatRequestMessages.Add(new ChatRequestToolMessage(rendezvousItem.Output, rendezvousItem.ToolCallId));
                }
            }
        }
    }
}
