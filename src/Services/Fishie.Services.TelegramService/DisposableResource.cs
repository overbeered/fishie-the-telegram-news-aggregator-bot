using Fishie.Core;
using System;

namespace Fishie.Services.TelegramService
{
    public class DisposableResource : IDisposableResource
    {
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {

            }
            disposed = true;
        }

        ~DisposableResource()
        {
            Dispose(false);
        }
    }
}