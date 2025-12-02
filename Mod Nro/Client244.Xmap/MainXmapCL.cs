using System;
using System.Collections.Generic;
using Mod.CuongLe;
using UnityEngine;

namespace Client244.Xmap;

public class MainXmapCL : IActionListener, IChatable
{
	public static MainXmapCL _Instance;

	private XmapPathfinder pathfinder;

	private static int[] wayPointMapLeft = new int[2];

	private static int[] wayPointMapCenter = new int[2];

	private static int[] wayPointMapRight = new int[2];

	public static bool isXmaping;

	public static int IdMapEnd;

	public static bool isEatChicken = true;

	private static bool isHarvestPean;

	private static bool isUseCapsule = true;

	private static bool isUsingCapsule;

	private static bool isOpeningPanel;

	private static float lastTimeOpenedPanel;

	public static float lastWaitTime;

	private static float lastErrorTime;

	private static float lastItemUseTime;

	private static bool isUsingItem;

	public static bool xmapErrr;

	private static bool findNpc29to27;

	private static float lastMapChangeTime;

	private static int lastProcessedMap = -1;

	private static bool isProcessingMapChange;

	public static float customMapDelay = 0.5f;

	public static string tileChatDelay = "Delay Xamp";

	public static bool teleDirect;

	public static MainXmapCL getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new MainXmapCL();
		}
		return _Instance;
	}

	private MainXmapCL()
	{
		pathfinder = XmapPathfinder.GetInstance();
	}

	public static void Update()
	{
		if (Char.myCharz().meDead)
		{
			lastWaitTime = Time.realtimeSinceStartup + 1f;
			if (isXmaping && !AutoTrainCL.isGoBack && GameCanvas.gameTick % 100 == 0)
			{
				Service.gI().returnTownFromDead();
			}
		}
		else if (TileMap.mapID == IdMapEnd)
		{
			FinishXmap();
		}
		else
		{
			if (TryEatChicken() || !ShouldUpdateXmap() || GameCanvas.isWait())
			{
				return;
			}
			if (TileMap.mapID != lastProcessedMap)
			{
				lastProcessedMap = TileMap.mapID;
				lastMapChangeTime = Time.realtimeSinceStartup;
				isProcessingMapChange = false;
			}
			if (isProcessingMapChange)
			{
				float num = Time.realtimeSinceStartup - lastMapChangeTime;
				if (num < customMapDelay)
				{
					return;
				}
			}
			if (!HandleFutureMapSpecialCase())
			{
				UpdateXmap(IdMapEnd);
			}
		}
	}

	public static bool TryEatChicken()
	{
		if (!isEatChicken || (TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23))
		{
			return false;
		}
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			if ((itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1) && itemMap.template.id == 74)
			{
				Char.myCharz().itemFocus = itemMap;
				if (Time.realtimeSinceStartup - lastWaitTime > 0.6f)
				{
					lastWaitTime = Time.realtimeSinceStartup;
					Service.gI().pickItem(Char.myCharz().itemFocus.itemMapID);
				}
				return true;
			}
		}
		return false;
	}

	private static bool ShouldUpdateXmap()
	{
		if (!isXmaping)
		{
			return false;
		}
		if (Time.realtimeSinceStartup - lastWaitTime <= 0.3f)
		{
			return false;
		}
		if (Char.ischangingMap || Controller.isStopReadMessage)
		{
			return false;
		}
		int num = (GameScr.canAutoPlay ? 15 : 25);
		return GameCanvas.gameTick % num == 0;
	}

	private static bool HandleFutureMapSpecialCase()
	{
		if (!DataXmap.IsFutureMap(IdMapEnd))
		{
			return false;
		}
		if (Char.myCharz().taskMaint.taskId <= 24)
		{
			xmapErrr = true;
			return true;
		}
		if (GameScr.findNPCInMap(38) != null)
		{
			findNpc29to27 = false;
			return false;
		}
		switch (TileMap.mapID)
		{
		case 27:
			UpdateXmap(28);
			findNpc29to27 = false;
			return true;
		case 28:
			UpdateXmap(findNpc29to27 ? 27 : 29);
			return true;
		case 29:
			findNpc29to27 = true;
			UpdateXmap(28);
			return true;
		default:
			return false;
		}
	}

	public static void UpdateXmap(int mapID)
	{
		if (DataXmap.linkMaps.ContainsKey(999))
		{
			DataXmap.linkMaps.Remove(999);
		}
		DataXmap.linkMaps.Add(999, new List<NextMap>());
		DataXmap.linkMaps[999].Add(new NextMap(24 + Char.myCharz().cgender, 10, "OK"));
		if (IdMapEnd == 160 && !isUsingItem)
		{
			if (!ModProCL.ExistItemBag(992))
			{
				xmapErrr = true;
				return;
			}
			isUsingItem = true;
			lastItemUseTime = Time.realtimeSinceStartup;
			ModProCL.useItem(992);
			return;
		}
		if (IdMapEnd == 181 && !ModProCL.ExistItemBag(1852))
		{
			xmapErrr = true;
			return;
		}
		XmapPathfinder instance = XmapPathfinder.GetInstance();
		int[] array = instance.FindPath(mapID, TileMap.mapID, Char.myCharz().cPower, Char.myCharz().taskMaint.taskId > 30);
		if (array == null)
		{
			HandlePathNotFound(mapID);
		}
		else if (!TryUseCapsule(array) && !CheckClanRequirement(array) && (!isUsingItem || Time.realtimeSinceStartup - lastItemUseTime >= 0.5f))
		{
			if (isUsingItem && TileMap.mapID == 160)
			{
				isUsingItem = false;
			}
			isProcessingMapChange = true;
			GotoNextMap(array[1]);
		}
	}

	private static void HandlePathNotFound(int mapID)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup - lastErrorTime >= 1f)
		{
			XmapPathfinder instance = XmapPathfinder.GetInstance();
			string pathErrorMessage = instance.GetPathErrorMessage(mapID, TileMap.mapID, Char.myCharz().cPower, Char.myCharz().taskMaint.taskId > 30);
			GameScr.info1.addInfo(pathErrorMessage);
			lastErrorTime = realtimeSinceStartup;
			xmapErrr = true;
		}
	}

	private static bool TryUseCapsule(int[] path)
	{
		if (!isUseCapsule)
		{
			return false;
		}
		if (!isUsingCapsule && path.Length > 4)
		{
			Item[] arrItemBag = Char.myCharz().arrItemBag;
			Item[] array = arrItemBag;
			foreach (Item item in array)
			{
				if (item != null && (item.template.id == 194 || (item.template.id == 193 && item.quantity > 1)))
				{
					isUsingCapsule = true;
					isOpeningPanel = false;
					lastTimeOpenedPanel = Time.realtimeSinceStartup;
					GameCanvas.panel.mapNames = null;
					Service.gI().useItem(0, 1, -1, item.template.id);
					return true;
				}
			}
		}
		if (isUsingCapsule && !isOpeningPanel && Time.realtimeSinceStartup - lastTimeOpenedPanel < 0.5f)
		{
			return true;
		}
		if (isUsingCapsule && !isOpeningPanel && GameCanvas.panel.mapNames == null)
		{
			isUsingCapsule = false;
			isOpeningPanel = true;
			return true;
		}
		if (isUsingCapsule && !isOpeningPanel)
		{
			for (int num = path.Length - 1; num >= 1; num--)
			{
				string[] mapNames = GameCanvas.panel.mapNames;
				string[] array2 = mapNames;
				foreach (string text in array2)
				{
					if (text.Contains(TileMap.mapNames[path[num]]))
					{
						isOpeningPanel = true;
						Service.gI().requestMapSelect(Array.IndexOf(GameCanvas.panel.mapNames, text));
						return true;
					}
				}
			}
			isOpeningPanel = true;
		}
		return false;
	}

	private static bool CheckClanRequirement(int[] path)
	{
		if (path == null || path.Length == 0)
		{
			return true;
		}
		int mapID = TileMap.mapID;
		if (mapID != path[0] || Char.ischangingMap || Controller.isStopReadMessage)
		{
			return true;
		}
		if (Char.myCharz().clan != null)
		{
			return false;
		}
		if (DataXmap.khiGasMapSet.Contains(IdMapEnd) || DataXmap.manhVoBTMapSet.Contains(IdMapEnd) || (IdMapEnd >= 53 && IdMapEnd <= 62))
		{
			xmapErrr = true;
			return true;
		}
		return false;
	}

	private static void GotoNextMap(int nextMapID)
	{
		XmapPathfinder instance = XmapPathfinder.GetInstance();
		instance.FindNextMapToGo(TileMap.mapID, nextMapID)?.GotoMap();
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			ShowPlanetMenu();
			break;
		case 2:
			ToggleSetting(ref isEatChicken, "Ăn Đùi Gà", "AutoMapIsEatChicken");
			break;
		case 3:
			ToggleSetting(ref isHarvestPean, "Thu Đậu", "AutoMapIsHarvestPean");
			break;
		case 4:
			ToggleSetting(ref isUseCapsule, "Sử Dụng Capsule", "AutoMapIsUseCsb");
			break;
		case 5:
			SaveData();
			break;
		case 6:
			ShowMapsMenu((int[])p);
			break;
		case 7:
			StartGoToMap((int)p);
			break;
		case 8:
			FinishXmap();
			break;
		case 9:
		{
			int[] array = new int[7] { 300, 500, 1000, 1500, 2000, 2500, 3000 };
			MyVector myVector = new MyVector();
			int[] array2 = array;
			foreach (int num in array2)
			{
				myVector.addElement(new Command($"{num} mili giây", getInstance(), 10, num));
			}
			myVector.addElement(new Command("Tùy chỉnh", getInstance(), 12, null));
			GameCanvas.menu.startAt(myVector, 3);
			break;
		}
		case 10:
			SetMapDelayFromMs((int)p);
			break;
		case 11:
			teleDirect = !teleDirect;
			ChatPopup.addChatPopupMultiLineGameline("Đã lưu Kiểu Xmap: " + (teleDirect ? "Tele trực tiếp" : "Chạy bộ qua map") + " cho lần sau");
			Rms.saveRMSInt("TypeXmap", teleDirect ? 1 : 0);
			break;
		case 12:
			ChatTextField.gI().strChat = tileChatDelay;
			ChatTextField.gI().tfChat.name = "Nhập mili giây (300-5000)";
			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			ChatTextField.gI().startChat2(getInstance(), "");
			break;
		}
	}

	private static void ToggleSetting(ref bool setting, string name, string rmsKey)
	{
		setting = !setting;
		GameScr.info1.addInfo(name + "\n" + (setting ? "[STATUS: ON]" : "[STATUS: OFF]"));
        ShowMenu();
    }

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		if (isXmaping)
		{
			myVector.addElement(new Command("Dừng load map", getInstance(), 8, null));
		}
		myVector.addElement(new Command("Load Map", getInstance(), 1, null));
		myVector.addElement(new Command($"Delay: {customMapDelay * 1000f} mili giây", getInstance(), 9, null));
		myVector.addElement(new Command("Loại: " + (teleDirect ? "Tele" : "Chạy bộ"), getInstance(), 11, null));
		myVector.addElement(new Command("Ăn Đùi Gà\n" + (isEatChicken ? "[ON]" : "[OFF]"), getInstance(), 2, null));
		myVector.addElement(new Command("Thu Đậu\n" + (isHarvestPean ? "[ON]" : "[OFF]"), getInstance(), 3, null));
		myVector.addElement(new Command("Dùng Capsule\n" + (isUseCapsule ? "[ON]" : "[OFF]"), getInstance(), 4, null));
		myVector.addElement(new Command("Lưu cài đặt", getInstance(), 5, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ShowPlanetMenu()
	{
		MyVector myVector = new MyVector();
		foreach (KeyValuePair<string, int[]> item in DataXmap.planetDictionary)
		{
			myVector.addElement(new Command(item.Key, getInstance(), 6, item.Value));
		}
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ShowMapsMenu(int[] mapIDs)
	{
		MyVector myVector = new MyVector();
		int cgender = Char.myCharz().cgender;
		foreach (int num in mapIDs)
		{
			if (IsMapValidForGender(num, cgender))
			{
				myVector.addElement(new Command(GetMapName(num), getInstance(), 7, num));
			}
		}
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static bool IsMapValidForGender(int mapID, int gender)
	{
		return (gender != 0 || (mapID != 22 && mapID != 23)) && (gender != 1 || (mapID != 21 && mapID != 23)) && (gender != 2 || (mapID != 21 && mapID != 22));
	}

	private static string GetMapName(int mapID)
	{
		if (1 == 0)
		{
		}
		string result = mapID switch
		{
			129 => $"{TileMap.mapNames[mapID]} 23\n[{mapID}]", 
			113 => $"Siêu hạng\n[{mapID}]", 
			_ => $"{TileMap.mapNames[mapID]}\n[{mapID}]", 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static void StartGoToMap(int mapID)
	{
		isXmaping = true;
		IdMapEnd = mapID;
		lastProcessedMap = -1;
		isProcessingMapChange = false;
	}

	public static void FinishXmap()
	{
		isXmaping = false;
		isUsingCapsule = false;
		isOpeningPanel = false;
		xmapErrr = false;
		lastProcessedMap = -1;
		isProcessingMapChange = false;
	}

	public static void LoadData()
	{
		int num = Rms.loadRMSIntVIP("AutoMapDelay");
		if (num >= 300 && num <= 5000)
		{
			customMapDelay = (float)num / 1000f;
		}
		else
		{
			customMapDelay = 0.3f;
		}
		teleDirect = Rms.loadRMSInt("TypeXmap") == 1;
		isEatChicken = Rms.loadRMSInt("AutoMapIsEatChicken") != 0;
		isUseCapsule = Rms.loadRMSInt("AutoMapIsUseCsb") != 0;
		isHarvestPean = Rms.loadRMSInt("AutoMapIsHarvestPean") == 1;
	}

	private static void SaveData()
	{
		Rms.saveRMSInt("AutoMapIsEatChicken", isEatChicken ? 1 : 0);
		Rms.saveRMSInt("AutoMapIsHarvestPean", isHarvestPean ? 1 : 0);
		Rms.saveRMSInt("AutoMapIsUseCsb", isUseCapsule ? 1 : 0);
		ChatPopup.addChatPopupMultiLineGameline("Đã lưu dữ liệu thành công");
	}

	public static void SetMapDelayFromMs(int milliseconds)
	{
		if (milliseconds < 300 || milliseconds > 5000)
		{
			GameScr.info1.addInfo("Lỗi: Delay phải từ 300-5000 mili giây!");
			return;
		}
		customMapDelay = (float)milliseconds / 1000f;
		Rms.saveRMSIntVIP("AutoMapDelay", milliseconds);
		Npc npc = new Npc(0, 0, 0, 0, 0, GameScr.info1.charId[Char.myCharz().cgender][2]);
		ChatPopup.addChatPopupMultiLineGameline($"Đã lưu cho lần sau Delay: {milliseconds} mili giây");
	}

	private static void LoadWaypointsInMap()
	{
		ResetSavedWaypoints();
		int num = TileMap.vGo.size();
		if (num != 2)
		{
			LoadMultipleWaypoints(num);
		}
		else
		{
			LoadTwoWaypoints();
		}
	}

	private static void LoadMultipleWaypoints(int count)
	{
		for (int i = 0; i < count; i++)
		{
			Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
			if (waypoint.maxX < 60)
			{
				wayPointMapLeft[0] = waypoint.minX + 15;
				wayPointMapLeft[1] = waypoint.maxY;
			}
			else if (waypoint.maxX > TileMap.pxw - 60)
			{
				wayPointMapRight[0] = waypoint.maxX - 15;
				wayPointMapRight[1] = waypoint.maxY;
			}
			else
			{
				wayPointMapCenter[0] = waypoint.minX + 15;
				wayPointMapCenter[1] = waypoint.maxY;
			}
		}
	}

	private static void LoadTwoWaypoints()
	{
		Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(0);
		Waypoint waypoint2 = (Waypoint)TileMap.vGo.elementAt(1);
		bool flag = waypoint.maxX < 60 && waypoint2.maxX < 60;
		bool flag2 = waypoint.minX > TileMap.pxw - 60 && waypoint2.minX > TileMap.pxw - 60;
		if (flag || flag2)
		{
			wayPointMapLeft[0] = waypoint.minX + 15;
			wayPointMapLeft[1] = waypoint.maxY;
			wayPointMapRight[0] = waypoint2.maxX - 15;
			wayPointMapRight[1] = waypoint2.maxY;
		}
		else if (waypoint.maxX < waypoint2.maxX)
		{
			wayPointMapLeft[0] = waypoint.minX + 15;
			wayPointMapLeft[1] = waypoint.maxY;
			wayPointMapRight[0] = waypoint2.maxX - 15;
			wayPointMapRight[1] = waypoint2.maxY;
		}
		else
		{
			wayPointMapLeft[0] = waypoint2.minX + 15;
			wayPointMapLeft[1] = waypoint2.maxY;
			wayPointMapRight[0] = waypoint.maxX - 15;
			wayPointMapRight[1] = waypoint.maxY;
		}
	}

	private static void ResetSavedWaypoints()
	{
		wayPointMapLeft = new int[2];
		wayPointMapCenter = new int[2];
		wayPointMapRight = new int[2];
	}

	public static int GetYGround(int x)
	{
		int num = 50;
		int num2 = 0;
		while (num2 < 30)
		{
			num2++;
			num += 24;
			if (TileMap.tileTypeAt(x, num, 2))
			{
				if (num % 24 != 0)
				{
					num -= num % 24;
				}
				break;
			}
		}
		return num;
	}

	public static void TeleportTo(int x, int y)
	{
		if (GameScr.canAutoPlay)
		{
			Char.myCharz().cx = x;
			Char.myCharz().cy = y;
			Service.gI().charMove();
			return;
		}
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
		Char.myCharz().cy = y + 1;
		Service.gI().charMove();
		Char.myCharz().cy = y;
		Service.gI().charMove();
	}

	public static void LoadMapLeft()
	{
		LoadMap(0);
	}

	public static void LoadMapCenter()
	{
		LoadMap(2);
	}

	public static void LoadMapRight()
	{
		LoadMap(1);
	}

	private static void LoadMap(int position)
	{
		if (DataXmap.IsNRDMap(TileMap.mapID))
		{
			TeleportInNRDMap(position);
			return;
		}
		LoadWaypointsInMap();
		switch (position)
		{
		case 0:
			if (wayPointMapLeft[0] != 0 && wayPointMapLeft[1] != 0)
			{
				TeleportTo(wayPointMapLeft[0], wayPointMapLeft[1]);
			}
			else
			{
				TeleportTo(60, GetYGround(60));
			}
			break;
		case 1:
			if (wayPointMapRight[0] != 0 && wayPointMapRight[1] != 0)
			{
				TeleportTo(wayPointMapRight[0], wayPointMapRight[1]);
			}
			else
			{
				TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
			}
			break;
		case 2:
			if (wayPointMapCenter[0] != 0 && wayPointMapCenter[1] != 0)
			{
				TeleportTo(wayPointMapCenter[0], wayPointMapCenter[1]);
			}
			else
			{
				TeleportTo(TileMap.pxw / 2, GetYGround(TileMap.pxw / 2));
			}
			break;
		}
		Service.gI().charMove();
		if (TileMap.mapID != 7 && TileMap.mapID != 14 && TileMap.mapID != 0)
		{
			Service.gI().requestChangeMap();
		}
		else
		{
			Service.gI().getMapOffline();
		}
		Char.ischangingMap = true;
	}

	private static void TeleportInNRDMap(int position)
	{
		switch (position)
		{
		case 0:
			TeleportTo(60, GetYGround(60));
			break;
		case 1:
			TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
			break;
		case 2:
			TeleportToNRDNpc();
			break;
		}
	}

	private static void TeleportToNRDNpc()
	{
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(i);
			if (npc.template.npcTemplateId >= 30 && npc.template.npcTemplateId <= 36)
			{
				Char.myCharz().npcFocus = npc;
				TeleportTo(npc.cx, npc.cy - 3);
				break;
			}
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (text == null || text.Trim().Length == 0 || ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Trim().Length == 0)
		{
			ChatTextField.gI().isShow = false;
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(tileChatDelay))
		{
			if (int.TryParse(text, out var result))
			{
				SetMapDelayFromMs(result);
			}
			else
			{
				GameScr.info1.addInfo("Lỗi: chỉ được nhập số nguyên!");
			}
			ResetChatTextField();
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

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().isShow = false;
	}
}
