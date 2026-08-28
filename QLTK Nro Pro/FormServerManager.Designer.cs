namespace QLTK_Nro_Pro
{
    partial class FormServerManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvServers = new DataGridView();
            colSTT = new DataGridViewTextBoxColumn();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            grpInput = new GroupBox();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAdd = new Button();
            txtName = new TextBox();
            lblName = new Label();
            txtId = new TextBox();
            lblId = new Label();
            btnResetDefault = new Button();
            btnOpenFile = new Button();
            btnSaveAndClose = new Button();
            btnClose = new Button();
            lblHeader = new Label();
            lblSubHeader = new Label();
            lblCount = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvServers).BeginInit();
            grpInput.SuspendLayout();
            SuspendLayout();
            // 
            // dgvServers
            // 
            dgvServers.AllowUserToAddRows = false;
            dgvServers.AllowUserToDeleteRows = false;
            dgvServers.AllowUserToResizeRows = false;
            dgvServers.BackgroundColor = Color.White;
            dgvServers.BorderStyle = BorderStyle.Fixed3D;
            dgvServers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvServers.Columns.AddRange(new DataGridViewColumn[] { colSTT, colId, colName });
            dgvServers.Location = new Point(14, 60);
            dgvServers.MultiSelect = false;
            dgvServers.Name = "dgvServers";
            dgvServers.ReadOnly = true;
            dgvServers.RowHeadersVisible = false;
            dgvServers.RowHeadersWidth = 51;
            dgvServers.RowTemplate.Height = 28;
            dgvServers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvServers.Size = new Size(350, 360);
            dgvServers.TabIndex = 0;
            dgvServers.SelectionChanged += dgvServers_SelectionChanged;
            // 
            // colSTT
            // 
            colSTT.HeaderText = "STT";
            colSTT.MinimumWidth = 6;
            colSTT.Name = "colSTT";
            colSTT.ReadOnly = true;
            colSTT.Width = 45;
            // 
            // colId
            // 
            colId.HeaderText = "ID Server";
            colId.MinimumWidth = 6;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 85;
            // 
            // colName
            // 
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.HeaderText = "Tên Hiển Thị";
            colName.MinimumWidth = 6;
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // grpInput
            // 
            grpInput.Controls.Add(btnDelete);
            grpInput.Controls.Add(btnEdit);
            grpInput.Controls.Add(btnAdd);
            grpInput.Controls.Add(txtName);
            grpInput.Controls.Add(lblName);
            grpInput.Controls.Add(txtId);
            grpInput.Controls.Add(lblId);
            grpInput.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpInput.Location = new Point(374, 53);
            grpInput.Name = "grpInput";
            grpInput.Size = new Size(220, 245);
            grpInput.TabIndex = 1;
            grpInput.TabStop = false;
            grpInput.Text = "Thông tin Server";
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = Color.DarkRed;
            btnDelete.Location = new Point(14, 203);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(192, 32);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "🗑️ Xóa Server";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnEdit.Location = new Point(114, 163);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(92, 32);
            btnEdit.TabIndex = 5;
            btnEdit.Text = "✏️ Sửa";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(14, 163);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(92, 32);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "➕ Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtName
            // 
            txtName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtName.Location = new Point(14, 118);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "vd: Vũ Trụ 16 [23]";
            txtName.Size = new Size(192, 27);
            txtName.TabIndex = 3;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblName.Location = new Point(14, 93);
            lblName.Name = "lblName";
            lblName.Size = new Size(82, 20);
            lblName.TabIndex = 2;
            lblName.Text = "Tên Server:";
            // 
            // txtId
            // 
            txtId.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtId.Location = new Point(14, 53);
            txtId.Name = "txtId";
            txtId.PlaceholderText = "vd: 23";
            txtId.Size = new Size(192, 27);
            txtId.TabIndex = 1;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblId.Location = new Point(14, 28);
            lblId.Name = "lblId";
            lblId.Size = new Size(71, 20);
            lblId.TabIndex = 0;
            lblId.Text = "ID Server:";
            // 
            // btnResetDefault
            // 
            btnResetDefault.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResetDefault.Location = new Point(374, 305);
            btnResetDefault.Name = "btnResetDefault";
            btnResetDefault.Size = new Size(220, 32);
            btnResetDefault.TabIndex = 7;
            btnResetDefault.Text = "🔄 Khôi phục mặc định";
            btnResetDefault.UseVisualStyleBackColor = true;
            btnResetDefault.Click += btnResetDefault_Click;
            // 
            // btnOpenFile
            // 
            btnOpenFile.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnOpenFile.Location = new Point(374, 343);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(220, 32);
            btnOpenFile.TabIndex = 8;
            btnOpenFile.Text = "📂 Mở file Data/servers.ini";
            btnOpenFile.UseVisualStyleBackColor = true;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // btnSaveAndClose
            // 
            btnSaveAndClose.BackColor = Color.FromArgb(41, 128, 185);
            btnSaveAndClose.FlatStyle = FlatStyle.Flat;
            btnSaveAndClose.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveAndClose.ForeColor = Color.White;
            btnSaveAndClose.Location = new Point(374, 381);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(130, 38);
            btnSaveAndClose.TabIndex = 9;
            btnSaveAndClose.Text = "💾 Lưu thay đổi";
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // btnClose
            // 
            btnClose.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(510, 381);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(84, 38);
            btnClose.TabIndex = 10;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = true;
            lblHeader.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHeader.ForeColor = Color.FromArgb(33, 33, 33);
            lblHeader.Location = new Point(14, 9);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(276, 28);
            lblHeader.TabIndex = 11;
            lblHeader.Text = "⚙️ Quản lý danh sách Server";
            // 
            // lblSubHeader
            // 
            lblSubHeader.AutoSize = true;
            lblSubHeader.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubHeader.ForeColor = Color.Gray;
            lblSubHeader.Location = new Point(14, 35);
            lblSubHeader.Name = "lblSubHeader";
            lblSubHeader.Size = new Size(371, 19);
            lblSubHeader.TabIndex = 12;
            lblSubHeader.Text = "Định dạng file: idserver|tên server (Tự động đồng bộ QLTK)";
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblCount.ForeColor = Color.DimGray;
            lblCount.Location = new Point(14, 425);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(111, 19);
            lblCount.TabIndex = 13;
            lblCount.Text = "Tổng số: 0 server";
            // 
            // FormServerManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(612, 452);
            Controls.Add(lblCount);
            Controls.Add(lblSubHeader);
            Controls.Add(lblHeader);
            Controls.Add(btnClose);
            Controls.Add(btnSaveAndClose);
            Controls.Add(btnOpenFile);
            Controls.Add(btnResetDefault);
            Controls.Add(grpInput);
            Controls.Add(dgvServers);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormServerManager";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý danh sách Server";
            Load += FormServerManager_Load;
            ((System.ComponentModel.ISupportInitialize)dgvServers).EndInit();
            grpInput.ResumeLayout(false);
            grpInput.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvServers;
        private DataGridViewTextBoxColumn colSTT;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private GroupBox grpInput;
        private Label lblId;
        private TextBox txtId;
        private Label lblName;
        private TextBox txtName;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnResetDefault;
        private Button btnOpenFile;
        private Button btnSaveAndClose;
        private Button btnClose;
        private Label lblHeader;
        private Label lblSubHeader;
        private Label lblCount;
    }
}
