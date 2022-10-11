using Fishie.Core.Models;
using Fishie.Database.Context;
using Fishie.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fishie.Tests.Database.RepositoriesTests;

public class ChannelRepositoryTests
{
    private readonly ChannelRepository _channelRepository;

    public ChannelRepositoryTests()
    {
        _channelRepository = new ChannelRepository(new NpgSqlContext(new DbContextOptionsBuilder<NpgSqlContext>()
            .UseInMemoryDatabase("FishieDBChannelRepositoryTest")
            .Options));
    }

    [Fact]
    public async Task ExistsFalseAsync()
    {
        // Arrange & Act
        var result = await _channelRepository.ExistsAsync(new Channel(9991, 123, "name", "user"));

        // Assert
        Assert.True(!result);
    }

    [Fact]
    public async Task AddAsync()
    {
        // Arrange
        var channel = new Channel(1, 123, "name", "user");

        // Act
        await _channelRepository.AddAsync(channel);
        var result = await _channelRepository.ExistsAsync(channel);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUsernameAsync()
    {
        // Arrange
        var channel = new Channel(2, 1234, "name", "user2");
        await _channelRepository.AddAsync(channel);

        // Act
        var resultTrue = await _channelRepository.ExistsAsync(channel);
        await _channelRepository.DeleteAsync(channel.Username!);
        var resultFalse = await _channelRepository.ExistsAsync(channel);

        // Assert
        Assert.True(resultTrue);
        Assert.True(!resultFalse);
    }

    [Fact]
    public async Task DeleteChannelIdAsync()
    {
        // Arrange
        var channel = new Channel(2, 1234, "name", "user2");
        await _channelRepository.AddAsync(channel);

        // Act
        var resultTrue = await _channelRepository.ExistsAsync(channel);
        await _channelRepository.DeleteAsync(channel.Id!);
        var resultFalse = await _channelRepository.ExistsAsync(channel);

        // Assert
        Assert.True(resultTrue);
        Assert.True(!resultFalse);
    }

    [Fact]
    public async Task FindChannelUsernameAsync()
    {
        // Arrange
        var channel = new Channel(3, 12345, "name", "user3");
        await _channelRepository.AddAsync(channel);

        // Act
        var result = await _channelRepository.FindAsync(channel.Username!);

        // Assert
        Assert.Equal(channel.Id, result!.Id);
        Assert.Equal(channel.Name, result!.Name);
        Assert.Equal(channel.AccessHash, result!.AccessHash);
        Assert.Equal(channel.Username, result!.Username);
    }

    [Fact]
    public async Task FindChannelIdAsync()
    {
        // Arrange
        var channel = new Channel(4, 123456, "name", "user4");
        await _channelRepository.AddAsync(channel);

        // Act
        var result = await _channelRepository.FindAsync(channel.Id!);

        // Assert
        Assert.Equal(channel.Id, result!.Id);
        Assert.Equal(channel.Name, result!.Name);
        Assert.Equal(channel.AccessHash, result!.AccessHash);
        Assert.Equal(channel.Username, result!.Username);
    }

    [Fact]
    public async Task FindAllAsync()
    {
        // Arrange
        var channel = new Channel(5, 1234567, "name", "user5");
        await _channelRepository.AddAsync(channel);

        // Act
        var result = await _channelRepository.FindAllAsync();

        // Assert
        Assert.NotNull(result);
    }
}