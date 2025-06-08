using MaterialSkin;
using MaterialSkin.Controls;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static QLTK_Nro_Pro.ProxyChecker;

namespace QLTK_Nro_Pro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
        }

        public static string nro244 = Path.Combine(Application.StartupPath, "Nro_244.exe");
        private int indexSTT = 0;
        private int delaySocket = 150;
        private void Form1_Load(object sender, EventArgs e)
        {
            txtProxy.UseSystemPasswordChar = true;
            pass.UseSystemPasswordChar = true;
            gbSize.Enabled = false;
            gbChat.Enabled = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            var screen = Screen.PrimaryScreen.WorkingArea;
            int x = screen.Right - this.Width;
            int y = screen.Bottom - this.Height;
            this.Location = new Point(x, y);
            if (!Directory.Exists(Path.Combine(Path.GetTempPath(), "cuongle4399", "mod 244")))
            {
                Directory.CreateDirectory((Path.Combine(Path.GetTempPath(), "cuongle4399", "mod 244")));
                if (!File.Exists(info.string_1))
                {
                    File.Create(info.string_1).Close();
                }
            }
            CheckForUpdates();

            try
            {
                string[] a = File.ReadAllText(info.filePath).Split('|');
                txtX.Text = a[0];
                txtY.Text = a[1];
            }
            catch { }
            try
            {
                txtChat.Text = File.ReadAllText(info.ChatPublic);
            }
            catch
            {

            }

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            docFile();


        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.ExitThread();
            Environment.Exit(0);
        }
        private void docFile()
        {
            if (!File.Exists(info.string_1))
            {
                File.Create(info.string_1).Close();
            }
            try
            {
                indexSTT = dataGridView1.RowCount;
                string[] a = File.ReadAllLines(info.string_1);
                for (int i = 0; i < a.Length; i++)
                {
                    string[] b = a[i].Split('|');

                    dataGridView1.Rows.Add(new object[]{
                    b[0],
                   b[1],
                   b[2],
                    b[3],
                    b[4],
                    b[5],
                    b[6],
                });
                    indexSTT++;

                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 40;
                }
            }
            catch
            {
                if (MessageBox.Show("Dữ liệu của phần mềm bị xung đột.Bạn có muốn reset dữ liệu không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Delete(info.string_1);
                }
                else
                {
                    Application.ExitThread();
                    Environment.Exit(0);
                }
            }
        }
        public static string smethod_2(string str, string key)
        {
            byte[] array = Convert.FromBase64String(str);
            byte[] key2 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] bytes = new TripleDESCryptoServiceProvider
            {
                Key = key2,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            }.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
            return Encoding.UTF8.GetString(bytes);
        }
        public void ghifile(DataGridView gridView)
        {
            if (gridView.RowCount > 0)
            {
                String Text = "";
                for (int i = 0; i < gridView.Rows.Count; i++)
                {
                    Text += i + "|" +
                            gridView.Rows[i].Cells[1].Value.ToString() + "|" +
                            gridView.Rows[i].Cells[2].Value.ToString() + "|" +
                            gridView.Rows[i].Cells[3].Value.ToString() + "|" +
                            gridView.Rows[i].Cells[4].Value.ToString() + "|" +
                            gridView.Rows[i].Cells[5].Value.ToString() + "|" +
                            gridView.Rows[i].Cells[6].Value.ToString();
                    if (i != gridView.Rows.Count - 1)
                    {
                        Text += '\n';
                    }
                }
                File.WriteAllText(info.string_1, Text);
            }
        }

        public string Reserver(int x)
        {
            if (x == 13)
            {
                return "Võ đài liên vũ trụ[13]";
            }
            if (x == 14)
            {
                return "Universe1 (14)";
            }
            if (x == 15)
            {
                return "Naga [15]";
            }
            if (x == 16)
            {
                return "Super 1 [16]";
            }
            if (x == 17)
            {
                return "Super 2 [17]";
            }
            if (x == 18)
            {
                return "13 [18]";
            }
            if (x == 19)
            {
                return "VIP 2 [19]";
            }
            if (x == 20)
            {
                return "14 [20]";
            }
            return x.ToString();
        }
        public int server(string x)
        {
            if (x.Equals("Võ đài liên vũ trụ [13]"))
            {
                return 13;
            }
            if (x.Equals("Universe1 (14)"))
            {
                return 14;
            }
            if (x.Equals("Naga [15]"))
            {
                return 15;
            }
            if (x.Equals("Super 1 [16]"))
            {
                return 16;
            }
            if (x.Equals("Super 2 [17]"))
            {
                return 17;
            }
            if (x.Equals("13 [18]"))
            {
                return 18;
            }
            if (x.Equals("VIP 2 [19]"))
            {
                return 19;
            }
            if (x.Equals("14 [20]"))
            {
                return 20;
            }
            return int.Parse(x);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            string prc = "Nro_244";
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(prc);
            if (process.Length == 0)
            {
                MessageBox.Show("Đã tắt hết toàn bộ Tab game rồi mà :(", "Cường có điều muốn nói", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            foreach (Process item in process)
            {
                item.Kill();

            }
        }
        private bool updating = false;
        private static readonly HttpClient httpClient = new HttpClient();

        private async Task CheckForUpdates()
        {
            if (updating)
            {
                return;
            }
            updating = true;

            try
            {
                string versionInfo = await httpClient.GetStringAsync(info.CheckOfUpdate);
                if (!versionInfo.Contains("2.1"))
                {
                    if (MessageBox.Show("Đã có phiên bản mới. Bạn có muốn cập nhật không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://drive.google.com/drive/u/0/folders/1-FQLm1txnoQcAtvkl27g7Osed58yRSgL",
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            updating = false;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            string prc = "Nro_244";
            System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName(prc);
            if (process.Length == 0)
            {
                MessageBox.Show("Đã tắt hết toàn bộ Tab game rồi mà :(", "Cường có điều muốn nói", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            foreach (Process item in process)
            {
                item.Kill();

            }
        }
        public static string smethod_1(string string_5, string string_6)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string_5);
            byte[] key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string_6));
            byte[] array = new TripleDESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            }.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(array, 0, array.Length);
        }
        private static List<IntPtr> gameWindows = new List<IntPtr>();
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        public void SortWindows()
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
        public void SortWindows2()
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
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern bool SetWindowText(IntPtr hWnd, string windowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int SizeW, int SizeH, bool Repaint);
        public static void smethod_3(int int_0, string PathGame, string nameWindowGame)
        {
            if (!info.bool_0)
            {
                info.bool_0 = true;
                Process.Start(fileName: PathGame, arguments: $"\"{File.ReadAllLines(info.string_1)[int_0 - 1]}\"");

                Thread.Sleep(700);
                IntPtr hWnd = FindWindow(null, nameWindowGame);

                if (hWnd != IntPtr.Zero)
                {
                    SetWindowText(hWnd, "ID: " + (int_0 - 1).ToString() + " Cuong Le");
                    gameWindows.Add(hWnd);
                }
                info.bool_0 = false;
            }
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtX.Text) >= 450 && int.Parse(txtY.Text) >= 500)
                {
                    SortWindows2();
                }
                else
                {
                    SortWindows();
                }
            }
            catch
            {
                SortWindows2();
            }


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_user.Text) || string.IsNullOrEmpty(txt_server.Text) || string.IsNullOrEmpty(pass.Text))
            {
                MessageBox.Show("Nhập đầy đủ vào rồi thêm cục cưng ", "Thông báo", MessageBoxButtons.OK);
                txt_user.Focus();
                return;
            }
            string proxyType = "3";
            if (rdoHTTP.Checked)
                proxyType = "1";
            else if (rdoSOCK5S.Checked)
                proxyType = "2";
            dataGridView1.Rows.Add(new object[]
            {
                indexSTT,
                    txt_user.Text,
                    server(txt_server.Text),
                    smethod_1(pass.Text, "ud"),
                    txt_note.Text,
                    txtProxy.Text,
                    proxyType
                });
            dataGridView1.Rows[dataGridView1.RowCount - 1].Height = 40;

            indexSTT++;
            ghifile(dataGridView1);
            txt_user.Clear();
        }



        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            if (check.Checked)
            {

                pass.UseSystemPasswordChar = false;
                pass.Focus();
            }
            else
            {

                pass.UseSystemPasswordChar = true;
                pass.Focus();
            }
        }


        private void btnupdateSize_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtX.Text) < 0 || int.Parse(txtX.Text) > 4000 || int.Parse(txtY.Text) < 0 || int.Parse(txtY.Text) > 4000)
                {
                    MessageBox.Show("Kích thước không hợp lệ phải lớn hơn 0 và nhỏ hơn 4000 ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(txtX.Text) || string.IsNullOrEmpty(txtY.Text))
                {
                    MessageBox.Show("Nhập đầy đủ kích thước theo chiều ngang và chiều dọc", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                File.WriteAllText(info.filePath, txtX.Text + '|' + txtY.Text + '|' + '0');
                MessageBox.Show("Đã cập nhập kích thước game thành công", "Thông báo");
            }
            catch
            {
                MessageBox.Show("Lỗi", "Thông báo");

            }
        }

        private void btnUpdateChat_Click(object sender, EventArgs e)
        {

            File.WriteAllText(info.ChatGlobal, txtChat.Text);
            File.WriteAllText(info.ChatPublic, txtChat.Text);
            File.WriteAllText(info.ChatInbox, txtChat.Text);
            MessageBox.Show("Đã cập nhập nội dung chat thành công", "Thông báo");
        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_user.Text) || string.IsNullOrEmpty(txt_server.Text) || string.IsNullOrEmpty(pass.Text))
            {
                MessageBox.Show("Nhập đầy đủ vào rồi thêm cục cưng ", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value = txt_user.Text;
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value = smethod_1(pass.Text, "ud");
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value = server(txt_server.Text);
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value = txt_note.Text;
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[5].Value = txtProxy.Text;
            string typeProxy = "1";
            if (rdoSOCK5S.Checked)
            {
                typeProxy = "2";
            }
            else if (rdoHTTPS.Checked)
            {
                typeProxy = "3";
            }
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6].Value = typeProxy;
            ghifile(dataGridView1);
            MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null && dataGridView1.RowCount > 1)
            {
                int index = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows.RemoveAt(index);
                ghifile(dataGridView1);
                dataGridView1.Rows.Clear();
            }
            else if (dataGridView1.RowCount == 1)
            {
                dataGridView1.Rows.Clear();
                File.WriteAllText(info.string_1, string.Empty);
            }

            docFile();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string pathGame1 = nro244;
            string nameWindowGame1 = info.NameWindownro244;
            if (dataGridView1.CurrentRow.Index >= 0)
            {
                try
                {
                    int index = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    smethod_3(index + 1, pathGame1, nameWindowGame1);
                }
                catch
                {
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            txt_user.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_server.Text = Reserver(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()));
            pass.Text = smethod_2(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), "ud");
            txt_note.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtProxy.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            string value = dataGridView1.Rows[e.RowIndex].Cells[6].Value?.ToString();

            if (value == "1")
                rdoHTTP.Checked = true;
            else if (value == "2")
                rdoSOCK5S.Checked = true;
            else
                rdoHTTPS.Checked = true;
            txt_server.Refresh();
        }

        private void materialButton16_Click(object sender, EventArgs e)
        {
        }
        private void nextMap(int x)
        {
            File.WriteAllText("Data/LoadMap.ini", "T|" + x.ToString());
        }

        private void materialButton1_Click_1(object sender, EventArgs e)
        {
            nextMap(44);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            nextMap(14);
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            nextMap(15);
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            nextMap(16);
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            nextMap(17);
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            nextMap(18);
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {
            nextMap(20);
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            nextMap(19);
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            nextMap(35);
        }

        private void materialButton12_Click(object sender, EventArgs e)
        {
            nextMap(36);
        }

        private void materialButton11_Click(object sender, EventArgs e)
        {
            nextMap(37);
        }

        private void materialButton10_Click(object sender, EventArgs e)
        {
            nextMap(38);
        }

        private void materialButton15_Click(object sender, EventArgs e)
        {
            nextMap(52);
        }

        private void materialButton14_Click(object sender, EventArgs e)
        {
            nextMap(26);
        }

        private void materialButton18_Click(object sender, EventArgs e)
        {
            nextMap(113);
        }

        private void materialButton13_Click(object sender, EventArgs e)
        {
            nextMap(129);
        }

        private void materialButton37_Click(object sender, EventArgs e)
        {
            nextMap(127);
        }

        private void materialButton36_Click(object sender, EventArgs e)
        {
            nextMap(42);
        }

        private void materialButton35_Click(object sender, EventArgs e)
        {
            nextMap(0);
        }

        private void materialButton34_Click(object sender, EventArgs e)
        {
            nextMap(1);
        }

        private void materialButton33_Click(object sender, EventArgs e)
        {
            nextMap(2);
        }

        private void materialButton32_Click(object sender, EventArgs e)
        {
            nextMap(3);
        }

        private void materialButton31_Click(object sender, EventArgs e)
        {
            nextMap(4);
        }

        private void materialButton30_Click(object sender, EventArgs e)
        {
            nextMap(5);
        }

        private void materialButton29_Click(object sender, EventArgs e)
        {
            nextMap(6);
        }

        private void materialButton28_Click(object sender, EventArgs e)
        {
            nextMap(27);
        }

        private void materialButton27_Click(object sender, EventArgs e)
        {
            nextMap(28);
        }

        private void materialButton26_Click(object sender, EventArgs e)
        {
            nextMap(29);
        }

        private void materialButton25_Click(object sender, EventArgs e)
        {
            nextMap(30);
        }

        private void materialButton24_Click(object sender, EventArgs e)
        {
            nextMap(47);
        }

        private void materialButton23_Click(object sender, EventArgs e)
        {
            nextMap(24);
        }

        private void materialButton22_Click(object sender, EventArgs e)
        {
            nextMap(46);
        }

        private void materialButton21_Click(object sender, EventArgs e)
        {
            nextMap(45);
        }

        private void materialButton54_Click(object sender, EventArgs e)
        {
            nextMap(43);
        }

        private void materialButton53_Click(object sender, EventArgs e)
        {
            nextMap(7);
        }

        private void materialButton52_Click(object sender, EventArgs e)
        {
            nextMap(8);
        }

        private void materialButton51_Click(object sender, EventArgs e)
        {
            nextMap(9);
        }

        private void materialButton50_Click(object sender, EventArgs e)
        {
            nextMap(12);
        }

        private void materialButton43_Click(object sender, EventArgs e)
        {
            nextMap(11);
        }

        private void materialButton49_Click(object sender, EventArgs e)
        {
            nextMap(13);
        }

        private void materialButton42_Click(object sender, EventArgs e)
        {
            nextMap(10);
        }

        private void materialButton47_Click(object sender, EventArgs e)
        {
            nextMap(31);
        }

        private void materialButton48_Click(object sender, EventArgs e)
        {
            nextMap(32);
        }

        private void materialButton46_Click(object sender, EventArgs e)
        {
            nextMap(33);
        }

        private void materialButton45_Click(object sender, EventArgs e)
        {
            nextMap(34);
        }

        private void materialButton44_Click(object sender, EventArgs e)
        {
            nextMap(25);
        }

        private void materialButton72_Click(object sender, EventArgs e)
        {
            nextMap(68);
        }

        private void materialButton71_Click(object sender, EventArgs e)
        {
            nextMap(69);
        }

        private void materialButton70_Click(object sender, EventArgs e)
        {
            nextMap(70);
        }

        private void materialButton69_Click(object sender, EventArgs e)
        {
            nextMap(71);
        }

        private void materialButton68_Click(object sender, EventArgs e)
        {
            nextMap(72);
        }

        private void materialButton67_Click(object sender, EventArgs e)
        {
            nextMap(64);
        }

        private void materialButton66_Click(object sender, EventArgs e)
        {
            nextMap(65);
        }

        private void materialButton65_Click(object sender, EventArgs e)
        {
            nextMap(63);
        }

        private void materialButton64_Click(object sender, EventArgs e)
        {
            nextMap(66);
        }

        private void materialButton63_Click(object sender, EventArgs e)
        {
            nextMap(67);
        }

        private void materialButton62_Click(object sender, EventArgs e)
        {
            nextMap(73);
        }

        private void materialButton61_Click(object sender, EventArgs e)
        {
            nextMap(74);
        }

        private void materialButton60_Click(object sender, EventArgs e)
        {
            nextMap(75);
        }

        private void materialButton59_Click(object sender, EventArgs e)
        {
            nextMap(76);
        }

        private void materialButton58_Click(object sender, EventArgs e)
        {
            nextMap(77);
        }

        private void materialButton57_Click(object sender, EventArgs e)
        {
            nextMap(81);
        }

        private void materialButton56_Click(object sender, EventArgs e)
        {
            nextMap(82);
        }

        private void materialButton55_Click(object sender, EventArgs e)
        {
            nextMap(83);
        }

        private void materialButton19_Click(object sender, EventArgs e)
        {
            nextMap(79);
        }

        private void materialButton17_Click(object sender, EventArgs e)
        {
            nextMap(80);
        }

        private void materialButton126_Click(object sender, EventArgs e)
        {
            nextMap(131);
        }

        private void materialButton125_Click(object sender, EventArgs e)
        {
            nextMap(132);
        }

        private void materialButton124_Click(object sender, EventArgs e)
        {
            nextMap(133);
        }

        private void materialButton90_Click(object sender, EventArgs e)
        {
            nextMap(102);
        }

        private void materialButton89_Click(object sender, EventArgs e)
        {
            nextMap(92);
        }

        private void materialButton88_Click(object sender, EventArgs e)
        {
            nextMap(93);
        }

        private void materialButton87_Click(object sender, EventArgs e)
        {
            nextMap(94);
        }

        private void materialButton86_Click(object sender, EventArgs e)
        {
            nextMap(96);
        }

        private void materialButton85_Click(object sender, EventArgs e)
        {
            nextMap(97);
        }

        private void materialButton84_Click(object sender, EventArgs e)
        {
            nextMap(98);
        }

        private void materialButton83_Click(object sender, EventArgs e)
        {
            nextMap(99);
        }

        private void materialButton82_Click(object sender, EventArgs e)
        {
            nextMap(100);
        }

        private void materialButton81_Click(object sender, EventArgs e)
        {
            nextMap(103);
        }

        private void materialButton108_Click(object sender, EventArgs e)
        {
            nextMap(109);
        }

        private void materialButton107_Click(object sender, EventArgs e)
        {
            nextMap(108);
        }

        private void materialButton106_Click(object sender, EventArgs e)
        {
            nextMap(107);
        }

        private void materialButton105_Click(object sender, EventArgs e)
        {
            nextMap(110);
        }

        private void materialButton104_Click(object sender, EventArgs e)
        {
            nextMap(106);
        }

        private void materialButton103_Click(object sender, EventArgs e)
        {
            nextMap(105);
        }

        private void materialButton123_Click(object sender, EventArgs e)
        {
            nextMap(53);
        }

        private void materialButton122_Click(object sender, EventArgs e)
        {
            nextMap(58);
        }

        private void materialButton121_Click(object sender, EventArgs e)
        {
            nextMap(59);
        }

        private void materialButton120_Click(object sender, EventArgs e)
        {
            nextMap(60);
        }

        private void materialButton119_Click(object sender, EventArgs e)
        {
            nextMap(61);
        }

        private void materialButton118_Click(object sender, EventArgs e)
        {
            nextMap(62);
        }

        private void materialButton117_Click(object sender, EventArgs e)
        {
            nextMap(55);
        }

        private void materialButton116_Click(object sender, EventArgs e)
        {
            nextMap(56);
        }

        private void materialButton115_Click(object sender, EventArgs e)
        {
            nextMap(54);
        }

        private void materialButton114_Click(object sender, EventArgs e)
        {
            nextMap(57);
        }

        private void materialButton113_Click(object sender, EventArgs e)
        {
            nextMap(84);
        }

        private void materialButton16_Click_1(object sender, EventArgs e)
        {
            nextMap(-99);
        }

        private void materialButton20_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Data/LoadMap.ini", "F|-1");
            MessageBox.Show("Đã Khôi phục NextMap khắc phục lỗi", "Thông báo");
        }

        private void materialButton38_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://www.facebook.com/profile.php?id=100071743014602",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void materialButton38_Click_1(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://cuongle4399.github.io/web-mod-nro/#",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void materialButton39_Click(object sender, EventArgs e)
        {
            File.WriteAllText(info.filePath, 1050.ToString() + '|' + 591.ToString() + '|' + '0');
            gbSize.Enabled = true;
            txtX.Text = "1100";
            txtY.Text = "619";
            gbSize.Enabled = false;
            MessageBox.Show("Đã cập nhập kích thước game thành công", "Thông báo");
        }

        private void materialButton40_Click(object sender, EventArgs e)
        {
            File.WriteAllText(info.filePath, 350.ToString() + '|' + 400.ToString() + '|' + '0');
            gbSize.Enabled = true;
            txtX.Text = "350";
            txtY.Text = "400";
            gbSize.Enabled = false;
            MessageBox.Show("Đã cập nhập kích thước game thành công", "Thông báo");
        }

        private void switchSize_CheckedChanged(object sender, EventArgs e)
        {
            bool switch1 = switchSize.Checked;
            if (switch1)
            {
                gbSize.Enabled = true;
            }
            else
            {
                gbSize.Enabled = false;
            }
        }

        private void materialButton41_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                if (int.TryParse(txtidBegin.Value.ToString(), out int indexBegin) && int.TryParse(txtidEnd.Value.ToString(), out int indexEnd))
                {
                    if (indexBegin < 0 || indexBegin >= dataGridView1.RowCount || indexEnd < 0 || indexEnd >= dataGridView1.RowCount)
                    {
                        MessageBox.Show("ID bắt đầu và ID kết thúc phải tồn tại trong danh sách và phải lớn hơn 0.", "Thông báo");
                        return;
                    }

                    for (int i = indexBegin; i <= indexEnd; i++)
                    {
                        string pathGame1 = nro244;
                        string nameWindowGame1 = info.NameWindownro244;
                        try
                        {

                            smethod_3(i + 1, pathGame1, nameWindowGame1);
                            Thread.Sleep(1300);
                        }
                        catch
                        {
                        }

                    }

                }
            });

        }
        public void switchSocket()
        {
            bool switch1 = checkOnSocket.Checked;
            if (switch1)
            {
                gbSkill.Enabled = true;
                gbZone.Enabled = true;
                gbItem.Enabled = true;
                gbChatGame.Enabled = true;
                tabThapCam.Enabled = true;
                gbNpc.Enabled = true;
                gbMap.Enabled = true;
                gbChat.Enabled = true;
            }
            else
            {
                gbSkill.Enabled = false;
                gbZone.Enabled = false;
                gbItem.Enabled = false;
                gbChatGame.Enabled = false;
                tabThapCam.Enabled = false;
                gbNpc.Enabled = false;
                gbMap.Enabled = false;
                gbChat.Enabled = false;
            }
        }
        private void materialSwitch1_CheckedChanged_1(object sender, EventArgs e)
        {
            switchSocket();
            if (checkOnSocket.Checked)
            {
                TCPSocket.startServer();
                Task.Run(() =>
                {
                    while (true)
                    {
                        string count = TCPSocket.GetCountClientConnect().ToString();

                        btnCountClient.Invoke(new Action(() =>
                        {
                            btnCountClient.Text = count;
                        }));

                        Thread.Sleep(1000);
                    }
                });

            }
            else
            {
                TCPSocket.stopServer();
            }
        }

        private void btnBongTai_Click(object sender, EventArgs e)
        {
            TCPSocket.send("bongtai");
            Thread.Sleep(delaySocket);
        }

        private void btnCsBay_Click(object sender, EventArgs e)
        {

        }

        private void materialButton73_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|0");
            Thread.Sleep(delaySocket);
        }

        private void materialButton74_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|1");
            Thread.Sleep(delaySocket);
        }

        private void materialButton75_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|2");
            Thread.Sleep(delaySocket);
        }

        private void materialButton76_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|3");
            Thread.Sleep(delaySocket);
        }

        private void materialButton77_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|4");
            Thread.Sleep(delaySocket);
        }

        private void materialButton78_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|5");
            Thread.Sleep(delaySocket);
        }

        private void materialButton79_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|6");
            Thread.Sleep(delaySocket);
        }

        private void materialButton80_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|7");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|381");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|1150");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|384");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|1153");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|382");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|1152");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|383");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|1151");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|385");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|395");
            Thread.Sleep(delaySocket);
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            TCPSocket.send("chat|" + txtChatGame.Text);
            Thread.Sleep(delaySocket);
            txtChatGame.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TCPSocket.send("khu|" + txtKHU.Text);
            Thread.Sleep(delaySocket);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdItem.Text, out int id))
            {
                TCPSocket.send("item|" + id.ToString());
                Thread.Sleep(delaySocket);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đúng dạng id Item", "Thông báo");
            }

        }
        public void changeStatus(Button x)
        {
            if (x.Text.Contains("ON"))
            {
                x.Text = x.Text.Replace("ON", "OFF");
            }
            else
            {
                x.Text = x.Text.Replace("OFF", "ON");
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            TCPSocket.send("bongtai");
            Thread.Sleep(delaySocket);
        }

        private void materialButton91_Click(object sender, EventArgs e)
        {
            TCPSocket.send("skill|8");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            TCPSocket.send("BatCo93");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            TCPSocket.send("TatCo93");
            Thread.Sleep(delaySocket);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TCPSocket.send("Boom97");
            changeStatus((Button)sender);
            Thread.Sleep(delaySocket);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TCPSocket.send("fBoss97");
            changeStatus((Button)sender);
            Thread.Sleep(delaySocket);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TCPSocket.send("teleBoss97");
            changeStatus((Button)sender);
            Thread.Sleep(delaySocket);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TCPSocket.send("aBoss97");
            changeStatus((Button)sender);
            Thread.Sleep(delaySocket);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TCPSocket.send("doBoss97");
            changeStatus((Button)sender);
            Thread.Sleep(delaySocket);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            TCPSocket.send("doBoss97VIP");
            changeStatus((Button)sender);
            Thread.Sleep(delaySocket);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TCPSocket.send("trainMob23");
            Thread.Sleep(delaySocket);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdNpc.Text, out int id))
            {
                TCPSocket.send("teleIdNpc|" + id);
                Thread.Sleep(delaySocket);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đúng dạng id NPC", "Thông báo");
            }

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            TCPSocket.send("item|521");
            Thread.Sleep(delaySocket);
        }

        private void btnTeleNPC_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtIdNpc.Text, out int id))
            {
                TCPSocket.send("teleIdNpc|" + id);
                Thread.Sleep(delaySocket);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đúng dạng id NPC", "Thông báo");
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            TCPSocket.send("NextMapLeft");
            Thread.Sleep(delaySocket);
        }

        private void btnBetween_Click(object sender, EventArgs e)
        {
            TCPSocket.send("NextMapBetween");
            Thread.Sleep(delaySocket);
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            TCPSocket.send("NextMapRight");
            Thread.Sleep(delaySocket);
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("trainMob231");
            Thread.Sleep(delaySocket);
        }

        private void materialButton92_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "AppData", "LocalLow", "Team", "ragonboy244"
                );

                if (Directory.Exists(path))
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        File.Delete(file);
                    }
                    MessageBox.Show("Đã xóa dữ liệu game\nMở game lại để tải dữ liệu game", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi: ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void materialButton21_Click_1(object sender, EventArgs e)
        {

        }

        private void materialButton102_Click_1(object sender, EventArgs e)
        {
            nextMap(153);
        }

        private void materialButton21_Click_2(object sender, EventArgs e)
        {
            nextMap(46);
        }

        private void materialButton22_Click_1(object sender, EventArgs e)
        {
            nextMap(45);
        }

        private void materialButton93_Click(object sender, EventArgs e)
        {
            nextMap(48);
        }

        private void materialButton94_Click(object sender, EventArgs e)
        {
            nextMap(154);
        }

        private void materialButton95_Click(object sender, EventArgs e)
        {
            nextMap(155);
        }

        private void materialButton97_Click(object sender, EventArgs e)
        {
            nextMap(50);
        }

        private void materialButton96_Click(object sender, EventArgs e)
        {
            nextMap(166);
        }

        private void materialButton101_Click(object sender, EventArgs e)
        {
            nextMap(156);
        }

        private void materialButton100_Click(object sender, EventArgs e)
        {
            nextMap(157);
        }

        private void materialButton99_Click(object sender, EventArgs e)
        {
            nextMap(158);
        }

        private void materialButton98_Click(object sender, EventArgs e)
        {
            nextMap(159);
        }

        private void materialButton127_Click(object sender, EventArgs e)
        {
            nextMap(149);
        }

        private void materialButton112_Click(object sender, EventArgs e)
        {
            nextMap(147);
        }

        private void materialButton111_Click(object sender, EventArgs e)
        {
            nextMap(152);
        }

        private void materialButton110_Click(object sender, EventArgs e)
        {
            nextMap(151);
        }

        private void materialButton109_Click(object sender, EventArgs e)
        {
            nextMap(148);
        }

        private void txtidBegin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtY_TextChanged(object sender, EventArgs e)
        {

        }

        private void gbSkill_Enter(object sender, EventArgs e)
        {

        }

        private void QLTK_Click(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            int buttonColumnIndex = 5;
            int proxyColumnIndex = 5;

            if (e.ColumnIndex == buttonColumnIndex && e.RowIndex >= 0)
            {
                e.Handled = true;

                var proxyValue = dataGridView1.Rows[e.RowIndex].Cells[proxyColumnIndex].Value as string;
                var typeProxy = dataGridView1.Rows[e.RowIndex].Cells[proxyColumnIndex + 1].Value as string;

                Color backColor;
                string buttonText;

                if (string.IsNullOrEmpty(proxyValue))
                {
                    backColor = dataGridView1.DefaultCellStyle.BackColor;
                    buttonText = "None";
                }
                else if (!ProxyValidator.IsValidProxy(proxyValue, typeProxy))
                {
                    backColor = System.Drawing.Color.LightCoral;
                    buttonText = "Sai";
                }
                else
                {
                    backColor = System.Drawing.Color.MediumSeaGreen;
                    buttonText = "OK";
                }

                using (var brush = new System.Drawing.SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    buttonText,
                    e.CellStyle.Font,
                    e.CellBounds,
                    System.Drawing.Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Graphics.DrawRectangle(Pens.Gray, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
            }
        }

        private void checkProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkProxy.Checked)
            {

                txtProxy.UseSystemPasswordChar = false;
                txtProxy.Focus();
            }
            else
            {

                txtProxy.UseSystemPasswordChar = true;
                txtProxy.Focus();
            }
        }

        private async void button11_Click(object sender, EventArgs e)
        {
            btnCheckProxy.Enabled = false;

            try
            {
                string proxyString = txtStringProxy.Text.Trim().Replace(" ", "");
                string proxyType;

                if (string.IsNullOrEmpty(proxyString))
                {
                    txtThongBaoProxy.Text = "Vui lòng nhập proxy";
                    txtThongBaoProxy.ForeColor = Color.Red;
                    return;
                }

                switch (cbbTypeProxy1.SelectedIndex)
                {
                    case 0:
                        proxyType = "http";
                        break;
                    case 1:
                        proxyType = "socks5";
                        break;
                    case 2:
                        proxyType = "https";
                        break;
                    default:
                        txtThongBaoProxy.Text = "Vui lòng chọn loại proxy";
                        txtThongBaoProxy.ForeColor = Color.Red;
                        return;
                }

                if (!ProxyValidator.IsValidProxy(txtStringProxy.Text, (cbbTypeProxy1.SelectedIndex + 1).ToString()))
                {
                    txtThongBaoProxy.Text = "Proxy phải có định dạng: ip:port:user:pass";
                    txtThongBaoProxy.ForeColor = Color.Red;
                    return;
                }

                txtThongBaoProxy.Text = "Đang kiểm tra vui lòng đợi...";
                txtThongBaoProxy.ForeColor = Color.Blue;

                bool isAlive = await ProxyChecker.CheckProxy(proxyString, proxyType);

                if (isAlive)
                {
                    txtThongBaoProxy.Text = "Proxy Alive";
                    txtThongBaoProxy.ForeColor = Color.Green;
                }
                else
                {
                    txtThongBaoProxy.Text = "Proxy Dead";
                    txtThongBaoProxy.ForeColor = Color.Red;
                }
            }
            catch
            {
                txtThongBaoProxy.Text = "Lỗi kiểm tra proxy";
                txtThongBaoProxy.ForeColor = Color.Red;
            }
            finally
            {
                btnCheckProxy.Enabled = true;
            }
        }

        private void txtThongBaoProxy_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point(
                this.Location.X + (this.Width - frm.Width) / 2,
                this.Location.Y + (this.Height - frm.Height) / 2
            );
            frm.ShowDialog();
        }
    }
}
