using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwoPole.Chameleon3.Foundation
{
    public static class SocketExtensions
    {
        private const int DefaultTimeoutMillseconds = 1000 * 10;

        public static Task<int> ReceiveAsync(this Socket socket,
            byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            var tcs = new TaskCompletionSource<int>();
            socket.BeginReceive(buffer, offset, size, socketFlags, iar =>
            {
                try { tcs.TrySetResult(socket.EndReceive(iar)); }
                catch (Exception exc) { tcs.TrySetException(exc); }
            }, null);
            return tcs.Task;
        }

        public static Task ConnectAsync(this Socket socket, IPAddress address, int port)
        {
            return socket.ConnectAsync(new IPEndPoint(address, port));
        }

        public static Task ConnectAsync(this Socket socket, EndPoint endPoint)
        {
            return socket.ConnectAsync(endPoint, CancellationToken.None);
        }

        public static Task ConnectAsync(this Socket socket, EndPoint endPoint, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<object>();
            //token.Register(() => socket.DisconnectAsync(new SocketAsyncEventArgs()));
            try
            {
                socket.BeginConnect(endPoint, ar =>
                {
                    try
                    {
                        if (token.IsCancellationRequested)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            ((Socket)ar.AsyncState).EndConnect(ar);
                            tcs.TrySetResult(new object());
                        }
                    }
                    catch (Exception exp) 
                    {
                        tcs.TrySetException(exp);
                    }
                }, socket);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            return tcs.Task;
        }

        public static Task ConnectExAsync(this Socket socket, IPAddress address, int port,
            int timeoutMillseconds = DefaultTimeoutMillseconds)
        {
            return socket.ConnectExAsync(new IPEndPoint(address, port), timeoutMillseconds);
        }

        public static Task ConnectExAsync(this Socket socket, EndPoint endPoint,
            int timeoutMillseconds = DefaultTimeoutMillseconds)
        {
            var source = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeoutMillseconds));
            //source.Token.Register(() =>
            //{
            //    throw new TimeoutException(string.Format("连接 {0} 超时", endPoint));
            //});
            return socket.ConnectAsync(endPoint, source.Token);
        }
    }
}
