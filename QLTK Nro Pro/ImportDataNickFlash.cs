using QLTK_Nro_Pro.Presenter;
using QLTK_Nro_Pro.Presenter.Socket;
using System.Data;
using System.Text;

namespace QLTK_Nro_Pro
{
    public partial class ImportDataNickFlash : Form
    {
        private DataGridView dataGridView1;
        private int indexSTT;

        public ImportDataNickFlash(DataGridView dgv, ref int stt)
        {
            InitializeComponent();
            dataGridView1 = dgv;
            indexSTT = stt;
        }

        private void btnImportFileData_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text files (*.txt)|*.txt|Tất cả các file|*.*";
                    ofd.Title = "Chọn file danh sách tài khoản";

                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;

                    string[] lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                    if (lines.Length == 0)
                    {
                        MessageBox.Show("File trống, không có tài khoản nào.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    HashSet<string> existingAccounts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells.Count > 3 &&
                            row.Cells[1].Value != null &&
                            row.Cells[2].Value != null &&
                            row.Cells[3].Value != null)
                        {
                            string user = row.Cells[1].Value.ToString().Trim();
                            string server = row.Cells[2].Value.ToString().Trim();
                            string pass = CryptoManager.Decryptor(row.Cells[3].Value.ToString().Trim(), "ud");
                            existingAccounts.Add($"{user}|{pass}|{server}");
                        }
                    }

                    List<(string user, string pass, string server)> allAccounts = new();
                    List<string> invalidLines = new();
                    foreach (string rawLine in lines)
                    {
                        if (string.IsNullOrWhiteSpace(rawLine))
                            continue;

                        string line = rawLine.Trim();
                        string[] parts = line.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length < 3)
                        {
                            invalidLines.Add(line);
                            continue;
                        }

                        string user = parts[0].Trim();
                        string pass = parts[1].Trim();
                        string server = parts[2].Trim();

                        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(server))
                        {
                            invalidLines.Add(line);
                            continue;
                        }

                        allAccounts.Add((user, pass, server));
                    }

                    if (allAccounts.Count == 0)
                    {
                        MessageBox.Show("Không có tài khoản hợp lệ trong file.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult choice = MessageBox.Show(
                        $"File có {allAccounts.Count} tài khoản hợp lệ và {invalidLines.Count} dòng lỗi.\n\n" +
                        "Nhấn YES để thêm tất cả (kể cả đã tồn tại).\n" +
                        "Nhấn NO để chỉ thêm nick mới (chưa trùng user + pass + server).",
                        "Xác nhận import",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );

                    if (choice == DialogResult.Cancel)
                        return;

                    bool addAll = (choice == DialogResult.Yes);

                    var accountsToAdd = addAll
                        ? allAccounts
                        : allAccounts.Where(a => !existingAccounts.Contains($"{a.user}|{a.pass}|{a.server}")).ToList();

                    if (accountsToAdd.Count == 0)
                    {
                        MessageBox.Show("Không có tài khoản mới để thêm.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    int totalAdded = 0;
                    int totalUpdated = 0;

                    foreach (var acc in accountsToAdd)
                    {
                        string key = $"{acc.user}|{acc.pass}|{acc.server}";

                        if (existingAccounts.Contains(key))
                        {
                            if (addAll)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    string user = row.Cells[1].Value?.ToString().Trim();
                                    string server = row.Cells[2].Value?.ToString().Trim();
                                    string pass = CryptoManager.Decryptor(row.Cells[3].Value?.ToString().Trim() ?? "", "ud");

                                    if ($"{user}|{pass}|{server}".Equals(key, StringComparison.OrdinalIgnoreCase))
                                    {
                                        totalUpdated++;
                                        break;
                                    }
                                }
                            }
                            continue;
                        }

                        string proxyType = "1";
                        string note = "Import File";

                        var rowIndex = dataGridView1.Rows.Add(new object[]
                        {
                            indexSTT,
                            acc.user,
                            LoadData.server(acc.server),
                            CryptoManager.Encryptor(acc.pass, "ud"),
                            note,
                            "",
                            proxyType
                        });

                        var newRow = dataGridView1.Rows[rowIndex];
                        newRow.Height = 40;

                        newRow.Tag = new RowOriginalData
                        {
                            TaiKhoan = acc.user,
                            GhiChu = note,
                            BackColor = newRow.DefaultCellStyle.BackColor,
                            ForeColor = newRow.DefaultCellStyle.ForeColor,
                            SelectionBackColor = newRow.DefaultCellStyle.SelectionBackColor,
                            SelectionForeColor = newRow.DefaultCellStyle.SelectionForeColor
                        };

                        existingAccounts.Add(key);
                        indexSTT++;
                        totalAdded++;
                    }

                    LoadData.ghifile(dataGridView1);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"✅ Đã thêm {totalAdded} tài khoản mới.");
                    if (addAll)
                        sb.AppendLine($"🔁 Cập nhật {totalUpdated} tài khoản trùng (giữ nguyên dữ liệu).");

                    if (invalidLines.Count > 0)
                    {
                        sb.AppendLine($"\n⚠ Có {invalidLines.Count} dòng không hợp lệ (không đúng định dạng):");
                        foreach (string invalid in invalidLines.Take(10))
                            sb.AppendLine(" - " + invalid);
                        if (invalidLines.Count > 10)
                            sb.AppendLine($"... và {invalidLines.Count - 10} dòng nữa.");
                    }

                    MessageBox.Show(sb.ToString(), "Hoàn tất import", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import file: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
