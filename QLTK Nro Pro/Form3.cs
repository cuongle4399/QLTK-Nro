using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace QLTK_Nro_Pro
{
    public partial class frmCapcha : Form
    {
        public frmCapcha()
        {
            InitializeComponent();
        }

        private void ShowHuongDan()
        {
            RichTextBox.Text =
@"📘 HƯỚNG DẪN SỬ DỤNG AUTO GIẢI CAPTCHA (chỉ hoạt động khi mở game bằng SIZE PIXEL
còn SIZE 2D không giải Capcha được đâu)

🔹 B1: Ấn vào nút 🔑 *Mua Key Captcha* nếu chưa có để mở Shop A Phạm Giang:
    👉 https://api.phamgiang.net/

🔹 B2: Sau khi mua thành công, bạn sẽ nhận được một key LICENSE.
    → Hãy copy key này và dán vào phần mềm QLTK → nhấn nút 💾 *Lưu*.

🔹 B3: Mở một tab game bất kỳ muốn dùng Giải Captcha:
    → Ấn phím F1 để mở Hành Trang
    → Chọn Tab *Giao Diện* trong menu bên phải
    → Nhấn nút ⚙️ *Mở Auto Giải Captcha* để bật chức năng.

✅ Hệ thống sẽ tự động giải Captcha mỗi khi xuất hiện!";
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ShowHuongDan();
            RichTextBox.SelectionStart = 0;
            RichTextBox.ScrollToCaret();
        }

        private void RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.LinkText,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở liên kết: " + ex.Message);
            }
        }
    }
}
