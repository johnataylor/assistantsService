namespace AssistantsProxy.Services
{
    public interface IRunsWorkQueue<T>
    {
        Task EnqueueAsync(T value);

        Task<IWorkItem> DequeueAsync();

        public interface IWorkItem
        {
            public T? Value { get; }

            public Task AcknowledgeAsync();
        }
    }
}
