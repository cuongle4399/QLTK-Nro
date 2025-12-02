namespace Mod.community;

public class AutoPean : IActionListener, IChatable
{
	public static AutoPean _Instance;

	public static bool isAutoRequestPean;

	private static long lastTimeRequestedPean;

	public static bool isAutoDonatePean;

	public static bool isAutoHarvestPean;

	private static int minimumHPPercent;

	private static int minimumMPPercent;

	private static int minimumHP;

	private static int minimumMP;

	private static bool isSaveData;

	private static string[] inputHPPercent;

	private static string[] inputMPPercent;

	private static string[] inputHP;

	private static string[] inputMP;

	public static AutoPean getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new AutoPean();
		}
		return _Instance;
	}

	public static void Update()
	{
		if (isAutoRequestPean)
		{
			RequestPean();
		}
		if (isAutoDonatePean && GameCanvas.gameTick % 40 == 0)
		{
			DonatePean();
		}
		if (isAutoHarvestPean)
		{
			HarvestPean();
		}
		if (!Char.myCharz().meDead)
		{
			AutoUsePean();
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
		{
			ChatTextField.gI().isShow = false;
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputHPPercent[0]))
		{
			try
			{
				int num = int.Parse(ChatTextField.gI().tfChat.getText());
				if (num >= 100)
				{
					num = 99;
				}
				if (num <= 0)
				{
					num = 1;
				}
				minimumHPPercent = num;
				GameScr.info1.addInfo("Auto pean khi HP dưới: " + num + "%");
				if (isSaveData)
				{
					Rms.saveRMSInt("AutoPeanPercentHP", minimumHPPercent);
				}
			}
			catch
			{
				GameScr.info1.addInfo("%HP không hợp lệ, vui lòng nhập lại");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputMPPercent[0]))
		{
			try
			{
				int num2 = int.Parse(ChatTextField.gI().tfChat.getText());
				if (num2 >= 100)
				{
					num2 = 99;
				}
				if (num2 <= 0)
				{
					num2 = 1;
				}
				minimumMPPercent = num2;
				GameScr.info1.addInfo("Auto pean khi MP dưới: " + num2 + "%");
				if (isSaveData)
				{
					Rms.saveRMSInt("AutoPeanPercentMP", minimumMPPercent);
				}
			}
			catch
			{
				GameScr.info1.addInfo("%MP không hợp lệ, vui lòng nhập lại");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputHP[0]))
		{
			try
			{
				int num3 = (minimumHP = int.Parse(ChatTextField.gI().tfChat.getText()));
				if (isSaveData)
				{
					Rms.saveRMSString("AutoPeanHP", minimumHP.ToString());
				}
				GameScr.info1.addInfo("Auto pean khi HP dưới: " + NinjaUtil.getMoneys(num3) + "HP");
			}
			catch
			{
				GameScr.info1.addInfo("HP không hợp lệ, vui lòng nhập lại");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputMP[0]))
		{
			try
			{
				int num4 = (minimumMP = int.Parse(ChatTextField.gI().tfChat.getText()));
				if (isSaveData)
				{
					Rms.saveRMSString("AutoPeanMP", minimumMP.ToString());
				}
				GameScr.info1.addInfo("Auto pean khi MP dưới: " + NinjaUtil.getMoneys(num4) + "MP");
			}
			catch
			{
				GameScr.info1.addInfo("MP không hợp lệ, vui lòng nhập lại");
			}
			ResetChatTextField();
		}
		else
		{
			Service.gI().chat(text);
			ChatTextField.gI().isShow = false;
		}
	}

	public void onCancelChat()
	{
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			isAutoRequestPean = !isAutoRequestPean;
			GameScr.info1.addInfo("Xin Đậu\n" + (isAutoRequestPean ? "[STATUS: ON]" : "[STATUS: OFF]"));
			if (isSaveData)
			{
				Rms.saveRMSInt("AutoPeanIsAutoRequestPean", isAutoRequestPean ? 1 : 0);
			}
			break;
		case 2:
			isAutoDonatePean = !isAutoDonatePean;
			GameScr.info1.addInfo("Cho Đậu\n" + (isAutoDonatePean ? "[STATUS: ON]" : "[STATUS: OFF]"));
			if (isSaveData)
			{
				Rms.saveRMSInt("AutoPeanIsAutoSendPean", isAutoDonatePean ? 1 : 0);
			}
			break;
		case 3:
			isAutoHarvestPean = !isAutoHarvestPean;
			GameScr.info1.addInfo("Thu Đậu\n" + (isAutoHarvestPean ? "[STATUS: ON]" : "[STATUS: OFF]"));
			if (isSaveData)
			{
				Rms.saveRMSInt("AutoPeanIsAutoHarvestPean", isAutoHarvestPean ? 1 : 0);
			}
			break;
		case 4:
			if (minimumHP != 0)
			{
				minimumHP = 0;
				GameScr.info1.addInfo("Auto đậu: 0HP");
				if (isSaveData)
				{
					Rms.saveRMSString("AutoPeanHP", minimumHP.ToString());
				}
			}
			else if (minimumHP == 0)
			{
				ChatTextField.gI().strChat = inputHP[0];
				ChatTextField.gI().tfChat.name = inputHP[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
			}
			break;
		case 5:
			if (minimumHPPercent != 0)
			{
				minimumHPPercent = 0;
				GameScr.info1.addInfo("Auto đậu: 0% HP");
				if (isSaveData)
				{
					Rms.saveRMSInt("AutoPeanPercentHP", minimumHPPercent);
				}
			}
			else if (minimumHPPercent == 0)
			{
				ChatTextField.gI().strChat = inputHPPercent[0];
				ChatTextField.gI().tfChat.name = inputHPPercent[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
			}
			break;
		case 6:
			if (minimumMP != 0)
			{
				minimumMP = 0;
				GameScr.info1.addInfo("Auto đậu: 0MP");
				if (isSaveData)
				{
					Rms.saveRMSString("AutoPeanMP", minimumMP.ToString());
				}
			}
			else if (minimumMP == 0)
			{
				ChatTextField.gI().strChat = inputMP[0];
				ChatTextField.gI().tfChat.name = inputMP[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
			}
			break;
		case 7:
			if (minimumMPPercent != 0)
			{
				minimumMPPercent = 0;
				GameScr.info1.addInfo("Auto đậu: 0% MP");
				if (isSaveData)
				{
					Rms.saveRMSInt("AutoPeanPercentMP", minimumMPPercent);
				}
			}
			else if (minimumMPPercent == 0)
			{
				ChatTextField.gI().strChat = inputMPPercent[0];
				ChatTextField.gI().tfChat.name = inputMPPercent[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
			}
			break;
		case 8:
			isSaveData = !isSaveData;
			GameScr.info1.addInfo("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"));
			Rms.saveRMSInt("AutoPeanIsSaveRms", isSaveData ? 1 : 0);
			if (isSaveData)
			{
				SaveData();
			}
			break;
		}
	}

	public static void ShowMenu()
	{
		LoadData();
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Xin Đậu\n" + (isAutoRequestPean ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 1, null));
		myVector.addElement(new Command("Cho Đậu\n" + (isAutoDonatePean ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 2, null));
		myVector.addElement(new Command("Thu Đậu\n" + (isAutoHarvestPean ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 3, null));
		myVector.addElement(new Command("Ăn Đậu Khi HP Dưới: " + NinjaUtil.getMoneys(minimumHP) + "HP", getInstance(), 4, null));
		myVector.addElement(new Command("Ăn Đậu Khi HP Dưới: " + minimumHPPercent + "%", getInstance(), 5, null));
		myVector.addElement(new Command("Ăn Đậu Khi MP Dưới: " + NinjaUtil.getMoneys(minimumMP) + "MP", getInstance(), 6, null));
		myVector.addElement(new Command("Ăn Đậu Khi MP Dưới: " + minimumMPPercent + "%", getInstance(), 7, null));
		myVector.addElement(new Command("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 8, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void LoadData()
	{
		isSaveData = Rms.loadRMSInt("AutoPeanIsSaveRms") == 1;
		if (isSaveData)
		{
			isAutoRequestPean = Rms.loadRMSInt("AutoPeanIsAutoRequestPean") == 1;
			isAutoDonatePean = Rms.loadRMSInt("AutoPeanIsAutoSendPean") == 1;
			isAutoHarvestPean = Rms.loadRMSInt("AutoPeanIsAutoHarvestPean") == 1;
			if (Rms.loadRMSInt("AutoPeanPercentHP") == -1)
			{
				minimumHPPercent = 0;
			}
			else
			{
				minimumHPPercent = Rms.loadRMSInt("AutoPeanPercentHP");
			}
			if (Rms.loadRMSInt("AutoPeanPercentMP") == -1)
			{
				minimumMPPercent = 0;
			}
			else
			{
				minimumMPPercent = Rms.loadRMSInt("AutoPeanPercentMP");
			}
			if (Rms.loadRMSString("AutoPeanHP") == null)
			{
				minimumHP = 0;
			}
			else
			{
				minimumHP = int.Parse(Rms.loadRMSString("AutoPeanHP"));
			}
			if (Rms.loadRMSString("AutoPeanMP") == null)
			{
				minimumMP = 0;
			}
			else
			{
				minimumMP = int.Parse(Rms.loadRMSString("AutoPeanMP"));
			}
		}
	}

	private static void SaveData()
	{
		Rms.saveRMSInt("AutoPeanIsAutoRequestPean", isAutoRequestPean ? 1 : 0);
		Rms.saveRMSInt("AutoPeanIsAutoSendPean", isAutoDonatePean ? 1 : 0);
		Rms.saveRMSInt("AutoPeanIsAutoHarvestPean", isAutoHarvestPean ? 1 : 0);
		Rms.saveRMSString("AutoPeanHP", minimumHP.ToString());
		Rms.saveRMSInt("AutoPeanPercentHP", minimumHPPercent);
		Rms.saveRMSString("AutoPeanMP", minimumMP.ToString());
		Rms.saveRMSInt("AutoPeanPercentMP", minimumMPPercent);
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	private static void RequestPean()
	{
		if (mSystem.currentTimeMillis() - lastTimeRequestedPean >= 301000)
		{
			lastTimeRequestedPean = mSystem.currentTimeMillis();
			Service.gI().clanMessage(1, "", -1);
		}
	}

	private static void DonatePean()
	{
		for (int i = 0; i < ClanMessage.vMessage.size(); i++)
		{
			ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(i);
			if (clanMessage.maxCap != 0 && clanMessage.playerName != Char.myCharz().cName && clanMessage.recieve != clanMessage.maxCap)
			{
				Service.gI().clanDonate(clanMessage.id);
				break;
			}
		}
	}

	private static void HarvestPean()
	{
		if (TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
		{
			if (Char.myCharz().arrItemBox[i] != null && Char.myCharz().arrItemBox[i].template.type == 6)
			{
				num += Char.myCharz().arrItemBox[i].quantity;
			}
		}
		if (num < 20 && GameCanvas.gameTick % 120 == 0)
		{
			for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
			{
				if (Char.myCharz().arrItemBag[j] != null && Char.myCharz().arrItemBag[j].template.type == 6)
				{
					Service.gI().getItem(1, (sbyte)j);
					break;
				}
			}
		}
		if (GameScr.gI().magicTree.currPeas > 0 && (GameScr.hpPotion < 10 || num < 20) && GameCanvas.gameTick % 200 == 0)
		{
			Service.gI().openMenu(4);
			Service.gI().menu(4, 0, 0);
		}
	}

	private static void AutoUsePean()
	{
		if (GameScr.hpPotion > 0)
		{
			if (minimumHPPercent != 0 && isMyHPLowerThan(minimumHP, minimumHPPercent) && GameCanvas.gameTick % 40 == 0)
			{
				GameScr.gI().doUseHP();
			}
			else if (minimumMPPercent != 0 && isMyMPLowerThan(minimumMP, minimumMPPercent) && GameCanvas.gameTick % 40 == 0)
			{
				GameScr.gI().doUseHP();
			}
		}
	}

	public static int MyHPPercent()
	{
		return (int)(Char.myCharz().cHP * 100 / Char.myCharz().cHPFull);
	}

	public static int MyMPPercent()
	{
		return (int)(Char.myCharz().cMP * 100 / Char.myCharz().cMPFull);
	}

	private static bool isMyHPLowerThan(int minHP, int minHPPercent)
	{
		if (Char.myCharz().cHP > 0)
		{
			if (MyHPPercent() > minHPPercent)
			{
				return Char.myCharz().cHP < minHP;
			}
			return true;
		}
		return false;
	}

	private static bool isMyMPLowerThan(int minMP, int minMPPercent)
	{
		if (Char.myCharz().cHP > 0)
		{
			if (MyMPPercent() > minMPPercent)
			{
				return Char.myCharz().cMP < minMP;
			}
			return true;
		}
		return false;
	}

	static AutoPean()
	{
		inputHPPercent = new string[2] { "Nhập %HP Pean", "%HP" };
		inputMPPercent = new string[2] { "Nhập %MP Pean", "%MP" };
		inputHP = new string[2] { "Nhập HP Pean", "HP" };
		inputMP = new string[2] { "Nhập MP Pean", "MP" };
		LoadData();
	}
}
