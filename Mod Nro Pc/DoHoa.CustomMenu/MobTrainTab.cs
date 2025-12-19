using System;
using System.Collections.Generic;
using DoHoa.CustomMenu.Shared;
using Mod.CuongLe;

namespace DoHoa.CustomMenu;

public static class MobTrainTab
{
	public static List<MobTrain> MobTrains;

	public static int ScrollOffset;

	static MobTrainTab()
	{
		MobTrains = new List<MobTrain>();
		ScrollOffset = 0;
	}

	public static void LoadMobsFromMap()
	{
		MobTrains.Clear();
		ScrollOffset = 0;
		List<int> selectedMobIds = GetSelectedMobIds();
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob != null && !mob.isMobMe)
			{
				MobTrain item = new MobTrain
				{
					Name = mob.getTemplate().name,
					MobId = mob.mobId,
					TemplateId = mob.templateId,
					X = mob.xFirst,
					Y = mob.yFirst,
					HP = mob.maxHp,
					AutoFlag = selectedMobIds.Contains(mob.mobId)
				};
				MobTrains.Add(item);
			}
		}
	}

	public static void UpdateMobTrainFlags()
	{
		List<int> selectedMobIds = GetSelectedMobIds();
		foreach (MobTrain mobTrain in MobTrains)
		{
			mobTrain.AutoFlag = selectedMobIds.Contains(mobTrain.MobId);
		}
	}

	public static void Paint(mGraphics g, int panelX, int contentY)
	{
		bool flag = mGraphics.zoomLevel <= 1;
		bool flag2 = MobTrains.Count > 0;
		if (!flag2)
		{
			mFont.tahoma_7b_white.drawString(g, "Không có quái trong map", panelX + MenuHelper.PanelWidth / 2, contentY + 20, 2);
			mFont.tahoma_7b_white.drawString(g, "Vui lòng vào map có quái", panelX + MenuHelper.PanelWidth / 2, contentY + 35, 2);
			g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		}
		int scrollOffset = ScrollOffset;
		int num = System.Math.Min(scrollOffset + MenuHelper.Rows, MobTrains.Count);
		int num2 = (flag2 ? GetFocusedIndex(panelX, contentY, scrollOffset, num) : (-1));
		if (flag2)
		{
			for (int i = scrollOffset; i < num; i++)
			{
				int num3 = i - scrollOffset;
				int num4 = num3 % MenuHelper.Rows;
				int num5 = contentY + num4 * 32;
				MobTrain mobTrain = MobTrains[i];
				bool flag3 = i == num2;
				int color = (flag3 ? 6052956 : 3815994);
				int color2 = (flag3 ? 33679 : 4934475);
				g.setColor(color);
				int x = panelX + 4;
				int w = MenuHelper.PanelWidth - 8;
				if (!flag)
				{
					x = panelX + 4 + 50;
					w = MenuHelper.PanelWidth - 8 - 50;
				}
				g.fillRect(x, num5, w, 30);
				if (!flag)
				{
					g.setColor(color2);
					g.fillRect(panelX + 4, num5, 50, 30);
				}
				if (!flag)
				{
					try
					{
						MobTemplate mobTemplate = Mob.arrMobTemplate[mobTrain.TemplateId];
						if (mobTemplate != null && mobTemplate.data != null)
						{
							int x2 = panelX + 4 + 25;
							int y = num5 + 32 + 4;
							mobTemplate.data.paintFrame(g, 0, x2, y, 0, 2);
						}
					}
					catch
					{
					}
				}
				mFont mFont = (flag3 ? mFont.tahoma_7b_white : mFont.tahoma_7b_yellow);
				mFont mFont2 = (flag3 ? mFont.tahoma_7_white : mFont.tahoma_7_blue1);
				int x3 = (flag ? (panelX + 4 + 4) : (panelX + 4 + 50 + 4));
				mFont.drawString(g, mobTrain.Name, x3, num5 + 2, 0);
				mFont2.drawString(g, $"HP:{mobTrain.HP} - ID:{mobTrain.MobId}", x3, num5 + 14, 0);
				GameCanvas.paintz.paintCheckPass(g, panelX + MenuHelper.PanelWidth - 26, num5 + 4, mobTrain.AutoFlag, focus: false);
			}
			int maxScrollOffset = MenuHelper.CalculateMaxScrollOffset(MobTrains.Count);
			MenuHelper.DrawScrollBar(g, panelX, contentY, MobTrains.Count, ScrollOffset, maxScrollOffset);
		}
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		DrawControlButtons(g, panelX, contentY, flag2);
	}

	private static void DrawControlButtons(mGraphics g, int panelX, int contentY, bool hasMob)
	{
		int num = contentY + MenuHelper.ContentHeight + 8;
		int num2 = MenuHelper.PanelWidth - 16;
		int num3 = (num2 - 16) / 3;
		mFont tahoma_7b_white = mFont.tahoma_7b_white;
		int num4 = panelX + 8;
		int num5 = num4 + num3 + 8;
		int num6 = num5 + num3 + 8;
		g.setColor(hasMob ? 9268835 : 7697781);
		g.fillRect(num4, num, num3, 18);
		tahoma_7b_white.drawString(g, "Chọn hết", num4 + num3 / 2, num + 4, 2);
		int color = (AutoTrainCL.autoNeBoss ? 16754470 : 8026746);
		g.setColor(color);
		g.fillRect(num5, num, num3, 18);
		tahoma_7b_white.drawString(g, "NÉ BOSS", num5 + num3 / 2, num + 4, 2);
		bool flag = hasMob && AutoTrainCL.listMobIds.Count > 0;
		string st = (AutoTrainCL.isAutoTrain ? "DỪNG" : "TRAIN");
		int color2 = ((!flag) ? 7697781 : ((!AutoTrainCL.isAutoTrain) ? 5025616 : ((mSystem.currentTimeMillis() / 300 % 2 == 0L) ? 16711680 : 12986408)));
		g.setColor(color2);
		g.fillRect(num6, num, num3, 18);
		tahoma_7b_white.drawString(g, st, num6 + num3 / 2, num + 4, 2);
		int num7 = num + 26;
		int num8 = (num2 - 16) / 3;
		int num9 = panelX + 8;
		int num10 = num9 + num8 + 8;
		int num11 = num10 + num8 + 8;
		int color3 = ((!hasMob) ? 7697781 : (AutoTrainCL.isAvoidSuperMob ? 15684432 : 8026746));
		g.setColor(color3);
		g.fillRect(num9, num7, num8, 18);
		tahoma_7b_white.drawString(g, "Né SQuai", num9 + num8 / 2, num7 + 4, 2);
		string st2 = $"{GetSelectedMobIds().Count} Quái";
		mFont tahoma_7b_yellow = mFont.tahoma_7b_yellow;
		tahoma_7b_yellow.drawString(g, st2, num10 + num8 / 2, num7 + 4, 2);
		int color4 = (AutoTrainCL.autoHopThe ? 6732650 : 8026746);
		g.setColor(color4);
		g.fillRect(num11, num7, num8, 18);
		tahoma_7b_white.drawString(g, "H.THỂ", num11 + num8 / 2, num7 + 4, 2);
		int num12 = num7 + 26;
		int num13 = panelX + 8;
		int num14 = num13 + num8 + 8;
		int num15 = num14 + num8 + 8;
		int color5 = ((!hasMob) ? 7697781 : ((AutoTrainCL.autoChangeZone || AutoTrainCL.SpamChangeZone) ? 14172949 : 8026746));
		g.setColor(color5);
		g.fillRect(num13, num12, num8, 18);
		tahoma_7b_white.drawString(g, "Khu ít ?", num13 + num8 / 2, num12 + 4, 2);
		g.setColor(hasMob ? 10586239 : 7697781);
		g.fillRect(num14, num12, num8, 18);
		tahoma_7b_white.drawString(g, "LOẠI", num14 + num8 / 2, num12 + 4, 2);
		string st3 = ((!AutoTrainCL.isGoBack) ? "GB" : (AutoTrainCL.isGobackCoordinate ? "GB TĐ" : "GB MAP"));
		int color6 = (AutoTrainCL.isGoBack ? 11225020 : 8026746);
		g.setColor(color6);
		g.fillRect(num15, num12, num8, 18);
		tahoma_7b_white.drawString(g, st3, num15 + num8 / 2, num12 + 4, 2);
	}

	public static void HandleClick(int panelX, int contentY)
	{
		bool flag = MobTrains.Count > 0;
		List<int> orCreateSelectedMobIds = GetOrCreateSelectedMobIds();
		if (flag)
		{
			int x = panelX + MenuHelper.PanelWidth - 28 - 4;
			int scrollOffset = ScrollOffset;
			int num = System.Math.Min(scrollOffset + MenuHelper.Rows, MobTrains.Count);
			for (int i = scrollOffset; i < num; i++)
			{
				int num2 = i - scrollOffset;
				int num3 = num2 % MenuHelper.Rows;
				int y = contentY + num3 * 32;
				if (!GameCanvas.isPointerHoldIn(x, y, 32, 32))
				{
					continue;
				}
				MobTrain mobTrain = MobTrains[i];
				mobTrain.AutoFlag = !mobTrain.AutoFlag;
				if (mobTrain.AutoFlag)
				{
					if (!orCreateSelectedMobIds.Contains(mobTrain.MobId))
					{
						orCreateSelectedMobIds.Add(mobTrain.MobId);
					}
					GameScr.info1.addInfo($"Đã thêm {mobTrain.Name} (ID:{mobTrain.MobId})");
				}
				else
				{
					orCreateSelectedMobIds.Remove(mobTrain.MobId);
					GameScr.info1.addInfo($"Đã loại bỏ {mobTrain.Name} (ID:{mobTrain.MobId})");
				}
				GameCanvas.clearAllPointerEvent();
				return;
			}
		}
		int num4 = contentY + MenuHelper.ContentHeight + 8;
		int num5 = MenuHelper.PanelWidth - 16;
		int num6 = (num5 - 16) / 3;
		int num7 = panelX + 8;
		int num8 = num7 + num6 + 8;
		int x2 = num8 + num6 + 8;
		int num9 = num4 + 26;
		int num10 = (num5 - 16) / 3;
		int num11 = panelX + 8;
		int x3 = num11 + num10 * 2 + 16;
		int y2 = num9 + 26;
		int num12 = panelX + 8;
		int num13 = num12 + num10 + 8;
		int x4 = num13 + num10 + 8;
		if (flag && GameCanvas.isPointerHoldIn(num7, num4, num6, 18))
		{
			orCreateSelectedMobIds.Clear();
			foreach (MobTrain mobTrain2 in MobTrains)
			{
				mobTrain2.AutoFlag = true;
				orCreateSelectedMobIds.Add(mobTrain2.MobId);
			}
			GameScr.info1.addInfo("Đã chọn hết quái trong map");
			GameCanvas.clearAllPointerEvent();
		}
		else if (GameCanvas.isPointerHoldIn(num8, num4, num6, 18))
		{
			AutoTrainCL.autoNeBoss = !AutoTrainCL.autoNeBoss;
			GameScr.info1.addInfo(AutoTrainCL.autoNeBoss ? "Né Boss: Đã Bật" : "Né Boss: Đã Tắt");
			AutoTrainCL.autoChangeZone = false;
			AutoTrainCL.SpamChangeZone = false;
			GameCanvas.clearAllPointerEvent();
		}
		else if (GameCanvas.isPointerHoldIn(x2, num4, num6, 18))
		{
			if (flag && (orCreateSelectedMobIds.Count > 0 || AutoTrainCL.isAutoTrain))
			{
				AutoTrainCL.isAutoTrain = !AutoTrainCL.isAutoTrain;
				GameScr.info1.addInfo(AutoTrainCL.isAutoTrain ? "Auto Train: Đã Bật" : "Auto Train: Đã Dừng");
			}
			else
			{
				GameScr.info1.addInfo("Chưa chọn quái hoặc không có quái");
			}
			GameCanvas.clearAllPointerEvent();
		}
		else if (flag && GameCanvas.isPointerHoldIn(num11, num9, num10, 18))
		{
			AutoTrainCL.isAvoidSuperMob = !AutoTrainCL.isAvoidSuperMob;
			GameScr.info1.addInfo(AutoTrainCL.isAvoidSuperMob ? "Né Siêu Quái: Đã Bật" : "Né Siêu Quái: Đã Tắt");
			GameCanvas.clearAllPointerEvent();
		}
		else if (GameCanvas.isPointerHoldIn(x3, num9, num10, 18))
		{
			AutoTrainCL.autoHopThe = !AutoTrainCL.autoHopThe;
			if (AutoTrainCL.autoHopThe && (ModProCL.ExistPotara() == -1 || TileMap.mapID == Char.myCharz().cgender + 21))
			{
				AutoTrainCL.autoHopThe = false;
				GameScr.info1.addInfo("Yêu cầu có bông tai và ra khỏi map nhà");
			}
			else
			{
				GameScr.info1.addInfo(AutoTrainCL.autoHopThe ? "Auto Hợp Thể: Đã Bật" : "Auto Hợp Thể: Đã Tắt");
			}
			GameCanvas.clearAllPointerEvent();
		}
		else if (GameCanvas.isPointerHoldIn(num12, y2, num10, 18))
		{
			AutoTrainCL.ShowMenuKhuIt();
			GameCanvas.clearAllPointerEvent();
		}
		else if (flag && GameCanvas.isPointerHoldIn(num13, y2, num10, 18))
		{
			ShowSelectByTypeMenu();
			GameCanvas.clearAllPointerEvent();
		}
		else if (GameCanvas.isPointerHoldIn(x4, y2, num10, 18))
		{
			AutoTrainCL.ShowMenuGoback();
			GameCanvas.clearAllPointerEvent();
		}
	}

	public static void HandleScroll(int deltaY)
	{
		int num = -deltaY / 4;
		int val = MenuHelper.CalculateMaxScrollOffset(MobTrains.Count);
		int val2 = ScrollOffset + num;
		val2 = System.Math.Max(0, System.Math.Min(val2, val));
		if (val2 != ScrollOffset)
		{
			ScrollOffset = val2;
			GameCanvas.pyLast = GameCanvas.py;
		}
	}

	private static int GetFocusedIndex(int panelX, int contentY, int startIndex, int endIndex)
	{
		if (!GameCanvas.isPointerDown || !GameCanvas.isPointerJustRelease)
		{
			return -1;
		}
		int px = GameCanvas.px;
		int py = GameCanvas.py;
		for (int i = startIndex; i < endIndex; i++)
		{
			int num = i - startIndex;
			int num2 = num % MenuHelper.Rows;
			int num3 = contentY + num2 * 32;
			if (px >= panelX + 4 && px <= panelX + MenuHelper.PanelWidth - 4 && py >= num3 && py <= num3 + 32)
			{
				return i;
			}
		}
		return -1;
	}

	private static void ShowSelectByTypeMenu()
	{
		MyVector myVector = new MyVector();
		List<MobTrain> list = new List<MobTrain>();
		foreach (MobTrain mobTrain2 in MobTrains)
		{
			bool flag = false;
			foreach (MobTrain item in list)
			{
				if (item.TemplateId == mobTrain2.TemplateId)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(mobTrain2);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			MobTrain mobTrain3 = list[i];
			int num = 0;
			foreach (MobTrain mobTrain4 in MobTrains)
			{
				if (mobTrain4.TemplateId == mobTrain3.TemplateId)
				{
					num++;
				}
			}
			string caption = $"{mobTrain3.Name}\n[{NinjaUtil.getMoneys(mobTrain3.HP)}HP] - SL:{num}";
			myVector.addElement(new Command(caption, AutoTrainCL.getInstance(), 1, mobTrain3.TemplateId));
		}
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static List<int> GetSelectedMobIds()
	{
		int mapID = TileMap.mapID;
		if (AutoTrainCL.listMobIds.ContainsKey(mapID))
		{
			return AutoTrainCL.listMobIds[mapID];
		}
		return new List<int>();
	}

	private static List<int> GetOrCreateSelectedMobIds()
	{
		int mapID = TileMap.mapID;
		if (!AutoTrainCL.listMobIds.ContainsKey(mapID))
		{
			AutoTrainCL.listMobIds[mapID] = new List<int>();
		}
		return AutoTrainCL.listMobIds[mapID];
	}
}
