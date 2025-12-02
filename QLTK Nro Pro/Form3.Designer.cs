namespace QLTK_Nro_Pro
{
    partial class frmCapcha
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCapcha));
            RichTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // RichTextBox
            // 
            RichTextBox.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            RichTextBox.Location = new Point(8, 12);
            RichTextBox.Name = "RichTextBox";
            RichTextBox.ReadOnly = true;
            RichTextBox.Size = new Size(516, 258);
            RichTextBox.TabIndex = 0;
            RichTextBox.Text = "";
            RichTextBox.LinkClicked += RichTextBox_LinkClicked;
            // 
            // frmCapcha
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(539, 282);
            Controls.Add(RichTextBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmCapcha";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Auto Captcha";
            Load += Form3_Load;
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox RichTextBox;
    }
}