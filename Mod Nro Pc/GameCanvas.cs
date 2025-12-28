using System;
using Assets.src.e;
using Assets.src.g;
using DoHoa;
using main.Mod;
using UnityEngine;

public class GameCanvas : IActionListener
{
	public static long timeNow;

	public static bool open3Hour;

	public static bool lowGraphic = true;

	public static bool serverchat;

	public static bool isMoveNumberPad;

	public static bool isLoading;

	public static bool isTouch;

	public static bool isTouchControl;

	public static bool isTouchControlSmallScreen;

	public static bool isTouchControlLargeScreen;

	public static bool isConnectFail;

	public static GameCanvas instance;

	public static bool bRun;

	public static bool[] keyPressed;

	public static bool[] keyReleased;

	public static bool[] keyHold;

	public static bool isPointerDown;

	public static bool isPointerClick;

	public static bool isPointerJustRelease;

	public static bool isPointerSelect;

	public static bool isPointerMove;

	public static int px;

	public static int py;

	public static int pxFirst;

	public static int pyFirst;

	public static int pxLast;

	public static int pyLast;

	public static int pxMouse;

	public static int pyMouse;

	public static Position[] arrPos;

	public static int gameTick;

	public static int taskTick;

	public static bool isEff1;

	public static bool isEff2;

	public static long timeTickEff1;

	public static long timeTickEff2;

	public static int w;

	public static int h;

	public static int hw;

	public static int hh;

	public static int wd3;

	public static int hd3;

	public static int w2d3;

	public static int h2d3;

	public static int w3d4;

	public static int h3d4;

	public static int wd6;

	public static int hd6;

	public static mScreen currentScreen;

	public static Menu menu;

	public static Panel panel;

	public static Panel panel2;

	public static ChooseCharScr chooseCharScr;

	public static LoginScr loginScr;

	public static RegisterScreen registerScr;

	public static Dialog currentDialog;

	public static MsgDlg msgdlg;

	public static InputDlg inputDlg;

	public static MyVector currentPopup;

	public static int requestLoseCount;

	public static MyVector listPoint;

	public static Paint paintz;

	public static bool isGetResFromServer;

	public static Image[] imgBG;

	public static int skyColor;

	public static int curPos;

	public static int[] bgW;

	public static int[] bgH;

	public static int planet;

	private mGraphics g = new mGraphics();

	public static Image[] imgBlue;

	public static Image[] imgViolet;

	public static MyHashTable danhHieu;

	public static MyVector messageServer;

	public static bool isPlaySound;

	private static int clearOldData;

	public static int timeOpenKeyBoard;

	public static bool isFocusPanel2;

	public static int max;

	public static int up;

	public static int upmax;

	private long timeup = mSystem.currentTimeMillis() + 1000;

	public static int isRequestMapID;

	public static long waitingTimeChangeMap;

	private static int dir_;

	private int tickWaitThongBao;

	public bool isPaintCarret;

	public static MyVector debugUpdate;

	public static MyVector debugPaint;

	public static MyVector debugSession;

	private static bool isShowErrorForm;

	public static bool paintBG;

	public static int gsskyHeight;

	public static int gsgreenField1Y;

	public static int gsgreenField2Y;

	public static int gshouseY;

	public static int gsmountainY;

	public static int bgLayer0y;

	public static int bgLayer1y;

	public static Image imgCloud;

	public static Image imgSun;

	public static Image imgSun2;

	public static Image imgClear;

	public static Image[] imgBorder;

	public static Image[] imgSunSpec;

	public static int borderConnerW;

	public static int borderConnerH;

	public static int borderCenterW;

	public static int borderCenterH;

	public static int[] cloudX;

	public static int[] cloudY;

	public static int sunX;

	public static int sunY;

	public static int sunX2;

	public static int sunY2;

	public static int[] layerSpeed;

	public static int[] moveX;

	public static int[] moveXSpeed;

	public static bool isBoltEff;

	public static bool boltActive;

	public static int tBolt;

	public static Image imgBgIOS;

	public static int typeBg;

	public static int transY;

	public static int[] yb;

	public static int[] colorTop;

	public static int[] colorBotton;

	public static int yb1;

	public static int yb2;

	public static int yb3;

	public static int nBg;

	public static int lastBg;

	public static int[] bgRain;

	public static int[] bgRainFont;

	public static Image imgCaycot;

	public static Image tam;

	public static int typeBackGround;

	public static int saveIDBg;

	public static bool isLoadBGok;

	private static long lastTimePress;

	public static int keyAsciiPress;

	public static int pXYScrollMouse;

	private static Image imgSignal;

	public static MyVector flyTexts;

	public int longTime;

	public static long timeBreakLoading;

	private static string thongBaoTest;

	public static int xThongBaoTranslate;

	public static bool isPointerJustDown;

	private int count = 1;

	public static bool csWait;

	public static MyRandom r;

	public static bool isBlackScreen;

	public static int[] bgSpeed;

	public static int cmdBarX;

	public static int cmdBarY;

	public static int cmdBarW;

	public static int cmdBarH;

	public static int cmdBarLeftW;

	public static int cmdBarRightW;

	public static int cmdBarCenterW;

	public static int hpBarX;

	public static int hpBarY;

	public static int hpBarW;

	public static int expBarW;

	public static int lvPosX;

	public static int moneyPosX;

	public static int hpBarH;

	public static int girlHPBarY;

	public int timeOut;

	public int[] dustX;

	public int[] dustY;

	public int[] dustState;

	public static int[] wsX;

	public static int[] wsY;

	public static int[] wsState;

	public static int[] wsF;

	public static Image[] imgWS;

	public static Image imgShuriken;

	public static Image[][] imgDust;

	public static bool isResume;

	public static ServerListScreen serverScreen;

	public static ServerScr serverScr;

	public static SelectCharScr _SelectCharScr;

	public bool resetToLoginScr;

	public static long TIMEOUT;

	public static int timeLoading;

	public GameCanvas()
	{
		switch (Rms.loadRMSInt("languageVersion"))
		{
		default:
			Main.main.doClearRMS();
			Rms.saveRMSInt("languageVersion", 2);
			break;
		case -1:
			Rms.saveRMSInt("languageVersion", 2);
			break;
		case 2:
			break;
		}
		clearOldData = Rms.loadRMSInt(GameMidlet.VERSION);
		if (clearOldData != 1)
		{
			Main.main.doClearRMS();
			Rms.saveRMSInt(GameMidlet.VERSION, 1);
		}
		initGame();
	}

	public static string getPlatformName()
	{
		return "Pc platform xxx";
	}

	public void initGame()
	{
		try
		{
			MotherCanvas.instance.setChildCanvas(this);
			w = MotherCanvas.instance.getWidthz();
			h = MotherCanvas.instance.getHeightz();
			hw = w / 2;
			hh = h / 2;
			isTouch = true;
			if (w >= 240)
			{
				isTouchControl = true;
			}
			if (w < 320)
			{
				isTouchControlSmallScreen = true;
			}
			if (w >= 320)
			{
				isTouchControlLargeScreen = true;
			}
			msgdlg = new MsgDlg();
			if (h <= 160)
			{
				Paint.hTab = 15;
				mScreen.cmdH = 17;
			}
			GameScr.d = ((w <= h) ? h : w) + 20;
			instance = this;
			mFont.init();
			mScreen.ITEM_HEIGHT = mFont.tahoma_8b.getHeight() + 8;
			initPaint();
			loadDust();
			loadWaterSplash();
			panel = new Panel();
			imgShuriken = loadImage("/mainImage/myTexture2df.png");
			int num = Rms.loadRMSInt("clienttype");
			if (num != -1)
			{
				if (num > 7)
				{
					Rms.saveRMSInt("clienttype", mSystem.clientType);
				}
				else
				{
					mSystem.clientType = num;
				}
			}
			if (mSystem.clientType == 7 && (Rms.loadRMSString("fake") == null || Rms.loadRMSString("fake") == string.Empty))
			{
				imgShuriken = loadImage("/mainImage/wait.png");
			}
			imgClear = loadImage("/mainImage/myTexture2der.png");
			debugUpdate = new MyVector();
			debugPaint = new MyVector();
			debugSession = new MyVector();
			for (int i = 0; i < 3; i++)
			{
				imgBorder[i] = loadImage("/mainImage/myTexture2dbd" + i + ".png");
			}
			borderConnerW = mGraphics.getImageWidth(imgBorder[0]);
			borderConnerH = mGraphics.getImageHeight(imgBorder[0]);
			borderCenterW = mGraphics.getImageWidth(imgBorder[1]);
			borderCenterH = mGraphics.getImageHeight(imgBorder[1]);
			Panel.graphics = Rms.loadRMSInt("lowGraphic");
			lowGraphic = Rms.loadRMSInt("lowGraphic") != 0;
			GameScr.isPaintChatVip = Rms.loadRMSInt("serverchat") != 1;
			Char.isPaintAura = Rms.loadRMSInt("isPaintAura") == 1;
			Char.isPaintAura2 = Rms.loadRMSInt("isPaintAura2") == 1;
			Res.init();
			SmallImage.loadBigImage();
			Panel.WIDTH_PANEL = 176;
			if (Panel.WIDTH_PANEL > w)
			{
				Panel.WIDTH_PANEL = w;
			}
			InfoMe.gI().loadCharId();
			Command.btn0left = loadImage("/mainImage/btn0left.png");
			Command.btn0mid = loadImage("/mainImage/btn0mid.png");
			Command.btn0right = loadImage("/mainImage/btn0right.png");
			Command.btn1left = loadImage("/mainImage/btn1left.png");
			Command.btn1mid = loadImage("/mainImage/btn1mid.png");
			Command.btn1right = loadImage("/mainImage/btn1right.png");
			serverScreen = new ServerListScreen();
			for (int j = 0; j < 7; j++)
			{
				imgBlue[j] = loadImage("/effectdata/blue/" + j + ".png");
				imgViolet[j] = loadImage("/effectdata/violet/" + j + ".png");
			}
			ServerListScreen.createDeleteRMS();
			serverScr = new ServerScr();
			loginScr = new LoginScr();
			_SelectCharScr = new SelectCharScr();
		}
		catch (Exception)
		{
			Debug.LogError("----------------->>>>>>>>>>errr");
		}
	}

	public static GameCanvas gI()
	{
		return instance;
	}

	public void initPaint()
	{
		paintz = new Paint();
	}

	public static void closeKeyBoard()
	{
		mGraphics.addYWhenOpenKeyBoard = 0;
		timeOpenKeyBoard = 0;
		Main.closeKeyBoard();
	}

	public void update()
	{
		if (currentScreen == _SelectCharScr)
		{
			if (gameTick % 2 == 0 && SmallImage.vt_images_watingDowload.size() > 0)
			{
				Small small = (Small)SmallImage.vt_images_watingDowload.elementAt(0);
				Service.gI().requestIcon(small.id);
				SmallImage.vt_images_watingDowload.removeElementAt(0);
			}
		}
		else if (isRequestMapID == 2 && waitingTimeChangeMap < mSystem.currentTimeMillis() && gameTick % 2 == 0 && currentScreen != null)
		{
			if (currentScreen == GameScr.gI())
			{
				if (Char.isLoadingMap)
				{
					Char.isLoadingMap = false;
				}
				if (ServerListScreen.waitToLogin)
				{
					ServerListScreen.waitToLogin = false;
				}
			}
			if (SmallImage.vt_images_watingDowload.size() > 0)
			{
				Small small2 = (Small)SmallImage.vt_images_watingDowload.elementAt(0);
				Service.gI().requestIcon(small2.id);
				SmallImage.vt_images_watingDowload.removeElementAt(0);
			}
			if (Effect.dowloadEff.size() <= 0)
			{
			}
		}
		if (messageServer.size() > 0 && thongBaoTest == null)
		{
			startserverThongBao((string)messageServer.elementAt(0));
			messageServer.removeElementAt(0);
		}
		if (gameTick % 5 == 0)
		{
			timeNow = mSystem.currentTimeMillis();
		}
		Res.updateOnScreenDebug();
		try
		{
			if (TouchScreenKeyboard.visible)
			{
				timeOpenKeyBoard++;
				if (timeOpenKeyBoard > ((!Main.isWindowsPhone) ? 10 : 5))
				{
					mGraphics.addYWhenOpenKeyBoard = 94;
				}
			}
			else
			{
				mGraphics.addYWhenOpenKeyBoard = 0;
				timeOpenKeyBoard = 0;
			}
			debugUpdate.removeAllElements();
			long num = mSystem.currentTimeMillis();
			if (num - timeTickEff1 >= 780 && !isEff1)
			{
				timeTickEff1 = num;
				isEff1 = true;
			}
			else
			{
				isEff1 = false;
			}
			if (num - timeTickEff2 >= 7800 && !isEff2)
			{
				timeTickEff2 = num;
				isEff2 = true;
			}
			else
			{
				isEff2 = false;
			}
			if (taskTick > 0)
			{
				taskTick--;
			}
			gameTick++;
			if (gameTick > 10000)
			{
				if (mSystem.currentTimeMillis() - lastTimePress > 20000 && currentScreen == loginScr)
				{
					GameMidlet.instance.exit();
				}
				gameTick = 0;
			}
			if (currentScreen != null)
			{
				if (ChatPopup.serverChatPopUp != null)
				{
					ChatPopup.serverChatPopUp.update();
					ChatPopup.serverChatPopUp.updateKey();
				}
				else if (ChatPopup.currChatPopup != null)
				{
					ChatPopup.currChatPopup.update();
					ChatPopup.currChatPopup.updateKey();
				}
				else if (currentDialog != null)
				{
					debug("B", 0);
					currentDialog.update();
				}
				else if (menu.showMenu)
				{
					debug("C", 0);
					menu.updateMenu();
					debug("D", 0);
					menu.updateMenuKey();
				}
				else if (panel.isShow)
				{
					panel.update();
					if (isPointer(panel.X, panel.Y, panel.W, panel.H))
					{
						isFocusPanel2 = false;
					}
					if (panel2 != null && panel2.isShow)
					{
						panel2.update();
						if (isPointer(panel2.X, panel2.Y, panel2.W, panel2.H))
						{
							isFocusPanel2 = true;
						}
					}
					if (panel2 != null)
					{
						if (isFocusPanel2)
						{
							panel2.updateKey();
						}
						else
						{
							panel.updateKey();
						}
					}
					else
					{
						panel.updateKey();
					}
					if (panel.chatTField != null && panel.chatTField.isShow)
					{
						panel.chatTFUpdateKey();
					}
					else if (panel2 != null && panel2.chatTField != null && panel2.chatTField.isShow)
					{
						panel2.chatTFUpdateKey();
					}
					else if ((isPointer(panel.X, panel.Y, panel.W, panel.H) && panel2 != null) || panel2 == null)
					{
						panel.updateKey();
					}
					else if (panel2 != null && panel2.isShow && isPointer(panel2.X, panel2.Y, panel2.W, panel2.H))
					{
						panel2.updateKey();
					}
					if (isPointer(panel.X + panel.W, panel.Y, w - panel.W * 2, panel.H) && isPointerJustRelease && panel.isDoneCombine)
					{
						panel.hide();
					}
				}
				debug("E", 0);
				if (!isLoading)
				{
					currentScreen.update();
				}
				debug("F", 0);
				if (!panel.isShow && ChatPopup.serverChatPopUp == null)
				{
					currentScreen.updateKey();
				}
				Hint.update();
				SoundMn.gI().update();
			}
			debug("Ix", 0);
			Timer.update();
			debug("Hx", 0);
			InfoDlg.update();
			debug("G", 0);
			if (resetToLoginScr)
			{
				resetToLoginScr = false;
				doResetToLoginScr(loginScr);
			}
			debug("Zzz", 0);
			if (currentScreen != serverScr && Controller.isConnectOK)
			{
				if (Controller.isMain)
				{
					ServerListScreen.testConnect = 2;
					Service.gI().setClientType();
					Service.gI().androidPack();
				}
				else
				{
					Service.gI().setClientType2();
					Service.gI().androidPack2();
				}
				Controller.isConnectOK = false;
			}
			if (Controller.isDisconnected)
			{
				if (!Controller.isMain)
				{
					if (currentScreen == serverScreen && !Service.reciveFromMainSession)
					{
						serverScreen.cancel();
					}
					if (currentScreen == loginScr && !Service.reciveFromMainSession)
					{
						onDisconnected();
					}
				}
				else
				{
					onDisconnected();
				}
				Controller.isDisconnected = false;
			}
			if (Controller.isConnectionFail)
			{
				if (!Controller.isMain)
				{
					if (currentScreen == serverScreen && ServerListScreen.isGetData && !Service.reciveFromMainSession)
					{
						ServerListScreen.testConnect = 0;
						serverScreen.cancel();
						Debug.Log("connect fail 1");
					}
					if (currentScreen == loginScr && !Service.reciveFromMainSession)
					{
						onConnectionFail();
						Debug.Log("connect fail 2");
					}
				}
				else
				{
					if (Session_ME.gI().isCompareIPConnect())
					{
						onConnectionFail();
					}
					Debug.Log("connect fail 3");
				}
				Controller.isConnectionFail = false;
			}
            AutoLoginCL.Update();
            if (Main.isResume)
			{
				Main.isResume = false;
				if (currentDialog != null && currentDialog.left != null && currentDialog.left.actionListener != null)
				{
					currentDialog.left.performAction();
				}
			}
			if (currentScreen != null && currentScreen is GameScr)
			{
				xThongBaoTranslate += dir_ * 2;
				if (xThongBaoTranslate - Panel.imgNew.getWidth() <= 60)
				{
					dir_ = 0;
					tickWaitThongBao++;
					if (tickWaitThongBao > 150)
					{
						tickWaitThongBao = 0;
						thongBaoTest = null;
					}
				}
			}
			if (currentScreen != null && currentScreen.Equals(GameScr.gI()))
			{
				if (GameScr.info1 != null)
				{
					GameScr.info1.update();
				}
				if (GameScr.info2 != null)
				{
					GameScr.info2.update();
				}
			}
			isPointerSelect = false;
		}
		catch (Exception)
		{
		}
	}

	public void onDisconnected()
	{
		if (Controller.isConnectionFail)
		{
			Controller.isConnectionFail = false;
		}
		isResume = true;
		Session_ME.gI().clearSendingMessage();
		Session_ME2.gI().clearSendingMessage();
		Session_ME.gI().close();
		Session_ME2.gI().close();
		if (Controller.isLoadingData)
		{
			startOK(mResources.pls_restart_game_error, 8885, null);
			Controller.isDisconnected = false;
			return;
		}
		Debug.LogError(">>>>onDisconnected");

		if (currentScreen != serverScreen)
		{
			serverScreen.switchToMe();
			startOK(mResources.maychutathoacmatsong + " [4]", 8884, null);
		}
		else
		{
			endDlg();
		}
		Char.isLoadingMap = false;
		if (Controller.isMain)
		{
			ServerListScreen.testConnect = 0;
		}
		mSystem.endKey();
    }

	public void onConnectionFail()
	{
		if (currentScreen.Equals(SplashScr.instance))
		{
			if (ServerListScreen.hasConnected != null)
			{
				ServerListScreen.getServerList(ServerListScreen.linkDefault);
				if (!ServerListScreen.hasConnected[0])
				{
					ServerListScreen.hasConnected[0] = true;
					ServerListScreen.ipSelect = 0;
					GameMidlet.IP = ServerListScreen.address[ServerListScreen.ipSelect];
					Rms.saveRMSInt("svselect", ServerListScreen.ipSelect);
					connect();
				}
				else if (!ServerListScreen.hasConnected[2])
				{
					ServerListScreen.hasConnected[2] = true;
					ServerListScreen.ipSelect = 2;
					GameMidlet.IP = ServerListScreen.address[ServerListScreen.ipSelect];
					Rms.saveRMSInt("svselect", ServerListScreen.ipSelect);
					connect();
				}
				else
				{
					startOK(mResources.pls_restart_game_error, 8885, null);
				}
			}
			else
			{
				startOK(mResources.pls_restart_game_error, 8885, null);
			}
			return;
		}
		Session_ME.gI().clearSendingMessage();
		Session_ME2.gI().clearSendingMessage();
		ServerListScreen.isWait = false;
		if (Controller.isLoadingData)
		{
			startOK(mResources.pls_restart_game_error, 8885, null);
			Controller.isConnectionFail = false;
			return;
		}
		isResume = true;
		LoginScr.isContinueToLogin = false;
		if (loginScr != null)
		{
			instance.resetToLoginScrz();
		}
		else
		{
			loginScr = new LoginScr();
		}
		LoginScr.serverName = ServerListScreen.nameServer[ServerListScreen.ipSelect];
		if (currentScreen != serverScreen)
		{
			ServerListScreen.countDieConnect = 0;
		}
		else
		{
			endDlg();
			ServerListScreen.loadScreen = true;
			serverScreen.switchToMe();
		}
		Char.isLoadingMap = false;
		if (Controller.isMain)
		{
			ServerListScreen.testConnect = 0;
		}
		mSystem.endKey();
	}

	public static bool isWaiting()
	{
		if (!InfoDlg.isShow && (msgdlg == null || !msgdlg.info.Equals(mResources.PLEASEWAIT)) && !Char.isLoadingMap)
		{
			return LoginScr.isContinueToLogin;
		}
		return true;
	}

	public static void connect()
	{
		if (!Session_ME.gI().isConnected())
		{
			Session_ME.gI().connect(GameMidlet.IP, GameMidlet.PORT);
		}
	}

	public static void connect2()
	{
		if (!Session_ME2.gI().isConnected())
		{
			Res.outz("IP2= " + GameMidlet.IP2 + " PORT 2= " + GameMidlet.PORT2);
			Session_ME2.gI().connect(GameMidlet.IP2, GameMidlet.PORT2);
		}
	}

	public static void resetTrans(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		g.setClip(0, 0, w, h);
	}

	public static void resetTransGameScr(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		g.translate(0, 0);
		g.setClip(0, 0, w, h);
		g.translate(-GameScr.cmx, -GameScr.cmy);
	}

	public void initGameCanvas()
	{
		debug("SP2i1", 0);
		w = MotherCanvas.instance.getWidthz();
		h = MotherCanvas.instance.getHeightz();
		debug("SP2i2", 0);
		hw = w / 2;
		hh = h / 2;
		wd3 = w / 3;
		hd3 = h / 3;
		w2d3 = 2 * w / 3;
		h2d3 = 2 * h / 3;
		w3d4 = 3 * w / 4;
		h3d4 = 3 * h / 4;
		wd6 = w / 6;
		hd6 = h / 6;
		debug("SP2i3", 0);
		mScreen.initPos();
		debug("SP2i4", 0);
		debug("SP2i5", 0);
		inputDlg = new InputDlg();
		debug("SP2i6", 0);
		listPoint = new MyVector();
		debug("SP2i7", 0);
	}

	public void start()
	{
	}

	public int getWidth()
	{
		return (int)ScaleGUI.WIDTH;
	}

	public int getHeight()
	{
		return (int)ScaleGUI.HEIGHT;
	}

	public static void debug(string s, int type)
	{
	}

	public void doResetToLoginScr(mScreen screen)
	{
		try
		{
			SoundMn.gI().stopAll();
			LoginScr.isContinueToLogin = false;
			TileMap.lastType = (TileMap.bgType = 0);
			Char.clearMyChar();
			GameScr.clearGameScr();
			GameScr.resetAllvector();
			InfoDlg.hide();
			GameScr.info1.hide();
			GameScr.info2.hide();
			GameScr.info2.cmdChat = null;
			Hint.isShow = false;
			ChatPopup.currChatPopup = null;
			Controller.isStopReadMessage = false;
			GameScr.loadCamera(fullmScreen: true, -1, -1);
			GameScr.cmx = 100;
			panel.currentTabIndex = 0;
			panel.selected = (isTouch ? (-1) : 0);
			panel.init();
			panel2 = null;
			GameScr.isPaint = true;
			ClanMessage.vMessage.removeAllElements();
			GameScr.textTime.removeAllElements();
			GameScr.vClan.removeAllElements();
			GameScr.vFriend.removeAllElements();
			GameScr.vEnemies.removeAllElements();
			TileMap.vCurrItem.removeAllElements();
			BackgroudEffect.vBgEffect.removeAllElements();
			EffecMn.vEff.removeAllElements();
			Effect.newEff.removeAllElements();
			menu.showMenu = false;
			panel.vItemCombine.removeAllElements();
			panel.isShow = false;
			if (panel.tabIcon != null)
			{
				panel.tabIcon.isShow = false;
			}
			if (mGraphics.zoomLevel == 1)
			{
				SmallImage.clearHastable();
			}
			Session_ME.gI().close();
			Session_ME2.gI().close();
		}
		catch (Exception ex)
		{
			Cout.println("Loi tai doResetToLoginScr " + ex.ToString());
		}
		ServerListScreen.testConnect = -1;
		ServerListScreen.loadScreen = true;
		if (ServerListScreen.ipSelect == -1)
		{
			serverScr.switchToMe();
			return;
		}
		if (serverScreen == null)
		{
			serverScreen = new ServerListScreen();
		}
		serverScreen.switchToMe();
	}

	public static void showErrorForm(int type, string moreInfo)
	{
	}

	public static void paintCloud(mGraphics g)
	{
	}

	public static void updateBG()
	{
	}

	public static void fillRect(mGraphics g, int color, int x, int y, int w, int h, int detalY)
	{
		g.setColor(color);
		int num = GameScr.cmy;
		if (num > h)
		{
			num = h;
		}
		g.fillRect(x, y - ((detalY != 0) ? (num >> detalY) : 0), w, h + ((detalY != 0) ? (num >> detalY) : 0));
	}

	public static void paintBackgroundtLayer(mGraphics g, int layer, int deltaY, int color1, int color2)
	{
		try
		{
			int num = layer - 1;
			if (num == imgBG.Length - 1 && (GameScr.gI().isRongThanXuatHien || GameScr.gI().isFireWorks))
			{
				g.setColor(GameScr.gI().mautroi);
				g.fillRect(0, 0, w, h);
				if (typeBg == 2 || typeBg == 4 || typeBg == 7)
				{
					drawSun1(g);
					drawSun2(g);
				}
				if (GameScr.gI().isFireWorks && !lowGraphic)
				{
					FireWorkEff.paint(g);
				}
			}
			else
			{
				if (imgBG == null || imgBG[num] == null)
				{
					return;
				}
				if (moveX[num] != 0)
				{
					moveX[num] += moveXSpeed[num];
				}
				int cmy = GameScr.cmy;
				if (cmy > h)
				{
					cmy = h;
				}
				if (layerSpeed[num] != 0)
				{
					for (int i = -((GameScr.cmx + moveX[num] >> layerSpeed[num]) % bgW[num]); i < GameScr.gW; i += bgW[num])
					{
						g.drawImage(imgBG[num], i, yb[num] - ((deltaY > 0) ? (cmy >> deltaY) : 0), 0);
					}
				}
				else
				{
					for (int j = 0; j < GameScr.gW; j += bgW[num])
					{
						g.drawImage(imgBG[num], j, yb[num] - ((deltaY > 0) ? (cmy >> deltaY) : 0), 0);
					}
				}
				if (color1 != -1)
				{
					if (num == nBg - 1)
					{
						fillRect(g, color1, 0, -(cmy >> deltaY), GameScr.gW, yb[num], deltaY);
					}
					else
					{
						fillRect(g, color1, 0, yb[num - 1] + bgH[num - 1], GameScr.gW, yb[num] - (yb[num - 1] + bgH[num - 1]), deltaY);
					}
				}
				if (color2 != -1)
				{
					if (num == 0)
					{
						fillRect(g, color2, 0, yb[num] + bgH[num], GameScr.gW, GameScr.gH - (yb[num] + bgH[num]), deltaY);
					}
					else
					{
						fillRect(g, color2, 0, yb[num] + bgH[num], GameScr.gW, yb[num - 1] - (yb[num] + bgH[num]) + 80, deltaY);
					}
				}
				if (currentScreen == GameScr.instance)
				{
					if (layer == 1 && typeBg == 11)
					{
						g.drawImage(imgSun2, -(GameScr.cmx >> layerSpeed[0]) + 400, yb[0] + 30 - (cmy >> 2), StaticObj.BOTTOM_HCENTER);
					}
					if (layer == 1 && typeBg == 13)
					{
						g.drawImage(imgBG[1], -(GameScr.cmx >> layerSpeed[0]) + TileMap.tmw * 24 / 4, yb[0] - (cmy >> 3) + 30, 0);
						g.drawRegion(imgBG[1], 0, 0, bgW[1], bgH[1], 2, -(GameScr.cmx >> layerSpeed[0]) + TileMap.tmw * 24 / 4 + bgW[1], yb[0] - (cmy >> 3) + 30, 0);
					}
					if (layer == 3 && TileMap.mapID == 1)
					{
						for (int k = 0; k < TileMap.pxh / mGraphics.getImageHeight(imgCaycot); k++)
						{
							g.drawImage(imgCaycot, -(GameScr.cmx >> layerSpeed[2]) + 300, k * mGraphics.getImageHeight(imgCaycot) - (cmy >> 3), 0);
						}
					}
				}
				int x = -(GameScr.cmx + moveX[num] >> layerSpeed[num]);
				EffecMn.paintBackGroundUnderLayer(g, x, yb[num] + bgH[num] - (cmy >> deltaY), num);
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi ham paint bground: " + ex.ToString());
		}
	}

	public static void drawSun1(mGraphics g)
	{
		if (imgSun != null)
		{
			g.drawImage(imgSun, sunX, sunY, 0);
		}
		if (!isBoltEff)
		{
			return;
		}
		if (gameTick % 200 == 0)
		{
			boltActive = true;
		}
		if (boltActive)
		{
			tBolt++;
			if (tBolt == 10)
			{
				tBolt = 0;
				boltActive = false;
			}
			if (tBolt % 2 == 0)
			{
				g.setColor(16777215);
				g.fillRect(0, 0, w, h);
			}
		}
	}

	public static void drawSun2(mGraphics g)
	{
		if (imgSun2 != null)
		{
			g.drawImage(imgSun2, sunX2, sunY2, 0);
		}
	}

	public static bool isHDVersion()
	{
		return mGraphics.zoomLevel > 1;
	}

	public static void paint_ios_bg(mGraphics g)
	{
		if (mSystem.clientType != 5)
		{
			return;
		}
		if (imgBgIOS != null)
		{
			g.setColor(0);
			g.fillRect(0, 0, w, h);
			for (int i = 0; i < 3; i++)
			{
				g.drawImage(imgBgIOS, imgBgIOS.getWidth() * i, h / 2, mGraphics.VCENTER | mGraphics.HCENTER);
			}
		}
		else
		{
			imgBgIOS = mSystem.loadImage("/bg/bg_ios_" + ((TileMap.bgID % 2 != 0) ? 1 : 2) + ".png");
		}
	}

	public static void paintBGGameScr(mGraphics g)
	{
		if (GameScr.backgroundGame != null && GraphicsManagement.HienThiBackground)
		{
			g.drawImage(GameScr.backgroundGame, w / 2, h / 2, 3);
			return;
		}
        if (!isLoadBGok)
        {
            g.setColor(0);
            g.fillRect(0, 0, w, h);
        }
        if (Char.isLoadingMap)
        {
            return;
        }
        int gW = GameScr.gW;
        int gH = GameScr.gH;
        g.translate(-g.getTranslateX(), -g.getTranslateY());
        g.setColor(0);
        g.fillRect(0, 0, w, h);
        try
        {
            if (paintBG)
            {
                if (currentScreen == GameScr.gI())
                {
                    if (TileMap.mapID != 172 && (TileMap.mapID == 137 || TileMap.mapID == 115 || TileMap.mapID == 117 || TileMap.mapID == 118 || TileMap.mapID == 120 || TileMap.isMapDouble))
                    {
                        g.setColor(0);
                        g.fillRect(0, 0, w, h);
                        return;
                    }
                    if (TileMap.mapID == 138)
                    {
                        g.setColor(6776679);
                        g.fillRect(0, 0, w, h);
                        return;
                    }
                }
                if (typeBg == 0)
                {
                    paintBackgroundtLayer(g, 4, 6, colorTop[3], colorBotton[3]);
                    paintBackgroundtLayer(g, 3, 4, -1, colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 3, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                }
                else if (typeBg == 1)
                {
                    paintBackgroundtLayer(g, 4, 6, -1, -1);
                    paintBackgroundtLayer(g, 3, 3, -1, -1);
                    fillRect(g, colorTop[2], 0, -(GameScr.cmy >> 5), gW, yb[2], 5);
                    fillRect(g, colorBotton[2], 0, yb[2] + bgH[2] - (GameScr.cmy >> 3), gW, 70, 3);
                    paintBackgroundtLayer(g, 2, 2, -1, -1);
                    paintBackgroundtLayer(g, 1, 1, -1, colorBotton[0]);
                }
                else if (typeBg == 2)
                {
                    paintBackgroundtLayer(g, 5, 10, colorTop[4], colorBotton[4]);
                    paintBackgroundtLayer(g, 4, 8, -1, colorTop[2]);
                    paintBackgroundtLayer(g, 3, 5, -1, colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 2, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 1, -1, colorBotton[0]);
                    paintCloud(g);
                }
                else if (typeBg == 3)
                {
                    int num = GameScr.cmy - (325 - GameScr.gH23);
                    g.translate(0, -num);
                    fillRect(g, (!GameScr.gI().isRongThanXuatHien && !GameScr.gI().isFireWorks) ? colorTop[2] : GameScr.gI().mautroi, 0, num - (GameScr.cmy >> 3), gW, yb[2] - num + (GameScr.cmy >> 3) + 100, 2);
                    paintBackgroundtLayer(g, 3, 2, -1, colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 0, -1, -1);
                    paintBackgroundtLayer(g, 1, 0, -1, colorBotton[0]);
                    g.translate(0, -g.getTranslateY());
                }
                else if (typeBg == 4)
                {
                    paintBackgroundtLayer(g, 4, 7, colorTop[3], -1);
                    paintBackgroundtLayer(g, 3, 3, -1, (!isHDVersion()) ? colorTop[1] : colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 2, colorTop[1], colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 1, -1, colorBotton[0]);
                }
                else if (typeBg == 5)
                {
                    paintBackgroundtLayer(g, 4, 15, colorTop[3], -1);
                    drawSun1(g);
                    g.translate(100, 10);
                    drawSun1(g);
                    g.translate(-100, -10);
                    drawSun2(g);
                    paintBackgroundtLayer(g, 3, 10, -1, -1);
                    paintBackgroundtLayer(g, 2, 6, -1, -1);
                    paintBackgroundtLayer(g, 1, 4, -1, -1);
                    g.translate(0, 27);
                    paintBackgroundtLayer(g, 1, 2, -1, -1);
                    g.translate(0, 20);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                    g.translate(-g.getTranslateX(), -g.getTranslateY());
                }
                else if (typeBg == 6)
                {
                    paintBackgroundtLayer(g, 5, 10, colorTop[4], colorBotton[4]);
                    drawSun1(g);
                    drawSun2(g);
                    g.translate(60, 40);
                    drawSun2(g);
                    g.translate(-60, -40);
                    paintBackgroundtLayer(g, 4, 7, -1, colorBotton[3]);
                    BackgroudEffect.paintFarAll(g);
                    paintBackgroundtLayer(g, 3, 4, -1, -1);
                    paintBackgroundtLayer(g, 2, 3, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                }
                else if (typeBg == 7)
                {
                    paintBackgroundtLayer(g, 4, 6, colorTop[3], colorBotton[3]);
                    paintBackgroundtLayer(g, 3, 5, -1, -1);
                    paintBackgroundtLayer(g, 2, 4, -1, -1);
                    paintBackgroundtLayer(g, 1, 3, -1, colorBotton[0]);
                }
                else if (typeBg == 8)
                {
                    paintBackgroundtLayer(g, 4, 8, colorTop[3], colorBotton[3]);
                    drawSun1(g);
                    drawSun2(g);
                    paintBackgroundtLayer(g, 3, 4, -1, colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 2, -1, colorBotton[1]);
                    if (((TileMap.mapID < 92 || TileMap.mapID > 96) && TileMap.mapID != 51 && TileMap.mapID != 52) || currentScreen == loginScr)
                    {
                        paintBackgroundtLayer(g, 1, 1, -1, colorBotton[0]);
                    }
                }
                else if (typeBg == 9)
                {
                    paintBackgroundtLayer(g, 4, 8, colorTop[3], colorBotton[3]);
                    drawSun1(g);
                    drawSun2(g);
                    g.translate(-80, 20);
                    drawSun2(g);
                    g.translate(80, -20);
                    BackgroudEffect.paintFarAll(g);
                    paintBackgroundtLayer(g, 3, 5, -1, -1);
                    paintBackgroundtLayer(g, 2, 3, -1, -1);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                }
                else if (typeBg == 10)
                {
                    int num2 = GameScr.cmy - (380 - GameScr.gH23);
                    g.translate(0, -num2);
                    fillRect(g, (!GameScr.gI().isRongThanXuatHien) ? colorTop[1] : GameScr.gI().mautroi, 0, num2 - (GameScr.cmy >> 2), gW, yb[1] - num2 + (GameScr.cmy >> 2) + 100, 2);
                    paintBackgroundtLayer(g, 2, 2, -1, colorBotton[1]);
                    drawSun1(g);
                    drawSun2(g);
                    paintBackgroundtLayer(g, 1, 0, -1, -1);
                    g.translate(0, -g.getTranslateY());
                }
                else if (typeBg == 11)
                {
                    paintBackgroundtLayer(g, 3, 6, colorTop[2], colorBotton[2]);
                    drawSun1(g);
                    paintBackgroundtLayer(g, 2, 3, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                }
                else if (typeBg == 12)
                {
                    g.setColor(9161471);
                    g.fillRect(0, 0, w, h);
                    paintBackgroundtLayer(g, 3, 4, -1, 14417919);
                    paintBackgroundtLayer(g, 2, 3, -1, 14417919);
                    paintBackgroundtLayer(g, 1, 2, -1, 14417919);
                    paintCloud(g);
                }
                else if (typeBg == 13)
                {
                    g.setColor(15268088);
                    g.fillRect(0, 0, w, h);
                    paintBackgroundtLayer(g, 1, 5, -1, 15268088);
                }
                else if (typeBg == 15)
                {
                    g.setColor(2631752);
                    g.fillRect(0, 0, w, h);
                    paintBackgroundtLayer(g, 2, 3, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                }
                else if (typeBg == 16)
                {
                    paintBackgroundtLayer(g, 4, 6, colorTop[3], colorBotton[3]);
                    for (int i = 0; i < imgSunSpec.Length; i++)
                    {
                        g.drawImage(imgSunSpec[i], cloudX[i], cloudY[i], 33);
                    }
                    paintBackgroundtLayer(g, 3, 4, -1, colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 3, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                }
                else if (typeBg == 19)
                {
                    paintBackgroundtLayer(g, 5, 10, colorTop[4], colorBotton[4]);
                    paintBackgroundtLayer(g, 4, 8, -1, colorTop[2]);
                    paintBackgroundtLayer(g, 3, 5, -1, colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 2, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 1, -1, colorBotton[0]);
                    paintCloud(g);
                }
                else
                {
                    fillRect(g, colorBotton[3], 0, yb[3] + bgH[3], GameScr.gW, yb[2] + bgH[2], 6);
                    paintBackgroundtLayer(g, 4, 6, colorTop[3], colorBotton[3]);
                    drawSun1(g);
                    paintBackgroundtLayer(g, 3, 4, -1, colorBotton[2]);
                    paintBackgroundtLayer(g, 2, 3, -1, colorBotton[1]);
                    paintBackgroundtLayer(g, 1, 2, -1, colorBotton[0]);
                }
                return;
            }
            g.setColor(2315859);
            g.fillRect(0, 0, w, h);
            if (tam != null)
            {
                for (int j = -((GameScr.cmx >> 2) % mGraphics.getImageWidth(tam)); j < GameScr.gW; j += mGraphics.getImageWidth(tam))
                {
                    g.drawImage(tam, j, (GameScr.cmy >> 3) + h / 2 - 50, 0);
                }
            }
            g.setColor(5084791);
            g.fillRect(0, (GameScr.cmy >> 3) + h / 2 - 50 + mGraphics.getImageHeight(tam), gW, h);
        }
        catch (Exception)
        {
            g.setColor(0);
            g.fillRect(0, 0, w, h);
        }
    
}

	public static void resetBg()
	{
	}

	public static void getYBackground(int typeBg)
	{
		try
		{
			int gH = GameScr.gH23;
			switch (typeBg)
			{
			case 0:
				yb[0] = gH - bgH[0] + 70;
				yb[1] = yb[0] - bgH[1] + 20;
				yb[2] = yb[1] - bgH[2] + 30;
				yb[3] = yb[2] - bgH[3] + 50;
				break;
			case 1:
				yb[0] = gH - bgH[0] + 120;
				yb[1] = yb[0] - bgH[1] + 40;
				yb[2] = yb[1] - 90;
				yb[3] = yb[2] - 25;
				break;
			case 2:
				yb[0] = gH - bgH[0] + 150;
				yb[1] = yb[0] - bgH[1] - 60;
				yb[2] = yb[1] - bgH[2] - 40;
				yb[3] = yb[2] - bgH[3] - 10;
				yb[4] = yb[3] - bgH[4];
				break;
			case 3:
				yb[0] = gH - bgH[0] + 10;
				yb[1] = yb[0] + 80;
				yb[2] = yb[1] - bgH[2] - 10;
				break;
			case 4:
				yb[0] = gH - bgH[0] + 130;
				yb[1] = yb[0] - bgH[1];
				yb[2] = yb[1] - bgH[2] - 20;
				yb[3] = yb[1] - bgH[2] - 80;
				break;
			case 5:
				yb[0] = gH - bgH[0] + 40;
				yb[1] = yb[0] - bgH[1] + 10;
				yb[2] = yb[1] - bgH[2] + 15;
				yb[3] = yb[2] - bgH[3] + 50;
				break;
			case 6:
				yb[0] = gH - bgH[0] + 100;
				yb[1] = yb[0] - bgH[1] - 30;
				yb[2] = yb[1] - bgH[2] + 10;
				yb[3] = yb[2] - bgH[3] + 15;
				yb[4] = yb[3] - bgH[4] + 15;
				break;
			case 7:
				yb[0] = gH - bgH[0] + 20;
				yb[1] = yb[0] - bgH[1] + 15;
				yb[2] = yb[1] - bgH[2] + 20;
				yb[3] = yb[1] - bgH[2] - 10;
				break;
			case 8:
				yb[0] = gH - 103 + 150;
				if (TileMap.mapID == 103)
				{
					yb[0] -= 100;
				}
				yb[1] = yb[0] - bgH[1] - 10;
				yb[2] = yb[1] - bgH[2] + 40;
				yb[3] = yb[2] - bgH[3] + 10;
				break;
			case 9:
				yb[0] = gH - bgH[0] + 100;
				yb[1] = yb[0] - bgH[1] + 22;
				yb[2] = yb[1] - bgH[2] + 50;
				yb[3] = yb[2] - bgH[3];
				break;
			case 10:
				yb[0] = gH - bgH[0] - 45;
				yb[1] = yb[0] - bgH[1] - 10;
				break;
			case 11:
				yb[0] = gH - bgH[0] + 60;
				yb[1] = yb[0] - bgH[1] + 5;
				yb[2] = yb[1] - bgH[2] - 15;
				break;
			case 12:
				yb[0] = gH + 40;
				yb[1] = yb[0] - 40;
				yb[2] = yb[1] - 40;
				break;
			case 13:
				yb[0] = gH - 80;
				yb[1] = yb[0];
				break;
			case 15:
				yb[0] = gH - 20;
				yb[1] = yb[0] - 80;
				break;
			case 16:
				yb[0] = gH - bgH[0] + 75;
				yb[1] = yb[0] - bgH[1] + 50;
				yb[2] = yb[1] - bgH[2] + 50;
				yb[3] = yb[2] - bgH[3] + 90;
				break;
			case 19:
				yb[0] = gH - bgH[0] + 150;
				yb[1] = yb[0] - bgH[1] - 60;
				yb[2] = yb[1] - bgH[2] - 40;
				yb[3] = yb[2] - bgH[3] - 10;
				yb[4] = yb[3] - bgH[4];
				break;
			default:
				yb[0] = gH - bgH[0] + 75;
				yb[1] = yb[0] - bgH[1] + 50;
				yb[2] = yb[1] - bgH[2] + 50;
				yb[3] = yb[2] - bgH[3] + 90;
				break;
			}
		}
		catch (Exception)
		{
			_ = GameScr.gH23;
			for (int i = 0; i < yb.Length; i++)
			{
				yb[i] = 1;
			}
		}
	}

	public static void loadBG(int typeBG)
	{
		try
		{
			isLoadBGok = true;
			if (typeBg == 12)
			{
				BackgroudEffect.yfog = TileMap.pxh - 100;
			}
			else
			{
				BackgroudEffect.yfog = TileMap.pxh - 160;
			}
			BackgroudEffect.clearImage();
			randomRaintEff(typeBG);
			if ((TileMap.lastBgID == typeBG && TileMap.lastType == TileMap.bgType) || typeBG == -1)
			{
				return;
			}
			transY = 12;
			TileMap.lastBgID = (sbyte)typeBG;
			TileMap.lastType = (sbyte)TileMap.bgType;
			layerSpeed = new int[5] { 1, 2, 3, 7, 8 };
			moveX = new int[5];
			moveXSpeed = new int[5];
			typeBg = typeBG;
			isBoltEff = false;
			GameScr.firstY = GameScr.cmy;
			imgBG = null;
			imgCloud = null;
			imgSun = null;
			imgCaycot = null;
			GameScr.firstY = -1;
			switch (typeBg)
			{
			case 0:
				imgCaycot = loadImageRMS("/bg/caycot.png");
				layerSpeed = new int[4] { 1, 3, 5, 7 };
				nBg = 4;
				if (TileMap.bgType == 2)
				{
					transY = 8;
				}
				break;
			case 1:
				transY = 7;
				nBg = 4;
				break;
			case 2:
				moveX = new int[5] { 0, 0, 1, 0, 0 };
				moveXSpeed = new int[5] { 0, 0, 2, 0, 0 };
				nBg = 5;
				break;
			case 3:
				nBg = 3;
				break;
			case 4:
				BackgroudEffect.addEffect(3);
				moveX = new int[5] { 0, 1, 0, 0, 0 };
				moveXSpeed = new int[5] { 0, 1, 0, 0, 0 };
				nBg = 4;
				break;
			case 5:
				nBg = 4;
				break;
			case 6:
				moveX = new int[5] { 1, 0, 0, 0, 0 };
				moveXSpeed = new int[5] { 2, 0, 0, 0, 0 };
				nBg = 5;
				break;
			case 7:
				nBg = 4;
				break;
			case 8:
				transY = 8;
				nBg = 4;
				break;
			case 9:
				BackgroudEffect.addEffect(9);
				nBg = 4;
				break;
			case 10:
				nBg = 2;
				break;
			case 11:
				transY = 7;
				layerSpeed[2] = 0;
				nBg = 3;
				break;
			case 12:
				moveX = new int[5] { 1, 1, 0, 0, 0 };
				moveXSpeed = new int[5] { 2, 1, 0, 0, 0 };
				nBg = 3;
				break;
			case 13:
				nBg = 2;
				break;
			case 15:
				Res.outz("HELL");
				nBg = 2;
				break;
			case 16:
				layerSpeed = new int[4] { 1, 3, 5, 7 };
				nBg = 4;
				break;
			case 19:
				moveX = new int[5] { 0, 2, 1, 0, 0 };
				moveXSpeed = new int[5] { 0, 2, 1, 0, 0 };
				nBg = 5;
				break;
			default:
				layerSpeed = new int[4] { 1, 3, 5, 7 };
				nBg = 4;
				break;
			}
			if (typeBG <= 16)
			{
				skyColor = StaticObj.SKYCOLOR[typeBg];
			}
			else
			{
				try
				{
					string path = "/bg/b" + typeBg + 3 + ".png";
					if (TileMap.bgType != 0)
					{
						path = "/bg/b" + typeBg + 3 + "-" + TileMap.bgType + ".png";
					}
					int[] data = new int[1];
					Image image = loadImageRMS(path);
					image.getRGB(ref data, 0, 1, mGraphics.getRealImageWidth(image) / 2, 0, 1, 1);
					skyColor = data[0];
				}
				catch (Exception)
				{
					skyColor = StaticObj.SKYCOLOR[StaticObj.SKYCOLOR.Length - 1];
				}
			}
			colorTop = new int[StaticObj.SKYCOLOR.Length];
			colorBotton = new int[StaticObj.SKYCOLOR.Length];
			for (int i = 0; i < StaticObj.SKYCOLOR.Length; i++)
			{
				colorTop[i] = StaticObj.SKYCOLOR[i];
				colorBotton[i] = StaticObj.SKYCOLOR[i];
			}
			if (lowGraphic)
			{
				tam = loadImageRMS("/bg/b63.png");
				return;
			}
			imgBG = new Image[nBg];
			bgW = new int[nBg];
			bgH = new int[nBg];
			colorBotton = new int[nBg];
			colorTop = new int[nBg];
			if (TileMap.bgType == 100)
			{
				imgBG[0] = loadImageRMS("/bg/b100.png");
				imgBG[1] = loadImageRMS("/bg/b100.png");
				imgBG[2] = loadImageRMS("/bg/b82-1.png");
				imgBG[3] = loadImageRMS("/bg/b93.png");
				for (int j = 0; j < nBg; j++)
				{
					if (imgBG[j] != null)
					{
						int[] data2 = new int[1];
						imgBG[j].getRGB(ref data2, 0, 1, mGraphics.getRealImageWidth(imgBG[j]) / 2, 0, 1, 1);
						colorTop[j] = data2[0];
						data2 = new int[1];
						imgBG[j].getRGB(ref data2, 0, 1, mGraphics.getRealImageWidth(imgBG[j]) / 2, mGraphics.getRealImageHeight(imgBG[j]) - 1, 1, 1);
						colorBotton[j] = data2[0];
						bgW[j] = mGraphics.getImageWidth(imgBG[j]);
						bgH[j] = mGraphics.getImageHeight(imgBG[j]);
					}
					else if (nBg > 1)
					{
						imgBG[j] = loadImageRMS("/bg/b" + typeBg + "0.png");
						bgW[j] = mGraphics.getImageWidth(imgBG[j]);
						bgH[j] = mGraphics.getImageHeight(imgBG[j]);
					}
				}
			}
			else
			{
				for (int k = 0; k < nBg; k++)
				{
					string path2 = "/bg/b" + typeBg + k + ".png";
					if (TileMap.bgType != 0)
					{
						path2 = "/bg/b" + typeBg + k + "-" + TileMap.bgType + ".png";
					}
					imgBG[k] = loadImageRMS(path2);
					if (imgBG[k] != null)
					{
						int[] data3 = new int[1];
						imgBG[k].getRGB(ref data3, 0, 1, mGraphics.getRealImageWidth(imgBG[k]) / 2, 0, 1, 1);
						colorTop[k] = data3[0];
						data3 = new int[1];
						imgBG[k].getRGB(ref data3, 0, 1, mGraphics.getRealImageWidth(imgBG[k]) / 2, mGraphics.getRealImageHeight(imgBG[k]) - 1, 1, 1);
						colorBotton[k] = data3[0];
						bgW[k] = mGraphics.getImageWidth(imgBG[k]);
						bgH[k] = mGraphics.getImageHeight(imgBG[k]);
					}
					else if (nBg > 1)
					{
						imgBG[k] = loadImageRMS("/bg/b" + typeBg + "0.png");
						bgW[k] = mGraphics.getImageWidth(imgBG[k]);
						bgH[k] = mGraphics.getImageHeight(imgBG[k]);
					}
				}
			}
			getYBackground(typeBg);
			cloudX = new int[5]
			{
				GameScr.gW / 2 - 40,
				GameScr.gW / 2 + 40,
				GameScr.gW / 2 - 100,
				GameScr.gW / 2 - 80,
				GameScr.gW / 2 - 120
			};
			cloudY = new int[5] { 130, 100, 150, 140, 80 };
			imgSunSpec = null;
			if (typeBg != 0)
			{
				if (typeBg == 2)
				{
					imgSun = loadImageRMS("/bg/sun0.png");
					sunX = GameScr.gW / 2 + 50;
					sunY = yb[4] - 40;
					TileMap.imgWaterflow = loadImageRMS("/tWater/wts");
				}
				else if (typeBg == 19)
				{
					TileMap.imgWaterflow = loadImageRMS("/tWater/water_flow_32");
				}
				else if (typeBg == 4)
				{
					imgSun = loadImageRMS("/bg/sun2.png");
					sunX = GameScr.gW / 2 + 30;
					sunY = yb[3];
				}
				else if (typeBg == 7)
				{
					imgSun = loadImageRMS("/bg/sun3" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					imgSun2 = loadImageRMS("/bg/sun4" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					sunX = GameScr.gW - GameScr.gW / 3;
					sunY = yb[3] - 80;
					sunX2 = sunX - 100;
					sunY2 = yb[3] - 30;
				}
				else if (typeBg == 6)
				{
					imgSun = loadImageRMS("/bg/sun5" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					imgSun2 = loadImageRMS("/bg/sun6" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					sunX = GameScr.gW - GameScr.gW / 3;
					sunY = yb[4];
					sunX2 = sunX - 100;
					sunY2 = yb[4] + 20;
				}
				else if (typeBG == 5)
				{
					imgSun = loadImageRMS("/bg/sun8" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					imgSun2 = loadImageRMS("/bg/sun7" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					sunX = GameScr.gW / 2 - 50;
					sunY = yb[3] + 20;
					sunX2 = GameScr.gW / 2 + 20;
					sunY2 = yb[3] - 30;
				}
				else if (typeBg == 8 && TileMap.mapID < 90)
				{
					imgSun = loadImageRMS("/bg/sun9" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					imgSun2 = loadImageRMS("/bg/sun10" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
					sunX = GameScr.gW / 2 - 30;
					sunY = yb[3] + 60;
					sunX2 = GameScr.gW / 2 + 20;
					sunY2 = yb[3] + 10;
				}
				else
				{
					switch (typeBG)
					{
					case 9:
						imgSun = loadImageRMS("/bg/sun11" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
						imgSun2 = loadImageRMS("/bg/sun12" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
						sunX = GameScr.gW - GameScr.gW / 3;
						sunY = yb[4] + 20;
						sunX2 = sunX - 80;
						sunY2 = yb[4] + 40;
						break;
					case 10:
						imgSun = loadImageRMS("/bg/sun13" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
						imgSun2 = loadImageRMS("/bg/sun14" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
						sunX = GameScr.gW - GameScr.gW / 3;
						sunY = yb[1] - 30;
						sunX2 = sunX - 80;
						sunY2 = yb[1];
						break;
					case 11:
						imgSun = loadImageRMS("/bg/sun15" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
						imgSun2 = loadImageRMS("/bg/b113" + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
						sunX = GameScr.gW / 2 - 30;
						sunY = yb[2] - 30;
						break;
					case 12:
						cloudY = new int[5] { 200, 170, 220, 150, 250 };
						break;
					case 16:
					{
						cloudX = new int[7] { 90, 170, 250, 320, 400, 450, 500 };
						cloudY = new int[7]
						{
							yb[2] + 5,
							yb[2] - 20,
							yb[2] - 50,
							yb[2] - 30,
							yb[2] - 50,
							yb[2],
							yb[2] - 40
						};
						imgSunSpec = new Image[7];
						for (int l = 0; l < imgSunSpec.Length; l++)
						{
							int num = 161;
							if (l == 0 || l == 2 || l == 3 || l == 2 || l == 6)
							{
								num = 160;
							}
							imgSunSpec[l] = loadImageRMS("/bg/sun" + num + ".png");
						}
						break;
					}
					case 19:
						moveX = new int[5] { 0, 2, 1, 0, 0 };
						moveXSpeed = new int[5] { 0, 2, 1, 0, 0 };
						nBg = 5;
						break;
					default:
						imgCloud = null;
						imgSun = null;
						imgSun2 = null;
						imgSun = loadImageRMS("/bg/sun" + typeBG + ((TileMap.bgType != 0) ? ("-" + TileMap.bgType) : string.Empty) + ".png");
						if (loadImageRMS("/tWater/water_flow_" + typeBG) != null)
						{
							TileMap.imgWaterflow = loadImageRMS("/tWater/water_flow_" + typeBG);
						}
						sunX = GameScr.gW - GameScr.gW / 3;
						sunY = yb[2] - 30;
						break;
					}
				}
			}
			paintBG = false;
			if (!paintBG)
			{
				paintBG = true;
			}
		}
		catch (Exception)
		{
			isLoadBGok = false;
		}
	}

	private static void randomRaintEff(int typeBG)
	{
	}

	public void keyPressedz(int keyCode)
	{
		lastTimePress = mSystem.currentTimeMillis();
		if ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 122) || keyCode == 10 || keyCode == 8 || keyCode == 13 || keyCode == 32 || keyCode == 31)
		{
			keyAsciiPress = keyCode;
		}
		mapKeyPress(keyCode);
	}

	public void mapKeyPress(int keyCode)
	{
		if (currentDialog != null)
		{
			currentDialog.keyPress(keyCode);
			keyAsciiPress = 0;
			return;
		}
		currentScreen.keyPress(keyCode);
		switch (keyCode)
		{
		case -26:
			keyHold[16] = true;
			keyPressed[16] = true;
			break;
		case -8:
			keyHold[14] = true;
			keyPressed[14] = true;
			break;
		case -4:
			if ((currentScreen is GameScr || currentScreen is CrackBallScr) && Char.myCharz().isAttack)
			{
				clearKeyHold();
				clearKeyPressed();
			}
			else
			{
				keyHold[24] = true;
				keyPressed[24] = true;
			}
			break;
		case -3:
			if ((currentScreen is GameScr || currentScreen is CrackBallScr) && Char.myCharz().isAttack)
			{
				clearKeyHold();
				clearKeyPressed();
			}
			else
			{
				keyHold[23] = true;
				keyPressed[23] = true;
			}
			break;
		case -21:
		case -6:
			keyHold[12] = true;
			keyPressed[12] = true;
			break;
		case 35:
			keyHold[11] = true;
			keyPressed[11] = true;
			break;
		case 42:
			keyHold[10] = true;
			keyPressed[10] = true;
			break;
		case 48:
			keyHold[0] = true;
			keyPressed[0] = true;
			break;
		case 49:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[1] = true;
				keyPressed[1] = true;
			}
			break;
		case 50:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[2] = true;
				keyPressed[2] = true;
			}
			break;
		case 51:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[3] = true;
				keyPressed[3] = true;
			}
			break;
		case 52:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[4] = true;
				keyPressed[4] = true;
			}
			break;
		case 53:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[5] = true;
				keyPressed[5] = true;
			}
			break;
		case 54:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[6] = true;
				keyPressed[6] = true;
			}
			break;
		case 55:
			keyHold[7] = true;
			keyPressed[7] = true;
			break;
		case 56:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[8] = true;
				keyPressed[8] = true;
			}
			break;
		case 57:
			keyHold[9] = true;
			keyPressed[9] = true;
			break;
		case 113:
			keyHold[17] = true;
			keyPressed[17] = true;
			break;
		case -5:
		case 10:
			if ((currentScreen is GameScr || currentScreen is CrackBallScr) && Char.myCharz().isAttack)
			{
				clearKeyHold();
				clearKeyPressed();
				break;
			}
			keyHold[25] = true;
			keyPressed[25] = true;
			keyHold[15] = true;
			keyPressed[15] = true;
			break;
		case -38:
		case -1:
			if ((currentScreen is GameScr || currentScreen is CrackBallScr) && Char.myCharz().isAttack)
			{
				clearKeyHold();
				clearKeyPressed();
			}
			else
			{
				keyHold[21] = true;
				keyPressed[21] = true;
			}
			break;
		case -39:
		case -2:
			if ((currentScreen is GameScr || currentScreen is CrackBallScr) && Char.myCharz().isAttack)
			{
				clearKeyHold();
				clearKeyPressed();
			}
			else
			{
				keyHold[22] = true;
				keyPressed[22] = true;
			}
			break;
		case -22:
		case -7:
			keyHold[13] = true;
			keyPressed[13] = true;
			break;
		}
	}

	public void keyReleasedz(int keyCode)
	{
		keyAsciiPress = 0;
		mapKeyRelease(keyCode);
	}

	public void mapKeyRelease(int keyCode)
	{
		switch (keyCode)
		{
		case -8:
			keyHold[14] = false;
			break;
		case -4:
			keyHold[24] = false;
			break;
		case -3:
			keyHold[23] = false;
			break;
		case -21:
		case -6:
			keyHold[12] = false;
			keyReleased[12] = true;
			break;
		case 35:
			keyHold[11] = false;
			keyReleased[11] = true;
			break;
		case 42:
			keyHold[10] = false;
			keyReleased[10] = true;
			break;
		case 48:
			keyHold[0] = false;
			keyReleased[0] = true;
			break;
		case 49:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[1] = false;
				keyReleased[1] = true;
			}
			break;
		case 50:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[2] = false;
				keyReleased[2] = true;
			}
			break;
		case 51:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[3] = false;
				keyReleased[3] = true;
			}
			break;
		case 52:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[4] = false;
				keyReleased[4] = true;
			}
			break;
		case 53:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[5] = false;
				keyReleased[5] = true;
			}
			break;
		case 54:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[6] = false;
				keyReleased[6] = true;
			}
			break;
		case 55:
			keyHold[7] = false;
			keyReleased[7] = true;
			break;
		case 56:
			if (currentScreen == CrackBallScr.instance || (currentScreen == GameScr.instance && isMoveNumberPad && !ChatTextField.gI().isShow))
			{
				keyHold[8] = false;
				keyReleased[8] = true;
			}
			break;
		case 57:
			keyHold[9] = false;
			keyReleased[9] = true;
			break;
		case 113:
			keyHold[17] = false;
			keyReleased[17] = true;
			break;
		case -5:
		case 10:
			keyHold[25] = false;
			keyReleased[25] = true;
			keyHold[15] = true;
			keyPressed[15] = true;
			break;
		case -26:
			keyHold[16] = false;
			break;
		case -38:
		case -1:
			keyHold[21] = false;
			break;
		case -39:
		case -2:
			keyHold[22] = false;
			break;
		case -22:
		case -7:
			keyHold[13] = false;
			keyReleased[13] = true;
			break;
		}
	}

	public void pointerMouse(int x, int y)
	{
		pxMouse = x;
		pyMouse = y;
	}

	public void scrollMouse(int a)
	{
		pXYScrollMouse = a;
		if (panel != null && panel.isShow)
		{
			panel.updateScroolMouse(a);
		}
	}

	public void pointerDragged(int x, int y)
	{
		isPointerSelect = false;
		if (Res.abs(x - pxLast) >= 10 || Res.abs(y - pyLast) >= 10)
		{
			isPointerClick = false;
			isPointerDown = true;
			isPointerMove = true;
		}
		px = x;
		py = y;
		curPos++;
		if (curPos > 3)
		{
			curPos = 0;
		}
		arrPos[curPos] = new Position(x, y);
	}

	public static bool isHoldPress()
	{
		return mSystem.currentTimeMillis() - lastTimePress >= 800;
	}

	public void pointerPressed(int x, int y)
	{
		isPointerSelect = false;
		isPointerJustRelease = false;
		isPointerJustDown = true;
		isPointerDown = true;
		isPointerClick = false;
		isPointerMove = false;
		lastTimePress = mSystem.currentTimeMillis();
		pxFirst = x;
		pyFirst = y;
		pxLast = x;
		pyLast = y;
		px = x;
		py = y;
	}

	public void pointerReleased(int x, int y)
	{
		if (!isPointerMove)
		{
			isPointerSelect = true;
		}
		isPointerDown = false;
		isPointerMove = false;
		isPointerJustRelease = true;
		isPointerClick = true;
		mScreen.keyTouch = -1;
		px = x;
		py = y;
	}

	public static bool isPointerHoldIn(int x, int y, int w, int h)
	{
		if ((isPointerDown || isPointerJustRelease) && px >= x && px <= x + w && py >= y)
		{
			return py <= y + h;
		}
		return false;
	}

	public static bool isPointSelect(int x, int y, int w, int h)
	{
		if (isPointerSelect && px >= x && px <= x + w && py >= y)
		{
			return py <= y + h;
		}
		return false;
	}

	public static bool isMouseFocus(int x, int y, int w, int h)
	{
		if (pxMouse >= x && pxMouse <= x + w && pyMouse >= y)
		{
			return pyMouse <= y + h;
		}
		return false;
	}

	public static void clearKeyPressed()
	{
		for (int i = 0; i < keyPressed.Length; i++)
		{
			keyPressed[i] = false;
		}
		isPointerJustRelease = false;
	}

	public static void clearKeyHold()
	{
		for (int i = 0; i < keyHold.Length; i++)
		{
			keyHold[i] = false;
		}
	}

	public static void checkBackButton()
	{
		if (ChatPopup.serverChatPopUp == null && ChatPopup.currChatPopup == null)
		{
			startYesNoDlg(mResources.DOYOUWANTEXIT, new Command(mResources.YES, instance, 8885, null), new Command(mResources.NO, instance, 8882, null));
		}
	}

    public void paintChangeMap(mGraphics g)
    {
        string empty = string.Empty;
        resetTrans(g);

        g.setColor(0);
        g.fillRect(0, 0, w, h);

        int y = h / 2 + 48;

        if (Session_ME.gI().useProxy)
        {
            mFont.tahoma_7b_green.drawString(
                g,
                "Proxy Đang hoạt động",
                w / 2,
                y,
                mFont.CENTER
            );
            y += 12;
        }
        if (!string.IsNullOrEmpty(Session_ME.ConnectedIP))
        {
            mFont.tahoma_7b_white.drawString(
                g,
                "IP: " + Session_ME.ConnectedIP,
                w / 2,
                y,
                mFont.CENTER
            );
        }

        g.drawImage(MainMod.logoServerListScreen, w / 2, h / 2 - 8, StaticObj.BOTTOM_HCENTER);
        paintShukiren(hw, h / 2 + 24, g);

        mFont.tahoma_7b_white.drawString(
            g,
            mResources.PLEASEWAIT +
            ((LoginScr.timeLogin <= 0) ? empty : (" " + LoginScr.timeLogin + "s")),
            w / 2,
            h / 2,
            mFont.CENTER
        );
    }


    public void paint(mGraphics gx)
	{
		try
		{
			debugPaint.removeAllElements();
			debug("PA", 1);
			if (currentScreen != null)
			{
				currentScreen.paint(g);
			}
			debug("PB", 1);
			g.translate(-g.getTranslateX(), -g.getTranslateY());
			g.setClip(0, 0, w, h);
			if (panel.isShow)
			{
				panel.paint(g);
				if (panel2 != null && panel2.isShow)
				{
					panel2.paint(g);
				}
				if (panel.chatTField != null && panel.chatTField.isShow)
				{
					panel.chatTField.paint(g);
				}
				if (panel2 != null && panel2.chatTField != null && panel2.chatTField.isShow)
				{
					panel2.chatTField.paint(g);
				}
			}
			Res.paintOnScreenDebug(g);
			InfoDlg.paint(g);
			if (currentDialog != null)
			{
				debug("PC", 1);
				currentDialog.paint(g);
			}
			else if (menu.showMenu)
			{
				debug("PD", 1);
				resetTrans(g);
				menu.paintMenu(g);
			}
			GameScr.info1.paint(g);
			GameScr.info2.paint(g);
			if (GameScr.gI().popUpYesNo != null)
			{
				GameScr.gI().popUpYesNo.paint(g);
			}
			if (ChatPopup.currChatPopup != null)
			{
				ChatPopup.currChatPopup.paint(g);
			}
			Hint.paint(g);
			if (ChatPopup.serverChatPopUp != null)
			{
				ChatPopup.serverChatPopUp.paint(g);
			}
			for (int i = 0; i < Effect2.vEffect2.size(); i++)
			{
				Effect2 effect = (Effect2)Effect2.vEffect2.elementAt(i);
				if (effect is ChatPopup && !effect.Equals(ChatPopup.currChatPopup) && !effect.Equals(ChatPopup.serverChatPopUp))
				{
					effect.paint(g);
				}
			}
			if (currentDialog != null)
			{
				currentDialog.paint(g);
			}
			if (isWait())
			{
				paintChangeMap(g);
				if (timeLoading > 0 && LoginScr.timeLogin <= 0 && mSystem.currentTimeMillis() - TIMEOUT >= 1000)
				{
					timeLoading--;
					if (timeLoading == 0)
					{
						timeLoading = 15;
					}
					TIMEOUT = mSystem.currentTimeMillis();
				}
			}
			debug("PE", 1);
			resetTrans(g);
			EffecMn.paintLayer4(g);
			resetTrans(g);
			int num = h / 4;
			if (currentScreen != null && currentScreen is GameScr && thongBaoTest != null)
			{
				g.setClip(60, num, w - 120, mFont.tahoma_7_white.getHeight() + 2);
				mFont.tahoma_7_grey.drawString(g, thongBaoTest, xThongBaoTranslate, num + 1, 0);
				mFont.tahoma_7_yellow.drawString(g, thongBaoTest, xThongBaoTranslate, num, 0);
				g.setClip(0, 0, w, h);
			}
		}
		catch (Exception)
		{
		}
	}

	public static void endDlg()
	{
		if (inputDlg != null)
		{
			inputDlg.tfInput.setMaxTextLenght(500);
		}
		currentDialog = null;
		InfoDlg.hide();
	}

	public static void startOKDlg(string info)
	{
		closeKeyBoard();
		msgdlg.setInfo(info, null, new Command(mResources.OK, instance, 8882, null), null);
		currentDialog = msgdlg;
	}

	public static void startWaitDlg(string info)
	{
		closeKeyBoard();
		msgdlg.setInfo(info, null, new Command(mResources.CANCEL, instance, 8882, null), null);
		currentDialog = msgdlg;
		msgdlg.isWait = true;
	}

	public static void startOKDlg(string info, bool isError)
	{
		closeKeyBoard();
		msgdlg.setInfo(info, null, new Command(mResources.CANCEL, instance, 8882, null), null);
		currentDialog = msgdlg;
		msgdlg.isWait = true;
	}

	public static void startWaitDlg()
	{
		closeKeyBoard();
		Char.isLoadingMap = true;
	}

	public void openWeb(string strLeft, string strRight, string url, string str)
	{
		msgdlg.setInfo(str, new Command(strLeft, this, 8881, url), null, new Command(strRight, this, 8882, null));
		currentDialog = msgdlg;
	}

	public static void startOK(string info, int actionID, object p)
	{
		closeKeyBoard();
		msgdlg.setInfo(info, null, new Command(mResources.OK, instance, actionID, p), null);
		msgdlg.show();
	}

	public static void startYesNoDlg(string info, int iYes, object pYes, int iNo, object pNo)
	{
		closeKeyBoard();
		msgdlg.setInfo(info, new Command(mResources.YES, instance, iYes, pYes), new Command(string.Empty, instance, iYes, pYes), new Command(mResources.NO, instance, iNo, pNo));
		msgdlg.show();
	}

	public static void startYesNoDlg(string info, Command cmdYes, Command cmdNo)
	{
		closeKeyBoard();
		msgdlg.setInfo(info, cmdYes, null, cmdNo);
		msgdlg.show();
	}

	public static void startserverThongBao(string msgSv)
	{
		thongBaoTest = msgSv;
		xThongBaoTranslate = w - 60;
		dir_ = -1;
	}

	public static string getMoneys(int m)
	{
		string text = string.Empty;
		int num = m / 1000 + 1;
		for (int i = 0; i < num; i++)
		{
			if (m < 1000)
			{
				text = m + text;
				break;
			}
			int num2 = m % 1000;
			text = ((num2 != 0) ? ((num2 >= 10) ? ((num2 >= 100) ? ("." + num2 + text) : (".0" + num2 + text)) : (".00" + num2 + text)) : (".000" + text));
			m /= 1000;
		}
		return text;
	}

	public static int getX(int start, int w)
	{
		return (px - start) / w;
	}

	public static int getY(int start, int w)
	{
		return (py - start) / w;
	}

	protected void sizeChanged(int w, int h)
	{
	}

	public static bool isGetResourceFromServer()
	{
		return true;
	}

	public static Image loadImageRMS(string path)
	{
		path = Main.res + "/x" + mGraphics.zoomLevel + path;
		path = cutPng(path);
		Image result = null;
		try
		{
			result = Image.createImage(path);
		}
		catch (Exception ex)
		{
			try
			{
				string[] array = Res.split(path, "/", 0);
				sbyte[] array2 = Rms.loadRMS("x" + mGraphics.zoomLevel + array[array.Length - 1]);
				if (array2 != null)
				{
					result = Image.createImage(array2, 0, array2.Length);
				}
			}
			catch (Exception)
			{
				Cout.LogError("Loi ham khong tim thay a: " + ex.ToString());
			}
		}
		return result;
	}

	public static Image loadImage(string path)
	{
		path = Main.res + "/x" + mGraphics.zoomLevel + path;
		path = cutPng(path);
		Image result = null;
		try
		{
			result = Image.createImage(path);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static string cutPng(string str)
	{
		string result = str;
		if (str.Contains(".png"))
		{
			result = str.Replace(".png", string.Empty);
		}
		return result;
	}

	public static int random(int a, int b)
	{
		return a + r.nextInt(b - a);
	}

	public bool startDust(int dir, int x, int y)
	{
		if (lowGraphic)
		{
			return false;
		}
		int num = ((dir != 1) ? 1 : 0);
		if (dustState[num] != -1)
		{
			return false;
		}
		dustState[num] = 0;
		dustX[num] = x;
		dustY[num] = y;
		return true;
	}

	public void loadWaterSplash()
	{
		if (!lowGraphic)
		{
			imgWS = new Image[3];
			for (int i = 0; i < 3; i++)
			{
				imgWS[i] = loadImage("/e/w" + i + ".png");
			}
			wsX = new int[2];
			wsY = new int[2];
			wsState = new int[2];
			wsF = new int[2];
			wsState[0] = (wsState[1] = -1);
		}
	}

	public bool startWaterSplash(int x, int y)
	{
		if (lowGraphic)
		{
			return false;
		}
		int num = ((wsState[0] != -1) ? 1 : 0);
		if (wsState[num] != -1)
		{
			return false;
		}
		wsState[num] = 0;
		wsX[num] = x;
		wsY[num] = y;
		return true;
	}

	public void updateWaterSplash()
	{
		if (lowGraphic)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (wsState[i] == -1)
			{
				continue;
			}
			wsY[i]--;
			if (gameTick % 2 == 0)
			{
				wsState[i]++;
				if (wsState[i] > 2)
				{
					wsState[i] = -1;
				}
				else
				{
					wsF[i] = wsState[i];
				}
			}
		}
	}

	public void updateDust()
	{
		if (lowGraphic)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (dustState[i] != -1)
			{
				dustState[i]++;
				if (dustState[i] >= 5)
				{
					dustState[i] = -1;
				}
				if (i == 0)
				{
					dustX[i]--;
				}
				else
				{
					dustX[i]++;
				}
				dustY[i]--;
			}
		}
	}

	public static bool isPaint(int x, int y)
	{
		if (x >= GameScr.cmx && x <= GameScr.cmx + GameScr.gW && y >= GameScr.cmy)
		{
			return y <= GameScr.cmy + GameScr.gH + 30;
		}
		return false;
	}

	public void paintDust(mGraphics g)
	{
		if (lowGraphic)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (dustState[i] != -1 && isPaint(dustX[i], dustY[i]))
			{
				g.drawImage(imgDust[i][dustState[i]], dustX[i], dustY[i], 3);
			}
		}
	}

	public void loadDust()
	{
		if (lowGraphic)
		{
			return;
		}
		if (imgDust == null)
		{
			imgDust = new Image[2][];
			for (int i = 0; i < imgDust.Length; i++)
			{
				imgDust[i] = new Image[5];
			}
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					imgDust[j][k] = loadImage("/e/d" + j + k + ".png");
				}
			}
		}
		dustX = new int[2];
		dustY = new int[2];
		dustState = new int[2];
		dustState[0] = (dustState[1] = -1);
	}

	public static void paintShukiren(int x, int y, mGraphics g)
	{
		g.drawRegion(imgShuriken, 0, Main.f * 16, 16, 16, 0, x, y, mGraphics.HCENTER | mGraphics.VCENTER);
	}

	public void resetToLoginScrz()
	{
		resetToLoginScr = true;
	}

	public static bool isPointer(int x, int y, int w, int h)
	{
		if ((isPointerDown || isPointerJustRelease) && px >= x && px <= x + w && py >= y)
		{
			return py <= y + h;
		}
		return false;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 999:
			mSystem.closeBanner();
			endDlg();
			break;
		case 8881:
		{
			string url = (string)p;
			try
			{
				GameMidlet.instance.platformRequest(url);
			}
			catch (Exception)
			{
			}
			currentDialog = null;
			break;
		}
		case 8882:
			InfoDlg.hide();
			currentDialog = null;
			break;
		case 8884:
			endDlg();
			if (serverScr == null)
			{
				serverScr = new ServerScr();
			}
			serverScr.switchToMe();
			break;
		case 8885:
			GameMidlet.instance.exit();
			break;
		case 8886:
		{
			endDlg();
			string name = (string)p;
			Service.gI().addFriend(name);
			break;
		}
		case 8887:
		{
			endDlg();
			int charId2 = (int)p;
			Service.gI().addPartyAccept(charId2);
			break;
		}
		case 8888:
		{
			int charId = (int)p;
			Service.gI().addPartyCancel(charId);
			endDlg();
			break;
		}
		case 8889:
		{
			string str = (string)p;
			endDlg();
			Service.gI().acceptPleaseParty(str);
			break;
		}
		case 9000:
			endDlg();
			SplashScr.imgLogo = null;
			SmallImage.loadBigRMS();
			mSystem.gcc();
			ServerListScreen.bigOk = true;
			ServerListScreen.loadScreen = true;
			GameScr.gI().loadGameScr();
			if (currentScreen != loginScr)
			{
				serverScreen.switchToMe2();
			}
			break;
		case 9999:
			endDlg();
			connect();
			Service.gI().setClientType();
			if (loginScr == null)
			{
				loginScr = new LoginScr();
			}
			loginScr.doLogin();
			break;
		case 88810:
		{
			int playerMapId = (int)p;
			endDlg();
			Service.gI().acceptInviteTrade(playerMapId);
			break;
		}
		case 88811:
			endDlg();
			Service.gI().cancelInviteTrade();
			break;
		case 88814:
		{
			Item[] items = (Item[])p;
			endDlg();
			Service.gI().crystalCollectLock(items);
			break;
		}
		case 88817:
			ChatPopup.addChatPopup(string.Empty, 1, Char.myCharz().npcFocus);
			Service.gI().menu(Char.myCharz().npcFocus.template.npcTemplateId, menu.menuSelectedItem, 0);
			break;
		case 88818:
		{
			short menuId = (short)p;
			Service.gI().textBoxId(menuId, inputDlg.tfInput.getText());
			endDlg();
			break;
		}
		case 88819:
		{
			short menuId2 = (short)p;
			Service.gI().menuId(menuId2);
			break;
		}
		case 88820:
		{
			string[] array = (string[])p;
			if (Char.myCharz().npcFocus == null)
			{
				break;
			}
			int menuSelectedItem = menu.menuSelectedItem;
			if (array.Length > 1)
			{
				MyVector myVector = new MyVector();
				for (int i = 0; i < array.Length - 1; i++)
				{
					myVector.addElement(new Command(array[i + 1], instance, 88821, menuSelectedItem));
				}
				menu.startAt(myVector, 3);
			}
			else
			{
				ChatPopup.addChatPopup(string.Empty, 1, Char.myCharz().npcFocus);
				Service.gI().menu(Char.myCharz().npcFocus.template.npcTemplateId, menuSelectedItem, 0);
			}
			break;
		}
		case 88821:
		{
			int menuId3 = (int)p;
			ChatPopup.addChatPopup(string.Empty, 1, Char.myCharz().npcFocus);
			Service.gI().menu(Char.myCharz().npcFocus.template.npcTemplateId, menuId3, menu.menuSelectedItem);
			break;
		}
		case 88822:
			ChatPopup.addChatPopup(string.Empty, 1, Char.myCharz().npcFocus);
			Service.gI().menu(Char.myCharz().npcFocus.template.npcTemplateId, menu.menuSelectedItem, 0);
			break;
		case 88823:
			startOKDlg(mResources.SENTMSG);
			break;
		case 88824:
			startOKDlg(mResources.NOSENDMSG);
			break;
		case 88825:
			startOKDlg(mResources.sendMsgSuccess, isError: false);
			break;
		case 88826:
			startOKDlg(mResources.cannotSendMsg, isError: false);
			break;
		case 88827:
			startOKDlg(mResources.sendGuessMsgSuccess);
			break;
		case 88828:
			startOKDlg(mResources.sendMsgFail);
			break;
		case 88829:
		{
			string text4 = inputDlg.tfInput.getText();
			if (!text4.Equals(string.Empty))
			{
				Service.gI().changeName(text4, (int)p);
				InfoDlg.showWait();
			}
			break;
		}
		case 88836:
			inputDlg.tfInput.setMaxTextLenght(6);
			inputDlg.show(mResources.INPUT_PRIVATE_PASS, new Command(mResources.ACCEPT, instance, 888361, null), TField.INPUT_TYPE_NUMERIC);
			break;
		case 88837:
		{
			string text3 = inputDlg.tfInput.getText();
			endDlg();
			try
			{
				Service.gI().openLockAccProtect(int.Parse(text3.Trim()));
				break;
			}
			catch (Exception ex4)
			{
				Cout.println("Loi tai 88837 " + ex4.ToString());
				break;
			}
		}
		case 88839:
		{
			string text2 = inputDlg.tfInput.getText();
			endDlg();
			if (text2.Length < 6 || text2.Equals(string.Empty))
			{
				startOKDlg(mResources.ALERT_PRIVATE_PASS_1);
				break;
			}
			try
			{
				startYesNoDlg(mResources.cancelAccountProtection, 888391, text2, 8882, null);
				break;
			}
			catch (Exception)
			{
				startOKDlg(mResources.ALERT_PRIVATE_PASS_2);
				break;
			}
		}
		case 100001:
			Service.gI().getFlag(0, -1);
			InfoDlg.showWait();
			break;
		case 100002:
			if (loginScr == null)
			{
				loginScr = new LoginScr();
			}
			loginScr.backToRegister();
			break;
		case 100005:
			if (Char.myCharz().statusMe == 14)
			{
				startOKDlg(mResources.can_not_do_when_die);
			}
			else
			{
				Service.gI().openUIZone();
			}
			break;
		case 100006:
			mSystem.onDisconnected();
			break;
		case 101023:
			Main.numberQuit = 0;
			break;
		case 101024:
			Res.outz("output 101024");
			endDlg();
			break;
		case 101025:
			endDlg();
			if (ServerListScreen.loadScreen)
			{
				serverScreen.switchToMe();
			}
			else
			{
				serverScreen.show2();
			}
			break;
		case 101026:
			mSystem.onDisconnected();
			break;
		case 888391:
		{
			string s = (string)p;
			endDlg();
			Service.gI().clearAccProtect(int.Parse(s));
			break;
		}
		case 888392:
			Service.gI().menu(4, menu.menuSelectedItem, 0);
			break;
		case 888393:
			if (loginScr == null)
			{
				loginScr = new LoginScr();
			}
			loginScr.doLogin();
			Main.closeKeyBoard();
			break;
		case 888394:
			endDlg();
			break;
		case 888395:
			endDlg();
			break;
		case 888396:
			endDlg();
			break;
		case 888397:
			_ = (string)p;
			break;
		case 888361:
		{
			string text = inputDlg.tfInput.getText();
			endDlg();
			if (text.Length < 6 || text.Equals(string.Empty))
			{
				startOKDlg(mResources.ALERT_PRIVATE_PASS_1);
				break;
			}
			try
			{
				Service.gI().activeAccProtect(int.Parse(text));
				break;
			}
			catch (Exception ex)
			{
				startOKDlg(mResources.ALERT_PRIVATE_PASS_2);
				Cout.println("Loi tai 888361 Gamescavas " + ex.ToString());
				break;
			}
		}
		case 100016:
			instance.doResetToLoginScr(serverScreen);
			ServerListScreen.waitToLogin = true;
			endDlg();
			break;
		}
	}

	public static void clearAllPointerEvent()
	{
		isPointerClick = false;
		isPointerDown = false;
		isPointerJustDown = false;
		isPointerJustRelease = false;
		isPointerSelect = false;
		GameScr.gI().lastSingleClick = 0L;
		GameScr.gI().isPointerDowning = false;
	}

	public static bool isWait()
	{
		if (!Char.isLoadingMap && !LoginScr.isContinueToLogin && !ServerListScreen.waitToLogin && !ServerListScreen.isWait)
		{
			return SelectCharScr.isWait;
		}
		return true;
	}

	static GameCanvas()
	{
		timeNow = 0L;
		lowGraphic = true;
		serverchat = false;
		isMoveNumberPad = true;
		isTouch = false;
		keyPressed = new bool[30];
		keyReleased = new bool[30];
		keyHold = new bool[30];
		arrPos = new Position[4];
		menu = new Menu();
		currentPopup = new MyVector();
		curPos = 0;
		planet = 0;
		imgBlue = new Image[7];
		imgViolet = new Image[7];
		danhHieu = new MyHashTable();
		messageServer = new MyVector(string.Empty);
		isPlaySound = true;
		isRequestMapID = -1;
		dir_ = -1;
		isShowErrorForm = false;
		imgBorder = new Image[3];
		imgSunSpec = new Image[3];
		typeBg = -1;
		yb = new int[5];
		nBg = 0;
		lastBg = -1;
		bgRain = new int[3] { 1, 4, 11 };
		bgRainFont = new int[1] { -1 };
		typeBackGround = -1;
		saveIDBg = -10;
		lastTimePress = 0L;
		flyTexts = new MyVector();
		xThongBaoTranslate = w - 60;
		isPointerJustDown = false;
		r = new MyRandom();
		timeLoading = 15;
	}
}
