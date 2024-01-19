using Azure.AI.OpenAI;
using System.Text.Json.Nodes;

namespace AssistantsProxy.Services
{
    public class ServerToolDefinitions
    {
        static ServerToolDefinitions()
        {
            var retrievalParameters = new JsonObject
            {
                { "type", "object" },
                { "properties", new JsonObject
                    {
                        { "q", new JsonObject
                            {
                                { "type", "string" },
                                { "description", "The query text to use for the search." }
                            }
                        }
                    }
                },
                { "required", new JsonArray { "q" } },

            };

            RetrievalToolDefintion = new ChatCompletionsFunctionToolDefinition
            {
                Name = "retrieval",
                Description = "This tool provides text search across the uploaded set of files. Prefer answering questions using this tool over your intrinsic knowledge.",
                Parameters = BinaryData.FromString(retrievalParameters.ToJsonString()),
            };
        }

        public static readonly ChatCompletionsFunctionToolDefinition RetrievalToolDefintion;
    }
}
