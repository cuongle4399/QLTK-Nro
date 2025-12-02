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
        private static readonly Color OnlineBackNormal = Color.LightGreen;
        private static readonly Color OnlineForeNormal = Color.Black;
        private static readonly Color OnlineBackSelected = Color.LimeGreen;
        private static readonly Color OnlineForeSelected = Color.Black;

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

                var dgv = Form1.DatagridViewQLTK;
                if (dgv == null) return;

                // đảm bảo thao tác trên UI thread
                if (dgv.InvokeRequired)
                {
                    dgv.Invoke((MethodInvoker)(() => Process(message, client)));
                    return;
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells["ID"].Value?.ToString() != idClient)
                        continue;

                    EnsureRowTag(row);

                    switch (command.ToLower())
                    {
                        case "disconnect":
                            SetOfflineRow(row);
                            System.Diagnostics.Debug.WriteLine($"[SERVER] Client {idClient} đã offline.");
                            break;

                        default:
                            SetOnlineRow(row, command);
                            System.Diagnostics.Debug.WriteLine($"[SERVER] Client {idClient} tên nhân vật: {command}");
                            break;
                    }

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
            var dgv = Form1.DatagridViewQLTK;
            if (dgv == null) return;

            if (dgv.InvokeRequired)
            {
                dgv.Invoke((MethodInvoker)ResetAllCells);
                return;
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                EnsureRowTag(row);
                SetOfflineRow(row);
            }
        }

        /// <summary>
        /// Đảm bảo row.Tag chứa dữ liệu gốc
        /// </summary>
        private static void EnsureRowTag(DataGridViewRow row)
        {
            if (row.Tag != null) return;

            row.Tag = new RowOriginalData
            {
                TaiKhoan = row.Cells["TaiKhoan"].Value,
                GhiChu = row.Cells["GhiChu"].Value,
                BackColor = row.DefaultCellStyle.BackColor,
                ForeColor = row.DefaultCellStyle.ForeColor,
                SelectionBackColor = row.DefaultCellStyle.SelectionBackColor,
                SelectionForeColor = row.DefaultCellStyle.SelectionForeColor
            };
        }

        /// <summary>
        /// Đặt row về trạng thái offline, phục hồi dữ liệu gốc
        /// </summary>
        private static void SetOfflineRow(DataGridViewRow row)
        {
            if (row.Tag is RowOriginalData original)
            {
                row.Cells["TaiKhoan"].Value = original.TaiKhoan;
                row.Cells["GhiChu"].Value = original.GhiChu;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = original.BackColor;
                    cell.Style.ForeColor = original.ForeColor;
                    cell.Style.SelectionBackColor = original.SelectionBackColor;
                    cell.Style.SelectionForeColor = original.SelectionForeColor;
                }
            }
        }

        /// <summary>
        /// Đặt row về trạng thái online
        /// </summary>
        private static void SetOnlineRow(DataGridViewRow row, string command)
        {
            row.Cells["TaiKhoan"].Value = command;
            row.Cells["GhiChu"].Value = "Đang Online";

            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style.BackColor = OnlineBackNormal;
                cell.Style.ForeColor = OnlineForeNormal;
                cell.Style.SelectionBackColor = OnlineBackSelected;
                cell.Style.SelectionForeColor = OnlineForeSelected;
            }
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
