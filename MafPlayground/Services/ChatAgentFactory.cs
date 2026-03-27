using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace MafPlayground.Services;

public interface IChatAgentFactory
{
    ChatClientAgent CreateInstance(ChatClient chatClient, string? agentName = null, string? description = null, Action<ChatClientAgentOptions>? action = null);
}

public class ChatAgentFactory : IChatAgentFactory
{
    public ChatClientAgent CreateInstance(ChatClient chatClient, string? agentName = default, string? description = default, Action<ChatClientAgentOptions>? action = default)
    {
        var options = new ChatClientAgentOptions
        {
            Id = Guid.NewGuid().ToString(),
            Description = description,
            Name = agentName
        };
        action?.Invoke(options);
        return chatClient.AsAIAgent(options);
    }
}
