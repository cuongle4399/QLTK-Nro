using System;
using System.Security.Cryptography;
using System.Text;

public class AutoLoginCL
{
	private static AutoLoginCL _instance;

	private const long RETRY_DELAY = 20000L;

	private const long CONNECT_COOLDOWN = 1000L;

	private const long CONNECTION_TIMEOUT = 30000L;

	private const long SERVER_SWITCH_DELAY = 1500L;

	private const long LOGIN_WAIT_DELAY = 1000L;

	private const long LOGIN_RETRY_WAIT_DELAY = 2000L;

	private const long LOGIN_TIMEOUT = 30000L;

	private const int MAX_RETRY_ATTEMPTS = 5;

	private static readonly TimeSpan MAINTENANCE_START = new TimeSpan(3, 29, 0);

	private static readonly TimeSpan MAINTENANCE_END = new TimeSpan(3, 50, 0);

	public static bool IsEnabled;

	public static int steps;

	public static string idClientSocket;

	public static string Account;

	public static int server;

	public static string Password;

	public static int LasterLogin;

	private static bool hasShownTimeRestrictionMessage;

	private static int retryCount;

	private static long targetTime;

	private static long lastConnectAttempt;

	private static bool waitingForConnection;

	private static bool hasInitialized;

	private static bool credentialsSaved;

	private static bool shouldIgnoreNextDisconnect;

	private static bool hasDisconnectedForMaintenance;

	private static byte[] _md5KeyCache;

	private static string _lastDisplayedMessage = "";

	private static long _lastMessageUpdateTime;

	public static AutoLoginCL getInstance()
	{
		return _instance ?? (_instance = new AutoLoginCL());
	}

	public static void OnLoginSuccess()
	{
		if (retryCount > 0 || waitingForConnection || credentialsSaved)
		{
			ResetRetryState();
		}
	}

	private static void OnLoginFail()
	{
		retryCount++;
		if (retryCount >= 5)
		{
			IsEnabled = false;
			Reset();
			return;
		}
		CleanupConnection();
		targetTime = mSystem.currentTimeMillis() + 20000;
		steps = 6;
		hasShownTimeRestrictionMessage = false;
		credentialsSaved = false;
	}

	private static void ResetRetryState()
	{
		retryCount = 0;
		targetTime = 0L;
		waitingForConnection = false;
		lastConnectAttempt = 0L;
		credentialsSaved = false;
	}

	private static bool IsLoginSuccess()
	{
		try
		{
			Char myChar = Char.myCharz();
			return !(GameCanvas.currentScreen is ServerListScreen) && !(GameCanvas.currentScreen is LoginScr) && Session_ME.gI().isConnected() && myChar != null && !string.IsNullOrEmpty(myChar.cName);
		}
		catch
		{
			return false;
		}
	}

	private static bool HasValidCredentials()
	{
		if (string.IsNullOrEmpty(Account) || Account.Trim().Length == 0)
		{
			return false;
		}
		if (string.IsNullOrEmpty(Password) || Password.Trim().Length == 0)
		{
			return false;
		}
		return true;
	}

	private static bool CanAttemptConnect()
	{
		long now = mSystem.currentTimeMillis();
		if (now - lastConnectAttempt < 1000)
		{
			return false;
		}
		lastConnectAttempt = now;
		return true;
	}

	private static bool IsInMaintenanceTime()
	{
		TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
		return timeOfDay >= MAINTENANCE_START && timeOfDay < MAINTENANCE_END;
	}

	private static void CleanupConnection()
	{
		try
		{
			Session_ME session = Session_ME.gI();
			if (session != null)
			{
				bool wasConnected = session.isConnected() || Session_ME.connecting;
				if (wasConnected)
				{
					shouldIgnoreNextDisconnect = true;
					session.close();
					Session_ME.connected = false;
					Session_ME.connecting = false;
				}
				waitingForConnection = false;
				if (!wasConnected)
				{
					shouldIgnoreNextDisconnect = false;
				}
			}
		}
		catch
		{
		}
	}

	private static void SaveCredentialsToRMS(bool force = false)
	{
		if (!HasValidCredentials() || (credentialsSaved && !force))
		{
			return;
		}
		try
		{
			Rms.saveRMSString("acc", Account);
			Rms.saveRMSString("pass", Password);
			credentialsSaved = true;
		}
		catch
		{
			credentialsSaved = false;
		}
	}

	public static void Update()
	{
		if (!IsEnabled || !ServerListScreen.bigOk)
		{
			return;
		}
		if (IsInMaintenanceTime())
		{
			if (!hasDisconnectedForMaintenance)
			{
				if (Session_ME.gI().isConnected() || Session_ME.connecting)
				{
					GameCanvas.gI().onDisconnected();
					shouldIgnoreNextDisconnect = true;
				}
				hasDisconnectedForMaintenance = true;
				steps = 0;
				GameScr.info1?.addInfo("Auto Login: Đang bảo trì (3h29-3h50), đợi tiếp tục...");
			}
			return;
		}
		if (hasDisconnectedForMaintenance)
		{
			hasDisconnectedForMaintenance = false;
			steps = 1;
			ResetRetryState();
			GameScr.info1?.addInfo("Auto Login: Bảo trì kết thúc, tiếp tục login");
		}
		if (steps > 0 && steps < 5 && IsLoginSuccess())
		{
			OnLoginSuccess();
			return;
		}
		HandleStep(steps);
		ShowLoginStatus();
	}

	private static void HandleStep(int currentStep)
	{
		switch (currentStep)
		{
		case 0:
			HandleStep0();
			break;
		case 1:
			HandleStep1();
			break;
		case 2:
			HandleStep2();
			break;
		case 3:
			HandleStep3();
			break;
		case 4:
			HandleStep4();
			break;
		case 6:
			HandleStep6();
			break;
		case 8:
			HandleStep8();
			break;
		case 5:
		case 7:
			break;
		}
	}

	private static void HandleStep0()
	{
		if (HasValidCredentials())
		{
			Char myChar = Char.myCharz();
			bool hasCharacter = myChar != null && !string.IsNullOrEmpty(myChar.cName);
			if ((hasInitialized || hasCharacter) && (GameCanvas.currentScreen is LoginScr || GameCanvas.currentScreen is ServerListScreen))
			{
				steps = 1;
				ResetRetryState();
			}
		}
	}

	private static void HandleStep1()
	{
		if (!ServerListScreen.loadScreen)
		{
			return;
		}
		if (!HasValidCredentials())
		{
			IsEnabled = false;
			steps = 0;
			return;
		}
		int serverIndex = server - 1;
		if (!IsValidServerIndex(serverIndex))
		{
			IsEnabled = false;
			steps = 0;
			return;
		}
		bool isAlreadyConnected = Session_ME.gI().isConnected() && ServerListScreen.ipSelect == serverIndex;
		if (ServerListScreen.ipSelect != serverIndex)
		{
			SwitchServer(serverIndex);
			return;
		}
		if (!credentialsSaved)
		{
			SaveCredentialsToRMS();
		}
		if (isAlreadyConnected)
		{
			targetTime = mSystem.currentTimeMillis() + 1000;
			steps = 3;
		}
		else if (CanAttemptConnect())
		{
			ConnectToServer();
		}
	}

	private static void HandleStep2()
	{
		if (Session_ME.gI().isConnected())
		{
			long delay = ((retryCount == 0) ? 1000 : 2000);
			targetTime = mSystem.currentTimeMillis() + delay;
			waitingForConnection = false;
			steps = 3;
		}
		else if (!Session_ME.connecting && waitingForConnection)
		{
			if (mSystem.currentTimeMillis() - lastConnectAttempt > 30000)
			{
				OnLoginFail();
				waitingForConnection = false;
			}
		}
		else if (!Session_ME.connecting && ServerListScreen.loadScreen)
		{
			steps = 1;
		}
	}

	private static void HandleStep3()
	{
		long timeRemaining = targetTime - mSystem.currentTimeMillis();
		LasterLogin = (int)(timeRemaining / 1000);
		if (!Session_ME.gI().isConnected())
		{
			OnLoginFail();
		}
		else if (timeRemaining <= 0)
		{
			try
			{
				GameCanvas.serverScreen.perform(3, null);
				GameCanvas.gameTick = 0;
				targetTime = mSystem.currentTimeMillis() + 30000;
				steps = 4;
			}
			catch
			{
				OnLoginFail();
			}
		}
	}

	private static void HandleStep4()
	{
		long timeRemaining = targetTime - mSystem.currentTimeMillis();
		LasterLogin = (int)(timeRemaining / 1000);
		if (IsLoginSuccess())
		{
			OnLoginSuccess();
		}
		else if (timeRemaining <= 0 || !Session_ME.gI().isConnected())
		{
			OnLoginFail();
		}
	}

	private static void HandleStep6()
	{
		long timeRemaining = targetTime - mSystem.currentTimeMillis();
		LasterLogin = (int)(timeRemaining / 1000);
		if (timeRemaining <= 0)
		{
			steps = 1;
		}
	}

	private static void HandleStep8()
	{
		if (targetTime - mSystem.currentTimeMillis() <= 0 && ServerListScreen.loadScreen && CanAttemptConnect())
		{
			hasInitialized = true;
			bool isServerSelected = Session_ME.gI().isConnected() && ServerListScreen.ipSelect == server - 1;
			if (retryCount > 0 && !isServerSelected)
			{
				CleanupConnection();
			}
			GameCanvas.serverScreen.selectServer();
			waitingForConnection = true;
			steps = 2;
		}
	}

	private static bool IsValidServerIndex(int index)
	{
		return index >= 0 && ServerListScreen.nameServer != null && index < ServerListScreen.nameServer.Length;
	}

	private static void SwitchServer(int selectedServerIndex)
	{
		if (hasInitialized && retryCount > 0)
		{
			CleanupConnection();
		}
		if (!credentialsSaved)
		{
			SaveCredentialsToRMS();
		}
		Rms.saveRMSInt("svselect", selectedServerIndex);
		ServerListScreen.ipSelect = selectedServerIndex;
		GameCanvas.serverScreen.selectServer();
		targetTime = mSystem.currentTimeMillis() + 1500;
		steps = 8;
	}

	private static void ConnectToServer()
	{
		hasInitialized = true;
		GameCanvas.serverScreen.selectServer();
		waitingForConnection = true;
		steps = 2;
	}

	public static void InitLoginData()
	{
		try
		{
			string[] args = Environment.GetCommandLineArgs()[1].Split('|');
			idClientSocket = args[0];
			Account = args[1]?.Trim() ?? string.Empty;
			server = int.Parse(args[2]);
			Password = DecryptString(args[3], "ud");
			if (!HasValidCredentials())
			{
				throw new Exception("Invalid credentials");
			}
			IsEnabled = true;
			steps = 1;
			hasInitialized = false;
			ResetRetryState();
		}
		catch
		{
			Account = "";
			Password = "";
			IsEnabled = false;
			hasInitialized = false;
			steps = 0;
			credentialsSaved = false;
		}
	}

	public static string DecryptString(string str, string key)
	{
		try
		{
			byte[] encryptedData = Convert.FromBase64String(str);
			byte[] keyHash = _md5KeyCache ?? (_md5KeyCache = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key)));
			using TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
			{
				Key = keyHash,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			};
			byte[] decrypted = provider.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
			return Encoding.UTF8.GetString(decrypted);
		}
		catch
		{
			return string.Empty;
		}
	}

	public static void Toggle()
	{
		IsEnabled = !IsEnabled;
		if (!IsEnabled)
		{
			CleanupConnection();
			steps = 0;
		}
		else
		{
			steps = 1;
		}
		credentialsSaved = false;
		retryCount = 0;
		hasDisconnectedForMaintenance = false;
		GameScr.info1?.addInfo("Auto Login: " + (IsEnabled ? "Bật" : "Tắt"));
	}

	public static void Reset()
	{
		CleanupConnection();
		steps = 0;
		retryCount = 0;
		targetTime = 0L;
		waitingForConnection = false;
		lastConnectAttempt = 0L;
		hasInitialized = false;
		credentialsSaved = false;
		hasDisconnectedForMaintenance = false;
	}

	public static void OnGameScreenChanged()
	{
		if (steps > 0 && steps < 5 && IsLoginSuccess())
		{
			OnLoginSuccess();
		}
	}

	public static void OnDisconnected()
	{
		if (shouldIgnoreNextDisconnect)
		{
			shouldIgnoreNextDisconnect = false;
		}
		else if (!IsEnabled || !HasValidCredentials())
		{
		}
		else
		{
			OnLoginFail();
		}
	}

	private static void ShowLoginStatus()
	{
		string message = string.Empty;
		switch (steps)
		{
		case 2:
			if (waitingForConnection)
			{
				message = "Đang kết nối...\nTài khoản: " + Account + "\nServer: " + server;
			}
			break;
		case 3:
			if (LasterLogin > 0)
			{
				message = "Đợi " + LasterLogin + "s trước khi đăng nhập\nTài khoản: " + Account + "\nServer: " + server;
			}
			break;
		case 4:
			if (LasterLogin > 0)
			{
				message = "Đang đăng nhập... " + LasterLogin + "s\nTài khoản: " + Account + "\nServer: " + server;
			}
			break;
		case 6:
			if (LasterLogin > 0)
			{
				message = "Thử lại sau " + LasterLogin + "s (Lần " + retryCount + ")\nTài khoản: " + Account + "\nServer: " + server;
			}
			break;
		case 8:
			message = "Đang đổi server...\nTài khoản: " + Account + "\nServer: " + server;
			break;
		}
		if (!string.IsNullOrEmpty(message) && !message.Equals(_lastDisplayedMessage))
		{
			_lastDisplayedMessage = message;
			_lastMessageUpdateTime = mSystem.currentTimeMillis();
			GameCanvas.endDlg();
			GameCanvas.startOKDlg(message);
		}
	}
}
