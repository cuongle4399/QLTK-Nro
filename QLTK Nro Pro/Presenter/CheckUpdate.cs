using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK_Nro_Pro.Presenter
{
    internal class CheckUpdate
    {
        public static string version = "3.4.1";
        private static bool updating = false;
        private static readonly HttpClient httpClient = new HttpClient();
        public static string CheckOfUpdate = "https://www.dropbox.com/scl/fi/0iacu1ujg30nbl7suncea/checkVersionNro.txt?rlkey=7vcuj3zim83gvviimg8k26ns5&st=2g41ahjg&dl=1";

        public static async Task CheckForUpdates()
        {
            if (updating)
            {
                return;
            }
            updating = true;

            try
            {
                string versionInfo = await httpClient.GetStringAsync(CheckOfUpdate);
                if (!versionInfo.Contains(version))
                {
                    if (MessageBox.Show("Đã có phiên bản mới. Bạn có muốn cập nhật không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            string exePath = Path.Combine(Application.StartupPath, "UpdateMod.exe");

                            if (File.Exists(exePath))
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = exePath,
                                    UseShellExecute = true
                                });
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy file cập nhật (UpdateMod.exe).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi khởi chạy cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            updating = false;
        }
    }
}
