using System;

public class Effect_End
{
	public const sbyte Lvlpaint_All = -1;

	public const sbyte Lvlpaint_Front = 0;

	public const sbyte Lvlpaint_Mid = 1;

	public const sbyte Lvlpaint_Mid_2 = 2;

	public const sbyte Lvlpaint_Behind = 3;

	public const short End_String_Lose = 0;

	public const short End_String_Win = 1;

	public const short End_String_Draw = 2;

	public const short End_FireWork = 3;

	public const short End_line_in = 9;

	public const short End_e8_rock = 10;

	public const short End_e8_ice = 11;

	public const short End_SUB_MaFuBa = 16;

	public const short End_SUB_Destroy = 17;

	public const short End_POW_Kamex10 = 18;

	public const short End_POW_Destroy = 19;

	public const short End_POW_MaFuBa = 20;

	public const short End_GONG_Kamex10 = 21;

	public const short End_GONG_Destroy = 22;

	public const short End_GONG_MaFuBa = 23;

	public const short End_Skill_Kamex10 = 24;

	public const short End_Skill_Destroy = 25;

	public const short End_Skill_MaFuBa = 26;

	private MyVector VecEffEnd = new MyVector("EffectEnd VecEffEnd");

	public FrameImage fraImgEff;

	public byte[] nFrame = new byte[10];

	public byte[] nFrame_2 = new byte[10];

	public int typePaint;

	public int typeEffect;

	public int typeSub;

	public int range;

	public short idEndeff;

	public int fRemove;

	public int fMove;

	public int n_frame;

	public int x;

	public int y;

	public int w;

	public int h;

	public int dir;

	public int dir_nguoc;

	public int levelPaint;

	public int f;

	public int frame;

	public int fSpeed;

	public int vx;

	public int vy;

	public int x1000;

	public int y1000;

	public int vx1000;

	public int vy1000;

	public int dy_throw;

	public int vMax;

	public int toX;

	public int toY;

	public int stt;

	public int dx;

	public int dy;

	public short timeRemove;

	public long time;

	public bool isRemove;

	public bool isAddSub;

	public Char charUse;

	public Point[] listObj;

	public Point target;

	public static short[][] arrInfoEff = new short[29][]
	{
		new short[3] { 68, 264, 4 },
		new short[3] { 30, 120, 4 },
		new short[3] { 66, 280, 4 },
		new short[3] { 0, 0, 1 },
		new short[3] { 111, 68, 2 },
		new short[3] { 90, 68, 2 },
		new short[3] { 125, 68, 2 },
		new short[3] { 47, 282, 6 },
		new short[3] { 10, 40, 4 },
		new short[3] { 92, 525, 7 },
		new short[3] { 62, 372, 6 },
		new short[3] { 80, 352, 4 },
		new short[3] { 80, 352, 4 },
		new short[3] { 80, 352, 4 },
		new short[3] { 72, 240, 3 },
		new short[3] { 20, 42, 3 },
		new short[3] { 65, 160, 4 },
		new short[3] { 50, 300, 6 },
		new short[3] { 84, 168, 2 },
		new short[3] { 90, 540, 6 },
		new short[3] { 180, 900, 6 },
		new short[3] { 62, 186, 3 },
		new short[3] { 34, 80, 4 },
		new short[3] { 140, 560, 4 },
		new short[3] { 64, 600, 6 },
		new short[3] { 36, 200, 5 },
		new short[3] { 35, 200, 5 },
		new short[3] { 50, 250, 5 },
		new short[3] { 50, 240, 6 }
	};

	public int life;

	public int goc_Arc;

	public int va;

	public int gocT_Arc;

	public byte[] mpaintone_Arrow = new byte[24]
	{
		12, 11, 10, 9, 8, 7, 6, 5, 4, 3,
		2, 1, 0, 23, 22, 21, 20, 19, 18, 17,
		16, 15, 14, 13
	};

	public byte[] mImageArrow = new byte[24]
	{
		0, 0, 2, 1, 1, 2, 0, 0, 2, 1,
		1, 2, 0, 0, 2, 1, 1, 2, 0, 0,
		2, 1, 1, 2
	};

	public byte[] mXoayArrow = new byte[24]
	{
		2, 2, 3, 3, 3, 4, 5, 5, 5, 5,
		5, 1, 0, 0, 0, 0, 0, 7, 6, 6,
		6, 6, 6, 2
	};

	private int rS;

	private int angleS;

	private int angleO;

	private int iAngleS;

	private int iDotS;

	private int[] xArgS;

	private int[] yArgS;

	private int[] xDotS;

	private int[] yDotS;

	public static int[][] colorStar = new int[3][]
	{
		new int[3] { 16310304, 16298056, 16777215 },
		new int[3] { 7045120, 12643960, 16777215 },
		new int[3] { 2407423, 11987199, 16777215 }
	};

	private int[] colorpaint;

	private int indexColorStar;

	private int xline;

	private int yline;

	private FrameImage[] fra_skill;

	public Effect_End(int type, int typeSub, int x, int y, int levelPaint, int dir, short timeRemove, Point[] listObj)
	{
		f = 0;
		stt = 0;
		typeEffect = type;
		this.typeSub = typeSub;
		this.x = x;
		this.y = y;
		this.levelPaint = levelPaint;
		this.dir = dir;
		dir_nguoc = ((dir == -1) ? 2 : 0);
		time = mSystem.currentTimeMillis();
		this.timeRemove = timeRemove;
		isRemove = (isAddSub = false);
		n_frame = 4;
		if (listObj != null)
		{
			this.listObj = new Point[listObj.Length];
			for (int i = 0; i < this.listObj.Length; i++)
			{
				this.listObj[i] = listObj[i];
			}
		}
		get_Img_Skill();
		create_Effect();
	}

	public Effect_End(int type, int typeSub, int typePaint, Char charUse, Point target, int levelPaint, short timeRemove, short range)
	{
		f = 0;
		stt = 0;
		typeEffect = type;
		this.typeSub = typeSub;
		this.typePaint = typePaint;
		this.charUse = charUse;
		if (charUse.containsCaiTrang(1265))
		{
			if (typeEffect == 21 || typeEffect == 22 || typeEffect == 23)
			{
				this.charUse.cx += 10 * this.charUse.cdir;
			}
			else if (typeEffect == 18 || typeEffect == 19 || typeEffect == 20)
			{
				this.charUse.cx += -15 * this.charUse.cdir;
			}
			else
			{
				this.charUse.cx += 15 * this.charUse.cdir;
			}
		}
		x = this.charUse.cx;
		y = this.charUse.cy;
		dir = this.charUse.cdir;
		dir_nguoc = ((dir == -1) ? 2 : 0);
		this.target = target;
		this.levelPaint = levelPaint;
		time = mSystem.currentTimeMillis();
		this.timeRemove = timeRemove;
		this.range = range;
		isRemove = (isAddSub = false);
		n_frame = 4;
		get_Img_Skill();
		create_Effect();
	}

	public Effect_End(int type, int typeSub, int typePaint, int x, int y, int levelPaint, int dir, short timeRemove, Point[] listObj)
	{
		f = 0;
		stt = 0;
		typeEffect = type;
		this.typeSub = typeSub;
		this.typePaint = typePaint;
		this.x = x;
		this.y = y;
		this.levelPaint = levelPaint;
		this.dir = dir;
		dir_nguoc = ((dir == -1) ? 2 : 0);
		time = mSystem.currentTimeMillis();
		this.timeRemove = timeRemove;
		isRemove = (isAddSub = false);
		n_frame = 4;
		if (listObj != null)
		{
			this.listObj = new Point[listObj.Length];
			for (int i = 0; i < this.listObj.Length; i++)
			{
				this.listObj[i] = listObj[i];
			}
		}
		get_Img_Skill();
		create_Effect();
	}

	public static Image getImage(int id)
	{
		if (id < 0)
		{
			return null;
		}
		string path = "/e/e_" + id + ".png";
		Image result = null;
		try
		{
			result = mSystem.loadImage(path);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static void setSoundSkill_END(int x, int y, int typeEffect)
	{
		try
		{
			int num = -1;
			int num2 = Res.random(3);
			if (num >= 0)
			{
				SoundMn.playSound(x, y, num, SoundMn.volume);
			}
		}
		catch (Exception ex)
		{
			Res.err("ERR setSoundSkill_END: " + ex.ToString());
		}
	}

	public void create_Effect()
	{
		try
		{
			setSoundSkill_END(x, y, typeEffect);
			switch (typeEffect)
			{
			case 0:
			case 1:
			case 2:
				set_End_String(typeEffect);
				break;
			case 3:
				set_FireWork();
				break;
			case 16:
			case 17:
				set_Sub();
				break;
			case 18:
			case 19:
			case 20:
				set_Pow();
				break;
			case 21:
			case 22:
			case 23:
				set_Gong();
				break;
			case 24:
				set_Skill_Kamex10();
				break;
			case 25:
				set_Skill_Destroy();
				break;
			case 26:
				set_Skill_MaFuba();
				break;
			case 9:
				set_LINE_IN();
				break;
			case 10:
			case 11:
				set_End_Rock();
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 12:
			case 13:
			case 14:
			case 15:
				break;
			}
		}
		catch (Exception ex)
		{
			Res.err("ERR create_Effect: " + ex.ToString());
			removeEff();
		}
	}

	public void update()
	{
		try
		{
			f++;
			switch (typeEffect)
			{
			case 0:
			case 1:
			case 2:
				upd_End_String();
				break;
			case 3:
				upd_FireWork();
				break;
			case 16:
			case 17:
				upd_Sub();
				break;
			case 18:
			case 19:
			case 20:
				upd_Pow();
				break;
			case 21:
			case 22:
			case 23:
				upd_Gong();
				break;
			case 24:
				upd_Skill_Kamex10();
				break;
			case 25:
				upd_Skill_Destroy();
				break;
			case 26:
				upd_Skill_MaFuba();
				break;
			case 9:
				upd_LINE_IN();
				break;
			case 10:
			case 11:
				upd_End_Rock();
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 12:
			case 13:
			case 14:
			case 15:
				break;
			}
		}
		catch (Exception ex)
		{
			Res.err("ERR update: " + ex.ToString());
			removeEff();
		}
	}

	public void paint(mGraphics g)
	{
		try
		{
			if (isRemove || f < 0)
			{
				return;
			}
			switch (typeEffect)
			{
			case 0:
			case 1:
			case 2:
				pnt_End_String(g);
				break;
			case 3:
				pnt_FireWork(g);
				break;
			case 17:
				pnt_Sub(g, mGraphics.VCENTER);
				break;
			case 16:
				if (typeSub == 0)
				{
					pnt_Sub(g, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
				else
				{
					pnt_Sub(g, mGraphics.VCENTER | mGraphics.HCENTER);
				}
				break;
			case 18:
			case 19:
			case 20:
				pnt_Pow(g, mGraphics.BOTTOM | mGraphics.HCENTER);
				break;
			case 21:
			case 22:
			case 23:
				pnt_Gong(g, mGraphics.VCENTER | mGraphics.HCENTER);
				break;
			case 24:
				pnt_Skill_Kamex10(g);
				break;
			case 25:
				pnt_Skill_Destroy(g);
				break;
			case 26:
				pnt_Skill_MaFuba(g);
				break;
			case 9:
				pnt_LINE_IN(g);
				break;
			case 10:
			case 11:
				pnt_End_Rock(g);
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 12:
			case 13:
			case 14:
			case 15:
				break;
			}
		}
		catch (Exception ex)
		{
			Res.err(ex.ToString());
			removeEff();
		}
	}

	public void removeEff()
	{
		isRemove = true;
	}

	public void createDanFocus(bool isRandom, Char obj)
	{
		if (isRandom)
		{
			switch (Res.random(4))
			{
			case 0:
				gocT_Arc = 90;
				break;
			case 1:
				gocT_Arc = 270;
				break;
			case 2:
				gocT_Arc = 180;
				break;
			case 3:
				gocT_Arc = 0;
				break;
			}
		}
		else if (obj.cdir == 1)
		{
			gocT_Arc = 0;
		}
		else
		{
			gocT_Arc = 180;
		}
		va = (short)(256 * vMax);
		vx = 0;
		vy = 0;
		life = 0;
		vx1000 = va * Res.cos(gocT_Arc) >> 10;
		vy1000 = va * Res.sin(gocT_Arc) >> 10;
	}

	public void updateAngleXP(int fmove)
	{
		if (f < fmove)
		{
			return;
		}
		if (charUse == null || target == null || f >= fRemove)
		{
			f = fRemove;
			return;
		}
		int num = target.x - charUse.cx;
		int num2 = target.y - charUse.cy;
		life++;
		if ((Res.abs(num) < 10 && Res.abs(num2) < 10) || life > fRemove)
		{
			f = fRemove;
			return;
		}
		int num3 = Res.angle(num, num2);
		if (Res.abs(num3 - gocT_Arc) < 90 || num * num + num2 * num2 > 4096)
		{
			if (Res.abs(num3 - gocT_Arc) < 15)
			{
				gocT_Arc = num3;
			}
			else if ((num3 - gocT_Arc >= 0 && num3 - gocT_Arc < 180) || num3 - gocT_Arc < -180)
			{
				gocT_Arc = Res.fixangle(gocT_Arc + 15);
			}
			else
			{
				gocT_Arc = Res.fixangle(gocT_Arc - 15);
			}
		}
		if (f > fRemove * 2 / 3 && va < 8192)
		{
			va += 3096;
		}
		vx1000 = va * Res.cos(gocT_Arc) >> 10;
		vy1000 = va * Res.sin(gocT_Arc) >> 10;
		num += vx1000;
		int num4 = num >> 10;
		x += num4;
		num &= 0x3FF;
		num2 += vy1000;
		int num5 = num2 >> 10;
		y += num5;
		num2 &= 0x3FF;
	}

	public int setFrameAngle(int goc)
	{
		if (goc <= 15 || goc > 345)
		{
			return 12;
		}
		int num = (goc - 15) / 15 + 1;
		if (num > 24)
		{
			num = 24;
		}
		return mpaintone_Arrow[num];
	}

	public void create_Arrow(int vMax, Point targetPoint)
	{
		this.vMax = vMax;
		int num = 0;
		int num2 = 0;
		if (targetPoint != null)
		{
			num = targetPoint.x - x;
			num2 = targetPoint.y - y;
			toX = targetPoint.x;
			toY = targetPoint.y;
		}
		else
		{
			num = toX - x;
			num2 = toY - y;
		}
		if (x > toX)
		{
			dir = 2;
			dir_nguoc = 0;
		}
		else
		{
			dir = 0;
			dir_nguoc = 2;
		}
		int frameAngle = Res.angle(num, num2);
		frame = setFrameAngle(frameAngle);
		fSpeed = frame;
		create_Speed(num, num2);
	}

	public void create_Speed(int dx, int dy)
	{
		int num = 0;
		int num2 = 0;
		int num3 = Res.getDistance(dx, dy) / vMax;
		if (num3 == 0)
		{
			num3 = 1;
		}
		num = dx / num3;
		num2 = dy / num3;
		if (num == 0 && dx < num3)
		{
			num = ((dx >= 0) ? 1 : (-1));
		}
		if (num2 == 0 && dy < num3)
		{
			num2 = ((dy >= 0) ? 1 : (-1));
		}
		if (Res.abs(num) > Res.abs(dx))
		{
			num = dx;
		}
		if (Res.abs(num2) > Res.abs(dy))
		{
			num2 = dy;
		}
		vx = num;
		vy = num2;
	}

	public void moveTo_xy(int toX, int toY, int fMove, int typeEff_End, int rangeEnd)
	{
		if (f < fMove)
		{
			frame = setFrameAngle((dir == -1) ? 180 : 0);
			return;
		}
		frame = fSpeed;
		if (Res.abs(x - toX) < Res.abs(vx))
		{
			x = toX;
			vx = 0;
		}
		else
		{
			x += vx;
		}
		if (Res.abs(y - toY) < Res.abs(vy))
		{
			y = toY;
			vy = 0;
		}
		else
		{
			y += vy;
		}
		if (Res.abs(x - toX) >= Res.abs(vMax) || Res.abs(y - toY) >= Res.abs(vMax) || typeEff_End < 0)
		{
			return;
		}
		if (target != null)
		{
			int num = target.x;
			int num2 = target.y;
			if (rangeEnd > 0)
			{
				num += Res.random_Am(0, rangeEnd);
				num2 += Res.random_Am(0, rangeEnd);
			}
			GameScr.addEffectEnd(typeEff_End, 0, 0, num, num2, 1, 0, -1, null);
			removeEff();
		}
		else if (isAddSub)
		{
			isAddSub = false;
			int num3 = x;
			int num4 = y;
			if (rangeEnd > 1)
			{
				num3 += Res.random_Am_0(rangeEnd);
				num4 += Res.random_Am_0(rangeEnd);
			}
			GameScr.addEffectEnd(typeEff_End, 0, 0, num3, num4, 1, 0, -1, null);
		}
	}

	public void paint_Arrow(mGraphics g, FrameImage frm, int index, int x, int y, int anchor, bool isCountFr)
	{
		if (frm != null)
		{
			int num = frm.nFrame / 3;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = 0;
			int num3 = 3;
			if (frm.nFrame <= 6)
			{
				num2 = ((frm.nFrame <= 3) ? (f % num) : ((f / num3 % 2 != 0) ? 3 : 0));
			}
			else
			{
				num = 1;
				num2 = ((f / num3 - fMove > 8) ? 6 : ((f / num3 - fMove > 4) ? 3 : 0));
			}
			int idx = num * mImageArrow[index] + num2;
			if (frm.nFrame < 3)
			{
				idx = f / num3 % frm.nFrame;
			}
			if (isCountFr)
			{
				idx = f / num3 % frm.nFrame;
			}
			frm.drawFrame(idx, x, y, mXoayArrow[index], anchor, g);
		}
	}

	private void set_End_String(int typeEffect)
	{
		switch (typeEffect)
		{
		case 0:
			fraImgEff = new FrameImage(4);
			break;
		case 1:
			fraImgEff = new FrameImage(5);
			break;
		case 2:
			fraImgEff = new FrameImage(6);
			break;
		}
		fRemove = 100;
		dy_throw = GameCanvas.h / 3 + 10;
		vy = 10;
		y1000 = 0;
		isAddSub = false;
	}

	private void upd_End_String()
	{
		x = GameCanvas.hw;
		y = y1000;
		if (f > fRemove)
		{
			removeEff();
		}
		vy++;
		if (vy > 15)
		{
			vy = 15;
		}
		if (y1000 + vy < dy_throw)
		{
			y1000 += vy;
			return;
		}
		y1000 = dy_throw;
		if (!isAddSub)
		{
			isAddSub = true;
			if (typeSub != -1)
			{
				GameScr.addEffectEnd(typeSub, 0, 0, x, y, levelPaint, 0, -1, null);
			}
		}
	}

	private void pnt_End_String(mGraphics g)
	{
		if (fraImgEff != null)
		{
			fraImgEff.drawFrame(f / 5 % fraImgEff.nFrame, x, y, 0, 33, g);
		}
	}

	private void set_FireWork()
	{
		int num = 0;
		num = Res.random(3, 5);
		fRemove = 90;
		for (int i = 0; i < num; i++)
		{
			Point point = new Point();
			point.x = x + Res.random_Am_0(4);
			point.y = y + Res.random_Am_0(5);
			if (typeSub == 0)
			{
				point.fRe = Res.random(10);
				int num2 = 1;
				if (i % 2 == 0)
				{
					num2 = -1;
				}
				point.x = x + Res.random(arrInfoEff[5][0] / 2) * num2;
				point.y = y - Res.random(arrInfoEff[5][1] / 2);
				point.fraImgEff = new FrameImage(7);
			}
			VecEffEnd.addElement(point);
		}
	}

	private void upd_FireWork()
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			point.update();
			if (point.f == point.fRe)
			{
				SoundMn.playSound(point.x, point.y, SoundMn.FIREWORK, SoundMn.volume);
			}
			if (point.f - point.fRe <= point.fraImgEff.nFrame * 3 - 1)
			{
				continue;
			}
			point.f = 0;
			if (typeSub == 0)
			{
				point.fRe = Res.random(10);
				int num = 1;
				if (i % 2 == 0)
				{
					num = -1;
				}
				point.x = x + Res.random(arrInfoEff[5][0] / 2) * num;
				point.y = y - Res.random(arrInfoEff[5][1] / 2);
			}
		}
		if (f >= fRemove)
		{
			removeEff();
		}
	}

	private void pnt_FireWork(mGraphics g)
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			if (point.f - point.fRe > -1 && point.fraImgEff != null)
			{
				point.fraImgEff.drawFrame((point.f - point.fRe) / 3 % point.fraImgEff.nFrame, point.x, point.y, 0, 3, g);
			}
		}
	}

	private void set_Skill_Kamex10()
	{
		w = fra_skill[0].frameWidth;
		h = fra_skill[0].frameHeight;
		vMax = Res.abs(x - target.x);
		nFrame = new byte[6] { 0, 0, 0, 1, 1, 1 };
		isAddSub = false;
		SoundMn.playSound(x, y, SoundMn.KAMEX10_1, SoundMn.volume);
	}

	private void upd_Skill_Kamex10()
	{
		fSpeed++;
		w += 20;
		if (w > vMax)
		{
			w = vMax;
		}
		x = charUse.cx + 10;
		y = charUse.cy - 3;
		if (dir == -1)
		{
			x = charUse.cx - w - 10;
		}
		if (!isAddSub && GameCanvas.timeNow - time >= timeRemove)
		{
			f = 0;
			nFrame = new byte[6] { 2, 2, 2, 3, 3, 3 };
			isAddSub = true;
		}
		if (f > nFrame.Length - 1)
		{
			if (isAddSub)
			{
				removeEff();
			}
			else
			{
				f = 0;
			}
		}
	}

	private void pnt_Skill_Kamex10(mGraphics g)
	{
		if (fra_skill != null)
		{
			g.setClip(x, y - h / 2, w, h);
			Fill_Rect_Img(g, fra_skill[0], fra_skill[1], fra_skill[2], nFrame[f], x, y, vMax);
			GameCanvas.resetTransGameScr(g);
			if (dir == -1 && fra_skill[0] != null)
			{
				fra_skill[0].drawFrame(nFrame[f], x + w - fra_skill[0].frameWidth, y - fra_skill[0].frameHeight / 2 - 1, 2, 0, g);
			}
		}
	}

	private void set_Skill_Destroy()
	{
		x = charUse.cx + 20 * charUse.cdir;
		int num = 15;
		fMove = timeRemove / num;
		if (target != null)
		{
			for (int i = 0; i < num; i++)
			{
				Point point = new Point();
				point.fraImgEff = fra_skill[0];
				point.fraImgEff_2 = fra_skill[2];
				point.x = x;
				point.y = y;
				if (target != null)
				{
					point.toX = target.x;
					point.toY = target.y;
					if (range > 0)
					{
						point.toX += Res.random_Am(0, range);
						point.toY += Res.random_Am(0, range);
					}
				}
				vMax = Res.random(9, 12);
				if (i == num - 1)
				{
					point.fraImgEff = fra_skill[1];
					point.fraImgEff_2 = fra_skill[3];
					point.toX = target.x;
					point.toY = target.y;
					vMax = 9;
				}
				point.isPaint = false;
				point.isChange = false;
				point.isRemove = false;
				point.create_Arrow(vMax);
				VecEffEnd.addElement(point);
			}
		}
		else
		{
			removeEff();
		}
	}

	private void upd_Skill_Destroy()
	{
		int num = 0;
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			if (!point.isPaint && GameCanvas.timeNow - time >= i * fMove)
			{
				point.isPaint = true;
				GameScr.addEffectEnd(17, 0, typePaint, charUse.cx, charUse.cy - 3, 2, dir_nguoc, -1, null);
				if (i == VecEffEnd.size() - 1)
				{
					SoundMn.playSound(point.x, point.y, SoundMn.DESTROY_1, SoundMn.volume);
				}
				else
				{
					SoundMn.playSound(point.x, point.y, SoundMn.DESTROY_0, SoundMn.volume);
				}
			}
			if (point.isPaint && !point.isRemove)
			{
				point.f++;
				if (!point.isChange)
				{
					if (point.f < 10 && i == VecEffEnd.size() - 1 && charUse != null && !TileMap.tileTypeAt(charUse.cx - (charUse.chw + 1) * charUse.cdir, charUse.cy, (charUse.cdir != 1) ? 4 : 8))
					{
						charUse.cx -= charUse.cdir;
					}
					point.moveTo_xy(point.toX, point.toY);
					if (point.x == point.toX)
					{
						point.isChange = true;
						point.f = 0;
					}
				}
				if (point.isChange && point.f >= n_frame * point.fraImgEff_2.nFrame)
				{
					point.isRemove = true;
				}
			}
			if (point.isRemove)
			{
				num++;
			}
		}
		if (num == VecEffEnd.size())
		{
			removeEff();
		}
	}

	private void pnt_Skill_Destroy(mGraphics g)
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			if (point.isPaint && !point.isRemove)
			{
				if (!point.isChange)
				{
					point.paint_Arrow(g, point.fraImgEff, mGraphics.VCENTER | mGraphics.HCENTER, isCountFr: false);
				}
				if (point.isChange)
				{
					point.fraImgEff_2.drawFrame(point.f / n_frame % point.fraImgEff_2.nFrame, point.x, point.y, dir_nguoc, mGraphics.VCENTER | mGraphics.HCENTER, g);
				}
			}
		}
	}

	private void set_Skill_MaFuba()
	{
		nFrame = new byte[9] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
		isAddSub = false;
		fMove = 10;
		x1000 = x;
		y1000 = y + 12;
		dy = 25;
		dy_throw = 19;
		if (typeSub == 1)
		{
			dy_throw = 21;
		}
		else if (typeSub == 2)
		{
			dy_throw = 31;
		}
		h = fra_skill[1].frameHeight + 50 - dy_throw;
		vy = 1;
		vy1000 = 1;
		y = y1000 - h;
		rS = 90;
		vMax = 1;
		angleS = (angleO = 25);
		iDotS = 1;
		if (listObj != null && listObj.Length != 0)
		{
			iDotS = listObj.Length;
		}
		iAngleS = 360 / iDotS;
		xArgS = new int[iDotS];
		yArgS = new int[iDotS];
		xDotS = new int[iDotS];
		yDotS = new int[iDotS];
		GameScr.addEffectEnd(16, 0, typePaint, x1000, y1000, 1, 0, -1, null);
		SoundMn.playSound(x, y, SoundMn.MAFUBA_0, SoundMn.volume);
	}

	private void changeAngleStar()
	{
		if (vMax < 40)
		{
			vMax += 2;
		}
		angleS = angleO;
		angleS -= vMax;
		if (angleS >= 360)
		{
			angleS -= 360;
		}
		if (angleS < 0)
		{
			angleS = 360 + angleS;
		}
		angleO = angleS;
	}

	private void setDotStar()
	{
		for (int i = 0; i < yArgS.Length; i++)
		{
			if (angleS >= 360)
			{
				angleS -= 360;
			}
			if (angleS < 0)
			{
				angleS = 360 + angleS;
			}
			yArgS[i] = Res.abs(rS * Res.sin(angleS) / 1024);
			xArgS[i] = Res.abs(rS * Res.cos(angleS) / 1024);
			if (angleS < 90)
			{
				xDotS[i] = x + xArgS[i];
				yDotS[i] = y - yArgS[i];
			}
			else if (angleS >= 90 && angleS < 180)
			{
				xDotS[i] = x - xArgS[i];
				yDotS[i] = y - yArgS[i];
			}
			else if (angleS >= 180 && angleS < 270)
			{
				xDotS[i] = x - xArgS[i];
				yDotS[i] = y + yArgS[i];
			}
			else
			{
				xDotS[i] = x + xArgS[i];
				yDotS[i] = y + yArgS[i];
			}
			angleS -= iAngleS;
		}
	}

	private void upd_Skill_MaFuba()
	{
		if (stt == 0)
		{
			if (f == 3)
			{
				SoundMn.playSound(x, y, SoundMn.MAFUBA_1, SoundMn.volume);
			}
			frame++;
			if (frame > nFrame.Length - 1)
			{
				frame = nFrame.Length - 1;
			}
			if (f == fMove + 4)
			{
				GameScr.addEffectEnd(16, 1, typePaint, x, y, 3, 0, 2945, null);
			}
			if (f > fMove + 4)
			{
				rS--;
				if (rS < 0)
				{
					rS = 0;
					f = 0;
					fSpeed = 0;
					nFrame_2 = new byte[22]
					{
						1, 1, 0, 0, 0, 0, 1, 1, 1, 1,
						0, 0, 0, 1, 1, 1, 0, 0, 1, 1,
						1, 2
					};
					hideListObj_Mafuba(ishide: true);
					stt = 1;
				}
				else
				{
					changeAngleStar();
					setDotStar();
					updListObj_Mafuba(ismafuba: true);
				}
			}
		}
		else if (stt == 1)
		{
			fSpeed++;
			if (fSpeed > nFrame_2.Length - 1)
			{
				fSpeed = nFrame_2.Length - 1;
				if (GameCanvas.gameTick % 2 == 0)
				{
					vy1000++;
				}
				vy += vy1000;
				if (vy >= h - fra_skill[0].frameHeight - dy + dy_throw)
				{
					vy = h - fra_skill[0].frameHeight - dy + dy_throw;
					f = 0;
					fSpeed = 0;
					stt = 2;
					nFrame_2 = new byte[11]
					{
						3, 3, 3, 3, 3, 4, 4, 4, 5, 5,
						5
					};
				}
			}
		}
		else if (stt == 2)
		{
			fSpeed++;
			if (fSpeed > nFrame_2.Length - 1)
			{
				stt = 3;
				frame = 0;
				nFrame = new byte[17]
				{
					2, 2, 1, 1, 0, 0, 3, 3, 3, 0,
					0, 0, 4, 4, 4, 0, 0
				};
			}
		}
		else if (stt == 3)
		{
			frame++;
			if (frame == 3)
			{
				SoundMn.playSound(x, y, SoundMn.MAFUBA_1, SoundMn.volume);
			}
			if (frame > nFrame.Length - 1)
			{
				frame = 0;
				stt = 4;
				nFrame = new byte[51]
				{
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 3, 3, 3,
					0, 0, 0, 4, 4, 4, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 3, 3, 0, 0, 4,
					4
				};
			}
		}
		else
		{
			frame++;
			if (frame > nFrame.Length - 1)
			{
				frame = 0;
			}
			if (GameCanvas.timeNow - time >= timeRemove)
			{
				GameScr.addEffectEnd(16, 0, typePaint, x1000, y1000, 1, 0, -1, null);
				updListObj_Mafuba(ismafuba: false);
				removeEff();
			}
		}
	}

	private void pnt_Skill_MaFuba(mGraphics g)
	{
		if (fra_skill == null)
		{
			return;
		}
		if (nFrame != null)
		{
			fra_skill[0].drawFrame(nFrame[frame], x1000, y1000, 0, mGraphics.BOTTOM | mGraphics.HCENTER, g);
		}
		if (stt == 1 || stt == 2)
		{
			int anchor = mGraphics.BOTTOM | mGraphics.HCENTER;
			int num = dy;
			if (nFrame_2[fSpeed] == 0 || nFrame_2[fSpeed] == 1)
			{
				anchor = mGraphics.VCENTER | mGraphics.HCENTER;
				num = 0;
			}
			fra_skill[1].drawFrame(nFrame_2[fSpeed], x, y + num + vy, 0, anchor, g);
		}
	}

	private void Fill_Rect_Img(mGraphics g, FrameImage head, FrameImage body, FrameImage foot, int frame, int x, int y, int w)
	{
		int num = 0;
		int num2 = w;
		bool flag = false;
		if (head != null && foot != null)
		{
			flag = true;
			num2 = w - (head.frameWidth + foot.frameWidth);
		}
		if (num2 > 0)
		{
			num = num2 / body.frameWidth;
			if (num2 % body.frameWidth > 0)
			{
				num++;
			}
			if (dir == -1)
			{
				for (int i = 0; i < num; i++)
				{
					int num3 = 0;
					num3 = ((i != num - 1) ? ((!flag) ? (x + i * body.frameWidth) : (x + foot.frameWidth + body.frameWidth + i * body.frameWidth)) : ((!flag) ? (x + w - body.frameWidth) : (x + foot.frameWidth)));
					body.drawFrame(frame, num3, y - body.frameHeight / 2, 2, 0, g);
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					int num4 = 0;
					num4 = ((j != num - 1) ? ((!flag) ? (x + j * body.frameWidth) : (x + j * body.frameWidth + head.frameWidth)) : ((!flag) ? (x + w - body.frameWidth) : (x + w - (body.frameWidth + foot.frameWidth))));
					body.drawFrame(frame, num4, y - body.frameHeight / 2, 0, 0, g);
				}
			}
		}
		if (dir == -1)
		{
			head?.drawFrame(frame, x + w - head.frameWidth, y - head.frameHeight / 2, 2, 0, g);
			foot?.drawFrame(frame, x, y - foot.frameHeight / 2, 2, 0, g);
		}
		else
		{
			head?.drawFrame(frame, x, y - head.frameHeight / 2, 0, 0, g);
			foot?.drawFrame(frame, x + w - foot.frameWidth - 1, y - foot.frameHeight / 2, 0, 0, g);
		}
	}

	private void set_LINE_IN()
	{
		indexColorStar = typeSub;
		x1000 = x * 1000;
		y1000 = y * 1000;
		fRemove = Res.random(4, 6);
		vMax = 5;
		xline = 10;
		yline = 20;
		create_Star_Line_In(vMax, xline, yline, 0);
	}

	private void upd_LINE_IN()
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Line line = (Line)VecEffEnd.elementAt(i);
			line.update();
			if (f >= fRemove)
			{
				VecEffEnd.removeElement(line);
				i--;
			}
		}
		if (f >= fRemove)
		{
			if (GameCanvas.timeNow - time >= timeRemove)
			{
				VecEffEnd.removeAllElements();
				removeEff();
			}
			else
			{
				fRemove = Res.random(4, 6);
				f = 0;
				create_Star_Line_In(vMax, xline, yline, 0);
			}
		}
	}

	private void create_Star_Line_In(int vline, int minline, int maxline, int numpoint)
	{
		if (f == -1)
		{
			VecEffEnd.removeAllElements();
		}
		int num = 4;
		colorpaint = new int[num];
		if (maxline <= minline)
		{
			maxline = minline + 1;
		}
		for (int i = 0; i < num; i++)
		{
			if (Res.random(2) == 0)
			{
				colorpaint[i] = colorStar[indexColorStar][Res.random(3)];
			}
			else
			{
				colorpaint[i] = colorStar[indexColorStar][2];
			}
		}
		for (int j = 0; j < num; j++)
		{
			Line line = new Line();
			int num2 = 5 + 180 / num * j;
			int num3 = 180 / num + 180 / num * j - 5;
			if (num3 <= num2)
			{
				num3 = num2 + 1;
			}
			int num4 = Res.random(minline, maxline);
			int num5 = Res.random(vline, vline + 3);
			int num6 = Res.random(num2, num3);
			int num7 = Res.random(13, 23);
			bool is2Line = Res.random(4) == 0;
			num6 = Res.fixangle(num6 % 360);
			line.setLine(x1000 - Res.sin(num6) * (num4 + num7), y1000 - Res.cos(num6) * (num4 + num7), x1000 - Res.sin(num6) * num7, y1000 - Res.cos(num6) * num7, Res.sin(num6) * num5, Res.cos(num6) * num5, is2Line);
			if (numpoint > 0)
			{
				line.type = Res.random(numpoint);
			}
			VecEffEnd.addElement(line);
			line = new Line();
			num6 += 180 + Res.random_Am(2, 5);
			num6 = Res.fixangle(num6 % 360);
			line.setLine(x1000 - Res.sin(num6) * (num4 + num7), y1000 - Res.cos(num6) * (num4 + num7), x1000 - Res.sin(num6) * num7, y1000 - Res.cos(num6) * num7, Res.sin(num6) * num5, Res.cos(num6) * num5, is2Line);
			if (numpoint > 0)
			{
				line.type = Res.random(numpoint);
			}
			VecEffEnd.addElement(line);
		}
	}

	private void pnt_LINE_IN(mGraphics g)
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Line line = (Line)VecEffEnd.elementAt(i);
			if (line != null)
			{
				int color = 0;
				if (i / 2 < colorpaint.Length)
				{
					color = colorpaint[i / 2];
				}
				g.setColor(color);
				g.drawLine(line.x0 / 1000, line.y0 / 1000, line.x1 / 1000, line.y1 / 1000);
				if (line.is2Line)
				{
					g.drawLine(line.x0 / 1000 + 1, line.y0 / 1000, line.x1 / 1000 + 1, line.y1 / 1000);
				}
			}
		}
	}

	private void set_End_Rock()
	{
		fraImgEff = new FrameImage(8);
		fRemove = Res.random(23, 27);
		int num = Res.random(1, 3);
		toY = y - 40;
		for (int i = 0; i < num; i++)
		{
			Point point = new Point();
			point.x = x + Res.random_Am(0, 20);
			point.y = y + Res.random_Am_0(7);
			if (typeEffect == 10)
			{
				point.frame = Res.random(0, fraImgEff.nFrame - 2);
			}
			else if (typeEffect == 11)
			{
				point.frame = Res.random(2, fraImgEff.nFrame);
			}
			else
			{
				point.frame = Res.random(0, fraImgEff.nFrame);
			}
			point.dis = Res.random(2);
			point.vy = -Res.random(1, 4);
			VecEffEnd.addElement(point);
		}
	}

	private void upd_End_Rock()
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			point.update();
			if (point.y < toY)
			{
				VecEffEnd.removeElementAt(i);
				i--;
			}
		}
		if (f >= fRemove)
		{
			removeEff();
		}
	}

	private void pnt_End_Rock(mGraphics g)
	{
		for (int i = 0; i < VecEffEnd.size(); i++)
		{
			Point point = (Point)VecEffEnd.elementAt(i);
			if (fraImgEff != null)
			{
				fraImgEff.drawFrame(point.frame, point.x, point.y, 0, mGraphics.VCENTER | mGraphics.HCENTER, g);
			}
		}
	}

	private void updListObj_Mafuba(bool ismafuba)
	{
		if (listObj == null)
		{
			return;
		}
		for (int i = 0; i < listObj.Length; i++)
		{
			if (listObj[i] == null)
			{
				continue;
			}
			if (listObj[i].type == 0)
			{
				Mob mob = GameScr.findMobInMap(listObj[i].id);
				if (mob != null)
				{
					mob.isMafuba = ismafuba;
					mob.isHide = false;
					mob.xMFB = xDotS[i];
					mob.yMFB = yDotS[i];
				}
				continue;
			}
			Char obj = null;
			obj = ((Char.myCharz().charID != listObj[i].id) ? GameScr.findCharInMap(listObj[i].id) : Char.myCharz());
			if (obj != null)
			{
				obj.isMafuba = ismafuba;
				obj.isHide = false;
				obj.xMFB = xDotS[i];
				obj.yMFB = yDotS[i];
			}
		}
	}

	private void hideListObj_Mafuba(bool ishide)
	{
		if (listObj == null)
		{
			return;
		}
		for (int i = 0; i < listObj.Length; i++)
		{
			if (listObj[i] == null)
			{
				continue;
			}
			if (listObj[i].type == 0)
			{
				Mob mob = GameScr.findMobInMap(listObj[i].id);
				if (mob != null)
				{
					mob.isHide = ishide;
				}
				continue;
			}
			Char obj = null;
			obj = ((Char.myCharz().charID != listObj[i].id) ? GameScr.findCharInMap(listObj[i].id) : Char.myCharz());
			if (obj != null)
			{
				obj.isHide = ishide;
			}
		}
	}

	private void get_Img_Skill()
	{
		int num = 0;
		int[] array = null;
		int[] array2 = null;
		switch (typeEffect)
		{
		case 18:
			num = 24;
			array = new int[1];
			array2 = new int[1] { 9 };
			break;
		case 21:
			num = 24;
			array = new int[1] { 1 };
			array2 = new int[1] { 10 };
			break;
		case 24:
			num = 24;
			array = new int[3] { 2, 3, 4 };
			array2 = new int[3] { 11, 12, 13 };
			break;
		case 19:
			num = 25;
			array = new int[1];
			array2 = new int[1] { 14 };
			break;
		case 22:
			num = 25;
			array = new int[1] { 1 };
			array2 = new int[1] { 15 };
			break;
		case 17:
			num = 25;
			array = new int[1] { 2 };
			array2 = new int[1] { 16 };
			break;
		case 25:
			num = 25;
			array = new int[4] { 3, 4, 5, 6 };
			array2 = new int[4] { 17, 18, 19, 20 };
			break;
		case 20:
			num = 26;
			array = new int[1];
			array2 = new int[1] { 21 };
			break;
		case 23:
			num = 26;
			array = new int[1] { 1 };
			array2 = new int[1] { 22 };
			break;
		case 16:
			num = 26;
			if (typeSub == 0)
			{
				array = new int[1] { 7 };
				array2 = new int[1] { 28 };
			}
			if (typeSub == 1)
			{
				array = new int[1] { 2 };
				array2 = new int[1] { 23 };
			}
			break;
		case 26:
		{
			num = 26;
			int num2 = 0;
			int num3 = 0;
			if (typeSub == 0)
			{
				num2 = 4;
				num3 = 25;
			}
			else if (typeSub == 1)
			{
				num2 = 5;
				num3 = 26;
			}
			else if (typeSub == 2)
			{
				num2 = 6;
				num3 = 27;
			}
			array = new int[2] { num2, 3 };
			array2 = new int[2] { num3, 24 };
			break;
		}
		}
		if (array == null || array2 == null)
		{
			return;
		}
		fra_skill = new FrameImage[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string nameImg = "Skills_" + num + "_" + typePaint + "_" + array[i];
			FrameImage frameImage = mSystem.getFraImage(nameImg);
			if (frameImage == null)
			{
				frameImage = new FrameImage(array2[i]);
			}
			if (frameImage != null)
			{
				fra_skill[i] = frameImage;
			}
		}
	}

	private void set_Gong()
	{
		if (charUse != null)
		{
			if (typeEffect == 21)
			{
				x = charUse.cx - 3 * charUse.cdir;
				y = charUse.cy;
				SoundMn.playSound(x, y, SoundMn.KAMEX10_0, SoundMn.volume);
			}
			else if (typeEffect == 22)
			{
				x = charUse.cx + 20 * charUse.cdir;
				y = charUse.cy - 4;
				SoundMn.playSound(x, y, SoundMn.DESTROY_2, SoundMn.volume);
			}
			else if (typeEffect == 23)
			{
				x = charUse.cx;
				y = charUse.cy - 50;
				SoundMn.playSound(x, y, SoundMn.MAFUBA_2, SoundMn.volume);
			}
			else
			{
				x = charUse.cx;
				y = charUse.cy;
			}
		}
	}

	private void upd_Gong()
	{
		if (charUse != null)
		{
			if (typeEffect == 21)
			{
				x = charUse.cx - 3 * charUse.cdir;
				y = charUse.cy;
			}
			else if (typeEffect == 22)
			{
				x = charUse.cx + 20 * charUse.cdir;
				y = charUse.cy - 4;
			}
			else if (typeEffect == 23)
			{
				x = charUse.cx;
				y = charUse.cy - 50;
			}
			else
			{
				x = charUse.cx;
				y = charUse.cy;
			}
		}
		if (timeRemove > 0)
		{
			if (GameCanvas.timeNow - time >= timeRemove)
			{
				removeEff();
			}
		}
		else if (f >= fra_skill[0].nFrame * n_frame)
		{
			removeEff();
		}
	}

	private void pnt_Gong(mGraphics g, int anchor)
	{
		if (fra_skill[0] != null)
		{
			fra_skill[0].drawFrame(f / n_frame % fra_skill[0].nFrame, x, y, dir_nguoc, anchor, g);
		}
	}

	private void set_Pow()
	{
		nFrame = null;
		n_frame = 3;
		if (typeEffect == 18)
		{
			if (typeSub == 0)
			{
				nFrame = new byte[9] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
			}
			else
			{
				nFrame = new byte[12]
				{
					3, 3, 3, 4, 4, 4, 5, 5, 5, 6,
					6, 6
				};
			}
		}
	}

	private void upd_Pow()
	{
		if (charUse != null)
		{
			x = charUse.cx;
			y = charUse.cy + 13;
		}
		if (timeRemove > 0)
		{
			if (GameCanvas.timeNow - time >= timeRemove)
			{
				removeEff();
			}
		}
		else if (nFrame != null)
		{
			if (f > nFrame.Length)
			{
				removeEff();
			}
		}
		else if (f >= fra_skill[0].nFrame * n_frame)
		{
			removeEff();
		}
	}

	private void pnt_Pow(mGraphics g, int anchor)
	{
		if (fra_skill[0] != null)
		{
			if (nFrame != null)
			{
				fra_skill[0].drawFrame(nFrame[f % nFrame.Length], x, y, dir_nguoc, anchor, g);
			}
			else
			{
				fra_skill[0].drawFrame(f / n_frame % fra_skill[0].nFrame, x, y, dir_nguoc, anchor, g);
			}
		}
	}

	private void set_Sub()
	{
		if (typeEffect == 17)
		{
			x += ((dir != 0) ? (-fra_skill[0].frameWidth) : 0);
		}
	}

	private void upd_Sub()
	{
		if (timeRemove > 0)
		{
			if (GameCanvas.timeNow - time >= timeRemove)
			{
				removeEff();
			}
		}
		else if (f >= fra_skill[0].nFrame * n_frame)
		{
			removeEff();
		}
	}

	private void pnt_Sub(mGraphics g, int anchor)
	{
		fra_skill[0].drawFrame(f / n_frame % fra_skill[0].nFrame, x, y, dir, anchor, g);
	}

	private void set_()
	{
	}

	private void upd_()
	{
	}

	private void pnt_(mGraphics g)
	{
	}
}
