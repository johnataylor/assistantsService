using AssistantsProxy.Schema;

namespace AssistantsProxy.Models.Implementation
{
    public class AssistantsModel : IAssistantsModel
    {
        public Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams, string? bearerToken)
        {
            // create
            // save
            // return

            throw new NotImplementedException();
        }

        public Task DeleteAsync(string assistantId, string? bearerToken)
        {
            // delete

            throw new NotImplementedException();
        }

        public Task<AssistantList<Assistant>?> ListAsync(string? bearerToken)
        {
            // load all
            // list pagination

            throw new NotImplementedException();
        }

        public Task<Assistant?> RetrieveAsync(string assistantId, string? bearerToken)
        {
            // load
            // return

            throw new NotImplementedException();
        }

        public Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams, string? bearerToken)
        {
            // load
            // update
            // save
            // return

            throw new NotImplementedException();
        }
    }
}
