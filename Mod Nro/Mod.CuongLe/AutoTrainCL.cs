using Client244.Xmap;
using DoHoa.CustomMenu;
using Mod.community;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Mod.CuongLe;

public class AutoTrainCL : IActionListener, IChatable
{
    private static AutoTrainCL _Instance;

    public static bool isAvoidSuperMob;

    public static bool isGoBack;

    public static bool isGobackCoordinate;

    public static int gobackX;

    public static int gobackY;

    public static int gobackMapID;

    public static int gobackZoneID;

    public static bool isAutoTrain;

    public static int minimumMPGoHome;

    private static string[] inputMPPercentGoHome;

    private static string[] inputHPAboveMobTRain;

    private static string[] inputHPBelowMobTRain;

    public static Dictionary<int, List<int>> listMobIds;

    public static long lastTimeAddNewMob;

    private static long lastTimeTeleportToMob;

    public static bool AutoChangeClothes;

    private static int typeMobChange;

    private static bool isHandlingFlyingMob;

    private static long lastFlyingMobTeleTime;

    public static bool autoHopThe;

    public static long lastHopTheTime;

    public static bool autoNeBoss;

    public static long lastUpdateNeBoss;

    public static bool ReturnedGoback;

    public static bool autoChangeZone;

    public static long lastautoChangeZone;

    public static bool SpamChangeZone;

    public static long lastSpamChangeZone;

    public static bool TYPEAK;

    private static readonly HashSet<int> NO_FOCUS_SKILLS;

    public static long OnlyHitWhenAboveHP = 0;

    public static long OnlyHitWhenBelowHP = long.MaxValue;

    public static AutoTrainCL getInstance()
    {
        if (_Instance == null)
        {
            _Instance = new AutoTrainCL();
        }
        return _Instance;
    }

    public static void updateAutoHopThe()
    {
        if (!autoHopThe || Char.myCharz().isNhapThe || Char.myCharz().meDead || GameCanvas.gameTick % 5 != 0)
        {
            return;
        }
        long num = mSystem.currentTimeMillis();
        if (num - lastHopTheTime < 1000)
        {
            return;
        }
        int num2 = ModProCL.ExistPotara();
        if (num2 != -1)
        {
            Item item = ModProCL.FindItemBag(num2);
            if (item != null)
            {
                Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                lastHopTheTime = num;
            }
        }
    }

    public static void updateNeBoss()
    {
        if (!autoNeBoss || mSystem.currentTimeMillis() - lastUpdateNeBoss < 1200)
        {
            return;
        }
        lastUpdateNeBoss = mSystem.currentTimeMillis();
        if (AutoBossCL.checkBoss())
        {
            AutoBossCL.offPaintZone = true;
            Service.gI().openUIZone();
            gobackZoneID = Random.Range(0, GameScr.gI().zones.Length);
            GameScr.isChangeZone = true;
            if (!isGoBack)
            {
                Service.gI().requestChangeZone(gobackZoneID, -1);
            }
        }
    }

    public static void UpdateAutoChangeZoneItNguoi()
    {
        if (!autoChangeZone
            || mSystem.currentTimeMillis() - lastautoChangeZone < (GameScr.canAutoPlay ? 5700 : 11000)
            || (isAutoTrain && Char.myCharz().mobFocus != null) || TileMap.mapID == Char.myCharz().cgender + 21)
        {
            return;
        }

        lastautoChangeZone = mSystem.currentTimeMillis();
        AutoBossCL.offPaintZone = true;
        Service.gI().openUIZone();
        int currentZone = TileMap.zoneID;
        int currentNumPlayer = GameScr.gI().numPlayer[currentZone];

        if (currentNumPlayer <= 1)
        {
            return;
        }

        int bestZone = -1;
        int minPlayer = int.MaxValue;

        for (int i = 0; i < GameScr.gI().zones.Length; i++)
        {
            if (i == currentZone)
                continue;

            int numPlayer = GameScr.gI().numPlayer[i];
            int maxPlayer = GameScr.gI().maxPlayer[i];

            if (numPlayer < maxPlayer)
            {
                if (numPlayer == 0)
                {
                    bestZone = i;
                    minPlayer = 0;
                    break;
                }

                if (numPlayer < minPlayer)
                {
                    minPlayer = numPlayer;
                    bestZone = i;
                }
            }
        }

        if (bestZone != -1)
        {
            if (minPlayer + 1 >= GameScr.gI().numPlayer[currentZone])
                return;

            gobackZoneID = bestZone;
            GameScr.isChangeZone = true;

            if (!isGoBack)
            {
                Service.gI().requestChangeZone(gobackZoneID, -1);
            }
        }
    }


    public static void UpdateSpamChangeZoneItNguoi()
    {
        if (!SpamChangeZone || mSystem.currentTimeMillis() - lastSpamChangeZone < (GameScr.canAutoPlay ? 5700 : 11000) || (isAutoTrain && Char.myCharz().mobFocus != null) || TileMap.mapID == Char.myCharz().cgender + 21)
        {
            return;
        }
        lastSpamChangeZone = mSystem.currentTimeMillis();
        AutoBossCL.offPaintZone = true;
        Service.gI().openUIZone();
        int num = -1;
        int num2 = int.MaxValue;
        int zoneID = TileMap.zoneID;
        for (int num3 = GameScr.gI().zones.Length - 1; num3 >= 0; num3--)
        {
            if (GameScr.gI().zones[num3] != zoneID)
            {
                int num4 = GameScr.gI().numPlayer[num3];
                int num5 = GameScr.gI().maxPlayer[num3];
                if (num4 < num5)
                {
                    if (num4 == 0)
                    {
                        num = num3;
                        break;
                    }
                    if (num4 < num2)
                    {
                        num2 = num4;
                        num = num3;
                    }
                }
            }
        }
        if (num != -1)
        {
            gobackZoneID = num;
            GameScr.isChangeZone = true;
            if (!isGoBack)
            {
                Service.gI().requestChangeZone(gobackZoneID, -1);
            }
        }
    }
    public static void Update()
    {
        if (ReturnedGoback && !MainXmapCL.isXmaping)
        {
            updateNeBoss();
            UpdateAutoChangeZoneItNguoi();
            UpdateSpamChangeZoneItNguoi();
        }
        updateAutoHopThe();
        if (Char.myCharz().mobFocus != null && (Char.myCharz().mobFocus.hp <= 0 || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0))
        {
            Char.myCharz().mobFocus = null;
        }
        if (isAutoTrain && GameCanvas.gameTick % 20 == 0)
        {
            if (!GameScr.canAutoPlay)
            {
                TuMoTDLT();
            }
            DoIt();
        }
        if (Char.myCharz().cStamina <= 5 && GameCanvas.gameTick % 140 == 0)
        {
            UseGrape();
        }
        if (!isGoBack)
        {
            return;
        }
        ReturnedGoback = false;
        if (Char.myCharz().meDead && GameCanvas.gameTick % 180 == 0)
        {
            Service.gI().returnTownFromDead();
        }
        if (isMeOutOfMpOR1HP())
        {
            int num = 21 + Char.myCharz().cgender;
            if (TileMap.mapID != num)
            {
                GameScr.isAutoPlay = false;
                Char.myCharz().mobFocus = null;
                if (GameCanvas.gameTick % 60 == 0 && !MainXmapCL.isXmaping)
                {
                    MainXmapCL.StartGoToMap(num);
                }
            }
            else
            {
                MainXmapCL.TryEatChicken();
            }
        }
        else if (TileMap.mapID != gobackMapID)
        {
            GameScr.isAutoPlay = false;
            MainXmapCL.StartGoToMap(gobackMapID);
        }
        else if (TileMap.mapID == gobackMapID)
        {
            if (!isGobackCoordinate && GameCanvas.gameTick % 140 == 0)
            {
                GameScr.isAutoPlay = true;
            }
            if (TileMap.zoneID != gobackZoneID && !Char.ischangingMap && !Controller.isStopReadMessage && GameCanvas.gameTick % 120 == 0)
            {
                Service.gI().requestChangeZone(gobackZoneID, -1);
            }
            if (isGobackCoordinate && (Char.myCharz().cx != gobackX || Char.myCharz().cy != gobackY) && GameCanvas.gameTick % 140 == 0)
            {
                TeleportTo(gobackX, gobackY);
            }
            if (TileMap.mapID == gobackMapID && TileMap.zoneID == gobackZoneID && (!isGobackCoordinate || (Char.myCharz().cx == gobackX && Char.myCharz().cy == gobackY)) && GameCanvas.gameTick % 140 == 0)
            {
                ReturnedGoback = true;
            }
        }
    }

    public void onChatFromMe(string text, string to)
    {
        if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
        {
            if (ChatTextField.gI().strChat.Equals(inputMPPercentGoHome[0]))
            {
                try
                {
                    int num = (minimumMPGoHome = int.Parse(ChatTextField.gI().tfChat.getText()));
                    GameScr.info1.addInfo("Về Nhà Khi MP Dưới\n[" + num + "%]");
                }
                catch
                {
                    GameScr.info1.addInfo("%MP Không Hợp Lệ, Vui Lòng Nhập Lại");
                }
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(inputHPAboveMobTRain[0]))
            {
                try
                {
                    OnlyHitWhenAboveHP = long.Parse(ChatTextField.gI().tfChat.getText());
                    GameScr.info1.addInfo("Chỉ đánh quái khi HP trên " + Res.formatNumber2(OnlyHitWhenAboveHP));
                }
                catch
                {
                    GameScr.info1.addInfo("Hp phải là 1 số , Vui Lòng Nhập Lại");
                    OnlyHitWhenAboveHP = 0;
                }
                ResetChatTextField();
            }
            else if (ChatTextField.gI().strChat.Equals(inputHPBelowMobTRain[0]))
            {
                try
                {
                    OnlyHitWhenBelowHP = long.Parse(ChatTextField.gI().tfChat.getText());
                    GameScr.info1.addInfo("Chỉ đánh quái khi HP dưới " + Res.formatNumber2(OnlyHitWhenBelowHP));
                }
                catch
                {
                    GameScr.info1.addInfo("Hp phải là 1 số , Vui Lòng Nhập Lại");
                    OnlyHitWhenBelowHP = long.MaxValue;
                }
                ResetChatTextField();
            }
        }
        else
        {
            ChatTextField.gI().isShow = false;
        }
    }

    public void onCancelChat()
    {
        ChatTextField.gI().strChat = "Chat";
        ChatTextField.gI().tfChat.name = "chat";
        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
        ChatTextField.gI().isShow = false;
    }

    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 1:
                {
                    int num = (int)p;
                    List<int> currentMapMobList = GetCurrentMapMobList();
                    currentMapMobList.Clear();
                    for (int i = 0; i < GameScr.vMob.size(); i++)
                    {
                        Mob mob = (Mob)GameScr.vMob.elementAt(i);
                        if (!mob.isMobMe && mob.templateId == num)
                        {
                            currentMapMobList.Add(mob.mobId);
                        }
                    }
                    MobTrainTab.UpdateMobTrainFlags();
                    TurnOnAutoTrain();
                    break;
                }
            case 2:
                {
                    List<int> currentMapMobList2 = GetCurrentMapMobList();
                    currentMapMobList2.Clear();
                    for (int j = 0; j < GameScr.vMob.size(); j++)
                    {
                        Mob mob2 = (Mob)GameScr.vMob.elementAt(j);
                        if (!mob2.isMobMe)
                        {
                            currentMapMobList2.Add(mob2.mobId);
                        }
                    }
                    MobTrainTab.UpdateMobTrainFlags();
                    TurnOnAutoTrain();
                    break;
                }
            case 3:
                TurnOnAutoTrain();
                break;
            case 4:
                isAvoidSuperMob = !isAvoidSuperMob;
                GameScr.info1.addInfo("Né Siêu Quái\n" + (isAvoidSuperMob ? "[STATUS: OFF]" : "[STATUS: ON]"));
                break;
            case 5:
                ShowMenuGoback();
                break;
            case 6:
                listMobIds.Clear();
                isAutoTrain = false;
                GameScr.info1.addInfo("Đã Clear Danh Sách Train!");
                break;
            case 7:
                if (Char.myCharz().mobFocus == null)
                {
                    GameScr.info1.addInfo("Vui Lòng Chọn Quái!");
                }
                if (Char.myCharz().mobFocus != null)
                {
                    GetCurrentMapMobList().Add(Char.myCharz().mobFocus.mobId);
                    GameScr.info1.addInfo("Đã Thêm Quái: " + Char.myCharz().mobFocus.mobId);
                }
                break;
            case 8:
                isAutoTrain = false;
                Char.myCharz().mobFocus = null;
                GameScr.info1.addInfo("Đã Tắt Auto Train!");
                break;
            case 9:
                if (isGoBack)
                {
                    isGoBack = false;
                    GameScr.info1.addInfo("Goback\n[STATUS: OFF]");
                }
                else if (!isGoBack)
                {
                    isGobackCoordinate = false;
                    isGoBack = true;
                    gobackMapID = TileMap.mapID;
                    gobackZoneID = TileMap.zoneID;
                    GameScr.info1.addInfo("Goback\n[" + TileMap.mapNames[gobackMapID] + "]\n[" + gobackZoneID + "]");
                }
                break;
            case 10:
                if (isGoBack)
                {
                    isGoBack = false;
                    GameScr.info1.addInfo("Goback\n[STATUS: OFF]");
                }
                else if (!isGoBack)
                {
                    isGobackCoordinate = true;
                    isGoBack = true;
                    gobackMapID = TileMap.mapID;
                    gobackZoneID = TileMap.zoneID;
                    gobackX = Char.myCharz().cx;
                    gobackY = Char.myCharz().cy;
                    GameScr.info1.addInfo("Goback Tọa Độ\n[" + gobackX + "-" + gobackY + "]");
                }
                break;
            case 11:
                ChatTextField.gI().strChat = inputMPPercentGoHome[0];
                ChatTextField.gI().tfChat.name = inputMPPercentGoHome[1];
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 12:
                if (Char.myCharz().getGender() == "TĐ" || Char.myCharz().getGender() == "NM")
                {
                    GameScr.info1.addInfo("Chỉ dành cho xd");
                    break;
                }
                if (AutoItem.set1.Count == 0 || AutoItem.set2.Count == 0)
                {
                    GameScr.info1.addInfo("Vui lòng thêm đồ cho set 1 và sét 2");
                    break;
                }
                AutoChangeClothes = !AutoChangeClothes;
                GameScr.info1.addInfo("|0| Auto mặc sét 1 khi khỉ, khỉ sịt mặc sét 2: " + (AutoChangeClothes ? "Bật" : "Tắt"));
                if (AutoChangeClothes)
                {
                    new Thread(changeclothes).Start();
                }
                break;
            case 13:
                autoChangeZone = !autoChangeZone;
                if (TileMap.mapID == Char.myCharz().cgender + 21)
                {
                    SpamChangeZone = false;
                    ChatPopup.addChatPopupMultiLineGameline("Trong nhà mà auto đổi khu cái đjt mọe mày à");
                    return;
                }
                autoNeBoss = false;
                GameScr.info1.addInfo("Auto đổi khu ít người: " + (autoChangeZone ? "Bật" : "Tắt"));
                break;
            case 14:
                autoHopThe = !autoHopThe;
                if (autoHopThe)
                {
                    if (ModProCL.ExistPotara() == -1)
                    {
                        autoHopThe = false;
                        ChatPopup.addChatPopupMultiLineGameline("Mày làm cak j có bông tai mà auto hợp thể ???");
                    }
                    else if (TileMap.mapID == Char.myCharz().cgender + 21)
                    {
                        autoHopThe = false;
                        ChatPopup.addChatPopupMultiLineGameline("Vui lòng ra khỏi nhà để mở auto hợp thể!");
                    }
                }
                GameScr.info1.addInfo("Auto hợp thể: " + (autoHopThe ? "Bật" : "Tắt"));
                break;
            case 15:
                autoNeBoss = !autoNeBoss;
                if (autoNeBoss)
                {
                    autoChangeZone = false;
                    SpamChangeZone = false;
                }

                GameScr.info1.addInfo("Auto né Boss: " + (autoNeBoss ? "Bật" : "Tắt"));
                break;
            case 16:
                TYPEAK = !TYPEAK;
                Rms.saveRMSInt("TYPETRAIN", TYPEAK ? 1 : 0);
                ChatPopup.addChatPopupMultiLineGameline("Loại Train có TĐLT: " + (TYPEAK ? "AK" : "MẶC ĐỊNH") + " đã được lưu cho các lần mở game sau luôn");
                break;
            case 17:
                MainMenu.ToggleMenu(show: true, 2);
                break;
            case 18:
                ChatTextField.gI().strChat = inputHPAboveMobTRain[0];
                ChatTextField.gI().tfChat.name = inputHPAboveMobTRain[1];
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 19:
                ChatTextField.gI().strChat = inputHPBelowMobTRain[0];
                ChatTextField.gI().tfChat.name = inputHPBelowMobTRain[1];
                ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 20:
                ShowMenuConfigHPTrainMob();
                break;
            case 21:
                SpamChangeZone = !SpamChangeZone;
                if (TileMap.mapID == Char.myCharz().cgender + 21)
                {
                    SpamChangeZone = false;
                    ChatPopup.addChatPopupMultiLineGameline("Trong nhà mà auto đổi khu cái đjt mọe mày à");
                    return;
                }
                autoNeBoss = false;
                GameScr.info1.addInfo("Spam đổi khu ít người: " + (SpamChangeZone ? "Bật" : "Tắt"));
                break;
            default:
                return;
        }
    }

    public static void ShowMenu()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Mở Menu Train", getInstance(), 17, null));
        myVector.addElement(new Command("SEND DAME TRAIN: " + (TYPEAK ? "Ak [Không phụ thuộc FPS]" : "Mặc định"), getInstance(), 16, null));
        myVector.addElement(new Command("Cấu hình HP train Quái" , getInstance(), 20, null));
        GameCanvas.menu.startAt(myVector, 3);
    }
    public static void ShowMenuKhuIt()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command($"Auto Khu ít: " + (autoChangeZone ? "Bật" : "Tắt"), getInstance(), 13, null));
        myVector.addElement(new Command($"Spam Khu ít: " + (SpamChangeZone ? "Bật" : "Tắt"), getInstance(), 21, null));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void ShowMenuGoback()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Goback\n" + (isGoBack ? ("[" + TileMap.mapNames[gobackMapID] + "]\n[" + gobackZoneID + "]") : "[STATUS: OFF]"), getInstance(), 9, null));
        myVector.addElement(new Command("Goback Tọa Độ\n" + ((!isGoBack || !isGobackCoordinate) ? "[STATUS: OFF]" : ("[" + gobackX + "-" + gobackY + "]")), getInstance(), 10, null));
        myVector.addElement(new Command("Về Nhà Khi MP Dưới\n[" + minimumMPGoHome + "%]", getInstance(), 11, null));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void ShowMenuConfigHPTrainMob()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Chỉ đánh quái hp trên: " +Res.formatNumber2(OnlyHitWhenAboveHP), getInstance(), 18, null));
        myVector.addElement(new Command("Chỉ đánh quái hp dưới: " + Res.formatNumber2(OnlyHitWhenBelowHP), getInstance(), 19, null));
        GameCanvas.menu.startAt(myVector, 3);
    }

    private static void ResetChatTextField()
    {
        ChatTextField.gI().strChat = "Chat";
        ChatTextField.gI().tfChat.name = "chat";
        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
        ChatTextField.gI().isShow = false;
    }

    private static void TeleportTo(int x, int y)
    {
        Char.myCharz().cx = x;
        Char.myCharz().cy = y;
        Service.gI().charMove();
        Char.myCharz().cx = x;
        Char.myCharz().cy = y + 1;
        Service.gI().charMove();
        Char.myCharz().cx = x;
        Char.myCharz().cy = y;
        Service.gI().charMove();
    }

    private static bool isMeCanAttack(Mob mob)
    {
        if (!GameScr.canAutoPlay && mob.checkIsBoss())
        {
            if (mob.checkIsBoss())
            {
                return isAvoidSuperMob;
            }
            return false;
        }
        return true;
    }

    private static bool isMeOutOfMpOR1HP()
    {
        return Char.myCharz().cMP < Char.myCharz().cMPFull * minimumMPGoHome / 100 || Char.myCharz().cHP == 1;
    }

    private static Mob GetNextMob(int type)
    {
        List<int> currentMapMobList = GetCurrentMapMobList();
        if (currentMapMobList.Count == 0)
        {
            return null;
        }
        if (type == 1)
        {
            long num = mSystem.currentTimeMillis();
            Mob result = null;
            for (int i = 0; i < currentMapMobList.Count; i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(currentMapMobList[i]);
                long cTimeDie = mob.cTimeDie;
                if (!mob.isMobMe && mob.status != 0 && cTimeDie < num && isMeCanAttack(mob) && mob.hp > OnlyHitWhenAboveHP && mob.hp < OnlyHitWhenBelowHP)
                {
                    result = mob;
                    num = cTimeDie;
                }
            }
            return result;
        }
        Mob result2 = null;
        int num2 = int.MaxValue;
        for (int j = 0; j < currentMapMobList.Count; j++)
        {
            Mob mob2 = (Mob)GameScr.vMob.elementAt(currentMapMobList[j]);
            if (mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe && isMeCanAttack(mob2) && mob2.hp > OnlyHitWhenAboveHP && mob2.hp < OnlyHitWhenBelowHP)
            {
                int num3 = Math.abs(Char.myCharz().cx - mob2.xFirst);
                int num4 = Math.abs(Char.myCharz().cy - mob2.yFirst);
                int num5 = num3 + num4;
                if (num5 < num2)
                {
                    result2 = mob2;
                    num2 = num5;
                }
            }
        }
        return result2;
    }

    public static void TuMoTDLT()
    {
        try
        {
            if (!ModProCL.ExistItemBag(521) || ItemTime.isExistItem(4387) || AutoBuyItemCL.timeItemDatBiet(521) == 0)
            {
                return;
            }
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
                Item item = Char.myCharz().arrItemBag[i];
                if (item != null && item.template.id == 521)
                {
                    Service.gI().useItem(0, 1, (sbyte)i, -1);
                    break;
                }
            }
        }
        catch
        {
        }
    }

    private static void TurnOnAutoTrain()
    {
        if (GetCurrentMapMobList().Count == 0)
        {
            GameScr.info1.addInfo("Danh Sách Tàn Sát Trống!");
            isAutoTrain = false;
        }
        else
        {
            isAutoTrain = true;
        }
    }

    static AutoTrainCL()
    {
        NO_FOCUS_SKILLS = new HashSet<int> { 6, 8, 12, 13, 19, 21 };
        autoHopThe = false;
        lastHopTheTime = 0L;
        lastUpdateNeBoss = 0L;
        ReturnedGoback = true;
        isHandlingFlyingMob = false;
        lastFlyingMobTeleTime = 0L;
        typeMobChange = -1;
        listMobIds = new Dictionary<int, List<int>>();
        minimumMPGoHome = 5;
        inputMPPercentGoHome = new string[2] { "Nhập %MP", "%MP"};
        inputHPAboveMobTRain = new string[2] { "Nhập hp quái chỉ đánh khi mục tiêu trên", "hp" };
        inputHPBelowMobTRain = new string[2] { "Nhập hp quái chỉ đánh khi mục tiêu dưới", "hp" };
    }

    private static void DoIt()
    {
        if (!isAutoTrain || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 || AutoBossCL.tanCongBoss || ModProCL.tieuDietNguoiBatCo || MainXmapCL.isXmaping || Char.myCharz().isWaitMonkey || Char.myCharz().isCharge || (isGoBack && isMeOutOfMpOR1HP()))
        {
            return;
        }
        if (Char.myCharz().mobFocus != null && !Char.myCharz().mobFocus.isMobMe && (Char.myCharz().mobFocus.hp <= 0 || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0 || !isMeCanAttack(Char.myCharz().mobFocus) 
            || Char.myCharz().mobFocus.hp < OnlyHitWhenAboveHP || Char.myCharz().mobFocus.hp > OnlyHitWhenBelowHP))
        {
            Char.myCharz().mobFocus = null;
            isHandlingFlyingMob = false;
        }
        if (listMobIds.Count == 0)
        {
            if (mSystem.currentTimeMillis() - lastTimeAddNewMob > 5000)
            {
                lastTimeAddNewMob = mSystem.currentTimeMillis();
                GameScr.info1.addInfo("Danh Sách Tàn Sát Trống!");
            }
            isAutoTrain = false;
            return;
        }
        if (Char.myCharz().mobFocus != null && !Char.myCharz().mobFocus.isMobMe)
        {
            if (Char.myCharz().mobFocus.hp <= 0 || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0 || !isMeCanAttack(Char.myCharz().mobFocus))
            {
                Char.myCharz().mobFocus = null;
                isHandlingFlyingMob = false;
            }
        }
        else
        {
            if (!GameScr.canAutoPlay && AutoPick.isAutoPick)
            {
                AutoPick.FocusToNearestItem();
                if (Char.myCharz().itemFocus != null)
                {
                    ItemMap itemFocus = Char.myCharz().itemFocus;
                    int num = Math.abs(Char.myCharz().cx - itemFocus.x);
                    int num2 = Math.abs(Char.myCharz().cy - itemFocus.y);
                    if (num > 50 || num2 > 50)
                    {
                        MainXmapCL.TeleportTo(itemFocus.x, itemFocus.y);
                    }
                    AutoPick.PickIt();
                    AutoPick.FocusToNearestItem();
                    return;
                }
            }
            else
            {
                Char.myCharz().itemFocus = null;
            }
            if (Char.myCharz().itemFocus == null)
            {
                Mob nextMob = GetNextMob(0);
                Char.myCharz().clearFocus(0);
                if (nextMob == null)
                {
                    nextMob = GetNextMob(1);
                    Char.myCharz().clearFocus(0);
                    if (nextMob == null)
                    {
                        return;
                    }
                    if (!GameScr.canAutoPlay)
                    {
                        Char.myCharz().currentMovePoint = new MovePoint(nextMob.xFirst, nextMob.yFirst);
                        Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
                    }
                }
                else
                {
                    Char.myCharz().mobFocus = nextMob;
                    if (GameScr.canAutoPlay)
                    {
                        Char.myCharz().cx = nextMob.x;
                        Char.myCharz().cy = nextMob.y;
                        Service.gI().charMove();
                    }
                }
                isHandlingFlyingMob = false;
            }
        }
        if (Char.myCharz().mobFocus == null || (Char.myCharz().skillInfoPaint() != null && Char.myCharz().indexSkill < Char.myCharz().skillInfoPaint().Length && Char.myCharz().dart != null && Char.myCharz().arr != null))
        {
            return;
        }
        if (Char.myCharz().mobFocus != null && GameScr.canAutoPlay && mSystem.currentTimeMillis() - lastTimeTeleportToMob > 100 && Res.distance(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y) > 50)
        {
            lastTimeTeleportToMob = mSystem.currentTimeMillis();
            Char.myCharz().cx = Char.myCharz().mobFocus.x;
            Char.myCharz().cy = Char.myCharz().mobFocus.y;
            Service.gI().charMove();
        }
        if (!GameScr.canAutoPlay && Char.myCharz().mobFocus != null)
        {
            bool flag = Char.myCharz().mobFocus.getTemplate().type == 4;
            long num3 = mSystem.currentTimeMillis();
            if (flag)
            {
                if (isHandlingFlyingMob && num3 - lastFlyingMobTeleTime <= 500)
                {
                    return;
                }
                if (!isHandlingFlyingMob)
                {
                    MainXmapCL.TeleportTo(Char.myCharz().mobFocus.x, MainXmapCL.GetYGround(Char.myCharz().mobFocus.x));
                    isHandlingFlyingMob = true;
                    lastFlyingMobTeleTime = num3;
                    typeMobChange = Char.myCharz().mobFocus.mobId;
                    return;
                }
                MainXmapCL.TeleportTo(Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y);
                lastFlyingMobTeleTime = num3;
                Skill skill = ChooseSkill();
                if (skill != null)
                {
                    UseSkillWithProperMethod(skill);
                }
                return;
            }
            int num4 = Res.distance(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().mobFocus.xFirst, Char.myCharz().mobFocus.yFirst);
            if (num4 > 50)
            {
                MainXmapCL.TeleportTo(Char.myCharz().mobFocus.xFirst, Char.myCharz().mobFocus.yFirst);
                typeMobChange = Char.myCharz().mobFocus.mobId;
                return;
            }
            Skill skill2 = ChooseSkill();
            if (skill2 != null)
            {
                UseSkillWithProperMethod(skill2);
            }
            else
            {
                SkillAk();
            }
        }
        else
        {
            Skill skill3 = ChooseSkill();
            if (skill3 != null)
            {
                UseSkillWithProperMethod(skill3);
            }
            else
            {
                SkillAk();
            }
        }
    }

    private static void UseSkillWithProperMethod(Skill skill)
    {
        bool flag = skill != null && !TYPEAK && GameScr.canAutoPlay;
        if (flag)
        {
            GameScr.gI().doSelectSkill(skill, true);
        }
        else
        {
            int id = (int)skill.template.id;
            bool flag2 = NO_FOCUS_SKILLS.Contains(id);
            if (flag2)
            {
                GameScr.gI().doSelectSkill(skill, true);
                sbyte skillNotFocusStatus = GetSkillNotFocusStatus(id);
                Service.gI().skill_not_focus(skillNotFocusStatus);
            }
            else
            {
                bool flag3 = skill == Char.myCharz().myskill && (id == 0 || id == 17 || id == 4 || id == 2 || id == 9 || id == 1 || id == 5 || id == 3);
                if (flag3)
                {
                    AutoSkill.AutoSendAttack();
                }
                else
                {
                    GameScr.gI().doSelectSkill(skill, true);
                }
            }
        }
    }

    private static sbyte GetSkillNotFocusStatus(int skillId)
    {
        return skillId switch
        {
            6 => 0,
            8 => 1,
            12 => 8,
            13 => 6,
            19 => 9,
            21 => 10,
            _ => 0,
        };
    }

    public static void SkillAk()
    {
        Skill skill = null;
        HashSet<int> hashSet = new HashSet<int>();
        bool flag = false;
        SkillTrain[] skillTrains = SkillTrainTab.SkillTrains;
        SkillTrain[] array = skillTrains;
        SkillTrain[] array2 = array;
        foreach (SkillTrain skillTrain in array2)
        {
            if (skillTrain != null && skillTrain.AutoFlag)
            {
                hashSet.Add(skillTrain.Id);
                if (skillTrain.Id == 9 || skillTrain.Id == 17)
                {
                    flag = true;
                }
            }
        }
        for (int j = 0; j < Char.myCharz().vSkill.size(); j++)
        {
            if (Char.myCharz().vSkill.elementAt(j) is Skill skill2 && hashSet.Contains(skill2.template.id))
            {
                int id = skill2.template.id;
                if ((id == 0 || id == 2 || id == 4) && !flag)
                {
                    skill = skill2;
                    break;
                }
                if (id == 9 || id == 17)
                {
                    skill = skill2;
                    break;
                }
            }
        }
        if (skill != null)
        {
            UseSkillWithProperMethod(skill);
        }
    }

    public static void SkillAkServer()
    {
        Skill skill = null;
        for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
        {
            if (Char.myCharz().vSkill.elementAt(i) is Skill skill2)
            {
                int id = skill2.template.id;
                if (id == 17)
                {
                    skill = skill2;
                    break;
                }
                if (id == 0 || id == 2 || id == 4)
                {
                    skill = skill2;
                }
            }
        }
        if (skill != null && Char.myCharz().myskill != skill)
        {
            GameScr.gI().doSelectSkill(skill, isShortcut: true);
        }
        AutoSkill.SendAttackToMobFocus();
    }

    public static void UseGrape()
    {
        for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
        {
            Item item = Char.myCharz().arrItemBag[i];
            if (item != null && item.template.id == 212)
            {
                Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                return;
            }
        }
        for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
        {
            Item item2 = Char.myCharz().arrItemBag[j];
            if (item2 != null && item2.template.id == 211)
            {
                Service.gI().useItem(0, 1, (sbyte)item2.indexUI, -1);
                break;
            }
        }
    }

    public static Skill ChooseSkill()
    {
        Skill skill = null;
        bool flag = false;
        for (int i = 0; i < SkillTrainTab.SkillTrains.Length; i++)
        {
            if (SkillTrainTab.SkillTrains[i] != null && SkillTrainTab.SkillTrains[i].Id == 17 && SkillTrainTab.SkillTrains[i].AutoFlag)
            {
                flag = true;
                break;
            }
        }
        for (int j = 0; j < Char.myCharz().vSkill.size(); j++)
        {
            if (Char.myCharz().vSkill.elementAt(j) == null)
            {
                continue;
            }
            Skill skill2 = (Skill)Char.myCharz().vSkill.elementAt(j);
            if ((flag && skill2.template.id == 2) || skill2.template.id == 7)
            {
                continue;
            }
            bool flag2 = false;
            for (int k = 0; k < SkillTrainTab.SkillTrains.Length; k++)
            {
                if (SkillTrainTab.SkillTrains[k] != null && SkillTrainTab.SkillTrains[k].Id == skill2.template.id && SkillTrainTab.SkillTrains[k].AutoFlag)
                {
                    flag2 = true;
                    break;
                }
            }
            if (!flag2 || skill2.paintCanNotUseSkill || (Char.myCharz().isMonkey == 1 && skill2.template.id == 13))
            {
                continue;
            }
            long num = ((skill2.template.manaUseType == 2) ? 1 : ((skill2.template.manaUseType == 1) ? (skill2.manaUse * Char.myCharz().cMPFull / 100) : skill2.manaUse));
            if (Char.myCharz().cMP >= num)
            {
                if (skill == null)
                {
                    skill = skill2;
                }
                else if (skill.coolDown < skill2.coolDown)
                {
                    skill = skill2;
                }
            }
        }
        return skill;
    }

    private static List<int> GetCurrentMapMobList()
    {
        int mapID = TileMap.mapID;
        if (!listMobIds.ContainsKey(mapID))
        {
            listMobIds[mapID] = new List<int>();
        }
        return listMobIds[mapID];
    }

    public static void changeclothes()
    {
        while (AutoChangeClothes)
        {
            if (Char.myCharz().meDead)
            {
                Thread.Sleep(1000);
            }
            else if (Char.myCharz().isWaitMonkey)
            {
                Thread.Sleep(1000);
            }
            else if (Char.myCharz().isMonkey == 1)
            {
                AutoItem.useSet(0);
                Thread.Sleep(2000);
            }
            else
            {
                AutoItem.useSet(1);
                Thread.Sleep(2000);
            }
        }
    }

    public static void LoadData()
    {
        TYPEAK = Rms.loadRMSInt("TYPETRAIN") == 1;
    }
}
