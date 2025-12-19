using System;
using Assets.src.g;

public class Mob : IMapObject
{
	public const sbyte TYPE_DUNG = 0;

	public const sbyte TYPE_DI = 1;

	public const sbyte TYPE_NHAY = 2;

	public const sbyte TYPE_LET = 3;

	public const sbyte TYPE_BAY = 4;

	public const sbyte TYPE_BAY_DAU = 5;

	public static MobTemplate[] arrMobTemplate;

	public const sbyte MA_INHELL = 0;

	public const sbyte MA_DEADFLY = 1;

	public const sbyte MA_STANDWAIT = 2;

	public const sbyte MA_ATTACK = 3;

	public const sbyte MA_STANDFLY = 4;

	public const sbyte MA_WALK = 5;

	public const sbyte MA_FALL = 6;

	public const sbyte MA_INJURE = 7;

	public bool changBody;

	public short smallBody;

	public bool isHintFocus;

	public string flystring;

	public int flyx;

	public int flyy;

	public int flyIndex;

	public bool isFreez;

	public int seconds;

	public long last;

	public long cur;

	public int holdEffID;

	public long hp;

	public long maxHp;

	public long hpInjure;

	public int x;

	public int y;

	public int dir = 1;

	public int dirV = 1;

	public int status;

	public int p1;

	public int p2;

	public int p3;

	public int xFirst;

	public int yFirst;

	public int vy;

	public int exp;

	public int w;

	public int h;

	public int charIndex;

	public int timeStatus;

	public int mobId;

	public bool isx;

	public bool isy;

	public bool isDisable;

	public bool isDontMove;

	public bool isFire;

	public bool isIce;

	public bool isWind;

	public bool isDie;

	public MyVector vMobMove = new MyVector();

	public bool isGo;

	public string mobName;

	public int templateId;

	public short pointx;

	public short pointy;

	public Char cFocus;

	public long dame;

	public long dameMp;

	public int sys;

	public sbyte levelBoss;

	public sbyte level;

	public bool isBoss;

	public bool isMobMe;

	public static MyVector lastMob = new MyVector();

	public static MyVector newMob = new MyVector();

	public bool isMafuba;

	public int xMFB;

	public int yMFB;

	public int xSd;

	public int ySd;

	private bool isOutMap;

	private int wCount;

	public bool isShadown = true;

	private int tick;

	private int frame;

	public static Image imgHP = GameCanvas.loadImage("/mainImage/myTexture2dmobHP.png");

	private bool wy;

	private int wt;

	private int fy;

	private int ty;

	public int typeSuperEff;

	public bool isBusyAttackSomeOne = true;

	public int[] stand = new int[12]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1
	};

	public int[] move = new int[15]
	{
		1, 1, 1, 1, 2, 2, 2, 2, 3, 3,
		3, 3, 2, 2, 2
	};

	public int[] moveFast = new int[7] { 1, 1, 2, 2, 3, 3, 2 };

	public int[] attack1 = new int[3] { 4, 5, 6 };

	public int[] attack2 = new int[3] { 7, 8, 9 };

	public int[] hurt = new int[1];

	private int color = 8421504;

	public int len = 24;

	public int w_hp_bar = 24;

	public int per = 100;

	public int per_tem = 100;

	public byte h_hp_bar = 4;

	public Image imgHPtem;

	private int offset;

	public bool isHide;

	private sbyte[] cou = new sbyte[2] { -1, 1 };

	public Char injureBy;

	public bool injureThenDie;

	public Mob mobToAttack;

	public int forceWait;

	public bool blindEff;

	public bool sleepEff;

	public long cTimeDie;

	public static int countMobErrr = 0;

	private int[][] frameArr = new int[6][]
	{
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 },
		new int[8] { 0, 0, 0, 0, 1, 1, 1, 1 }
	};

	private bool isGetFr = true;

	public Mob()
	{
	}

	public Mob(int mobId, bool isDisable, bool isDontMove, bool isFire, bool isIce, bool isWind, int templateId, int sys, long hp, sbyte level, long maxp, short pointx, short pointy, sbyte status, sbyte levelBoss)
	{
		this.isDisable = isDisable;
		this.isDontMove = isDontMove;
		this.isFire = isFire;
		this.isIce = isIce;
		this.isWind = isWind;
		this.sys = sys;
		this.mobId = mobId;
		this.templateId = templateId;
		this.hp = hp;
		this.level = level;
		xFirst = (x = (this.pointx = pointx));
		yFirst = (y = (this.pointy = pointy));
		this.status = status;
		if (templateId != 70)
		{
			checkData();
			getData();
		}
		if (!isExistNewMob(templateId + string.Empty))
		{
			newMob.addElement(templateId + string.Empty);
		}
		maxHp = maxp;
		this.levelBoss = levelBoss;
		updateHp_bar();
		isDie = false;
		xSd = pointx;
		ySd = pointy;
		if (isNewModStand())
		{
			stand = new int[17]
			{
				0, 0, 0, 0, 0, 1, 1, 1, 1, 1,
				2, 2, 2, 2, 2, 2, 2
			};
			move = new int[17]
			{
				0, 0, 0, 0, 0, 1, 1, 1, 1, 1,
				2, 2, 2, 2, 2, 2, 2
			};
			moveFast = new int[17]
			{
				0, 0, 0, 0, 0, 1, 1, 1, 1, 1,
				2, 2, 2, 2, 2, 2, 2
			};
			attack1 = new int[12]
			{
				3, 3, 3, 3, 4, 4, 4, 4, 5, 5,
				5, 5
			};
			attack2 = new int[12]
			{
				3, 3, 3, 3, 4, 4, 4, 4, 5, 5,
				5, 5
			};
		}
		else if (isNewMod())
		{
			stand = new int[12]
			{
				0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
				1, 1
			};
			move = new int[16]
			{
				1, 1, 1, 1, 2, 2, 2, 2, 1, 1,
				1, 1, 3, 3, 3, 3
			};
			moveFast = new int[8] { 1, 1, 2, 2, 1, 1, 3, 3 };
			attack1 = new int[11]
			{
				4, 4, 4, 5, 5, 5, 6, 6, 6, 6,
				6
			};
			attack2 = new int[11]
			{
				7, 7, 7, 8, 8, 8, 9, 9, 9, 9,
				9
			};
		}
		else if (isSpecial())
		{
			stand = new int[12]
			{
				0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
				1, 1
			};
			move = new int[16]
			{
				2, 2, 3, 3, 2, 2, 4, 4, 2, 2,
				3, 3, 2, 2, 4, 4
			};
			moveFast = new int[8] { 2, 2, 3, 3, 2, 2, 4, 4 };
			attack1 = new int[8] { 5, 6, 7, 8, 9, 10, 11, 12 };
			attack2 = new int[4] { 5, 12, 13, 14 };
		}
		else
		{
			stand = new int[12]
			{
				0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
				1, 1
			};
			move = new int[15]
			{
				1, 1, 1, 1, 2, 2, 2, 2, 3, 3,
				3, 3, 2, 2, 2
			};
			moveFast = new int[7] { 1, 1, 2, 2, 3, 3, 2 };
			attack1 = new int[3] { 4, 5, 6 };
			attack2 = new int[3] { 7, 8, 9 };
		}
	}

	public bool isBigBoss()
	{
		return this is BachTuoc || this is BigBoss2 || this is BigBoss || this is NewBoss;
	}

	public void getData()
	{
		try
		{
			if (arrMobTemplate[templateId].data == null)
			{
				arrMobTemplate[templateId].data = new EffectData();
				string text = "/Mob/" + templateId;
				DataInputStream dataInputStream = MyStream.readFile(text);
				if (dataInputStream != null)
				{
					arrMobTemplate[templateId].data.readData(text + "/data");
					arrMobTemplate[templateId].data.img = GameCanvas.loadImage(text + "/img.png");
				}
				else
				{
					Service.gI().requestModTemplate(templateId);
				}
				if (lastMob.size() > 15)
				{
					arrMobTemplate[int.Parse((string)lastMob.elementAt(0))].data = null;
					lastMob.removeElementAt(0);
				}
				lastMob.addElement(templateId + string.Empty);
			}
			else
			{
				w = arrMobTemplate[templateId].data.width;
				h = arrMobTemplate[templateId].data.height;
			}
		}
		catch (Exception)
		{
		}
	}

	public virtual void setBody(short id)
	{
		changBody = true;
		smallBody = id;
	}

	public virtual void clearBody()
	{
		changBody = false;
	}

	public static bool isExistNewMob(string id)
	{
		for (int i = 0; i < newMob.size(); i++)
		{
			string text = (string)newMob.elementAt(i);
			if (text.Equals(id))
			{
				return true;
			}
		}
		return false;
	}

	public void checkData()
	{
		int num = 0;
		for (int i = 0; i < arrMobTemplate.Length; i++)
		{
			if (arrMobTemplate[i].data != null)
			{
				num++;
			}
		}
		if (num < 10)
		{
			return;
		}
		for (int j = 0; j < arrMobTemplate.Length; j++)
		{
			if (arrMobTemplate[j].data != null && num > 5)
			{
				arrMobTemplate[j].data = null;
			}
		}
	}

	public void checkFrameTick(int[] array)
	{
		if (tick > array.Length - 1)
		{
			tick = 0;
		}
		frame = array[tick];
		tick++;
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

	public void updateSuperEff()
	{
		if (typeSuperEff == 0 && GameCanvas.gameTick % 25 == 0)
		{
			ServerEffect.addServerEffect(114, this, 1);
		}
		if (typeSuperEff == 1 && GameCanvas.gameTick % 4 == 0)
		{
			ServerEffect.addServerEffect(132, this, 1);
		}
		if (typeSuperEff == 2 && GameCanvas.gameTick % 7 == 0)
		{
			ServerEffect.addServerEffect(131, this, 1);
		}
	}

	public virtual void update()
	{
		if (isMafuba)
		{
			return;
		}
		GetFrame();
		if (blindEff && GameCanvas.gameTick % 5 == 0)
		{
			ServerEffect.addServerEffect(113, x, y, 1);
		}
		if (sleepEff && GameCanvas.gameTick % 10 == 0)
		{
			EffecMn.addEff(new Effect(41, x, y, 3, 1, 1));
		}
		if (!GameCanvas.lowGraphic && status != 1 && status != 0 && !GameCanvas.lowGraphic && GameCanvas.gameTick % (15 + mobId * 2) == 0)
		{
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				Char obj = (Char)GameScr.vCharInMap.elementAt(i);
				if (obj != null && obj.isFlyAndCharge && obj.cf == 32)
				{
					Char obj2 = new Char();
					obj2.cx = obj.cx;
					obj2.cy = obj.cy - obj.ch;
					if (obj.cgender == 0)
					{
						MonsterDart.addMonsterDart(x + dir * w, y, checkIsBoss(), -100L, -100L, obj2, 25);
					}
				}
			}
			if (Char.myCharz().isFlyAndCharge && Char.myCharz().cf == 32)
			{
				Char obj3 = new Char();
				obj3.cx = Char.myCharz().cx;
				obj3.cy = Char.myCharz().cy - Char.myCharz().ch;
				if (Char.myCharz().cgender == 0)
				{
					MonsterDart.addMonsterDart(x + dir * w, y, checkIsBoss(), -100L, -100L, obj3, 25);
				}
			}
		}
		if (holdEffID != 0 && GameCanvas.gameTick % 5 == 0)
		{
			EffecMn.addEff(new Effect(holdEffID, x, y + 24, 3, 5, 1));
		}
		if (isFreez)
		{
			if (GameCanvas.gameTick % 5 == 0)
			{
				ServerEffect.addServerEffect(113, x, y, 1);
			}
			long num = mSystem.currentTimeMillis();
			if (num - last >= 1000)
			{
				seconds--;
				last = num;
				if (seconds < 0)
				{
					isFreez = false;
					seconds = 0;
				}
			}
			if (isTypeNewMod())
			{
				frame = hurt[GameCanvas.gameTick % hurt.Length];
			}
			else if (isNewModStand())
			{
				frame = attack1[GameCanvas.gameTick % attack1.Length];
			}
			else if (isNewMod())
			{
				if (GameCanvas.gameTick % 20 > 5)
				{
					frame = 11;
				}
				else
				{
					frame = 10;
				}
			}
			else if (isSpecial())
			{
				if (GameCanvas.gameTick % 20 > 5)
				{
					frame = 1;
				}
				else
				{
					frame = 15;
				}
			}
			else if (GameCanvas.gameTick % 20 > 5)
			{
				frame = 11;
			}
			else
			{
				frame = 10;
			}
		}
		if (!isUpdate())
		{
			return;
		}
		if (isShadown)
		{
			updateShadown();
		}
		if (vMobMove == null && arrMobTemplate[templateId].rangeMove != 0)
		{
			return;
		}
		if (status != 3 && isBusyAttackSomeOne)
		{
			if (cFocus != null)
			{
				cFocus.doInjure(dame, dameMp, isCrit: false, isMob: true);
			}
			else if (mobToAttack != null)
			{
				mobToAttack.setInjure();
			}
			isBusyAttackSomeOne = false;
		}
		if (levelBoss > 0)
		{
			updateSuperEff();
		}
		switch (status)
		{
		case 1:
			isDisable = false;
			isDontMove = false;
			isFire = false;
			isIce = false;
			isWind = false;
			y += p1;
			if (GameCanvas.gameTick % 2 == 0)
			{
				if (p2 > 1)
				{
					p2--;
				}
				else if (p2 < -1)
				{
					p2++;
				}
			}
			x += p2;
			if (isTypeNewMod())
			{
				frame = hurt[GameCanvas.gameTick % hurt.Length];
			}
			else if (isNewModStand())
			{
				frame = attack1[GameCanvas.gameTick % attack1.Length];
			}
			else if (isNewMod())
			{
				frame = 11;
			}
			else if (isSpecial())
			{
				frame = 15;
			}
			else
			{
				frame = 11;
			}
			if (isDie)
			{
				isDie = false;
				if (isMobMe)
				{
					for (int j = 0; j < GameScr.vMob.size(); j++)
					{
						if (((Mob)GameScr.vMob.elementAt(j)).mobId == mobId)
						{
							GameScr.vMob.removeElementAt(j);
						}
					}
				}
				p1 = 0;
				p2 = 0;
				x = (y = 0);
				hp = getTemplate().hp;
				status = 0;
				timeStatus = 0;
			}
			else
			{
				isDie = true;
			}
			break;
		case 2:
			if (holdEffID == 0 && !isFreez && !blindEff && !sleepEff)
			{
				timeStatus = 0;
				updateMobStandWait();
			}
			break;
		case 4:
			if (holdEffID == 0 && !blindEff && !sleepEff && !isFreez)
			{
				timeStatus = 0;
				p1++;
				if (p1 > 40 + mobId % 5)
				{
					y -= 2;
					status = 5;
					p1 = 0;
				}
			}
			break;
		case 3:
			if (holdEffID == 0 && !blindEff && !sleepEff && !isFreez)
			{
				updateMobAttack();
			}
			break;
		case 5:
			if (holdEffID != 0 || blindEff || sleepEff)
			{
				break;
			}
			if (isFreez)
			{
				if (arrMobTemplate[templateId].type == 4)
				{
					ty++;
					wt++;
					fy += ((!wy) ? 1 : (-1));
					if (wt == 10)
					{
						wt = 0;
						wy = !wy;
					}
				}
			}
			else
			{
				timeStatus = 0;
				updateMobWalk();
			}
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
		}
	}

	public void setInjure()
	{
		if (hp > 0 && status != 3 && status != 7)
		{
			timeStatus = 4;
			status = 7;
			if (getTemplate().type != 0 && Res.abs(x - xFirst) < 30)
			{
				x -= 10 * dir;
			}
		}
	}

	public static BigBoss getBigBoss()
	{
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob is BigBoss)
			{
				return (BigBoss)mob;
			}
		}
		return null;
	}

	public static BigBoss2 getBigBoss2()
	{
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob is BigBoss2)
			{
				return (BigBoss2)mob;
			}
		}
		return null;
	}

	public static BachTuoc getBachTuoc()
	{
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob is BachTuoc)
			{
				return (BachTuoc)mob;
			}
		}
		return null;
	}

	public static NewBoss getNewBoss(sbyte idBoss)
	{
		Mob mob = (Mob)GameScr.vMob.elementAt(idBoss);
		if (mob is NewBoss)
		{
			return (NewBoss)mob;
		}
		return null;
	}

	public static void removeBigBoss()
	{
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob is BigBoss)
			{
				GameScr.vMob.removeElement(mob);
				break;
			}
		}
	}

	public void setAttack(Char cFocus)
	{
		isBusyAttackSomeOne = true;
		mobToAttack = null;
		this.cFocus = cFocus;
		p1 = 0;
		p2 = 0;
		status = 3;
		tick = 0;
		dir = ((cFocus.cx > x) ? 1 : (-1));
		int cx = cFocus.cx;
		int cy = cFocus.cy;
		if (Res.abs(cx - x) < w * 2 && Res.abs(cy - y) < h * 2)
		{
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

	private bool isNewModStand()
	{
		return templateId == 76;
	}

	private bool isNewMod()
	{
		if (templateId >= 73 && !isNewModStand())
		{
			return true;
		}
		return false;
	}

	private void updateInjure()
	{
		if (!isBusyAttackSomeOne && GameCanvas.gameTick % 4 == 0)
		{
			if (isTypeNewMod())
			{
				frame = hurt[GameCanvas.gameTick % hurt.Length];
			}
			else if (isNewModStand())
			{
				frame = attack1[GameCanvas.gameTick % attack1.Length];
			}
			else if (isNewMod())
			{
				if (frame != 10)
				{
					frame = 10;
				}
				else
				{
					frame = 11;
				}
			}
			else if (isSpecial())
			{
				if (frame != 1)
				{
					frame = 1;
				}
				else
				{
					frame = 15;
				}
			}
			else if (frame != 10)
			{
				frame = 10;
			}
			else
			{
				frame = 11;
			}
		}
		timeStatus--;
		if (timeStatus <= 0 && (isTypeNewMod() || isNewModStand() || (isNewMod() && frame == 11) || (isSpecial() && frame == 15) || (templateId < 58 && frame == 11)))
		{
			if ((injureBy != null && injureThenDie) || hp == 0)
			{
				status = 1;
				p2 = injureBy.cdir << 1;
				p1 = -3;
				p3 = 0;
			}
			else
			{
				status = 5;
				if (injureBy != null)
				{
					dir = -injureBy.cdir;
					if (Res.abs(x - injureBy.cx) < 24)
					{
						status = 2;
					}
				}
				p1 = (p2 = (p3 = 0));
				timeStatus = 0;
			}
			injureBy = null;
		}
		else if (arrMobTemplate[templateId].type != 0 && injureBy != null)
		{
			int num = -injureBy.cdir << 1;
			if (x > xFirst - arrMobTemplate[templateId].rangeMove && x < xFirst + arrMobTemplate[templateId].rangeMove)
			{
				x -= num;
			}
		}
	}

	private void updateMobStandWait()
	{
		checkFrameTick(stand);
		switch (arrMobTemplate[templateId].type)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			p1++;
			if (p1 > 10 + mobId % 10 && (cFocus == null || Res.abs(cFocus.cx - x) > 80) && (mobToAttack == null || Res.abs(mobToAttack.x - x) > 80))
			{
				status = 5;
			}
			break;
		case 4:
		case 5:
			p1++;
			if (p1 > mobId % 3 && (cFocus == null || Res.abs(cFocus.cx - x) > 80) && (mobToAttack == null || Res.abs(mobToAttack.x - x) > 80))
			{
				status = 5;
			}
			break;
		}
		if (cFocus != null && GameCanvas.gameTick % (10 + p1 % 20) == 0)
		{
			if (cFocus.cx > x)
			{
				dir = 1;
			}
			else
			{
				dir = -1;
			}
		}
		else if (mobToAttack != null && GameCanvas.gameTick % (10 + p1 % 20) == 0)
		{
			if (mobToAttack.x > x)
			{
				dir = 1;
			}
			else
			{
				dir = -1;
			}
		}
		if (forceWait > 0)
		{
			forceWait--;
			status = 2;
		}
	}

	public void updateMobAttack()
	{
		int[] array = ((p3 != 0) ? attack2 : attack1);
		if (tick < array.Length)
		{
			checkFrameTick(array);
			if (x >= GameScr.cmx && x <= GameScr.cmx + GameCanvas.w && p3 == 0 && GameCanvas.gameTick % 2 == 0)
			{
				SoundMn.gI().charPunch(isKick: false, 0.05f);
			}
		}
		if (p1 == 0)
		{
			int num = 0;
			int num2 = 0;
			num = ((cFocus == null) ? mobToAttack.x : cFocus.cx);
			num2 = ((cFocus == null) ? mobToAttack.y : cFocus.cy);
			if (!isNewMod())
			{
				if (x > xFirst + arrMobTemplate[templateId].rangeMove)
				{
					p1 = 1;
				}
				if (x < xFirst - arrMobTemplate[templateId].rangeMove)
				{
					p1 = 1;
				}
			}
			p2++;
			if (p2 > array.Length - 1 || p1 == 1)
			{
				p1 = 1;
				if (p3 == 0)
				{
					if (cFocus != null)
					{
						cFocus.doInjure(dame, dameMp, isCrit: false, isMob: true);
					}
					else
					{
						mobToAttack.setInjure();
					}
					isBusyAttackSomeOne = false;
				}
				else
				{
					if (cFocus != null)
					{
						MonsterDart.addMonsterDart(x + dir * w, y, checkIsBoss(), dame, dameMp, cFocus, getTemplate().dartType);
					}
					else
					{
						Char obj = new Char();
						obj.cx = mobToAttack.x;
						obj.cy = mobToAttack.y;
						obj.charID = -100;
						MonsterDart.addMonsterDart(x + dir * w, y, checkIsBoss(), dame, dameMp, obj, getTemplate().dartType);
					}
					isBusyAttackSomeOne = false;
				}
			}
			dir = ((x < num) ? 1 : (-1));
		}
		else if (p1 == 1)
		{
			if (arrMobTemplate[templateId].type == 0 || isDontMove || isIce || !isWind)
			{
			}
			if (tick == array.Length)
			{
				status = 2;
				p1 = 0;
				p2 = 0;
				tick = 0;
			}
		}
		if (tick == 5 && cFocus != null && cFocus.charID == Char.myCharz().charID)
		{
			if (templateId == 88 && p3 != 0)
			{
				GameScr.shock_scr = 2;
			}
			if (templateId == 89)
			{
				GameScr.shock_scr = 2;
			}
		}
	}

	public void updateMobWalk()
	{
		int num = 0;
		try
		{
			if (injureThenDie)
			{
				status = 1;
				p2 = injureBy.cdir << 3;
				p1 = -5;
				p3 = 0;
			}
			num = 1;
			if (isFreez)
			{
				checkFrameTick(stand);
			}
			else if ((!isDontMove || !isWind) && !isIce)
			{
				status = 2;
			}
		}
		catch (Exception)
		{
			Cout.println("lineee: " + num);
		}
	}

	public MobTemplate getTemplate()
	{
		return arrMobTemplate[templateId];
	}

	public bool isPaint()
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
		if (arrMobTemplate[templateId] == null)
		{
			return false;
		}
		if (arrMobTemplate[templateId].data == null)
		{
			return false;
		}
		if (arrMobTemplate[templateId].data.img == null)
		{
			return false;
		}
		if (status == 0)
		{
			return false;
		}
		return true;
	}

	public bool isUpdate()
	{
		if (arrMobTemplate[templateId] == null)
		{
			return false;
		}
		if (arrMobTemplate[templateId].data == null)
		{
			return false;
		}
		if (status == 0)
		{
			return false;
		}
		return true;
	}

	public bool checkIsBoss()
	{
		if (isBoss || levelBoss > 0)
		{
			return true;
		}
		return false;
	}

	public void updateHp_bar()
	{
		len = (int)(hp * 100 / maxHp * w_hp_bar) / 100;
		per = (int)(hp * 100 / maxHp);
		if (per == 100)
		{
			per_tem = per;
		}
		if (per >= 100)
		{
			per_tem = per;
		}
		offset = 0;
		if (per < 30)
		{
			color = 15473700;
			imgHPtem = GameScr.imgHP_tm_do;
		}
		else if (per < 60)
		{
			color = 16744448;
			imgHPtem = GameScr.imgHP_tm_vang;
		}
		else
		{
			color = 11992374;
			imgHPtem = GameScr.imgHP_tm_xanh;
		}
	}

	public virtual void paint(mGraphics g)
	{
		if (isHide)
		{
			return;
		}
		if (isMafuba)
		{
			if (!changBody)
			{
				arrMobTemplate[templateId].data.paintFrame(g, frame, xMFB, yMFB, (dir != 1) ? 1 : 0, 2);
			}
			else
			{
				SmallImage.drawSmallImage(g, smallBody, xMFB, yMFB, (dir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
		}
		else
		{
			if (!isPaint() || (status == 1 && p3 > 0 && GameCanvas.gameTick % 3 == 0))
			{
				return;
			}
			g.translate(0, GameCanvas.transY);
			if (!changBody)
			{
				arrMobTemplate[templateId].data.paintFrame(g, frame, x, y + fy, (dir != 1) ? 1 : 0, 2);
			}
			else
			{
				SmallImage.drawSmallImage(g, smallBody, x, y + fy - 9, (dir != 1) ? 2 : 0, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			g.translate(0, -GameCanvas.transY);
			mFont.tahoma_7b_white.drawString(g, mobId.ToString() ?? "", x, y - h / 2 - 10, mFont.CENTER, mFont.tahoma_7_grey);
			if (Char.myCharz().mobFocus == null || !Char.myCharz().mobFocus.Equals(this) || status == 1 || hp <= 0 || imgHPtem == null)
			{
				return;
			}
			int imageWidth = mGraphics.getImageWidth(imgHPtem);
			int imageHeight = mGraphics.getImageHeight(imgHPtem);
			int num = imageWidth * per / 100;
			int num2 = num;
			if (per_tem >= per)
			{
				num2 = imageWidth * (per_tem -= ((GameCanvas.gameTick % 6 <= 3) ? offset : offset++)) / 100;
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
			g.drawImage(GameScr.imgHP_tm_xam, x - (imageWidth >> 1), y - h - 5, mGraphics.TOP | mGraphics.LEFT);
			g.setColor(16777215);
			g.fillRect(x - (imageWidth >> 1), y - h - 5, num2, 2);
			g.drawRegion(imgHPtem, 0, 0, num, imageHeight, 0, x - (imageWidth >> 1), y - h - 5, mGraphics.TOP | mGraphics.LEFT);
		}
	}

	public int getHPColor()
	{
		return 16711680;
	}

	public void startDie()
	{
		cTimeDie = mSystem.currentTimeMillis();
		hp = 0L;
		injureThenDie = true;
		hp = 0L;
		status = 1;
		Res.outz("MOB DIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEe");
		p1 = -3;
		p2 = -dir;
		p3 = 0;
	}

	public void attackOtherMob(Mob mobToAttack)
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

	public int getX()
	{
		return x;
	}

	public int getY()
	{
		return y;
	}

	public int getH()
	{
		return h;
	}

	public int getW()
	{
		return w;
	}

	public void stopMoving()
	{
		if (status == 5)
		{
			status = 2;
			p1 = (p2 = (p3 = 0));
			forceWait = 50;
		}
	}

	public bool isInvisible()
	{
		return status == 0 || status == 1;
	}

	public void removeHoldEff()
	{
		if (holdEffID != 0)
		{
			holdEffID = 0;
		}
	}

	public void removeBlindEff()
	{
		blindEff = false;
	}

	public void removeSleepEff()
	{
		sleepEff = false;
	}

	public void GetFrame()
	{
		if (isGetFr && isTypeNewMod() && arrMobTemplate[templateId].data != null)
		{
			frameArr = (int[][])Controller.frameHT_NEWBOSS.get(templateId + string.Empty);
			stand = frameArr[0];
			move = frameArr[1];
			moveFast = frameArr[2];
			attack1 = frameArr[3];
			attack2 = frameArr[4];
			hurt = frameArr[5];
			isGetFr = false;
		}
	}

	private bool isTypeNewMod()
	{
		if (arrMobTemplate[templateId].data != null && arrMobTemplate[templateId].data.typeData == 2)
		{
			return true;
		}
		return false;
	}
}
