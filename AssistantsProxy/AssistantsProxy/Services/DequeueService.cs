namespace AssistantsProxy.Services
{
    public class DequeueService : BackgroundService
    {
        private readonly IRunsWorkQueue<RunsWorkItemValue> _queue;
        private readonly IServiceProvider _serviceProvider;

        public DequeueService(IServiceProvider serviceProvider, IRunsWorkQueue<RunsWorkItemValue> queue)
        {
            _serviceProvider = serviceProvider;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var queueResponse = await _queue.DequeueAsync();

                    if (queueResponse.Value == null)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                    else
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var runExecutor = scope.ServiceProvider.GetRequiredService<IRunExecutor>();
                            await runExecutor.ProcessWorkItemAsync(queueResponse.Value);
                            await queueResponse.AcknowledgeAsync();
                        }
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
