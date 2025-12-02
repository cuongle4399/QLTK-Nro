using MaterialSkin;
using MaterialSkin.Controls;
using QLTK_Nro_Pro.Handlers;
using QLTK_Nro_Pro.Presenter;
using QLTK_Nro_Pro.Presenter.ProxyManager;
using QLTK_Nro_Pro.Presenter.Socket;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace QLTK_Nro_Pro
{
    public partial class Form1 : MaterialForm
    {
        public static DataGridView DatagridViewQLTK;
        public static string nro244 = Path.Combine(Application.StartupPath, "Nro_244.exe");
        private int indexSTT = 0;
        private int delaySocket = 50;
        private string ImageTestAPI = "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";
        public static string ChatPublic = "Data\\TextChatPublic.ini";
        public static string ChatGlobal = "Data\\TextChatGlobal.ini";
        public static string ChatInbox = "Data\\TextChatInbox.ini";
        private readonly MaterialSkinManager materialSkinManager;

        public Form1()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            #region ThemeForm
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Grey800,
                Primary.Grey700,
                Primary.Grey600,
                Accent.Red400,
                TextShade.WHITE
            );
            #endregion
            DatagridViewQLTK = this.dataGridView1;
            this.Text = this.Text + " " + CheckUpdate.version;
            XmapHandler.BindMapButtons(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtProxy.UseSystemPasswordChar = true;
            pass.UseSystemPasswordChar = true;
            var screen = Screen.PrimaryScreen.WorkingArea;
            int x = screen.Right - this.Width;
            int y = screen.Bottom - this.Height;
            this.Location = new Point(x, y);
            LoadData.createFolderData();
            CheckUpdate.CheckForUpdates();
            txtAPICapcha.Text = File.ReadAllText(LoadData.PathAPI);
            ShowImage(ImageTestAPI);
            #region LoadSize
            try
            {
                string[] a = File.ReadAllText(TabManager.filePath).Split('|');
                txtX.Text = a[0];
                txtY.Text = a[1];
            }
            catch { }
            #endregion
            #region Chat
            try
            {
                txtChat.Text = File.ReadAllText(ChatPublic);
            }
            catch
            {

            }
            #endregion
            #region LoadData
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            LoadData.docFile(ref indexSTT, dataGridView1);
            #endregion
            #region startSocket
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
            #endregion

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            TCPSocket.stopServer();
            base.OnFormClosing(e);
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            TabManager.closeGame();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtX.Text) >= 450 && int.Parse(txtY.Text) >= 500)
                {
                    TabManager.sortTabGame2D();
                }
                else
                {
                    TabManager.sortTabGamePixel();
                }
            }
            catch
            {
                TabManager.sortTabGame2D();
            }


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_user.Text) || string.IsNullOrEmpty(txt_server.Text) || string.IsNullOrEmpty(pass.Text))
            {
                MessageBox.Show("Nhập đầy đủ vào rồi thêm cục cưng", "Thông báo", MessageBoxButtons.OK);
                txt_user.Focus();
                return;
            }

            string proxyType = "1";
            if (rdoSOCK5S.Checked) proxyType = "2";
            else if (rdoHTTPS.Checked) proxyType = "3";

            var rowIndex = dataGridView1.Rows.Add(new object[]
            {
        indexSTT,
        txt_user.Text,
        LoadData.server(txt_server.Text),
        CryptoManager.Encryptor(pass.Text, "ud"),
        txt_note.Text,
        txtProxy.Text,
        proxyType
            });

            var row = dataGridView1.Rows[rowIndex];
            row.Height = 40;

            row.Tag = new RowOriginalData
            {
                TaiKhoan = txt_user.Text,
                GhiChu = txt_note.Text,
                BackColor = row.DefaultCellStyle.BackColor,
                ForeColor = row.DefaultCellStyle.ForeColor,
                SelectionBackColor = row.DefaultCellStyle.SelectionBackColor,
                SelectionForeColor = row.DefaultCellStyle.SelectionForeColor
            };

            indexSTT++;
            LoadData.ghifile(dataGridView1);

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
                File.WriteAllText(TabManager.filePath, txtX.Text + '|' + txtY.Text + '|' + '0');
                MessageBox.Show("Đã cập nhập kích thước game thành công", "Thông báo");
            }
            catch
            {
                MessageBox.Show("Lỗi", "Thông báo");

            }
        }

        private void btnUpdateChat_Click(object sender, EventArgs e)
        {

            File.WriteAllText(ChatGlobal, txtChat.Text);
            File.WriteAllText(ChatPublic, txtChat.Text);
            File.WriteAllText(ChatInbox, txtChat.Text);
            MessageBox.Show("Đã cập nhập nội dung chat thành công", "Thông báo");
        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null) return;

            if (string.IsNullOrEmpty(txt_user.Text) || string.IsNullOrEmpty(txt_server.Text) || string.IsNullOrEmpty(pass.Text))
            {
                MessageBox.Show("Nhập đầy đủ vào rồi sửa", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            var row = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex];

            row.Cells[1].Value = txt_user.Text;
            row.Cells[2].Value = LoadData.server(txt_server.Text);
            row.Cells[3].Value = CryptoManager.Encryptor(pass.Text, "ud");
            row.Cells[4].Value = txt_note.Text;
            row.Cells[5].Value = txtProxy.Text;

            string typeProxy = "1";
            if (rdoSOCK5S.Checked) typeProxy = "2";
            else if (rdoHTTPS.Checked) typeProxy = "3";
            row.Cells[6].Value = typeProxy;

            // Cập nhật lại Tag
            if (row.Tag is RowOriginalData original)
            {
                original.TaiKhoan = txt_user.Text;
                original.GhiChu = txt_note.Text;
            }
            else
            {
                row.Tag = new RowOriginalData
                {
                    TaiKhoan = txt_user.Text,
                    GhiChu = txt_note.Text,
                    BackColor = row.DefaultCellStyle.BackColor,
                    ForeColor = row.DefaultCellStyle.ForeColor,
                    SelectionBackColor = row.DefaultCellStyle.SelectionBackColor,
                    SelectionForeColor = row.DefaultCellStyle.SelectionForeColor
                };
            }

            LoadData.ghifile(dataGridView1);
            MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null || dataGridView1.RowCount == 0) return;

            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(index);
            indexSTT = dataGridView1.Rows.Count;
            LoadData.ghifile(dataGridView1);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string pathGame1 = nro244;
            string nameWindowGame1 = TabManager.NameWindownro244;
            if (dataGridView1.CurrentRow.Index >= 0)
            {
                try
                {
                    int index = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    TabManager.startGame(index + 1, pathGame1, nameWindowGame1);
                }
                catch
                {
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var row = dataGridView1.Rows[e.RowIndex];

            // Nếu có dữ liệu gốc thì dùng
            string taiKhoan = row.Cells[1].Value?.ToString();
            string ghiChu = row.Cells[4].Value?.ToString();

            if (row.Tag is RowOriginalData original)
            {
                if (original.TaiKhoan != null) taiKhoan = original.TaiKhoan.ToString();
                if (original.GhiChu != null) ghiChu = original.GhiChu.ToString();
            }

            txt_user.Text = taiKhoan;
            txt_note.Text = ghiChu;

            txt_server.Text = LoadData.Reserver(int.Parse(row.Cells[2].Value?.ToString() ?? "0"));
            pass.Text = CryptoManager.Decryptor(row.Cells[3].Value?.ToString(), "ud");
            txtProxy.Text = row.Cells[5].Value?.ToString();

            string value = row.Cells[6].Value?.ToString();
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
            File.WriteAllText(TabManager.filePath, 1068.ToString() + '|' + 600.ToString() + '|' + '0');
            gbSize.Enabled = true;
            txtX.Text = "1068";
            txtY.Text = "600";
            switchSize.Checked = false;
            gbSize.Enabled = false;
            MessageBox.Show("Đã cập nhập kích thước game thành công", "Thông báo");
        }

        private void materialButton40_Click(object sender, EventArgs e)
        {
            File.WriteAllText(TabManager.filePath, 350.ToString() + '|' + 400.ToString() + '|' + '0');
            gbSize.Enabled = true;
            txtX.Text = "350";
            txtY.Text = "400";
            switchSize.Checked = false;
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
                        string nameWindowGame1 = TabManager.NameWindownro244;
                        try
                        {

                            TabManager.startGame(i + 1, pathGame1, nameWindowGame1);
                            Thread.Sleep(1300);
                        }
                        catch
                        {
                        }

                    }

                }
            });

        }

        private void materialButton92_Click(object sender, EventArgs e)
        {
            LoadData.DeleteDataGame();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAPICapcha.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập key API Capcha");
                return;
            }
            File.WriteAllText(LoadData.PathAPI, txtAPICapcha.Text.Trim());
            MessageBox.Show("Đã Lưu key API thành công");
        }

        private void materialButton128_Click(object sender, EventArgs e)
        {
            string folderPath = Application.StartupPath;

            if (Directory.Exists(folderPath))
            {
                Process.Start("explorer.exe", folderPath);
            }
            else
            {
                MessageBox.Show("❌ Thư mục không tồn tại. Bạn không giải nén file à!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private static readonly HttpClient _httpClient = new HttpClient();

        private async void button12_Click(object sender, EventArgs e)
        {
            string apiKey = txtAPICapcha.Text.Trim();
            if (string.IsNullOrEmpty(apiKey))
            {
                MessageBox.Show("Vui lòng nhập API key!");
                return;
            }

            lblAPICapcha.Text = "🔄 Kiểm tra...";
            lblAPICapcha.ForeColor = Color.Gray;

            await TestCaptcha(apiKey, ImageTestAPI);
        }

        private void ShowImage(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                using (var ms = new MemoryStream(bytes))
                {
                    pBAPI.Image = Image.FromStream(ms);
                }
            }
            catch
            {
                lblAPICapcha.Text = "❌ Ảnh Capcha lỗi!";
                lblAPICapcha.ForeColor = Color.Red;
            }
        }

        private async Task TestCaptcha(string apiKey, string base64Image)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                lblAPICapcha.Text = "📸 Đang xử lý ảnh...";
                lblAPICapcha.ForeColor = Color.Gray;

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["image"] = base64Image
                });

                lblAPICapcha.Text = "📤 Đang gửi...";
                lblAPICapcha.ForeColor = Color.Gray;

                var res = await _httpClient.PostAsync($"https://api.phamgiang.net/captcha/nro?token={apiKey}", content);

                lblAPICapcha.Text = "📥 Đang phản hồi...";
                lblAPICapcha.ForeColor = Color.Gray;

                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                {
                    lblAPICapcha.Text = $"❌ HTTP {(int)res.StatusCode}";
                    lblAPICapcha.ForeColor = Color.Red;
                    return;
                }

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                int status = root.GetProperty("status").GetInt32();
                string captcha = root.GetProperty("captcha").GetString() ?? "Không có";
                double confidence = root.GetProperty("confidence").GetDouble();
                int time = root.GetProperty("time").GetInt32();
                string message = root.TryGetProperty("message", out var msgProp) ? msgProp.GetString() ?? "" : "";

                sw.Stop();

                lblAPICapcha.ForeColor = status == 0 ? Color.Green : Color.Red;
                lblAPICapcha.Text = $"Captcha [OK]: {captcha}";
            }
            catch (Exception ex)
            {
                lblAPICapcha.Text = $"❌ Lỗi: {ex.Message}";
                lblAPICapcha.ForeColor = Color.Red;
            }
        }


        private void materialButton129_Click(object sender, EventArgs e)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "https://api.phamgiang.net/",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi mở link: {ex.Message}");
            }
        }

        private void materialButton130_Click(object sender, EventArgs e)
        {
            frmCapcha frm = new frmCapcha();
            frm.ShowDialog();
        }
        #region SocketHome
        private void btnChat_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("chat|" + txtChatGame.Text);
            Thread.Sleep(delaySocket);
            txtChatGame.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("khu|" + txtKHU.Text);
            Thread.Sleep(delaySocket);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|" + txtIdItem.Text);
            Thread.Sleep(delaySocket);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|381");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|1150");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|384");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|1153");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|382");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|1152");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|383");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|1151");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox9_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|385");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox14_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|1154");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox10_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("bongtai");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox13_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("BatCoDen");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox15_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("TatCo");
            Thread.Sleep(delaySocket);
        }

        private void pictureBox11_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("item|521");
            Thread.Sleep(delaySocket);
        }

        private void btnTeleNPC_Click_1(object sender, EventArgs e)
        {
            TCPSocket.send("teleIdNpc|" + txtIdNpc.Value);
            Thread.Sleep(delaySocket);
        }
        #endregion
        public void changeStatus(Button x)
        {
            if (x.Text.Contains("ON"))
                x.Text = x.Text.Replace("ON", "OFF");
            else
                x.Text = x.Text.Replace("OFF", "ON");
        }

        private void SendCommand(Button btn, string command)
        {
            string cmdToSend;

            if (btn.Text.Contains("ON"))
            {
                cmdToSend = "OFF" + command;
                TCPSocket.send(cmdToSend);
                changeStatus(btn);
            }
            else if (btn.Text.Contains("OFF"))
            {
                cmdToSend = "ON" + command;
                TCPSocket.send(cmdToSend);
                changeStatus(btn);
            }
            else
            {
                cmdToSend = command;
                TCPSocket.send(cmdToSend);
            }

            Thread.Sleep(delaySocket);
            //MessageBox.Show(cmdToSend);
        }


        #region Socket Auto Boss
        private void button3_Click(object sender, EventArgs e) => SendCommand((Button)sender, "Boom");
        private void button4_Click(object sender, EventArgs e) => SendCommand((Button)sender, "findBoss");
        private void button5_Click(object sender, EventArgs e) => SendCommand((Button)sender, "teleBoss");
        private void button7_Click(object sender, EventArgs e) => SendCommand((Button)sender, "acttackBoss");
        private void button8_Click(object sender, EventArgs e) => SendCommand((Button)sender, "doBoss");
        private void button13_Click(object sender, EventArgs e) => SendCommand((Button)sender, "autoWhis");
        private void button9_Click(object sender, EventArgs e) => SendCommand((Button)sender, "farmNappa|" + cbbBossNappa.SelectedIndex);
        private void button14_Click(object sender, EventArgs e) => SendCommand((Button)sender, "findBossTrungMabu");
        #endregion

        #region TrainMob
        private void button6_Click(object sender, EventArgs e) => SendCommand((Button)sender, "trainMob");
        private void button16_Click(object sender, EventArgs e) => SendCommand((Button)sender, "goBack");
        private void button17_Click(object sender, EventArgs e) => SendCommand((Button)sender, "goBackToaDo");

        #endregion

        #region Auto Bo Mong
        private void materialButton20_Click_1(object sender, EventArgs e)
        {
            string cmd = "BoMong|" + TypeNV.Text.ToLower().Trim() + "|" + cbbTypeNVGold.SelectedIndex + "|"
                         + chkNextGold.Checked + "|" + chkNextMob.Checked + "|" + chkNextHuman.Checked;
            SendCommand((Button)sender, cmd);
        }
        #endregion

        #region Auto Pet
        private void button18_Click(object sender, EventArgs e) => SendCommand((Button)sender, "deSua");
        private void button19_Click(object sender, EventArgs e) => SendCommand((Button)sender, "deKOK");
        private void button20_Click(object sender, EventArgs e) => SendCommand((Button)sender, "deCoDen");
        private void button21_Click(object sender, EventArgs e) => SendCommand((Button)sender, "deAutoNhat");
        private void button22_Click(object sender, EventArgs e) => SendCommand((Button)sender, "deGim");
        private void button23_Click(object sender, EventArgs e) => SendCommand((Button)sender, "deTTNL|" + txtPercenHP.Value);
        private void materialButton131_Click(object sender, EventArgs e) => SendCommand((Button)sender, "xinDau");
        private void materialButton132_Click(object sender, EventArgs e) => SendCommand((Button)sender, "ThuDau");
        private void materialButton133_Click(object sender, EventArgs e) => SendCommand((Button)sender, "ChoDau");
        #endregion

        private void btnReduceCPU_Click(object sender, EventArgs e) => SendCommand((Button)sender, $"reduceCPU|{txtFps.Value}");

        private void btnNhapCodeLive_Click(object sender, EventArgs e) => SendCommand((Button)sender, $"NhapCodeLive|{txtCodeLive.Text.Trim()}");

        private void materialButton77_Click(object sender, EventArgs e) => SendCommand((Button)sender, $"TagNameAutoBoss|{txtTagNameBoss.Text.Trim().ToLower()}");

        private void btnshow_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "• Mỗi tên boss cách nhau bằng 1 dấu phẩy (,).\n" +
                "• Không phân biệt chữ hoa hay chữ thường.\n" +
                "• Nếu để trống, hệ thống sẽ tự động dò, tele, tấn công tất cả các loại boss không lọc theo tên.",
                "Hướng dẫn cấu hình boss",
                MessageBoxButtons.OK
            );
        }

        private void btnImportDataNick_Click(object sender, EventArgs e)
        {
            ImportDataNickFlash importDataNickFlash = new ImportDataNickFlash(dataGridView1, ref indexSTT);
            importDataNickFlash.ShowDialog();
        }

        private void btnBoxZalo_Click(object sender, EventArgs e)
        {
            var url = "https://zalo.me/g/stvcsp741";
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở link!\n" + ex.Message);
            }
        }

        private void btnNeSieuQuai_Click(object sender, EventArgs e) => SendCommand((Button)sender, "autoNeSieuQuai");
        private void btnAkDame_Click(object sender, EventArgs e) => SendCommand((Button)sender, "trainAkDame");
        private void btnAutoNhat_Click(object sender, EventArgs e) => SendCommand((Button)sender, "AutoNhat");
        private void btnNeBoss_Click(object sender, EventArgs e) => SendCommand((Button)sender, "autoNeBoss");
        private void btnAutoHopThe_Click(object sender, EventArgs e) => SendCommand((Button)sender, "autoHopThe");
        private void btnSpamZoneIt_Click(object sender, EventArgs e) => SendCommand((Button)sender, "spamZoneIt");
        private void btnAutoZoneIt_Click(object sender, EventArgs e) => SendCommand((Button)sender, "autoZoneIt");
    }

}