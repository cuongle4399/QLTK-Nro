using UnityEngine;

namespace DoHoa
{
    /// <summary>
    /// Lớp hỗ trợ chuyển đổi hình ảnh sang đồ họa pixel (pixel art) cho game Nro.
    /// </summary>
    public class PixelGraphics
    {
        // Trạng thái bật/tắt của chế độ đồ họa pixel
        public static bool isEnabled = false;

        // Tỷ lệ pixel (ví dụ: 2 nghĩa là gộp 2x2 pixel gốc thành 1 pixel lớn)
        public static int pixelScale = 2;

        /// <summary>
        /// Áp dụng hiệu ứng pixel cho một đối tượng Image.
        /// Thường được gọi sau khi hình ảnh được tải từ server.
        /// </summary>
        /// <param name="img">Đối tượng hình ảnh cần xử lý</param>
        public static void Apply(Image img)
        {
            if (img == null || img.texture == null)
            {
                return;
            }

            try
            {
                Texture2D tex = img.texture;
                int width = tex.width;
                int height = tex.height;

                // Nếu hình ảnh quá nhỏ thì không xử lý
                if (width < pixelScale || height < pixelScale)
                {
                    return;
                }

                Texture2D newTex = new Texture2D(width, height, tex.format, false);
                newTex.filterMode = FilterMode.Point;
                newTex.anisoLevel = 0;

                for (int y = 0; y < height; y += pixelScale)
                {
                    for (int x = 0; x < width; x += pixelScale)
                    {
                        // Lấy màu mẫu tại điểm ảnh gốc
                        Color sampleColor = tex.GetPixel(x, y);

                        // Đổ màu mẫu vào khối pixelScale x pixelScale
                        for (int dy = 0; dy < pixelScale && (y + dy) < height; dy++)
                        {
                            for (int dx = 0; dx < pixelScale && (x + dx) < width; dx++)
                            {
                                newTex.SetPixel(x + dx, y + dy, sampleColor);
                            }
                        }
                    }
                }

                newTex.Apply();
                
                // Gán texture mới đã được pixel hóa
                img.texture = newTex;
                
                // Đảm bảo chất lượng hiển thị sắc nét (Point filtering)
                Image.setTextureQuality(img.texture);
            }
            catch (System.Exception ex)
            {
                Cout.LogError("Lỗi PixelGraphics.Apply: " + ex.Message);
            }
        }

        /// <summary>
        /// Phương thức hỗ trợ xử lý toàn bộ các ảnh đã tải (nếu cần thiết)
        /// </summary>
        public static void ProcessAllIcons()
        {
            if (SmallImage.imgNew == null) return;

            for (int i = 0; i < SmallImage.imgNew.Length; i++)
            {
                if (SmallImage.imgNew[i] != null && SmallImage.imgNew[i].img != null)
                {
                    Apply(SmallImage.imgNew[i].img);
                }
            }
        }
    }
}
