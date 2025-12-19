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
        public static string PathFolderData = Path.Combine(Path.GetTempPath(), "cuongle4399", "mod 244");
        public static string PathData = Path.Combine(PathFolderData, "data");
        public static void ghifile(DataGridView gridView)
        {
            if (gridView.RowCount == 0)
            {
                File.WriteAllText(PathData, string.Empty); 
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

            File.WriteAllLines(PathData, lines);
        }



        public static void docFile(ref int indexSTT, DataGridView data)
        {
            if (!File.Exists(PathData))
            {
                File.Create(PathData).Close();
            }

            try
            {
                data.Rows.Clear();
                indexSTT = 0;
                string[] a = File.ReadAllLines(PathData);

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
                    File.Delete(PathData);
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
            if (x == 13)
            {
                return "Võ đài liên vũ trụ[13]";
            }
            if (x == 14)
            {
                return "Universe1 (14)";
            }
            if (x == 15)
            {
                return "Naga [15]";
            }
            if (x == 16)
            {
                return "Super 1 [16]";
            }
            if (x == 17)
            {
                return "Super 2 [17]";
            }
            if (x == 18)
            {
                return "13 [18]";
            }
            if (x == 19)
            {
                return "VIP 2 [19]";
            }
            if (x == 20)
            {
                return "14 [20]";
            }
            if (x == 21)
            {
                return "Super 3 [21]";
            }
            return x.ToString();
        }
        public static int server(string x)
        {
            if (x.Equals("Võ đài liên vũ trụ [13]"))
            {
                return 13;
            }
            if (x.Equals("Universe1 (14)"))
            {
                return 14;
            }
            if (x.Equals("Naga [15]"))
            {
                return 15;
            }
            if (x.Equals("Super 1 [16]"))
            {
                return 16;
            }
            if (x.Equals("Super 2 [17]"))
            {
                return 17;
            }
            if (x.Equals("13 [18]"))
            {
                return 18;
            }
            if (x.Equals("VIP 2 [19]"))
            {
                return 19;
            }
            if (x.Equals("14 [20]"))
            {
                return 20;
            }
            if (x.Equals("Super 3 [21]"))
            {
                return 21;
            }
            return int.Parse(x);
        }
        #endregion
        #region createFolderData
        public static string PathAPI = Path.Combine("Data", "keyAPI.ini");
        public static string PathServerAPI = Path.Combine("Data", "serverAPI.ini");
        public static void createFolderData()
        {
            if (!Directory.Exists(Path.Combine(Path.GetTempPath(), "cuongle4399", "mod 244")))
            {
                Directory.CreateDirectory((Path.Combine(Path.GetTempPath(), "cuongle4399", "mod 244")));
                if (!File.Exists(PathData))
                {
                    File.Create(PathData).Close();
                }
            }
            string folderPath = Path.GetDirectoryName(PathAPI);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!File.Exists(PathServerAPI))
            {
                File.WriteAllText(PathAPI, "");
            }
            string folderPathServerAPI = Path.GetDirectoryName(PathServerAPI);
            if (!Directory.Exists(folderPathServerAPI))
            {
                Directory.CreateDirectory(folderPathServerAPI);
            }

            if (!File.Exists(PathServerAPI))
            {
                File.WriteAllText(PathServerAPI, "");
            }
        }
        #endregion
    }
}
