using System;
using System.Linq;
using Xmap;
using Mod.community;
using UnityEngine;

namespace Mod.CuongLe;

#region Enums
public enum QuestType
{
    TrainMonster,      // Nhiệm vụ đánh quái thường
    TrainGold,         // Nhiệm vụ train quái lấy vàng
    SuicideGold,       // Nhiệm vụ tự sát lụm vàng
    KillPlayer,        // Nhiệm vụ pem người
    Steal              // Nhiệm vụ ăn trộm (bỏ qua)
}

public enum AutoState
{
    Idle,
    NavigateToQuestGiver,
    ConfirmQuestGiver,
    WaitConfirmation,
    WaitForQuestSignal,
    RetryQuestSignal,
    CompleteQuest,
    CancelQuest_Confirm,
    CancelQuest_Wait,
    FinishQuest_Navigate,
    FinishQuest_Wait,
    ExecuteQuest_Navigate,
    ExecuteQuest_Execute,
    ExecuteQuest_Wait
}
#endregion

#region Quest Configuration
public class QuestConfig
{
    public QuestType Type { get; set; }
    public bool IsEnabled { get; set; }
    public int MapId { get; set; } = -1;
    public int ZoneId { get; set; } = -1;
    public int MobId { get; set; } = -1;
    public string MobName { get; set; }

    public QuestConfig(QuestType type)
    {
        Type = type;
        IsEnabled = true;
    }
}

public class QuestSettings
{
    public string Difficulty { get; set; } = "siêu khó";
    public QuestConfig TrainMonster { get; set; } = new QuestConfig(QuestType.TrainMonster);
    public QuestConfig TrainGold { get; set; } = new QuestConfig(QuestType.TrainGold);
    public QuestConfig SuicideGold { get; set; } = new QuestConfig(QuestType.SuicideGold);
    public QuestConfig KillPlayer { get; set; } = new QuestConfig(QuestType.KillPlayer);
    public bool UseGoldSuicideMode { get; set; } = false;
    public bool KepMode { get; set; } = false;

    public void LoadFromRMS()
    {
        Difficulty = Rms.loadRMSString("level");
        if (string.IsNullOrEmpty(Difficulty))
            Difficulty = "siêu khó";

        TrainGold.IsEnabled = Rms.loadRMSInt("nextnvVang") != 1;
        KillPlayer.IsEnabled = Rms.loadRMSInt("nextnvNguoi") != 1;
        TrainMonster.IsEnabled = Rms.loadRMSInt("nextnvQuai") != 1;
        UseGoldSuicideMode = Rms.loadRMSInt("ChooseTypeGod") == 1;
    }

    public void SaveToRMS()
    {
        Rms.saveRMSString("level", Difficulty);
        Rms.saveRMSInt("nextnvVang", TrainGold.IsEnabled ? 0 : 1);
        Rms.saveRMSInt("nextnvNguoi", KillPlayer.IsEnabled ? 0 : 1);
        Rms.saveRMSInt("nextnvQuai", TrainMonster.IsEnabled ? 0 : 1);
        Rms.saveRMSInt("ChooseTypeGod", UseGoldSuicideMode ? 1 : 0);
    }

    public QuestConfig GetConfig(QuestType type)
    {
        return type switch
        {
            QuestType.TrainMonster => TrainMonster,
            QuestType.TrainGold => TrainGold,
            QuestType.SuicideGold => SuicideGold,
            QuestType.KillPlayer => KillPlayer,
            _ => null
        };
    }
}
#endregion

#region Quest Handlers
public abstract class QuestHandler
{
    protected AutoboMongCL bot;
    protected QuestConfig config;

    public QuestHandler(AutoboMongCL bot, QuestConfig config)
    {
        this.bot = bot;
        this.config = config;
    }

    public abstract void OnNavigate();
    public abstract void OnExecute();
    public abstract void OnUpdate();

    protected void SetupAutoTrain()
    {
        AutoTrainCL.isGobackCoordinate = false;
        AutoTrainCL.isGoBack = true;
        AutoTrainCL.gobackMapID = TileMap.mapID;
        AutoTrainCL.gobackZoneID = TileMap.zoneID;
    }

    protected bool NavigateToMap(int mapId, int zoneId = -1)
    {
        if (TileMap.mapID != mapId)
        {
            if (!MainXmapCL.isXmaping)
            {
                MainXmapCL.StartGoToMap(mapId);
            }
            return false;
        }

        if (!MainXmapCL.isXmaping && zoneId > 0 && TileMap.zoneID != zoneId)
        {
            Service.gI().requestChangeZone(zoneId, -1);
            return false;
        }

        return !MainXmapCL.isXmaping;
    }
}

public class TrainMonsterHandler : QuestHandler
{
    public TrainMonsterHandler(AutoboMongCL bot, QuestConfig config) : base(bot, config) { }

    public override void OnNavigate()
    {
        int zoneId = (config.MapId >= 63 && config.MapId <= 83) ? 1 : AutoboMongCL.RandomZoneFrom2To14();

        if (NavigateToMap(config.MapId, TileMap.zoneID <= 1 ? zoneId : -1))
        {
            bot.TransitionTo(AutoState.ExecuteQuest_Execute, 0f);
        }
        else
        {
            bot.SetDelay(1f);
        }
    }

    public override void OnExecute()
    {
        SetupAutoTrain();
        AutoTrainCL.getInstance().perform(1, config.MobId);
        bot.TransitionTo(AutoState.ExecuteQuest_Wait, 1.2f);
    }

    public override void OnUpdate()
    {
        if (InfoMe.FinishBoMong)
        {
            bot.TransitionTo(AutoState.CompleteQuest, 0f);
            AutoboMongCL.StatusBoMong = "Nhận thưởng";
        }
    }
}

public class TrainGoldHandler : QuestHandler
{
    public TrainGoldHandler(AutoboMongCL bot, QuestConfig config) : base(bot, config) { }

    public override void OnNavigate()
    {
        int zoneId = (config.MapId == 10 || config.MapId == 68 || config.MapId == 77) ? 1 : AutoboMongCL.RandomZoneFrom2To14();

        if (NavigateToMap(config.MapId, TileMap.zoneID < 1 ? zoneId : -1))
        {
            bot.TransitionTo(AutoState.ExecuteQuest_Execute, 0f);
        }
        else
        {
            bot.SetDelay(1f);
        }
    }

    public override void OnExecute()
    {
        SetupAutoTrain();
        AutoPick.isAutoPick = true;
        MainXmapCL.isEatChicken = true;
        AutoPick.pickByList = 0;

        int mobId = config.MapId == 77 ? 53 : (config.MapId == 68 ? 39 : -1);
        AutoTrainCL.getInstance().perform(mobId != -1 ? 1 : 2, mobId == -1 ? null : (object)mobId);
        bot.TransitionTo(AutoState.ExecuteQuest_Wait, 1.2f);
    }

    public override void OnUpdate()
    {
        if (InfoMe.FinishBoMong)
        {
            bot.TransitionTo(AutoState.CompleteQuest, 0f);
            AutoboMongCL.StatusBoMong = "Nhận thưởng";
        }
    }
}

public class SuicideGoldHandler : QuestHandler
{
    private bool pickingGold = false;

    public SuicideGoldHandler(AutoboMongCL bot, QuestConfig config) : base(bot, config) { }

    public override void OnNavigate()
    {
        if (Char.myCharz().isDie || ModProCL.suicide)
            return;

        int zoneId = AutoboMongCL.RandomZoneFrom2To14();

        if (NavigateToMap(config.MapId, TileMap.zoneID <= 1 ? zoneId : -1))
        {
            SetupAutoTrain();
            bot.TransitionTo(AutoState.ExecuteQuest_Execute, 0f);
        }
        else
        {
            bot.SetDelay(1f);
        }
    }

    public override void OnExecute()
    {
        if (InfoMe.FinishBoMong)
        {
            ModProCL.suicide = false;
            bot.TransitionTo(AutoState.CompleteQuest, 0f);
            AutoboMongCL.StatusBoMong = "Nhận thưởng";
            return;
        }

        if (ModProCL.suicide || Char.myCharz().isDie || GameCanvas.gameTick % 5 != 0)
        {
            bot.SetDelay(0.5f);
            return;
        }

        if (TileMap.mapID != config.MapId)
        {
            bot.SetDelay(1.7f);
            return;
        }

        if (TileMap.zoneID <= 1)
        {
            int zoneId = AutoboMongCL.RandomZoneFrom2To14();
            Service.gI().requestChangeZone(zoneId, -1);
            bot.SetDelay(1.2f);
            return;
        }

        PickupGold();

        if (!pickingGold)
        {
            ModProCL.startTuDamBanThan();
            bot.SetDelay(3f);
        }
        else
        {
            bot.SetDelay(1f);
        }
    }

    public override void OnUpdate()
    {
        PickupGold();
    }

    private void PickupGold()
    {
        pickingGold = false;

        for (int i = 0; i < GameScr.vItemMap.size(); i++)
        {
            ItemMap item = (ItemMap)GameScr.vItemMap.elementAt(i);
            if (item != null && item.playerId == Char.myCharz().charID)
            {
                int templateId = item.template.id;
                if (templateId == 76 || templateId == 188 || templateId == 189 || templateId == 190)
                {
                    pickingGold = true;
                    MainXmapCL.TeleportTo(item.x, item.y);
                    Service.gI().pickItem(item.itemMapID);
                }
            }
        }

        if (pickingGold)
        {
            bot.SetDelay(0.5f);
        }
    }
}

public class KillPlayerHandler : QuestHandler
{
    public KillPlayerHandler(AutoboMongCL bot, QuestConfig config) : base(bot, config) { }

    public override void OnNavigate()
    {
        if (Char.myCharz().isDie)
            return;

        int zoneId = config.ZoneId != -1 ? config.ZoneId : 0;

        if (NavigateToMap(config.MapId, TileMap.zoneID != zoneId ? zoneId : -1))
        {
            SetupAutoTrain();
            bot.TransitionTo(AutoState.ExecuteQuest_Execute, 0f);
        }
        else
        {
            bot.SetDelay(1f);
        }
    }

    public override void OnExecute()
    {
        int zoneId = config.ZoneId != -1 ? config.ZoneId : 0;

        if (InfoMe.FinishBoMong)
        {
            DisablePKMode();
            bot.TransitionTo(AutoState.CompleteQuest, 0f);
            AutoboMongCL.StatusBoMong = "Nhận thưởng";
            return;
        }

        if (Char.myCharz().isDie || GameCanvas.gameTick % 5 != 0)
        {
            bot.SetDelay(0.5f);
            return;
        }

        if (TileMap.mapID != config.MapId)
        {
            bot.SetDelay(1.5f);
            return;
        }

        if (TileMap.zoneID != zoneId)
        {
            Service.gI().requestChangeZone(zoneId, -1);
            bot.SetDelay(1.2f);
            return;
        }

        if (!ModProCL.tieuDietNguoiBatCo)
        {
            EnablePKMode();
            bot.SetDelay(1.5f);
        }
    }

    public override void OnUpdate()
    {
        // Logic xử lý trong OnExecute
    }

    private void EnablePKMode()
    {
        AutoBossCL.tanCongBoss = false;
        ModProCL.tieuDietNguoiBatCo = true;
        Char.myCharz().mobFocus = null;
        Char.myCharz().itemFocus = null;
        Char.myCharz().npcFocus = null;
        GameScr.info1.addInfo("Chế độ đồ sát người: ON");
    }

    private void DisablePKMode()
    {
        ModProCL.tieuDietNguoiBatCo = false;
        AutoBossCL.tanCongBoss = false;
        ModProCL.listNguoiCoDen.Clear();
        GameScr.info1.addInfo("Chế độ đồ sát người: OFF");
    }
}
#endregion

#region Main Bot Class
public class AutoboMongCL : IActionListener, IChatable
{
    private static AutoboMongCL _Instance;
    private static readonly object _lock = new object();

    // Public states
    public static bool autoboMong;
    public static string StatusBoMong = "nhận nv";
    public static int completedTasks = 0;
    public static int cancekTasks = 0;

    // Settings
    public static QuestSettings Settings = new QuestSettings();

    // Quest data
    public static MobInfoCL[] MobDatabase;

    // State machine
    public AutoState currentState = AutoState.Idle;
    private float stateTimer = 0f;
    private float delayDuration = 0f;

    // Quest handlers
    private QuestHandler currentQuestHandler;
    private QuestType currentQuestType;

    public static AutoboMongCL getInstance()
    {
        if (_Instance == null)
        {
            lock (_lock)
            {
                if (_Instance == null)
                {
                    _Instance = new AutoboMongCL();
                }
            }
        }
        return _Instance;
    }

    static AutoboMongCL()
    {
        InitializeMobDatabase();
    }

    public static void update()
    {
        if (!autoboMong || InfoMe.EndNvBoMong)
        {
            getInstance().currentState = AutoState.Idle;
        }
        else
        {
            getInstance().HandleAutoState();
        }
    }

    private void HandleAutoState()
    {
        stateTimer += Time.unscaledDeltaTime;
        if (stateTimer < delayDuration)
            return;

        switch (currentState)
        {
            case AutoState.Idle:
                break;

            case AutoState.NavigateToQuestGiver:
                HandleNavigateToQuestGiver();
                break;

            case AutoState.ConfirmQuestGiver:
                HandleConfirmQuestGiver();
                break;

            case AutoState.WaitConfirmation:
                if (!NextMap.confirming)
                    TransitionTo(AutoState.WaitForQuestSignal, 1f);
                break;

            case AutoState.WaitForQuestSignal:
                HandleWaitForQuestSignal();
                break;

            case AutoState.RetryQuestSignal:
                if (!NextMap.confirming)
                    TransitionTo(AutoState.WaitForQuestSignal, 1.7f);
                break;

            case AutoState.CompleteQuest:
                HandleCompleteQuest();
                break;

            case AutoState.CancelQuest_Confirm:
                NextMap.startComfirmNpc(17, "nhiệm vụ hàng ngày", "hủy nhiệm vụ");
                TransitionTo(AutoState.CancelQuest_Wait, 1.7f);
                break;

            case AutoState.CancelQuest_Wait:
                if (!NextMap.confirming)
                {
                    cancekTasks++;
                    TransitionTo(AutoState.NavigateToQuestGiver, 0f);
                }
                break;

            case AutoState.FinishQuest_Navigate:
                HandleFinishQuestNavigate();
                break;

            case AutoState.FinishQuest_Wait:
                HandleFinishQuestWait();
                break;

            case AutoState.ExecuteQuest_Navigate:
                currentQuestHandler?.OnNavigate();
                break;

            case AutoState.ExecuteQuest_Execute:
                currentQuestHandler?.OnExecute();
                break;

            case AutoState.ExecuteQuest_Wait:
                currentQuestHandler?.OnUpdate();
                break;
        }
    }

    private void HandleNavigateToQuestGiver()
    {
        if (TileMap.mapID != 47)
        {
            if (!MainXmapCL.isXmaping)
                MainXmapCL.StartGoToMap(47);
        }
        else if (!MainXmapCL.isXmaping)
        {
            TransitionTo(AutoState.ConfirmQuestGiver, 0.15f);
        }
        else
        {
            SetDelay(1f);
        }
    }

    private void HandleConfirmQuestGiver()
    {
        ModProCL.teleNPC(17);
        NextMap.startComfirmNpc(17, "nhiệm vụ hàng ngày", Settings.Difficulty, "", "", "chi tiết nhiệm vụ", "", "", "nhận thưởng");
        TransitionTo(AutoState.WaitConfirmation, 0.15f);
    }

    private void HandleWaitForQuestSignal()
    {
        if (!InfoMe.NhanTinHieu && currentQuestHandler == null)
        {
            ModProCL.teleNPC(17);
            NextMap.startComfirmNpc(17, "nhiệm vụ hàng ngày", Settings.Difficulty, "", "", "chi tiết nhiệm vụ", "", "", "nhận thưởng");
            TransitionTo(AutoState.RetryQuestSignal, 0.2f);
        }
    }

    private void HandleCompleteQuest()
    {
        if (InfoMe.FinishBoMong)
        {
            CleanupQuestState();
            TransitionTo(AutoState.FinishQuest_Navigate, 0f);
        }
    }

    private void HandleFinishQuestNavigate()
    {
        if (Char.myCharz().meDead)
        {
            Service.gI().returnTownFromDead();
            SetDelay(0.5f);
            return;
        }

        if (TileMap.mapID != 47)
        {
            if (!MainXmapCL.isXmaping)
                MainXmapCL.StartGoToMap(47);
            SetDelay(1.2f);
            return;
        }

        if (!MainXmapCL.isXmaping)
        {
            ModProCL.teleNPC(17);
            NextMap.startComfirmNpc(17, "nhiệm vụ hàng ngày", "nhận thưởng");
            TransitionTo(AutoState.FinishQuest_Wait, 0.15f);
        }
    }

    private void HandleFinishQuestWait()
    {
        if (!NextMap.confirming)
        {
            ResetQuestState();
            completedTasks++;
            TransitionTo(AutoState.NavigateToQuestGiver, 0f);
        }
    }

    public void TransitionTo(AutoState newState, float delay)
    {
        currentState = newState;
        SetDelay(delay);
    }

    public void SetDelay(float delay)
    {
        delayDuration = delay;
        stateTimer = 0f;
    }

    private void CleanupQuestState()
    {
        if (AutoPick.isAutoPick)
        {
            AutoPick.isAutoPick = false;
            AutoPick.pickByList = 0;
        }

        if (!MainXmapCL.isEatChicken)
            MainXmapCL.isEatChicken = true;

        AutoTrainCL.isGoBack = false;
        ModProCL.suicide = false;
        AutoTrainCL.getInstance().perform(8, null);

        if (ModProCL.tieuDietNguoiBatCo)
        {
            ModProCL.tieuDietNguoiBatCo = false;
            AutoBossCL.tanCongBoss = false;
            ModProCL.listNguoiCoDen.Clear();
            GameScr.info1.addInfo("Chế độ đồ sát người: OFF");
        }
    }

    private void ResetQuestState()
    {
        InfoMe.FinishBoMong = false;
        InfoMe.NhanTinHieu = false;
        currentQuestHandler = null;
    }

    #region Quest Initialization
    public void StartQuest(QuestType questType, string mobName = null)
    {
        currentQuestType = questType;
        QuestConfig config = Settings.GetConfig(questType);

        if (config == null)
        {
            CancelCurrentQuest();
            return;
        }

        // Setup config based on quest type
        switch (questType)
        {
            case QuestType.TrainMonster:
                if (!SetupTrainMonsterQuest(config, mobName))
                {
                    CancelCurrentQuest();
                    return;
                }
                currentQuestHandler = new TrainMonsterHandler(this, config);
                StatusBoMong = "Train quái";
                break;

            case QuestType.TrainGold:
                SetupTrainGoldQuest(config);
                currentQuestHandler = new TrainGoldHandler(this, config);
                StatusBoMong = "Train vàng";
                break;

            case QuestType.SuicideGold:
                SetupSuicideGoldQuest(config);
                currentQuestHandler = new SuicideGoldHandler(this, config);
                StatusBoMong = "Vàng bản thân";
                break;

            case QuestType.KillPlayer:
                SetupKillPlayerQuest(config);
                currentQuestHandler = new KillPlayerHandler(this, config);
                StatusBoMong = "Pem Người";
                break;
        }

        TransitionTo(AutoState.ExecuteQuest_Navigate, 0f);
    }

    private bool SetupTrainMonsterQuest(QuestConfig config, string mobName)
    {
        if (string.IsNullOrEmpty(mobName))
            return false;

        string mapMobData = GetMapMobID(mobName);
        if (mapMobData == "-1|-1")
        {
            ChatPopup.addChatPopupMultiLineGame("Lỗi tìm map quái", 0, null);
            return false;
        }

        string[] data = mapMobData.Split('|');
        if (!int.TryParse(data[0], out int mapId) || !int.TryParse(data[1], out int mobId))
            return false;

        config.MapId = mapId;
        config.MobId = mobId;
        config.MobName = mobName;
        return true;
    }

    private void SetupTrainGoldQuest(QuestConfig config)
    {
        config.MapId = Settings.TrainGold.MapId != -1
            ? Settings.TrainGold.MapId
            : GetDefaultTrainGoldMapId();
    }

    private void SetupSuicideGoldQuest(QuestConfig config)
    {
        MainXmapCL.isEatChicken = false;
        config.MapId = Char.myCharz().cgender + 42;
    }

    private void SetupKillPlayerQuest(QuestConfig config)
    {
        MainXmapCL.isEatChicken = false;
        config.MapId = Settings.KillPlayer.MapId != -1
            ? Settings.KillPlayer.MapId
            : Char.myCharz().cgender + 42;
        config.ZoneId = Settings.KillPlayer.ZoneId;
    }

    private void CancelCurrentQuest()
    {
        TransitionTo(AutoState.CancelQuest_Confirm, 0.15f);
        StatusBoMong = "hủy nhiệm vụ";
    }
    #endregion

    #region Helper Methods
    private static int GetDefaultTrainGoldMapId()
    {
        int taskId = Char.myCharz().taskMaint.taskId;
        long power = Char.myCharz().cPower;

        if (taskId < 22) return 10;
        if (taskId == 22 || taskId == 23) return 68;
        if (taskId <= 25) return 77;
        if (taskId >= 33 && power >= 60000000000L) return 155;
        return 80;
    }

    public static string GetMapMobID(string mobName)
    {
        if (string.IsNullOrEmpty(mobName?.Trim()))
            return "-1|-1";

        string searchName = mobName.ToLower().Trim();
        int bestMatchLength = -1;
        string result = "-1|-1";

        foreach (var mob in MobDatabase)
        {
            string dbName = mob.NameMob.ToLower().Trim();
            if (searchName.Contains(dbName) && dbName.Length > bestMatchLength)
            {
                bestMatchLength = dbName.Length;
                result = $"{mob.IdMap}|{mob.IdMob}";
            }
        }

        return result;
    }

    public static int RandomZoneFrom2To14()
    {
        return UnityEngine.Random.Range(2, 15);
    }
    #endregion

    #region Menu & UI
    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 1: // Toggle auto
                ToggleAuto();
                break;
            case 2: // Change difficulty
                CycleDifficulty();
                ShowMenu();
                break;
            case 3: // Toggle skip gold quest
                Settings.TrainGold.IsEnabled = !Settings.TrainGold.IsEnabled;
                ShowMenu();
                break;
            case 5: // Toggle skip player quest
                Settings.KillPlayer.IsEnabled = !Settings.KillPlayer.IsEnabled;
                ShowMenu();
                break;
            case 6: // Toggle skip monster quest
                Settings.TrainMonster.IsEnabled = !Settings.TrainMonster.IsEnabled;
                ShowMenu();
                break;
            case 7: // Save settings
                Settings.SaveToRMS();
                ChatPopup.addChatPopupMultiLineGame("Đã lưu cài đặt", 0, null);
                break;
            case 8: // Toggle gold mode
                Settings.UseGoldSuicideMode = !Settings.UseGoldSuicideMode;
                ChatPopup.addChatPopupMultiLineGame(
                    Settings.UseGoldSuicideMode ? "Đã chọn tự pem lụm vàng bản thân" : "Đã chọn train quái lụm vàng",
                    0, null);
                break;
            case 9: // Set gold map
                ChatTextField.gI().strChat = "idMapVang";
                ChatTextField.gI().tfChat.name = "Nhập ID Map làm nv up vàng";
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 10: // Set player map
                ChatTextField.gI().strChat = "idMapNvNguoi";
                ChatTextField.gI().tfChat.name = "Nhập ID Map làm nv pem người";
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 11: // Set player zone
                ChatTextField.gI().strChat = "zoneMapNvNguoi";
                ChatTextField.gI().tfChat.name = "Nhập Khu Map làm nv pem Người";
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 12: // Toggle Kep mode
                ToggleKepMode();
                ShowMenu();
                break;
        }
    }

    private void ToggleAuto()
    {
        autoboMong = !autoboMong;

        if (!autoboMong)
        {
            StopAuto();
        }
        else if (Settings.KepMode)
        {
            autoboMong = false;
            ChatPopup.addChatPopupMultiLineGame("Nick kẹp nv Người đang bật. Bạn không thể mở AutoBoMong dc nhé!", 0, null);
        }
        else if (!InfoMe.EndNvBoMong)
        {
            StartAuto();
        }
        else
        {
            autoboMong = false;
            ChatPopup.addChatPopupMultiLineGame($"Đã hết nv hằng ngày rồi mà => [Hoàn thành: {completedTasks} + Đã hủy: {cancekTasks}]", 0, null);
        }
    }

    public void StopAuto()
    {
        MainXmapCL.FinishXmap();
        AutoTrainCL.isGoBack = false;
        InfoMe.FinishBoMong = false;
        MainXmapCL.isEatChicken = true;
        AutoPick.isAutoPick = false;
        AutoPick.pickByList = 0;
        currentState = AutoState.Idle;
        currentQuestHandler = null;
    }

    private void CycleDifficulty()
    {
        Settings.Difficulty = Settings.Difficulty switch
        {
            "siêu khó" => "dễ",
            "dễ" => "khó",
            _ => "siêu khó"
        };
    }

    private void ToggleKepMode()
    {
        Settings.KepMode = !Settings.KepMode;

        if (Settings.KepMode)
        {
            Service.gI().getFlag(1, 8);
            if (Char.myCharz().cFlag == 0)
            {
                Settings.KepMode = false;
                ChatPopup.addChatPopupMultiLineGameline("Cờ đen chưa được bật, mở auto kẹp thất bại. Mở lại đi");
                return;
            }
            AutoTrainCL.isGoBack = true;
            AutoTrainCL.gobackMapID = TileMap.mapID;
            AutoTrainCL.gobackZoneID = TileMap.zoneID;
            MainXmapCL.isEatChicken = false;
        }
        else
        {
            AutoTrainCL.isGoBack = false;
            MainXmapCL.isEatChicken = true;
        }
        GameScr.info1.addInfo("Auto nick Kẹp: " + (Settings.KepMode ? "ON" : "OFF"));
    }

    public static void ShowMenu()
    {
        MyVector menu = new MyVector();

        menu.addElement(new Command(autoboMong ? "Dừng" : "Bắt đầu", getInstance(), 1, null));
        menu.addElement(new Command($"Đổi mức độ\nHiện tại: {Settings.Difficulty}", getInstance(), 2, null));
        menu.addElement(new Command($"Bỏ nv nhặt vàng: {(Settings.TrainGold.IsEnabled ? "OFF" : "ON")}", getInstance(), 3, null));
        menu.addElement(new Command($"Bỏ nv pem người: {(Settings.KillPlayer.IsEnabled ? "OFF" : "ON")}", getInstance(), 5, null));
        menu.addElement(new Command($"Bỏ nv pem quái: {(Settings.TrainMonster.IsEnabled ? "OFF" : "ON")}", getInstance(), 6, null));
        menu.addElement(new Command($"Chọn kiểu NV vàng: {(Settings.UseGoldSuicideMode ? "Nhặt vàng" : "Up Quái")}", getInstance(), 8, null));

        int goldMapId = Settings.TrainGold.MapId != -1 ? Settings.TrainGold.MapId : GetDefaultTrainGoldMapId();
        menu.addElement(new Command($"Đổi map up vàng: {TileMap.mapNames[goldMapId]}", getInstance(), 9, null));

        int playerMapId = Settings.KillPlayer.MapId == -1 ? Char.myCharz().cgender + 42 : Settings.KillPlayer.MapId;
        int playerZone = Settings.KillPlayer.ZoneId != -1 ? Settings.KillPlayer.ZoneId : 0;
        menu.addElement(new Command($"Đổi map nv pem người: {TileMap.mapNames[playerMapId]}", getInstance(), 10, null));
        menu.addElement(new Command($"Đổi khu nv pem người: {playerZone}", getInstance(), 11, null));
        menu.addElement(new Command($"Auto Kẹp nv Pem người: {(Settings.KepMode ? "ON" : "OFF")}", getInstance(), 12, null));
        menu.addElement(new Command("Lưu cài đặt", getInstance(), 7, null));

        GameCanvas.menu.startAt(menu, 3);
        GameCanvas.menu.setMenuHeaderText("Mặc định bỏ qua nhiệm vụ ăn trộm\nNhớ nhiệm vụ pem người phải lấy nick kẹp");
    }

    public static void loadData()
    {
        Settings.LoadFromRMS();
    }

    public static void StartAuto()
    {
        InfoMe.FinishBoMong = false;
        InfoMe.NhanTinHieu = false;
        MainXmapCL.isEatChicken = true;
        AutoPick.isAutoPick = false;
        AutoPick.pickByList = 0;
        AutoTrainCL.isGoBack = false;
        AutoTrainCL.TuMoTDLT();
        getInstance().TransitionTo(AutoState.NavigateToQuestGiver, 0f);
        StatusBoMong = "nhận nv";
    }
    #endregion

    #region Chat Interface
    public void onChatFromMe(string text, string to)
    {
        if (string.IsNullOrEmpty(text) || ChatTextField.gI().tfChat.getText() == null)
        {
            ResetChatTextField();
            return;
        }

        string chatType = ChatTextField.gI().strChat;

        if (chatType.Equals("idMapVang"))
        {
            HandleMapInput(text, "vàng", id => Settings.TrainGold.MapId = id);
        }
        else if (chatType.Equals("idMapNvNguoi"))
        {
            HandleMapInput(text, "pem người", id => Settings.KillPlayer.MapId = id);
        }
        else if (chatType.Equals("zoneMapNvNguoi"))
        {
            HandleZoneInput(text);
        }
        else
        {
            Service.gI().chat(text);
            ChatTextField.gI().isShow = false;
        }
    }

    private void HandleMapInput(string text, string questName, Action<int> setMapId)
    {
        try
        {
            if (string.IsNullOrEmpty(text.Trim()))
            {
                GameScr.info1.addInfo("Nhập đi chứ dm!");
                return;
            }

            if (int.TryParse(text.Trim(), out int mapId) &&
                mapId >= 0 && mapId < TileMap.mapNames.Length &&
                TileMap.mapNames[mapId] != null)
            {
                setMapId(mapId);
                GameScr.info1.addInfo($"Đã đổi map {questName} thành {TileMap.mapNames[mapId]}");
            }
            else
            {
                GameScr.info1.addInfo($"Id Map {questName} không tồn tại!");
            }
        }
        catch
        {
            GameScr.info1.addInfo("Vui lòng nhập đúng định dạng Id Map");
        }
        ResetChatTextField();
    }

    private void HandleZoneInput(string text)
    {
        try
        {
            if (string.IsNullOrEmpty(text.Trim()))
            {
                GameScr.info1.addInfo("Nhập đi chứ dm!");
                return;
            }

            if (int.TryParse(text.Trim(), out int zone) && zone >= 0)
            {
                Settings.KillPlayer.ZoneId = zone;
                GameScr.info1.addInfo($"Đã đổi khu pem người thành {zone}");
            }
            else
            {
                GameScr.info1.addInfo("Khu pem người không tồn tại!");
            }
        }
        catch
        {
            GameScr.info1.addInfo("Vui lòng nhập đúng định dạng khu");
        }
        ResetChatTextField();
    }

    public void onCancelChat() { }

    private static void ResetChatTextField()
    {
        ChatTextField.gI().strChat = "Chat";
        ChatTextField.gI().tfChat.name = "chat";
        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
        ChatTextField.gI().isShow = false;
    }
    #endregion

    #region Mob Database
    private static void InitializeMobDatabase()
    {
        MobDatabase = new MobInfoCL[]
        {
            new MobInfoCL("Mộc nhân", 14, 0),
            new MobInfoCL("Khủng long", 1, 1),
            new MobInfoCL("Lợn lòi", 8, 2),
            new MobInfoCL("Quỷ đất", 15, 3),
            new MobInfoCL("Khủng long mẹ", 2, 4),
            new MobInfoCL("Lợn lòi mẹ", 9, 5),
            new MobInfoCL("Quỷ đất mẹ", 16, 6),
            new MobInfoCL("Thằn lằn bay", 3, 7),
            new MobInfoCL("Phi long", 11, 8),
            new MobInfoCL("Quỷ bay", 17, 9),
            new MobInfoCL("Thằn lằn mẹ", 4, 10),
            new MobInfoCL("Phi long mẹ", 12, 11),
            new MobInfoCL("Quỷ bay mẹ", 18, 12),
            new MobInfoCL("Ốc mượn hồn", 29, 13),
            new MobInfoCL("Ốc sên", 33, 14),
            new MobInfoCL("Heo Xayda mẹ", 37, 15),
            new MobInfoCL("Heo rừng", 28, 16),
            new MobInfoCL("Heo da xanh", 32, 17),
            new MobInfoCL("Heo Xayda", 36, 18),
            new MobInfoCL("Heo rừng mẹ", 6, 19),
            new MobInfoCL("Heo xanh mẹ", 10, 20),
            new MobInfoCL("Alien", 19, 21),
            new MobInfoCL("Bulon", 30, 22),
            new MobInfoCL("Ukulele", 34, 23),
            new MobInfoCL("Quỷ mập", 38, 24),
            new MobInfoCL("Tambourine", 6, 25),
            new MobInfoCL("Drum", 10, 26),
            new MobInfoCL("Akkuman", 19, 27),
            new MobInfoCL("Không tặc", 29, 31),
            new MobInfoCL("Quỷ đầu to", 33, 32),
            new MobInfoCL("Quỷ địa ngục", 37, 33),
            new MobInfoCL("Nappa", 68, 39),
            new MobInfoCL("Soldier", 70, 40),
            new MobInfoCL("Appule", 71, 41),
            new MobInfoCL("Raspberry", 71, 42),
            new MobInfoCL("Thằn lằn xanh", 72, 43),
            new MobInfoCL("Quỷ đầu nhọn", 64, 44),
            new MobInfoCL("Quỷ đầu vàng", 63, 45),
            new MobInfoCL("Quỷ da tím", 66, 46),
            new MobInfoCL("Quỷ già", 67, 47),
            new MobInfoCL("Cá sấu", 73, 48),
            new MobInfoCL("Dơi da xanh", 67, 49),
            new MobInfoCL("Quỷ chim", 81, 50),
            new MobInfoCL("Lính đầu trọc", 74, 51),
            new MobInfoCL("Lính tai dài", 76, 52),
            new MobInfoCL("Lính vũ trụ", 77, 53),
            new MobInfoCL("Khỉ lông đen", 82, 54),
            new MobInfoCL("Khỉ giáp sắt", 83, 55),
            new MobInfoCL("Khỉ lông đỏ", 79, 56),
            new MobInfoCL("Khỉ lông vàng", 80, 57),
            new MobInfoCL("Xên con cấp 1", 92, 58),
            new MobInfoCL("Xên con cấp 2", 93, 59),
            new MobInfoCL("Xên con cấp 3", 94, 60),
            new MobInfoCL("Xên con cấp 4", 96, 61),
            new MobInfoCL("Xên con cấp 5", 97, 62),
            new MobInfoCL("Xên con cấp 6", 98, 63),
            new MobInfoCL("Xên con cấp 7", 99, 64),
            new MobInfoCL("Xên con cấp 8", 100, 65),
            new MobInfoCL("Tai tím", 106, 66),
            new MobInfoCL("Abo", 107, 67),
            new MobInfoCL("Kado", 109, 68),
            new MobInfoCL("Da xanh", 110, 69),
            new MobInfoCL("Khỉ lông xanh", 155, 78),
            new MobInfoCL("Taburine Đỏ", 155, 79),
            new MobInfoCL("Ếch mặt đỏ", 166, 86),
            new MobInfoCL("Jinai", 166, 87),
            new MobInfoCL("Máy đo sức mạnh", 42, 94)
        };
    }
    #endregion

    public static void Paint(mGraphics g) { }
}
#endregion