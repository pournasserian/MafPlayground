using MafPlayground.Models;
using System.Text.Json;

namespace MafPlayground.Repositories;

internal class JsonChatClientRepository : IChatClientRepository
{
    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "chatclients.json");
    
    public async Task<ChatClient?> Get(string id, CancellationToken cancellationToken = default)
    {
        var clients = await GetAll(cancellationToken);
        return clients.SingleOrDefault(c => c.Id == id);
    }
    public async Task<ChatClient> Create(ChatClient chatClient, CancellationToken cancellationToken = default)
    {
        var clients = (await GetAll(cancellationToken)).ToList();
        clients.Add(chatClient);
        await SaveAll(clients, cancellationToken);
        return chatClient;
    }
    public async Task<ChatClient> Update(ChatClient chatClient, CancellationToken cancellationToken = default)
    {
        var clients = (await GetAll(cancellationToken)).ToList();
        var index = clients.FindIndex(c => c.Id == chatClient.Id);
        if (index >= 0)
        {
            clients[index] = chatClient;
            await SaveAll(clients, cancellationToken);
            return chatClient;
        }
        else { 
            throw new KeyNotFoundException($"ChatClient with Id {chatClient.Id} not found.");
        }
    }
    public async Task<ChatClient> Delete(string id, CancellationToken cancellationToken = default)
    {
        var clients = (await GetAll(cancellationToken)).ToList();
        var index = clients.FindIndex(c => c.Id == id);
        if (index >= 0)
        {
            var deletedClient = clients[index];
            clients.RemoveAt(index);
            await SaveAll(clients, cancellationToken);
            return deletedClient;
        }
        else
        {
            throw new KeyNotFoundException($"ChatClient with Id {id} not found.");
        }
    }
    public async Task<IEnumerable<ChatClient>> GetAll(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath))
            return [];
        var json = await File.ReadAllTextAsync(_filePath, cancellationToken);
        return JsonSerializer.Deserialize<List<ChatClient>>(json) ?? new List<ChatClient>();
    }
    private async Task SaveAll(List<ChatClient> clients, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, json, cancellationToken);
    }
}