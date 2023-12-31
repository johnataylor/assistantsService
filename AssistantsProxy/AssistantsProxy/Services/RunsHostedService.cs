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
        private readonly IRunsWorkQueue<RunsWorkItemValue> _queue;
        private readonly IChatClient _chatClient;

        public RunsHostedService(IConfiguration configuration, IRunsWorkQueue<RunsWorkItemValue> queue, IChatClient chatClient)
        {
            _assistantsModel = new AssistantsModel(configuration);
            _threadsModel = new ThreadsModel(configuration);
            _messagesModel = new MessagesModel(configuration);
            _runsModel = new RunsModel(configuration, queue);
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
                        var value = response.Value;

                        // Load metadata

                        var assistant = await _assistantsModel.RetrieveAsync(value.AssistantId, null) ?? throw new KeyNotFoundException(value.AssistantId);

                        var asssitantThread = await _threadsModel.RetrieveAsync(value.ThreadId, null) ?? throw new KeyNotFoundException(value.ThreadId);

                        var run = await _runsModel.RetrieveAsync(value.ThreadId, value.RunId, null) ?? throw new KeyNotFoundException(value.RunId);

                        await _runsModel.SetStatus(value.RunId, "in_progress");

                        // Run the regular Chat Completion Functions loop

                        // BEGIN LOOP

                        var messageListResponse = await _messagesModel.ListAsync(value.ThreadId, null);

                        var currentMessages = messageListResponse?.Data ?? throw new KeyNotFoundException($"messages for thread '{value.ThreadId}'");

                        var prompt = PromptFactory.Create(assistant, asssitantThread, run, currentMessages);

                        var newMessage = await _chatClient.CallAsync(prompt);

                        // TODO use MessageManager to manage the conversation history
                        // var updatedMessages = MessageManager.Update(currentMessages, newMessage);
                        // save updated messages

                        // just for now, append the new message to the conversation

                        var content = newMessage.Content?[0].Text?.Value ?? throw new Exception("expected some content");

                        var messageCreateParams = new MessageCreateParams
                        {
                            Content = content,
                            Role = newMessage.Role
                        };

                        await _messagesModel.CreateAsync(value.ThreadId, messageCreateParams, null);

                        // END LOOP

                        // Update run status

                        await _runsModel.SetStatus(value.RunId, "completed");

                        // TODO...

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
    }
}
