using System;
using Xmap;
using main.Mod;

namespace Mod.community;

public class Boss
{
	public string NameBoss;

	public string MapName;

	public int MapId;

	public DateTime AppearTime;

	public static int widthTextGo = mFont.tahoma_7_white.getWidth(" [Go]");

	public Boss()
	{
	}

	public Boss(string chatVip)
	{
		chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|");
		string[] array = chatVip.Split('|');
		NameBoss = array[0].Trim();
		MapName = array[1].Trim();
		MapId = GetMapID(MapName);
		AppearTime = DateTime.Now;
	}

	public int GetMapID(string mapName)
	{
		for (int i = 0; i < TileMap.mapNames.Length; i++)
		{
			if (TileMap.mapNames[i] != null && i != 40 && i != 39 && i != 155 && TileMap.mapNames[i].ToLower().Trim().Replace("  ", " ")
				.Equals(mapName.ToLower().Trim().Replace("  ", " ")))
			{
				if ((NameBoss.ToLower().Trim().Replace("  ", " ")
					.StartsWith("tiểu đội trưởng") || NameBoss.ToLower().Trim().Replace("  ", " ")
					.StartsWith("số")) && mapName == "Trạm tàu vũ trụ")
				{
					return 25;
				}
				return i;
			}
		}
		return -1;
	}

	public static void controlGotoBoss()
	{
		int num = 42;
		int num2 = mFont.tahoma_7_yellow.getWidth(" [Go]") + 2;
		int num3 = 8;
		for (int i = 0; i < MainMod.listBosses.Count; i++)
		{
			if (MainMod.listBosses[i] == null)
			{
				continue;
			}
			if (GameCanvas.isPointerHoldIn(GameCanvas.w - 2 - num2, num, num2, num3) && GameCanvas.isPointerClick)
			{
				if (TileMap.mapID != MainMod.listBosses[i].MapId)
				{
					MainXmapCL.StartGoToMap(MainMod.listBosses[i].MapId);
				}
				SoundMn.gI().buttonClick();
				GameCanvas.clearAllPointerEvent();
			}
			num += num3;
		}
	}

	public void Paint(mGraphics g, int x, int y, int align)
	{
		TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
		int num = (int)timeSpan.TotalSeconds;
		_ = mFont.tahoma_7_yellow;
		if (TileMap.mapID == MapId)
		{
			_ = mFont.tahoma_7_red;
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				if (((Char)GameScr.vCharInMap.elementAt(i)).cName.Equals(NameBoss))
				{
					_ = mFont.tahoma_7b_red;
					break;
				}
			}
		}
		if (GetMapID(MapName) != TileMap.mapID)
		{
			mFont.tahoma_7.drawString(g, NameBoss + " - " + MapName + " - " + ((num < 60) ? (num + "s") : (timeSpan.Minutes + "ph")) + " trước", x, y, align);
			mFont.tahoma_7_white.drawString(g, " [Go]", x + widthTextGo, y, align);
		}
		else
		{
			mFont.tahoma_7_yellow.drawString(g, NameBoss + " - " + MapName + " - " + ((num < 60) ? (num + "s") : (timeSpan.Minutes + "ph")) + " trước", x, y, align);
			mFont.tahoma_7_white.drawString(g, " [Go]", x + widthTextGo, y, align);
		}
	}
}
