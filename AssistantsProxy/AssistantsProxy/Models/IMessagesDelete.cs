namespace AssistantsProxy.Models
{
    public interface IMessagesDelete
    {
        Task DeleteMessages(string threadId);
    }
}
