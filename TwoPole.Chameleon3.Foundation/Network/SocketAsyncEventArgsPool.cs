using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation.Network
{
    public sealed class SocketAsyncEventArgsPool : IDisposable
    {
        private ConcurrentStack<SocketAsyncEventArgs> _pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            this._pool = new ConcurrentStack<SocketAsyncEventArgs>();
        }

        public bool TryPop(out SocketAsyncEventArgs args)
        {
            return this._pool.TryPop(out args);
        }

        public void Push(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            }

            this._pool.Push(args);
        }

        public void Dispose()
        {
            while (this._pool.Count > 0)
            {
                SocketAsyncEventArgs eventArgs;
                if (this._pool.TryPop(out eventArgs))
                {
                    eventArgs.Dispose();
                }
            }
        }
    }
}
