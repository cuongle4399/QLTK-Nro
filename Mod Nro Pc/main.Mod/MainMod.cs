using Assets.src.g;
using DoHoa;
using DoHoa.CustomMenu;
using DoHoa.CustomMenu.Shared;
using Mod.community;
using Mod.CuongLe;
using Mod_nro.MenuDataGame;
using ModCak.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using Xmap;

namespace main.Mod;

public class MainMod : IActionListener, IChatable
{
    public static MainMod _Instance;

    public static int int_0;

    public static int runSpeed;

    public static bool isAutoRevive;

    public static bool isLockFocus;

    public static int charIDLock;

    public static string[] inputLockFocusCharID;

    public static int zoneIdNRD;

    public static int mapIdNRD;

    public static bool isOpenMenuNPC;

    public static bool isAutoEnterNRDMap;

    public static string[] nameMapsNRD;

    public static int int_4;

    public static bool bool_1;

    public static string[] inputHPPercentFusionDance;

    public static string[] inputHPFusionDance;

    public static int minumumHPPercentFusionDance;

    public static int minimumHPFusionDance;

    public static List<int> listCharIDs;

    public static string[] inputCharID;

    public static bool isAutoLockControl;

    public static bool isAutoTeleport;

    private static string thongbaoVIPne;

    public static bool isAutoAttackBoss;

    public static int HPLimit;

    public static string[] inputHPLimit;

    public static bool isAutoAttackOtherChars;

    public static int limitHPChar;

    public static string[] inputHPChar;

    public static List<Boss> listBosses;

    public static Image logoServerListScreen;

    public static Image logoGameScreen;

    public static List<Image> listBackgroundImages;

    public static List<Color> listFlagColor;

    public static int widthRect;

    public static int heightRect;

    public static List<Char> listCharsInMap;

    public static bool isUsingSkill;

    public static long lastTimeConnected;

    public static bool isUsingCapsule;

    public static string linkFb;

    public static int delay;

    public static int indexBackgroundImages;

    public static long lastTimeChangeBackground;

    public static int server;

    public static string APIKey;

    public static string APIServer;


    public static bool isAutoT77;

    public static bool isAutoBomPicPoc;

    private static bool thongBao;

    public static int time_;

    public static bool toiUuCPU;

    public static long GoldCurrent;

    public static bool infoTrainGold;

    public static bool infoTrainPet;

    public static long GoldUpdate;

    public static long GoldUpdateRealTime;

    public static string VersionMod;

    private static int frameCount;

    private static long lastTime;

    private static int fps;

    private static int stopMap;

    private static bool checkSkill;

    public static bool blockAutoGame;

    public static bool AutoCapCha;

    public static bool configStartGame;

    private static readonly Dictionary<int, long> lastPower;

    private static readonly Dictionary<int, long> lastUpdateTime;

    public static long basePetPower;

    public static bool basePowerSet;

    public static long lastPetInfoCallTime;

    public static string[] inputAutoLoginOffline;

    public static MainMod getInstance()
    {
        if (_Instance == null)
        {
            _Instance = new MainMod();
        }
        return _Instance;
    }

    public static void Update()
    {
        DataItem.HandleInput();
        SocketGame.ProcessMessages();
        AutoPean.Update();
        ModProCL.update();
        MainXmapCL.Update();
        if (!blockAutoGame)
        {
            AutoboMongCL.update();
            AutoTrainCL.Update();
            AutoBossCL.Update();
            AutoBuyItemCL.update();
            YardatCL.update();
            AutoPetCL.update();
            NhapCodeLive.getInstance().update();
        }
        ShowSetKH.update();
        AutoVutDoCL.update();
        if (thongBao && GameCanvas.gameTick % 700 == 0)
        {
            GameScr.gI().chatVip(thongbaoVIPne);
            thongBao = false;
        }

        if (AutoCapCha)
        {
            CaptchaSolver.Update();
        }

        if (GraphicsManagement.isShowCharsInMap)
        {
            listCharsInMap.Clear();
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char obj = (Char)GameScr.vCharInMap.elementAt(i);
                if (obj.cName != null && obj.cName != "" && !obj.isPet && !obj.isMiniPet && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$") && obj.cName != "Trọng tài")
                {
                    listCharsInMap.Add(obj);
                }
            }
        }
        if (isAutoEnterNRDMap)
        {
            EnterNRDMap();
        }
        if (isAutoRevive)
        {
            Revive();
        }
        if (isLockFocus)
        {
            FocusTo(charIDLock);
        }
        AutoItem.Update();
        AutoChat.Update();
        AutoSkill.Update();
        AutoPick.Update();
        AutoPoint.Update();
        Char.myCharz().cspeed = runSpeed;
    }

    public static int CalculateFPS()
    {
        frameCount++;
        long num = mSystem.currentTimeMillis();
        long num2 = num - lastTime;
        if (num2 >= 500)
        {
            fps = (int)((double)frameCount * 1000.0 / (double)num2 + 0.5);
            frameCount = 0;
            lastTime = num;
        }
        return fps;
    }

    private static void PaintFocus(mGraphics g)
    {
        string text = null;
        if (Char.myCharz().mobFocus != null)
        {
            text = Char.myCharz().mobFocus.getTemplate().name + " [" + Res.FormatNumberVIP(Char.myCharz().mobFocus.hp) + "/" + Res.FormatNumberVIP(Char.myCharz().mobFocus.maxHp) + "]";
        }
        else if (Char.myCharz().charFocus != null && isBoss(Char.myCharz().charFocus))
        {
            text = Char.myCharz().charFocus.cName + " [" + Res.FormatNumberVIP(Char.myCharz().charFocus.cHP) + "/" + Res.FormatNumberVIP(Char.myCharz().charFocus.cHPFull) + "]";
        }
        if (!string.IsNullOrEmpty(text))
        {
            int x = GameCanvas.w / 2;
            int y = GameScr.logoInGame.getHeight() + 10;
            mFont.tahoma_7_yellow.drawString(g, text, x, y, mFont.CENTER, mFont.tahoma_7_grey);
        }
    }

    public static void Paint(mGraphics g)
    {
        DataItem.Paint(g);
        if (DataItem.IsShow)
        {
            return;
        }
        bool flag = isMeInNRDMap();
        GraphicsManagement.Paint(g);
        if (MainMenu.isShowMenuVIP)
        {
            MainMenu.Paint(g);
            return;
        }
        if (GameScr.logoInGame != null && GraphicsManagement.HienThiLogo)
        {
            g.drawImage(GameScr.logoInGame, GameCanvas.w / 2, 1, 1);
        }
        paintListBosses(g);
        if (GraphicsManagement.isShowCharsInMap)
        {
            paintListCharsInMap(g);
        }
        if (!flag)
        {
            PaintFocus(g);
        }
        int num = 8;
        int num2 = GameCanvas.h - 197;
        GraphicsManagement.DrawFont.drawString(g, TileMap.mapNames[TileMap.mapID] + " [" + TileMap.mapID + "] Zone: " + TileMap.zoneID, 10, num2, 0);
        num2 += num;
        GraphicsManagement.DrawFont.drawString(g, "x: " + Char.myCharz().cx + " y: " + Char.myCharz().cy, 10, num2, 0);
        num2 += num;
        string st = string.Format("[{0}] {1}", CalculateFPS(), SocketGame.IsRunning ? "QLTK: OK" : "QLTK: Đéo");
        int width = GameScr.imgPanel.getWidth();
        int height = GameScr.imgPanel.getHeight();
        GraphicsManagement.DrawFont.drawString(g, st, width / 2 + width * 5 / 100, height - height * 30 / 100, 0);
        if (AutoBossCL.DoBoss)
        {
            GraphicsManagement.DrawFont.drawString(g, "Bắt đầu tìm khu boss từ " + TileMap.zoneID + "->" + AutoBossCL.CountZoneMap, 10, num2, 0);
            num2 += num;
        }
        if (AutoSkill.isAutoSendAttack)
        {
            GraphicsManagement.DrawFont.drawString(g, "Tự đánh: on", 10, num2, 0);
            num2 += num;
        }
        if (isAutoRevive)
        {
            GraphicsManagement.DrawFont.drawString(g, "Hồi sinh: on", 10, num2, 0);
            num2 += num;
        }
        if (AutoPetCL.DeSuaLapem)
        {
            GraphicsManagement.DrawFont.drawString(g, $"Đệ kêu: {AutoPetCL.soLanDeKeu} lần | đã đấm: {AutoPetCL.soLanTanCong} lần", 10, num2, 0);
            num2 += num;
        }
        if (AutoPetCL.TTNL)
        {
            GraphicsManagement.DrawFont.drawString(g, $"Tự động TTNL khi HP dưới {AutoPetCL.PercentCharge}%", 10, num2, 0);
            num2 += num;
        }
        if (AutoPetCL.AutoNhatItemPet)
        {
            GraphicsManagement.DrawFont.drawString(g, $"Số item nhặt từ đệ: {AutoPetCL.SoItemNhatTuPet}", 10, num2, 0);
            num2 += num;
        }

        // ✅ THAY THẾ PHẦN HIỂN THỊ CAPTCHA
        if (CaptchaSolver.IsSolving())
        {
            GraphicsManagement.DrawFont.drawString(g, CaptchaSolver.GetStatus(), 10, num2, 0);
            num2 += num;
            GraphicsManagement.DrawFont.drawString(g, "Số lần giải Capcha thành công: " + CaptchaSolver.GetSolvedCount(), 10, num2, 0);
            num2 += num;
        }

        if (AutoFarmBossNappa.DoSatBossNapa)
        {
            string text = ((AutoFarmBossNappa.typeBoss == 0) ? "Kuku" : ((AutoFarmBossNappa.typeBoss == 1) ? "Mập" : "Rambo"));
            GraphicsManagement.DrawFont.drawString(g, "Auto Farm Nappa: " + text + "- " + AutoFarmBossNappa.statusBossNappa, 10, num2, 0);
            num2 += num;
        }
        if (AutoBossCL.findBossMod)
        {
            GraphicsManagement.DrawFont.drawString(g, "Auto Map Trứng Mabu: on", 10, num2, 0);
            num2 += num;
        }
        if (AutoPick.isAutoPick)
        {
            GraphicsManagement.DrawFont.drawString(g, "Auto nhặt: on", 10, num2, 0);
            num2 += num;
        }
        if (AutoBossCL.aGimBoss)
        {
            GraphicsManagement.DrawFont.drawString(g, "Auto gim Boss:  on", 10, num2, 0);
            num2 += num;
        }
        if (AutoBossCL.AutoteleBoss)
        {
            GraphicsManagement.DrawFont.drawString(g, "Auto tele Boss:  on", 10, num2, 0);
            num2 += num;
        }
        if (AutoBossCL.tanCongBoss)
        {
            GraphicsManagement.DrawFont.drawString(g, "Auto tan cong Boss:  on", 10, num2, 0);
            num2 += num;
        }
        if (AutoboMongCL.autoboMong)
        {
            string difficulty = char.ToUpper(AutoboMongCL.Settings.Difficulty[0]) + AutoboMongCL.Settings.Difficulty.Substring(1);
            string statusText = "Bò Mộng: " + difficulty + "-" + AutoboMongCL.StatusBoMong + " [Hoàn thành: " + AutoboMongCL.completedTasks + " | Hủy: " + AutoboMongCL.cancekTasks + "]";
            GraphicsManagement.DrawFont.drawString(g, statusText, 10, num2, 0);
            num2 += num;
        }
        if (isLockFocus)
        {
            GraphicsManagement.DrawFont.drawString(g, "Khóa: " + charIDLock, 10, num2, 0);
            num2 += num;
        }
        if (isAutoEnterNRDMap)
        {
            GraphicsManagement.DrawFont.drawString(g, "Đang auto nrd: " + mapIdNRD + "sk" + zoneIdNRD, 15, num2, 0);
            num2 += num;
        }
        if (AutoPetCL.aGimPet)
        {
            GraphicsManagement.DrawFont.drawString(g, "Auto Gim Đệ: " + (AutoPetCL.aGimPet ? "Bật" : "Tắt"), 10, num2, 0);
            num2 += num;
        }
        if (!MainXmapCL.isXmaping)
        {
            if (ModProCL.petw)
            {
                infoCharView(g, Char.myPetz(), 10, num2);
                num2 += num * 5;
            }
            if (infoTrainGold)
            {
                infoTrain(g, 10, num2);
                num2 += num * 3;
            }
        }
        if (ModProCL.hienThiDoKH)
        {
            ShowSetKH.paintDOKH(10, num2, g);
            num2 += num;
        }
        if (ModProCL.banDo)
        {
            string sellStatus = "Đang auto bán đồ rác | Đã bán: " + ShowSetKH.GetSellCount() + " cái";
            GraphicsManagement.DrawFont.drawString(g, sellStatus, 10, num2, 0);
            num2 += num;
        }
        if (ModProCL.catDoVIP)
        {
            string storeStatus;
            if (ModProCL.isFULLBox())
            {
                storeStatus = "Rương FULL rồi | Đã cất: " + ShowSetKH.GetStoreCount() + " cái";
            }
            else
            {
                storeStatus = "Đang auto cất đồ (TL+KH) vào rương | Đã cất: " + ShowSetKH.GetStoreCount() + " cái";
            }
            GraphicsManagement.DrawFont.drawString(g, storeStatus, 10, num2, 0);
            num2 += num;
        }
        if (MainXmapCL.isXmaping)
        {
            int width2 = GameScr.imgLbtn.getWidth();
            int height2 = GameScr.imgLbtn.getHeight();
            int num3 = GameCanvas.w / 2 - width2 / 2;
            int num4 = GameCanvas.h / 2 - height2 / 2;
            g.drawImage((stopMap != 1) ? GameScr.imgLbtn : GameScr.imgLbtnFocus, num3, num4);
            int x = num3 + width2 / 2;
            int y = num4 + height2 / 2 - 3;
            mFont.tahoma_7b_dark.drawString(g, "Stop Xmap", x, y, mFont.CENTER);
            string st2 = "Đang tới: " + TileMap.mapNames[MainXmapCL.IdMapEnd];
            if (MainXmapCL.xmapErrr)
            {
                st2 = "Bạn chưa thể tới: " + TileMap.mapNames[MainXmapCL.IdMapEnd];
            }
            GraphicsManagement.DrawFont.drawString(g, st2, x, num4 - 12, mFont.CENTER);
        }
        AutoBuyItemCL.Paint(g, ref num2, num);
    }

    public static void paintListCharsInMap(mGraphics g)
    {
        bool flag = isMeInNRDMap();
        int num = (flag ? 35 : 88);
        widthRect = 120;
        heightRect = 7;
        GUIStyle other = new GUIStyle
        {
            font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
            fontSize = 7 * mGraphics.zoomLevel,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperLeft
        };
        List<string> list = new List<string>();
        if (Char.myCharz().holder)
        {
            if (Char.myCharz().charHold != null)
            {
                list.Add("Bạn đang trói " + Char.myCharz().charHold.cName);
            }
            else if (Char.myCharz().mobHold != null)
            {
                list.Add("Bạn đang trói " + Char.myCharz().mobHold.getTemplate().name);
            }
        }
        for (int i = 0; i < listCharsInMap.Count; i++)
        {
            Char obj = listCharsInMap[i];
            if (obj.cName == null || obj.cName == "" || obj.isPet || obj.isMiniPet || obj.cName.StartsWith("#") || obj.cName.StartsWith("$") || obj.cName == "Trọng tài")
            {
                continue;
            }
            g.setColor(2721889, 0.5f);
            g.fillRect(GameCanvas.w - widthRect, num + 1, widthRect - 2, heightRect);
            paintPKFlag(g, obj, num);
            if (obj.isNRD)
            {
                paintCharNRD(g, obj);
            }
            string text = obj.cName + " [" + NinjaUtil.getMoneys(obj.cHP) + "]";
            bool flag2 = isBoss(obj);
            if (!flag2)
            {
                text = obj.cName + " [" + NinjaUtil.getMoneys(obj.cHP) + " - " + obj.getGender() + "]";
            }
            GUIStyle gUIStyle = new GUIStyle(other);
            if (Char.myCharz().charFocus != null && Char.myCharz().charFocus.cName == obj.cName)
            {
                g.setColor(14155776);
                g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy + 1, obj.cx - GameScr.cmx, obj.cy - GameScr.cmy);
                gUIStyle.normal.textColor = Color.red;
            }
            else if (flag2)
            {
                g.setColor(16383818);
                g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy + 1, obj.cx - GameScr.cmx, obj.cy - GameScr.cmy);
                gUIStyle.normal.textColor = Color.yellow;
            }
            else if (obj.cHPFull > 100000000 && obj.cHP > 0 && flag && !obj.isNRD)
            {
                gUIStyle.normal.textColor = Color.magenta;
            }
            else
            {
                gUIStyle.normal.textColor = Color.black;
            }
            g.drawString(i + 1 + ". " + text, GameCanvas.w - widthRect + 2, num, gUIStyle);
            num += heightRect + 1;
            if (obj.holder)
            {
                if (obj.charHold == Char.myCharz())
                {
                    list.Add(obj.cName + " đang trói bạn");
                }
                else if (obj.charHold != null)
                {
                    list.Add(obj.cName + " đang trói " + obj.charHold.cName);
                }
                else if (obj.mobHold != null)
                {
                    list.Add(obj.cName + " đang trói " + obj.mobHold.getTemplate().name);
                }
            }
        }
        int height = GameScr.logoInGame.getHeight();
        int num2 = ((!flag) ? (height + 20) : (height + 30));
        foreach (string item in list)
        {
            mFont.tahoma_7b_dark.drawString(g, item, GameCanvas.w / 2, num2, mFont.CENTER);
            num2 += 9;
        }
    }

    public static void paintCharNRD(mGraphics g, Char ch)
    {
        int num = GameScr.logoInGame.getHeight() + 20;
        int num2 = 9;
        string text = ch.cName + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + "]";
        if (ch.isNRD)
        {
            text = text + " - Còn: " + ch.timeNRD + " giây";
        }
        if (ch.isFreez)
        {
            text = text + " - Bị TDHS: " + ch.freezSeconds + " giây";
        }
        mFont.tahoma_7b_red.drawString(g, text, GameCanvas.w / 2, num, mFont.CENTER);
        num += num2;
    }

    public static void paintListBosses(mGraphics g)
    {
        if (GraphicsManagement.isHuntingBoss && !isMeInNRDMap())
        {
            int num = 42;
            for (int i = 0; i < listBosses.Count; i++)
            {
                listBosses[i].Paint(g, GameCanvas.w - 2 - mFont.tahoma_7_white.getWidth(" [Go]"), num, mFont.RIGHT);
                num += 8;
            }
        }
    }

    public void onChatFromMe(string text, string to)
    {
        if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
        {
            ChatTextField.gI().isShow = false;
            ResetChatTextField();
        }
        else if (ChatTextField.gI().strChat.Equals(inputLockFocusCharID[0]))
        {
            try
            {
                int num = (charIDLock = int.Parse(ChatTextField.gI().tfChat.getText()));
                isLockFocus = true;
                GameScr.info1.addInfo("Đã Thêm: " + num);
            }
            catch
            {
                GameScr.info1.addInfo("CharID Không Hợp Lệ, Vui Lòng Nhập Lại");
            }
            ResetChatTextField();
        }
        else if (ChatTextField.gI().strChat.Equals(inputHPFusionDance[0]))
        {
            try
            {
                int num2 = (minimumHPFusionDance = int.Parse(ChatTextField.gI().tfChat.getText()));
                GameScr.info1.addInfo("Hợp Thể Khi HP Dưới: " + Res.formatNumber2(num2));
            }
            catch
            {
                GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!");
            }
            ResetChatTextField();
        }
        else if (ChatTextField.gI().strChat.Equals(inputCharID[0]))
        {
            try
            {
                int item = int.Parse(ChatTextField.gI().tfChat.getText());
                listCharIDs.Add(item);
                GameScr.info1.addInfo("Đã Thêm: " + item);
            }
            catch
            {
                GameScr.info1.addInfo("CharID Không Hợp Lệ, Vui Lòng Nhập Lại!");
            }
            ResetChatTextField();
        }
        else if (ChatTextField.gI().strChat.Equals(inputHPLimit[0]))
        {
            try
            {
                int num3 = (HPLimit = int.Parse(ChatTextField.gI().tfChat.getText()));
                GameScr.info1.addInfo("Limit: " + NinjaUtil.getMoneys(num3) + " HP");
            }
            catch
            {
                GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!");
            }
            ResetChatTextField();
        }
        else if (ChatTextField.gI().strChat.Equals(inputHPChar[0]))
        {
            try
            {
                int num4 = (limitHPChar = int.Parse(ChatTextField.gI().tfChat.getText()));
                GameScr.info1.addInfo("Limit: " + NinjaUtil.getMoneys(num4) + " HP");
            }
            catch
            {
                GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!");
            }
            ResetChatTextField();
        }
        else
        {
            if (!ChatTextField.gI().strChat.Equals(inputHPPercentFusionDance[0]))
            {
                return;
            }
            try
            {
                int num5 = int.Parse(ChatTextField.gI().tfChat.getText());
                if (num5 > 99)
                {
                    num5 = 99;
                }
                minumumHPPercentFusionDance = num5;
                GameScr.info1.addInfo("Hợp Thể Khi HP Dưới: " + num5 + "%");
            }
            catch
            {
                GameScr.info1.addInfo("%HP Không Hợp Lệ, Vui Lòng Nhập Lại!");
            }
            ResetChatTextField();
        }
    }

    public void onCancelChat()
    {
        ResetChatTextField();
    }

    public void perform(int idAction, object p)
    {
        switch (idAction)
        {
            case 1:
                MainXmapCL.ShowMenu();
                break;
            case 2:
                AutoSkill.ShowMenu();
                break;
            case 3:
                AutoPean.ShowMenu();
                break;
            case 4:
                AutoPick.ShowMenu();
                break;
            case 6:
                AutoChat.ShowMenu();
                break;
            case 7:
                break;
            case 8:
                ShowMenuMore();
                break;
            case 9:
                if (minumumHPPercentFusionDance > 0)
                {
                    minumumHPPercentFusionDance = 0;
                    GameScr.info1.addInfo("Hợp thể khi HP dưới: 0% HP");
                }
                else
                {
                    ChatTextField.gI().strChat = inputHPPercentFusionDance[0];
                    ChatTextField.gI().tfChat.name = inputHPPercentFusionDance[1];
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                }
                break;
            case 10:
                if (minimumHPFusionDance > 0)
                {
                    minimumHPFusionDance = 0;
                    GameScr.info1.addInfo("Hợp thể khi HP dưới: 0");
                }
                else
                {
                    ChatTextField.gI().strChat = inputHPFusionDance[0];
                    ChatTextField.gI().tfChat.name = inputHPFusionDance[1];
                    ChatTextField.gI().startChat2(getInstance(), string.Empty);
                }
                break;
            case 11:
                AutoLoginCL.Toggle();
                break;
            case 12:
                isAutoLockControl = !isAutoLockControl;
                GameScr.info1.addInfo("Auto Khống Chế\n" + (isAutoLockControl ? "[STATUS: ON]" : "[STATUS: OFF]"));
                break;
            case 13:
                isAutoTeleport = !isAutoTeleport;
                GameScr.info1.addInfo("Auto Teleport\n" + (isAutoTeleport ? "[STATUS: ON]" : "[STATUS: OFF]"));
                break;
            case 14:
                AutoPetCL.ShowMenu();
                break;
            case 15:
                ChatTextField.gI().strChat = inputCharID[0];
                ChatTextField.gI().tfChat.name = inputCharID[1];
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 16:
                {
                    int num2 = (int)p;
                    if (num2 != 0)
                    {
                        listCharIDs.Add(num2);
                        GameScr.info1.addInfo("Đã Thêm: " + num2);
                    }
                    break;
                }
            case 17:
                {
                    int num = (int)p;
                    if (num != 0)
                    {
                        listCharIDs.Remove(num);
                        GameScr.info1.addInfo("Đã Xóa: " + num);
                    }
                    break;
                }
            case 18:
                AutoboMongCL.ShowMenu();
                break;
            case 19:
                isAutoAttackBoss = !isAutoAttackBoss;
                GameScr.info1.addInfo("Tấn Công Boss\n" + (isAutoAttackBoss ? "[STATUS: ON]" : "[STATUS: OFF]"));
                break;
            case 20:
                ChatTextField.gI().strChat = inputHPLimit[0];
                ChatTextField.gI().tfChat.name = inputHPLimit[1];
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 21:
                ShowMenuInfoMod();
                break;
            case 22:
                isAutoAttackOtherChars = !isAutoAttackOtherChars;
                GameScr.info1.addInfo("Tàn Sát Người\n" + (isAutoAttackOtherChars ? "[STATUS: ON]" : "[STATUS: OFF]"));
                break;
            case 23:
                ChatTextField.gI().strChat = inputHPChar[0];
                ChatTextField.gI().tfChat.name = inputHPChar[1];
                ChatTextField.gI().startChat2(getInstance(), string.Empty);
                break;
            case 24:
                MucTieuCL.ShowMenu();
                break;
            case 25:
                AutoVutDoCL.ShowMenu();
                break;
            case 26:
                AutoBossCL.ShowMenuBoss();
                break;
            case 27:
                YardatCL.ShowMenu();
                break;
            case 28:
                ShowMenuCak();
                break;
            case 29:
                AutoBuyItemCL.showMenu();
                break;
            case 98:
                Application.OpenURL("https://www.youtube.com/@cuongmikasa");
                break;
            case 99:
                Application.OpenURL("https://github.com/cuongle4399/QLTK-Nro");
                break;
            case 100:
                Application.OpenURL("https://www.facebook.com/cuong.le.810822/");
                break;
            case 103:
                Application.OpenURL("https://electroheavenvn.github.io/DataNRO/TeaMobi/?server=Server3");
                break;
        }
    }

    public static bool UpdateKey(int unused)
    {
        if (MainXmapCL.isXmaping)
        {
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.A)
        {
            AutoSkill.isAutoSendAttack = !AutoSkill.isAutoSendAttack;
            GameScr.info1.addInfo("Tự Đánh\n" + (AutoSkill.isAutoSendAttack ? "[STATUS: ON]" : "[STATUS: OFF]"));
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.B)
        {
            Service.gI().friend(0, -1);
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_K)
        {
            for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
            {
               Item item = Char.myCharz().arrItemBag[i];
                if (item != null && item.template.id == 993)
                {
                    GameScr.info1.addInfo("Đã sử dụng: " + item.itemOption[0].param);
                    break;
                }
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_B)
        {
            string text = "";
            for (int i = 0; i < Char.vItemTime.size(); i++)
            {
                ItemTime itemTime = (ItemTime)Char.vItemTime.elementAt(i);
                text = text + itemTime.idIcon + " ";
            }
            ChatPopup.addChatPopupMultiLineGameline(text);
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.C)
        {
            for (int j = 0; j < Char.myCharz().arrItemBag.Length; j++)
            {
                Item item = Char.myCharz().arrItemBag[j];
                if (item != null && (item.template.id == 194 || item.template.id == 193))
                {
                    Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                    break;
                }
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.D)
        {
            AutoSkill.FreezeSelectedSkill();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_D)
        {
            Controller.gI().loadCurrMap((sbyte)ModProCL.GetClosestGroundY(Char.myCharz().cx, Char.myCharz().cy));

            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.E)
        {
            isAutoRevive = !isAutoRevive;
            GameScr.info1.addInfo("Auto Hồi Sinh\n" + (isAutoRevive ? "[STATUS: ON]" : "[STATUS: OFF]"));
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.F)
        {
            useHopThe();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.G)
        {
            if (Char.myCharz().charFocus == null)
            {
                GameScr.info1.addInfo("Vui Lòng Chọn Mục Tiêu!");
            }
            else
            {
                Service.gI().giaodich(0, Char.myCharz().charFocus.charID, -1, -1);
                GameScr.info1.addInfo("Đã Gửi Lời Mời Giao Dịch Đến: " + Char.myCharz().charFocus.cName);
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.I)
        {
            isLockFocus = !isLockFocus;
            if (!isLockFocus)
            {
                GameScr.info1.addInfo("Khoá Mục Tiêu\n[STATUS: OFF]");
            }
            else if (Char.myCharz().charFocus == null)
            {
                GameScr.info1.addInfo("Vui Lòng Chọn Mục Tiêu!");
            }
            else
            {
                charIDLock = Char.myCharz().charFocus.charID;
                GameScr.info1.addInfo("Đã Khóa: " + Char.myCharz().charFocus.cName);
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.J)
        {
            MainXmapCL.LoadMapLeft();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.K)
        {
            MainXmapCL.LoadMapCenter();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.L)
        {
            MainXmapCL.LoadMapRight();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.M)
        {
            Service.gI().openUIZone();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.N)
        {
            if (isMeInNRDMap())
            {
                AutoPick.isAutoPick = !AutoPick.isAutoPick;
                GameScr.info1.addInfo("Auto Nhặt\n" + (AutoPick.isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"));
            }
            else
            {
                AutoPick.ShowMenu();
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.O)
        {
            isAutoEnterNRDMap = !isAutoEnterNRDMap;
            isOpenMenuNPC = true;
            GameScr.info1.addInfo("Auto Vào NRD\n" + (isAutoEnterNRDMap ? "[STATUS: ON]" : "[STATUS: OFF]"));
            return true;
        }

        if (GameCanvas.keyAsciiPress == Hotkeys.P)
        {
            if (!SocketGame.IsRunning)
            {
                SocketGame.Connect();
            }
            else
            {
                SocketGame.Disconnect();
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_C)
        {
            toiUuCPU = !toiUuCPU;
            if (toiUuCPU)
            {
                GameScr.info1.addInfo("Tối ưu cpu");
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 30;
            }
            else
            {
                GameScr.info1.addInfo("Đã tắt tối ưu cpu");
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 140;
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.T)
        {
            UseItem(521);
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_T)
        {
            for (int i = 0; i < Char.myCharz().arrItemBody.Length; i++)
            {
                Item item = Char.myCharz().arrItemBody[i];
                if (item != null && (item.template.id >= 529 && item.template.id <= 531) ||
                    (item.template.id >= 534 && item.template.id <= 536))
                {
                    Service.gI().getItem(5, (sbyte)i);
                }
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.X)
        {
            ShowMenu();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.Z)
        {
            if (Char.myCharz().cFlag == 0)
            {
                Service.gI().getFlag(1, 8);
                GameScr.info1.addInfo("Đã gửi yêu cầu bật cờ đen");
            }
            else
            {
                Service.gI().getFlag(1, 0);
                GameScr.info1.addInfo("Đã gửi yêu cầu tắt cờ");
            }
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.S)
        {
            AutoBossCL.aGimBoss = !AutoBossCL.aGimBoss;
            GameScr.info1.addInfo("Auto gim boss: " + (AutoBossCL.aGimBoss ? "Bật" : "Tắt"));
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_A)
        {
            AutoItem.useSet(0);
            GameScr.info1.addInfo("Đã mặc sét 1");
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_Z)
        {
            AutoItem.useSet(1);
            GameScr.info1.addInfo("Đã mặc sét 2");
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_P)
        {
            GameCanvas.gI().onDisconnected();
            return true;
        }
        if (GameCanvas.keyAsciiPress == Hotkeys.SHIFT_U)
        {
            if (AutoBossCL.findBossMod)
            {
                AutoBossCL.findBossMod = false;
            }
            if (AutoBossCL.tanCongBoss)
            {
                AutoBossCL.tanCongBoss = false;
                AutoBossCL.listBossTrongKhu.Clear();
            }
            if (AutoBossCL.AutoteleBoss)
            {
                AutoBossCL.AutoteleBoss = false;
            }
            if (ModProCL.tieuDietNguoiBatCo)
            {
                ModProCL.tieuDietNguoiBatCo = false;
                ModProCL.listNguoiCoDen.Clear();
            }
            if (AutoFarmBossNappa.DoSatBossNapa)
            {
                AutoFarmBossNappa.Stop();
            }
            if (AutoBossCL.aGimBoss)
            {
                AutoBossCL.aGimBoss = false;
            }
            if (AutoBossCL.aWhis)
            {
                AutoBossCL.aWhis = false;
            }
            if (AutoboMongCL.autoboMong)
            {
                AutoboMongCL.getInstance().StopAuto();
            }
            return true;
        }
        return true;
    }

    public static void ShowMenu()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Auto Map", getInstance(), 1, null));
        myVector.addElement(new Command("Auto Boss", getInstance(), 26, null));
        myVector.addElement(new Command("Auto vứt đồ", getInstance(), 25, null));
        myVector.addElement(new Command("Auto bò mộng", getInstance(), 18, null));
        myVector.addElement(new Command("Auto Pick", getInstance(), 4, null));
        myVector.addElement(new Command("Auto Mua Item khi hết", getInstance(), 29, null));
        myVector.addElement(new Command("Auto Đệ Tử", getInstance(), 14, null));
        myVector.addElement(new Command("More", getInstance(), 8, null));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void ShowMenuInfoMod()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Youtube của Cường", getInstance(), 98, null));
        myVector.addElement(new Command("Mã nguồn của Mod", getInstance(), 99, null));
        myVector.addElement(new Command("Facebook Cường Lê", getInstance(), 100, null));
        myVector.addElement(new Command("Web xem id vật phẩm", getInstance(), 103, null));
        GameCanvas.menu.startAt(myVector, 3);
    }


    public static void ShowMenuCak()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Auto Login: " + (AutoLoginCL.IsEnabled ? "ON" : "OFF"), getInstance(), 11, null));
        myVector.addElement(new Command("Auto Mục Tiêu", getInstance(), 24, null));
        myVector.addElement(new Command("Đang khỉ set 1, ngược set 2: " + (AutoTrainCL.AutoChangeClothes ? "ON" : "OFF"), AutoTrainCL.getInstance(), 12, null));
        myVector.addElement(new Command("Tiêu diệt all người bật cờ: " + (ModProCL.tieuDietNguoiBatCo ? "Bật" : "Tắt"), ModProCL.getInstance(), 22, null));
        myVector.addElement(new Command("Tự đấm bản thân đến chết", ModProCL.getInstance(), 25, null));
        myVector.addElement(new Command("Bán đồ rác khi full ht: " + (ModProCL.banDo ? "Bật" : "Tắt"), ModProCL.getInstance(), 27, null));
        myVector.addElement(new Command("Áp dụng bán/cất cooler: " + (ShowSetKH.applyDooCooler ? "ON" : "OFF"), ModProCL.getInstance(), 29, null));
        myVector.addElement(new Command("Cất đồ sao,kh,TL khi full ht: " + (ModProCL.catDoVIP ? "Bật" : "Tắt"), ModProCL.getInstance(), 28, null));
        GameCanvas.menu.startAt(myVector, 3);
        GameCanvas.menu.setMenuHeaderText("Đồ rác là đồ có id dưới 200 và không phải đồ sao pha lê, đồ kích hoạt, đồ thần linh\n" +
            "Đồ cooler cũng như đồ rác nhưng là đồ có id dưới 300 ví dụ: kaio,lưỡng long,rada 11,... ");
    }

    public static void ShowMenuMore()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Auto Chat", getInstance(), 6, null));
        myVector.addElement(new Command("Auto Skill", getInstance(), 2, null));
        myVector.addElement(new Command("Auto Pean", getInstance(), 3, null));
        myVector.addElement(new Command("Yardat", getInstance(), 27, null));
        myVector.addElement(new Command("Info Mod", getInstance(), 21, null));
        myVector.addElement(new Command("Auto cặt :)", getInstance(), 28, null));
        GameCanvas.menu.startAt(myVector, 3);
    }

    public static void ResetChatTextField()
    {
        ChatTextField.gI().strChat = "Chat";
        ChatTextField.gI().tfChat.name = "chat";
        ChatTextField.gI().isShow = false;
    }

    public static void UseItem(int templateId)
    {
        for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
        {
            Item item = Char.myCharz().arrItemBag[i];
            if (item != null && item.template.id == templateId)
            {
                Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
                break;
            }
        }
    }

    public static void TeleportTo(int x, int y)
    {
        if (GameScr.canAutoPlay)
        {
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
            return;
        }
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

    public static int GetYGround(int x)
    {
        int num = 50;
        int num2 = 0;
        while (num2 < 30)
        {
            num2++;
            num += 24;
            if (TileMap.tileTypeAt(x, num, 2))
            {
                if (num % 24 != 0)
                {
                    num -= num % 24;
                }
                break;
            }
        }
        return num;
    }

    static MainMod()
    {
        lastPower = new Dictionary<int, long>();
        lastUpdateTime = new Dictionary<int, long>();
        basePetPower = 0L;
        basePowerSet = false;
        lastPetInfoCallTime = 0L;
        configStartGame = false;
        frameCount = 0;
        lastTime = 1000L;
        fps = 0;
        stopMap = 0;
        checkSkill = false;
        listFlagColor = new List<Color>();
        listCharsInMap = new List<Char>();
        linkFb = "https://www.facebook.com/cuongle1002/";
        thongBao = true;
        GoldCurrent = 0L;
        GoldUpdate = 0L;
        GoldUpdateRealTime = 0L;
        VersionMod = "3.4.8";
        listBosses = new List<Boss>();
        listBackgroundImages = new List<Image>();
        limitHPChar = -1;
        inputHPChar = new string[2] { "Nhập HP Char:", "HP" };
        inputHPLimit = new string[2] { "Nhập HP:", "HP" };
        listCharIDs = new List<int>();
        inputCharID = new string[2] { "Nhập charID:", "charID" };
        inputHPPercentFusionDance = new string[2] { "Nhập %HP ", "%HP" };
        inputAutoLoginOffline = new string[2] { "Nhập giờ nghỉ Auto Login (vd: 03:00-04:00)", "Offline (giờ)" };
        inputHPFusionDance = new string[2] { "Nhập HP", "HP" };
        nameMapsNRD = new string[7] { "Hành tinh M-2", "Hành tinh Polaris", "Hành tinh Cretaceous", "Hành tinh Monmaasu", "Hành tinh Rudeeze", "Hành tinh Gelbo", "Hành tinh Tigere" };
        inputLockFocusCharID = new string[2] { "Nhập charID lock", "charID" };
        runSpeed = 8;
        thongbaoVIPne = "Mod: Phím X mở menu, [ Shift + A, Shift + Z ], thay set đồ, Shift + U để dừng bất kỳ auto nào";
    }

    public static void Revive()
    {
        if (Char.myCharz().luong + Char.myCharz().luongKhoa > 0 && Char.myCharz().meDead && Char.myCharz().cHP <= 0 && GameCanvas.gameTick % 20 == 0)
        {
            Service.gI().wakeUpFromDead();
            Char.myCharz().meDead = false;
            Char.myCharz().statusMe = 1;
            Char.myCharz().cHP = Char.myCharz().cHPFull;
            Char.myCharz().cMP = Char.myCharz().cMPFull;
            Char obj = Char.myCharz();
            Char obj2 = Char.myCharz();
            Char.myCharz().cp3 = 0;
            obj2.cp2 = 0;
            obj.cp1 = 0;
            ServerEffect.addServerEffect(109, Char.myCharz(), 2);
            GameScr.gI().center = null;
            GameScr.isHaveSelectSkill = true;
        }
    }

    public static void FocusTo(int charId)
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char obj = (Char)GameScr.vCharInMap.elementAt(i);
            if (!obj.isMiniPet && !obj.isPet && obj.charID == charId)
            {
                Char.myCharz().mobFocus = null;
                Char.myCharz().npcFocus = null;
                Char.myCharz().itemFocus = null;
                Char.myCharz().charFocus = obj;
                break;
            }
        }
    }

    public static bool isMeInNRDMap()
    {
        return TileMap.mapID >= 85 && TileMap.mapID <= 91;
    }

    public static bool isBoss(Char ch)
    {
        return ch.cName != null && ch.cName != "" && !ch.isPet && !ch.isMiniPet && char.IsUpper(char.Parse(ch.cName.Substring(0, 1))) && ch.cName != "Trọng tài" && !ch.cName.StartsWith("#") && !ch.cName.StartsWith("$");
    }

    public static void EnterNRDMap()
    {
        if (isOpenMenuNPC && (TileMap.mapID == 24 || TileMap.mapID == 25 || TileMap.mapID == 26) && GameCanvas.gameTick % 20 == 0)
        {
            Service.gI().openMenu(29);
            Service.gI().confirmMenu(29, 1);
            if (GameCanvas.panel.mapNames != null && GameCanvas.panel.mapNames.Length > 6 && GameCanvas.panel.mapNames[mapIdNRD - 1] == nameMapsNRD[mapIdNRD - 1])
            {
                Service.gI().requestMapSelect(mapIdNRD - 1);
                isOpenMenuNPC = false;
            }
        }
        if (isMeInNRDMap() && !Char.isLoadingMap && !Controller.isStopReadMessage && GameCanvas.gameTick % 20 == 0)
        {
            Service.gI().requestChangeZone(zoneIdNRD, -1);
            isAutoEnterNRDMap = false;
            isOpenMenuNPC = true;
        }
    }

    public static void updateTouch()
    {
        if (MainXmapCL.isXmaping)
        {
            if (GameCanvas.isPointerHoldIn(GameCanvas.w / 2 - GameScr.imgLbtn.getWidth() - 20, GameCanvas.h / 2 - GameScr.imgLbtn.getHeight() - 20, GameScr.imgLbtn.getWidth() + 60, GameScr.imgLbtn.getHeight() + 60) && GameCanvas.isPointerJustRelease && GameCanvas.isPointerClick)
            {
                stopMap = 1;
                MainXmapCL.FinishXmap();
                SoundMn.gI().buttonClick();
                GameCanvas.clearAllPointerEvent();
                if (AutoTrainCL.isGoBack)
                {
                    AutoTrainCL.isGoBack = false;
                }
                if (AutoBossCL.findBossMod)
                {
                    AutoBossCL.findBossMod = false;
                }
                if (ModProCL.tieuDietNguoiBatCo)
                {
                    ModProCL.tieuDietNguoiBatCo = false;
                    ModProCL.listNguoiCoDen.Clear();
                }
                if (AutoFarmBossNappa.DoSatBossNapa)
                {
                    AutoFarmBossNappa.Stop();
                }
                if (AutoBossCL.aWhis)
                {
                    AutoBossCL.aWhis = false;
                }
                if (AutoboMongCL.autoboMong)
                {
                    AutoboMongCL.getInstance().StopAuto();
                }
                if (AutoTrainCL.isAutoTrain)
                {
                    AutoTrainCL.isAutoTrain = false;
                }
                stopMap = 0;
            }
            else
            {
                GameCanvas.clearAllPointerEvent();
            }
        }
        if (!MainMenu.isShowMenuVIP)
        {
            if (GraphicsManagement.isShowCharsInMap)
            {
                controlTeleChar();
            }
            if (GraphicsManagement.isHuntingBoss)
            {
                Boss.controlGotoBoss();
            }
            if (GameCanvas.isPointerHoldIn(2, 2, GameScr.imgPanel.getWidth() + 5, GameScr.imgPanel.getHeight()) && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease && !GameCanvas.panel.isAccept && GameScr.gI().popUpYesNo == null)
            {
                ShowMenu();
                GameCanvas.clearAllPointerEvent();
            }
        }
        if (GameCanvas.isPointerHoldIn(160, 3, GraphicsManagement.imgTheBai.getWidth() + 5, GraphicsManagement.imgTheBai.getHeight()) && GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease && !MainMenu.isShowMenuVIP && !GameCanvas.panel.isShow && GameScr.gI().popUpYesNo == null)
        {
            MainMenu.ToggleMenu(show: true);
        }
        else if (MainMenu.isShowMenuVIP)
        {
            int panelX = MenuHelper.PanelX;
            int panelY = MenuHelper.PanelY;
            int panelWidth = MenuHelper.PanelWidth;
            int panelHeight = MenuHelper.PanelHeight;
            if (GameCanvas.isPointerHoldIn(panelX, panelY, panelWidth, panelHeight))
            {
                MainMenu.HandleClick();
                GameCanvas.clearAllPointerEvent();
            }
            else if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
            {
                MainMenu.ToggleMenu(show: false);
            }
        }
    }

    public static void LoadData()
    {
        SocketGame.Connect();
        AutoVutDoCL.loadData();
        AutoboMongCL.loadData();
        GraphicsManagement.loadData();
        MainXmapCL.LoadData();
        AutoTrainCL.LoadData();
        LoadFlagColor();
        CaptchaSolver.Initialize();
        if (mGraphics.zoomLevel == 2)
        {
            try
            {
                logoGameScreen = Image.__createImage(Convert.FromBase64String(GraphicsManagement.logoCuongLe));
                logoServerListScreen = Image.__createImage(Convert.FromBase64String(GraphicsManagement.logoCuongLe));
            }
            catch
            {
            }
        }
        if (mGraphics.zoomLevel == 1)
        {
            try
            {
                logoServerListScreen = Image.__createImage(Convert.FromBase64String(GraphicsManagement.logoPixel));
                logoGameScreen = Image.__createImage(Convert.FromBase64String(GraphicsManagement.logoPixel));
            }
            catch
            {
            }
        }
        try
        {
            APIKey = File.ReadAllText("Data\\keyAPI.ini");
            APIServer = File.ReadAllText("Data\\serverAPI.ini");
        }
        catch
        {
        }
        AutoLoginCL.InitLoginData();
    }

    public static void LoadFlagColor()
    {
        listFlagColor.Add(Color.black);
        listFlagColor.Add(new Color(0f, 0.99609375f, 0.99609375f));
        listFlagColor.Add(Color.red);
        listFlagColor.Add(new Color(0.54296875f, 0f, 0.54296875f));
        listFlagColor.Add(Color.yellow);
        listFlagColor.Add(Color.green);
        listFlagColor.Add(new Color(0.99609375f, 0.51171875f, 125f / 128f));
        listFlagColor.Add(new Color(0.80078125f, 51f / 128f, 0f));
        listFlagColor.Add(Color.black);
        listFlagColor.Add(Color.blue);
        listFlagColor.Add(Color.red);
        listFlagColor.Add(Color.blue);
    }

    private static void LogCapchaError(string message)
    {
        try
        {
            string logPath = Path.Combine("Data", "logErrorCapcha.txt");
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(logPath, logMessage + Environment.NewLine);
        }
        catch { }
    }


    public static string DecryptString(string str, string key)
    {
        byte[] array = Convert.FromBase64String(str);
        byte[] key2 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
        byte[] bytes = new TripleDESCryptoServiceProvider
        {
            Key = key2,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        }.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
        return Encoding.UTF8.GetString(bytes);
    }

    public static void checkInputSHIFT(ref int num)
    {
        if (num >= 97 && num <= 122)
        {
            num -= 32;
        }
    }

    public static void infoCharView(mGraphics g, Char c, int xStart, int yStart)
    {
        if (c == null || g == null)
        {
            return;
        }
        try
        {
            Char obj = Char.myCharz();
            if (obj == null)
            {
                return;
            }
            long num = mSystem.currentTimeMillis();
            if (num - lastPetInfoCallTime >= 1000)
            {
                lastPetInfoCallTime = num;
                Service.gI().petInfo();
            }
            if (ModProCL.petw && !basePowerSet)
            {
                basePetPower = c.cPower;
                basePowerSet = true;
            }
            if (!ModProCL.petw && basePowerSet)
            {
                basePowerSet = false;
                basePetPower = 0L;
            }
            if (c.charID != obj.charID)
            {
                if (!lastUpdateTime.TryGetValue(c.charID, out var value))
                {
                    value = 0L;
                }
                if (num - value >= 1000)
                {
                    lastUpdateTime[c.charID] = num;
                    if (!lastPower.TryGetValue(c.charID, out var value2))
                    {
                        value2 = c.cPower;
                    }
                    lastPower[c.charID] = c.cPower;
                }
            }
            GraphicsManagement.DrawFont.drawString(g, $"HP: {c.cHP}/{c.cHPFull}", xStart, yStart, 0);
            GraphicsManagement.DrawFont.drawString(g, $"MP: {c.cMP}/{c.cMPFull}", xStart, yStart + 8, 0);
            GraphicsManagement.DrawFont.drawString(g, $"SĐ: {c.cDamFull}", xStart, yStart + 16, 0);
            GraphicsManagement.DrawFont.drawString(g, "Thể Lực: " + NinjaUtil.getMoneys(c.cStamina), xStart, yStart + 24, 0);
            GraphicsManagement.DrawFont.drawString(g, "SM: " + NinjaUtil.getMoneys(c.cPower), xStart, yStart + 32, 0);
        }
        catch
        {
            GraphicsManagement.DrawFont.drawString(g, "Lỗi hiển thị thông tin", xStart, yStart, 0);
        }
    }

    public static void paintPKFlag(mGraphics g, Char @char, int num)
    {
        if (@char.cFlag != 0 && @char.cFlag != -1)
        {
            SmallImage.drawFlagSquare(g, @char.cFlag, GameCanvas.w - widthRect - 8, num + 1);
        }
    }

    public static void infoTrain(mGraphics g, int xStart, int yStart)
    {
        GraphicsManagement.DrawFont.drawString(g, "Vàng : " + Res.FormatNumberVIP(Char.myCharz().xu), xStart, yStart, 0);
        GraphicsManagement.DrawFont.drawString(g, "Số vàng up được : " + Res.FormatNumberVIP(Char.myCharz().xu - GoldCurrent), xStart, yStart + 8, 0);
        if (Char.myCharz().xu - GoldUpdate > 0 && GameCanvas.gameTick % 30 == 0)
        {
            GoldUpdateRealTime = Char.myCharz().xu - GoldUpdate;
            GoldUpdate = Char.myCharz().xu;
        }
        GraphicsManagement.DrawFont.drawString(g, "Vàng của 1 hit: " + Res.FormatNumberVIP(GoldUpdateRealTime), xStart, yStart + 16, 0);
    }

    public static void useHopThe()
    {
        int num = ModProCL.ExistPotara();
        if (num != -1)
        {
            UseItem(num);
        }
        else
        {
            GameScr.info1.addInfo("Bạn không có bông tai!!");
        }
    }

    public static void controlTeleChar()
    {
        int num = (isMeInNRDMap() ? 35 : 88);
        widthRect = 120;
        heightRect = 7;
        for (int i = 0; i < listCharsInMap.Count; i++)
        {
            if (listCharsInMap[i] == null)
            {
                continue;
            }
            if (GameCanvas.isPointerHoldIn(GameCanvas.w - widthRect, num + 1, widthRect * 3 / 4, heightRect) && GameCanvas.isPointerClick)
            {
                if (Char.myCharz().charFocus == listCharsInMap[i])
                {
                    MainXmapCL.TeleportTo(listCharsInMap[i].cx, listCharsInMap[i].cy);
                }
                else
                {
                    Char.myCharz().focusManualTo(listCharsInMap[i]);
                }
                SoundMn.gI().buttonClick();
                Char.myCharz().currentMovePoint = null;
                GameCanvas.clearAllPointerEvent();
            }
            num += heightRect + 1;
        }
    }

    public static void updateInfo101(string chatVip)
    {
        if (chatVip.Equals(thongbaoVIPne) || chatVip.Contains("vừa xuất hiện"))
        {
            return;
        }
        string text = "[" + DateTime.Now.ToString("HH'h'mm'p'ss's'") + "] ";
        for (int i = 0; i < Panel.vGameInfo.size(); i++)
        {
            GameInfo gameInfo = (GameInfo)Panel.vGameInfo.elementAt(i);
            if (gameInfo.id == 101)
            {
                gameInfo.content = text + chatVip + "\n" + gameInfo.content;
                string[] array = gameInfo.content.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length > 150)
                {
                    array = array.Take(150).ToArray();
                    gameInfo.content = string.Join("\n", array) + "\n";
                }
                break;
            }
        }
    }
}
