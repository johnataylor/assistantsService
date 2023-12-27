using Azure.Storage.Queues;
using System.Text.Json;
using System.Text;

namespace AssistantsProxy.Services
{
    public class RunsWorkQueue : IRunsWorkQueue<RunsWorkItemValue>
    {
        private readonly QueueClient _queueClient;
        private const string QueueName = "work";

        public RunsWorkQueue(IConfiguration configuration)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _queueClient = new QueueClient(connectionString, QueueName);
        }

        public async Task EnqueueAsync(RunsWorkItemValue value)
        {
            await _queueClient.SendMessageAsync(new BinaryData(value));
        }

        public async Task<IRunsWorkQueue<RunsWorkItemValue>.IWorkItem> DequeueAsync()
        {
            var response = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromSeconds(30));

            if (response.Value.Length > 0)
            {
                var queueMessage = response.Value[0];

                var json = Encoding.UTF8.GetString(queueMessage.Body);
                var value = JsonSerializer.Deserialize<RunsWorkItemValue>(json) ?? throw new Exception("expected a RunsWorkItemValue");
                return new WorkItem(_queueClient, queueMessage.MessageId, queueMessage.PopReceipt, value);
            }

            return WorkItem.Empty;
        }

        private class WorkItem : IRunsWorkQueue<RunsWorkItemValue>.IWorkItem
        {
            private QueueClient? _queue;
            private string _messageId;
            private string _popReceipt;

            public static readonly WorkItem Empty = new WorkItem();

            private WorkItem()
            {
                _messageId = string.Empty;
                _popReceipt = string.Empty;
            }

            public WorkItem(QueueClient queue, string messageId, string popReceipt, RunsWorkItemValue? value)
            {
                _queue = queue;
                _messageId = messageId;
                _popReceipt = popReceipt;
                Value = value;
            }

            public RunsWorkItemValue? Value { get; private set; }

            public Task AcknowledgeAsync()
            {
                return _queue?.DeleteMessageAsync(_messageId, _popReceipt) ?? throw new Exception("no value was retrieved");
            }
        }
    }
}
