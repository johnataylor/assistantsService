﻿using AssistantsProxy.Models;
using AssistantsProxy.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [Route("/v1/threads/{threadId}/runs")]
    public class RunsController : AssistantsControllerBase
    {
        private readonly IRunsModel _model;

        public RunsController(IRunsModel model)
        {
            _model = model;
        }

        [HttpPost]
        public Task<ThreadRun?> CreateAsync([FromRoute]string threadId, RunCreateParams runCreateParams)
        { 
            return _model.CreateAsync(threadId, runCreateParams, BearerToken);
        }

        [HttpGet("{runId}")]
        public Task<ThreadRun?> RetrieveAsync([FromRoute]string threadId, string runId)
        {
            return _model.RetrieveAsync(threadId, runId, BearerToken);
        }

        [HttpPost("{runId}")]
        public Task<ThreadRun?> UpdateAsync([FromRoute]string threadId, string runId, RunUpdateParams runUpdateParams)
        {
            return _model.UpdateAsync(threadId, runId, runUpdateParams, BearerToken);
        }

        [HttpPost("{runId}/cancel")]
        public Task<ThreadRun?> CancelAsync([FromRoute]string threadId, string runId)
        {
            return _model.CancelAsync(threadId, runId, BearerToken);
        }

        [HttpPost("{runId}/submit_tool_outputs")]
        public Task<ThreadRun?> SubmitToolsOutputs([FromRoute]string threadId, string runId, RunSubmitToolOutputsParams runSubmitToolOutputsParams)
        {
            return _model.SubmitToolsOutputs(threadId, runId, runSubmitToolOutputsParams, BearerToken);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult HandleError() => ErrorMessage();
    }
}
