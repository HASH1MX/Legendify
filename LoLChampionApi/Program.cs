using LoLChampionApi.Data;
using LoLChampionApi.Repositories;
using LoLChampionApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Local React dev server
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

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

// Auto-migrate database on startup with retry policy
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ChampionContext>();

    try
    {
        logger.LogInformation("Attempting to apply database migrations...");
        
        // Simple retry logic
        int retries = 5;
        while (retries > 0)
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    logger.LogInformation("Database migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation("No pending migrations found. Database is up to date.");
                }
                break; // Success
            }
            catch (Exception ex)
            {
                retries--;
                logger.LogError(ex, "An error occurred while migrating the database. Retries remaining: {Retries}", retries);
                if (retries == 0) throw;
                Thread.Sleep(2000); // Wait 2 seconds before retrying
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
