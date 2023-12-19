using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models.Proxy
{
    public class ProxyMessagesModel : IMessagesModel
    {
        public async Task<ThreadMessage?> CreateAsync(string threadId, MessageCreateParams messageCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(messageCreateParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/messages", inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadMessage>(content);
        }

        public async Task<ThreadMessage?> RetrieveAsync(string threadId, string messageId, string? bearerToken)
        {
            var (_, __, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/messages/" + messageId, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadMessage>(content);
        }

        public async Task<ThreadMessage?> UpdateAsync(string threadId, string messageId, MessageUpdateParams messageUpdateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(messageUpdateParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/messages/" + messageId, inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadMessage>(content);
        }

        public async Task<AssistantList<ThreadMessage>?> ListAsync(string threadId, string? bearerToken)
        {
            var (_, __, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/messages", Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<AssistantList<ThreadMessage>>(content);
        }
    }
}
