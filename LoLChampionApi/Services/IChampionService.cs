using LoLChampionApi.Models;

namespace LoLChampionApi.Services;

public interface IChampionService
{
    Task<IEnumerable<Champion>> GetAllAsync();
    Task<Champion?> GetByIdAsync(int id);
    Task<Champion> CreateAsync(Champion champion);
    Task<bool> UpdateAsync(int id, Champion champion);
    Task<bool> DeleteAsync(int id);
}
