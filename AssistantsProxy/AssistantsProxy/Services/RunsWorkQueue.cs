namespace AssistantsProxy.Services
{
    public class RunsWorkQueue : AzureWorkQueue<RunsWorkItemValue>
    {
        public RunsWorkQueue(IConfiguration configuration) : base(configuration, "work") { }
    }
}
