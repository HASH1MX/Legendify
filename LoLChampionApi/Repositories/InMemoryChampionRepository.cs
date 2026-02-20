using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using LoLChampionApi.Models;

namespace LoLChampionApi.Repositories;

public class InMemoryChampionRepository : IChampionRepository
{
    private readonly ConcurrentDictionary<int, Champion> _champions = new();
    private int _nextId;

    public Task<IEnumerable<Champion>> GetAllAsync()
    {
        return Task.FromResult(_champions.Values.AsEnumerable());
    }

    public Task<Champion?> GetByIdAsync(int id)
    {
        _champions.TryGetValue(id, out var champion);
        return Task.FromResult(champion);
    }

    public Task<Champion> CreateAsync(Champion champion)
    {
        var id = Interlocked.Increment(ref _nextId);
        var storedChampion = new Champion
        {
            Id = id,
            Name = champion.Name,
            Role = champion.Role,
            ImageUrl = champion.ImageUrl
        };

        _champions[id] = storedChampion;

        return Task.FromResult(storedChampion);
    }

    public Task<bool> UpdateAsync(Champion champion)
    {
        if (!_champions.ContainsKey(champion.Id))
        {
            return Task.FromResult(false);
        }

        var updatedChampion = new Champion
        {
            Id = champion.Id,
            Name = champion.Name,
            Role = champion.Role,
            ImageUrl = champion.ImageUrl
        };

        _champions[champion.Id] = updatedChampion;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(_champions.TryRemove(id, out _));
    }
}
