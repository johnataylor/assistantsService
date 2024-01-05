using AssistantsProxy.Models;
using AssistantsProxy.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [Route("/v1/threads/{threadId}/runs/{runId}/steps")]
    public class StepsController : AssistantsControllerBase
    {
        private readonly IStepsModel _model;

        public StepsController(IStepsModel model)
        {
            _model = model;
        }

        [HttpGet("{stepId}")]
        public Task<RunStep?> Retrieve([FromRoute] string threadId, [FromRoute] string runId, string stepId)
        {
            return _model.RetrieveAsync(threadId, runId, stepId, BearerToken);
        }

        [HttpGet]
        public Task<AssistantList<RunStep>?> ListAsync([FromRoute] string threadId, [FromRoute] string runId)
        {
            return _model.ListAsync(threadId, runId, BearerToken);
        }
    }
}
