using LoLChampionApi.Models;
using LoLChampionApi.Repositories;
using LoLChampionApi.Services;
using Moq;
using Xunit;

namespace LoLChampionApi.Tests;

public class ChampionServiceTests
{
    private readonly Mock<IChampionRepository> _mockRepo;
    private readonly ChampionService _service;

    public ChampionServiceTests()
    {
        _mockRepo = new Mock<IChampionRepository>();
        _service = new ChampionService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllChampions()
    {
        // Arrange
        var champions = new List<Champion>
        {
            new() { Id = 1, Name = "Ahri", Role = "Mage" },
            new() { Id = 2, Name = "Garen", Role = "Fighter" }
        };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(champions);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        _mockRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsChampion_WhenExists()
    {
        // Arrange
        var champion = new Champion { Id = 1, Name = "Ahri" };
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(champion);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ahri", result!.Name);
    }

    [Fact]
    public async Task CreateAsync_CallsRepository()
    {
        // Arrange
        var champion = new Champion { Name = "Zed", Role = "Assassin" };
        _mockRepo.Setup(repo => repo.CreateAsync(champion)).ReturnsAsync(champion);

        // Act
        var result = await _service.CreateAsync(champion);

        // Assert
        Assert.Equal("Zed", result.Name);
        _mockRepo.Verify(repo => repo.CreateAsync(champion), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenIdMismatch()
    {
        // Arrange
        var champion = new Champion { Id = 2, Name = "Jinx" };

        // Act
        var result = await _service.UpdateAsync(1, champion); // Mismatched ID

        // Assert
        Assert.False(result);
        _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Champion>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrue_WhenIdMatches()
    {
        // Arrange
        var champion = new Champion { Id = 1, Name = "Jinx" };
        _mockRepo.Setup(repo => repo.UpdateAsync(champion)).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateAsync(1, champion);

        // Assert
        Assert.True(result);
        _mockRepo.Verify(repo => repo.UpdateAsync(champion), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_CallsRepository()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
        _mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }
}
