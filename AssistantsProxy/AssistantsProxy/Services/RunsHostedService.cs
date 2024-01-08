using AssistantsProxy.Models.Implementation;
using AssistantsProxy.Schema;

namespace AssistantsProxy.Services
{
    public class RunsHostedService : BackgroundService
    {
        private readonly AssistantsModel _assistantsModel;
        private readonly ThreadsModel _threadsModel;
        private readonly MessagesModel _messagesModel;
        private readonly RunsModel _runsModel;
        private readonly StepsModel _stepsModel;

        private readonly IRunsWorkQueue<RunsWorkItemValue> _queue;
        private readonly IChatClient _chatClient;

        public RunsHostedService(IConfiguration configuration, IRunsWorkQueue<RunsWorkItemValue> queue, IChatClient chatClient)
        {
            _assistantsModel = new AssistantsModel(configuration);
            _threadsModel = new ThreadsModel(configuration);
            _messagesModel = new MessagesModel(configuration);
            _runsModel = new RunsModel(configuration, queue);
            _stepsModel = new StepsModel(configuration);
            _queue = queue;
            _chatClient = chatClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _queue.DequeueAsync();

                    if (response.Value == null)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                    else
                    {
                        await ProcessWorkItem(response.Value);

                        await response.AcknowledgeAsync();
                    }
                }
                catch (Exception e)
                {
                    // TODO logging

                    var msg = e.Message;
                }
            }
        }
        private async Task ProcessWorkItem(RunsWorkItemValue workItem)
        {
            // Load metadata

            var assistant = await _assistantsModel.RetrieveAsync(workItem.AssistantId, null) ?? throw new KeyNotFoundException(workItem.AssistantId);

            var asssitantThread = await _threadsModel.RetrieveAsync(workItem.ThreadId, null) ?? throw new KeyNotFoundException(workItem.ThreadId);

            var run = await _runsModel.RetrieveAsync(workItem.ThreadId, workItem.RunId, null) ?? throw new KeyNotFoundException(workItem.RunId);

            await _runsModel.SetStatus(workItem.RunId, "in_progress");

            // enqueued item for function output
            // add in progress step for message creation

            var messageListResponse = await _messagesModel.ListAsync(workItem.ThreadId, null);

            var currentMessages = messageListResponse?.Data ?? throw new KeyNotFoundException($"messages for thread '{workItem.ThreadId}'");

            var prompt = PromptFactory.Create(assistant, asssitantThread, run, currentMessages, workItem.RunSubmitToolOutputsParams);

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

                await _stepsModel.AddMessageCreationStepAsync(workItem.ThreadId, workItem.RunId, workItem.AssistantId, newThreadMessageId);

                await _runsModel.SetStatus(workItem.RunId, "completed");
            }
            else if (callResult is ToollCallResult toolCallResult)
            {
                await _runsModel.SetRequiredAction(workItem.RunId, toolCallResult.ToolCalls);

                // create step for tool
            }

            // END LOOP - logical "loop"
        }
    }
}
