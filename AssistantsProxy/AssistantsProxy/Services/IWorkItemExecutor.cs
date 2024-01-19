namespace AssistantsProxy.Services
{
    public interface IWorkItemExecutor<T>
    {
        Task ProcessWorkItemAsync(T workItem);
    }
}
