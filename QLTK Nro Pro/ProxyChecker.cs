using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace QLTK_Nro_Pro
{
    public class ProxyChecker
    {
        public static async Task<bool> CheckProxy(string proxyString, string proxyType)
        {
            try
            {
                string[] proxyParts = proxyString.Split(':');
                if (proxyParts.Length < 2) return false;

                string ip = proxyParts[0];
                if (!int.TryParse(proxyParts[1], out int port)) return false;
                string username = proxyParts.Length > 2 ? proxyParts[2] : null;
                string password = proxyParts.Length > 3 ? proxyParts[3] : null;

                switch (proxyType.ToLower())
                {
                    case "http":
                    case "https":
                        return await CheckHttpProxy(ip, port, proxyType, username, password);
                    case "socks5":
                        return await CheckSocks5Proxy(ip, port, username, password);
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static async Task<bool> CheckHttpProxy(string ip, int port, string proxyType, string username, string password)
        {
            try
            {
                using (var cts = new CancellationTokenSource(5000))
                {
                    HttpClientHandler handler = new HttpClientHandler
                    {
                        Proxy = new WebProxy($"{proxyType}://{ip}:{port}")
                    };

                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        handler.Proxy.Credentials = new NetworkCredential(username, password);
                    }

                    using (var client = new HttpClient(handler))
                    {
                        client.Timeout = TimeSpan.FromSeconds(5);
                        var requestTask = client.GetAsync("https://www.google.com", cts.Token);
                        if (await Task.WhenAny(requestTask, Task.Delay(5000, cts.Token)) != requestTask)
                            return false;
                        var response = await requestTask;
                        return response.IsSuccessStatusCode;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private static async Task<bool> CheckSocks5Proxy(string ip, int port, string username, string password)
        {
            try
            {
                using (var cts = new CancellationTokenSource(5000))
                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(ip, port);
                    if (await Task.WhenAny(connectTask, Task.Delay(2000, cts.Token)) != connectTask)
                        return false;
                    await connectTask;

                    var stream = client.GetStream();
                    stream.ReadTimeout = 3000;
                    stream.WriteTimeout = 3000;

                    byte[] initRequest = !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)
                        ? new byte[] { 0x05, 0x02, 0x00, 0x02 } 
                        : new byte[] { 0x05, 0x01, 0x00 };
                    await stream.WriteAsync(initRequest, 0, initRequest.Length, cts.Token);

                    byte[] initResponse = new byte[2];
                    var initReadTask = stream.ReadAsync(initResponse, 0, initResponse.Length, cts.Token);
                    if (await Task.WhenAny(initReadTask, Task.Delay(3000, cts.Token)) != initReadTask)
                        return false;
                    await initReadTask;
                    if (initResponse[0] != 0x05)
                        return false;

                    if (initResponse[1] == 0x02 && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        byte[] authRequest = new byte[] { 0x01, (byte)username.Length }
                            .Concat(Encoding.ASCII.GetBytes(username))
                            .Concat(new byte[] { (byte)password.Length })
                            .Concat(Encoding.ASCII.GetBytes(password))
                            .ToArray();
                        await stream.WriteAsync(authRequest, 0, authRequest.Length, cts.Token);

                        byte[] authResponse = new byte[2];
                        var authReadTask = stream.ReadAsync(authResponse, 0, authResponse.Length, cts.Token);
                        if (await Task.WhenAny(authReadTask, Task.Delay(3000, cts.Token)) != authReadTask)
                            return false;
                        await authReadTask;
                        if (authResponse[0] != 0x01 || authResponse[1] != 0x00)
                            return false;
                    }
                    else if (initResponse[1] != 0x00)
                    {
                        return false;
                    }

                    byte[] connectRequest = new byte[] { 0x05, 0x01, 0x00, 0x03, (byte)"www.google.com".Length }
                        .Concat(Encoding.ASCII.GetBytes("www.google.com"))
                        .Concat(new byte[] { 0x00, 0x50 })
                        .ToArray();
                    await stream.WriteAsync(connectRequest, 0, connectRequest.Length, cts.Token);

                    byte[] connectResponse = new byte[10];
                    var connectReadTask = stream.ReadAsync(connectResponse, 0, connectResponse.Length, cts.Token);
                    if (await Task.WhenAny(connectReadTask, Task.Delay(3000, cts.Token)) != connectReadTask)
                        return false;
                    await connectReadTask;
                    if (connectResponse[0] != 0x05 || connectResponse[1] != 0x00)
                        return false;

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}