using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [Route("v1/assistants")]
    [Produces("application/json")]
    public abstract class AssistantsControllerBase : ControllerBase
    {
        [HttpPost]
        public Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams)
        {
            return CreateImplementationAsync(assistantCreateParams);
        }

        [HttpGet]
        public Task<AssistantList<Assistant>?> ListAsync()
        {
            return ListImplementationAsync();
        }

        [HttpGet("{assistantId}")]
        public Task<Assistant?> RetrieveAsync(string assistantId)
        {
            return RetrieveImplementationAsync(assistantId);
        }

        [HttpPost("{assistantId}")]
        public Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams)
        {
            return UpdateImplementationAsync(assistantId, assistantUpdateParams);
        }

        [HttpDelete("{assistantId}")]
        public Task DeleteAsync(string assistantId)
        {
            return DeleteImplementationAsync(assistantId);
        }

        protected abstract Task<Assistant?> CreateImplementationAsync(AssistantCreateParams assistantCreateParams);
        protected abstract Task<AssistantList<Assistant>?> ListImplementationAsync();
        protected abstract Task<Assistant?> RetrieveImplementationAsync(string assistantId);
        protected abstract Task<Assistant?> UpdateImplementationAsync(string assistantId, AssistantUpdateParams assistantUpdateParams);
        protected abstract Task DeleteImplementationAsync(string assistantId);

    }
}
