using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.SendCommands
{
    /// <summary>
    /// Send a message to the chat where the command was called
    /// </summary>
    internal class SendCommandHandler : AsyncRequestHandler<SendCommand>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendCommandHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Handle(SendCommand request, CancellationToken cancellationToken)
        {
            await CommandResponseHelper.ExecuteAsync(_serviceScopeFactory,
                request.Client!,
                (long)request.ChatId!,
                request.Action!);
        }
    }
}
