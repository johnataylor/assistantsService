using System.Net.Http.Headers;

namespace AssistantsProxy
{
    public static class HttpProxyHelpers
    {
        public static async Task<string> ReadContentAsync(HttpRequest request)
        {
            using var body = new MemoryStream();
            await request.Body.CopyToAsync(body);
            body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(body);
            return reader.ReadToEnd();
        }

        public static (string contentType, string? openAiBeta, string? bearerToken) ReadHeaders(HttpRequest request)
        {
            var contentType = request.ContentType ?? string.Empty;
            var openAiBeta = request.Headers["OpenAi-Beta"].Single();
            var bearerToken = request.Headers.Authorization[0]?.Split(' ')[1];
            return (contentType, openAiBeta, bearerToken);
        }

        public static async Task<(int statusCode, string content)> MakePostRequest(string requestUri, string content, string? openAiBeta, string? bearerToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Add("OpenAI-Beta", openAiBeta);

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return ((int)response.StatusCode, responseContent);
        }

        public static async Task<(int statusCode, string content)> MakeGetRequest(string requestUri, string? openAiBeta, string? bearerToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Add("OpenAI-Beta", openAiBeta);

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return ((int)response.StatusCode, responseContent);
        }

        public static async Task<(int statusCode, string content)> MakeDeleteRequest(string requestUri, string? openAiBeta, string? bearerToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Add("OpenAI-Beta", openAiBeta);

            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return ((int)response.StatusCode, responseContent);
        }
    }
}
