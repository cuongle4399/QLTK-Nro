using System;
using System.Text.RegularExpressions;

namespace QLTK_Nro_Pro
{
    internal class ProxyValidator
    {
        public static bool IsValidProxy(string proxy, string proxyType)
        {
            if (string.IsNullOrWhiteSpace(proxy) || string.IsNullOrEmpty(proxyType))
                return false;
            proxy = proxy.Replace(" ", "").Trim();
            string basicPattern = @"^[^:\s]+:\d{1,5}:[^:\s]+:[^:\s]+$";
            if (!Regex.IsMatch(proxy, basicPattern))
                return false;

            string[] parts = proxy.Split(':');
            string host = parts[0];
            string portStr = parts[1];
            string user = parts[2];
            string pass = parts[3];

            if (!int.TryParse(portStr, out int port) || port < 1 || port > 65535)
                return false;

            if (user.Length == 0 || pass.Length == 0)
                return false;

            return proxyType switch
            {
                "1" => IsValidHttpHost(host),
                "2" => IsValidSocks5Host(host), 
                "3" => IsValidHttpsHost(host), 
                _ => false
            };
        }

        private static bool IsValidHttpHost(string host)
        {
            string ipv4Pattern = @"^(\d{1,3}\.){3}\d{1,3}$";
            if (!Regex.IsMatch(host, ipv4Pattern))
                return false;

            string[] ipBlocks = host.Split('.');
            foreach (string block in ipBlocks)
            {
                if (!int.TryParse(block, out int num) || num < 0 || num > 255)
                    return false;
            }
            return true;
        }

        private static bool IsValidHttpsHost(string host)
        {
            if (Regex.IsMatch(host, @"^(\d{1,3}\.){3}\d{1,3}$"))
            {
                string[] ipBlocks = host.Split('.');
                foreach (string block in ipBlocks)
                {
                    if (!int.TryParse(block, out int num) || num < 0 || num > 255)
                        return false;
                }
                return true;
            }

            string hostnamePattern = @"^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}$";
            return Regex.IsMatch(host, hostnamePattern);
        }

        private static bool IsValidSocks5Host(string host)
        {
            if (Regex.IsMatch(host, @"^(\d{1,3}\.){3}\d{1,3}$"))
            {
                string[] ipBlocks = host.Split('.');
                foreach (string block in ipBlocks)
                {
                    if (!int.TryParse(block, out int num) || num < 0 || num > 255)
                        return false;
                }
                return true;
            }

            string ipv6Pattern = @"^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$";
            if (Regex.IsMatch(host, ipv6Pattern))
            {
                string[] ipv6Blocks = host.Split(':');
                foreach (string block in ipv6Blocks)
                {
                    if (!int.TryParse(block, System.Globalization.NumberStyles.HexNumber, null, out int num) || num < 0 || num > 65535)
                        return false;
                }
                return true;
            }

            string hostnamePattern = @"^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}$";
            return Regex.IsMatch(host, hostnamePattern);
        }
    }
}