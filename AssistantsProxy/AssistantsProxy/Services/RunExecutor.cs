﻿using AssistantsProxy.Models;
using AssistantsProxy.Schema;

namespace AssistantsProxy.Services
{
    public class RunExecutor : IWorkItemExecutor<RunsWorkItemValue>
    {
        private readonly IAssistantsModel _assistantsModel;
        private readonly IThreadsModel _threadsModel;
        private readonly IMessagesModel _messagesModel;
        private readonly IRunsModel _runsModel;
        private readonly IRunsUpdate _runsUpdate;
        private readonly IStepsUpdate _stepsUpdate;
        private readonly IChatClient _chatClient;
        private readonly ILogger<RunExecutor> _logger;

        public RunExecutor(
            IAssistantsModel assistantsModel,
            IThreadsModel threadsModel,
            IMessagesModel messagesModel,
            IRunsModel runsModel,
            IRunsUpdate runsUpdate,
            IStepsUpdate stepsModel,
            IChatClient chatClient,
            ILogger<RunExecutor> logger)
        {
            _assistantsModel = assistantsModel;
            _threadsModel = threadsModel;
            _messagesModel = messagesModel;
            _runsModel = runsModel;
            _runsUpdate = runsUpdate;
            _stepsUpdate = stepsModel;
            _chatClient = chatClient;
            _logger = logger;
        }

        public async Task ProcessWorkItemAsync(RunsWorkItemValue workItem)
        {
            // Load metadata

            var run = await _runsModel.RetrieveAsync(workItem.ThreadId, workItem.RunId, null);

            if (run == null)
            {
                _logger.LogWarning($"Running work item {workItem.ThreadId} {workItem.RunId} run == null");
                return;
            }

            if (run.Status == "cancelled" || run.Status == "cancelling" || run.Status == "failed" || run.Status == "completed" || run.Status == "expired")
            {
                // this run is done, no need for any further work
                return;
            }

            var assistant = await _assistantsModel.RetrieveAsync(workItem.AssistantId, null);

            if (assistant == null)
            {
                _logger.LogWarning($"Running work item {workItem.ThreadId} {workItem.RunId} assistant == null");
                return;
            }

            var asssitantThread = await _threadsModel.RetrieveAsync(workItem.ThreadId, null);

            if (asssitantThread == null)
            {
                _logger.LogWarning($"Running work item {workItem.ThreadId} {workItem.RunId} assistantThread == null");
                return;
            }

            _logger.LogInformation($"Running work item for {run.Id} {assistant.Id} {asssitantThread.Id}");

            await _runsUpdate.SetInProgressAsync(workItem.RunId);

            var messageListResponse = await _messagesModel.ListAsync(workItem.ThreadId, null);

            var currentMessages = messageListResponse?.Data ?? throw new KeyNotFoundException($"messages for thread '{workItem.ThreadId}'");

            if (workItem.Rendezvous != null)
            {
                await _stepsUpdate.UpdateFunctionToolCallsStepAsync(workItem.ThreadId, workItem.RunId, workItem.Rendezvous);
            }

            var prompt = PromptFactory.Create(assistant, run, currentMessages, workItem.Rendezvous);

            var callResult = await _chatClient.CallAsync(prompt);

            if (callResult is MessageCallResult messageCallResult)
            {
                await ProcessMessageCallResult(workItem, messageCallResult);
            }
            else if (callResult is ToolCallResult toolCallResult)
            {
                await ProcessToolCallResult(workItem, toolCallResult);
            }
        }

        private async Task ProcessMessageCallResult(RunsWorkItemValue workItem, MessageCallResult messageCallResult)
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
            await _runsUpdate.SetCompletedAsync(workItem.ThreadId, workItem.RunId);
        }

        private async Task ProcessToolCallResult(RunsWorkItemValue workItem, ToolCallResult toolCallResult)
        {
            // TODO separate server tool calls from client tool calls (TODO server tools are not yet implemented)

            // server tool calls should be queued up for local execution - using their own queue - with the results coming in through the work queue
            // client tool call results intrinsically come in through the work queue from the runs model

            // some complexity arises if this list of tools is hetergeneous as the assumption is that all the 'parallel" tool results are collected
            // together for the next call to the LLM. OpenAI place this restriction that the client tool call results should arrive together but
            // we have to do extra work to coordinate those results with the server tool call results

            // the algorithm is to accumulate tool call results until all the results are in, and then and only then should we add an item to the
            // work queue so perhaps it would be better to do this work in the Runs model than here

            // basically treat external and internal tools pretty much the same way, and have the wait-all always applied

            await _runsUpdate.SetRequiresActionAsync(workItem.ThreadId, workItem.RunId, toolCallResult.ToolCalls);

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
