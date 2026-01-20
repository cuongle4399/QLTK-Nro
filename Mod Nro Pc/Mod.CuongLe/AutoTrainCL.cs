using DoHoa.CustomMenu;
using Mod.community;
using System.Collections.Generic;
using System.Threading;
using Xmap;

namespace Mod.CuongLe
{
    public class AutoTrainCL : IActionListener, IChatable
    {
        private static AutoTrainCL _Instance;
        public static AutoTrainCL getInstance() => _Instance ??= new AutoTrainCL();

        public static bool isAvoidSuperMob;
        public static bool isGoBack;
        public static bool isGobackCoordinate;
        public static int gobackX;
        public static int gobackY;
        public static int gobackMapID;
        public static int gobackZoneID;
        public static bool isAutoTrain;
        public static int minimumMPGoHome = 5;

        private static readonly string[] inputMPPercentGoHome = { "Nhập %MP", "%MP" };
        private static readonly string[] inputHPAboveMobTRain = { "Nhập hp quái chỉ đánh khi mục tiêu trên", "hp" };
        private static readonly string[] inputHPBelowMobTRain = { "Nhập hp quái chỉ đánh khi mục tiêu dưới", "hp" };

        public static Dictionary<int, List<int>> listMobIds = new Dictionary<int, List<int>>();
        public static long lastTimeAddNewMob;
        private static long lastTimeTeleportToMob;

        public static bool AutoChangeClothes;
        private static int typeMobChange = -1;

        private static bool isHandlingFlyingMob;
        private static long lastFlyingMobTeleTime;

        public static bool autoHopThe;
        public static long lastHopTheTime;

        public static bool autoNeBoss;
        public static long lastUpdateNeBoss;

        public static bool ReturnedGoback = true;

        public static bool autoChangeZone;
        public static long lastautoChangeZone;

        public static bool SpamChangeZone;
        public static long lastSpamChangeZone;

        public static bool TYPEAK;
        private static readonly HashSet<int> NO_FOCUS_SKILLS = new HashSet<int> { 6, 8, 12, 13, 19, 21 };

        public static long OnlyHitWhenAboveHP = 0L;
        public static long OnlyHitWhenBelowHP = long.MaxValue;

        public static bool checkLag;
        private static long lastCheckLagTime;
        private static long lastRecordedCPower;

        public static void LoadData()
        {
            TYPEAK = Rms.loadRMSInt("TYPETRAIN") == 1;
        }

        public static void updateAutoHopThe()
        {
            var me = Char.myCharz();
            if (!autoHopThe || me.isNhapThe || me.meDead || GameCanvas.gameTick % 5 != 0)
                return;

            long now = mSystem.currentTimeMillis();
            if (now - lastHopTheTime < 1000)
                return;

            int potaraIndex = ModProCL.ExistPotara();
            if (potaraIndex != -1)
            {
                Item item = ModProCL.FindItemBag(potaraIndex);
                if (item != null)
                {
                    Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                    lastHopTheTime = now;
                }
            }
        }

        public static void updateNeBoss()
        {
            if (!autoNeBoss || mSystem.currentTimeMillis() - lastUpdateNeBoss < 1200)
                return;

            lastUpdateNeBoss = mSystem.currentTimeMillis();
            if (AutoBossCL.checkBoss())
            {
                AutoBossCL.offPaintZone = true;
                Service.gI().openUIZone();
                gobackZoneID = UnityEngine.Random.Range(0, GameScr.gI().zones.Length);
                GameScr.isChangeZone = true;
                if (!isGoBack)
                {
                    Service.gI().requestChangeZone(gobackZoneID, -1);
                }
            }
        }

        public static void UpdateAutoChangeZoneItNguoi()
        {
            var me = Char.myCharz();
            if (!autoChangeZone ||
                mSystem.currentTimeMillis() - lastautoChangeZone < (GameScr.canAutoPlay ? 5700 : 11000) ||
                (isAutoTrain && me.mobFocus != null) ||
                TileMap.mapID == me.cgender + 21)
            {
                return;
            }

            lastautoChangeZone = mSystem.currentTimeMillis();
            TryFindAndChangeZone(isSpam: false);
        }

        public static void UpdateSpamChangeZoneItNguoi()
        {
            var me = Char.myCharz();
            if (!SpamChangeZone ||
                mSystem.currentTimeMillis() - lastSpamChangeZone < (GameScr.canAutoPlay ? 5700 : 11000) ||
                (isAutoTrain && me.mobFocus != null) ||
                TileMap.mapID == me.cgender + 21)
            {
                return;
            }

            lastSpamChangeZone = mSystem.currentTimeMillis();
            TryFindAndChangeZone(isSpam: true);
        }

        private static void TryFindAndChangeZone(bool isSpam)
        {
            AutoBossCL.offPaintZone = true;
            Service.gI().openUIZone();

            int currentZone = TileMap.zoneID;
            int currentNumPlayer = GameScr.gI().numPlayer[currentZone];

            if (!isSpam && currentNumPlayer <= 1) return;

            int bestZone = -1;
            int minPlayer = int.MaxValue;

            if (isSpam)
            {
                for (int i = GameScr.gI().zones.Length - 1; i >= 0; i--)
                {
                    if (GameScr.gI().zones[i] == currentZone) continue;

                    int num = GameScr.gI().numPlayer[i];
                    int max = GameScr.gI().maxPlayer[i];

                    if (num < max)
                    {
                        if (num == 0)
                        {
                            bestZone = i;
                            break;
                        }
                        if (num < minPlayer)
                        {
                            minPlayer = num;
                            bestZone = i;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameScr.gI().zones.Length; i++)
                {
                    if (i == currentZone) continue;

                    int num = GameScr.gI().numPlayer[i];
                    int max = GameScr.gI().maxPlayer[i];

                    if (num < max)
                    {
                        if (num == 0)
                        {
                            bestZone = i;
                            minPlayer = 0;
                            break;
                        }
                        if (num < minPlayer)
                        {
                            minPlayer = num;
                            bestZone = i;
                        }
                    }
                }
            }

            bool shouldChange = bestZone != -1;
            if (!isSpam) shouldChange = shouldChange && (minPlayer + 1 < currentNumPlayer);

            if (shouldChange)
            {
                gobackZoneID = bestZone;
                GameScr.isChangeZone = true;
                if (!isGoBack)
                {
                    Service.gI().requestChangeZone(gobackZoneID, -1);
                }
            }
        }

        public static void Update()
        {

            var me = Char.myCharz();

            if (ReturnedGoback && !MainXmapCL.isXmaping)
            {
                UpdateCheckLag();
                updateNeBoss();
                UpdateAutoChangeZoneItNguoi();
                UpdateSpamChangeZoneItNguoi();
            }

            updateAutoHopThe();

            if (me.mobFocus != null && (me.mobFocus.hp <= 0 || me.mobFocus.status == 1 || me.mobFocus.status == 0))
            {
                me.mobFocus = null;
            }

            if (isAutoTrain && GameCanvas.gameTick % 20 == 0)
            {
                if (!GameScr.canAutoPlay)
                {
                    TuMoTDLT();
                }
                DoIt();
            }

            if (me.cStamina <= 5 && GameCanvas.gameTick % 140 == 0)
            {
                UseGrape();
            }

            if (!isGoBack) return;

            ReturnedGoback = false;

            if (me.meDead && GameCanvas.gameTick % 180 == 0)
            {
                Service.gI().returnTownFromDead();
            }

            if (isMeOutOfMpOR1HP() && MainXmapCL.isEatChicken)
            {
                int homeMapId = 21 + me.cgender;
                if (TileMap.mapID != homeMapId)
                {
                    GameScr.isAutoPlay = false;
                    me.mobFocus = null;
                    if (GameCanvas.gameTick % 60 == 0 && !MainXmapCL.isXmaping)
                    {
                        MainXmapCL.StartGoToMap(homeMapId);
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
            else
            {
                if (!isGobackCoordinate && GameCanvas.gameTick % 140 == 0)
                {
                    GameScr.isAutoPlay = true;
                }

                if (TileMap.zoneID != gobackZoneID && !Char.ischangingMap && !Controller.isStopReadMessage && GameCanvas.gameTick % 120 == 0)
                {
                    Service.gI().requestChangeZone(gobackZoneID, -1);
                }

                if (isGobackCoordinate && (me.cx != gobackX || me.cy != gobackY) && GameCanvas.gameTick % 140 == 0)
                {
                    TeleportTo(gobackX, gobackY);
                }

                if (TileMap.mapID == gobackMapID && TileMap.zoneID == gobackZoneID &&
                    (!isGobackCoordinate || (me.cx == gobackX && me.cy == gobackY)) &&
                    GameCanvas.gameTick % 140 == 0)
                {
                    ReturnedGoback = true;
                }
            }
        }

        public void onChatFromMe(string text, string to)
        {
            if (!string.IsNullOrEmpty(ChatTextField.gI().tfChat.getText()) && !string.IsNullOrEmpty(text))
            {
                string chatStr = ChatTextField.gI().strChat;
                string inputVal = ChatTextField.gI().tfChat.getText();

                if (chatStr.Equals(inputMPPercentGoHome[0]))
                {
                    if (int.TryParse(inputVal, out int val))
                    {
                        minimumMPGoHome = val;
                        GameScr.info1.addInfo($"Về Nhà Khi MP Dưới\n[{minimumMPGoHome}%]");
                    }
                    else GameScr.info1.addInfo("%MP Không Hợp Lệ");
                    ResetChatTextField();
                }
                else if (chatStr.Equals(inputHPAboveMobTRain[0]))
                {
                    if (long.TryParse(inputVal, out long val))
                    {
                        OnlyHitWhenAboveHP = val;
                        GameScr.info1.addInfo($"Chỉ đánh quái khi HP trên {Res.formatNumber2(OnlyHitWhenAboveHP)}");
                    }
                    else
                    {
                        GameScr.info1.addInfo("HP không hợp lệ");
                        OnlyHitWhenAboveHP = 0L;
                    }
                    ResetChatTextField();
                }
                else if (chatStr.Equals(inputHPBelowMobTRain[0]))
                {
                    if (long.TryParse(inputVal, out long val))
                    {
                        OnlyHitWhenBelowHP = val;
                        GameScr.info1.addInfo($"Chỉ đánh quái khi HP dưới {Res.formatNumber2(OnlyHitWhenBelowHP)}");
                    }
                    else
                    {
                        GameScr.info1.addInfo("HP không hợp lệ");
                        OnlyHitWhenBelowHP = long.MaxValue;
                    }
                    ResetChatTextField();
                }
            }
            else
            {
                Service.gI().chat(text);
                ChatTextField.gI().isShow = false;
            }
        }

        public void onCancelChat()
        {
            ResetChatTextField();
        }

        public void perform(int idAction, object p)
        {
            var me = Char.myCharz();
            switch (idAction)
            {
                case 1:
                    {
                        int templateId = (int)p;
                        var list = GetCurrentMapMobList();
                        list.Clear();
                        for (int i = 0; i < GameScr.vMob.size(); i++)
                        {
                            Mob mob = (Mob)GameScr.vMob.elementAt(i);
                            if (!mob.isMobMe && mob.templateId == templateId) list.Add(mob.mobId);
                        }
                        MobTrainTab.UpdateMobTrainFlags();
                        TurnOnAutoTrain();
                        break;
                    }
                case 2:
                    {
                        var list = GetCurrentMapMobList();
                        list.Clear();
                        for (int i = 0; i < GameScr.vMob.size(); i++)
                        {
                            Mob mob = (Mob)GameScr.vMob.elementAt(i);
                            if (!mob.isMobMe) list.Add(mob.mobId);
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
                    if (me.mobFocus == null) GameScr.info1.addInfo("Vui Lòng Chọn Quái!");
                    else
                    {
                        GetCurrentMapMobList().Add(me.mobFocus.mobId);
                        GameScr.info1.addInfo("Đã Thêm Quái: " + me.mobFocus.mobId);
                    }
                    break;
                case 8:
                    isAutoTrain = false;
                    me.mobFocus = null;
                    GameScr.info1.addInfo("Đã Tắt Auto Train!");
                    break;
                case 9:
                    if (isGoBack)
                    {
                        isGoBack = false;
                        GameScr.info1.addInfo("Goback\n[STATUS: OFF]");
                    }
                    else
                    {
                        isGobackCoordinate = false;
                        isGoBack = true;
                        gobackMapID = TileMap.mapID;
                        gobackZoneID = TileMap.zoneID;
                        GameScr.info1.addInfo($"Goback\n[{TileMap.mapNames[gobackMapID]}]\n[{gobackZoneID}]");
                    }
                    break;
                case 10:
                    if (isGoBack)
                    {
                        isGoBack = false;
                        GameScr.info1.addInfo("Goback\n[STATUS: OFF]");
                    }
                    else
                    {
                        isGobackCoordinate = true;
                        isGoBack = true;
                        gobackMapID = TileMap.mapID;
                        gobackZoneID = TileMap.zoneID;
                        gobackX = me.cx;
                        gobackY = me.cy;
                        GameScr.info1.addInfo($"Goback Tọa Độ\n[{gobackX}-{gobackY}]");
                    }
                    break;
                case 11:
                    OpenChat(inputMPPercentGoHome, TField.INPUT_TYPE_NUMERIC);
                    break;
                case 12:
                    string gender = me.getGender();
                    if (gender == "TĐ" || gender == "NM")
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
                    if (AutoChangeClothes) new Thread(changeclothes).Start();
                    break;
                case 13:
                    autoChangeZone = !autoChangeZone;
                    if (TileMap.mapID == me.cgender + 21)
                    {
                        SpamChangeZone = false;
                        ChatPopup.addChatPopupMultiLineGameline("Trong nhà mà auto đổi khu cái đjt mọe mày à");
                    }
                    else
                    {
                        autoNeBoss = false;
                        GameScr.info1.addInfo("Auto đổi khu ít người: " + (autoChangeZone ? "Bật" : "Tắt"));
                    }
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
                        else if (TileMap.mapID == me.cgender + 21)
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
                    OpenChat(inputHPAboveMobTRain, TField.INPUT_TYPE_NUMERIC);
                    break;
                case 19:
                    OpenChat(inputHPBelowMobTRain, TField.INPUT_TYPE_NUMERIC);
                    break;
                case 20:
                    ShowMenuConfigHPTrainMob();
                    break;
                case 21:
                    SpamChangeZone = !SpamChangeZone;
                    if (TileMap.mapID == me.cgender + 21)
                    {
                        SpamChangeZone = false;
                        ChatPopup.addChatPopupMultiLineGameline("Trong nhà mà auto đổi khu cái đjt mọe mày à");
                    }
                    else
                    {
                        autoNeBoss = false;
                        GameScr.info1.addInfo("Spam đổi khu ít người: " + (SpamChangeZone ? "Bật" : "Tắt"));
                    }
                    break;
            }
        }

        private void OpenChat(string[] config, int type)
        {
            ChatTextField.gI().strChat = config[0];
            ChatTextField.gI().tfChat.name = config[1];
            ChatTextField.gI().tfChat.setIputType(type);
            ChatTextField.gI().startChat2(getInstance(), string.Empty);
        }
        public static void ShowMenuKhuIt()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Auto Khu ít: " + (autoChangeZone ? "Bật" : "Tắt"), getInstance(), 13, null));
            myVector.addElement(new Command("Spam Khu ít: " + (SpamChangeZone ? "Bật" : "Tắt"), getInstance(), 21, null));
            GameCanvas.menu.startAt(myVector, 3);
        }

        public static void ShowMenuGoback()
        {
            string gobackStatus = isGoBack ? $"[{TileMap.mapNames[gobackMapID]}]\n[{gobackZoneID}]" : "[STATUS: OFF]";
            string coordStatus = (!isGoBack || !isGobackCoordinate) ? "[STATUS: OFF]" : $"[{gobackX}-{gobackY}]";

            MyVector myVector = new MyVector();
            myVector.addElement(new Command($"Goback\n{gobackStatus}", getInstance(), 9, null));
            myVector.addElement(new Command($"Goback Tọa Độ\n{coordStatus}", getInstance(), 10, null));
            myVector.addElement(new Command($"Về Nhà Khi MP Dưới\n[{minimumMPGoHome}%]", getInstance(), 11, null));
            GameCanvas.menu.startAt(myVector, 3);
        }

        public static void ShowMenuConfigHPTrainMob()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Chỉ đánh quái hp trên: " + Res.formatNumber2(OnlyHitWhenAboveHP), getInstance(), 18, null));
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
            var me = Char.myCharz();
            if (GameScr.canAutoPlay)
            {
                me.cx = x;
                me.cy = y;
                Service.gI().charMove();
                return;
            }
            me.cx = x;
            me.cy = y;
            Service.gI().charMove();
            me.cx = x;
            me.cy = y + 1;
            Service.gI().charMove();
            me.cx = x;
            me.cy = y;
            Service.gI().charMove();
        }

        private static bool isMeCanAttack(Mob mob)
        {
            if (!GameScr.canAutoPlay && mob.checkIsBoss())
            {
                if (mob.checkIsBoss()) return isAvoidSuperMob;
                return false;
            }
            return true;
        }

        private static bool isMeOutOfMpOR1HP()
        {
            var me = Char.myCharz();
            return me.cMP < me.cMPFull * minimumMPGoHome / 100 || me.cHP == 1;
        }

        private static Mob GetNextMob(int type)
        {
            List<int> currentMapMobList = GetCurrentMapMobList();
            if (currentMapMobList.Count == 0) return null;

            var me = Char.myCharz();

            if (type == 1)
            {
                long currentTime = mSystem.currentTimeMillis();
                Mob result = null;
                for (int i = 0; i < currentMapMobList.Count; i++)
                {
                    Mob mob = (Mob)GameScr.vMob.elementAt(currentMapMobList[i]);
                    if (!mob.isMobMe && mob.status != 0 && mob.cTimeDie < currentTime &&
                        isMeCanAttack(mob) && mob.hp > OnlyHitWhenAboveHP && mob.hp < OnlyHitWhenBelowHP)
                    {
                        result = mob;
                        currentTime = mob.cTimeDie;
                    }
                }
                return result;
            }
            else
            {
                Mob result = null;
                int minDistance = int.MaxValue;
                for (int j = 0; j < currentMapMobList.Count; j++)
                {
                    Mob mob = (Mob)GameScr.vMob.elementAt(currentMapMobList[j]);
                    if (mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isMobMe &&
                        isMeCanAttack(mob) && mob.hp > OnlyHitWhenAboveHP && mob.hp < OnlyHitWhenBelowHP)
                    {
                        int dist = Math.Abs(me.cx - mob.xFirst) + Math.Abs(me.cy - mob.yFirst);
                        if (dist < minDistance)
                        {
                            result = mob;
                            minDistance = dist;
                        }
                    }
                }
                return result;
            }
        }

        public static void TuMoTDLT()
        {
            try
            {
                if (!ModProCL.ExistItemBag(521) || ItemTime.isExistItem(4387) || AutoBuyItemCL.timeItemDatBiet(521) == 0)
                    return;

                var arrItem = Char.myCharz().arrItemBag;
                for (int i = 0; i < arrItem.Length; i++)
                {
                    Item item = arrItem[i];
                    if (item != null && item.template.id == 521)
                    {
                        Service.gI().useItem(0, 1, (sbyte)i, -1);
                        break;
                    }
                }
            }
            catch { }
        }

        private static void TurnOnAutoTrain()
        {
            if (GetCurrentMapMobList().Count == 0)
            {
                GameScr.info1.addInfo("Danh Sách Tàn Sát Trống!");
                isAutoTrain = false;
                lastCheckLagTime = 0;
            }
            else
            {
                isAutoTrain = true;
            }
        }

        private static void DoIt()
        {
            var me = Char.myCharz();
            if (!isAutoTrain || me.statusMe == 14 || me.statusMe == 5 || AutoBossCL.tanCongBoss ||
                ModProCL.tieuDietNguoiBatCo || MainXmapCL.isXmaping || me.isWaitMonkey || me.isCharge ||
                (isGoBack && isMeOutOfMpOR1HP()))
            {
                return;
            }

            if (me.mobFocus != null && !me.mobFocus.isMobMe)
            {
                if (me.mobFocus.hp <= 0 || me.mobFocus.status == 1 || me.mobFocus.status == 0 ||
                    !isMeCanAttack(me.mobFocus) ||
                    me.mobFocus.hp < OnlyHitWhenAboveHP || me.mobFocus.hp > OnlyHitWhenBelowHP)
                {
                    me.mobFocus = null;
                    isHandlingFlyingMob = false;
                }
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

            if (me.mobFocus == null)
            {
                if (!GameScr.canAutoPlay && AutoPick.isAutoPick)
                {
                    AutoPick.FocusToNearestItem();
                    if (me.itemFocus != null)
                    {
                        ItemMap itemFocus = me.itemFocus;
                        int distX = Math.Abs(me.cx - itemFocus.x);
                        int distY = Math.Abs(me.cy - itemFocus.y);
                        if (distX > 50 || distY > 50)
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
                    me.itemFocus = null;
                }

                if (me.itemFocus == null)
                {
                    Mob nextMob = GetNextMob(0);
                    me.clearFocus(0);
                    if (nextMob == null)
                    {
                        nextMob = GetNextMob(1);
                        me.clearFocus(0);
                        if (nextMob == null) return;

                        if (!GameScr.canAutoPlay)
                        {
                            me.currentMovePoint = new MovePoint(nextMob.xFirst, nextMob.yFirst);
                            me.endMovePointCommand = new Command(null, null, 8002, null);
                        }
                    }
                    else
                    {
                        me.mobFocus = nextMob;
                        if (GameScr.canAutoPlay)
                        {
                            me.cx = nextMob.x;
                            me.cy = nextMob.y;
                            Service.gI().charMove();
                        }
                    }
                    isHandlingFlyingMob = false;
                }
            }

            if (me.mobFocus == null || (me.skillInfoPaint() != null && me.indexSkill < me.skillInfoPaint().Length && me.dart != null && me.arr != null))
            {
                return;
            }

            if (me.mobFocus != null && GameScr.canAutoPlay && mSystem.currentTimeMillis() - lastTimeTeleportToMob > 100 &&
    Res.distance(me.cx, me.cy, me.mobFocus.x, me.mobFocus.y) > 50)
            {
                lastTimeTeleportToMob = mSystem.currentTimeMillis();
                me.cx = me.mobFocus.x;
                me.cy = me.mobFocus.y;
                Service.gI().charMove();
            }

            if (!GameScr.canAutoPlay && me.mobFocus != null)
            {
                bool isFlyingMob = me.mobFocus.getTemplate().type == 4;
                long now = mSystem.currentTimeMillis();

                if (isFlyingMob)
                {
                    if (isHandlingFlyingMob && now - lastFlyingMobTeleTime <= 500) return;

                    if (!isHandlingFlyingMob)
                    {
                        MainXmapCL.TeleportTo(me.mobFocus.x, MainXmapCL.GetYGround(me.mobFocus.x));
                        isHandlingFlyingMob = true;
                        lastFlyingMobTeleTime = now;
                        typeMobChange = me.mobFocus.mobId;
                        return;
                    }

                    MainXmapCL.TeleportTo(me.mobFocus.x, me.mobFocus.y);
                    lastFlyingMobTeleTime = now;
                    Skill skill = ChooseSkill();
                    if (skill != null) UseSkillWithProperMethod(skill);
                    return;
                }

                int distFirst = Res.distance(me.cx, me.cy, me.mobFocus.xFirst, me.mobFocus.yFirst);
                if (distFirst > 50)
                {
                    MainXmapCL.TeleportTo(me.mobFocus.xFirst, me.mobFocus.yFirst);
                    typeMobChange = me.mobFocus.mobId;
                    return;
                }

                Skill chosenSkill = ChooseSkill();
                if (chosenSkill != null) UseSkillWithProperMethod(chosenSkill);
                else SkillAk();
            }
            else
            {
                Skill chosenSkill = ChooseSkill();
                if (chosenSkill != null) UseSkillWithProperMethod(chosenSkill);
                else SkillAk();
            }
        }

        private static void UseSkillWithProperMethod(Skill skill)
        {
            if (skill == null) return;

            if (!TYPEAK && GameScr.canAutoPlay)
            {
                GameScr.gI().doSelectSkill(skill, isShortcut: true);
                return;
            }

            int id = skill.template.id;
            var me = Char.myCharz();

            if (NO_FOCUS_SKILLS.Contains(id))
            {
                GameScr.gI().doSelectSkill(skill, isShortcut: true);
                sbyte skillNotFocusStatus = GetSkillNotFocusStatus(id);
                Service.gI().skill_not_focus(skillNotFocusStatus);
            }
            else if (skill == me.myskill && (id == 0 || id == 17 || id == 4 || id == 2 || id == 9 || id == 1 || id == 5 || id == 3))
            {
                AutoSkill.AutoSendAttackVIP();
            }
            else
            {
                GameScr.gI().doSelectSkill(skill, isShortcut: true);
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
            HashSet<int> allowedSkills = new HashSet<int>();
            bool hasSpecial = false;

            foreach (var st in SkillTrainTab.SkillTrains)
            {
                if (st != null && st.AutoFlag)
                {
                    allowedSkills.Add(st.Id);
                    if (st.Id == 9 || st.Id == 17) hasSpecial = true;
                }
            }

            var me = Char.myCharz();
            for (int j = 0; j < me.vSkill.size(); j++)
            {
                if (me.vSkill.elementAt(j) is Skill s && allowedSkills.Contains(s.template.id))
                {
                    int id = s.template.id;
                    if ((id == 0 || id == 2 || id == 4) && !hasSpecial)
                    {
                        skill = s;
                        break;
                    }
                    if (id == 9 || id == 17)
                    {
                        skill = s;
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
            var me = Char.myCharz();
            for (int i = 0; i < me.vSkill.size(); i++)
            {
                if (me.vSkill.elementAt(i) is Skill s)
                {
                    int id = s.template.id;
                    if (id == 17)
                    {
                        skill = s;
                        break;
                    }
                    if (id == 0 || id == 2 || id == 4)
                    {
                        skill = s;
                    }
                }
            }
            if (skill != null && me.myskill != skill)
            {
                GameScr.gI().doSelectSkill(skill, isShortcut: true);
            }
            AutoSkill.SendAttackToMobFocus();
        }

        public static void UseGrape()
        {
            var arr = Char.myCharz().arrItemBag;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null && arr[i].template.id == 212)
                {
                    Service.gI().useItem(0, 1, (sbyte)arr[i].indexUI, -1);
                    return;
                }
            }
            for (int j = 0; j < arr.Length; j++)
            {
                if (arr[j] != null && arr[j].template.id == 211)
                {
                    Service.gI().useItem(0, 1, (sbyte)arr[j].indexUI, -1);
                    break;
                }
            }
        }

        public static Skill ChooseSkill()
        {
            Skill skill = null;
            bool hasSkill17 = false;

            for (int i = 0; i < SkillTrainTab.SkillTrains.Length; i++)
            {
                if (SkillTrainTab.SkillTrains[i] != null && SkillTrainTab.SkillTrains[i].Id == 17 && SkillTrainTab.SkillTrains[i].AutoFlag)
                {
                    hasSkill17 = true;
                    break;
                }
            }

            var me = Char.myCharz();
            for (int j = 0; j < me.vSkill.size(); j++)
            {
                if (me.vSkill.elementAt(j) == null) continue;
                Skill s = (Skill)me.vSkill.elementAt(j);

                if ((hasSkill17 && s.template.id == 2) || s.template.id == 7) continue;

                bool isConfigured = false;
                for (int k = 0; k < SkillTrainTab.SkillTrains.Length; k++)
                {
                    if (SkillTrainTab.SkillTrains[k] != null && SkillTrainTab.SkillTrains[k].Id == s.template.id && SkillTrainTab.SkillTrains[k].AutoFlag)
                    {
                        isConfigured = true;
                        break;
                    }
                }

                if (!isConfigured || s.paintCanNotUseSkill ||
                    (me.isMonkey == 1 && s.template.id == 13) ||
                    (s.template.id == 19 && ModProCL.checkItemTime(3784)))
                {
                    continue;
                }

                long manaUse = (s.template.manaUseType == 2) ? 1 :
                               (s.template.manaUseType == 1 ? (s.manaUse * me.cMPFull / 100) : s.manaUse);

                if (me.cMP >= manaUse)
                {
                    if (skill == null) skill = s;
                    else if (skill.coolDown < s.coolDown) skill = s;
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
                var me = Char.myCharz();
                if (me.meDead || me.isWaitMonkey)
                {
                    Thread.Sleep(1000);
                }
                else if (me.isMonkey == 1)
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
        public static void UpdateCheckLag()
        {
            if (!checkLag || MainXmapCL.isXmaping)
                return;

            var me = Char.myCharz();
            long now = mSystem.currentTimeMillis();

            if (lastCheckLagTime == 0)
            {
                lastCheckLagTime = now;
                lastRecordedCPower = me.cPower;
                return;
            }

            if (now - lastCheckLagTime >= 300000)
            {
                if (me.cPower == lastRecordedCPower)
                {
                    GameCanvas.gI().onDisconnected();
                    GameScr.info1.addInfo("Phát hiện lag! Ngắt kết nối.");
                }

                lastCheckLagTime = now;
                lastRecordedCPower = me.cPower;
            }
        }
    }
}