using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLTK_Nro_Pro.Presenter.Socket
{
    internal class TCPSocket
    {
        private static TcpListener server;
        private static List<TcpClient> Clients = new List<TcpClient>();
        private static int ClientCount = 0;
        private static bool isRunning = false;

        public static void startServer()
        {
            Task.Run(() =>
            {
                server = new TcpListener(IPAddress.Any, 9999);
                server.Start();
                isRunning = true;
                Console.WriteLine("[SERVER] Đã khởi động trên cổng 9999");

                while (isRunning)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        lock (Clients) Clients.Add(client);
                        Interlocked.Increment(ref ClientCount);

                        Console.WriteLine("[SERVER] Client kết nối, tổng: " + ClientCount);

                        Thread clientThread = new Thread(() => HandleClient(client));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
            });
        }

        private static void HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine("[SERVER] Nhận: " + line);

                        ReceiveDataClient.Process(line, client);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SERVER] Lỗi client: " + ex.Message);
            }
            finally
            {
                lock (Clients) Clients.Remove(client);
                Interlocked.Decrement(ref ClientCount);
                try { client.Close(); } catch { }
                Console.WriteLine("[SERVER] Client ngắt, còn lại: " + ClientCount);
            }
        }

        public static void stopServer()
        {
            isRunning = false;
            try
            {
                lock (Clients)
                {
                    foreach (var client in Clients)
                    {
                        try { client.Close(); } catch { }
                    }
                    Clients.Clear();
                }
                server?.Stop();
                Console.WriteLine("[SERVER] Đã dừng");
            }
            catch { }
        }

        public static void send(string message)
        {
            if (!message.EndsWith("\n"))
                message += "\n";

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
                        stream.Flush();
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
