using System;
using System.Threading;

public class CrackBallScr : mScreen
{
	public static CrackBallScr instance;

	private BallInfo[] listBall;

	private byte step;

	private byte typePrice;

	private int rO;

	private int xO;

	private int yO;

	private int angle;

	private int iAngle;

	private int iDot;

	private int yTo;

	private int indexSelect;

	private int indexSkillSelect;

	private int numTicket;

	private int xP;

	private int yP;

	private int wP;

	private int hP;

	private int price;

	private int cost;

	private int countFr;

	private int countKame;

	private int frame;

	private int vp;

	private int[] xArg;

	private int[] yArg;

	private int[] xDot;

	private int[] yDot;

	private short[] idItem;

	private long timeStart;

	private long timeKame;

	private bool isKame;

	private bool isCanSkill;

	private bool isSendSv;

	private short idTicket;

	private static int ySkill;

	private static int[] xSkill;

	private static FrameImage fraImgKame;

	private static FrameImage fraImgKame_1;

	private static FrameImage fraImgKame_2;

	private static Image imgX;

	private static Image imgReplay;

	public static bool isAutoCrackBall;

	public static bool isCallStop;

	public static bool isContinue;

	private byte[] fr = new byte[21]
	{
		19, 19, 19, 19, 19, 19, 19, 19, 19, 19,
		19, 19, 19, 19, 19, 19, 19, 19, 19, 19,
		20
	};

	private byte[] nFrame = new byte[12]
	{
		0, 0, 0, 1, 1, 1, 2, 2, 2, 3,
		3, 3
	};

	public CrackBallScr()
	{
		xSkill = new int[3];
		xSkill[0] = 16;
		ySkill = GameCanvas.h - 41;
		xSkill[1] = GameCanvas.w - 40;
		xSkill[2] = (xSkill[0] + xSkill[1]) / 2;
		Image img = GameCanvas.loadImage("/e/e_1.png");
		fraImgKame = new FrameImage(img, 30, 30);
		Image img2 = GameCanvas.loadImage("/e/e_0.png");
		fraImgKame_1 = new FrameImage(img2, 68, 65);
		Image img3 = GameCanvas.loadImage("/e/e_2.png");
		fraImgKame_2 = new FrameImage(img3, 66, 70);
		imgReplay = GameCanvas.loadImage("/e/nut2.png");
		imgX = GameCanvas.loadImage("/e/nut3.png");
		wP = 230;
		xP = GameCanvas.hw - wP / 2;
		hP = 40;
		yP = -hP;
	}

	public static CrackBallScr gI()
	{
		if (instance == null)
		{
			instance = new CrackBallScr();
		}
		return instance;
	}

	public void SetCrackBallScr(short[] idImage, byte typePrice, int price, short idTicket)
	{
		if (idImage != null && idImage.Length != 0)
		{
			yTo = Char.myCharz().cy - 10;
			setAuraItem();
			listBall = new BallInfo[idImage.Length];
			for (int i = 0; i < listBall.Length; i++)
			{
				listBall[i] = new BallInfo();
				listBall[i].idImg = idImage[i];
				listBall[i].count = i * 25;
				listBall[i].yTo = -999;
				listBall[i].vx = Res.random(2, 5);
				listBall[i].dir = Res.random(-1, 2);
				listBall[i].SetChar();
			}
			isCanSkill = false;
			isKame = false;
			isSendSv = false;
			timeStart = GameCanvas.timeNow + Res.random(1000, 2000);
			step = 0;
			indexSelect = -1;
			indexSkillSelect = -1;
			this.typePrice = typePrice;
			this.price = price;
			cost = 0;
			Char.myCharz().moveTo(470, 408, 1);
			Char.myCharz().cdir = -1;
			Char.myCharz().statusMe = 1;
			countFr = 0;
			countKame = 0;
			frame = 0;
			vp = 0;
			yP = -hP;
			this.idTicket = idTicket;
			numTicket = 0;
			checkNumTicket();
			switchToMe();
			SoundMn.gI().hoisinh();
		}
	}

	private void setAuraItem()
	{
		rO = GameCanvas.hh / 3 + 10;
		if (rO > 50)
		{
			rO = 50;
		}
		xO = 360;
		GameScr.cmx = GameScr.cmxLim / 2;
		yO = GameScr.cmy + GameCanvas.hh / 3 + 30;
		iDot = 175;
		angle = 0;
		iAngle = 360 / iDot;
		xArg = new int[iDot];
		yArg = new int[iDot];
		xDot = new int[iDot];
		yDot = new int[iDot];
		setDotPosition();
	}

	private void setDotPosition()
	{
		if (GameCanvas.lowGraphic)
		{
			return;
		}
		for (int i = 0; i < yArg.Length; i++)
		{
			yArg[i] = Res.abs(rO * Res.sin(angle) / 1024);
			xArg[i] = Res.abs(rO * Res.cos(angle) / 1024);
			if (angle < 90)
			{
				xDot[i] = xO + xArg[i];
				yDot[i] = yO - yArg[i];
			}
			else if (angle >= 90 && angle < 180)
			{
				xDot[i] = xO - xArg[i];
				yDot[i] = yO - yArg[i];
			}
			else if (angle >= 180 && angle < 270)
			{
				xDot[i] = xO - xArg[i];
				yDot[i] = yO + yArg[i];
			}
			else
			{
				xDot[i] = xO + xArg[i];
				yDot[i] = yO + yArg[i];
			}
			angle += iAngle;
		}
	}

	public void perform(int idAction, object p)
	{
	}

	public override void update()
	{
		try
		{
			cost = price * checkNum();
			checkNumTicket();
			GameScr.gI().update();
			if (timeStart - GameCanvas.timeNow > 0)
			{
				for (int i = 0; i < listBall.Length; i++)
				{
					listBall[i].count += 2;
					if (listBall[i].count >= iDot)
					{
						listBall[i].count = 0;
					}
					listBall[i].x = xDot[listBall[i].count];
					listBall[i].y = yDot[listBall[i].count];
				}
				return;
			}
			if (step == 0)
			{
				step = 1;
			}
			if (step == 1)
			{
				for (int j = 0; j < listBall.Length; j++)
				{
					if (listBall[j].yTo == -999 || listBall[j].isDone)
					{
						continue;
					}
					if (listBall[j].y < listBall[j].yTo)
					{
						if (listBall[j].vy < 0)
						{
							listBall[j].vy = 0;
						}
						if (listBall[j].y + listBall[j].vy > listBall[j].yTo)
						{
							listBall[j].y = listBall[j].yTo;
						}
						else
						{
							listBall[j].y += listBall[j].vy;
						}
						listBall[j].vy++;
					}
					else
					{
						if (listBall[j].vy > 0)
						{
							listBall[j].vy = 0;
						}
						listBall[j].y += listBall[j].vy;
						listBall[j].vy--;
					}
					if (listBall[j].y == listBall[j].yTo)
					{
						Effect me = new Effect(19, listBall[j].x - 5, listBall[j].y + 25, 2, 1, -1);
						EffecMn.addEff(me);
						SoundMn.gI().charFall();
						listBall[j].isDone = true;
						if (!isCanSkill)
						{
							isCanSkill = true;
						}
					}
				}
			}
			if (step == 2)
			{
				for (int k = 0; k < listBall.Length; k++)
				{
					if (listBall[k].isDone)
					{
						continue;
					}
					if (listBall[k].y > -10)
					{
						if (listBall[k].vy > 0)
						{
							listBall[k].vy = 0;
						}
						listBall[k].y += listBall[k].vy;
						listBall[k].vy--;
						listBall[k].x += listBall[k].vx * listBall[k].dir;
						listBall[k].vx -= 3;
					}
					if (listBall[k].y == -10)
					{
						listBall[k].isPaint = false;
					}
				}
				countFr++;
				if (countFr > fr.Length - 1)
				{
					countFr = fr.Length - 1;
					isKame = true;
					SoundMn.gI().newKame();
					if (!isSendSv && timeKame - GameCanvas.timeNow < 0)
					{
						Service.gI().SendCrackBall(2, (byte)(checkTicket() + checkNum()));
						isSendSv = true;
					}
				}
				Char.myCharz().cf = fr[countFr];
				countKame++;
				if (countKame > 5)
				{
					countKame = 0;
				}
				frame = nFrame[countKame];
			}
			if (step == 3)
			{
				if (countKame <= 5)
				{
					countKame = 5;
				}
				countKame++;
				if (countKame > nFrame.Length - 1)
				{
					countKame = nFrame.Length - 1;
					step = 4;
					isKame = false;
					int num = 0;
					for (int l = 0; l < listBall.Length; l++)
					{
						if (listBall[l].isDone && !listBall[l].isSetImg)
						{
							listBall[l].idImg = idItem[num];
							listBall[l].isSetImg = true;
							num++;
						}
					}
				}
				frame = nFrame[countKame];
			}
			if (step == 4)
			{
				for (int m = 0; m < listBall.Length; m++)
				{
					if (listBall[m].isPaint)
					{
						listBall[m].xTo = Char.myCharz().cx;
					}
				}
				step = 5;
			}
			if (step != 5)
			{
				return;
			}
			vp++;
			if (yP < GameCanvas.hh / 3)
			{
				if (yP + vp > GameCanvas.hh / 3)
				{
					yP = GameCanvas.hh / 3;
				}
				else
				{
					yP += vp;
				}
			}
			for (int n = 0; n < listBall.Length; n++)
			{
				if (!listBall[n].isPaint)
				{
					continue;
				}
				if (listBall[n].x < listBall[n].xTo)
				{
					if (listBall[n].vx < 0)
					{
						listBall[n].vx = 0;
					}
					if (listBall[n].x + listBall[n].vx > listBall[n].xTo)
					{
						listBall[n].x = listBall[n].xTo;
					}
					else
					{
						listBall[n].x += listBall[n].vx;
					}
					listBall[n].vx++;
				}
				else
				{
					if (listBall[n].vx > 0)
					{
						listBall[n].vx = 0;
					}
					listBall[n].x += listBall[n].vx;
					listBall[n].vx--;
				}
				if (listBall[n].x == listBall[n].xTo)
				{
					listBall[n].isPaint = false;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public override void updateKey()
	{
		if (InfoDlg.isLock)
		{
			return;
		}
		if (GameCanvas.isTouch && !ChatTextField.gI().isShow && !GameCanvas.menu.showMenu)
		{
			updateKeyTouchControl();
		}
		if (isAutoCrackBall && !GameCanvas.keyPressed[0])
		{
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
		}
		if (GameCanvas.keyPressed[0])
		{
			doClickSkill(2);
		}
		for (int i = 1; i < 8; i++)
		{
			if (GameCanvas.keyPressed[i])
			{
				GameCanvas.keyPressed[i] = false;
				doClickBall(i - 1);
			}
		}
		if (GameCanvas.keyPressed[12])
		{
			GameCanvas.keyPressed[12] = false;
			doClickSkill(0);
		}
		if (GameCanvas.keyPressed[13])
		{
			GameCanvas.keyPressed[13] = false;
			doClickSkill(1);
		}
		GameCanvas.clearKeyPressed();
	}

	private void updateKeyTouchControl()
	{
		if (step == 1 && GameCanvas.isPointerClick)
		{
			for (int i = 0; i < listBall.Length; i++)
			{
				if (GameCanvas.isPointerHoldIn(listBall[i].x - 20 - GameScr.cmx, listBall[i].y - 10 - GameScr.cmy, 30, 30) && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					doClickBall(i);
				}
			}
		}
		if (!GameCanvas.isPointerClick)
		{
			return;
		}
		for (int j = 0; j < xSkill.Length; j++)
		{
			if (GameCanvas.isPointerHoldIn(xSkill[j], ySkill, 36, 36) && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
			{
				if (isAutoCrackBall && j != 2)
				{
					break;
				}
				doClickSkill(j);
			}
		}
	}

	private void doClickBall(int index)
	{
		if (!listBall[index].isDone)
		{
			SoundMn.gI().getItem();
			long num = ((typePrice != 0) ? Char.myCharz().checkLuong() : Char.myCharz().xu);
			if (checkTicket() >= numTicket && num < cost + price)
			{
				string s = mResources.not_enough_money_1 + " " + ((typePrice != 0) ? mResources.LUONG : mResources.XU);
				GameScr.info1.addInfo(s);
			}
			else
			{
				indexSelect = index;
				listBall[indexSelect].yTo = yTo + Res.random(-3, 3);
			}
		}
	}

	private void doClickSkill(int index)
	{
		indexSkillSelect = index;
		if (indexSkillSelect == 2)
		{
			isAutoCrackBall = !isAutoCrackBall;
			if (isAutoCrackBall)
			{
				startAutoCrackBall();
			}
			else
			{
				isCallStop = true;
			}
		}
		else if (index == 0)
		{
			if (step < 2)
			{
				if (checkTicket() + checkNum() > 0)
				{
					step = 2;
					SoundMn.gI().gong();
					Char.myCharz().setSkillPaint(GameScr.sks[13], 0);
					timeKame = GameCanvas.timeNow + Res.random(2000, 3000);
				}
			}
			else if (yP == GameCanvas.hh / 3)
			{
				Service.gI().SendCrackBall(typePrice, 0);
			}
		}
		else
		{
			if (isAutoCrackBall)
			{
				stopAutoCrackBall();
			}
			GameScr.gI().isRongThanXuatHien = false;
			GameScr.gI().switchToMe();
		}
	}

	public override void paint(mGraphics g)
	{
		try
		{
			GameScr.gI().paint(g);
			g.translate(-GameScr.cmx, -GameScr.cmy);
			g.translate(0, GameCanvas.transY);
			for (int i = 0; i < listBall.Length; i++)
			{
				if (listBall[i].isPaint && listBall[i].y > listBall[i].yTo - 20)
				{
					g.drawImage(TileMap.bong, listBall[i].x, listBall[i].yTo + 7, mGraphics.VCENTER | mGraphics.HCENTER);
				}
			}
			for (int j = 0; j < listBall.Length; j++)
			{
				if (listBall[j].isPaint)
				{
					SmallImage.drawSmallImage(g, listBall[j].idImg, listBall[j].x, listBall[j].y, 0, mGraphics.VCENTER | mGraphics.HCENTER);
				}
			}
			if (isKame)
			{
				if (fraImgKame != null)
				{
					int num = Char.myCharz().cx - fraImgKame.frameWidth - 28;
					for (int k = 0; k < GameCanvas.w / fraImgKame.frameWidth + 1; k++)
					{
						fraImgKame.drawFrame(frame, num - k * (fraImgKame.frameWidth - 1), Char.myCharz().cy - fraImgKame.frameHeight / 2 - 12 + 2, 0, 0, g);
					}
				}
				if (fraImgKame_1 != null)
				{
					int num2 = Char.myCharz().cx - fraImgKame_1.frameWidth - 10;
					fraImgKame_1.drawFrame(frame, num2 - 5, Char.myCharz().cy - fraImgKame_1.frameHeight / 2 - 12, 0, 0, g);
				}
			}
			GameScr.resetTranslate(g);
			int num3 = 240;
			int num4 = GameCanvas.w - num3;
			int num5 = 15;
			g.setColor(13524492);
			g.fillRect(num4, num5 - 15, num3, 15);
			g.drawImage(Panel.imgXu, num4 + 11, num5 - 7, 3);
			g.drawImage(Panel.imgLuong, num4 + 90, num5 - 8, 3);
			mFont.tahoma_7_yellow.drawString(g, Char.myCharz().xuStr + string.Empty, num4 + 24, num5 - 13, mFont.LEFT, mFont.tahoma_7_grey);
			mFont.tahoma_7_yellow.drawString(g, Char.myCharz().luongStr + string.Empty, num4 + 100, num5 - 13, mFont.LEFT, mFont.tahoma_7_grey);
			g.drawImage(Panel.imgLuongKhoa, num4 + 150, num5 - 7, 3);
			mFont.tahoma_7_yellow.drawString(g, Char.myCharz().luongKhoaStr + string.Empty, num4 + 160, num5 - 13, mFont.LEFT, mFont.tahoma_7_grey);
			g.drawImage(Panel.imgTicket, num4 + 200, num5 - 7, 3);
			mFont.tahoma_7_yellow.drawString(g, numTicket + string.Empty, num4 + 210, num5 - 13, mFont.LEFT, mFont.tahoma_7_grey);
			if (step < 4)
			{
				int num6 = num3 / 2 + 20;
				int num7 = GameCanvas.w - num6;
				g.setColor(11837316);
				g.fillRect(num7, num5, num6, 15);
				if (typePrice == 0)
				{
					g.drawImage(Panel.imgXu, num7 + 21, num5 + 8, 3);
				}
				else
				{
					g.drawImage(Panel.imgLuongKhoa, num7 + 21, num5 + 7, 3);
					g.drawImage(Panel.imgLuong, num7 + 18, num5 + 7, 3);
				}
				mFont.tahoma_7_red.drawString(g, " -" + cost, num7 + 30, num5 + 2, mFont.LEFT, mFont.tahoma_7_grey);
				g.drawImage(Panel.imgTicket, num7 + 80, num5 + 7, 3);
				mFont.tahoma_7_red.drawString(g, " -" + checkTicket(), num7 + 90, num5 + 2, mFont.LEFT, mFont.tahoma_7_grey);
			}
			g.drawImage(GameScr.imgSkill, xSkill[0], ySkill, 0);
			if (indexSkillSelect == 0)
			{
				g.drawImage(GameScr.imgSkill2, xSkill[0], ySkill, 0);
			}
			if (step < 3)
			{
				SmallImage.drawSmallImage(g, 540, xSkill[0] + 14, ySkill + 14, 0, StaticObj.VCENTER_HCENTER);
			}
			else
			{
				g.drawImage(imgReplay, xSkill[0] + 14 - 10, ySkill + 14 - 10, 0);
			}
			g.drawImage(GameScr.imgSkill, xSkill[1], ySkill, 0);
			if (indexSkillSelect == 1)
			{
				g.drawImage(GameScr.imgSkill2, xSkill[1], ySkill, 0);
			}
			g.drawImage(imgX, xSkill[1] + 14 - 10, ySkill + 14 - 10, 0);
			if (step > 3)
			{
				GameCanvas.paintz.paintFrameSimple(xP, yP, wP, hP, g);
				int num8 = GameCanvas.hw - idItem.Length * 30 / 2;
				for (int l = 0; l < idItem.Length; l++)
				{
					SmallImage.drawSmallImage(g, idItem[l], num8 + 5 + l * 30, yP + 10, 0, 0);
				}
			}
			if (isAutoCrackBall)
			{
				g.drawImage(GameScr.imgSkill2, (xSkill[0] + xSkill[1]) / 2, ySkill, 0);
			}
			else
			{
				g.drawImage(GameScr.imgSkill, (xSkill[0] + xSkill[1]) / 2, ySkill, 0);
			}
			SmallImage.drawSmallImage(g, 4387, (xSkill[0] + xSkill[1]) / 2 + 14, ySkill + 14, 0, StaticObj.VCENTER_HCENTER);
		}
		catch (Exception)
		{
		}
	}

	public void DoneCrackBallScr(short[] idImage)
	{
		step = 3;
		idItem = idImage;
	}

	public override void switchToMe()
	{
		GameScr.isPaintOther = true;
		GameScr.gI().isRongThanXuatHien = true;
		base.switchToMe();
	}

	private byte checkTicket()
	{
		byte b = 0;
		for (int i = 0; i < listBall.Length; i++)
		{
			if (listBall[i].isDone)
			{
				b++;
			}
		}
		if (b > numTicket)
		{
			b = (byte)numTicket;
		}
		return b;
	}

	private byte checkNum()
	{
		byte b = 0;
		for (int i = 0; i < listBall.Length; i++)
		{
			if (listBall[i].isDone)
			{
				b++;
			}
		}
		b -= checkTicket();
		if (b <= 0)
		{
			b = 0;
		}
		return b;
	}

	private void checkNumTicket()
	{
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			if (Char.myCharz().arrItemBag[i] != null && Char.myCharz().arrItemBag[i].template.id == idTicket)
			{
				numTicket = Char.myCharz().arrItemBag[i].quantity;
				break;
			}
		}
	}

	private void useSkillCrackBall()
	{
		if (step < 2)
		{
			if (checkTicket() + checkNum() > 0)
			{
				step = 2;
				SoundMn.gI().gong();
				Char.myCharz().setSkillPaint(GameScr.sks[13], 0);
				timeKame = GameCanvas.timeNow + Res.random(2000, 3000);
			}
		}
		else if (yP == GameCanvas.hh / 3)
		{
			Service.gI().SendCrackBall(typePrice, 0);
		}
	}

	public void startAutoCrackBall()
	{
		Thread thread = new Thread(AutoCrackBall);
		thread.Start();
	}

	public void stopAutoCrackBall()
	{
		isAutoCrackBall = false;
		indexSkillSelect = -1;
		isCallStop = false;
		isContinue = false;
	}

	public void AutoCrackBall()
	{
		int num = 0;
		bool flag = false;
		try
		{
			while (isAutoCrackBall && GameCanvas.currentScreen == instance)
			{
				indexSkillSelect = 2;
				while (num < 7 && step != 5)
				{
					doClickBall(num);
					num++;
					Thread.Sleep(300);
				}
				if (num == 7)
				{
					Thread.Sleep(800);
					gI().useSkillCrackBall();
					Thread.Sleep(4000);
					if (isCallStop)
					{
						stopAutoCrackBall();
						Thread.ResetAbort();
						break;
					}
					if (step == 5)
					{
						gI().useSkillCrackBall();
						num = 0;
					}
					Thread.Sleep(1000);
				}
				if (step == 5 && num == 0 && !flag)
				{
					flag = true;
					gI().useSkillCrackBall();
				}
			}
		}
		catch (Exception)
		{
		}
	}
}
