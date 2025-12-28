using System.Threading;
using Xmap;
using Mod.community;

namespace Mod.CuongLe;

public class AutoPetCL : IActionListener, IChatable
{
	private enum TeleState
	{
		Idle,
		TeleToItem,
		WaitingPickItem,
		TeleBack,
		WaitingDelayAfterBack
	}

	public static bool attackingPet;

	public static bool DeSuaLapem;

	public static bool isKOK;

	public static bool autoFlag;

	public static bool AutoNhatItemPet;

	public static bool pickingUpPet;

	public static readonly string inputPercent;

	public static int PercentCharge;

	public static bool TTNL;

	public static bool HealingPower;

	public static int soLanDeKeu;

	public static int soLanTanCong;

	public static int SoItemNhatTuPet;

	public static bool aGimPet;

	public static AutoPetCL _Instance;

	private static long delayPick;

	private static long delayAfterGoBack;

	private static TeleState teleState;

	private static ItemMap targetItem;

	private static int xOld;

	private static int yOld;

	private static long teleTimeout;

	private static long TELEPORT_MAX_WAIT;

	private static long focusPetTimer;

	public static bool autotachHopThe;

	public static long lastHopTheTime;

	static AutoPetCL()
	{
		inputPercent = "Nhập % Hp,Ki khi còn sẽ tái tạo";
		PercentCharge = 0;
		soLanDeKeu = 0;
		soLanTanCong = 0;
		SoItemNhatTuPet = 0;
		delayPick = 0L;
		delayAfterGoBack = 0L;
		teleState = TeleState.Idle;
		targetItem = null;
		teleTimeout = 0L;
		TELEPORT_MAX_WAIT = 2000L;
		focusPetTimer = 0L;
	}

	public static AutoPetCL getInstance()
	{
		return _Instance ?? (_Instance = new AutoPetCL());
	}

	public void perform(int idAction, object p)
	{
		if ((AutoTrainCL.autoHopThe || YardatCL.petGoHome) && idAction != 6 && idAction != 3 && idAction != 5)
		{
			GameScr.info1.addInfo("Vui lòng tắt auto hợp thể hoặc auto đệ về nhà!!!");
			return;
		}
		switch (idAction)
		{
		case 1:
			DeSuaLapem = !DeSuaLapem;
			GameScr.info1.addInfo("Tự động pem khi đệ sủa: " + (DeSuaLapem ? "Bật" : "Tắt"));
			ShowMenu();
			break;
		case 2:
			isKOK = !isKOK;
			new Thread(autoDeKOK).Start();
			GameScr.info1.addInfo("Auto Up Kaioken: " + (isKOK ? "[STATUS:ON]" : "[STATUS:OFF]"));
			ShowMenu();
			break;
		case 3:
			autoFlag = !autoFlag;
			new Thread(autoCoDen).Start();
			GameScr.info1.addInfo(autoFlag ? "Auto Cờ đen chống địch: ON" : "Auto Cờ đen chống địch: OFF");
			ShowMenu();
			break;
		case 4:
			AutoNhatItemPet = !AutoNhatItemPet;
			GameScr.info1.addInfo("Auto nhặt đồ đệ khi Pem: " + (AutoNhatItemPet ? "Bật" : "Tắt"));
			ShowMenu();
			break;
		case 5:
			HandleTTNL();
			break;
		case 6:
			HealingPower = !HealingPower;
			new Thread(AutoHealingPower).Start();
			ShowMenu();
			break;
		case 7:
			aGimPet = !aGimPet;
			GameScr.info1.addInfo("Auto Gim Đệ: " + (aGimPet ? "Bật" : "Tắt"));
			ShowMenu();
			break;
		case 8:
			autotachHopThe = !autotachHopThe;
			if (autotachHopThe)
			{
				if (ModProCL.ExistPotara() == -1)
				{
					autotachHopThe = false;
					ChatPopup.addChatPopupMultiLineGameline("Mày làm cak j có bông tai mà auto tách hợp thể ???");
				}
				else if (TileMap.mapID == Char.myCharz().cgender + 21)
				{
					autotachHopThe = false;
					ChatPopup.addChatPopupMultiLineGameline("Vui lòng ra khỏi nhà để mở auto tách hợp thể!");
				}
			}
			GameScr.info1.addInfo("Auto tách hợp thể: " + (autotachHopThe ? "Bật" : "Tắt"));
			break;
		}
	}

	public void HandleTTNL()
	{
		if (TTNL)
		{
			TTNL = false;
			GameScr.info1.addInfo("Đã tắt Tái tạo năng lượng khi hp,ki thấp");
			return;
		}
		if (Char.myCharz().getGender() != "XD")
		{
			GameScr.info1.addInfo("Chức năng chỉ dành cho XD");
			TTNL = false;
			return;
		}
		if (!AutoSkill.checkSkill(8))
		{
			GameScr.info1.addInfo("Bạn không có skill tái tạo");
			TTNL = false;
			return;
		}
		ChatTextField.gI().strChat = inputPercent;
		ChatTextField.gI().tfChat.name = "Nhập % hp,ki sẽ tái tạo từ 1 đến 100";
		ChatTextField.gI().startChat2(getInstance(), string.Empty);
		TTNL = true;
	}

	public void onChatFromMe(string text, string a)
	{
		if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()))
		{
			ChatTextField.gI().isShow = false;
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputPercent))
		{
			HandlePercentInput(text);
		}
		else
		{
			Service.gI().chat(text);
			ChatTextField.gI().isShow = false;
		}
	}

	private void HandlePercentInput(string text)
	{
		try
		{
			if (int.TryParse(text.Trim(), out var result) && result >= 1 && result <= 100)
			{
				PercentCharge = result;
				GameScr.info1.addInfo("Sẽ tái tạo năng lượng khi % hp,ki <= " + PercentCharge);
			}
			else
			{
				GameScr.info1.addInfo("Vui lòng nhập lại đúng số từ 1-100");
			}
		}
		catch
		{
			GameScr.info1.addInfo("Lỗi cụ r!!!!");
		}
		ResetChatTextField();
	}

	public void onCancelChat()
	{
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().isShow = false;
	}

	public static bool checkDeKeu(string s)
	{
		return s.ToLower().Contains("sao sư phụ không đánh đi?");
	}

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Tự động pem khi đệ sủa: " + (DeSuaLapem ? "Bật" : "Tắt"), getInstance(), 1, null));
		myVector.addElement(new Command("Auto up đệ KOK: " + (isKOK ? "Bật" : "Tắt"), getInstance(), 2, null));
		myVector.addElement(new Command("Auto cờ đen chống địch: " + (autoFlag ? "Bật" : "Tắt"), getInstance(), 3, null));
		myVector.addElement(new Command("Auto Nhặt đồ đệ pem: " + (AutoNhatItemPet ? "Bật" : "Tắt"), getInstance(), 4, null));
		myVector.addElement(new Command("Tự động Tái tạo khi HP,KI dưới " + PercentCharge + "%: " + (TTNL ? "Bật" : "Tắt"), getInstance(), 5, null));
		myVector.addElement(new Command("Auto trị thương bản thân(namek) " + (HealingPower ? "Bật" : "Tắt"), getInstance(), 6, null));
		myVector.addElement(new Command("Auto Gim Đệ " + (aGimPet ? "Bật" : "Tắt"), getInstance(), 7, null));
		myVector.addElement(new Command("Auto tách hợp thể: " + (autotachHopThe ? "Bật" : "Tắt"), getInstance(), 8, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	public static void update()
	{
		if (!Char.myCharz().isDie && !attackingPet)
		{
			long now = mSystem.currentTimeMillis();
			HandleAutoPick(now);
			HandleTTNLUpdate(now);
			UpdateAutoFocusPet();
			updateAutoTachHopThe();
		}
	}

	public static void updateAutoTachHopThe()
	{
		if (!autotachHopThe || !Char.myCharz().isNhapThe || Char.myCharz().meDead || GameCanvas.gameTick % 5 != 0)
		{
			return;
		}
		long num = mSystem.currentTimeMillis();
		if (num - lastHopTheTime < 1000)
		{
			return;
		}
		int num2 = ModProCL.ExistPotara();
		if (num2 != -1)
		{
			Item item = ModProCL.FindItemBag(num2);
			if (item != null)
			{
				Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
				lastHopTheTime = num;
			}
		}
	}

	private static void HandleAutoPick(long now)
	{
		switch (teleState)
		{
		case TeleState.Idle:
			TryFindItem(now);
			break;
		case TeleState.TeleToItem:
			if (HasReachedPosition(targetItem.x, targetItem.y) || now > teleTimeout)
			{
				Service.gI().pickItem(targetItem.itemMapID);
				SoItemNhatTuPet++;
				teleTimeout = now + 1000;
				teleState = TeleState.WaitingPickItem;
			}
			break;
		case TeleState.WaitingPickItem:
			if (now >= teleTimeout)
			{
				MainXmapCL.TeleportTo(xOld, yOld);
				teleTimeout = now + TELEPORT_MAX_WAIT;
				teleState = TeleState.TeleBack;
			}
			break;
		case TeleState.TeleBack:
			if (HasReachedPosition(xOld, yOld) || now > teleTimeout)
			{
				delayAfterGoBack = now + 100;
				teleState = TeleState.WaitingDelayAfterBack;
			}
			break;
		case TeleState.WaitingDelayAfterBack:
			if (now >= delayAfterGoBack)
			{
				pickingUpPet = false;
				delayPick = now;
				teleState = TeleState.Idle;
				targetItem = null;
			}
			break;
		}
	}

	private static void TryFindItem(long now)
	{
		if (!AutoNhatItemPet || now - delayPick <= 400)
		{
			return;
		}
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			if (itemMap != null && itemMap.template != null && itemMap.playerId == Char.myCharz().charID)
			{
				targetItem = itemMap;
				xOld = Char.myCharz().cx;
				yOld = Char.myCharz().cy;
				bool isGoBack = AutoTrainCL.isGoBack;
				if (isGoBack)
				{
					AutoTrainCL.isGoBack = false;
				}
				pickingUpPet = true;
				MainXmapCL.TeleportTo(targetItem.x, targetItem.y);
				teleTimeout = now + TELEPORT_MAX_WAIT;
				teleState = TeleState.TeleToItem;
				if (isGoBack)
				{
					AutoTrainCL.isGoBack = true;
				}
				break;
			}
		}
	}

	private static void HandleTTNLUpdate(long now)
	{
		if (!TTNL || PercentCharge < 1 || PercentCharge > 100 || Char.myCharz().isWaitMonkey || Char.myCharz().isCharge || (AutoPean.MyMPPercent() > PercentCharge && AutoPean.MyHPPercent() > PercentCharge) || GameCanvas.gameTick % 20 != 0)
		{
			return;
		}
		for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
		{
			if (Char.myCharz().vSkill.elementAt(i) is Skill skill && skill.template.id == 8 && !skill.paintCanNotUseSkill)
			{
				if (Char.myCharz().myskill != skill)
				{
					GameScr.gI().doSelectSkill(skill, isShortcut: true);
				}
				GameScr.gI().doSelectSkill(skill, isShortcut: true);
			}
		}
	}

	private static bool HasReachedPosition(int x, int y)
	{
		int num = Char.myCharz().cx - x;
		int num2 = Char.myCharz().cy - y;
		int num3 = 5;
		return num * num + num2 * num2 <= num3 * num3;
	}

	public static void autoDeKOK()
	{
		int cx = Char.myCharz().cx;
		int cy = Char.myCharz().cy;
		while (isKOK)
		{
			if (Char.myCharz().meDead || AutoTrainCL.isAutoTrain)
			{
				isKOK = false;
				break;
			}
			while (MainXmapCL.isXmaping)
			{
				Thread.Sleep(1000);
			}
			Char.myCharz().currentMovePoint = new MovePoint(cx + 10, cy);
			Thread.Sleep(500);
			if (!AutoTrainCL.isGoBack)
			{
				Char.myCharz().currentMovePoint = new MovePoint(cx - 10, cy);
			}
			Thread.Sleep(500);
		}
	}

	public static void FindMobForPet()
	{
		attackingPet = true;
		soLanDeKeu++;
		MyVector myVector = new MyVector();
		Mob mob = FindClosestMob();
		bool isGoBack = AutoTrainCL.isGoBack;
		if (isGoBack)
		{
			AutoTrainCL.isGoBack = false;
		}
		int cx = Char.myCharz().cx;
		int cy = Char.myCharz().cy;
		if (mob != null)
		{
			YardatCL.selectSkill();
			MainXmapCL.TeleportTo(mob.x, mob.y);
			myVector.addElement(mob);
			Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
			Thread.Sleep(500);
			Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
			soLanTanCong++;
		}
		if (isGoBack)
		{
			AutoTrainCL.isGoBack = true;
		}
		else
		{
			MainXmapCL.TeleportTo(cx, cy);
		}
		attackingPet = false;
	}

	private static Mob FindClosestMob()
	{
		Mob mob = null;
		float num = float.MaxValue;
		int cx = Char.myCharz().cx;
		int cy = Char.myCharz().cy;
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob2 = (Mob)GameScr.vMob.elementAt(i);
			if (mob2 != null && mob2.getTemplate().type != 4)
			{
				int num2 = mob2.x - cx;
				int num3 = mob2.y - cy;
				float num4 = num2 * num2 + num3 * num3;
				if (num4 < num)
				{
					num = num4;
					mob = mob2;
				}
			}
		}
		if (mob == null)
		{
			for (int j = 0; j < GameScr.vMob.size(); j++)
			{
				Mob mob3 = (Mob)GameScr.vMob.elementAt(j);
				if (mob3 != null)
				{
					int num5 = mob3.x - cx;
					int num6 = mob3.y - cy;
					float num7 = num5 * num5 + num6 * num6;
					if (num7 < num)
					{
						num = num7;
						mob = mob3;
					}
				}
			}
		}
		return mob;
	}

	public static void autoCoDen()
	{
		while (autoFlag)
		{
			if (!checkCoDen() && Char.myCharz().cFlag == 0)
			{
				Service.gI().getFlag(1, 8);
			}
			if (checkCoDen() && Char.myCharz().cFlag == 8)
			{
				Service.gI().getFlag(1, 0);
			}
			Thread.Sleep(1000);
		}
		while (Char.myCharz().cFlag != 0)
		{
			Service.gI().getFlag(1, 0);
			Thread.Sleep(5000);
		}
	}

	public static bool checkCoDen()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (obj.cFlag != 0 && obj.charID > 0)
			{
				return true;
			}
		}
		return false;
	}

	public static void AutoHealingPower()
	{
		if (!Char.myCharz().getGender().Equals("NM"))
		{
			HealingPower = false;
			GameScr.info1.addInfo("Chỉ namek mới có thể dùng !!");
			return;
		}
		if (HealingPower)
		{
			GameScr.info1.addInfo("Auto trị thương bản thân: Bật");
		}
		while (HealingPower)
		{
			SkillHealing();
			Thread.Sleep(((Skill)Char.myCharz().vSkillFight.elementAt(2)).coolDown);
		}
		HealingPower = false;
		GameScr.info1.addInfo("Auto trị thương bản thân:Tắt");
	}

	public static void SkillHealing()
	{
		if (!Char.myCharz().meDead)
		{
			Service.gI().selectSkill(7);
			SelectMe(Char.myCharz());
			Service.gI().selectSkill(Char.myCharz().myskill.template.id);
		}
	}

	public static void SelectMe(Char player)
	{
		try
		{
			MyVector myVector = new MyVector();
			myVector.addElement(player);
			Service.gI().sendPlayerAttack(new MyVector(), myVector, -1);
		}
		catch
		{
		}
	}

	public static void UpdateAutoFocusPet()
	{
		if (!aGimPet || mSystem.currentTimeMillis() < focusPetTimer)
		{
			return;
		}
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (obj.cName != null && obj.cName != "" && obj.charID == Char.myCharz().charID * -1)
			{
				Char.myCharz().npcFocus = null;
				Char.myCharz().charFocus = obj;
				Char.myCharz().mobFocus = null;
			}
		}
		focusPetTimer = mSystem.currentTimeMillis() + 500;
	}
}
