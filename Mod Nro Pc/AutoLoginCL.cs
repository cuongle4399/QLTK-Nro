using System;
using System.Security.Cryptography;
using System.Text;

public class AutoLoginCL
{
    private static AutoLoginCL _instance;

    private const long LOGIN_TIMEOUT_BUFFER = 10000L;
    public static bool IsEnabled;
    public static string idClientSocket;
    public static string Account;
    public static int server;
    public static string Password;
    public static int LasterLogin;

    private static long loginStartTime;
    private static bool hasPerformedLogin;
    public static bool dataLoaded;
    public static bool isLoadingData;
    private static byte[] _md5KeyCache;
    private static string _lastDisplayedMessage = "";
    private static long Wait = 0;
    public static bool isFirstLogin = true;

    public static AutoLoginCL getInstance()
        => _instance ?? (_instance = new AutoLoginCL());


    public static void Update()
    {
        long now = mSystem.currentTimeMillis();

        if (now < Wait)
            return;

        Wait = now + 1000; if (!IsEnabled || !HasValidCredentials() || !ServerListScreen.bigOk)
            return;
        if (Session_ME.gI().isConnected() && !IsLoginSuccess())
        {
            if (!hasPerformedLogin)
            {
                int serverIndex = server - 1;
                if (IsValidServerIndex(serverIndex) && ServerListScreen.ipSelect != serverIndex)
                {
                    SwitchServer(serverIndex);
                    return;
                }
                GameCanvas.serverScreen.perform(7, null);
                SaveCredentialsToRMS();
                GameCanvas.serverScreen.switchToMe();
                GameCanvas.serverScreen.perform(3, null);
                hasPerformedLogin = true;
                loginStartTime = mSystem.currentTimeMillis();
            }
            else if (hasPerformedLogin && loginStartTime > 0)
            {
                CheckLoginTimeout();
            }

        }
        else
        {
            if (IsLoginSuccess() && isFirstLogin)
            {
                isFirstLogin = false;
            }
            ShowLoginStatus();
            hasPerformedLogin = false;
            loginStartTime = 0;
        }
    }


    private static void CheckLoginTimeout()
    {
        try
        {
            if (LoginScr.timeLogin > 0)
            {
                long elapsed = mSystem.currentTimeMillis() - LoginScr.currTimeLogin;
                long timeoutLimit = LoginScr.timeLogin + LOGIN_TIMEOUT_BUFFER;

                if (elapsed > timeoutLimit)
                {
                    OnLoginFail();
                }
            }
        }
        catch
        {
        }
    }


    private static void OnLoginFail()
    {
        hasPerformedLogin = false;
        loginStartTime = 0;
    }

    public static void Reset()
    {
        hasPerformedLogin = false;
        loginStartTime = 0;
        dataLoaded = false;
    }

    public static void Toggle()
    {
        IsEnabled = !IsEnabled;

        if (!IsEnabled)
        {
            hasPerformedLogin = false;
            loginStartTime = 0;
        }

        GameScr.info1?.addInfo($"Auto Login: {(IsEnabled ? "Bật" : "Tắt")}");
    }


    private static bool HasValidCredentials()
        => !string.IsNullOrEmpty(idClientSocket) &&
           !string.IsNullOrEmpty(Account) &&
           !string.IsNullOrEmpty(Password) &&
           server > 0;

    private static bool IsLoginSuccess()
    {
        try
        {
            Char myChar = Char.myCharz();
            return !(GameCanvas.currentScreen is ServerListScreen) &&
                   !(GameCanvas.currentScreen is LoginScr) &&
                   myChar != null &&
                   !string.IsNullOrEmpty(myChar.cName);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidServerIndex(int index)
        => ServerListScreen.nameServer != null &&
           index >= 0 &&
           index < ServerListScreen.nameServer.Length;

    private static void SaveCredentialsToRMS()
    {
        try
        {
            Rms.saveRMSString("acc", Account);
            Rms.saveRMSString("pass", Password);
            Rms.saveRMSInt("svselect", server - 1);
        }
        catch { }
    }

    private static void SwitchServer(int index)
    {
        try
        {
            Rms.saveRMSInt("svselect", index);
            ServerListScreen.ipSelect = index;
            GameCanvas.serverScreen.selectServer();
        }
        catch { }
    }


    public static void InitLoginData()
    {
        try
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length < 2)
                return;

            string[] data = args[1].Split('|');
            if (data.Length < 4)
                return;

            idClientSocket = data[0];
            Account = data[1];
            server = int.Parse(data[2]);
            Password = DecryptString(data[3], "ud");

            if (HasValidCredentials())
            {
                IsEnabled = true;
            }
            else
            {
                IsEnabled = false;
            }
        }
        catch
        {
            IsEnabled = false;
        }
    }

    private static void ShowLoginStatus()
    {
        if (IsLoginSuccess())
        {
            if (!string.IsNullOrEmpty(_lastDisplayedMessage))
            {
                _lastDisplayedMessage = "";
                GameCanvas.endDlg();
            }
            return;
        }

        long now = mSystem.currentTimeMillis();

        long remainMs = ServerListScreen.count_reConnect - now;
        int remainSec = (int)System.Math.Ceiling(remainMs / 1000.0);
        if (remainSec < 0) remainSec = 0;

        string message =
            "Tài khoản : " + Account + "\n" +
            "Server : " + server + "\n" +
            "Kết nối lại sau: " + remainSec + "s";

        if (!message.Equals(_lastDisplayedMessage))
        {
            _lastDisplayedMessage = message;

            GameCanvas.endDlg();
            GameCanvas.startOKDlg(message);
        }
    }



    public static string DecryptString(string str, string key)
    {
        try
        {
            byte[] encrypted = Convert.FromBase64String(str);
            byte[] keyHash = _md5KeyCache ??= MD5.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(key));

            using var des = new TripleDESCryptoServiceProvider
            {
                Key = keyHash,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            return Encoding.UTF8.GetString(
                des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length)
            );
        }
        catch
        {
            return string.Empty;
        }
    }
}