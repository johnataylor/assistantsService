using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models
{
    public class AssistantsModel : IAssistantsModel
    {
        public async Task<Assistant?> CreateAsync(AssistantCreateParams assistantCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(assistantCreateParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/assistants", inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task<AssistantList<Assistant>?> ListAsync(string? bearerToken)
        {
            var (_, __, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/assistants", Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<AssistantList<Assistant>>(content);
        }

        public async Task<Assistant?> RetrieveAsync(string assistantId, string? bearerToken)
        {
            var (_, __, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/assistants/" + assistantId, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task<Assistant?> UpdateAsync(string assistantId, AssistantUpdateParams assistantUpdateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(assistantUpdateParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/assistants" + assistantId, inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<Assistant>(content);
        }

        public async Task DeleteAsync(string assistantId, string? bearerToken)
        {
            var _ = await HttpProxyHelpers.MakeDeleteRequest(Constants.BaseUri + "/v1/assistants" + assistantId, Constants.OpenAIBeta, bearerToken);
        }
    }
}
