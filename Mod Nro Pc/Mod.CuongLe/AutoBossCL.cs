using main.Mod;
using System;
using System.Collections.Generic;
using Xmap;

namespace Mod.CuongLe;

public class AutoBossCL : IActionListener
{
    public static bool DoBoss;

    public static bool findBossMod;

    public static bool aGimBoss;

    public static bool aWhis;

    public static bool AutoteleBoss;

    public static bool tanCongBoss;

    public static List<string> targetBossNames;

    private static int doBossState;

    private static int doBossTargetZone;

    private static long doBossTimer;

    private static long focusBossTimer;

    private static int whisState;

    private static long whisTimer;

    private static bool whisFlag;

    private static int findBossState;

    private static long findBossTimer;

    private static long nextTeleTime;

    public static int CountZoneMap;

    public static bool offPaintZone;

    public static int mapDoBossOld;

    public static List<Char> listBossTrongKhu;

    private static AutoBossCL _Instance;

    public static AutoBossCL getInstance()
    {
        if (_Instance == null)
        {
            _Instance = new AutoBossCL();
        }
        return _Instance;
    }

    static AutoBossCL()
    {
        targetBossNames = new List<string>();
        listBossTrongKhu = new List<Char>();
        targetBossNames = new List<string>();
        mapDoBossOld = -1;
        doBossState = 0;
        doBossTargetZone = 0;
        doBossTimer = 0L;
        focusBossTimer = 0L;
        whisState = 0;
        whisTimer = 0L;
        whisFlag = false;
        findBossState = 0;
        findBossTimer = 0L;
        nextTeleTime = 0L;
    }

    public static void ShowMenuBoss()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command(DoBoss ? "Dò boss: ON" : "Dò boss: OFF", getInstance(), 3, null));
        myVector.addElement(new Command(aGimBoss ? "Auto gim boss : ON" : "Auto gim boss: OFF", getInstance(), 9, null));
        myVector.addElement(new Command(AutoFarmBossNappa.DoSatBossNapa ? "Farm boss napa : ON" : "Farm boss napa: OFF", getInstance(), 13, null));
        myVector.addElement(new Command((AutoFarmBossNappa.typeBoss == 0) ? "Đổi Farm Nappa: Boss Kuku" : ((AutoFarmBossNappa.typeBoss == 1) ? "Đổi Farm Nappa: Boss Mập đầu ngáo" : "Đổi Farm Nappa: Boss Rambo"), getInstance(), 1, null));
        myVector.addElement(new Command(aWhis ? "Auto Leo top whis: ON" : "Auto Leo top whis: OFF", getInstance(), 10, null));
        myVector.addElement(new Command(findBossMod ? "Auto map Boss trứng mabu : ON" : "Auto map Boss trứng mabu: OFF", getInstance(), 11, null));
        myVector.addElement(new Command("Auto dịch theo Boss: " + (AutoteleBoss ? "ON" : "OFF"), getInstance(), 17, null));
        myVector.addElement(new Command("Auto dịch + Tấn công Boss " + (tanCongBoss ? "ON" : "OFF"), getInstance(), 23, null));
        GameCanvas.menu.startAt(myVector, 4);
    }

    public void perform(int IdAction, object p)
    {
        switch (IdAction)
        {
            case 1:
                switch (AutoFarmBossNappa.typeBoss)
                {
                    case 0:
                        AutoFarmBossNappa.typeBoss = 1;
                        GameScr.info1.addInfo("Đã đổi Farm Boss Nappa thành Mập đầu đinh");
                        break;
                    case 1:
                        AutoFarmBossNappa.typeBoss = 2;
                        GameScr.info1.addInfo("Đã đổi Farm Boss Nappa thành Rambo");
                        break;
                    case 2:
                        AutoFarmBossNappa.typeBoss = 0;
                        GameScr.info1.addInfo("Đã đổi Farm Boss Nappa thành KuKu");
                        break;
                }
                ShowMenuBoss();
                break;
            case 3:
                DoBoss = !DoBoss;
                if (!DoBoss)
                {
                    StopAutoDoBoss();
                }
                GameScr.info1.addInfo(DoBoss ? "Dò boss: ON" : "Dò boss: OFF");
                break;
            case 9:
                aGimBoss = !aGimBoss;
                GameScr.info1.addInfo("Auto gim boss: " + (aGimBoss ? "Bật" : "Tắt"));
                ShowMenuBoss();
                break;
            case 10:
                aWhis = !aWhis;
                if (!aWhis)
                {
                    StopAutoWhis();
                }
                GameScr.info1.addInfo("Auto leo Tháp Whis: " + (aWhis ? "Bật" : "Tắt"));
                break;
            case 17:
                AutoteleBoss = !AutoteleBoss;
                GameScr.info1.addInfo("Auto dịch theo Boss\n" + (AutoteleBoss ? "[STATUS: ON]" : "[STATUS: OFF]"));
                ShowMenuBoss();
                break;
            case 11:
                findBossMod = !findBossMod;
                if (findBossMod)
                {
                    GameScr.info1.addInfo("|0|Auto tìm boss Hirde " + (findBossMod ? "Bật" : "Tắt"));
                }
                break;
            case 13:
                AutoFarmBossNappa.DoSatBossNapa = !AutoFarmBossNappa.DoSatBossNapa;
                if (!AutoFarmBossNappa.DoSatBossNapa)
                {
                    AutoFarmBossNappa.Stop();
                }
                GameScr.info1.addInfo("|0|Auto đánh Boss Napa " + (AutoFarmBossNappa.DoSatBossNapa ? "Bật" : "Tắt"));
                break;
            case 23:
                ModProCL.tieuDietNguoiBatCo = false;
                tanCongBoss = !tanCongBoss;
                if (tanCongBoss)
                {
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().npcFocus = null;
                    AutoteleBoss = true;
                }
                else
                {
                    listBossTrongKhu.Clear();
                    AutoteleBoss = false;
                }
                GameScr.info1.addInfo("Tấn công Boss: " + (tanCongBoss ? " ON " : "OFF"));
                ShowMenuBoss();
                break;
        }
    }

    public static void Update()
    {
        AutoFarmBossNappa.Update();
        UpdateAutoFocusBoss();
        UpdateAutoWhis();
        UpdateAutoDoBoss();
        UpdateFindBossHi();
        UpdateTeleBoss();
    }

    private static bool IsBossInTargetList(string bossName)
    {
        if (targetBossNames.Count == 0)
        {
            return true;
        }
        foreach (string targetBossName in targetBossNames)
        {
            if (!string.IsNullOrEmpty(targetBossName) && bossName.IndexOf(targetBossName, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsValidBoss(Char c)
    {
        if (c == null || c.cName == null || c.cName == "" || c.isPet || c.isMiniPet)
        {
            return false;
        }
        if (!char.IsUpper(c.cName[0]))
        {
            return false;
        }
        if (c.cName == "Trọng tài" || c.cName.StartsWith("#") || c.cName.StartsWith("$") || (c.cName.Contains("Broly") && !c.cName.Contains("Super")))
        {
            return false;
        }
        if (c.cHP <= 0)
        {
            return false;
        }
        if (c.cx >= TileMap.GetMapEndX() - 10 || c.cy >= TileMap.GetMapEndY() - 10)
        {
            return false;
        }
        if (AutoFarmBossNappa.DoSatBossNapa && !AutoFarmBossNappa.IsBossNappa(c.cName))
        {
            return false;
        }
        if (!IsBossInTargetList(c.cName))
        {
            return false;
        }
        return true;
    }

    public static bool checkBoss()
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char c = (Char)GameScr.vCharInMap.elementAt(i);
            if (IsValidBoss(c))
            {
                return true;
            }
        }
        return false;
    }

    public static void UpdateAutoDoBoss()
    {
        if (!DoBoss)
        {
            return;
        }
        try
        {
            if (TileMap.mapID != mapDoBossOld)
            {
                mapDoBossOld = TileMap.mapID;
                doBossState = 0;
                GameScr.info1.addInfo("Đổi map -> Reset dò boss lại từ đầu");
            }
            switch (doBossState)
            {
                case 0:
                    if (TileMap.mapID == 23 || TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 47 || TileMap.mapID == 48 || TileMap.mapID == 50 || TileMap.mapID == 116)
                    {
                        DoBoss = false;
                        GameScr.info1.addInfo("Boss đâu ra trong map này ông thần ??");
                        break;
                    }
                    if (checkBoss())
                    {
                        GameScr.info1.addInfo("Đã tìm thấy boss" + GetTargetBossInfo());
                        DoBoss = false;
                        break;
                    }
                    AutoTrainCL.TuMoTDLT();
                    doBossTargetZone = TileMap.zoneID + 1;
                    offPaintZone = true;
                    Service.gI().openUIZone();
                    doBossState = -1;
                    break;
                case -1:
                    if (!offPaintZone)
                    {
                        CountZoneMap = ((GameScr.gI().zones != null) ? (GameScr.gI().zones.Length - 1) : 0);
                        doBossState = 1;
                    }
                    break;
                case 1:
                    if (doBossTargetZone <= CountZoneMap && !Char.myCharz().meDead)
                    {
                        Service.gI().requestChangeZone(doBossTargetZone, -1);
                        doBossTimer = mSystem.currentTimeMillis() + 1500;
                        doBossState = 2;
                    }
                    else
                    {
                        DoBoss = false;
                    }
                    break;
                case 2:
                    if (mSystem.currentTimeMillis() >= doBossTimer)
                    {
                        if (TileMap.zoneID != doBossTargetZone)
                        {
                            Service.gI().requestChangeZone(doBossTargetZone, -1);
                            doBossTimer = mSystem.currentTimeMillis() + 1500;
                        }
                        else
                        {
                            doBossState = 3;
                        }
                    }
                    break;
                case 3:
                    if (checkBoss())
                    {
                        Char firstValidBoss = GetFirstValidBoss();
                        string text = ((firstValidBoss != null) ? (" (" + firstValidBoss.cName + ")") : "");
                        GameScr.info1.addInfo("Đã tìm thấy boss" + text);
                        DoBoss = false;
                    }
                    else
                    {
                        long num = (GameScr.canAutoPlay ? 5200 : 10500);
                        doBossTimer = mSystem.currentTimeMillis() + num;
                        doBossState = 4;
                    }
                    break;
                case 4:
                    if (mSystem.currentTimeMillis() >= doBossTimer)
                    {
                        doBossTargetZone++;
                        doBossState = 1;
                    }
                    break;
            }
        }
        catch
        {
            GameScr.info1.addInfo("Lỗi r");
            DoBoss = false;
        }
    }

    public static void StopAutoDoBoss()
    {
        DoBoss = false;
        doBossState = 0;
    }

    public static void UpdateAutoFocusBoss()
    {
        if (!aGimBoss || mSystem.currentTimeMillis() < focusBossTimer)
        {
            return;
        }
        Char obj = null;
        long num = long.MaxValue;
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char obj2 = (Char)GameScr.vCharInMap.elementAt(i);
            if (IsValidBoss(obj2) && obj2.cHP < num)
            {
                num = obj2.cHP;
                obj = obj2;
            }
        }
        if (obj != null)
        {
            Char.myCharz().npcFocus = null;
            Char.myCharz().charFocus = obj;
            Char.myCharz().mobFocus = null;
        }
        focusBossTimer = mSystem.currentTimeMillis() + 500;
    }

    public static void UpdateAutoWhis()
    {
        if (!aWhis || Char.myCharz().meDead)
        {
            if (aWhis)
            {
                StopAutoWhis();
            }
            return;
        }
        long num = mSystem.currentTimeMillis();
        switch (whisState)
        {
            case 0:
                if (TileMap.mapID != 154)
                {
                    MainXmapCL.StartGoToMap(154);
                    whisState = 1;
                }
                else
                {
                    whisState = 2;
                }
                break;
            case 1:
                if (!MainXmapCL.isXmaping)
                {
                    whisState = 2;
                }
                break;
            case 2:
                tanCongBoss = true;
                ModProCL.tieuDietNguoiBatCo = false;
                if (!checkBossWhis())
                {
                    Service.gI().openMenu(56);
                    whisTimer = num + 500;
                    whisState = 20;
                }
                else
                {
                    whisState = 3;
                }
                break;
            case 20:
                if (num >= whisTimer && GameCanvas.menu.showMenu)
                {
                    SelectMenuStartsWith("[LV");
                    whisTimer = num + 500;
                    whisState = 3;
                }
                break;
            case 3:
                if (num >= whisTimer)
                {
                    if (checkBossWhis())
                    {
                        whisFlag = true;
                        whisTimer = num + 2000;
                        whisState = 4;
                    }
                    else
                    {
                        whisState = 5;
                        whisTimer = num + 3500;
                    }
                }
                break;
            case 4:
                if (num >= whisTimer)
                {
                    if (!whisFlag)
                    {
                        StopAutoWhis();
                        break;
                    }
                    whisFlag = false;
                    whisState = 2;
                }
                break;
            case 5:
                if (num >= whisTimer)
                {
                    whisState = 2;
                }
                break;
        }
    }

    private static void SelectMenuStartsWith(string prefix)
    {
        for (int i = 0; i < GameCanvas.menu.menuItems.size(); i++)
        {
            string text = ((Command)GameCanvas.menu.menuItems.elementAt(i)).caption.Replace("\r", "").Replace("\n", " ");
            if (text.StartsWith(prefix))
            {
                GameCanvas.menu.menuSelectedItem = i;
                GameCanvas.menu.performSelect();
                GameCanvas.menu.doCloseMenu();
                break;
            }
        }
    }

    public static void StopAutoWhis()
    {
        MainXmapCL.FinishXmap();
        tanCongBoss = false;
        aWhis = false;
        whisState = 0;
        GameScr.info1.addInfo("Auto Whis đã được ngắt");
    }

    public static void UpdateFindBossHi()
    {
        if (!findBossMod)
        {
            return;
        }
        switch (findBossState)
        {
            case 0:
                AutoTrainCL.TuMoTDLT();
                if (TileMap.mapID != 126)
                {
                    MainXmapCL.StartGoToMap(126);
                    findBossState = 1;
                }
                else
                {
                    findBossState = 3;
                }
                break;
            case 1:
                if (!MainXmapCL.isXmaping)
                {
                    findBossTimer = mSystem.currentTimeMillis() + 1000;
                    findBossState = 3;
                }
                break;
            case 3:
                if (mSystem.currentTimeMillis() >= findBossTimer)
                {
                    if (ModProCL.CheckBossMob(70))
                    {
                        GameScr.info1.addInfo("Đã tìm thấy boss!");
                        findBossMod = false;
                        findBossState = 0;
                    }
                    else
                    {
                        GameScr.info1.addInfo("Không thấy boss, quay về TP.Vegeta để reset...");
                        MainXmapCL.StartGoToMap(19);
                        findBossState = 4;
                    }
                }
                break;
            case 4:
                if (!MainXmapCL.isXmaping)
                {
                    findBossTimer = mSystem.currentTimeMillis() + 800;
                    findBossState = 5;
                }
                break;
            case 5:
                if (mSystem.currentTimeMillis() >= findBossTimer)
                {
                    MainXmapCL.StartGoToMap(126);
                    findBossState = 1;
                }
                break;
            case 2:
                break;
        }
    }

    public static void UpdateTeleBoss()
    {
        if (!AutoteleBoss || mSystem.currentTimeMillis() < nextTeleTime)
        {
            return;
        }
        Char obj = null;
        long num = long.MaxValue;
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char obj2 = (Char)GameScr.vCharInMap.elementAt(i);
            if (obj2 != null && MainMod.isBoss(obj2) && !Char.myCharz().meDead && obj2.cHP > 0 && obj2.cx > 10 && obj2.cy < TileMap.GetMapEndY() - 10 && obj2.cx < TileMap.GetMapEndX() - 10 && (!AutoFarmBossNappa.DoSatBossNapa || AutoFarmBossNappa.IsBossNappa(obj2.cName)) && IsBossInTargetList(obj2.cName) && obj2.cHP < num)
            {
                num = obj2.cHP;
                obj = obj2;
            }
        }
        if (obj != null)
        {
            Char.myCharz().charFocus = obj;
            int num2 = obj.cx - Char.myCharz().cx;
            int num3 = obj.cy - Char.myCharz().cy;
            double num4 = System.Math.Sqrt(num2 * num2 + num3 * num3);
            if (num4 > 30.0)
            {
                MainXmapCL.TeleportTo(obj.cx, ModProCL.GetClosestGroundY(obj.cx, obj.cy));
            }
            nextTeleTime = mSystem.currentTimeMillis() + 2500;
        }
    }

    public static bool checkBossWhis()
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char obj = (Char)GameScr.vCharInMap.elementAt(i);
            if (obj.cName != null && obj.cName.StartsWith("Whis") && !obj.isPet && !obj.isMiniPet && char.IsUpper(char.Parse(obj.cName.Substring(0, 1))) && obj.cName != "Trọng tài" && !obj.cName.StartsWith("#") && !obj.cName.StartsWith("$"))
            {
                return true;
            }
        }
        return false;
    }

    public static void updateListBoss()
    {
        listBossTrongKhu.Clear();
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char obj = (Char)GameScr.vCharInMap.elementAt(i);
            if (obj.cName != null && obj.cName != "" && obj.cName != "Trọng tài" && obj.cName != "Broly" && MainMod.isBoss(obj) && obj.cx > 10 && obj.cHP > 0 && obj.cy < TileMap.GetMapEndY() && obj.cx < TileMap.GetMapEndX() && IsBossInTargetList(obj.cName))
            {
                listBossTrongKhu.Add(obj);
            }
        }
    }

    private static Char GetFirstValidBoss()
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            Char obj = (Char)GameScr.vCharInMap.elementAt(i);
            if (IsValidBoss(obj))
            {
                return obj;
            }
        }
        return null;
    }

    private static string GetTargetBossInfo()
    {
        if (targetBossNames.Count == 0)
        {
            return "";
        }
        Char firstValidBoss = GetFirstValidBoss();
        return (firstValidBoss != null) ? (" (" + firstValidBoss.cName + ")") : "";
    }

    public static void AddTargetBoss(string bossName)
    {
        if (!string.IsNullOrEmpty(bossName) && !targetBossNames.Contains(bossName))
        {
            targetBossNames.Add(bossName);
            GameScr.info1.addInfo("Đã thêm boss: " + bossName);
        }
    }

    public static void RemoveTargetBoss(string bossName)
    {
        if (targetBossNames.Contains(bossName))
        {
            targetBossNames.Remove(bossName);
            GameScr.info1.addInfo("Đã xóa boss: " + bossName);
        }
    }

    public static void ClearTargetBossList()
    {
        targetBossNames.Clear();
        GameScr.info1.addInfo("Đã xóa danh sách boss (dò tất cả)");
    }

    public static string GetTargetBossList()
    {
        if (targetBossNames.Count == 0)
        {
            return "Dò tất cả boss";
        }
        return "Boss: " + string.Join(", ", targetBossNames.ToArray());
    }
}
