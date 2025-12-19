using System;
using UnityEngine;

public class RadarScr : mScreen
{
	public const sbyte SUBCMD_ALL = 0;

	public const sbyte SUBCMD_USE = 1;

	public const sbyte SUBCMD_LEVEL = 2;

	public const sbyte SUBCMD_AMOUNT = 3;

	public const sbyte SUBCMD_AURA = 4;

	public static RadarScr instance;

	public static bool TYPE_UI;

	public static FrameImage fraImgFocus;

	public static FrameImage fraImgFocusNone;

	public static FrameImage fraEff;

	private static Image imgUI;

	private static Image imgUIText;

	private static Image imgArrow_Left;

	private static Image imgArrow_Right;

	private static Image imgArrow_Down;

	private static Image imgLock;

	private static Image imgUse_0;

	private static Image imgUse;

	private static Image imgBack;

	private static Image imgChange;

	private static Image imgBar_0;

	private static Image imgBar_1;

	private static Image imgPro_0;

	private static Image imgPro_1;

	private static Image[] imgRank;

	public static int xUi;

	public static int yUi;

	public static int wUi;

	public static int hUi;

	public static int xMon;

	public static int yMon;

	public static int xText;

	public static int yText;

	public static int wText;

	public static int cmyText;

	public static int hText;

	public static int yCmd;

	public static int[] xCmd = new int[0];

	public static int[] dxCmd = new int[0];

	private static int[][] xyArrow;

	private static int[][] xyItem;

	private static int[] index = new int[5] { -2, -1, 0, 1, 2 };

	private int dyArrow;

	private int[] dxArrow;

	private int page;

	private int maxpage;

	private int indexFocus;

	public static MyVector list;

	public static MyVector listUse;

	private static int num;

	private static int numMax;

	private Info_RadaScr focus_card;

	private int pxx;

	private int pyy;

	private int xClip;

	private int wClip;

	private int yClip;

	private int hClip;

	public RadarScr()
	{
		TYPE_UI = true;
		Image img = mSystem.loadImage("/radar/17.png");
		Image img2 = mSystem.loadImage("/radar/3.png");
		Image img3 = mSystem.loadImage("/radar/23.png");
		fraImgFocus = new FrameImage(img, 28, 28);
		fraImgFocusNone = new FrameImage(img2, 30, 30);
		fraEff = new FrameImage(img3, 11, 11);
		imgUI = mSystem.loadImage("/radar/0.png");
		imgArrow_Left = mSystem.loadImage("/radar/1.png");
		imgArrow_Right = mSystem.loadImage("/radar/2.png");
		imgUIText = mSystem.loadImage("/radar/17.png");
		imgArrow_Down = mSystem.loadImage("/radar/4.png");
		imgLock = mSystem.loadImage("/radar/5.png");
		imgUse_0 = mSystem.loadImage("/radar/6.png");
		imgRank = new Image[7];
		for (int i = 0; i < 7; i++)
		{
			imgRank[i] = mSystem.loadImage("/radar/" + (i + 7) + ".png");
		}
		imgUse = mSystem.loadImage("/radar/14.png");
		imgBack = mSystem.loadImage("/radar/15.png");
		imgChange = mSystem.loadImage("/radar/16.png");
		imgUIText = mSystem.loadImage("/radar/18.png");
		imgBar_1 = mSystem.loadImage("/radar/19.png");
		imgPro_0 = mSystem.loadImage("/radar/20.png");
		imgPro_1 = mSystem.loadImage("/radar/21.png");
		imgBar_0 = mSystem.loadImage("/radar/22.png");
		wUi = 200;
		hUi = 219;
		xUi = GameCanvas.hw - (wUi + 40) / 2;
		yUi = GameCanvas.hh - hUi / 2;
		xText = xUi + wUi - 81;
		yText = yUi + 29;
		wText = 120;
		hText = 80;
		xyArrow = new int[3][]
		{
			new int[2]
			{
				xUi + 34,
				yUi + hUi - 42
			},
			new int[2]
			{
				xUi + wUi / 2 - imgArrow_Down.getWidth() / 2,
				yUi + hUi / 2 + 33
			},
			new int[2]
			{
				xUi + wUi - 41,
				yUi + hUi - 42
			}
		};
		xyItem = new int[5][]
		{
			new int[2]
			{
				xUi + 25,
				yUi + hUi - 82
			},
			new int[2]
			{
				xUi + 57,
				yUi + hUi - 62
			},
			new int[2]
			{
				xUi + wUi / 2 - 14,
				yUi + hUi - 102
			},
			new int[2]
			{
				xUi + wUi - 57 - 28,
				yUi + hUi - 62
			},
			new int[2]
			{
				xUi + wUi - 25 - 28,
				yUi + hUi - 82
			}
		};
		dxArrow = new int[2];
		dyArrow = 0;
		xMon = xUi + 73;
		yMon = yUi + hUi / 2 + 5;
		yCmd = yUi + hUi - 22;
		xCmd = new int[3]
		{
			xUi + wUi / 2 - 8 - 80,
			xUi + wUi / 2 - 8,
			xUi + wUi / 2 - 8 + 80
		};
		dxCmd = new int[3];
		yClip = yText + 10 + 70;
		hClip = 0;
		list = new MyVector();
		listUse = new MyVector();
		page = 1;
		maxpage = 2;
	}

	public static RadarScr gI()
	{
		if (instance == null)
		{
			instance = new RadarScr();
		}
		return instance;
	}

	public void SetRadarScr(MyVector list, int num, int numMax)
	{
		RadarScr.list = list;
		SetNum(num, numMax);
		page = 1;
		indexFocus = 2;
		listIndex();
		TYPE_UI = true;
		SetListUse();
		if (TYPE_UI)
		{
			maxpage = list.size() / 5 + ((list.size() % 5 > 0) ? 1 : 0);
		}
		else
		{
			maxpage = listUse.size() / 5 + ((listUse.size() % 5 > 0) ? 1 : 0);
		}
	}

	public static void SetNum(int num, int numMax)
	{
		RadarScr.num = num;
		RadarScr.numMax = numMax;
	}

	public static void SetListUse()
	{
		listUse = new MyVector(string.Empty);
		for (int i = 0; i < list.size(); i++)
		{
			Info_RadaScr info_RadaScr = (Info_RadaScr)list.elementAt(i);
			if (info_RadaScr != null && info_RadaScr.isUse == 1)
			{
				listUse.addElement(info_RadaScr);
			}
		}
	}

	public void listIndex()
	{
		MyVector myVector = listUse;
		if (TYPE_UI)
		{
			myVector = list;
		}
		int num = (page - 1) * 5;
		int num2 = num + 5;
		for (int i = num; i < num2; i++)
		{
			if (i >= myVector.size())
			{
				index[i - num] = -1;
				continue;
			}
			Info_RadaScr info_RadaScr = (Info_RadaScr)myVector.elementAt(i);
			if (info_RadaScr != null)
			{
				index[i - num] = info_RadaScr.id;
			}
		}
		cmyText = 0;
		hText = 0;
		SoundMn.gI().radarItem();
	}

	public override void update()
	{
		try
		{
			if (hText < 80)
			{
				hText += 4;
				if (hText > 80)
				{
					hText = 80;
				}
			}
			focus_card = Info_RadaScr.GetInfo(listUse, index[indexFocus]);
			if (TYPE_UI)
			{
				focus_card = Info_RadaScr.GetInfo(list, index[indexFocus]);
			}
			GameScr.gI().update();
			if (GameCanvas.gameTick % 10 < 6)
			{
				if (GameCanvas.gameTick % 2 == 0)
				{
					dyArrow--;
				}
			}
			else
			{
				dyArrow = 0;
			}
			if (focus_card != null)
			{
				int num = focus_card.amount * 100 / focus_card.max_amount;
				hClip = num * imgBar_1.getHeight() / 100;
				int num2 = RadarScr.num * 100 / list.size();
				wClip = num2 * imgPro_1.getWidth() / 100;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("-upd-radaScr-null: " + ex.ToString());
		}
	}

	public override void updateKey()
	{
		if (!InfoDlg.isLock)
		{
			if (GameCanvas.isTouch && !ChatTextField.gI().isShow && !GameCanvas.menu.showMenu)
			{
				updateKeyTouchControl();
			}
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
			if (GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23] = false;
				doKeyItem(1);
			}
			if (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24] = false;
				doKeyItem(0);
			}
			if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
				doClickUse(1);
			}
			if (GameCanvas.keyPressed[13])
			{
				doClickUse(2);
			}
			if (GameCanvas.keyPressed[12])
			{
				GameCanvas.keyPressed[12] = false;
				doClickUse(0);
			}
			GameCanvas.clearKeyPressed();
		}
	}

	private void doChangeUI()
	{
		TYPE_UI = !TYPE_UI;
		page = 1;
		indexFocus = 0;
		if (TYPE_UI)
		{
			maxpage = list.size() / 5 + ((list.size() % 5 > 0) ? 1 : 0);
		}
		else
		{
			maxpage = listUse.size() / 5 + ((listUse.size() % 5 > 0) ? 1 : 0);
		}
		listIndex();
		hText = 0;
	}

	private void updateKeyTouchControl()
	{
		if (GameCanvas.isPointerClick)
		{
			for (int i = 0; i < 5; i++)
			{
				if (GameCanvas.isPointerHoldIn(xyItem[i][0], xyItem[i][1], 30, 30) && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease && i != indexFocus)
				{
					doClickItem(i);
				}
			}
			if (GameCanvas.isPointerHoldIn(xyArrow[0][0] - 5, xyArrow[0][1] - 5, 20, 20))
			{
				if (GameCanvas.isPointerDown)
				{
					dxArrow[0] = 1;
				}
				if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					doClickArrow(0);
					dxArrow[0] = 0;
				}
			}
			if (GameCanvas.isPointerHoldIn(xyArrow[2][0] - 5, xyArrow[2][1] - 5, 20, 20))
			{
				if (GameCanvas.isPointerDown)
				{
					dxArrow[1] = 1;
				}
				if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					doClickArrow(1);
					dxArrow[1] = 0;
				}
			}
			for (int j = 0; j < xCmd.Length; j++)
			{
				if (GameCanvas.isPointerHoldIn(xCmd[j] - 5, yCmd - 5, 20, 20))
				{
					if (GameCanvas.isPointerDown)
					{
						dxCmd[j] = 1;
					}
					if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
					{
						doClickUse(j);
						dxCmd[j] = 0;
					}
				}
			}
		}
		else
		{
			dxCmd[0] = 0;
			dxCmd[1] = 0;
			dxCmd[2] = 0;
			dxArrow[0] = 0;
			dxArrow[1] = 0;
		}
		if (!GameCanvas.isPointerHoldIn(xText, 0, wText, yText + hText))
		{
			return;
		}
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
			if (cmyText > focus_card.cp.lim)
			{
				cmyText = focus_card.cp.lim;
			}
		}
		else
		{
			pyy = 0;
			pyy = 0;
		}
	}

	private void doClickUse(int i)
	{
		switch (i)
		{
		case 0:
			doChangeUI();
			break;
		case 1:
			if (focus_card != null)
			{
				Service.gI().SendRada(1, focus_card.id);
			}
			break;
		case 2:
			GameScr.gI().switchToMe();
			break;
		}
		SoundMn.gI().radarClick();
	}

	private void doClickArrow(int dir)
	{
		if (TYPE_UI)
		{
			maxpage = list.size() / 5 + ((list.size() % 5 > 0) ? 1 : 0);
		}
		else
		{
			maxpage = listUse.size() / 5 + ((listUse.size() % 5 > 0) ? 1 : 0);
		}
		int num = page;
		if (dir == 0)
		{
			if (page == 1)
			{
				return;
			}
			num--;
			if (num < 1)
			{
				num = 1;
			}
		}
		else
		{
			if (page == maxpage)
			{
				return;
			}
			num++;
			if (num > maxpage)
			{
				num = maxpage;
			}
		}
		if (num != page)
		{
			page = num;
			listIndex();
		}
	}

	private void doClickItem(int focus)
	{
		indexFocus = focus;
		listIndex();
	}

	private void doKeyText(int type)
	{
		cmyText += 12 * type;
		if (cmyText < 0)
		{
			cmyText = 0;
		}
		if (cmyText > focus_card.cp.lim)
		{
			cmyText = focus_card.cp.lim;
		}
	}

	private void doKeyItem(int type)
	{
		int num = indexFocus;
		int num2 = page;
		num = ((type != 0) ? (num - 1) : (num + 1));
		if (num >= index.Length)
		{
			if (page < maxpage)
			{
				num = 0;
				num2++;
			}
			else
			{
				num = index.Length - 1;
			}
		}
		if (num < 0)
		{
			if (page > 1)
			{
				num = index.Length - 1;
				num2--;
			}
			else
			{
				num = 0;
			}
		}
		if (num != indexFocus)
		{
			indexFocus = num;
			cmyText = 0;
			hText = 0;
		}
		if (num2 != page)
		{
			page = num2;
			listIndex();
		}
	}

	public override void paint(mGraphics g)
	{
		try
		{
			GameScr.gI().paint(g);
			g.translate(-GameScr.cmx, -GameScr.cmy);
			g.translate(0, GameCanvas.transY);
			GameScr.resetTranslate(g);
			g.drawImage(imgUI, xUi, yUi, 0);
			g.drawImage(imgPro_0, xUi + wUi / 2 - imgPro_0.getWidth() / 2, yUi - imgPro_0.getHeight() / 2 - 2, 0);
			g.setClip(xUi + wUi / 2 - imgPro_0.getWidth() / 2 + 13, yUi - imgPro_0.getHeight() / 2 + 3, wClip, imgPro_0.getHeight());
			g.drawImage(imgPro_1, xUi + wUi / 2 - imgPro_0.getWidth() / 2 + 13, yUi - imgPro_0.getHeight() / 2 + 3, 0);
			GameScr.resetTranslate(g);
			g.drawImage(imgChange, xCmd[0], yCmd + dxCmd[0], 0);
			g.drawImage(imgUse_0, xCmd[1], yCmd + dxCmd[1], 0);
			g.drawImage(imgBack, xCmd[2], yCmd + dxCmd[2], 0);
			if (TYPE_UI)
			{
				g.drawRegion(imgUse, 0, 0, 17, 17, 0, xCmd[1], yCmd + dxCmd[1], 0);
			}
			else
			{
				g.drawRegion(imgUse, 0, 0, 17, 17, 1, xCmd[1], yCmd + dxCmd[1], 0);
			}
			if (focus_card != null)
			{
				g.setClip(xUi + 30, yUi + 13, wUi - 60, hUi / 2);
				focus_card.paintInfo(g, xMon, yMon);
				GameScr.resetTranslate(g);
				mFont.tahoma_7b_yellow.drawString(g, ((focus_card.level <= 0) ? " " : ("Lv." + focus_card.level + " ")) + focus_card.name, xUi + wUi / 2, yUi + 15, 2);
				mFont.tahoma_7_white.drawString(g, "no." + focus_card.no, xUi + 30, yText - 2, 0);
				g.drawImage(imgBar_0, xUi + 36, yText + 10, 0);
				g.setClip(xUi + 36, yClip - hClip, 7, hClip);
				g.drawImage(imgBar_1, xUi + 36, yText + 10, 0);
				GameScr.resetTranslate(g);
				g.drawImage(imgRank[focus_card.rank], xUi + 39 - 5 + 14, yText + 12, 0);
			}
			g.setClip(xText, yText, wText + 5, hText + 8);
			if (focus_card != null)
			{
				g.drawImage(imgUIText, xText, yText, 0);
			}
			GameScr.resetTranslate(g);
			g.setClip(xText, yText + 1, wText, hText + 5);
			if (focus_card != null && focus_card.cp != null)
			{
				if (focus_card.cp.says == null)
				{
					return;
				}
				focus_card.cp.paintRada(g, cmyText);
			}
			GameScr.resetTranslate(g);
			if ((!TYPE_UI && listUse.size() > 5) || TYPE_UI)
			{
				if (page > 1)
				{
					g.drawImage(imgArrow_Left, xyArrow[0][0], xyArrow[0][1] + dxArrow[0], 0);
				}
				if (page < maxpage)
				{
					g.drawImage(imgArrow_Right, xyArrow[2][0], xyArrow[2][1] + dxArrow[1], 0);
				}
			}
			for (int i = 0; i < index.Length; i++)
			{
				int num = 0;
				int num2 = 0;
				int idx = 0;
				if (i == indexFocus)
				{
					num = dyArrow;
					num2 = -10;
					idx = 1;
					g.drawImage(imgArrow_Down, xyItem[i][0] + 10, xyItem[i][1] + dyArrow + 29 + num2, 0);
				}
				Info_RadaScr info = Info_RadaScr.GetInfo(listUse, index[i]);
				if (TYPE_UI)
				{
					info = Info_RadaScr.GetInfo(list, index[i]);
				}
				if (info != null)
				{
					fraImgFocus.drawFrame(info.rank, xyItem[i][0], xyItem[i][1] + num + num2, 0, 0, g);
					SmallImage.drawSmallImage(g, info.idIcon, xyItem[i][0] + 14, xyItem[i][1] + 14 + num + num2, 0, StaticObj.VCENTER_HCENTER);
					info.paintEff(g, xyItem[i][0], xyItem[i][1] + num + num2);
					if (info.level == 0)
					{
						g.drawImage(imgLock, xyItem[i][0], xyItem[i][1] + num + num2, 0);
					}
					if (i == indexFocus)
					{
						fraImgFocus.drawFrame(7, xyItem[i][0], xyItem[i][1] + num + num2, 0, 0, g);
					}
					if (info.isUse == 1)
					{
						fraImgFocus.drawFrame(8, xyItem[i][0], xyItem[i][1] + num + num2, 0, 0, g);
					}
				}
				else
				{
					fraImgFocusNone.drawFrame(idx, xyItem[i][0] - 1, xyItem[i][1] - 1 + num + num2, 0, 0, g);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("-pnt-radaScr-null: " + ex.ToString());
		}
	}

	public override void switchToMe()
	{
		GameScr.isPaintOther = true;
		base.switchToMe();
	}
}
