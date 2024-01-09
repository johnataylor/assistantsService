namespace AssistantsProxy.Services
{
    public class DequeueService : BackgroundService
    {
        private readonly IRunsWorkQueue<RunsWorkItemValue> _queue;

        public DequeueService(IServiceProvider services, IRunsWorkQueue<RunsWorkItemValue> queue)
        {
            Services = services;
            _queue = queue;
        }

        public IServiceProvider Services { get; }


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
                        using (var scope = Services.CreateScope())
                        {
                            var runExecutor = scope.ServiceProvider.GetRequiredService<IRunExecutor>();
                            await runExecutor.ProcessWorkItemAsync(response.Value);

                            await response.AcknowledgeAsync();
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
