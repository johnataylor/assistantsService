using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models.Proxy
{
    public class ProxyRunsModel : IRunsModel
    {
        public async Task<ThreadRun?> CreateAsync(string threadId, RunCreateParams runCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(runCreateParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs", inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> RetrieveAsync(string threadId, string runId, string? bearerToken)
        {
            var (_, __, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> UpdateAsync(string threadId, string runId, RunUpdateParams runUpdateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(runUpdateParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId, inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> CancelAsync(string threadId, string runId, string? bearerToken)
        {
            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/cancel", string.Empty, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> SubmitToolsOutputs(string threadId, string runId, RunSubmitToolOutputsParams runSubmitToolOutputsParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(runSubmitToolOutputsParams);

            var (_, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/submit_tool_outputs", inboundContent, Constants.OpenAIBeta, bearerToken);

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }
    }
}
