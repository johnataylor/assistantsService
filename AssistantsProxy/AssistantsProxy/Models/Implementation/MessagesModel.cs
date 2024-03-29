﻿using AssistantsProxy.Schema;
using Azure.Storage.Blobs;

namespace AssistantsProxy.Models.Implementation
{
    public class MessagesModel : IMessagesModel, IMessagesDelete
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<MessagesModel> _logger;
        private const string ContainerName = "messages";

        public MessagesModel(IConfiguration configuration, ILogger<MessagesModel> logger)
        {
            var connectionString = configuration["BlobConnectionString"] ?? throw new ArgumentException("you must configure a blob storage connection string");
            _containerClient = new BlobContainerClient(connectionString, ContainerName);
            _logger = logger;
        }

        public async Task<ThreadMessage?> CreateAsync(string threadId, MessageCreateParams messageCreateParams, string? bearerToken)
        {
            // TODO check the thread exists

            var newThreadMessage = new ThreadMessage
            {
                Object = "thread.message",
                Id = $"msg_{Guid.NewGuid()}",
                Content = new []
                {
                    new MessageContent
                    {
                        Text = new MessageContentText
                        {
                            Value = messageCreateParams.Content
                        },
                        Type = "text"
                    }
                },
                Role = messageCreateParams.Role,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                ThreadId = threadId
            };

            var blobName = GetBlobName(threadId);
            var blobClient = _containerClient.GetBlobClient(blobName);

            var threadMessages = await blobClient.ExistsAsync()
                    ?
                await BlobStorageHelpers.DownloadAsync<List<ThreadMessage>>(_containerClient, blobName) ?? new List<ThreadMessage>()
                    :
                new List<ThreadMessage>();

            threadMessages.Insert(0, newThreadMessage);

            await blobClient.UploadAsync(new BinaryData(threadMessages), true);

            _logger.LogInformation($"Create Message {newThreadMessage.Id}");

            return newThreadMessage;
        }

        public async Task<AssistantList<ThreadMessage>?> ListAsync(string threadId, string? bearerToken)
        {
            // TODO check the thread exists

            var blobName = GetBlobName(threadId);

            var threadMessages = await BlobStorageHelpers.DownloadAsync<List<ThreadMessage>>(_containerClient, blobName) ?? new List<ThreadMessage>();

            var assistantList = new AssistantList<ThreadMessage>
            {
                Object = "list",
                Data = threadMessages.ToArray()
            };

            return assistantList;
        }

        public async Task<ThreadMessage?> RetrieveAsync(string threadId, string messageId, string? bearerToken)
        {
            // TODO check the thread exists

            var blobName = GetBlobName(threadId);

            var threadMessages = await BlobStorageHelpers.DownloadAsync<List<ThreadMessage>>(_containerClient, blobName) ?? new List<ThreadMessage>();

            var threadMessage = threadMessages.FirstOrDefault(item => item.Id == messageId);

            return threadMessage;
        }

        public Task<ThreadMessage?> UpdateAsync(string threadId, string messageId, MessageUpdateParams messageUpdateParams, string? bearerToken)
        {
            // the update only applies to Metadata, so there is little point in implementing this until we have that implemented

            // load
            // update the message in the list
            // save
            // return

            // TODO: and add appropriate eTag checks and retries for concurrency control

            throw new NotImplementedException();
        }

        public Task DeleteMessages(string threadId)
        {
            var blobName = GetBlobName(threadId);

            // TODO: check if exists

            return _containerClient.DeleteBlobAsync(blobName);
        }

        private static string GetBlobName(string threadId) => $"{threadId}_messages";
    }
}
