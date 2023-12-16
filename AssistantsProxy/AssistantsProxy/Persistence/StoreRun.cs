namespace AssistantsProxy.Persistence
{
    public class StoreRun
    {
        public StoreRun()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}
