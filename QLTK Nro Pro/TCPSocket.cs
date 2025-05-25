using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLTK_Nro_Pro
{
    internal class TCPSocket
    {
        private static TcpListener server;
        private static List<TcpClient> Clients = new List<TcpClient>();
        private static int ClientCount = 0;
        public static void startServer()
        {
            Task.Run(() =>
            {
                server = new TcpListener(IPAddress.Any, 9999);
                server.Start();
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    lock (Clients) Clients.Add(client);
                    Interlocked.Increment(ref ClientCount);

                    // Bắt đầu xử lý client riêng
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            });
        }
        private static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes == 0) break; // client đã đóng kết nối

                }
            }
            catch
            {
                
            }
            finally
            {
                lock (Clients) Clients.Remove(client);
                Interlocked.Decrement(ref ClientCount);
                client.Close();
            }
        }

        public static void stopServer()
        {
            server.Stop();
        }
        public static void send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            lock (Clients)
            {
                for (int i = Clients.Count - 1; i >= 0; i--) 
                {
                    TcpClient client = Clients[i];
                    if (!client.Connected)
                    {
                        Clients.RemoveAt(i);
                        Interlocked.Decrement(ref ClientCount);
                        continue;
                    }

                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
                    }
                    catch
                    {
                        Clients.RemoveAt(i);
                        Interlocked.Decrement(ref ClientCount);
                    }
                }
            }
        }

        public static int GetCountClientConnect()
        {
            return ClientCount;
        }
            
    }
}
