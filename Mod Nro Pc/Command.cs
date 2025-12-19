public class Command
{
	public bool isDisplay;

	public ActionChat actionChat;

	public string caption;

	public string[] subCaption;

	public IActionListener actionListener;

	public int idAction;

	public bool isPlaySoundButton = true;

	public Image img;

	public Image imgFocus;

	public Image imgBtn;

	public int x;

	public int y;

	public int w = mScreen.cmdW;

	public int h = mScreen.cmdH;

	public int hw;

	private int lenCaption;

	public bool isFocus;

	public object p;

	public int type;

	public string caption2 = string.Empty;

	public static Image btn0left;

	public static Image btn0mid;

	public static Image btn0right;

	public static Image btn1left;

	public static Image btn1mid;

	public static Image btn1right;

	public bool cmdClosePanel;

	public bool isPaintNew;

	public Command(string caption, IActionListener actionListener, int action, object p, int x, int y)
	{
		this.caption = caption;
		idAction = action;
		this.actionListener = actionListener;
		this.p = p;
		this.x = x;
		this.y = y;
	}

	public Command()
	{
	}

	public Command(string caption, IActionListener actionListener, int action, object p)
	{
		this.caption = caption;
		idAction = action;
		this.actionListener = actionListener;
		this.p = p;
	}

	public Command(string caption, int action, object p)
	{
		this.caption = caption;
		idAction = action;
		this.p = p;
	}

	public Command(string caption, int action)
	{
		this.caption = caption;
		idAction = action;
	}

	public Command(string caption, int action, int x, int y)
	{
		this.caption = caption;
		idAction = action;
		this.x = x;
		this.y = y;
	}

	public void perform(string str)
	{
		if (actionChat != null)
		{
			actionChat(str);
		}
	}

	public void performAction()
	{
		GameCanvas.clearAllPointerEvent();
		if (isPlaySoundButton && ((caption != null && !caption.Equals(string.Empty) && !caption.Equals(mResources.saying)) || img != null))
		{
			SoundMn.gI().buttonClick();
		}
		if (idAction > 0)
		{
			if (actionListener != null)
			{
				actionListener.perform(idAction, p);
			}
			else
			{
				GameScr.gI().actionPerform(idAction, p);
			}
		}
	}

	public void setType()
	{
		type = 1;
		w = 160;
		hw = 80;
	}

	public void paint(mGraphics g)
	{
		if (img != null)
		{
			g.drawImage(img, x, y + mGraphics.addYWhenOpenKeyBoard, 0);
			if (isFocus)
			{
				if (imgFocus == null)
				{
					if (cmdClosePanel)
					{
						g.drawImage(ItemMap.imageFlare, x + 8, y + mGraphics.addYWhenOpenKeyBoard + 8, 3);
					}
					else
					{
						g.drawImage(ItemMap.imageFlare, x - (img.Equals(GameScr.imgMenu) ? 10 : 0), y + mGraphics.addYWhenOpenKeyBoard, 0);
					}
				}
				else
				{
					g.drawImage(imgFocus, x, y + mGraphics.addYWhenOpenKeyBoard, 0);
				}
			}
			if (caption != "menu" && caption != null)
			{
				if (!isFocus)
				{
					mFont.tahoma_7b_dark.drawString(g, caption, x + mGraphics.getImageWidth(img) / 2, y + mGraphics.getImageHeight(img) / 2 - 5, 2);
				}
				else
				{
					mFont.tahoma_7b_green2.drawString(g, caption, x + mGraphics.getImageWidth(img) / 2, y + mGraphics.getImageHeight(img) / 2 - 5, 2);
				}
			}
			return;
		}
		if (caption != string.Empty)
		{
			if (!isFocus)
			{
				paintOngMau(btn0left, btn0mid, btn0right, x, y, w, g);
			}
			else
			{
				paintOngMau(btn1left, btn1mid, btn1right, x, y, w, g);
			}
		}
		int num = 0;
		int num2 = x + w / 2;
		if (imgBtn != null)
		{
			num = imgBtn.getWidth();
			num2 = x + num;
			if (!isFocus)
			{
				g.drawImage(imgBtn, x, y, 0);
			}
			else
			{
				g.drawImage(imgBtn, x, y + 1, 0);
			}
		}
		if (!isFocus)
		{
			mFont.tahoma_7b_dark.drawString(g, caption, num2, y + 7, (num == 0) ? 2 : 0);
		}
		else
		{
			mFont.tahoma_7b_green2.drawString(g, caption, num2, y + 7, (num == 0) ? 2 : 0);
		}
	}

	public static void paintOngMau(Image img0, Image img1, Image img2, int x, int y, int size, mGraphics g)
	{
		for (int i = 10; i <= size - 20; i += 10)
		{
			g.drawImage(img1, x + i, y, 0);
		}
		int num = size % 10;
		if (num > 0)
		{
			g.drawRegion(img1, 0, 0, num, 24, 0, x + size - 10 - num, y, 0);
		}
		g.drawImage(img0, x, y, 0);
		g.drawImage(img2, x + size - 10, y, 0);
	}

	public bool isPointerPressInside()
	{
		isFocus = false;
		if (GameCanvas.isPointerHoldIn(x, y, w, h))
		{
			if (GameCanvas.isPointerDown)
			{
				isFocus = true;
			}
			if (GameCanvas.isPointerJustRelease)
			{
				return true;
			}
		}
		return false;
	}

	public bool isPointerPressInsideCamera(int cmx, int cmy)
	{
		isFocus = false;
		if (GameCanvas.isPointerHoldIn(x - cmx, y - cmy, w, h))
		{
			Res.outz("w= " + w);
			if (GameCanvas.isPointerDown)
			{
				isFocus = true;
			}
			if (GameCanvas.isPointerJustRelease)
			{
				return true;
			}
		}
		return false;
	}
}
