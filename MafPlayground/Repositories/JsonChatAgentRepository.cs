using MafPlayground.Models;
using System.Text.Json;

namespace MafPlayground.Repositories;

internal class JsonChatAgentRepository : IChatAgentRepository
{
    private readonly static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "chatagents.json");
    private readonly static JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };
    public async Task<ChatAgent?> Get(string id, CancellationToken cancellationToken = default)
    {
        var agents = await GetAll(cancellationToken);
        return agents.SingleOrDefault(c => c.Id == id);
    }
    public async Task<ChatAgent> Create(ChatAgent chatAgent, CancellationToken cancellationToken = default)
    {
        var agents = (await GetAll(cancellationToken)).ToList();
        agents.Add(chatAgent);
        await SaveAll(agents, cancellationToken);
        return chatAgent;
    }
    public async Task<ChatAgent> Update(ChatAgent chatAgent, CancellationToken cancellationToken = default)
    {
        var agents = (await GetAll(cancellationToken)).ToList();
        var index = agents.FindIndex(c => c.Id == chatAgent.Id);
        if (index >= 0)
        {
            agents[index] = chatAgent;
            await SaveAll(agents, cancellationToken);
            return chatAgent;
        }
        else
        {
            throw new KeyNotFoundException($"ChatAgent with Id {chatAgent.Id} not found.");
        }
    }
    public async Task<ChatAgent> Delete(string id, CancellationToken cancellationToken = default)
    {
        var agents = (await GetAll(cancellationToken)).ToList();
        var index = agents.FindIndex(c => c.Id == id);
        if (index >= 0)
        {
            var deletedAgent = agents[index];
            agents.RemoveAt(index);
            await SaveAll(agents, cancellationToken);
            return deletedAgent;
        }
        else
        {
            throw new KeyNotFoundException($"ChatAgent with Id {id} not found.");
        }
    }
    public async Task<IEnumerable<ChatAgent>> GetAll(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath))
            return [];
        var json = await File.ReadAllTextAsync(_filePath, cancellationToken);
        return JsonSerializer.Deserialize<List<ChatAgent>>(json) ?? [];
    }
    private async Task SaveAll(List<ChatAgent> agents, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(agents, _jsonSerializerOptions);
        await File.WriteAllTextAsync(_filePath, json, cancellationToken);
    }
}