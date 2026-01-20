using System;
using Xmap;

namespace Mod.CuongLe;

public class ShowSetKH
{
    public struct SetResult
    {
        public string Output;

        public bool[] IsFullSet;
    }

    private static bool isTrain;

    private static bool isGoback;

    private static int sellState;

    private static long sellTimer;

    private static int bagIndex;

    private static int catDoState;

    private static bool catDoFlagTrain;

    private static bool catDoFlagGoBack;

    private static long catDoTimer;

    private static int catDoIndex;

    private static int boxSlotBefore;

    private static int itemsSoldCount = 0;

    private static int itemsStoredCount = 0;

    // Biến kiểm soát áp dụng đồ cooler
    public static bool applyDooCooler = false;

    public static void update()
    {
        UpdateSellTrashItem();
        UpdateCatDo();
    }

    public static SetResult InDanhSachDoKH()
    {
        string text = "";
        int[] array = new int[5];
        int[] array2 = new int[5];
        int[] array3 = new int[5];
        int[] array4 = new int[5];
        int[] array5 = new int[5];
        DemItems(Char.myCharz().arrItemBag, array, array2, array3, array4, array5);
        DemItems(Char.myCharz().arrItemBox, array, array2, array3, array4, array5);
        DemItems(Char.myCharz().arrItemBody, array, array2, array3, array4, array5);
        bool[] array6 = new bool[5];
        int[][] array7 = new int[5][] { array, array2, array3, array4, array5 };
        for (int i = 0; i < array7.Length; i++)
        {
            array6[i] = array7[i][0] >= 1 && array7[i][1] >= 1 && array7[i][2] >= 1 && array7[i][3] >= 1 && array7[i][4] >= 1;
        }
        if (Char.myCharz().getGender().Equals("TĐ"))
        {
            text += DinhDangOutput("Sgk", array);
            text += DinhDangOutput("Kok", array2);
            text += DinhDangOutput("txh", array3);
            text += DinhDangOutput("Gohan", array4);
            text += DinhDangOutput("Kirin", array5);
        }
        else if (Char.myCharz().getGender().Equals("XD"))
        {
            text += DinhDangOutput("Kkr", array);
            text += DinhDangOutput("Ca Đíc", array2);
            text += DinhDangOutput("Cađic M", array3);
            text += DinhDangOutput("Gohan", array4);
            text += DinhDangOutput("Nappa", array5);
        }
        else
        {
            text += DinhDangOutput("Picolo", array);
            text += DinhDangOutput("Daimao", array2);
            text += DinhDangOutput("Ốc tiêu", array3);
            text += DinhDangOutput("Gohan", array4);
            text += DinhDangOutput("Nail", array5);
        }
        return new SetResult
        {
            Output = text,
            IsFullSet = array6
        };
    }

    public static void paintDOKH(int x, int y, mGraphics g)
    {
        SetResult setResult = InDanhSachDoKH();
        if (string.IsNullOrEmpty(setResult.Output))
        {
            GameScr.info1.addInfo("Nick bạn deo co sét kích hoạt ok");
            ModProCL.hienThiDoKH = false;
            return;
        }
        string[] array = setResult.Output.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int height = mFont.tahoma_7b_yellow.getHeight();
        for (int i = 0; i < array.Length && i < setResult.IsFullSet.Length; i++)
        {
            if (setResult.IsFullSet[i])
            {
                mFont.tahoma_7_yellow.drawString(g, array[i], x, y + i * height, 0);
            }
            else
            {
                mFont.tahoma_7.drawString(g, array[i], x, y + i * height, 0);
            }
        }
    }

    private static void DemItems(Item[] items, int[] set1, int[] set2, int[] set3, int[] set4, int[] set5)
    {
        Item[] array;
        if (Char.myCharz().getGender().Equals("TĐ"))
        {
            array = items;
            Item[] array2 = array;
            Item[] array3 = array2;
            Item[] array4 = array3;
            Item[] array5 = array4;
            Item[] array6 = array5;
            foreach (Item item in array6)
            {
                try
                {
                    if (item != null && item.itemOption[1] != null && item.itemOption[2] != null && item.template.type <= 4)
                    {
                        if (item.itemOption[1].optionTemplate.name.StartsWith("Set Sôngôku") || item.itemOption[2].optionTemplate.name.StartsWith("Set Sôngôku"))
                        {
                            TangCount(set1, item);
                        }
                        else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Thần Vũ Trụ") || item.itemOption[2].optionTemplate.name.StartsWith("Set Thần Vũ Trụ"))
                        {
                            TangCount(set2, item);
                        }
                        else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Thê") || item.itemOption[2].optionTemplate.name.StartsWith("Set Thê"))
                        {
                            TangCount(set3, item);
                        }
                        else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Gohan") || item.itemOption[2].optionTemplate.name.StartsWith("Set Gohan"))
                        {
                            TangCount(set4, item);
                        }
                        else if (item.itemOption[1].optionTemplate.name.StartsWith("Set Kirin") || item.itemOption[2].optionTemplate.name.StartsWith("Set Kirin"))
                        {
                            TangCount(set5, item);
                        }
                    }
                }
                catch
                {
                }
            }
            return;
        }
        if (Char.myCharz().getGender().Equals("XD"))
        {
            array = items;
            Item[] array7 = array;
            Item[] array8 = array7;
            Item[] array9 = array8;
            Item[] array10 = array9;
            Item[] array11 = array10;
            foreach (Item item2 in array11)
            {
                try
                {
                    if (item2 != null && item2.itemOption[1] != null && item2.itemOption[2] != null && item2.template.type <= 4)
                    {
                        if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Kakarot") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Kakarot"))
                        {
                            TangCount(set1, item2);
                        }
                        else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Ca Đíc") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Ca Đíc"))
                        {
                            TangCount(set2, item2);
                        }
                        else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Cađic M") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Cađic M"))
                        {
                            TangCount(set3, item2);
                        }
                        else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Gohan") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Gohan"))
                        {
                            TangCount(set4, item2);
                        }
                        else if (item2.itemOption[1].optionTemplate.name.StartsWith("Set Nappa") || item2.itemOption[2].optionTemplate.name.StartsWith("Set Nappa"))
                        {
                            TangCount(set5, item2);
                        }
                    }
                }
                catch
                {
                }
            }
            return;
        }
        array = items;
        Item[] array12 = array;
        Item[] array13 = array12;
        Item[] array14 = array13;
        Item[] array15 = array14;
        Item[] array16 = array15;
        foreach (Item item3 in array16)
        {
            try
            {
                if (item3 != null && item3.itemOption[1] != null && item3.itemOption[2] != null && item3.template.type <= 4)
                {
                    if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Picolo") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Picolo"))
                    {
                        TangCount(set1, item3);
                    }
                    else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Pikkoro Daimao") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Pikkoro Daimao"))
                    {
                        TangCount(set2, item3);
                    }
                    else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Ốc tiêu") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Ốc tiêu"))
                    {
                        TangCount(set3, item3);
                    }
                    else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Gohan") || item3.itemOption[2].optionTemplate.name.StartsWith("Set Gohan"))
                    {
                        TangCount(set4, item3);
                    }
                    else if (item3.itemOption[1].optionTemplate.name.StartsWith("Set Nail chiến binh") || item3.itemOption[2].optionTemplate.name.StartsWith("Nail chiến binh"))
                    {
                        TangCount(set5, item3);
                    }
                }
            }
            catch
            {
            }
        }
    }

    private static void TangCount(int[] array, Item item)
    {
        array[item.template.type]++;
    }

    private static string DinhDangOutput(string name, int[] array)
    {
        bool flag = true;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != 0)
            {
                flag = false;
                break;
            }
        }
        if (flag)
        {
            return string.Empty;
        }
        return string.Concat(name + " ", array[0].ToString(), " ao ", array[1].ToString(), " w ", array[2].ToString(), " gang ", array[3].ToString(), " jay ", array[4].ToString(), " rd\n");
    }

    public static bool checkKH(Item item)
    {
        for (int i = 0; i < item.itemOption.Length; i++)
        {
            if (item.itemOption[i].optionTemplate.id == 107)
            {
                return true;
            }
            if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
            {
                return true;
            }
            if (item.template.type < 4)
            {
                return true;
            }
        }
        return false;
    }

    public static int soluongsao(Item item)
    {
        for (int i = 0; i < item.itemOption.Length; i++)
        {
            if (item.itemOption[i].optionTemplate.id == 107)
            {
                return item.itemOption[i].param;
            }
        }
        return 0;
    }

    public static void UpdateSellTrashItem()
    {
        if (!ModProCL.banDo)
        {
            return;
        }
        switch (sellState)
        {
            case 0:
                // Kiểm tra hành trang FULL và có đồ rác
                if (ModProCL.isFULLBag() && HasTrashItem())
                {
                    if (AutoTrainCL.isAutoTrain)
                    {
                        AutoTrainCL.isAutoTrain = false;
                        isTrain = true;
                    }
                    if (AutoTrainCL.isGoBack)
                    {
                        isGoback = true;
                        AutoTrainCL.isGoBack = false;
                    }
                    sellState = 1;
                }
                break;
            case 1:
                if (TileMap.mapID != 26)
                {
                    MainXmapCL.StartGoToMap(26);
                    sellState = 2;
                }
                else
                {
                    sellState = 3;
                }
                break;
            case 2:
                if (!MainXmapCL.isXmaping)
                {
                    sellState = 3;
                }
                break;
            case 3:
                ModProCL.teleNPC(16);
                sellTimer = mSystem.currentTimeMillis() + 500;
                sellState = 4;
                break;
            case 4:
                if (mSystem.currentTimeMillis() >= sellTimer)
                {
                    bagIndex = Char.myCharz().arrItemBag.Length - 1;
                    sellState = 5;
                }
                break;
            case 5:
                if (bagIndex >= 0)
                {
                    Item item = Char.myCharz().arrItemBag[bagIndex];
                    if (item != null && IsTrashItem(item))
                    {
                        Service.gI().saleItem(0, 1, (short)bagIndex);
                        Service.gI().saleItem(1, 1, (short)bagIndex);
                        sellTimer = mSystem.currentTimeMillis() + 800;
                        sellState = 6;
                    }
                    else
                    {
                        bagIndex--;
                        sellTimer = mSystem.currentTimeMillis() + 100;
                        sellState = 5;
                    }
                }
                else
                {
                    // Kiểm tra xem còn đồ rác không, nếu còn thì bắt đầu lại từ đầu hành trang
                    if (HasTrashItem())
                    {
                        bagIndex = Char.myCharz().arrItemBag.Length - 1;
                        sellState = 5;
                    }
                    else
                    {
                        sellState = 7;
                    }
                }
                break;
            case 6:
                if (mSystem.currentTimeMillis() >= sellTimer)
                {
                    bagIndex--;
                    sellTimer = mSystem.currentTimeMillis() + 100;
                    sellState = 5; // Quay lại state 5 để tiếp tục bán
                }
                break;
            case 7:
                if (isGoback)
                {
                    isGoback = false;
                    AutoTrainCL.isGoBack = true;
                }
                if (isTrain)
                {
                    AutoTrainCL.isAutoTrain = true;
                    isTrain = false;
                }
                sellState = 0;
                itemsSoldCount = 0; // Reset biến đếm số đồ đã bán
                break;
        }
    }

    public static bool itemKH(Item item)
    {
        bool result = false;
        for (int i = 0; i < item.itemOption.Length; i++)
        {
            if (item.itemOption[i].optionTemplate.name.StartsWith("$"))
            {
                return true;
            }
        }
        return result;
    }

    public static bool itemStar(Item item)
    {
        bool result = false;
        for (int i = 0; i < item.itemOption.Length; i++)
        {
            try
            {
                if (item.itemOption[i].optionTemplate.name.StartsWith("#") && item.itemOption[i].param > 0)
                {
                    return true;
                }
            }
            catch
            {
            }
        }
        return result;
    }

    public static void UpdateCatDo()
    {
        if (!ModProCL.catDoVIP)
        {
            return;
        }
        switch (catDoState)
        {
            case 0:
                // Kiểm tra hành trang FULL, rương chưa FULL, và có đồ cần cất (TL + KH)
                if (ModProCL.isFULLBag() && !ModProCL.isFULLBox() && HasItemToStoreTLandKH())
                {
                    ModProCL.tieuDietNguoiBatCo = false;
                    AutoBossCL.tanCongBoss = false;
                    catDoFlagTrain = false;
                    catDoFlagGoBack = false;
                    if (AutoTrainCL.isAutoTrain)
                    {
                        AutoTrainCL.isAutoTrain = false;
                        catDoFlagTrain = true;
                    }
                    if (AutoTrainCL.isGoBack)
                    {
                        catDoFlagGoBack = true;
                        AutoTrainCL.isGoBack = false;
                    }
                    if (TileMap.mapID != Char.myCharz().cgender + 21)
                    {
                        MainXmapCL.StartGoToMap(Char.myCharz().cgender + 21);
                        catDoState = 1;
                    }
                    else
                    {
                        catDoState = 2;
                    }
                }
                else
                {
                    ModProCL.catDoVIP = false;
                }
                break;
            case 1:
                if (!MainXmapCL.isXmaping)
                {
                    if (TileMap.mapID != Char.myCharz().cgender + 21)
                    {
                        ModProCL.catDoVIP = false;
                        catDoState = 0;
                    }
                    else
                    {
                        catDoState = 2;
                    }
                }
                break;
            case 2:
                ModProCL.teleNPC(3);
                Service.gI().openMenu(3);
                catDoTimer = mSystem.currentTimeMillis() + 300;
                catDoState = 3;
                break;
            case 3:
                if (mSystem.currentTimeMillis() >= catDoTimer)
                {
                    catDoIndex = Char.myCharz().arrItemBag.Length - 1;
                    catDoState = 4;
                }
                break;
            case 4:
                if (catDoIndex >= 0)
                {
                    Item item = Char.myCharz().arrItemBag[catDoIndex];
                    // Chỉ cất đồ TL và KH
                    if (item != null && CanStoreTLandKH(item))
                    {
                        Service.gI().getItem(1, (sbyte)catDoIndex);
                        itemsStoredCount++; // Tăng số lượng đồ đã cất
                        catDoTimer = mSystem.currentTimeMillis() + 700;
                        catDoState = 5;
                    }
                    else
                    {
                        catDoIndex--;
                        catDoTimer = mSystem.currentTimeMillis() + 100;
                    }
                }
                else
                {
                    // Kiểm tra xem còn đồ TL + KH không, nếu còn thì bắt đầu lại
                    if (HasItemToStoreTLandKH())
                    {
                        catDoIndex = Char.myCharz().arrItemBag.Length - 1;
                        catDoState = 4;
                    }
                    else
                    {
                        catDoState = 6;
                    }
                }
                break;
            case 5:
                if (mSystem.currentTimeMillis() >= catDoTimer)
                {
                    catDoIndex--;
                    catDoState = 4; // Quay lại state 4 để tiếp tục cất
                }
                break;
            case 6:
                if (catDoFlagGoBack)
                {
                    AutoTrainCL.isGoBack = true;
                }
                if (catDoFlagTrain)
                {
                    AutoTrainCL.isAutoTrain = true;
                }
                ModProCL.catDoVIP = false;
                itemsStoredCount = 0; // Reset biến đếm số đồ đã cất
                catDoState = 0;
                break;
        }
    }

    public static bool itemTL(Item item)
    {
        if (item.template.id != 555 && item.template.id != 557 && item.template.id != 559 && item.template.id != 556 && item.template.id != 562 && item.template.id != 563 && item.template.id != 561 && item.template.id != 558 && item.template.id != 560 && item.template.id != 564 && item.template.id != 566 && item.template.id != 567)
        {
            return item.template.id == 565;
        }
        return true;
    }

    // Kiểm tra xem hành trang có đồ rác không
    private static bool HasTrashItem()
    {
        foreach (Item item in Char.myCharz().arrItemBag)
        {
            if (item != null && IsTrashItem(item))
            {
                return true;
            }
        }
        return false;
    }

    // Kiểm tra xem item có phải đồ rác không
    private static bool IsTrashItem(Item item)
    {
        if (item == null)
            return false;

        // Loại trừ: đồ KH, đồ sao, đồ TL
        if (itemKH(item) || itemStar(item) || itemTL(item))
            return false;

        // Chỉ xét loại đồ từ 0-4 (áo, vũ khí, gang, dây, vòng)
        if (item.template.type < 0 || item.template.type > 4)
            return false;

        // Nếu applyDooCooler = true: bán đồ có ID < 300
        // Nếu applyDooCooler = false: bán đồ có ID < 200
        int idThreshold = applyDooCooler ? 300 : 200;
        if (item.template.id >= idThreshold)
            return false;

        return true;
    }

    // Kiểm tra xem hành trang có đồ cần cất không
    private static bool HasItemToStore()
    {
        foreach (Item item in Char.myCharz().arrItemBag)
        {
            if (item != null && CanStoreItem(item))
            {
                return true;
            }
        }
        return false;
    }

    // Kiểm tra xem item có phải đồ cần cất không
    private static bool CanStoreItem(Item item)
    {
        if (item == null)
            return false;

        // Chỉ xét loại đồ từ 0-4
        if (item.template.type < 0 || item.template.type > 4)
            return false;

        // Cất KH, sao, TL
        if (itemKH(item) || itemStar(item) || itemTL(item))
            return true;

        // Nếu applyDooCooler = true: cất thêm đồ color (ID 200-299)
        if (applyDooCooler && item.template.id >= 200 && item.template.id < 300)
            return true;

        return false;
    }

    // Kiểm tra xem hành trang có đồ TL và KH không
    private static bool HasItemToStoreTLandKH()
    {
        foreach (Item item in Char.myCharz().arrItemBag)
        {
            if (item != null && CanStoreTLandKH(item))
            {
                return true;
            }
        }
        return false;
    }

    // Kiểm tra xem item có phải đồ TL hoặc KH không (chỉ cất 2 loại này)
    private static bool CanStoreTLandKH(Item item)
    {
        if (item == null)
            return false;

        // Chỉ xét loại đồ từ 0-4
        if (item.template.type < 0 || item.template.type > 4)
            return false;

        // Chỉ cất đồ TL hoặc KH
        if (itemTL(item) || itemKH(item))
            return true;

        // Nếu applyDooCooler = true: cất thêm đồ color
        if (applyDooCooler && item.template.id >= 200 && item.template.id < 300)
            return true;

        return false;
    }

    public static int GetSellCount()
    {
        return itemsSoldCount;
    }

    public static int GetStoreCount()
    {
        return itemsStoredCount;
    }

    public static void ResetSellCount()
    {
        itemsSoldCount = 0;
    }

    public static void ResetStoreCount()
    {
        itemsStoredCount = 0;
    }
}