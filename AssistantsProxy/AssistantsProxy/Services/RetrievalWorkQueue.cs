using AssistantsProxy.Models;

namespace AssistantsProxy.Services
{
    public class RetrievalWorkQueue : AzureWorkQueue<RetrievalWorkItem>
    {
        public RetrievalWorkQueue(IConfiguration configuration) : base(configuration, "retrieval") { }
    }
}
