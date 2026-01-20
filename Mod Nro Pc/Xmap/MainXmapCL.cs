using Mod.CuongLe;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Xmap;

public class MainXmapCL : IActionListener, IChatable
{
    #region Singleton
    public static MainXmapCL _Instance;

    public static MainXmapCL getInstance()
    {
        if (_Instance == null)
            _Instance = new MainXmapCL();
        return _Instance;
    }

    private MainXmapCL()
    {
        pathfinder = XmapPathfinder.GetInstance();
    }
    #endregion

    #region Constants
    private const float UPDATE_INTERVAL = 0.4f;
    private const float ERROR_COOLDOWN = 1f;
    private const float ITEM_USE_DELAY = 0.5f;
    private const float CHICKEN_PICKUP_DELAY = 0.6f;
    private const float CAPSULE_PANEL_DELAY = 0.5f;
    private const int CHICKEN_ITEM_ID = 74;
    private const int CAPSULE_194_ID = 194;
    private const int CAPSULE_193_ID = 193;
    private const int TEMPORARY_MAP_999 = 999;
    private const int MIN_PATH_LENGTH_FOR_CAPSULE = 4;
    #endregion

    #region Fields - Core
    private XmapPathfinder pathfinder;
    #endregion

    #region Fields - State Management
    public static bool isXmaping;
    public static int IdMapEnd;
    public static bool xmapErrr;
    public static float lastWaitTime;

    private static int lastProcessedMap = -1;
    private static bool isProcessingMapChange;
    private static float lastMapChangeTime;
    private static float lastErrorTime;
    private static float lastNpcIndexActionTime;
    #endregion

    #region Fields - Settings
    public static bool isEatChicken = true;
    public static bool isUseCapsule = true;
    public static bool teleDirect;
    public static float customMapDelay = 0.5f;
    public static string tileChatDelay = "Delay Xamp";

    private static bool isHarvestPean;
    #endregion

    #region Fields - Capsule System
    private static bool isUsingCapsule;
    private static bool isOpeningPanel;
    private static float lastTimeOpenedPanel;
    #endregion

    #region Fields - Future Map Special Logic
    private static bool findNpc29to27;
    #endregion

    #region Fields - Waypoint Cache
    private static int[] wayPointMapLeft = new int[2];
    private static int[] wayPointMapCenter = new int[2];
    private static int[] wayPointMapRight = new int[2];
    #endregion

    #region Main Update Loop
    public static void Update()
    {
        NextMap.UpdateConfirmNpc();

        Char me = Char.myCharz();
        float now = Time.realtimeSinceStartup;
        int currentMap = TileMap.mapID;

        if (HandleDeathState(me, now)) return;
        if (HandleDestinationReached(currentMap)) return;
        if (!ShouldContinueUpdate(now)) return;

        // Check delay cho NPC index action
        if (IsWaitingForNpcIndexDelay(now))
            return;

        HandleMapChange(currentMap, now);

        if (isProcessingMapChange && now - lastMapChangeTime < customMapDelay)
            return;

        if (!HandleFutureMapSpecialCase())
            UpdateXmap(IdMapEnd);
    }

    private static bool HandleDeathState(Char me, float now)
    {
        if (!me.meDead) return false;

        lastWaitTime = now + 1f;
        if (isXmaping && !AutoTrainCL.isGoBack && GameCanvas.gameTick % 100 == 0)
            Service.gI().returnTownFromDead();

        return true;
    }

    private static bool HandleDestinationReached(int currentMap)
    {
        if (currentMap != IdMapEnd) return false;
        FinishXmap();
        return true;
    }

    private static bool ShouldContinueUpdate(float now)
    {
        if (TryEatChicken()) return false;
        if (!ShouldUpdateXmap(now)) return false;
        if (GameCanvas.isWait()) return false;
        return true;
    }

    private static void HandleMapChange(int currentMap, float now)
    {
        if (currentMap != lastProcessedMap)
        {
            lastProcessedMap = currentMap;
            lastMapChangeTime = now;
            isProcessingMapChange = false;
        }
    }
    #endregion

    #region Update Conditions
    private static bool ShouldUpdateXmap(float now)
    {
        if (!isXmaping) return false;
        if (now - lastWaitTime <= UPDATE_INTERVAL) return false;
        if (Char.ischangingMap || Controller.isStopReadMessage) return false;

        int mod = GameScr.canAutoPlay ? 15 : 35;
        return GameCanvas.gameTick % mod == 0;
    }

    private static bool IsWaitingForNpcIndexDelay(float now)
    {
        if (lastNpcIndexActionTime > 0)
        {
            if (now - lastNpcIndexActionTime < customMapDelay + 1.2f)
            {
                return true;
            }
            else
            {
                // Reset sau khi delay đã qua
                lastNpcIndexActionTime = 0;
            }
        }
        return false;
    }
    #endregion

    #region Chicken Pickup System
    public static bool TryEatChicken()
    {
        if (!ShouldTryEatChicken()) return false;

        float now = Time.realtimeSinceStartup;
        int size = GameScr.vItemMap.size();

        for (int i = 0; i < size; i++)
        {
            ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
            if (IsChickenItem(itemMap))
            {
                PickupChicken(itemMap, now);
                return true;
            }
        }
        return false;
    }

    private static bool ShouldTryEatChicken()
    {
        if (!isEatChicken) return false;
        int mapID = TileMap.mapID;
        return mapID == 21 || mapID == 22 || mapID == 23;
    }

    private static bool IsChickenItem(ItemMap itemMap)
    {
        if (itemMap.template.id != CHICKEN_ITEM_ID) return false;
        int myCharID = Char.myCharz().charID;
        return itemMap.playerId == myCharID || itemMap.playerId == -1;
    }

    private static void PickupChicken(ItemMap itemMap, float now)
    {
        Char.myCharz().itemFocus = itemMap;
        if (now - lastWaitTime > CHICKEN_PICKUP_DELAY)
        {
            lastWaitTime = now;
            Service.gI().pickItem(itemMap.itemMapID);
        }
    }
    #endregion

    #region Future Map Special Case
    private static bool HandleFutureMapSpecialCase()
    {
        if (!DataXmap.IsFutureMap(IdMapEnd))
            return false;

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

        return ProcessFutureMapNavigation();
    }

    private static bool ProcessFutureMapNavigation()
    {
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
    #endregion

    #region Main Navigation Logic
    public static void UpdateXmap(int mapID)
    {
        Char me = Char.myCharz();
        float now = Time.realtimeSinceStartup;

        SetupGenderPortal(me);

        int[] path = FindPathToDestination(me, mapID);

        if (path == null)
        {
            HandlePathNotFound(mapID);
            return;
        }

        if (TryUseCapsule(path)) return;
        if (CheckClanRequirement(path)) return;

        isProcessingMapChange = true;
        GotoNextMap(path[1]);
    }

    private static void SetupGenderPortal(Char me)
    {
        if (!DataXmap.linkMaps.ContainsKey(TEMPORARY_MAP_999))
            DataXmap.linkMaps[TEMPORARY_MAP_999] = new List<NextMap>();

        var list = DataXmap.linkMaps[TEMPORARY_MAP_999];
        list.Clear();
        list.Add(new NextMap(24 + me.cgender, 10, "OK"));
    }

    private static int[] FindPathToDestination(Char me, int mapID)
    {
        return XmapPathfinder.GetInstance().FindPath(
            mapID,
            TileMap.mapID,
            me.cPower,
            me.taskMaint.taskId > 30
        );
    }

    private static void GotoNextMap(int nextMapID)
    {
        XmapPathfinder.GetInstance()
            .FindNextMapToGo(TileMap.mapID, nextMapID)
            ?.GotoMap();
    }
    #endregion

    #region Error Handling
    private static void HandlePathNotFound(int mapID)
    {
        float now = Time.realtimeSinceStartup;
        if (now - lastErrorTime < ERROR_COOLDOWN)
            return;

        string msg = XmapPathfinder.GetInstance().GetPathErrorMessage(
            mapID,
            TileMap.mapID,
            Char.myCharz().cPower,
            Char.myCharz().taskMaint.taskId > 30
        );

        GameScr.info1.addInfo(msg);
        lastErrorTime = now;
        xmapErrr = true;
    }
    #endregion

    #region Capsule System
    private static bool TryUseCapsule(int[] path)
    {
        if (!isUseCapsule) return false;

        if (ShouldInitializeCapsule(path))
        {
            InitializeCapsuleUse();
            return true;
        }

        if (IsWaitingForPanel())
            return true;

        if (ShouldResetCapsuleState())
        {
            ResetCapsuleState();
            return true;
        }

        if (isUsingCapsule && !isOpeningPanel)
            return TrySelectCapsuleDestination(path);

        return false;
    }

    private static bool ShouldInitializeCapsule(int[] path)
    {
        if (isUsingCapsule) return false;
        if (path.Length <= MIN_PATH_LENGTH_FOR_CAPSULE) return false;

        Item[] arrItemBag = Char.myCharz().arrItemBag;
        foreach (Item item in arrItemBag)
        {
            if (item != null && IsCapsuleItem(item))
                return true;
        }
        return false;
    }

    private static bool IsCapsuleItem(Item item)
    {
        return item.template.id == CAPSULE_194_ID ||
               (item.template.id == CAPSULE_193_ID && item.quantity > 1);
    }

    private static void InitializeCapsuleUse()
    {
        Item capsule = FindCapsuleInBag();
        if (capsule == null) return;

        isUsingCapsule = true;
        isOpeningPanel = false;
        lastTimeOpenedPanel = Time.realtimeSinceStartup;
        GameCanvas.panel.mapNames = null;
        Service.gI().useItem(0, 1, -1, capsule.template.id);
    }

    private static Item FindCapsuleInBag()
    {
        Item[] arrItemBag = Char.myCharz().arrItemBag;
        foreach (Item item in arrItemBag)
        {
            if (item != null && IsCapsuleItem(item))
                return item;
        }
        return null;
    }

    private static bool IsWaitingForPanel()
    {
        return isUsingCapsule &&
               !isOpeningPanel &&
               Time.realtimeSinceStartup - lastTimeOpenedPanel < CAPSULE_PANEL_DELAY;
    }

    private static bool ShouldResetCapsuleState()
    {
        return isUsingCapsule &&
               !isOpeningPanel &&
               GameCanvas.panel.mapNames == null;
    }

    private static void ResetCapsuleState()
    {
        isUsingCapsule = false;
        isOpeningPanel = true;
    }

    private static bool TrySelectCapsuleDestination(int[] path)
    {
        for (int i = path.Length - 1; i >= 1; i--)
        {
            string targetMapName = TileMap.mapNames[path[i]];

            for (int j = 0; j < GameCanvas.panel.mapNames.Length; j++)
            {
                if (GameCanvas.panel.mapNames[j].Contains(targetMapName))
                {
                    isOpeningPanel = true;
                    Service.gI().requestMapSelect(j);
                    return true;
                }
            }
        }

        isOpeningPanel = true;
        return false;
    }
    #endregion

    #region Clan Requirement Check
    private static bool CheckClanRequirement(int[] path)
    {
        if (path == null || path.Length == 0) return true;
        if (TileMap.mapID != path[0]) return true;
        if (Char.ischangingMap || Controller.isStopReadMessage) return true;
        if (Char.myCharz().clan != null) return false;

        if (DataXmap.RequiresClan(IdMapEnd))
        {
            xmapErrr = true;
            return true;
        }

        return false;
    }
    #endregion

    #region Xmap Control
    public static void StartGoToMap(int mapID)
    {
        isXmaping = true;
        IdMapEnd = mapID;
        lastProcessedMap = -1;
        isProcessingMapChange = false;
        xmapErrr = false;
    }

    public static void FinishXmap()
    {
        isXmaping = false;
        isUsingCapsule = false;
        isOpeningPanel = false;
        xmapErrr = false;
        lastProcessedMap = -1;
        isProcessingMapChange = false;
        lastNpcIndexActionTime = 0;
    }

    public static void SetNpcIndexActionTime(float time)
    {
        lastNpcIndexActionTime = time;
    }
    #endregion

    #region Menu System
    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 1: ShowPlanetMenu(); break;
            case 2: ToggleSetting(ref isEatChicken, "Ăn Đùi Gà", "AutoMapIsEatChicken"); break;
            case 3: ToggleSetting(ref isHarvestPean, "Thu Đậu", "AutoMapIsHarvestPean"); break;
            case 4: ToggleSetting(ref isUseCapsule, "Sử Dụng Capsule", "AutoMapIsUseCsb"); break;
            case 5: SaveData(); break;
            case 6: ShowMapsMenu((int[])p); break;
            case 7: StartGoToMap((int)p); break;
            case 8: FinishXmap(); break;
            case 9: ShowDelayMenu(); break;
            case 10: SetMapDelayFromMs((int)p); break;
            case 11: ToggleTeleMode(); break;
            case 12: ShowCustomDelayInput(); break;
        }
    }

    private static void ToggleSetting(ref bool setting, string name, string rmsKey)
    {
        setting = !setting;
        string status = setting ? "[STATUS: ON]" : "[STATUS: OFF]";
        GameScr.info1.addInfo($"{name}\n{status}");
        ShowMenu();
    }

    public static void ShowMenu()
    {
        MyVector myVector = new MyVector();

        if (isXmaping)
            myVector.addElement(new Command("Dừng load map", getInstance(), 8, null));

        myVector.addElement(new Command("Load Map", getInstance(), 1, null));
        myVector.addElement(new Command($"Delay: {customMapDelay * 1000f} mili giây", getInstance(), 9, null));
        myVector.addElement(new Command($"Loại: {(teleDirect ? "Tele" : "Chạy bộ")}", getInstance(), 11, null));
        myVector.addElement(new Command($"Ăn Đùi Gà\n{(isEatChicken ? "[ON]" : "[OFF]")}", getInstance(), 2, null));
        myVector.addElement(new Command($"Thu Đậu\n{(isHarvestPean ? "[ON]" : "[OFF]")}", getInstance(), 3, null));
        myVector.addElement(new Command($"Dùng Capsule\n{(isUseCapsule ? "[ON]" : "[OFF]")}", getInstance(), 4, null));
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

        foreach (int mapID in mapIDs)
        {
            if (IsMapValidForGender(mapID, cgender))
            {
                myVector.addElement(new Command(GetMapName(mapID), getInstance(), 7, mapID));
            }
        }
        GameCanvas.menu.startAt(myVector, 3);
    }

    private static void ShowDelayMenu()
    {
        int[] delays = { 300, 500, 1000, 1500, 2000, 2500, 3000 };
        MyVector myVector = new MyVector();

        foreach (int delay in delays)
        {
            myVector.addElement(new Command($"{delay} mili giây", getInstance(), 10, delay));
        }

        myVector.addElement(new Command("Tùy chỉnh", getInstance(), 12, null));
        GameCanvas.menu.startAt(myVector, 3);
    }

    private static void ToggleTeleMode()
    {
        teleDirect = !teleDirect;
        string mode = teleDirect ? "Tele trực tiếp" : "Chạy bộ qua map";
        ChatPopup.addChatPopupMultiLineGameline($"Đã lưu Kiểu Xmap: {mode} cho lần sau");
        Rms.saveRMSInt("TypeXmap", teleDirect ? 1 : 0);
    }

    private static void ShowCustomDelayInput()
    {
        ChatTextField.gI().strChat = tileChatDelay;
        ChatTextField.gI().tfChat.name = "Nhập mili giây (300-5000)";
        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
        ChatTextField.gI().startChat2(getInstance(), "");
    }

    private static bool IsMapValidForGender(int mapID, int gender)
    {
        if (gender == 0 && (mapID == 22 || mapID == 23)) return false;
        if (gender == 1 && (mapID == 21 || mapID == 23)) return false;
        if (gender == 2 && (mapID == 21 || mapID == 22)) return false;
        return true;
    }

    private static string GetMapName(int mapID)
    {
        return mapID switch
        {
            129 => $"{TileMap.mapNames[mapID]} 23\n[{mapID}]",
            113 => $"Siêu hạng\n[{mapID}]",
            _ => $"{TileMap.mapNames[mapID]}\n[{mapID}]"
        };
    }
    #endregion

    #region Data Persistence
    public static void LoadData()
    {
        int delay = Rms.loadRMSIntVIP("AutoMapDelay");
        customMapDelay = (delay >= 300 && delay <= 5000) ? delay / 1000f : 0.3f;

        teleDirect = Rms.loadRMSInt("TypeXmap") == 1;
        isEatChicken = Rms.loadRMSInt("AutoMapIsEatChicken") != 0;
        isUseCapsule = Rms.loadRMSInt("AutoMapIsUseCsb") != 0;
        isHarvestPean = Rms.loadRMSInt("AutoMapIsHarvestPean") == 1;
        NextMap.InfoTextMenuXmap = File.ReadAllText("Data//TextNpcXmap.ini");
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

        customMapDelay = milliseconds / 1000f;
        Rms.saveRMSIntVIP("AutoMapDelay", milliseconds);
        ChatPopup.addChatPopupMultiLineGameline($"Đã lưu cho lần sau Delay: {milliseconds} mili giây");
    }
    #endregion

    #region Chat Interface
    public void onChatFromMe(string text, string to)
    {
        if (string.IsNullOrEmpty(text) ||
     string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) ||
     text.Trim().Length == 0 ||
     ChatTextField.gI().tfChat.getText().Trim().Length == 0)
        {
            ChatTextField.gI().isShow = false;
            ResetChatTextField();
            return;
        }


        if (ChatTextField.gI().strChat.Equals(tileChatDelay))
        {
            if (int.TryParse(text, out int result))
                SetMapDelayFromMs(result);
            else
                GameScr.info1.addInfo("Lỗi: chỉ được nhập số nguyên!");

            ResetChatTextField();
        }
        else
        {
            ResetChatTextField();
            Service.gI().chat(text);
        }
    }

    public void onCancelChat() { }

    private static void ResetChatTextField()
    {
        ChatTextField.gI().strChat = "Chat";
        ChatTextField.gI().tfChat.name = "chat";
        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
        ChatTextField.gI().isShow = false;
    }
    #endregion

    #region Waypoint Utilities (Legacy Support)
    private static void LoadWaypointsInMap()
    {
        ResetSavedWaypoints();
        int count = TileMap.vGo.size();

        if (count != 2)
            LoadMultipleWaypoints(count);
        else
            LoadTwoWaypoints();
    }

    private static void LoadMultipleWaypoints(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Waypoint wp = (Waypoint)TileMap.vGo.elementAt(i);

            if (wp.maxX < 60)
            {
                wayPointMapLeft[0] = wp.minX + 15;
                wayPointMapLeft[1] = wp.maxY;
            }
            else if (wp.maxX > TileMap.pxw - 60)
            {
                wayPointMapRight[0] = wp.maxX - 15;
                wayPointMapRight[1] = wp.maxY;
            }
            else
            {
                wayPointMapCenter[0] = wp.minX + 15;
                wayPointMapCenter[1] = wp.maxY;
            }
        }
    }

    private static void LoadTwoWaypoints()
    {
        Waypoint wp1 = (Waypoint)TileMap.vGo.elementAt(0);
        Waypoint wp2 = (Waypoint)TileMap.vGo.elementAt(1);

        bool bothLeft = wp1.maxX < 60 && wp2.maxX < 60;
        bool bothRight = wp1.minX > TileMap.pxw - 60 && wp2.minX > TileMap.pxw - 60;

        if (bothLeft || bothRight)
        {
            wayPointMapLeft[0] = wp1.minX + 15;
            wayPointMapLeft[1] = wp1.maxY;
            wayPointMapRight[0] = wp2.maxX - 15;
            wayPointMapRight[1] = wp2.maxY;
        }
        else if (wp1.maxX < wp2.maxX)
        {
            wayPointMapLeft[0] = wp1.minX + 15;
            wayPointMapLeft[1] = wp1.maxY;
            wayPointMapRight[0] = wp2.maxX - 15;
            wayPointMapRight[1] = wp2.maxY;
        }
        else
        {
            wayPointMapLeft[0] = wp2.minX + 15;
            wayPointMapLeft[1] = wp2.maxY;
            wayPointMapRight[0] = wp1.maxX - 15;
            wayPointMapRight[1] = wp1.maxY;
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
        int y = 50;
        int attempts = 0;

        while (attempts < 30)
        {
            attempts++;
            y += 24;

            if (TileMap.tileTypeAt(x, y, 2))
            {
                if (y % 24 != 0)
                    y -= y % 24;
                break;
            }
        }

        return y;
    }

    public static void TeleportTo(int x, int y)
    {
        Char me = Char.myCharz();
        me.cx = x;
        me.cy = y;
        Service.gI().charMove();

        if (!GameScr.canAutoPlay)
        {
            me.cy = y + 1;
            Service.gI().charMove();
            me.cy = y;
            Service.gI().charMove();
        }
    }

    public static void LoadMapLeft() => LoadMap(0);
    public static void LoadMapCenter() => LoadMap(2);
    public static void LoadMapRight() => LoadMap(1);

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
                TeleportToPosition(wayPointMapLeft, 60);
                break;
            case 1:
                TeleportToPosition(wayPointMapRight, TileMap.pxw - 60);
                break;
            case 2:
                TeleportToPosition(wayPointMapCenter, TileMap.pxw / 2);
                break;
        }

        Service.gI().charMove();

        if (TileMap.mapID == 7 || TileMap.mapID == 14 || TileMap.mapID == 0)
            Service.gI().getMapOffline();
        else
            Service.gI().requestChangeMap();

        Char.ischangingMap = true;
    }

    private static void TeleportToPosition(int[] waypoint, int defaultX)
    {
        if (waypoint[0] != 0 && waypoint[1] != 0)
            TeleportTo(waypoint[0], waypoint[1]);
        else
            TeleportTo(defaultX, GetYGround(defaultX));
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
    #endregion
}