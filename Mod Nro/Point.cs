public class Point
{
	public sbyte type;

	public int x;

	public int y;

	public int g;

	public int v;

	public int vMax;

	public int w;

	public int h;

	public int color;

	public int limitY;

	public int vx;

	public int vy;

	public int x2;

	public int y2;

	public int toX;

	public int toY;

	public int dis;

	public int f;

	public int ftam;

	public int fRe;

	public int frame;

	public int maxframe;

	public int fSmall;

	public int goc;

	public int gocT_Arc;

	public int idir;

	public int dirThrow;

	public int dir;

	public int dir_nguoc;

	public int idSkill;

	public int id;

	public int levelPaint;

	public int num_per_frame = 1;

	public int life;

	public int goc_Arc;

	public int vx1000;

	public int vy1000;

	public int va;

	public int x1000;

	public int y1000;

	public int vX1000;

	public int vY1000;

	public long time;

	public long timecount;

	public MyVector vecEffPoint;

	public string name;

	public string info;

	public bool isRemove;

	public bool isSmall;

	public bool isPaint;

	public bool isChange;

	public static FrameImage[] FraEffInMap;

	public FrameImage fraImgEff;

	public FrameImage fraImgEff_2;

	public short index;

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

	public Point()
	{
	}

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public Point(int x, int y, int goc)
	{
		this.x = x;
		this.y = y;
		this.goc = goc;
	}

	public void update()
	{
		f++;
		x += vx;
		y += vy;
	}

	public void update_not_f()
	{
		x += vx;
		y += vy;
	}

	public void paint(mGraphics g)
	{
		if (!isRemove)
		{
			int num = 0;
			if (isSmall && f >= fSmall)
			{
				num = 1;
			}
			FraEffInMap[color].drawFrame(frame / 2 + num, x, y, dis, 3, g);
		}
	}

	public void updateInMap()
	{
		f++;
		if (maxframe > 1)
		{
			frame++;
			if (frame / 2 >= maxframe)
			{
				frame = 0;
			}
		}
		if (f >= fRe)
		{
			isRemove = true;
		}
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

	public void create_Arrow(int vMax)
	{
		this.vMax = vMax;
		int num = 0;
		int num2 = 0;
		num = toX - x;
		num2 = toY - y;
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
		create_Speed(num, num2);
	}

	public void create_Speed(int dx, int dy)
	{
		int frameAngle = Res.angle(dx, dy);
		frame = setFrameAngle(frameAngle);
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

	public void moveTo_xy(int toX, int toY)
	{
		int num = toX - x;
		int dy = toY - y;
		if (num > 1)
		{
			int frameAngle = Res.angle(num, dy);
			frame = setFrameAngle(frameAngle);
		}
		if (Res.abs(vx) > 0)
		{
			if (Res.abs(x - toX) < Res.abs(vx))
			{
				x = toX;
				vx = 0;
			}
			else
			{
				x += vx;
			}
		}
		else
		{
			x = toX;
			vx = 0;
		}
		if (Res.abs(vy) > 0)
		{
			if (Res.abs(y - toY) < Res.abs(vy))
			{
				y = toY;
				vy = 0;
			}
			else
			{
				y += vy;
			}
		}
		else
		{
			y = toY;
			vy = 0;
		}
	}

	public void paint_Arrow(mGraphics g, FrameImage frm, int anchor, bool isCountFr)
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
			num2 = ((frm.nFrame <= 3) ? (f % num) : ((f / num3 % 2 != 0) ? 3 : 0));
			int idx = num * mImageArrow[frame] + num2;
			if (frm.nFrame < 3)
			{
				idx = f / num3 % frm.nFrame;
			}
			if (isCountFr)
			{
				idx = f / num3 % frm.nFrame;
			}
			frm.drawFrame(idx, x, y, mXoayArrow[frame], anchor, g);
		}
	}
}
