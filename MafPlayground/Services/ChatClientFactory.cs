using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;

namespace MafPlayground.Services;

public interface IChatClientFactory
{
    ChatClient CreateInstance(string apiKey, string model, Action<OpenAIClientOptions>? action);
}

public class ChatClientFactory(ILoggerFactory loggerFactory) : IChatClientFactory
{
    public ChatClient CreateInstance(string apiKey, string model, Action<OpenAIClientOptions>? action)
    {
        var credential = new ApiKeyCredential(apiKey);

        // create openAIClientOptions instance with default options
        var options = new OpenAIClientOptions
        {
            // Enable detailed logging of requests and responses for debugging purposes
            ClientLoggingOptions = new ClientLoggingOptions
            {
                EnableLogging = true,
                EnableMessageContentLogging = true,
                EnableMessageLogging = true,
                LoggerFactory = loggerFactory
            },
            // TODO: Consider allowing users to specify a custom User-Agent string or application ID for better telemetry and debugging
            // UserAgentApplicationId = ????,
        };

        // Add a custom policy to log raw request and response details
        options.AddPolicy(new RawLoggingPolicy(), PipelinePosition.PerCall);

        action?.Invoke(options);

        return new ChatClient(model, credential, options);
    }
}
