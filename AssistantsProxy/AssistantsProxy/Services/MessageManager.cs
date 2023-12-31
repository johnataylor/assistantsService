using AssistantsProxy.Schema;

namespace AssistantsProxy.Services
{
    public static class MessageManager
    {
        public static ThreadMessage[] Update(ThreadMessage[]? currentMessages, ThreadMessage newMessage)
        {
            currentMessages = currentMessages ?? throw new ArgumentNullException(nameof(currentMessages));

            var newMessages = new List<ThreadMessage>(currentMessages) { newMessage };

            // TODO manage message array collection

            return newMessages.ToArray();
        }
    }
}
