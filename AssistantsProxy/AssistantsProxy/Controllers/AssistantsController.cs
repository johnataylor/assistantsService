using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AssistantsProxy.Controllers
{
    [ApiController]
    public class AssistantsController : AssistantsControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        protected override async Task<Assistant?> CreateImplementationAsync(AssistantCreateParams assistantCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(assistantCreateParams);
            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        protected override async Task<AssistantList<Assistant>?> ListImplementationAsync()
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, _, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<AssistantList<Assistant>>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        protected override async Task<Assistant?> RetrieveImplementationAsync(string assistantId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, _, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        protected override async Task<Assistant?> UpdateImplementationAsync(string assistantId, AssistantUpdateParams assistantUpdateParams)
        {
            var inboundContent = JsonSerializer.Serialize(assistantUpdateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        protected override async Task DeleteImplementationAsync(string assistantId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var statusCode = await HttpProxyHelpers.MakeDeleteRequest(requestUri, openAiBeta, bearerToken);
        }
    }
}
