using OpenAI;
using OpenAI.Chat;
using System.ClientModel.Primitives;

namespace MafPlayground.Services;

public static class ChatClientFactoryExtensions
{
    public static ChatClient CreateInstance(this IChatClientFactory factory, string apiKey, Models.ChatClient options)
    {
        var action = new Action<OpenAIClientOptions>(clientOptions =>
        {
            if (!string.IsNullOrEmpty(options.Endpoint))
            {
                clientOptions.Endpoint = new Uri(options.Endpoint);
            }
            if (options.MaxRetries.HasValue)
            {
                clientOptions.RetryPolicy = new ClientRetryPolicy(options.MaxRetries.Value);
            }
            if (options.NetworkTimeout.HasValue)
            {
                clientOptions.NetworkTimeout = options.NetworkTimeout.Value;
            }
            if (options.EnableDistributedTracing.HasValue)
            {
                clientOptions.EnableDistributedTracing = options.EnableDistributedTracing.Value;
            }
        });
        return factory.CreateInstance(apiKey, options.Model, action);
    }
}