using LoLChampionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LoLChampionApi.Data;

public class ChampionContext : DbContext
{
    public ChampionContext(DbContextOptions<ChampionContext> options) : base(options)
    {
    }

    public DbSet<Champion> Champions { get; set; } = null!;
}
