namespace AssistantsProxy.Persistence
{
    public class StoreThread
    {
        public StoreThread()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
    }
}
