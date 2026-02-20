using LoLChampionApi.Models;
using LoLChampionApi.Repositories;

namespace LoLChampionApi.Services;

public class ChampionService : IChampionService
{
    private readonly IChampionRepository _repository;

    public ChampionService(IChampionRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Champion>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Champion?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<Champion> CreateAsync(Champion champion)
    {
        return _repository.CreateAsync(champion);
    }

    public async Task<bool> UpdateAsync(int id, Champion champion)
    {
        if (id != champion.Id)
        {
            return false;
        }

        return await _repository.UpdateAsync(champion);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }
}
