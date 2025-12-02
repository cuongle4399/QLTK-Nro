using QLTK_Nro_Pro.Service;
using System;
using System.Threading;
using System.Windows.Forms;

namespace QLTK_Nro_Pro
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.SetCompatibleTextRenderingDefault(false);

            bool isNewInstance;
            using (Mutex mutex = new Mutex(true, "QLTK_Nro_Pro_OnlyOneInstance", out isNewInstance))
            {
                if (isNewInstance)
                {
                    Form1 form = new Form1();

                    DiscordRPCService.Initialize(form);

                    Application.Run(form);

                    DiscordRPCService.Dispose();
                }
                else
                {
                    MessageBox.Show(
                        "Phần mềm đã được mở rồi!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }
    }
}
