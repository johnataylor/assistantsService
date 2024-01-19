using Azure.Storage.Queues;
using System.Text.Json;
using System.Text;

namespace AssistantsProxy.Services
{
    public class AzureWorkQueue<T> : IWorkItemQueue<T>
    {
        private readonly QueueClient _queueClient;

        public AzureWorkQueue(IConfiguration configuration, string queueName)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _queueClient = new QueueClient(connectionString, queueName);
        }

        public async Task EnqueueAsync(T value)
        {
            await _queueClient.SendMessageAsync(new BinaryData(value));
        }

        public async Task<IWorkItem<T>> DequeueAsync()
        {
            var response = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromSeconds(30));

            if (response.Value.Length > 0)
            {
                var queueMessage = response.Value[0];

                var json = Encoding.UTF8.GetString(queueMessage.Body);
                var value = JsonSerializer.Deserialize<T>(json) ?? throw new Exception("expected a RunsWorkItemValue");
                return new AzureStorageQueueWorkItem<T>(_queueClient, queueMessage.MessageId, queueMessage.PopReceipt, value);
            }

            return AzureStorageQueueWorkItem<T>.Empty;
        }
    }
}
