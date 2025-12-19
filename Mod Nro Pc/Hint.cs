using System;

public class Hint
{
	public static int x;

	public static int y;

	public static int type;

	public static int t;

	public static int xF;

	public static int yF;

	public static bool isShow;

	public static bool activeClick;

	public static bool isViewMap;

	public static bool isCloseMap;

	public static bool isViewPotential;

	public static bool isPaint;

	public static bool isCamera;

	public static int trans;

	public static bool paintFlare;

	public static bool isPaintArrow;

	private int s = 2;

	public static bool isOnTask(int tastId, int index)
	{
		if (Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId == tastId && Char.myCharz().taskMaint.index == index)
		{
			return true;
		}
		return false;
	}

	public static bool isPaintz()
	{
		if (isOnTask(0, 3) && GameCanvas.panel.currentTabIndex == 0 && (GameCanvas.panel.cmy < 0 || GameCanvas.panel.cmy > 30))
		{
			return false;
		}
		if (isOnTask(2, 0) && GameCanvas.panel.isShow && GameCanvas.panel.currentTabIndex != 0)
		{
			return false;
		}
		return true;
	}

	public static void clickNpc()
	{
		if (GameCanvas.panel.isShow)
		{
			isPaint = false;
		}
		if (GameScr.getNpcTask() != null)
		{
			x = GameScr.getNpcTask().cx;
			y = GameScr.getNpcTask().cy;
			trans = 0;
			isCamera = true;
			type = (GameCanvas.isTouch ? 1 : 0);
		}
	}

	public static void nextMap(int index)
	{
		if (!GameCanvas.panel.isShow && PopUp.vPopups.size() - 1 >= index)
		{
			PopUp popUp = (PopUp)PopUp.vPopups.elementAt(index);
			x = popUp.cx + popUp.sayWidth / 2;
			y = popUp.cy + 30;
			if (popUp.isHide || !popUp.isPaint)
			{
				isPaint = false;
			}
			else
			{
				isPaint = true;
			}
			type = 0;
			isCamera = true;
			trans = 0;
			if (!GameCanvas.isTouch)
			{
				isPaint = false;
			}
		}
	}

	public static void clickMob()
	{
		type = 1;
		if (GameCanvas.panel.isShow)
		{
			isPaint = false;
		}
		bool flag = false;
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob.isHintFocus)
			{
				flag = true;
				break;
			}
		}
		for (int j = 0; j < GameScr.vMob.size(); j++)
		{
			Mob mob2 = (Mob)GameScr.vMob.elementAt(j);
			if (mob2.isHintFocus)
			{
				x = mob2.x;
				y = mob2.y + 5;
				isCamera = true;
				if (mob2.status == 0)
				{
					mob2.isHintFocus = false;
				}
				break;
			}
			if (!flag)
			{
				if (mob2.status != 0)
				{
					mob2.isHintFocus = true;
					break;
				}
				mob2.isHintFocus = false;
			}
		}
	}

	public static bool isHaveItem()
	{
		if (GameCanvas.panel.isShow)
		{
			isPaint = false;
		}
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			if (itemMap.playerId == Char.myCharz().charID && itemMap.template.id == 73)
			{
				type = 1;
				x = itemMap.x;
				y = itemMap.y + 5;
				isCamera = true;
				return true;
			}
		}
		return false;
	}

	public static void paintArrowPointToHint(mGraphics g)
	{
		try
		{
			if (!isPaintArrow || (x > GameScr.cmx && x < GameScr.cmx + GameScr.gW && y > GameScr.cmy && y < GameScr.cmy + GameScr.gH) || GameCanvas.gameTick % 10 < 5 || ChatPopup.currChatPopup != null || ChatPopup.serverChatPopUp != null || GameCanvas.panel.isShow || !isCamera)
			{
				return;
			}
			int num = x - Char.myCharz().cx;
			int num2 = y - Char.myCharz().cy;
			int num3 = 0;
			int num4 = 0;
			int arg = 0;
			if (num > 0 && num2 >= 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					num3 = GameScr.gW - 10;
					num4 = GameScr.gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						num4 = GameScr.gH / 2 + 10;
					}
					arg = 0;
				}
				else
				{
					num3 = GameScr.gW / 2;
					num4 = GameScr.gH - 10;
					arg = 5;
				}
			}
			else if (num >= 0 && num2 < 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					num3 = GameScr.gW - 10;
					num4 = GameScr.gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						num4 = GameScr.gH / 2 + 10;
					}
					arg = 0;
				}
				else
				{
					num3 = GameScr.gW / 2;
					num4 = 10;
					arg = 6;
				}
			}
			if (num < 0 && num2 >= 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					num3 = 10;
					num4 = GameScr.gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						num4 = GameScr.gH / 2 + 10;
					}
					arg = 3;
				}
				else
				{
					num3 = GameScr.gW / 2;
					num4 = GameScr.gH - 10;
					arg = 5;
				}
			}
			else if (num <= 0 && num2 < 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					num3 = 10;
					num4 = GameScr.gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						num4 = GameScr.gH / 2 + 10;
					}
					arg = 3;
				}
				else
				{
					num3 = GameScr.gW / 2;
					num4 = 10;
					arg = 6;
				}
			}
			GameScr.resetTranslate(g);
			g.drawRegion(GameScr.arrow, 0, 0, 13, 16, arg, num3, num4, StaticObj.VCENTER_HCENTER);
		}
		catch (Exception)
		{
		}
	}

	public static void paint(mGraphics g)
	{
		if (ChatPopup.serverChatPopUp != null || Char.myCharz().isUsePlane || Char.myCharz().isTeleport)
		{
			return;
		}
		paintArrowPointToHint(g);
		if (GameCanvas.menu.tDelay == 0 && isPaint && ChatPopup.scr == null && !Char.ischangingMap && GameCanvas.currentScreen == GameScr.gI() && (!GameCanvas.panel.isShow || GameCanvas.panel.cmx == 0))
		{
			if (isCamera)
			{
				g.translate(-GameScr.cmx, -GameScr.cmy);
			}
			if (trans == 0)
			{
				g.drawImage(Panel.imgBantay, x - 15, y, 0);
			}
			if (trans == 1)
			{
				g.drawRegion(Panel.imgBantay, 0, 0, 14, 16, 2, x + 15, y, StaticObj.TOP_RIGHT);
			}
			if (paintFlare)
			{
				g.drawImage(ItemMap.imageFlare, x, y, 3);
			}
		}
	}

	public static void hint()
	{
		if (Char.myCharz().taskMaint != null && GameCanvas.currentScreen == GameScr.instance)
		{
			int taskId = Char.myCharz().taskMaint.taskId;
			int index = Char.myCharz().taskMaint.index;
			isCamera = false;
			trans = 0;
			type = 0;
			isPaint = true;
			isPaintArrow = true;
			if (GameCanvas.menu.showMenu && taskId > 0)
			{
				isPaint = false;
			}
			switch (taskId)
			{
			case 0:
				if (ChatPopup.currChatPopup != null || Char.myCharz().statusMe == 14)
				{
					x = GameCanvas.w / 2;
					y = GameCanvas.h - 15;
					return;
				}
				if (index == 0 && TileMap.vGo.size() != 0)
				{
					x = ((Waypoint)TileMap.vGo.elementAt(0)).minX - 100;
					y = ((Waypoint)TileMap.vGo.elementAt(0)).minY + 40;
					isCamera = true;
				}
				if (index == 1)
				{
					nextMap(0);
				}
				if (index == 2)
				{
					clickNpc();
				}
				if (index == 3)
				{
					if (!GameCanvas.panel.isShow)
					{
						clickNpc();
					}
					else if (GameCanvas.panel.currentTabIndex == 0)
					{
						if (GameCanvas.panel.cp == null)
						{
							x = GameCanvas.panel.xScroll + GameCanvas.panel.wScroll / 2;
							y = GameCanvas.panel.yScroll + 20;
						}
						else if (GameCanvas.menu.tDelay != 0)
						{
							x = GameCanvas.panel.xScroll + 25;
							y = GameCanvas.panel.yScroll + 60;
						}
					}
					else if (GameCanvas.panel.currentTabIndex == 1)
					{
						x = GameCanvas.panel.startTabPos + 10;
						y = 65;
					}
				}
				if (index == 4)
				{
					if (GameCanvas.panel.isShow)
					{
						x = GameCanvas.panel.cmdClose.x + 5;
						y = GameCanvas.panel.cmdClose.y + 5;
					}
					else if (GameCanvas.menu.showMenu)
					{
						x = GameCanvas.w / 2;
						y = GameCanvas.h - 20;
					}
					else
					{
						clickNpc();
					}
				}
				if (index == 5)
				{
					clickNpc();
				}
				return;
			case 1:
				if (ChatPopup.currChatPopup != null || Char.myCharz().statusMe == 14)
				{
					x = GameCanvas.w / 2;
					y = GameCanvas.h - 15;
					return;
				}
				if (index == 0)
				{
					if (TileMap.isOfflineMap())
					{
						nextMap(0);
					}
					else
					{
						clickMob();
					}
				}
				if (index == 1)
				{
					if (!TileMap.isOfflineMap())
					{
						nextMap(1);
					}
					else
					{
						clickNpc();
					}
				}
				return;
			case 2:
				if (ChatPopup.currChatPopup != null || Char.myCharz().statusMe == 14)
				{
					x = GameCanvas.w / 2;
					y = GameCanvas.h - 15;
					return;
				}
				if (index == 0)
				{
					if (!TileMap.isOfflineMap())
					{
						isViewMap = true;
					}
					if (!GameCanvas.panel.isShow)
					{
						if (!isViewMap)
						{
							x = GameScr.gI().cmdMenu.x;
							y = GameScr.gI().cmdMenu.y + 13;
							trans = 1;
						}
						else
						{
							if (GameScr.getTaskMapId() == TileMap.mapID)
							{
								if (!isHaveItem())
								{
									clickMob();
								}
							}
							else
							{
								nextMap(0);
							}
							if (isViewMap)
							{
								isCloseMap = true;
							}
						}
					}
					else if (!isViewMap)
					{
						if (GameCanvas.panel.currentTabIndex == 0)
						{
							int num = ((GameCanvas.h <= 300) ? 10 : 15);
							x = GameCanvas.panel.xScroll + GameCanvas.panel.wScroll / 2;
							y = GameCanvas.panel.yScroll + GameCanvas.panel.hScroll - num;
						}
						else
						{
							x = GameCanvas.panel.startTabPos + 10;
							y = 65;
						}
					}
					else if (!isCloseMap)
					{
						x = GameCanvas.panel.cmdClose.x + 5;
						y = GameCanvas.panel.cmdClose.y + 5;
					}
					else
					{
						isPaint = false;
					}
					if (Char.myCharz().cMP <= 0)
					{
						x = GameScr.xHP + 5;
						y = GameScr.yHP + 13;
						isCamera = false;
					}
				}
				if (index == 1)
				{
					isPaint = false;
					isPaintArrow = false;
				}
				return;
			case 3:
				if (ChatPopup.currChatPopup != null || Char.myCharz().statusMe == 14)
				{
					x = GameCanvas.w / 2;
					y = GameCanvas.h - 15;
				}
				else if (index == 0)
				{
					if (!GameCanvas.panel.isShow)
					{
						if (!isViewPotential)
						{
							x = GameScr.gI().cmdMenu.x;
							y = GameScr.gI().cmdMenu.y + 13;
							trans = 1;
						}
						else
						{
							if (GameScr.getTaskMapId() == TileMap.mapID)
							{
								if (!isHaveItem())
								{
									clickMob();
								}
							}
							else
							{
								nextMap(0);
							}
							if (isViewMap)
							{
								isCloseMap = true;
							}
						}
					}
					else if (!isViewPotential)
					{
						int num2 = ((GameCanvas.h <= 300) ? 10 : 15);
						x = GameCanvas.panel.xScroll + 10 + 108 - 18;
						y = 65;
					}
					else if (!isCloseMap)
					{
						x = GameCanvas.panel.cmdClose.x + 5;
						y = GameCanvas.panel.cmdClose.y + 5;
					}
					else
					{
						isPaint = false;
					}
					if (Char.myCharz().cMP <= 0)
					{
						x = GameScr.xHP + 5;
						y = GameScr.yHP + 13;
						isCamera = false;
					}
				}
				else
				{
					isPaint = false;
					isPaintArrow = false;
				}
				return;
			}
			if (Char.myCharz().taskMaint.taskId == 9 && Char.myCharz().taskMaint.index == 2)
			{
				for (int i = 0; i < PopUp.vPopups.size(); i++)
				{
					PopUp popUp = (PopUp)PopUp.vPopups.elementAt(i);
					if (popUp.cy <= 24)
					{
						x = popUp.cx + popUp.sayWidth / 2;
						y = popUp.cy + 30;
						isCamera = true;
						isPaint = false;
						isPaintArrow = true;
						return;
					}
				}
			}
			isPaint = false;
			isPaintArrow = false;
		}
		else
		{
			isPaint = false;
			isPaintArrow = false;
		}
	}

	public static void update()
	{
		hint();
		int num = ((trans != 0) ? (-2) : 2);
		if (!activeClick)
		{
			paintFlare = false;
			t++;
			if (t == 50)
			{
				t = 0;
				activeClick = true;
			}
			return;
		}
		t++;
		if (type == 0)
		{
			if (t == 2)
			{
				x += 2 * num;
				y -= 4;
				paintFlare = true;
			}
			if (t == 4)
			{
				x -= 2 * num;
				y += 4;
				activeClick = false;
				paintFlare = false;
				t = 0;
			}
			if (t > 4)
			{
				activeClick = false;
			}
		}
		if (type != 1)
		{
			return;
		}
		if (t == 2)
		{
			if (GameCanvas.isTouch)
			{
				GameScr.startFlyText(mResources.press_twice, x, y + 10, 0, 20, mFont.MISS_ME);
			}
			paintFlare = true;
			x += 2 * num;
			y -= 4;
		}
		if (t == 4)
		{
			paintFlare = false;
			x -= num;
			y += 2;
		}
		if (t == 6)
		{
			paintFlare = true;
			x += num;
			y -= 2;
		}
		if (t == 8)
		{
			paintFlare = false;
			x -= num;
			y += 2;
		}
		if (t == 10)
		{
			x -= num;
			y += 2;
			activeClick = false;
			t = 0;
		}
	}
}
