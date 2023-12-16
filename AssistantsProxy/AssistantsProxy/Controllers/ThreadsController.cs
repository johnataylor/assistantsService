using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AssistantsProxy.Controllers
{
    [ApiController]
    public class ThreadsController : ThreadsControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        protected override async Task<AssistantThread?> CreateImplementationAsync(ThreadCreateParams? threadCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var thread = JsonSerializer.Deserialize<AssistantThread>(content);

            Response.StatusCode = statusCode;

            return thread;
        }

        protected override async Task<AssistantThread?> CreateAndRunImplementationAsync(ThreadCreateAndRunParams? threadCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var thread = JsonSerializer.Deserialize<AssistantThread>(content);

            Response.StatusCode = statusCode;

            return thread;
        }

        protected override async Task<Assistant?> RetrieveImplementationAsync(string threadId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, _, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        protected override async Task<Assistant?> UpdateImplementationAsync(string threadId, ThreadUpdateParams threadUpdateParams)
        {
            var inboundContent = JsonSerializer.Serialize(threadUpdateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        protected override async Task DeleteImplementationAsync(string threadId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var statusCode = await HttpProxyHelpers.MakeDeleteRequest(requestUri, openAiBeta, bearerToken);
        }
    }
}