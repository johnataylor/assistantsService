using AssistantsProxy.Models.Implementation;
using AssistantsProxy.Models;
using AssistantsProxy.Services;
using AssistantsProxy.Models.Proxy;

namespace AssistantsProxy
{
    public static class RuntimeExtensions
    {
        public static IServiceCollection AddAssistantsRuntime(this IServiceCollection services)
        {
            // you need to create 5 Azure Blob Storage Containers and an Azure Storage Queue
            // then you'll need configuration for "OpenAIKey," "DeploymentOrModelName" and "BlobConnectionString"
            services.AddScoped<IAssistantsModel, AssistantsModel>();
            services.AddScoped<IThreadsModel, ThreadsModel>();
            services.AddScoped<IMessagesModel, MessagesModel>();
            services.AddScoped<IRunsModel, RunsModel>();
            services.AddScoped<IStepsModel, StepsModel>();
            services.AddSingleton<IRunsWorkQueue<RunsWorkItemValue>, RunsWorkQueue>();
            services.AddSingleton<IChatClient, ChatClient>();
            services.AddHostedService<RunsHostedService>();
            return services;
        }

        public static IServiceCollection AddAssistantsPassThroughProxy(this IServiceCollection services)
        {
            // the pass through proxy serializes and deserializes the traffic into the .NET objects used by the runtime - it's a test
            services.AddScoped<IAssistantsModel, ProxyAssistantsModel>();
            services.AddScoped<IThreadsModel, ProxyThreadsModel>();
            services.AddScoped<IMessagesModel, ProxyMessagesModel>();
            services.AddScoped<IRunsModel, ProxyRunsModel>();
            services.AddScoped<IStepsModel, ProxyStepsModel>();
            return services;
        }
    }
}
