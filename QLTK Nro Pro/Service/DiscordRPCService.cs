using DiscordRPC;
using System;
using System.Windows.Forms;
namespace QLTK_Nro_Pro.Service
{
    internal static class DiscordRPCService
    {
        private static DiscordRpcClient client;

        public static void Initialize(Form form)
        {
            client = new DiscordRpcClient("1401384990952132769");
            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = "Ngọc Rồng Online",
                State = Presenter.CheckUpdate.version,
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "Ngọc Rồng Online"
                },
                Timestamps = Timestamps.Now
            });
        }

        public static void Dispose()
        {
            client?.Dispose();
            client = null;
        }
    }
}
