using System;
using Xmap;
using UnityEngine;

namespace Mod.CuongLe;

public class AutoFarmBossNappa
{
	private static AutoFarmBossNappa _Instance;

	public static bool DoSatBossNapa;

	public static int typeBoss;

	public static string statusBossNappa;

	private static int napaState;

	private static long napaTimer;

	private static int startMapNapa;

	private static int targetZoneNapa;

	private static long bossEntryTime;

	private static bool bossDamaged;

	private const long BOSS_NO_DAMAGE_TIMEOUT_MS = 10000L;

	private static bool napaMapInitialized;

	private static bool resumeFromDeathOrLac;

	private static long lastBossHp;

	private static long lastBossHpCheckTime;

	private static int consecutiveNoDamageCount;

	private static long lastPickItemTime;

	private static int pickItemAttempts;

	private const int MAX_PICK_ATTEMPTS = 5;

	private const long PICK_ITEM_DELAY = 800L;

	public static AutoFarmBossNappa getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new AutoFarmBossNappa();
		}
		return _Instance;
	}

	static AutoFarmBossNappa()
	{
		statusBossNappa = "";
		napaMapInitialized = false;
		resumeFromDeathOrLac = false;
		bossEntryTime = 0L;
		bossDamaged = false;
		lastBossHp = -1L;
		lastBossHpCheckTime = 0L;
		napaState = 0;
		napaTimer = 0L;
		startMapNapa = 68;
		targetZoneNapa = 2;
		typeBoss = 0;
		DoSatBossNapa = false;
		consecutiveNoDamageCount = 0;
		lastPickItemTime = 0L;
		pickItemAttempts = 0;
	}

	public static void Update()
	{
		try
		{
			if (!DoSatBossNapa)
			{
				return;
			}
			if (Char.myCharz().meDead)
			{
				if (GameCanvas.gameTick % 40 == 0)
				{
					statusBossNappa = "Đang hồi sinh...";
					Service.gI().returnTownFromDead();
					napaTimer = mSystem.currentTimeMillis() + 1500;
					if (startMapNapa > 0)
					{
						GoToStartMap();
						resumeFromDeathOrLac = true;
					}
				}
				return;
			}
			if (napaState > 0 && TileMap.mapID != startMapNapa && !MainXmapCL.isXmaping)
			{
				statusBossNappa = "Quay lại map boss (lạc đường)";
				GoToStartMap();
				resumeFromDeathOrLac = true;
				return;
			}
			switch (napaState)
			{
			case 0:
				statusBossNappa = "Khởi tạo hệ thống";
				InitStartMap();
				break;
			case 1:
				if (TileMap.mapID != startMapNapa)
				{
					statusBossNappa = "Đang di chuyển đến map boss";
					GoToStartMap();
				}
				else if (!AutoBossCL.offPaintZone)
				{
					statusBossNappa = "Khởi tạo danh sách khu";
					InitZones();
					napaState = 3;
				}
				break;
			case 2:
				if (!MainXmapCL.isXmaping)
				{
					statusBossNappa = "Mở UI Zone";
					AutoBossCL.offPaintZone = true;
					Service.gI().openUIZone();
					napaState = 1;
				}
				else
				{
					statusBossNappa = "Đang Xmap đến map boss";
				}
				break;
			case 3:
				if (targetZoneNapa <= AutoBossCL.CountZoneMap && !Char.myCharz().meDead)
				{
					statusBossNappa = $"Chuẩn bị đổi khu {targetZoneNapa}";
					RequestZone(targetZoneNapa);
					napaState = 4;
				}
				else
				{
					statusBossNappa = "Hết khu, chuyển map tiếp theo";
					NextMap();
				}
				break;
			case 4:
				if (mSystem.currentTimeMillis() >= napaTimer)
				{
					if (TileMap.zoneID == targetZoneNapa)
					{
						statusBossNappa = $"Đã vào khu {targetZoneNapa}";
						napaTimer = mSystem.currentTimeMillis() + 1500;
						napaState = 41;
					}
					else
					{
						statusBossNappa = $"Đang chờ vào khu {targetZoneNapa}";
						RequestZone(targetZoneNapa);
					}
				}
				else
				{
					statusBossNappa = $"Đang đổi khu {targetZoneNapa}...";
				}
				break;
			case 41:
				if (mSystem.currentTimeMillis() >= napaTimer)
				{
					statusBossNappa = "Map đã load, bắt đầu kiểm tra boss";
					napaState = 5;
				}
				else
				{
					statusBossNappa = $"Đang đợi map load (Khu {targetZoneNapa})...";
				}
				break;
			case 5:
				statusBossNappa = "Kiểm tra boss trong khu";
				if (checkBossNappa())
				{
					HandleBossFound();
					break;
				}
				lastBossHp = -1L;
				bossEntryTime = 0L;
				bossDamaged = false;
				consecutiveNoDamageCount = 0;
				napaTimer = mSystem.currentTimeMillis() + (GameScr.canAutoPlay ? 5200 : 10500);
				statusBossNappa = "Không có boss, chờ chuyển khu tiếp theo";
				napaState = 7;
				break;
			case 51:
				statusBossNappa = $"Đang theo dõi HP boss (Khu {TileMap.zoneID})";
				HandleBossMonitor();
				break;
			case 6:
				statusBossNappa = $"Đang đánh boss (Khu {TileMap.zoneID})";
				HandleBossFight();
				break;
			case 61:
				statusBossNappa = "Đang nhặt mảnh găng thiên sứ";
				HandlePickItems();
				break;
			case 7:
				if (mSystem.currentTimeMillis() >= napaTimer)
				{
					if (targetZoneNapa < AutoBossCL.CountZoneMap)
					{
						targetZoneNapa++;
						statusBossNappa = $"Chuyển sang khu {targetZoneNapa}";
						napaState = 3;
					}
					else
					{
						statusBossNappa = "Hết khu, chuyển map";
						NextMap();
					}
				}
				else
				{
					statusBossNappa = $"Đang chờ... (Khu {targetZoneNapa})";
				}
				break;
			}
		}
		catch
		{
			Stop();
			statusBossNappa = "Lỗi hệ thống - Đã dừng";
			GameScr.info1.addInfo("Lỗi update auto farm boss nappa");
		}
	}

	private static void GoToStartMap()
	{
		MainXmapCL.StartGoToMap(startMapNapa);
		statusBossNappa = $"Đang Xmap đến map {startMapNapa}";
		napaState = 2;
		napaMapInitialized = false;
	}

	private static void InitStartMap()
	{
		switch (typeBoss)
		{
		case 0:
			startMapNapa = UnityEngine.Random.Range(68, 73);
			statusBossNappa = $"Chọn map Kuku ({startMapNapa})";
			break;
		case 1:
			startMapNapa = UnityEngine.Random.Range(64, 68);
			statusBossNappa = $"Chọn map Mập đầu đinh ({startMapNapa})";
			break;
		case 2:
			startMapNapa = UnityEngine.Random.Range(73, 78);
			statusBossNappa = $"Chọn map Rambo ({startMapNapa})";
			break;
		default:
			startMapNapa = 68;
			statusBossNappa = "Chọn map mặc định (68)";
			break;
		}
		AutoTrainCL.TuMoTDLT();
		AutoBossCL.aGimBoss = (AutoBossCL.AutoteleBoss = (AutoBossCL.tanCongBoss = true));
		ModProCL.tieuDietNguoiBatCo = false;
		napaMapInitialized = false;
		resumeFromDeathOrLac = false;
		consecutiveNoDamageCount = 0;
		napaState = 1;
	}

	private static void InitZones()
	{
		AutoBossCL.CountZoneMap = ((GameScr.gI().zones != null) ? (GameScr.gI().zones.Length - 1) : 0);
		statusBossNappa = $"Khởi tạo: {AutoBossCL.CountZoneMap} khu";
		if (napaMapInitialized)
		{
			return;
		}
		if (resumeFromDeathOrLac)
		{
			if (targetZoneNapa < 2 || targetZoneNapa > AutoBossCL.CountZoneMap)
			{
				targetZoneNapa = 2;
			}
			statusBossNappa = $"Tiếp tục từ khu {targetZoneNapa}";
			resumeFromDeathOrLac = false;
		}
		else
		{
			targetZoneNapa = 2;
			statusBossNappa = "Bắt đầu từ khu 2";
		}
		napaMapInitialized = true;
	}

	private static void RequestZone(int zone)
	{
		Service.gI().requestChangeZone(zone, -1);
		napaTimer = mSystem.currentTimeMillis() + 1200;
		statusBossNappa = $"Request đổi khu {zone}";
	}

	private static void HandleBossFound()
	{
		long num = mSystem.currentTimeMillis();
		Char firstBossInMap = getFirstBossInMap();
		if (firstBossInMap == null)
		{
			lastBossHp = -1L;
			bossEntryTime = 0L;
			bossDamaged = false;
			consecutiveNoDamageCount = 0;
			napaTimer = num + (GameScr.canAutoPlay ? 5500 : 10500);
			statusBossNappa = "Boss biến mất, chờ chuyển khu";
			napaState = 7;
		}
		else
		{
			lastBossHp = firstBossInMap.cHP;
			lastBossHpCheckTime = num;
			bossEntryTime = num;
			bossDamaged = false;
			consecutiveNoDamageCount = 0;
			Char.myCharz().mobFocus = null;
			Char.myCharz().itemFocus = null;
			Char.myCharz().npcFocus = null;
			statusBossNappa = $"Tìm thấy boss {firstBossInMap.cName} (HP: {firstBossInMap.cHP})";
			napaState = 51;
		}
	}

	private static void HandleBossMonitor()
	{
		long num = mSystem.currentTimeMillis();
		Char firstBossInMap = getFirstBossInMap();
		if (firstBossInMap == null)
		{
			statusBossNappa = "Boss biến mất khi theo dõi";
			lastBossHp = -1L;
			bossEntryTime = 0L;
			bossDamaged = false;
			consecutiveNoDamageCount = 0;
			if (targetZoneNapa < AutoBossCL.CountZoneMap)
			{
				targetZoneNapa++;
				napaState = 3;
			}
			else
			{
				NextMap();
			}
			return;
		}
		if (num - lastBossHpCheckTime >= 2000)
		{
			if (firstBossInMap.cHP < lastBossHp)
			{
				bossDamaged = true;
				consecutiveNoDamageCount = 0;
				lastBossHp = firstBossInMap.cHP;
				lastBossHpCheckTime = num;
				napaTimer = num + 2500;
				statusBossNappa = $"Boss đang bị đánh (HP: {firstBossInMap.cHP})";
				napaState = 6;
			}
			else if (firstBossInMap.cHP == lastBossHp)
			{
				consecutiveNoDamageCount++;
				lastBossHpCheckTime = num;
				statusBossNappa = $"Theo dõi boss - HP không đổi lần {consecutiveNoDamageCount} (HP: {firstBossInMap.cHP})";
			}
			else
			{
				lastBossHp = firstBossInMap.cHP;
				lastBossHpCheckTime = num;
				consecutiveNoDamageCount = 0;
			}
		}
		else
		{
			statusBossNappa = $"Đang theo dõi boss (HP: {firstBossInMap.cHP})";
		}
		if ((num - bossEntryTime >= 10000 || consecutiveNoDamageCount >= 5) && !bossDamaged)
		{
			statusBossNappa = "Boss ảo hoặc không thể đánh, bỏ qua khu";
			lastBossHp = -1L;
			bossEntryTime = 0L;
			bossDamaged = false;
			consecutiveNoDamageCount = 0;
			if (targetZoneNapa < AutoBossCL.CountZoneMap)
			{
				targetZoneNapa++;
				napaState = 3;
			}
			else
			{
				NextMap();
			}
		}
	}

	private static void HandleBossFight()
	{
		if (mSystem.currentTimeMillis() < napaTimer)
		{
			statusBossNappa = $"Đang đánh boss (Khu {TileMap.zoneID})";
		}
		else if (checkBossNappa())
		{
			Char firstBossInMap = getFirstBossInMap();
			if (firstBossInMap != null)
			{
				long num = mSystem.currentTimeMillis();
				if (num - lastBossHpCheckTime >= 3000)
				{
					if (firstBossInMap.cHP < lastBossHp)
					{
						consecutiveNoDamageCount = 0;
						lastBossHp = firstBossInMap.cHP;
						lastBossHpCheckTime = num;
					}
					else if (firstBossInMap.cHP == lastBossHp)
					{
						consecutiveNoDamageCount++;
						lastBossHpCheckTime = num;
						if (consecutiveNoDamageCount >= 3)
						{
							statusBossNappa = "Boss kẹt/ảo khi đánh, chuyển khu";
							ResetBossState();
							MoveToNextZone();
							return;
						}
					}
					else
					{
						lastBossHp = firstBossInMap.cHP;
						lastBossHpCheckTime = num;
						consecutiveNoDamageCount = 0;
					}
				}
			}
			napaTimer = mSystem.currentTimeMillis() + 2000;
			statusBossNappa = "Boss còn sống, tiếp tục đánh";
		}
		else
		{
			statusBossNappa = "Boss đã chết, kiểm tra item";
			pickItemAttempts = 0;
			lastPickItemTime = 0L;
			napaState = 61;
		}
	}

	private static void HandlePickItems()
	{
		long num = mSystem.currentTimeMillis();
		if (!HasGangThienSuItems())
		{
			statusBossNappa = "Không còn mảnh găng, chuyển khu";
			ResetBossState();
			MoveToNextZone();
		}
		else if (num - lastPickItemTime < 800)
		{
			statusBossNappa = $"Đang nhặt mảnh găng ({pickItemAttempts}/{5})";
		}
		else if (PickAllItemsGangThienSu())
		{
			lastPickItemTime = num;
			pickItemAttempts++;
			statusBossNappa = $"Đã nhặt mảnh găng ({pickItemAttempts})";
			if (pickItemAttempts >= 5)
			{
				statusBossNappa = "Đã nhặt đủ số lần, chuyển khu";
				ResetBossState();
				MoveToNextZone();
			}
		}
		else
		{
			statusBossNappa = "Đã nhặt hết mảnh găng, chuyển khu";
			ResetBossState();
			MoveToNextZone();
		}
	}

	private static void ResetBossState()
	{
		lastBossHp = -1L;
		bossEntryTime = 0L;
		bossDamaged = false;
		consecutiveNoDamageCount = 0;
		AutoBossCL.listBossTrongKhu.Clear();
	}

	private static void MoveToNextZone()
	{
		if (TileMap.zoneID < AutoBossCL.CountZoneMap)
		{
			targetZoneNapa = TileMap.zoneID + 1;
			napaState = 3;
		}
		else
		{
			NextMap();
		}
	}

	private static bool HasGangThienSuItems()
	{
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			if (itemMap != null && (itemMap.playerId == Char.myCharz().charID || itemMap.template.id == 1070))
			{
				return true;
			}
		}
		return false;
	}

	private static void NextMap()
	{
		switch (typeBoss)
		{
		case 0:
			startMapNapa = ((startMapNapa >= 72) ? 68 : (startMapNapa + 1));
			statusBossNappa = $"Chuyển map Kuku tiếp theo ({startMapNapa})";
			break;
		case 1:
			startMapNapa = ((startMapNapa >= 67) ? 64 : (startMapNapa + 1));
			statusBossNappa = $"Chuyển map Mập đầu đinh tiếp theo ({startMapNapa})";
			break;
		case 2:
			startMapNapa = ((startMapNapa >= 77) ? 73 : (startMapNapa + 1));
			statusBossNappa = $"Chuyển map Rambo tiếp theo ({startMapNapa})";
			break;
		}
		targetZoneNapa = 2;
		napaMapInitialized = false;
		resumeFromDeathOrLac = false;
		napaState = 1;
	}

	private static Char getFirstBossInMap()
	{
		string[] array = new string[3] { "Kuku", "Mập đầu đinh", "Rambo" };
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (obj == null || obj.cName == null || string.IsNullOrEmpty(obj.cName) || obj.cHP <= 0 || obj.isPet || obj.isMiniPet)
			{
				continue;
			}
			string[] array2 = array;
			string[] array3 = array2;
			string[] array4 = array3;
			string[] array5 = array4;
			string[] array6 = array5;
			string[] array7 = array6;
			foreach (string value in array7)
			{
				if (obj.cName.StartsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return obj;
				}
			}
		}
		return null;
	}

	public static bool checkBossNappa()
	{
		string[] array = new string[3] { "Kuku", "Mập đầu đinh", "Rambo" };
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (obj == null || obj.cName == null || string.IsNullOrEmpty(obj.cName) || obj.isPet || obj.isMiniPet || obj.cHP <= 0 || obj.cx > TileMap.GetMapEndX() - 10 || obj.cy > TileMap.GetMapEndY() - 10 || !char.IsUpper(obj.cName[0]))
			{
				continue;
			}
			string[] array2 = array;
			string[] array3 = array2;
			string[] array4 = array3;
			string[] array5 = array4;
			string[] array6 = array5;
			string[] array7 = array6;
			foreach (string value in array7)
			{
				if (obj.cName.StartsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool PickAllItemsGangThienSu()
	{
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			if (itemMap != null && (itemMap.playerId == Char.myCharz().charID || itemMap.template.id == 1070))
			{
				MainXmapCL.TeleportTo(itemMap.x, itemMap.y);
				Service.gI().pickItem(itemMap.itemMapID);
				return true;
			}
		}
		return false;
	}

	public static bool IsBossNappa(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		return name.StartsWith("Kuku", StringComparison.OrdinalIgnoreCase) || name.StartsWith("Mập đầu đinh", StringComparison.OrdinalIgnoreCase) || name.StartsWith("Rambo", StringComparison.OrdinalIgnoreCase);
	}

	public static void Stop()
	{
		MainXmapCL.FinishXmap();
		DoSatBossNapa = false;
		AutoBossCL.tanCongBoss = false;
		AutoBossCL.aGimBoss = false;
		AutoBossCL.AutoteleBoss = false;
		napaState = 0;
		targetZoneNapa = 2;
		consecutiveNoDamageCount = 0;
		statusBossNappa = "Đã dừng auto farm boss Napa";
	}
}
