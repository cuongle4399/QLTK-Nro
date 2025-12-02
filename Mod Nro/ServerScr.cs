using System;

public class ServerScr : mScreen, IActionListener
{
	private int mainSelect;

	private MyVector vecServer = new MyVector();

	private Command cmdCheck;

	public const int icmd = 100;

	private int wc;

	private int hc;

	private int w2c;

	private int numw;

	private int numh;

	private Command cmdGlobal;

	private Command cmdVietNam;

	private const string RMS_SELECT_AREA = "area_select";

	public bool isChooseArea;

	public bool isPaintNewUi;

	private ListNew list;

	public sbyte select_Area;

	public sbyte select_Lang;

	public sbyte select_typeSv;

	private Command cmdChooseArea;

	private bool isPaint_select_area;

	private bool isPaint_select_lang;

	private int x;

	private int y;

	private int w;

	private int h;

	private int xName;

	private int yName;

	private int xsub;

	private int ysub;

	private int wsub;

	private int hsub;

	private int xsubpaint;

	private int ysubpaint;

	private int xPop;

	private int yPop;

	private int wPop;

	private int hPop;

	private int xinfo;

	private int yinfo;

	private int winfo;

	private int hinfo;

	private int yBox;

	private int wBox;

	private int hBox;

	private int ntypeSv;

	private int wCheck;

	private int xPopUp_Area;

	private int yPopUp_Area;

	private int xPopUp_Lang;

	private int yPopUp_Lang;

	private int htext = 15;

	private string[] strLang = new string[3] { "Tiếng Việt", "English", "Indo" };

	private string[] strArea = new string[2] { "VIỆT NAM", "GLOBAL" };

	private string[] strTypeSV = new string[2] { "Máy chủ tiêu chuẩn", "Máy chủ Super" };

	private string[] strTypeSV_info = new string[2] { "Máy chủ tiêu chuẩn:\nTiến trình game bình thường.", "Máy chủ Super:\n -Không thể giao dịch vàng.\n x3 Sức mạnh\n x3 Tiềm năng\n x3 Vàng\n x3 Vật phẩm khác" };

	private string strShowAll = "Chỉ hiện thị máy chủ đã chơi.";

	public int cmy;

	public static Image[] iconHead;

	public static bool isShowSv_HaveChar;

	public ServerScr()
	{
		TileMap.bgID = (byte)(mSystem.currentTimeMillis() % 9);
		if (TileMap.bgID == 5 || TileMap.bgID == 6)
		{
			TileMap.bgID = 4;
		}
		GameScr.loadCamera(fullmScreen: true, -1, -1);
		GameScr.cmx = 100;
		GameScr.cmy = 200;
	}

	public override void switchToMe()
	{
		Res.outz("switchToMe >>>>ServerScr: " + Rms.loadRMSInt(ServerListScreen.RMS_svselect));
		SoundMn.gI().stopAll();
		base.switchToMe();
		loadIconHead();
		mainSelect = ServerListScreen.ipSelect;
		numw = 1;
		numh = 1;
		Load_NewUI();
		if (!isPaintNewUi && !isChooseArea)
		{
			cmdGlobal = new Command(strArea[0], this, 98, null);
			cmdGlobal.x = 0;
			cmdGlobal.y = 0;
			cmdVietNam = new Command(strArea[1], this, 97, null);
			cmdVietNam.x = 50;
			cmdVietNam.y = 0;
			vecServer = new MyVector();
			vecServer.addElement(cmdGlobal);
			vecServer.addElement(cmdVietNam);
			sort();
		}
	}

	private void sort()
	{
		mainSelect = ServerListScreen.ipSelect;
		w2c = 5;
		wc = 76;
		hc = mScreen.cmdH;
		numw = 2;
		if (vecServer.size() > 2)
		{
			numw = GameCanvas.w / (wc + w2c);
		}
		numh = vecServer.size() / numw + ((vecServer.size() % numw != 0) ? 1 : 0);
		for (int i = 0; i < vecServer.size(); i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			if (command != null)
			{
				int num = GameCanvas.hw - numw * (wc + w2c) / 2;
				int num2 = num + i % numw * (wc + w2c);
				int num3 = GameCanvas.hh - numh * (hc + w2c) / 2;
				int num4 = num3 + i / numw * (hc + w2c);
				command.x = num2;
				command.y = num4;
				command.w = wc;
			}
		}
	}

	private void sort_newUI()
	{
		mainSelect = ServerListScreen.ipSelect;
		w2c = 5;
		wc = 76;
		hc = mScreen.cmdH;
		numw = 1;
		int num = xsub + wsub / 2 + 3;
		ysubpaint = ysub + 5;
		numw = wsub / (wc + w2c);
		numh = vecServer.size() / numw + ((vecServer.size() % numw != 0) ? 1 : 0);
		xsubpaint = num - numw * (wc + w2c) / 2;
		for (int i = 0; i < vecServer.size(); i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			if (command != null)
			{
				int num2 = xsubpaint + i % numw * (wc + w2c);
				int num3 = ysubpaint + i / numw * (hc + w2c);
				command.x = num2;
				command.y = num3;
				command.w = wc;
			}
		}
		list = new ListNew(xsub, ysub, wsub, hsub, 0, 0, 0, isLim0: true);
		list.setMaxCamera(numh * (hc + w2c) - hsub);
		list.resetList();
	}

	public override void update()
	{
		GameScr.cmx++;
		if (GameScr.cmx > GameCanvas.w * 3 + 100)
		{
			GameScr.cmx = 100;
		}
		if (!isPaintNewUi)
		{
			for (int i = 0; i < vecServer.size(); i++)
			{
				Command command = (Command)vecServer.elementAt(i);
				if (!GameCanvas.isTouch)
				{
					if (i == mainSelect)
					{
						if (GameCanvas.gameTick % 10 < 4)
						{
							command.isFocus = true;
						}
						else
						{
							command.isFocus = false;
						}
						cmdCheck = new Command(mResources.SELECT, this, command.idAction, null);
						center = cmdCheck;
					}
					else
					{
						command.isFocus = false;
					}
				}
				else if (command != null && command.isPointerPressInside())
				{
					command.performAction();
				}
			}
		}
		UpdTouch_NewUI();
		UpdTouch_NewUI_Popup();
		ServerListScreen.updateDeleteData();
	}

	public override void paint(mGraphics g)
	{
		GameCanvas.paintBGGameScr(g);
		if (isChooseArea)
		{
			paintChooseArea(g);
		}
		else if (isPaintNewUi)
		{
			paintNewSelectMenu(g);
			if (ServerListScreen.cmdDeleteRMS != null)
			{
				mFont.tahoma_7_white.drawString(g, mResources.xoadulieu, GameCanvas.w - 2, GameCanvas.h - 15, 1, mFont.tahoma_7_grey);
			}
		}
		else
		{
			for (int i = 0; i < vecServer.size(); i++)
			{
				if (vecServer.elementAt(i) != null)
				{
					((Command)vecServer.elementAt(i)).paint(g);
				}
			}
		}
		base.paint(g);
	}

	public override void updateKey()
	{
		base.updateKey();
		int num = mainSelect % numw;
		int num2 = mainSelect / numw;
		if (GameCanvas.keyPressed[4])
		{
			if (num > 0)
			{
				mainSelect--;
			}
			GameCanvas.keyPressed[4] = false;
		}
		else if (GameCanvas.keyPressed[6])
		{
			if (num < numw - 1)
			{
				mainSelect++;
			}
			GameCanvas.keyPressed[6] = false;
		}
		else if (GameCanvas.keyPressed[2])
		{
			if (num2 > 0)
			{
				mainSelect -= numw;
			}
			GameCanvas.keyPressed[2] = false;
		}
		else if (GameCanvas.keyPressed[8])
		{
			if (num2 < numh - 1)
			{
				mainSelect += numw;
			}
			GameCanvas.keyPressed[8] = false;
		}
		if (mainSelect < 0)
		{
			mainSelect = 0;
		}
		if (mainSelect >= vecServer.size())
		{
			mainSelect = vecServer.size() - 1;
		}
		if (GameCanvas.keyPressed[5])
		{
			((Command)vecServer.elementAt(num)).performAction();
			GameCanvas.keyPressed[5] = false;
		}
		GameCanvas.clearKeyPressed();
	}

	public void perform(int idAction, object p)
	{
		Res.outz("idAction >>>>   " + idAction);
		switch (idAction)
		{
		case 999:
			Save_RMS_Area();
			SetNewSelectMenu(select_Area, 0);
			break;
		case 97:
		{
			if (isPaintNewUi)
			{
				break;
			}
			vecServer.removeAllElements();
			for (int i = 0; i < ServerListScreen.nameServer.Length; i++)
			{
				if (ServerListScreen.language[i] != 0)
				{
					vecServer.addElement(new Command(ServerListScreen.nameServer[i], this, 100 + i, null));
				}
			}
			sort();
			break;
		}
		case 98:
		{
			if (isPaintNewUi)
			{
				break;
			}
			vecServer.removeAllElements();
			for (int j = 0; j < ServerListScreen.nameServer.Length; j++)
			{
				if (ServerListScreen.language[j] == 0)
				{
					vecServer.addElement(new Command(ServerListScreen.nameServer[j], this, 100 + j, null));
				}
			}
			sort();
			break;
		}
		case 99:
			Session_ME.gI().clearSendingMessage();
			ServerListScreen.SetIpSelect(mainSelect, issave: false);
			GameCanvas.serverScreen.selectServer();
			GameCanvas.serverScreen.switchToMe();
			break;
		default:
			Session_ME.gI().close();
			ServerListScreen.SetIpSelect(idAction - 100, issave: true);
			ServerListScreen.ConnectIP();
			if (GameCanvas.serverScreen == null)
			{
				GameCanvas.serverScreen = new ServerListScreen();
			}
			GameCanvas.serverScreen.selectServer();
			GameCanvas.serverScreen.switchToMe();
			break;
		}
	}

	public void SetNewSelectMenu(int area, int typeSv)
	{
		isChooseArea = false;
		if (mSystem.clientType != 1)
		{
			isPaintNewUi = true;
		}
		wCheck = 10;
		w = GameCanvas.w / 3 * 2;
		h = GameCanvas.h / 3 * 2;
		x = (GameCanvas.w - w) / 2;
		y = (GameCanvas.h - h) / 2 + 20;
		xName = GameCanvas.w / 2;
		yName = y - 30;
		wsub = w / 3 * 2;
		wPop = w - wsub - 15;
		if (wPop < 80)
		{
			wPop = 80;
			wsub = w - wPop - 15;
		}
		hsub = h - 10 - wCheck;
		xsub = x + w - wsub - 5;
		ysub = y + 5;
		xPop = x + 5;
		yPop = y + 5;
		hPop = 20;
		xinfo = x + 5;
		yinfo = y + strTypeSV.Length * (hPop + 5) + 5;
		winfo = wPop;
		hinfo = h - (5 + strTypeSV.Length * (hPop + 5) + 5) - wCheck;
		yBox = 10;
		wBox = 70;
		hBox = 20;
		GetVecTypeSv((sbyte)area, (sbyte)typeSv);
	}

	private void GetVecTypeSv(sbyte area, sbyte typeSv)
	{
		vecServer.removeAllElements();
		ntypeSv = 1;
		select_Area = area;
		mResources.loadLanguague(area);
		for (int i = 0; i < ServerListScreen.nameServer.Length; i++)
		{
			if (area == 1)
			{
				if (ServerListScreen.language[i] != 0 && ServerListScreen.typeSv[i] == 1)
				{
					ntypeSv = 2;
				}
			}
			else if (ServerListScreen.typeSv[i] == 1)
			{
				ntypeSv = 2;
			}
		}
		if (typeSv > (sbyte)(ntypeSv - 1))
		{
			typeSv = (sbyte)(ntypeSv - 1);
		}
		select_typeSv = typeSv;
		for (int j = 0; j < ServerListScreen.nameServer.Length; j++)
		{
			if (area == 1)
			{
				if (ServerListScreen.language[j] == 0)
				{
					continue;
				}
				if (ServerListScreen.typeSv[j] == 1)
				{
					ntypeSv = 2;
				}
				if (ServerListScreen.typeSv[j] != typeSv)
				{
					continue;
				}
				int num = -1;
				if (ServerListScreen.typeClass != null && j < ServerListScreen.typeClass.Length)
				{
					num = ServerListScreen.typeClass[j];
				}
				if (!isShowSv_HaveChar || num != -1)
				{
					Command command = new Command(ServerListScreen.nameServer[j], this, 100 + j, null);
					command.isPaintNew = ServerListScreen.isNew[j] == 1;
					if (num > -1)
					{
						command.imgBtn = iconHead[num];
					}
					vecServer.addElement(command);
				}
				continue;
			}
			if (ServerListScreen.typeSv[j] == 1)
			{
				ntypeSv = 2;
			}
			if (ServerListScreen.language[j] != 0 || ServerListScreen.typeSv[j] != typeSv)
			{
				continue;
			}
			int num2 = -1;
			if (ServerListScreen.typeClass != null && j < ServerListScreen.typeClass.Length)
			{
				num2 = ServerListScreen.typeClass[j];
			}
			if (!isShowSv_HaveChar || num2 != -1)
			{
				Command command2 = new Command(ServerListScreen.nameServer[j], this, 100 + j, null);
				command2.isPaintNew = ServerListScreen.isNew[j] == 1;
				if (num2 > -1)
				{
					command2.imgBtn = iconHead[num2];
				}
				vecServer.addElement(command2);
			}
		}
		Sort_NewSv();
		sort_newUI();
	}

	private void paintChooseArea(mGraphics g)
	{
		if (isChooseArea)
		{
			paint_Area(g, GameCanvas.hw - wBox / 2, yBox);
			paint_Lang(g, GameCanvas.hw + 20, yBox);
			cmdChooseArea.paint(g);
		}
	}

	private void paintNewSelectMenu(mGraphics g)
	{
		if (!isPaintNewUi)
		{
			return;
		}
		g.setColor(14601141);
		g.fillRect(x, y, w, h);
		PopUp.paintPopUp(g, xName - 50, yName, 100, 20, 0, isButton: true);
		mFont.tahoma_7b_dark.drawString(g, mResources.selectServer2, xName, yName + 5, 2);
		for (int i = 0; i < ntypeSv; i++)
		{
			int num = yPop + i * (hPop + 5);
			PopUp.paintPopUp(g, xPop, num, wPop, hPop, (select_typeSv == i) ? 1 : 0, isButton: true);
			mFont.tahoma_7b_dark.drawString(g, strTypeSV[i], xPop + wPop / 2, num + 5, 2);
		}
		g.setColor(10254674);
		g.fillRect(xinfo, yinfo, winfo, hinfo);
		string[] array = mFont.tahoma_7.splitFontArray(strTypeSV_info[select_typeSv], winfo - 10);
		for (int j = 0; j < array.Length; j++)
		{
			mFont.tahoma_7_white.drawString(g, array[j], xinfo + 5, yinfo + 5 + j * 11, 0);
		}
		paintShowAllCheck(g);
		paint_Area(g, 10, yBox);
		paint_Lang(g, GameCanvas.w - wBox - 10, yBox);
		g.setColor(10254674);
		g.fillRect(xsub, ysub, wsub, hsub);
		g.setClip(xsub, ysub, wsub, hsub);
		g.translate(0, -list.cmx);
		for (int k = 0; k < vecServer.size(); k++)
		{
			Command command = (Command)vecServer.elementAt(k);
			if (command != null)
			{
				command.paint(g);
				if (command.isPaintNew && GameCanvas.gameTick % 10 > 1)
				{
					g.drawImage(Panel.imgNew, command.x + 60, command.y, 0);
				}
			}
		}
		GameCanvas.resetTrans(g);
	}

	private void paint_Area(mGraphics g, int x, int y)
	{
		x -= 5;
		xPopUp_Area = x;
		PopUp.paintPopUp(g, x, y, wBox, hBox, 0, isButton: true);
		mFont.tahoma_7b_dark.drawString(g, strArea[select_Area], x + (wBox - 10) / 2, y + 5, 2);
		g.drawRegion(Mob.imgHP, 0, 30, 9, 6, 0, x + wBox - 10, y + 14, mGraphics.BOTTOM | mGraphics.HCENTER);
		if (!isPaint_select_area)
		{
			return;
		}
		yPopUp_Area = y + hBox + 5;
		g.setColor(10254674);
		g.fillRect(x, yPopUp_Area, wBox, strArea.Length * htext + 1);
		for (int i = 0; i < strArea.Length; i++)
		{
			mFont.tahoma_7_white.drawString(g, strArea[i], x + wBox / 2, yPopUp_Area + i * htext + 2, 2);
			if (select_Area == i)
			{
				g.setColor(15591444);
				g.drawRect(x + 2, yPopUp_Area + i * htext + 1, wBox - 4, htext - 2);
			}
		}
	}

	private void paint_Lang(mGraphics g, int x, int y)
	{
	}

	private void UpdTouch_NewUI()
	{
		if (!isPaintNewUi)
		{
			return;
		}
		int num = 0;
		if (list != null)
		{
			list.moveCamera();
			if (GameCanvas.isPointer(xsub, 0, wsub, GameCanvas.h))
			{
				list.update_Pos_UP_DOWN();
			}
			num = list.cmx;
		}
		if (GameCanvas.isPointer(xsub, ysub, wsub, hsub))
		{
			int num2 = (GameCanvas.px - xsubpaint) / (wc + w2c) + (GameCanvas.py - ysubpaint + num) / (hc + w2c) * numw;
			int num3 = vecServer.size();
			if (num2 >= 0 && num2 < num3)
			{
				mainSelect = num2;
				for (int i = 0; i < vecServer.size(); i++)
				{
					Command command = (Command)vecServer.elementAt(i);
					if (command == null)
					{
						continue;
					}
					if (i == mainSelect)
					{
						if (command.isPointerPressInsideCamera(0, num))
						{
							command.performAction();
						}
					}
					else
					{
						command.isFocus = false;
					}
				}
			}
		}
		if (GameCanvas.isPointer(xinfo - 2, yinfo + hinfo, wCheck + 4, wCheck + 4) && GameCanvas.isPointerJustRelease)
		{
			isShowSv_HaveChar = !isShowSv_HaveChar;
			GetVecTypeSv(select_Area, select_typeSv);
		}
		if (ntypeSv == 1)
		{
			return;
		}
		for (sbyte b = 0; b < ntypeSv; b++)
		{
			int num4 = yPop + b * (hPop + 5);
			if (GameCanvas.isPointerHoldIn(xPop, num4, wPop, hPop) && GameCanvas.isPointerDown)
			{
				GetVecTypeSv(select_Area, b);
				break;
			}
		}
	}

	private void UpdTouch_NewUI_Popup()
	{
		if (GameCanvas.isPointer(xPopUp_Area, yBox, wBox, hBox) && GameCanvas.isPointerJustRelease)
		{
			isPaint_select_area = !isPaint_select_area;
			isPaint_select_lang = false;
			GameCanvas.isPointerJustRelease = false;
		}
		if (!isPaint_select_area)
		{
			return;
		}
		for (sbyte b = 0; b < strArea.Length; b++)
		{
			int num = yPopUp_Area + b * htext;
			if (GameCanvas.isPointerHoldIn(xPopUp_Area, num, wBox, htext) && GameCanvas.isPointerDown)
			{
				if (isChooseArea)
				{
					select_Area = b;
				}
				else
				{
					SetNewSelectMenu(b, select_typeSv);
				}
				isPaint_select_lang = (isPaint_select_area = false);
				break;
			}
		}
	}

	private void Load_NewUI()
	{
		if (GameCanvas.isTouch)
		{
			if (Rms.loadRMS("area_select") == null)
			{
				isChooseArea = true;
				cmdChooseArea = new Command(mResources.OK, this, 999, null);
				cmdChooseArea.x = GameCanvas.hw - 38;
				cmdChooseArea.y = GameCanvas.hh + 50;
				vecServer = new MyVector();
				vecServer.addElement(cmdChooseArea);
				yBox = GameCanvas.hh - 30;
				wBox = 70;
				hBox = 20;
			}
			else
			{
				isChooseArea = false;
				Load_RMS_Area();
				SetNewSelectMenu(select_Area, select_typeSv);
			}
		}
	}

	private void Save_RMS_Area()
	{
		Rms.saveRMS("area_select", new sbyte[2] { select_Area, select_Lang });
	}

	private void Load_RMS_Area()
	{
		sbyte[] array = Rms.loadRMS("area_select");
		try
		{
			select_Area = array[0];
			select_Lang = array[1];
		}
		catch (Exception)
		{
			select_Area = (select_Lang = 0);
		}
	}

	public void Sort_NewSv()
	{
		for (int i = 0; i < vecServer.size() - 1; i++)
		{
			Command command = (Command)vecServer.elementAt(i);
			for (int j = i + 1; j < vecServer.size(); j++)
			{
				Command command2 = (Command)vecServer.elementAt(j);
				if (command2.isPaintNew && !command.isPaintNew)
				{
					Command command3 = command2;
					command2 = command;
					command = command3;
					vecServer.setElementAt(command, i);
					vecServer.setElementAt(command2, j);
				}
			}
		}
	}

	public void loadIconHead()
	{
		if (iconHead == null)
		{
			iconHead = new Image[3];
			for (int i = 0; i < iconHead.Length; i++)
			{
				iconHead[i] = GameCanvas.loadImage("/iconHead_" + i + ".png");
			}
		}
	}

	public void paintShowAllCheck(mGraphics g)
	{
		int num = xinfo;
		int num2 = yinfo + hinfo + 2;
		g.setColor(16777215);
		g.fillRect(num, num2, wCheck, wCheck);
		if (isShowSv_HaveChar)
		{
			g.setColor(3329330);
			g.fillRect(num + 1, num2 + 1, wCheck - 2, wCheck - 2);
		}
		mFont.tahoma_7b_dark.drawString(g, strShowAll, num + wCheck + 2, num2, 0);
	}
}
