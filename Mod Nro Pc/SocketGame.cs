using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SocketManagerCL;

public class SocketGame
{
	private static TcpClient client;

	private static NetworkStream stream;

	private static Thread listenThread;

	private static bool SentDataInfoToServer = true;

	private static volatile bool isRunning;

	private static readonly Queue<string> messageQueue = new Queue<string>();

	private static readonly object queueLock = new object();

	private static StringBuilder sb = new StringBuilder();

	public static bool IsRunning => isRunning;

	public static bool Connect(string host = "127.0.0.1", int port = 9999)
	{
		try
		{
			client = new TcpClient();
			client.ReceiveBufferSize = 4096;
			client.SendBufferSize = 4096;
			client.Connect(host, port);
			stream = client.GetStream();
			isRunning = true;
			listenThread = new Thread(ListenFromServer);
			listenThread.IsBackground = true;
			listenThread.Start();
			SentDataInfoToServer = true;
			return true;
		}
		catch
		{
			Disconnect();
			return false;
		}
	}

	private static void ListenFromServer()
	{
		byte[] array = new byte[4096];
		try
		{
			while (isRunning)
			{
				int num = stream.Read(array, 0, array.Length);
				if (num <= 0)
				{
					Disconnect();
					break;
				}
				sb.Append(Encoding.UTF8.GetString(array, 0, num));
				string text = sb.ToString();
				int num2;
				while ((num2 = text.IndexOf('\n')) != -1)
				{
					string text2 = text.Substring(0, num2).Trim();
					if (!string.IsNullOrEmpty(text2))
					{
						lock (queueLock)
						{
							messageQueue.Enqueue(text2);
						}
					}
					text = text.Substring(num2 + 1);
				}
				sb.Length = 0;
				sb.Append(text);
			}
		}
		catch
		{
			Disconnect();
		}
	}

	public static void Send(string message)
	{
		try
		{
			if (isRunning && stream != null)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(message + "\n");
				stream.Write(bytes, 0, bytes.Length);
			}
		}
		catch
		{
			Disconnect();
		}
	}

	public static void ProcessMessages()
	{
		if (IsRunning && SentDataInfoToServer && Char.myCharz() != null)
		{
			SendCharInfo();
		}
		lock (queueLock)
		{
			while (messageQueue.Count > 0)
			{
				string message = messageQueue.Dequeue();
				HandlerSocket.XuLyDuLieu(message);
			}
		}
	}

	public static void Disconnect()
	{
		if (!isRunning)
		{
			return;
		}
		isRunning = false;
		try
		{
			if (client != null && client.Connected && stream != null)
			{
				try
				{
					string text = AutoLoginCL.idClientSocket + "|disconnect";
					byte[] bytes = Encoding.UTF8.GetBytes(text + "\n");
					stream.Write(bytes, 0, bytes.Length);
					stream.Flush();
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
		try
		{
			stream?.Close();
		}
		catch
		{
		}
		try
		{
			client?.Close();
		}
		catch
		{
		}
		listenThread = null;
		stream = null;
		client = null;
		sb.Length = 0;
		SentDataInfoToServer = false;
	}

	public static void SendCharInfo()
	{
		if (isRunning)
		{
			try
			{
				string message = AutoLoginCL.idClientSocket + "|" + Char.myCharz().cName;
				Send(message);
			}
			catch
			{
			}
			SentDataInfoToServer = false;
		}
	}
}
