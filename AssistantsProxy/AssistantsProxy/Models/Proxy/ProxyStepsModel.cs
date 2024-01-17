using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models.Proxy
{
    public class ProxyStepsModel : IStepsModel
    {

        public async Task<RunStep?> RetrieveAsync(string threadId, string runId, string stepId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/steps/" + stepId, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<RunStep>(content);
        }

        public async Task<AssistantList<RunStep>?> ListAsync(string threadId, string runId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/steps", Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            content = JsonHelpers.FixJson(content);

            return JsonSerializer.Deserialize<AssistantList<RunStep>>(content);
        }
    }
}
