using DoHoa;
using main.Mod;
using Mod_nro.MenuDataGame;
using Xmap;

namespace Mod.CuongLe;

public class MenuGiaoDien
{
	public static string[] menuMod = new string[12]
	{
		"Data Item", "logoPixel Game", "Background", "Thông Báo Boss", "Danh sách nhân vật", "Địa hình lưới", "Danh sách SKH", "Thông tin up vàng", "Auto Giải Capcha", "Thông tin up đệ",
		"Paint Hành trang CPU nhẹ", "Xmap skip text NPC sự kiện"
	};

	public static bool[] getArrMod()
	{
		return new bool[12]
		{
			DataItem.IsShow,
			GraphicsManagement.HienThiLogo,
			GraphicsManagement.HienThiBackground,
			GraphicsManagement.isHuntingBoss,
			GraphicsManagement.isShowCharsInMap,
			GraphicsManagement.MapLuoi,
			ModProCL.hienThiDoKH,
			MainMod.infoTrainGold,
			MainMod.AutoCapCha,
			ModProCL.petw,
			GraphicsManagement.paintInventoryReduceCPU,
			NextMap.nextSuKien
		};
	}
}
