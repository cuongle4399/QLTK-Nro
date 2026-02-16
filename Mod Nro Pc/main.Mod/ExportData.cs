using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DoHoa.CustomMenu;

namespace ModCak.main.Mod
{
    internal class ExportData
    {
        public static void ExportItemsToFile()
        {
            try
            {
                string filePath = @"C:\Users\Cuong Le\Desktop\Data Game\Item.txt";

                // Tạo thư mục nếu chưa tồn tại
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                StringBuilder sb = new StringBuilder();

                // Thêm từng item, mỗi item một dòng
                foreach (var item in AutoItemTab.AutoItems)
                {
                    sb.AppendLine($"{item.Id}. {item.Name}");
                }

                // Ghi file
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

                Console.WriteLine($"Xuất file thành công tại: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xuất file: {ex.Message}");
            }
        }
    }
}