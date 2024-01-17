using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models.Proxy
{
    public class ProxyThreadsModel : IThreadsModel
    {
        public async Task<AssistantThread?> CreateAsync(ThreadCreateParams? threadCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads", inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<AssistantThread>(content);
        }

        public async Task<AssistantThread?> CreateAndRunAsync(ThreadCreateAndRunParams? threadCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(threadCreateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/runs", inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<AssistantThread>(content);
        }

        public async Task<AssistantThread?> RetrieveAsync(string threadId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<AssistantThread>(content);
        }

        public async Task<AssistantThread?> UpdateAsync(string threadId, ThreadUpdateParams threadUpdateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(threadUpdateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId, inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<AssistantThread>(content);
        }

        public async Task DeleteAsync(string threadId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeDeleteRequest(Constants.BaseUri + "/v1/threads/" + threadId, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }
        }
    }
}