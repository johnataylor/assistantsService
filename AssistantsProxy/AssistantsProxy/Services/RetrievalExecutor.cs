using AssistantsProxy.Models;
using AssistantsProxy.Schema;

namespace AssistantsProxy.Services
{
    public class RetrievalExecutor : IWorkItemExecutor<RetrievalWorkItem>
    {
        private readonly IRunsModel _runsModel;

        public RetrievalExecutor(IRunsModel runsModel)
        {
            _runsModel = runsModel;
        }

        public async Task ProcessWorkItemAsync(RetrievalWorkItem workItem)
        {
            await Task.Delay(3000);

            var submitToolOutputsParams = new RunSubmitToolOutputsParams
            {
                ToolOutputs = [new ToolOutput { ToolCallId = workItem.Id, Output = "Oslo" }]
            };

            await _runsModel.SubmitToolsOutputs(workItem.ThreadId, workItem.RunId, submitToolOutputsParams, null);
        }
    }
}
