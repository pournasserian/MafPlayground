using MafPlayground.Models;
using MafPlayground.Repositories;

namespace MafPlayground.Services;

public interface IChatAgentService
{
    Task<IEnumerable<ChatAgent>> GetAll(CancellationToken cancellationToken = default);
    Task<ChatAgent?> Get(string id, CancellationToken cancellationToken = default);
    Task<ChatAgent> Create(ChatAgent chatAgent, CancellationToken cancellationToken = default);
    Task<ChatAgent> Update(ChatAgent chatAgent, CancellationToken cancellationToken = default);
    Task<ChatAgent> Delete(string id, CancellationToken cancellationToken = default);
}

internal class ChatAgentService(IChatAgentRepository repository) : IChatAgentService
{
    private readonly IChatAgentRepository _repository = repository;

    public Task<IEnumerable<ChatAgent>> GetAll(CancellationToken cancellationToken = default)
    {
        return _repository.GetAll(cancellationToken);
    }

    public Task<ChatAgent?> Get(string id, CancellationToken cancellationToken = default)
    {
        return _repository.Get(id, cancellationToken);
    }

    public Task<ChatAgent> Create(ChatAgent chatAgent, CancellationToken cancellationToken = default)
    {
        return _repository.Create(chatAgent, cancellationToken);
    }

    public Task<ChatAgent> Update(ChatAgent chatAgent, CancellationToken cancellationToken = default)
    {
        return _repository.Update(chatAgent, cancellationToken);
    }

    public Task<ChatAgent> Delete(string id, CancellationToken cancellationToken = default)
    {
        return _repository.Delete(id, cancellationToken);
    }
}