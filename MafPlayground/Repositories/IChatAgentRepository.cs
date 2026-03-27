using MafPlayground.Models;

namespace MafPlayground.Repositories;

public interface IChatAgentRepository
{
    Task<ChatAgent?> Get(string id, CancellationToken cancellationToken = default);
    Task<ChatAgent> Create(ChatAgent chatAgent, CancellationToken cancellationToken = default);
    Task<ChatAgent> Update(ChatAgent chatAgent, CancellationToken cancellationToken = default);
    Task<ChatAgent> Delete(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatAgent>> GetAll(CancellationToken cancellationToken = default);
}
