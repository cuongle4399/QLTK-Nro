using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using UnityEngine;

public class Session_ME : ISession
{
	public class Sender
	{
		public List<Message> sendingMessage;

		public Sender()
		{
			sendingMessage = new List<Message>();
		}

		public void AddMessage(Message message)
		{
			sendingMessage.Add(message);
		}

		public void run()
		{
			while (connected)
			{
				try
				{
					if (getKeyComplete)
					{
						while (sendingMessage.Count > 0)
						{
							Message m = sendingMessage[0];
							doSendMessage(m);
							sendingMessage.RemoveAt(0);
						}
					}
					try
					{
						Thread.Sleep(5);
					}
					catch (Exception ex)
					{
						Cout.LogError(ex.ToString());
					}
				}
				catch (Exception)
				{
					Res.outz("error send message! ");
				}
			}
		}
	}

	private class MessageCollector
	{
		public void run()
		{
			try
			{
				while (connected)
				{
					Message message = readMessage();
					if (message == null)
					{
						break;
					}
					try
					{
						if (message.command == -27)
						{
							getKey(message);
						}
						else
						{
							onRecieveMsg(message);
						}
					}
					catch (Exception)
					{
						Cout.println("LOI NHAN  MESS THU 1");
					}
					try
					{
						Thread.Sleep(5);
					}
					catch (Exception)
					{
						Cout.println("LOI NHAN  MESS THU 2");
					}
				}
			}
			catch (Exception ex3)
			{
				Debug.Log("error read message!");
				Debug.Log(ex3.Message.ToString());
			}
			if (!connected)
			{
				return;
			}
			if (messageHandler != null)
			{
				if (currentTimeMillis() - timeConnected > 500)
				{
					messageHandler.onDisconnected(isMainSession);
				}
				else
				{
					messageHandler.onConnectionFail(isMainSession);
				}
			}
			if (sc != null)
			{
				cleanNetwork();
			}
		}

		private void getKey(Message message)
		{
			try
			{
				sbyte b = message.reader().readSByte();
				key = new sbyte[b];
				for (int i = 0; i < b; i++)
				{
					key[i] = message.reader().readSByte();
				}
				for (int j = 0; j < key.Length - 1; j++)
				{
					key[j + 1] ^= key[j];
				}
				getKeyComplete = true;
				GameMidlet.IP2 = message.reader().readUTF();
				GameMidlet.PORT2 = message.reader().readInt();
				GameMidlet.isConnect2 = ((message.reader().readByte() != 0) ? true : false);
				if (isMainSession && GameMidlet.isConnect2)
				{
					GameCanvas.connect2();
				}
			}
			catch (Exception)
			{
			}
		}

		private Message readMessage2(sbyte cmd)
		{
			int num = readKey(dis.ReadSByte()) + 128;
			int num2 = readKey(dis.ReadSByte()) + 128;
			int num3 = readKey(dis.ReadSByte()) + 128;
			int num4 = (num3 * 256 + num2) * 256 + num;
			sbyte[] array = new sbyte[num4];
			byte[] data = dis.ReadBytes(num4);
			array = ArrayCast.cast(data);
			recvByteCount += 5 + num4;
			int num5 = recvByteCount + sendByteCount;
			strRecvByteCount = num5 / 1024 + "." + num5 % 1024 / 102 + "Kb";
			if (getKeyComplete)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = readKey(array[i]);
				}
			}
			return new Message(cmd, array);
		}

		private Message readMessage()
		{
			try
			{
				sbyte b = dis.ReadSByte();
				if (getKeyComplete)
				{
					b = readKey(b);
				}
				if (b == -32 || b == -66 || b == 11 || b == -67 || b == -74 || b == -87 || b == 66 || b == 12)
				{
					return readMessage2(b);
				}
				int num;
				if (getKeyComplete)
				{
					sbyte b2 = dis.ReadSByte();
					sbyte b3 = dis.ReadSByte();
					num = ((readKey(b2) & 0xFF) << 8) | (readKey(b3) & 0xFF);
				}
				else
				{
					sbyte b4 = dis.ReadSByte();
					sbyte b5 = dis.ReadSByte();
					num = (b4 & 0xFF00) | (b5 & 0xFF);
				}
				sbyte[] array = new sbyte[num];
				byte[] data = dis.ReadBytes(num);
				array = ArrayCast.cast(data);
				recvByteCount += 5 + num;
				int num2 = recvByteCount + sendByteCount;
				strRecvByteCount = num2 / 1024 + "." + num2 % 1024 / 102 + "Kb";
				if (getKeyComplete)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = readKey(array[i]);
					}
				}
				return new Message(b, array);
			}
			catch (Exception ex)
			{
				Debug.Log(ex.StackTrace.ToString());
			}
			return null;
		}
	}

	protected static Session_ME instance = new Session_ME();

	private static Stream dataStream;

	private static BinaryReader dis;

	private static BinaryWriter dos;

	public static IMessageHandler messageHandler;

	public static bool isMainSession = true;

	private static TcpClient sc;

	public static bool connected;

	public static bool connecting;

	private static Sender sender = new Sender();

	public static Thread initThread;

	public static Thread collectorThread;

	public static Thread sendThread;

	public static int sendByteCount;

	public static int recvByteCount;

	private static bool getKeyComplete;

	public static sbyte[] key = null;

	private static sbyte curR;

	private static sbyte curW;

	private static int timeConnected;

	private long lastTimeConn;

	public static string strRecvByteCount = string.Empty;

	public static bool isCancel;

	private string host;

	private int port;

	private long timeWaitConnect;

	public static int count;

	private string proxyType = "";

	private string proxyHost = "";

	private int proxyPort = 0;

	private string proxyUsername = "";

	private string proxyPassword = "";

	public bool useProxy;

    public static string ConnectedIP = "";

    public static MyVector recieveMsg = new MyVector();

	public Session_ME()
	{
		Debug.Log("init Session_ME");
		useProxy = false;
	}

	public void clearSendingMessage()
	{
		sender.sendingMessage.Clear();
	}

	public static Session_ME gI()
	{
		if (instance == null)
		{
			instance = new Session_ME();
		}
		return instance;
	}

	public bool isConnected()
	{
		return connected && sc != null && dis != null;
	}

	public void setHandler(IMessageHandler msgHandler)
	{
		messageHandler = msgHandler;
	}

	public void connect(string host, int port)
	{
		if (connected || connecting)
		{
			Debug.Log(">>>return connect ...!" + connected + "  ::  " + connecting);
			return;
		}
		if (mSystem.currentTimeMillis() < timeWaitConnect)
		{
			Debug.LogError(">>>>chặn việc nó kết nối 2 3 lần liên tục");
			return;
		}
		timeWaitConnect = mSystem.currentTimeMillis() + 50;
		if (isMainSession)
		{
			ServerListScreen.testConnect = -1;
		}
		SetProxy();
		this.host = host;
		this.port = port;
		getKeyComplete = false;
		close();
		Debug.Log("connecting...!");
		Debug.Log("host: " + host);
		Debug.Log("port: " + port);
		initThread = new Thread(NetworkInit);
		initThread.Start();
	}

	private void NetworkInit()
	{
		isCancel = false;
		connecting = true;
		Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
		connected = true;
		try
		{
			doConnect(host, port);
            ConnectedIP = GetConnectedIP();
            messageHandler.onConnectOK(isMainSession);
		}
		catch (Exception)
		{
			if (messageHandler != null)
			{
				close();
				messageHandler.onConnectionFail(isMainSession);
			}
		}
	}

	public void SetProxy()
	{
		try
		{
			if (Environment.GetCommandLineArgs().Length > 1)
			{
				string[] array = Environment.GetCommandLineArgs()[1].Split('|');
				Debug.Log("Command line args[1]: " + Environment.GetCommandLineArgs()[1]);
				string text = array[5].Replace(" ", "").Trim();
				string text2 = array[6].Replace(" ", "").Trim();
				Debug.Log("Proxy string: " + text + ", Proxy type: " + text2);
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				bool flag9 = false;
				bool flag10 = false;
				bool flag11 = false;
				if (1 == 0)
				{
				}
				string text3 = text2 switch
				{
					"1" => "http", 
					"2" => "socks5", 
					"3" => "https", 
					_ => "http", 
				};
				if (1 == 0)
				{
				}
				string text4 = text3;
				bool flag12 = false;
				string text5 = text4;
				bool flag13 = false;
				string text6 = text5;
				bool flag14 = false;
				string text7 = text6;
				bool flag15 = false;
				string text8 = text7;
				bool flag16 = false;
				string text9 = text8;
				bool flag17 = false;
				string text10 = text9;
				bool flag18 = false;
				string text11 = text10;
				bool flag19 = false;
				string text12 = text11;
				bool flag20 = false;
				string text13 = text12;
				bool flag21 = false;
				string text14 = text13;
				bool flag22 = false;
				proxyType = text14;
				string[] array2 = text.Split(':');
				if (array2.Length == 4)
				{
					proxyHost = array2[0];
					if (int.TryParse(array2[1], out var result))
					{
						proxyPort = result;
						proxyUsername = array2[2];
						proxyPassword = array2[3];
						useProxy = true;
						Debug.Log($"Proxy configured: {proxyType}:{proxyHost}:{proxyPort}:{proxyUsername}:{proxyPassword}");
					}
					else
					{
						useProxy = false;
						proxyHost = null;
						proxyPort = 0;
						proxyUsername = null;
						proxyPassword = null;
						proxyType = null;
						Debug.Log("Invalid port format, proxy disabled.");
					}
				}
				else
				{
					useProxy = false;
					proxyHost = null;
					proxyPort = 0;
					proxyUsername = null;
					proxyPassword = null;
					proxyType = null;
					Debug.Log($"Invalid proxy format ({array2.Length} parts), expected 4, disabled.");
				}
			}
			else
			{
				useProxy = false;
				proxyHost = null;
				proxyPort = 0;
				proxyUsername = null;
				proxyPassword = null;
				proxyType = null;
				Debug.Log("No command line arguments provided, proxy disabled.");
			}
		}
		catch (Exception ex)
		{
			useProxy = false;
			proxyHost = null;
			proxyPort = 0;
			proxyUsername = null;
			proxyPassword = null;
			proxyType = null;
			Debug.LogError("Error processing proxy: " + ex.Message);
		}
	}

	public void doConnect(string host, int port)
	{
		sc = new TcpClient();
		try
		{
			if (useProxy)
			{
				if (proxyType == "http")
				{
					sc.Connect(proxyHost, proxyPort);
					dataStream = sc.GetStream();
					string text = $"CONNECT {host}:{port} HTTP/1.1\r\nHost: {host}:{port}\r\n";
					if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
					{
						string text2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(proxyUsername + ":" + proxyPassword));
						text = text + "Proxy-Authorization: Basic " + text2 + "\r\n";
					}
					text += "\r\n";
					byte[] bytes = Encoding.UTF8.GetBytes(text);
					dataStream.Write(bytes, 0, bytes.Length);
					dataStream.Flush();
					byte[] array = new byte[1024];
					int num = dataStream.Read(array, 0, array.Length);
					string text3 = Encoding.UTF8.GetString(array, 0, num);
					if (!text3.StartsWith("HTTP/") || !text3.Contains("200"))
					{
						Debug.LogError("Lỗi kết nối proxy HTTP: " + text3);
						throw new Exception("Kết nối proxy HTTP thất bại: " + text3);
					}
					Debug.Log("Đã thiết lập đường hầm proxy HTTP.");
				}
				else if (proxyType == "https")
				{
					sc.Connect(proxyHost, proxyPort);
					SslStream sslStream = new SslStream(sc.GetStream(), false, (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => true);
					sslStream.AuthenticateAsClient(proxyHost);
					dataStream = sslStream;
					string text4 = $"CONNECT {host}:{port} HTTP/1.1\r\n" + $"Host: {host}:{port}\r\n";
					if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
					{
						string text5 = Convert.ToBase64String(Encoding.UTF8.GetBytes(proxyUsername + ":" + proxyPassword));
						text4 = text4 + "Proxy-Authorization: Basic " + text5 + "\r\n";
					}
					text4 += "\r\n";
					byte[] bytes2 = Encoding.UTF8.GetBytes(text4);
					dataStream.Write(bytes2, 0, bytes2.Length);
					dataStream.Flush();
					byte[] array2 = new byte[1024];
					int num2 = dataStream.Read(array2, 0, array2.Length);
					string text6 = Encoding.UTF8.GetString(array2, 0, num2);
					if (!text6.Contains("HTTP/1.1 200"))
					{
						throw new Exception("Kết nối proxy HTTPS thất bại: " + text6);
					}
					Debug.Log("Đã thiết lập đường hầm proxy HTTPS.");
				}
				else
				{
					if (!(proxyType == "socks5"))
					{
						throw new Exception("Loại proxy không hỗ trợ: " + proxyType);
					}
					sc.Connect(proxyHost, proxyPort);
					ConnectSocks5(sc, host, port);
					dataStream = sc.GetStream();
					Debug.Log("Đã thiết lập kết nối proxy SOCKS5.");
				}
			}
			else
			{
				sc.Connect(host, port);
				dataStream = sc.GetStream();
				Debug.Log("Đã kết nối trực tiếp tới server đích.");
			}
			dis = new BinaryReader(dataStream, new UTF8Encoding());
			dos = new BinaryWriter(dataStream, new UTF8Encoding());
			sendThread = new Thread(sender.run);
			sendThread.Start();
			MessageCollector messageCollector = new MessageCollector();
			collectorThread = new Thread(messageCollector.run);
			collectorThread.Start();
			timeConnected = currentTimeMillis();
			connecting = false;
			doSendMessage(new Message(-27));
			key = null;
		}
		catch (Exception ex)
		{
			Debug.LogError("doConnect thất bại: " + ex.Message);
			throw;
		}
	}

	private void ConnectSocks5(TcpClient client, string targetHost, int targetPort)
	{
		try
		{
			NetworkStream stream = client.GetStream();
			byte[] array = new byte[4] { 5, 2, 0, 2 };
			stream.Write(array, 0, array.Length);
			stream.Flush();
			byte[] array2 = new byte[2];
			stream.Read(array2, 0, 2);
			if (array2[0] != 5)
			{
				throw new Exception("Phản hồi SOCKS5 không hợp lệ");
			}
			if (array2[1] == 2 && !string.IsNullOrEmpty(proxyUsername))
			{
				byte[] array3 = new byte[3 + proxyUsername.Length + proxyPassword.Length];
				array3[0] = 1;
				array3[1] = (byte)proxyUsername.Length;
				Array.Copy(Encoding.UTF8.GetBytes(proxyUsername), 0, array3, 2, proxyUsername.Length);
				array3[2 + proxyUsername.Length] = (byte)proxyPassword.Length;
				Array.Copy(Encoding.UTF8.GetBytes(proxyPassword), 0, array3, 3 + proxyUsername.Length, proxyPassword.Length);
				stream.Write(array3, 0, array3.Length);
				stream.Flush();
				byte[] array4 = new byte[2];
				stream.Read(array4, 0, 2);
				if (array4[1] != 0)
				{
					throw new Exception("Xác thực SOCKS5 thất bại");
				}
			}
			else if (array2[1] != 0)
			{
				throw new Exception("Proxy SOCKS5 yêu cầu phương thức xác thực không hỗ trợ");
			}
			List<byte> list = new List<byte> { 5, 1, 0 };
			if (IPAddress.TryParse(targetHost, out var _))
			{
				list.Add(1);
				byte[] addressBytes = IPAddress.Parse(targetHost).GetAddressBytes();
				list.AddRange(addressBytes);
			}
			else
			{
				list.Add(3);
				list.Add((byte)targetHost.Length);
				list.AddRange(Encoding.UTF8.GetBytes(targetHost));
			}
			list.Add((byte)(targetPort >> 8));
			list.Add((byte)(targetPort & 0xFF));
			stream.Write(list.ToArray(), 0, list.Count);
			stream.Flush();
			byte[] array5 = new byte[10];
			stream.Read(array5, 0, 10);
			if (array5[1] != 0)
			{
				throw new Exception("Kết nối SOCKS5 thất bại: " + array5[1]);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Kết nối SOCKS5 thất bại: " + ex.Message);
			throw;
		}
	}

	public void sendMessage(Message message)
	{
		count++;
		Res.outz("SEND MSG: " + message.command);
		sender.AddMessage(message);
	}

	private static void doSendMessage(Message m)
	{
		sbyte[] data = m.getData();
		try
		{
			if (getKeyComplete)
			{
				sbyte value = writeKey(m.command);
				dos.Write(value);
			}
			else
			{
				dos.Write(m.command);
			}
			if (data != null)
			{
				int num = data.Length;
				if (getKeyComplete)
				{
					int num2 = writeKey((sbyte)(num >> 8));
					dos.Write((sbyte)num2);
					int num3 = writeKey((sbyte)(num & 0xFF));
					dos.Write((sbyte)num3);
				}
				else
				{
					dos.Write((ushort)num);
				}
				if (getKeyComplete)
				{
					for (int i = 0; i < data.Length; i++)
					{
						sbyte value2 = writeKey(data[i]);
						dos.Write(value2);
					}
				}
				sendByteCount += 5 + data.Length;
			}
			else
			{
				if (getKeyComplete)
				{
					int num4 = 0;
					int num5 = writeKey((sbyte)(num4 >> 8));
					dos.Write((sbyte)num5);
					int num6 = writeKey((sbyte)(num4 & 0xFF));
					dos.Write((sbyte)num6);
				}
				else
				{
					dos.Write((ushort)0);
				}
				sendByteCount += 5;
			}
			dos.Flush();
		}
		catch (Exception ex)
		{
			Debug.Log(ex.StackTrace);
			dos.Flush();
		}
	}

	public static sbyte readKey(sbyte b)
	{
		sbyte[] array = key;
		sbyte result = (sbyte)((array[curR++] & 0xFF) ^ (b & 0xFF));
		if (curR >= key.Length)
		{
			curR %= (sbyte)key.Length;
		}
		return result;
	}

	public static sbyte writeKey(sbyte b)
	{
		sbyte[] array = key;
		sbyte result = (sbyte)((array[curW++] & 0xFF) ^ (b & 0xFF));
		if (curW >= key.Length)
		{
			curW %= (sbyte)key.Length;
		}
		return result;
	}

	public static void onRecieveMsg(Message msg)
	{
		if (Thread.CurrentThread.Name == Main.mainThreadName)
		{
			messageHandler.onMessage(msg);
		}
		else
		{
			recieveMsg.addElement(msg);
		}
	}

	public static void update()
	{
		while (recieveMsg.size() > 0)
		{
			Message message = (Message)recieveMsg.elementAt(0);
			if (Controller.isStopReadMessage)
			{
				break;
			}
			if (message == null)
			{
				recieveMsg.removeElementAt(0);
				break;
			}
			messageHandler.onMessage(message);
			recieveMsg.removeElementAt(0);
		}
	}

	public void close()
	{
		cleanNetwork();
    }

	private static void cleanNetwork()
	{
		key = null;
		curR = 0;
		curW = 0;
		Debug.LogError(">>>cleanNetwork ...!");
		try
		{
			connected = false;
			connecting = false;
			if (sc != null)
			{
				sc.Close();
				sc = null;
			}
			if (dataStream != null)
			{
				dataStream.Close();
				dataStream = null;
			}
			if (dos != null)
			{
				dos.Close();
				dos = null;
			}
			if (dis != null)
			{
				dis.Close();
				dis = null;
			}
			if (Thread.CurrentThread.Name == Main.mainThreadName)
			{
				if (sendThread != null && sendThread.IsAlive)
				{
					sendThread.Abort();
				}
				sendThread = null;
				if (initThread != null && initThread.IsAlive)
				{
					initThread.Abort();
				}
				initThread = null;
				if (collectorThread != null && collectorThread.IsAlive)
				{
					collectorThread.Abort();
				}
				collectorThread = null;
			}
			else
			{
				sendThread = null;
				initThread = null;
				collectorThread = null;
			}
			if (isMainSession)
			{
				ServerListScreen.testConnect = 0;
			}
			Controller.isGet_CLIENT_INFO = false;
			Debug.Log(">>>cleanNetwork completed successfully!");
            ConnectedIP = "";
        }
		catch (Exception ex)
		{
			Debug.LogError("Lỗi khi cleanNetwork: " + ex.Message);
            ConnectedIP = "";
        }
	}

	public static int currentTimeMillis()
	{
		return Environment.TickCount;
	}

	public static byte convertSbyteToByte(sbyte var)
	{
		if (var > 0)
		{
			return (byte)var;
		}
		return (byte)(var + 256);
	}

	public static byte[] convertSbyteToByte(sbyte[] var)
	{
		byte[] array = new byte[var.Length];
		for (int i = 0; i < var.Length; i++)
		{
			if (var[i] > 0)
			{
				array[i] = (byte)var[i];
			}
			else
			{
				array[i] = (byte)(var[i] + 256);
			}
		}
		return array;
	}

	public bool isCompareIPConnect()
	{
		return true;
	}
    public string GetConnectedIP()
    {
        try
        {
            if (sc == null || !sc.Connected)
                return "DISCONNECTED";

            if (sc.Client.RemoteEndPoint is IPEndPoint remote)
            {
                // IP mà SERVER NHÌN THẤY của tab game này
                return remote.Address.ToString();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("GetConnectedIP error: " + ex.Message);
        }
        return "UNKNOWN";
    }
}
