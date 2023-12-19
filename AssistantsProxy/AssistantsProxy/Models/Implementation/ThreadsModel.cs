using AssistantsProxy.Schema;

namespace AssistantsProxy.Models.Implementation
{
    public class ThreadsModel : IThreadsModel
    {
        public Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams, string? bearerToken)
        {
            throw new NotImplementedException();
        }

        public Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams, string? bearerToken)
        {
            // create
            // save
            // return

            throw new NotImplementedException();
        }

        public Task DeleteAsync(string threadId, string? bearerToken)
        {
            // delete

            throw new NotImplementedException();
        }

        public Task<Assistant?> RetrieveAsync(string threadId, string? bearerToken)
        {
            // get

            throw new NotImplementedException();
        }

        public Task<Assistant?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams, string? bearerToken)
        {
            // load
            // update
            // save
            // return

            throw new NotImplementedException();
        }
    }
}
