using QLTK_Nro_Pro.Presenter.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTK_Nro_Pro.Presenter
{
    internal class LoadData
    {
        #region Load/Save data
        public static void ghifile(DataGridView gridView)
        {
            if (gridView.RowCount == 0)
            {
                File.WriteAllText(AppConstants.PathData, string.Empty); 
                return;
            }

            List<string> lines = new List<string>();

            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                var row = gridView.Rows[i];
                if (row.IsNewRow) continue;
                string taiKhoan = row.Cells[1].Value?.ToString();
                string ghiChu = row.Cells[4].Value?.ToString();

                if (row.Tag is RowOriginalData original)
                {
                    if (original.TaiKhoan != null) taiKhoan = original.TaiKhoan.ToString();
                    if (original.GhiChu != null) ghiChu = original.GhiChu.ToString();
                }

                string line =
                    i + "|" +
                    taiKhoan + "|" +              
                    row.Cells[2].Value + "|" +  
                    row.Cells[3].Value + "|" + 
                    ghiChu + "|" +               
                    row.Cells[5].Value + "|" +    
                    row.Cells[6].Value;       

                lines.Add(line);
            }

            File.WriteAllLines(AppConstants.PathData, lines);
        }



        public static void docFile(ref int indexSTT, DataGridView data)
        {
            if (!File.Exists(AppConstants.PathData))
            {
                File.Create(AppConstants.PathData).Close();
            }

            try
            {
                data.Rows.Clear();
                indexSTT = 0;
                string[] a = File.ReadAllLines(AppConstants.PathData);

                foreach (string line in a)
                {
                    string[] b = line.Split('|');
                    if (b.Length < 7) continue;

                    int rowIndex = data.Rows.Add(new object[]
                    {
                b[0], b[1], b[2], b[3], b[4], b[5], b[6]
                    });

                    var row = data.Rows[rowIndex];
                    row.Height = 40;

                    row.Tag = new RowOriginalData
                    {
                        TaiKhoan = b[1],
                        GhiChu = b[4],
                        BackColor = row.DefaultCellStyle.BackColor,
                        ForeColor = row.DefaultCellStyle.ForeColor,
                        SelectionBackColor = row.DefaultCellStyle.SelectionBackColor,
                        SelectionForeColor = row.DefaultCellStyle.SelectionForeColor
                    };

                    indexSTT++;
                }
            }
            catch
            {
                if (MessageBox.Show("Dữ liệu bị lỗi. Bạn có muốn reset dữ liệu không?",
                                    "Thông báo",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    File.Delete(AppConstants.PathData);
                    data.Rows.Clear();
                }
                else
                {
                    Application.ExitThread();
                    Environment.Exit(0);
                }
            }
        }


        #endregion
        #region DeleteDataGame

        public static void DeleteDataGame()
        {
            try
            {
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "AppData", "LocalLow", "Team", "ragonboy244"
                );

                if (Directory.Exists(path))
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        File.Delete(file);
                    }
                    MessageBox.Show("Đã xóa dữ liệu game\nMở game lại để tải dữ liệu game", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi: ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region FormatServer
        public static string Reserver(int x)
        {
            return ServerManager.GetServerName(x);
        }
        public static int server(string x)
        {
            return ServerManager.GetServerId(x);
        }
        #endregion
        #region createFolderData
        public static void createFolderData()
        {
            if (!Directory.Exists(Path.Combine(Path.GetTempPath(), "cuongle4399", "mod 244")))
            {
                Directory.CreateDirectory((Path.Combine(Path.GetTempPath(), "cuongle4399", "mod 244")));
                if (!File.Exists(AppConstants.PathData))
                {
                    File.Create(AppConstants.PathData).Close();
                }
            }
            string folderPath = Path.GetDirectoryName(AppConstants.PathAPI);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!File.Exists(AppConstants.PathAPI))
            {
                File.WriteAllText(AppConstants.PathAPI, "");
            }
            string folderPathServerAPI = Path.GetDirectoryName(AppConstants.PathServerAPI);
            if (!string.IsNullOrEmpty(folderPathServerAPI) && !Directory.Exists(folderPathServerAPI))
            {
                Directory.CreateDirectory(folderPathServerAPI);
            }

            if (!File.Exists(AppConstants.PathServerAPI))
            {
                File.WriteAllText(AppConstants.PathServerAPI, "");
            }

            ServerManager.Init();
        }
        #endregion
    }
}
