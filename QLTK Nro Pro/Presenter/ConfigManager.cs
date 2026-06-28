using System;
using System.IO;
using System.Windows.Forms;

namespace QLTK_Nro_Pro.Presenter
{
    public static class ConfigManager
    {
        public static string LoadText(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ConfigManager] Lỗi đọc file {filePath}: {ex.Message}");
            }
            return string.Empty;
        }

        public static void SaveText(string filePath, string content)
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(filePath, content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu file cấu hình {Path.GetFileName(filePath)}: {ex.Message}", "Lỗi");
            }
        }

        public static void SaveChat(string content)
        {
            SaveText(AppConstants.ChatGlobal, content);
            SaveText(AppConstants.ChatPublic, content);
            SaveText(AppConstants.ChatInbox, content);
        }
    }
}
