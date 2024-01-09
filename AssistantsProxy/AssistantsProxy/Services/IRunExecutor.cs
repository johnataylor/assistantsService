namespace AssistantsProxy.Services
{
    public interface IRunExecutor
    {
        Task ProcessWorkItemAsync(RunsWorkItemValue workItem);
    }
}
