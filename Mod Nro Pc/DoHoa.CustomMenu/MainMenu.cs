using System;
using DoHoa.CustomMenu.Shared;

namespace DoHoa.CustomMenu;

public static class MainMenu
{
	public static bool isShowMenuVIP;

	private static int currentTab;

	public static int Columns => 1;

	public static int Rows => MenuHelper.Rows;

	public static int PanelWidth => MenuHelper.PanelWidth;

	public static int PanelHeight => MenuHelper.PanelHeight;

	public static int PanelX => MenuHelper.PanelX;

	public static int PanelY => MenuHelper.PanelY;

	static MainMenu()
	{
		currentTab = 0;
	}

	public static void EnsureCachedValues()
	{
		int num = currentTab;
		bool flag = false;
		bool flag2 = false;
		if (1 == 0)
		{
		}
		int num2 = num switch
		{
			0 => AutoItemTab.AutoItems.Length, 
			1 => SkillTrainTab.SkillTrains.Length, 
			2 => MobTrainTab.MobTrains.Count, 
			_ => 1, 
		};
		if (1 == 0)
		{
		}
		int num3 = num2;
		bool flag3 = false;
		int num4 = num3;
		bool flag4 = false;
		int totalItems = num4;
		int num5 = currentTab;
		bool flag5 = false;
		bool flag6 = false;
		if (1 == 0)
		{
		}
		num2 = num5 switch
		{
			1 => 26, 
			2 => 78, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		num3 = num2;
		bool flag7 = false;
		num4 = num3;
		bool flag8 = false;
		int buttonPanelHeight = num4;
		MenuHelper.UpdateCachedValues(currentTab, totalItems, buttonPanelHeight);
	}

	public static void Paint(mGraphics g)
	{
		EnsureCachedValues();
		int panelX = MenuHelper.PanelX;
		int panelY = MenuHelper.PanelY;
		int panelWidth = MenuHelper.PanelWidth;
		int panelHeight = MenuHelper.PanelHeight;
		g.setColor(2894892);
		g.fillRoundRect(panelX, panelY, panelWidth, panelHeight, 8, 8);
		g.setColor(49151);
		g.drawRoundRect(panelX, panelY, panelWidth, panelHeight, 8, 8);
		DrawTabs(g, panelX, panelY, panelWidth);
		int num = panelY + 30 + 8;
		int num2 = num - 4;
		int h = panelY + panelHeight - num2;
		g.setClip(panelX, num2, panelWidth, h);
		switch (currentTab)
		{
		case 0:
			AutoItemTab.Paint(g, panelX, num);
			break;
		case 1:
			SkillTrainTab.Paint(g, panelX, num);
			break;
		case 2:
			MobTrainTab.Paint(g, panelX, num);
			break;
		}
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
	}

	private static void DrawTabs(mGraphics g, int panelX, int panelY, int panelWidth)
	{
		bool flag = mGraphics.zoomLevel <= 1;
		int num = panelWidth / 3;
		int h = 30;
		string st = (flag ? "Item" : "Auto Item");
		string st2 = (flag ? "Skill" : "Skill Train");
		string st3 = (flag ? "Mob" : "Mob Train");
		int color = ((currentTab == 0) ? 49151 : 4079166);
		g.setColor(color);
		g.fillRect(panelX, panelY, num, h);
		mFont.tahoma_7b_white.drawString(g, st, panelX + num / 2, panelY + 9, 2);
		int color2 = ((currentTab == 1) ? 49151 : 4079166);
		g.setColor(color2);
		g.fillRect(panelX + num, panelY, num, h);
		mFont.tahoma_7b_white.drawString(g, st2, panelX + num + num / 2, panelY + 9, 2);
		int color3 = ((currentTab == 2) ? 49151 : 4079166);
		g.setColor(color3);
		g.fillRect(panelX + num * 2, panelY, panelWidth - num * 2, h);
		mFont.tahoma_7b_white.drawString(g, st3, panelX + num * 2 + (panelWidth - num * 2) / 2, panelY + 9, 2);
	}

	public static void HandleClick()
	{
		if (!GameCanvas.isPointerClick || !GameCanvas.isPointerJustRelease)
		{
			HandleScroll();
			return;
		}
		EnsureCachedValues();
		int num = System.Math.Abs(GameCanvas.px - GameCanvas.pxLast);
		int num2 = System.Math.Abs(GameCanvas.py - GameCanvas.pyLast);
		if (num > 10 || num2 > 10)
		{
			GameCanvas.clearAllPointerEvent();
			return;
		}
		int panelX = MenuHelper.PanelX;
		int panelY = MenuHelper.PanelY;
		int num3 = 30;
		int num4 = MenuHelper.PanelWidth / 3;
		int contentY = panelY + num3 + 8;
		if (GameCanvas.isPointerHoldIn(panelX, panelY, num4, num3))
		{
			if (currentTab != 0)
			{
				currentTab = 0;
				UpdateCachedValues();
			}
			GameCanvas.clearAllPointerEvent();
			return;
		}
		if (GameCanvas.isPointerHoldIn(panelX + num4, panelY, num4, num3))
		{
			if (currentTab != 1)
			{
				currentTab = 1;
				UpdateCachedValues();
			}
			GameCanvas.clearAllPointerEvent();
			return;
		}
		if (GameCanvas.isPointerHoldIn(panelX + num4 * 2, panelY, MenuHelper.PanelWidth - num4 * 2, num3))
		{
			if (currentTab != 2)
			{
				currentTab = 2;
				MobTrainTab.LoadMobsFromMap();
				UpdateCachedValues();
			}
			GameCanvas.clearAllPointerEvent();
			return;
		}
		switch (currentTab)
		{
		case 0:
			AutoItemTab.HandleClick(panelX, contentY);
			break;
		case 1:
			SkillTrainTab.HandleClick(panelX, contentY);
			break;
		case 2:
			MobTrainTab.HandleClick(panelX, contentY);
			break;
		}
	}

	private static void HandleScroll()
	{
		int panelX = MenuHelper.PanelX;
		int panelY = MenuHelper.PanelY;
		int num = 30;
		int y = panelY + num + 8;
		if (GameCanvas.isPointerDown && GameCanvas.isPointerHoldIn(panelX, y, MenuHelper.PanelWidth, MenuHelper.ContentHeight))
		{
			int deltaY = GameCanvas.py - GameCanvas.pyLast;
			switch (currentTab)
			{
			case 0:
				AutoItemTab.HandleScroll(deltaY);
				break;
			case 1:
				SkillTrainTab.HandleScroll(deltaY);
				break;
			case 2:
				MobTrainTab.HandleScroll(deltaY);
				break;
			}
		}
	}

	private static void UpdateCachedValues()
	{
		int buttonPanelHeight = 0;
		if (currentTab == 1)
		{
			buttonPanelHeight = 26;
		}
		else if (currentTab == 2)
		{
			buttonPanelHeight = 78;
		}
		int num = currentTab;
		bool flag = false;
		bool flag2 = false;
		if (1 == 0)
		{
		}
		int num2 = num switch
		{
			0 => AutoItemTab.AutoItems.Length, 
			1 => SkillTrainTab.SkillTrains.Length, 
			2 => MobTrainTab.MobTrains.Count, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		int num3 = num2;
		bool flag3 = false;
		int num4 = num3;
		bool flag4 = false;
		int totalItems = num4;
		MenuHelper.UpdateCachedValues(currentTab, totalItems, buttonPanelHeight);
		int num5 = MenuHelper.CalculateMaxScrollOffset(totalItems);
		switch (currentTab)
		{
		case 0:
			if (AutoItemTab.ScrollOffset > num5)
			{
				AutoItemTab.ScrollOffset = num5;
			}
			break;
		case 1:
			if (SkillTrainTab.ScrollOffset > num5)
			{
				SkillTrainTab.ScrollOffset = num5;
			}
			break;
		case 2:
			if (MobTrainTab.ScrollOffset > num5)
			{
				MobTrainTab.ScrollOffset = num5;
			}
			break;
		}
	}

	public static void LoadMobsFromMap()
	{
		MobTrainTab.LoadMobsFromMap();
	}

	public static void UpdateMobTrainFlags()
	{
		MobTrainTab.UpdateMobTrainFlags();
	}

	public static void SaveConfig()
	{
		SkillTrainTab.SaveConfig();
	}

	public static void LoadConfig()
	{
		SkillTrainTab.LoadConfig();
	}

	public static void ResetConfig()
	{
		SkillTrainTab.ResetConfig();
	}

	public static void ToggleMenu(bool show, int? tabIndex = null)
	{
		isShowMenuVIP = show;
		if (!show)
		{
			GameCanvas.clearAllPointerEvent();
			return;
		}
		MobTrainTab.LoadMobsFromMap();
		if (tabIndex.HasValue)
		{
			int value = tabIndex.Value;
			int num = value;
			int num2 = num;
			if ((uint)num2 <= 2u && value != currentTab)
			{
				currentTab = value;
			}
		}
		UpdateCachedValues();
		EnsureCachedValues();
		GameCanvas.clearAllPointerEvent();
	}
}
