using LoLChampionApi.Data;
using LoLChampionApi.Repositories;
using LoLChampionApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core with PostgreSQL
// Use connection string from configuration or environment variable (defaulting for Docker usage)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=postgres;Port=5432;Database=lol_champions;Username=postgres;Password=password";

builder.Services.AddDbContext<ChampionContext>(options =>
    options.UseNpgsql(connectionString));

// Register repository (using Scoped now because DbContext is Scoped)
builder.Services.AddScoped<IChampionRepository, PostgresChampionRepository>();
builder.Services.AddScoped<IChampionService, ChampionService>();

var app = builder.Build();

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChampionContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
