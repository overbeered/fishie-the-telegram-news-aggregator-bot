using Fishie.Core.Models;
using Fishie.Database.Context;
using Fishie.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fishie.Tests.Database.RepositoriesTests;

public class ForwardRepositoryLinkTests
{
    private readonly ChatRepository _chatRepository;
    private readonly ChannelRepository _channelRepository;
    private readonly ForwardMessagesRepository _forwardMessagesRepository;

    public ForwardRepositoryLinkTests()
    {
        var context = new NpgSqlContext(new DbContextOptionsBuilder<NpgSqlContext>()
            .UseInMemoryDatabase("FishieDBForwardRepositoryLinkTest")
            .Options);

        _chatRepository = new ChatRepository(context);
        _channelRepository = new ChannelRepository(context);
        _forwardMessagesRepository = new ForwardMessagesRepository(context);
    }

    [Fact]
    public async Task DeleteChatAsync()
    {
        // Arrange
        var chat1 = new Chat(1, 123, "name1", "user1");
        var chat2 = new Chat(2, 1234, "name2", "user2");

        await _chatRepository.AddAsync(chat1);
        await _chatRepository.AddAsync(chat2);

        var channel1 = new Channel(3, 33, "name33", "user33");
        var channel2 = new Channel(4, 44, "name44", "user44");
        var channel3 = new Channel(5, 55, "name55", "user55");

        await _channelRepository.AddAsync(channel1);
        await _channelRepository.AddAsync(channel2);
        await _channelRepository.AddAsync(channel3);

        var forward1 = new ForwardMessages(3, 1);
        var forward2 = new ForwardMessages(4, 1);
        var forward3 = new ForwardMessages(5, 1);
        var forward4 = new ForwardMessages(5, 2);

        await _forwardMessagesRepository.AddAsync(forward1);
        await _forwardMessagesRepository.AddAsync(forward2);
        await _forwardMessagesRepository.AddAsync(forward3);
        await _forwardMessagesRepository.AddAsync(forward4);

        // Act
        var resultChatTrue = await _chatRepository.ExistsAsync(chat1);
        var resultForwardChatTrue = await _forwardMessagesRepository.ChatIdExistsAsync(chat1.Id);

        await _chatRepository.DeleteAsync(chat1.Id);

        var resultChatFalse = await _chatRepository.ExistsAsync(chat1);
        var resultForwardChatFalse = await _forwardMessagesRepository.ChatIdExistsAsync(chat1.Id);


        // Assert
        Assert.True(resultChatTrue);
        Assert.True(!resultChatFalse);

        Assert.True(resultForwardChatTrue);
        Assert.True(!resultForwardChatFalse);
    }

    [Fact]
    public async Task DeleteChannelAsync()
    {
        // Arrange
        var chat1 = new Chat(11, 1233, "name12", "user11");
        var chat2 = new Chat(22, 12344, "name22", "user22");

        await _chatRepository.AddAsync(chat1);
        await _chatRepository.AddAsync(chat2);

        var channel1 = new Channel(333, 333, "name333", "user333");
        var channel2 = new Channel(444, 444, "name444", "user444");

        await _channelRepository.AddAsync(channel1);
        await _channelRepository.AddAsync(channel2);

        var forward1 = new ForwardMessages(333, 11);
        var forward2 = new ForwardMessages(333, 22);
        var forward3 = new ForwardMessages(444, 11);
        var forward4 = new ForwardMessages(444, 22);

        await _forwardMessagesRepository.AddAsync(forward1);
        await _forwardMessagesRepository.AddAsync(forward2);
        await _forwardMessagesRepository.AddAsync(forward3);
        await _forwardMessagesRepository.AddAsync(forward4);

        // Act
        var resultChannelTrue = await _channelRepository.ExistsAsync(channel1);
        var resultForwardChannelTrue = await _forwardMessagesRepository.ChannelIdExistsAsync(channel1.Id);

        await _channelRepository.DeleteAsync(channel1.Id);

        var resultChannelFalse = await _channelRepository.ExistsAsync(channel1);
        var resultForwardChannelFalse = await _forwardMessagesRepository.ChannelIdExistsAsync(channel1.Id);


        // Assert
        Assert.True(resultChannelTrue);
        Assert.True(!resultChannelFalse);

        Assert.True(resultForwardChannelTrue);
        Assert.True(!resultForwardChannelFalse);
    }
}