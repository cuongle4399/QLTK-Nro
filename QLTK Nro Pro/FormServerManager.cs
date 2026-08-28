using QLTK_Nro_Pro.Presenter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QLTK_Nro_Pro
{
    public partial class FormServerManager : Form
    {
        private List<ServerItem> _serverList = new List<ServerItem>();

        public FormServerManager()
        {
            InitializeComponent();
        }

        private void FormServerManager_Load(object sender, EventArgs e)
        {
            ServerManager.LoadServers();
            _serverList = ServerManager.Servers.Select(s => new ServerItem(s.Id, s.Name)).ToList();
            RefreshGrid();
        }

        private void RefreshGrid(int selectId = -1)
        {
            dgvServers.Rows.Clear();

            for (int i = 0; i < _serverList.Count; i++)
            {
                var s = _serverList[i];
                int rowIndex = dgvServers.Rows.Add(new object[] { i + 1, s.Id, s.Name });
                dgvServers.Rows[rowIndex].Tag = s;

                if (selectId != -1 && s.Id == selectId)
                {
                    dgvServers.ClearSelection();
                    dgvServers.Rows[rowIndex].Selected = true;
                }
            }

            lblCount.Text = $"Tổng số: {_serverList.Count} server";

            if (selectId == -1 && dgvServers.Rows.Count > 0 && dgvServers.SelectedRows.Count == 0)
            {
                dgvServers.Rows[0].Selected = true;
            }
        }

        private void dgvServers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvServers.SelectedRows.Count > 0)
            {
                var row = dgvServers.SelectedRows[0];
                if (row.Tag is ServerItem item)
                {
                    txtId.Text = item.Id.ToString();
                    txtName.Text = item.Name;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text.Trim(), out int id) || id <= 0)
            {
                MessageBox.Show("Vui lòng nhập ID Server là một số nguyên dương hợp lệ (vd: 23)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus();
                return;
            }

            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên Server hiển thị (vd: Vũ Trụ 16 [23])!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            var existing = _serverList.FirstOrDefault(s => s.Id == id);
            if (existing != null)
            {
                var result = MessageBox.Show(
                    $"ID Server [{id}] đã tồn tại với tên: \"{existing.Name}\".\nBạn có muốn cập nhật tên thành \"{name}\" không?",
                    "Trùng ID Server",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    existing.Name = name;
                    RefreshGrid(id);
                }
                return;
            }

            _serverList.Add(new ServerItem(id, name));
            RefreshGrid(id);
            txtId.Clear();
            txtName.Clear();
            txtId.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvServers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn 1 server trong danh sách để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(txtId.Text.Trim(), out int newId) || newId <= 0)
            {
                MessageBox.Show("Vui lòng nhập ID Server là một số nguyên dương hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus();
                return;
            }

            string newName = txtName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Vui lòng nhập tên Server hiển thị!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            var selectedRow = dgvServers.SelectedRows[0];
            if (selectedRow.Tag is ServerItem currentItem)
            {
                // If changing ID, check if new ID already exists on another item
                if (currentItem.Id != newId && _serverList.Any(s => s.Id == newId))
                {
                    MessageBox.Show($"ID Server [{newId}] đã được sử dụng bởi server khác!", "Trùng ID Server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                currentItem.Id = newId;
                currentItem.Name = newName;
                RefreshGrid(newId);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvServers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn 1 server trong danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvServers.SelectedRows[0];
            if (selectedRow.Tag is ServerItem item)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa Server [{item.Id}] \"{item.Name}\" không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    _serverList.Remove(item);
                    RefreshGrid();
                    txtId.Clear();
                    txtName.Clear();
                }
            }
        }

        private void btnResetDefault_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc muốn khôi phục danh sách về 22 server mặc định ban đầu?",
                "Khôi phục mặc định",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                _serverList = ServerManager.GetDefaultServers();
                RefreshGrid();
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                string path = AppConstants.PathServers;
                if (!File.Exists(path))
                {
                    ServerManager.SaveServers(_serverList);
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở file cấu hình: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            if (_serverList.Count == 0)
            {
                MessageBox.Show("Danh sách server không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ServerManager.SaveServers(_serverList);
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
