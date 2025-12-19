using System;

namespace DoHoa.CustomMenu.Shared;

public static class MenuHelper
{
	public static int PanelWidth { get; set; }

	public static int PanelHeight => GameCanvas.h - 2;

	public static int PanelX => 1;

	public static int PanelY => 1;

	public static int Rows { get; set; }

	public static int ContentHeight { get; set; }

	public static int currentTab { get; set; }

	public static void UpdateCachedValues(int currentTab, int totalItems, int buttonPanelHeight)
	{
		PanelWidth = GameCanvas.w * 38 / 100;
		PanelWidth = System.Math.Max(136, PanelWidth);
		ContentHeight = PanelHeight - 30 - 8 - 8 - buttonPanelHeight;
		Rows = ContentHeight / 32;
		Rows = System.Math.Max(1, Rows);
	}

	public static int CalculateMaxScrollOffset(int totalItems)
	{
		return System.Math.Max(0, totalItems - Rows);
	}

	public static void DrawScrollBar(mGraphics g, int panelX, int contentY, int totalItems, int currentScrollOffset, int maxScrollOffset)
	{
		if (maxScrollOffset > 0 && totalItems > Rows)
		{
			int x = panelX + PanelWidth - 4;
			int contentHeight = ContentHeight;
			g.setColor(5066061);
			g.fillRect(x, contentY, 2, contentHeight);
			int val = contentHeight * Rows / totalItems;
			val = System.Math.Max(10, val);
			int num = contentHeight - val;
			int y = contentY + num * currentScrollOffset / maxScrollOffset;
			g.setColor(49151);
			g.fillRect(x, y, 2, val);
		}
	}

	public static string NormalizeSkillName(string name)
	{
		bool flag = false;
		string result;
		switch (name)
		{
		case "Chiêu Kamejoko":
			result = "Kamejoko";
			break;
		case "Chiêu đấm Dragon":
		case "Chiêu đấm Demon":
		case "Chiêu đấm Galick":
			result = "Đấm";
			break;
		case "Chiêu Masenko":
			result = "Masenko";
			break;
		case "Chiêu Antomic":
			result = "Antomic";
			break;
		case "Thái Dương Hạ San":
			result = "TDHS";
			break;
		case "Tái tạo năng lượng":
			result = "Tái tạo";
			break;
		case "Quả cầu kênh khí":
			result = "Kênh khí";
			break;
		case "Makankosappo":
			result = "Laze";
			break;
		case "Đẻ trứng":
			result = "Trứng";
			break;
		case "Biến hình":
			result = "Khỉ";
			break;
		case "Tự phát nổ":
			result = "Boom";
			break;
		case "Biến Sôcôla":
			result = "Sôcôla";
			break;
		case "Dịch chuyển tức thời":
			result = "Dịch chuyển";
			break;
		case "Khiên năng lượng":
			result = "Khiên";
			break;
		case "Cađíc liên hoàn chưởng":
			result = "Cađíc LH";
			break;
		default:
			result = name;
			break;
		}
		bool flag2 = false;
		return result;
	}
}
