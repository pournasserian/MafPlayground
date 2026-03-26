using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace MafPlayground.Services;

public interface IChatAgentFactory
{
    ChatClientAgent CreateAgent(ChatClient chatClient, string? instructions = null, string? name = null, string? description = null, IList<AITool>? tools = null);
    ChatClientAgent CreateAgent(ChatClient chatClient, Action<ChatClientAgentOptions>? action);
}

public class ChatAgentFactory : IChatAgentFactory
{
    private readonly IChatClientFactory _chatClientFactory;
    private readonly ILogger<ChatAgentFactory> _logger;
    private readonly IServiceProvider _services;
    private readonly ILoggerFactory _loggerFactory;

    public ChatAgentFactory(IChatClientFactory chatClientFactory, ILogger<ChatAgentFactory> logger, IServiceProvider services, ILoggerFactory loggerFactory)
    {
        _chatClientFactory = chatClientFactory;
        _logger = logger;
        _services = services;
        _loggerFactory = loggerFactory;
    }

    public ChatClientAgent CreateAgent(ChatClient chatClient, Action<ChatClientAgentOptions>? action)
    {
        var options = new ChatClientAgentOptions
        {
            Id = Guid.NewGuid().ToString(),
            Description = "A helpful assistant that can answer questions and perform tasks.",
            Name = "HelperBot",
            ChatOptions = new ChatOptions
            {


            }
        };
        action?.Invoke(options);
        return chatClient.AsAIAgent(options);
    }

    public ChatClientAgent CreateAgent(ChatClient chatClient,
        string? instructions = null,
        string? name = null,
        string? description = null,
        IList<AITool>? tools = null)
    {
        return chatClient.AsAIAgent(instructions, name, description, tools, null, _loggerFactory, _services);
    }
}
