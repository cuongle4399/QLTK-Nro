using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace QLTK_Nro_Pro.Presenter.Socket
{
    /// <summary>
    /// Lưu thông tin gốc của một row
    /// </summary>
    internal class RowOriginalData
    {
        public object TaiKhoan { get; set; }
        public object GhiChu { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }
        public Color SelectionBackColor { get; set; }
        public Color SelectionForeColor { get; set; }
    }

    internal class ReceiveDataClient
    {
        public static IAccountStatusView StatusView { get; set; }

        /// <summary>
        /// Xử lý dữ liệu client gửi lên
        /// </summary>
        public static void Process(string message, TcpClient client)
        {
            try
            {
                var parts = message.Split('|');
                if (parts.Length < 2)
                {
                    LogRaw(message);
                    return;
                }

                string idClient = parts[0];
                string command = parts[1];

                if (StatusView == null) return;

                switch (command.ToLower())
                {
                    case "disconnect":
                        StatusView.SetAccountOffline(idClient);
                        System.Diagnostics.Debug.WriteLine($"[SERVER] Client {idClient} đã offline.");
                        break;

                    default:
                        StatusView.SetAccountOnline(idClient, command);
                        System.Diagnostics.Debug.WriteLine($"[SERVER] Client {idClient} tên nhân vật: {command}");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SERVER] Lỗi xử lý dữ liệu: {ex}");
            }
        }

        /// <summary>
        /// Reset toàn bộ DataGridView về trạng thái gốc
        /// </summary>
        public static void ResetAllCells()
        {
            StatusView?.ResetAllCells();
        }

        /// <summary>
        /// Log dữ liệu raw
        /// </summary>
        private static void LogRaw(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[SERVER] Dữ liệu raw: {message}");
        }
    }
}
