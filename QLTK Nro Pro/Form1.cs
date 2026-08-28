using MaterialSkin;
using MaterialSkin.Controls;
using QLTK_Nro_Pro.HandlerSocket;
using QLTK_Nro_Pro.Presenter;
using QLTK_Nro_Pro.Presenter.ProxyManager;
using QLTK_Nro_Pro.Presenter.Socket;
using QLTK_Nro_Pro.Service;
using System.Diagnostics;
using System.Text.Json;
using File = System.IO.File;

namespace QLTK_Nro_Pro
{
    public partial class Form1 : MaterialForm, IAccountStatusView
    {
        public static DataGridView DatagridViewQLTK;
        private CaptchaApiManager _captchaApiManager;
        private int indexSTT = 0;
        public static int CountClient = 0;
        private readonly MaterialSkinManager materialSkinManager;
        private SocketClientUpdater socketClientUpdater;
        private ProcessResourceMonitor _processMonitor;
        private System.Windows.Forms.Timer _pcTimer;
        public Form1()
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns["Tick"].ReadOnly = false;

            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView1.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridView1.IsCurrentCellDirty)
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            this.Text = "QLTK Nro Cuong Le - " + CheckUpdate.version;
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
            ServerManager.PopulateComboBox(txt_server);
            CheckUpdate.CheckForUpdates();
            _captchaApiManager = new CaptchaApiManager(lblAPICapcha, pBAPI, cbbServerAPI, txtServerAPI);
            _captchaApiManager.ShowImage(AppConstants.ImageTestAPI);
            txtAPICapcha.Text = ConfigManager.LoadText(AppConstants.PathAPI);
            #region LoadSize
            string sizeContent = ConfigManager.LoadText(TabManager.filePath);
            if (!string.IsNullOrEmpty(sizeContent))
            {
                string[] a = sizeContent.Split('|');
                if (a.Length >= 2)
                {
                    txtX.Text = a[0];
                    txtY.Text = a[1];
                }
            }
            #endregion
            #region Hardware Monitor (RUN BACKGROUND)
            Task.Run(async () =>
            {
                await Task.Delay(1000); // cho form load xong

                _processMonitor = new ProcessResourceMonitor();

                this.BeginInvoke((Action)(() =>
                {
                    _pcTimer = new System.Windows.Forms.Timer
                    {
                        Interval = 800
                    };
                    _pcTimer.Tick += PcTimer_Tick;
                    _pcTimer.Start();
                }));
            });
            #endregion

            #region Chat
            txtChat.Text = ConfigManager.LoadText(AppConstants.ChatPublic);
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
            socketClientUpdater = new SocketClientUpdater(this, btnAutoOpenTab);
            ReceiveDataClient.StatusView = this;

            Task.Run(() =>
            {
                while (true)
                {
                    CountClient = TCPSocket.GetCountClientConnect();
                    Thread.Sleep(1000);
                }
            });
            #endregion

            #region AdminDev
            Load_API_CapCha();
            if (File.Exists("admin.ini"))
            {
                btnAdminDev.Text = "Chế độ nhà phát triển: ON";
                btnAdminDev.BackColor = Color.CadetBlue;
            }
            else
            {
                btnAdminDev.Text = "Chế độ nhà phát triển: OFF";
                btnAdminDev.BackColor = Color.White;
            }
            #endregion
            #region LoadTextXmapNpc
            txtVut.Text = ConfigManager.LoadText(TabManager.ListVutItem);
            #endregion
            #region LoadListVutItem
            txtTextXmap.Text = ConfigManager.LoadText(TabManager.TextNpcXmap);
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
        private void PcTimer_Tick(object sender, EventArgs e)
        {
            lblConfigPC.Text = _processMonitor.GetProcessUsage(TabManager.GetOpenedTabProcesses());
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

            ServerManager.AddOrEnsureServer(txt_server.Text);
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
                ConfigManager.SaveText(TabManager.filePath, txtX.Text + '|' + txtY.Text + '|' + '0');
                MessageBox.Show("Đã cập nhập kích thước game thành công", "Thông báo");
            }
            catch
            {
                MessageBox.Show("Lỗi", "Thông báo");
            }
        }

        private void btnUpdateChat_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveChat(txtChat.Text);
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

            ServerManager.AddOrEnsureServer(txt_server.Text);
            row.Cells[1].Value = txt_user.Text;
            row.Cells[2].Value = LoadData.server(txt_server.Text);
            row.Cells[3].Value = CryptoManager.Encryptor(pass.Text, "ud");
            row.Cells[4].Value = txt_note.Text;
            row.Cells[5].Value = txtProxy.Text;

            string typeProxy = "1";
            if (rdoSOCK5S.Checked) typeProxy = "2";
            else if (rdoHTTPS.Checked) typeProxy = "3";
            row.Cells[6].Value = typeProxy;

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
            string pathGame1 = AppConstants.nro244;
            string nameWindowGame1 = TabManager.NameWindownro244;
            if (dataGridView1.CurrentRow.Index >= 0)
            {
                try
                {
                    int index = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    TabManager.startGame(index + 1, pathGame1, nameWindowGame1);
                }
                catch { }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var row = dataGridView1.Rows[e.RowIndex];

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

        private void materialButton16_Click(object sender, EventArgs e) { }

        private void materialButton20_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveText("Data/LoadMap.ini", "F|-1");
            MessageBox.Show("Đã Khôi phục NextMap khắc phục lỗi", "Thông báo");
        }

        private void materialButton38_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = AppConstants.LinkFBCuongLe,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void materialButton38_Click_1(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = AppConstants.LinkWebMod,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void materialButton39_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveText(TabManager.filePath, 1068.ToString() + '|' + 600.ToString() + '|' + '0');
            gbSize.Enabled = true;
            txtX.Text = "1068";
            txtY.Text = "600";
            switchSize.Checked = false;
            gbSize.Enabled = false;
            MessageBox.Show("Đã cập nhập kích thước game thành công", "Thông báo");
        }

        private void materialButton40_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveText(TabManager.filePath, 350.ToString() + '|' + 400.ToString() + '|' + '0');
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
            gbSize.Enabled = switch1;
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
                        string pathGame1 = AppConstants.nro244;
                        string nameWindowGame1 = TabManager.NameWindownro244;
                        try
                        {
                            TabManager.startGame(i + 1, pathGame1, nameWindowGame1);
                            Thread.Sleep(1300);
                        }
                        catch { }
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
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Tick")
            {
                e.Handled = false;
                return;
            }

            int proxyColumnIndex = 5;

            if (e.ColumnIndex == proxyColumnIndex && e.RowIndex >= 0)
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
                    backColor = Color.LightCoral;
                    buttonText = "Sai";
                }
                else
                {
                    backColor = Color.MediumSeaGreen;
                    buttonText = "OK";
                }

                using (var brush = new SolidBrush(backColor))
                    e.Graphics.FillRectangle(brush, e.CellBounds);

                TextRenderer.DrawText(
                    e.Graphics,
                    buttonText,
                    e.CellStyle.Font,
                    e.CellBounds,
                    Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Graphics.DrawRectangle(
                    Pens.Gray,
                    e.CellBounds.Left,
                    e.CellBounds.Top,
                    e.CellBounds.Width - 1,
                    e.CellBounds.Height - 1
                );
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
            if (string.IsNullOrEmpty(txtServerAPI.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập Server key API Capcha");
                return;
            }
            ConfigManager.SaveText(AppConstants.PathAPI, txtAPICapcha.Text.Trim());
            ConfigManager.SaveText(AppConstants.PathServerAPI, txtServerAPI.Text.Trim());
            MessageBox.Show("Đã Lưu key và server API thành công");
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
                MessageBox.Show("Thư mục không tồn tại. Bạn không giải nén file à!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            string apiKey = txtAPICapcha.Text.Trim();
            if (string.IsNullOrEmpty(apiKey))
            {
                MessageBox.Show("Vui lòng nhập API key!");
                return;
            }

            lblAPICapcha.Text = "🔄 Kiểm tra...";
            lblAPICapcha.ForeColor = System.Drawing.Color.Gray;
            await _captchaApiManager.TestCaptcha(apiKey, AppConstants.ImageTestAPI);
        }

        private void materialButton129_Click(object sender, EventArgs e)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = (cbbServerAPI.SelectedIndex == 0) ? "https://api.phamgiang.net/" : "http://api.tooltvt.com/",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở link: {ex.Message}");
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
            socketClientUpdater.SendChatMessage(txtChatGame.Text);
            txtChatGame.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            socketClientUpdater.SendKhuCommand(txtKHU.Text);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            socketClientUpdater.SendItemCommand(txtIdItem.Text);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("381");
        private void pictureBox2_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("1150");
        private void pictureBox3_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("384");
        private void pictureBox4_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("1153");
        private void pictureBox5_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("382");
        private void pictureBox6_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("1152");
        private void pictureBox7_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("383");
        private void pictureBox8_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("1151");
        private void pictureBox9_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("385");
        private void pictureBox14_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("1154");
        private void pictureBox10_Click_1(object sender, EventArgs e) => socketClientUpdater.SendBongTaiCommand();
        private void pictureBox13_Click_1(object sender, EventArgs e) => socketClientUpdater.SendBatCoDenCommand();
        private void pictureBox15_Click_1(object sender, EventArgs e) => socketClientUpdater.SendTatCoCommand();
        private void pictureBox11_Click_1(object sender, EventArgs e) => socketClientUpdater.SendItemCommand("521");
        private void btnTeleNPC_Click_1(object sender, EventArgs e) => socketClientUpdater.SendTeleNpcCommand((int)txtIdNpc.Value);
        #endregion

        public void ChangeStatus(Button x)
        {
            socketClientUpdater.ChangeStatus(x);
        }

        #region Socket Auto Boss
        private void button3_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "Boom");
        private void button4_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "findBoss");
        private void button5_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "teleBoss");
        private void button7_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "acttackBoss");
        private void button8_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "doBoss");
        private void button13_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "autoWhis");
        private void button9_Click(object sender, EventArgs e) => socketClientUpdater.SendFarmNappaCommand((Button)sender, cbbBossNappa.SelectedIndex);
        private void button14_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "findBossTrungMabu");
        #endregion

        #region TrainMob
        private void button6_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "trainMob");
        private void button16_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "goBack");
        private void button17_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "goBackToaDo");
        #endregion

        #region Auto Bo Mong
        private void materialButton20_Click_1(object sender, EventArgs e)
        {
            socketClientUpdater.SendBoMongCommand((Button)sender, TypeNV.Text, cbbTypeNVGold.SelectedIndex, chkNextGold.Checked, chkNextMob.Checked, chkNextHuman.Checked);
        }
        #endregion

        #region Auto Pet
        private void button18_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "deSua");
        private void button19_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "deKOK");
        private void button20_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "deCoDen");
        private void button21_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "deAutoNhat");
        private void button22_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "deGim");
        private void button23_Click(object sender, EventArgs e) => socketClientUpdater.SendTTNLCommand((Button)sender, (int)txtPercenHP.Value);
        private void materialButton131_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "xinDau");
        private void materialButton132_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "ThuDau");
        private void materialButton133_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "ChoDau");
        #endregion

        private void btnReduceCPU_Click(object sender, EventArgs e) => socketClientUpdater.SendReduceCPUCommand((Button)sender, (int)txtFps.Value);

        private void btnNhapCodeLive_Click(object sender, EventArgs e) => socketClientUpdater.SendCodeLiveCommand((Button)sender, txtCodeLive.Text);

        private void materialButton77_Click(object sender, EventArgs e) => socketClientUpdater.SendTagNameBossCommand((Button)sender, txtTagNameBoss.Text);

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

        private string _lastSelectedServer = "";

        private void txt_server_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_server.SelectedItem == null) return;

            string selected = txt_server.SelectedItem.ToString();
            if (selected == ServerManager.ADD_NEW_SERVER_ITEM)
            {
                using (FormServerManager form = new FormServerManager())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        ServerManager.PopulateComboBox(txt_server);
                        if (ServerManager.Servers.Count > 0)
                        {
                            txt_server.Text = ServerManager.Servers.Last().Name;
                            _lastSelectedServer = txt_server.Text;
                        }
                    }
                    else
                    {
                        ServerManager.PopulateComboBox(txt_server);
                        if (!string.IsNullOrEmpty(_lastSelectedServer))
                        {
                            txt_server.Text = _lastSelectedServer;
                        }
                    }
                }
            }
            else
            {
                _lastSelectedServer = selected;
            }
        }

        private void btnBoxZalo_Click(object sender, EventArgs e)
        {
            var url = AppConstants.LinkBoxZalo;
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

        private void btnNeSieuQuai_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "autoNeSieuQuai");
        private void btnAkDame_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "trainAkDame");
        private void btnAutoNhat_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "AutoNhat");
        private void btnNeBoss_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "autoNeBoss");
        private void btnAutoHopThe_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "autoHopThe");
        private void btnSpamZoneIt_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "spamZoneIt");
        private void btnAutoZoneIt_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "autoZoneIt");
        private void btnCheckLagTnsm_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "autochecklag");
        private void btnGiaiCapcha_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "autocapcha");

        private void cbbServerAPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ===== ĐỔI: Gọi CaptchaApiManager =====
            _captchaApiManager.HandleServerApiSelectionChanged(serverApi =>
            {
                File.WriteAllText(AppConstants.PathServerAPI, serverApi);
            });
        }


        public void Load_API_CapCha()
        {
            // ===== ĐỔI: Gọi CaptchaApiManager =====
            _captchaApiManager.LoadApiConfig(AppConstants.PathServerAPI);
        }

        private void btnAdminDev_Click(object sender, EventArgs e)
        {
            if (File.Exists("admin.ini"))
            {
                File.Delete("admin.ini");
                btnAdminDev.Text = "Chế độ nhà phát triển: OFF";
                btnAdminDev.BackColor = Color.White;
            }
            else
            {
                File.WriteAllText("admin.ini", "Cuong Le");
                btnAdminDev.Text = "Chế độ nhà phát triển: ON";
                btnAdminDev.BackColor = Color.CadetBlue;
            }
        }

        private void btnApDungVut_Click(object sender, EventArgs e) => socketClientUpdater.SendCommand((Button)sender, "listvut|" + txtVut.Text);

        private void btnTextXmap_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveText(Path.Combine("Data", "TextNpcXmap.ini"), txtTextXmap.Text);
            MessageBox.Show("Đã lưu text Xmap thành công!", "Thông báo");
        }

        private void btnSaveListThrow_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveText(Path.Combine("Data", "ListVutIdItem.ini"), txtVut.Text);
            MessageBox.Show("Đã lưu danh sách item vứt thành công!", "Thông báo");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Tick")
            {
                var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                bool current = false;

                if (cell.Value != null && cell.Value != DBNull.Value)
                    current = Convert.ToBoolean(cell.Value);

                cell.Value = !current;
            }
        }

        private CancellationTokenSource autoOpenCts;
        private bool isAutoOpenTab = false;

        private void btnAutoOpenTab_Click(object sender, EventArgs e)
        {
            if (!isAutoOpenTab)
            {
                isAutoOpenTab = true;

                btnAutoOpenTab.Text = "Auto mở tab: ON";
                btnAutoOpenTab.BackColor = Color.LimeGreen;

                TabManager.SyncOpenedTabsFromRunningGames();

                autoOpenCts = new CancellationTokenSource();
                var token = autoOpenCts.Token;

                Task.Run(async () =>
                {
                    try
                    {
                        while (!token.IsCancellationRequested)
                        {
                            TabManager.OpenTabsByTick();

                            await Task.Delay(3000, token);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }, token);
            }
            else
            {
                isAutoOpenTab = false;

                btnAutoOpenTab.Text = "Auto mở tab: OFF";
                btnAutoOpenTab.BackColor = Color.CadetBlue;

                autoOpenCts?.Cancel();
                autoOpenCts?.Dispose();
                autoOpenCts = null;

                TabManager.ClearOpenedTabs();
            }
        }

        private void btnThuMucTool_Click(object sender, EventArgs e)
        {
            string folderPath = Application.StartupPath;

            if (Directory.Exists(folderPath))
            {
                Process.Start("explorer.exe", folderPath);
            }
            else
            {
                MessageBox.Show("Thư mục không tồn tại. Bạn không giải nén file à!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region IAccountStatusView implementation

        private static readonly Color OnlineBackNormal = Color.LightGreen;
        private static readonly Color OnlineForeNormal = Color.Black;
        private static readonly Color OnlineBackSelected = Color.LimeGreen;
        private static readonly Color OnlineForeSelected = Color.Black;

        public void SetAccountOnline(string accountId, string characterName)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => SetAccountOnline(accountId, characterName)));
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["ID"].Value?.ToString() != accountId)
                    continue;

                EnsureRowTag(row);

                row.Cells["TaiKhoan"].Value = characterName;
                row.Cells["GhiChu"].Value = "Đang Online";

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = OnlineBackNormal;
                    cell.Style.ForeColor = OnlineForeNormal;
                    cell.Style.SelectionBackColor = OnlineBackSelected;
                    cell.Style.SelectionForeColor = OnlineForeSelected;
                }
                break;
            }
        }

        public void SetAccountOffline(string accountId)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => SetAccountOffline(accountId)));
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["ID"].Value?.ToString() != accountId)
                    continue;

                EnsureRowTag(row);
                RestoreOfflineRow(row);
                break;
            }
        }

        public void ResetAllCells()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => ResetAllCells()));
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                EnsureRowTag(row);
                RestoreOfflineRow(row);
            }
        }

        private void EnsureRowTag(DataGridViewRow row)
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

        private void RestoreOfflineRow(DataGridViewRow row)
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

        #endregion
    
    }
}