namespace AssistantsProxy.Services
{
    public class MessageCallResult : CallResultBase
    {
        public MessageCallResult(string content)
        {
            Content = content;
        }
        public string Content { get; init; }
    }
}
