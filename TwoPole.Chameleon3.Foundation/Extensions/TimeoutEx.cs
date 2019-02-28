using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TwoPole.Chameleon3.Foundation
{
    public static class TimeoutEx
    {
        private const int DefaultTimeoutMillseconds = 1000*5;

        public static TcpClient ConnectEx(this TcpClient client, 
            IPAddress address, int port, 
            int timeoutMillseconds = DefaultTimeoutMillseconds)
        {
            if(client == null)
                throw new ArgumentNullException("client");

            if (timeoutMillseconds <= 0)
                throw new ArgumentOutOfRangeException("timeoutMillseconds");
            ManualResetEvent timeoutEvent = new ManualResetEvent(false);
            var context = new TimeoutContext<TcpClient>(client, timeoutEvent);

            client.BeginConnect(address, port, TcpClientEndConnect, context);
            
            if (timeoutEvent.WaitOne(timeoutMillseconds, false))
            {
                if (context.Success)
                {
                    return client;
                }
                else
                {
                    throw context.Error;
                }
            }
            else
            {
                client.Close();
                throw new TimeoutException(string.Format("连接 {0}:{1} 超时", address, port));
            }
        }

        public static Socket ConnectEx(this Socket client, 
            IPAddress address, int port, 
            int timeoutMillseconds = DefaultTimeoutMillseconds)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (timeoutMillseconds <= 0)
                throw new ArgumentOutOfRangeException("timeoutMillseconds");

            ManualResetEvent timeoutEvent = new ManualResetEvent(false);
            var context = new TimeoutContext<Socket>(client, timeoutEvent);
            client.BeginConnect(address, port, SocketEndConnect, context);

            if (timeoutEvent.WaitOne(timeoutMillseconds, false))
            {
                if (context.Success)
                {
                    return client;
                }
                else
                {
                    throw context.Error;
                }
            }
            else
            {
                client.Close();
                throw new TimeoutException(string.Format("连接 {0}:{1} 超时", address, port));
            }
        }

        private static void TcpClientEndConnect(IAsyncResult asyncresult)
        {
            var context = (TimeoutContext<TcpClient>)asyncresult.AsyncState;
            var client = context.Client;
            try
            {
                if (client.Client != null)
                {
                    client.EndConnect(asyncresult);
                    context.Success = true;
                }
            }
            catch (Exception ex)
            {
                context.Success = false;
                context.Error = ex;
            }
            finally
            {
                context.TimeoutEvent.Set();
            }
        }

        private static void SocketEndConnect(IAsyncResult asyncresult)
        {
            var context = (TimeoutContext<Socket>)asyncresult.AsyncState;
            try
            {
                if (context.Client != null)
                {
                    context.Client.EndConnect(asyncresult);
                    context.Success = true;
                }
            }
            catch (Exception ex)
            {
                context.Success = false;
                context.Error = ex;
            }
            finally
            {
                context.TimeoutEvent.Set();
            }
        }

        private class TimeoutContext<T>
        {
            public T Client { get; private set; }

            public ManualResetEvent TimeoutEvent { get; private set; }

            public bool Success { get;  set; }

            public Exception Error { get;  set; }

            public TimeoutContext(T client, ManualResetEvent resetEvent)
            {
                this.Client = client;
                this.TimeoutEvent = resetEvent;
            }
        }
    }
}
