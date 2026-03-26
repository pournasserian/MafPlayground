using MafPlayground.Models;

namespace MafPlayground.Repositories;

public interface IChatClientRepository
{
    Task<ChatClient?> Get(string id, CancellationToken cancellationToken = default);
    Task<ChatClient> Create(ChatClient chatClient, CancellationToken cancellationToken = default);
    Task<ChatClient> Update(ChatClient chatClient, CancellationToken cancellationToken = default);
    Task<ChatClient> Delete(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatClient>> GetAll(CancellationToken cancellationToken = default);
}
