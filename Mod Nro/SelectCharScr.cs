using System;
using Assets.src.e;

public class SelectCharScr : mScreen, IActionListener
{
	public static bool isWait;

	public static SelectCharScr instance;

	public Char mychar;

	private int indexGender;

	private int cx;

	private int cy;

	private int dy = 45;

	private Command cmdSelectSv;

	private int[] bgID = new int[3] { 0, 4, 8 };

	private int[] f = new int[10] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };

	private int count;

	public SelectCharScr()
	{
		try
		{
			if (!GameCanvas.lowGraphic)
			{
				loadMapFromResource(new sbyte[3] { 39, 40, 41 });
			}
			loadMapTableFromResource(new sbyte[3] { 39, 40, 41 });
		}
		catch (Exception ex)
		{
			Cout.LogError("Tao char loi " + ex.ToString());
		}
		cx = 168;
		cy = 350;
		short num = 32000;
		SmallImage.imgNew = new Small[num];
		SmallImage.newSmallVersion = new sbyte[num];
		SmallImage.maxSmall = num;
	}

	public static SelectCharScr gI()
	{
		if (instance == null)
		{
			instance = new SelectCharScr();
		}
		return instance;
	}

	public static void loadMapFromResource(sbyte[] mapID)
	{
		Res.outz("newwwwwwwwww =============");
		DataInputStream dataInputStream = null;
		for (int i = 0; i < mapID.Length; i++)
		{
			dataInputStream = MyStream.readFile("/mymap/" + mapID[i]);
			MapTemplate.tmw[i] = (ushort)dataInputStream.read();
			MapTemplate.tmh[i] = (ushort)dataInputStream.read();
			Cout.LogError("Thong TIn : " + MapTemplate.tmw[i] + "::" + MapTemplate.tmh[i]);
			MapTemplate.maps[i] = new int[dataInputStream.available()];
			Cout.LogError("lent= " + MapTemplate.maps[i].Length);
			for (int j = 0; j < MapTemplate.tmw[i] * MapTemplate.tmh[i]; j++)
			{
				MapTemplate.maps[i][j] = dataInputStream.read();
			}
			MapTemplate.types[i] = new int[MapTemplate.maps[i].Length];
		}
	}

	public void loadMapTableFromResource(sbyte[] mapID)
	{
		if (GameCanvas.lowGraphic)
		{
			return;
		}
		DataInputStream dataInputStream = null;
		try
		{
			for (int i = 0; i < mapID.Length; i++)
			{
				dataInputStream = MyStream.readFile("/mymap/mapTable" + mapID[i]);
				Cout.LogError("mapTable : " + mapID[i]);
				short num = dataInputStream.readShort();
				MapTemplate.vCurrItem[i] = new MyVector();
				Res.outz("nItem= " + num);
				for (int j = 0; j < num; j++)
				{
					short id = dataInputStream.readShort();
					short num2 = dataInputStream.readShort();
					short num3 = dataInputStream.readShort();
					if (TileMap.getBIById(id) == null)
					{
						continue;
					}
					BgItem bIById = TileMap.getBIById(id);
					BgItem bgItem = new BgItem();
					bgItem.id = id;
					bgItem.idImage = bIById.idImage;
					bgItem.dx = bIById.dx;
					bgItem.dy = bIById.dy;
					bgItem.x = num2 * TileMap.size;
					bgItem.y = num3 * TileMap.size;
					bgItem.layer = bIById.layer;
					MapTemplate.vCurrItem[i].addElement(bgItem);
					if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
					{
						try
						{
							Image image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
							if (image == null)
							{
								BgItem.imgNew.put(bgItem.idImage + string.Empty, Image.createRGBImage(new int[1], 1, 1, bl: true));
								Service.gI().getBgTemplate(bgItem.idImage);
							}
							else
							{
								BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
							}
						}
						catch (Exception)
						{
							Image image2 = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
							if (image2 == null)
							{
								image2 = Image.createRGBImage(new int[1], 1, 1, bl: true);
								Service.gI().getBgTemplate(bgItem.idImage);
							}
							BgItem.imgNew.put(bgItem.idImage + string.Empty, image2);
						}
						BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
					}
					if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
					{
						BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
					}
					bgItem.changeColor();
				}
			}
		}
		catch (Exception ex2)
		{
			Cout.println("LOI TAI loadMapTableFromResource" + ex2.ToString());
		}
	}

	public void doChangeMap()
	{
		TileMap.maps = new int[MapTemplate.maps[indexGender].Length];
		for (int i = 0; i < MapTemplate.maps[indexGender].Length; i++)
		{
			TileMap.maps[i] = MapTemplate.maps[indexGender][i];
		}
		TileMap.types = MapTemplate.types[indexGender];
		TileMap.pxh = MapTemplate.pxh[indexGender];
		TileMap.pxw = MapTemplate.pxw[indexGender];
		TileMap.tileID = MapTemplate.pxw[indexGender];
		TileMap.tmw = MapTemplate.tmw[indexGender];
		TileMap.tmh = MapTemplate.tmh[indexGender];
		TileMap.tileID = bgID[indexGender] + 1;
		TileMap.loadMainTile();
		TileMap.loadTileCreatChar();
		GameCanvas.loadBG(bgID[indexGender]);
		GameScr.loadCamera(fullmScreen: true, cx, cy);
	}

	public void SetInfoChar(Char temp)
	{
		mychar = new Char();
		indexGender = (mychar.cgender = temp.cgender);
		mychar.head = temp.head;
		mychar.headICON = temp.headICON;
		mychar.body = temp.body;
		mychar.leg = temp.leg;
		mychar.bag = temp.bag;
		mychar.cName = temp.cName;
		switchToMe();
	}

	public override void switchToMe()
	{
		GameCanvas.menu.showMenu = false;
		GameCanvas.endDlg();
		GameScr.gI().initSelectChar();
		base.switchToMe();
		doChangeMap();
		Char.isLoadingMap = false;
		center = new Command(mResources.SELECT, this, 100, null);
		left = new Command(mResources.BACK, this, 101, null);
		cmdSelectSv = new Command(ServerListScreen.nameServer[ServerListScreen.ipSelect], this, 102, null);
		cmdSelectSv.x = 1;
		cmdSelectSv.y = 3;
	}

	public override void paint(mGraphics g)
	{
		if (!Controller.isGet_CLIENT_INFO || isWait || Char.isLoadingMap)
		{
			return;
		}
		GameCanvas.paintBGGameScr(g);
		g.translate(-GameScr.cmx, -GameScr.cmy);
		for (int i = 0; i < MapTemplate.vCurrItem[indexGender].size(); i++)
		{
			BgItem bgItem = (BgItem)MapTemplate.vCurrItem[indexGender].elementAt(i);
			if (bgItem.idImage != -1 && bgItem.layer == 1)
			{
				bgItem.paint(g);
			}
		}
		TileMap.paintTilemapLuoi(g);
		g.drawImage(TileMap.bong, GameScr.cmx + GameCanvas.hw, cy + dy + 1, 3);
		if (mychar != null)
		{
			mychar.paintCharBody(g, GameScr.cmx + GameCanvas.hw, cy + dy, 1, f[count], isPaintBag: true);
			mFont.tahoma_7b_yellow.drawString(g, mychar.cName, GameScr.cmx + GameCanvas.hw, cy - 15, mFont.CENTER, mFont.tahoma_7_greySmall);
		}
		g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
		base.paint(g);
		cmdSelectSv.paint(g);
	}

	public override void update()
	{
		base.update();
		if (!Session_ME.gI().isConnected())
		{
			isWait = true;
			count++;
			return;
		}
		isWait = false;
		count++;
		if (count > f.Length - 1)
		{
			count = 0;
		}
		if (cmdSelectSv != null && cmdSelectSv.isPointerPressInside())
		{
			cmdSelectSv.performAction();
		}
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 100:
			break;
		case 101:
			GameCanvas.serverScreen.switchToMe();
			break;
		case 102:
			GameCanvas.serverScr.switchToMe();
			break;
		}
	}
}
