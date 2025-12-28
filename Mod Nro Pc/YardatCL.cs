using Mod.community;
using Mod.CuongLe;
using System.Collections.Generic;
using System.Linq;

public class YardatCL : IActionListener
{
	private static YardatCL _Instance;

	public static bool autoPotara;

	public static bool petGoHome;

	public static bool fakeNhapThe;

	private static List<Char> listTargetAutoChangeFocus = new List<Char>();

	private static long lastPotaraTime;

	public static bool isAutoChangeFocus;

	private static long cooldownAutoChangeFocus;

	private static long lastTimeChangeFocus;

	private static int targetIndex;

	public static YardatCL getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new YardatCL();
		}
		return _Instance;
	}

	public static void update()
	{
		AutoChangeFocus();
		UpdatePetGoHome();
		UpdateAutoPotara();
	}

	public static void Paint(mGraphics g)
	{
	}

	public void perform(int idAction, object p)
	{
		if (idAction >= 1000 && idAction < 2000)
		{
			Char c = (Char)p;
			listTargetAutoChangeFocus.Remove(c);
			GameScr.info1.addInfo("Đã Xóa " + c.cName + " [" + c.charID + "]");
			ShowConfigMenuDanhChuyen();
			return;
		}
		if (idAction >= 2000 && idAction < 3000)
		{
			Char c2 = (Char)p;
			listTargetAutoChangeFocus.Add(c2);
			GameScr.info1.addInfo("Đã Thêm " + c2.cName + " [" + c2.charID + "]");
			ShowConfigMenuDanhChuyen();
			return;
		}
		switch (idAction)
		{
		case 1:
			isAutoChangeFocus = !isAutoChangeFocus;
			GameScr.info1.addInfo("Đánh Chuyển Mục Tiêu\n" + (isAutoChangeFocus ? "[STATUS: ON]" : "[STATUS: OFF]"));
			if (listTargetAutoChangeFocus.Count == 0)
			{
				GameScr.info1.addInfo("Danh sách chuyển mục tiêu trống!");
				isAutoChangeFocus = false;
			}
			if (isAutoChangeFocus)
			{
				AutoSkill.isAutoSendAttack = false;
			}
			break;
		case 2:
			autoPotara = !autoPotara;
			GameScr.info1.addInfo("Auto bông tai : " + (autoPotara ? "ON" : "OFF"));
			break;
		case 3:
			petGoHome = !petGoHome;
			if (petGoHome && !Char.myCharz().havePet)
			{
				GameScr.info1.addInfo("Mày làm gì có đệ ???");
			}
			else
			{
				GameScr.info1.addInfo("Auto Đệ về nhà : " + (petGoHome ? "ON" : "OFF"));
			}
			break;
		case 7:
			ShowMenu();
			break;
		case 99:
			ShowConfigMenuDanhChuyen();
			return;
		}
		ShowMenu();
	}

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Config mục tiêu", getInstance(), 99, null));
		myVector.addElement(new Command(autoPotara ? "Auto bông tai liên tục: ON" : "Auto bông tai liên tục: OFF", getInstance(), 2, null));
		myVector.addElement(new Command(petGoHome ? "Auto Đệ về nhà: ON" : "Auto Đệ về nhà: OFF", getInstance(), 3, null));
		myVector.addElement(new Command("Tự động Tái tạo khi HP,KI dưới " + AutoPetCL.PercentCharge + "%: " + (AutoPetCL.TTNL ? "Bật" : "Tắt"), AutoPetCL.getInstance(), 5, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	public static void ShowConfigMenuDanhChuyen()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Đánh Chuyển Mục Tiêu\n" + (isAutoChangeFocus ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 1, null));
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char c = (Char)GameScr.vCharInMap.elementAt(i);
			if (c != null && c.charID != Char.myCharz().charID && !c.isPet && !c.isMiniPet && c.charID <= 0 && c.cName != null)
			{
				bool added = listTargetAutoChangeFocus.Contains(c);
				int actionId = (added ? (1000 + i) : (2000 + i));
				string label = (added ? "Xóa " : "Thêm ") + c.cName + " [" + c.charID + "]";
				myVector.addElement(new Command(label, getInstance(), actionId, c));
			}
		}
		myVector.addElement(new Command("Quay Lại", getInstance(), 7, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static bool checkPet()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (obj.cName != null && obj.cName != "" && obj.charID == Char.myCharz().charID * -1)
			{
				return true;
			}
		}
		return false;
	}

	private static void UpdatePetGoHome()
	{
		if (petGoHome && Char.myPetz().petStatus != 3 && checkPet())
		{
			Service.gI().petStatus(3);
			Char.myPetz().petStatus = 3;
		}
	}

	private static void UpdateAutoPotara()
	{
		if (!autoPotara)
		{
			return;
		}
		if (Char.myCharz().meDead || ModProCL.ExistPotara() == -1)
		{
			GameScr.info1.addInfo("cook moe mày đi");
			autoPotara = false;
			return;
		}
		long num = mSystem.currentTimeMillis();
		int num2 = (Char.myCharz().isNhapThe ? 11000 : 2000);
		if (num - lastPotaraTime > num2)
		{
			int indexUI = ModProCL.FindItemBag(ModProCL.ExistPotara()).indexUI;
			Service.gI().useItem(0, 1, (sbyte)indexUI, -1);
			lastPotaraTime = num;
		}
	}

	public static void loadData()
	{
	}
    public static void selectSkill()
    {
        int[] source = new int[4] { 0, 2, 4, 17 };
        Skill selectedSkill = null;
        Skill[] keySkill = GameScr.keySkill;
        foreach (Skill s in keySkill)
        {
            if (s != null && source.Contains(s.template.id))
            {
                if (s.template.id == 17)
                {
                    selectedSkill = s;
                    break;
                }
                if (selectedSkill == null)
                {
                    selectedSkill = s;
                }
            }
        }
        if (selectedSkill != null && selectedSkill != Char.myCharz().myskill)
        {
            GameScr.gI().doSelectSkill(selectedSkill, isShortcut: true);
        }
    }
    private static void AutoChangeFocus()
	{
		if (!isAutoChangeFocus || Char.myCharz().isCharge)
		{
			return;
		}
		selectSkill();
		if (Char.myCharz().meDead || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 || Char.myCharz().myskill.template.type == 3 || Char.myCharz().myskill.template.id == 10 || Char.myCharz().myskill.template.id == 11 || Char.myCharz().myskill.paintCanNotUseSkill)
		{
			return;
		}
		cooldownAutoChangeFocus = GetCooldownAutoChangeFocus(Char.myCharz().myskill);
		if (targetIndex >= listTargetAutoChangeFocus.Count)
		{
			targetIndex = 0;
		}
		if (mSystem.currentTimeMillis() - lastTimeChangeFocus >= cooldownAutoChangeFocus)
		{
			lastTimeChangeFocus = mSystem.currentTimeMillis();
			Char.myCharz().charFocus = GameScr.findCharInMap(listTargetAutoChangeFocus[targetIndex].charID);
			targetIndex++;
			if (targetIndex >= listTargetAutoChangeFocus.Count)
			{
				targetIndex = 0;
			}
			if (Char.myCharz().charFocus != null && AutoSkill.isMeCanAttackChar(Char.myCharz().charFocus) && (double)Math.Abs(Char.myCharz().charFocus.cx - Char.myCharz().cx) < (double)Char.myCharz().myskill.dx * 1.5)
			{
				Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
				AutoSkill.SendAttackToCharFocus();
			}
		}
	}

	private static long GetCooldownAutoChangeFocus(Skill skill)
	{
		if (skill.coolDown <= 500)
		{
			return 1000L;
		}
		return (long)((double)skill.coolDown * 1.2 + 200.0);
	}
}
