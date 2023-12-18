using AssistantsProxy.Models;
using AssistantsProxy.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [Route("/v1/threads/{threadId}/messages")]
    public class MessagesController : AssistantsControllerBase
    {
        private readonly IMessagesModel _model;

        public MessagesController(IMessagesModel model)
        {
            _model = model;
        }

        [HttpPost]
        public Task<ThreadMessage?> CreateAsync([FromRoute]string threadId, MessageCreateParams messageCreateParams)
        {
            return _model.CreateAsync(threadId, messageCreateParams, BearerToken);
        }

        [HttpGet("{messageId}")]
        public Task<ThreadMessage?> Retrieve([FromRoute]string threadId, string messageId)
        {
            return _model.RetrieveAsync(threadId, messageId, BearerToken);
        }

        [HttpPost("{messageId}")]
        public Task<ThreadMessage?> UpdateAsync([FromRoute] string threadId, string messageId, MessageUpdateParams messageUpdateParams)
        {
            return _model.UpdateAsync(threadId, messageId, messageUpdateParams, BearerToken);
        }

        [HttpGet]
        public Task<AssistantList<ThreadMessage>?> ListAsync([FromRoute]string threadId)
        {
            return _model.ListAsync(threadId, BearerToken);
        }
    }
}
