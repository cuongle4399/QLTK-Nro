using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Xmap;

public class NextMap
{
    #region Enums
    public enum MoveType
    {
        Waypoint, NpcMenu, NpcIndex, Item, Walk
    }
    #endregion

    #region Constants
    private const float ENTER_DELAY = 0.05f;
    private const float TELEPORT_DELAY = 0.05f;
    private const float WALK_DELAY = 0.3f;
    private const float ITEM_USE_DELAY = 0.5f;
    private const int MAX_WALK_ATTEMPTS = 5;
    private const int GATE_EDGE_THRESHOLD = 60;
    private const int GATE_CENTER_OFFSET = 15;
    private const int TELEPORT_THRESHOLD = 30;
    private const int WALK_OFFSET = 15;
    private const int WALK_THRESHOLD = 20;
    private const int CONFIRM_TIMEOUT = 3500;
    private const int STEP_DELAY = 500;
    private const int NPC_CONFIRM_INIT_DELAY = 1100; // Delay 500ms trước khi bắt đầu confirm
    #endregion

    #region Static Fields - NPC Confirmation System
    public static short idNpcService;
    public static MenuOptions menuOptions;
    public static bool confirming;
    public static long delayConfirm;
    public static bool runningopennpc;
    public static int currentStep;

    private static long confirmStartTime;
    private static long lastStepTime;
    private static long npcConfirmBeginTime; // Thời gian bắt đầu confirm NPC
    private static bool NextInfoSuKien;
    private static string InfoSuKien;
    #endregion

    #region Static Fields - Special Events
    private static readonly HashSet<string> NpcMenuText = new HashSet<string>();
    public static bool nextSuKien;
    public static string InfoTextMenuXmap;
    #endregion

    #region Static Fields - Item System
    private static bool isUsingItem;
    private static float lastItemUseTime;
    private static int currentItemID;
    #endregion

    #region Instance Fields - Map Configuration
    public int MapID { get; private set; }
    public int NpcID { get; private set; }
    public int ItemID { get; private set; }
    public MenuOptions Options { get; private set; }
    public MoveType Type { get; private set; }
    public int WaypointPosition { get; set; }

    [Obsolete("Use Type property instead")]
    public bool walk; public int x;
    public int y;
    #endregion

    #region Instance Fields - Movement State
    private bool isEntering;
    private bool hasTeleported;
    private float enterDelayStart;
    private float teleportTime;
    private float walkDelayStart;
    private int teleportAttempts;
    private int walkAttempts;
    #endregion

    #region Structs
    public struct MenuOptions
    {
        public string[] Names;
        public string[] SubNames;
        public string[] SubNames2;
        public int[] IndexNpcs;

        public MenuOptions(
            string name1 = "", string name2 = "", string name3 = "",
            string sub1 = "", string sub2 = "", string sub3 = "",
            string sub1_2 = "", string sub2_2 = "", string sub3_2 = "",
            int idx1 = -1, int idx2 = -1, int idx3 = -1)
        {
            Names = new[] { name1, name2, name3 };
            SubNames = new[] { sub1, sub2, sub3 };
            SubNames2 = new[] { sub1_2, sub2_2, sub3_2 };
            IndexNpcs = new[] { idx1, idx2, idx3 };
        }

        public bool HasMenuOptions => !string.IsNullOrEmpty(Names[0]) || IndexNpcs[0] != -1;
        public bool HasIndexOptions => IndexNpcs[0] != -1;
        public bool HasNameOptions => !string.IsNullOrEmpty(Names[0]);
    }
    #endregion

    #region Constructor
    public NextMap(
        int mapID,
        int npcID,
        string selectName,
        string selectName2 = "",
        string selectName3 = "",
        bool walk = false,
        int x = -1,
        int y = -1,
        string selectIndexPhu1 = "",
        string selectIndexPhu2 = "",
        string selectIndexPhu3 = "",
        string selectIndexPhu1cua1 = "",
        string selectIndexPhu2cua2 = "",
        string selectIndexPhu3cua3 = "",
        int indexNpc = -1,
        int indexNpc2 = -1,
        int indexNpc3 = -1,
        int itemID = -1)
    {
        MapID = mapID;
        NpcID = npcID;
        ItemID = itemID;
        this.walk = walk;
        this.x = x;
        this.y = y;

        Options = new MenuOptions(
            selectName, selectName2, selectName3,
            selectIndexPhu1, selectIndexPhu2, selectIndexPhu3,
            selectIndexPhu1cua1, selectIndexPhu2cua2, selectIndexPhu3cua3,
            indexNpc, indexNpc2, indexNpc3
        );

        Type = DetermineMoveType();
    }
    #endregion

    #region Move Type Determination
    private MoveType DetermineMoveType()
    {
        if (ItemID != -1) return MoveType.Item;
        if (walk) return MoveType.Walk;
        if (NpcID == -1) return MoveType.Waypoint;
        if (Options.HasIndexOptions) return MoveType.NpcIndex;
        if (Options.HasNameOptions) return MoveType.NpcMenu;
        return MoveType.Waypoint;
    }
    #endregion

    #region Main Entry Point
    public void GotoMap()
    {
        switch (Type)
        {
            case MoveType.Walk:
                HandleWalkMove();
                break;

            case MoveType.Waypoint:
                HandleWaypointMove();
                break;

            case MoveType.NpcMenu:
            case MoveType.NpcIndex:
                HandleNpcMove();
                break;

            case MoveType.Item:
                HandleItemMove();
                break;
        }
    }
    #endregion

    #region Movement Type Handlers
    private void HandleWalkMove()
    {
        if (x != -1 && y != -1 && Char.myCharz().currentMovePoint == null)
        {
            Char.myCharz().currentMovePoint = new MovePoint(x, y);
        }
    }

    private void HandleWaypointMove()
    {
        Waypoint wp = GetWayPoint();
        if (wp != null)
            Enter(wp);
    }

    private void HandleNpcMove()
    {
        if (confirming) return;

        Npc npc = GetNPC(NpcID);
        if (npc == null) return;

        if (ShouldHandleHalloweenEvent())
        {
            HandleEvent();
            return;
        }

        if (Type == MoveType.NpcIndex)
            HandleNpcIndexInteraction();
        else if (Type == MoveType.NpcMenu)
            HandleNpcMenuInteraction();
    }

    private void HandleItemMove()
    {
        float now = Time.realtimeSinceStartup;

        // Initialize item use
        if (ItemID != -1 && !isUsingItem)
        {
            if (!ModProCL.ExistItemBag(ItemID))
            {
                MainXmapCL.xmapErrr = true;
                return;
            }

            isUsingItem = true;
            currentItemID = ItemID;
            lastItemUseTime = now;
            ModProCL.useItem(ItemID);
            return;
        }

        // Wait for item to be used
        if (isUsingItem && now - lastItemUseTime < ITEM_USE_DELAY)
            return;

        // Item used, reset and proceed to next map
        if (isUsingItem)
        {
            isUsingItem = false;
            Waypoint wp = GetWayPoint();
            if (wp != null)
                Enter(wp);
        }
    }
    #endregion

    #region NPC Interaction
    private void HandleNpcIndexInteraction()
    {
        Service service = Service.gI();
        ModProCL.teleNPC(NpcID);
        service.openMenu(NpcID);
        service.confirmMenu((short)NpcID, (sbyte)Options.IndexNpcs[0]);

        if (Options.IndexNpcs[1] != -1)
        {
            service.confirmMenu((short)NpcID, (sbyte)Options.IndexNpcs[1]);
            if (Options.IndexNpcs[2] != -1)
                service.confirmMenu((short)NpcID, (sbyte)Options.IndexNpcs[2]);
        }

        MainXmapCL.SetNpcIndexActionTime(Time.realtimeSinceStartup);
    }

    private void HandleNpcMenuInteraction()
    {
        ModProCL.teleNPC(NpcID);
        startComfirmNpc(
            (short)NpcID,
            Options.Names[0], Options.Names[1], Options.Names[2],
            Options.SubNames[0], Options.SubNames[1], Options.SubNames[2],
            Options.SubNames2[0], Options.SubNames2[1], Options.SubNames2[2]
        );
    }
    #endregion

    #region Halloween Event
    private bool ShouldHandleHalloweenEvent()
    {
        return nextSuKien && Options.IndexNpcs[0] != -1;
    }

    private void HandleEvent()
    {
        string key = $"{MapID}-{NpcID}";
        if (!NpcMenuText.Contains(key))
        {
            NpcMenuText.Add(key);
            startComfirmNpc((short)NpcID);
        }
    }
    #endregion

    #region Waypoint Entry System
    public void Enter(Waypoint wp)
    {
        float now = Time.realtimeSinceStartup;

        if (!InitializeEntryIfNeeded(now)) return;
        if (!CanProceedWithEntry(now)) return;
        if (HandleSpecialCases()) return;

        int targetX = CalculateTargetX(wp);
        int targetY = wp.maxY;

        if (!IsValidTarget(targetX, targetY))
        {
            ResetEnterState();
            return;
        }

        ProcessWaypointEntry(wp, targetX, targetY, now);
    }

    private bool InitializeEntryIfNeeded(float now)
    {
        if (!isEntering)
        {
            isEntering = true;
            enterDelayStart = now;
            teleportTime = 0;
            hasTeleported = false;
            teleportAttempts = 0;
            walkAttempts = 0;
            walkDelayStart = 0;
            return false;
        }
        return true;
    }

    private bool CanProceedWithEntry(float now)
    {
        if (now - enterDelayStart < ENTER_DELAY) return false;
        if (hasTeleported && now - teleportTime < TELEPORT_DELAY) return false;
        return true;
    }

    private bool HandleSpecialCases()
    {
        if (TileMap.mapID == 166 && MapID == 155)
        {
            MainXmapCL.LoadMapLeft();
            ResetEnterState();
            return true;
        }
        return false;
    }

    private bool IsValidTarget(int x, int y) => x != -1 && y != -1;

    private int CalculateTargetX(Waypoint wp)
    {
        if (wp.maxX < GATE_EDGE_THRESHOLD)
            return GATE_CENTER_OFFSET;
        if (wp.minX > TileMap.pxw - GATE_EDGE_THRESHOLD)
            return TileMap.pxw - GATE_CENTER_OFFSET;
        return (wp.minX + wp.maxX) >> 1;
    }

    private void ProcessWaypointEntry(Waypoint wp, int tx, int ty, float now)
    {
        Char me = Char.myCharz();
        int dx = Math.Abs(me.cx - tx);
        int dy = Math.Abs(me.cy - ty);
        bool isWideGate = wp.maxX >= GATE_EDGE_THRESHOLD && wp.minX <= TileMap.pxw - GATE_EDGE_THRESHOLD;

        me.cdir = me.cx < tx ? 1 : -1;

        if (MainXmapCL.teleDirect)
            ProcessDirectTeleport(tx, ty, dx, dy, wp, now);
        else
            ProcessWalkingMode(tx, ty, dx, dy, isWideGate, wp, now);
    }

    private void ProcessDirectTeleport(int tx, int ty, int dx, int dy, Waypoint wp, float now)
    {
        if (dx > 5 || dy > 5)
        {
            Teleport(tx, ty, now);
        }
        else if (Char.myCharz().currentMovePoint == null)
        {
            RequestMapChange(wp);
            ResetEnterState();
        }
    }

    private void ProcessWalkingMode(int tx, int ty, int dx, int dy, bool isWideGate, Waypoint wp, float now)
    {
        if (dx > TELEPORT_THRESHOLD || dy > TELEPORT_THRESHOLD)
        {
            int offset = isWideGate ? 0 : (Char.myCharz().cx < tx ? -WALK_THRESHOLD : WALK_THRESHOLD);
            Teleport(tx + offset, ty, now);
            return;
        }

        if (!isWideGate)
        {
            ProcessWalkToGate(tx, ty, now);
            return;
        }

        if (Char.myCharz().currentMovePoint == null)
        {
            RequestMapChange(wp);
            ResetEnterState();
        }
    }

    private void ProcessWalkToGate(int tx, int ty, float now)
    {
        Char me = Char.myCharz();

        if (me.currentMovePoint == null && now - walkDelayStart >= WALK_DELAY)
        {
            int offset = me.cx < tx ? -WALK_OFFSET : WALK_OFFSET;
            me.currentMovePoint = new MovePoint(tx - offset, ty);
            walkAttempts++;
            walkDelayStart = now;
        }
        else if (walkAttempts >= MAX_WALK_ATTEMPTS && me.currentMovePoint == null)
        {
            ControlCharacter(me.cdir, jump: true);
            walkAttempts = 0;
            walkDelayStart = now;
        }
    }
    #endregion

    #region Character Control
    private void ControlCharacter(int dir, bool jump)
    {
        Char me = Char.myCharz();
        if (me.isLockMove) return;

        if (jump)
        {
            me.cdir = dir;
            GameScr.gI().setCharJump(dir * 4);
            return;
        }

        me.cdir = dir;
        if (me.cx != me.cxSend)
            Service.gI().charMove();

        me.statusMe = 2;
        me.cvx = dir * me.cspeed;
        me.holder = false;
    }

    private void Teleport(int x, int y, float now)
    {
        TeleportTo(x, y);
        hasTeleported = true;
        teleportTime = now;
        enterDelayStart = now;
        teleportAttempts++;
    }

    public void TeleportTo(int x, int y)
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
    #endregion

    #region Map Transition
    private void RequestMapChange(Waypoint wp)
    {
        if (wp.isOffline)
            Service.gI().getMapOffline();
        else
            Service.gI().requestChangeMap();
    }

    private void ResetEnterState()
    {
        isEntering = false;
        hasTeleported = false;
        teleportTime = 0;
        teleportAttempts = 0;
        walkAttempts = 0;
    }
    #endregion

    #region Waypoint & NPC Utilities
    public Waypoint GetWayPoint()
    {
        string targetName = GetMapName();
        int size = TileMap.vGo.size();
        List<Waypoint> matched = new List<Waypoint>();

        for (int i = 0; i < size; i++)
        {
            Waypoint wp = (Waypoint)TileMap.vGo.elementAt(i);
            if (GetMapName(wp.popup) == targetName)
                matched.Add(wp);
        }

        if (matched.Count == 0) return null;
        if (matched.Count == 1 || WaypointPosition == 0) return matched[0];

        // Khi có nhiều waypoint trùng tên, chọn theo vị trí
        if (WaypointPosition == -1)
        {
            // Chọn waypoint bên trái nhất
            Waypoint leftmost = matched[0];
            for (int i = 1; i < matched.Count; i++)
            {
                if (matched[i].minX < leftmost.minX)
                    leftmost = matched[i];
            }
            return leftmost;
        }
        else if (WaypointPosition == 1)
        {
            // Chọn waypoint bên phải nhất
            Waypoint rightmost = matched[0];
            for (int i = 1; i < matched.Count; i++)
            {
                if (matched[i].minX > rightmost.minX)
                    rightmost = matched[i];
            }
            return rightmost;
        }

        return matched[0];
    }

    public string GetMapName() => TileMap.mapNames[MapID];

    public string GetMapName(PopUp popup)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string s in popup.says)
            sb.Append(s).Append(' ');
        return sb.ToString().Trim();
    }

    public static Npc GetNPC(int id)
    {
        int size = GameScr.vNpc.size();
        for (int i = 0; i < size; i++)
        {
            Npc npc = (Npc)GameScr.vNpc.elementAt(i);
            if (npc.template.npcTemplateId == id)
                return npc;
        }
        return null;
    }
    #endregion

    #region Static NPC Confirmation System
    public static void startComfirmNpc(
        short idnpc,
        string s1 = "", string s2 = "", string s3 = "",
        string s1Sub = "", string s2Sub = "", string s3Sub = "",
        string s1Sub2 = "", string s2Sub2 = "", string s3Sub2 = "",
        bool nextInfoSuKien = true,
        string infoSuKien = "")
    {
        if (string.IsNullOrEmpty(infoSuKien))
            infoSuKien = NextMap.InfoTextMenuXmap;

        idNpcService = idnpc;
        menuOptions = new MenuOptions(s1, s2, s3, s1Sub, s2Sub, s3Sub, s1Sub2, s2Sub2, s3Sub2);
        confirming = true;
        runningopennpc = false;
        currentStep = 0;
        confirmStartTime = Environment.TickCount;
        lastStepTime = 0L;
        npcConfirmBeginTime = Environment.TickCount; // Ghi nhận thời gian bắt đầu confirm
        NextInfoSuKien = nextInfoSuKien;
        InfoSuKien = infoSuKien;
    }

    public static void UpdateConfirmNpc()
    {
        if (!confirming) return;

        if (IsConfirmTimeout())
        {
            CancelConfirmation();
            return;
        }

        // Chờ 500ms trước khi bắt đầu xử lý confirm steps
        if (Environment.TickCount - npcConfirmBeginTime < NPC_CONFIRM_INIT_DELAY)
            return;

        if (!GameCanvas.menu.showMenu && !runningopennpc)
        {
            Service.gI().openMenu(idNpcService);
            runningopennpc = true;
        }

        if (!CanProcessMenuStep()) return;

        if (TryProcessEventStep()) return;

        ProcessMenuSteps();

        if (AreAllStepsCompleted())
            confirming = false;
    }

    private static bool IsConfirmTimeout()
    {
        return Environment.TickCount - confirmStartTime > CONFIRM_TIMEOUT;
    }

    private static void CancelConfirmation()
    {
        confirming = false;
        runningopennpc = false;
        currentStep = 0;
        GameCanvas.menu.doCloseMenu();
    }

    private static bool CanProcessMenuStep()
    {
        if (!GameCanvas.menu.showMenu) return false;
        if (lastStepTime > 0 && Environment.TickCount - lastStepTime < STEP_DELAY) return false;
        return true;
    }

    private static bool TryProcessEventStep()
    {
        if (!NextInfoSuKien || !nextSuKien) return false;

        if (SelectMenuByName(InfoSuKien))
        {
            InfoSuKien = "";
            runningopennpc = false;
            lastStepTime = Environment.TickCount;
            NextInfoSuKien = false;
            return true;
        }

        NextInfoSuKien = false;
        return false;
    }

    private static void ProcessMenuSteps()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentStep == i && !string.IsNullOrEmpty(menuOptions.Names[i]))
            {
                if (SelectMenuByName(menuOptions.Names[i], menuOptions.SubNames[i], menuOptions.SubNames2[i]))
                {
                    menuOptions.Names[i] = "";
                    menuOptions.SubNames[i] = "";
                    menuOptions.SubNames2[i] = "";
                    currentStep++;
                    lastStepTime = Environment.TickCount;
                    break;
                }
            }
        }
    }

    private static bool AreAllStepsCompleted()
    {
        return string.IsNullOrEmpty(menuOptions.Names[0]) &&
               string.IsNullOrEmpty(menuOptions.Names[1]) &&
               string.IsNullOrEmpty(menuOptions.Names[2]);
    }

    public static string NormalizeText(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        string text = new string(input.Where(c => !char.IsControl(c)).ToArray());
        return Regex.Replace(text.ToLower().Trim(), "\\s+", "");
    }

    private static bool SelectMenuByName(string nameIndex, string subName = "", string subName2 = "")
    {
        if (GameCanvas.menu.menuItems == null || GameCanvas.menu.menuItems.size() == 0)
            return false;

        string value = NormalizeText(nameIndex);
        string value2 = NormalizeText(subName);
        string value3 = NormalizeText(subName2);

        for (int i = 0; i < GameCanvas.menu.menuItems.size(); i++)
        {
            try
            {
                object obj = GameCanvas.menu.menuItems.elementAt(i);
                if (obj == null) continue;

                string text = NormalizeText(((Command)obj).caption ?? "");
                if (text.Equals(value) ||
                    (!string.IsNullOrEmpty(value2) && text.Equals(value2)) ||
                    (!string.IsNullOrEmpty(value3) && text.Equals(value3)))
                {
                    GameCanvas.menu.menuSelectedItem = i;
                    GameCanvas.menu.performSelect();
                    GameCanvas.menu.doCloseMenu();
                    return true;
                }
            }
            catch { }
        }
        return false;
    }
    #endregion
}