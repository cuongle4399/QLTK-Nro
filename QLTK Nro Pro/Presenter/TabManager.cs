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
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int SizeW, int SizeH, bool Repaint);
        private static List<IntPtr> gameWindows = new List<IntPtr>();

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
            if (gameWindows.Count == 0)
            {
                MessageBox.Show("Mở game chưa ?????", "Thông báo");
                return;
            }

            const int spacing = 5;
            const int cascadeOffset = 90;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int baseX = 0;
            int baseY = 0;
            int cascadeStep = 0;
            bool isRightSide = false;

            for (int i = 0; i < gameWindows.Count; i++)
            {
                IntPtr hWnd = gameWindows[i];
                if (hWnd != IntPtr.Zero)
                {
                    if (GetWindowRect(hWnd, out RECT rect))
                    {
                        int width = rect.Right - rect.Left;
                        int height = rect.Bottom - rect.Top;

                        int x, y;
                        if (!isRightSide)
                        {
                            x = baseX + (cascadeStep * cascadeOffset);
                            y = baseY + (cascadeStep * cascadeOffset);
                        }
                        else
                        {
                            x = baseX - (cascadeStep * cascadeOffset);
                            y = baseY + (cascadeStep * cascadeOffset);
                        }

                        if (y + height > screenHeight)
                        {
                            isRightSide = true;
                            baseX = screenWidth - width;
                            baseY = 0;
                            cascadeStep = 0;
                            x = baseX - (cascadeStep * cascadeOffset);
                            y = baseY + (cascadeStep * cascadeOffset);
                        }

                        MoveWindow(hWnd, x, y, width, height, true);
                        cascadeStep++;
                    }
                }
            }
        }
        #endregion

        #region Auto Sort Windows
        private static void AutoSortGameWindows()
        {
            try
            {
                Form1 form1 = null;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Form1)
                    {
                        form1 = frm as Form1;
                        break;
                    }
                }

                if (form1 == null) return;

                if (form1.InvokeRequired)
                {
                    form1.Invoke((MethodInvoker)(() =>
                    {
                        int screenWidth = 1068;  // default
                        int screenHeight = 600;  // default

                        try
                        {
                            var txtXCtrl = form1.Controls.Find("txtX", true).FirstOrDefault() as TextBox;
                            var txtYCtrl = form1.Controls.Find("txtY", true).FirstOrDefault() as TextBox;

                            if (txtXCtrl != null && int.TryParse(txtXCtrl.Text, out int x))
                                screenWidth = x;
                            if (txtYCtrl != null && int.TryParse(txtYCtrl.Text, out int y))
                                screenHeight = y;
                        }
                        catch { }

                        if (screenWidth >= 450 && screenHeight >= 500)
                        {
                            Console.WriteLine($"🔄 Sắp xếp cửa sổ (2D)...");
                            sortTabGame2D();
                        }
                        else
                        {
                            Console.WriteLine($"🔄 Sắp xếp cửa sổ (Pixel)...");
                            sortTabGamePixel();
                        }

                        Console.WriteLine($"✓ Sắp xếp hoàn tất");
                    }));
                }
                else
                {
                    sortTabGame2D();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi sắp xếp cửa sổ: {ex.Message}");
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
                    Console.WriteLine($"Không thể tắt tiến trình {item.ProcessName}: {ex.Message}");
                }
            }

            ClearOpenedTabs();
        }
        #endregion

        #region Logingame
        public static bool StartingGame;
        public static string filePath = "Data\\size.ini";
        public static string ListVutItem = "Data\\ListVutIdItem.ini";
        public static string TextNpcXmap = "Data\\TextNpcXmap.ini";
        public static string NameWindownro244 = "ragonboy244";

        private static Dictionary<int, int> openedTabProcesses = new Dictionary<int, int>();
        private static readonly object _lockObj = new object();

        [DllImport("user32.dll")]
        private static extern bool SetWindowText(IntPtr hWnd, string windowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void startGame(int index, string pathGame, string nameWindowGame, bool isAutoOpen = false)
        {
            lock (_lockObj)
            {
                if (StartingGame) return;
                StartingGame = true;
            }

            try
            {
                string[] lines = File.ReadAllLines(AppConstants.PathData);
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

                    lock (_lockObj)
                    {
                        openedTabProcesses[index - 1] = proc.Id;
                    }

                    if (isAutoOpen)
                    {
                        Task.Delay(2000).ContinueWith(_ =>
                        {
                            ResyncConfigToTab(index - 1);
                            AutoSortGameWindows();
                        });
                    }
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

        private static void ResyncConfigToTab(int tabId)
        {
            try
            {
                Form1 form1 = null;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is Form1)
                    {
                        form1 = frm as Form1;
                        break;
                    }
                }

                if (form1 == null) return;

                SendActiveConfigs(tabId, form1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi resync config: {ex.Message}");
            }
        }
        #endregion

        private static void SendActiveConfigs(int tabId, Form1 form1)
        {
            try
            {
                Console.WriteLine($"📤 Resync config cho tab {tabId}...");

                FindAndSendActiveButtons(form1.Controls, form1);

                Console.WriteLine($"✅ Resync config hoàn tất");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi resync: {ex.Message}");
            }
        }

        private static void FindAndSendActiveButtons(Control.ControlCollection controls, Form1 form1)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is Button btn && btn.Text.Contains("ON"))
                {
                    string cmdToSend = GetSocketCommandForButton(btn.Name, form1, addPrefix: true);
                    if (!string.IsNullOrEmpty(cmdToSend))
                    {
                        TCPSocket.send(cmdToSend);
                        Console.WriteLine($"  ✓ Gửi: {btn.Name} -> {cmdToSend}");
                        Thread.Sleep(50);
                    }
                }

                if (ctrl.HasChildren)
                {
                    FindAndSendActiveButtons(ctrl.Controls, form1);
                }
            }
        }

        private static string GetSocketCommandForButton(string buttonName, Form1 form1, bool addPrefix = false)
        {
            try
            {
                string baseCmd = "";

                switch (buttonName)
                {
                    case "button3": baseCmd = "Boom"; break;
                    case "button4": baseCmd = "findBoss"; break;
                    case "button5": baseCmd = "teleBoss"; break;
                    case "button7": baseCmd = "acttackBoss"; break;
                    case "button8": baseCmd = "doBoss"; break;
                    case "button13": baseCmd = "autoWhis"; break;
                    case "button14": baseCmd = "findBossTrungMabu"; break;
                    case "button9": baseCmd = "farmNappa|" + form1.cbbBossNappa.SelectedIndex; break;

                    case "button6": baseCmd = "trainMob"; break;
                    case "button16": baseCmd = "goBack"; break;
                    case "button17": baseCmd = "goBackToaDo"; break;

                    case "materialButton20":
                        baseCmd = "BoMong|" + form1.TypeNV.Text.ToLower().Trim() + "|"
                            + form1.cbbTypeNVGold.SelectedIndex + "|"
                            + form1.chkNextGold.Checked + "|"
                            + form1.chkNextMob.Checked + "|"
                            + form1.chkNextHuman.Checked;
                        break;

                    case "button18": baseCmd = "deSua"; break;
                    case "button19": baseCmd = "deKOK"; break;
                    case "button20": baseCmd = "deCoDen"; break;
                    case "button21": baseCmd = "deAutoNhat"; break;
                    case "button22": baseCmd = "deGim"; break;
                    case "button23": baseCmd = "deTTNL|" + (int)form1.txtPercenHP.Value; break;
                    case "materialButton131": baseCmd = "xinDau"; break;
                    case "materialButton132": baseCmd = "ThuDau"; break;
                    case "materialButton133": baseCmd = "ChoDau"; break;

                    case "btnReduceCPU": baseCmd = "reduceCPU|" + (int)form1.txtFps.Value; break;
                    case "btnNhapCodeLive": baseCmd = "NhapCodeLive|" + form1.txtCodeLive.Text.Trim(); break;
                    case "materialButton77": baseCmd = "TagNameAutoBoss|" + form1.txtTagNameBoss.Text.Trim().ToLower(); break;
                    case "btnApDungVut": baseCmd = "listvut|" + form1.txtVut.Text; break;

                    case "btnNeSieuQuai": baseCmd = "autoNeSieuQuai"; break;
                    case "btnAkDame": baseCmd = "trainAkDame"; break;
                    case "btnAutoNhat": baseCmd = "AutoNhat"; break;
                    case "btnNeBoss": baseCmd = "autoNeBoss"; break;
                    case "btnAutoHopThe": baseCmd = "autoHopThe"; break;
                    case "btnSpamZoneIt": baseCmd = "spamZoneIt"; break;
                    case "btnAutoZoneIt": baseCmd = "autoZoneIt"; break;

                    default: return "";
                }

                if (addPrefix && !string.IsNullOrEmpty(baseCmd))
                {
                    return "ON" + baseCmd;
                }

                return baseCmd;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi lấy command cho {buttonName}: {ex.Message}");
                return "";
            }
        }

        private static bool IsProcessAlive(int processId)
        {
            try
            {
                Process proc = Process.GetProcessById(processId);
                return !proc.HasExited;
            }
            catch
            {
                return false;
            }
        }

        public static bool OpenTabsByTick()
        {
            bool openedAnyTab = false;

            var dgv = Form1.DatagridViewQLTK;
            if (dgv == null || dgv.Rows.Count == 0)
                return false;

            string pathGame = AppConstants.nro244;
            string windowName = NameWindownro244;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                bool isTicked = row.Cells["Tick"].Value is bool b && b;
                if (!isTicked)
                {
                    continue;
                }

                if (!int.TryParse(row.Cells[0].Value?.ToString(), out int index))
                    continue;

                lock (_lockObj)
                {
                    if (openedTabProcesses.ContainsKey(index))
                    {
                        int pid = openedTabProcesses[index];

                        if (IsProcessAlive(pid))
                        {
                            Console.WriteLine($"✓✓✓ Tab {index}: Process {pid} đang chạy, BỎ QUA (KHÔNG MỞ LẠI)");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine($"✗ Tab {index}: Process {pid} đã tắt, sẽ mở lại");
                            openedTabProcesses.Remove(index);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"⚠ Tab {index}: Chưa được lưu, sẽ mở lần đầu");
                    }
                }

                startGame(index + 1, pathGame, windowName, isAutoOpen: true);
                openedAnyTab = true;
                Thread.Sleep(800);
            }

            return openedAnyTab;
        }

        public static void SyncOpenedTabsFromRunningGames()
        {
            Process[] processes = Process.GetProcessesByName("Nro_244");

            lock (_lockObj)
            {
                openedTabProcesses.Clear();
            }

            foreach (var proc in processes)
            {
                try
                {
                    string title = proc.MainWindowTitle;
                    if (string.IsNullOrEmpty(title)) continue;

                    if (title.Contains("ID:"))
                    {
                        int start = title.IndexOf("ID:") + 3;
                        int end = title.IndexOf(" ", start);
                        if (end > start)
                        {
                            string idStr = title.Substring(start, end - start);
                            if (int.TryParse(idStr, out int id))
                            {
                                lock (_lockObj)
                                {
                                    openedTabProcesses[id] = proc.Id;
                                    Console.WriteLine($"✓ Sync tab {id}: Process {proc.Id} đang chạy");
                                }

                                gameWindows.Add(proc.MainWindowHandle);
                            }
                        }
                    }
                }
                catch { }
            }

            Console.WriteLine($"📊 Tổng cộng {openedTabProcesses.Count} tab đang chạy");
        }

        public static void ClearOpenedTabs()
        {
            lock (_lockObj)
            {
                openedTabProcesses.Clear();
            }
            gameWindows.Clear();
        }
    }
}