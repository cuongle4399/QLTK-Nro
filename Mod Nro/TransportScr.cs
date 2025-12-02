public class TransportScr : mScreen, IActionListener
{
	public static TransportScr instance;

	public static Image ship;

	public static Image taungam;

	public sbyte type;

	public int speed = 5;

	public int[] posX;

	public int[] posY;

	public int[] posX2;

	public int[] posY2;

	private int cmx;

	private int n = 20;

	public short time;

	public short maxTime;

	public long last;

	public long curr;

	private bool isSpeed;

	private bool transNow;

	private int currSpeed;

	public TransportScr()
	{
		posX = new int[n];
		posY = new int[n];
		for (int i = 0; i < n; i++)
		{
			posX[i] = Res.random(0, GameCanvas.w);
			posY[i] = i * (GameCanvas.h / n);
		}
		posX2 = new int[n];
		posY2 = new int[n];
		for (int j = 0; j < n; j++)
		{
			posX2[j] = Res.random(0, GameCanvas.w);
			posY2[j] = j * (GameCanvas.h / n);
		}
	}

	public static TransportScr gI()
	{
		if (instance == null)
		{
			instance = new TransportScr();
		}
		return instance;
	}

	public override void switchToMe()
	{
		if (ship == null)
		{
			ship = GameCanvas.loadImage("/mainImage/myTexture2dfutherShip.png");
		}
		if (taungam == null)
		{
			taungam = GameCanvas.loadImage("/mainImage/taungam.png");
		}
		isSpeed = false;
		transNow = false;
		if (Char.myCharz().checkLuong() > 0 && type == 0)
		{
			center = new Command(mResources.faster, this, 1, null);
		}
		else
		{
			center = null;
		}
		currSpeed = 0;
		base.switchToMe();
	}

	public override void paint(mGraphics g)
	{
		g.setColor((type != 0) ? 3056895 : 0);
		g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
		for (int i = 0; i < n; i++)
		{
			g.setColor((type != 0) ? 11140863 : 14802654);
			g.fillRect(posX[i], posY[i], 10, 2);
		}
		if (type == 0)
		{
			g.drawRegion(ship, 0, 0, 72, 95, 7, cmx + currSpeed, GameCanvas.h / 2, 3);
		}
		if (type == 1)
		{
			g.drawRegion(taungam, 0, 0, 144, 79, 2, cmx + currSpeed, GameCanvas.h / 2, 3);
		}
		for (int j = 0; j < n; j++)
		{
			g.setColor((type != 0) ? 7536127 : 14935011);
			g.fillRect(posX2[j], posY2[j], 18, 3);
		}
		int num = maxTime - time;
		if (num >= 0)
		{
			string st = "Vui lòng đợi " + num + " giây nữa";
			mFont.tahoma_7b_white.drawString(g, st, GameCanvas.w - 10, 10, mFont.RIGHT, mFont.tahoma_7b_dark);
		}
		base.paint(g);
	}

	public override void update()
	{
		if (type == 0)
		{
			if (!isSpeed)
			{
				currSpeed = GameCanvas.w / 2 * time / maxTime;
			}
		}
		else
		{
			currSpeed += 2;
		}
		Controller.isStopReadMessage = false;
		cmx = (((GameCanvas.w / 2 + cmx) / 2 + cmx) / 2 + cmx) / 2;
		if (type == 1)
		{
			cmx = 0;
		}
		for (int i = 0; i < n; i++)
		{
			posX[i] -= speed / 2;
			if (posX[i] < -20)
			{
				posX[i] = GameCanvas.w;
			}
		}
		for (int j = 0; j < n; j++)
		{
			posX2[j] -= speed;
			if (posX2[j] < -20)
			{
				posX2[j] = GameCanvas.w;
			}
		}
		if (GameCanvas.gameTick % 3 == 0)
		{
			speed += ((!isSpeed) ? 1 : 2);
		}
		if (speed > ((!isSpeed) ? 25 : 80))
		{
			speed = ((!isSpeed) ? 25 : 80);
		}
		curr = mSystem.currentTimeMillis();
		if (curr - last >= 1000)
		{
			time++;
			last = curr;
		}
		if (isSpeed)
		{
			currSpeed += 3;
		}
		if (currSpeed >= GameCanvas.w / 2 + 30 && !transNow)
		{
			transNow = true;
			Service.gI().transportNow();
		}
		base.update();
	}

	public override void updateKey()
	{
		base.updateKey();
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1)
		{
			GameCanvas.startYesNoDlg(mResources.fasterQuestion, new Command(mResources.YES, this, 2, null), new Command(mResources.NO, this, 3, null));
		}
		if (idAction == 2 && Char.myCharz().checkLuong() > 0)
		{
			isSpeed = true;
			GameCanvas.endDlg();
			center = null;
		}
		if (idAction == 3)
		{
			GameCanvas.endDlg();
		}
	}
}
