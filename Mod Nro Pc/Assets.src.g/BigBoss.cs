using System;

namespace Assets.src.g;

public class BigBoss : Mob, IMapObject
{
	public static Image shadowBig = GameCanvas.loadImage("/mainImage/shadowBig.png");

	public static EffectData data;

	public int xTo;

	public int yTo;

	public bool haftBody;

	public bool change;

	public new int xSd;

	public new int ySd;

	private bool isOutMap;

	private int wCount;

	public new bool isShadown = true;

	private int tick;

	private int frame;

	private int fy;

	public new int typeSuperEff;

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

	public new int[] stand = new int[12]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1
	};

	public int[] stand_1 = new int[17]
	{
		37, 37, 37, 38, 38, 38, 39, 39, 40, 40,
		40, 39, 39, 39, 38, 38, 38
	};

	public new int[] move = new int[15]
	{
		1, 1, 1, 1, 2, 2, 2, 2, 3, 3,
		3, 3, 2, 2, 2
	};

	public new int[] moveFast = new int[7] { 1, 1, 2, 2, 3, 3, 2 };

	public new int[] attack1 = new int[12]
	{
		0, 0, 34, 34, 35, 35, 36, 36, 2, 2,
		1, 1
	};

	public new int[] attack2 = new int[23]
	{
		0, 0, 0, 4, 4, 6, 6, 9, 9, 10,
		10, 13, 13, 15, 15, 17, 17, 19, 19, 21,
		21, 23, 23
	};

	public int[] attack3 = new int[24]
	{
		0, 0, 1, 1, 4, 4, 6, 6, 8, 8,
		25, 25, 26, 26, 28, 28, 30, 30, 32, 32,
		2, 2, 1, 1
	};

	public int[] attack2_1 = new int[20]
	{
		37, 37, 5, 5, 7, 7, 11, 11, 14, 14,
		16, 16, 18, 18, 20, 20, 22, 22, 24, 24
	};

	public int[] attack3_1 = new int[21]
	{
		37, 37, 37, 38, 38, 5, 5, 7, 7, 11,
		11, 27, 27, 29, 29, 31, 31, 33, 33, 38,
		38
	};

	public int[] fly = new int[8] { 8, 8, 9, 9, 10, 10, 12, 12 };

	public int[] hitground = new int[24]
	{
		0, 0, 1, 1, 4, 4, 6, 6, 8, 8,
		25, 25, 26, 26, 28, 28, 30, 30, 32, 32,
		2, 2, 1, 1
	};

	private bool shock;

	private sbyte[] cou = new sbyte[2] { -1, 1 };

	public new Char injureBy;

	public new bool injureThenDie;

	public new Mob mobToAttack;

	public new int forceWait;

	public new bool blindEff;

	public new bool sleepEff;

	public BigBoss(int id, short px, short py, int templateID, long hp, long maxhp, int s)
	{
		xFirst = (x = px + 20);
		yFirst = (y = py);
		mobId = id;
		base.hp = hp;
		maxHp = maxhp;
		templateId = templateID;
		w_hp_bar = 100;
		h_hp_bar = 6;
		len = w_hp_bar;
		updateHp_bar();
		if (s == 0)
		{
			getDataB();
		}
		if (s == 1)
		{
			getDataB2();
		}
		if (s == 2)
		{
			getDataB2();
			haftBody = true;
		}
		status = 2;
	}

	public void getDataB2()
	{
		data = null;
		data = new EffectData();
		string patch = "/x" + mGraphics.zoomLevel + "/effectdata/" + 100 + "/data";
		try
		{
			data.readData2(patch);
			data.img = GameCanvas.loadImage("/effectdata/" + 100 + "/img.png");
		}
		catch (Exception)
		{
			Service.gI().requestModTemplate(templateId);
		}
		status = 2;
		w = data.width;
		h = data.height;
	}

	public void getDataB()
	{
		data = null;
		data = new EffectData();
		string patch = "/x" + mGraphics.zoomLevel + "/effectdata/" + 101 + "/data";
		try
		{
			data.readData2(patch);
			data.img = GameCanvas.loadImage("/effectdata/" + 101 + "/img.png");
			Res.outz("read xong data");
		}
		catch (Exception)
		{
			Service.gI().requestModTemplate(templateId);
		}
		w = data.width;
		h = data.height;
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

	private void updateShadown()
	{
		int size = TileMap.size;
		xSd = x;
		wCount = 0;
		if (ySd <= 0 || TileMap.tileTypeAt(xSd, ySd, 2))
		{
			return;
		}
		if (TileMap.tileTypeAt(xSd / size, ySd / size) == 0)
		{
			isOutMap = true;
		}
		else if (TileMap.tileTypeAt(xSd / size, ySd / size) != 0 && !TileMap.tileTypeAt(xSd, ySd, 2))
		{
			xSd = x;
			ySd = y;
			isOutMap = false;
		}
		while (isOutMap && wCount < 10)
		{
			wCount++;
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
		g.drawImage(shadowBig, xSd, yFirst, 3);
		g.setClip(GameScr.cmx, GameScr.cmy - GameCanvas.transY, GameScr.gW, GameScr.gH + 2 * GameCanvas.transY);
	}

	public new void updateSuperEff()
	{
	}

	public override void update()
	{
		if (!isUpdate())
		{
			return;
		}
		updateShadown();
		switch (status)
		{
		case 2:
			updateMobStandWait();
			break;
		case 4:
			timeStatus = 0;
			updateMobFly();
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
			break;
		case 0:
		case 1:
			updateDead();
			break;
		}
	}

	private void updateDead()
	{
		checkFrameTick((!haftBody) ? stand : stand_1);
		if (GameCanvas.gameTick % 5 == 0)
		{
			ServerEffect.addServerEffect(167, Res.random(x - getW() / 2, x + getW() / 2), Res.random(getY() + getH() / 2, getY() + getH()), 1);
		}
		if (x != xFirst || y != yFirst)
		{
			x += (xFirst - x) / 4;
			y += (yFirst - y) / 4;
		}
	}

	private void updateMobFly()
	{
		if (flyUp)
		{
			dy++;
			y -= dy;
			checkFrameTick(fly);
			if (y <= -500)
			{
				flyUp = false;
				flyDown = true;
				dy = 0;
			}
		}
		if (flyDown)
		{
			x = xTo;
			dy += 2;
			y += dy;
			checkFrameTick(hitground);
			if (y > yFirst)
			{
				y = yFirst;
				flyDown = false;
				dy = 0;
				status = 2;
				GameScr.shock_scr = 10;
				shock = true;
			}
		}
	}

	public new void setInjure()
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
		dir = ((cFocus.cx > x) ? 1 : (-1));
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

	private bool isSpecial()
	{
		if ((templateId >= 58 && templateId <= 65) || templateId == 67 || templateId == 68)
		{
			return true;
		}
		return false;
	}

	private void updateInjure()
	{
	}

	private void updateMobStandWait()
	{
		checkFrameTick((!haftBody) ? stand : stand_1);
		if (x != xFirst || y != yFirst)
		{
			x += (xFirst - x) / 4;
			y += (yFirst - y) / 4;
		}
	}

	public void setFly()
	{
		status = 4;
		flyUp = true;
	}

	public void setAttack(Char[] cAttack, long[] dame, sbyte type)
	{
		charAttack = cAttack;
		dameHP = dame;
		this.type = type;
		tick = 0;
		if (type < 3)
		{
			status = 3;
		}
		if (type == 3)
		{
			flyUp = true;
			status = 4;
		}
		if (type == 4)
		{
			for (int i = 0; i < charAttack.Length; i++)
			{
				charAttack[i].doInjure(dameHP[i], 0L, isCrit: false, isMob: false);
			}
		}
		if (type == 7)
		{
			status = 3;
		}
	}

	public new void updateMobAttack()
	{
		if (type == 7)
		{
			if (tick > 8)
			{
				tick = 8;
			}
			checkFrameTick(attack1);
			if (GameCanvas.gameTick % 4 == 0)
			{
				ServerEffect.addServerEffect(70, x + ((dir != 1) ? (-15) : 15), y - 40, 1);
			}
		}
		if (type == 0)
		{
			if (tick == attack1.Length - 1)
			{
				status = 2;
			}
			dir = ((x < charAttack[0].cx) ? 1 : (-1));
			checkFrameTick(attack1);
			if (tick == 8)
			{
				for (int i = 0; i < charAttack.Length; i++)
				{
					MonsterDart.addMonsterDart(x + ((dir != 1) ? (-45) : 45), y - 30, isBoss: true, dameHP[i], 0L, charAttack[i], 24);
				}
			}
		}
		if (type == 1)
		{
			if (tick == ((!haftBody) ? (attack2.Length - 1) : (attack2_1.Length - 1)))
			{
				status = 2;
			}
			dir = ((x < charAttack[0].cx) ? 1 : (-1));
			checkFrameTick((!haftBody) ? attack2 : attack2_1);
			x += (charAttack[0].cx - x) / 4;
			y += (charAttack[0].cy - y) / 4;
			if (tick == 18)
			{
				for (int j = 0; j < charAttack.Length; j++)
				{
					charAttack[j].doInjure(dameHP[j], 0L, isCrit: false, isMob: false);
					ServerEffect.addServerEffect(102, charAttack[j].cx, charAttack[j].cy, 1);
				}
			}
		}
		if (type == 8)
		{
		}
		if (type != 2)
		{
			return;
		}
		if (tick == ((!haftBody) ? (attack3.Length - 1) : (attack3_1.Length - 1)))
		{
			status = 2;
		}
		dir = ((x < charAttack[0].cx) ? 1 : (-1));
		checkFrameTick((!haftBody) ? attack3 : attack3_1);
		if (tick == 13)
		{
			GameScr.shock_scr = 10;
			shock = true;
			for (int k = 0; k < charAttack.Length; k++)
			{
				charAttack[k].doInjure(dameHP[k], 0L, isCrit: false, isMob: false);
			}
		}
	}

	public new void updateMobWalk()
	{
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

	public new bool checkIsBoss()
	{
		if (isBoss || levelBoss > 0)
		{
			return true;
		}
		return false;
	}

	public override void paint(mGraphics g)
	{
		if (data == null || isHide)
		{
			return;
		}
		if (isMafuba)
		{
			if (!changBody)
			{
				data.paintFrame(g, frame, xMFB, yMFB, (dir != 1) ? 1 : 0, 2);
			}
			else
			{
				SmallImage.drawSmallImage(g, smallBody, xMFB, yMFB, (dir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			return;
		}
		if (isShadown && status != 0)
		{
			paintShadow(g);
		}
		g.translate(0, GameCanvas.transY);
		if (!changBody)
		{
			data.paintFrame(g, frame, x, y + fy, (dir != 1) ? 1 : 0, 2);
		}
		else
		{
			SmallImage.drawSmallImage(g, smallBody, x, y + fy - 9, (dir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER);
		}
		g.translate(0, -GameCanvas.transY);
		int imageWidth = mGraphics.getImageWidth(imgHPtem);
		int imageHeight = mGraphics.getImageHeight(imgHPtem);
		int num = imageWidth;
		int num2 = imageWidth;
		int num3 = x - imageWidth;
		int num4 = y - h - 5;
		int num5 = imageWidth * 2 * per / 100;
		if (num5 > num)
		{
			num2 = num5 - num;
			if (num2 <= 0)
			{
				num2 = 0;
			}
		}
		else
		{
			num = num5;
			num2 = 0;
		}
		g.drawImage(GameScr.imgHP_tm_xam, num3, num4, mGraphics.TOP | mGraphics.LEFT);
		g.drawImage(GameScr.imgHP_tm_xam, num3 + imageWidth, num4, mGraphics.TOP | mGraphics.LEFT);
		g.drawRegion(imgHPtem, 0, 0, num, imageHeight, 0, num3, num4, mGraphics.TOP | mGraphics.LEFT);
		g.drawRegion(imgHPtem, 0, 0, num2, imageHeight, 0, num3 + imageWidth, num4, mGraphics.TOP | mGraphics.LEFT);
		if (shock)
		{
			Res.outz("type= " + type);
			tShock++;
			Effect me = new Effect((type != 2) ? 22 : 19, x + tShock * 50, y + 25, 2, 1, -1);
			EffecMn.addEff(me);
			Effect me2 = new Effect((type != 2) ? 22 : 19, x - tShock * 50, y + 25, 2, 1, -1);
			EffecMn.addEff(me2);
			if (tShock == 50)
			{
				tShock = 0;
				shock = false;
			}
		}
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
		dir = ((mobToAttack.x > x) ? 1 : (-1));
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
		return (!haftBody) ? (y - 60) : (y - 20);
	}

	public new int getH()
	{
		return 40;
	}

	public new int getW()
	{
		return 60;
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
}
