namespace AssistantsProxy.Services
{
    public interface IWorkItem<T>
    {
        T? Value { get; }

        Task AcknowledgeAsync();
    }
}
