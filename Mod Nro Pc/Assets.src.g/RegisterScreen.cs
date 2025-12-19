using System;

namespace Assets.src.g;

public class RegisterScreen : mScreen, IActionListener
{
	public TField tfUser;

	public TField tfNgay;

	public TField tfThang;

	public TField tfNam;

	public TField tfDiachi;

	public TField tfCMND;

	public TField tfNgayCap;

	public TField tfNoiCap;

	public TField tfSodt;

	public static bool isContinueToLogin = false;

	private int focus;

	private int wC;

	private int yL;

	private int defYL;

	public bool isCheck;

	public bool isRes;

	private Command cmdLogin;

	private Command cmdCheck;

	private Command cmdFogetPass;

	private Command cmdRes;

	private Command cmdMenu;

	private Command cmdBackFromRegister;

	public string listFAQ = string.Empty;

	public string titleFAQ;

	public string subtitleFAQ;

	private string numSupport = string.Empty;

	private string strUser;

	private string strPass;

	public static bool isLocal = false;

	public static bool isUpdateAll;

	public static bool isUpdateData;

	public static bool isUpdateMap;

	public static bool isUpdateSkill;

	public static bool isUpdateItem;

	public static string serverName;

	public static Image imgTitle;

	public int plX;

	public int plY;

	public int lY;

	public int lX;

	public int logoDes;

	public int lineX;

	public int lineY;

	public static int[] bgId = new int[5] { 0, 8, 2, 6, 9 };

	public static bool isTryGetIPFromWap;

	public static short timeLogin;

	public static long lastTimeLogin;

	public static long currTimeLogin;

	private int yt;

	private Command cmdSelect;

	private Command cmdOK;

	private int xLog;

	private int yLog;

	private int xP;

	private int yP;

	private int wP;

	private int hP;

	private string passRe = string.Empty;

	public bool isFAQ;

	private int tipid = -1;

	public bool isLogin2;

	private int v = 2;

	private int g;

	private int ylogo = -40;

	private int dir = 1;

	public static bool isLoggingIn;

	public RegisterScreen(sbyte haveName)
	{
		yLog = 130;
		TileMap.bgID = (sbyte)(mSystem.currentTimeMillis() % 9);
		if (TileMap.bgID == 5 || TileMap.bgID == 6)
		{
			TileMap.bgID = 4;
		}
		GameScr.loadCamera(fullmScreen: true, -1, -1);
		GameScr.cmx = 100;
		GameScr.cmy = 200;
		if (GameCanvas.h > 200)
		{
			defYL = GameCanvas.hh - 80;
		}
		else
		{
			defYL = GameCanvas.hh - 65;
		}
		resetLogo();
		wC = ((GameCanvas.w < 200) ? 140 : 160);
		yt = GameCanvas.hh - mScreen.ITEM_HEIGHT - 5;
		if (GameCanvas.h <= 160)
		{
			yt = 20;
		}
		tfSodt = new TField();
		tfSodt.setIputType(TField.INPUT_TYPE_NUMERIC);
		tfSodt.width = 220;
		tfSodt.height = mScreen.ITEM_HEIGHT + 2;
		tfSodt.name = "Số điện thoại/ địa chỉ email";
		if (haveName == 1)
		{
			tfSodt.setText("01234567890");
		}
		tfUser = new TField();
		tfUser.width = 220;
		tfUser.height = mScreen.ITEM_HEIGHT + 2;
		tfUser.isFocus = true;
		tfUser.name = "Họ và tên";
		if (haveName == 1)
		{
			tfUser.setText("Nguyễn Văn A");
		}
		tfUser.setIputType(TField.INPUT_TYPE_ANY);
		tfNgay = new TField();
		tfNgay.setIputType(TField.INPUT_TYPE_NUMERIC);
		tfNgay.width = 70;
		tfNgay.height = mScreen.ITEM_HEIGHT + 2;
		tfNgay.name = "Ngày sinh";
		if (haveName == 1)
		{
			tfNgay.setText("01");
		}
		tfThang = new TField();
		tfThang.setIputType(TField.INPUT_TYPE_NUMERIC);
		tfThang.width = 70;
		tfThang.height = mScreen.ITEM_HEIGHT + 2;
		tfThang.name = "Tháng sinh";
		if (haveName == 1)
		{
			tfThang.setText("01");
		}
		tfNam = new TField();
		tfNam.setIputType(TField.INPUT_TYPE_NUMERIC);
		tfNam.width = 70;
		tfNam.height = mScreen.ITEM_HEIGHT + 2;
		tfNam.name = "Năm sinh";
		if (haveName == 1)
		{
			tfNam.setText("1990");
		}
		tfDiachi = new TField();
		tfDiachi.setIputType(TField.INPUT_TYPE_ANY);
		tfDiachi.width = 220;
		tfDiachi.height = mScreen.ITEM_HEIGHT + 2;
		tfDiachi.name = "Địa chỉ đăng ký thường trú";
		if (haveName == 1)
		{
			tfDiachi.setText("123 đường số 1, Quận 1, TP.HCM");
		}
		tfCMND = new TField();
		tfCMND.setIputType(TField.INPUT_TYPE_NUMERIC);
		tfCMND.width = 220;
		tfCMND.height = mScreen.ITEM_HEIGHT + 2;
		tfCMND.name = "Số Chứng minh nhân dân hoặc số hộ chiếu";
		if (haveName == 1)
		{
			tfCMND.setText("123456789");
		}
		tfNgayCap = new TField();
		tfNgayCap.setIputType(TField.INPUT_TYPE_ANY);
		tfNgayCap.width = 220;
		tfNgayCap.height = mScreen.ITEM_HEIGHT + 2;
		tfNgayCap.name = "Ngày cấp";
		if (haveName == 1)
		{
			tfNgayCap.setText("01/01/2005");
		}
		tfNoiCap = new TField();
		tfNoiCap.setIputType(TField.INPUT_TYPE_ANY);
		tfNoiCap.width = 220;
		tfNoiCap.height = mScreen.ITEM_HEIGHT + 2;
		tfNoiCap.name = "Nơi cấp";
		if (haveName == 1)
		{
			tfNoiCap.setText("TP.HCM");
		}
		yt += 35;
		isCheck = true;
		focus = 0;
		cmdLogin = new Command((GameCanvas.w <= 200) ? mResources.login2 : mResources.login, GameCanvas.instance, 888393, null);
		cmdCheck = new Command(mResources.remember, this, 2001, null);
		cmdRes = new Command(mResources.register, this, 2002, null);
		cmdBackFromRegister = new Command(mResources.CANCEL, this, 10021, null);
		left = (cmdMenu = new Command(mResources.MENU, this, 2003, null));
		if (GameCanvas.isTouch)
		{
			cmdLogin.x = GameCanvas.w / 2 - 100;
			cmdMenu.x = GameCanvas.w / 2 - mScreen.cmdW - 8;
			if (GameCanvas.h >= 200)
			{
				cmdLogin.y = GameCanvas.h / 2 - 40;
				cmdMenu.y = yLog + 110;
			}
			cmdBackFromRegister.x = GameCanvas.w / 2 + 3;
			cmdBackFromRegister.y = yLog + 110;
			cmdRes.x = GameCanvas.w / 2 - 84;
			cmdRes.y = cmdMenu.y;
		}
		wP = 170;
		hP = ((!isRes) ? 100 : 110);
		xP = GameCanvas.hw - wP / 2;
		yP = tfUser.y - 15;
		int num = 4;
		int num2 = num * 32 + 23 + 33;
		if (num2 >= GameCanvas.w)
		{
			num--;
			num2 = num * 32 + 23 + 33;
		}
		xLog = GameCanvas.w / 2 - num2 / 2;
		yLog = 5;
		lY = ((GameCanvas.w < 200) ? (tfUser.y - 30) : (yLog - 30));
		tfUser.x = xLog + 10;
		tfUser.y = yLog + 20;
		cmdOK = new Command(mResources.OK, this, 2008, null);
		cmdOK.x = 260;
		cmdOK.y = GameCanvas.h - 60;
		cmdFogetPass = new Command("Thoát", this, 1003, null);
		cmdFogetPass.x = 260;
		cmdFogetPass.y = GameCanvas.h - 30;
		if (GameCanvas.w < 250)
		{
			cmdOK.x = GameCanvas.w / 2 - 80;
			cmdFogetPass.x = GameCanvas.w / 2 + 10;
			cmdFogetPass.y = (cmdOK.y = GameCanvas.h - 25);
		}
		center = cmdOK;
		left = cmdFogetPass;
	}

	public new void switchToMe()
	{
		Res.outz("Res switch");
		SoundMn.gI().stopAll();
		focus = 0;
		tfUser.isFocus = true;
		tfNgay.isFocus = false;
		if (GameCanvas.isTouch)
		{
			tfUser.isFocus = false;
			focus = -1;
		}
		base.switchToMe();
	}

	protected void doMenu()
	{
		MyVector myVector = new MyVector("vMenu Login");
		myVector.addElement(new Command(mResources.registerNewAcc, this, 2004, null));
		if (!isLogin2)
		{
			myVector.addElement(new Command(mResources.selectServer, this, 1004, null));
		}
		myVector.addElement(new Command(mResources.forgetPass, this, 1003, null));
		myVector.addElement(new Command(mResources.website, this, 1005, null));
		if (Rms.loadRMSInt("lowGraphic") != 0)
		{
			myVector.addElement(new Command(mResources.increase_vga, this, 10041, null));
		}
		else
		{
			myVector.addElement(new Command(mResources.decrease_vga, this, 10042, null));
		}
		myVector.addElement(new Command(mResources.EXIT, GameCanvas.instance, 8885, null));
		GameCanvas.menu.startAt(myVector, 0);
	}

	protected void doRegister()
	{
		if (tfUser.getText().Equals(string.Empty))
		{
			GameCanvas.startOKDlg(mResources.userBlank);
			return;
		}
		char[] array = tfUser.getText().ToCharArray();
		if (tfNgay.getText().Equals(string.Empty))
		{
			GameCanvas.startOKDlg(mResources.passwordBlank);
			return;
		}
		if (tfUser.getText().Length < 5)
		{
			GameCanvas.startOKDlg(mResources.accTooShort);
			return;
		}
		int num = 0;
		string text = null;
		if (mResources.language == 2)
		{
			if (tfUser.getText().IndexOf("@") == -1 || tfUser.getText().IndexOf(".") == -1)
			{
				text = mResources.emailInvalid;
			}
			num = 0;
		}
		else
		{
			try
			{
				long num2 = long.Parse(tfUser.getText());
				if (tfUser.getText().Length < 8 || tfUser.getText().Length > 12 || (!tfUser.getText().StartsWith("0") && !tfUser.getText().StartsWith("84")))
				{
					text = mResources.phoneInvalid;
				}
				num = 1;
			}
			catch (Exception)
			{
				if (tfUser.getText().IndexOf("@") == -1 || tfUser.getText().IndexOf(".") == -1)
				{
					text = mResources.emailInvalid;
				}
				num = 0;
			}
		}
		if (text != null)
		{
			GameCanvas.startOKDlg(text);
		}
		else
		{
			GameCanvas.msgdlg.setInfo(mResources.plsCheckAcc + ((num != 1) ? (mResources.email + ": ") : (mResources.phone + ": ")) + tfUser.getText() + "\n" + mResources.password + ": " + tfNgay.getText(), new Command(mResources.ACCEPT, this, 4000, null), null, new Command(mResources.NO, GameCanvas.instance, 8882, null));
		}
		GameCanvas.currentDialog = GameCanvas.msgdlg;
	}

	protected void doRegister(string user)
	{
	}

	public void doViewFAQ()
	{
		if (!listFAQ.Equals(string.Empty) || !listFAQ.Equals(string.Empty))
		{
		}
		if (!Session_ME.connected)
		{
			isFAQ = true;
			GameCanvas.connect();
		}
		GameCanvas.startWaitDlg();
	}

	protected void doSelectServer()
	{
		MyVector myVector = new MyVector("vServer");
		if (isLocal)
		{
			myVector.addElement(new Command("Server LOCAL", this, 20004, null));
		}
		myVector.addElement(new Command("Server Bokken", this, 20001, null));
		myVector.addElement(new Command("Server Shuriken", this, 20002, null));
		myVector.addElement(new Command("Server Tessen (mới)", this, 20003, null));
		GameCanvas.menu.startAt(myVector, 0);
		if (loadIndexServer() != -1 && !GameCanvas.isTouch)
		{
			GameCanvas.menu.menuSelectedItem = loadIndexServer();
		}
	}

	protected void saveIndexServer(int index)
	{
		Rms.saveRMSInt("indServer", index);
	}

	protected int loadIndexServer()
	{
		return Rms.loadRMSInt("indServer");
	}

	public void doLogin()
	{
	}

	public void savePass()
	{
	}

	public override void update()
	{
		tfUser.update();
		tfNgay.update();
		tfThang.update();
		tfNam.update();
		tfDiachi.update();
		tfCMND.update();
		tfNoiCap.update();
		tfSodt.update();
		tfNgayCap.update();
		for (int i = 0; i < Effect2.vEffect2.size(); i++)
		{
			Effect2 effect = (Effect2)Effect2.vEffect2.elementAt(i);
			effect.update();
		}
		if (isUpdateAll && !isUpdateData && !isUpdateItem && !isUpdateMap && !isUpdateSkill)
		{
			isUpdateAll = false;
			mSystem.gcc();
			Service.gI().finishUpdate();
		}
		GameScr.cmx++;
		if (GameScr.cmx > GameCanvas.w * 3 + 100)
		{
			GameScr.cmx = 100;
		}
		if (ChatPopup.currChatPopup != null)
		{
			return;
		}
		GameCanvas.debug("LGU1", 0);
		GameCanvas.debug("LGU2", 0);
		GameCanvas.debug("LGU3", 0);
		updateLogo();
		GameCanvas.debug("LGU4", 0);
		GameCanvas.debug("LGU5", 0);
		if (g >= 0)
		{
			ylogo += dir * g;
			g += dir * v;
			if (g <= 0)
			{
				dir *= -1;
			}
			if (ylogo > 0)
			{
				dir *= -1;
				g -= 2 * v;
			}
		}
		GameCanvas.debug("LGU6", 0);
		if (tipid >= 0 && GameCanvas.gameTick % 100 == 0)
		{
			doChangeTip();
		}
		if (GameCanvas.isTouch)
		{
			if (isRes)
			{
				center = cmdRes;
				left = cmdBackFromRegister;
			}
			else
			{
				center = cmdOK;
				left = cmdFogetPass;
			}
		}
		else if (isRes)
		{
			center = cmdRes;
			left = cmdBackFromRegister;
		}
		else
		{
			center = cmdOK;
			left = cmdFogetPass;
		}
	}

	private void doChangeTip()
	{
		tipid++;
		if (tipid >= mResources.tips.Length)
		{
			tipid = 0;
		}
		if (GameCanvas.currentDialog == GameCanvas.msgdlg && GameCanvas.msgdlg.isWait)
		{
			GameCanvas.msgdlg.setInfo(mResources.tips[tipid]);
		}
	}

	public void updateLogo()
	{
		if (defYL != yL)
		{
			yL += defYL - yL >> 1;
		}
	}

	public override void keyPress(int keyCode)
	{
		if (tfUser.isFocus)
		{
			tfUser.keyPressed(keyCode);
		}
		else if (tfNgay.isFocus)
		{
			tfNgay.keyPressed(keyCode);
		}
		else if (tfThang.isFocus)
		{
			tfThang.keyPressed(keyCode);
		}
		else if (tfNam.isFocus)
		{
			tfNam.keyPressed(keyCode);
		}
		else if (tfDiachi.isFocus)
		{
			tfDiachi.keyPressed(keyCode);
		}
		else if (tfCMND.isFocus)
		{
			tfCMND.keyPressed(keyCode);
		}
		else if (tfNoiCap.isFocus)
		{
			tfNoiCap.keyPressed(keyCode);
		}
		else if (tfSodt.isFocus)
		{
			tfSodt.keyPressed(keyCode);
		}
		else if (tfNgayCap.isFocus)
		{
			tfNgayCap.keyPressed(keyCode);
		}
		base.keyPress(keyCode);
	}

	public override void unLoad()
	{
		base.unLoad();
	}

	public override void paint(mGraphics g)
	{
		GameCanvas.debug("PLG1", 1);
		GameCanvas.paintBGGameScr(g);
		GameCanvas.debug("PLG2", 2);
		int num = tfUser.y - 50;
		if (GameCanvas.h <= 220)
		{
			num += 5;
		}
		if (ChatPopup.currChatPopup != null || ChatPopup.serverChatPopUp != null)
		{
			return;
		}
		if (GameCanvas.currentDialog == null)
		{
			xLog = 5;
			int num2 = 233;
			if (GameCanvas.w < 260)
			{
				xLog = (GameCanvas.w - 240) / 2;
			}
			yLog = (GameCanvas.h - num2) / 2;
			int num3 = ((GameCanvas.w < 200) ? 160 : 180);
			PopUp.paintPopUp(g, xLog, yLog, 240, num2, -1, isButton: true);
			if (GameCanvas.h > 160 && imgTitle != null)
			{
				g.drawImage(imgTitle, GameCanvas.hw, num, 3);
			}
			GameCanvas.debug("PLG4", 1);
			int num4 = 4;
			int num5 = num4 * 32 + 23 + 33;
			if (num5 >= GameCanvas.w)
			{
				num4--;
				num5 = num4 * 32 + 23 + 33;
			}
			tfSodt.x = xLog + 10;
			tfSodt.y = yLog + 15;
			tfUser.x = tfSodt.x;
			tfUser.y = tfSodt.y + 30;
			tfNgay.x = xLog + 10;
			tfNgay.y = tfUser.y + 30;
			tfThang.x = tfNgay.x + 75;
			tfThang.y = tfNgay.y;
			tfNam.x = tfThang.x + 75;
			tfNam.y = tfThang.y;
			tfDiachi.x = tfUser.x;
			tfDiachi.y = tfNgay.y + 30;
			tfCMND.x = tfUser.x;
			tfCMND.y = tfDiachi.y + 30;
			tfNgayCap.x = tfUser.x;
			tfNgayCap.y = tfCMND.y + 30;
			tfNoiCap.x = tfUser.x;
			tfNoiCap.y = tfNgayCap.y + 30;
			tfUser.paint(g);
			tfNgay.paint(g);
			tfThang.paint(g);
			tfNam.paint(g);
			tfDiachi.paint(g);
			tfCMND.paint(g);
			tfNgayCap.paint(g);
			tfNoiCap.paint(g);
			tfSodt.paint(g);
			int num6 = 0;
			if (GameCanvas.w >= 176)
			{
				num6 = 50;
			}
			else
			{
				mFont.tahoma_7b_green2.drawString(g, mResources.acc + ":", tfUser.x - 35, tfUser.y + 7, 0);
				mFont.tahoma_7b_green2.drawString(g, mResources.pwd + ":", tfNgay.x - 35, tfNgay.y + 7, 0);
				mFont.tahoma_7b_green2.drawString(g, mResources.server + ": " + serverName, GameCanvas.w / 2, tfNgay.y + 32, 2);
				if (isRes)
				{
				}
				num6 = 0;
			}
		}
		string vERSION = GameMidlet.VERSION;
		g.setColor(GameCanvas.skyColor);
		g.fillRect(GameCanvas.w - 40, 4, 36, 11);
		mFont.tahoma_7_grey.drawString(g, vERSION, GameCanvas.w - 22, 4, mFont.CENTER);
		GameCanvas.resetTrans(g);
		base.paint(g);
	}

	private void turnOffFocus()
	{
		tfUser.isFocus = false;
		tfNgay.isFocus = false;
		tfThang.isFocus = false;
		tfNam.isFocus = false;
		tfDiachi.isFocus = false;
		tfCMND.isFocus = false;
		tfNgayCap.isFocus = false;
		tfNoiCap.isFocus = false;
		tfSodt.isFocus = false;
	}

	private void processFocus()
	{
		turnOffFocus();
		switch (focus)
		{
		case 0:
			tfUser.isFocus = true;
			break;
		case 1:
			tfNgay.isFocus = true;
			break;
		case 2:
			tfThang.isFocus = true;
			break;
		case 3:
			tfNam.isFocus = true;
			break;
		case 4:
			tfDiachi.isFocus = true;
			break;
		case 5:
			tfCMND.isFocus = true;
			break;
		case 6:
			tfNgayCap.isFocus = true;
			break;
		case 7:
			tfNoiCap.isFocus = true;
			break;
		case 8:
			tfSodt.isFocus = true;
			break;
		}
	}

	public override void updateKey()
	{
		if (isContinueToLogin)
		{
			return;
		}
		if (!GameCanvas.isTouch)
		{
			if (tfUser.isFocus)
			{
				right = tfUser.cmdClear;
			}
			else if (tfNgay.isFocus)
			{
				right = tfNgay.cmdClear;
			}
			else if (tfThang.isFocus)
			{
				right = tfThang.cmdClear;
			}
			else if (tfNam.isFocus)
			{
				right = tfNam.cmdClear;
			}
			else if (tfDiachi.isFocus)
			{
				right = tfDiachi.cmdClear;
			}
			else if (tfCMND.isFocus)
			{
				right = tfCMND.cmdClear;
			}
			else if (tfNgayCap.isFocus)
			{
				right = tfNgayCap.cmdClear;
			}
			else if (tfNoiCap.isFocus)
			{
				right = tfNoiCap.cmdClear;
			}
			else if (tfSodt.isFocus)
			{
				right = tfSodt.cmdClear;
			}
		}
		if (GameCanvas.keyPressed[21])
		{
			focus--;
			if (focus < 0)
			{
				focus = 8;
			}
			processFocus();
		}
		else if (GameCanvas.keyPressed[22])
		{
			focus++;
			if (focus > 8)
			{
				focus = 0;
			}
			processFocus();
		}
		if (GameCanvas.keyPressed[21] || GameCanvas.keyPressed[22])
		{
			GameCanvas.clearKeyPressed();
			if (!isLogin2 || isRes)
			{
				if (focus == 1)
				{
					tfUser.isFocus = false;
					tfNgay.isFocus = true;
				}
				else if (focus == 0)
				{
					tfUser.isFocus = true;
					tfNgay.isFocus = false;
				}
				else
				{
					tfUser.isFocus = false;
					tfNgay.isFocus = false;
				}
			}
		}
		if (GameCanvas.isTouch)
		{
			if (isRes)
			{
				center = cmdRes;
				left = cmdBackFromRegister;
			}
			else
			{
				center = cmdOK;
				left = cmdFogetPass;
			}
		}
		else if (isRes)
		{
			center = cmdRes;
			left = cmdBackFromRegister;
		}
		else
		{
			center = cmdOK;
			left = cmdFogetPass;
		}
		if (GameCanvas.isPointerJustRelease)
		{
			if (GameCanvas.isPointerHoldIn(tfUser.x, tfUser.y, tfUser.width, tfUser.height))
			{
				focus = 0;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfNgay.x, tfNgay.y, tfNgay.width, tfNgay.height))
			{
				focus = 1;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfThang.x, tfThang.y, tfThang.width, tfThang.height))
			{
				focus = 2;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfNam.x, tfNam.y, tfNam.width, tfNam.height))
			{
				focus = 3;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfDiachi.x, tfDiachi.y, tfDiachi.width, tfDiachi.height))
			{
				focus = 4;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfCMND.x, tfCMND.y, tfCMND.width, tfCMND.height))
			{
				focus = 5;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfNgayCap.x, tfNgayCap.y, tfNgayCap.width, tfNgayCap.height))
			{
				focus = 6;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfNoiCap.x, tfNoiCap.y, tfNoiCap.width, tfNoiCap.height))
			{
				focus = 7;
				processFocus();
			}
			else if (GameCanvas.isPointerHoldIn(tfSodt.x, tfSodt.y, tfSodt.width, tfSodt.height))
			{
				focus = 8;
				processFocus();
			}
		}
		base.updateKey();
		GameCanvas.clearKeyPressed();
	}

	public void resetLogo()
	{
		yL = -50;
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1000:
			try
			{
				GameMidlet.instance.platformRequest((string)p);
			}
			catch (Exception ex)
			{
				ex.StackTrace.ToString();
			}
			GameCanvas.endDlg();
			break;
		case 1001:
			GameCanvas.endDlg();
			isRes = false;
			break;
		case 1004:
			ServerListScreen.doUpdateServer();
			GameCanvas.serverScreen.switchToMe();
			break;
		case 10021:
			actRegisterLeft();
			break;
		case 1003:
			Session_ME.gI().close();
			GameCanvas.serverScreen.switchToMe();
			break;
		case 1005:
			try
			{
				GameMidlet.instance.platformRequest("http://ngocrongonline.com");
				break;
			}
			catch (Exception ex2)
			{
				ex2.StackTrace.ToString();
				break;
			}
		case 2001:
			if (isCheck)
			{
				isCheck = false;
			}
			else
			{
				isCheck = true;
			}
			break;
		case 2002:
			doRegister();
			break;
		case 2003:
			doMenu();
			break;
		case 2004:
			actRegister();
			break;
		case 2008:
			if (tfNgay.getText().Equals(string.Empty) || tfThang.getText().Equals(string.Empty) || tfNam.getText().Equals(string.Empty) || tfDiachi.getText().Equals(string.Empty) || tfCMND.getText().Equals(string.Empty) || tfNgayCap.getText().Equals(string.Empty) || tfNoiCap.getText().Equals(string.Empty) || tfSodt.getText().Equals(string.Empty) || tfUser.getText().Equals(string.Empty))
			{
				GameCanvas.startOKDlg("Vui lòng điền đầy đủ thông tin");
				break;
			}
			GameCanvas.startOKDlg(mResources.PLEASEWAIT);
			Service.gI().charInfo(tfNgay.getText(), tfThang.getText(), tfNam.getText(), tfDiachi.getText(), tfCMND.getText(), tfNgayCap.getText(), tfNoiCap.getText(), tfSodt.getText(), tfUser.getText());
			break;
		case 4000:
			doRegister(tfUser.getText());
			break;
		}
	}

	public void actRegisterLeft()
	{
		if (isLogin2)
		{
			doLogin();
			return;
		}
		isRes = false;
		tfNgay.isFocus = false;
		tfUser.isFocus = true;
		left = cmdMenu;
	}

	public void actRegister()
	{
		GameCanvas.endDlg();
		GameCanvas.startOKDlg(mResources.regNote);
		isRes = true;
		tfNgay.isFocus = false;
		tfUser.isFocus = true;
	}

	public void backToRegister()
	{
		if (GameCanvas.loginScr.isLogin2)
		{
			GameCanvas.startYesNoDlg(mResources.note, new Command(mResources.YES, GameCanvas.panel, 10019, null), new Command(mResources.NO, GameCanvas.panel, 10020, null));
			return;
		}
		GameCanvas.instance.doResetToLoginScr(GameCanvas.loginScr);
		Session_ME.gI().close();
	}
}
