
using System;
using Android.Net.Wifi;
using Java.Net;
using System.Threading;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    public class UdpHelper
    {
        public bool IsThreadDisable = false;//指示监听线程是否终止
        private WifiManager.MulticastLock mylock;
        InetAddress mInetAddress;
        public UdpHelper()
        {

        }
        public UdpHelper(WifiManager manager)
        {
            mylock = manager.CreateMulticastLock("UDPwifi");
        }
        public void StartListen()
        {
            // UDP服务器监听的端口
            int port = 4000;
            // 接收的字节大小，客户端发送的数据不能超过这个大小
            byte[] message = new byte[512];
            DatagramSocket datagramSocket = new DatagramSocket(port);
            datagramSocket.Broadcast = true;
            DatagramPacket datagramPacket = new DatagramPacket(message, message.Length);
            try
            {
                while (!IsThreadDisable)
                {
                    datagramSocket.Receive(datagramPacket);
                    string strMsg = new Java.Lang.String(datagramPacket.GetData()).Trim();
                    string[] msg = strMsg.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                }
            }
            catch (System.Exception e)
            {

            }
        }
        public void Run()
        {
            Thread thread = new Thread((new ThreadStart(StartListen)));
            thread.Start();
        }
        public void Dispose()
        {

        }
    }
}