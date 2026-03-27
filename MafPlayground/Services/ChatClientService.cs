using MafPlayground.Models;
using MafPlayground.Repositories;

namespace MafPlayground.Services;

public interface IChatClientService
{
    Task<IEnumerable<ChatClient>> GetAll(CancellationToken cancellationToken = default);
    Task<ChatClient?> Get(string id, CancellationToken cancellationToken = default);
    Task<ChatClient> Create(ChatClient chatClient, CancellationToken cancellationToken = default);
    Task<ChatClient> Update(ChatClient chatClient, CancellationToken cancellationToken = default);
    Task<ChatClient> Delete(string id, CancellationToken cancellationToken = default);
}

internal class ChatClientService(IChatClientRepository repository) : IChatClientService
{
    public Task<IEnumerable<ChatClient>> GetAll(CancellationToken cancellationToken = default)
    {
        return repository.GetAll(cancellationToken);
    }
    public Task<ChatClient?> Get(string id, CancellationToken cancellationToken = default)
    {
        return repository.Get(id, cancellationToken) ?? Task.FromResult<ChatClient?>(null);
    }
    public Task<ChatClient> Create(ChatClient chatClient, CancellationToken cancellationToken = default)
    {
        return repository.Create(chatClient, cancellationToken);
    }
    public Task<ChatClient> Update(ChatClient chatClient, CancellationToken cancellationToken = default)
    {
        return repository.Update(chatClient, cancellationToken);
    }
    public Task<ChatClient> Delete(string id, CancellationToken cancellationToken = default)
    {
        return repository.Delete(id, cancellationToken);
    }
}