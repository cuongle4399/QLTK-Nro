using System.Collections.Generic;

namespace Mod.community;

public class AutoSkill : IActionListener, IChatable
{
	private static AutoSkill _Instance;

	public static bool isLoadKeySkill;

	public static bool isAutoSendAttack;

	private static long[] lastTimeSendAttack;

	public static bool isPetAskedForUseSkill;

	public static bool[] isAutoUseSkills;

	private static long[] lastTimeUseSkill;

	private static long[] timeAutoSkills;

	private static int indexSkillAuto;

	private static List<Char> listTargetAutoChangeFocus;

	private static bool isAutoShield;

	private static string[] inputDelay;

	private static bool isSaveData;

	private static long lastTimeAutoUseSkill;

	private static bool findMobComplete;

	private static bool debugMode;

	public static AutoSkill getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new AutoSkill();
		}
		return _Instance;
	}

	public static void Update()
	{
		if (isAutoSendAttack)
		{
			AutoSendAttack();
		}
		if (!Char.myCharz().meDead)
		{
			for (int i = 0; i < GameScr.keySkill.Length; i++)
			{
				if (isAutoUseSkills[i])
				{
					AutoUseSkill(i);
				}
			}
		}
		if (isLoadKeySkill && GameCanvas.gameTick % 20 == 0)
		{
			isLoadKeySkill = false;
			LoadKeySkills();
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
		{
			if (ChatTextField.gI().strChat.Equals(inputDelay[0]))
			{
				try
				{
					long num = long.Parse(ChatTextField.gI().tfChat.getText());
					timeAutoSkills[indexSkillAuto] = num;
					isAutoUseSkills[indexSkillAuto] = true;
					GameScr.info1.addInfo("Auto " + GameScr.keySkill[indexSkillAuto].template.name + ": " + NinjaUtil.getMoneys(num) + " mili giây");
				}
				catch
				{
					GameScr.info1.addInfo("Vui Lòng Nhập Lại Delay!");
				}
				ResetChatTextField();
			}
		}
		else
		{
			ResetChatTextField();
			Service.gI().chat(text);
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
			isAutoSendAttack = !isAutoSendAttack;
			GameScr.info1.addInfo("Tự Đánh\n" + (isAutoSendAttack ? "[STATUS: ON]" : "[STATUS: OFF]"));
			if (isAutoSendAttack)
			{
				YardatCL.isAutoChangeFocus = false;
			}
			break;
		case 2:
			break;
		case 3:
		{
			MyVector myVector = new MyVector();
			for (int i = 0; i < GameScr.keySkill.Length; i++)
			{
				myVector.addElement(new Command(((GameScr.keySkill[i] != null) ? GameScr.keySkill[i].template.name : "null") + "\n[" + (i + 1) + "]\n", getInstance(), 10, i));
			}
			GameCanvas.menu.startAt(myVector, 3);
			break;
		}
		case 4:
			isAutoShield = !isAutoShield;
			GameScr.info1.addInfo("Auto Khiên Pro\n" + (isAutoShield ? "[STATUS: ON]" : "[STATUS: OFF]"));
			break;
		case 5:
			GameScr.info1.addInfo("Không Hỗ Trợ Lưu Cài Đặt Ở Mục Này!");
			break;
		case 6:
			break;
		case 10:
			ShowMenuAutoSkill((int)p);
			break;
		case 11:
		{
			int num2 = (int)p;
			isAutoUseSkills[num2] = !isAutoUseSkills[num2];
			if (isAutoUseSkills[num2])
			{
				timeAutoSkills[num2] = -1L;
			}
			GameScr.info1.addInfo("Auto " + GameScr.keySkill[num2].template.name + (isAutoUseSkills[num2] ? (": " + NinjaUtil.getMoneys(timeAutoSkills[num2]) + " mili giây") : "\n[STATUS: OFF]"));
			break;
		}
		case 12:
			ChatTextField.gI().strChat = inputDelay[0];
			ChatTextField.gI().tfChat.name = inputDelay[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			indexSkillAuto = (int)p;
			break;
		case 13:
		{
			int num = (int)p;
			GameScr.keySkill[num].coolDown = 0;
			GameScr.keySkill[num].manaUse = 0;
			GameScr.info1.addInfo("Đóng Băng " + GameScr.keySkill[num].template.name);
			break;
		}
		case 7:
		case 8:
		case 9:
			break;
		}
	}

	public static void ShowMenu()
	{
		LoadData();
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Tự Đánh\n" + (isAutoSendAttack ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 1, null));
		myVector.addElement(new Command(GameScr.keySkill.Length + " Ô Kỹ Năng", getInstance(), 3, null));
		myVector.addElement(new Command("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 5, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ShowMenuAutoSkill(int skillIndex)
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Auto Sử Dụng\n" + (isAutoUseSkills[skillIndex] ? ("[" + NinjaUtil.getMoneys(timeAutoSkills[skillIndex]) + " mili giây]") : "[STATUS: OFF]"), getInstance(), 11, skillIndex));
		myVector.addElement(new Command("Nhập Delay\n[mili giây]", getInstance(), 12, skillIndex));
		myVector.addElement(new Command("Đóng Băng\n" + GameScr.keySkill[skillIndex].template.name, getInstance(), 13, skillIndex));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	private static void LoadData()
	{
	}

	private static void LoadKeySkills()
	{
		for (int i = 0; i < Char.myCharz().nClass.skillTemplates.Length; i++)
		{
			SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[i];
			Skill skill = Char.myCharz().getSkill(skillTemplate);
			if (skill != null)
			{
				GameScr.keySkill[i] = skill;
			}
			GameScr.gI().saveKeySkillToRMS();
		}
	}

	public static void AutoSendAttack()
	{
		if (Char.myCharz().meDead || Char.myCharz().cHP <= 0 || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 || Char.myCharz().myskill.template.type == 3 || Char.myCharz().myskill.template.id == 10 || Char.myCharz().myskill.template.id == 11 || Char.myCharz().isWaitMonkey || Char.myCharz().isCharge || (Char.myCharz().myskill.paintCanNotUseSkill && !GameCanvas.panel.isShow))
		{
			return;
		}
		int mySkillIndex = GetMySkillIndex();
		if (mSystem.currentTimeMillis() - lastTimeSendAttack[mySkillIndex] > GetCoolDown(Char.myCharz().myskill))
		{
			if (GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
			{
				Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
				SendAttackToMobFocus();
				lastTimeSendAttack[mySkillIndex] = mSystem.currentTimeMillis();
			}
			else if (Char.myCharz().charFocus != null && isMeCanAttackChar(Char.myCharz().charFocus) && (double)Math.Abs(Char.myCharz().charFocus.cx - Char.myCharz().cx) < (double)Char.myCharz().myskill.dx * 1.7)
			{
				Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
				SendAttackToCharFocus();
				lastTimeSendAttack[mySkillIndex] = mSystem.currentTimeMillis();
			}
		}
	}
    public static void AutoSendAttackVIP()
    {
        if (Char.myCharz().meDead || Char.myCharz().cHP <= 0 || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 || Char.myCharz().myskill.template.type == 3 || Char.myCharz().myskill.template.id == 10 || Char.myCharz().myskill.template.id == 11 || Char.myCharz().isWaitMonkey || Char.myCharz().isCharge || (Char.myCharz().myskill.paintCanNotUseSkill && !GameCanvas.panel.isShow))
        {
            return;
        }
        int mySkillIndex = GetMySkillIndex();
        if (GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
        {
            Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
            SendAttackToMobFocus();
            lastTimeSendAttack[mySkillIndex] = mSystem.currentTimeMillis();
        }
        else if (Char.myCharz().charFocus != null && isMeCanAttackChar(Char.myCharz().charFocus) && (double)Math.Abs(Char.myCharz().charFocus.cx - Char.myCharz().cx) < (double)Char.myCharz().myskill.dx * 1.7)
        {
            Char.myCharz().myskill.lastTimeUseThisSkill = mSystem.currentTimeMillis();
            SendAttackToCharFocus();
            lastTimeSendAttack[mySkillIndex] = mSystem.currentTimeMillis();
        }
    }
    private static void AutoUseSkill(int skillIndex)
	{
		if (TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 23)
		{
			return;
		}
		if (skillIndex >= GameScr.keySkill.Length)
		{
			skillIndex = GameScr.keySkill.Length - 1;
		}
		if (skillIndex < 0)
		{
			skillIndex = 0;
		}
		if (GameScr.keySkill[skillIndex] == null || GameScr.keySkill[skillIndex].paintCanNotUseSkill || (GameScr.keySkill[skillIndex].template.id == 19 && ModProCL.checkItemTime(3784)))
		{
			return;
		}
		if (GameScr.keySkill[skillIndex].coolDown == 0)
		{
			timeAutoSkills[skillIndex] = 500L;
		}
		else
		{
			if (!isMeHasEnoughMP(GameScr.keySkill[skillIndex]) || GameScr.gI().isCharging())
			{
				return;
			}
			long currentTime = mSystem.currentTimeMillis();
			if (GameCanvas.gameTick % 10 != 0)
			{
				return;
			}
			long timeSinceLastUse = currentTime - lastTimeUseSkill[skillIndex];
			long delay = timeAutoSkills[skillIndex];
			if (delay == -1 || timeSinceLastUse > delay)
			{
				lastTimeUseSkill[skillIndex] = currentTime;
				lastTimeAutoUseSkill = currentTime;
				GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], isShortcut: true);
				if (debugMode)
				{
					GameScr.info1.addInfo($"Skill {skillIndex}: Delay={delay}ms, Use Now!");
				}
			}
		}
	}

	public static bool isMeCanAttackChar(Char ch)
	{
		if (TileMap.mapID == 113)
		{
			if (ch != null && Char.myCharz().myskill != null)
			{
				if (ch.cTypePk != 5)
				{
					return ch.cTypePk == 3;
				}
				return true;
			}
			return false;
		}
		if (ch != null && Char.myCharz().myskill != null)
		{
			if (ch.statusMe == 14 || ch.statusMe == 5 || Char.myCharz().myskill.template.type == 2 || ((Char.myCharz().cFlag != 8 || ch.cFlag == 0) && (Char.myCharz().cFlag == 0 || ch.cFlag != 8) && (Char.myCharz().cFlag == ch.cFlag || Char.myCharz().cFlag == 0 || ch.cFlag == 0) && (ch.cTypePk != 3 || Char.myCharz().cTypePk != 3) && Char.myCharz().cTypePk != 5 && ch.cTypePk != 5 && (Char.myCharz().cTypePk != 1 || ch.cTypePk != 1) && (Char.myCharz().cTypePk != 4 || ch.cTypePk != 4)))
			{
				if (Char.myCharz().myskill.template.type == 2)
				{
					return ch.cTypePk != 5;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private static bool isMeHasEnoughMP(Skill skillToUse)
	{
		if (skillToUse.template.manaUseType == 2)
		{
			return true;
		}
		if (skillToUse.template.manaUseType != 1)
		{
			return Char.myCharz().cMP >= skillToUse.manaUse;
		}
		return Char.myCharz().cMP >= skillToUse.manaUse * Char.myCharz().cMPFull / 100;
	}

	public static void SendAttackToCharFocus()
	{
		try
		{
			if (!Char.myCharz().isWaitMonkey)
			{
				MyVector myVector = new MyVector();
				myVector.addElement(Char.myCharz().charFocus);
				Service.gI().sendPlayerAttack(new MyVector(), myVector, 2);
			}
		}
		catch
		{
		}
	}

	public static void SendAttackToMobFocus()
	{
		try
		{
			MyVector myVector = new MyVector();
			myVector.addElement(Char.myCharz().mobFocus);
			Service.gI().sendPlayerAttack(myVector, new MyVector(), -1);
		}
		catch
		{
		}
	}

	private static long GetCoolDown(Skill skill)
	{
		if (skill.template.id != 20 && skill.template.id != 22 && skill.template.id != 7 && skill.template.id != 18 && skill.template.id != 23)
		{
			return skill.coolDown + 100;
		}
		return (long)skill.coolDown + 500L;
	}

	private static int GetMySkillIndex()
	{
		for (int i = 0; i < GameScr.keySkill.Length; i++)
		{
			if (GameScr.keySkill[i] == Char.myCharz().myskill)
			{
				return i;
			}
		}
		return 0;
	}

	static AutoSkill()
	{
		debugMode = false;
		isLoadKeySkill = true;
		lastTimeSendAttack = new long[10];
		isAutoUseSkills = new bool[10];
		lastTimeUseSkill = new long[10];
		timeAutoSkills = new long[10];
		inputDelay = new string[2] { "Nhập delay", "mili giây" };
		LoadData();
	}

	public static void FreezeSelectedSkill()
	{
		int mySkillIndex = GetMySkillIndex();
		GameScr.keySkill[mySkillIndex].coolDown = 0;
		GameScr.keySkill[mySkillIndex].manaUse = 0;
		GameScr.info1.addInfo("Đóng Băng\n" + GameScr.keySkill[mySkillIndex].template.name);
	}

	public static bool checkSkill(sbyte idSkill)
	{
		for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
		{
			if (Char.myCharz().vSkill.elementAt(i) is Skill skill && skill.template.id == idSkill)
			{
				return true;
			}
		}
		return false;
	}

	public static bool checkCanUseSkill(sbyte idSkill)
	{
		for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
		{
			if (Char.myCharz().vSkill.elementAt(i) is Skill skill && skill.template.id == idSkill && !skill.paintCanNotUseSkill)
			{
				if (Char.myCharz().myskill != skill)
				{
					GameScr.gI().doSelectSkill(skill, isShortcut: true);
				}
				return true;
			}
		}
		return false;
	}
}
