namespace AssistantsProxy.Services
{
    public interface IWorkItemQueue<T>
    {
        Task EnqueueAsync(T value);

        Task<IWorkItem<T>> DequeueAsync();
    }
}
