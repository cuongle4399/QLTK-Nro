namespace QLTK_Nro_Pro
{
    partial class ImportDataNickFlash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportDataNickFlash));
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            txtPass = new TextBox();
            materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            importNickFile = new TabControl();
            tabPage2 = new TabPage();
            label2 = new Label();
            btnImportFileData = new MaterialSkin.Controls.MaterialButton();
            importLoNick = new TabPage();
            label1 = new Label();
            materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            txtAccount = new TextBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            importNickFile.SuspendLayout();
            tabPage2.SuspendLayout();
            importLoNick.SuspendLayout();
            SuspendLayout();
            // 
            // numericUpDown1
            // 
            numericUpDown1.Enabled = false;
            numericUpDown1.Location = new Point(189, 101);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(98, 27);
            numericUpDown1.TabIndex = 0;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Enabled = false;
            numericUpDown2.Location = new Point(343, 101);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(86, 27);
            numericUpDown2.TabIndex = 1;
            // 
            // materialLabel1
            // 
            materialLabel1.AutoSize = true;
            materialLabel1.Depth = 0;
            materialLabel1.Enabled = false;
            materialLabel1.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel1.ForeColor = SystemColors.ActiveCaptionText;
            materialLabel1.Location = new Point(15, 109);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new Size(145, 19);
            materialLabel1.TabIndex = 2;
            materialLabel1.Text = "Nhập Nick theo lô từ";
            // 
            // txtPass
            // 
            txtPass.Enabled = false;
            txtPass.Location = new Point(137, 61);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(292, 27);
            txtPass.TabIndex = 3;
            // 
            // materialLabel2
            // 
            materialLabel2.AutoSize = true;
            materialLabel2.Depth = 0;
            materialLabel2.Enabled = false;
            materialLabel2.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel2.ForeColor = SystemColors.ActiveCaptionText;
            materialLabel2.Location = new Point(15, 67);
            materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel2.Name = "materialLabel2";
            materialLabel2.Size = new Size(70, 19);
            materialLabel2.TabIndex = 4;
            materialLabel2.Text = "Mật Khẩu";
            // 
            // importNickFile
            // 
            importNickFile.Controls.Add(tabPage2);
            importNickFile.Controls.Add(importLoNick);
            importNickFile.Location = new Point(12, 12);
            importNickFile.Name = "importNickFile";
            importNickFile.SelectedIndex = 0;
            importNickFile.Size = new Size(448, 202);
            importNickFile.TabIndex = 5;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.DarkGray;
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(btnImportFileData);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(440, 169);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Import nick bằng file";
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(0, 0, 192);
            label2.Location = new Point(6, 103);
            label2.Name = "label2";
            label2.Size = new Size(428, 63);
            label2.TabIndex = 1;
            label2.Text = "Nội dung File .txt phải có định dạng : \"Tài khoản | Mật khẩu |server\" mỗi dòng 1 nick";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnImportFileData
            // 
            btnImportFileData.AutoSize = false;
            btnImportFileData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnImportFileData.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            btnImportFileData.Depth = 0;
            btnImportFileData.HighEmphasis = true;
            btnImportFileData.Icon = null;
            btnImportFileData.Location = new Point(118, 52);
            btnImportFileData.Margin = new Padding(4, 6, 4, 6);
            btnImportFileData.MouseState = MaterialSkin.MouseState.HOVER;
            btnImportFileData.Name = "btnImportFileData";
            btnImportFileData.NoAccentTextColor = Color.Empty;
            btnImportFileData.Size = new Size(198, 45);
            btnImportFileData.TabIndex = 0;
            btnImportFileData.Text = "Nhập File nick";
            btnImportFileData.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            btnImportFileData.UseAccentColor = false;
            btnImportFileData.UseVisualStyleBackColor = true;
            btnImportFileData.Click += btnImportFileData_Click;
            // 
            // importLoNick
            // 
            importLoNick.BackColor = Color.DarkGray;
            importLoNick.Controls.Add(label1);
            importLoNick.Controls.Add(materialLabel3);
            importLoNick.Controls.Add(txtAccount);
            importLoNick.Controls.Add(materialLabel1);
            importLoNick.Controls.Add(materialLabel2);
            importLoNick.Controls.Add(numericUpDown1);
            importLoNick.Controls.Add(txtPass);
            importLoNick.Controls.Add(numericUpDown2);
            importLoNick.Location = new Point(4, 29);
            importLoNick.Name = "importLoNick";
            importLoNick.Padding = new Padding(3);
            importLoNick.Size = new Size(440, 169);
            importLoNick.TabIndex = 0;
            importLoNick.Text = "Nhập nick theo lô";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(114, 141);
            label1.Name = "label1";
            label1.Size = new Size(211, 20);
            label1.TabIndex = 7;
            label1.Text = "Tạm bảo trì nào rảnh code sau";
            // 
            // materialLabel3
            // 
            materialLabel3.AutoSize = true;
            materialLabel3.Depth = 0;
            materialLabel3.Enabled = false;
            materialLabel3.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel3.ForeColor = SystemColors.ActiveCaptionText;
            materialLabel3.Location = new Point(15, 15);
            materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel3.Name = "materialLabel3";
            materialLabel3.Size = new Size(74, 19);
            materialLabel3.TabIndex = 6;
            materialLabel3.Text = "Tài Khoản";
            // 
            // txtAccount
            // 
            txtAccount.Enabled = false;
            txtAccount.Location = new Point(137, 15);
            txtAccount.Name = "txtAccount";
            txtAccount.Size = new Size(292, 27);
            txtAccount.TabIndex = 5;
            // 
            // ImportDataNickFlash
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(464, 217);
            Controls.Add(importNickFile);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ImportDataNickFlash";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nạp dữ liệu QLTK nhanh";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            importNickFile.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            importLoNick.ResumeLayout(false);
            importLoNick.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private TextBox txtPass;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private TabControl importNickFile;
        private TabPage importLoNick;
        private TextBox txtAccount;
        private TabPage tabPage2;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialButton btnImportFileData;
        private Label label1;
        private Label label2;
    }
}