using Azure.Storage.Queues;

namespace AssistantsProxy.Services
{
    public class AzureStorageQueueWorkItem<T> : IWorkItem<T>
    {
        private QueueClient? _queue;
        private string _messageId;
        private string _popReceipt;

        public static readonly IWorkItem<T> Empty = new AzureStorageQueueWorkItem<T>();

        private AzureStorageQueueWorkItem()
        {
            _messageId = string.Empty;
            _popReceipt = string.Empty;
        }

        public AzureStorageQueueWorkItem(QueueClient queue, string messageId, string popReceipt, T? value)
        {
            _queue = queue;
            _messageId = messageId;
            _popReceipt = popReceipt;
            Value = value;
        }

        public T? Value { get; private set; }

        public Task AcknowledgeAsync()
        {
            return _queue?.DeleteMessageAsync(_messageId, _popReceipt) ?? throw new Exception("no value was retrieved");
        }
    }
}
