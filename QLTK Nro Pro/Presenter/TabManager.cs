using QLTK_Nro_Pro.Presenter.Socket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTK_Nro_Pro.Presenter
{
    internal class TabManager
    {
        #region SortTabGame
        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int SizeW, int SizeH, bool Repaint); private static List<IntPtr> gameWindows = new List<IntPtr>();
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        public static void sortTabGamePixel()
        {
            if (gameWindows.Count == 0)
            {
                MessageBox.Show("Mở game chưa ?????", "Thông báo");
                return;
            }

            const int spacing = 5;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int num = 0;
            int rowHeight = 0;
            List<(int x, int y)> usedPositions = new List<(int x, int y)>();

            for (int i = 0; i < gameWindows.Count; i++)
            {
                IntPtr hWnd = gameWindows[i];
                if (hWnd != IntPtr.Zero)
                {
                    if (GetWindowRect(hWnd, out RECT rect))
                    {
                        int width = rect.Right - rect.Left;
                        int height = rect.Bottom - rect.Top;

                        int x = (width + spacing) * num;

                        if (x + width > screenWidth)
                        {
                            num = 0;
                            rowHeight += height + spacing;
                            x = (width + spacing) * num;
                        }

                        int y = rowHeight;

                        if (y + height > screenHeight)
                        {
                            MessageBox.Show("Không đủ không gian hiển thị cho các tab game!", "Thông báo");
                            return;
                        }

                        while (usedPositions.Any(pos => pos.x == x && pos.y == y))
                        {
                            num++;
                            x = (width + spacing) * num;

                            if (x + width > screenWidth)
                            {
                                num = 0;
                                rowHeight += height + spacing;
                                x = (width + spacing) * num;
                                y = rowHeight;
                            }
                        }

                        MoveWindow(hWnd, x, y, width, height, true);
                        usedPositions.Add((x, y));
                        num++;
                    }
                }
            }
        }
        public static void sortTabGame2D()
        {
            if (gameWindows.Count == 0) { MessageBox.Show("Mở game chưa ?????", "Thông báo"); return; }

            const int spacing = 5; // Khoảng cách nhỏ giữa các cửa sổ
            const int cascadeOffset = 90; // Độ lệch cho hiệu ứng chồng dần (pixel)
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int baseX = 0; // Vị trí X bắt đầu
            int baseY = 0; // Vị trí Y bắt đầu
            int cascadeStep = 0; // Theo dõi số bước chồng dần để tính độ lệch
            bool isRightSide = false; // Theo dõi xem có đang xếp ở phía bên phải không

            for (int i = 0; i < gameWindows.Count; i++)
            {
                IntPtr hWnd = gameWindows[i];
                if (hWnd != IntPtr.Zero)
                {
                    if (GetWindowRect(hWnd, out RECT rect))
                    {
                        int width = rect.Right - rect.Left;
                        int height = rect.Bottom - rect.Top;

                        // Tính vị trí với độ lệch chồng dần
                        int x, y;
                        if (!isRightSide)
                        {
                            // Chồng từ trái sang phải (mặc định ban đầu)
                            x = baseX + (cascadeStep * cascadeOffset);
                            y = baseY + (cascadeStep * cascadeOffset);
                        }
                        else
                        {
                            // Chồng từ phải sang trái
                            x = baseX - (cascadeStep * cascadeOffset);
                            y = baseY + (cascadeStep * cascadeOffset);
                        }

                        // Kiểm tra nếu cửa sổ vượt quá giới hạn chiều cao
                        if (y + height > screenHeight)
                        {
                            // Chuyển sang chế độ chồng từ phải sang trái
                            isRightSide = true;
                            baseX = screenWidth - width; // Bắt đầu từ bên phải
                            baseY = 0; // Trở lại đầu màn hình
                            cascadeStep = 0; // Đặt lại bước chồng
                            x = baseX - (cascadeStep * cascadeOffset); // Chồng dần sang trái
                            y = baseY + (cascadeStep * cascadeOffset);
                        }

                        // Di chuyển cửa sổ đến vị trí đã tính
                        MoveWindow(hWnd, x, y, width, height, true);
                        cascadeStep++;
                    }
                }
            }

        }
        #endregion
        #region CloseGame
        public static void closeGame()
        {
            string prc = "Nro_244";
            Process[] process = Process.GetProcessesByName(prc);

            var dgv = Form1.DatagridViewQLTK;
            if (dgv != null)
            {
                if (dgv.InvokeRequired)
                {
                    dgv.Invoke((MethodInvoker)(() => ReceiveDataClient.ResetAllCells()));
                }
                else
                {
                    ReceiveDataClient.ResetAllCells();
                }
            }

            if (process.Length == 0)
            {
                MessageBox.Show("Đã tắt hết toàn bộ Tab game rồi mà :(", "Cường có điều muốn nói", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (Process item in process)
            {
                try
                {
                    item.Kill();
                    item.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tắt tiến trình {item.ProcessName}: {ex.Message}");
                }
            }
        }
        #endregion
        #region Logingame
        public static bool StartingGame;
        public static string filePath = "Data\\size.ini";
        public static string NameWindownro244 = "ragonboy244";
        private static readonly object _lockObj = new object();
        [DllImport("user32.dll")]
        private static extern bool SetWindowText(IntPtr hWnd, string windowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void startGame(int index, string pathGame, string nameWindowGame)
        {
            lock (_lockObj)
            {
                if (StartingGame) return;
                StartingGame = true;
            }

            try
            {
                string[] lines = File.ReadAllLines(LoadData.PathData);
                if (index <= 0 || index > lines.Length)
                    throw new ArgumentOutOfRangeException(nameof(index), "Index ngoài phạm vi");

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pathGame,
                        Arguments = $"\"{lines[index - 1]}\"",
                        UseShellExecute = false
                    }
                };

                if (!proc.Start())
                    throw new Exception("Không thể khởi động game");

                IntPtr hWnd = IntPtr.Zero;
                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime).TotalSeconds < 10)
                {
                    proc.Refresh();
                    if (proc.MainWindowHandle != IntPtr.Zero)
                    {
                        hWnd = proc.MainWindowHandle;
                        break;
                    }

                    hWnd = FindWindow(null, nameWindowGame);
                    if (hWnd != IntPtr.Zero)
                        break;

                    Thread.Sleep(100);
                }

                if (hWnd != IntPtr.Zero)
                {
                    SetWindowText(hWnd, $"ID: {index - 1} Cuong Le");
                    gameWindows.Add(hWnd);
                }
                else
                {
                    Console.WriteLine("⚠ Không tìm thấy cửa sổ game");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi: " + ex.Message);
            }
            finally
            {
                StartingGame = false;
            }
        }
        #endregion
    }
}
