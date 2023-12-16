using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [Route("/v1/threads")]
    [Produces("application/json")]
    public abstract class ThreadsControllerBase : ControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        [HttpPost]
        public Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams)
        {
            return CreateImplementationAsync(threadCreateParams);
        }

        [HttpPost("runs")]
        public Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams)
        {
            return CreateAndRunImplementationAsync(threadCreateParams);
        }

        [HttpGet("{threadId}")]
        public Task<Assistant?> RetrieveAsync(string threadId)
        {
            return RetrieveImplementationAsync(threadId);
        }

        [HttpPost("{threadId}")]
        public Task<Assistant?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams)
        {
            return UpdateImplementationAsync(threadId, threadUpdateParams);
        }

        [HttpDelete("{threadId}")]
        public Task DeleteAsync(string threadId)
        {
            return DeleteImplementationAsync(threadId);
        }

        protected abstract Task<AssistantThread?> CreateImplementationAsync(ThreadCreateParams? threadCreateParams);
        protected abstract Task<AssistantThread?> CreateAndRunImplementationAsync(ThreadCreateAndRunParams? threadCreateParams);
        protected abstract Task<Assistant?> RetrieveImplementationAsync(string threadId);
        protected abstract Task<Assistant?> UpdateImplementationAsync(string threadId, ThreadUpdateParams threadUpdateParams);
        protected abstract Task DeleteImplementationAsync(string threadId);
    }
}