using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace MafPlayground.Services;

public static class ChatAgentFactoryExtensions
{
    public static ChatClientAgent CreateInstance(this IChatAgentFactory factory, ChatClient chatClient, Models.ChatAgent chatAgent)
    {
        var action = (ChatClientAgentOptions agentOptions) =>
        {
            agentOptions.ChatOptions ??= new ChatOptions();
            agentOptions.Name = chatAgent.Name;
            agentOptions.Description = chatAgent.Description;
            //agentOptions.Id = ????
            var options = agentOptions.ChatOptions;
            options.Instructions = chatAgent.Instructions;
            options.Temperature = chatAgent.Temperature;
            options.MaxOutputTokens = chatAgent.MaxOutputTokens;
            options.TopP = chatAgent.TopP;
            options.TopK = chatAgent.TopK;
            options.FrequencyPenalty = chatAgent.FrequencyPenalty;
            options.PresencePenalty = chatAgent.PresencePenalty;
            options.Seed = chatAgent.Seed;
            options.ModelId = chatAgent.ModelId;
            options.StopSequences = chatAgent.StopSequences;
            options.AllowMultipleToolCalls = chatAgent.AllowMultipleToolCalls;
            options.Reasoning = new ReasoningOptions
            {
                Effort = ReasoningEffort.Medium,
                Output = ReasoningOutput.Full
            };
            options.AdditionalProperties = [];
            options.ToolMode = ChatToolMode.RequireAny;
            options.ResponseFormat = new ChatResponseFormatText();
            //AllowBackgroundResponses = chatAgent.AllowBackgroundResponses,
            //ConversationId = chatAgent.ConversationId,
        };
        return factory.CreateInstance(chatClient, chatAgent.Name, chatAgent.Description, action);
    }
}