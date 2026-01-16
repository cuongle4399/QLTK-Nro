using System;
using System.Collections.Generic;
using DoHoa.CustomMenu.Shared;
using Mod.CuongLe;

namespace DoHoa.CustomMenu;

public static class MobTrainTab
{
    public static List<MobTrain> MobTrains;
    public static int ScrollOffset;

    private static int hoveredButton = -1;

    // Button position info
    private class ButtonPos
    {
        public int id;
        public int x;
        public int y;
        public int w;
        public int h;

        public ButtonPos(int id, int x, int y, int w, int h)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }
    }

    // Button definition
    private class ButtonDef
    {
        public int id;
        public string text;
        public bool enabled;
        public bool active;
        public Func<string> textFunc;

        public ButtonDef(int id, string text, bool enabled, bool active = false)
        {
            this.id = id;
            this.text = text;
            this.enabled = enabled;
            this.active = active;
            this.textFunc = null;
        }

        public ButtonDef(int id, Func<string> textFunc, bool enabled, bool active = false)
        {
            this.id = id;
            this.text = "";
            this.enabled = enabled;
            this.active = active;
            this.textFunc = textFunc;
        }

        public string GetText()
        {
            return textFunc != null ? textFunc() : text;
        }
    }

    static MobTrainTab()
    {
        MobTrains = new List<MobTrain>();
        ScrollOffset = 0;
    }

    public static void LoadMobsFromMap()
    {
        MobTrains.Clear();
        ScrollOffset = 0;
        List<int> selectedMobIds = GetSelectedMobIds();
        for (int i = 0; i < GameScr.vMob.size(); i++)
        {
            Mob mob = (Mob)GameScr.vMob.elementAt(i);
            if (mob != null && !mob.isMobMe)
            {
                MobTrain item = new MobTrain
                {
                    Name = mob.getTemplate().name,
                    MobId = mob.mobId,
                    TemplateId = mob.templateId,
                    X = mob.xFirst,
                    Y = mob.yFirst,
                    HP = mob.maxHp,
                    AutoFlag = selectedMobIds.Contains(mob.mobId)
                };
                MobTrains.Add(item);
            }
        }
    }

    public static void UpdateMobTrainFlags()
    {
        List<int> selectedMobIds = GetSelectedMobIds();
        foreach (MobTrain mobTrain in MobTrains)
        {
            mobTrain.AutoFlag = selectedMobIds.Contains(mobTrain.MobId);
        }
    }


    private static List<ButtonPos> GetFixedButtonPositions(int panelX, int contentY, bool hasMob)
    {
        int panelWidth = MenuHelper.PanelWidth - 16;

        // ĐIỂM BẮT ĐẦU: Nếu Zoom 1 thì đẩy lên cao hơn (giảm baseY)
        bool isZoom1 = mGraphics.zoomLevel == 1;
        int baseY = contentY + MenuHelper.ContentHeight + (isZoom1 ? -12 : 8);

        int cols = isZoom1 ? 3 : 4;           // Zoom 1: 3 cột
        int buttonHeight = isZoom1 ? 20 : 18; // Độ cao nút
        int spacing = isZoom1 ? 5 : 8;        // Khoảng cách ngang giữa các nút
        int rowSpacing = isZoom1 ? 3 : 8;     // Khoảng cách dọc giữa các hàng nút

        int buttonWidth = (panelWidth - (spacing * (cols - 1))) / cols;

        List<ButtonDef> buttons = GetButtonDefinitions(hasMob);
        List<ButtonPos> enabledPositions = new List<ButtonPos>();

        int posIndex = 0;
        int[] allButtonIds = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10};

        for (int i = 0; i < allButtonIds.Length; i++)
        {
            ButtonDef btn = buttons.Find(b => b.id == allButtonIds[i]);
            if (btn != null && btn.enabled)
            {
                int row = posIndex / cols;
                int col = posIndex % cols;

                int x = panelX + 8 + col * (buttonWidth + spacing);
                int y = baseY + row * (buttonHeight + rowSpacing);

                enabledPositions.Add(new ButtonPos(allButtonIds[i], x, y, buttonWidth, buttonHeight));
                posIndex++;
            }
        }
        return enabledPositions;
    }

    private static void DrawControlButtons(mGraphics g, int panelX, int contentY, bool hasMob)
    {
        List<ButtonDef> buttons = GetButtonDefinitions(hasMob);
        List<ButtonPos> positions = GetFixedButtonPositions(panelX, contentY, hasMob);

        foreach (ButtonDef btn in buttons)
        {
            if (!btn.enabled)
                continue;

            foreach (ButtonPos pos in positions)
            {
                if (pos.id == btn.id)
                {
                    DrawImageButton(g, pos.x, pos.y, pos.w, pos.h, btn.GetText(), btn.id, true, btn.active);
                    break;
                }
            }
        }
    }

    private static void DrawImageButton(mGraphics g, int x, int y, int width, int height, string text, int buttonIndex, bool enabled, bool active = false)
    {
        if (!enabled) return;
        bool isHovered = hoveredButton == buttonIndex;

        if (mGraphics.zoomLevel == 1)
        {
            // Nền nút: Active màu Vàng (0xFFAA00), Thường màu Đen/Xám
            int bgColor = active ? 0xFFAA00 : (isHovered ? 0x666666 : 0x222222);
            g.setColor(bgColor);
            g.fillRect(x, y, width, height);

            // Viền nút: Trắng nếu active cho nổi bật
            g.setColor(active ? 0xFFFFFF : 0x444444);
            g.drawRect(x, y, width, height);

            // Chữ: Ép màu trắng hoàn toàn
            mFont.tahoma_7_white.drawString(g, text, x + width / 2, y + height / 2 - 4, mFont.CENTER);
        }
        else
        {
            // Logic gốc khi Zoom > 1
            Image buttonImg = (isHovered || active) ? GameScr.imgLbtnFocus2 : GameScr.imgLbtn2;
            if (buttonImg != null)
            {
                g.drawRegion(buttonImg, 0, 0, buttonImg.getWidth(), buttonImg.getHeight(), 0, x + width / 2, y + height / 2, mGraphics.VCENTER | mGraphics.HCENTER);
            }
            else
            {
                g.setColor(active ? 16754470 : (isHovered ? 10586239 : 8026746));
                g.fillRect(x, y, width, height);
            }
            mFont.tahoma_7.drawString(g, text, x + width / 2, y + height / 2 - 3, mFont.CENTER);
        }
    }
    public static void Paint(mGraphics g, int panelX, int contentY)
    {
        // Kiểm tra zoom level để ẩn bớt chi tiết thừa
        bool isLowZoom = mGraphics.zoomLevel <= 1;
        bool flag2 = MobTrains.Count > 0;

        if (!flag2)
        {
            mFont.tahoma_7b_white.drawString(g, "Không có quái trong map", panelX + MenuHelper.PanelWidth / 2, contentY + 20, 2);
            mFont.tahoma_7b_white.drawString(g, "Vui lòng vào map có quái", panelX + MenuHelper.PanelWidth / 2, contentY + 35, 2);
            g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
        }

        int scrollOffset = ScrollOffset;
        int num = System.Math.Min(scrollOffset + MenuHelper.Rows, MobTrains.Count);
        int num2 = (flag2 ? GetFocusedIndex(panelX, contentY, scrollOffset, num) : (-1));

        if (flag2)
        {
            for (int i = scrollOffset; i < num; i++)
            {
                int num3 = i - scrollOffset;
                int num4 = num3 % MenuHelper.Rows;
                int num5 = contentY + num4 * 32;
                MobTrain mobTrain = MobTrains[i];
                bool flag3 = i == num2;
                int color = (flag3 ? 6052956 : 3815994);

                g.setColor(color);

                // Nếu zoom thấp, bỏ qua phần vẽ khung icon bên trái để text rộng hơn
                int x = panelX + 4;
                int w = MenuHelper.PanelWidth - 8;
                if (!isLowZoom)
                {
                    x = panelX + 4 + 50;
                    w = MenuHelper.PanelWidth - 8 - 50;
                }

                g.fillRect(x, num5, w, 30);

                if (!isLowZoom)
                {
                    // Chỉ vẽ nền icon và hình quái khi zoom cao
                    int color2 = (flag3 ? 33679 : 4934475);
                    g.setColor(color2);
                    g.fillRect(panelX + 4, num5, 50, 30);
                    try
                    {
                        MobTemplate mobTemplate = Mob.arrMobTemplate[mobTrain.TemplateId];
                        if (mobTemplate != null && mobTemplate.data != null)
                        {
                            mobTemplate.data.paintFrame(g, 0, panelX + 4 + 25, num5 + 32 + 4, 0, 2);
                        }
                    }
                    catch { }
                }

                mFont mFont1 = (flag3 ? mFont.tahoma_7b_white : mFont.tahoma_7b_yellow);
                mFont mFont2 = (flag3 ? mFont.tahoma_7_white : mFont.tahoma_7_blue1);

                // Căn lề text dựa trên việc có icon hay không
                int x3 = (isLowZoom ? (panelX + 8) : (panelX + 58));

                mFont1.drawString(g, mobTrain.Name, x3, num5 + 2, 0);
                mFont2.drawString(g, $"HP:{mobTrain.HP} - ID:{mobTrain.MobId}", x3, num5 + 14, 0);
                GameCanvas.paintz.paintCheckPass(g, panelX + MenuHelper.PanelWidth - 26, num5 + 4, mobTrain.AutoFlag, focus: false);
            }
            int maxScrollOffset = MenuHelper.CalculateMaxScrollOffset(MobTrains.Count);
            MenuHelper.DrawScrollBar(g, panelX, contentY, MobTrains.Count, ScrollOffset, maxScrollOffset);
        }
        g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
        DrawControlButtons(g, panelX, contentY, flag2);
    }
    public static void HandleClick(int panelX, int contentY)
    {
        bool flag = MobTrains.Count > 0;
        List<int> orCreateSelectedMobIds = GetOrCreateSelectedMobIds();

        if (flag)
        {
            int x = panelX + MenuHelper.PanelWidth - 28 - 4;
            int scrollOffset = ScrollOffset;
            int num = System.Math.Min(scrollOffset + MenuHelper.Rows, MobTrains.Count);
            for (int i = scrollOffset; i < num; i++)
            {
                int num2 = i - scrollOffset;
                int num3 = num2 % MenuHelper.Rows;
                int y = contentY + num3 * 32;
                if (!GameCanvas.isPointerHoldIn(x, y, 32, 32))
                {
                    continue;
                }
                MobTrain mobTrain = MobTrains[i];
                mobTrain.AutoFlag = !mobTrain.AutoFlag;
                if (mobTrain.AutoFlag)
                {
                    if (!orCreateSelectedMobIds.Contains(mobTrain.MobId))
                    {
                        orCreateSelectedMobIds.Add(mobTrain.MobId);
                    }
                    GameScr.info1.addInfo($"Đã thêm {mobTrain.Name} (ID:{mobTrain.MobId})");
                }
                else
                {
                    orCreateSelectedMobIds.Remove(mobTrain.MobId);
                    GameScr.info1.addInfo($"Đã loại bỏ {mobTrain.Name} (ID:{mobTrain.MobId})");
                }
                GameCanvas.clearAllPointerEvent();
                return;
            }
        }

        UpdateHoverState(panelX, contentY);

        List<ButtonDef> buttons = GetButtonDefinitions(flag);
        List<ButtonPos> positions = GetFixedButtonPositions(panelX, contentY, flag);

        foreach (ButtonPos bp in positions)
        {
            // Tìm button tương ứng
            ButtonDef btnDef = null;
            foreach (ButtonDef btn in buttons)
            {
                if (btn.id == bp.id && btn.enabled)
                {
                    btnDef = btn;
                    break;
                }
            }

            if (btnDef == null)
                continue;

            if (!GameCanvas.isPointerHoldIn(bp.x, bp.y, bp.w, bp.h))
                continue;

            HandleButtonClick(bp.id, flag, orCreateSelectedMobIds);
            GameCanvas.clearAllPointerEvent();
            return;
        }
    }

    private static void HandleButtonClick(int buttonId, bool hasMob, List<int> selectedMobIds)
    {
        switch (buttonId)
        {
            case 0:
                selectedMobIds.Clear();
                foreach (MobTrain mobTrain in MobTrains)
                {
                    mobTrain.AutoFlag = true;
                    selectedMobIds.Add(mobTrain.MobId);
                }
                GameScr.info1.addInfo("Đã chọn hết quái trong map");
                break;

            case 1:
                if ((hasMob && selectedMobIds.Count > 0) || AutoTrainCL.isAutoTrain)
                {
                    AutoTrainCL.isAutoTrain = !AutoTrainCL.isAutoTrain;
                    GameScr.info1.addInfo(AutoTrainCL.isAutoTrain ? "Auto Train: Đã Bật" : "Auto Train: Đã Dừng");
                }
                else
                {
                    GameScr.info1.addInfo("Chưa chọn quái hoặc không có quái");
                }
                break;

            case 2:
                AutoTrainCL.autoNeBoss = !AutoTrainCL.autoNeBoss;
                GameScr.info1.addInfo(AutoTrainCL.autoNeBoss ? "Né Boss: Đã Bật" : "Né Boss: Đã Tắt");
                AutoTrainCL.autoChangeZone = false;
                AutoTrainCL.SpamChangeZone = false;
                break;

            case 3:
                AutoTrainCL.isAvoidSuperMob = !AutoTrainCL.isAvoidSuperMob;
                GameScr.info1.addInfo(AutoTrainCL.isAvoidSuperMob ? "Né Siêu Quái: Đã Bật" : "Né Siêu Quái: Đã Tắt");
                break;

            case 4:
                AutoTrainCL.ShowMenuKhuIt();
                break;

            case 5:
                ShowSelectByTypeMenu();
                break;

            case 6:
                AutoTrainCL.autoHopThe = !AutoTrainCL.autoHopThe;
                if (AutoTrainCL.autoHopThe && (ModProCL.ExistPotara() == -1 || TileMap.mapID == Char.myCharz().cgender + 21))
                {
                    AutoTrainCL.autoHopThe = false;
                    GameScr.info1.addInfo("Yêu cầu có bông tai và ra khỏi map nhà");
                }
                else
                {
                    GameScr.info1.addInfo(AutoTrainCL.autoHopThe ? "Auto Hợp Thể: Đã Bật" : "Auto Hợp Thể: Đã Tắt");
                }
                break;

            case 7:
                AutoTrainCL.ShowMenuConfigHPTrainMob();
                break;

            case 8:
                AutoTrainCL.TYPEAK = !AutoTrainCL.TYPEAK;
                Rms.saveRMSInt("TYPETRAIN", AutoTrainCL.TYPEAK ? 1 : 0);

                GameScr.info1.addInfo(
                    "Dame Train: " + (AutoTrainCL.TYPEAK ? "AK [Không phụ thuộc FPS]" : "DEFAULT")
                );
                break;

            case 9:
                AutoTrainCL.ShowMenuGoback();
                break;

            case 10:
                AutoTrainCL.checkLag = !AutoTrainCL.checkLag;
                GameScr.info1.addInfo(AutoTrainCL.checkLag ? "Check Lag bằng tnsm [5 phút/ 1 lần]: Đã Bật" : "Check Lag bằng tnsm [5 phút/ 1 lần]: Đã Tắt");
                break;
        }
    }

    private static void UpdateHoverState(int panelX, int contentY)
    {
        hoveredButton = -1;

        if (!GameCanvas.isPointerDown)
            return;

        bool hasMob = MobTrains.Count > 0;
        List<ButtonDef> buttons = GetButtonDefinitions(hasMob);
        List<ButtonPos> positions = GetFixedButtonPositions(panelX, contentY, hasMob);

        foreach (ButtonPos bp in positions)
        {
            ButtonDef btnDef = null;
            foreach (ButtonDef btn in buttons)
            {
                if (btn.id == bp.id && btn.enabled)
                {
                    btnDef = btn;
                    break;
                }
            }

            if (btnDef == null)
                continue;

            if (GameCanvas.isPointerHoldIn(bp.x, bp.y, bp.w, bp.h))
            {
                hoveredButton = bp.id;
                return;
            }
        }
    }

    private static List<ButtonDef> GetButtonDefinitions(bool hasMob)
    {
        List<ButtonDef> buttons = new List<ButtonDef>
    {
        new ButtonDef(0, "Chọn hết", hasMob),
        new ButtonDef(1, () => AutoTrainCL.isAutoTrain ? "DỪNG" : "TRAIN", true, AutoTrainCL.isAutoTrain),
        new ButtonDef(2, "NÉ BOSS", true, AutoTrainCL.autoNeBoss),
        new ButtonDef(3, "Né SQuai", hasMob, !AutoTrainCL.isAvoidSuperMob),
        new ButtonDef(4, "Khu ít ?", hasMob, (AutoTrainCL.autoChangeZone || AutoTrainCL.SpamChangeZone)),
        new ButtonDef(5, "LOẠI", hasMob),
        new ButtonDef(6, "H.THỂ", true, AutoTrainCL.autoHopThe),
        new ButtonDef(7, "HP Quái", hasMob),
        new ButtonDef(
            8,
            () => AutoTrainCL.TYPEAK ? "AK" : "DEFAULT",
            true,
            AutoTrainCL.TYPEAK
        ),
        new ButtonDef(9, () => (!AutoTrainCL.isGoBack) ? "Goback" : (AutoTrainCL.isGobackCoordinate ? "GB TĐ" : "GB MAP"), true, AutoTrainCL.isGoBack),
        new ButtonDef(10, "Anti Lag", true, AutoTrainCL.checkLag)  // Nút Check Lag mới
    };

        return buttons;
    }

    public static void HandleScroll(int deltaY)
    {
        int num = -deltaY / 4;
        int val = MenuHelper.CalculateMaxScrollOffset(MobTrains.Count);
        int val2 = ScrollOffset + num;
        val2 = System.Math.Max(0, System.Math.Min(val2, val));
        if (val2 != ScrollOffset)
        {
            ScrollOffset = val2;
            GameCanvas.pyLast = GameCanvas.py;
        }
    }

    private static int GetFocusedIndex(int panelX, int contentY, int startIndex, int endIndex)
    {
        if (!GameCanvas.isPointerDown || !GameCanvas.isPointerJustRelease)
        {
            return -1;
        }
        int px = GameCanvas.px;
        int py = GameCanvas.py;
        for (int i = startIndex; i < endIndex; i++)
        {
            int num = i - startIndex;
            int num2 = num % MenuHelper.Rows;
            int num3 = contentY + num2 * 32;
            if (px >= panelX + 4 && px <= panelX + MenuHelper.PanelWidth - 4 && py >= num3 && py <= num3 + 32)
            {
                return i;
            }
        }
        return -1;
    }

    private static void ShowSelectByTypeMenu()
    {
        MyVector myVector = new MyVector();
        List<MobTrain> list = new List<MobTrain>();
        foreach (MobTrain mobTrain2 in MobTrains)
        {
            bool flag = false;
            foreach (MobTrain item in list)
            {
                if (item.TemplateId == mobTrain2.TemplateId)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                list.Add(mobTrain2);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            MobTrain mobTrain3 = list[i];
            int num = 0;
            foreach (MobTrain mobTrain4 in MobTrains)
            {
                if (mobTrain4.TemplateId == mobTrain3.TemplateId)
                {
                    num++;
                }
            }
            string caption = $"{mobTrain3.Name}\n[{NinjaUtil.getMoneys(mobTrain3.HP)}HP] - SL:{num}";
            myVector.addElement(new Command(caption, AutoTrainCL.getInstance(), 1, mobTrain3.TemplateId));
        }
        GameCanvas.menu.startAt(myVector, 3);

    }

    private static List<int> GetSelectedMobIds()
    {
        int mapID = TileMap.mapID;
        if (AutoTrainCL.listMobIds.ContainsKey(mapID))
        {
            return AutoTrainCL.listMobIds[mapID];
        }
        return new List<int>();
    }

    private static List<int> GetOrCreateSelectedMobIds()
    {
        int mapID = TileMap.mapID;
        if (!AutoTrainCL.listMobIds.ContainsKey(mapID))
        {
            AutoTrainCL.listMobIds[mapID] = new List<int>();
        }
        return AutoTrainCL.listMobIds[mapID];
    }
}