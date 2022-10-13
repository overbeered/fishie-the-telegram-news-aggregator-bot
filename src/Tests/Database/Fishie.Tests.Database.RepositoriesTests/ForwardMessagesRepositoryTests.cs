namespace Fishie.Tests.Database.RepositoriesTests;

public class ForwardMessagesRepositoryTests
{
    private readonly ForwardMessagesRepository _forwardMessagesRepository;

    public ForwardMessagesRepositoryTests()
    {
        _forwardMessagesRepository = new ForwardMessagesRepository(new NpgSqlContext(new DbContextOptionsBuilder<NpgSqlContext>()
            .UseInMemoryDatabase("FishieDBForwardMessagesRepositoryTest")
            .Options));
    }

    [Fact]
    public async Task ChatIdExistsFalseAsync()
    {
        // Arrange & Act
        var result = await _forwardMessagesRepository.ChatIdExistsAsync(9999);

        // Assert
        Assert.True(!result);
    }

    [Fact]
    public async Task ChannelIdExistsFalseAsync()
    {
        // Arrange & Act
        var result = await _forwardMessagesRepository.ChannelIdExistsAsync(9999);

        // Assert
        Assert.True(!result);
    }

    [Fact]
    public async Task ExistsFalseAsync()
    {
        // Arrange & Act
        var result = await _forwardMessagesRepository.ExistsAsync(new ForwardMessages(99, 99));

        // Assert
        Assert.True(!result);
    }

    [Fact]
    public async Task AddAsync()
    {
        // Arrange
        var forward = new ForwardMessages(1, 1);

        // Act
        await _forwardMessagesRepository.AddAsync(forward);
        var result = await _forwardMessagesRepository.ExistsAsync(forward);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange
        var forward = new ForwardMessages(2, 2);
        await _forwardMessagesRepository.AddAsync(forward);

        // Act
        var resultTrue = await _forwardMessagesRepository.ExistsAsync(forward);
        await _forwardMessagesRepository.RemoveAsync(forward);
        var resultFalse = await _forwardMessagesRepository.ExistsAsync(forward);

        // Assert
        Assert.True(resultTrue);
        Assert.True(!resultFalse);
    }

    [Fact]
    public async Task DeleteChannelIdAsync()
    {
        // Arrange
        var forward1 = new ForwardMessages(3, 3);
        var forward2 = new ForwardMessages(3, 33);
        var forward3 = new ForwardMessages(3, 333);

        await _forwardMessagesRepository.AddAsync(forward1);
        await _forwardMessagesRepository.AddAsync(forward2);
        await _forwardMessagesRepository.AddAsync(forward3);

        // Act
        var resultTrue1 = await _forwardMessagesRepository.ChannelIdExistsAsync(forward1.ChannelId);
        var resultTrue2 = await _forwardMessagesRepository.ChannelIdExistsAsync(forward2.ChannelId);
        var resultTrue3 = await _forwardMessagesRepository.ChannelIdExistsAsync(forward3.ChannelId);

        await _forwardMessagesRepository.RemoveChannelIdAsync(forward1.ChannelId);

        var resultFalse1 = await _forwardMessagesRepository.ChannelIdExistsAsync(forward1.ChannelId);
        var resultFalse2 = await _forwardMessagesRepository.ChannelIdExistsAsync(forward2.ChannelId);
        var resultFalse3 = await _forwardMessagesRepository.ChannelIdExistsAsync(forward3.ChannelId);

        // Assert
        Assert.True(resultTrue1);
        Assert.True(!resultFalse1);

        Assert.True(resultTrue2);
        Assert.True(!resultFalse2);

        Assert.True(resultTrue3);
        Assert.True(!resultFalse3);
    }

    [Fact]
    public async Task DeleteChatIdAsync()
    {
        // Arrange
        var forward1 = new ForwardMessages(3, 3);
        var forward2 = new ForwardMessages(33, 3);
        var forward3 = new ForwardMessages(333, 3);

        await _forwardMessagesRepository.AddAsync(forward1);
        await _forwardMessagesRepository.AddAsync(forward2);
        await _forwardMessagesRepository.AddAsync(forward3);

        // Act
        var resultTrue1 = await _forwardMessagesRepository.ChatIdExistsAsync(forward1.ChatId);
        var resultTrue2 = await _forwardMessagesRepository.ChatIdExistsAsync(forward2.ChatId);
        var resultTrue3 = await _forwardMessagesRepository.ChatIdExistsAsync(forward3.ChatId);

        await _forwardMessagesRepository.RemoveChatIdAsync(forward1.ChannelId);

        var resultFalse1 = await _forwardMessagesRepository.ChatIdExistsAsync(forward1.ChatId);
        var resultFalse2 = await _forwardMessagesRepository.ChatIdExistsAsync(forward2.ChatId);
        var resultFalse3 = await _forwardMessagesRepository.ChatIdExistsAsync(forward3.ChatId);


        // Assert
        Assert.True(resultTrue1);
        Assert.True(!resultFalse1);

        Assert.True(resultTrue2);
        Assert.True(!resultFalse2);

        Assert.True(resultTrue3);
        Assert.True(!resultFalse3);
    }

    [Fact]
    public async Task FindAllAsync()
    {
        // Arrange
        var forward1 = new ForwardMessages(4, 4);
        var forward2 = new ForwardMessages(5, 5);
        var forward3 = new ForwardMessages(6, 6);

        await _forwardMessagesRepository.AddAsync(forward1);
        await _forwardMessagesRepository.AddAsync(forward2);
        await _forwardMessagesRepository.AddAsync(forward3);

        // Act
        var result = await _forwardMessagesRepository.FindAllAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task FindChannelIdAsync()
    {
        // Arrange
        var forward1 = new ForwardMessages(7, 7);
        var forward2 = new ForwardMessages(7, 8);
        var forward3 = new ForwardMessages(7, 9);

        await _forwardMessagesRepository.AddAsync(forward1);
        await _forwardMessagesRepository.AddAsync(forward2);
        await _forwardMessagesRepository.AddAsync(forward3);

        // Act
        var result = await _forwardMessagesRepository.FindChannelIdAsync(forward1.ChannelId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task FindChatIdAsync()
    {
        // Arrange
        var forward1 = new ForwardMessages(88, 77);
        var forward2 = new ForwardMessages(99, 77);
        var forward3 = new ForwardMessages(10, 77);

        await _forwardMessagesRepository.AddAsync(forward1);
        await _forwardMessagesRepository.AddAsync(forward2);
        await _forwardMessagesRepository.AddAsync(forward3);

        // Act
        var result = await _forwardMessagesRepository.FindChatIdAsync(forward1.ChatId);

        // Assert
        Assert.NotNull(result);
    }
}
