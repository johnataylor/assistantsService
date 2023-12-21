using AssistantsProxy.Schema;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text.Json;
using System.Text;

namespace AssistantsProxy
{
    public static class BlobStorageHelpers
    {
        public static async Task<AssistantList<T>?> ListAsync<T>(BlobContainerClient containerClient)
        {
            var names = new List<string>();
            await foreach (Page<BlobItem> blobPage in containerClient.GetBlobsAsync().AsPages())
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    names.Add(blobItem.Name);
                }
            }

            var objs = new List<T>();
            foreach (var blobName in names)
            {
                var obj = await DownloadAsync<T>(containerClient, blobName);
                if (obj != null)
                {
                    objs.Add(obj);
                }
            }

            var assistantList = new AssistantList<T>
            {
                Data = objs.ToArray()
            };

            return assistantList;
        }

        public static async Task<T?> DownloadAsync<T>(BlobContainerClient containerClient, string blobName)
        {
            try
            {
                var blobClient = containerClient.GetBlobClient(blobName);
                var download = await blobClient.DownloadContentAsync();
                var json = Encoding.UTF8.GetString(download.Value.Content);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (RequestFailedException)
            {
                return default;
            }
        }
    }
}
