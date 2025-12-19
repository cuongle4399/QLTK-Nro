using System;
using System.IO;
using Assets.src.g;
using Xmap;
using DoHoa;
using DoHoa.CustomMenu;
using main.Mod;
using Mod_nro.MenuDataGame;
using Mod.community;
using Mod.CuongLe;
using UnityEngine;

public class GameScr : mScreen, IChatable
{
	public bool isWaitingDoubleClick;

	public long timeStartDblClick;

	public long timeEndDblClick;

	public static bool isPaintOther;

	public static MyVector textTime;

	public static bool isLoadAllData;

	public static GameScr instance;

	public static int gW;

	public static int gH;

	public static int gW2;

	public static int gssw;

	public static int gssh;

	public static int gH34;

	public static int gW3;

	public static int gH3;

	public static int gH23;

	public static int gW23;

	public static int gH2;

	public static int csPadMaxH;

	public static int cmdBarH;

	public static int gW34;

	public static int gW6;

	public static int gH6;

	public static int cmx;

	public static int cmy;

	public static int cmdx;

	public static int cmdy;

	public static int cmvx;

	public static int cmvy;

	public static int cmtoX;

	public static int cmtoY;

	public static int cmxLim;

	public static int cmyLim;

	public static int gssx;

	public static int gssy;

	public static int gssxe;

	public static int gssye;

	public Command cmdback;

	public Command cmdBag;

	public Command cmdSkill;

	public Command cmdTiemnang;

	public Command cmdtrangbi;

	public Command cmdInfo;

	public Command cmdFocus;

	public Command cmdFire;

	public static int d;

	public static int hpPotion;

	public static SkillPaint[] sks;

	public static Arrowpaint[] arrs;

	public static DartInfo[] darts;

	public static Part[] parts;

	public static EffectCharPaint[] efs;

	public static int lockTick;

	private int moveUp;

	private int moveDow;

	private int idTypeTask;

	private bool isstarOpen;

	private bool isChangeSkill;

	public static MyVector vClan;

	public static MyVector vPtMap;

	public static MyVector vFriend;

	public static MyVector vEnemies;

	public static MyVector vCharInMap;

	public static MyVector vItemMap;

	public static MyVector vMobAttack;

	public static MyVector vSet;

	public static MyVector vMob;

	public static MyVector vNpc;

	public static MyVector vFlag;

	public static NClass[] nClasss;

	public static int indexSize;

	public static int indexTitle;

	public static int indexSelect;

	public static int indexRow;

	public static int indexRowMax;

	public static int indexMenu;

	public Item itemFocus;

	public ItemOptionTemplate[] iOptionTemplates;

	public SkillOptionTemplate[] sOptionTemplates;

	private static Scroll scrInfo;

	public static Scroll scrMain;

	public static MyVector vItemUpGrade;

	public static bool isTypeXu;

	public static bool isViewNext;

	public static bool isViewClanMemOnline;

	public static bool isViewClanInvite;

	public static bool isChop;

	public static string titleInputText;

	public static int tickMove;

	public static bool isPaintAlert;

	public static bool isPaintTask;

	public static bool isPaintTeam;

	public static bool isPaintFindTeam;

	public static bool isPaintFriend;

	public static bool isPaintEnemies;

	public static bool isPaintItemInfo;

	public static bool isHaveSelectSkill;

	public static bool isPaintSkill;

	public static bool isPaintInfoMe;

	public static bool isPaintStore;

	public static bool isPaintNonNam;

	public static bool isPaintNonNu;

	public static bool isPaintAoNam;

	public static bool isPaintAoNu;

	public static bool isPaintGangTayNam;

	public static bool isPaintGangTayNu;

	public static bool isPaintQuanNam;

	public static bool isPaintQuanNu;

	public static bool isPaintGiayNam;

	public static bool isPaintGiayNu;

	public static bool isPaintLien;

	public static bool isPaintNhan;

	public static bool isPaintNgocBoi;

	public static bool isPaintPhu;

	public static bool isPaintWeapon;

	public static bool isPaintStack;

	public static bool isPaintStackLock;

	public static bool isPaintGrocery;

	public static bool isPaintGroceryLock;

	public static bool isPaintUpGrade;

	public static bool isPaintConvert;

	public static bool isPaintUpGradeGold;

	public static bool isPaintUpPearl;

	public static bool isPaintBox;

	public static bool isPaintSplit;

	public static bool isPaintCharInMap;

	public static bool isPaintTrade;

	public static bool isPaintZone;

	public static bool isPaintMessage;

	public static bool isPaintClan;

	public static bool isRequestMember;

	public static Char currentCharViewInfo;

	public static long[] exps;

	public static int[] crystals;

	public static int[] upClothe;

	public static int[] upAdorn;

	public static int[] upWeapon;

	public static int[] coinUpCrystals;

	public static int[] coinUpClothes;

	public static int[] coinUpAdorns;

	public static int[] coinUpWeapons;

	public static int[] maxPercents;

	public static int[] goldUps;

	public int tMenuDelay;

	public int zoneCol = 6;

	public int[] zones;

	public int[] pts;

	public int[] numPlayer;

	public int[] maxPlayer;

	public int[] rank1;

	public int[] rank2;

	public string[] rankName1;

	public string[] rankName2;

	public int typeTrade;

	public int typeTradeOrder;

	public int coinTrade;

	public int coinTradeOrder;

	public int timeTrade;

	public int indexItemUse = -1;

	public int cLastFocusID = -1;

	public int cPreFocusID = -1;

	public bool isLockKey;

	public static int[] tasks;

	public static int[] mapTasks;

	public static Image imgRoomStat;

	public static Image frBarPow0;

	public static Image frBarPow1;

	public static Image frBarPow2;

	public static Image frBarPow20;

	public static Image frBarPow21;

	public static Image frBarPow22;

	public MyVector texts;

	public string textsTitle;

	public static sbyte vcData;

	public static sbyte vcMap;

	public static sbyte vcSkill;

	public static sbyte vcItem;

	public static sbyte vsData;

	public static sbyte vsMap;

	public static sbyte vsSkill;

	public static sbyte vsItem;

	public static sbyte vcTask;

	public static Image imgArrow;

	public static Image imgArrow2;

	public static Image imgChat;

	public static Image imgChat2;

	public static Image imgMenu;

	public static Image imgFocus;

	public static Image imgFocus2;

	public static Image imgSkill;

	public static Image imgSkill2;

	public static Image imgHP1;

	public static Image imgHP2;

	public static Image imgHP3;

	public static Image imgHP4;

	public static Image imgFire0;

	public static Image imgFire1;

	public static Image imgNR1;

	public static Image imgNR2;

	public static Image imgNR3;

	public static Image imgNR4;

	public static Image imgLbtn;

	public static Image imgLbtnFocus;

	public static Image imgLbtn2;

	public static Image imgLbtnFocus2;

	public static Image imgAnalog1;

	public static Image imgAnalog2;

	public string tradeName = string.Empty;

	public string tradeItemName = string.Empty;

	public int timeLengthMap;

	public int timeStartMap;

	public static sbyte typeViewInfo;

	public static sbyte typeActive;

	public static InfoMe info1;

	public static InfoMe info2;

	public static Image imgPanel;

	public static Image imgPanel2;

	public static Image imgHP;

	public static Image imgMP;

	public static Image imgSP;

	public static Image imgHPLost;

	public static Image imgMPLost;

	public static Image imgHP_tm_do;

	public static Image imgHP_tm_vang;

	public static Image imgHP_tm_xam;

	public static Image imgHP_tm_xanh;

	public Mob mobCapcha;

	public MagicTree magicTree;

	private short l;

	public static int countEff;

	public static GamePad gamePad;

	public static Image imgChatPC;

	public static Image imgChatsPC2;

	public static int isAnalog;

	public static Image img_ct_bar_0;

	public static Image img_ct_bar_1;

	public static bool isUseTouch;

	public Command cmdDoiCo;

	public Command cmdLogOut;

	public Command cmdChatTheGioi;

	public Command cmdshowInfo;

	private static Command[] cmdTestLogin;

	public const int numSkill = 10;

	public const int numSkill_2 = 5;

	public static Skill[] keySkill;

	public static Skill[] onScreenSkill;

	public Command cmdMenu;

	public static int firstY;

	public static int wSkill;

	public static long deltaTime;

	public bool isPointerDowning;

	public bool isChangingCameraMode;

	private int ptLastDownX;

	private int ptLastDownY;

	private int ptFirstDownX;

	private int ptFirstDownY;

	private int ptDownTime;

	private bool disableSingleClick;

	public long lastSingleClick;

	public bool clickMoving;

	public bool clickOnTileTop;

	public bool clickMovingRed;

	public int clickToX;

	public int clickToY;

	private int lastClickCMX;

	private int lastClickCMY;

	private int clickMovingP1;

	private int clickMovingTimeOut;

	private long lastMove;

	public static bool isNewClanMessage;

	private long lastFire;

	private long lastUsePotion;

	public int auto;

	public int dem;

	private string strTam = string.Empty;

	private int a;

	public bool isFreez;

	public bool isUseFreez;

	public static Image imgTrans;

	public bool isRongThanXuatHien;

	public bool isRongNamek;

	public bool isSuperPower;

	public int tPower;

	public int xPower;

	public int yPower;

	public int dxPower;

	public bool activeRongThan;

	public bool isMeCallRongThan;

	public int mautroi;

	public int mapRID;

	public int zoneRID;

	public int bgRID = -1;

	public static int tam;

	public static bool isAutoPlay;

	public static bool canAutoPlay;

	public static bool isChangeZone;

	private int timeSkill;

	private int nSkill;

	private int selectedIndexSkill = -1;

	private Skill lastSkill;

	private bool doSeleckSkillFlag;

	public string strCapcha;

	private long longPress;

	private int move;

	public bool flareFindFocus;

	private int flareTime;

	public int keyTouchSkill = -1;

	private long lastSendUpdatePostion;

	public static long lastTick;

	public static long currTick;

	private int timeAuto;

	public static long lastXS;

	public static long currXS;

	public static int secondXS;

	public int runArrow;

	public static int isPaintRada;

	public static Image imgNut;

	public static Image imgNutF;

	public int[] keyCapcha;

	public static Image imgCapcha;

	public string keyInput;

	public static int disXC;

	public static bool isPaint;

	public static int shock_scr;

	private static int[] shock_x;

	private static int[] shock_y;

	private int tDoubleDelay;

	public static Image arrow;

	private static int yTouchBar;

	private static int xC;

	private static int yC;

	private static int xL;

	private static int yL;

	public int xR;

	public int yR;

	private static int xU;

	private static int yU;

	private static int xF;

	private static int yF;

	public static int xHP;

	public static int yHP;

	private static int xTG;

	private static int yTG;

	public static int[] xS;

	public static int[] yS;

	public static int xSkill;

	public static int ySkill;

	public static int padSkill;

	public long dMP;

	public long twMp;

	public bool isInjureMp;

	public long dHP;

	public long twHp;

	public bool isInjureHp;

	private long curr;

	private long last;

	private int secondVS;

	private int[] idVS = new int[2] { -1, -1 };

	public static string[] flyTextString;

	public static int[] flyTextX;

	public static int[] flyTextY;

	public static int[] flyTextYTo;

	public static int[] flyTextDx;

	public static int[] flyTextDy;

	public static int[] flyTextState;

	public static int[] flyTextColor;

	public static int[] flyTime;

	public static int[] splashX;

	public static int[] splashY;

	public static int[] splashState;

	public static int[] splashF;

	public static int[] splashDir;

	public static Image[] imgSplash;

	public static int cmdBarX;

	public static int cmdBarY;

	public static int cmdBarW;

	public static int cmdBarLeftW;

	public static int cmdBarRightW;

	public static int cmdBarCenterW;

	public static int hpBarX;

	public static int hpBarY;

	public static int spBarW;

	public static int mpBarW;

	public static int expBarW;

	public static int lvPosX;

	public static int moneyPosX;

	public static int hpBarH;

	public static int girlHPBarY;

	public static long hpBarW;

	public static Image[] imgCmdBar;

	private int imgScrW;

	public static int popupY;

	public static int popupX;

	public static int isborderIndex;

	public static int isselectedRow;

	private static Image imgNolearn;

	public int cmxp;

	public int cmvxp;

	public int cmdxp;

	public int cmxLimp;

	public int cmyLimp;

	public int cmyp;

	public int cmvyp;

	public int cmdyp;

	private int indexTiemNang;

	private string alertURL;

	private string fnick;

	public static int xstart;

	public static int ystart;

	public static int popupW;

	public static int popupH;

	public static int cmySK;

	public static int cmtoYSK;

	public static int cmdySK;

	public static int cmvySK;

	public static int cmyLimSK;

	public static int columns;

	public static int rows;

	private int totalRowInfo;

	private int ypaintKill;

	private int ylimUp;

	private int ylimDow;

	private int yPaint;

	public static int indexEff;

	public static EffectCharPaint effUpok;

	public static int inforX;

	public static int inforY;

	public static int inforW;

	public static int inforH;

	public Command cmdDead;

	public static bool notPaint;

	public static bool isPing;

	public static int INFO;

	public static int STORE;

	public static int ZONE;

	public static int UPGRADE;

	private int Hitem = 30;

	private int maxSizeRow = 5;

	private int isTranKyNang;

	private bool isTran;

	private int cmY_Old;

	private int cmX_Old;

	public PopUpYesNo popUpYesNo;

	public static MyVector vChatVip;

	public static int vBig;

	public bool isFireWorks;

	public int[] winnumber;

	public int[] randomNumber;

	public int[] tMove;

	public int[] moveCount;

	public int[] delayMove;

	public int moveIndex;

	private bool isWin;

	private string strFinish;

	private int tShow;

	private int xChatVip;

	private int currChatWidth;

	private bool startChat;

	public sbyte percentMabu;

	public bool mabuEff;

	public int tMabuEff;

	public static bool isPaintChatVip;

	public static sbyte mabuPercent;

	public static sbyte isNewMember;

	private string yourNumber = string.Empty;

	private string[] strPaint;

	public static Image imgHP_NEW;

	public static InfoPhuBan phuban_Info;

	public static FrameImage fra_PVE_Bar_0;

	public static FrameImage fra_PVE_Bar_1;

	public static Image imgVS;

	public static Image imgBall;

	public static Image imgKhung;

	public int countFrameSkill;

	public static Image imgBgIOS;

	public static int nCT_TeamB;

	public static int nCT_TeamA;

	public static long nCT_timeBallte;

	public static string nCT_team;

	public static int nCT_nBoyBaller;

	public static bool isPaint_CT;

	public static sbyte nCT_floor;

	public static bool is_Paint_boardCT_Expand;

	private static int xRect;

	private static int yRect;

	private static int wRect;

	private static int hRect;

	public static MyVector res_CT;

	public static int nTop;

	public static bool isPickNgocRong;

	public static int nUSER_CT;

	public static int nUSER_MAX_CT;

	public static bool isudungCapsun;

	public static bool isudungCapsun4;

	public static bool isudungCapsun3;

	private static bool thongBao;

	public static Image logoInGame;

	public static Image backgroundGame;

	public GameScr()
	{
		if (GameCanvas.w == 128 || GameCanvas.h <= 208)
		{
			indexSize = 20;
		}
		cmdback = new Command(string.Empty, 11021);
		cmdMenu = new Command("menu", 11000);
		cmdFocus = new Command(string.Empty, 11001);
		cmdMenu.img = imgMenu;
		cmdMenu.w = mGraphics.getImageWidth(cmdMenu.img) + 20;
		cmdMenu.isPlaySoundButton = false;
		cmdFocus.img = imgFocus;
		if (GameCanvas.isTouch)
		{
			cmdMenu.x = 0;
			cmdMenu.y = 50;
			cmdFocus = null;
		}
		else
		{
			cmdMenu.x = 0;
			cmdMenu.y = gH - 30;
			cmdFocus.x = gW - 32;
			cmdFocus.y = gH - 32;
		}
		right = cmdFocus;
		isPaintRada = 1;
		if (GameCanvas.isTouch)
		{
			isHaveSelectSkill = true;
		}
		cmdDoiCo = new Command("Đổi cờ", GameCanvas.gI(), 100001, null);
		cmdLogOut = new Command("Logout", GameCanvas.gI(), 100002, null);
		cmdChatTheGioi = new Command("chat world", GameCanvas.gI(), 100003, null);
		cmdshowInfo = new Command("InfoLog", GameCanvas.gI(), 100004, null);
		cmdDoiCo.setType();
		cmdLogOut.setType();
		cmdChatTheGioi.setType();
		cmdshowInfo.setType();
		cmdChatTheGioi.x = GameCanvas.w - cmdChatTheGioi.w;
		cmdshowInfo.x = GameCanvas.w - cmdshowInfo.w;
		cmdLogOut.x = GameCanvas.w - cmdLogOut.w;
		cmdDoiCo.x = GameCanvas.w - cmdDoiCo.w;
		cmdChatTheGioi.y = cmdChatTheGioi.h + mFont.tahoma_7_white.getHeight();
		cmdshowInfo.y = cmdChatTheGioi.h * 2 + mFont.tahoma_7_white.getHeight();
		cmdLogOut.y = cmdChatTheGioi.h * 3 + mFont.tahoma_7_white.getHeight();
		cmdDoiCo.y = cmdChatTheGioi.h * 4 + mFont.tahoma_7_white.getHeight();
	}

	public static void loadBg()
	{
		try
		{
			logoInGame = Image.createImage(File.ReadAllBytes("Data//logo//cuongle.png"));
		}
		catch
		{
			logoInGame = Image.__createImage(Convert.FromBase64String(GraphicsManagement.Logo));
		}
		try
		{
			backgroundGame = Image.createImage(File.ReadAllBytes("Data//logo//background.png"));
		}
		catch
		{
		}
		fra_PVE_Bar_0 = new FrameImage(mSystem.loadImage("/mainImage/i_pve_bar_0.png"), 6, 15);
		fra_PVE_Bar_1 = new FrameImage(mSystem.loadImage("/mainImage/i_pve_bar_1.png"), 38, 21);
		imgVS = mSystem.loadImage("/mainImage/i_vs.png");
		imgBall = mSystem.loadImage("/mainImage/i_charlife.png");
		imgHP_NEW = mSystem.loadImage("/mainImage/i_hp.png");
		imgKhung = mSystem.loadImage("/mainImage/i_khung.png");
		imgLbtn = GameCanvas.loadImage("/mainImage/myTexture2dbtnl.png");
		imgLbtnFocus = GameCanvas.loadImage("/mainImage/myTexture2dbtnlf.png");
		imgLbtn2 = GameCanvas.loadImage("/mainImage/myTexture2dbtnl2.png");
		imgLbtnFocus2 = GameCanvas.loadImage("/mainImage/myTexture2dbtnlf2.png");
		imgPanel = GameCanvas.loadImage("/mainImage/myTexture2dpanel.png");
		imgPanel2 = GameCanvas.loadImage("/mainImage/panel2.png");
		imgHP = GameCanvas.loadImage("/mainImage/myTexture2dHP.png");
		imgSP = GameCanvas.loadImage("/mainImage/SP.png");
		imgHPLost = GameCanvas.loadImage("/mainImage/myTexture2dhpLost.png");
		imgMPLost = GameCanvas.loadImage("/mainImage/myTexture2dmpLost.png");
		imgMP = GameCanvas.loadImage("/mainImage/myTexture2dMP.png");
		imgSkill = GameCanvas.loadImage("/mainImage/myTexture2dskill.png");
		imgSkill2 = GameCanvas.loadImage("/mainImage/myTexture2dskill2.png");
		imgMenu = GameCanvas.loadImage("/mainImage/myTexture2dmenu.png");
		imgFocus = GameCanvas.loadImage("/mainImage/myTexture2dfocus.png");
		imgHP_tm_do = GameCanvas.loadImage("/mainImage/tm-do.png");
		imgHP_tm_vang = GameCanvas.loadImage("/mainImage/tm-vang.png");
		imgHP_tm_xam = GameCanvas.loadImage("/mainImage/tm-xam.png");
		imgHP_tm_xanh = GameCanvas.loadImage("/mainImage/tm-xanh.png");
		imgChatPC = GameCanvas.loadImage("/pc/chat.png");
		imgChatsPC2 = GameCanvas.loadImage("/pc/chat2.png");
		if (GameCanvas.isTouch)
		{
			imgArrow = GameCanvas.loadImage("/mainImage/myTexture2darrow.png");
			imgArrow2 = GameCanvas.loadImage("/mainImage/myTexture2darrow2.png");
			imgChat = GameCanvas.loadImage("/mainImage/myTexture2dchat.png");
			imgChat2 = GameCanvas.loadImage("/mainImage/myTexture2dchat2.png");
			imgFocus2 = GameCanvas.loadImage("/mainImage/myTexture2dfocus2.png");
			imgHP1 = GameCanvas.loadImage("/mainImage/myTexture2dPea0.png");
			imgHP2 = GameCanvas.loadImage("/mainImage/myTexture2dPea1.png");
			imgAnalog1 = GameCanvas.loadImage("/mainImage/myTexture2danalog1.png");
			imgAnalog2 = GameCanvas.loadImage("/mainImage/myTexture2danalog2.png");
			imgHP3 = GameCanvas.loadImage("/mainImage/myTexture2dPea2.png");
			imgHP4 = GameCanvas.loadImage("/mainImage/myTexture2dPea3.png");
			imgFire0 = GameCanvas.loadImage("/mainImage/myTexture2dfirebtn0.png");
			imgFire1 = GameCanvas.loadImage("/mainImage/myTexture2dfirebtn1.png");
		}
		imgNR1 = GameCanvas.loadImage("/mainImage/myTexture2dPea_0.png");
		imgNR2 = GameCanvas.loadImage("/mainImage/myTexture2dPea_1.png");
		imgNR3 = GameCanvas.loadImage("/mainImage/myTexture2dPea_2.png");
		imgNR4 = GameCanvas.loadImage("/mainImage/myTexture2dPea_3.png");
		flyTextX = new int[5];
		flyTextY = new int[5];
		flyTextDx = new int[5];
		flyTextDy = new int[5];
		flyTextState = new int[5];
		flyTextString = new string[5];
		flyTextYTo = new int[5];
		flyTime = new int[5];
		flyTextColor = new int[8];
		for (int i = 0; i < 5; i++)
		{
			flyTextState[i] = -1;
		}
		sbyte[] array = Rms.loadRMS("NRdataVersion");
		sbyte[] array2 = Rms.loadRMS("NRmapVersion");
		sbyte[] array3 = Rms.loadRMS("NRskillVersion");
		sbyte[] array4 = Rms.loadRMS("NRitemVersion");
		if (array != null)
		{
			vcData = array[0];
		}
		if (array2 != null)
		{
			vcMap = array2[0];
		}
		if (array3 != null)
		{
			vcSkill = array3[0];
		}
		if (array4 != null)
		{
			vcItem = array4[0];
		}
		imgNut = GameCanvas.loadImage("/mainImage/myTexture2dnut.png");
		imgNutF = GameCanvas.loadImage("/mainImage/myTexture2dnutF.png");
		MobCapcha.init();
		isAnalog = ((Rms.loadRMSInt("analog") == 1) ? 1 : 0);
		gamePad = new GamePad();
		arrow = GameCanvas.loadImage("/mainImage/myTexture2darrow3.png");
		imgTrans = GameCanvas.loadImage("/bg/trans.png");
		imgRoomStat = GameCanvas.loadImage("/mainImage/myTexture2dstat.png");
		frBarPow0 = GameCanvas.loadImage("/mainImage/myTexture2dlineColor20.png");
		frBarPow1 = GameCanvas.loadImage("/mainImage/myTexture2dlineColor21.png");
		frBarPow2 = GameCanvas.loadImage("/mainImage/myTexture2dlineColor22.png");
		frBarPow20 = GameCanvas.loadImage("/mainImage/myTexture2dlineColor00.png");
		frBarPow21 = GameCanvas.loadImage("/mainImage/myTexture2dlineColor01.png");
		frBarPow22 = GameCanvas.loadImage("/mainImage/myTexture2dlineColor02.png");
	}

	public void initSelectChar()
	{
		readPart();
		SmallImage.init();
	}

	public static void paintOngMauPercent(Image img0, Image img1, Image img2, float x, float y, int size, float pixelPercent, mGraphics g)
	{
		int clipX = g.getClipX();
		int clipY = g.getClipY();
		int clipWidth = g.getClipWidth();
		int clipHeight = g.getClipHeight();
		g.setClip((int)x, (int)y, (int)pixelPercent, 13);
		int num = size / 15 - 2;
		for (int i = 0; i < num; i++)
		{
			g.drawImage(img1, x + (float)((i + 1) * 15), y, 0);
		}
		g.drawImage(img0, x, y, 0);
		g.drawImage(img1, x + (float)size - 30f, y, 0);
		g.drawImage(img2, x + (float)size - 15f, y, 0);
		g.setClip(clipX, clipY, clipWidth, clipHeight);
	}

	public void initTraining()
	{
		if (CreateCharScr.isCreateChar)
		{
			CreateCharScr.isCreateChar = false;
			right = null;
		}
	}

	public bool isMapDocNhan()
	{
		if (TileMap.mapID >= 53)
		{
			return TileMap.mapID <= 62;
		}
		return false;
	}

	public bool isMapFize()
	{
		return TileMap.mapID >= 63;
	}

	public override void switchToMe()
	{
		vChatVip.removeAllElements();
		ServerListScreen.isWait = false;
		if (BackgroudEffect.isHaveRain())
		{
			SoundMn.gI().rain();
		}
		LoginScr.isContinueToLogin = false;
		Char.isLoadingMap = false;
		if (!isPaintOther)
		{
			Service.gI().finishLoadMap();
		}
		if (TileMap.isTrainingMap())
		{
			initTraining();
		}
		info1.isUpdate = true;
		info2.isUpdate = true;
		resetButton();
		isLoadAllData = true;
		isPaintOther = false;
		base.switchToMe();
	}

	public static int getMaxExp(int level)
	{
		int num = 0;
		for (int i = 0; i <= level; i++)
		{
			num += (int)exps[i];
		}
		return num;
	}

	public static void resetAllvector()
	{
		vCharInMap.removeAllElements();
		Teleport.vTeleport.removeAllElements();
		vItemMap.removeAllElements();
		Effect2.vEffect2.removeAllElements();
		Effect2.vAnimateEffect.removeAllElements();
		Effect2.vEffect2Outside.removeAllElements();
		Effect2.vEffectFeet.removeAllElements();
		Effect2.vEffect3.removeAllElements();
		vMobAttack.removeAllElements();
		vMob.removeAllElements();
		vNpc.removeAllElements();
		Char.myCharz().vMovePoints.removeAllElements();
	}

	public void loadSkillShortcut()
	{
	}

	public void onOSkill(sbyte[] oSkillID)
	{
		Cout.println("GET onScreenSkill!");
		onScreenSkill = new Skill[10];
		if (oSkillID == null)
		{
			loadDefaultonScreenSkill();
			return;
		}
		for (int i = 0; i < oSkillID.Length; i++)
		{
			for (int j = 0; j < Char.myCharz().vSkillFight.size(); j++)
			{
				Skill skill = (Skill)Char.myCharz().vSkillFight.elementAt(j);
				if (skill.template.id == oSkillID[i])
				{
					onScreenSkill[i] = skill;
					break;
				}
			}
		}
	}

	public void onKSkill(sbyte[] kSkillID)
	{
		Cout.println("GET KEYSKILL!");
		keySkill = new Skill[10];
		if (kSkillID == null)
		{
			loadDefaultKeySkill();
			return;
		}
		for (int i = 0; i < kSkillID.Length; i++)
		{
			for (int j = 0; j < Char.myCharz().vSkillFight.size(); j++)
			{
				Skill skill = (Skill)Char.myCharz().vSkillFight.elementAt(j);
				if (skill.template.id == kSkillID[i])
				{
					keySkill[i] = skill;
					break;
				}
			}
		}
	}

	public void onCSkill(sbyte[] cSkillID)
	{
		Cout.println("GET CURRENTSKILL!");
		if (cSkillID == null || cSkillID.Length == 0)
		{
			if (Char.myCharz().vSkillFight.size() > 0)
			{
				Char.myCharz().myskill = (Skill)Char.myCharz().vSkillFight.elementAt(0);
			}
		}
		else
		{
			for (int i = 0; i < Char.myCharz().vSkillFight.size(); i++)
			{
				Skill skill = (Skill)Char.myCharz().vSkillFight.elementAt(i);
				if (skill.template.id == cSkillID[0])
				{
					Char.myCharz().myskill = skill;
					break;
				}
			}
		}
		if (Char.myCharz().myskill != null)
		{
			Service.gI().selectSkill(Char.myCharz().myskill.template.id);
			saveRMSCurrentSkill(Char.myCharz().myskill.template.id);
		}
	}

	private void loadDefaultonScreenSkill()
	{
		Cout.println("LOAD DEFAULT ONmScreen SKILL");
		for (int i = 0; i < onScreenSkill.Length && i < Char.myCharz().vSkillFight.size(); i++)
		{
			Skill skill = (Skill)Char.myCharz().vSkillFight.elementAt(i);
			onScreenSkill[i] = skill;
		}
		saveonScreenSkillToRMS();
	}

	private void loadDefaultKeySkill()
	{
		Cout.println("LOAD DEFAULT KEY SKILL");
		for (int i = 0; i < keySkill.Length && i < Char.myCharz().vSkillFight.size(); i++)
		{
			Skill skill = (Skill)Char.myCharz().vSkillFight.elementAt(i);
			keySkill[i] = skill;
		}
		saveKeySkillToRMS();
	}

	public void doSetOnScreenSkill(SkillTemplate skillTemplate)
	{
		Skill skill = Char.myCharz().getSkill(skillTemplate);
		MyVector myVector = new MyVector();
		for (int i = 0; i < 10; i++)
		{
			object[] p = new object[2]
			{
				skill,
				i + string.Empty
			};
			Command command = new Command(mResources.into_place + (i + 1), 11120, p);
			if (onScreenSkill[i] != null)
			{
				command.isDisplay = true;
			}
			myVector.addElement(command);
		}
		GameCanvas.menu.startAt(myVector, 0);
	}

	public void doSetKeySkill(SkillTemplate skillTemplate)
	{
		Cout.println("DO SET KEY SKILL");
		Skill skill = Char.myCharz().getSkill(skillTemplate);
		string[] array = ((!TField.isQwerty) ? mResources.key_skill : mResources.key_skill_qwerty);
		MyVector myVector = new MyVector();
		for (int i = 0; i < 10; i++)
		{
			object[] p = new object[2]
			{
				skill,
				i + string.Empty
			};
			myVector.addElement(new Command(array[i], 11121, p));
		}
		GameCanvas.menu.startAt(myVector, 0);
	}

	public void saveonScreenSkillToRMS()
	{
		sbyte[] array = new sbyte[onScreenSkill.Length];
		for (int i = 0; i < onScreenSkill.Length; i++)
		{
			if (onScreenSkill[i] == null)
			{
				array[i] = -1;
			}
			else
			{
				array[i] = onScreenSkill[i].template.id;
			}
		}
		Service.gI().changeOnKeyScr(array);
	}

	public void saveKeySkillToRMS()
	{
		sbyte[] array = new sbyte[keySkill.Length];
		for (int i = 0; i < keySkill.Length; i++)
		{
			if (keySkill[i] == null)
			{
				array[i] = -1;
			}
			else
			{
				array[i] = keySkill[i].template.id;
			}
		}
		Service.gI().changeOnKeyScr(array);
	}

	public void saveRMSCurrentSkill(sbyte id)
	{
	}

	public void addSkillShortcut(Skill skill)
	{
		Cout.println("ADD SKILL SHORTCUT TO SKILL " + skill.template.id);
		for (int i = 0; i < onScreenSkill.Length; i++)
		{
			if (onScreenSkill[i] == null)
			{
				onScreenSkill[i] = skill;
				break;
			}
		}
		for (int j = 0; j < keySkill.Length; j++)
		{
			if (keySkill[j] == null)
			{
				keySkill[j] = skill;
				break;
			}
		}
		if (Char.myCharz().myskill == null)
		{
			Char.myCharz().myskill = skill;
		}
		saveKeySkillToRMS();
		saveonScreenSkillToRMS();
	}

	public bool isBagFull()
	{
		for (int num = Char.myCharz().arrItemBag.Length - 1; num >= 0; num--)
		{
			if (Char.myCharz().arrItemBag[num] == null)
			{
				return false;
			}
		}
		return true;
	}

	public void createConfirm(string[] menu, Npc npc)
	{
		resetButton();
		isLockKey = true;
		left = new Command(menu[0], 130011, npc);
		right = new Command(menu[1], 130012, npc);
	}

	public void createMenu(string[] menu, Npc npc)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < menu.Length; i++)
		{
			myVector.addElement(new Command(menu[i], 11057, npc));
		}
		GameCanvas.menu.startAt(myVector, 2);
	}

	public void readPart()
	{
		DataInputStream dataInputStream = null;
		try
		{
			dataInputStream = new DataInputStream(Rms.loadRMS("NR_part"));
			int num = dataInputStream.readShort();
			parts = new Part[num];
			for (int i = 0; i < num; i++)
			{
				int type = dataInputStream.readByte();
				parts[i] = new Part(type);
				for (int j = 0; j < parts[i].pi.Length; j++)
				{
					parts[i].pi[j] = new PartImage();
					parts[i].pi[j].id = dataInputStream.readShort();
					parts[i].pi[j].dx = dataInputStream.readByte();
					parts[i].pi[j].dy = dataInputStream.readByte();
				}
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("LOI TAI readPart " + ex.ToString());
		}
		finally
		{
			try
			{
				dataInputStream.close();
			}
			catch (Exception ex2)
			{
				Res.outz2("LOI TAI readPart 2" + ex2.StackTrace);
			}
		}
	}

	public void readEfect()
	{
		DataInputStream dataInputStream = null;
		try
		{
			dataInputStream = new DataInputStream(Rms.loadRMS("NR_effect"));
			int num = dataInputStream.readShort();
			efs = new EffectCharPaint[num];
			for (int i = 0; i < num; i++)
			{
				efs[i] = new EffectCharPaint();
				efs[i].idEf = dataInputStream.readShort();
				efs[i].arrEfInfo = new EffectInfoPaint[dataInputStream.readByte()];
				for (int j = 0; j < efs[i].arrEfInfo.Length; j++)
				{
					efs[i].arrEfInfo[j] = new EffectInfoPaint();
					efs[i].arrEfInfo[j].idImg = dataInputStream.readShort();
					efs[i].arrEfInfo[j].dx = dataInputStream.readByte();
					efs[i].arrEfInfo[j].dy = dataInputStream.readByte();
				}
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			try
			{
				dataInputStream.close();
			}
			catch (Exception ex2)
			{
				Cout.LogError("Loi ham Eff: " + ex2.ToString());
			}
		}
	}

	public void readArrow()
	{
		DataInputStream dataInputStream = null;
		try
		{
			dataInputStream = new DataInputStream(Rms.loadRMS("NR_arrow"));
			int num = dataInputStream.readShort();
			arrs = new Arrowpaint[num];
			for (int i = 0; i < num; i++)
			{
				arrs[i] = new Arrowpaint();
				arrs[i].id = dataInputStream.readShort();
				arrs[i].imgId[0] = dataInputStream.readShort();
				arrs[i].imgId[1] = dataInputStream.readShort();
				arrs[i].imgId[2] = dataInputStream.readShort();
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			try
			{
				dataInputStream.close();
			}
			catch (Exception ex2)
			{
				Cout.LogError("Loi ham readArrow: " + ex2.ToString());
			}
		}
	}

	public void readDart()
	{
		DataInputStream dataInputStream = null;
		try
		{
			dataInputStream = new DataInputStream(Rms.loadRMS("NR_dart"));
			int num = dataInputStream.readShort();
			darts = new DartInfo[num];
			for (int i = 0; i < num; i++)
			{
				darts[i] = new DartInfo();
				darts[i].id = dataInputStream.readShort();
				darts[i].nUpdate = dataInputStream.readShort();
				darts[i].va = dataInputStream.readShort() * 256;
				darts[i].xdPercent = dataInputStream.readShort();
				int num2 = dataInputStream.readShort();
				darts[i].tail = new short[num2];
				for (int j = 0; j < num2; j++)
				{
					darts[i].tail[j] = dataInputStream.readShort();
				}
				num2 = dataInputStream.readShort();
				darts[i].tailBorder = new short[num2];
				for (int k = 0; k < num2; k++)
				{
					darts[i].tailBorder[k] = dataInputStream.readShort();
				}
				num2 = dataInputStream.readShort();
				darts[i].xd1 = new short[num2];
				for (int l = 0; l < num2; l++)
				{
					darts[i].xd1[l] = dataInputStream.readShort();
				}
				num2 = dataInputStream.readShort();
				darts[i].xd2 = new short[num2];
				for (int m = 0; m < num2; m++)
				{
					darts[i].xd2[m] = dataInputStream.readShort();
				}
				num2 = dataInputStream.readShort();
				darts[i].head = new short[num2][];
				for (int n = 0; n < num2; n++)
				{
					short num3 = dataInputStream.readShort();
					darts[i].head[n] = new short[num3];
					for (int num4 = 0; num4 < num3; num4++)
					{
						darts[i].head[n][num4] = dataInputStream.readShort();
					}
				}
				num2 = dataInputStream.readShort();
				darts[i].headBorder = new short[num2][];
				for (int num5 = 0; num5 < num2; num5++)
				{
					short num6 = dataInputStream.readShort();
					darts[i].headBorder[num5] = new short[num6];
					for (int num7 = 0; num7 < num6; num7++)
					{
						darts[i].headBorder[num5][num7] = dataInputStream.readShort();
					}
				}
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi ham ReadDart: " + ex.ToString());
		}
		finally
		{
			try
			{
				dataInputStream.close();
			}
			catch (Exception ex2)
			{
				Cout.LogError("Loi ham reaaDart: " + ex2.ToString());
			}
		}
	}

	public void readSkill()
	{
		DataInputStream dataInputStream = null;
		try
		{
			dataInputStream = new DataInputStream(Rms.loadRMS("NR_skill"));
			int num = dataInputStream.readShort();
			sks = new SkillPaint[Skills.skills.size()];
			for (int i = 0; i < num; i++)
			{
				short num2 = dataInputStream.readShort();
				if (num2 == 1111)
				{
					num2 = (short)(num - 1);
				}
				sks[num2] = new SkillPaint();
				sks[num2].id = num2;
				sks[num2].effectHappenOnMob = dataInputStream.readShort();
				if (sks[num2].effectHappenOnMob <= 0)
				{
					sks[num2].effectHappenOnMob = 80;
				}
				sks[num2].numEff = dataInputStream.readByte();
				sks[num2].skillStand = new SkillInfoPaint[dataInputStream.readByte()];
				for (int j = 0; j < sks[num2].skillStand.Length; j++)
				{
					sks[num2].skillStand[j] = new SkillInfoPaint();
					sks[num2].skillStand[j].status = dataInputStream.readByte();
					sks[num2].skillStand[j].effS0Id = dataInputStream.readShort();
					sks[num2].skillStand[j].e0dx = dataInputStream.readShort();
					sks[num2].skillStand[j].e0dy = dataInputStream.readShort();
					sks[num2].skillStand[j].effS1Id = dataInputStream.readShort();
					sks[num2].skillStand[j].e1dx = dataInputStream.readShort();
					sks[num2].skillStand[j].e1dy = dataInputStream.readShort();
					sks[num2].skillStand[j].effS2Id = dataInputStream.readShort();
					sks[num2].skillStand[j].e2dx = dataInputStream.readShort();
					sks[num2].skillStand[j].e2dy = dataInputStream.readShort();
					sks[num2].skillStand[j].arrowId = dataInputStream.readShort();
					sks[num2].skillStand[j].adx = dataInputStream.readShort();
					sks[num2].skillStand[j].ady = dataInputStream.readShort();
				}
				sks[num2].skillfly = new SkillInfoPaint[dataInputStream.readByte()];
				for (int k = 0; k < sks[num2].skillfly.Length; k++)
				{
					sks[num2].skillfly[k] = new SkillInfoPaint();
					sks[num2].skillfly[k].status = dataInputStream.readByte();
					sks[num2].skillfly[k].effS0Id = dataInputStream.readShort();
					sks[num2].skillfly[k].e0dx = dataInputStream.readShort();
					sks[num2].skillfly[k].e0dy = dataInputStream.readShort();
					sks[num2].skillfly[k].effS1Id = dataInputStream.readShort();
					sks[num2].skillfly[k].e1dx = dataInputStream.readShort();
					sks[num2].skillfly[k].e1dy = dataInputStream.readShort();
					sks[num2].skillfly[k].effS2Id = dataInputStream.readShort();
					sks[num2].skillfly[k].e2dx = dataInputStream.readShort();
					sks[num2].skillfly[k].e2dy = dataInputStream.readShort();
					sks[num2].skillfly[k].arrowId = dataInputStream.readShort();
					sks[num2].skillfly[k].adx = dataInputStream.readShort();
					sks[num2].skillfly[k].ady = dataInputStream.readShort();
				}
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi ham readSkill: " + ex.ToString());
		}
		finally
		{
			try
			{
				dataInputStream.close();
			}
			catch (Exception ex2)
			{
				Cout.LogError("Loi ham readskill: " + ex2.ToString());
			}
		}
	}

	public void readOk()
	{
		try
		{
			Res.outz("<readOk><vsData<" + vsData + "==" + vcData);
			Res.outz("<readOk><vsMap<" + vsMap + "==" + vcMap);
			Res.outz("<readOk><vsSkill<" + vsSkill + "==" + vcSkill);
			Res.outz("<readOk><vsItem<" + vsItem + "==" + vcItem);
			if (vsData == vcData && vsMap == vcMap && vsSkill == vcSkill && vsItem == vcItem)
			{
				Res.outz(vsData + "," + vsMap + "," + vsSkill + "," + vsItem);
				gI().readDart();
				gI().readEfect();
				gI().readArrow();
				gI().readSkill();
				Service.gI().clientOk();
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi ham readOk: " + ex.ToString());
		}
	}

	public static GameScr gI()
	{
		if (instance == null)
		{
			instance = new GameScr();
		}
		return instance;
	}

	public static void clearGameScr()
	{
		instance = null;
	}

	public void loadGameScr()
	{
		loadSplash();
		Res.init();
		loadInforBar();
	}

	public void doMenuInforMe()
	{
		scrMain.clear();
		scrInfo.clear();
		isViewNext = false;
		cmdBag = new Command(mResources.MENUME[0], 1100011);
		cmdSkill = new Command(mResources.MENUME[1], 1100012);
		cmdTiemnang = new Command(mResources.MENUME[2], 1100013);
		cmdInfo = new Command(mResources.MENUME[3], 1100014);
		cmdtrangbi = new Command(mResources.MENUME[4], 1100015);
		MyVector myVector = new MyVector();
		myVector.addElement(cmdBag);
		myVector.addElement(cmdSkill);
		myVector.addElement(cmdTiemnang);
		myVector.addElement(cmdInfo);
		myVector.addElement(cmdtrangbi);
		GameCanvas.menu.startAt(myVector, 3);
	}

	public void doMenusynthesis()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command(mResources.SYNTHESIS[0], 110002));
		myVector.addElement(new Command(mResources.SYNTHESIS[1], 1100032));
		myVector.addElement(new Command(mResources.SYNTHESIS[2], 1100033));
		GameCanvas.menu.startAt(myVector, 3);
	}

	public static void loadCamera(bool fullmScreen, int cx, int cy)
	{
		gW = GameCanvas.w;
		cmdBarH = 39;
		gH = GameCanvas.h;
		cmdBarW = gW;
		cmdBarX = 0;
		cmdBarY = GameCanvas.h - Paint.hTab - cmdBarH;
		girlHPBarY = 0;
		csPadMaxH = GameCanvas.h / 6;
		if (csPadMaxH < 48)
		{
			csPadMaxH = 48;
		}
		gW2 = gW >> 1;
		gH2 = gH >> 1;
		gW3 = gW / 3;
		gH3 = gH / 3;
		gW23 = gH - 120;
		gH23 = gH * 2 / 3;
		gW34 = 3 * gW / 4;
		gH34 = 3 * gH / 4;
		gW6 = gW / 6;
		gH6 = gH / 6;
		gssw = gW / TileMap.size + 2;
		gssh = gH / TileMap.size + 2;
		if (gW % 24 != 0)
		{
			gssw++;
		}
		cmxLim = (TileMap.tmw - 1) * TileMap.size - gW;
		cmyLim = (TileMap.tmh - 1) * TileMap.size - gH;
		if (cx == -1 && cy == -1)
		{
			cmx = (cmtoX = Char.myCharz().cx - gW2 + gW6 * Char.myCharz().cdir);
			cmy = (cmtoY = Char.myCharz().cy - gH23);
		}
		else
		{
			cmx = (cmtoX = cx - gW23 + gW6 * Char.myCharz().cdir);
			cmy = (cmtoY = cy - gH23);
		}
		firstY = cmy;
		if (cmx < 24)
		{
			cmx = (cmtoX = 24);
		}
		if (cmx > cmxLim)
		{
			cmx = (cmtoX = cmxLim);
		}
		if (cmy < 0)
		{
			cmy = (cmtoY = 0);
		}
		if (cmy > cmyLim)
		{
			cmy = (cmtoY = cmyLim);
		}
		gssx = cmx / TileMap.size - 1;
		if (gssx < 0)
		{
			gssx = 0;
		}
		gssy = cmy / TileMap.size;
		gssxe = gssx + gssw;
		gssye = gssy + gssh;
		if (gssy < 0)
		{
			gssy = 0;
		}
		if (gssye > TileMap.tmh - 1)
		{
			gssye = TileMap.tmh - 1;
		}
		TileMap.countx = (gssxe - gssx) * 4;
		if (TileMap.countx > TileMap.tmw)
		{
			TileMap.countx = TileMap.tmw;
		}
		TileMap.county = (gssye - gssy) * 4;
		if (TileMap.county > TileMap.tmh)
		{
			TileMap.county = TileMap.tmh;
		}
		TileMap.gssx = (Char.myCharz().cx - 2 * gW) / TileMap.size;
		if (TileMap.gssx < 0)
		{
			TileMap.gssx = 0;
		}
		TileMap.gssxe = TileMap.gssx + TileMap.countx;
		if (TileMap.gssxe > TileMap.tmw)
		{
			TileMap.gssxe = TileMap.tmw;
		}
		TileMap.gssy = (Char.myCharz().cy - 2 * gH) / TileMap.size;
		if (TileMap.gssy < 0)
		{
			TileMap.gssy = 0;
		}
		TileMap.gssye = TileMap.gssy + TileMap.county;
		if (TileMap.gssye > TileMap.tmh)
		{
			TileMap.gssye = TileMap.tmh;
		}
		ChatTextField.gI().parentScreen = instance;
		ChatTextField.gI().tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
		ChatTextField.gI().initChatTextField();
		if (GameCanvas.isTouch)
		{
			yTouchBar = gH - 88;
			xC = gW - 40;
			yC = 2;
			if (GameCanvas.w <= 240)
			{
				xC = gW - 35;
				yC = 5;
			}
			xF = gW - 55;
			yF = yTouchBar + 35;
			xTG = gW - 37;
			yTG = yTouchBar - 1;
			if (GameCanvas.w >= 450)
			{
				yTG -= 12;
				yHP -= 7;
				xF -= 10;
				yF -= 5;
				xTG -= 10;
			}
		}
		setSkillBarPosition();
		disXC = ((GameCanvas.w <= 200) ? 30 : 40);
		if (Rms.loadRMSInt("viewchat") == -1)
		{
			GameCanvas.panel.isViewChatServer = true;
		}
		else
		{
			GameCanvas.panel.isViewChatServer = Rms.loadRMSInt("viewchat") == 1;
		}
	}

	public static void setSkillBarPosition()
	{
		Skill[] array = ((!GameCanvas.isTouch) ? keySkill : onScreenSkill);
		xS = new int[array.Length];
		yS = new int[array.Length];
		if (GameCanvas.isTouchControlSmallScreen && isUseTouch)
		{
			xSkill = 23;
			ySkill = 52;
			padSkill = 5;
			for (int i = 0; i < xS.Length; i++)
			{
				xS[i] = i * (25 + padSkill);
				yS[i] = ySkill;
			}
			xHP = array.Length * (25 + padSkill);
			yHP = ySkill;
		}
		else
		{
			wSkill = 30;
			if (GameCanvas.w <= 320)
			{
				ySkill = gH - wSkill - 6;
				xSkill = gW2 - array.Length * wSkill / 2 - 25;
			}
			else
			{
				wSkill = 40;
				xSkill = 10;
				ySkill = GameCanvas.h - wSkill + 7;
			}
			for (int j = 0; j < xS.Length; j++)
			{
				xS[j] = j * wSkill;
				yS[j] = ySkill;
			}
			xHP = array.Length * wSkill;
			yHP = ySkill;
		}
		if (!GameCanvas.isTouch)
		{
			return;
		}
		xSkill = 17;
		ySkill = GameCanvas.h - 40;
		if (gamePad.isSmallGamePad && isAnalog == 1)
		{
			xHP = array.Length * wSkill;
			yHP = ySkill;
		}
		else
		{
			xHP = GameCanvas.w - 45;
			yHP = GameCanvas.h - 45;
		}
		setTouchBtn();
		for (int k = 0; k < xS.Length; k++)
		{
			xS[k] = k * wSkill;
			yS[k] = ySkill;
			if (mGraphics.zoomLevel == 1 && xS.Length > 5 && k >= xS.Length / 2)
			{
				xS[k] = (k - xS.Length / 2) * wSkill;
				yS[k] = ySkill - 32;
			}
		}
	}

	private static void updateCamera()
	{
		if (isPaintOther)
		{
			return;
		}
		if (cmx != cmtoX || cmy != cmtoY)
		{
			cmvx = cmtoX - cmx << 2;
			cmvy = cmtoY - cmy << 2;
			cmdx += cmvx;
			cmx += cmdx >> 4;
			cmdx &= 15;
			cmdy += cmvy;
			cmy += cmdy >> 4;
			cmdy &= 15;
			if (cmx < 24)
			{
				cmx = 24;
			}
			if (cmx > cmxLim)
			{
				cmx = cmxLim;
			}
			if (cmy < 0)
			{
				cmy = 0;
			}
			if (cmy > cmyLim)
			{
				cmy = cmyLim;
			}
		}
		gssx = cmx / TileMap.size - 1;
		if (gssx < 0)
		{
			gssx = 0;
		}
		gssy = cmy / TileMap.size;
		gssxe = gssx + gssw;
		gssye = gssy + gssh;
		if (gssy < 0)
		{
			gssy = 0;
		}
		if (gssye > TileMap.tmh - 1)
		{
			gssye = TileMap.tmh - 1;
		}
		TileMap.gssx = (Char.myCharz().cx - 2 * gW) / TileMap.size;
		if (TileMap.gssx < 0)
		{
			TileMap.gssx = 0;
		}
		TileMap.gssxe = TileMap.gssx + TileMap.countx;
		if (TileMap.gssxe > TileMap.tmw)
		{
			TileMap.gssxe = TileMap.tmw;
			TileMap.gssx = TileMap.gssxe - TileMap.countx;
		}
		TileMap.gssy = (Char.myCharz().cy - 2 * gH) / TileMap.size;
		if (TileMap.gssy < 0)
		{
			TileMap.gssy = 0;
		}
		TileMap.gssye = TileMap.gssy + TileMap.county;
		if (TileMap.gssye > TileMap.tmh)
		{
			TileMap.gssye = TileMap.tmh;
			TileMap.gssy = TileMap.gssye - TileMap.county;
		}
		scrMain.updatecm();
		scrInfo.updatecm();
	}

	public bool testAct()
	{
		for (sbyte b = 2; b < 9; b += 2)
		{
			if (GameCanvas.keyHold[b])
			{
				return false;
			}
		}
		return true;
	}

	public void clanInvite(string strInvite, int clanID, int code)
	{
		ClanObject clanObject = new ClanObject();
		clanObject.code = code;
		clanObject.clanID = clanID;
		startYesNoPopUp(strInvite, new Command(mResources.YES, 12002, clanObject), new Command(mResources.NO, 12003, clanObject));
	}

	public void playerMenu(Char c)
	{
		auto = 0;
		GameCanvas.clearKeyHold();
		if (Char.myCharz().charFocus.charID < 0 || Char.myCharz().charID < 0)
		{
			return;
		}
		MyVector vPlayerMenu = GameCanvas.panel.vPlayerMenu;
		if (vPlayerMenu.size() > 0)
		{
			return;
		}
		if (Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId > 1)
		{
			vPlayerMenu.addElement(new Command(mResources.make_friend, 11112, Char.myCharz().charFocus));
			vPlayerMenu.addElement(new Command(mResources.trade, 11113, Char.myCharz().charFocus));
		}
		if (Char.myCharz().clan != null && Char.myCharz().role < 2 && Char.myCharz().charFocus.clanID == -1)
		{
			vPlayerMenu.addElement(new Command(mResources.CHAR_ORDER[4], 110391));
		}
		if (Char.myCharz().charFocus.statusMe != 14 && Char.myCharz().charFocus.statusMe != 5)
		{
			if (Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId >= 14)
			{
				vPlayerMenu.addElement(new Command(mResources.CHAR_ORDER[0], 2003));
			}
		}
		else
		{
			_ = Char.myCharz().myskill.template.type;
		}
		if (Char.myCharz().clan != null && Char.myCharz().clan.ID == Char.myCharz().charFocus.clanID && Char.myCharz().charFocus.statusMe != 14 && Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId >= 14)
		{
			vPlayerMenu.addElement(new Command(mResources.CHAR_ORDER[1], 2004));
		}
		int num = Char.myCharz().nClass.skillTemplates.Length;
		for (int i = 0; i < num; i++)
		{
			SkillTemplate skillTemplate = Char.myCharz().nClass.skillTemplates[i];
			Skill skill = Char.myCharz().getSkill(skillTemplate);
			if (skill != null && skillTemplate.isBuffToPlayer() && skill.point >= 1)
			{
				vPlayerMenu.addElement(new Command(skillTemplate.name, 12004, skill));
			}
		}
	}

	public bool isAttack()
	{
		if (checkClickToBotton(Char.myCharz().charFocus))
		{
			return false;
		}
		if (checkClickToBotton(Char.myCharz().mobFocus))
		{
			return false;
		}
		if (checkClickToBotton(Char.myCharz().npcFocus))
		{
			return false;
		}
		if (ChatTextField.gI().isShow)
		{
			return false;
		}
		if (InfoDlg.isLock || Char.myCharz().isLockAttack || Char.isLockKey)
		{
			return false;
		}
		if (Char.myCharz().myskill != null && Char.myCharz().myskill.template.id == 6 && Char.myCharz().itemFocus != null)
		{
			pickItem();
			return false;
		}
		if (Char.myCharz().myskill != null && Char.myCharz().myskill.template.type == 2 && Char.myCharz().npcFocus == null && Char.myCharz().myskill.template.id != 6)
		{
			return checkSkillValid();
		}
		if (Char.myCharz().skillPaint != null || (Char.myCharz().mobFocus == null && Char.myCharz().npcFocus == null && Char.myCharz().charFocus == null && Char.myCharz().itemFocus == null))
		{
			return false;
		}
		if (Char.myCharz().mobFocus != null)
		{
			if (Char.myCharz().mobFocus.isBigBoss() && Char.myCharz().mobFocus.status == 4)
			{
				Char.myCharz().mobFocus = null;
				Char.myCharz().currentMovePoint = null;
			}
			isAutoPlay = true;
			if (!isMeCanAttackMob(Char.myCharz().mobFocus))
			{
				Res.outz("can not attack");
				return false;
			}
			if (mobCapcha != null)
			{
				return false;
			}
			if (Char.myCharz().myskill == null)
			{
				return false;
			}
			if (Char.myCharz().isSelectingSkillUseAlone())
			{
				return false;
			}
			int num = -1;
			int num2 = Res.abs(Char.myCharz().cx - cmx) * mGraphics.zoomLevel;
			if (Char.myCharz().charFocus != null)
			{
				num = Res.abs(Char.myCharz().cx - Char.myCharz().charFocus.cx) * mGraphics.zoomLevel;
			}
			else if (Char.myCharz().mobFocus != null)
			{
				num = Res.abs(Char.myCharz().cx - Char.myCharz().mobFocus.x) * mGraphics.zoomLevel;
			}
			if ((Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0 || Char.myCharz().myskill.template.type == 4 || num == -1 || num > num2) && Char.myCharz().myskill.template.type == 4)
			{
				if (Char.myCharz().mobFocus.x < Char.myCharz().cx)
				{
					Char.myCharz().cdir = -1;
				}
				else
				{
					Char.myCharz().cdir = 1;
				}
				doSelectSkill(Char.myCharz().myskill, isShortcut: true);
			}
			if (!checkSkillValid())
			{
				return false;
			}
			if (Char.myCharz().cx < Char.myCharz().mobFocus.getX())
			{
				Char.myCharz().cdir = 1;
			}
			else
			{
				Char.myCharz().cdir = -1;
			}
			int num3 = Math.Abs(Char.myCharz().cx - Char.myCharz().mobFocus.getX());
			int num4 = Math.Abs(Char.myCharz().cy - Char.myCharz().mobFocus.getY());
			Char.myCharz().cvx = 0;
			if (num3 > Char.myCharz().myskill.dx || num4 > Char.myCharz().myskill.dy)
			{
				bool flag = false;
				if (Char.myCharz().mobFocus is BigBoss || Char.myCharz().mobFocus is BigBoss2)
				{
					flag = true;
				}
				int num5 = (Char.myCharz().myskill.dx - ((!flag) ? 20 : 50)) * ((Char.myCharz().cx > Char.myCharz().mobFocus.getX()) ? 1 : (-1));
				if (num3 <= Char.myCharz().myskill.dx)
				{
					num5 = 0;
				}
				Char.myCharz().currentMovePoint = new MovePoint(Char.myCharz().mobFocus.getX() + num5, Char.myCharz().mobFocus.getY());
				Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				return false;
			}
			if (Char.myCharz().myskill.template.id == 20)
			{
				return true;
			}
			if (num4 > num3 && Res.abs(Char.myCharz().cy - Char.myCharz().mobFocus.getY()) > 30 && Char.myCharz().mobFocus.getTemplate().type == 4)
			{
				Char.myCharz().currentMovePoint = new MovePoint(Char.myCharz().cx + Char.myCharz().cdir, Char.myCharz().mobFocus.getY());
				Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				return false;
			}
			int num6 = 20;
			bool flag2 = false;
			if (Char.myCharz().mobFocus is BigBoss || Char.myCharz().mobFocus is BigBoss2)
			{
				flag2 = true;
			}
			if (Char.myCharz().myskill.dx > 100)
			{
				num6 = 60;
				if (num3 < 20)
				{
					Char.myCharz().createShadow(Char.myCharz().cx, Char.myCharz().cy, 10);
				}
			}
			bool flag3 = false;
			if ((TileMap.tileTypeAtPixel(Char.myCharz().cx, Char.myCharz().cy + 3) & 2) == 2)
			{
				int num7 = ((Char.myCharz().cx > Char.myCharz().mobFocus.getX()) ? 1 : (-1));
				if ((TileMap.tileTypeAtPixel(Char.myCharz().mobFocus.getX() + num6 * num7, Char.myCharz().cy + 3) & 2) != 2)
				{
					flag3 = true;
				}
			}
			if (num3 <= num6 && !flag3)
			{
				if (Char.myCharz().cx > Char.myCharz().mobFocus.getX())
				{
					int num8 = Char.myCharz().mobFocus.getX() + num6 + (flag2 ? 30 : 0);
					int i = Char.myCharz().mobFocus.getX();
					bool flag4 = false;
					for (; i < num8; i += 24)
					{
						if (TileMap.tileTypeAtPixel(i, Char.myCharz().cy + 3) == 8 || TileMap.tileTypeAtPixel(i, Char.myCharz().cy + 3) == 4)
						{
							flag4 = true;
							break;
						}
					}
					if (flag4)
					{
						Char.myCharz().cx = i - 24;
					}
					else
					{
						Char.myCharz().cx = num8;
					}
					Char.myCharz().cdir = -1;
				}
				else
				{
					int num9 = Char.myCharz().mobFocus.getX() - num6 - (flag2 ? 30 : 0);
					int num10 = Char.myCharz().mobFocus.getX();
					bool flag5 = false;
					while (num10 > num9)
					{
						if (TileMap.tileTypeAtPixel(num10, Char.myCharz().cy + 3) == 8 || TileMap.tileTypeAtPixel(num10, Char.myCharz().cy + 3) == 4)
						{
							flag5 = true;
							break;
						}
						num10 -= 24;
					}
					if (flag5)
					{
						Char.myCharz().cx = num10 + 24;
					}
					else
					{
						Char.myCharz().cx = num9;
					}
					Char.myCharz().cdir = 1;
				}
				Service.gI().charMove();
			}
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
			return true;
		}
		if (Char.myCharz().npcFocus != null)
		{
			if (Char.myCharz().npcFocus.isHide)
			{
				return false;
			}
			if (Char.myCharz().cx < Char.myCharz().npcFocus.cx)
			{
				Char.myCharz().cdir = 1;
			}
			else
			{
				Char.myCharz().cdir = -1;
			}
			if (Char.myCharz().cx < Char.myCharz().npcFocus.cx)
			{
				Char.myCharz().npcFocus.cdir = -1;
			}
			else
			{
				Char.myCharz().npcFocus.cdir = 1;
			}
			int num11 = Math.Abs(Char.myCharz().cx - Char.myCharz().npcFocus.cx);
			if (Math.Abs(Char.myCharz().cy - Char.myCharz().npcFocus.cy) > 40)
			{
				Char.myCharz().cy = Char.myCharz().npcFocus.cy - 40;
			}
			if (num11 < 60)
			{
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				if (tMenuDelay == 0)
				{
					if (Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.taskId == 0)
					{
						if (Char.myCharz().taskMaint.index < 4 && Char.myCharz().npcFocus.template.npcTemplateId == 4)
						{
							return false;
						}
						if (Char.myCharz().taskMaint.index < 3 && Char.myCharz().npcFocus.template.npcTemplateId == 3)
						{
							return false;
						}
					}
					tMenuDelay = 50;
					InfoDlg.showWait();
					Service.gI().charMove();
					Service.gI().openMenu(Char.myCharz().npcFocus.template.npcTemplateId);
				}
			}
			else
			{
				int num12 = (20 + Res.r.nextInt(20)) * ((Char.myCharz().cx > Char.myCharz().npcFocus.cx) ? 1 : (-1));
				Char.myCharz().currentMovePoint = new MovePoint(Char.myCharz().npcFocus.cx + num12, Char.myCharz().cy);
				Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
			}
			return false;
		}
		if (Char.myCharz().charFocus != null)
		{
			if (mobCapcha != null)
			{
				return false;
			}
			if (Char.myCharz().cx < Char.myCharz().charFocus.cx)
			{
				Char.myCharz().cdir = 1;
			}
			else
			{
				Char.myCharz().cdir = -1;
			}
			int num13 = Math.Abs(Char.myCharz().cx - Char.myCharz().charFocus.cx);
			int num14 = Math.Abs(Char.myCharz().cy - Char.myCharz().charFocus.cy);
			if (!Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus) && !Char.myCharz().isSelectingSkillBuffToPlayer())
			{
				if (num13 < 60 && num14 < 40)
				{
					playerMenu(Char.myCharz().charFocus);
					if (!GameCanvas.isTouch && Char.myCharz().charFocus.charID >= 0 && TileMap.mapID != 51 && TileMap.mapID != 52 && popUpYesNo == null)
					{
						GameCanvas.panel.setTypePlayerMenu(Char.myCharz().charFocus);
						GameCanvas.panel.show();
						Service.gI().getPlayerMenu(Char.myCharz().charFocus.charID);
						Service.gI().messagePlayerMenu(Char.myCharz().charFocus.charID);
					}
				}
				else
				{
					int num15 = (20 + Res.r.nextInt(20)) * ((Char.myCharz().cx > Char.myCharz().charFocus.cx) ? 1 : (-1));
					Char.myCharz().currentMovePoint = new MovePoint(Char.myCharz().charFocus.cx + num15, Char.myCharz().charFocus.cy);
					Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
					GameCanvas.clearKeyHold();
					GameCanvas.clearKeyPressed();
				}
				return false;
			}
			if (Char.myCharz().myskill == null)
			{
				return false;
			}
			if (!checkSkillValid())
			{
				return false;
			}
			if (Char.myCharz().cx < Char.myCharz().charFocus.cx)
			{
				Char.myCharz().cdir = 1;
			}
			else
			{
				Char.myCharz().cdir = -1;
			}
			Char.myCharz().cvx = 0;
			if (num13 > Char.myCharz().myskill.dx || num14 > Char.myCharz().myskill.dy)
			{
				int num16 = (Char.myCharz().myskill.dx - 20) * ((Char.myCharz().cx > Char.myCharz().charFocus.cx) ? 1 : (-1));
				if (num13 <= Char.myCharz().myskill.dx)
				{
					num16 = 0;
				}
				Char.myCharz().currentMovePoint = new MovePoint(Char.myCharz().charFocus.cx + num16, Char.myCharz().charFocus.cy);
				Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
				GameCanvas.clearKeyHold();
				GameCanvas.clearKeyPressed();
				return false;
			}
			if (Char.myCharz().myskill.template.id == 20)
			{
				return true;
			}
			int num17 = 20;
			if (Char.myCharz().myskill.dx > 60)
			{
				num17 = 60;
				if (num13 < 20)
				{
					Char.myCharz().createShadow(Char.myCharz().cx, Char.myCharz().cy, 10);
				}
			}
			bool flag6 = false;
			if ((TileMap.tileTypeAtPixel(Char.myCharz().cx, Char.myCharz().cy + 3) & 2) == 2)
			{
				int num18 = ((Char.myCharz().cx > Char.myCharz().charFocus.cx) ? 1 : (-1));
				if ((TileMap.tileTypeAtPixel(Char.myCharz().charFocus.cx + num17 * num18, Char.myCharz().cy + 3) & 2) != 2)
				{
					flag6 = true;
				}
			}
			if (num13 <= num17 && !flag6)
			{
				if (Char.myCharz().cx > Char.myCharz().charFocus.cx)
				{
					Char.myCharz().cx = Char.myCharz().charFocus.cx + num17;
					Char.myCharz().cdir = -1;
				}
				else
				{
					Char.myCharz().cx = Char.myCharz().charFocus.cx - num17;
					Char.myCharz().cdir = 1;
				}
				Service.gI().charMove();
			}
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
			return true;
		}
		if (Char.myCharz().itemFocus != null)
		{
			pickItem();
			return false;
		}
		return true;
	}

	public bool isMeCanAttackMob(Mob m)
	{
		if (m == null)
		{
			return false;
		}
		if (Char.myCharz().cTypePk == 5)
		{
			return true;
		}
		if (Char.myCharz().isAttacPlayerStatus() && !m.isMobMe)
		{
			return false;
		}
		if (Char.myCharz().mobMe != null && m.Equals(Char.myCharz().mobMe))
		{
			return false;
		}
		Char obj = findCharInMap(m.mobId);
		return obj == null || obj.cTypePk == 5 || Char.myCharz().isMeCanAttackOtherPlayer(obj);
	}

	private bool checkSkillValid()
	{
		if (Char.myCharz().myskill != null && ((Char.myCharz().myskill.template.manaUseType != 1 && Char.myCharz().cMP < Char.myCharz().myskill.manaUse) || (Char.myCharz().myskill.template.manaUseType == 1 && Char.myCharz().cMP < Char.myCharz().cMPFull * Char.myCharz().myskill.manaUse / 100)))
		{
			info1.addInfo(mResources.NOT_ENOUGH_MP);
			auto = 0;
			return false;
		}
		if (Char.myCharz().myskill == null || (Char.myCharz().myskill.template.maxPoint > 0 && Char.myCharz().myskill.point == 0))
		{
			GameCanvas.startOKDlg(mResources.SKILL_FAIL);
			return false;
		}
		return true;
	}

	private bool checkSkillValid2()
	{
		if ((Char.myCharz().myskill == null || ((Char.myCharz().myskill.template.manaUseType == 1 || Char.myCharz().cMP >= Char.myCharz().myskill.manaUse) && (Char.myCharz().myskill.template.manaUseType != 1 || Char.myCharz().cMP >= Char.myCharz().cMPFull * Char.myCharz().myskill.manaUse / 100))) && Char.myCharz().myskill != null)
		{
			if (Char.myCharz().myskill.template.maxPoint > 0)
			{
				return Char.myCharz().myskill.point != 0;
			}
			return true;
		}
		return false;
	}

	public void resetButton()
	{
		GameCanvas.menu.showMenu = false;
		ChatTextField.gI().close();
		ChatTextField.gI().center = null;
		isLockKey = false;
		typeTrade = 0;
		indexMenu = 0;
		indexSelect = 0;
		indexItemUse = -1;
		indexRow = -1;
		indexRowMax = 0;
		indexTitle = 0;
		typeTrade = (typeTradeOrder = 0);
		mSystem.endKey();
		if (Char.myCharz().cHP <= 0 || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
		{
			if (Char.myCharz().meDead)
			{
				cmdDead = new Command(mResources.DIES[0], 11038);
				center = cmdDead;
				Char.myCharz().cHP = 0L;
			}
			isHaveSelectSkill = false;
		}
		else
		{
			isHaveSelectSkill = true;
		}
		scrMain.clear();
	}

	public override void keyPress(int keyCode)
	{
		base.keyPress(keyCode);
	}

	public override void updateKey()
	{
		if (Controller.isStopReadMessage || Char.myCharz().isTeleport || Char.myCharz().isPaintNewSkill || InfoDlg.isLock)
		{
			return;
		}
		if (GameCanvas.isTouch && !ChatTextField.gI().isShow && !GameCanvas.menu.showMenu)
		{
			updateKeyTouchControl();
		}
		checkAuto();
		GameCanvas.debug("F2", 0);
		if (ChatPopup.currChatPopup != null)
		{
			Command cmdNextLine = ChatPopup.currChatPopup.cmdNextLine;
			if ((GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(cmdNextLine)) && cmdNextLine != null)
			{
				GameCanvas.isPointerJustRelease = false;
				GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
				mScreen.keyTouch = -1;
				cmdNextLine?.performAction();
			}
		}
		else if (!ChatTextField.gI().isShow)
		{
			if ((GameCanvas.keyPressed[12] || mScreen.getCmdPointerLast(GameCanvas.currentScreen.left)) && left != null)
			{
				GameCanvas.isPointerJustRelease = false;
				GameCanvas.isPointerClick = false;
				GameCanvas.keyPressed[12] = false;
				mScreen.keyTouch = -1;
				if (left != null)
				{
					left.performAction();
				}
			}
			if ((GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(GameCanvas.currentScreen.right)) && right != null)
			{
				GameCanvas.isPointerJustRelease = false;
				GameCanvas.isPointerClick = false;
				GameCanvas.keyPressed[13] = false;
				mScreen.keyTouch = -1;
				if (right != null)
				{
					right.performAction();
				}
			}
			if ((GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(GameCanvas.currentScreen.center)) && center != null)
			{
				GameCanvas.isPointerJustRelease = false;
				GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
				mScreen.keyTouch = -1;
				if (center != null)
				{
					center.performAction();
				}
			}
		}
		else
		{
			if (ChatTextField.gI().left != null && (GameCanvas.keyPressed[12] || mScreen.getCmdPointerLast(ChatTextField.gI().left)) && ChatTextField.gI().left != null)
			{
				ChatTextField.gI().left.performAction();
			}
			if (ChatTextField.gI().right != null && (GameCanvas.keyPressed[13] || mScreen.getCmdPointerLast(ChatTextField.gI().right)) && ChatTextField.gI().right != null)
			{
				ChatTextField.gI().right.performAction();
			}
			if (ChatTextField.gI().center != null && (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] || mScreen.getCmdPointerLast(ChatTextField.gI().center)) && ChatTextField.gI().center != null)
			{
				ChatTextField.gI().center.performAction();
			}
		}
		GameCanvas.debug("F6", 0);
		updateKeyAlert();
		GameCanvas.debug("F7", 0);
		if (Char.myCharz().currentMovePoint != null)
		{
			for (int i = 0; i < GameCanvas.keyPressed.Length; i++)
			{
				if (GameCanvas.keyPressed[i])
				{
					Char.myCharz().currentMovePoint = null;
					break;
				}
			}
		}
		GameCanvas.debug("F8", 0);
		if (ChatTextField.gI().isShow && GameCanvas.keyAsciiPress != 0)
		{
			ChatTextField.gI().keyPressed(GameCanvas.keyAsciiPress);
			GameCanvas.keyAsciiPress = 0;
		}
		else if (isLockKey)
		{
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
		}
		else
		{
			if (GameCanvas.menu.showMenu || isOpenUI() || Char.isLockKey)
			{
				return;
			}
			if (GameCanvas.keyPressed[10])
			{
				GameCanvas.keyPressed[10] = false;
				doUseHP();
				GameCanvas.clearKeyPressed();
			}
			if (GameCanvas.keyPressed[11] && mobCapcha == null)
			{
				if (popUpYesNo != null)
				{
					popUpYesNo.cmdYes.performAction();
				}
				else if (info2.info.info != null && info2.info.info.charInfo != null)
				{
					GameCanvas.panel.setTypeMessage();
					GameCanvas.panel.show();
				}
				GameCanvas.keyPressed[11] = false;
				GameCanvas.clearKeyPressed();
			}
			if (GameCanvas.keyAsciiPress != 0 && TField.isQwerty && GameCanvas.keyAsciiPress == 32)
			{
				doUseHP();
				GameCanvas.keyAsciiPress = 0;
				GameCanvas.clearKeyPressed();
			}
			if (GameCanvas.keyAsciiPress != 0 && mobCapcha == null && TField.isQwerty && GameCanvas.keyAsciiPress == 121)
			{
				if (popUpYesNo != null)
				{
					popUpYesNo.cmdYes.performAction();
					GameCanvas.keyAsciiPress = 0;
					GameCanvas.clearKeyPressed();
				}
				else if (info2.info.info != null && info2.info.info.charInfo != null)
				{
					GameCanvas.panel.setTypeMessage();
					GameCanvas.panel.show();
					GameCanvas.keyAsciiPress = 0;
					GameCanvas.clearKeyPressed();
				}
			}
			if (GameCanvas.keyPressed[10] && mobCapcha == null)
			{
				GameCanvas.keyPressed[10] = false;
				info2.doClick(10);
				GameCanvas.clearKeyPressed();
			}
			checkDrag();
			if (!Char.myCharz().isFlyAndCharge)
			{
				checkClick();
			}
			if (Char.myCharz().cmdMenu != null && Char.myCharz().cmdMenu.isPointerPressInside())
			{
				Char.myCharz().cmdMenu.performAction();
			}
			if (Char.myCharz().skillPaint != null)
			{
				return;
			}
			if (GameCanvas.keyAsciiPress != 0)
			{
				if (mobCapcha == null)
				{
					if (TField.isQwerty)
					{
						if (GameCanvas.keyPressed[1])
						{
							if (keySkill[0] != null)
							{
								doSelectSkill(keySkill[0], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[2])
						{
							if (keySkill[1] != null)
							{
								doSelectSkill(keySkill[1], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[3])
						{
							if (keySkill[2] != null)
							{
								doSelectSkill(keySkill[2], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[4])
						{
							if (keySkill[3] != null)
							{
								doSelectSkill(keySkill[3], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[5])
						{
							if (keySkill[4] != null)
							{
								doSelectSkill(keySkill[4], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[6])
						{
							if (keySkill[5] != null)
							{
								doSelectSkill(keySkill[5], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[7])
						{
							if (keySkill[6] != null)
							{
								doSelectSkill(keySkill[6], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[8])
						{
							if (keySkill[7] != null)
							{
								doSelectSkill(keySkill[7], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[9])
						{
							if (keySkill[8] != null)
							{
								doSelectSkill(keySkill[8], isShortcut: true);
							}
						}
						else if (GameCanvas.keyPressed[0])
						{
							if (keySkill[9] != null)
							{
								doSelectSkill(keySkill[9], isShortcut: true);
							}
						}
						else if (GameCanvas.keyAsciiPress == 114)
						{
							ChatTextField.gI().startChat(this, string.Empty);
						}
						else if (MainMod.UpdateKey(GameCanvas.keyAsciiPress) && DataItem.IsShow)
						{
						}
					}
					else if (!GameCanvas.isMoveNumberPad)
					{
						ChatTextField.gI().startChat(GameCanvas.keyAsciiPress, this, string.Empty);
					}
					else if (GameCanvas.keyAsciiPress == 55)
					{
						if (keySkill[0] != null)
						{
							doSelectSkill(keySkill[0], isShortcut: true);
						}
					}
					else if (GameCanvas.keyAsciiPress == 56)
					{
						if (keySkill[1] != null)
						{
							doSelectSkill(keySkill[1], isShortcut: true);
						}
					}
					else if (GameCanvas.keyAsciiPress == 57)
					{
						if (keySkill[(!Main.isPC) ? 2 : 21] != null)
						{
							doSelectSkill(keySkill[2], isShortcut: true);
						}
					}
					else if (GameCanvas.keyAsciiPress == 48)
					{
						ChatTextField.gI().startChat(this, string.Empty);
					}
				}
				else
				{
					char[] array = keyInput.ToCharArray();
					MyVector myVector = new MyVector();
					for (int j = 0; j < array.Length; j++)
					{
						myVector.addElement(array[j] + string.Empty);
					}
					myVector.removeElementAt(0);
					string text = (char)GameCanvas.keyAsciiPress + string.Empty;
					if (text.Equals(string.Empty) || text == null || text.Equals("\n"))
					{
						text = "-";
					}
					myVector.insertElementAt(text, myVector.size());
					keyInput = string.Empty;
					for (int k = 0; k < myVector.size(); k++)
					{
						keyInput += ((string)myVector.elementAt(k)).ToUpper();
					}
					Service.gI().mobCapcha((char)GameCanvas.keyAsciiPress);
				}
				GameCanvas.keyAsciiPress = 0;
			}
			if (Char.myCharz().statusMe == 1)
			{
				GameCanvas.debug("F10", 0);
				if (!doSeleckSkillFlag)
				{
					if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
					{
						GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
						doFire(isFireByShortCut: false, skipWaypoint: false);
					}
					else if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21])
					{
						if (!Char.myCharz().isLockMove)
						{
							setCharJump(0);
						}
					}
					else if (GameCanvas.keyHold[1] && mobCapcha == null)
					{
						if (!Main.isPC)
						{
							Char.myCharz().cdir = -1;
							if (!Char.myCharz().isLockMove)
							{
								setCharJump(-4);
							}
						}
					}
					else if (GameCanvas.keyHold[(!Main.isPC) ? 5 : 25] && mobCapcha == null)
					{
						if (!Main.isPC)
						{
							Char.myCharz().cdir = 1;
							if (!Char.myCharz().isLockMove)
							{
								setCharJump(4);
							}
						}
					}
					else if (GameCanvas.keyHold[(!Main.isPC) ? 4 : 23])
					{
						isAutoPlay = false;
						Char.myCharz().isAttack = false;
						if (Char.myCharz().cdir == 1)
						{
							Char.myCharz().cdir = -1;
						}
						else if (!Char.myCharz().isLockMove)
						{
							if (Char.myCharz().cx - Char.myCharz().cxSend != 0)
							{
								Service.gI().charMove();
							}
							Char.myCharz().statusMe = 2;
							Char.myCharz().cvx = -Char.myCharz().cspeed;
						}
						Char.myCharz().holder = false;
					}
					else if (GameCanvas.keyHold[(!Main.isPC) ? 6 : 24])
					{
						isAutoPlay = false;
						Char.myCharz().isAttack = false;
						if (Char.myCharz().cdir == -1)
						{
							Char.myCharz().cdir = 1;
						}
						else if (!Char.myCharz().isLockMove)
						{
							if (Char.myCharz().cx - Char.myCharz().cxSend != 0)
							{
								Service.gI().charMove();
							}
							Char.myCharz().statusMe = 2;
							Char.myCharz().cvx = Char.myCharz().cspeed;
						}
						Char.myCharz().holder = false;
					}
				}
			}
			else if (Char.myCharz().statusMe == 2)
			{
				GameCanvas.debug("F11", 0);
				if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
				{
					GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
					doFire(isFireByShortCut: false, skipWaypoint: true);
				}
				else if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21])
				{
					if (Char.myCharz().cx - Char.myCharz().cxSend != 0 || Char.myCharz().cy - Char.myCharz().cySend != 0)
					{
						Service.gI().charMove();
					}
					Char.myCharz().cvy = -10;
					Char.myCharz().statusMe = 3;
					Char.myCharz().cp1 = 0;
				}
				else if (GameCanvas.keyHold[1] && mobCapcha == null)
				{
					if (Main.isPC)
					{
						if (Char.myCharz().cx - Char.myCharz().cxSend != 0 || Char.myCharz().cy - Char.myCharz().cySend != 0)
						{
							Service.gI().charMove();
						}
						Char.myCharz().cdir = -1;
						Char.myCharz().cvy = -10;
						Char.myCharz().cvx = -4;
						Char.myCharz().statusMe = 3;
						Char.myCharz().cp1 = 0;
					}
				}
				else if (GameCanvas.keyHold[3] && mobCapcha == null)
				{
					if (!Main.isPC)
					{
						if (Char.myCharz().cx - Char.myCharz().cxSend != 0 || Char.myCharz().cy - Char.myCharz().cySend != 0)
						{
							Service.gI().charMove();
						}
						Char.myCharz().cdir = 1;
						Char.myCharz().cvy = -10;
						Char.myCharz().cvx = 4;
						Char.myCharz().statusMe = 3;
						Char.myCharz().cp1 = 0;
					}
				}
				else if (GameCanvas.keyHold[(!Main.isPC) ? 4 : 23])
				{
					isAutoPlay = false;
					if (Char.myCharz().cdir == 1)
					{
						Char.myCharz().cdir = -1;
					}
					else
					{
						Char.myCharz().cvx = -Char.myCharz().cspeed + Char.myCharz().cBonusSpeed;
					}
				}
				else if (GameCanvas.keyHold[(!Main.isPC) ? 6 : 24])
				{
					isAutoPlay = false;
					if (Char.myCharz().cdir == -1)
					{
						Char.myCharz().cdir = 1;
					}
					else
					{
						Char.myCharz().cvx = Char.myCharz().cspeed + Char.myCharz().cBonusSpeed;
					}
				}
			}
			else if (Char.myCharz().statusMe == 3)
			{
				isAutoPlay = false;
				GameCanvas.debug("F12", 0);
				if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
				{
					GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
					doFire(isFireByShortCut: false, skipWaypoint: true);
				}
				if (GameCanvas.keyHold[(!Main.isPC) ? 4 : 23] || (GameCanvas.keyHold[1] && mobCapcha == null))
				{
					if (Char.myCharz().cdir == 1)
					{
						Char.myCharz().cdir = -1;
					}
					else
					{
						Char.myCharz().cvx = -Char.myCharz().cspeed;
					}
				}
				else if (GameCanvas.keyHold[(!Main.isPC) ? 6 : 24] || (GameCanvas.keyHold[3] && mobCapcha == null))
				{
					if (Char.myCharz().cdir == -1)
					{
						Char.myCharz().cdir = 1;
					}
					else
					{
						Char.myCharz().cvx = Char.myCharz().cspeed;
					}
				}
				if ((GameCanvas.keyHold[(!Main.isPC) ? 2 : 21] || ((GameCanvas.keyHold[1] || GameCanvas.keyHold[3]) && mobCapcha == null)) && Char.myCharz().canFly && Char.myCharz().cMP > 0 && Char.myCharz().cp1 < 8 && Char.myCharz().cvy > -4)
				{
					Char.myCharz().cp1++;
					Char.myCharz().cvy = -7;
				}
			}
			else if (Char.myCharz().statusMe == 4)
			{
				GameCanvas.debug("F13", 0);
				if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
				{
					GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
					doFire(isFireByShortCut: false, skipWaypoint: true);
				}
				if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21] && Char.myCharz().cMP > 0 && Char.myCharz().canFly)
				{
					isAutoPlay = false;
					if ((Char.myCharz().cx - Char.myCharz().cxSend != 0 || Char.myCharz().cy - Char.myCharz().cySend != 0) && (Res.abs(Char.myCharz().cx - Char.myCharz().cxSend) > 96 || Res.abs(Char.myCharz().cy - Char.myCharz().cySend) > 24))
					{
						Service.gI().charMove();
					}
					Char.myCharz().cvy = -10;
					Char.myCharz().statusMe = 3;
					Char.myCharz().cp1 = 0;
				}
				if (GameCanvas.keyHold[(!Main.isPC) ? 4 : 23])
				{
					isAutoPlay = false;
					if (Char.myCharz().cdir == 1)
					{
						Char.myCharz().cdir = -1;
					}
					else
					{
						Char.myCharz().cp1++;
						Char.myCharz().cvx = -Char.myCharz().cspeed;
						if (Char.myCharz().cp1 > 5 && Char.myCharz().cvy > 6)
						{
							Char.myCharz().statusMe = 10;
							Char.myCharz().cp1 = 0;
							Char.myCharz().cvy = 0;
						}
					}
				}
				else if (GameCanvas.keyHold[(!Main.isPC) ? 6 : 24])
				{
					isAutoPlay = false;
					if (Char.myCharz().cdir == -1)
					{
						Char.myCharz().cdir = 1;
					}
					else
					{
						Char.myCharz().cp1++;
						Char.myCharz().cvx = Char.myCharz().cspeed;
						if (Char.myCharz().cp1 > 5 && Char.myCharz().cvy > 6)
						{
							Char.myCharz().statusMe = 10;
							Char.myCharz().cp1 = 0;
							Char.myCharz().cvy = 0;
						}
					}
				}
			}
			else if (Char.myCharz().statusMe == 10)
			{
				GameCanvas.debug("F14", 0);
				if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
				{
					GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
					doFire(isFireByShortCut: false, skipWaypoint: true);
				}
				if (Char.myCharz().canFly && Char.myCharz().cMP > 0)
				{
					if (GameCanvas.keyHold[(!Main.isPC) ? 2 : 21])
					{
						isAutoPlay = false;
						if ((Char.myCharz().cx - Char.myCharz().cxSend != 0 || Char.myCharz().cy - Char.myCharz().cySend != 0) && (Res.abs(Char.myCharz().cx - Char.myCharz().cxSend) > 96 || Res.abs(Char.myCharz().cy - Char.myCharz().cySend) > 24))
						{
							Service.gI().charMove();
						}
						Char.myCharz().cvy = -10;
						Char.myCharz().statusMe = 3;
						Char.myCharz().cp1 = 0;
					}
					else if (GameCanvas.keyHold[(!Main.isPC) ? 4 : 23])
					{
						isAutoPlay = false;
						if (Char.myCharz().cdir == 1)
						{
							Char.myCharz().cdir = -1;
						}
						else
						{
							Char.myCharz().cvx = -(Char.myCharz().cspeed + 1);
						}
					}
					else if (GameCanvas.keyHold[(!Main.isPC) ? 6 : 24])
					{
						if (Char.myCharz().cdir == -1)
						{
							Char.myCharz().cdir = 1;
						}
						else
						{
							Char.myCharz().cvx = Char.myCharz().cspeed + 1;
						}
					}
				}
			}
			else if (Char.myCharz().statusMe == 7)
			{
				GameCanvas.debug("F15", 0);
				if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
				{
					GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
				}
				if (GameCanvas.keyHold[(!Main.isPC) ? 4 : 23])
				{
					isAutoPlay = false;
					if (Char.myCharz().cdir == 1)
					{
						Char.myCharz().cdir = -1;
					}
					else
					{
						Char.myCharz().cvx = -Char.myCharz().cspeed + 2;
					}
				}
				else if (GameCanvas.keyHold[(!Main.isPC) ? 6 : 24])
				{
					isAutoPlay = false;
					if (Char.myCharz().cdir == -1)
					{
						Char.myCharz().cdir = 1;
					}
					else
					{
						Char.myCharz().cvx = Char.myCharz().cspeed - 2;
					}
				}
			}
			GameCanvas.debug("F17", 0);
			if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] && GameCanvas.keyAsciiPress != 56)
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22] = false;
				Char.myCharz().delayFall = 0;
			}
			if (GameCanvas.keyPressed[10])
			{
				GameCanvas.keyPressed[10] = false;
				doUseHP();
			}
			GameCanvas.debug("F20", 0);
			GameCanvas.clearKeyPressed();
			GameCanvas.debug("F23", 0);
			doSeleckSkillFlag = false;
		}
	}

	public bool isVsMap()
	{
		return true;
	}

	private void checkDrag()
	{
		if (isAnalog == 1 || gamePad.disableCheckDrag())
		{
			return;
		}
		Char.myCharz().cmtoChar = true;
		if (isUseTouch)
		{
			return;
		}
		if (GameCanvas.isPointerJustDown)
		{
			GameCanvas.isPointerJustDown = false;
			isPointerDowning = true;
			ptDownTime = 0;
			ptLastDownX = (ptFirstDownX = GameCanvas.px);
			ptLastDownY = (ptFirstDownY = GameCanvas.py);
		}
		if (isPointerDowning)
		{
			int num = GameCanvas.px - ptLastDownX;
			int num2 = GameCanvas.py - ptLastDownY;
			if (!isChangingCameraMode && (Res.abs(GameCanvas.px - ptFirstDownX) > 15 || Res.abs(GameCanvas.py - ptFirstDownY) > 15))
			{
				isChangingCameraMode = true;
			}
			ptLastDownX = GameCanvas.px;
			ptLastDownY = GameCanvas.py;
			ptDownTime++;
			if (isChangingCameraMode)
			{
				Char.myCharz().cmtoChar = false;
				cmx -= num;
				cmy -= num2;
				if (cmx < 24)
				{
					int num3 = (24 - cmx) / 3;
					if (num3 != 0)
					{
						cmx += num - num / num3;
					}
				}
				if (cmx < (isVsMap() ? 24 : 0))
				{
					cmx = (isVsMap() ? 24 : 0);
				}
				if (cmx > cmxLim)
				{
					int num4 = (cmx - cmxLim) / 3;
					if (num4 != 0)
					{
						cmx += num - num / num4;
					}
				}
				if (cmx > cmxLim + ((!isVsMap()) ? 24 : 0))
				{
					cmx = cmxLim + ((!isVsMap()) ? 24 : 0);
				}
				if (cmy < 0)
				{
					int num5 = -cmy / 3;
					if (num5 != 0)
					{
						cmy += num2 - num2 / num5;
					}
				}
				if (cmy < -((!isVsMap()) ? 24 : 0))
				{
					cmy = -((!isVsMap()) ? 24 : 0);
				}
				if (cmy > cmyLim)
				{
					cmy = cmyLim;
				}
				cmtoX = cmx;
				cmtoY = cmy;
			}
		}
		if (isPointerDowning && GameCanvas.isPointerJustRelease)
		{
			isPointerDowning = false;
			isChangingCameraMode = false;
			if (Res.abs(GameCanvas.px - ptFirstDownX) > 15 || Res.abs(GameCanvas.py - ptFirstDownY) > 15)
			{
				GameCanvas.isPointerJustRelease = false;
			}
		}
	}

	private void checkClick()
	{
		if (isCharging())
		{
			return;
		}
		if (popUpYesNo != null && popUpYesNo.cmdYes != null && popUpYesNo.cmdYes.isPointerPressInside())
		{
			popUpYesNo.cmdYes.performAction();
		}
		else
		{
			if (checkClickToCapcha())
			{
				return;
			}
			long num = mSystem.currentTimeMillis();
			if (lastSingleClick != 0)
			{
				lastSingleClick = 0L;
				GameCanvas.isPointerJustDown = false;
				if (!disableSingleClick)
				{
					checkSingleClick();
					GameCanvas.isPointerJustRelease = false;
					isWaitingDoubleClick = true;
					timeStartDblClick = mSystem.currentTimeMillis();
				}
			}
			if (isWaitingDoubleClick)
			{
				timeEndDblClick = mSystem.currentTimeMillis();
				if (timeEndDblClick - timeStartDblClick < 300 && GameCanvas.isPointerJustRelease)
				{
					isWaitingDoubleClick = false;
					checkDoubleClick();
				}
			}
			if (GameCanvas.isPointerJustRelease)
			{
				disableSingleClick = checkSingleClickEarly();
				lastSingleClick = num;
				lastClickCMX = cmx;
				lastClickCMY = cmy;
				GameCanvas.isPointerJustRelease = false;
			}
		}
	}

	private IMapObject findClickToItem(int px, int py)
	{
		IMapObject mapObject = null;
		int num = 0;
		int num2 = 30;
		MyVector[] array = new MyVector[4] { vMob, vNpc, vItemMap, vCharInMap };
		for (int i = 0; i < array.Length; i++)
		{
			for (int j = 0; j < array[i].size(); j++)
			{
				IMapObject mapObject2 = (IMapObject)array[i].elementAt(j);
				if (mapObject2.isInvisible())
				{
					continue;
				}
				if (mapObject2 is Mob)
				{
					Mob mob = (Mob)mapObject2;
					if (mob.isMobMe && mob.Equals(Char.myCharz().mobMe))
					{
						continue;
					}
				}
				int x = mapObject2.getX();
				int y = mapObject2.getY();
				int w = mapObject2.getW();
				int h = mapObject2.getH();
				if (!inRectangle(px, py, x - w / 2 - num2, y - h - num2, w + num2 * 2, h + num2 * 2))
				{
					continue;
				}
				if (mapObject == null)
				{
					mapObject = mapObject2;
					num = Res.abs(px - x) + Res.abs(py - y);
					if (i == 1)
					{
						return mapObject;
					}
				}
				else
				{
					int num3 = Res.abs(px - x) + Res.abs(py - y);
					if (num3 < num)
					{
						mapObject = mapObject2;
						num = num3;
					}
				}
			}
		}
		return mapObject;
	}

	private Mob findClickToMOB(int px, int py)
	{
		int num = 30;
		Mob mob = null;
		int num2 = 0;
		for (int i = 0; i < vMob.size(); i++)
		{
			Mob mob2 = (Mob)vMob.elementAt(i);
			if (mob2.isInvisible())
			{
				continue;
			}
			if (mob2 != null)
			{
				Mob mob3 = mob2;
				if (mob3.isMobMe && mob3.Equals(Char.myCharz().mobMe))
				{
					continue;
				}
			}
			int x = mob2.getX();
			int y = mob2.getY();
			int w = mob2.getW();
			int h = mob2.getH();
			if (!inRectangle(px, py, x - w / 2 - num, y - h - num, w + num * 2, h + num * 2))
			{
				continue;
			}
			if (mob == null)
			{
				mob = mob2;
				num2 = Res.abs(px - x) + Res.abs(py - y);
				continue;
			}
			int num3 = Res.abs(px - x) + Res.abs(py - y);
			if (num3 < num2)
			{
				mob = mob2;
				num2 = num3;
			}
		}
		return mob;
	}

	private bool inRectangle(int xClick, int yClick, int x, int y, int w, int h)
	{
		if (xClick >= x && xClick <= x + w && yClick >= y)
		{
			return yClick <= y + h;
		}
		return false;
	}

	private bool checkSingleClickEarly()
	{
		int num = GameCanvas.px + cmx;
		int num2 = GameCanvas.py + cmy;
		Char.myCharz().cancelAttack();
		IMapObject mapObject = findClickToItem(num, num2);
		if (mapObject == null)
		{
			return false;
		}
		if (Char.myCharz().isAttacPlayerStatus() && Char.myCharz().charFocus != null && !mapObject.Equals(Char.myCharz().charFocus) && !mapObject.Equals(Char.myCharz().charFocus.mobMe) && mapObject is Char)
		{
			Char obj = (Char)mapObject;
			if (obj.cTypePk != 5 && !obj.isAttacPlayerStatus())
			{
				checkClickMoveTo(num, num2, 2);
				return false;
			}
		}
		if (Char.myCharz().mobFocus == mapObject || Char.myCharz().itemFocus == mapObject)
		{
			doDoubleClickToObj(mapObject);
			return true;
		}
		if (TileMap.mapID == 51 && mapObject.Equals(Char.myCharz().npcFocus))
		{
			checkClickMoveTo(num, num2, 3);
			return false;
		}
		if (Char.myCharz().skillPaint != null || Char.myCharz().arr != null || Char.myCharz().dart != null || Char.myCharz().skillInfoPaint() != null)
		{
			return false;
		}
		Char.myCharz().focusManualTo(mapObject);
		mapObject.stopMoving();
		return false;
	}

	private void checkDoubleClick()
	{
		int num = GameCanvas.px + lastClickCMX;
		int num2 = GameCanvas.py + lastClickCMY;
		_ = Char.myCharz().cy;
		if (isLockKey)
		{
			return;
		}
		IMapObject mapObject = findClickToItem(num, num2);
		if (mapObject != null)
		{
			if (mapObject is Mob && !isMeCanAttackMob((Mob)mapObject))
			{
				checkClickMoveTo(num, num2, 4);
			}
			else
			{
				if (checkClickToBotton(mapObject) || (!mapObject.Equals(Char.myCharz().npcFocus) && mobCapcha != null))
				{
					return;
				}
				if (Char.myCharz().isAttacPlayerStatus() && Char.myCharz().charFocus != null && !mapObject.Equals(Char.myCharz().charFocus) && !mapObject.Equals(Char.myCharz().charFocus.mobMe) && mapObject is Char)
				{
					Char obj = (Char)mapObject;
					if (obj.cTypePk != 5 && !obj.isAttacPlayerStatus())
					{
						checkClickMoveTo(num, num2, 5);
						return;
					}
				}
				if (TileMap.mapID == 51 && mapObject.Equals(Char.myCharz().npcFocus))
				{
					checkClickMoveTo(num, num2, 6);
				}
				else
				{
					doDoubleClickToObj(mapObject);
				}
			}
		}
		else if (!checkClickToPopup(num, num2) && !checkClipTopChatPopUp(num, num2) && !Main.isPC)
		{
			checkClickMoveTo(num, num2, 7);
		}
	}

	private bool checkClickToBotton(IMapObject Object)
	{
		if (Object == null)
		{
			return false;
		}
		int y = Object.getY();
		int num = Char.myCharz().cy;
		if (y < num)
		{
			while (y < num)
			{
				num -= 5;
				if (TileMap.tileTypeAt(Char.myCharz().cx, num, 8192))
				{
					auto = 0;
					Char.myCharz().cancelAttack();
					Char.myCharz().currentMovePoint = null;
					return true;
				}
			}
		}
		return false;
	}

	public void doDoubleClickToObj(IMapObject obj)
	{
		if ((obj.Equals(Char.myCharz().npcFocus) || mobCapcha == null) && !checkClickToBotton(obj))
		{
			checkEffToObj(obj, isnew: false);
			Char.myCharz().cancelAttack();
			Char.myCharz().currentMovePoint = null;
			Char.myCharz().cvx = (Char.myCharz().cvy = 0);
			obj.stopMoving();
			auto = 10;
			doFire(isFireByShortCut: false, skipWaypoint: true);
			clickToX = obj.getX();
			clickToY = obj.getY();
			clickOnTileTop = false;
			clickMoving = true;
			clickMovingRed = true;
			clickMovingTimeOut = 20;
			clickMovingP1 = 30;
		}
	}

	private void checkSingleClick()
	{
		int xClick = GameCanvas.px + lastClickCMX;
		int yClick = GameCanvas.py + lastClickCMY;
		if (!isLockKey && !checkClickToPopup(xClick, yClick) && !checkClipTopChatPopUp(xClick, yClick))
		{
			checkClickMoveTo(xClick, yClick, 0);
		}
	}

	private bool checkClipTopChatPopUp(int xClick, int yClick)
	{
		if (Equals(info2) && gI().popUpYesNo != null)
		{
			return false;
		}
		if (info2.info.info != null && info2.info.info.charInfo != null)
		{
			int x = Res.abs(info2.cmx) + info2.info.X - 40;
			int y = Res.abs(info2.cmy) + info2.info.Y;
			if (inRectangle(xClick - cmx, yClick - cmy, x, y, 200, info2.info.H))
			{
				info2.doClick(10);
				return true;
			}
		}
		return false;
	}

	private bool checkClickToPopup(int xClick, int yClick)
	{
		for (int i = 0; i < PopUp.vPopups.size(); i++)
		{
			PopUp popUp = (PopUp)PopUp.vPopups.elementAt(i);
			if (inRectangle(xClick, yClick, popUp.cx, popUp.cy, popUp.cw, popUp.ch))
			{
				if (popUp.cy <= 24 && TileMap.isInAirMap() && Char.myCharz().cTypePk != 0)
				{
					return false;
				}
				if (popUp.isPaint)
				{
					popUp.doClick(10);
					return true;
				}
			}
		}
		return false;
	}

	private void checkClickMoveTo(int xClick, int yClick, int index)
	{
		if (gamePad.disableClickMove())
		{
			return;
		}
		Char.myCharz().cancelAttack();
		if (xClick < TileMap.pxw && xClick > TileMap.pxw - 32)
		{
			Char.myCharz().currentMovePoint = new MovePoint(TileMap.pxw, yClick);
			return;
		}
		if (xClick < 32 && xClick > 0)
		{
			Char.myCharz().currentMovePoint = new MovePoint(0, yClick);
			return;
		}
		if (xClick < TileMap.pxw && xClick > TileMap.pxw - 48)
		{
			Char.myCharz().currentMovePoint = new MovePoint(TileMap.pxw, yClick);
			return;
		}
		if (xClick < 48 && xClick > 0)
		{
			Char.myCharz().currentMovePoint = new MovePoint(0, yClick);
			return;
		}
		clickToX = xClick;
		clickToY = yClick;
		clickOnTileTop = false;
		Char.myCharz().delayFall = 0;
		int num = ((!Char.myCharz().canFly || Char.myCharz().cMP <= 0) ? 1000 : 0);
		if (clickToY > Char.myCharz().cy && Res.abs(clickToX - Char.myCharz().cx) < 12)
		{
			return;
		}
		for (int i = 0; i < 60 + num && clickToY + i < TileMap.pxh - 24; i += 24)
		{
			if (TileMap.tileTypeAt(clickToX, clickToY + i, 2))
			{
				clickToY = TileMap.tileYofPixel(clickToY + i);
				clickOnTileTop = true;
				break;
			}
		}
		for (int j = 0; j < 40 + num; j += 24)
		{
			if (TileMap.tileTypeAt(clickToX, clickToY - j, 2))
			{
				clickToY = TileMap.tileYofPixel(clickToY - j);
				clickOnTileTop = true;
				break;
			}
		}
		clickMoving = true;
		clickMovingRed = false;
		clickMovingP1 = ((!clickOnTileTop) ? 30 : ((yClick >= clickToY) ? clickToY : yClick));
		Char.myCharz().delayFall = 0;
		if (!clickOnTileTop && clickToY < Char.myCharz().cy - 50)
		{
			Char.myCharz().delayFall = 20;
		}
		clickMovingTimeOut = 30;
		auto = 0;
		if (Char.myCharz().holder)
		{
			Char.myCharz().removeHoleEff();
		}
		Char.myCharz().currentMovePoint = new MovePoint(clickToX, clickToY);
		Char.myCharz().cdir = ((Char.myCharz().cx - Char.myCharz().currentMovePoint.xEnd <= 0) ? 1 : (-1));
		Char.myCharz().endMovePointCommand = null;
		isAutoPlay = false;
	}

	private void checkAuto()
	{
		long num = mSystem.currentTimeMillis();
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] || GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23] || GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24] || GameCanvas.keyPressed[1] || GameCanvas.keyPressed[3])
		{
			auto = 0;
			isAutoPlay = false;
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] && !isPaintPopup())
		{
			if (auto == 0)
			{
				if (num - lastFire < 800 && checkSkillValid2() && (Char.myCharz().mobFocus != null || (Char.myCharz().charFocus != null && Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus))))
				{
					Res.outz("toi day");
					auto = 10;
					GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = false;
				}
			}
			else
			{
				auto = 0;
				GameCanvas.keyPressed[(!Main.isPC) ? 4 : 23] = (GameCanvas.keyPressed[(!Main.isPC) ? 6 : 24] = false);
			}
			lastFire = num;
		}
		if (GameCanvas.gameTick % 5 == 0 && auto > 0 && Char.myCharz().currentMovePoint == null)
		{
			if (Char.myCharz().myskill != null && (Char.myCharz().myskill.template.isUseAlone() || Char.myCharz().myskill.paintCanNotUseSkill))
			{
				return;
			}
			if ((Char.myCharz().mobFocus != null && Char.myCharz().mobFocus.status != 1 && Char.myCharz().mobFocus.status != 0 && Char.myCharz().charFocus == null) || (Char.myCharz().charFocus != null && Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus)))
			{
				if (Char.myCharz().myskill.paintCanNotUseSkill)
				{
					return;
				}
				doFire(isFireByShortCut: false, skipWaypoint: true);
			}
		}
		if (auto > 1)
		{
			auto--;
		}
	}

	public void doUseHP()
	{
		if (Char.myCharz().stone || Char.myCharz().blindEff || Char.myCharz().holdEffID > 0)
		{
			return;
		}
		long num = mSystem.currentTimeMillis();
		if (num - lastUsePotion >= 10000)
		{
			if (!Char.myCharz().doUsePotion())
			{
				info1.addInfo(mResources.HP_EMPTY);
				return;
			}
			ServerEffect.addServerEffect(11, Char.myCharz(), 5);
			ServerEffect.addServerEffect(104, Char.myCharz(), 4);
			lastUsePotion = num;
			SoundMn.gI().eatPeans();
		}
	}

	public void activeSuperPower(int x, int y)
	{
		if (!isSuperPower)
		{
			SoundMn.gI().bigeExlode();
			isSuperPower = true;
			tPower = 0;
			dxPower = 0;
			xPower = x - cmx;
			yPower = y - cmy;
		}
	}

	public void activeRongThanEff(bool isMe)
	{
		activeRongThan = true;
		isUseFreez = true;
		isMeCallRongThan = true;
		if (isMe)
		{
			EffecMn.addEff(new Effect(20, Char.myCharz().cx, Char.myCharz().cy - 77, 2, 8, 1));
		}
	}

	public void hideRongThanEff()
	{
		activeRongThan = false;
		isUseFreez = true;
		isMeCallRongThan = false;
	}

	public void doiMauTroi()
	{
		isRongThanXuatHien = true;
		mautroi = mGraphics.blendColor(0.4f, 0, GameCanvas.colorTop[GameCanvas.colorTop.Length - 1]);
	}

	public void callRongThan(int x, int y)
	{
		Res.outz("VE RONG THAN O VI TRI x= " + x + " y=" + y);
		doiMauTroi();
		EffecMn.addEff(new Effect((!isRongNamek) ? 17 : 25, x, y - 77, 2, -1, 1));
	}

	public void hideRongThan()
	{
		isRongThanXuatHien = false;
		EffecMn.removeEff(17);
		if (isRongNamek)
		{
			isRongNamek = false;
			EffecMn.removeEff(25);
		}
	}

	private void autoPlay()
	{
	}

	private void doFire(bool isFireByShortCut, bool skipWaypoint)
	{
		if (MainXmapCL.isXmaping)
		{
			return;
		}
		tam++;
		Waypoint waypoint = Char.myCharz().isInEnterOfflinePoint();
		Waypoint waypoint2 = Char.myCharz().isInEnterOnlinePoint();
		if (!skipWaypoint && waypoint != null && (Char.myCharz().mobFocus == null || (Char.myCharz().mobFocus != null && Char.myCharz().mobFocus.templateId == 0)))
		{
			waypoint.popup.command.performAction();
		}
		else if (!skipWaypoint && waypoint2 != null && (Char.myCharz().mobFocus == null || (Char.myCharz().mobFocus != null && Char.myCharz().mobFocus.templateId == 0)))
		{
			waypoint2.popup.command.performAction();
		}
		else
		{
			if ((TileMap.mapID == 51 && Char.myCharz().npcFocus != null) || Char.myCharz().statusMe == 14)
			{
				return;
			}
			Char.myCharz().cvx = (Char.myCharz().cvy = 0);
			if (Char.myCharz().isSelectingSkillUseAlone() && Char.myCharz().focusToAttack())
			{
				if (checkSkillValid())
				{
					Char.myCharz().currentFireByShortcut = isFireByShortCut;
					Char.myCharz().useSkillNotFocus();
				}
			}
			else if (isAttack())
			{
				if (Char.myCharz().isUseChargeSkill() && Char.myCharz().focusToAttack())
				{
					if (checkSkillValid())
					{
						Char.myCharz().currentFireByShortcut = isFireByShortCut;
						Char.myCharz().sendUseChargeSkill();
					}
					else
					{
						Char.myCharz().stopUseChargeSkill();
					}
				}
				else
				{
					bool flag = TileMap.tileTypeAt(Char.myCharz().cx, Char.myCharz().cy, 2);
					Char.myCharz().setSkillPaint(sks[Char.myCharz().myskill.skillId], (!flag) ? 1 : 0);
					if (flag)
					{
						Char.myCharz().delayFall = 20;
					}
					Char.myCharz().currentFireByShortcut = isFireByShortCut;
				}
			}
			if (Char.myCharz().isSelectingSkillBuffToPlayer())
			{
				auto = 0;
			}
		}
	}

	private void askToPick()
	{
		Npc npc = new Npc(5, 0, -100, 100, 5, info1.charId[Char.myCharz().cgender][2]);
		string nhatvatpham = mResources.nhatvatpham;
		string[] menu = new string[2]
		{
			mResources.YES,
			mResources.NO
		};
		npc.idItem = 673;
		gI().createMenu(menu, npc);
		ChatPopup.addChatPopupWithIcon(nhatvatpham, 100000, npc, 5820);
	}

	private void pickItem()
	{
		if (Char.myCharz().itemFocus == null)
		{
			return;
		}
		if (Char.myCharz().cx < Char.myCharz().itemFocus.x)
		{
			Char.myCharz().cdir = 1;
		}
		else
		{
			Char.myCharz().cdir = -1;
		}
		int num = Math.Abs(Char.myCharz().cx - Char.myCharz().itemFocus.x);
		int num2 = Math.Abs(Char.myCharz().cy - Char.myCharz().itemFocus.y);
		if (num <= 40 && num2 < 40)
		{
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
			if (Char.myCharz().itemFocus.template.id != 673)
			{
				Service.gI().pickItem(Char.myCharz().itemFocus.itemMapID);
			}
			else
			{
				askToPick();
			}
		}
		else
		{
			Char.myCharz().currentMovePoint = new MovePoint(Char.myCharz().itemFocus.x, Char.myCharz().itemFocus.y);
			Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
		}
	}

	public bool isCharging()
	{
		return Char.myCharz().isFlyAndCharge || Char.myCharz().isUseSkillAfterCharge || Char.myCharz().isStandAndCharge || Char.myCharz().isWaitMonkey || isSuperPower;
	}

	public void doSelectSkill(Skill skill, bool isShortcut)
	{
		if (Char.myCharz().isCreateDark || isCharging() || Char.myCharz().taskMaint.taskId <= 1)
		{
			return;
		}
		Char.myCharz().myskill = skill;
		if (lastSkill != skill && lastSkill != null)
		{
			Service.gI().selectSkill(skill.template.id);
			saveRMSCurrentSkill(skill.template.id);
			resetButton();
			lastSkill = skill;
			selectedIndexSkill = -1;
			gI().auto = 0;
			return;
		}
		if (Char.myCharz().isUseSkillSpec())
		{
			Res.outz(">>>use skill spec: " + skill.template.id);
			Char.myCharz().sendNewAttack(skill.template.id);
			saveRMSCurrentSkill(skill.template.id);
			resetButton();
			lastSkill = skill;
			selectedIndexSkill = -1;
			gI().auto = 0;
			return;
		}
		if (Char.myCharz().isSelectingSkillUseAlone())
		{
			Res.outz("use skill not focus");
			doUseSkillNotFocus(skill);
			lastSkill = skill;
			return;
		}
		selectedIndexSkill = -1;
		if (skill == null)
		{
			return;
		}
		Res.outz("only select skill");
		if (lastSkill != skill)
		{
			Service.gI().selectSkill(skill.template.id);
			saveRMSCurrentSkill(skill.template.id);
			resetButton();
		}
		if (Char.myCharz().charFocus != null || !Char.myCharz().isSelectingSkillBuffToPlayer())
		{
			if (Char.myCharz().focusToAttack())
			{
				doFire(isShortcut, skipWaypoint: true);
				doSeleckSkillFlag = true;
			}
			lastSkill = skill;
		}
	}

	public void doUseSkill(Skill skill, bool isShortcut)
	{
		if ((TileMap.mapID == 112 || TileMap.mapID == 113) && Char.myCharz().cTypePk == 0)
		{
			return;
		}
		if (Char.myCharz().isSelectingSkillUseAlone())
		{
			Res.outz("HERE");
			doUseSkillNotFocus(skill);
			return;
		}
		selectedIndexSkill = -1;
		if (skill != null)
		{
			Service.gI().selectSkill(skill.template.id);
			saveRMSCurrentSkill(skill.template.id);
			resetButton();
			Char.myCharz().myskill = skill;
			doFire(isShortcut, skipWaypoint: true);
		}
	}

	public void doUseSkillNotFocus(Skill skill)
	{
		if (((TileMap.mapID != 112 && TileMap.mapID != 113) || Char.myCharz().cTypePk != 0) && checkSkillValid())
		{
			selectedIndexSkill = -1;
			if (skill != null)
			{
				Service.gI().selectSkill(skill.template.id);
				saveRMSCurrentSkill(skill.template.id);
				resetButton();
				Char.myCharz().myskill = skill;
				Char.myCharz().useSkillNotFocus();
				Char.myCharz().currentFireByShortcut = true;
				auto = 0;
			}
		}
	}

	public void sortSkill()
	{
		for (int i = 0; i < Char.myCharz().vSkillFight.size() - 1; i++)
		{
			Skill skill = (Skill)Char.myCharz().vSkillFight.elementAt(i);
			for (int j = i + 1; j < Char.myCharz().vSkillFight.size(); j++)
			{
				Skill skill2 = (Skill)Char.myCharz().vSkillFight.elementAt(j);
				if (skill2.template.id < skill.template.id)
				{
					Skill skill3 = skill2;
					skill2 = skill;
					skill = skill3;
					Char.myCharz().vSkillFight.setElementAt(skill, i);
					Char.myCharz().vSkillFight.setElementAt(skill2, j);
				}
			}
		}
	}

	public void updateKeyTouchCapcha()
	{
		if (isNotPaintTouchControl())
		{
			return;
		}
		for (int i = 0; i < strCapcha.Length; i++)
		{
			keyCapcha[i] = -1;
			if (!GameCanvas.isTouchControl)
			{
				continue;
			}
			int num = (GameCanvas.w - strCapcha.Length * disXC) / 2;
			int w = strCapcha.Length * disXC;
			int y = GameCanvas.h - 40;
			int h = disXC;
			if (!GameCanvas.isPointerHoldIn(num, y, w, h))
			{
				continue;
			}
			int num2 = (GameCanvas.px - num) / disXC;
			if (i == num2)
			{
				keyCapcha[i] = 1;
			}
			if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease && i == num2)
			{
				char[] array = keyInput.ToCharArray();
				MyVector myVector = new MyVector();
				for (int j = 0; j < array.Length; j++)
				{
					myVector.addElement(array[j] + string.Empty);
				}
				myVector.removeElementAt(0);
				myVector.insertElementAt(strCapcha[i] + string.Empty, myVector.size());
				keyInput = string.Empty;
				for (int k = 0; k < myVector.size(); k++)
				{
					keyInput += ((string)myVector.elementAt(k)).ToUpper();
				}
				Service.gI().mobCapcha(strCapcha[i]);
			}
		}
	}

	public bool checkClickToCapcha()
	{
		if (mobCapcha == null)
		{
			return false;
		}
		int x = (GameCanvas.w - 5 * disXC) / 2;
		int w = 5 * disXC;
		int y = GameCanvas.h - 40;
		int h = disXC;
		return GameCanvas.isPointerHoldIn(x, y, w, h);
	}

	public void checkMouseChat()
	{
		if (GameCanvas.isMouseFocus(xC, yC, 34, 34))
		{
			if (!TileMap.isOfflineMap())
			{
				mScreen.keyMouse = 15;
			}
		}
		else if (GameCanvas.isMouseFocus(xHP, yHP, 40, 40))
		{
			if (Char.myCharz().statusMe != 14)
			{
				mScreen.keyMouse = 10;
			}
		}
		else if (GameCanvas.isMouseFocus(xF, yF, 40, 40))
		{
			if (Char.myCharz().statusMe != 14)
			{
				mScreen.keyMouse = 5;
			}
		}
		else if (cmdMenu != null && GameCanvas.isMouseFocus(cmdMenu.x, cmdMenu.y, cmdMenu.w / 2, cmdMenu.h))
		{
			mScreen.keyMouse = 1;
		}
		else
		{
			mScreen.keyMouse = -1;
		}
	}

	private void updateKeyTouchControl()
	{
		MainMod.updateTouch();
		if (isNotPaintTouchControl())
		{
			return;
		}
		mScreen.keyTouch = -1;
		if (GameCanvas.isTouchControl)
		{
			if (GameCanvas.isPointerHoldIn(0, 0, 60, 50) && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
			{
				if (Char.myCharz().cmdMenu != null)
				{
					Char.myCharz().cmdMenu.performAction();
				}
				Char.myCharz().currentMovePoint = null;
				GameCanvas.clearAllPointerEvent();
				flareFindFocus = true;
				flareTime = 5;
				return;
			}
			if (Main.isPC)
			{
				checkMouseChat();
			}
			if (!TileMap.isOfflineMap() && GameCanvas.isPointerHoldIn(xC, yC, 34, 34))
			{
				mScreen.keyTouch = 15;
				GameCanvas.isPointerJustDown = false;
				isPointerDowning = false;
				if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					ChatTextField.gI().startChat(this, string.Empty);
					SoundMn.gI().buttonClick();
					Char.myCharz().currentMovePoint = null;
					GameCanvas.clearAllPointerEvent();
					return;
				}
			}
			if (Char.myCharz().cmdMenu != null && GameCanvas.isPointerHoldIn(Char.myCharz().cmdMenu.x - 17, Char.myCharz().cmdMenu.y - 17, 34, 34))
			{
				mScreen.keyTouch = 20;
				GameCanvas.isPointerJustDown = false;
				isPointerDowning = false;
				if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					GameCanvas.clearAllPointerEvent();
					Char.myCharz().cmdMenu.performAction();
					return;
				}
			}
			updateGamePad();
			if (((isAnalog != 0) ? GameCanvas.isPointerHoldIn(xHP, yHP + 10, 34, 34) : GameCanvas.isPointerHoldIn(xHP, yHP + 10, 40, 40)) && Char.myCharz().statusMe != 14 && mobCapcha == null)
			{
				mScreen.keyTouch = 10;
				GameCanvas.isPointerJustDown = false;
				isPointerDowning = false;
				if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					GameCanvas.keyPressed[10] = true;
					GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
				}
			}
			if (((isAnalog != 0) ? GameCanvas.isPointerHoldIn(xHP + 5, yHP - 6 - 34 + 10, 34, 34) : GameCanvas.isPointerHoldIn(xHP + 5, yHP - 6 - 40 + 10, 40, 40)) && Char.myCharz().statusMe != 14 && mobCapcha == null)
			{
				if (isPickNgocRong)
				{
					mScreen.keyTouch = 14;
					GameCanvas.isPointerJustDown = false;
					isPointerDowning = false;
					if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
					{
						GameCanvas.keyPressed[14] = true;
						GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
						isPickNgocRong = false;
						Service.gI().useItem(-1, -1, -1, -1);
					}
				}
				else if (isudungCapsun4)
				{
					mScreen.keyTouch = 14;
					GameCanvas.isPointerJustDown = false;
					isPointerDowning = false;
					if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
					{
						GameCanvas.keyPressed[14] = true;
						GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
						for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
						{
							Item item = Char.myCharz().arrItemBag[i];
							if (item == null)
							{
								continue;
							}
							Res.err("find " + item.template.id);
							if (item.template.id == 194)
							{
								isudungCapsun4 = item.quantity > 0;
								if (isudungCapsun4)
								{
									Service.gI().useItem(0, 1, (sbyte)i, -1);
									break;
								}
							}
						}
					}
				}
				else if (isudungCapsun3)
				{
					mScreen.keyTouch = 14;
					GameCanvas.isPointerJustDown = false;
					isPointerDowning = false;
					if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
					{
						GameCanvas.keyPressed[14] = true;
						GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
						for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
						{
							Item item2 = Char.myCharz().arrItemBag[j];
							if (item2 != null && item2.template.id == 193)
							{
								isudungCapsun3 = item2.quantity > 0;
								if (isudungCapsun3)
								{
									Service.gI().useItem(0, 1, (sbyte)j, -1);
									break;
								}
							}
						}
					}
				}
			}
		}
		if (mobCapcha != null)
		{
			updateKeyTouchCapcha();
		}
		else if (isHaveSelectSkill)
		{
			if (isCharging())
			{
				return;
			}
			keyTouchSkill = -1;
			bool flag = false;
			if (onScreenSkill.Length > 5 && (GameCanvas.isPointerHoldIn(xSkill + xS[0] - wSkill / 2 + 12, yS[0] - wSkill / 2 + 12, 5 * wSkill, wSkill) || GameCanvas.isPointerHoldIn(xSkill + xS[5] - wSkill / 2 + 12, yS[5] - wSkill / 2 + 12, 5 * wSkill, wSkill)))
			{
				flag = true;
			}
			if (flag || GameCanvas.isPointerHoldIn(xSkill + xS[0] - wSkill / 2 + 12, yS[0] - wSkill / 2 + 12, 5 * wSkill, wSkill) || (!GameCanvas.isTouchControl && GameCanvas.isPointerHoldIn(xSkill + xS[0] - wSkill / 2 + 12, yS[0] - wSkill / 2 + 12, wSkill, onScreenSkill.Length * wSkill)))
			{
				GameCanvas.isPointerJustDown = false;
				isPointerDowning = false;
				int num = (GameCanvas.pxLast - (xSkill + xS[0] - wSkill / 2 + 12)) / wSkill;
				if (flag && GameCanvas.pyLast < yS[0])
				{
					num += 5;
				}
				keyTouchSkill = num;
				if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
				{
					GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
					selectedIndexSkill = num;
					if (indexSelect < 0)
					{
						indexSelect = 0;
					}
					if (!Main.isPC)
					{
						if (selectedIndexSkill > onScreenSkill.Length - 1)
						{
							selectedIndexSkill = onScreenSkill.Length - 1;
						}
					}
					else if (selectedIndexSkill > keySkill.Length - 1)
					{
						selectedIndexSkill = keySkill.Length - 1;
					}
					Skill skill = (Main.isPC ? keySkill[selectedIndexSkill] : onScreenSkill[selectedIndexSkill]);
					if (skill != null)
					{
						doSelectSkill(skill, isShortcut: true);
					}
				}
			}
		}
		if (GameCanvas.isPointerJustRelease)
		{
			if (GameCanvas.keyHold[1] || GameCanvas.keyHold[(!Main.isPC) ? 2 : 21] || GameCanvas.keyHold[3] || GameCanvas.keyHold[(!Main.isPC) ? 4 : 23] || GameCanvas.keyHold[(!Main.isPC) ? 6 : 24])
			{
				GameCanvas.isPointerJustRelease = false;
			}
			GameCanvas.keyHold[1] = false;
			GameCanvas.keyHold[(!Main.isPC) ? 2 : 21] = false;
			GameCanvas.keyHold[3] = false;
			GameCanvas.keyHold[(!Main.isPC) ? 4 : 23] = false;
			GameCanvas.keyHold[(!Main.isPC) ? 6 : 24] = false;
		}
	}

	public void setCharJumpAtt()
	{
		Char.myCharz().cvy = -10;
		Char.myCharz().statusMe = 3;
		Char.myCharz().cp1 = 0;
	}

	public void setCharJump(int cvx)
	{
		if (Char.myCharz().cx - Char.myCharz().cxSend != 0 || Char.myCharz().cy - Char.myCharz().cySend != 0)
		{
			Service.gI().charMove();
		}
		Char.myCharz().cvy = -10;
		Char.myCharz().cvx = cvx;
		Char.myCharz().statusMe = 3;
		Char.myCharz().cp1 = 0;
	}

	public void updateOpen()
	{
		if (isstarOpen)
		{
			if (moveUp > -3)
			{
				moveUp -= 4;
			}
			else
			{
				moveUp = -2;
			}
			if (moveDow < GameCanvas.h + 3)
			{
				moveDow += 4;
			}
			else
			{
				moveDow = GameCanvas.h + 2;
			}
			if (moveUp <= -2 && moveDow >= GameCanvas.h + 2)
			{
				isstarOpen = false;
			}
		}
	}

	public void initCreateCommand()
	{
	}

	public void checkCharFocus()
	{
	}

	public void updateXoSo()
	{
		if (tShow == 0)
		{
			return;
		}
		currXS = mSystem.currentTimeMillis();
		if (currXS - lastXS > 1000)
		{
			lastXS = mSystem.currentTimeMillis();
			secondXS++;
		}
		if (secondXS > 20)
		{
			for (int i = 0; i < winnumber.Length; i++)
			{
				randomNumber[i] = winnumber[i];
			}
			tShow--;
			if (tShow == 0)
			{
				yourNumber = string.Empty;
				info1.addInfo(strFinish);
				secondXS = 0;
			}
			return;
		}
		if (moveIndex > winnumber.Length - 1)
		{
			tShow--;
			if (tShow == 0)
			{
				yourNumber = string.Empty;
				info1.addInfo(strFinish);
			}
			return;
		}
		if (moveIndex < randomNumber.Length)
		{
			if (tMove[moveIndex] == 15)
			{
				if (randomNumber[moveIndex] == winnumber[moveIndex] - 1)
				{
					delayMove[moveIndex] = 10;
				}
				if (randomNumber[moveIndex] == winnumber[moveIndex])
				{
					tMove[moveIndex] = -1;
					moveIndex++;
				}
			}
			else if (GameCanvas.gameTick % 5 == 0)
			{
				tMove[moveIndex]++;
			}
		}
		for (int j = 0; j < winnumber.Length; j++)
		{
			if (tMove[j] == -1)
			{
				continue;
			}
			moveCount[j]++;
			if (moveCount[j] > tMove[j] + delayMove[j])
			{
				moveCount[j] = 0;
				randomNumber[j]++;
				if (randomNumber[j] >= 10)
				{
					randomNumber[j] = 0;
				}
			}
		}
	}

	public override void update()
	{
		MainMod.Update();
		if (GameCanvas.keyPressed[16])
		{
			GameCanvas.keyPressed[16] = false;
			Char.myCharz().findNextFocusByKey();
		}
		if (GameCanvas.keyPressed[13] && !GameCanvas.panel.isShow)
		{
			GameCanvas.keyPressed[13] = false;
			Char.myCharz().findNextFocusByKey();
		}
		if (GameCanvas.keyPressed[17])
		{
			GameCanvas.keyPressed[17] = false;
			Char.myCharz().searchItem();
			if (Char.myCharz().itemFocus != null)
			{
				pickItem();
			}
		}
		if (GameCanvas.gameTick % 100 == 0 && TileMap.mapID == 137)
		{
			shock_scr = 30;
		}
		if (isAutoPlay && GameCanvas.gameTick % 20 == 0)
		{
			autoPlay();
		}
		updateXoSo();
		mSystem.checkAdComlete();
		try
		{
			if (LoginScr.isContinueToLogin)
			{
				LoginScr.isContinueToLogin = false;
			}
			if (tickMove == 1)
			{
				lastTick = mSystem.currentTimeMillis();
			}
			if (tickMove == 100)
			{
				tickMove = 0;
				currTick = mSystem.currentTimeMillis();
				int second = (int)(currTick - lastTick) / 1000;
				Service.gI().checkMMove(second);
			}
			if (lockTick > 0)
			{
				lockTick--;
				if (lockTick == 0)
				{
					Controller.isStopReadMessage = false;
				}
			}
			checkCharFocus();
			GameCanvas.debug("E1", 0);
			updateCamera();
			GameCanvas.debug("E2", 0);
			ChatTextField.gI().update();
			GameCanvas.debug("E3", 0);
			for (int i = 0; i < vCharInMap.size(); i++)
			{
				((Char)vCharInMap.elementAt(i)).update();
			}
			for (int j = 0; j < Teleport.vTeleport.size(); j++)
			{
				((Teleport)Teleport.vTeleport.elementAt(j)).update();
			}
			Char.myCharz().update();
			_ = Char.myCharz().statusMe;
			if (popUpYesNo != null)
			{
				popUpYesNo.update();
			}
			EffecMn.update();
			GameCanvas.debug("E5x", 0);
			for (int k = 0; k < vMob.size(); k++)
			{
				((Mob)vMob.elementAt(k)).update();
			}
			GameCanvas.debug("E6", 0);
			for (int l = 0; l < vNpc.size(); l++)
			{
				((Npc)vNpc.elementAt(l)).update();
			}
			nSkill = onScreenSkill.Length;
			for (int num = onScreenSkill.Length - 1; num >= 0; num--)
			{
				if (onScreenSkill[num] != null)
				{
					nSkill = num + 1;
					break;
				}
				nSkill--;
			}
			setSkillBarPosition();
			GameCanvas.debug("E7", 0);
			GameCanvas.gI().updateDust();
			GameCanvas.debug("E8", 0);
			updateFlyText();
			PopUp.updateAll();
			updateSplash();
			updateSS();
			GameCanvas.updateBG();
			GameCanvas.debug("E9", 0);
			updateClickToArrow();
			GameCanvas.debug("E10", 0);
			for (int m = 0; m < vItemMap.size(); m++)
			{
				((ItemMap)vItemMap.elementAt(m)).update();
			}
			GameCanvas.debug("E11", 0);
			GameCanvas.debug("E13", 0);
			for (int num2 = Effect2.vRemoveEffect2.size() - 1; num2 >= 0; num2--)
			{
				Effect2.vEffect2.removeElement(Effect2.vRemoveEffect2.elementAt(num2));
				Effect2.vRemoveEffect2.removeElementAt(num2);
			}
			for (int n = 0; n < Effect2.vEffect2.size(); n++)
			{
				((Effect2)Effect2.vEffect2.elementAt(n)).update();
			}
			for (int num3 = 0; num3 < Effect2.vEffect2Outside.size(); num3++)
			{
				((Effect2)Effect2.vEffect2Outside.elementAt(num3)).update();
			}
			for (int num4 = 0; num4 < Effect2.vAnimateEffect.size(); num4++)
			{
				((Effect2)Effect2.vAnimateEffect.elementAt(num4)).update();
			}
			for (int num5 = 0; num5 < Effect2.vEffectFeet.size(); num5++)
			{
				((Effect2)Effect2.vEffectFeet.elementAt(num5)).update();
			}
			for (int num6 = 0; num6 < Effect2.vEffect3.size(); num6++)
			{
				((Effect2)Effect2.vEffect3.elementAt(num6)).update();
			}
			BackgroudEffect.updateEff();
			info1.update();
			info2.update();
			GameCanvas.debug("E15", 0);
			if (currentCharViewInfo != null && !currentCharViewInfo.Equals(Char.myCharz()))
			{
				currentCharViewInfo.update();
			}
			runArrow++;
			if (runArrow > 3)
			{
				runArrow = 0;
			}
			if (isInjureHp)
			{
				twHp++;
				if (twHp == 20)
				{
					twHp = 0L;
					isInjureHp = false;
				}
			}
			else if (dHP > Char.myCharz().cHP)
			{
				long num7 = dHP - Char.myCharz().cHP >> 1;
				if (num7 < 1)
				{
					num7 = 1L;
				}
				dHP -= num7;
			}
			else
			{
				dHP = Char.myCharz().cHP;
			}
			if (isInjureMp)
			{
				twMp++;
				if (twMp == 20)
				{
					twMp = 0L;
					isInjureMp = false;
				}
			}
			else if (dMP > Char.myCharz().cMP)
			{
				long num8 = dMP - Char.myCharz().cMP >> 1;
				if (num8 < 1)
				{
					num8 = 1L;
				}
				dMP -= num8;
			}
			else
			{
				dMP = Char.myCharz().cMP;
			}
			if (tMenuDelay > 0)
			{
				tMenuDelay--;
			}
			if (isRongThanMenu())
			{
				int num9 = 100;
				while (yR - num9 < cmy)
				{
					cmy--;
				}
			}
			for (int num10 = 0; num10 < Char.vItemTime.size(); num10++)
			{
				((ItemTime)Char.vItemTime.elementAt(num10)).update();
			}
			for (int num11 = 0; num11 < textTime.size(); num11++)
			{
				((ItemTime)textTime.elementAt(num11)).update();
			}
			updateChatVip();
		}
		catch (Exception)
		{
		}
		if (GameCanvas.gameTick % 4000 == 1000)
		{
			checkRemoveImage();
		}
		EffectManager.update();
	}

	public void updateKeyChatPopUp()
	{
	}

	public bool isRongThanMenu()
	{
		return isMeCallRongThan;
	}

	public void paintEffect(mGraphics g)
	{
		for (int i = 0; i < Effect2.vEffect2.size(); i++)
		{
			Effect2 effect = (Effect2)Effect2.vEffect2.elementAt(i);
			if (effect != null && !(effect is ChatPopup))
			{
				effect.paint(g);
			}
		}
		if (!GameCanvas.lowGraphic)
		{
			for (int j = 0; j < Effect2.vAnimateEffect.size(); j++)
			{
				((Effect2)Effect2.vAnimateEffect.elementAt(j)).paint(g);
			}
		}
		for (int k = 0; k < Effect2.vEffect2Outside.size(); k++)
		{
			((Effect2)Effect2.vEffect2Outside.elementAt(k)).paint(g);
		}
	}

	public void paintBgItem(mGraphics g, int layer)
	{
		if (TileMap.mapID == 48 && layer == 3 && GameCanvas.bgW != null && GameCanvas.bgW[0] != 0)
		{
			for (int i = 0; i < TileMap.pxw / GameCanvas.bgW[0] + 1; i++)
			{
				g.drawImage(GameCanvas.imgBG[0], i * GameCanvas.bgW[0], TileMap.pxh - GameCanvas.bgH[0] - 70, 0);
			}
		}
	}

	public void paintBlackSky(mGraphics g)
	{
		if (!GameCanvas.lowGraphic)
		{
			g.fillTrans(imgTrans, 0, 0, GameCanvas.w, GameCanvas.h);
		}
	}

	public void paintCapcha(mGraphics g)
	{
		MobCapcha.paint(g, Char.myCharz().cx, Char.myCharz().cy);
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		if (GameCanvas.menu.showMenu || GameCanvas.panel.isShow || ChatPopup.currChatPopup != null || !GameCanvas.isTouch || DataItem.IsShow)
		{
			return;
		}
		for (int i = 0; i < strCapcha.Length; i++)
		{
			int x = (GameCanvas.w - strCapcha.Length * disXC) / 2 + i * disXC + disXC / 2;
			if (keyCapcha[i] == -1)
			{
				g.drawImage(imgNut, x, GameCanvas.h - 25, 3);
				mFont.tahoma_7b_dark.drawString(g, strCapcha[i] + string.Empty, x, GameCanvas.h - 30, 2);
			}
			else
			{
				g.drawImage(imgNutF, x, GameCanvas.h - 25, 3);
				mFont.tahoma_7b_green2.drawString(g, strCapcha[i] + string.Empty, x, GameCanvas.h - 30, 2);
			}
		}
	}

	public override void paint(mGraphics g)
	{
		countEff = 0;
		if (!isPaint)
		{
			return;
		}
		GameCanvas.debug("PA1", 1);
		if (isUseFreez && ChatPopup.currChatPopup == null)
		{
			dem++;
			if ((dem < 30 && dem >= 0 && GameCanvas.gameTick % 4 == 0) || (dem >= 30 && dem <= 50 && GameCanvas.gameTick % 3 == 0) || dem > 50)
			{
				if (dem <= 50)
				{
					return;
				}
				if (isUseFreez)
				{
					isUseFreez = false;
					dem = 0;
					if (activeRongThan)
					{
						callRongThan(xR, yR);
					}
					else
					{
						hideRongThan();
					}
				}
				paintInfoBar(g);
				g.translate(-cmx, -cmy);
				g.translate(0, GameCanvas.transY);
				Char.myCharz().paint(g);
				mSystem.paintFlyText(g);
				resetTranslate(g);
				paintSelectedSkill(g);
				return;
			}
		}
		GameCanvas.debug("PA2", 1);
		GameCanvas.paintBGGameScr(g);
		paint_ios_bg(g);
		if ((isRongThanXuatHien || isFireWorks) && TileMap.bgID != 3)
		{
			paintBlackSky(g);
		}
		GameCanvas.debug("PA3", 1);
		if (shock_scr > 0)
		{
			g.translate(-cmx + shock_x[shock_scr % shock_x.Length], -cmy + shock_y[shock_scr % shock_y.Length]);
			shock_scr--;
		}
		else
		{
			g.translate(-cmx, -cmy);
		}
		if (isSuperPower)
		{
			int tx = ((GameCanvas.gameTick % 3 != 0) ? (-3) : 3);
			g.translate(tx, 0);
		}
		BackgroudEffect.paintBehindTileAll(g);
		EffecMn.paintLayer1(g);
		if (GraphicsManagement.MapLuoi)
		{
			TileMap.paintTilemapLuoi(g);
		}
		else
		{
			TileMap.paintTilemap(g);
		}
		TileMap.paintOutTilemap(g);
		for (int i = 0; i < vCharInMap.size(); i++)
		{
			Char obj = (Char)vCharInMap.elementAt(i);
			if (obj.isMabuHold && TileMap.mapID == 128)
			{
				obj.paintHeadWithXY(g, obj.cx, obj.cy, 0);
			}
		}
		if (Char.myCharz().isMabuHold && TileMap.mapID == 128)
		{
			Char.myCharz().paintHeadWithXY(g, Char.myCharz().cx, Char.myCharz().cy, 0);
		}
		paintBgItem(g, 2);
		if (Char.myCharz().cmdMenu != null && GameCanvas.isTouch)
		{
			if (mScreen.keyTouch == 20)
			{
				g.drawImage(imgChat2, Char.myCharz().cmdMenu.x + cmx, Char.myCharz().cmdMenu.y + cmy, mGraphics.HCENTER | mGraphics.VCENTER);
			}
			else
			{
				g.drawImage(imgChat, Char.myCharz().cmdMenu.x + cmx, Char.myCharz().cmdMenu.y + cmy, mGraphics.HCENTER | mGraphics.VCENTER);
			}
		}
		GameCanvas.debug("PA4", 1);
		GameCanvas.debug("PA5", 1);
		BackgroudEffect.paintBackAll(g);
		EffectManager.lowEffects.paintAll(g);
		for (int j = 0; j < Effect2.vEffectFeet.size(); j++)
		{
			((Effect2)Effect2.vEffectFeet.elementAt(j)).paint(g);
		}
		for (int k = 0; k < Teleport.vTeleport.size(); k++)
		{
			((Teleport)Teleport.vTeleport.elementAt(k)).paintHole(g);
		}
		for (int l = 0; l < vNpc.size(); l++)
		{
			((Npc)vNpc.elementAt(l)).paint(g);
		}
		g.translate(0, GameCanvas.transY);
		GameCanvas.debug("PA7", 1);
		GameCanvas.debug("PA8", 1);
		for (int m = 0; m < vCharInMap.size(); m++)
		{
			Char obj2 = null;
			try
			{
				obj2 = (Char)vCharInMap.elementAt(m);
			}
			catch (Exception ex)
			{
				Cout.LogError("Loi ham paint char gamesc: " + ex.ToString());
			}
		}
		EffecMn.paintLayer2(g);
		for (int n = 0; n < vMob.size(); n++)
		{
			((Mob)vMob.elementAt(n)).paint(g);
		}
		for (int num = 0; num < Teleport.vTeleport.size(); num++)
		{
			((Teleport)Teleport.vTeleport.elementAt(num)).paint(g);
		}
		for (int num2 = 0; num2 < vCharInMap.size(); num2++)
		{
			Char obj3 = null;
			try
			{
				obj3 = (Char)vCharInMap.elementAt(num2);
			}
			catch (Exception)
			{
			}
			if (obj3 != null && (!GameCanvas.panel.isShow || !GameCanvas.panel.isTypeShop()))
			{
				obj3.paint(g);
			}
		}
		Char.myCharz().paint(g);
		if (Char.myCharz().skillPaint != null && Char.myCharz().skillInfoPaint() != null && Char.myCharz().indexSkill < Char.myCharz().skillInfoPaint().Length)
		{
			Char.myCharz().paintCharWithSkill(g);
			Char.myCharz().paintMount2(g);
		}
		for (int num3 = 0; num3 < vCharInMap.size(); num3++)
		{
			Char obj4 = null;
			try
			{
				obj4 = (Char)vCharInMap.elementAt(num3);
			}
			catch (Exception ex3)
			{
				Cout.LogError("Loi ham paint char gamescr: " + ex3.ToString());
			}
			if (obj4 != null && (!GameCanvas.panel.isShow || !GameCanvas.panel.isTypeShop()) && obj4.skillPaint != null && obj4.skillInfoPaint() != null && obj4.indexSkill < obj4.skillInfoPaint().Length)
			{
				obj4.paintCharWithSkill(g);
				obj4.paintMount2(g);
			}
		}
		for (int num4 = 0; num4 < vItemMap.size(); num4++)
		{
			((ItemMap)vItemMap.elementAt(num4)).paint(g);
		}
		g.translate(0, -GameCanvas.transY);
		GameCanvas.debug("PA9", 1);
		paintSplash(g);
		GameCanvas.debug("PA10", 1);
		GameCanvas.debug("PA11", 1);
		GameCanvas.debug("PA13", 1);
		paintEffect(g);
		paintBgItem(g, 3);
		for (int num5 = 0; num5 < vNpc.size(); num5++)
		{
			((Npc)vNpc.elementAt(num5)).paintName(g);
		}
		EffecMn.paintLayer3(g);
		for (int num6 = 0; num6 < vNpc.size(); num6++)
		{
			Npc npc = (Npc)vNpc.elementAt(num6);
			if (npc.chatInfo != null)
			{
				npc?.chatInfo.paint(g, npc.cx, npc.cy - npc.ch - GameCanvas.transY, npc.cdir);
			}
		}
		for (int num7 = 0; num7 < vCharInMap.size(); num7++)
		{
			Char obj5 = null;
			try
			{
				obj5 = (Char)vCharInMap.elementAt(num7);
			}
			catch (Exception)
			{
			}
			if (obj5 != null && obj5.chatInfo != null)
			{
				obj5.chatInfo.paint(g, obj5.cx, obj5.cy - obj5.ch, obj5.cdir);
			}
		}
		if (Char.myCharz().chatInfo != null)
		{
			Char.myCharz().chatInfo.paint(g, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, Char.myCharz().cdir);
		}
		EffectManager.mid_2Effects.paintAll(g);
		EffectManager.midEffects.paintAll(g);
		PopUp.paintAll(g);
		if (TileMap.mapID == 120)
		{
			if (percentMabu != 100)
			{
				int w = percentMabu * mGraphics.getImageWidth(imgHPLost) / 100;
				_ = percentMabu;
				g.drawImage(imgHPLost, TileMap.pxw / 2 - mGraphics.getImageWidth(imgHPLost) / 2, 220, 0);
				g.setClip(TileMap.pxw / 2 - mGraphics.getImageWidth(imgHPLost) / 2, 220, w, 10);
				g.drawImage(imgHP, TileMap.pxw / 2 - mGraphics.getImageWidth(imgHPLost) / 2, 220, 0);
				g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
			}
			if (mabuEff)
			{
				tMabuEff++;
				if (GameCanvas.gameTick % 3 == 0)
				{
					EffecMn.addEff(new Effect(19, Res.random(TileMap.pxw / 2 - 50, TileMap.pxw / 2 + 50), 340, 2, 1, -1));
				}
				if (GameCanvas.gameTick % 15 == 0)
				{
					EffecMn.addEff(new Effect(18, Res.random(TileMap.pxw / 2 - 5, TileMap.pxw / 2 + 5), Res.random(300, 320), 2, 1, -1));
				}
				if (tMabuEff == 100)
				{
					activeSuperPower(TileMap.pxw / 2, 300);
				}
				if (tMabuEff == 110)
				{
					tMabuEff = 0;
					mabuEff = false;
				}
			}
		}
		bool flag = true;
		for (int num8 = 0; num8 < BackgroudEffect.vBgEffect.size(); num8++)
		{
			if (((BackgroudEffect)BackgroudEffect.vBgEffect.elementAt(num8)).typeEff == 0)
			{
				flag = false;
				break;
			}
		}
		if (mGraphics.zoomLevel <= 1 || Main.isIpod || Main.isIphone4)
		{
			flag = false;
		}
		if (flag)
		{
			_ = isRongThanXuatHien;
		}
		mSystem.paintFlyText(g);
		GameCanvas.debug("PA14", 1);
		GameCanvas.debug("PA15", 1);
		GameCanvas.debug("PA16", 1);
		paintArrowPointToNPC(g);
		GameCanvas.debug("PA17", 1);
		if (!isPaintOther && isPaintRada == 1 && !GameCanvas.panel.isShow && !MainMenu.isShowMenuVIP && !DataItem.IsShow)
		{
			paintInfoBar(g);
		}
		resetTranslate(g);
		if (!isPaintOther)
		{
			MainMod.Paint(g);
			GameCanvas.debug("PA21", 1);
			GameCanvas.debug("PA18", 1);
			g.translate(-g.getTranslateX(), -g.getTranslateY());
			if ((TileMap.mapID == 128 || TileMap.mapID == 127) && mabuPercent != 0)
			{
				int num9 = 30;
				int num10 = 200;
				g.setColor(0);
				g.fillRect(num9 - 27, num10 - 112, 54, 8);
				g.setColor(16711680);
				g.setClip(num9 - 25, num10 - 110, mabuPercent, 4);
				g.fillRect(num9 - 25, num10 - 110, 50, 4);
				g.setClip(0, 0, 3000, 3000);
				mFont.tahoma_7b_white.drawString(g, "Mabu", num9, num10 - 112 + 10, 2, mFont.tahoma_7b_dark);
			}
			GameCanvas.debug("PA19", 1);
			GameCanvas.debug("PA20", 1);
			resetTranslate(g);
			paintSelectedSkill(g);
			GameCanvas.debug("PA22", 1);
			resetTranslate(g);
			if (GameCanvas.isTouch && GameCanvas.isTouchControl)
			{
				paintTouchControl(g);
			}
			resetTranslate(g);
			paintChatVip(g);
			if (!GameCanvas.panel.isShow && !MainMenu.isShowMenuVIP && GameCanvas.currentDialog == null && ChatPopup.currChatPopup == null && ChatPopup.serverChatPopUp == null && GameCanvas.currentScreen.Equals(instance) && !DataItem.IsShow)
			{
				base.paint(g);
				if (mScreen.keyMouse == 1 && cmdMenu != null)
				{
					g.drawImage(ItemMap.imageFlare, cmdMenu.x + 7, cmdMenu.y + 15, 3);
				}
			}
			resetTranslate(g);
			int num11 = 100 + ((Char.vItemTime.size() != 0) ? (textTime.size() * 12) : 0);
			if (Char.myCharz().clan != null)
			{
				int num12 = 0;
				int num13 = 0;
				int num14 = (GameCanvas.h - 100 - 60) / 12;
				for (int num15 = 0; num15 < vCharInMap.size(); num15++)
				{
					Char obj6 = (Char)vCharInMap.elementAt(num15);
					if (obj6.clanID == -1 || obj6.clanID != Char.myCharz().clan.ID)
					{
						continue;
					}
					if (obj6.isOutX() && obj6.cx < Char.myCharz().cx)
					{
						int num16 = num14;
						if (Char.vItemTime.size() != 0)
						{
							num16 -= textTime.size();
						}
						if (num12 <= num16)
						{
							mFont.tahoma_7_green.drawString(g, obj6.cName, 20, num11 - 12 + num12 * 12, mFont.LEFT, mFont.tahoma_7_grey);
							obj6.paintHp(g, 10, num11 + num12 * 12 - 5);
							num12++;
						}
					}
					else if (obj6.isOutX() && obj6.cx > Char.myCharz().cx && num13 <= num14)
					{
						mFont.tahoma_7_green.drawString(g, obj6.cName, GameCanvas.w - 25, num11 - 12 + num13 * 12, mFont.RIGHT, mFont.tahoma_7_grey);
						obj6.paintHp(g, GameCanvas.w - 15, num11 + num13 * 12 - 5);
						num13++;
					}
				}
			}
			ChatTextField.gI().paint(g);
			if (isNewClanMessage && !GameCanvas.panel.isShow && GameCanvas.gameTick % 4 == 0 && !DataItem.IsShow)
			{
				g.drawImage(ItemMap.imageFlare, cmdMenu.x + 15, cmdMenu.y + 30, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			if (isSuperPower)
			{
				dxPower += 5;
				if (tPower >= 0)
				{
					tPower += dxPower;
				}
				Res.outz("x power= " + xPower);
				if (tPower < 0)
				{
					tPower--;
					if (tPower == -20)
					{
						isSuperPower = false;
						tPower = 0;
						dxPower = 0;
					}
				}
				else if ((xPower - tPower > 0 || tPower < TileMap.pxw) && tPower > 0)
				{
					g.setColor(16777215);
					if (!GameCanvas.lowGraphic)
					{
						g.fillArg(0, 0, GameCanvas.w, GameCanvas.h, 0, 0);
					}
					else
					{
						g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
					}
				}
				else
				{
					tPower = -1;
				}
			}
			for (int num17 = 0; num17 < Char.vItemTime.size(); num17++)
			{
				((ItemTime)Char.vItemTime.elementAt(num17)).paint(g, cmdMenu.x + 32 + num17 * 24, 55);
			}
			for (int num18 = 0; num18 < textTime.size(); num18++)
			{
				((ItemTime)textTime.elementAt(num18)).paintText(g, cmdMenu.x + ((Char.vItemTime.size() == 0) ? 25 : 5), ((Char.vItemTime.size() == 0) ? 45 : 90) + num18 * 12);
			}
			paintXoSo(g);
			if (mResources.language == 1)
			{
				long second = mSystem.currentTimeMillis() - deltaTime;
				mFont.tahoma_7b_white.drawString(g, NinjaUtil.getDate2(second), 10, GameCanvas.h - 65, 0, mFont.tahoma_7b_dark);
			}
			if (!yourNumber.Equals(string.Empty))
			{
				for (int num19 = 0; num19 < strPaint.Length; num19++)
				{
					mFont.tahoma_7b_white.drawString(g, strPaint[num19], 5, 85 + num19 * 18, 0, mFont.tahoma_7b_dark);
				}
			}
		}
		int num20 = 0;
		int num21 = GameCanvas.hw;
		if (num21 > 200)
		{
			num21 = 200;
		}
		paintPhuBanBar(g, num20 + GameCanvas.w / 2, 0, num21);
		EffectManager.hiEffects.paintAll(g);
		if (nCT_timeBallte > mSystem.currentTimeMillis() && TileMap.mapID == 170 && isPaint_CT && nCT_nBoyBaller / 2 > 0)
		{
			try
			{
				paint_CT(g, num20 + GameCanvas.w / 2, 0, num21);
			}
			catch (Exception)
			{
			}
		}
		if (TileMap.mapID == 172)
		{
			_ = mResources.WAIT + "  " + nUSER_CT + "/" + nUSER_MAX_CT;
			mFont.tahoma_7b_dark.drawString(g, mResources.WAIT + "  " + nUSER_CT + "/" + nUSER_MAX_CT, GameCanvas.w - 10, 40, 1);
		}
	}

	private void paintXoSo(mGraphics g)
	{
		if (tShow != 0)
		{
			string text = string.Empty;
			for (int i = 0; i < winnumber.Length; i++)
			{
				text = text + randomNumber[i] + " ";
			}
			PopUp.paintPopUp(g, 20, 45, 95, 35, 16777215, isButton: false);
			mFont.tahoma_7b_dark.drawString(g, mResources.kquaVongQuay, 68, 50, 2);
			mFont.tahoma_7b_dark.drawString(g, text + string.Empty, 68, 65, 2);
		}
	}

	private void checkEffToObj(IMapObject obj, bool isnew)
	{
		if (obj == null || tDoubleDelay > 0)
		{
			return;
		}
		tDoubleDelay = 10;
		int x = obj.getX();
		int num = Res.abs(Char.myCharz().cx - x);
		int loopCount = ((num <= 80) ? 1 : ((num > 80 && num <= 200) ? 2 : ((num <= 200 || num > 400) ? 4 : 3)));
		if (!isnew)
		{
			if (obj.Equals(Char.myCharz().mobFocus) || (obj.Equals(Char.myCharz().charFocus) && Char.myCharz().isMeCanAttackOtherPlayer(Char.myCharz().charFocus)))
			{
				ServerEffect.addServerEffect(135, obj.getX(), obj.getY(), loopCount);
			}
			else if (obj.Equals(Char.myCharz().npcFocus) || obj.Equals(Char.myCharz().itemFocus) || obj.Equals(Char.myCharz().charFocus))
			{
				ServerEffect.addServerEffect(136, obj.getX(), obj.getY(), loopCount);
			}
		}
		else
		{
			ServerEffect.addServerEffect(136, obj.getX(), obj.getY(), loopCount);
		}
	}

	private void updateClickToArrow()
	{
		if (tDoubleDelay > 0)
		{
			tDoubleDelay--;
		}
		if (clickMoving)
		{
			clickMoving = false;
			IMapObject mapObject = findClickToItem(clickToX, clickToY);
			if (mapObject == null || (mapObject != null && mapObject.Equals(Char.myCharz().npcFocus) && TileMap.mapID == 51))
			{
				ServerEffect.addServerEffect(134, clickToX, clickToY + GameCanvas.transY / 2, 3);
			}
		}
	}

	private void paintWaypointArrow(mGraphics g)
	{
		int num = 10;
		Task taskMaint = Char.myCharz().taskMaint;
		if (taskMaint != null && taskMaint.taskId == 0 && ((taskMaint.index != 1 && taskMaint.index < 6) || taskMaint.index == 0))
		{
			return;
		}
		for (int i = 0; i < TileMap.vGo.size(); i++)
		{
			Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
			if (waypoint.minY == 0 || waypoint.maxY >= TileMap.pxh - 24)
			{
				if (waypoint.maxY <= TileMap.pxh / 2)
				{
					int x = waypoint.minX + (waypoint.maxX - waypoint.minX) / 2;
					int y = waypoint.minY + (waypoint.maxY - waypoint.minY) / 2 + runArrow;
					if (GameCanvas.isTouch)
					{
						y = waypoint.maxY + (waypoint.maxY - waypoint.minY) + runArrow + num;
					}
					g.drawRegion(arrow, 0, 0, 13, 16, 6, x, y, StaticObj.VCENTER_HCENTER);
				}
				else if (waypoint.minY >= TileMap.pxh / 2)
				{
					g.drawRegion(arrow, 0, 0, 13, 16, 4, waypoint.minX + (waypoint.maxX - waypoint.minX) / 2, waypoint.minY - 12 - runArrow, StaticObj.VCENTER_HCENTER);
				}
			}
			else if (waypoint.minX >= 0 && waypoint.minX < 24)
			{
				if (!GameCanvas.isTouch)
				{
					g.drawRegion(arrow, 0, 0, 13, 16, 2, waypoint.maxX + 12 + runArrow, waypoint.maxY - 12, StaticObj.VCENTER_HCENTER);
				}
				else
				{
					g.drawRegion(arrow, 0, 0, 13, 16, 2, waypoint.maxX + 12 + runArrow, waypoint.maxY - 32, StaticObj.VCENTER_HCENTER);
				}
			}
			else if (waypoint.minX <= TileMap.tmw * 24 && waypoint.minX >= TileMap.tmw * 24 - 48)
			{
				if (!GameCanvas.isTouch)
				{
					g.drawRegion(arrow, 0, 0, 13, 16, 0, waypoint.minX - 12 - runArrow, waypoint.maxY - 12, StaticObj.VCENTER_HCENTER);
				}
				else
				{
					g.drawRegion(arrow, 0, 0, 13, 16, 0, waypoint.minX - 12 - runArrow, waypoint.maxY - 32, StaticObj.VCENTER_HCENTER);
				}
			}
			else
			{
				g.drawRegion(arrow, 0, 0, 13, 16, 4, waypoint.minX + (waypoint.maxX - waypoint.minX) / 2, waypoint.maxY - 48 - runArrow, StaticObj.VCENTER_HCENTER);
			}
		}
	}

	public static Npc findNPCInMap(short id)
	{
		for (int i = 0; i < vNpc.size(); i++)
		{
			Npc npc = (Npc)vNpc.elementAt(i);
			if (npc.template.npcTemplateId == id)
			{
				return npc;
			}
		}
		return null;
	}

	public static Char findCharInMap(int charId)
	{
		for (int i = 0; i < vCharInMap.size(); i++)
		{
			Char obj = (Char)vCharInMap.elementAt(i);
			if (obj.charID == charId)
			{
				return obj;
			}
		}
		return null;
	}

	public static void RemovefindCharInMap(int charId)
	{
		for (int i = 0; i < vCharInMap.size(); i++)
		{
			Char obj = (Char)vCharInMap.elementAt(i);
			if (obj.charID == charId)
			{
				vCharInMap.removeElementAt(i);
				i--;
			}
		}
	}

	public static Mob findMobInMap(sbyte mobIndex)
	{
		return (Mob)vMob.elementAt(mobIndex);
	}

	public static Mob findMobInMap(int mobId)
	{
		for (int i = 0; i < vMob.size(); i++)
		{
			Mob mob = (Mob)vMob.elementAt(i);
			if (mob.mobId == mobId)
			{
				return mob;
			}
		}
		return null;
	}

	public static Npc getNpcTask()
	{
		for (int i = 0; i < vNpc.size(); i++)
		{
			Npc npc = (Npc)vNpc.elementAt(i);
			if (npc.template.npcTemplateId == getTaskNpcId())
			{
				return npc;
			}
		}
		return null;
	}

	private void paintArrowPointToNPC(mGraphics g)
	{
		try
		{
			if (ChatPopup.currChatPopup != null)
			{
				return;
			}
			int taskNpcId = getTaskNpcId();
			if (taskNpcId == -1)
			{
				return;
			}
			Npc npc = null;
			for (int i = 0; i < vNpc.size(); i++)
			{
				Npc npc2 = (Npc)vNpc.elementAt(i);
				if (npc2.template.npcTemplateId == taskNpcId)
				{
					if (npc == null)
					{
						npc = npc2;
					}
					else if (Res.abs(npc2.cx - Char.myCharz().cx) < Res.abs(npc.cx - Char.myCharz().cx))
					{
						npc = npc2;
					}
				}
			}
			if (npc == null || npc.statusMe == 15 || (npc.cx > cmx && npc.cx < cmx + gW && npc.cy > cmy && npc.cy < cmy + gH) || GameCanvas.gameTick % 10 < 5)
			{
				return;
			}
			int num = npc.cx - Char.myCharz().cx;
			int num2 = npc.cy - Char.myCharz().cy;
			int x = 0;
			int y = 0;
			int arg = 0;
			if (num > 0 && num2 >= 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					x = gW - 10;
					y = gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						y = gH / 2 + 10;
					}
					arg = 0;
				}
				else
				{
					x = gW / 2;
					y = gH - 10;
					arg = 5;
				}
			}
			else if (num >= 0 && num2 < 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					x = gW - 10;
					y = gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						y = gH / 2 + 10;
					}
					arg = 0;
				}
				else
				{
					x = gW / 2;
					y = 10;
					arg = 6;
				}
			}
			if (num < 0 && num2 >= 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					x = 10;
					y = gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						y = gH / 2 + 10;
					}
					arg = 3;
				}
				else
				{
					x = gW / 2;
					y = gH - 10;
					arg = 5;
				}
			}
			else if (num <= 0 && num2 < 0)
			{
				if (Res.abs(num) >= Res.abs(num2))
				{
					x = 10;
					y = gH / 2 + 30;
					if (GameCanvas.isTouch)
					{
						y = gH / 2 + 10;
					}
					arg = 3;
				}
				else
				{
					x = gW / 2;
					y = 10;
					arg = 6;
				}
			}
			resetTranslate(g);
			g.drawRegion(arrow, 0, 0, 13, 16, arg, x, y, StaticObj.VCENTER_HCENTER);
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi ham arrow to npc: " + ex.ToString());
		}
	}

	public static void resetTranslate(mGraphics g)
	{
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		g.setClip(0, -200, GameCanvas.w, 200 + GameCanvas.h);
	}

	private void paintTouchControl(mGraphics g)
	{
		if (isNotPaintTouchControl())
		{
			return;
		}
		resetTranslate(g);
		if (!TileMap.isOfflineMap() && !isVS())
		{
			if (mScreen.keyTouch == 15 || mScreen.keyMouse == 15)
			{
				g.drawImage((!Main.isPC) ? imgChat2 : imgChatsPC2, xC + 17, yC + 17 + mGraphics.addYWhenOpenKeyBoard, mGraphics.HCENTER | mGraphics.VCENTER);
			}
			else
			{
				g.drawImage((!Main.isPC) ? imgChat : imgChatPC, xC + 17, yC + 17 + mGraphics.addYWhenOpenKeyBoard, mGraphics.HCENTER | mGraphics.VCENTER);
			}
			if (mScreen.keyTouch == 15 || mScreen.keyMouse == 15)
			{
				g.drawImage((!Main.isPC) ? imgChatsPC2 : imgChatsPC2, xC + 17, yC + 17 + mGraphics.addYWhenOpenKeyBoard, mGraphics.HCENTER | mGraphics.VCENTER);
			}
			else
			{
				g.drawImage((!Main.isPC) ? imgChatsPC2 : imgChatPC, xC + 17, yC + 17 + mGraphics.addYWhenOpenKeyBoard, mGraphics.HCENTER | mGraphics.VCENTER);
			}
		}
		_ = isUseTouch;
	}

	public void paintImageBarRight(mGraphics g, Char c)
	{
		int num = (int)(c.cHP * hpBarW / c.cHPFull);
		int num2 = (int)(c.cMP * mpBarW / c.cMPFull);
		int num3 = (int)(dHP * hpBarW / c.cHPFull);
		int num4 = (int)(dMP * mpBarW / c.cMPFull);
		g.setClip(GameCanvas.w / 2 + 58 - mGraphics.getImageWidth(imgPanel), 0, 95, 100);
		g.drawRegion(imgPanel, 0, 0, mGraphics.getImageWidth(imgPanel), mGraphics.getImageHeight(imgPanel), 2, GameCanvas.w / 2 + 60, 0, mGraphics.RIGHT | mGraphics.TOP);
		g.setClip((int)(GameCanvas.w / 2 + 60 - 83 - hpBarW + hpBarW - num3), 5, num3, 10);
		g.drawImage(imgHPLost, GameCanvas.w / 2 + 60 - 83, 5, mGraphics.RIGHT | mGraphics.TOP);
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		g.setClip((int)(GameCanvas.w / 2 + 60 - 83 - hpBarW + hpBarW - num), 5, num, 10);
		g.drawImage(imgHP, GameCanvas.w / 2 + 60 - 83, 5, mGraphics.RIGHT | mGraphics.TOP);
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		g.setClip((int)(GameCanvas.w / 2 + 60 - 83 - mpBarW + hpBarW - num4), 20, num4, 6);
		g.drawImage(imgMPLost, GameCanvas.w / 2 + 60 - 83, 20, mGraphics.RIGHT | mGraphics.TOP);
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		g.setClip((int)(GameCanvas.w / 2 + 60 - 83 - mpBarW + hpBarW - num2), 20, num2, 6);
		g.drawImage(imgMP, GameCanvas.w / 2 + 60 - 83, 20, mGraphics.RIGHT | mGraphics.TOP);
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
	}

	private void paintImageBar(mGraphics g, bool isLeft, Char c)
	{
		if (c != null)
		{
			int num;
			int num2;
			int num3;
			int num4;
			if (c.charID == Char.myCharz().charID)
			{
				num = (int)(dHP * hpBarW / c.cHPFull);
				num2 = (int)(dMP * mpBarW / c.cMPFull);
				num3 = (int)(c.cHP * hpBarW / c.cHPFull);
				num4 = (int)(c.cMP * mpBarW / c.cMPFull);
			}
			else
			{
				num = (int)(c.dHP * hpBarW / c.cHPFull);
				num2 = c.perCentMp * mpBarW / 100;
				num3 = (int)(c.cHP * hpBarW / c.cHPFull);
				num4 = c.perCentMp * mpBarW / 100;
			}
			if (Char.myCharz().secondPower > 0)
			{
				int w = Char.myCharz().powerPoint * spBarW / Char.myCharz().maxPowerPoint;
				g.drawImage(imgPanel2, 58, 29, 0);
				g.setClip(83, 31, w, 10);
				g.drawImage(imgSP, 83, 31, 0);
				g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
				mFont.tahoma_7_white.drawString(g, Char.myCharz().strInfo + ":" + Char.myCharz().powerPoint + "/" + Char.myCharz().maxPowerPoint, 115, 29, 2);
			}
			if (c.charID != Char.myCharz().charID)
			{
				g.setClip(mGraphics.getImageWidth(imgPanel) - 95, 0, 95, 100);
			}
			g.drawImage(imgPanel, 0, 0, 0);
			if (isLeft)
			{
				g.setClip(83, 5, num, 10);
			}
			else
			{
				g.setClip((int)(83 + hpBarW - num), 5, num, 10);
			}
			g.drawImage(imgHPLost, 83, 5, 0);
			g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
			if (isLeft)
			{
				g.setClip(83, 5, num3, 10);
			}
			else
			{
				g.setClip((int)(83 + hpBarW - num3), 5, num3, 10);
			}
			g.drawImage(imgHP, 83, 5, 0);
			g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
			if (isLeft)
			{
				g.setClip(83, 20, num2, 6);
			}
			else
			{
				g.setClip(83 + mpBarW - num2, 20, num2, 6);
			}
			g.drawImage(imgMPLost, 83, 20, 0);
			g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
			if (isLeft)
			{
				g.setClip(83, 20, num2, 6);
			}
			else
			{
				g.setClip(83 + mpBarW - num4, 20, num4, 6);
			}
			g.drawImage(imgMP, 83, 20, 0);
			g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
			if (Char.myCharz().cMP == 0L && GameCanvas.gameTick % 10 > 5)
			{
				g.setClip(83, 20, 2, 6);
				g.drawImage(imgMPLost, 83, 20, 0);
				g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
			}
			if (mGraphics.zoomLevel > 1)
			{
				GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
				gUIStyle.normal.textColor = Color.yellow;
				gUIStyle.fontSize = 17;
				g.drawString(NinjaUtil.getMoneys(c.cHP), 85, 4, gUIStyle);
				gUIStyle.fontSize = 10;
				g.drawString(NinjaUtil.getMoneys(c.cMP), 85, 19, gUIStyle);
			}
		}
	}

	public void getInjure()
	{
	}

	public void starVS()
	{
		curr = (last = mSystem.currentTimeMillis());
		secondVS = 180;
	}

	private Char findCharVS1()
	{
		for (int i = 0; i < vCharInMap.size(); i++)
		{
			Char obj = (Char)vCharInMap.elementAt(i);
			if (obj.cTypePk != 0)
			{
				return obj;
			}
		}
		return null;
	}

	private Char findCharVS2()
	{
		for (int i = 0; i < vCharInMap.size(); i++)
		{
			Char obj = (Char)vCharInMap.elementAt(i);
			if (obj.cTypePk != 0 && obj != findCharVS1())
			{
				return obj;
			}
		}
		return null;
	}

	private void paintInfoBar(mGraphics g)
	{
		resetTranslate(g);
		if (TileMap.mapID == 130 && findCharVS1() != null && findCharVS2() != null)
		{
			g.translate(GameCanvas.w / 2 - 62, 0);
			paintImageBar(g, isLeft: true, findCharVS1());
			g.translate(-(GameCanvas.w / 2 - 65), 0);
			paintImageBarRight(g, findCharVS2());
			findCharVS1().paintHeadWithXY(g, 137, 25, 0);
			findCharVS2().paintHeadWithXY(g, GameCanvas.w - 15 - 122, 25, 2);
		}
		else if (isVS() && Char.myCharz().charFocus != null)
		{
			g.translate(GameCanvas.w / 2 - 62, 0);
			paintImageBar(g, isLeft: true, Char.myCharz().charFocus);
			g.translate(-(GameCanvas.w / 2 - 65), 0);
			paintImageBarRight(g, Char.myCharz());
			Char.myCharz().paintHeadWithXY(g, 137, 25, 0);
			Char.myCharz().charFocus.paintHeadWithXY(g, GameCanvas.w - 15 - 122, 25, 2);
		}
		else if (ispaintPhubangBar() && isSmallScr())
		{
			paintHPBar_NEW(g, 1, 1, Char.myCharz());
		}
		else
		{
			paintImageBar(g, isLeft: true, Char.myCharz());
			if (Char.myCharz().isInEnterOfflinePoint() != null || Char.myCharz().isInEnterOnlinePoint() != null)
			{
				mFont.tahoma_7_green2.drawString(g, mResources.enter, imgScrW / 2, 8 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
			}
			else if (Char.myCharz().mobFocus != null)
			{
				if (Char.myCharz().mobFocus.getTemplate() != null)
				{
					mFont.tahoma_7b_green2.drawString(g, Char.myCharz().mobFocus.getTemplate().name, imgScrW / 2, 9 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
				}
				if (Char.myCharz().mobFocus.templateId != 0)
				{
					mFont.tahoma_7b_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().mobFocus.hp) + string.Empty, imgScrW / 2, 22 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
				}
			}
			else if (Char.myCharz().npcFocus != null)
			{
				mFont.tahoma_7b_green2.drawString(g, Char.myCharz().npcFocus.template.name, imgScrW / 2, 9 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
				if (Char.myCharz().npcFocus.template.npcTemplateId == 4)
				{
					mFont.tahoma_7b_green2.drawString(g, gI().magicTree.currPeas + "/" + gI().magicTree.maxPeas, imgScrW / 2, 22 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
				}
			}
			else if (Char.myCharz().charFocus != null)
			{
				mFont.tahoma_7b_green2.drawString(g, Char.myCharz().charFocus.cName, imgScrW / 2, 9 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
				mFont.tahoma_7b_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().charFocus.cHP) + string.Empty, imgScrW / 2, 22 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
			}
			else
			{
				mFont.tahoma_7b_green2.drawString(g, Char.myCharz().cName, imgScrW / 2, 9 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
				mFont.tahoma_7b_green2.drawString(g, NinjaUtil.getMoneys(Char.myCharz().cPower) + string.Empty, imgScrW / 2, 22 + mGraphics.addYWhenOpenKeyBoard, mFont.CENTER);
			}
		}
		g.translate(-g.getTranslateX(), -g.getTranslateY());
		if (isVS() && secondVS > 0)
		{
			curr = mSystem.currentTimeMillis();
			if (curr - last >= 1000)
			{
				last = mSystem.currentTimeMillis();
				secondVS--;
			}
			mFont.tahoma_7b_white.drawString(g, secondVS + string.Empty, GameCanvas.w / 2, 13, 2, mFont.tahoma_7b_dark);
		}
		if (flareFindFocus)
		{
			g.drawImage(ItemMap.imageFlare, 40, 35, mGraphics.BOTTOM | mGraphics.HCENTER);
			flareTime--;
			if (flareTime < 0)
			{
				flareTime = 0;
				flareFindFocus = false;
			}
		}
	}

	public bool isVS()
	{
		if (TileMap.isVoDaiMap())
		{
			if (Char.myCharz().cTypePk == 0)
			{
				if (TileMap.mapID == 130 && findCharVS1() != null)
				{
					return findCharVS2() != null;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private void paintSelectedSkill(mGraphics g)
	{
		if (mobCapcha != null)
		{
			paintCapcha(g);
		}
		else
		{
			if (GameCanvas.currentDialog != null || ChatPopup.currChatPopup != null || GameCanvas.menu.showMenu || isPaintPopup() || GameCanvas.panel.isShow || Char.myCharz().taskMaint.taskId == 0 || ChatTextField.gI().isShow || GameCanvas.currentScreen == MoneyCharge.instance || MainMenu.isShowMenuVIP || DataItem.IsShow)
			{
				return;
			}
			long num = mSystem.currentTimeMillis() - lastUsePotion;
			int num2 = 0;
			if (num < 10000)
			{
				num2 = (int)(num * 20 / 10000);
			}
			if (!GameCanvas.isTouch)
			{
				g.drawImage((mScreen.keyTouch != 10) ? imgSkill : imgSkill2, xSkill + xHP - 1, yHP - 1, 0);
				SmallImage.drawSmallImage(g, 542, xSkill + xHP + 3, yHP + 3, 0, 0);
				mFont.number_gray.drawString(g, string.Empty + hpPotion, xSkill + xHP + 22, yHP + 15, 1);
				if (num < 10000)
				{
					g.setColor(2721889);
					num2 = (int)(num * 20 / 10000);
					g.fillRect(xSkill + xHP + 3, yHP + 3 + num2, 20, 20 - num2);
				}
			}
			else if (Char.myCharz().statusMe != 14)
			{
				if (gamePad.isSmallGamePad)
				{
					if (isAnalog != 1)
					{
						g.setColor(9670800);
						g.fillRect(xHP + 9, yHP + 10 + 10, 22, 20);
						g.setColor(16777215);
						g.fillRect(xHP + 9, yHP + 10 + ((num2 != 0) ? (20 - num2) : 0) + 10, 22, (num2 == 0) ? 20 : num2);
						g.drawImage((mScreen.keyTouch != 10) ? imgHP1 : imgHP2, xHP, yHP + 10, 0);
						mFont.tahoma_7_red.drawString(g, string.Empty + hpPotion, xHP + 20, yHP + 15 + 10, 2);
						if (isPickNgocRong)
						{
							g.drawImage((mScreen.keyTouch != 14) ? imgNR1 : imgNR2, xHP + 5, yHP - 6 - 40 + 10, 0);
						}
						else if (isudungCapsun4)
						{
							g.drawImage((mScreen.keyTouch != 14) ? imgNutF : imgNut, xHP + 5, yHP - 6 - 40 + 10, 0);
							SmallImage.drawSmallImage(g, 1088, xHP - 7 + 5, yHP - 6 - 40 - 7 + 10, 0, 0);
						}
						else if (isudungCapsun3)
						{
							g.drawImage((mScreen.keyTouch != 14) ? imgNutF : imgNut, xHP + 5, yHP - 6 - 40 + 10, 0);
							SmallImage.drawSmallImage(g, 1087, xHP - 7 + 5, yHP - 6 - 40 - 7 + 10, 0, 0);
						}
					}
					else if (isAnalog == 1)
					{
						int num3 = 10;
						g.drawImage((mScreen.keyTouch != 10) ? imgSkill : imgSkill2, xSkill + xHP - 1, yHP - 1 + num3, 0);
						SmallImage.drawSmallImage(g, 542, xSkill + xHP + 3, yHP + 3 + num3, 0, 0);
						mFont.number_gray.drawString(g, string.Empty + hpPotion, xSkill + xHP + 22, yHP + 13 + num3, 1);
						if (num < 10000)
						{
							g.setColor(2721889);
							num2 = (int)(num * 20 / 10000);
							g.fillRect(xSkill + xHP + 3, yHP + 3 + num2 + num3, 20, 20 - num2);
						}
						if (isPickNgocRong)
						{
							g.drawImage((mScreen.keyTouch != 14) ? imgNR3 : imgNR4, xHP + 20 + 5, yHP + 20 - 6 - 40 + 10, mGraphics.HCENTER | mGraphics.VCENTER);
						}
						else if (isudungCapsun4)
						{
							g.drawImage((mScreen.keyTouch != 14) ? imgNut : imgNutF, xHP + 20 + 5, yHP + 20 - 6 - 40 + 10, mGraphics.HCENTER | mGraphics.VCENTER);
							SmallImage.drawSmallImage(g, 1088, xHP + 20 - 7 + 5, yHP + 20 - 6 - 40 - 7 + 10, 0, 0);
						}
						else if (isudungCapsun3)
						{
							g.drawImage((mScreen.keyTouch != 14) ? imgNut : imgNutF, xHP + 20 + 5, yHP + 20 - 6 - 40 + 10, mGraphics.HCENTER | mGraphics.VCENTER);
							SmallImage.drawSmallImage(g, 1087, xHP + 20 - 7 + 5, yHP + 20 - 6 - 40 - 7 + 10, 0, 0);
						}
					}
				}
				else if (isAnalog != 1)
				{
					g.setColor(9670800);
					g.fillRect(xHP + 9, yHP + 10 - 6, 22, 20);
					g.setColor(16777215);
					g.fillRect(xHP + 9, yHP + 10 + ((num2 != 0) ? (20 - num2) : 0) - 6, 22, (num2 == 0) ? 20 : num2);
					g.drawImage((mScreen.keyTouch != 10) ? imgHP1 : imgHP2, xHP, yHP - 6, 0);
					mFont.tahoma_7_red.drawString(g, string.Empty + hpPotion, xHP + 20, yHP + 15 - 6, 2);
					if (isPickNgocRong)
					{
						g.drawImage((mScreen.keyTouch != 14) ? imgNR1 : imgNR2, xHP, yHP - 6 - 40, 0);
					}
					else if (isudungCapsun4)
					{
						g.drawImage((mScreen.keyTouch != 14) ? imgNut : imgNutF, xHP + 20, yHP + 20 - 6 - 40, mGraphics.HCENTER | mGraphics.VCENTER);
						SmallImage.drawSmallImage(g, 1088, xHP + 20 - 7, yHP + 20 - 6 - 40 - 7, 0, 0);
					}
					else if (isudungCapsun3)
					{
						g.drawImage((mScreen.keyTouch != 14) ? imgNut : imgNutF, xHP + 20, yHP + 20 - 6 - 40, mGraphics.HCENTER | mGraphics.VCENTER);
						SmallImage.drawSmallImage(g, 1087, xHP + 20 - 7, yHP + 20 - 6 - 40 - 7, 0, 0);
					}
				}
				else
				{
					g.setColor(9670800);
					g.fillRect(xHP + 10, yHP + 10 - 6 + 10, 20, 18);
					g.setColor(16777215);
					g.fillRect(xHP + 10, yHP + 10 + ((num2 != 0) ? (20 - num2) : 0) - 6 + 10, 20, (num2 == 0) ? 18 : num2);
					g.drawImage((mScreen.keyTouch != 10) ? imgHP3 : imgHP4, xHP + 20, yHP + 20 - 6 + 10, mGraphics.HCENTER | mGraphics.VCENTER);
					mFont.tahoma_7_red.drawString(g, string.Empty + hpPotion, xHP + 20, yHP + 15 - 6 + 10, 2);
					if (isPickNgocRong)
					{
						g.drawImage((mScreen.keyTouch != 14) ? imgNR3 : imgNR4, xHP + 20 + 5, yHP + 20 - 6 - 40 + 10, mGraphics.HCENTER | mGraphics.VCENTER);
					}
					else if (isudungCapsun4)
					{
						g.drawImage((mScreen.keyTouch != 14) ? imgNut : imgNutF, xHP + 20 + 5, yHP + 20 - 6 - 40 + 10, mGraphics.HCENTER | mGraphics.VCENTER);
						SmallImage.drawSmallImage(g, 1088, xHP + 20 - 7 + 5, yHP + 20 - 6 - 40 - 7 + 10, 0, 0);
					}
					else if (isudungCapsun3)
					{
						g.drawImage((mScreen.keyTouch != 14) ? imgNut : imgNutF, xHP + 20 + 5, yHP + 20 - 6 - 40 + 10, mGraphics.HCENTER | mGraphics.VCENTER);
						SmallImage.drawSmallImage(g, 1087, xHP + 20 - 7 + 5, yHP + 20 - 6 - 40 - 7 + 10, 0, 0);
					}
				}
			}
			if (isHaveSelectSkill)
			{
				Skill[] array = (Main.isPC ? keySkill : ((!GameCanvas.isTouch) ? keySkill : onScreenSkill));
				_ = mScreen.keyTouch;
				if (!GameCanvas.isTouch)
				{
					g.setColor(11152401);
					g.fillRect(xSkill + xHP + 2, yHP - 10 + 6, 20, 10);
					mFont.tahoma_7_white.drawString(g, "*", xSkill + xHP + 12, yHP - 8 + 6, mFont.CENTER);
				}
				int num4 = (Main.isPC ? array.Length : ((!GameCanvas.isTouch) ? array.Length : nSkill));
				for (int i = 0; i < num4; i++)
				{
					if (Main.isPC)
					{
						string[] array3;
						if (TField.isQwerty)
						{
							string[] array2 = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", null };
							array3 = array2;
							array2[9] = "0";
						}
						else
						{
							string[] array4 = new string[5] { "7", "8", "9", "10", null };
							array3 = array4;
							array4[4] = "11";
						}
						string[] array5 = array3;
						int num5 = -13;
						if (mGraphics.zoomLevel == 1 && num4 > 5 && i < 5)
						{
							num5 = 27;
						}
						mFont.tahoma_7b_dark.drawString(g, array5[i], xSkill + xS[i] + 14, yS[i] + num5, mFont.CENTER);
						mFont.tahoma_7b_white.drawString(g, array5[i], xSkill + xS[i] + 14, yS[i] + num5 + 1, mFont.CENTER);
					}
					else if (!GameCanvas.isTouch)
					{
						string[] array7;
						if (TField.isQwerty)
						{
							string[] array6 = new string[5] { "Q", "W", "E", "R", null };
							array7 = array6;
							array6[4] = "T";
						}
						else
						{
							string[] array8 = new string[5] { "7", "8", "9", "1", null };
							array7 = array8;
							array8[4] = "3";
						}
						string[] array9 = array7;
						g.setColor(11152401);
						g.fillRect(xSkill + xS[i] + 2, yS[i] - 10 + 8, 20, 10);
						mFont.tahoma_7_white.drawString(g, array9[i], xSkill + xS[i] + 12, yS[i] - 10 + 6, mFont.CENTER);
					}
					Skill skill = array[i];
					if (skill != Char.myCharz().myskill)
					{
						g.drawImage(imgSkill, xSkill + xS[i] - 1, yS[i] - 1, 0);
					}
					if (skill == null)
					{
						continue;
					}
					if (skill == Char.myCharz().myskill)
					{
						g.drawImage(imgSkill2, xSkill + xS[i] - 1, yS[i] - 1, 0);
						if (GameCanvas.isTouch && !Main.isPC)
						{
							g.drawRegion(Mob.imgHP, 0, 12, 9, 6, 0, xSkill + xS[i] + 8, yS[i] - 7, 0);
						}
					}
					skill.paint(xSkill + xS[i] + 13, yS[i] + 13, g);
					if ((i == selectedIndexSkill && !isPaintUI() && GameCanvas.gameTick % 10 > 5) || i == keyTouchSkill)
					{
						g.drawImage(ItemMap.imageFlare, xSkill + xS[i] + 13, yS[i] + 14, 3);
					}
					long num6 = skill.coolDown - mSystem.currentTimeMillis() + skill.lastTimeUseThisSkill;
					mFont.tahoma_7b_dark.drawString(g, (num6 > 0) ? string.Concat(num6 / 1000) : string.Empty, xSkill + xS[i] + 14, yS[i] + 8, mFont.CENTER, mFont.tahoma_7_red);
					if (AutoSkill.isAutoUseSkills != null && i < AutoSkill.isAutoUseSkills.Length && AutoSkill.isAutoUseSkills[i])
					{
						g.setColor(0);
						g.drawRect(xSkill + xS[i] + 2, yS[i] + 2, 21, 21);
					}
				}
			}
			paintGamePad(g);
		}
	}

	public void paintOpen(mGraphics g)
	{
		if (isstarOpen)
		{
			g.translate(-g.getTranslateX(), -g.getTranslateY());
			g.fillRect(0, 0, GameCanvas.w, moveUp);
			g.setColor(10275899);
			g.fillRect(0, moveUp - 1, GameCanvas.w, 1);
			g.fillRect(0, moveDow + 1, GameCanvas.w, 1);
		}
	}

	public static void startFlyText(string flyString, int x, int y, int dx, int dy, int color)
	{
		int num = -1;
		for (int i = 0; i < 5; i++)
		{
			if (flyTextState[i] == -1)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			return;
		}
		flyTextColor[num] = color;
		flyTextString[num] = Res.FormatStringVIP(flyString);
		flyTextX[num] = x;
		flyTextY[num] = y;
		flyTextDx[num] = dx;
		flyTextDy[num] = ((dy >= 0) ? 5 : (-5));
		flyTextState[num] = 0;
		flyTime[num] = 0;
		flyTextYTo[num] = 10;
		for (int j = 0; j < 5; j++)
		{
			if (flyTextState[j] != -1 && num != j && flyTextDy[num] < 0 && Res.abs(flyTextX[num] - flyTextX[j]) <= 20 && flyTextYTo[num] == flyTextYTo[j])
			{
				flyTextYTo[num] += 10;
			}
		}
	}

	public static void updateFlyText()
	{
		for (int i = 0; i < 5; i++)
		{
			if (flyTextState[i] == -1)
			{
				continue;
			}
			if (flyTextState[i] > flyTextYTo[i])
			{
				flyTime[i]++;
				if (flyTime[i] == 25)
				{
					flyTime[i] = 0;
					flyTextState[i] = -1;
					flyTextYTo[i] = 0;
					flyTextDx[i] = 0;
					flyTextX[i] = 0;
				}
			}
			else
			{
				flyTextState[i] += Res.abs(flyTextDy[i]);
				flyTextX[i] += flyTextDx[i];
				flyTextY[i] += flyTextDy[i];
			}
		}
	}

	public static void loadSplash()
	{
		if (imgSplash == null)
		{
			imgSplash = new Image[3];
			for (int i = 0; i < 3; i++)
			{
				imgSplash[i] = GameCanvas.loadImage("/e/sp" + i + ".png");
			}
		}
		splashX = new int[2];
		splashY = new int[2];
		splashState = new int[2];
		splashF = new int[2];
		splashDir = new int[2];
		splashState[0] = (splashState[1] = -1);
	}

	public static bool startSplash(int x, int y, int dir)
	{
		int num = ((splashState[0] != -1) ? 1 : 0);
		if (splashState[num] != -1)
		{
			return false;
		}
		splashState[num] = 0;
		splashDir[num] = dir;
		splashX[num] = x;
		splashY[num] = y;
		return true;
	}

	public static void updateSplash()
	{
		for (int i = 0; i < 2; i++)
		{
			if (splashState[i] != -1)
			{
				splashState[i]++;
				splashX[i] += splashDir[i] << 2;
				splashY[i]--;
				if (splashState[i] >= 6)
				{
					splashState[i] = -1;
				}
				else
				{
					splashF[i] = (splashState[i] >> 1) % 3;
				}
			}
		}
	}

	public static void paintSplash(mGraphics g)
	{
		for (int i = 0; i < 2; i++)
		{
			if (splashState[i] != -1)
			{
				if (splashDir[i] == 1)
				{
					g.drawImage(imgSplash[splashF[i]], splashX[i], splashY[i], 3);
				}
				else
				{
					g.drawRegion(imgSplash[splashF[i]], 0, 0, mGraphics.getImageWidth(imgSplash[splashF[i]]), mGraphics.getImageHeight(imgSplash[splashF[i]]), 2, splashX[i], splashY[i], 3);
				}
			}
		}
	}

	private void loadInforBar()
	{
		imgScrW = 84;
		hpBarW = 66L;
		mpBarW = 59;
		hpBarX = 52;
		hpBarY = 10;
		spBarW = 61;
		expBarW = gW - 61;
	}

	public void updateSS()
	{
		if (indexMenu != -1)
		{
			if (cmySK != cmtoYSK)
			{
				cmvySK = cmtoYSK - cmySK << 2;
				cmdySK += cmvySK;
				cmySK += cmdySK >> 4;
				cmdySK &= 15;
			}
			if (Math.Abs(cmtoYSK - cmySK) < 15 && cmySK < 0)
			{
				cmtoYSK = 0;
			}
			if (Math.Abs(cmtoYSK - cmySK) < 15 && cmySK > cmyLimSK)
			{
				cmtoYSK = cmyLimSK;
			}
		}
	}

	public void updateKeyAlert()
	{
		if (!isPaintAlert || GameCanvas.currentDialog != null)
		{
			return;
		}
		bool flag = false;
		if (GameCanvas.keyPressed[Key.NUM8])
		{
			indexRow++;
			if (indexRow >= texts.size())
			{
				indexRow = 0;
			}
			flag = true;
		}
		else if (GameCanvas.keyPressed[Key.NUM2])
		{
			indexRow--;
			if (indexRow < 0)
			{
				indexRow = texts.size() - 1;
			}
			flag = true;
		}
		if (flag)
		{
			scrMain.moveTo(indexRow * scrMain.ITEM_SIZE);
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
		}
		if (GameCanvas.isTouch)
		{
			ScrollResult scrollResult = scrMain.updateKey();
			if (scrollResult.isDowning || scrollResult.isFinish)
			{
				indexRow = scrollResult.selected;
				flag = true;
			}
		}
		if (!flag || indexRow < 0 || indexRow >= texts.size())
		{
			return;
		}
		string text = (string)texts.elementAt(indexRow);
		fnick = null;
		alertURL = null;
		center = null;
		ChatTextField.gI().center = null;
		int startIndex;
		if ((startIndex = text.IndexOf("http://")) >= 0)
		{
			Cout.println("currentLine: " + text);
			alertURL = text.Substring(startIndex);
			center = new Command(mResources.open_link, 12000);
			if (!GameCanvas.isTouch)
			{
				ChatTextField.gI().center = new Command(mResources.open_link, null, 12000, null);
			}
		}
		else
		{
			if (text.IndexOf("@") < 0)
			{
				return;
			}
			string text2 = text.Substring(2);
			text2 = text2.Trim();
			startIndex = text2.IndexOf("@");
			string text3 = text2.Substring(startIndex);
			int num = text3.IndexOf(" ");
			num = ((num > 0) ? (num + startIndex) : (startIndex + text3.Length));
			fnick = text2.Substring(startIndex + 1, num);
			if (!fnick.Equals(string.Empty) && !fnick.Equals(Char.myCharz().cName))
			{
				center = new Command(mResources.SELECT, 12009, fnick);
				if (!GameCanvas.isTouch)
				{
					ChatTextField.gI().center = new Command(mResources.SELECT, null, 12009, fnick);
				}
			}
			else
			{
				fnick = null;
				center = null;
			}
		}
	}

	public bool isPaintPopup()
	{
		if (!isPaintItemInfo && !isPaintInfoMe && !isPaintStore && !isPaintWeapon && !isPaintNonNam && !isPaintNonNu && !isPaintAoNam && !isPaintAoNu && !isPaintGangTayNam && !isPaintGangTayNu && !isPaintQuanNam && !isPaintQuanNu && !isPaintGiayNam && !isPaintGiayNu && !isPaintLien && !isPaintNhan && !isPaintNgocBoi && !isPaintPhu && !isPaintStack && !isPaintStackLock && !isPaintGrocery && !isPaintGroceryLock && !isPaintUpGrade && !isPaintConvert && !isPaintSplit && !isPaintUpPearl && !isPaintBox && !isPaintTrade && !isPaintAlert && !isPaintZone && !isPaintTeam && !isPaintClan && !isPaintFindTeam && !isPaintTask && !isPaintFriend && !isPaintEnemies && !isPaintCharInMap)
		{
			return isPaintMessage;
		}
		return true;
	}

	public bool isNotPaintTouchControl()
	{
		if ((GameCanvas.isTouchControl || GameCanvas.currentScreen != gI()) && GameCanvas.isTouch && !ChatTextField.gI().isShow && !InfoDlg.isShow && GameCanvas.currentDialog == null && ChatPopup.currChatPopup == null && !GameCanvas.menu.showMenu && !GameCanvas.panel.isShow && !DataItem.IsShow)
		{
			return isPaintPopup();
		}
		return true;
	}

	public bool isPaintUI()
	{
		if (!isPaintStore && !isPaintWeapon && !isPaintNonNam && !isPaintNonNu && !isPaintAoNam && !isPaintAoNu && !isPaintGangTayNam && !isPaintGangTayNu && !isPaintQuanNam && !isPaintQuanNu && !isPaintGiayNam && !isPaintGiayNu && !isPaintLien && !isPaintNhan && !isPaintNgocBoi && !isPaintPhu && !isPaintStack && !isPaintStackLock && !isPaintGrocery && !isPaintGroceryLock && !isPaintUpGrade && !isPaintConvert && !isPaintSplit && !isPaintUpPearl && !isPaintBox)
		{
			return isPaintTrade;
		}
		return true;
	}

	public bool isOpenUI()
	{
		if (!isPaintItemInfo && !isPaintInfoMe && !isPaintStore && !isPaintNonNam && !isPaintNonNu && !isPaintAoNam && !isPaintAoNu && !isPaintGangTayNam && !isPaintGangTayNu && !isPaintQuanNam && !isPaintQuanNu && !isPaintGiayNam && !isPaintGiayNu && !isPaintLien && !isPaintNhan && !isPaintNgocBoi && !isPaintPhu && !isPaintWeapon && !isPaintStack && !isPaintStackLock && !isPaintGrocery && !isPaintGroceryLock && !isPaintUpGrade && !isPaintConvert && !isPaintUpPearl && !isPaintBox && !isPaintSplit)
		{
			return isPaintTrade;
		}
		return true;
	}

	public static void setPopupSize(int w, int h)
	{
		if (GameCanvas.w == 128 || GameCanvas.h <= 208)
		{
			w = 126;
			h = 160;
		}
		indexTitle = 0;
		popupW = w;
		popupH = h;
		popupX = gW2 - w / 2;
		popupY = gH2 - h / 2;
		if (GameCanvas.isTouch && !isPaintZone && !isPaintTeam && !isPaintClan && !isPaintCharInMap && !isPaintFindTeam && !isPaintFriend && !isPaintEnemies && !isPaintTask && !isPaintMessage)
		{
			if (GameCanvas.h <= 240)
			{
				popupY -= 10;
			}
			if (GameCanvas.isTouch && !GameCanvas.isTouchControlSmallScreen && GameCanvas.currentScreen is GameScr)
			{
				popupW = 310;
				popupX = gW / 2 - popupW / 2;
				if (isPaintInfoMe && indexMenu > 0)
				{
					popupW = w;
					popupX = gW2 - w / 2;
				}
			}
		}
		if (popupY < -10)
		{
			popupY = -10;
		}
		if (GameCanvas.h > 208 && popupY < 0)
		{
			popupY = 0;
		}
		if (GameCanvas.h == 208 && popupY < 10)
		{
			popupY = 10;
		}
	}

	public static void loadImg()
	{
		TileMap.loadTileImage();
	}

	public void paintTitle(mGraphics g, string title, bool arrow)
	{
		int num = gW / 2;
		g.setColor(Paint.COLORDARK);
		g.fillRoundRect(num - mFont.tahoma_8b.getWidth(title) / 2 - 12, popupY + 4, mFont.tahoma_8b.getWidth(title) + 22, 24, 6, 6);
		if ((indexTitle == 0 || GameCanvas.isTouch) && arrow)
		{
			SmallImage.drawSmallImage(g, 989, num - mFont.tahoma_8b.getWidth(title) / 2 - 15 - 7 - ((GameCanvas.gameTick % 8 <= 3) ? 2 : 0), popupY + 16, 2, StaticObj.VCENTER_HCENTER);
			SmallImage.drawSmallImage(g, 989, num + mFont.tahoma_8b.getWidth(title) / 2 + 15 + 5 + ((GameCanvas.gameTick % 8 <= 3) ? 2 : 0), popupY + 16, 0, StaticObj.VCENTER_HCENTER);
		}
		if (indexTitle == 0)
		{
			g.setColor(Paint.COLORFOCUS);
		}
		else
		{
			g.setColor(Paint.COLORBORDER);
		}
		g.drawRoundRect(num - mFont.tahoma_8b.getWidth(title) / 2 - 12, popupY + 4, mFont.tahoma_8b.getWidth(title) + 22, 24, 6, 6);
		mFont.tahoma_8b.drawString(g, title, num, popupY + 9, 2);
	}

	public static int getTaskMapId()
	{
		if (Char.myCharz().taskMaint == null)
		{
			return -1;
		}
		return mapTasks[Char.myCharz().taskMaint.index];
	}

	public static sbyte getTaskNpcId()
	{
		sbyte result = 0;
		if (Char.myCharz().taskMaint == null)
		{
			result = -1;
		}
		else if (Char.myCharz().taskMaint.index <= tasks.Length - 1)
		{
			result = (sbyte)tasks[Char.myCharz().taskMaint.index];
		}
		return result;
	}

	public void refreshTeam()
	{
	}

	public void onChatFromMe(string text, string to)
	{
		Res.outz("CHAT");
		if (!isPaintMessage || GameCanvas.isTouch)
		{
			ChatTextField.gI().isShow = false;
		}
		ModProCL.onChatFromMe(text, to);
		if (text.StartsWith("cheat_"))
		{
			try
			{
				float num = (Time.timeScale = float.Parse(text.Split('_')[1]));
				info1.addInfo("Cheat: " + num);
				text = string.Empty;
				return;
			}
			catch
			{
			}
		}
		if (text.StartsWith("fakename_"))
		{
			try
			{
				string cName = text.Split('_')[1];
				Char.myCharz().cName = cName;
				info1.addInfo("Con chào bố: " + Char.myCharz().cName + " ạ!");
				text = string.Empty;
				return;
			}
			catch
			{
			}
		}
		if (to.Equals(mResources.chat_player))
		{
			if (info2.playerID != Char.myCharz().charID)
			{
				Service.gI().chatPlayer(text, info2.playerID);
			}
		}
		else if (!text.Equals(string.Empty))
		{
			Service.gI().chat(text);
		}
	}

	public void onCancelChat()
	{
		if (isPaintMessage)
		{
			isPaintMessage = false;
			ChatTextField.gI().center = null;
		}
	}

	public void openWeb(string strLeft, string strRight, string url, string title, string str)
	{
		isPaintAlert = true;
		isLockKey = true;
		indexRow = 0;
		setPopupSize(175, 200);
		textsTitle = title;
		texts = mFont.tahoma_7.splitFontVector(str, popupW - 30);
		center = null;
		left = new Command(strLeft, 11068, url);
		right = new Command(strRight, 11069);
	}

	public void sendSms(string strLeft, string strRight, short port, string syntax, string title, string str)
	{
		isPaintAlert = true;
		isLockKey = true;
		indexRow = 0;
		setPopupSize(175, 200);
		textsTitle = title;
		texts = mFont.tahoma_7.splitFontVector(str, popupW - 30);
		center = null;
		MyVector myVector = new MyVector();
		myVector.addElement(string.Empty + port);
		myVector.addElement(syntax);
		left = new Command(strLeft, 11074);
		right = new Command(strRight, 11075);
	}

	public void actMenu()
	{
		GameCanvas.panel.setTypeMain();
		GameCanvas.panel.show();
	}

	public void openUIZone(Message message)
	{
		InfoDlg.hide();
		try
		{
			zones = new int[message.reader().readByte()];
			pts = new int[zones.Length];
			numPlayer = new int[zones.Length];
			maxPlayer = new int[zones.Length];
			rank1 = new int[zones.Length];
			rankName1 = new string[zones.Length];
			rank2 = new int[zones.Length];
			rankName2 = new string[zones.Length];
			for (int i = 0; i < zones.Length; i++)
			{
				zones[i] = message.reader().readByte();
				pts[i] = message.reader().readByte();
				numPlayer[i] = message.reader().readByte();
				maxPlayer[i] = message.reader().readByte();
				if (message.reader().readByte() == 1)
				{
					rankName1[i] = message.reader().readUTF();
					rank1[i] = message.reader().readInt();
					rankName2[i] = message.reader().readUTF();
					rank2[i] = message.reader().readInt();
				}
			}
		}
		catch (Exception ex)
		{
			Cout.LogError("Loi ham OPEN UIZONE " + ex.ToString());
		}
		if (!AutoBossCL.offPaintZone)
		{
			GameCanvas.panel.setTypeZone();
			GameCanvas.panel.show();
		}
	}

	public void showViewInfo()
	{
		indexMenu = 3;
		isPaintInfoMe = true;
		setPopupSize(175, 200);
	}

	private void actDead()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command(mResources.DIES[1], 110381));
		myVector.addElement(new Command(mResources.DIES[2], 110382));
		myVector.addElement(new Command(mResources.DIES[3], 110383));
		GameCanvas.menu.startAt(myVector, 3);
	}

	public void startYesNoPopUp(string info, Command cmdYes, Command cmdNo)
	{
		popUpYesNo = new PopUpYesNo();
		popUpYesNo.setPopUp(info, cmdYes, cmdNo);
	}

	public void player_vs_player(int playerId, int xu, string info, sbyte typePK)
	{
		Char obj = findCharInMap(playerId);
		if (obj != null)
		{
			if (typePK == 3)
			{
				startYesNoPopUp(info, new Command(mResources.OK, 2000, obj), new Command(mResources.CLOSE, 2009, obj));
			}
			if (typePK == 4)
			{
				startYesNoPopUp(info, new Command(mResources.OK, 2005, obj), new Command(mResources.CLOSE, 2009, obj));
			}
		}
	}

	public void giaodich(int playerID)
	{
		Char obj = findCharInMap(playerID);
		if (obj != null)
		{
			startYesNoPopUp(obj.cName + mResources.want_to_trade, new Command(mResources.YES, 11114, obj), new Command(mResources.NO, 2009, obj));
		}
	}

	public void getFlagImage(int charID, sbyte cflag)
	{
		if (vFlag.size() == 0)
		{
			Service.gI().getFlag(2, cflag);
			Res.outz("getFlag1");
			return;
		}
		if (charID == Char.myCharz().charID)
		{
			Res.outz("my cflag: isme");
			if (Char.myCharz().isGetFlagImage(cflag))
			{
				Res.outz("my cflag: true");
				for (int i = 0; i < vFlag.size(); i++)
				{
					PKFlag pKFlag = (PKFlag)vFlag.elementAt(i);
					if (pKFlag != null && pKFlag.cflag == cflag)
					{
						Res.outz("my cflag: cflag==");
						Char.myCharz().flagImage = pKFlag.IDimageFlag;
					}
				}
			}
			else if (!Char.myCharz().isGetFlagImage(cflag))
			{
				Res.outz("my cflag: false");
				Service.gI().getFlag(2, cflag);
			}
			return;
		}
		Res.outz("my cflag: not me");
		if (findCharInMap(charID) == null)
		{
			return;
		}
		if (findCharInMap(charID).isGetFlagImage(cflag))
		{
			Res.outz("my cflag: true");
			for (int j = 0; j < vFlag.size(); j++)
			{
				PKFlag pKFlag2 = (PKFlag)vFlag.elementAt(j);
				if (pKFlag2 != null && pKFlag2.cflag == cflag)
				{
					Res.outz("my cflag: cflag==");
					findCharInMap(charID).flagImage = pKFlag2.IDimageFlag;
				}
			}
		}
		else if (!findCharInMap(charID).isGetFlagImage(cflag))
		{
			Res.outz("my cflag: false");
			Service.gI().getFlag(2, cflag);
		}
	}

	public void actionPerform(int idAction, object p)
	{
		Cout.println("PERFORM WITH ID = " + idAction);
		switch (idAction)
		{
		case 1:
			GameCanvas.endDlg();
			break;
		case 2:
			GameCanvas.menu.showMenu = false;
			break;
		case 2000:
			popUpYesNo = null;
			GameCanvas.endDlg();
			if ((Char)p == null)
			{
				Service.gI().player_vs_player(1, 3, -1);
				break;
			}
			Service.gI().player_vs_player(1, 3, ((Char)p).charID);
			Service.gI().charMove();
			break;
		case 2001:
			GameCanvas.endDlg();
			break;
		case 2003:
			GameCanvas.endDlg();
			InfoDlg.showWait();
			Service.gI().player_vs_player(0, 3, Char.myCharz().charFocus.charID);
			break;
		case 2004:
			GameCanvas.endDlg();
			Service.gI().player_vs_player(0, 4, Char.myCharz().charFocus.charID);
			break;
		case 2005:
			GameCanvas.endDlg();
			popUpYesNo = null;
			if ((Char)p == null)
			{
				Service.gI().player_vs_player(1, 4, -1);
			}
			else
			{
				Service.gI().player_vs_player(1, 4, ((Char)p).charID);
			}
			break;
		case 2006:
			GameCanvas.endDlg();
			Service.gI().player_vs_player(2, 4, Char.myCharz().charFocus.charID);
			break;
		case 2007:
			GameCanvas.endDlg();
			GameMidlet.instance.exit();
			break;
		case 2009:
			popUpYesNo = null;
			break;
		case 8002:
			doFire(isFireByShortCut: false, skipWaypoint: true);
			GameCanvas.clearKeyHold();
			GameCanvas.clearKeyPressed();
			break;
		case 11000:
			actMenu();
			break;
		case 11001:
			Char.myCharz().findNextFocusByKey();
			break;
		case 11002:
			GameCanvas.panel.hide();
			break;
		case 11038:
			actDead();
			break;
		case 11059:
		{
			Skill skill = onScreenSkill[selectedIndexSkill];
			doUseSkill(skill, isShortcut: false);
			center = null;
			break;
		}
		case 11057:
		{
			Effect2.vEffect2Outside.removeAllElements();
			Effect2.vEffect2.removeAllElements();
			Npc npc = (Npc)p;
			if (npc.idItem == 0)
			{
				Service.gI().confirmMenu((short)npc.template.npcTemplateId, (sbyte)GameCanvas.menu.menuSelectedItem);
			}
			else if (GameCanvas.menu.menuSelectedItem == 0)
			{
				Service.gI().pickItem(npc.idItem);
			}
			break;
		}
		case 11111:
			if (Char.myCharz().charFocus != null)
			{
				InfoDlg.showWait();
				if (GameCanvas.panel.vPlayerMenu.size() <= 0)
				{
					playerMenu(Char.myCharz().charFocus);
				}
				GameCanvas.panel.setTypePlayerMenu(Char.myCharz().charFocus);
				GameCanvas.panel.show();
				Service.gI().getPlayerMenu(Char.myCharz().charFocus.charID);
				Service.gI().messagePlayerMenu(Char.myCharz().charFocus.charID);
			}
			break;
		case 11112:
		{
			Char obj3 = (Char)p;
			Service.gI().friend(1, obj3.charID);
			break;
		}
		case 11113:
		{
			Char obj2 = (Char)p;
			if (obj2 != null)
			{
				Service.gI().giaodich(0, obj2.charID, -1, -1);
			}
			break;
		}
		case 11114:
		{
			popUpYesNo = null;
			GameCanvas.endDlg();
			Char obj = (Char)p;
			if (obj != null)
			{
				Service.gI().giaodich(1, obj.charID, -1, -1);
			}
			break;
		}
		case 11115:
			if (Char.myCharz().charFocus != null)
			{
				InfoDlg.showWait();
				Service.gI().playerMenuAction(Char.myCharz().charFocus.charID, (short)Char.myCharz().charFocus.menuSelect);
			}
			break;
		case 11120:
		{
			object[] array2 = (object[])p;
			Skill skill4 = (Skill)array2[0];
			int num2 = int.Parse((string)array2[1]);
			for (int j = 0; j < onScreenSkill.Length; j++)
			{
				if (onScreenSkill[j] == skill4)
				{
					onScreenSkill[j] = null;
				}
			}
			onScreenSkill[num2] = skill4;
			saveonScreenSkillToRMS();
			break;
		}
		case 11121:
		{
			object[] array = (object[])p;
			Skill skill3 = (Skill)array[0];
			int num = int.Parse((string)array[1]);
			for (int i = 0; i < keySkill.Length; i++)
			{
				if (keySkill[i] == skill3)
				{
					keySkill[i] = null;
				}
			}
			keySkill[num] = skill3;
			saveKeySkillToRMS();
			break;
		}
		case 11067:
			if (TileMap.zoneID != indexSelect)
			{
				Service.gI().requestChangeZone(indexSelect, indexItemUse);
				InfoDlg.showWait();
			}
			else
			{
				info1.addInfo(mResources.ZONE_HERE);
			}
			break;
		case 12000:
			Service.gI().getClan(1, -1, null);
			break;
		case 12001:
			GameCanvas.endDlg();
			break;
		case 12002:
		{
			GameCanvas.endDlg();
			ClanObject clanObject2 = (ClanObject)p;
			Service.gI().clanInvite(1, -1, clanObject2.clanID, clanObject2.code);
			popUpYesNo = null;
			break;
		}
		case 12003:
		{
			ClanObject clanObject = (ClanObject)p;
			GameCanvas.endDlg();
			Service.gI().clanInvite(2, -1, clanObject.clanID, clanObject.code);
			popUpYesNo = null;
			break;
		}
		case 12004:
		{
			Skill skill2 = (Skill)p;
			doUseSkill(skill2, isShortcut: true);
			Char.myCharz().saveLoadPreviousSkill();
			break;
		}
		case 12005:
			if (GameCanvas.serverScr == null)
			{
				GameCanvas.serverScr = new ServerScr();
			}
			GameCanvas.serverScr.switchToMe();
			GameCanvas.endDlg();
			break;
		case 12006:
			GameMidlet.instance.exit();
			break;
		case 110001:
			GameCanvas.panel.setTypeMain();
			GameCanvas.panel.show();
			break;
		case 110004:
			GameCanvas.menu.showMenu = false;
			break;
		case 110382:
			Service.gI().returnTownFromDead();
			break;
		case 110391:
			Service.gI().clanInvite(0, Char.myCharz().charFocus.charID, -1, -1);
			break;
		case 888351:
			Service.gI().petStatus(5);
			GameCanvas.endDlg();
			break;
		case 110383:
			Service.gI().wakeUpFromDead();
			break;
		}
	}

	private static void setTouchBtn()
	{
		if (isAnalog != 0)
		{
			xTG = (xF = GameCanvas.w - 45);
			if (gamePad.isLargeGamePad)
			{
				xSkill = gamePad.wZone + 20;
				wSkill = 35;
				xHP = xF - 45;
			}
			else if (gamePad.isMediumGamePad)
			{
				xHP = xF - 45;
			}
			yF = GameCanvas.h - 45;
			yTG = yF - 45;
		}
	}

	private void updateGamePad()
	{
		if (isAnalog == 0 || Char.myCharz().statusMe == 14)
		{
			return;
		}
		if (GameCanvas.isPointerHoldIn(xF, yF, 40, 40))
		{
			mScreen.keyTouch = 5;
			if (GameCanvas.isPointerJustRelease)
			{
				GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25] = true;
				GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
			}
		}
		gamePad.update();
		if (GameCanvas.isPointerHoldIn(xTG, yTG, 34, 34))
		{
			mScreen.keyTouch = 13;
			GameCanvas.isPointerJustDown = false;
			isPointerDowning = false;
			if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
			{
				Char.myCharz().findNextFocusByKey();
				GameCanvas.isPointerClick = (GameCanvas.isPointerJustDown = (GameCanvas.isPointerJustRelease = false));
			}
		}
	}

	private void paintGamePad(mGraphics g)
	{
		if (isAnalog != 0 && Char.myCharz().statusMe != 14)
		{
			g.drawImage((mScreen.keyTouch != 5 && mScreen.keyMouse != 5) ? imgFire0 : imgFire1, xF + 20, yF + 20, mGraphics.HCENTER | mGraphics.VCENTER);
			gamePad.paint(g);
			g.drawImage((mScreen.keyTouch != 13) ? imgFocus : imgFocus2, xTG + 20, yTG + 20, mGraphics.HCENTER | mGraphics.VCENTER);
		}
	}

	public void showWinNumber(string num, string finish)
	{
		winnumber = new int[num.Length];
		randomNumber = new int[num.Length];
		tMove = new int[num.Length];
		moveCount = new int[num.Length];
		delayMove = new int[num.Length];
		try
		{
			for (int i = 0; i < num.Length; i++)
			{
				winnumber[i] = short.Parse(num[i].ToString());
				randomNumber[i] = Res.random(0, 11);
				tMove[i] = 1;
				delayMove[i] = 0;
			}
		}
		catch (Exception)
		{
		}
		tShow = 100;
		moveIndex = 0;
		strFinish = finish;
		lastXS = (currXS = mSystem.currentTimeMillis());
	}

	public void chatVip(string chatVip)
	{
		if (!startChat)
		{
			currChatWidth = mFont.tahoma_7b_yellowSmall.getWidth(chatVip);
			xChatVip = GameCanvas.w;
			startChat = true;
		}
		if (chatVip.StartsWith("!"))
		{
			chatVip = chatVip.Substring(1);
			isFireWorks = true;
		}
		if (chatVip.StartsWith("BOSS"))
		{
			MainMod.listBosses.Add(new Boss(chatVip));
			if (MainMod.listBosses.Count > 5)
			{
				MainMod.listBosses.RemoveAt(0);
			}
		}
		vChatVip.addElement(chatVip);
		MainMod.updateInfo101(chatVip);
	}

	public void clearChatVip()
	{
		vChatVip.removeAllElements();
		xChatVip = GameCanvas.w;
		startChat = false;
	}

	public void paintChatVip(mGraphics g)
	{
		if (vChatVip.size() != 0 && isPaintChatVip && !MainMenu.isShowMenuVIP && !DataItem.IsShow)
		{
			g.setClip(0, GameCanvas.h - 13, GameCanvas.w, 15);
			g.fillRect(0, GameCanvas.h - 13, GameCanvas.w, 15, 0, 90);
			string st = (string)vChatVip.elementAt(0);
			mFont.tahoma_7b_yellow.drawString(g, st, xChatVip, GameCanvas.h - 13, 0, mFont.tahoma_7b_dark);
		}
	}

	public void updateChatVip()
	{
		if (!startChat)
		{
			return;
		}
		xChatVip -= 2;
		if (xChatVip < -currChatWidth)
		{
			xChatVip = GameCanvas.w;
			vChatVip.removeElementAt(0);
			if (vChatVip.size() == 0)
			{
				isFireWorks = false;
				startChat = false;
			}
			else
			{
				currChatWidth = mFont.tahoma_7b_white.getWidth((string)vChatVip.elementAt(0));
			}
		}
	}

	public void showYourNumber(string strNum)
	{
		yourNumber = strNum;
		strPaint = mFont.tahoma_7.splitFontArray(yourNumber, 500);
	}

	public static void checkRemoveImage()
	{
		ImgByName.checkDelHash(ImgByName.hashImagePath, 10, isTrue: false);
	}

	public static void StartServerPopUp(string strMsg)
	{
		GameCanvas.endDlg();
		int avatar = 1139;
		ChatPopup.addBigMessage(strMsg, 100000, new Npc(-1, 0, 0, 0, 0, 0)
		{
			avatar = avatar
		});
		ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
		ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 35;
		ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
	}

	public static bool ispaintPhubangBar()
	{
		if (TileMap.mapPhuBang())
		{
			return phuban_Info.type_PB == 0;
		}
		return false;
	}

	public void paintPhuBanBar(mGraphics g, int x, int y, int w)
	{
		if (phuban_Info == null || isPaintOther || isPaintRada != 1 || GameCanvas.panel.isShow || !ispaintPhubangBar() || DataItem.IsShow)
		{
			return;
		}
		if (w < fra_PVE_Bar_1.frameWidth + fra_PVE_Bar_0.frameWidth * 4)
		{
			w = fra_PVE_Bar_1.frameWidth + fra_PVE_Bar_0.frameWidth * 4;
		}
		if (x > GameCanvas.w - w / 2)
		{
			x = GameCanvas.w - w / 2;
		}
		if (x < mGraphics.getImageWidth(imgKhung) + w / 2 + 10)
		{
			x = mGraphics.getImageWidth(imgKhung) + w / 2 + 10;
		}
		int frameHeight = fra_PVE_Bar_0.frameHeight;
		int num = y + frameHeight + mGraphics.getImageHeight(imgBall) / 2 + 2;
		int frameWidth = fra_PVE_Bar_1.frameWidth;
		int num2 = w / 2 - frameWidth / 2;
		int num3 = x - w / 2;
		int num4 = x + frameWidth / 2;
		int y2 = y + 3;
		int num5 = num2 - fra_PVE_Bar_0.frameWidth;
		int num6 = num5 / fra_PVE_Bar_0.frameWidth;
		if (num5 % fra_PVE_Bar_0.frameWidth > 0)
		{
			num6++;
		}
		for (int i = 0; i < num6; i++)
		{
			if (i < num6 - 1)
			{
				fra_PVE_Bar_0.drawFrame(1, num3 + fra_PVE_Bar_0.frameWidth + i * fra_PVE_Bar_0.frameWidth, y2, 0, 0, g);
			}
			else
			{
				fra_PVE_Bar_0.drawFrame(1, num3 + num5, y2, 0, 0, g);
			}
			if (i < num6 - 1)
			{
				fra_PVE_Bar_0.drawFrame(1, num4 + i * fra_PVE_Bar_0.frameWidth, y2, 0, 0, g);
			}
			else
			{
				fra_PVE_Bar_0.drawFrame(1, num4 + num5 - fra_PVE_Bar_0.frameWidth, y2, 0, 0, g);
			}
		}
		fra_PVE_Bar_0.drawFrame(0, num3, y2, 2, 0, g);
		fra_PVE_Bar_0.drawFrame(0, num4 + num5, y2, 0, 0, g);
		if (phuban_Info.pointTeam1 > 0)
		{
			int idx = 2;
			int idx2 = 3;
			if (phuban_Info.color_1 == 4)
			{
				idx = 4;
				idx2 = 5;
			}
			int num7 = phuban_Info.pointTeam1 * num2 / phuban_Info.maxPoint;
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num7 > num2)
			{
				num7 = num2;
			}
			g.setClip(num3 + num2 - num7, y2, num7, frameHeight);
			for (int j = 0; j < num6; j++)
			{
				if (j < num6 - 1)
				{
					fra_PVE_Bar_0.drawFrame(idx2, num3 + fra_PVE_Bar_0.frameWidth + j * fra_PVE_Bar_0.frameWidth, y2, 0, 0, g);
				}
				else
				{
					fra_PVE_Bar_0.drawFrame(idx2, num3 + num5, y2, 0, 0, g);
				}
			}
			fra_PVE_Bar_0.drawFrame(idx, num3, y2, 2, 0, g);
			GameCanvas.resetTrans(g);
		}
		if (phuban_Info.pointTeam2 > 0)
		{
			int idx3 = 2;
			int idx4 = 3;
			if (phuban_Info.color_2 == 4)
			{
				idx3 = 4;
				idx4 = 5;
			}
			int num8 = phuban_Info.pointTeam2 * num2 / phuban_Info.maxPoint;
			if (num8 < 0)
			{
				num8 = 0;
			}
			if (num8 > num2)
			{
				num8 = num2;
			}
			g.setClip(num4, y2, num8, frameHeight);
			for (int k = 0; k < num6; k++)
			{
				if (k < num6 - 1)
				{
					fra_PVE_Bar_0.drawFrame(idx4, num4 + k * fra_PVE_Bar_0.frameWidth, y2, 0, 0, g);
				}
				else
				{
					fra_PVE_Bar_0.drawFrame(idx4, num4 + num5 - fra_PVE_Bar_0.frameWidth, y2, 0, 0, g);
				}
			}
			fra_PVE_Bar_0.drawFrame(idx3, num4 + num5, y2, 0, 0, g);
			GameCanvas.resetTrans(g);
		}
		fra_PVE_Bar_1.drawFrame(0, x - frameWidth / 2, y, 0, 0, g);
		string timeCountDown = mSystem.getTimeCountDown(phuban_Info.timeStart, phuban_Info.timeSecond, isOnlySecond: true, isShortText: false);
		mFont.tahoma_7b_yellow.drawString(g, timeCountDown, x + 1, y + fra_PVE_Bar_1.frameHeight / 2 - mFont.tahoma_7b_green2.getHeight() / 2, 2);
		Panel.setTextColor(phuban_Info.color_1, 1).drawString(g, phuban_Info.nameTeam1, x - 5, num + 5, 1);
		Panel.setTextColor(phuban_Info.color_2, 1).drawString(g, phuban_Info.nameTeam2, x + 5, num + 5, 0);
		if (phuban_Info.type_PB != 0)
		{
			int y3 = y + frameHeight / 2 - 2;
			mFont.bigNumber_While.drawString(g, string.Empty + phuban_Info.pointTeam1, num3 + num2 / 2, y3, 2);
			mFont.bigNumber_While.drawString(g, string.Empty + phuban_Info.pointTeam2, num4 + num2 / 2, y3, 2);
		}
		g.drawImage(imgVS, x, y + fra_PVE_Bar_1.frameHeight + 2, 3);
		if (phuban_Info.type_PB == 0)
		{
			paintChienTruong_Life(g, phuban_Info.maxLife, phuban_Info.color_1, phuban_Info.lifeTeam1, x - 13, phuban_Info.color_2, phuban_Info.lifeTeam2, x + 13, num);
		}
	}

	public static void paintChienTruong_Life(mGraphics g, int maxLife, int cl1, int lifeTeam1, int x1, int cl2, int lifeTeam2, int x2, int y)
	{
		if (imgBall == null)
		{
			return;
		}
		int num = mGraphics.getImageHeight(imgBall) / 2;
		for (int i = 0; i < maxLife; i++)
		{
			int num2 = 0;
			if (i < lifeTeam1)
			{
				num2 = 1;
			}
			g.drawRegion(imgBall, 0, num2 * num, mGraphics.getImageWidth(imgBall), num, 0, x1 - i * (num + 1), y, mGraphics.VCENTER | mGraphics.HCENTER);
		}
		for (int j = 0; j < maxLife; j++)
		{
			int num3 = 0;
			if (j < lifeTeam2)
			{
				num3 = 1;
			}
			g.drawRegion(imgBall, 0, num3 * num, mGraphics.getImageWidth(imgBall), num, 0, x2 + j * (num + 1), y, mGraphics.VCENTER | mGraphics.HCENTER);
		}
	}

	public static void paintHPBar_NEW(mGraphics g, int x, int y, Char c)
	{
		g.drawImage(imgKhung, x, y, 0);
		int x2 = x + 3;
		int num = y + 19;
		int width = imgHP_NEW.getWidth();
		int num2 = imgHP_NEW.getHeight() / 2;
		int num3 = (int)(c.cHP * width / c.cHPFull);
		if (num3 <= 0)
		{
			num3 = 1;
		}
		else if (num3 > width)
		{
			num3 = width;
		}
		g.drawRegion(imgHP_NEW, 0, num2, num3, num2, 0, x2, num, 0);
		int num4 = (int)(c.cMP * width / c.cMPFull);
		if (num4 <= 0)
		{
			num4 = 1;
		}
		else if (num4 > width)
		{
			num4 = width;
		}
		g.drawRegion(imgHP_NEW, 0, 0, num4, num2, 0, x2, num + 6, 0);
		int x3 = x + imgKhung.getWidth() / 2 + 1;
		int y2 = num + 13;
		mFont.tahoma_7_green2.drawString(g, c.cName, x3, y + 4, 2);
		if (c.mobFocus != null)
		{
			if (c.mobFocus.getTemplate() != null)
			{
				mFont.tahoma_7_green2.drawString(g, c.mobFocus.getTemplate().name, x3, y2, 2);
			}
		}
		else if (c.npcFocus != null)
		{
			mFont.tahoma_7_green2.drawString(g, c.npcFocus.template.name, x3, y2, 2);
		}
		else if (c.charFocus != null)
		{
			mFont.tahoma_7_green2.drawString(g, c.charFocus.cName, x3, y2, 2);
		}
	}

	public static void addEffectEnd(int type, int subtype, int typePaint, int x, int y, int levelPaint, int dir, short timeRemove, Point[] listObj)
	{
		addEffect2Vector(new Effect_End(type, subtype, typePaint, x, y, levelPaint, dir, timeRemove, listObj));
	}

	public static void addEffectEnd_Target(int type, int subtype, int typePaint, Char charUse, Point target, int levelPaint, short timeRemove, short range)
	{
		addEffect2Vector(new Effect_End(type, subtype, typePaint, charUse.clone(), target, levelPaint, timeRemove, range));
	}

	public static void addEffect2Vector(Effect_End eff)
	{
		if (eff.levelPaint == 0)
		{
			EffectManager.addHiEffect(eff);
		}
		else if (eff.levelPaint == 1)
		{
			EffectManager.addMidEffects(eff);
		}
		else if (eff.levelPaint == 2)
		{
			EffectManager.addMid_2Effects(eff);
		}
		else
		{
			EffectManager.addLowEffect(eff);
		}
	}

	public static bool setIsInScreen(int x, int y, int wOne, int hOne)
	{
		if (x >= cmx - wOne && x <= cmx + GameCanvas.w + wOne && y >= cmy - hOne)
		{
			return y <= cmy + GameCanvas.h + hOne * 3 / 2;
		}
		return false;
	}

	public static bool isSmallScr()
	{
		return GameCanvas.w <= 320;
	}

	private void paint_ios_bg(mGraphics g)
	{
		if (mSystem.clientType == 5)
		{
			if (imgBgIOS != null)
			{
				g.setColor(16777215);
				g.fillRect(0, 0, GameCanvas.w, GameCanvas.h);
				g.drawImage(imgBgIOS, GameCanvas.w / 2, GameCanvas.h / 2, mGraphics.VCENTER | mGraphics.HCENTER);
			}
			else
			{
				imgBgIOS = GameCanvas.loadImage("/bg/bg_ios_" + ((TileMap.bgID % 2 != 0) ? 1 : 2) + ".png");
			}
		}
	}

	public void paint_CT(mGraphics g, int x, int y, int w)
	{
		w = 194;
		w = 182;
		w = 170;
		int num = 66;
		int num2 = 11;
		if (x > GameCanvas.w - w / 2)
		{
			x = GameCanvas.w - w / 2;
		}
		if (x < mGraphics.getImageWidth(imgKhung) + w / 2 + 10)
		{
			x = mGraphics.getImageWidth(imgKhung) + w / 2 + 10;
		}
		_ = fra_PVE_Bar_0.frameHeight;
		_ = mGraphics.getImageHeight(imgBall) / 2;
		int frameWidth = fra_PVE_Bar_1.frameWidth;
		int num3 = w / 2 - frameWidth / 2;
		int num4 = x - w / 2 + 3;
		int num5 = x + frameWidth / 2;
		int num6 = y + 3;
		int num7 = num3 - fra_PVE_Bar_0.frameWidth;
		int num8 = num7 / fra_PVE_Bar_0.frameWidth;
		if (num7 % fra_PVE_Bar_0.frameWidth > 0)
		{
			num8++;
		}
		for (int i = 0; i < num8; i++)
		{
			if (i < num8 - 1)
			{
				g.drawRegion(img_ct_bar_0, 0, 15, mGraphics.getImageWidth(img_ct_bar_0), 15, 2, num4 + fra_PVE_Bar_0.frameWidth + i * fra_PVE_Bar_0.frameWidth, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
			}
			else
			{
				g.drawRegion(img_ct_bar_0, 0, 15, mGraphics.getImageWidth(img_ct_bar_0), 15, 2, num4 + num7, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
			}
			if (i < num8 - 1)
			{
				g.drawRegion(img_ct_bar_0, 0, 15, mGraphics.getImageWidth(img_ct_bar_0), 15, 2, num5 + i * fra_PVE_Bar_0.frameWidth, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
			}
			else
			{
				g.drawRegion(img_ct_bar_0, 0, 15, mGraphics.getImageWidth(img_ct_bar_0), 15, 2, num5 + num7 - fra_PVE_Bar_0.frameWidth, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
			}
		}
		fra_PVE_Bar_0.drawFrame(0, num4, num6, 2, 0, g);
		fra_PVE_Bar_0.drawFrame(0, num5 + num7, num6, 0, 0, g);
		int num9 = nCT_TeamA * 100 / (nCT_nBoyBaller / 2) * num / 100;
		if (num9 > 0)
		{
			if (num9 < 6)
			{
				num9 = 6;
			}
			g.setClip(num4, num6, num9, 15);
		}
		if (nCT_TeamA > 0)
		{
			for (int j = 0; j < num2; j++)
			{
				if (j == 0)
				{
					g.drawRegion(img_ct_bar_0, 0, 60, mGraphics.getImageWidth(img_ct_bar_0), 15, 2, num4, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
				}
				else
				{
					g.drawRegion(img_ct_bar_0, 0, 75, mGraphics.getImageWidth(img_ct_bar_0), 15, 2, num4 + j * 6, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
				}
			}
		}
		GameCanvas.resetTrans(g);
		int num10 = nCT_TeamB * 100 / (nCT_nBoyBaller / 2) * num / 100;
		if (num - (num - num10) > 0)
		{
			if (num10 < 6)
			{
				num10 = 6;
			}
			g.setClip(num5 + num - num10, num6, num - (num - num10), 15);
		}
		if (nCT_TeamB > 0)
		{
			for (int k = 0; k < num2; k++)
			{
				if (k == 0)
				{
					g.drawRegion(img_ct_bar_0, 0, 30, mGraphics.getImageWidth(img_ct_bar_0), 15, 0, num5 + num7, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
				}
				else
				{
					g.drawRegion(img_ct_bar_0, 0, 45, mGraphics.getImageWidth(img_ct_bar_0), 15, 0, num5 + num7 - k * 6, num6, mGraphics.TOP | mGraphics.LEFT, isClip: true);
				}
			}
		}
		GameCanvas.resetTrans(g);
		fra_PVE_Bar_1.drawFrame(0, x - frameWidth / 2 + 1, y, 0, 0, g);
		string st = NinjaUtil.getTime((int)((nCT_timeBallte - mSystem.currentTimeMillis()) / 1000)) + string.Empty;
		mFont.tahoma_7b_yellow.drawString(g, st, num4 + w / 2 - 2, y + 5, 2);
		mFont.tahoma_7_grey.drawString(g, "Tầng " + nCT_floor, num4 + w / 2 - 3, y + fra_PVE_Bar_1.frameHeight, mFont.CENTER);
		int width = mFont.tahoma_7b_red.getWidth(nCT_TeamA + string.Empty);
		mFont.tahoma_7b_blue.drawString(g, nCT_TeamA + string.Empty, x - frameWidth / 2 - width, num6 + fra_PVE_Bar_1.frameHeight, 0);
		SmallImage.drawSmallImage(g, 2325, x - frameWidth / 2 - width - 15, num6 + fra_PVE_Bar_1.frameHeight, 2, mGraphics.TOP | mGraphics.LEFT);
		width = mFont.tahoma_7b_red.getWidth(nCT_TeamB + string.Empty);
		mFont.tahoma_7b_red.drawString(g, nCT_TeamB + string.Empty, x + frameWidth / 2, num6 + fra_PVE_Bar_1.frameHeight, 0);
		SmallImage.drawSmallImage(g, 2323, x + frameWidth / 2 + width + 3, num6 + fra_PVE_Bar_1.frameHeight, 0, mGraphics.TOP | mGraphics.LEFT);
		paint_board_CT(g, GameCanvas.w - mFont.tahoma_7b_dark.getWidth("#01 AAAAAAAAAA"), 40);
		GameCanvas.resetTrans(g);
	}

	private void paint_board_CT(mGraphics g, int x, int y)
	{
		if (!is_Paint_boardCT_Expand)
		{
			string s = "#01 nnnnnnnnnnnn";
			int width = mFont.tahoma_7.getWidth(s);
			int num = GameCanvas.w - width - 20;
			for (int i = 0; i < nTop; i++)
			{
				mFont mFont2 = mFont.tahoma_7_white;
				switch (i)
				{
				case 0:
					mFont2 = mFont.tahoma_7_red;
					break;
				case 1:
					mFont2 = mFont.tahoma_7_yellow;
					break;
				case 2:
					mFont2 = mFont.tahoma_7_blue;
					break;
				}
				if (i == nTop - 1)
				{
					mFont2 = mFont.tahoma_7_green;
				}
				string[] array = Res.split((string)res_CT.elementAt(i), "|", 0);
				int[] array2 = new int[2] { 0, 18 };
				for (int j = 0; j < 2; j++)
				{
					mFont2.drawString(g, array[j], num + array2[j], y + i * mFont.tahoma_7.getHeight(), 0, mFont.tahoma_7);
				}
			}
			GameCanvas.resetTrans(g);
			xRect = num;
			yRect = y;
			wRect = width + 10;
			hRect = mFont.tahoma_7b_dark.getHeight() * 6;
		}
		else
		{
			string s2 = "#01 namec1000000 0001   00000";
			int[] array3 = new int[4] { 0, 18, 80, 101 };
			int width2 = mFont.tahoma_7.getWidth(s2);
			int num2 = GameCanvas.w - width2 - 20;
			for (int k = 0; k < nTop; k++)
			{
				string[] array4 = Res.split((string)res_CT.elementAt(k), "|", 0);
				mFont mFont3 = mFont.tahoma_7_white;
				switch (k)
				{
				case 0:
					mFont3 = mFont.tahoma_7_red;
					break;
				case 1:
					mFont3 = mFont.tahoma_7_yellow;
					break;
				case 2:
					mFont3 = mFont.tahoma_7_blue;
					break;
				}
				if (k == nTop - 1)
				{
					mFont3 = mFont.tahoma_7_green;
				}
				int y2 = k * mFont.tahoma_7_white.getHeight() + y;
				for (int l = 0; l < array3.Length; l++)
				{
					mFont3.drawString(g, array4[l], num2 + array3[l], y2, 0, mFont.tahoma_7);
				}
			}
			xRect = num2;
			yRect = y;
			wRect = width2 + 10;
			hRect = mFont.tahoma_7b_dark.getHeight() * 6;
		}
		GameCanvas.resetTrans(g);
	}

	static GameScr()
	{
		isPaintOther = false;
		textTime = new MyVector(string.Empty);
		isLoadAllData = false;
		vClan = new MyVector();
		vPtMap = new MyVector();
		vFriend = new MyVector();
		vEnemies = new MyVector();
		vCharInMap = new MyVector();
		vItemMap = new MyVector();
		vMobAttack = new MyVector();
		vSet = new MyVector();
		vMob = new MyVector();
		vNpc = new MyVector();
		vFlag = new MyVector();
		indexSize = 28;
		indexTitle = 0;
		indexSelect = 0;
		indexRow = -1;
		indexMenu = 0;
		scrInfo = new Scroll();
		scrMain = new Scroll();
		vItemUpGrade = new MyVector();
		isViewClanMemOnline = false;
		isViewClanInvite = true;
		titleInputText = string.Empty;
		isPaintAlert = false;
		isPaintTask = false;
		isPaintTeam = false;
		isPaintFindTeam = false;
		isPaintFriend = false;
		isPaintEnemies = false;
		isPaintItemInfo = false;
		isHaveSelectSkill = false;
		isPaintSkill = false;
		isPaintInfoMe = false;
		isPaintStore = false;
		isPaintNonNam = false;
		isPaintNonNu = false;
		isPaintAoNam = false;
		isPaintAoNu = false;
		isPaintGangTayNam = false;
		isPaintGangTayNu = false;
		isPaintQuanNam = false;
		isPaintQuanNu = false;
		isPaintGiayNam = false;
		isPaintGiayNu = false;
		isPaintLien = false;
		isPaintNhan = false;
		isPaintNgocBoi = false;
		isPaintPhu = false;
		isPaintWeapon = false;
		isPaintStack = false;
		isPaintStackLock = false;
		isPaintGrocery = false;
		isPaintGroceryLock = false;
		isPaintUpGrade = false;
		isPaintConvert = false;
		isPaintUpGradeGold = false;
		isPaintUpPearl = false;
		isPaintBox = false;
		isPaintSplit = false;
		isPaintCharInMap = false;
		isPaintTrade = false;
		isPaintZone = false;
		isPaintMessage = false;
		isPaintClan = false;
		isRequestMember = false;
		typeViewInfo = 0;
		typeActive = 0;
		info1 = new InfoMe();
		info2 = new InfoMe();
		gamePad = new GamePad();
		isAnalog = 0;
		img_ct_bar_0 = mSystem.loadImage("/mainImage/i_pve_bar_0.png");
		img_ct_bar_1 = mSystem.loadImage("/mainImage/i_pve_bar_1.png");
		cmdTestLogin = null;
		keySkill = new Skill[10];
		onScreenSkill = new Skill[10];
		tam = 0;
		isPaint = true;
		shock_x = new int[4] { 1, -1, 1, -1 };
		shock_y = new int[4] { 1, -1, -1, 1 };
		popupW = 140;
		popupH = 160;
		columns = 6;
		indexEff = 0;
		notPaint = false;
		isPing = false;
		INFO = 0;
		STORE = 1;
		ZONE = 2;
		UPGRADE = 3;
		vChatVip = new MyVector();
		nCT_TeamB = 50;
		nCT_TeamA = 50;
		nCT_nBoyBaller = 100;
		res_CT = new MyVector();
		nTop = 1;
		isPickNgocRong = false;
		thongBao = true;
	}

	public static void RemoveCharInMap(int charId)
	{
		for (int i = 0; i < vCharInMap.size(); i++)
		{
			if (((Char)vCharInMap.elementAt(i)).charID == charId)
			{
				vCharInMap.removeElementAt(i);
				i--;
			}
		}
	}

	internal static void addEffectEnd(short type, int v1, int hw, int hh, int v2, int v3, int v4, object value)
	{
		throw new NotImplementedException();
	}

}
