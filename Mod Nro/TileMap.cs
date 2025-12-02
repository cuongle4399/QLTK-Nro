using System;

public class TileMap
{
	public const int T_EMPTY = 0;

	public const int T_TOP = 2;

	public const int T_LEFT = 4;

	public const int T_RIGHT = 8;

	public const int T_TREE = 16;

	public const int T_WATERFALL = 32;

	public const int T_WATERFLOW = 64;

	public const int T_TOPFALL = 128;

	public const int T_OUTSIDE = 256;

	public const int T_DOWN1PIXEL = 512;

	public const int T_BRIDGE = 1024;

	public const int T_UNDERWATER = 2048;

	public const int T_SOLIDGROUND = 4096;

	public const int T_BOTTOM = 8192;

	public const int T_DIE = 16384;

	public const int T_HEBI = 32768;

	public const int T_BANG = 65536;

	public const int T_JUM8 = 131072;

	public const int T_NT0 = 262144;

	public const int T_NT1 = 524288;

	public const int T_CENTER = 1;

	public static int tmw;

	public static int tmh;

	public static int pxw;

	public static int pxh;

	public static int tileID;

	public static int lastTileID;

	public static int[] maps;

	public static int[] types;

	public static Image[] imgTile;

	public static Image imgTileSmall;

	public static Image imgMiniMap;

	public static Image imgWaterfall;

	public static Image imgTopWaterfall;

	public static Image imgWaterflow;

	public static Image imgWaterlowN;

	public static Image imgWaterlowN2;

	public static Image imgWaterF;

	public static Image imgLeaf;

	public static sbyte size;

	private static int bx;

	private static int dbx;

	private static int fx;

	private static int dfx;

	public static string[] instruction;

	public static int[] iX;

	public static int[] iY;

	public static int[] iW;

	public static int iCount;

	public static bool isMapDouble;

	public static string mapName;

	public static sbyte versionMap;

	public static int mapID;

	public static int lastBgID;

	public static int zoneID;

	public static int bgID;

	public static int bgType;

	public static int lastType;

	public static int typeMap;

	public static sbyte planetID;

	public static sbyte lastPlanetId;

	public static long timeTranMini;

	public static MyVector vGo;

	public static MyVector vItemBg;

	public static MyVector vCurrItem;

	public static string[] mapNames;

	public static sbyte MAP_NORMAL;

	public static Image bong;

	public const int TRAIDAT_DOINUI = 0;

	public const int TRAIDAT_RUNG = 1;

	public const int TRAIDAT_DAORUA = 2;

	public const int TRAIDAT_DADO = 3;

	public const int NAMEK_THUNGLUNG = 5;

	public const int NAMEK_DOINUI = 4;

	public const int NAMEK_RUNG = 6;

	public const int NAMEK_DAO = 7;

	public const int SAYAI_DOINUI = 8;

	public const int SAYAI_RUNG = 9;

	public const int SAYAI_CITY = 10;

	public const int SAYAI_NIGHT = 11;

	public const int KAMISAMA = 12;

	public const int TIME_ROOM = 13;

	public const int HELL = 15;

	public const int BEERUS = 16;

	public const int THE_HELL = 19;

	public static Image[] bgItem;

	public static MyVector vObject;

	public static int[] offlineId;

	public static int[] highterId;

	public static int[] toOfflineId;

	public static int[][] tileType;

	public static int[][][] tileIndex;

	public static int sizeMiniMap;

	public static int gssx;

	public static int gssxe;

	public static int gssy;

	public static int gssye;

	public static int countx;

	public static int county;

	private static int[] colorMini;

	public static int yWater;

	public static void loadBg()
	{
		bong = GameCanvas.loadImage("/mainImage/myTexture2dbong.png");
	}

	public static bool isVoDaiMap()
	{
		if (mapID != 51 && mapID != 103 && mapID != 112 && mapID != 113 && mapID != 129)
		{
			return mapID == 130;
		}
		return true;
	}

	public static bool isTrainingMap()
	{
		if (mapID != 39 && mapID != 40)
		{
			return mapID == 41;
		}
		return true;
	}

	public static bool mapPhuBang()
	{
		if (GameScr.phuban_Info != null)
		{
			return mapID == GameScr.phuban_Info.idmapPaint;
		}
		return false;
	}

	public static BgItem getBIById(int id)
	{
		for (int i = 0; i < vItemBg.size(); i++)
		{
			BgItem bgItem = (BgItem)vItemBg.elementAt(i);
			if (bgItem.id == id)
			{
				return bgItem;
			}
		}
		return null;
	}

	public static bool isOfflineMap()
	{
		for (int i = 0; i < offlineId.Length; i++)
		{
			if (mapID == offlineId[i])
			{
				return true;
			}
		}
		return false;
	}

	public static bool isHighterMap()
	{
		for (int i = 0; i < offlineId.Length; i++)
		{
			if (mapID == highterId[i])
			{
				return true;
			}
		}
		return false;
	}

	public static bool isToOfflineMap()
	{
		for (int i = 0; i < toOfflineId.Length; i++)
		{
			if (mapID == toOfflineId[i])
			{
				return true;
			}
		}
		return false;
	}

	public static void freeTilemap()
	{
		imgTile = null;
		mSystem.gcc();
	}

	public static void loadTileCreatChar()
	{
	}

	public static bool isExistMoreOne(int id)
	{
		if (id == 156 || id == 330 || id == 345 || id == 334)
		{
			return false;
		}
		if (mapID == 54 || mapID == 55 || mapID == 56 || mapID == 57 || mapID == 58 || mapID == 59 || mapID == 103)
		{
			return false;
		}
		int num = 0;
		for (int i = 0; i < vCurrItem.size(); i++)
		{
			if (((BgItem)vCurrItem.elementAt(i)).id == id)
			{
				num++;
			}
		}
		return num > 2;
	}

	public static void loadTileImage()
	{
		if (imgWaterfall == null)
		{
			imgWaterfall = GameCanvas.loadImageRMS("/tWater/wtf.png");
		}
		if (imgTopWaterfall == null)
		{
			imgTopWaterfall = GameCanvas.loadImageRMS("/tWater/twtf.png");
		}
		if (imgWaterflow == null)
		{
			imgWaterflow = GameCanvas.loadImageRMS("/tWater/wts.png");
		}
		if (imgWaterlowN == null)
		{
			imgWaterlowN = GameCanvas.loadImageRMS("/tWater/wtsN.png");
		}
		if (imgWaterlowN2 == null)
		{
			imgWaterlowN2 = GameCanvas.loadImageRMS("/tWater/wtsN2.png");
		}
		mSystem.gcc();
	}

	public static void setTile(int index, int[] mapsArr, int type)
	{
		for (int i = 0; i < mapsArr.Length; i++)
		{
			if (maps[index] == mapsArr[i])
			{
				types[index] |= type;
				break;
			}
		}
	}

	public static void loadMap(int tileId)
	{
		pxh = tmh * size;
		pxw = tmw * size;
		Res.outz("load tile ID= " + tileID);
		int num = tileId - 1;
		try
		{
			for (int i = 0; i < tmw * tmh; i++)
			{
				for (int j = 0; j < tileType[num].Length; j++)
				{
					setTile(i, tileIndex[num][j], tileType[num][j]);
				}
			}
		}
		catch (Exception)
		{
			Cout.println("Error Load Map");
			GameMidlet.instance.exit();
		}
	}

	public static bool isInAirMap()
	{
		if (mapID != 45 && mapID != 46)
		{
			return mapID == 48;
		}
		return true;
	}

	public static bool isDoubleMap()
	{
		if (!isMapDouble && mapID != 45 && mapID != 46 && mapID != 48 && mapID != 51 && mapID != 52 && mapID != 103 && mapID != 112 && mapID != 113 && mapID != 115 && mapID != 117 && mapID != 118 && mapID != 119 && mapID != 120 && mapID != 121 && mapID != 125 && mapID != 129)
		{
			return mapID == 130;
		}
		return true;
	}

	public static void getTile()
	{
		if (Main.typeClient == 3 || Main.typeClient == 5)
		{
			if (mGraphics.zoomLevel == 1)
			{
				imgTile = new Image[1];
				imgTile[0] = GameCanvas.loadImage("/t/" + tileID + ".png");
				return;
			}
			imgTile = new Image[100];
			for (int i = 0; i < imgTile.Length; i++)
			{
				imgTile[i] = GameCanvas.loadImage("/t/" + tileID + "/" + (i + 1) + ".png");
			}
			return;
		}
		if (mGraphics.zoomLevel == 1)
		{
			if (imgTile != null)
			{
				for (int j = 0; j < imgTile.Length; j++)
				{
					if (imgTile[j] != null)
					{
						imgTile[j].texture = null;
						imgTile[j] = null;
					}
				}
				mSystem.gcc();
			}
			imgTile = new Image[100];
			string empty = string.Empty;
			for (int k = 0; k < imgTile.Length; k++)
			{
				empty = ((k >= 9) ? ("/t/" + tileID + "/t_" + (k + 1)) : ("/t/" + tileID + "/t_0" + (k + 1)));
				imgTile[k] = GameCanvas.loadImage(empty);
			}
			return;
		}
		Image image = GameCanvas.loadImageRMS("/t/" + tileID + "$1.png");
		if (image != null)
		{
			Rms.DeleteStorage("x" + mGraphics.zoomLevel + "t" + tileID);
			imgTile = new Image[100];
			for (int l = 0; l < imgTile.Length; l++)
			{
				imgTile[l] = GameCanvas.loadImageRMS("/t/" + tileID + "$" + (l + 1) + ".png");
			}
		}
		else
		{
			image = GameCanvas.loadImageRMS("/t/" + tileID + ".png");
			if (image != null)
			{
				Rms.DeleteStorage("$");
				imgTile = new Image[1];
				imgTile[0] = image;
			}
		}
	}

	public static void paintTile(mGraphics g, int frame, int indexX, int indexY)
	{
		if (imgTile != null)
		{
			if (imgTile.Length == 1)
			{
				g.drawRegion(imgTile[0], 0, frame * size, size, size, 0, indexX * size, indexY * size, 0);
			}
			else
			{
				g.drawImage(imgTile[frame], indexX * size, indexY * size, 0);
			}
		}
	}

	public static void paintTile(mGraphics g, int frame, int x, int y, int w, int h)
	{
		if (imgTile != null)
		{
			if (imgTile.Length == 1)
			{
				g.drawRegion(imgTile[0], 0, frame * w, w, w, 0, x, y, 0);
			}
			else
			{
				g.drawImage(imgTile[frame], x, y, 0);
			}
		}
	}

	public static void paintTilemapLOW(mGraphics g)
	{
		for (int i = GameScr.gssx; i < GameScr.gssxe; i++)
		{
			for (int j = GameScr.gssy; j < GameScr.gssye; j++)
			{
				int num = maps[j * tmw + i] - 1;
				if (num != -1)
				{
					paintTile(g, num, i, j);
				}
				if ((tileTypeAt(i, j) & 0x20) == 32)
				{
					g.drawRegion(imgWaterfall, 0, 24 * (GameCanvas.gameTick % 4), 24, 24, 0, i * size, j * size, 0);
				}
				else if ((tileTypeAt(i, j) & 0x40) == 64)
				{
					if ((tileTypeAt(i, j - 1) & 0x20) == 32)
					{
						g.drawRegion(imgWaterfall, 0, 24 * (GameCanvas.gameTick % 4), 24, 24, 0, i * size, j * size, 0);
					}
					else if ((tileTypeAt(i, j - 1) & 0x1000) == 4096)
					{
						paintTile(g, 21, i, j);
					}
					Image arg = ((tileID == 5) ? imgWaterlowN : ((tileID != 8) ? imgWaterflow : imgWaterlowN2));
					g.drawRegion(arg, 0, (GameCanvas.gameTick % 8 >> 2) * 24, 24, 24, 0, i * size, j * size, 0);
				}
				if ((tileTypeAt(i, j) & 0x800) == 2048)
				{
					if ((tileTypeAt(i, j - 1) & 0x20) == 32)
					{
						g.drawRegion(imgWaterfall, 0, 24 * (GameCanvas.gameTick % 4), 24, 24, 0, i * size, j * size, 0);
					}
					else if ((tileTypeAt(i, j - 1) & 0x1000) == 4096)
					{
						paintTile(g, 21, i, j);
					}
					paintTile(g, maps[j * tmw + i] - 1, i, j);
				}
			}
		}
	}

	public static void paintTilemap(mGraphics g)
	{
		if (Char.isLoadingMap)
		{
			return;
		}
		GameScr.gI().paintBgItem(g, 1);
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			((ItemMap)GameScr.vItemMap.elementAt(i)).paintAuraItemEff(g);
		}
		for (int j = GameScr.gssx; j < GameScr.gssxe; j++)
		{
			for (int k = GameScr.gssy; k < GameScr.gssye; k++)
			{
				if (j == 0 || j == tmw - 1)
				{
					continue;
				}
				int num = maps[k * tmw + j] - 1;
				if ((tileTypeAt(j, k) & 0x100) == 256)
				{
					continue;
				}
				if ((tileTypeAt(j, k) & 0x20) == 32)
				{
					g.drawRegion(imgWaterfall, 0, 24 * (GameCanvas.gameTick % 8 >> 1), 24, 24, 0, j * size, k * size, 0);
				}
				else if ((tileTypeAt(j, k) & 0x80) == 128)
				{
					g.drawRegion(imgTopWaterfall, 0, 24 * (GameCanvas.gameTick % 8 >> 1), 24, 24, 0, j * size, k * size, 0);
				}
				else
				{
					if (tileID == 13 && num != -1)
					{
						continue;
					}
					if (tileID == 2 && (tileTypeAt(j, k) & 0x200) == 512 && num != -1)
					{
						paintTile(g, num, j * size, k * size, 24, 1);
						paintTile(g, num, j * size, k * size + 1, 24, 24);
					}
					_ = tileID;
					if ((tileTypeAt(j, k) & 0x10) == 16)
					{
						bx = j * size - GameScr.cmx;
						dbx = bx - GameScr.gW2;
						dfx = (size - 2) * dbx / size;
						fx = dfx + GameScr.gW2;
						paintTile(g, num, fx + GameScr.cmx, k * size, 24, 24);
					}
					else if ((tileTypeAt(j, k) & 0x200) == 512)
					{
						if (num != -1)
						{
							paintTile(g, num, j * size, k * size, 24, 1);
							paintTile(g, num, j * size, k * size + 1, 24, 24);
						}
					}
					else if (num != -1)
					{
						paintTile(g, num, j, k);
					}
				}
			}
		}
		if (GameScr.cmx < 24)
		{
			for (int l = GameScr.gssy; l < GameScr.gssye; l++)
			{
				int num2 = maps[l * tmw + 1] - 1;
				if (num2 != -1)
				{
					paintTile(g, num2, 0, l);
				}
			}
		}
		if (GameScr.cmx <= GameScr.cmxLim)
		{
			return;
		}
		int num3 = tmw - 2;
		for (int m = GameScr.gssy; m < GameScr.gssye; m++)
		{
			int num4 = maps[m * tmw + num3] - 1;
			if (num4 != -1)
			{
				paintTile(g, num4, num3 + 1, m);
			}
		}
	}

	public static bool isWaterEff()
	{
		if (mapID != 54 && mapID != 55 && mapID != 56 && mapID != 57 && mapID != 138)
		{
			return mapID != 167;
		}
		return false;
	}

	public static void paintOutTilemap(mGraphics g)
	{
		if (GameCanvas.lowGraphic)
		{
			return;
		}
		int num = 0;
		for (int i = GameScr.gssx; i < GameScr.gssxe; i++)
		{
			for (int j = GameScr.gssy; j < GameScr.gssye; j++)
			{
				num++;
				if ((tileTypeAt(i, j) & 0x40) != 64)
				{
					continue;
				}
				Image arg = ((tileID == 5) ? imgWaterlowN : ((tileID != 8) ? imgWaterflow : imgWaterlowN2));
				if (!isWaterEff())
				{
					g.drawRegion(arg, 0, 0, 24, 24, 0, i * size, j * size - 1, 0);
					g.drawRegion(arg, 0, 0, 24, 24, 0, i * size, j * size - 3, 0);
				}
				g.drawRegion(arg, 0, (GameCanvas.gameTick % 8 >> 2) * 24, 24, 24, 0, i * size, j * size - 12, 0);
				if (yWater == 0 && isWaterEff())
				{
					yWater = j * size - 12;
					int color = 16777215;
					if (GameCanvas.typeBg == 2)
					{
						color = 10871287;
					}
					else if (GameCanvas.typeBg == 4)
					{
						color = 8111470;
					}
					else if (GameCanvas.typeBg == 7)
					{
						color = 5693125;
					}
					else if (GameCanvas.typeBg == 19)
					{
						color = 16711680;
					}
					BackgroudEffect.addWater(color, yWater + 15);
				}
			}
		}
		BackgroudEffect.paintWaterAll(g);
	}

	public static void loadMapFromResource(int mapID)
	{
		DataInputStream dataInputStream = MyStream.readFile("/mymap/" + mapID);
		tmw = (ushort)dataInputStream.read();
		tmh = (ushort)dataInputStream.read();
		maps = new int[dataInputStream.available()];
		for (int i = 0; i < tmw * tmh; i++)
		{
			maps[i] = (ushort)dataInputStream.read();
		}
		types = new int[maps.Length];
	}

	public static int tileAt(int x, int y)
	{
		try
		{
			return maps[y * tmw + x];
		}
		catch (Exception)
		{
			return 1000;
		}
	}

	public static int tileTypeAt(int x, int y)
	{
		try
		{
			return types[y * tmw + x];
		}
		catch (Exception)
		{
			return 1000;
		}
	}

	public static int tileTypeAtPixel(int px, int py)
	{
		try
		{
			return types[py / size * tmw + px / size];
		}
		catch (Exception)
		{
			return 1000;
		}
	}

	public static bool tileTypeAt(int px, int py, int t)
	{
		try
		{
			return (types[py / size * tmw + px / size] & t) == t;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void setTileTypeAtPixel(int px, int py, int t)
	{
		types[py / size * tmw + px / size] |= t;
	}

	public static void setTileTypeAt(int x, int y, int t)
	{
		types[y * tmw + x] = t;
	}

	public static void killTileTypeAt(int px, int py, int t)
	{
		types[py / size * tmw + px / size] &= ~t;
	}

	public static int tileYofPixel(int py)
	{
		return py / size * size;
	}

	public static int tileXofPixel(int px)
	{
		return px / size * size;
	}

	public static int GetMapEndX()
	{
		return pxw;
	}

	public static int GetMapEndY()
	{
		return pxh;
	}

	public static void loadMainTile()
	{
		if (lastTileID != tileID)
		{
			getTile();
			lastTileID = tileID;
		}
	}

	static TileMap()
	{
		lastTileID = -1;
		size = 24;
		isMapDouble = false;
		mapName = string.Empty;
		versionMap = 1;
		lastBgID = -1;
		lastType = -1;
		lastPlanetId = -1;
		vGo = new MyVector();
		vItemBg = new MyVector();
		vCurrItem = new MyVector();
		MAP_NORMAL = 0;
		bgItem = new Image[8];
		vObject = new MyVector();
		offlineId = new int[6] { 21, 22, 23, 39, 40, 41 };
		highterId = new int[6] { 21, 22, 23, 24, 25, 26 };
		toOfflineId = new int[3] { 0, 7, 14 };
		sizeMiniMap = 2;
		colorMini = new int[2] { 5257738, 8807192 };
		yWater = 0;
	}

	public static void paintTilemapLuoi(mGraphics g)
	{
		if (Char.isLoadingMap)
		{
			return;
		}
		g.setColor(0);
		GameScr.gI().paintBgItem(g, 1);
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			((ItemMap)GameScr.vItemMap.elementAt(i)).paintAuraItemEff(g);
		}
		for (int j = GameScr.gssx; j < GameScr.gssxe; j++)
		{
			for (int k = GameScr.gssy; k < GameScr.gssye; k++)
			{
				int num = k * tmw + j;
				int num2 = j * 24;
				int num3 = k * 24;
				bool flag = tileTypeAt(num2, num3, 2);
				bool flag2 = tileTypeAt(num2, (k + 1) * 24, 2);
				bool flag3 = tileTypeAt(num2, (k + 2) * 24, 2);
				if (maps[num] != 0 && (flag || (!flag && !flag2 && !flag3)))
				{
					if (j > 0)
					{
						g.drawRect(num2, num3 + 8, 24, 24);
					}
					else
					{
						g.fillRect(num2, k * 8, 24, 24);
					}
				}
			}
		}
		if (GameScr.cmx < 24)
		{
			for (int l = GameScr.gssy; l < GameScr.gssye; l++)
			{
				int num4 = maps[l * tmw + 1] - 1;
				if (num4 != -1)
				{
					paintTile(g, num4, 0, l);
				}
			}
		}
		if (GameScr.cmx <= GameScr.cmxLim)
		{
			return;
		}
		int num5 = tmw - 2;
		for (int m = GameScr.gssy; m < GameScr.gssye; m++)
		{
			int num6 = maps[m * tmw + num5] - 1;
			if (num6 != -1)
			{
				paintTile(g, num6, num5 + 1, m);
			}
		}
	}
}
