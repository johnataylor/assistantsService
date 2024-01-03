using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models.Proxy
{
    public class ProxyStepsModel : IStepsModel
    {

        public async Task<RunStep?> RetrieveAsync(string threadId, string runId, string stepId, string? bearerToken)
        {
            var (_, __, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/steps/" + stepId, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<RunStep>(content);
        }

        public async Task<AssistantList<RunStep>?> ListAsync(string threadId, string runId, string? bearerToken)
        {
            var (_, __, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/steps", Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<AssistantList<RunStep>>(content);
        }
    }
}
