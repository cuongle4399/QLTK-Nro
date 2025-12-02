using main.Mod;

public class SplashScr : mScreen
{
	public static int splashScrStat;

	private bool isCheckConnect;

	private bool isSwitchToLogin;

	public static int nData = -1;

	public static int maxData = -1;

	public static SplashScr instance;

	public static Image imgLogo;

	private int timeLoading = 10;

	public long TIMEOUT;

	public SplashScr()
	{
		instance = this;
	}

	public static void loadSplashScr()
	{
		splashScrStat = 0;
	}

	public override void update()
	{
		splashScrStat++;
		if (splashScrStat == 30 && !isCheckConnect)
		{
			isCheckConnect = true;
			MainMod.LoadData();
			if (Rms.loadRMSInt("serverchat") != -1)
			{
				GameScr.isPaintChatVip = Rms.loadRMSInt("serverchat") == 0;
			}
			if (Rms.loadRMSInt("isPlaySound") != -1)
			{
				GameCanvas.isPlaySound = Rms.loadRMSInt("isPlaySound") == 1;
			}
			if (GameCanvas.isPlaySound)
			{
				SoundMn.gI().loadSound(TileMap.mapID);
			}
			SoundMn.gI().getStrOption();
			ServerListScreen.loadIP();
		}
		if (splashScrStat >= 150)
		{
			if (Session_ME.gI().isConnected())
			{
				ServerListScreen.loadScreen = true;
				GameCanvas.serverScreen.switchToMe();
			}
			else
			{
				mSystem.onDisconnected();
				if (GameCanvas.serverScreen == null)
				{
					GameCanvas.serverScreen = new ServerListScreen();
				}
				GameCanvas.serverScreen.switchToMe();
			}
		}
		ServerListScreen.updateDeleteData();
	}

	public static void loadIP()
	{
		Res.err(">>>>>loadIP:  svselect == " + Rms.loadRMSInt(ServerListScreen.RMS_svselect));
		ServerListScreen.SetIpSelect(Rms.loadRMSInt(ServerListScreen.RMS_svselect), issave: false);
		if (ServerListScreen.ipSelect == -1)
		{
			Res.err(">>>loadIP:  svselect == -1");
			if (ServerListScreen.serverPriority == -1)
			{
				ServerListScreen.SetIpSelect(ServerListScreen.serverPriority, issave: true);
			}
			else
			{
				ServerListScreen.SetIpSelect(ServerListScreen.serverPriority, issave: true);
			}
		}
		ServerListScreen.ConnectIP();
	}

	public override void paint(mGraphics g)
	{
		if (imgLogo != null && splashScrStat < 30)
		{
			g.setColor(16777215);
			g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
			g.drawImage(imgLogo, GameCanvas.w / 2, GameCanvas.h / 2, 3);
		}
		if (nData != -1)
		{
			g.setColor(0);
			g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
			if (Session_ME.gI().useProxy)
			{
				mFont.tahoma_7b_green.drawString(g, "Proxy Đang hoạt động", GameCanvas.w / 2, GameCanvas.h / 2 + 48, 2);
			}
			g.drawImage(MainMod.logoServerListScreen, GameCanvas.w / 2, GameCanvas.h / 2 - 24, StaticObj.BOTTOM_HCENTER);
			GameCanvas.paintShukiren(GameCanvas.hw, GameCanvas.h / 2 + 24, g);
			mFont.tahoma_7b_white.drawString(g, mResources.downloading_data + nData * 100 / maxData + "%", GameCanvas.w / 2, GameCanvas.h / 2, 2);
		}
		else if (splashScrStat >= 30)
		{
			g.setColor(0);
			g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
			GameCanvas.paintShukiren(GameCanvas.hw, GameCanvas.hh, g);
			ServerListScreen.paintDeleteData(g);
		}
	}

	public static void loadImg()
	{
		imgLogo = GameCanvas.loadImage("/gamelogo.png");
	}
}
