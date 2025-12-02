public class Position
{
	public int x;

	public int y;

	public int anchor;

	public int g;

	public int v;

	public int w;

	public int h;

	public int color;

	public int limitY;

	public Layer layer;

	public short yTo;

	public short xTo;

	public short distant;

	public Position()
	{
		x = 0;
		y = 0;
	}

	public Position(int x, int y, int anchor)
	{
		this.x = x;
		this.y = y;
		this.anchor = anchor;
	}

	public Position(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void setPosTo(int xT, int yT)
	{
		xTo = (short)xT;
		yTo = (short)yT;
		distant = (short)Res.distance(x, y, xTo, yTo);
	}

	public int translate()
	{
		if (x == xTo && y == yTo)
		{
			return -1;
		}
		if (Math.abs((xTo - x) / 2) <= 1 && Math.abs((yTo - y) / 2) <= 1)
		{
			x = xTo;
			y = yTo;
			return 0;
		}
		if (x != xTo)
		{
			x += (xTo - x) / 2;
		}
		if (y != yTo)
		{
			y += (yTo - y) / 2;
		}
		if (Res.distance(x, y, xTo, yTo) <= distant / 5)
		{
			return 2;
		}
		return 1;
	}

	public void update()
	{
		layer.update();
	}

	public void paint(mGraphics g)
	{
		layer.paint(g, x, y);
	}
}
