using System.Collections.Generic;
using System.Text;
using Xmap;
using UnityEngine;

public class NextMap
{
    public int MapID;
    public int NpcID;

    public string NameIndex1;
    public string NameIndex2;
    public string NameIndex3;

    public string NameIndex1Phu;
    public string NameIndex2Phu;
    public string NameIndex3Phu;

    public string NameIndex1Phu2;
    public string NameIndex2Phu2;
    public string NameIndex3Phu2;

    public int indexNpc;
    public int indexNpc2;
    public int indexNpc3;

    public bool walk;
    public int x;
    public int y;

    private bool isEntering;
    private bool hasTeleported;
    private float enterDelayStart;
    private float teleportTime;
    private float walkDelayStart;

    private int teleportAttempts;
    private int walkAttempts;

    private static readonly HashSet<string> NpcDaTuChoiKeo = new HashSet<string>();
    public static bool nextSuKienHalloween;

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
        int indexNpc3 = -1)
    {
        MapID = mapID;
        NpcID = npcID;

        NameIndex1 = selectName;
        NameIndex2 = selectName2;
        NameIndex3 = selectName3;

        this.walk = walk;
        this.x = x;
        this.y = y;

        NameIndex1Phu = selectIndexPhu1;
        NameIndex2Phu = selectIndexPhu2;
        NameIndex3Phu = selectIndexPhu3;

        NameIndex1Phu2 = selectIndexPhu1cua1;
        NameIndex2Phu2 = selectIndexPhu2cua2;
        NameIndex3Phu2 = selectIndexPhu3cua3;

        this.indexNpc = indexNpc;
        this.indexNpc2 = indexNpc2;
        this.indexNpc3 = indexNpc3;
    }

    // =========================
    // ENTRY POINT
    // =========================
    public void GotoMap()
    {
        if (walk)
        {
            if (x != -1 && y != -1 && Char.myCharz().currentMovePoint == null)
            {
                Char.myCharz().currentMovePoint = new MovePoint(x, y);
            }
            return;
        }

        if (NpcID == -1)
        {
            Waypoint wp = GetWayPoint();
            if (wp != null)
            {
                Enter(wp);
            }
            return;
        }

        HandleNpcInteraction();
    }

    // =========================
    // NPC HANDLING
    // =========================
    private void HandleNpcInteraction()
    {
        if (ModProCL.confirming)
            return;

        Npc npc = GetNPC(NpcID);
        if (npc == null)
            return;

        if (nextSuKienHalloween && indexNpc != -1)
        {
            string key = MapID + "-" + NpcID;
            if (!NpcDaTuChoiKeo.Contains(key))
            {
                NpcDaTuChoiKeo.Add(key);
                ModProCL.startComfirmNpc((short)NpcID);
                return;
            }
        }

        if (indexNpc != -1)
        {
            Service service = Service.gI();
            service.openMenu(NpcID);
            service.confirmMenu((short)NpcID, (sbyte)indexNpc);

            if (indexNpc2 != -1)
            {
                service.confirmMenu((short)NpcID, (sbyte)indexNpc2);
                if (indexNpc3 != -1)
                    service.confirmMenu((short)NpcID, (sbyte)indexNpc3);
            }
            return;
        }

        if (!string.IsNullOrEmpty(NameIndex1))
        {
            ModProCL.startComfirmNpc(
                (short)NpcID,
                NameIndex1,
                NameIndex2,
                NameIndex3,
                NameIndex1Phu,
                NameIndex2Phu,
                NameIndex3Phu,
                NameIndex1Phu2,
                NameIndex2Phu2,
                NameIndex3Phu2
            );
        }
    }

    // =========================
    // WAYPOINT
    // =========================
    public Waypoint GetWayPoint()
    {
        string targetName = GetMapName();
        int size = TileMap.vGo.size();

        for (int i = 0; i < size; i++)
        {
            Waypoint wp = (Waypoint)TileMap.vGo.elementAt(i);
            if (GetMapName(wp.popup) == targetName)
                return wp;
        }
        return null;
    }

    public string GetMapName()
    {
        return TileMap.mapNames[MapID];
    }

    public string GetMapName(PopUp popup)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string s in popup.says)
            sb.Append(s).Append(' ');
        return sb.ToString().Trim();
    }

    // =========================
    // ENTER MAP
    // =========================
    public void Enter(Waypoint wp)
    {
        float now = Time.realtimeSinceStartup;

        if (!isEntering)
        {
            isEntering = true;
            enterDelayStart = now;
            teleportTime = 0;
            hasTeleported = false;
            teleportAttempts = 0;
            walkAttempts = 0;
            walkDelayStart = 0;
            return;
        }

        if (now - enterDelayStart < 0.05f)
            return;

        if (hasTeleported && now - teleportTime < 0.05f)
            return;

        if (TileMap.mapID == 166 && MapID == 155)
        {
            MainXmapCL.LoadMapLeft();
            ResetEnterState();
            return;
        }

        int targetX = CalculateTargetX(wp);
        int targetY = wp.maxY;

        if (targetX == -1 || targetY == -1)
        {
            ResetEnterState();
            return;
        }

        ProcessWaypointEntry(wp, targetX, targetY, now);
    }

    private int CalculateTargetX(Waypoint wp)
    {
        if (wp.maxX < 60)
            return 15;
        if (wp.minX > TileMap.pxw - 60)
            return TileMap.pxw - 15;
        return (wp.minX + wp.maxX) >> 1;
    }

    private void ProcessWaypointEntry(Waypoint wp, int tx, int ty, float now)
    {
        Char me = Char.myCharz();
        int dx = Mathf.Abs(me.cx - tx);
        int dy = Mathf.Abs(me.cy - ty);

        bool isWideGate = wp.maxX >= 60 && wp.minX <= TileMap.pxw - 60;
        me.cdir = me.cx < tx ? 1 : -1;

        if (MainXmapCL.teleDirect)
        {
            if (dx > 5 || dy > 5)
                Teleport(tx, ty, now);
            else if (me.currentMovePoint == null)
            {
                RequestMapChange(wp);
                ResetEnterState();
            }
            return;
        }

        if (dx > 30 || dy > 30)
        {
            int offset = isWideGate ? 0 : (me.cx < tx ? -20 : 20);
            Teleport(tx + offset, ty, now);
            return;
        }

        if (!isWideGate)
        {
            HandleWalk(tx, ty, now);
            return;
        }

        if (me.currentMovePoint == null)
        {
            RequestMapChange(wp);
            ResetEnterState();
        }
    }

    private void HandleWalk(int tx, int ty, float now)
    {
        Char me = Char.myCharz();

        if (me.currentMovePoint == null && now - walkDelayStart >= 0.3f)
        {
            int offset = me.cx < tx ? -15 : 15;
            me.currentMovePoint = new MovePoint(tx - offset, ty);
            walkAttempts++;
            walkDelayStart = now;
        }
        else if (walkAttempts >= 5 && me.currentMovePoint == null)
        {
            ControlCharacter(me.cdir, true);
            walkAttempts = 0;
            walkDelayStart = now;
        }
    }

    private void RequestMapChange(Waypoint wp)
    {
        if (wp.isOffline)
            Service.gI().getMapOffline();
        else
            Service.gI().requestChangeMap();
    }

    private void ControlCharacter(int dir, bool jump)
    {
        Char me = Char.myCharz();
        if (me.isLockMove)
            return;

        if (jump)
        {
            GameScr g = GameScr.gI();
            me.cdir = dir;
            g.setCharJump(dir * 4);
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

    private void ResetEnterState()
    {
        isEntering = false;
        hasTeleported = false;
        teleportTime = 0;
        teleportAttempts = 0;
        walkAttempts = 0;
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
}
