namespace AssistantsProxy.Persistence
{
    public class StoreAssistant
    {
        public StoreAssistant()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
    }
}
