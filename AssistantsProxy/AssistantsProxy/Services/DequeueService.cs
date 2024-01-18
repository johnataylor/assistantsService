namespace AssistantsProxy.Services
{
    public class DequeueService : BackgroundService
    {
        private readonly IRunsWorkQueue<RunsWorkItemValue> _queue;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DequeueService> _logger;

        public DequeueService(IServiceProvider serviceProvider, IRunsWorkQueue<RunsWorkItemValue> queue, ILogger<DequeueService> logger)
        {
            _serviceProvider = serviceProvider;
            _queue = queue;
            _logger = logger;
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
                    // TODO improve logging
                    _logger.LogError(e.Message);
                }
            }
        }
    }
}
