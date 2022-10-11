using Fishie.Core.Models;
using Fishie.Database.Context;
using Fishie.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fishie.Tests.Database.RepositoriesTests;

public class ChatRepositoryTests
{
    private readonly ChatRepository _chatRepository;

    public ChatRepositoryTests()
    {
        _chatRepository = new ChatRepository(new NpgSqlContext(new DbContextOptionsBuilder<NpgSqlContext>()
            .UseInMemoryDatabase("FishieDBChatRepositoryTest")
            .Options));
    }

    [Fact]
    public async Task ExistsFalseAsync()
    {
        // Arrange & Act
        var result = await _chatRepository.ExistsAsync(new Chat(12, 123, "name", "user"));

        // Assert
        Assert.True(!result);
    }

    [Fact]
    public async Task AddAsync()
    {
        // Arrange
        var chat = new Chat(1, 123, "name", "user");

        // Act
        await _chatRepository.AddAsync(chat);
        var result = await _chatRepository.ExistsAsync(chat);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteChatNameAsync()
    {
        // Arrange
        var chat = new Chat(2, 1234, "name2", "user2");
        await _chatRepository.AddAsync(chat);

        // Act
        var resultTrue = await _chatRepository.ExistsAsync(chat);
        await _chatRepository.DeleteAsync(chat.Name!);
        var resultFalse = await _chatRepository.ExistsAsync(chat);

        // Assert
        Assert.True(resultTrue);
        Assert.True(!resultFalse);
    }

    [Fact]
    public async Task DeleteChatIdAsync()
    {
        // Arrange
        var chat = new Chat(2, 1234, "name2", "user2");
        await _chatRepository.AddAsync(chat);

        // Act
        var resultTrue = await _chatRepository.ExistsAsync(chat);
        await _chatRepository.DeleteAsync(chat.Id!);
        var resultFalse = await _chatRepository.ExistsAsync(chat);

        // Assert
        Assert.True(resultTrue);
        Assert.True(!resultFalse);
    }

    [Fact]
    public async Task FindChatNameAsync()
    {
        // Arrange
        var chat = new Chat(3, 12345, "name3", "user3");
        await _chatRepository.AddAsync(chat);

        // Act
        var result = await _chatRepository.FindAsync(chat.Name!);

        // Assert
        Assert.Equal(chat.Id, result!.Id);
        Assert.Equal(chat.Name, result!.Name);
        Assert.Equal(chat.AccessHash, result!.AccessHash);
        Assert.Equal(chat.Username, result!.Username);
    }

    [Fact]
    public async Task FindChatIdAsync()
    {
        // Arrange
        var chat = new Chat(4, 123456, "name4", "user4");
        await _chatRepository.AddAsync(chat);

        // Act
        var result = await _chatRepository.FindAsync(chat.Id!);

        // Assert
        Assert.Equal(chat.Id, result!.Id);
        Assert.Equal(chat.Name, result!.Name);
        Assert.Equal(chat.AccessHash, result!.AccessHash);
        Assert.Equal(chat.Username, result!.Username);
    }

    [Fact]
    public async Task FindAllAsync()
    {
        // Arrange
        var chat = new Chat(5, 1234567, "name5", "user5");
        await _chatRepository.AddAsync(chat);

        // Act
        var result = await _chatRepository.FindAllAsync();

        // Assert
        Assert.NotNull(result);
    }
}