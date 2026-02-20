using LoLChampionApi.Repositories;
using LoLChampionApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IChampionRepository, InMemoryChampionRepository>();
builder.Services.AddScoped<IChampionService, ChampionService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
