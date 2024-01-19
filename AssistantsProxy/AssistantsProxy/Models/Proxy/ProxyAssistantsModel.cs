using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models.Proxy
{
    public class ProxyAssistantsModel : IAssistantsModel
    {
        public async Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(assistantCreateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/assistants", inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task<AssistantList<Assistant>?> ListAsync(string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/assistants", Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<AssistantList<Assistant>>(content);
        }

        public async Task<Assistant?> RetrieveAsync(string assistantId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/assistants/" + assistantId, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(assistantUpdateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/assistants/" + assistantId, inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task DeleteAsync(string assistantId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeDeleteRequest(Constants.BaseUri + "/v1/assistants/" + assistantId, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }
        }
    }
}
