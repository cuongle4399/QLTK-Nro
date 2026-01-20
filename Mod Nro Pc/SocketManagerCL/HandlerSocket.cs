using main.Mod;
using Mod.community;
using Mod.CuongLe;
using ModCak.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using Xmap;

namespace SocketManagerCL;

internal class HandlerSocket
{
    private static readonly Dictionary<string, Action<string>> farmBossNappaActions = new Dictionary<string, Action<string>>
    {
        {
            "ONfarmNappa",
            msg =>
            {
                AutoFarmBossNappa.typeBoss = int.Parse(msg.Split('|')[1]);
                AutoFarmBossNappa.DoSatBossNapa = true;
                GameScr.info1.addInfo("|0|Auto đánh Boss Napa: Bật");
            }
        },
        {
            "OFFfarmNappa",
            _ =>
            {
                AutoFarmBossNappa.DoSatBossNapa = false;
                AutoFarmBossNappa.Stop();
                GameScr.info1.addInfo("|0|Auto đánh Boss Napa: Tắt");
            }
        }
    };

    private static readonly Dictionary<string, Action> autoPetActions = new Dictionary<string, Action>
    {
        { "ONdeSua", () => { AutoPetCL.DeSuaLapem = true; GameScr.info1.addInfo("Tự động pem khi đệ sủa: Bật"); } },
        { "OFFdeSua", () => { AutoPetCL.DeSuaLapem = false; GameScr.info1.addInfo("Tự động pem khi đệ sủa: Tắt"); } },
        { "ONdeKOK", () => { AutoPetCL.isKOK = true; new Thread(AutoPetCL.autoDeKOK).Start(); GameScr.info1.addInfo("Auto Up Kaioken: ON"); } },
        { "OFFdeKOK", () => { AutoPetCL.isKOK = false; GameScr.info1.addInfo("Auto Up Kaioken: OFF"); } },
        { "ONdeCoDen", () => { AutoPetCL.autoFlag = true; new Thread(AutoPetCL.autoCoDen).Start(); GameScr.info1.addInfo("Auto Cờ đen chống địch: ON"); } },
        { "OFFdeCoDen", () => { AutoPetCL.autoFlag = false; GameScr.info1.addInfo("Auto Cờ đen chống địch: OFF"); } },
        { "ONdeAutoNhat", () => { AutoPetCL.AutoNhatItemPet = true; GameScr.info1.addInfo("Auto nhặt đồ đệ khi Pem: Bật"); } },
        { "OFFdeAutoNhat", () => { AutoPetCL.AutoNhatItemPet = false; GameScr.info1.addInfo("Auto nhặt đồ đệ khi Pem: Tắt"); } },
        { "ONdeGim", () => { AutoPetCL.aGimPet = true; GameScr.info1.addInfo("Auto Gim Đệ: Bật"); } },
        { "OFFdeGim", () => { AutoPetCL.aGimPet = false; GameScr.info1.addInfo("Auto Gim Đệ: Tắt"); } },
        { "OFFdeTTNL", () => { AutoPetCL.TTNL = false; GameScr.info1.addInfo("Đã tắt Tái tạo năng lượng khi hp,ki thấp"); } },
        { "ONxinDau", () => { AutoPean.isAutoRequestPean = true; GameScr.info1.addInfo("Xin Đậu: ON"); } },
        { "OFFxinDau", () => { AutoPean.isAutoRequestPean = false; GameScr.info1.addInfo("Xin Đậu: OFF"); } },
        { "ONChoDau", () => { AutoPean.isAutoDonatePean = true; GameScr.info1.addInfo("Cho Đậu: ON"); } },
        { "OFFChoDau", () => { AutoPean.isAutoDonatePean = false; GameScr.info1.addInfo("Cho Đậu: OFF"); } },
        { "ONThuDau", () => { AutoPean.isAutoHarvestPean = true; GameScr.info1.addInfo("Thu Đậu: ON"); } },
        { "OFFThuDau", () => { AutoPean.isAutoHarvestPean = false; GameScr.info1.addInfo("Thu Đậu: OFF"); } }
    };

    private static void SafeExecute(Action action)
    {
        try { action(); }
        catch { GameScr.info1.addInfo("|1|Lỗi nhận dữ liệu QLTK"); }
    }

    public static void handlerXamp(string message)
    {
        if (!message.StartsWith("xmap|")) return;
        SafeExecute(() =>
        {
            int num = int.Parse(message.Split('|')[1]);
            if (num == -99) num = Char.myCharz().cgender + 21;
            MainXmapCL.StartGoToMap(num);
        });
    }

    public static void handlerItem(string message)
    {
        if (message.StartsWith("item|"))
            SafeExecute(() => ModProCL.useItem(int.Parse(message.Split('|')[1])));
    }

    public static void handlerConfigTagNameBoss(string message)
    {
        if (!message.StartsWith("TagNameAutoBoss|")) return;
        SafeExecute(() =>
        {
            var names = message.Split('|')[1].Split(',');
            AutoBossCL.targetBossNames.Clear();
            var validNames = new List<string>();
            foreach (var name in names)
            {
                var trimmed = name.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    AutoBossCL.targetBossNames.Add(trimmed);
                    validNames.Add(trimmed);
                }
            }
            var msg = validNames.Count == 0
                ? "Cấu hình rỗng — hệ thống sẽ dò và tấn công tất cả boss."
                : "Đã cấu hình thành công: " + string.Join(", ", validNames.ToArray());
            GameScr.info1.addInfo(msg);
        });
    }

    public static void handlerChat(string message)
    {
        if (message.StartsWith("chat|"))
            SafeExecute(() => Service.gI().chat(message.Split('|')[1]));
    }

    public static void handlerZone(string message)
    {
        if (message.StartsWith("khu|"))
            SafeExecute(() => Service.gI().requestChangeZone(int.Parse(message.Split('|')[1]), -1));
    }

    public static void handlerNhapCodeLive(string message)
    {
        if (message.StartsWith("NhapCodeLive|"))
            SafeExecute(() =>
            {
                var code = message.Split('|')[1];
                GameScr.info1.addInfo(message);
                NhapCodeLive.getInstance().code = code;
                NhapCodeLive.getInstance().isEnable = true;
                NhapCodeLive.getInstance().isGoBack = false;
                NhapCodeLive.getInstance().isTrain = false;
            });
    }

    public static void handlerFPS(string message)
    {
        SafeExecute(() =>
        {
            var parts = message.Split('|');
            if (parts.Length < 2) return;

            int fps = 30;
            if (int.TryParse(parts[1], out var result)) fps = result;

            if (parts[0] == "ONreduceCPU")
            {
                MainMod.toiUuCPU = true;
                GameScr.info1.addInfo("Tối ưu CPU, FPS = " + fps);
                Application.targetFrameRate = fps;
            }
            else if (parts[0] == "OFFreduceCPU")
            {
                MainMod.toiUuCPU = false;
                GameScr.info1.addInfo("Đã tắt tối ưu CPU");
                Application.targetFrameRate = 140;
            }
            QualitySettings.vSyncCount = 0;
        });
    }

    public static void handlerTeleNpc(string message)
    {
        if (message.StartsWith("teleIdNpc|"))
            SafeExecute(() => ModProCL.teleNPC(int.Parse(message.Split('|')[1])));
    }

    public static void handlerListVutON(string message)
    {
        if (message.StartsWith("ONlistvut|"))
            SafeExecute(() =>
            {
                AutoVutDoCL.listVutDo.Clear();
                foreach (var id in message.Split('|')[1].Split(','))
                    AutoVutDoCL.listVutDo.Add(int.Parse(id));
                AutoVutDoCL.autoVut = true;
                GameScr.info1.addInfo("Auto vứt item: " + (AutoVutDoCL.autoVut ? "Bật" : "Tắt"));
            });
    }

    public static void handlerListVutOFF(string message)
    {
        if (message.StartsWith("OFFlistvut|"))
            SafeExecute(() =>
            {
                AutoVutDoCL.listVutDo.Clear();
                AutoVutDoCL.autoVut = false;
                GameScr.info1.addInfo("Auto vứt item: " + (AutoVutDoCL.autoVut ? "Bật" : "Tắt"));
            });
    }

    public static void handlerFarmBossNappa(string message)
    {
        foreach (var kv in farmBossNappaActions)
        {
            if (message.StartsWith(kv.Key))
            {
                SafeExecute(() => kv.Value(message));
                break;
            }
        }
    }

    public static void handlerAutoBoMong(string message)
    {
        if (message.StartsWith("ONBoMong"))
        {
            SafeExecute(() =>
            {
                var parts = message.Split('|');
                if (parts.Length >= 6)
                {
                    // Parse settings
                    AutoboMongCL.Settings.Difficulty = parts[1];
                    AutoboMongCL.Settings.UseGoldSuicideMode = parts[2] == "1";
                    AutoboMongCL.Settings.TrainGold.IsEnabled = parts[3].ToLower() != "true";
                    AutoboMongCL.Settings.TrainMonster.IsEnabled = parts[4].ToLower() != "true";
                    AutoboMongCL.Settings.KillPlayer.IsEnabled = parts[5].ToLower() != "true";

                    // Save and start
                    AutoboMongCL.Settings.SaveToRMS();

                    if (!InfoMe.EndNvBoMong)
                    {
                        AutoboMongCL.autoboMong = true;
                        AutoboMongCL.StartAuto();
                    }
                }
            });
        }
        else if (message.StartsWith("OFFBoMong"))
        {
            SafeExecute(() =>
            {
                AutoboMongCL.autoboMong = false;
                AutoboMongCL.getInstance().StopAuto();
            });
        }
    }

    public static void handlerAutoPet(string message)
    {
        if (message.StartsWith("ONdeTTNL"))
        {
            SafeExecute(() =>
            {
                AutoPetCL.PercentCharge = int.Parse(message.Split('|')[1]);
                if (AutoPetCL.TTNL)
                {
                    AutoPetCL.TTNL = false;
                    GameScr.info1.addInfo("Đã tắt Tái tạo năng lượng khi hp,ki thấp");
                }
                else if (Char.myCharz().getGender() != "XD")
                {
                    GameScr.info1.addInfo("Chức năng chỉ dành cho XD");
                    AutoPetCL.TTNL = false;
                }
                else if (!AutoSkill.checkSkill(8))
                {
                    GameScr.info1.addInfo("Bạn không có skill tái tạo");
                    AutoPetCL.TTNL = false;
                }
                else
                {
                    AutoPetCL.TTNL = true;
                }
            });
        }
        else if (autoPetActions.TryGetValue(message, out var action))
        {
            SafeExecute(action);
        }
    }

    private static void UseBoom()
    {
        Skill skill = null;
        foreach (var s in GameScr.keySkill)
        {
            if (s != null && !s.paintCanNotUseSkill && s.template.id == 14)
            {
                int manaCost = (int)((s.template.manaUseType == 2) ? 1
                    : (s.template.manaUseType == 1) ? (s.manaUse * Char.myCharz().cMPFull / 100)
                    : s.manaUse);
                if (Char.myCharz().cMP >= manaCost && (skill == null || skill.coolDown < s.coolDown))
                    skill = s;
            }
        }
        if (skill != null)
        {
            GameScr.gI().doSelectSkill(skill, isShortcut: true);
            GameScr.gI().doSelectSkill(skill, isShortcut: true);
        }
    }

    private static void TeleportToBoss()
    {
        for (int i = 0; i < GameScr.vCharInMap.size(); i++)
        {
            var obj = (Char)GameScr.vCharInMap.elementAt(i);
            if (MainMod.isBoss(obj) && !Char.myCharz().meDead)
            {
                Char.myCharz().charFocus = obj;
                MainXmapCL.TeleportTo(obj.cx, obj.cy - 1);
                break;
            }
        }
    }

    public static void XuLyDuLieu(string message)
    {
        handlerChat(message);
        handlerItem(message);
        handlerXamp(message);
        handlerZone(message);
        handlerTeleNpc(message);
        handlerListVutON(message);
        handlerListVutOFF(message);
        handlerAutoBoMong(message);
        handlerAutoPet(message);
        handlerFarmBossNappa(message);
        handlerNhapCodeLive(message);
        handlerFPS(message);
        handlerConfigTagNameBoss(message);

        SafeExecute(() =>
        {
            switch (message)
            {
                case "bongtai":
                    MainMod.useHopThe();
                    break;
                case "Boom":
                    UseBoom();
                    break;
                case "BatCoDen":
                    Service.gI().getFlag(1, 8);
                    break;
                case "TatCo":
                    Service.gI().getFlag(1, 0);
                    break;
                case "ONfindBoss":
                    AutoBossCL.aGimBoss = true;
                    GameScr.info1.addInfo("Auto gim boss: ON");
                    break;
                case "OFFfindBoss":
                    AutoBossCL.aGimBoss = false;
                    GameScr.info1.addInfo("Auto gim boss: OFF");
                    break;
                case "teleBoss":
                    TeleportToBoss();
                    break;
                case "ONacttackBoss":
                    ModProCL.tieuDietNguoiBatCo = false;
                    AutoBossCL.tanCongBoss = true;
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().npcFocus = null;
                    AutoBossCL.AutoteleBoss = true;
                    GameScr.info1.addInfo("Tấn công Boss: ON");
                    break;
                case "OFFacttackBoss":
                    AutoBossCL.listBossTrongKhu.Clear();
                    AutoBossCL.AutoteleBoss = false;
                    AutoBossCL.tanCongBoss = false;
                    GameScr.info1.addInfo("Tấn công Boss: OFF");
                    break;
                case "ONdoBoss":
                    AutoBossCL.DoBoss = true;
                    GameScr.info1.addInfo("Dò boss: ON");
                    break;
                case "OFFdoBoss":
                    AutoBossCL.DoBoss = false;
                    AutoBossCL.StopAutoDoBoss();
                    GameScr.info1.addInfo("Dò boss: OFF");
                    break;
                case "ONautocapcha":
                    {
                        if (mGraphics.zoomLevel != 1)
                        {
                            GameScr.info1.addInfo("Chỉ sử dụng ở size game pixel!");
                            MainMod.AutoCapCha = false;
                            break;
                        }
                        string text = "Data";
                        string path = Path.Combine(text, "keyAPI.ini");
                        bool flag = true;
                        if (!Directory.Exists(text))
                        {
                            Directory.CreateDirectory(text);
                            flag = false;
                        }
                        if (!File.Exists(path))
                        {
                            File.WriteAllText(path, "");
                            flag = false;
                        }
                        string value = File.ReadAllText(path).Trim();
                        if (string.IsNullOrEmpty(value))
                        {
                            flag = false;
                        }
                        if (!flag)
                        {
                            GameScr.info1.addInfo("Vui lòng nhập key API ở QLTK mục khác!");
                            MainMod.AutoCapCha = false;
                        }
                        else
                        {
                            MainMod.AutoCapCha = true;
                            CaptchaSolver.countCaptchaSolved = 0;
                            GameScr.info1.addInfo("Auto Giải Capcha: Bật!");
                        }
                    }
                    break;
                case "OFFautocapcha":
                MainMod.AutoCapCha = false;
                GameScr.info1.addInfo("Auto Giải Capcha: Tắt!");
                break;
                case "ONautoWhis":
                    AutoBossCL.aWhis = true;
                    break;
                case "OFFautoWhis":
                    AutoBossCL.aWhis = false;
                    AutoBossCL.StopAutoWhis();
                    break;
                case "ONautochecklag":
                    AutoTrainCL.checkLag = true;
                    GameScr.info1.addInfo("Auto check lag: Bật");
                    break;
                case "OFFautochecklag":
                    AutoTrainCL.checkLag = false;
                    GameScr.info1.addInfo("Auto check lag: Tắt");
                    break;
                case "ONfindBossTrungMabu":
                    AutoBossCL.findBossMod = true;
                    GameScr.info1.addInfo("|0|Auto tìm boss Hirde: Bật");
                    break;
                case "OFFfindBossTrungMabu":
                    AutoBossCL.findBossMod = false;
                    GameScr.info1.addInfo("|0|Auto tìm boss Hirde: Tắt");
                    break;
                case "ONtrainMob":
                    AutoTrainCL.getInstance().perform(2, null);
                    break;
                case "OFFtrainMob":
                    AutoTrainCL.getInstance().perform(8, null);
                    break;
                case "ONgoBack":
                    AutoTrainCL.isGobackCoordinate = false;
                    AutoTrainCL.isGoBack = true;
                    AutoTrainCL.gobackMapID = TileMap.mapID;
                    AutoTrainCL.gobackZoneID = TileMap.zoneID;
                    GameScr.info1.addInfo("Goback\n[" + TileMap.mapNames[AutoTrainCL.gobackMapID] + "]\n[" + AutoTrainCL.gobackZoneID + "]");
                    break;
                case "OFFgoBack":
                    AutoTrainCL.isGoBack = false;
                    GameScr.info1.addInfo("Goback [STATUS: OFF]");
                    break;
                case "OFFautoNeSieuQuai":
                    AutoTrainCL.isAvoidSuperMob = false;
                    GameScr.info1.addInfo("né siêu quái [STATUS: OFF]");
                    break;
                case "ONautoNeSieuQuai":
                    AutoTrainCL.isAvoidSuperMob = true;
                    GameScr.info1.addInfo("né siêu quái [STATUS: ON]");
                    break;
                case "OFFtrainAkDame":
                    AutoTrainCL.TYPEAK = false;
                    GameScr.info1.addInfo("Loại train ak dame [STATUS: OFF]");
                    break;
                case "ONtrainAkDame":
                    AutoTrainCL.TYPEAK = true;
                    GameScr.info1.addInfo("Loại train ak dame [STATUS: ON]");
                    break;
                case "ONAutoNhat":
                    AutoPick.isAutoPick = true;
                    GameScr.info1.addInfo("Auto nhặt [STATUS: ON]");
                    break;
                case "OFFAutoNhat":
                    AutoPick.isAutoPick = false;
                    GameScr.info1.addInfo("Auto nhặt [STATUS: OFF]");
                    break;
                case "ONautoNeBoss":
                    AutoTrainCL.autoNeBoss = true;
                    AutoTrainCL.autoChangeZone = false;
                    AutoTrainCL.SpamChangeZone = false;
                    GameScr.info1.addInfo("Auto né boss [STATUS: ON]");
                    break;
                case "OFFautoNeBoss":
                    AutoTrainCL.autoNeBoss = false;
                    GameScr.info1.addInfo("Auto né boss [STATUS: OFF]");
                    break;
                case "ONautoHopThe":
                    if (ModProCL.ExistPotara() != -1 && TileMap.mapID != Char.myCharz().cgender + 21)
                        AutoTrainCL.autoHopThe = true;
                    GameScr.info1.addInfo("Auto Hợp thể [STATUS: ON]");
                    break;
                case "OFFautoHopThe":
                    AutoTrainCL.autoHopThe = false;
                    GameScr.info1.addInfo("Auto Hợp thể [STATUS: OFF]");
                    break;
                case "ONspamZoneIt":
                    AutoTrainCL.SpamChangeZone = true;
                    GameScr.info1.addInfo("Auto spam khu ít [STATUS: ON]");
                    break;
                case "OFFspamZoneIt":
                    AutoTrainCL.SpamChangeZone = false;
                    GameScr.info1.addInfo("Auto spam khu ít [STATUS: OFF]");
                    break;
                case "ONautoZoneIt":
                    AutoTrainCL.autoChangeZone = true;
                    GameScr.info1.addInfo("Auto khu ít [STATUS: ON]");
                    break;
                case "OFFautoZoneIt":
                    AutoTrainCL.autoChangeZone = false;
                    GameScr.info1.addInfo("Auto khu ít [STATUS: OFF]");
                    break;
                case "ONgoBackToaDo":
                    AutoTrainCL.isGobackCoordinate = true;
                    AutoTrainCL.isGoBack = true;
                    AutoTrainCL.gobackMapID = TileMap.mapID;
                    AutoTrainCL.gobackZoneID = TileMap.zoneID;
                    AutoTrainCL.gobackX = Char.myCharz().cx;
                    AutoTrainCL.gobackY = Char.myCharz().cy;
                    GameScr.info1.addInfo("Goback Tọa Độ\n[" + AutoTrainCL.gobackX + "-" + AutoTrainCL.gobackY + "]");
                    break;
                case "OFFgoBackToaDo":
                    AutoTrainCL.isGoBack = false;
                    GameScr.info1.addInfo("Goback [STATUS: OFF]");
                    break;
            }
        });
    }
}