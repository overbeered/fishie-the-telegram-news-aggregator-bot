using Fishie.Core;
using Fishie.Services.TelegramService.Commands.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fishie.Services.TelegramService.Commands.SendCommands
{
    /// <summary>
    /// Send a message to the chat where the command was called
    /// </summary>
    internal class SendCommandHandler : AsyncRequestHandler<SendCommand>, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IDisposableResource _disposableResource;

        public SendCommandHandler(IServiceScopeFactory serviceScopeFactory, IDisposableResource disposableResource)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _disposableResource = disposableResource;
        }

        public void Dispose()
        {
            _disposableResource?.Dispose();
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
