using AssistantsProxy.Models;
using AssistantsProxy.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [Route("/v1/threads")]
    public class ThreadsController : AssistantsControllerBase
    {
        private readonly IThreadsModel _model;

        public ThreadsController(IThreadsModel model)
        {
            _model = model;
        }

        [HttpPost]
        public Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams)
        {
            return _model.CreateAsync(threadCreateParams, BearerToken);
        }

        [HttpPost("runs")]
        public Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams)
        {
            return _model.CreateAndRunAsync(threadCreateParams, BearerToken);
        }

        [HttpGet("{threadId}")]
        public Task<AssistantThread?> RetrieveAsync(string threadId)
        {
            return _model.RetrieveAsync(threadId, BearerToken);
        }

        [HttpPost("{threadId}")]
        public Task<AssistantThread?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams)
        {
            return _model.UpdateAsync(threadId, threadUpdateParams, BearerToken);
        }

        [HttpDelete("{threadId}")]
        public Task DeleteAsync(string threadId)
        {
            return _model.DeleteAsync(threadId, BearerToken);
        }
    }
}