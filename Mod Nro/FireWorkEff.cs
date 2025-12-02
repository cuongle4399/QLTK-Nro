using System;

public class FireWorkEff
{
	private static int w;

	private static int h;

	private static MyRandom r = new MyRandom();

	private static MyVector mg = new MyVector();

	private static int f = 17;

	private static int x;

	private static int y;

	private static int ag;

	private static int x0;

	private static int y0;

	private static int t;

	private static int v;

	private static int ymax = 269;

	private static float a;

	private static int[] mang_x = new int[3];

	private static int[] mang_y = new int[3];

	private static bool st = false;

	private static long last = 0L;

	private static long delay = 150L;

	public static void preDraw()
	{
		if (st)
		{
			animate();
		}
		if (t > 32 && st)
		{
			st = false;
			mg.removeAllElements();
			mg.addElement(new FireWorkMn(Res.random(50, GameCanvas.w - 50), Res.random(GameCanvas.h - 100, GameCanvas.h), 5, 72));
		}
	}

	public static void paint(mGraphics g)
	{
		preDraw();
		g.setColor(0);
		g.fillRect(0, 0, w, h);
		g.setColor(16711680);
		for (int i = 0; i < mg.size(); i++)
		{
			((FireWorkMn)mg.elementAt(i)).paint(g);
		}
		if (!st)
		{
			keyPressed(-(Math.abs(r.nextInt() % 3) + 5));
		}
	}

	public static void keyPressed(int k)
	{
		if (k == -5 && !st)
		{
			x0 = w / 2;
			ag = 80;
			st = true;
			add();
		}
		else if (k == -7 && !st)
		{
			ag = 60;
			x0 = 0;
			st = true;
			add();
		}
		else if (k == -6 && !st)
		{
			ag = 120;
			x0 = w;
			st = true;
			add();
		}
	}

	public static void add()
	{
		y0 = 0;
		v = 16;
		t = 0;
		a = 0f;
		for (int i = 0; i < 3; i++)
		{
			mang_y[i] = 0;
			mang_x[i] = x0;
		}
		st = true;
	}

	public static void animate()
	{
		mang_y[2] = mang_y[1];
		mang_x[2] = mang_x[1];
		mang_y[1] = mang_y[0];
		mang_x[1] = mang_x[0];
		mang_y[0] = y;
		mang_x[0] = x;
		x = Res.cos((int)((double)ag * System.Math.PI / 180.0)) * v * t + x0;
		y = (int)((float)(v * Res.sin((int)((double)ag * System.Math.PI / 180.0)) * t) - a * (float)t * (float)t / 2f) + y0;
		if (time() - last >= delay)
		{
			t++;
			last = time();
		}
	}

	public static long time()
	{
		return mSystem.currentTimeMillis();
	}
}
