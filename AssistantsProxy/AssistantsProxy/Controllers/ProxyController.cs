using AssistantsProxy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AssistantsProxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProxyController : ControllerBase
    {
        private string _baseUri = "https://api.openai.com";

        [Route("/v1/assistants")]
        [HttpPost]
        public async Task<Assistant?> AssistantsAsync(CreateAssistant createAssistant)
        {
            var inboundContent = JsonSerializer.Serialize(createAssistant);
            var (inboundContentType, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<Assistant>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [Route("/v1/assistants")]
        [HttpGet]
        public async Task<AssistantList<Assistant>?> GetAssistantsAsync()
        {
            var (_, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var (statusCode, _, content) = await MakeGetRequest(requestUri, openAiBeta, bearerToken);

            var assistant = JsonSerializer.Deserialize<AssistantList<Assistant>>(content);

            Response.StatusCode = statusCode;

            return assistant;
        }

        [Route("/v1/assistants/{assistantId}")]
        [HttpDelete]
        public async Task DeleteAssistantsAsync(string assistantId)
        {
            var (_, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var statusCode = await MakeDeleteRequest(requestUri, openAiBeta, bearerToken);
        }

        [Route("/v1/threads")]
        [HttpPost]
        public async Task<AssistantThread?> ThreadsAsync()
        {
            // not seeing any content

            var inboundContent = await ReadContentAsync();
            var (inboundContentType, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            var thread = JsonSerializer.Deserialize<AssistantThread>(content);

            Response.StatusCode = statusCode;

            return thread;
        }

        [Route("/v1/threads/{threadId}/messages")]
        [HttpPost]
        public async Task<ThreadMessage?> ThreadsMessages(string threadId, CreateMessage createMessage)
        {
            var inboundContent = JsonSerializer.Serialize(createMessage);

            var (inboundContentType, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;
            var threadMessage = JsonSerializer.Deserialize<ThreadMessage>(content);
            return threadMessage;
        }

        [Route("/v1/threads/{threadId}/runs")]
        [HttpPost]
        public async Task<ThreadRun?> ThreadsRuns(string threadId, RunRequest runRequest)
        { 
            var inboundContent = JsonSerializer.Serialize(runRequest);
            var (inboundContentType, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var (statusCode, content) = await MakePostRequest(requestUri, inboundContent, inboundContentType, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }

        [Route("/v1/threads/{threadId}/runs/{runId}")]
        [HttpGet]
        public async Task<ThreadRun?> GetThreadRun(string threadId, string runId)
        {
            var (_, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var (statusCode, contentType, content) = await MakeGetRequest(requestUri, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var threadRun = JsonSerializer.Deserialize<ThreadRun>(content);

            return threadRun;
        }

        [Route("/v1/threads/{threadId}/messages")]
        [HttpGet]
        public async Task<AssistantList<ThreadMessage>?> GetThreadMessages(string threadId)
        {
            var (_, openAiBeta, bearerToken) = ReadHeaders();
            var requestUri = _baseUri + Request.Path;

            var (statusCode, contentType, content) = await MakeGetRequest(requestUri, openAiBeta, bearerToken);

            Response.StatusCode = statusCode;

            var messageList = JsonSerializer.Deserialize<AssistantList<ThreadMessage>>(content);

            Response.Headers.ContentType = contentType;

            return messageList;
        }

        private async Task<string> ReadContentAsync()
        {
            using var body = new MemoryStream();
            await Request.Body.CopyToAsync(body);
            body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(body);
            return reader.ReadToEnd();
        }

        private (string contentType, string? openAiBeta, string? bearerToken) ReadHeaders()
        {
            var contentType = Request.ContentType ?? string.Empty;
            var openAiBeta = Request.Headers["OpenAi-Beta"].Single();
            var bearerToken = Request.Headers.Authorization[0]?.Split(' ')[1];
            return (contentType, openAiBeta, bearerToken);
        }

        private static async Task<(int statusCode, string content)> MakePostRequest(string requestUri, string content, string contentType, string? openAiBeta, string? bearerToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Add("OpenAI-Beta", openAiBeta);

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return ((int)response.StatusCode, responseContent);
        }

        private static async Task<(int statusCode, string contentType, string content)> MakeGetRequest(string requestUri, string? openAiBeta, string? bearerToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Add("OpenAI-Beta", openAiBeta);

            var response = await client.SendAsync(request);
            var responseContentType = response.Content.Headers.ContentType?.ToString() ?? string.Empty;
            var responseContent = await response.Content.ReadAsStringAsync();

            return ((int)response.StatusCode, responseContentType, responseContent);
        }

        
        private static async Task<int> MakeDeleteRequest(string requestUri, string? openAiBeta, string? bearerToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Add("OpenAI-Beta", openAiBeta);

            var response = await client.SendAsync(request);

            return (int)response.StatusCode;
        }
    }
}