using System.Collections.Generic;

namespace Client244.Xmap;

public static class DataXmap
{
	public static readonly Dictionary<int, List<NextMap>> linkMaps;

	public static readonly Dictionary<string, int[]> planetDictionary;

	public static readonly HashSet<int> khiGasMapSet;

	public static readonly HashSet<int> manhVoBTMapSet;

	public static readonly HashSet<int> futureMapSet;

	public static readonly int[] idMapsNamek;

	public static readonly int[] idMapsXayda;

	public static readonly int[] idMapsTraiDat;

	public static readonly int[] idMapsTuongLai;

	public static readonly int[] idMapsCold;

	public static readonly int[] idMapsNappa;

	public static readonly int[] idMapsThapleo;

	public static readonly int[] idMapsManhVoBT;

	public static readonly int[] idMapsKhiGas;

	public static readonly int[] idMapsKhac;

	public const long POWER_REQUIREMENT_40B = 40000000000L;

	public const long POWER_REQUIREMENT_60B = 60000000000L;

	public const int CLAN_MAP_START = 53;

	public const int CLAN_MAP_END = 62;

	public const int COLD_MAP_START = 105;

	public const int COLD_MAP_END = 110;

	public const int NRD_MAP_START = 85;

	public const int NRD_MAP_END = 91;

	public const int SPECIAL_MAP_START = 153;

	public const int SPECIAL_MAP_END = 159;

	static DataXmap()
	{
		linkMaps = new Dictionary<int, List<NextMap>>();
		planetDictionary = new Dictionary<string, int[]>();
		khiGasMapSet = new HashSet<int>();
		manhVoBTMapSet = new HashSet<int>();
		futureMapSet = new HashSet<int>();
		idMapsNamek = new int[15]
		{
			43, 22, 7, 8, 9, 11, 12, 13, 10, 31,
			32, 33, 34, 43, 25
		};
		idMapsXayda = new int[20]
		{
			44, 23, 14, 15, 16, 17, 18, 20, 19, 35,
			36, 37, 38, 52, 44, 26, 84, 113, 127, 129
		};
		idMapsTraiDat = new int[26]
		{
			42, 21, 0, 1, 2, 3, 4, 5, 6, 27,
			28, 29, 30, 47, 42, 24, 53, 58, 59, 60,
			61, 62, 55, 56, 54, 57
		};
		idMapsTuongLai = new int[10] { 102, 92, 93, 94, 96, 97, 98, 99, 100, 103 };
		idMapsCold = new int[6] { 109, 108, 107, 110, 106, 105 };
		idMapsNappa = new int[23]
		{
			68, 69, 70, 71, 72, 64, 65, 63, 66, 67,
			73, 74, 75, 76, 77, 81, 82, 83, 79, 80,
			131, 132, 133
		};
		idMapsThapleo = new int[7] { 46, 45, 48, 50, 154, 155, 166 };
		idMapsManhVoBT = new int[5] { 153, 156, 157, 158, 159 };
		idMapsKhiGas = new int[5] { 149, 147, 152, 151, 148 };
		idMapsKhac = new int[4] { 181, 139, 140, 126 };
		InitializeHashSets();
		LoadLinkMapsXmap();
		LoadNPCLinkMapsXmap();
		AddPlanetXmap();
	}

	private static void InitializeHashSets()
	{
		int[] array = idMapsKhiGas;
		foreach (int item in array)
		{
			khiGasMapSet.Add(item);
		}
		int[] array2 = idMapsManhVoBT;
		foreach (int item2 in array2)
		{
			manhVoBTMapSet.Add(item2);
		}
		int[] array3 = idMapsTuongLai;
		foreach (int item3 in array3)
		{
			futureMapSet.Add(item3);
		}
	}

	private static void LoadLinkMapsXmap()
	{
		AddLinkMapsXmap(0, 21);
		AddLinkMapsXmap(1, 47);
		AddLinkMapsXmap(47, 111);
		AddLinkMapsXmap(2, 24);
		AddLinkMapsXmap(5, 29);
		AddLinkMapsXmap(7, 22);
		AddLinkMapsXmap(9, 25);
		AddLinkMapsXmap(13, 33);
		AddLinkMapsXmap(14, 23);
		AddLinkMapsXmap(16, 26);
		AddLinkMapsXmap(20, 37);
		AddLinkMapsXmap(39, 21);
		AddLinkMapsXmap(40, 22);
		AddLinkMapsXmap(41, 23);
		AddLinkMapsXmap(109, 105);
		AddLinkMapsXmap(109, 106);
		AddLinkMapsXmap(106, 107);
		AddLinkMapsXmap(108, 105);
		AddLinkMapsXmap(80, 105);
		AddLinkMapsXmap(3, 27, 28, 29, 30);
		AddLinkMapsXmap(11, 31, 32, 33, 34);
		AddLinkMapsXmap(17, 35, 36, 37, 38);
		AddLinkMapsXmap(109, 108, 107, 110, 106);
		AddLinkMapsXmap(47, 46, 45, 48);
		AddLinkMapsXmap(131, 132, 133);
		AddLinkMapsXmap(42, 0, 1, 2, 3, 4, 5, 6);
		AddLinkMapsXmap(43, 7, 8, 9, 11, 12, 13, 10);
		AddLinkMapsXmap(52, 44, 14, 15, 16, 17, 18, 20, 19);
		AddLinkMapsXmap(53, 58, 59, 60, 61, 62, 55, 56, 54, 57);
		AddLinkMapsXmap(68, 69, 70, 71, 72, 64, 65, 63, 66, 67, 73, 74, 75, 76, 77, 81, 82, 83, 79, 80);
		AddLinkMapsXmap(102, 92, 93, 94, 96, 97, 98, 99, 100, 103);
		AddLinkMapsXmap(153, 156, 157, 158, 159);
		AddLinkMapsXmap(46, 45, 48, 50, 154, 155, 166);
		AddLinkMapsXmap(149, 147, 152, 151, 148);
		AddLinkMapsXmap(139, 140);
		AddLinkMapsXmap(160, 161, 162, 163);
		AddLinkMapsXmap(84, 104);
	}

	private static void LoadNPCLinkMapsXmap()
	{
		AddNPCLinkMapsXmap(19, 68, 12, "Đến Nappa", "", "", walk: false, -1, -1, "Đồng ý");
		AddNPCLinkMapsXmap(19, 109, 12, "Đến Cold");
		AddPortalGroup(24, new int[3] { 25, 26, 84 }, 10, new int[3] { 0, 1, 2 });
		AddPortalGroup(25, new int[3] { 24, 26, 84 }, 11, new int[3] { 0, 1, 2 });
		AddPortalGroup(26, new int[3] { 24, 25, 84 }, 12, new int[3] { 0, 1, 2 });
		AddPortalGroup(84, new int[3] { 24, 25, 26 }, 10, new int[3]);
		AddNPCLinkMapsXmap(27, 102, 38, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 1);
		AddNPCLinkMapsXmap(28, 102, 38, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 1);
		AddNPCLinkMapsXmap(29, 102, 38, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 1);
		AddNPCLinkMapsXmap(102, 27, 38, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 1);
		AddNPCLinkMapsXmap(27, 53, 25, "Vào (miễn phí)", "", "", walk: false, -1, -1, "Tham Gia", "OK");
		AddNPCLinkMapsXmap(52, 127, 44, "OK");
		AddNPCLinkMapsXmap(52, 129, 23, "Đại Hội Võ Thuật Lần thứ 23");
		AddNPCLinkMapsXmap(52, 113, 23, "Giải Siêu Hạng");
		AddNPCLinkMapsXmap(113, 52, 22, "Về Đại Hội Võ Thuật");
		AddNPCLinkMapsXmap(127, 52, 44, "Về Đại Hội Võ Thuật");
		AddNPCLinkMapsXmap(129, 52, 23, "Về Đại Hội Võ Thuật");
		AddNPCLinkMapsXmap(68, 19, 12, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 0);
		AddNPCLinkMapsXmap(80, 131, 60, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 0);
		AddNPCLinkMapsXmap(131, 80, 60, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 1);
		AddNPCLinkMapsXmap(5, 153, 13, "Nói chuyện", "Về khu vực bang");
		AddNPCLinkMapsXmap(153, 5, 10, "Đảo Kame");
		AddNPCLinkMapsXmap(153, 156, 47, "OK");
		AddNPCLinkMapsXmap(45, 48, 19, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 3);
		AddNPCLinkMapsXmap(48, 45, 20, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 3, 0);
		AddNPCLinkMapsXmap(48, 50, 20, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 3, 1);
		AddNPCLinkMapsXmap(50, 48, 44, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 0);
		AddNPCLinkMapsXmap(50, 154, 44, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 1);
		AddNPCLinkMapsXmap(154, 50, 55, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 0);
		AddNPCLinkMapsXmap(154, 155, 44, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 1);
		AddNPCLinkMapsXmap(155, 154, 44, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 0);
		AddNPCLinkMapsXmap(155, 166, -1, "", "", "", walk: true, 1400, 600);
		AddNPCLinkMapsXmap(46, 47, -1, "", "", "", walk: true, 80, 700);
		AddNPCLinkMapsXmap(45, 46, -1, "", "", "", walk: true, 80, 700);
		AddNPCLinkMapsXmap(46, 45, -1, "", "", "", walk: true, 380, 90);
		AddNPCLinkMapsXmap(0, 149, 67, "OK", "", "", walk: false, -1, -1, "Đồng ý");
		AddNPCLinkMapsXmap(24, 139, 63, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 0);
		AddNPCLinkMapsXmap(139, 24, 63, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", 0);
		AddNPCLinkMapsXmap(126, 19, 53, "OK");
		AddNPCLinkMapsXmap(19, 126, 53, "OK");
		AddNPCLinkMapsXmap(52, 181, 44, "Bình hút năng lượng", "OK");
		AddNPCLinkMapsXmap(181, 52, 44, "Về nhà");
	}

	private static void AddPlanetXmap()
	{
		planetDictionary.Add("Trái đất", idMapsTraiDat);
		planetDictionary.Add("Namek", idMapsNamek);
		planetDictionary.Add("Xayda", idMapsXayda);
		planetDictionary.Add("Fide", idMapsNappa);
		planetDictionary.Add("Tương lai", idMapsTuongLai);
		planetDictionary.Add("Cold", idMapsCold);
		planetDictionary.Add("Tháp leo", idMapsThapleo);
		planetDictionary.Add("Khuc vực bang", idMapsManhVoBT);
		planetDictionary.Add("Khi Gas", idMapsKhiGas);
		planetDictionary.Add("Map Khác", idMapsKhac);
	}

	public static void AddLinkMapsXmap(params int[] link)
	{
		for (int i = 0; i < link.Length; i++)
		{
			if (!linkMaps.ContainsKey(link[i]))
			{
				linkMaps.Add(link[i], new List<NextMap>());
			}
			if (i > 0)
			{
				linkMaps[link[i]].Add(new NextMap(link[i - 1], -1, ""));
			}
			if (i < link.Length - 1)
			{
				linkMaps[link[i]].Add(new NextMap(link[i + 1], -1, ""));
			}
		}
	}

	public static void AddNPCLinkMapsXmap(int currentMapID, int nextMapID, int npcID = -1, string selectName = "", string selectName2 = "", string selectName3 = "", bool walk = false, int x = -1, int y = -1, string selectNamePhu = "", string selectNamePhu2 = "", string selectNamePhu3 = "", string selectNamePhuVIP2 = "", string selectNamePhu2VIP2 = "", string selectNamePhu3VIP2 = "", int indexNpc = -1, int indexNpc2 = -1, int indexNpc3 = -1)
	{
		if (!linkMaps.ContainsKey(currentMapID))
		{
			linkMaps.Add(currentMapID, new List<NextMap>());
		}
		linkMaps[currentMapID].Add(new NextMap(nextMapID, npcID, selectName, selectName2, selectName3, walk, x, y, selectNamePhu, selectNamePhu2, selectNamePhu3, selectNamePhuVIP2, selectNamePhu2VIP2, selectNamePhu3VIP2, indexNpc, indexNpc2, indexNpc3));
	}

	private static void AddPortalGroup(int fromMap, int[] toMaps, int npcID, int[] indices)
	{
		for (int i = 0; i < toMaps.Length; i++)
		{
			int indexNpc = indices[i];
			AddNPCLinkMapsXmap(fromMap, toMaps[i], npcID, "", "", "", walk: false, -1, -1, "", "", "", "", "", "", indexNpc);
		}
	}

	public static bool IsNRDMap(int mapID)
	{
		return mapID >= 85 && mapID <= 91;
	}

	public static bool IsFutureMap(int mapID)
	{
		return futureMapSet.Contains(mapID);
	}
}
