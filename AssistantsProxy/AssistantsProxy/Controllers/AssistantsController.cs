using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AssistantsProxy.Controllers
{
    [Route("v1/assistants")]
    [ApiController]
    [Produces("application/json")]
    public class AssistantsController : ControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        [HttpPost]
        public async Task<Assistant?> Create(AssistantCreateParams assistantCreateParams)
        {
            var inboundContent = JsonSerializer.Serialize(assistantCreateParams);
            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [HttpGet]
        public async Task<AssistantList<Assistant>?> List()
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, _, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<AssistantList<Assistant>>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [HttpGet("{assistantId}")]
        public async Task<Assistant?> Retrieve(string assistantId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, _, content) = await HttpProxyHelpers.MakeGetRequest(requestUri, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [HttpPost("{assistantId}")]
        public async Task<Assistant?> Update(string assistantId, AssistantUpdateParams assistantUpdateParams)
        {
            var inboundContent = JsonSerializer.Serialize(assistantUpdateParams);

            var (inboundContentType, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [HttpDelete("{assistantId}")]
        public async Task Delete(string assistantId)
        {
            var (_, openAiBeta, bearerToken) = HttpProxyHelpers.ReadHeaders(Request);
            var requestUri = _baseUri + Request.Path;

            var statusCode = await HttpProxyHelpers.MakeDeleteRequest(requestUri, openAiBeta, bearerToken);
        }
    }
}
