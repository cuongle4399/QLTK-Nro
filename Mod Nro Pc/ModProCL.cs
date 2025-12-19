using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Xmap;
using main.Mod;
using Mod.community;
using Mod.CuongLe;
using UnityEngine;

public class ModProCL : IActionListener
{
	private enum SuicideState
	{
		Idle,
		RequestFlag,
		AttackPlayer
	}

	private static ModProCL _Instance;

	public static float timeScale;

	public static bool isAttackBoss;

	public static long timeCanAttack;

	public static bool petw;

	public static int ntMin;

	public static int ntNow;

	public static bool tieuDietNguoiBatCo;

	public static List<Char> listNguoiCoDen;

	public static bool hienThiDoKH;

	public static int songlamccginua;

	public static bool banDo;

	public static bool catDoVIP;

	public static bool suicide;

	private static SuicideState currentState;

	private static float stateTimer;

	private static float delayDuration;

	private static readonly float flagDelay;

	private static readonly float attackDelay;

	public static short idNpcService;

	public static string nameIndex1;

	public static string nameIndex2;

	public static string nameIndex3;

	public static string s1SubName;

	public static string s2SubName;

	public static string s3SubName;

	public static string s1SubName2;

	public static string s2SubName2;

	public static string s3SubName2;

	public static bool confirming;

	public static long delayConfirm;

	public static bool runningopennpc;

	public static int currentStep;

	private static long confirmStartTime;

	private static readonly int confirmTimeout;

	private static readonly int stepDelay;

	private static long lastStepTime;

	private static bool NextInfoSuKien;

	private static string InfoSuKien;

	public static ModProCL getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new ModProCL();
		}
		return _Instance;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 22:
			AutoBossCL.tanCongBoss = false;
			tieuDietNguoiBatCo = !tieuDietNguoiBatCo;
			if (!tieuDietNguoiBatCo)
			{
				listNguoiCoDen.Clear();
			}
			else if (Char.myCharz().cFlag == 0)
			{
				Service.gI().getFlag(1, 8);
			}
			Char.myCharz().mobFocus = null;
			Char.myCharz().itemFocus = null;
			Char.myCharz().npcFocus = null;
			GameScr.info1.addInfo("Chế độ đồ sát người: " + (tieuDietNguoiBatCo ? " ON " : "OFF"));
			break;
		case 24:
			hienThiDoKH = !hienThiDoKH;
			if (hienThiDoKH)
			{
				MainMod.infoTrainGold = false;
				petw = false;
				MainMod.GoldCurrent = 0L;
			}
			break;
		case 25:
			GameScr.info1.addInfo("Tiến hành bug nv pem người bò mộng");
			startTuDamBanThan();
			break;
		case 26:
			break;
		case 27:
			banDo = !banDo;
			GameScr.info1.addInfo("|0|Bán đồ đang: " + (banDo ? "Bật" : "Tắt"));
			break;
		case 28:
			catDoVIP = !catDoVIP;
			GameScr.info1.addInfo("|0|Cất đồ kh+ spl vào rương: " + (catDoVIP ? "Bật" : "Tắt"));
			break;
		case 29:
			break;
		case 30:
			MainMod.infoTrainGold = !MainMod.infoTrainGold;
			if (MainMod.infoTrainGold)
			{
				hienThiDoKH = false;
				petw = false;
				MainMod.GoldCurrent = Char.myCharz().xu;
				MainMod.GoldUpdate = Char.myCharz().xu;
			}
			else
			{
				MainMod.GoldUpdateRealTime = 0L;
			}
			break;
		case 23:
			break;
		}
	}

	public static void AutoHealingPower2()
	{
		while (AutoPetCL.HealingPower)
		{
			AutoPetCL.SkillHealing();
			int coolDown = ((Skill)Char.myCharz().vSkillFight.elementAt(2)).coolDown;
			for (int i = 0; i < coolDown / 1000; i++)
			{
				if (!AutoPetCL.HealingPower)
				{
					break;
				}
				Thread.Sleep(1000);
			}
		}
	}

	public static Item FindItemBag(int id)
	{
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			if (Char.myCharz().arrItemBag[i] != null && Char.myCharz().arrItemBag[i].template.id == id)
			{
				return Char.myCharz().arrItemBag[i];
			}
		}
		return null;
	}

	public static void useItem(int x)
	{
		for (sbyte b = 0; b < Char.myCharz().arrItemBag.Length; b++)
		{
			if (Char.myCharz().arrItemBag[b] != null && Char.myCharz().arrItemBag[b].template.id == x)
			{
				Service.gI().useItem(0, 1, b, -1);
				break;
			}
		}
	}

	public static void onChatFromMe(string text, string to)
	{
		if (text.StartsWith("k_"))
		{
			try
			{
				int zoneId = int.Parse(text.Split('_')[1]);
				Service.gI().requestChangeZone(zoneId, -1);
			}
			catch
			{
			}
		}
		if (text.StartsWith("s_"))
		{
			try
			{
				int num = (MainMod.runSpeed = int.Parse(text.Split('_')[1]));
				GameScr.info1.addInfo("Tốc Độ Di Chuyển: " + num);
			}
			catch
			{
			}
		}
	}

	static ModProCL()
	{
		confirmStartTime = 0L;
		confirmTimeout = 5000;
		stepDelay = 500;
		lastStepTime = 0L;
		listNguoiCoDen = new List<Char>();
		songlamccginua = 500;
		currentState = SuicideState.Idle;
		stateTimer = 0f;
		delayDuration = 0f;
		flagDelay = 1.1f;
		attackDelay = 0.5f;
		nameIndex1 = "";
		nameIndex2 = "";
		nameIndex3 = "";
		s1SubName = "";
		s2SubName = "";
		s3SubName = "";
		s1SubName2 = "";
		s2SubName2 = "";
		s3SubName2 = "";
		delayConfirm = 0L;
		currentStep = 0;
		timeScale = 1.8f;
	}

	public static int ExistPotara()
	{
		int num = -1;
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		Item[] array = arrItemBag;
		Item[] array2 = array;
		Item[] array3 = array2;
		foreach (Item item in array3)
		{
			if (item == null)
			{
				continue;
			}
			int id = item.template.id;
			if (id == 454 || id == 921 || id == 1884)
			{
				if (id == 1884)
				{
					return 1884;
				}
				if (id > num)
				{
					num = id;
				}
			}
		}
		return num;
	}

	public static bool ExistItemBag(int x)
	{
		for (sbyte b = 0; b < Char.myCharz().arrItemBag.Length; b++)
		{
			if (Char.myCharz().arrItemBag[b] != null && Char.myCharz().arrItemBag[b].template.id == x)
			{
				return true;
			}
		}
		return false;
	}

	public static void isMeCanAttack()
	{
		if (!AutoSkill.isAutoSendAttack)
		{
			AutoSkill.isAutoSendAttack = true;
		}
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (MainMod.isBoss(obj))
			{
				Char.myCharz().charFocus = obj;
				if ((Math.Abs(Char.myCharz().cx - obj.cx) > 40 || Math.Abs(Char.myCharz().cy - obj.cy) > 40) && mSystem.currentTimeMillis() - timeCanAttack >= 500)
				{
					timeCanAttack = mSystem.currentTimeMillis();
					MainMod.TeleportTo(obj.cx, MainMod.GetYGround(obj.cx));
					Service.gI().charMove();
					Char.myCharz().cy = obj.cy;
					Service.gI().charMove();
				}
				break;
			}
		}
	}

	public static void update()
	{
		UpdateConfirmNpc();
		UpdateTuBuKU();
		if (AutoBossCL.tanCongBoss && !Char.myCharz().meDead && GameCanvas.gameTick % 30 == 0)
		{
			AutoBossCL.updateListBoss();
			if (AutoBossCL.listBossTrongKhu.Contains(Char.myCharz().charFocus))
			{
				ChienDau();
			}
		}
		if (tieuDietNguoiBatCo && !Char.myCharz().meDead && GameCanvas.gameTick % 30 == 0)
		{
			updateCoDen();
			if (Char.myCharz().cFlag == 0)
			{
				Service.gI().getFlag(1, 8);
				return;
			}
			Char charFocus = Char.myCharz().charFocus;
			if (charFocus == null || charFocus.cHP < 0 || charFocus.meDead || charFocus.cFlag == 0 || Res.distance(Char.myCharz().cx, Char.myCharz().cy, charFocus.cx, charFocus.cy) > 50)
			{
				for (int i = 0; i < listNguoiCoDen.Count; i++)
				{
					Char obj = listNguoiCoDen[i];
					if (GameScr.vCharInMap.contains(obj) && obj.cHP > 0)
					{
						Char.myCharz().mobFocus = null;
						Char.myCharz().npcFocus = null;
						Char.myCharz().itemFocus = null;
						Char.myCharz().charFocus = obj;
						if (!obj.meDead && obj.cHP > 0 && obj.cFlag != 0 && obj.charID > 0 && tieuDietNguoiBatCo && Res.distance(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().charFocus.cx, Char.myCharz().charFocus.cy) > 50)
						{
							MainXmapCL.TeleportTo(obj.cx, GetClosestGroundY(obj.cx, obj.cy));
							break;
						}
					}
				}
			}
		}
		if (tieuDietNguoiBatCo && listNguoiCoDen.Contains(Char.myCharz().charFocus) && Char.myCharz().charFocus.cFlag != 0 && GameCanvas.gameTick % 20 == 0 && !Char.myCharz().charFocus.meDead && Char.myCharz().charFocus.cHP > 0 && Char.myCharz().charFocus.cFlag != 0)
		{
			ChienDau();
		}
		if (isAttackBoss)
		{
			isMeCanAttack();
		}
	}

	public static void teleNPC(int idNpc)
	{
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(i);
			if (npc.template.npcTemplateId == idNpc)
			{
				Char.myCharz().npcFocus = npc;
				MainXmapCL.TeleportTo(npc.cx, npc.cy - 3);
				break;
			}
		}
	}

	public static int MyHPPercent()
	{
		return (int)(Char.myCharz().cHP * 100 / Char.myCharz().cHPFull);
	}

	public static bool SkipTau(Teleport teleport)
	{
		if (teleport.isMe)
		{
			if (teleport.type == 0)
			{
				Controller.isStopReadMessage = false;
				Char.ischangingMap = true;
				Teleport.vTeleport.removeElement(teleport);
			}
			else
			{
				if (Char.myCharz().isTeleport)
				{
					Char.myCharz().cy = (teleport.y = teleport.y2);
				}
				Char.myCharz().isTeleport = false;
			}
		}
		else
		{
			Char obj = GameScr.findCharInMap(teleport.id);
			if (obj != null)
			{
				if (teleport.type == 0)
				{
					if (teleport.isDown)
					{
						teleport.y = teleport.y2;
					}
				}
				else
				{
					if (obj.isTeleport)
					{
						obj.cy = (teleport.y = teleport.y2);
					}
					obj.isTeleport = false;
				}
			}
		}
		return true;
	}

	public static void ChienDau()
	{
		Skill skill = AutoTrainCL.ChooseSkill();
		if (skill != null)
		{
			if (skill == Char.myCharz().myskill && (skill.template.id == 0 || skill.template.id == 17 || skill.template.id == 4 || skill.template.id == 2 || skill.template.id == 9 || skill.template.id == 1 || skill.template.id == 5 || skill.template.id == 3))
			{
				AutoSkill.AutoSendAttack();
			}
			else
			{
				GameScr.gI().doSelectSkill(skill, isShortcut: true);
			}
		}
	}

	public static void updateCoDen()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (obj.cName != null && obj.cName != "" && !obj.isPet && !obj.isMiniPet && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$") && obj.cName != "Trọng tài" && obj.cFlag != 0 && !char.IsUpper(char.Parse(obj.cName.Substring(0, 1))) && obj.cHP > 0)
			{
				listNguoiCoDen.Add(obj);
			}
		}
	}

	public static bool checkItemTime(int idIcon)
	{
		for (int i = 0; i < Char.vItemTime.size(); i++)
		{
			if (((ItemTime)Char.vItemTime.elementAt(i)).idIcon == idIcon)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CheckBossMob(int idBossMod)
	{
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (!mob.isMobMe && mob.templateId == idBossMod && mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isDie && mob.x > 0 && mob.xFirst > 0)
			{
				return true;
			}
		}
		return false;
	}

	public static void startTuDamBanThan()
	{
		if (!suicide)
		{
			suicide = true;
			currentState = SuicideState.RequestFlag;
			stateTimer = 0f;
			delayDuration = flagDelay;
		}
	}

	public static void UpdateTuBuKU()
	{
		if (currentState == SuicideState.Idle)
		{
			return;
		}
		stateTimer += Time.deltaTime;
		switch (currentState)
		{
		case SuicideState.RequestFlag:
			if (stateTimer >= delayDuration)
			{
				if (Char.myCharz().cFlag == 0)
				{
					Service.gI().getFlag(1, 8);
					stateTimer = 0f;
					delayDuration = flagDelay;
				}
				else
				{
					currentState = SuicideState.AttackPlayer;
					stateTimer = 0f;
					delayDuration = attackDelay;
				}
			}
			break;
		case SuicideState.AttackPlayer:
			if (stateTimer >= delayDuration)
			{
				if (Char.myCharz().cHP > 0 && !Char.myCharz().meDead)
				{
					MyVector myVector = new MyVector();
					myVector.addElement(Char.myCharz());
					Service.gI().sendPlayerAttack(new MyVector(), myVector, -1);
					stateTimer = 0f;
					delayDuration = attackDelay;
				}
				else
				{
					suicide = false;
					currentState = SuicideState.Idle;
					stateTimer = 0f;
					delayDuration = 0f;
				}
			}
			break;
		}
	}

	public static bool isFULLBag()
	{
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			if (Char.myCharz().arrItemBag[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	public static bool isFULLBox()
	{
		for (int i = 0; i < Char.myCharz().arrItemBox.Length; i++)
		{
			if (Char.myCharz().arrItemBox[i] == null)
			{
				return false;
			}
		}
		return true;
	}

	public static int countEmptyBox()
	{
		int num = 0;
		for (int num2 = Char.myCharz().arrItemBox.Length - 1; num2 >= 0; num2--)
		{
			if (Char.myCharz().arrItemBox[num2] == null)
			{
				num++;
			}
		}
		return num;
	}

	public static int countEmptyBag()
	{
		int num = 0;
		for (int num2 = Char.myCharz().arrItemBag.Length - 1; num2 >= 0; num2--)
		{
			if (Char.myCharz().arrItemBag[num2] == null)
			{
				num++;
			}
		}
		return num;
	}

	public static void startComfirmNpc(short idnpc, string s1 = "", string s2 = "", string s3 = "", string s1Sub = "", string s2Sub = "", string s3Sub = "", string s1Sub2 = "", string s2Sub2 = "", string s3Sub2 = "", bool nextInfoSuKien = true, string infoSuKien = "Từ chối nhận kẹo")
	{
		idNpcService = idnpc;
		nameIndex1 = s1;
		nameIndex2 = s2;
		nameIndex3 = s3;
		s1SubName = s1Sub;
		s2SubName = s2Sub;
		s3SubName = s3Sub;
		s1SubName2 = s1Sub2;
		s2SubName2 = s2Sub2;
		s3SubName2 = s3Sub2;
		confirming = true;
		runningopennpc = false;
		currentStep = 0;
		confirmStartTime = Environment.TickCount;
		lastStepTime = 0L;
		NextInfoSuKien = nextInfoSuKien;
		InfoSuKien = infoSuKien;
	}

	public static void UpdateConfirmNpc()
	{
		if (!confirming)
		{
			return;
		}
		if (Environment.TickCount - confirmStartTime > confirmTimeout)
		{
			confirming = false;
			runningopennpc = false;
			currentStep = 0;
			GameCanvas.menu.doCloseMenu();
			return;
		}
		if (!GameCanvas.menu.showMenu && !runningopennpc)
		{
			Service.gI().openMenu(idNpcService);
			runningopennpc = true;
		}
		if (!GameCanvas.menu.showMenu || (lastStepTime > 0 && Environment.TickCount - lastStepTime < stepDelay))
		{
			return;
		}
		if (NextInfoSuKien && NextMap.nextSuKienHalloween)
		{
			if (SelectMenuByName(InfoSuKien))
			{
				InfoSuKien = "";
				runningopennpc = false;
				lastStepTime = Environment.TickCount;
				return;
			}
			NextInfoSuKien = false;
		}
		if (currentStep == 0 && !string.IsNullOrEmpty(nameIndex1))
		{
			if (SelectMenuByName(nameIndex1, s1SubName, s1SubName2))
			{
				nameIndex1 = "";
				s1SubName = "";
				s1SubName2 = "";
				currentStep = 1;
				lastStepTime = Environment.TickCount;
			}
		}
		else if (currentStep == 1 && !string.IsNullOrEmpty(nameIndex2))
		{
			if (SelectMenuByName(nameIndex2, s2SubName, s2SubName2))
			{
				nameIndex2 = "";
				s2SubName = "";
				s2SubName2 = "";
				currentStep = 2;
				lastStepTime = Environment.TickCount;
			}
		}
		else if (currentStep == 2 && !string.IsNullOrEmpty(nameIndex3) && SelectMenuByName(nameIndex3, s3SubName, s3SubName2))
		{
			nameIndex3 = "";
			s3SubName = "";
			s3SubName2 = "";
			currentStep = 3;
			lastStepTime = Environment.TickCount;
		}
		if (string.IsNullOrEmpty(nameIndex1) && string.IsNullOrEmpty(nameIndex2) && string.IsNullOrEmpty(nameIndex3))
		{
			confirming = false;
		}
	}

	private static string NormalizeText(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return "";
		}
		string text = new string(input.Where((char c) => !char.IsControl(c)).ToArray());
		return Regex.Replace(text.ToLower().Trim(), "\\s+", "");
	}

	private static bool SelectMenuByName(string nameIndex, string subName = "", string subName2 = "")
	{
		if (GameCanvas.menu.menuItems == null || GameCanvas.menu.menuItems.size() == 0)
		{
			return false;
		}
		string value = NormalizeText(nameIndex);
		string value2 = NormalizeText(subName);
		string value3 = NormalizeText(subName2);
		for (int i = 0; i < GameCanvas.menu.menuItems.size(); i++)
		{
			try
			{
				object obj = GameCanvas.menu.menuItems.elementAt(i);
				if (obj != null)
				{
					string text = NormalizeText(((Command)obj).caption ?? "");
					if (text.Equals(value) || (!string.IsNullOrEmpty(value2) && text.Equals(value2)) || (!string.IsNullOrEmpty(value3) && text.Equals(value3)))
					{
						GameCanvas.menu.menuSelectedItem = i;
						GameCanvas.menu.performSelect();
						GameCanvas.menu.doCloseMenu();
						return true;
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return false;
	}

	public static int GetClosestGroundY(int x, int targetY)
	{
		List<int> list = new List<int>();
		int num = 24;
		int i = 50;
		for (int mapEndY = TileMap.GetMapEndY(); i <= mapEndY; i += num)
		{
			if (TileMap.tileTypeAt(x, i, 2))
			{
				if (i % 24 != 0)
				{
					i -= i % 24;
				}
				list.Add(i);
			}
		}
		if (list.Count == 0)
		{
			GameScr.info1.addInfo($"Không tìm thấy mặt đất tại x={x}, sử dụng y={MainXmapCL.GetYGround(x)}");
			return MainXmapCL.GetYGround(x);
		}
		int result = list[0];
		int num2 = Math.Abs(list[0] - targetY);
		foreach (int item in list)
		{
			int num3 = Math.Abs(item - targetY);
			if (num3 < num2)
			{
				num2 = num3;
				result = item;
			}
		}
		return result;
	}
}
