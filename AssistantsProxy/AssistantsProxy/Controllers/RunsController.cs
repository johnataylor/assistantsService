using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AssistantsProxy.Controllers
{
    [Route("/v1/threads/{threadId}/runs")]
    [ApiController]
    public class RunsController : ControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        [HttpPost]
        public async Task<ThreadRun?> Create([FromRoute]string threadId, RunCreateParams runCreateParams)
        { 
            var inboundContent = JsonSerializer.Serialize(runCreateParams);
            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }

        [HttpGet("{runId}")]
        public async Task<ThreadRun?> Retrieve([FromRoute]string threadId, string runId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, contentType, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }

        [HttpPost("{runId}")]
        public async Task<ThreadRun?> Update([FromRoute]string threadId, string runId, RunUpdateParams runUpdateParams)
        {
            var inboundContent = JsonSerializer.Serialize(runUpdateParams);
            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }

        [HttpPost("{runId}/cancel")]
        public async Task<ThreadRun?> Cancel([FromRoute]string threadId, string runId, RunCreateParams runCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(runCreateParams);
            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }
    }
}
