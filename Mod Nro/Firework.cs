using System;

public class Firework
{
	public int w;

	public int h;

	public int v;

	public int x0;

	public int x;

	public int y;

	public int y0;

	public int angle;

	public int t;

	public int cl = 16711680;

	private float a;

	private long last;

	private long delay = 150L;

	private bool act = true;

	private int[] arr_x = new int[2];

	private int[] arr_y = new int[2];

	public Firework(int x0, int y0, int v, int angle, int cl)
	{
		this.y0 = y0;
		this.x0 = x0;
		a = 1f;
		this.v = v;
		this.angle = angle;
		w = GameCanvas.w;
		h = GameCanvas.h;
		last = time();
		for (int i = 0; i < 2; i++)
		{
			arr_x[i] = x0;
			arr_y[i] = y0;
		}
		this.cl = cl;
	}

	public void preDraw()
	{
		if (time() - last >= delay)
		{
			t++;
			last = time();
			arr_x[1] = arr_x[0];
			arr_y[1] = arr_y[0];
			arr_x[0] = x;
			arr_y[0] = y;
			x = Res.cos((int)((double)angle * System.Math.PI / 180.0)) * v * t + x0;
			y = (int)((float)(v * Res.sin((int)((double)angle * System.Math.PI / 180.0)) * t) - a * (float)t * (float)t / 2f) + y0;
		}
	}

	public void paint(mGraphics g)
	{
		Drawline(g, w - x, h - y, cl);
		for (int i = 0; i < 2; i++)
		{
			Drawline(g, w - arr_x[i], h - arr_y[i], cl);
		}
		if (act)
		{
			preDraw();
		}
	}

	public long time()
	{
		return mSystem.currentTimeMillis();
	}

	public void Drawline(mGraphics g, int x, int y, int color)
	{
		g.setColor(color);
		g.fillRect(x, y, 1, 2);
	}
}
