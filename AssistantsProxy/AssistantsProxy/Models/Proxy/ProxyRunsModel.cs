using AssistantsProxy.Schema;
using System.Text.Json;

namespace AssistantsProxy.Models.Proxy
{
    public class ProxyRunsModel : IRunsModel
    {
        public async Task<ThreadRun?> CreateAsync(string threadId, RunCreateParams runCreateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(runCreateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs", inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> RetrieveAsync(string threadId, string runId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakeGetRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> UpdateAsync(string threadId, string runId, RunUpdateParams runUpdateParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(runUpdateParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId, inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> CancelAsync(string threadId, string runId, string? bearerToken)
        {
            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/cancel", string.Empty, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }

        public async Task<ThreadRun?> SubmitToolsOutputs(string threadId, string runId, RunSubmitToolOutputsParams runSubmitToolOutputsParams, string? bearerToken)
        {
            var inboundContent = JsonSerializer.Serialize(runSubmitToolOutputsParams);

            var (statusCode, content) = await HttpProxyHelpers.MakePostRequest(Constants.BaseUri + "/v1/threads/" + threadId + "/runs/" + runId + "/submit_tool_outputs", inboundContent, Constants.OpenAIBeta, bearerToken);

            if (statusCode != 200)
            {
                throw new ErrorMessageException(statusCode, JsonSerializer.Deserialize<ErrorMessage>(content));
            }

            return JsonSerializer.Deserialize<ThreadRun>(content);
        }
    }
}
