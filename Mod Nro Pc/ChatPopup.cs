using System;
using System.Collections.Generic;
using Mod_nro.MenuDataGame;

public class ChatPopup : Effect2, IActionListener
{
	public int sayWidth = 100;

	public int delay;

	public int sayRun;

	public string[] says;

	public int cx;

	public int cy;

	public int ch;

	public int cmx;

	public int cmy;

	public int lim;

	public Npc c;

	private bool outSide;

	public static long curr;

	public static long last;

	private int currentLine;

	private string[] lines;

	public Command cmdNextLine;

	public Command cmdMsg1;

	public Command cmdMsg2;

	public static ChatPopup currChatPopup;

	public static ChatPopup serverChatPopUp;

	public static string nextMultiChatPopUp;

	public static Npc nextChar;

	public bool isShopDetail;

	public sbyte starSlot;

	public sbyte maxStarSlot;

	public static Scroll scr;

	public static bool isHavePetNpc;

	public int mH;

	public static int performDelay;

	public int dx;

	public int dy;

	public int second;

	private Point[] saoPoint = new Point[14];

	private int indexStar;

	private int indexStar2;

	public bool[] starCuongHoa = new bool[20];

	public static int numSlot;

	private int nMaxslot_duoi;

	private int nMaxslot_tren;

	private int nslot_duoi;

	private Image imgStar;

	public int strY;

	private int iconID;

	public bool isClip;

	public static int cmyText;

	private int pxx;

	private int pyy;

	public static void addNextPopUpMultiLine(string strNext, Npc next)
	{
		nextMultiChatPopUp = strNext;
		nextChar = next;
		if (currChatPopup == null)
		{
			addChatPopupMultiLineGame(nextMultiChatPopUp, 100000, nextChar);
			nextMultiChatPopUp = null;
			nextChar = null;
		}
	}

	public static void addBigMessage(string chat, int howLong, Npc c)
	{
	}

	public static void addChatPopupMultiLine(string chat, int howLong, Npc c)
	{
		GameScr.info1.addInfo(chat);
	}

	public static ChatPopup addChatPopupWithIcon(string chat, int howLong, Npc c, int idIcon)
	{
		performDelay = 10;
		ChatPopup chatPopup = new ChatPopup();
		chatPopup.sayWidth = GameCanvas.w - 30 - (GameCanvas.menu.showMenu ? GameCanvas.menu.menuX : 0);
		if (chatPopup.sayWidth > 320)
		{
			chatPopup.sayWidth = 320;
		}
		if (chat.Length < 10)
		{
			chatPopup.sayWidth = 64;
		}
		if (GameCanvas.w == 128)
		{
			chatPopup.sayWidth = 128;
		}
		chatPopup.says = mFont.tahoma_7_red.splitFontArray(chat, chatPopup.sayWidth - 10);
		chatPopup.delay = howLong;
		chatPopup.c = c;
		chatPopup.iconID = idIcon;
		Char.chatPopup = chatPopup;
		chatPopup.ch = 15 - chatPopup.sayRun + chatPopup.says.Length * 12 + 10;
		if (chatPopup.ch > GameCanvas.h - 80)
		{
			chatPopup.ch = GameCanvas.h - 80;
		}
		chatPopup.mH = 10;
		if (GameCanvas.menu.showMenu)
		{
			chatPopup.mH = 0;
		}
		Effect2.vEffect2.addElement(chatPopup);
		isHavePetNpc = false;
		if (c != null && c.charID == 5)
		{
			isHavePetNpc = true;
			GameScr.info1.addInfo(string.Empty, 1);
		}
		curr = (last = mSystem.currentTimeMillis());
		chatPopup.ch += 15;
		return chatPopup;
	}

	public static ChatPopup addChatPopup(string chat, int howLong, Npc c)
	{
		performDelay = 10;
		ChatPopup chatPopup = new ChatPopup();
		chatPopup.sayWidth = GameCanvas.w - 30 - (GameCanvas.menu.showMenu ? GameCanvas.menu.menuX : 0);
		if (chatPopup.sayWidth > 320)
		{
			chatPopup.sayWidth = 320;
		}
		if (chat.Length < 10)
		{
			chatPopup.sayWidth = 64;
		}
		if (GameCanvas.w == 128)
		{
			chatPopup.sayWidth = 128;
		}
		chatPopup.says = mFont.tahoma_7_red.splitFontArray(chat, chatPopup.sayWidth - 10);
		chatPopup.delay = howLong;
		chatPopup.c = c;
		Char.chatPopup = chatPopup;
		chatPopup.ch = 15 - chatPopup.sayRun + chatPopup.says.Length * 12 + 10;
		if (chatPopup.ch > GameCanvas.h - 80)
		{
			chatPopup.ch = GameCanvas.h - 80;
		}
		chatPopup.mH = 10;
		if (GameCanvas.menu.showMenu)
		{
			chatPopup.mH = 0;
		}
		Effect2.vEffect2.addElement(chatPopup);
		isHavePetNpc = false;
		if (c != null && c.charID == 5)
		{
			isHavePetNpc = true;
			GameScr.info1.addInfo(string.Empty, 1);
		}
		curr = (last = mSystem.currentTimeMillis());
		return chatPopup;
	}

	public override void update()
	{
		if (scr != null)
		{
			GameScr.info1.isUpdate = false;
			scr.updatecm();
		}
		else
		{
			GameScr.info1.isUpdate = true;
		}
		if (GameCanvas.menu.showMenu)
		{
			strY = 0;
			cx = GameCanvas.w / 2 - sayWidth / 2 - 1;
			cy = GameCanvas.menu.menuY - ch;
		}
		else
		{
			strY = 0;
			if (GameScr.gI().right != null || GameScr.gI().left != null || GameScr.gI().center != null || cmdNextLine != null || cmdMsg1 != null)
			{
				strY = 5;
				cx = GameCanvas.w / 2 - sayWidth / 2 - 1;
				cy = GameCanvas.h - 20 - ch;
			}
			else
			{
				cx = GameCanvas.w / 2 - sayWidth / 2 - 1;
				cy = GameCanvas.h - 5 - ch;
			}
		}
		if (delay > 0)
		{
			delay--;
		}
		if (performDelay > 0)
		{
			performDelay--;
		}
		else
		{
			GameScr.info1.info.time = 0;
			for (int i = 0; i < GameScr.info1.info.infoWaitToShow.size(); i++)
			{
				if (((InfoItem)GameScr.info1.info.infoWaitToShow.elementAt(i)).speed != 70)
				{
					((InfoItem)GameScr.info1.info.infoWaitToShow.elementAt(i)).speed = 10;
				}
			}
		}
		if (sayRun > 1)
		{
			sayRun--;
		}
		if ((c != null && Char.chatPopup != null && Char.chatPopup != this) || (c != null && Char.chatPopup == null) || delay <= 0)
		{
			Effect2.vEffect2Outside.removeElement(this);
			Effect2.vEffect2.removeElement(this);
		}
	}

	public override void paint(mGraphics g)
	{
		if (GameScr.gI().activeRongThan && GameScr.gI().isUseFreez)
		{
			return;
		}
		GameCanvas.resetTrans(g);
		int num = cx;
		int num2 = cy;
		int num3 = sayWidth + 2;
		int num4 = ch;
		if ((num <= 0 || num2 <= 0) && !GameCanvas.panel.isShow && !DataItem.IsShow)
		{
			return;
		}
		if (c != null)
		{
			int num5 = ((GameCanvas.gameTick % 10 <= 2) ? 1 : 0);
			SmallImage.drawSmallImage(g, c.avatar, cx + 14, cy + num5, 0, StaticObj.BOTTOM_LEFT);
		}
		if (iconID != 0)
		{
			int num6 = ((GameCanvas.gameTick % 10 <= 2) ? 1 : 0);
			SmallImage.drawSmallImage(g, iconID, cx + num3 / 2, cy + ch - 15 + num6, 0, StaticObj.VCENTER_HCENTER);
		}
		PopUp.paintPopUp(g, num, num2, num3, num4, 16777215, isButton: false);
		if (scr != null)
		{
			g.setClip(num, num2, num3, num4 - 16);
			g.translate(0, -scr.cmy);
		}
		int tx = 0;
		int ty = 0;
		if (isClip)
		{
			tx = g.getTranslateX();
			ty = g.getTranslateY();
			g.setClip(num, num2 + 1, num3, num4 - 17);
			g.translate(0, -cmyText);
		}
		int num7 = -1;
		for (int i = 0; i < says.Length; i++)
		{
			if (says[i].StartsWith("--"))
			{
				g.setColor(0);
				g.fillRect(num + 10, cy + sayRun + i * 12 + 6, num3 - 20, 1);
				continue;
			}
			mFont mFont2 = mFont.tahoma_7;
			int num8 = 2;
			string st = says[i];
			int num9;
			if (says[i].StartsWith("|"))
			{
				string[] array = Res.split(says[i], "|", 0);
				if (array.Length == 3)
				{
					st = array[2];
				}
				if (array.Length == 4)
				{
					st = array[3];
					num8 = int.Parse(array[2]);
				}
				num9 = int.Parse(array[1]);
				num7 = num9;
			}
			else
			{
				num9 = num7;
			}
			switch (num9)
			{
			case -1:
				mFont2 = mFont.tahoma_7;
				break;
			case 0:
				mFont2 = mFont.tahoma_7b_dark;
				break;
			case 1:
				mFont2 = mFont.tahoma_7b_green;
				break;
			case 2:
				mFont2 = mFont.tahoma_7b_blue;
				break;
			case 3:
				mFont2 = mFont.tahoma_7_red;
				break;
			case 4:
				mFont2 = mFont.tahoma_7_green;
				break;
			case 5:
				mFont2 = mFont.tahoma_7_blue;
				break;
			case 7:
				mFont2 = mFont.tahoma_7b_red;
				break;
			case 8:
				mFont2 = mFont.tahoma_7b_yellow;
				break;
			}
			if (says[i].StartsWith("<"))
			{
				string[] array2 = Res.split(Res.split(says[i], "<", 0)[1], ">", 1);
				if (second == 0)
				{
					second = int.Parse(array2[1]);
				}
				else
				{
					curr = mSystem.currentTimeMillis();
					if (curr - last >= 1000)
					{
						last = curr;
						second--;
					}
				}
				st = second + " " + array2[2];
				mFont2.drawString(g, st, cx + sayWidth / 2, cy + sayRun + i * 12 - strY + 12, num8);
			}
			else
			{
				if (num8 == 2)
				{
					mFont2.drawString(g, st, cx + sayWidth / 2, cy + sayRun + i * 12 - strY + 12, num8);
				}
				if (num8 == 1)
				{
					mFont2.drawString(g, st, cx + sayWidth - 5, cy + sayRun + i * 12 - strY + 12, num8);
				}
			}
		}
		if (isClip)
		{
			GameCanvas.resetTrans(g);
			g.translate(tx, ty);
		}
		if (maxStarSlot > 4)
		{
			nMaxslot_tren = (maxStarSlot + 1) / 2;
			nMaxslot_duoi = maxStarSlot - nMaxslot_tren;
			int[] array3 = new int[maxStarSlot];
			int[] array4 = new int[maxStarSlot];
			for (int j = 0; j < nMaxslot_tren; j++)
			{
				g.drawImage(Panel.imgMaxStar, num + num3 / 2 - nMaxslot_tren * 20 / 2 + j * 20 + mGraphics.getImageWidth(Panel.imgMaxStar), num2 + num4 - 17, 3);
				array3[j] = num + num3 / 2 - nMaxslot_tren * 20 / 2 + j * 20 + mGraphics.getImageWidth(Panel.imgMaxStar);
				array4[j] = num2 + num4 - 17;
			}
			for (int k = 0; k < nMaxslot_duoi; k++)
			{
				g.drawImage(Panel.imgMaxStar, num + num3 / 2 - nMaxslot_duoi * 20 / 2 + k * 20 + mGraphics.getImageWidth(Panel.imgMaxStar), num2 + num4 - 8, 3);
				array3[nMaxslot_tren + k] = num + num3 / 2 - nMaxslot_duoi * 20 / 2 + k * 20 + mGraphics.getImageWidth(Panel.imgMaxStar);
				array4[nMaxslot_tren + k] = num2 + num4 - 8;
			}
			if (maxStarSlot >= 7)
			{
				for (int l = 7; l < maxStarSlot; l++)
				{
					if (starCuongHoa[l])
					{
						g.drawImage(Panel.imgStarCuongHoa, array3[l], array4[l], 3);
					}
				}
			}
			if (starSlot > 0)
			{
				imgStar = Panel.imgStar;
				if (starSlot >= nMaxslot_tren)
				{
					nslot_duoi = starSlot - nMaxslot_tren;
					for (int m = 0; m < nMaxslot_tren; m++)
					{
						g.drawImage(imgStar, num + num3 / 2 - nMaxslot_tren * 20 / 2 + m * 20 + mGraphics.getImageWidth(imgStar), num2 + num4 - 17, 3);
					}
					for (int n = 0; n < nslot_duoi; n++)
					{
						if (n + nMaxslot_tren >= numSlot)
						{
							imgStar = Panel.imgStar8;
						}
						g.drawImage(imgStar, num + num3 / 2 - nMaxslot_duoi * 20 / 2 + n * 20 + mGraphics.getImageWidth(imgStar), num2 + num4 - 8, 3);
					}
				}
				else
				{
					for (int num10 = 0; num10 < starSlot; num10++)
					{
						g.drawImage(imgStar, num + num3 / 2 - nMaxslot_tren * 20 / 2 + num10 * 20 + mGraphics.getImageWidth(imgStar), num2 + num4 - 17, 3);
					}
				}
			}
		}
		else
		{
			for (int num11 = 0; num11 < maxStarSlot; num11++)
			{
				g.drawImage(Panel.imgMaxStar, num + num3 / 2 - maxStarSlot * 20 / 2 + num11 * 20 + mGraphics.getImageWidth(Panel.imgMaxStar), num2 + num4 - 13, 3);
			}
			if (starSlot > 0)
			{
				for (int num12 = 0; num12 < starSlot; num12++)
				{
					g.drawImage(Panel.imgStar, num + num3 / 2 - maxStarSlot * 20 / 2 + num12 * 20 + mGraphics.getImageWidth(Panel.imgStar), num2 + num4 - 13, 3);
				}
			}
		}
		paintCmd(g);
	}

	public void paintRada(mGraphics g, int cmyText)
	{
		int num = cx;
		int num2 = cy;
		int num3 = sayWidth;
		_ = ch;
		int translateX = g.getTranslateX();
		int translateY = g.getTranslateY();
		g.translate(0, -cmyText);
		if ((num <= 0 || num2 <= 0) && !GameCanvas.panel.isShow && !DataItem.IsShow)
		{
			return;
		}
		int num4 = -1;
		for (int i = 0; i < says.Length; i++)
		{
			if (says[i].StartsWith("--"))
			{
				g.setColor(16777215);
				g.fillRect(num + 10, cy + sayRun + i * 12 - 6, num3 - 20, 1);
				continue;
			}
			mFont mFont2 = mFont.tahoma_7_white;
			int num5 = 2;
			string st = says[i];
			int num6;
			if (says[i].StartsWith("|"))
			{
				string[] array = Res.split(says[i], "|", 0);
				if (array.Length == 3)
				{
					st = array[2];
				}
				if (array.Length == 4)
				{
					st = array[3];
					num5 = int.Parse(array[2]);
				}
				num6 = int.Parse(array[1]);
				num4 = num6;
			}
			else
			{
				num6 = num4;
			}
			switch (num6)
			{
			case -1:
				mFont2 = mFont.tahoma_7_white;
				break;
			case 0:
				mFont2 = mFont.tahoma_7b_white;
				break;
			case 1:
				mFont2 = mFont.tahoma_7b_green;
				break;
			case 2:
				mFont2 = mFont.tahoma_7b_red;
				break;
			}
			if (says[i].StartsWith("<"))
			{
				string[] array2 = Res.split(Res.split(says[i], "<", 0)[1], ">", 1);
				if (second == 0)
				{
					second = int.Parse(array2[1]);
				}
				else
				{
					curr = mSystem.currentTimeMillis();
					if (curr - last >= 1000)
					{
						last = curr;
						second--;
					}
				}
				st = second + " " + array2[2];
				mFont2.drawString(g, st, cx + sayWidth / 2, cy + sayRun + i * 12 - strY, num5);
			}
			else
			{
				if (num5 == 2)
				{
					mFont2.drawString(g, st, cx + sayWidth / 2, cy + sayRun + i * 12 - strY, num5);
				}
				if (num5 == 1)
				{
					mFont2.drawString(g, st, cx + sayWidth - 5, cy + sayRun + i * 12 - strY, num5);
				}
			}
		}
		GameCanvas.resetTrans(g);
		g.translate(translateX, translateY);
	}

	private void doKeyText(int type)
	{
		cmyText += 12 * type;
		if (cmyText < 0)
		{
			cmyText = 0;
		}
		if (cmyText > lim)
		{
			cmyText = lim;
		}
	}

	public void updateKey()
	{
		if (isClip)
		{
			if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] = false;
				doKeyText(1);
			}
			if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] = false;
				doKeyText(-1);
			}
			if (GameCanvas.isPointerHoldIn(cx, 0, sayWidth + 2, ch))
			{
				if (GameCanvas.isPointerMove)
				{
					if (pyy == 0)
					{
						pyy = GameCanvas.py;
					}
					pxx = pyy - GameCanvas.py;
					if (pxx != 0)
					{
						cmyText += pxx;
						pyy = GameCanvas.py;
					}
					if (cmyText < 0)
					{
						cmyText = 0;
					}
					if (cmyText > lim)
					{
						cmyText = lim;
					}
				}
				else
				{
					pyy = 0;
					pyy = 0;
				}
			}
		}
		if (scr != null)
		{
			if (GameCanvas.isTouch)
			{
				scr.updateKey();
			}
			if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21])
			{
				scr.cmtoY -= 12;
				if (scr.cmtoY < 0)
				{
					scr.cmtoY = 0;
				}
			}
			if (GameCanvas.keyHold[(!Main.isPC) ? 8 : 22])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] = false;
				scr.cmtoY += 12;
				if (scr.cmtoY > scr.cmyLim)
				{
					scr.cmtoY = scr.cmyLim;
				}
			}
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(GameCanvas.currentScreen.center))
		{
			GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
			mScreen.keyTouch = -1;
			if (cmdNextLine != null)
			{
				cmdNextLine.performAction();
			}
			else if (cmdMsg1 != null)
			{
				cmdMsg1.performAction();
			}
			else if (cmdMsg2 != null)
			{
				cmdMsg2.performAction();
			}
		}
		if (scr == null || !scr.pointerIsDowning)
		{
			if (cmdMsg1 != null && (GameCanvas.keyPressed[12] || GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(cmdMsg1)))
			{
				GameCanvas.keyPressed[12] = false;
				GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
				GameCanvas.isPointerClick = false;
				GameCanvas.isPointerJustRelease = false;
				cmdMsg1.performAction();
				mScreen.keyTouch = -1;
			}
			if (cmdMsg2 != null && (GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(cmdMsg2)))
			{
				GameCanvas.keyPressed[13] = false;
				GameCanvas.isPointerClick = false;
				GameCanvas.isPointerJustRelease = false;
				cmdMsg2.performAction();
				mScreen.keyTouch = -1;
			}
		}
	}

	public void paintCmd(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		GameCanvas.paintz.paintTabSoft(g);
		if (cmdNextLine != null)
		{
			GameCanvas.paintz.paintCmdBar(g, null, cmdNextLine, null);
		}
		if (cmdMsg1 != null)
		{
			GameCanvas.paintz.paintCmdBar(g, cmdMsg1, null, cmdMsg2);
		}
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1000)
		{
			try
			{
				GameMidlet.instance.platformRequest((string)p);
			}
			catch (Exception)
			{
			}
			if (!Main.isPC)
			{
				GameMidlet.instance.notifyDestroyed();
			}
			else
			{
				idAction = 1001;
			}
			GameCanvas.endDlg();
		}
		if (idAction == 1001)
		{
			scr = null;
			Char.chatPopup = null;
			serverChatPopUp = null;
			GameScr.info1.isUpdate = true;
			Char.isLockKey = false;
			if (isHavePetNpc)
			{
				GameScr.info1.info.time = 0;
				GameScr.info1.info.info.speed = 10;
			}
		}
		if (idAction != 8000 || performDelay > 0)
		{
			return;
		}
		int num = currChatPopup.currentLine;
		num++;
		if (num >= currChatPopup.lines.Length)
		{
			Char.chatPopup = null;
			currChatPopup = null;
			GameScr.info1.isUpdate = true;
			Char.isLockKey = false;
			if (nextMultiChatPopUp != null)
			{
				addChatPopupMultiLineGame(nextMultiChatPopUp, 100000, nextChar);
				nextMultiChatPopUp = null;
				nextChar = null;
			}
			else
			{
				if (!isHavePetNpc)
				{
					return;
				}
				GameScr.info1.info.time = 0;
				for (int i = 0; i < GameScr.info1.info.infoWaitToShow.size(); i++)
				{
					if (((InfoItem)GameScr.info1.info.infoWaitToShow.elementAt(i)).speed == 10000000)
					{
						((InfoItem)GameScr.info1.info.infoWaitToShow.elementAt(i)).speed = 10;
					}
				}
			}
		}
		else
		{
			ChatPopup chatPopup = addChatPopup(currChatPopup.lines[num], currChatPopup.delay, currChatPopup.c);
			chatPopup.currentLine = num;
			chatPopup.lines = currChatPopup.lines;
			chatPopup.cmdNextLine = currChatPopup.cmdNextLine;
			currChatPopup = chatPopup;
		}
	}

	static ChatPopup()
	{
		numSlot = 7;
	}

	public static void addChatPopupMultiLineGame(string chat, int howLong, Npc c)
	{
		string[] array = Res.split(chat, "\n", 0);
		Char.isLockKey = true;
		currChatPopup = addChatPopup(chat, howLong, c);
		currChatPopup.currentLine = 0;
		currChatPopup.lines = array;
		string caption = mResources.CONTINUE;
		if (array.Length == 1)
		{
			caption = mResources.CLOSE;
		}
		currChatPopup.cmdNextLine = new Command(caption, currChatPopup, 8000, null);
		currChatPopup.cmdNextLine.x = GameCanvas.w / 2 - 35;
		currChatPopup.cmdNextLine.y = GameCanvas.h - 35;
		SoundMn.gI().openDialog();
	}

	public static void addBigMessageGame(string chat, int howLong, Npc c)
	{
		string[] array = new string[1] { chat };
		if (c.charID != 5 && GameScr.info1.isDone)
		{
			GameScr.info1.isUpdate = false;
		}
		Char.isLockKey = true;
		serverChatPopUp = addChatPopup(array[0], howLong, c);
		serverChatPopUp.strY = 5;
		serverChatPopUp.cx = GameCanvas.w / 2 - serverChatPopUp.sayWidth / 2 - 1;
		serverChatPopUp.cy = GameCanvas.h - 20 - serverChatPopUp.ch;
		serverChatPopUp.currentLine = 0;
		serverChatPopUp.lines = array;
		scr = new Scroll();
		int nItem = serverChatPopUp.says.Length;
		scr.setStyle(nItem, 12, serverChatPopUp.cx, serverChatPopUp.cy - serverChatPopUp.strY + 12, serverChatPopUp.sayWidth + 2, serverChatPopUp.ch - 25, styleUPDOWN: true, 1);
		SoundMn.gI().openDialog();
	}

	public static void addChatPopupMultiLineGameline(string chat, int howLong = 0, Npc c = null, int line = 10)
	{
		string[] array = chat.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		List<string> list = new List<string>();
		string text = "";
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			text = text + array[i] + "\n";
			num++;
			if (num == line || i == array.Length - 1)
			{
				list.Add(text.TrimEnd('\n'));
				text = "";
				num = 0;
			}
		}
		Char.isLockKey = true;
		currChatPopup = addChatPopup(list[0], howLong, c);
		currChatPopup.currentLine = 0;
		currChatPopup.lines = list.ToArray();
		string caption = mResources.CONTINUE;
		if (list.Count == 1)
		{
			caption = mResources.CLOSE;
		}
		currChatPopup.cmdNextLine = new Command(caption, currChatPopup, 8000, null);
		currChatPopup.cmdNextLine.x = GameCanvas.w / 2 - 35;
		currChatPopup.cmdNextLine.y = GameCanvas.h - 35;
		SoundMn.gI().openDialog();
	}
}
