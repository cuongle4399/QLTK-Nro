using System;

public class NewBoss : Mob, IMapObject
{
	public static Image shadowBig = mSystem.loadImage("/mainImage/shadowBig.png");

	public int xTo;

	public int yTo;

	public bool haftBody;

	public bool change;

	public new int xSd;

	public new int ySd;

	private int wCount;

	public new bool isShadown = true;

	private int tick;

	private int frame;

	public new static Image imgHP = mSystem.loadImage("/mainImage/myTexture2dmobHP.png");

	private bool wy;

	private int wt;

	private int fy;

	private int ty;

	public new int typeSuperEff;

	private Char focus;

	private bool flyUp;

	private bool flyDown;

	private int dy;

	public bool changePos;

	private int tShock;

	public new bool isBusyAttackSomeOne = true;

	private int tA;

	private Char[] charAttack;

	private long[] dameHP;

	private sbyte type;

	private int ff;

	private int offset;

	private int xTempRight = -1;

	private int xTempLeft = -1;

	private int yTemp = -1;

	private sbyte[] cou = new sbyte[2] { -1, 1 };

	public new Char injureBy;

	public new bool injureThenDie;

	public new Mob mobToAttack;

	public new int forceWait;

	public new bool blindEff;

	public new bool sleepEff;

	private int[][] frameArr = new int[17][]
	{
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 }
	};

	public new const sbyte stand = 0;

	public const sbyte moveFra = 1;

	public new const sbyte attack1 = 2;

	public new const sbyte attack2 = 3;

	public const sbyte attack3 = 4;

	public const sbyte attack4 = 5;

	public const sbyte attack5 = 6;

	public const sbyte attack6 = 7;

	public const sbyte attack7 = 8;

	public const sbyte attack8 = 9;

	public const sbyte attack9 = 10;

	public const sbyte attack10 = 11;

	public new const sbyte hurt = 12;

	public const sbyte die = 13;

	public const sbyte fly = 14;

	public const sbyte adddame = 15;

	public const sbyte typeEff = 16;

	public NewBoss(int id, short px, short py, int templateID, long hp, long maxHp, int s)
	{
		mobId = id;
		x = (xFirst = px + 20);
		y = (yFirst = py);
		xTo = x;
		yTo = y;
		base.maxHp = maxHp;
		base.hp = hp;
		templateId = templateID;
		h_hp_bar = 6;
		w_hp_bar = 100;
		len = w_hp_bar;
		updateHp_bar();
		if (Mob.arrMobTemplate[templateId].data == null)
		{
			Service.gI().requestModTemplate(templateId);
		}
		status = 2;
		frameArr = null;
	}

	public override void setBody(short id)
	{
		changBody = true;
		smallBody = id;
	}

	public override void clearBody()
	{
		changBody = false;
	}

	public new static bool isExistNewMob(string id)
	{
		for (int i = 0; i < Mob.newMob.size(); i++)
		{
			string text = (string)Mob.newMob.elementAt(i);
			if (text.Equals(id))
			{
				return true;
			}
		}
		return false;
	}

	public new void checkFrameTick(int[] array)
	{
		tick++;
		if (tick > array.Length - 1)
		{
			tick = 0;
		}
		frame = array[tick];
	}

	public void updateShadown()
	{
		int num = 0;
		xSd = x;
		if (TileMap.tileTypeAt(x, y, 2))
		{
			ySd = y;
			return;
		}
		ySd = y;
		while (num < 30)
		{
			num++;
			ySd += 24;
			if (TileMap.tileTypeAt(xSd, ySd, 2))
			{
				if (ySd % 24 != 0)
				{
					ySd -= ySd % 24;
				}
				break;
			}
		}
	}

	private void paintShadow(mGraphics g)
	{
		int size = TileMap.size;
		if ((TileMap.mapID < 114 || TileMap.mapID > 120) && TileMap.mapID != 127 && TileMap.mapID != 128)
		{
			if (TileMap.tileTypeAt(xSd + size / 2, ySd + 1, 4))
			{
				g.setClip(xSd / size * size, (ySd - 30) / size * size, size, 100);
			}
			else if (TileMap.tileTypeAt((xSd - size / 2) / size, (ySd + 1) / size) == 0)
			{
				g.setClip(xSd / size * size, (ySd - 30) / size * size, 100, 100);
			}
			else if (TileMap.tileTypeAt((xSd + size / 2) / size, (ySd + 1) / size) == 0)
			{
				g.setClip(xSd / size * size, (ySd - 30) / size * size, size, 100);
			}
			else if (TileMap.tileTypeAt(xSd - size / 2, ySd + 1, 8))
			{
				g.setClip(xSd / 24 * size, (ySd - 30) / size * size, size, 100);
			}
		}
		g.drawImage(shadowBig, xSd, ySd - 5, 3);
		g.setClip(GameScr.cmx, GameScr.cmy - GameCanvas.transY, GameScr.gW, GameScr.gH + 2 * GameCanvas.transY);
	}

	public new void updateSuperEff()
	{
	}

	public override void update()
	{
		if (frameArr == null && Mob.arrMobTemplate[templateId].data != null)
		{
			GetFrame();
		}
		if (frameArr == null || !isUpdate())
		{
			return;
		}
		updateShadown();
		switch (status)
		{
		case 2:
			updateMobStandWait();
			break;
		case 3:
			updateMobAttack();
			break;
		case 5:
			timeStatus = 0;
			updateMobWalk();
			break;
		case 6:
			timeStatus = 0;
			p1++;
			y += p1;
			if (y >= yFirst)
			{
				y = yFirst;
				p1 = 0;
				status = 5;
			}
			break;
		case 7:
			updateInjure();
			base.update();
			break;
		case 0:
		case 1:
			updateDead();
			break;
		case 4:
			updateMobFly();
			break;
		}
	}

	private void updateDead()
	{
		tick++;
		if (tick > frameArr[13].Length - 1)
		{
			tick = frameArr[13].Length - 1;
		}
		frame = frameArr[13][tick];
		if (x != xTo || y != yTo)
		{
			x += (xTo - x) / 4;
			y += (yTo - y) / 4;
		}
	}

	private void updateMobFly()
	{
	}

	public new void setAttack(Char cFocus)
	{
		isBusyAttackSomeOne = true;
		mobToAttack = null;
		base.cFocus = cFocus;
		p1 = 0;
		p2 = 0;
		status = 3;
		tick = 0;
		int cx = cFocus.cx;
		int cy = cFocus.cy;
		if (Res.abs(cx - x) < w * 2 && Res.abs(cy - y) < h * 2)
		{
			if (x < cx)
			{
				x = cx - w;
			}
			else
			{
				x = cx + w;
			}
			p3 = 0;
		}
		else
		{
			p3 = 1;
		}
	}

	private void updateInjure()
	{
	}

	private void updateMobStandWait()
	{
		checkFrameTick(frameArr[0]);
		if (x != xTo || y != yTo)
		{
			x += (xTo - x) / 4;
			y += (yTo - y) / 4;
		}
	}

	public void setFly()
	{
		status = 4;
		flyUp = true;
	}

	public void setAttack(Char[] cAttack, long[] dame, sbyte type, sbyte dir)
	{
		charAttack = cAttack;
		dameHP = dame;
		this.type = type;
		base.dir = dir;
		status = 3;
		if (x != xTo || y != yTo)
		{
			x += (xTo - x) / 4;
			y += (yTo - y) / 4;
		}
	}

	public new void updateMobAttack()
	{
		if (tick == frameArr[type + 1].Length - 1)
		{
			status = 2;
		}
		checkFrameTick(frameArr[type + 1]);
		if (tick == frameArr[15][type - 1])
		{
			for (int i = 0; i < charAttack.Length; i++)
			{
				charAttack[i].doInjure(dameHP[i], 0L, isCrit: false, isMob: false);
				ServerEffect.addServerEffect(frameArr[16][type - 1], charAttack[i].cx, charAttack[i].cy, 1);
			}
		}
	}

	public new void updateMobWalk()
	{
		checkFrameTick(frameArr[1]);
		sbyte speed = Mob.arrMobTemplate[templateId].speed;
		int num = speed;
		if (Res.abs(x - xTo) < speed)
		{
			num = Res.abs(x - xTo);
		}
		x += ((x >= xTo) ? (-num) : num);
		y = yTo;
		if (x < xTo)
		{
			dir = 1;
		}
		else if (x > xTo)
		{
			dir = -1;
		}
		if (Res.abs(x - xTo) <= 1)
		{
			x = xTo;
			status = 2;
		}
	}

	public new bool isPaint()
	{
		if (x < GameScr.cmx)
		{
			return false;
		}
		if (x > GameScr.cmx + GameScr.gW)
		{
			return false;
		}
		if (y < GameScr.cmy)
		{
			return false;
		}
		if (y > GameScr.cmy + GameScr.gH + 30)
		{
			return false;
		}
		if (status == 0)
		{
			return false;
		}
		return true;
	}

	public new bool isUpdate()
	{
		if (status == 0)
		{
			return false;
		}
		return true;
	}

	public override void paint(mGraphics g)
	{
		if (Mob.arrMobTemplate[templateId].data == null || isHide)
		{
			return;
		}
		if (isMafuba)
		{
			if (!changBody)
			{
				Mob.arrMobTemplate[templateId].data.paintFrame(g, frame, xMFB, yMFB, (dir != 1) ? 1 : 0, 2);
			}
			else
			{
				SmallImage.drawSmallImage(g, smallBody, xMFB, yMFB, (dir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			return;
		}
		if (isShadown)
		{
			paintShadow(g);
		}
		g.translate(0, GameCanvas.transY);
		if (!changBody)
		{
			int num = 33;
			if (yTemp == -1)
			{
				yTemp = y;
			}
			if (TileMap.tileTypeAt(x + num, y + fy, 4))
			{
				xTempLeft = TileMap.tileXofPixel(x + num) - num;
				xTempRight = TileMap.tileXofPixel(x + num);
				if (x > xTempLeft && x < xTempRight && xTempRight != -1)
				{
					x = xTempLeft;
				}
			}
			if (y < yTemp && yTemp != -1)
			{
				yTemp = y;
				x += num;
			}
			if (y > yTemp)
			{
				yTemp = y;
				x -= num;
			}
			Mob.arrMobTemplate[templateId].data.paintFrame(g, frame, x, y + fy, (dir != 1) ? 1 : 0, 2);
		}
		else
		{
			SmallImage.drawSmallImage(g, smallBody, x, y + fy - 9, (dir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER);
		}
		g.translate(0, -GameCanvas.transY);
		if (hp <= 0)
		{
			return;
		}
		int imageWidth = mGraphics.getImageWidth(imgHPtem);
		int imageHeight = mGraphics.getImageHeight(imgHPtem);
		int num2 = imageWidth;
		int num3 = imageWidth;
		int num4 = x - imageWidth;
		int num5 = y - h - 5;
		int num6 = imageWidth * 2 * per / 100;
		int num7 = num6;
		if (per_tem >= per)
		{
			num7 = imageWidth * (per_tem -= ((GameCanvas.gameTick % 6 <= 3) ? offset : offset++)) / 100;
			if (per_tem <= 0)
			{
				per_tem = 0;
			}
			if (per_tem < per)
			{
				per_tem = per;
			}
			if (offset >= 3)
			{
				offset = 3;
			}
		}
		if (num6 > num2)
		{
			num3 = num6 - num2;
			if (num3 <= 0)
			{
				num3 = 0;
			}
		}
		else
		{
			num2 = num6;
			num3 = 0;
		}
		g.drawImage(GameScr.imgHP_tm_xam, num4, num5, mGraphics.TOP | mGraphics.LEFT);
		g.drawImage(GameScr.imgHP_tm_xam, num4 + imageWidth, num5, mGraphics.TOP | mGraphics.LEFT);
		g.setColor(16777215);
		g.fillRect(num4, num5, num7, 2);
		g.drawRegion(imgHPtem, 0, 0, num2, imageHeight, 0, num4, num5, mGraphics.TOP | mGraphics.LEFT);
		g.drawRegion(imgHPtem, 0, 0, num3, imageHeight, 0, num4 + imageWidth, num5, mGraphics.TOP | mGraphics.LEFT);
	}

	public new int getHPColor()
	{
		return 16711680;
	}

	public new void startDie()
	{
		hp = 0L;
		injureThenDie = true;
		hp = 0L;
		status = 1;
		p1 = -3;
		p2 = -dir;
		p3 = 0;
	}

	public new void attackOtherMob(Mob mobToAttack)
	{
		this.mobToAttack = mobToAttack;
		isBusyAttackSomeOne = true;
		cFocus = null;
		p1 = 0;
		p2 = 0;
		status = 3;
		tick = 0;
		int num = mobToAttack.x;
		int num2 = mobToAttack.y;
		if (Res.abs(num - x) < w * 2 && Res.abs(num2 - y) < h * 2)
		{
			if (x < num)
			{
				x = num - w;
			}
			else
			{
				x = num + w;
			}
			p3 = 0;
		}
		else
		{
			p3 = 1;
		}
	}

	public new int getX()
	{
		return x;
	}

	public new int getY()
	{
		return y;
	}

	public new int getH()
	{
		return h;
	}

	public new int getW()
	{
		return w;
	}

	public new void stopMoving()
	{
		if (status == 5)
		{
			status = 2;
			p1 = (p2 = (p3 = 0));
			forceWait = 50;
		}
	}

	public new bool isInvisible()
	{
		return status == 0 || status == 1;
	}

	public new void removeHoldEff()
	{
		if (holdEffID != 0)
		{
			holdEffID = 0;
		}
	}

	public new void removeBlindEff()
	{
		blindEff = false;
	}

	public new void removeSleepEff()
	{
		sleepEff = false;
	}

	public new void move(short xMoveTo, short yMoveTo)
	{
		if (yMoveTo != -1)
		{
			if (Res.distance(x, y, xTo, yTo) > 100)
			{
				x = xMoveTo;
				y = yMoveTo;
				status = 2;
			}
			else
			{
				xTo = xMoveTo;
				yTo = yMoveTo;
				status = 5;
			}
		}
		else
		{
			xTo = xMoveTo;
			status = 5;
		}
	}

	public new void GetFrame()
	{
		try
		{
			frameArr = (int[][])Controller.frameHT_NEWBOSS.get(templateId + string.Empty);
			w = Mob.arrMobTemplate[templateId].data.width;
			h = Mob.arrMobTemplate[templateId].data.height;
		}
		catch (Exception)
		{
		}
	}

	public void setDie()
	{
		status = 0;
	}
}
