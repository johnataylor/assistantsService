namespace AssistantsProxy.Persistence
{
    public class Store
    {
        private readonly IDictionary<string, StoreAssistant> _assistants = new Dictionary<string, StoreAssistant>();
        private readonly IDictionary<string, StoreThread> _threads = new Dictionary<string, StoreThread>();
        private readonly IDictionary<string, StoreRun> _runs = new Dictionary<string, StoreRun>();

        public Task<StoreAssistant> CreateAssistantAsync()
        {
            var storeAssistant = new StoreAssistant();
            _assistants.Add(storeAssistant.Id, storeAssistant);
            return Task.FromResult(storeAssistant);
        }

        public Task<StoreAssistant> GetAssistantAsync(string assistantId)
        {
            return Task.FromResult(_assistants[assistantId]);
        }

        public Task DeleteAssistantAsync(string assistantId)
        {
            _assistants.Remove(assistantId);
            return Task.CompletedTask;
        }

        public Task<StoreThread> CreateThreadAsync()
        {
            var storeThread = new StoreThread();
            _threads.Add(storeThread.Id, storeThread);
            return Task.FromResult(storeThread);
        }

        public Task<StoreThread> GetThreadAsync(string threadId)
        {
            return Task.FromResult(_threads[threadId]);
        }

        public Task DeleteThreadAsync(string threadId)
        {
            _threads.Remove(threadId);
            return Task.CompletedTask;
        }

        public Task<StoreRun> CreateRunAsync(string assistantId, string threadId)
        {
            var storeRun = new StoreRun();
            _runs.Add(storeRun.Id, storeRun);

            // enqueue work

            return Task.FromResult(storeRun);
        }

        public Task<StoreRun> GetRunAsync(string runId)
        {
            return Task.FromResult(_runs[runId]);
        }

        public Task CancelRunAsync(string runId)
        {
            throw new NotImplementedException();
        }
    }
}
