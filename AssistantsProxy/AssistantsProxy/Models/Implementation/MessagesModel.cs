using AssistantsProxy.Schema;

namespace AssistantsProxy.Models.Implementation
{
    public class MessagesModel : IMessagesModel
    {
        public Task<ThreadMessage?> CreateAsync(string threadId, MessageCreateParams messageCreateParams, string? bearerToken)
        {            
            // create
            // save
            // return

            throw new NotImplementedException();
        }

        public Task<AssistantList<ThreadMessage>?> ListAsync(string threadId, string? bearerToken)
        {
            // load all
            // list pagination

            throw new NotImplementedException();
        }

        public Task<ThreadMessage?> RetrieveAsync(string threadId, string messageId, string? bearerToken)
        {
            // load
            // return

            throw new NotImplementedException();
        }

        public Task<ThreadMessage?> UpdateAsync(string threadId, string messageId, MessageUpdateParams messageUpdateParams, string? bearerToken)
        {
            // load
            // update
            // save
            // return

            throw new NotImplementedException();
        }
    }
}
