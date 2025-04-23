using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using MaterialSkin;
using MaterialSkin.Controls;

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
        private void Form1_Load(object sender, EventArgs e)
        {
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
                });
                    indexSTT++;

                }
            }
            catch { }
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
                            gridView.Rows[i].Cells[4].Value.ToString();
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
                if (!versionInfo.Contains("1.6"))
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
                Process.Start(arguments: File.ReadAllLines(info.string_1)[int_0 - 1], fileName: PathGame);
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
            SortWindows();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_user.Text) || string.IsNullOrEmpty(txt_server.Text) || string.IsNullOrEmpty(txt_password.Text))
            {
                MessageBox.Show("Nhập đầy đủ vào rồi thêm cục cưng ", "Thông báo", MessageBoxButtons.OK);
                txt_user.Focus();
                return;
            }
            dataGridView1.Rows.Add(new object[]
            {
                indexSTT,
                    txt_user.Text,
                    server(txt_server.Text),
                    smethod_1(txt_password.Text, "ud"),
                    txt_note.Text,

                });
            indexSTT++;
            ghifile(dataGridView1);
            txt_user.Clear();
        }



        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            if (check.Checked)
            {
                txt_password.Focus();
                txt_password.Password = false;
            }
            else
            {
                txt_password.Focus();
                txt_password.Password = true;
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
            if (string.IsNullOrEmpty(txt_user.Text) || string.IsNullOrEmpty(txt_server.Text) || string.IsNullOrEmpty(txt_password.Text))
            {
                MessageBox.Show("Nhập đầy đủ vào rồi thêm cục cưng ", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value = txt_user.Text;
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value = smethod_1(txt_password.Text, "ud");
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value = server(txt_server.Text);
            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value = txt_note.Text;
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
            txt_password.Text = smethod_2(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), "ud");
            txt_note.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
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
            txtX.Text = "1050";
            txtY.Text = "591";
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

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            bool switch1 = switchChat.Checked;
            if (switch1)
            {
                gbChat.Enabled = true;
            }
            else
            {
                gbChat.Enabled = false;
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


    }
}
