using System.Collections.Generic;
using System.Text;
using Client244.Xmap;
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

	public NextMap(int mapID, int npcID, string selectName, string selectName2 = "", string selectName3 = "", bool walk = false, int x = -1, int y = -1, string selectIndexPhu1 = "", string selectIndexPhu2 = "", string selectIndexPhu3 = "", string selectIndexPhu1cua1 = "", string selectIndexPhu2cua2 = "", string selectIndexPhu3cua3 = "", int indexNpc = -1, int indexNpc2 = -1, int indexNpc3 = -1)
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

	public void GotoMap()
	{
		if (walk)
		{
			if (x != -1 && y != -1)
			{
				Char.myCharz().currentMovePoint = new MovePoint(x, y);
			}
		}
		else if (NpcID == -1)
		{
			Waypoint wayPoint = GetWayPoint();
			if (wayPoint != null)
			{
				Enter(wayPoint);
			}
		}
		else
		{
			HandleNpcInteraction();
		}
	}

	private void HandleNpcInteraction()
	{
		Npc nPC = GetNPC(NpcID);
		if (nPC == null || ModProCL.confirming)
		{
			return;
		}
		if (nextSuKienHalloween && indexNpc != -1)
		{
			string item = $"{MapID}-{NpcID}";
			if (!NpcDaTuChoiKeo.Contains(item))
			{
				ModProCL.startComfirmNpc((short)NpcID);
				NpcDaTuChoiKeo.Add(item);
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
				{
					service.confirmMenu((short)NpcID, (sbyte)indexNpc3);
				}
			}
		}
		else if (!string.IsNullOrEmpty(NameIndex1))
		{
			ModProCL.startComfirmNpc((short)NpcID, NameIndex1, NameIndex2, NameIndex3, NameIndex1Phu, NameIndex2Phu, NameIndex3Phu, NameIndex1Phu2, NameIndex2Phu2, NameIndex3Phu2);
		}
	}

	public Waypoint GetWayPoint()
	{
		for (int i = 0; i < TileMap.vGo.size(); i++)
		{
			Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
			if (GetMapName() == GetMapName(waypoint.popup))
			{
				return waypoint;
			}
		}
		return null;
	}

	public string GetMapName()
	{
		return TileMap.mapNames[MapID];
	}

	public void Enter(Waypoint wp)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (!isEntering)
		{
			isEntering = true;
			enterDelayStart = realtimeSinceStartup;
			hasTeleported = false;
			teleportAttempts = 0;
			walkAttempts = 0;
			walkDelayStart = 0f;
		}
		else
		{
			if (realtimeSinceStartup - enterDelayStart < 0.05f || (hasTeleported && realtimeSinceStartup - teleportTime < 0.05f))
			{
				return;
			}
			if (TileMap.mapID == 166 && MapID == 155)
			{
				MainXmapCL.LoadMapLeft();
				ResetEnterState();
				return;
			}
			int num = CalculateTargetX(wp);
			int maxY = wp.maxY;
			if (num == -1 || maxY == -1)
			{
				ResetEnterState();
			}
			else
			{
				ProcessWaypointEntry(wp, num, maxY, realtimeSinceStartup);
			}
		}
	}

	private int CalculateTargetX(Waypoint wp)
	{
		if (wp.maxX < 60)
		{
			return 15;
		}
		if (wp.minX > TileMap.pxw - 60)
		{
			return TileMap.pxw - 15;
		}
		return (wp.minX + wp.maxX) / 2;
	}

	private void ProcessWaypointEntry(Waypoint wp, int targetX, int targetY, float now)
	{
		int num = Mathf.Abs(Char.myCharz().cx - targetX);
		int num2 = Mathf.Abs(Char.myCharz().cy - targetY);
		bool flag = wp.maxX >= 60 && wp.minX <= TileMap.pxw - 60;
		Char.myCharz().cdir = ((Char.myCharz().cx < targetX) ? 1 : (-1));
		if (MainXmapCL.teleDirect)
		{
			if (num > 5 || num2 > 5)
			{
				Teleport(targetX, targetY, now);
			}
			else if (Char.myCharz().currentMovePoint == null)
			{
				RequestMapChange(wp);
				ResetEnterState();
			}
		}
		else if (num > 30 || num2 > 30)
		{
			int num3 = ((!flag) ? ((Char.myCharz().cx < targetX) ? (-20) : 20) : 0);
			Teleport(targetX + num3, targetY, now);
		}
		else if (!flag)
		{
			HandleWalkToWaypoint(targetX, targetY, now);
		}
		else if (Char.myCharz().currentMovePoint == null)
		{
			RequestMapChange(wp);
			ResetEnterState();
		}
	}

	private void HandleWalkToWaypoint(int targetX, int targetY, float now)
	{
		if (Char.myCharz().currentMovePoint == null && now - walkDelayStart >= 0.3f)
		{
			int num = ((Char.myCharz().cx < targetX) ? (-15) : 15);
			Char.myCharz().currentMovePoint = new MovePoint(targetX - num, targetY);
			walkAttempts++;
			walkDelayStart = now;
		}
		else if (walkAttempts >= 5 && Char.myCharz().currentMovePoint == null)
		{
			ControlCharacter(Char.myCharz().cdir, isJump: true);
			walkAttempts = 0;
			walkDelayStart = now;
		}
	}

	private void RequestMapChange(Waypoint wp)
	{
		if (wp.isOffline)
		{
			Service.gI().getMapOffline();
		}
		else
		{
			Service.gI().requestChangeMap();
		}
	}

	private void ControlCharacter(int dir, bool isJump)
	{
		if (Char.myCharz().isLockMove)
		{
			return;
		}
		if (isJump)
		{
			GameScr gameScr = GameScr.gI();
			switch (dir)
			{
			case 0:
				gameScr.setCharJump(0);
				break;
			case -1:
				Char.myCharz().cdir = -1;
				gameScr.setCharJump(-4);
				break;
			case 1:
				Char.myCharz().cdir = 1;
				gameScr.setCharJump(4);
				break;
			}
		}
		else
		{
			Char.myCharz().cdir = dir;
			if (Char.myCharz().cx != Char.myCharz().cxSend)
			{
				Service.gI().charMove();
			}
			Char.myCharz().statusMe = 2;
			Char.myCharz().cvx = dir * Char.myCharz().cspeed;
			Char.myCharz().holder = false;
		}
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
		teleportTime = 0f;
		teleportAttempts = 0;
		walkAttempts = 0;
	}

	public string GetMapName(PopUp popup)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] says = popup.says;
		string[] array = says;
		string[] array2 = array;
		foreach (string value in array2)
		{
			stringBuilder.Append(value).Append(' ');
		}
		return stringBuilder.ToString().Trim();
	}

	public void TeleportTo(int x, int y)
	{
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
		if (!GameScr.canAutoPlay)
		{
			Char.myCharz().cy = y + 1;
			Service.gI().charMove();
			Char.myCharz().cy = y;
			Service.gI().charMove();
		}
	}

	public static Npc GetNPC(int idNpc)
	{
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(i);
			if (npc.template.npcTemplateId == idNpc)
			{
				return npc;
			}
		}
		return null;
	}
}
