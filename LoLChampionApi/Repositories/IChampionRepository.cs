using LoLChampionApi.Models;

namespace LoLChampionApi.Repositories;

public interface IChampionRepository
{
    Task<IEnumerable<Champion>> GetAllAsync();
    Task<Champion?> GetByIdAsync(int id);
    Task<Champion> CreateAsync(Champion champion);
    Task<bool> UpdateAsync(Champion champion);
    Task<bool> DeleteAsync(int id);
}
