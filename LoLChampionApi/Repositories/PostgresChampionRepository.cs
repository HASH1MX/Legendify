using LoLChampionApi.Data;
using LoLChampionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoLChampionApi.Repositories;

public class PostgresChampionRepository : IChampionRepository
{
    private readonly ChampionContext _context;

    public PostgresChampionRepository(ChampionContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Champion>> GetAllAsync()
    {
        return await _context.Champions.ToListAsync();
    }

    public async Task<Champion?> GetByIdAsync(int id)
    {
        return await _context.Champions.FindAsync(id);
    }

    public async Task<Champion> CreateAsync(Champion champion)
    {
        _context.Champions.Add(champion);
        await _context.SaveChangesAsync();
        return champion;
    }

    public async Task<bool> UpdateAsync(Champion champion)
    {
        var existing = await _context.Champions.FindAsync(champion.Id);
        if (existing == null)
        {
            return false;
        }

        existing.Name = champion.Name;
        existing.Role = champion.Role;
        existing.ImageUrl = champion.ImageUrl;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Champions.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        _context.Champions.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
