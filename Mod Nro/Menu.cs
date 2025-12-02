public class Menu
{
	public bool showMenu;

	public MyVector menuItems;

	public int menuSelectedItem;

	public int menuX;

	public int menuY;

	public int menuW;

	public int menuH;

	public static int[] menuTemY;

	public static int cmtoX;

	public static int cmx;

	public static int cmdy;

	public static int cmvy;

	public static int cmxLim;

	public static int xc;

	private Command left = new Command(mResources.SELECT, 0);

	private Command right = new Command(mResources.CLOSE, 0, GameCanvas.w - 71, GameCanvas.h - mScreen.cmdH + 1);

	private Command center;

	public static Image imgMenu1;

	public static Image imgMenu2;

	private bool disableClose;

	public int tDelay;

	public int w;

	private int pa;

	private bool trans;

	private int pointerDownTime;

	private int pointerDownFirstX;

	private int[] pointerDownLastX = new int[3];

	private bool pointerIsDowning;

	private bool isDownWhenRunning;

	private bool wantUpdateList;

	private int waitToPerform;

	private int cmRun;

	private bool touch;

	private bool close;

	private int cmvx;

	private int cmdx;

	private bool isClose;

	public bool[] isNotClose;

	public static void loadBg()
	{
		imgMenu1 = GameCanvas.loadImage("/mainImage/myTexture2dbtMenu1.png");
		imgMenu2 = GameCanvas.loadImage("/mainImage/myTexture2dbtMenu2.png");
	}

	public void startWithoutCloseButton(MyVector menuItems, int pos)
	{
		startAt(menuItems, pos);
		disableClose = true;
	}

	public void startAt(MyVector menuItems, int x, int y)
	{
		startAt(menuItems, 0);
		menuX = x;
		menuY = y;
		while (menuY + menuH > GameCanvas.h)
		{
			menuY -= 2;
		}
	}

	public void startAt(MyVector menuItems, int pos)
	{
		if (showMenu)
		{
			return;
		}
		isClose = false;
		touch = false;
		close = false;
		tDelay = 0;
		if (menuItems.size() == 1)
		{
			menuSelectedItem = 0;
			Command command = (Command)menuItems.elementAt(0);
			if (command != null && command.caption.Equals(mResources.saying))
			{
				command.performAction();
				showMenu = false;
				InfoDlg.showWait();
				return;
			}
		}
		SoundMn.gI().openMenu();
		isNotClose = new bool[menuItems.size()];
		for (int i = 0; i < isNotClose.Length; i++)
		{
			isNotClose[i] = false;
		}
		disableClose = false;
		ChatPopup.currChatPopup = null;
		Effect2.vEffect2.removeAllElements();
		Effect2.vEffect2Outside.removeAllElements();
		InfoDlg.hide();
		if (menuItems.size() != 0)
		{
			this.menuItems = menuItems;
			menuW = 60;
			menuH = 60;
			for (int j = 0; j < menuItems.size(); j++)
			{
				Command command2 = (Command)menuItems.elementAt(j);
				command2.isPlaySoundButton = false;
				int width = mFont.tahoma_7_yellow.getWidth(command2.caption);
				command2.subCaption = mFont.tahoma_7_yellow.splitFontArray(command2.caption, menuW - 10);
			}
			menuTemY = new int[menuItems.size()];
			menuX = (GameCanvas.w - menuItems.size() * menuW) / 2;
			if (menuX < 1)
			{
				menuX = 1;
			}
			menuY = GameCanvas.h - menuH - (Paint.hTab + 1) - 1;
			if (GameCanvas.isTouch)
			{
				menuY -= 3;
			}
			menuY += 27;
			for (int k = 0; k < menuTemY.Length; k++)
			{
				menuTemY[k] = GameCanvas.h;
			}
			showMenu = true;
			menuSelectedItem = 0;
			cmxLim = this.menuItems.size() * menuW - GameCanvas.w;
			if (cmxLim < 0)
			{
				cmxLim = 0;
			}
			cmtoX = 0;
			cmx = 0;
			xc = 50;
			w = menuItems.size() * menuW - 1;
			if (w > GameCanvas.w - 2)
			{
				w = GameCanvas.w - 2;
			}
			if (GameCanvas.isTouch && !Main.isPC)
			{
				menuSelectedItem = -1;
			}
		}
	}

	public bool isScrolling()
	{
		if ((!isClose && menuTemY[menuTemY.Length - 1] > menuY) || (isClose && menuTemY[menuTemY.Length - 1] < GameCanvas.h))
		{
			return true;
		}
		return false;
	}

	public void updateMenuKey()
	{
		if ((GameScr.gI().activeRongThan && GameScr.gI().isUseFreez) || !showMenu || isScrolling())
		{
			return;
		}
		bool flag = false;
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] || GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
		{
			flag = true;
			menuSelectedItem--;
			if (menuSelectedItem < 0)
			{
				menuSelectedItem = menuItems.size() - 1;
			}
		}
		else if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] || GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
		{
			flag = true;
			menuSelectedItem++;
			if (menuSelectedItem > menuItems.size() - 1)
			{
				menuSelectedItem = 0;
			}
		}
		else if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
		{
			if (center != null)
			{
				if (center.idAction > 0)
				{
					if (center.actionListener == GameScr.gI())
					{
						GameScr.gI().actionPerform(center.idAction, center.p);
					}
					else
					{
						perform(center.idAction, center.p);
					}
				}
			}
			else
			{
				waitToPerform = 2;
			}
		}
		else if (GameCanvas.keyPressed[12] && !GameScr.gI().isRongThanMenu())
		{
			if (isScrolling())
			{
				return;
			}
			if (left.idAction > 0)
			{
				perform(left.idAction, left.p);
			}
			else
			{
				waitToPerform = 2;
			}
			SoundMn.gI().buttonClose();
		}
		else if (!GameScr.gI().isRongThanMenu() && !disableClose && (GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(right)))
		{
			if (isScrolling())
			{
				return;
			}
			if (!close)
			{
				close = true;
			}
			isClose = true;
			SoundMn.gI().buttonClose();
		}
		if (flag)
		{
			cmtoX = menuSelectedItem * menuW + menuW - GameCanvas.w / 2;
			if (cmtoX > cmxLim)
			{
				cmtoX = cmxLim;
			}
			if (cmtoX < 0)
			{
				cmtoX = 0;
			}
			if (menuSelectedItem == menuItems.size() - 1 || menuSelectedItem == 0)
			{
				cmx = cmtoX;
			}
		}
		bool flag2 = true;
		if (GameCanvas.panel.cp != null && GameCanvas.panel.cp.isClip)
		{
			if (!GameCanvas.isPointerHoldIn(GameCanvas.panel.cp.cx, 0, GameCanvas.panel.cp.sayWidth + 2, GameCanvas.panel.cp.ch))
			{
				flag2 = true;
			}
			else
			{
				flag2 = false;
				GameCanvas.panel.cp.updateKey();
			}
		}
		if (!disableClose && GameCanvas.isPointerJustRelease && !GameCanvas.isPointer(menuX, menuY, w, menuH) && !pointerIsDowning && !GameScr.gI().isRongThanMenu() && flag2)
		{
			if (!isScrolling())
			{
				pointerDownTime = (pointerDownFirstX = 0);
				pointerIsDowning = false;
				GameCanvas.clearAllPointerEvent();
				Res.outz("menu select= " + menuSelectedItem);
				isClose = true;
				close = true;
				SoundMn.gI().buttonClose();
			}
			return;
		}
		if (GameCanvas.isPointerDown)
		{
			if (!pointerIsDowning && GameCanvas.isPointer(menuX, menuY, w, menuH))
			{
				for (int i = 0; i < pointerDownLastX.Length; i++)
				{
					pointerDownLastX[0] = GameCanvas.px;
				}
				pointerDownFirstX = GameCanvas.px;
				pointerIsDowning = true;
				isDownWhenRunning = cmRun != 0;
				cmRun = 0;
			}
			else if (pointerIsDowning)
			{
				pointerDownTime++;
				if (pointerDownTime > 5 && pointerDownFirstX == GameCanvas.px && !isDownWhenRunning)
				{
					pointerDownFirstX = -1000;
					menuSelectedItem = (cmtoX + GameCanvas.px - menuX) / menuW;
				}
				int num = GameCanvas.px - pointerDownLastX[0];
				if (num != 0 && menuSelectedItem != -1)
				{
					menuSelectedItem = -1;
				}
				for (int num2 = pointerDownLastX.Length - 1; num2 > 0; num2--)
				{
					pointerDownLastX[num2] = pointerDownLastX[num2 - 1];
				}
				pointerDownLastX[0] = GameCanvas.px;
				cmtoX -= num;
				if (cmtoX < 0)
				{
					cmtoX = 0;
				}
				if (cmtoX > cmxLim)
				{
					cmtoX = cmxLim;
				}
				if (cmx < 0 || cmx > cmxLim)
				{
					num /= 2;
				}
				cmx -= num;
				if (cmx < -(GameCanvas.h / 3))
				{
					wantUpdateList = true;
				}
				else
				{
					wantUpdateList = false;
				}
			}
		}
		if (GameCanvas.isPointerJustRelease && pointerIsDowning)
		{
			int i2 = GameCanvas.px - pointerDownLastX[0];
			GameCanvas.isPointerJustRelease = false;
			if (Res.abs(i2) < 20 && Res.abs(GameCanvas.px - pointerDownFirstX) < 20 && !isDownWhenRunning)
			{
				cmRun = 0;
				cmtoX = cmx;
				pointerDownFirstX = -1000;
				menuSelectedItem = (cmtoX + GameCanvas.px - menuX) / menuW;
				pointerDownTime = 0;
				waitToPerform = 10;
			}
			else if (menuSelectedItem != -1 && pointerDownTime > 5)
			{
				pointerDownTime = 0;
				waitToPerform = 1;
			}
			else if (menuSelectedItem == -1 && !isDownWhenRunning)
			{
				if (cmx < 0)
				{
					cmtoX = 0;
				}
				else if (cmx > cmxLim)
				{
					cmtoX = cmxLim;
				}
				else
				{
					int num3 = GameCanvas.px - pointerDownLastX[0] + (pointerDownLastX[0] - pointerDownLastX[1]) + (pointerDownLastX[1] - pointerDownLastX[2]);
					num3 = ((num3 > 10) ? 10 : ((num3 < -10) ? (-10) : 0));
					cmRun = -num3 * 100;
				}
			}
			pointerIsDowning = false;
			pointerDownTime = 0;
			GameCanvas.isPointerJustRelease = false;
		}
		GameCanvas.clearKeyPressed();
		GameCanvas.clearKeyHold();
	}

	public void moveCamera()
	{
		if (cmRun != 0 && !pointerIsDowning)
		{
			cmtoX += cmRun / 100;
			if (cmtoX < 0)
			{
				cmtoX = 0;
			}
			else if (cmtoX > cmxLim)
			{
				cmtoX = cmxLim;
			}
			else
			{
				cmx = cmtoX;
			}
			cmRun = cmRun * 9 / 10;
			if (cmRun < 100 && cmRun > -100)
			{
				cmRun = 0;
			}
		}
		if (cmx != cmtoX && !pointerIsDowning)
		{
			cmvx = cmtoX - cmx << 2;
			cmdx += cmvx;
			cmx += cmdx >> 4;
			cmdx &= 15;
		}
	}

	public void paintMenu(mGraphics g)
	{
		if (GameScr.gI().activeRongThan && GameScr.gI().isUseFreez)
		{
			return;
		}
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		g.translate(-cmx, 0);
		for (int i = 0; i < menuItems.size(); i++)
		{
			if (i == menuSelectedItem)
			{
				g.drawImage(imgMenu2, menuX + i * menuW + 1, menuTemY[i], 0);
			}
			else
			{
				g.drawImage(imgMenu1, menuX + i * menuW + 1, menuTemY[i], 0);
			}
			Command command = (Command)menuItems.elementAt(i);
			string[] array = command.subCaption;
			if (array == null)
			{
				array = new string[1] { ((Command)menuItems.elementAt(i)).caption };
			}
			int num = menuTemY[i] + (menuH - array.Length * 14) / 2 + 1;
			for (int j = 0; j < array.Length; j++)
			{
				if (i == menuSelectedItem)
				{
					mFont.tahoma_7b_green2.drawString(g, array[j], menuX + i * menuW + menuW / 2, num + j * 14, 2);
				}
				else if (command.isDisplay)
				{
					mFont.tahoma_7b_red.drawString(g, array[j], menuX + i * menuW + menuW / 2, num + j * 14, 2);
				}
				else
				{
					mFont.tahoma_7b_dark.drawString(g, array[j], menuX + i * menuW + menuW / 2, num + j * 14, 2);
				}
			}
		}
		g.translate(-g.getTranslateX(), -g.getTranslateY());
	}

	public void doCloseMenu()
	{
		Res.outz("CLOSE MENU");
		isClose = false;
		showMenu = false;
		InfoDlg.hide();
		if (close)
		{
			GameCanvas.panel.cp = null;
			Char.chatPopup = null;
			if (GameCanvas.panel2 != null && GameCanvas.panel2.cp != null)
			{
				GameCanvas.panel2.cp = null;
			}
		}
		else
		{
			if (!touch)
			{
				return;
			}
			GameCanvas.panel.cp = null;
			if (GameCanvas.panel2 != null && GameCanvas.panel2.cp != null)
			{
				GameCanvas.panel2.cp = null;
			}
			if (menuSelectedItem >= 0)
			{
				Command command = (Command)menuItems.elementAt(menuSelectedItem);
				if (command != null)
				{
					SoundMn.gI().buttonClose();
					command.performAction();
				}
			}
		}
	}

	public void performSelect()
	{
		InfoDlg.hide();
		if (menuSelectedItem >= 0)
		{
			((Command)menuItems.elementAt(menuSelectedItem))?.performAction();
		}
	}

	public void updateMenu()
	{
		moveCamera();
		if (!isClose)
		{
			tDelay++;
			for (int i = 0; i < menuTemY.Length; i++)
			{
				if (menuTemY[i] > menuY)
				{
					int num = menuTemY[i] - menuY >> 1;
					if (num < 1)
					{
						num = 1;
					}
					if (tDelay > i)
					{
						menuTemY[i] -= num;
					}
				}
			}
			if (menuTemY[menuTemY.Length - 1] <= menuY)
			{
				tDelay = 0;
			}
		}
		else
		{
			tDelay++;
			for (int j = 0; j < menuTemY.Length; j++)
			{
				if (menuTemY[j] < GameCanvas.h)
				{
					int num2 = (GameCanvas.h - menuTemY[j] >> 1) + 2;
					if (num2 < 1)
					{
						num2 = 1;
					}
					if (tDelay > j)
					{
						menuTemY[j] += num2;
					}
				}
			}
			if (menuTemY[menuTemY.Length - 1] >= GameCanvas.h)
			{
				tDelay = 0;
				doCloseMenu();
			}
		}
		if (xc != 0)
		{
			xc >>= 1;
			if (xc < 0)
			{
				xc = 0;
			}
		}
		if (isScrolling() || waitToPerform <= 0)
		{
			return;
		}
		waitToPerform--;
		if (waitToPerform == 0)
		{
			if (menuSelectedItem >= 0 && !isNotClose[menuSelectedItem])
			{
				isClose = true;
				touch = true;
				GameCanvas.panel.cp = null;
			}
			else
			{
				performSelect();
			}
		}
	}

	public void perform(int idAction, object p)
	{
	}
}
