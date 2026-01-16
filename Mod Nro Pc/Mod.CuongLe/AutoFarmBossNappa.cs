using System;
using Xmap;
using UnityEngine;

namespace Mod.CuongLe;

public class AutoFarmBossNappa
{
    #region Enums
    private enum BossType
    {
        Kuku = 0,
        MapDauDinh = 1,
        Rambo = 2
    }

    private enum FarmState
    {
        Initialize = 0,
        WaitingForMapLoad = 1,
        XmapToMap = 2,
        InitializeZones = 3,
        RequestingZoneChange = 4,
        WaitingForZoneLoad = 41,
        CheckingForBoss = 5,
        MonitoringBossHealth = 51,
        FightingBoss = 6,
        PickingUpItems = 61,
        WaitingBeforeNextZone = 7
    }

    private enum MapRange
    {
        KukuStart = 68,
        KukuEnd = 72,
        MapDauDinhStart = 64,
        MapDauDinhEnd = 67,
        RamboStart = 73,
        RamboEnd = 77
    }
    #endregion

    #region Constants
    private const long BOSS_NO_DAMAGE_TIMEOUT_MS = 10000L;
    private const int MAX_PICK_ATTEMPTS = 5;
    private const long PICK_ITEM_DELAY_MS = 800L;
    private const long ZONE_CHANGE_DELAY_MS = 1200L;
    private const long MAP_LOAD_DELAY_MS = 1500L;
    private const long BOSS_FIGHT_CHECK_DELAY_MS = 2000L;
    private const long HP_CHECK_INTERVAL_MS = 2000L;
    private const long HP_STUCK_CHECK_INTERVAL_MS = 3000L;
    private const long WAIT_AFTER_BOSS_DEATH_MS = 2000L;
    private const int MAX_CONSECUTIVE_NO_DAMAGE = 5;
    private const int MAX_CONSECUTIVE_NO_DAMAGE_IN_FIGHT = 3;
    private const int DEFAULT_START_ZONE = 2;
    private const int GANG_THIEN_SU_ITEM_ID = 1070;

    private static readonly string[] BOSS_NAMES = { "Kuku", "Mập đầu đinh", "Rambo" };
    #endregion

    #region Singleton
    private static AutoFarmBossNappa _instance;

    public static AutoFarmBossNappa getInstance()
    {
        if (_instance == null)
        {
            _instance = new AutoFarmBossNappa();
        }
        return _instance;
    }
    #endregion

    #region Public Fields
    public static bool DoSatBossNapa;
    public static int typeBoss;
    public static string statusBossNappa;
    #endregion

    #region Private State Fields
    private static FarmState currentState;
    private static long stateTimer;
    private static int currentMapId;
    private static int targetZone;
    #endregion

    #region Boss Tracking Fields
    private static long bossEntryTime;
    private static bool bossDamaged;
    private static long lastBossHp;
    private static long lastBossHpCheckTime;
    private static int consecutiveNoDamageCount;
    #endregion

    #region Item Pickup Fields
    private static long lastPickItemTime;
    private static int pickItemAttempts;
    private static long bossDeathTime;
    #endregion

    #region Flags
    private static bool mapInitialized;
    private static bool resumeFromDeathOrDisconnect;
    #endregion

    #region Initialization
    static AutoFarmBossNappa()
    {
        ResetAllState();
    }

    private static void ResetAllState()
    {
        statusBossNappa = "";
        currentState = FarmState.Initialize;
        stateTimer = 0L;
        currentMapId = (int)MapRange.KukuStart;
        targetZone = DEFAULT_START_ZONE;
        typeBoss = 0;
        DoSatBossNapa = false;

        ResetBossTracking();
        ResetItemPickup();
        ResetFlags();
    }

    private static void ResetBossTracking()
    {
        bossEntryTime = 0L;
        bossDamaged = false;
        lastBossHp = -1L;
        lastBossHpCheckTime = 0L;
        consecutiveNoDamageCount = 0;
        bossDeathTime = 0L;
    }

    private static void ResetItemPickup()
    {
        lastPickItemTime = 0L;
        pickItemAttempts = 0;
    }

    private static void ResetFlags()
    {
        mapInitialized = false;
        resumeFromDeathOrDisconnect = false;
    }
    #endregion

    #region Main Update Loop
    public static void Update()
    {
        try
        {
            if (!DoSatBossNapa) return;

            if (HandlePlayerDeath()) return;
            if (HandlePlayerLost()) return;

            ProcessCurrentState();
        }
        catch
        {
            Stop();
            statusBossNappa = "Lỗi hệ thống - Đã dừng";
            GameScr.info1.addInfo("Lỗi update auto farm boss nappa");
        }
    }

    private static bool HandlePlayerDeath()
    {
        if (!Char.myCharz().meDead) return false;

        if (GameCanvas.gameTick % 40 == 0)
        {
            statusBossNappa = "Đang hồi sinh...";
            Service.gI().returnTownFromDead();
            stateTimer = mSystem.currentTimeMillis() + MAP_LOAD_DELAY_MS;

            if (currentMapId > 0)
            {
                GoToStartMap();
                resumeFromDeathOrDisconnect = true;
            }
        }
        return true;
    }

    private static bool HandlePlayerLost()
    {
        if (currentState == FarmState.Initialize) return false;
        if (TileMap.mapID == currentMapId || MainXmapCL.isXmaping) return false;

        statusBossNappa = "Quay lại map boss (lạc đường)";
        GoToStartMap();
        resumeFromDeathOrDisconnect = true;
        return true;
    }

    private static void ProcessCurrentState()
    {
        switch (currentState)
        {
            case FarmState.Initialize:
                HandleInitialize();
                break;
            case FarmState.WaitingForMapLoad:
                HandleWaitingForMapLoad();
                break;
            case FarmState.XmapToMap:
                HandleXmapToMap();
                break;
            case FarmState.InitializeZones:
                HandleInitializeZones();
                break;
            case FarmState.RequestingZoneChange:
                HandleRequestingZoneChange();
                break;
            case FarmState.WaitingForZoneLoad:
                HandleWaitingForZoneLoad();
                break;
            case FarmState.CheckingForBoss:
                HandleCheckingForBoss();
                break;
            case FarmState.MonitoringBossHealth:
                HandleMonitoringBossHealth();
                break;
            case FarmState.FightingBoss:
                HandleFightingBoss();
                break;
            case FarmState.PickingUpItems:
                HandlePickingUpItems();
                break;
            case FarmState.WaitingBeforeNextZone:
                HandleWaitingBeforeNextZone();
                break;
        }
    }
    #endregion

    #region State Handlers
    private static void HandleInitialize()
    {
        statusBossNappa = "Khởi tạo hệ thống";
        InitializeStartMap();
    }

    private static void HandleWaitingForMapLoad()
    {
        if (TileMap.mapID != currentMapId)
        {
            statusBossNappa = "Đang di chuyển đến map boss";
            GoToStartMap();
        }
        else if (!AutoBossCL.offPaintZone)
        {
            statusBossNappa = "Khởi tạo danh sách khu";
            InitializeZones();
            currentState = FarmState.InitializeZones;
        }
    }

    private static void HandleXmapToMap()
    {
        if (!MainXmapCL.isXmaping)
        {
            statusBossNappa = "Mở UI Zone";
            AutoBossCL.offPaintZone = true;
            Service.gI().openUIZone();
            currentState = FarmState.WaitingForMapLoad;
        }
        else
        {
            statusBossNappa = "Đang Xmap đến map boss";
        }
    }

    private static void HandleInitializeZones()
    {
        if (targetZone <= AutoBossCL.CountZoneMap && !Char.myCharz().meDead)
        {
            statusBossNappa = $"Chuẩn bị đổi khu {targetZone}";
            RequestZoneChange(targetZone);
            currentState = FarmState.RequestingZoneChange;
        }
        else
        {
            statusBossNappa = "Hết khu, chuyển map tiếp theo";
            MoveToNextMap();
        }
    }

    private static void HandleRequestingZoneChange()
    {
        long currentTime = mSystem.currentTimeMillis();

        if (currentTime < stateTimer)
        {
            statusBossNappa = $"Đang đổi khu {targetZone}...";
            return;
        }

        if (TileMap.zoneID == targetZone)
        {
            statusBossNappa = $"Đã vào khu {targetZone}";
            stateTimer = currentTime + MAP_LOAD_DELAY_MS;
            currentState = FarmState.WaitingForZoneLoad;
        }
        else
        {
            statusBossNappa = $"Đang chờ vào khu {targetZone}";
            RequestZoneChange(targetZone);
        }
    }

    private static void HandleWaitingForZoneLoad()
    {
        if (mSystem.currentTimeMillis() >= stateTimer)
        {
            statusBossNappa = "Map đã load, bắt đầu kiểm tra boss";
            currentState = FarmState.CheckingForBoss;
        }
        else
        {
            statusBossNappa = $"Đang đợi map load (Khu {targetZone})...";
        }
    }

    private static void HandleCheckingForBoss()
    {
        statusBossNappa = "Kiểm tra boss trong khu";

        if (IsBossPresent())
        {
            OnBossFound();
        }
        else
        {
            OnBossNotFound();
        }
    }

    private static void HandleMonitoringBossHealth()
    {
        statusBossNappa = $"Đang theo dõi HP boss (Khu {TileMap.zoneID})";

        Char boss = GetFirstBossInMap();
        if (boss == null)
        {
            OnBossDisappeared();
            return;
        }

        long currentTime = mSystem.currentTimeMillis();
        if (currentTime - lastBossHpCheckTime < HP_CHECK_INTERVAL_MS)
        {
            statusBossNappa = $"Đang theo dõi boss (HP: {boss.cHP})";
            return;
        }

        CheckBossHealthChange(boss, currentTime);
        CheckForPhantomBoss(currentTime);
    }

    private static void HandleFightingBoss()
    {
        long currentTime = mSystem.currentTimeMillis();

        if (currentTime < stateTimer)
        {
            statusBossNappa = $"Đang đánh boss (Khu {TileMap.zoneID})";
            return;
        }

        if (IsBossPresent())
        {
            CheckBossFightProgress(currentTime);
            stateTimer = currentTime + BOSS_FIGHT_CHECK_DELAY_MS;
            statusBossNappa = "Boss còn sống, tiếp tục đánh";
        }
        else
        {
            statusBossNappa = "Boss đã chết, chờ 2 giây rồi kiểm tra item";
            bossDeathTime = currentTime;
            ResetItemPickup();
            currentState = FarmState.PickingUpItems;
        }
    }

    private static void HandlePickingUpItems()
    {
        long currentTime = mSystem.currentTimeMillis();

        // Chờ 2 giây sau khi boss chết trước khi bắt đầu nhặt item
        if (currentTime - bossDeathTime < WAIT_AFTER_BOSS_DEATH_MS)
        {
            statusBossNappa = $"Chờ boss respawn/tải item ({currentTime - bossDeathTime}ms)";
            return;
        }

        if (!HasGangThienSuItems())
        {
            statusBossNappa = "Không có mảnh thiên sứ, chuyển khu";
            OnItemPickupComplete();
            return;
        }

        if (currentTime - lastPickItemTime < PICK_ITEM_DELAY_MS)
        {
            statusBossNappa = $"Đang nhặt mảnh thiên sứ ({pickItemAttempts}/{MAX_PICK_ATTEMPTS})";
            return;
        }

        if (PickAllGangThienSuItems())
        {
            lastPickItemTime = currentTime;
            pickItemAttempts++;
            statusBossNappa = $"Đã nhặt mảnh thiên sứ (lần {pickItemAttempts}/{MAX_PICK_ATTEMPTS})";

            if (pickItemAttempts >= MAX_PICK_ATTEMPTS)
            {
                statusBossNappa = "Đã nhặt đủ số lần, chuyển khu";
                OnItemPickupComplete();
            }
        }
        else
        {
            statusBossNappa = "Đã nhặt hết mảnh thiên sứ, chuyển khu";
            OnItemPickupComplete();
        }
    }

    private static void HandleWaitingBeforeNextZone()
    {
        if (mSystem.currentTimeMillis() >= stateTimer)
        {
            if (targetZone < AutoBossCL.CountZoneMap)
            {
                targetZone++;
                statusBossNappa = $"Chuyển sang khu {targetZone}";
                currentState = FarmState.InitializeZones;
            }
            else
            {
                statusBossNappa = "Hết khu, chuyển map";
                MoveToNextMap();
            }
        }
        else
        {
            statusBossNappa = $"Đang chờ... (Khu {targetZone})";
        }
    }
    #endregion

    #region Boss Detection & Tracking
    private static Char GetFirstBossInMap()
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char character = (Char)GameScr.vCharInMap.elementAt(i);
            if (IsValidBoss(character))
            {
                return character;
            }
        }
        return null;
    }

    private static bool IsBossPresent()
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char character = (Char)GameScr.vCharInMap.elementAt(i);
            if (IsValidBossInBounds(character))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsValidBoss(Char character)
    {
        if (character == null || string.IsNullOrEmpty(character.cName)) return false;
        if (character.cHP <= 0 || character.isPet || character.isMiniPet) return false;

        return StartsWithBossName(character.cName);
    }

    private static bool IsValidBossInBounds(Char character)
    {
        if (!IsValidBoss(character)) return false;
        if (!char.IsUpper(character.cName[0])) return false;
        if (character.cx > TileMap.GetMapEndX() - 10) return false;
        if (character.cy > TileMap.GetMapEndY() - 10) return false;

        return true;
    }

    private static bool StartsWithBossName(string name)
    {
        foreach (string bossName in BOSS_NAMES)
        {
            if (name.StartsWith(bossName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsBossNappa(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        return StartsWithBossName(name);
    }

    public static bool checkBossNappa() => IsBossPresent();
    #endregion

    #region Boss Events
    private static void OnBossFound()
    {
        Char boss = GetFirstBossInMap();
        if (boss == null)
        {
            OnBossNotFound();
            return;
        }

        long currentTime = mSystem.currentTimeMillis();
        lastBossHp = boss.cHP;
        lastBossHpCheckTime = currentTime;
        bossEntryTime = currentTime;
        bossDamaged = false;
        consecutiveNoDamageCount = 0;

        ClearPlayerFocus();

        statusBossNappa = $"Tìm thấy boss {boss.cName} (HP: {boss.cHP})";
        currentState = FarmState.MonitoringBossHealth;
    }

    private static void OnBossNotFound()
    {
        ResetBossTracking();
        long delay = GameScr.canAutoPlay ? 5200 : 10500;
        stateTimer = mSystem.currentTimeMillis() + delay;
        statusBossNappa = "Không có boss, chờ chuyển khu tiếp theo";
        currentState = FarmState.WaitingBeforeNextZone;
    }

    private static void OnBossDisappeared()
    {
        statusBossNappa = "Boss biến mất khi theo dõi";
        ResetBossTracking();
        MoveToNextZoneOrMap();
    }

    private static void OnItemPickupComplete()
    {
        ResetBossTracking();
        AutoBossCL.listBossTrongKhu.Clear();
        MoveToNextZoneOrMap();
    }
    #endregion

    #region Boss Health Monitoring
    private static void CheckBossHealthChange(Char boss, long currentTime)
    {
        if (boss.cHP < lastBossHp)
        {
            OnBossTakingDamage(boss, currentTime);
        }
        else if (boss.cHP == lastBossHp)
        {
            OnBossHealthStagnant(boss, currentTime);
        }
        else
        {
            OnBossHealthIncreased(boss, currentTime);
        }
    }

    private static void OnBossTakingDamage(Char boss, long currentTime)
    {
        bossDamaged = true;
        consecutiveNoDamageCount = 0;
        lastBossHp = boss.cHP;
        lastBossHpCheckTime = currentTime;
        stateTimer = currentTime + 2500;
        statusBossNappa = $"Boss đang bị đánh (HP: {boss.cHP})";
        currentState = FarmState.FightingBoss;
    }

    private static void OnBossHealthStagnant(Char boss, long currentTime)
    {
        consecutiveNoDamageCount++;
        lastBossHpCheckTime = currentTime;
        statusBossNappa = $"Theo dõi boss - HP không đổi lần {consecutiveNoDamageCount} (HP: {boss.cHP})";
    }

    private static void OnBossHealthIncreased(Char boss, long currentTime)
    {
        lastBossHp = boss.cHP;
        lastBossHpCheckTime = currentTime;
        consecutiveNoDamageCount = 0;
    }

    private static void CheckForPhantomBoss(long currentTime)
    {
        bool timeoutReached = currentTime - bossEntryTime >= BOSS_NO_DAMAGE_TIMEOUT_MS;
        bool tooManyNoDamageChecks = consecutiveNoDamageCount >= MAX_CONSECUTIVE_NO_DAMAGE;

        if ((timeoutReached || tooManyNoDamageChecks) && !bossDamaged)
        {
            statusBossNappa = "Boss ảo hoặc không thể đánh, bỏ qua khu";
            ResetBossTracking();
            MoveToNextZoneOrMap();
        }
    }

    private static void CheckBossFightProgress(long currentTime)
    {
        Char boss = GetFirstBossInMap();
        if (boss == null) return;

        if (currentTime - lastBossHpCheckTime < HP_STUCK_CHECK_INTERVAL_MS) return;

        if (boss.cHP < lastBossHp)
        {
            consecutiveNoDamageCount = 0;
            lastBossHp = boss.cHP;
            lastBossHpCheckTime = currentTime;
        }
        else if (boss.cHP == lastBossHp)
        {
            consecutiveNoDamageCount++;
            lastBossHpCheckTime = currentTime;

            if (consecutiveNoDamageCount >= MAX_CONSECUTIVE_NO_DAMAGE_IN_FIGHT)
            {
                statusBossNappa = "Boss kẹt/ảo khi đánh, chuyển khu";
                ResetBossTracking();
                MoveToNextZoneOrMap();
            }
        }
        else
        {
            lastBossHp = boss.cHP;
            lastBossHpCheckTime = currentTime;
            consecutiveNoDamageCount = 0;
        }
    }
    #endregion

    #region Item Management
    private static bool HasGangThienSuItems()
    {
        for (int i = 0; i < GameScr.vItemMap.size(); i++)
        {
            ItemMap item = (ItemMap)GameScr.vItemMap.elementAt(i);
            if (IsGangThienSuItem(item))
            {
                return true;
            }
        }
        return false;
    }

    private static bool PickAllGangThienSuItems()
    {
        for (int i = 0; i < GameScr.vItemMap.size(); i++)
        {
            ItemMap item = (ItemMap)GameScr.vItemMap.elementAt(i);
            if (IsGangThienSuItem(item))
            {
                TeleportAndPickItem(item);
                return true;
            }
        }
        return false;
    }

    private static bool IsGangThienSuItem(ItemMap item)
    {
        if (item == null) return false;
        return item.playerId == Char.myCharz().charID ||
               item.template.id == GANG_THIEN_SU_ITEM_ID;
    }

    private static void TeleportAndPickItem(ItemMap item)
    {
        MainXmapCL.TeleportTo(item.x, item.y);
        Service.gI().pickItem(item.itemMapID);
    }
    #endregion

    #region Map & Zone Navigation
    private static void InitializeStartMap()
    {
        BossType bossType = (BossType)typeBoss;

        switch (bossType)
        {
            case BossType.Kuku:
                currentMapId = UnityEngine.Random.Range((int)MapRange.KukuStart, (int)MapRange.KukuEnd + 1);
                statusBossNappa = $"Chọn map Kuku ({currentMapId})";
                break;
            case BossType.MapDauDinh:
                currentMapId = UnityEngine.Random.Range((int)MapRange.MapDauDinhStart, (int)MapRange.MapDauDinhEnd + 1);
                statusBossNappa = $"Chọn map Mập đầu đinh ({currentMapId})";
                break;
            case BossType.Rambo:
                currentMapId = UnityEngine.Random.Range((int)MapRange.RamboStart, (int)MapRange.RamboEnd + 1);
                statusBossNappa = $"Chọn map Rambo ({currentMapId})";
                break;
            default:
                currentMapId = (int)MapRange.KukuStart;
                statusBossNappa = "Chọn map mặc định (68)";
                break;
        }

        ConfigureAutoSettings();
        ResetFlags();
        consecutiveNoDamageCount = 0;
        currentState = FarmState.WaitingForMapLoad;
    }

    private static void ConfigureAutoSettings()
    {
        AutoTrainCL.TuMoTDLT();
        AutoBossCL.aGimBoss = true;
        AutoBossCL.AutoteleBoss = true;
        AutoBossCL.tanCongBoss = true;
        ModProCL.tieuDietNguoiBatCo = false;
    }

    private static void InitializeZones()
    {
        AutoBossCL.CountZoneMap = (GameScr.gI().zones != null)
            ? GameScr.gI().zones.Length - 1
            : 0;

        statusBossNappa = $"Khởi tạo: {AutoBossCL.CountZoneMap} khu";

        if (mapInitialized) return;

        if (resumeFromDeathOrDisconnect)
        {
            if (targetZone < DEFAULT_START_ZONE || targetZone > AutoBossCL.CountZoneMap)
            {
                targetZone = DEFAULT_START_ZONE;
            }
            statusBossNappa = $"Tiếp tục từ khu {targetZone}";
            resumeFromDeathOrDisconnect = false;
        }
        else
        {
            targetZone = DEFAULT_START_ZONE;
            statusBossNappa = "Bắt đầu từ khu 2";
        }

        mapInitialized = true;
    }

    private static void GoToStartMap()
    {
        MainXmapCL.StartGoToMap(currentMapId);
        statusBossNappa = $"Đang Xmap đến map {currentMapId}";
        currentState = FarmState.XmapToMap;
        mapInitialized = false;
    }

    private static void RequestZoneChange(int zone)
    {
        Service.gI().requestChangeZone(zone, -1);
        stateTimer = mSystem.currentTimeMillis() + ZONE_CHANGE_DELAY_MS;
        statusBossNappa = $"Request đổi khu {zone}";
    }

    private static void MoveToNextZoneOrMap()
    {
        if (TileMap.zoneID < AutoBossCL.CountZoneMap)
        {
            targetZone = TileMap.zoneID + 1;
            currentState = FarmState.InitializeZones;
        }
        else
        {
            MoveToNextMap();
        }
    }

    private static void MoveToNextMap()
    {
        BossType bossType = (BossType)typeBoss;

        switch (bossType)
        {
            case BossType.Kuku:
                currentMapId = (currentMapId >= (int)MapRange.KukuEnd)
                    ? (int)MapRange.KukuStart
                    : currentMapId + 1;
                statusBossNappa = $"Chuyển map Kuku tiếp theo ({currentMapId})";
                break;
            case BossType.MapDauDinh:
                currentMapId = (currentMapId >= (int)MapRange.MapDauDinhEnd)
                    ? (int)MapRange.MapDauDinhStart
                    : currentMapId + 1;
                statusBossNappa = $"Chuyển map Mập đầu đinh tiếp theo ({currentMapId})";
                break;
            case BossType.Rambo:
                currentMapId = (currentMapId >= (int)MapRange.RamboEnd)
                    ? (int)MapRange.RamboStart
                    : currentMapId + 1;
                statusBossNappa = $"Chuyển map Rambo tiếp theo ({currentMapId})";
                break;
        }

        targetZone = DEFAULT_START_ZONE;
        ResetFlags();
        currentState = FarmState.WaitingForMapLoad;
    }
    #endregion

    #region Utility Methods
    private static void ClearPlayerFocus()
    {
        Char.myCharz().mobFocus = null;
        Char.myCharz().itemFocus = null;
        Char.myCharz().npcFocus = null;
    }
    #endregion

    #region Public Control Methods
    public static void Stop()
    {
        MainXmapCL.FinishXmap();
        DoSatBossNapa = false;
        AutoBossCL.tanCongBoss = false;
        AutoBossCL.aGimBoss = false;
        AutoBossCL.AutoteleBoss = false;
        currentState = FarmState.Initialize;
        targetZone = DEFAULT_START_ZONE;
        consecutiveNoDamageCount = 0;
        statusBossNappa = "Đã dừng auto farm boss Napa";
    }
    #endregion
}