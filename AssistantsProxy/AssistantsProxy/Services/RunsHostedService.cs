using AssistantsProxy.Models.Implementation;

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
                var response = await _queue.DequeueAsync();

                if (response.Value == null)
                {
                    await Task.Delay(1000, stoppingToken);
                }
                else
                {
                    var value = response.Value;

                    // Load metadata

                    var assistant = await _assistantsModel.RetrieveAsync(value.AssistantId, null);

                    var asssitantThread = await _threadsModel.RetrieveAsync(value.ThreadId, null);

                    var run = await _runsModel.RetrieveAsync(value.ThreadId, value.RunId, null);

                    // Run the regular Chat Completion Functions loop

                    // BEGIN LOOP

                    var currentMessages = await _messagesModel.ListAsync(value.ThreadId, null);

                    var prompt = PromptFactory.Create(assistant, asssitantThread, run, currentMessages?.Data);

                    var newMessage = await _chatClient.CallAsync(prompt);

                    var updatedMessages = MessageManager.Update(currentMessages?.Data, newMessage);

                    // save updated messages

                    // END LOOP

                    // Update run status

                    // TODO...

                    await response.AcknowledgeAsync();
                }
            }
        }
    }
}
