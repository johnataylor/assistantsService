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

        public RunsHostedService(IConfiguration configuration, IRunsWorkQueue<RunsWorkItemValue> queue)
        {
            _assistantsModel = new AssistantsModel(configuration);
            _threadsModel = new ThreadsModel(configuration);
            _messagesModel = new MessagesModel(configuration);
            _runsModel = new RunsModel(configuration, queue);
            _queue = queue;
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

                    // (1) load metadata

                    //   (1.1) load assistant

                    //   (1.2) load thread

                    //   (1.3) load run

                    // (2) run the regular Chat Completion Functions loop

                    //   (2.1) load messages

                    //   (2.2) create prompt

                    //   (2.3) call GPT

                    //   (2.4) update messages

                    //   (2.5) save messages

                    // (3) update run status

                    await response.AcknowledgeAsync();
                }
            }
        }
    }
}
