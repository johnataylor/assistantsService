using AssistantsProxy.Models;
using AssistantsProxy.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [Route("v1/assistants")]
    public class AssistantsController : AssistantsControllerBase
    {
        private readonly IAssistantsModel _model;

        public AssistantsController(IAssistantsModel model)
        {
            _model = model;
        }

        [HttpPost]
        public Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams)
        {
            return _model.CreateAsync(assistantCreateParams, BearerToken);
        }

        [HttpGet]
        public Task<AssistantList<Assistant>?> ListAsync()
        {
            return _model.ListAsync(BearerToken);
        }

        [HttpGet("{assistantId}")]
        public Task<Assistant?> RetrieveAsync(string assistantId)
        {
            return _model.RetrieveAsync(assistantId, BearerToken);
        }

        [HttpPost("{assistantId}")]
        public Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams)
        {
            return _model.UpdateAsync(assistantId, assistantUpdateParams, BearerToken);
        }

        [HttpDelete("{assistantId}")]
        public Task DeleteAsync(string assistantId)
        {
            return _model.DeleteAsync(assistantId, BearerToken);
        }
    }
}
