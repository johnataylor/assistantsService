using AssistantsProxy.Schema;

namespace AssistantsProxy.Models.Implementation
{
    public class RunsModel : IRunsModel
    {
        public Task<ThreadRun?> CreateAsync(string threadId, RunCreateParams runCreateParams, string? bearerToken)
        {
            // load assistant
            // load thread
            // enqueue workload
            // save a record representing the state of the run
            // return

            throw new NotImplementedException();
        }

        public Task<ThreadRun?> RetrieveAsync(string threadId, string runId, string? bearerToken)
        {
            // load the state of the run

            throw new NotImplementedException();
        }

        public Task<ThreadRun?> UpdateAsync(string threadId, string runId, RunUpdateParams runUpdateParams, string? bearerToken)
        {
            throw new NotImplementedException();
        }
        public Task<ThreadRun?> CancelAsync(string threadId, string runId, string? bearerToken)
        {
            // delete the run

            throw new NotImplementedException();
        }
    }
}
