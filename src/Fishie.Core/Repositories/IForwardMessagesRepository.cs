using Fishie.Core.Models;

namespace Fishie.Core.Repositories;

/// <summary>
///  Defines ForwardMessages repository interface.
/// </summary>
public interface IForwardMessagesRepository
{
    Task AddAsync(ForwardMessages forwardMessages);

    Task DeleteAsync(ForwardMessages forwardMessages);

    Task<List<ForwardMessages?>> FindAllAsync();

    Task<List<ForwardMessages?>> FindChannelIdAsync(long channelId);

    Task<List<ForwardMessages?>> FindChatIdAsync(long chatId);

    Task DeleteChannelIdAsync(long channnelId);

    Task DeleteChatIdAsync(long chatId);

    Task<bool> ExistsAsync(ForwardMessages forwardMessages);

    Task<bool> ChatIdExistsAsync(long chatId);

    Task<bool> ChannelIdExistsAsync(long channelId);
}