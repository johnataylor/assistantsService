using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models
{
    public class ThreadsModel : IThreadsModel
    {
        public async Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads", inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<AssistantThread>(content);
        }

        public async Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/runs", inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<AssistantThread>(content);
        }

        public async Task<Assistant?> RetrieveAsync(string threadId, string? bearerToken)
        {
            var (_, _, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task<Assistant?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(threadUpdateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId, inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task DeleteAsync(string threadId, string? bearerToken)
        {
            var statusCode = await HttpProxyHelpers.MakeDeleteRequest(Constants.BaseUri + "/v1/threads/" + threadId, Constants.OpenAIBeta, bearerToken);
        }
    }
}