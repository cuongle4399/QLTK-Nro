public class TabClanIcon : IActionListener
{
	private int x;

	private int y;

	private int w;

	private int h;

	private Command left;

	private Command right;

	private Command center;

	private int WIDTH = 24;

	public int nItem;

	private int disStart = 50;

	public static Scroll scrMain;

	public int cmtoX;

	public int cmx;

	public int cmvx;

	public int cmdx;

	public bool isShow;

	public bool isGetName;

	public string text;

	private bool isRequest;

	private bool isUpdate;

	public MyVector vItems = new MyVector();

	private int msgID;

	private int select;

	private int lastSelect;

	private ScrollResult sr;

	public TabClanIcon()
	{
		left = new Command(mResources.SELECT, this, 1, null);
		right = new Command(mResources.CLOSE, this, 2, null);
	}

	public void init()
	{
		if (isGetName)
		{
			w = 170;
			h = 118;
			x = GameCanvas.w / 2 - w / 2;
			y = GameCanvas.h / 2 - h / 2;
		}
		else
		{
			w = 170;
			h = 170;
			x = GameCanvas.w / 2 - w / 2;
			y = GameCanvas.h / 2 - h / 2;
			if (GameCanvas.h < 240)
			{
				y -= 10;
			}
		}
		cmx = x;
		cmtoX = 0;
		if (!isRequest)
		{
			nItem = ClanImage.vClanImage.size();
		}
		else
		{
			nItem = vItems.size();
		}
		if (GameCanvas.isTouch)
		{
			left.x = x;
			left.y = y + h + 5;
			right.x = x + w - 68;
			right.y = y + h + 5;
		}
		scrMain = new Scroll();
		scrMain.setStyle(nItem, WIDTH, x, y + disStart, w, h - disStart, styleUPDOWN: true, 1);
	}

	public void show(bool isGetName)
	{
		if (Char.myCharz().clan != null)
		{
			isUpdate = true;
		}
		isShow = true;
		this.isGetName = isGetName;
		init();
	}

	public void showRequest(int msgID)
	{
		isShow = true;
		isRequest = true;
		this.msgID = msgID;
		init();
	}

	public void hide()
	{
		cmtoX = x + w;
		SmallImage.clearHastable();
	}

	public void paintPeans(mGraphics g)
	{
	}

	public void paintIcon(mGraphics g)
	{
		g.translate(-cmx, 0);
		PopUp.paintPopUp(g, x, y - 17, w, h + 17, -1, isButton: true);
		mFont.tahoma_7b_dark.drawString(g, mResources.select_clan_icon, x + w / 2, y - 7, 2);
		if (lastSelect >= 0 && lastSelect <= ClanImage.vClanImage.size() - 1)
		{
			ClanImage clanImage = (ClanImage)ClanImage.vClanImage.elementAt(lastSelect);
			if (clanImage.idImage != null)
			{
				Char.myCharz().paintBag(g, clanImage.idImage, GameCanvas.w / 2, y + 45, 1, isPaintChar: false);
			}
		}
		Char.myCharz().paintCharBody(g, GameCanvas.w / 2, y + 45, 1, Char.myCharz().cf, isPaintBag: false);
		g.setClip(x, y + disStart, w, h - disStart - 10);
		if (scrMain != null)
		{
			g.translate(0, -scrMain.cmy);
		}
		for (int i = 0; i < nItem; i++)
		{
			int num = x + 10;
			int num2 = y + i * WIDTH + disStart;
			if (num2 + WIDTH - ((scrMain != null) ? scrMain.cmy : 0) >= y + disStart && num2 - ((scrMain != null) ? scrMain.cmy : 0) <= y + disStart + h)
			{
				ClanImage clanImage2 = (ClanImage)ClanImage.vClanImage.elementAt(i);
				mFont mFont2 = mFont.tahoma_7_grey;
				if (i == lastSelect)
				{
					mFont2 = mFont.tahoma_7_blue;
				}
				if (clanImage2.name != null)
				{
					mFont2.drawString(g, clanImage2.name, num + 20, num2, 0);
				}
				if (clanImage2.xu > 0)
				{
					mFont2.drawString(g, clanImage2.xu + " " + mResources.XU, num + w - 20, num2, mFont.RIGHT);
				}
				else if (clanImage2.luong > 0)
				{
					mFont2.drawString(g, clanImage2.luong + " " + mResources.LUONG, num + w - 20, num2, mFont.RIGHT);
				}
				if (clanImage2.idImage != null)
				{
					SmallImage.drawSmallImage(g, clanImage2.idImage[0], num, num2, 0, 0);
				}
			}
		}
		g.translate(0, -g.getTranslateY());
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		GameCanvas.paintz.paintCmdBar(g, left, center, right);
	}

	public void paint(mGraphics g)
	{
		if (!isRequest)
		{
			paintIcon(g);
		}
		else
		{
			paintPeans(g);
		}
	}

	public void update()
	{
		if (scrMain != null)
		{
			scrMain.updatecm();
		}
		if (cmx != cmtoX)
		{
			cmvx = cmtoX - cmx << 2;
			cmdx += cmvx;
			cmx += cmdx >> 3;
			cmdx &= 15;
		}
		if (Math.abs(cmtoX - cmx) < 10)
		{
			cmx = cmtoX;
		}
		if (cmx >= x + w - 10 && cmtoX >= x + w - 10)
		{
			isShow = false;
		}
	}

	public void updateKey()
	{
		if (left != null && (GameCanvas.keyPressed[12] || mScreen.getCmdPointerLast(left)))
		{
			left.performAction();
		}
		if (right != null && (GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(right)))
		{
			right.performAction();
		}
		if (center != null && (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(center)))
		{
			center.performAction();
		}
		if (!isGetName)
		{
			if (scrMain == null)
			{
				return;
			}
			if (GameCanvas.isTouch)
			{
				scrMain.updateKey();
				select = scrMain.selectedItem;
			}
			if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] = false;
				select--;
				if (select < 0)
				{
					select = nItem - 1;
				}
				scrMain.moveTo(select * scrMain.ITEM_SIZE);
			}
			if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] = false;
				select++;
				if (select > nItem - 1)
				{
					select = 0;
				}
				scrMain.moveTo(select * scrMain.ITEM_SIZE);
			}
			if (select != -1)
			{
				lastSelect = select;
			}
		}
		GameCanvas.clearKeyHold();
		GameCanvas.clearKeyPressed();
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 2)
		{
			hide();
		}
		if (idAction != 1 || isGetName)
		{
			return;
		}
		if (!isRequest)
		{
			if (lastSelect >= 0)
			{
				hide();
				if (Char.myCharz().clan == null)
				{
					Service.gI().getClan(2, ((ClanImage)ClanImage.vClanImage.elementAt(lastSelect)).ID, text);
				}
				else
				{
					Service.gI().getClan(4, ((ClanImage)ClanImage.vClanImage.elementAt(lastSelect)).ID, string.Empty);
				}
			}
		}
		else if (lastSelect >= 0)
		{
			Item item = (Item)vItems.elementAt(select);
		}
	}
}
