public class TextInfo
{
	public static int dx;

	public static int tx;

	public static int wStr;

	public static bool isBack;

	public static string laststring = string.Empty;

	public static void reset()
	{
		dx = 0;
		tx = 0;
		isBack = false;
	}

	public static void paint(mGraphics g, string str, int x, int y, int w, int h, mFont f)
	{
		if (wStr != f.getWidth(str) || !laststring.Equals(str))
		{
			laststring = str;
			dx = 0;
			wStr = f.getWidth(str);
			isBack = false;
			tx = 0;
		}
		g.setClip(x, y, w, h);
		if (wStr > w)
		{
			f.drawString(g, str, x - dx, y, 0);
		}
		else
		{
			f.drawString(g, str, x + w / 2, y, 2);
		}
		GameCanvas.resetTrans(g);
		if (wStr <= w)
		{
			return;
		}
		if (!isBack)
		{
			tx++;
			if (tx > 50)
			{
				dx++;
				if (dx >= wStr)
				{
					tx = 0;
					dx = -w + 30;
					isBack = true;
				}
			}
			return;
		}
		if (dx < 0)
		{
			int num = w + dx >> 1;
			dx += num;
		}
		if (dx > 0)
		{
			dx = 0;
		}
		if (dx == 0)
		{
			tx++;
			if (tx == 50)
			{
				tx = 0;
				isBack = false;
			}
		}
	}
}
