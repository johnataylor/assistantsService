using AssistantsProxy.Models;
using AssistantsProxy.Schema;

namespace AssistantsProxy.Services
{
    public class RunExecutor : IRunExecutor
    {
        private readonly IAssistantsModel _assistantsModel;
        private readonly IThreadsModel _threadsModel;
        private readonly IMessagesModel _messagesModel;
        private readonly IRunsModel _runsModel;
        private readonly IRunsUpdate _runsUpdate;
        private readonly IStepsUpdate _stepsUpdate;
        private readonly IChatClient _chatClient;

        public RunExecutor(
            IAssistantsModel assistantsModel,
            IThreadsModel threadsModel,
            IMessagesModel messagesModel,
            IRunsModel runsModel,
            IRunsUpdate runsUpdate,
            IStepsUpdate stepsModel,
            IChatClient chatClient)
        {
            _assistantsModel = assistantsModel;
            _threadsModel = threadsModel;
            _messagesModel = messagesModel;
            _runsModel = runsModel;
            _runsUpdate = runsUpdate;
            _stepsUpdate = stepsModel;
            _chatClient = chatClient;
        }

        public async Task ProcessWorkItemAsync(RunsWorkItemValue workItem)
        {
            // Load metadata

            var assistant = await _assistantsModel.RetrieveAsync(workItem.AssistantId, null) ?? throw new KeyNotFoundException(workItem.AssistantId);

            var asssitantThread = await _threadsModel.RetrieveAsync(workItem.ThreadId, null) ?? throw new KeyNotFoundException(workItem.ThreadId);

            var run = await _runsModel.RetrieveAsync(workItem.ThreadId, workItem.RunId, null) ?? throw new KeyNotFoundException(workItem.RunId);

            await _runsUpdate.SetInProgressAsync(workItem.RunId);

            var messageListResponse = await _messagesModel.ListAsync(workItem.ThreadId, null);

            var currentMessages = messageListResponse?.Data ?? throw new KeyNotFoundException($"messages for thread '{workItem.ThreadId}'");

            if (workItem.RunSubmitToolOutputsParams != null)
            {
                await _stepsUpdate.UpdateFunctionToolCallsStepAsync(workItem.ThreadId, workItem.RunId, workItem.RunSubmitToolOutputsParams);
            }

            var prompt = PromptFactory.Create(assistant, run, currentMessages, workItem.RunSubmitToolOutputsParams);

            var callResult = await _chatClient.CallAsync(prompt);

            if (callResult is MessageCallResult messageCallResult)
            {
                var messageCreateParams = new MessageCreateParams
                {
                    Content = messageCallResult.Content,
                    Role = "assistant"
                };

                var newThreadMessage = await _messagesModel.CreateAsync(workItem.ThreadId, messageCreateParams, null);

                // TODO use MessageManager to manage the conversation history
                // var updatedMessages = MessageManager.Update(currentMessages, newMessage);
                // save updated messages

                var newThreadMessageId = newThreadMessage?.Id ?? throw new Exception("create new message failed");

                await _stepsUpdate.AddMessageCreationStepAsync(workItem.ThreadId, workItem.RunId, workItem.AssistantId, newThreadMessageId);

                await _runsUpdate.SetCompletedAsync(workItem.RunId);
            }
            else if (callResult is ToolCallResult toolCallResult)
            {
                await _runsUpdate.SetRequiresActionAsync(workItem.RunId, toolCallResult.ToolCalls);

                var stepToolCalls = toolCallResult.ToolCalls.Select(toolCall =>
                    new FunctionToolCall
                    {
                        Id = toolCall.Id,
                        Function = new FunctionToolCallFunction { Name = toolCall.Function?.Name, Arguments = toolCall.Function?.Arguments }
                    });

                await _stepsUpdate.AddFunctionToolCallsStepAsync(workItem.ThreadId, workItem.RunId, workItem.AssistantId, stepToolCalls.ToArray());
            }
        }
    }
}
