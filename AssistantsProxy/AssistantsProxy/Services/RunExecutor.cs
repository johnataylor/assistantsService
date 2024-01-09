using AssistantsProxy.Models;

namespace AssistantsProxy.Services
{
    public class RunExecutor : IRunExecutor
    {
        private readonly IAssistantsModel _assistantsModel;
        private readonly IThreadsModel _threadsModel;
        private readonly IMessagesModel _messagesModel;
        private readonly IRunsModel _runsModel;
        private readonly IStepsModel _stepsModel;
        private readonly IChatClient _chatClient;

        public RunExecutor(
            IAssistantsModel assistantsModel,
            IThreadsModel threadsModel,
            IMessagesModel messagesModel,
            IRunsModel runsModel,               // needs richer interface
            IStepsModel stepsModel,             // needs richer interface
            IChatClient chatClient)
        {
            _assistantsModel = assistantsModel;
            _threadsModel = threadsModel;
            _messagesModel = messagesModel;
            _runsModel = runsModel;
            _stepsModel = stepsModel;
            _chatClient = chatClient;
        }
        public Task ProcessWorkItemAsync(RunsWorkItemValue workItem)
        {
            throw new NotImplementedException();
        }
    }
}
