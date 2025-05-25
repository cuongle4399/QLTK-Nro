namespace QLTK_Nro_Pro
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            button2 = new Button();
            label3 = new Label();
            button3 = new Button();
            label4 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.LightCoral;
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(58, 42);
            button1.TabIndex = 0;
            button1.Text = "Sai";
            button1.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(76, 12);
            label1.Name = "label1";
            label1.Size = new Size(149, 42);
            label1.TabIndex = 1;
            label1.Text = "Trạng thái proxy sai định dạng";
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(76, 74);
            label2.Name = "label2";
            label2.Size = new Size(420, 64);
            label2.TabIndex = 3;
            label2.Text = "Proxy đúng định dạng nhưng cũng có khả năng đã hết hạn. Muốn check thì bên mục KHÁC có chức năng check proxy hoặc vào dc game tức là proxy ok";
            // 
            // button2
            // 
            button2.BackColor = Color.MediumSeaGreen;
            button2.Location = new Point(12, 74);
            button2.Name = "button2";
            button2.Size = new Size(58, 42);
            button2.TabIndex = 2;
            button2.Text = "OK";
            button2.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(295, 12);
            label3.Name = "label3";
            label3.Size = new Size(186, 42);
            label3.TabIndex = 5;
            label3.Text = "Trạng thái chưa lắp proxy";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ButtonFace;
            button3.Location = new Point(232, 12);
            button3.Name = "button3";
            button3.Size = new Size(57, 42);
            button3.TabIndex = 4;
            button3.Text = "None";
            button3.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label4.Location = new Point(15, 149);
            label4.Name = "label4";
            label4.Size = new Size(466, 122);
            label4.TabIndex = 6;
            label4.Text = resources.GetString("label4.Text");
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 278);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button3);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(button1);
            MaximizeBox = false;
            Name = "Form2";
            Text = "Hướng dẫn lắp proxy vào game";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private Button button2;
        private Label label3;
        private Button button3;
        private Label label4;
    }
}