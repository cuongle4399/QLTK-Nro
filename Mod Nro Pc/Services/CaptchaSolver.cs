using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ModCak.Services;

public class CustomWebClient : WebClient
{
    public int Timeout { get; set; } = 30000;
    protected override WebRequest GetWebRequest(Uri address)
    {
        WebRequest request = base.GetWebRequest(address);
        request.Timeout = Timeout;
        return request;
    }
}

public class CaptchaSolver
{
    public static bool isSolvingCapcha = false;
    public static bool captchaSolved = false;
    public static long lastCaptchaSolvedTime = 0;
    public static long lastInputTime = 0; public static long lastAPIRetryTime = 0;
    public static string statusCapcha = "";
    public static int countCaptchaSolved = 0;

    private const long WAIT_CAPTCHA_DISAPPEAR = 5000; private const int API_TIMEOUT = 30000; private static string apiKey = "";
    private static string apiServer = "";

    public static void Initialize()
    {
        try
        {
            apiKey = File.ReadAllText(Path.Combine("Data", "keyAPI.ini"))
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            apiServer = File.ReadAllText(Path.Combine("Data", "serverAPI.ini"))
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();

            LogCapchaError($"INFO: Initialized - Key length: {apiKey.Length}, Server valid: {!string.IsNullOrEmpty(apiServer)}");
        }
        catch (Exception ex)
        {
            LogCapchaError($"ERR: Initialize failed - {ex.Message}");
        }
    }

    public static void Update()
    {
        if (captchaSolved)
        {
            CheckCaptchaDisappeared();
        }

        if (ShouldSolveCaptcha() && !isSolvingCapcha && !captchaSolved)
        {
            if (GameCanvas.gameTick % 170 == 0)
            {
                new Thread(SolveCaptcha).Start();
            }
        }
    }

    private static bool ShouldSolveCaptcha()
    {
        return (!MobCapcha.isAttack || !MobCapcha.explode)
            && GameScr.gI().mobCapcha != null;
    }

    public static void CheckCaptchaDisappeared()
    {
        long now = mSystem.currentTimeMillis();

        if ((GameScr.gI().mobCapcha == null || (MobCapcha.isAttack && MobCapcha.explode))
    && (now - lastCaptchaSolvedTime) > WAIT_CAPTCHA_DISAPPEAR)
        {
            ResetCaptchaState();
            LogCapchaError("INFO: Captcha đã biến mất, reset flag");
        }
    }

    public static void SolveCaptcha()
    {
        isSolvingCapcha = true;
        statusCapcha = "Đang giải Captcha...";
        LogCapchaError("=== BẮT ĐẦU GIẢI CAPTCHA ===");
        Thread.Sleep(1000);

        try
        {
            if (!ValidateConfiguration())
            {
                return;
            }

            if (!ValidateCaptchaImage())
            {
                return;
            }

            string imageBase64 = EncodeCaptchaImage();
            if (string.IsNullOrEmpty(imageBase64))
            {
                return;
            }

            string response = SendToAPI(imageBase64);
            if (string.IsNullOrEmpty(response))
            {
                HandleAPIError("Response trống");
                return;
            }

            ProcessAPIResponse(response);
        }
        catch (WebException ex)
        {
            HandleWebException(ex);
        }
        catch (Exception ex)
        {
            statusCapcha = "Lỗi: " + ex.Message;
            LogCapchaError($"ERR: Exception = {ex.Message} | StackTrace: {ex.StackTrace}");
        }
        finally
        {
            isSolvingCapcha = false;
        }
    }

    private static bool ValidateConfiguration()
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            statusCapcha = "Không tìm thấy API Key!";
            LogCapchaError("ERR: API Key rỗng");
            return false;
        }

        if (string.IsNullOrEmpty(apiServer) || !apiServer.Contains("token="))
        {
            statusCapcha = "Không tìm thấy API Server hoặc sai định dạng!";
            LogCapchaError("ERR: API Server không hợp lệ");
            return false;
        }

        LogCapchaError("INFO: Configuration valid");
        return true;
    }

    private static bool ValidateCaptchaImage()
    {
        if (GameScr.imgCapcha == null || GameScr.imgCapcha.texture == null)
        {
            statusCapcha = "Không có ảnh Captcha!";
            LogCapchaError("ERR: Không có ảnh Captcha");
            return false;
        }

        LogCapchaError("INFO: Captcha image valid");
        return true;
    }

    private static string EncodeCaptchaImage()
    {
        try
        {
            statusCapcha = "Đang mã hóa ảnh Captcha...";
            LogCapchaError("INFO: Bắt đầu mã hóa ảnh");

            string imageBase64 = Convert.ToBase64String(
                GameScr.imgCapcha.texture.EncodeToPNG()
            );

            LogCapchaError($"INFO: Ảnh mã hóa xong, size: {imageBase64.Length}");
            return imageBase64;
        }
        catch (Exception ex)
        {
            statusCapcha = "Lỗi mã hóa ảnh: " + ex.Message;
            LogCapchaError($"ERR: Encode failed - {ex.Message}");
            return null;
        }
    }

    private static string SendToAPI(string imageBase64)
    {
        try
        {
            string address = apiServer + apiKey;
            LogCapchaError($"INFO: API URL = {address}");

            statusCapcha = "Đang gửi yêu cầu giải Captcha...";
            LogCapchaError("INFO: Gửi request tới API");

            using (CustomWebClient webClient = new CustomWebClient())
            {
                webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                webClient.Headers.Add("Accept", "application/json");
                webClient.Encoding = Encoding.UTF8;
                webClient.Timeout = API_TIMEOUT;

                NameValueCollection data = new NameValueCollection
                {
                    ["image"] = imageBase64
                };

                byte[] responseBytes = webClient.UploadValues(address, "POST", data);
                string response = Encoding.UTF8.GetString(responseBytes);

                LogCapchaError($"INFO: API Response = {response}");
                return response;
            }
        }
        catch (Exception ex)
        {
            LogCapchaError($"ERR: SendToAPI failed - {ex.Message}");
            return null;
        }
    }

    private static void ProcessAPIResponse(string response)
    {
        Match matchCaptcha = Regex.Match(response, "\"captcha\"\\s*:\\s*\"(\\d+)\"");
        Match matchStatus = Regex.Match(response, "\"status\"\\s*:\\s*(\\d+)");

        if (matchCaptcha.Success && matchStatus.Success && matchStatus.Groups[1].Value == "0")
        {
            string captcha = matchCaptcha.Groups[1].Value;
            LogCapchaError($"INFO: Nhận được Captcha = {captcha}");

            if (ValidateCaptchaFormat(captcha))
            {
                InputCaptcha(captcha);
            }
            else
            {
                statusCapcha = "Captcha không hợp lệ: " + captcha;
                LogCapchaError($"ERR: Captcha không hợp lệ (length={captcha.Length}): {captcha}");
            }
        }
        else
        {
            HandleAPIError(response);
        }
    }

    private static bool ValidateCaptchaFormat(string captcha)
    {
        return captcha.Length >= 4 && captcha.Length <= 7;
    }

    private static void InputCaptcha(string captcha)
    {
        statusCapcha = "Nhập Captcha: " + captcha;
        LogCapchaError($"INFO: Bắt đầu nhập Captcha: {captcha}");
        Thread.Sleep(500);

        foreach (char c in captcha)
        {
            if (Service.gI() == null)
            {
                statusCapcha = "Lỗi: Service không sẵn sàng!";
                LogCapchaError("ERR: Service.gI() == null");
                return;
            }

            Service.gI().mobCapcha(c);
            LogCapchaError($"INFO: Nhập ký tự '{c}'");
            Thread.Sleep(Res.random(200, 400));
        }

        Thread.Sleep(500);
        if (Service.gI() != null)
        {
            Service.gI().mobCapcha((char)13); LogCapchaError("INFO: Gửi Enter key");
        }

        lastInputTime = mSystem.currentTimeMillis();

        MarkCaptchaAsSolved();
    }

    private static void MarkCaptchaAsSolved()
    {
        countCaptchaSolved++;
        statusCapcha = "Nhập xong, chờ captcha mất...";
        captchaSolved = true;
        lastCaptchaSolvedTime = mSystem.currentTimeMillis();
        lastInputTime = mSystem.currentTimeMillis(); LogCapchaError("SUCCESS: Nhập captcha xong, chờ captcha biến mất...");
    }

    private static void HandleAPIError(string response)
    {
        statusCapcha = "API lỗi: Thử lại sau 3 giây...";
        LogCapchaError($"ERR: API lỗi, retry sau 3s: {response}");

        Thread.Sleep(3000);
        lastAPIRetryTime = mSystem.currentTimeMillis(); new Thread(SolveCaptcha).Start();
    }

    private static void HandleWebException(WebException ex)
    {
        try
        {
            string err = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            statusCapcha = "Lỗi mạng/API: " + err;
            LogCapchaError($"ERR: WebException = {err}");
        }
        catch
        {
            statusCapcha = "Lỗi mạng/API: " + ex.Message;
            LogCapchaError($"ERR: WebException = {ex.Message}");
        }
    }

    private static void ResetCaptchaState()
    {
        statusCapcha = "Captcha đã reset!";
        captchaSolved = false;
        lastAPIRetryTime = 0; LogCapchaError("INFO: Reset captcha state");
    }

    private static void LogCapchaError(string message)
    {
        try
        {
            string logPath = Path.Combine("Data", "logErrorCapcha.txt");
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(logPath, logMessage + Environment.NewLine);
        }
        catch { }
    }

    public static void Reset()
    {
        ResetCaptchaState();
        isSolvingCapcha = false;
        LogCapchaError("INFO: Manual reset captcha");
    }

    public static string GetStatus()
    {
        return statusCapcha;
    }

    public static int GetSolvedCount()
    {
        return countCaptchaSolved;
    }

    public static bool IsSolving()
    {
        return isSolvingCapcha;
    }
}