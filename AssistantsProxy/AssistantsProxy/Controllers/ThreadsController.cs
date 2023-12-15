using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AssistantsProxy.Controllers
{
    [ApiController]
    [Route("/v1/threads")]
    [Produces("application/json")]
    public class ThreadsController : ControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        [HttpPost]
        public async Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var thread = JsonSerializer.Deserialize<AssistantThread>(content);

            Response.StatusCode = statusCode;

            return thread;
        }

        [HttpPost("runs")]
        public async Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var thread = JsonSerializer.Deserialize<AssistantThread>(content);

            Response.StatusCode = statusCode;

            return thread;
        }

        [HttpGet("{threadId}")]
        public async Task<Assistant?> Retrieve(string threadId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, _, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [HttpPost("{threadId}")]
        public async Task<Assistant?> Update(string threadId, ThreadUpdateParams threadUpdateParams)
        {
            var inboundContent = JsonSerializer.Serialize(threadUpdateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [HttpDelete("{threadId}")]
        public async Task Delete(string threadId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var statusCode = await HttpProxyHelpers.MakeDeleteRequest(requestUri, openAiBeta, bearerToken);
        }

        /*
        [HttpPost("{threadId}/messages")]
        public async Task<ThreadMessage?> ThreadsMessages(string threadId, CreateMessage createMessage)
        {
            var inboundContent = JsonSerializer.Serialize(createMessage);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;
            var threadMessage = JsonSerializer.Deserialize<ThreadMessage>(content);
            return threadMessage;
        }
        */

        /*
        [HttpPost("{threadId}/runs")]
        public async Task<ThreadRun?> ThreadsRuns(string threadId, RunRequest runRequest)
        { 
            var inboundContent = JsonSerializer.Serialize(runRequest);
            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }

        [HttpGet("{threadId}/runs/{runId}")]
        public async Task<ThreadRun?> GetThreadRun(string threadId, string runId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, contentType, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }
        */

        /*
        [HttpGet("{threadId}/messages")]
        public async Task<AssistantList<ThreadMessage>?> GetThreadMessages(string threadId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, contentType, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var messageList = JsonSerializer.Deserialize<AssistantList<ThreadMessage>>(content);

            Response.Headers.ContentType = contentType;

            return messageList;
        }
        */
    }
}