using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Demo_core.Lib
{
    class SoceketSever_Lib
    {
      
        #region TCP
        /// <summary>
        /// TCP客户端连接
        /// </summary>
        Socket socketClient;
        public void Socket_Creat()
        {


            try
            {
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("192.168.3.43"), 64020);
                socketClient.Connect(remoteEP);
            }

            catch (Exception e)
            {
                ;
            }


        }


        public byte[] Receive()
        {
            byte[] buffuer_receive = new byte[20];
            try
            {
                socketClient.BeginReceive(buffuer_receive, 0, buffuer_receive.Length, SocketFlags.None,
                    new AsyncCallback(ReceiveCallback), null);

            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return buffuer_receive;
        }
        public async Task<byte[]> Receive0()
        {
            byte[] buffuer_receive = new byte[20];
            try
            {
                socketClient.Receive(buffuer_receive, SocketFlags.None);

            }
            catch (Exception ex)
            {
                ;
            }
            return await Task.FromResult(buffuer_receive);
        }
   
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
               
                Socket client = (Socket)ar.AsyncState;
                //int bytesSent = client.EndReceive(ar);

            }
            catch (Exception e)
            {
                ;
            }
        }

        public void Send(String data)
        {
            byte[] byteData = Encoding.Default.GetBytes(data);


            socketClient.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), socketClient);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {

                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);

            }
            catch (Exception e)
            {
                ;
            }
        }
        #endregion
        #region  UDP
        /// <summary>
        /// udp本地端口绑定
        /// </summary>
        IPEndPoint iPEndPoint;
        public void Socket_Udp()
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse("192.168.3.132"), 6400);
            udpClient = new UdpClient(iPEndPoint);



        }

        /// <summary>
        /// udp_Send目标IP绑定
        /// </summary>
        /// <returns></returns>
        public void Socket_UdpSend()
        {
            IPEndPoint iPEndPoint0 = new IPEndPoint(IPAddress.Parse("192.168.3.33"), 64000);
            byte[] sendbuf = Encoding.Default.GetBytes("m0\r\n");

            udpClient.Send(sendbuf, sendbuf.Length, iPEndPoint0);

        }
        /// <summary>
        /// udp_Resive接收消息
        /// </summary>
        /// <returns></returns>
        UdpClient udpClient;
        public byte[] Socket_UdpRevice()
        {
            byte[] buffuer = new byte[20];
            try
            {
                IPEndPoint iPEndPoint0 = new IPEndPoint(IPAddress.Parse("192.168.3.132"), 6400);

                buffuer = udpClient.Receive(ref iPEndPoint);
            }
            catch (Exception e)
            {
                ;
            }
            return buffuer;

        }

        #endregion
    }
}
