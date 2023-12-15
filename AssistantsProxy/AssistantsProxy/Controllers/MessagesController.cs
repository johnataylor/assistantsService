using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AssistantsProxy.Controllers
{
    [Route("/v1/threads/{threadId}/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        [HttpPost]
        public async Task<ThreadMessage?> Create([FromRoute]string threadId, MessageCreateParams messageCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(messageCreateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;
            var threadMessage = JsonSerializer.Deserialize<ThreadMessage>(content);
            return threadMessage;
        }

        [HttpGet("{messageId}")]
        public async Task<ThreadMessage?> Retrieve([FromRoute]string threadId, string messageId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, contentType, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadMessage = JsonSerializer.Deserialize<ThreadMessage>(content);

            Response.Headers.ContentType = contentType;

            return threadMessage;
        }

        [HttpPost("{messageId}")]
        public async Task<ThreadMessage?> Update([FromRoute] string threadId, string messageId, MessageUpdateParams messageUpdateParams)
        {
            var inboundContent = JsonSerializer.Serialize(messageUpdateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;
            var threadMessage = JsonSerializer.Deserialize<ThreadMessage>(content);
            return threadMessage;
        }

        [HttpGet]
        public async Task<AssistantList<ThreadMessage>?> List([FromRoute]string threadId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, contentType, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var messageList = JsonSerializer.Deserialize<AssistantList<ThreadMessage>>(content);

            Response.Headers.ContentType = contentType;

            return messageList;
        }
    }
}
