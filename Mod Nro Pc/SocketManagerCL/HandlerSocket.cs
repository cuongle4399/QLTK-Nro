using System;
using System.Collections.Generic;
using System.Threading;
using Xmap;
using main.Mod;
using Mod.community;
using Mod.CuongLe;
using UnityEngine;

namespace SocketManagerCL;

internal class HandlerSocket
{
	private static readonly Dictionary<string, Action<string>> farmBossNappaActions = new Dictionary<string, Action<string>>
	{
		{
			"ONfarmNappa",
			delegate(string msg)
			{
				AutoFarmBossNappa.typeBoss = int.Parse(msg.Split('|')[1]);
				AutoFarmBossNappa.DoSatBossNapa = true;
				GameScr.info1.addInfo("|0|Auto đánh Boss Napa: Bật");
			}
		},
		{
			"OFFfarmNappa",
			delegate
			{
				AutoFarmBossNappa.DoSatBossNapa = false;
				AutoFarmBossNappa.Stop();
				GameScr.info1.addInfo("|0|Auto đánh Boss Napa: Tắt");
			}
		}
	};

	private static readonly Dictionary<string, Action> autoPetActions = new Dictionary<string, Action>
	{
		{
			"ONdeSua",
			delegate
			{
				AutoPetCL.DeSuaLapem = true;
				GameScr.info1.addInfo("Tự động pem khi đệ sủa: Bật");
			}
		},
		{
			"OFFdeSua",
			delegate
			{
				AutoPetCL.DeSuaLapem = false;
				GameScr.info1.addInfo("Tự động pem khi đệ sủa: Tắt");
			}
		},
		{
			"ONdeKOK",
			delegate
			{
				AutoPetCL.isKOK = true;
				new Thread(AutoPetCL.autoDeKOK).Start();
				GameScr.info1.addInfo("Auto Up Kaioken: ON");
			}
		},
		{
			"OFFdeKOK",
			delegate
			{
				AutoPetCL.isKOK = false;
				GameScr.info1.addInfo("Auto Up Kaioken: OFF");
			}
		},
		{
			"ONdeCoDen",
			delegate
			{
				AutoPetCL.autoFlag = true;
				new Thread(AutoPetCL.autoCoDen).Start();
				GameScr.info1.addInfo("Auto Cờ đen chống địch: ON");
			}
		},
		{
			"OFFdeCoDen",
			delegate
			{
				AutoPetCL.autoFlag = false;
				GameScr.info1.addInfo("Auto Cờ đen chống địch: OFF");
			}
		},
		{
			"ONdeAutoNhat",
			delegate
			{
				AutoPetCL.AutoNhatItemPet = true;
				GameScr.info1.addInfo("Auto nhặt đồ đệ khi Pem: Bật");
			}
		},
		{
			"OFFdeAutoNhat",
			delegate
			{
				AutoPetCL.AutoNhatItemPet = false;
				GameScr.info1.addInfo("Auto nhặt đồ đệ khi Pem: Tắt");
			}
		},
		{
			"ONdeGim",
			delegate
			{
				AutoPetCL.aGimPet = true;
				GameScr.info1.addInfo("Auto Gim Đệ: Bật");
			}
		},
		{
			"OFFdeGim",
			delegate
			{
				AutoPetCL.aGimPet = false;
				GameScr.info1.addInfo("Auto Gim Đệ: Tắt");
			}
		},
		{
			"OFFdeTTNL",
			delegate
			{
				AutoPetCL.TTNL = false;
				GameScr.info1.addInfo("Đã tắt Tái tạo năng lượng khi hp,ki thấp");
			}
		},
		{
			"ONxinDau",
			delegate
			{
				AutoPean.isAutoRequestPean = true;
				GameScr.info1.addInfo("Xin Đậu: ON");
			}
		},
		{
			"OFFxinDau",
			delegate
			{
				AutoPean.isAutoRequestPean = false;
				GameScr.info1.addInfo("Xin Đậu: OFF");
			}
		},
		{
			"ONChoDau",
			delegate
			{
				AutoPean.isAutoDonatePean = true;
				GameScr.info1.addInfo("Cho Đậu: ON");
			}
		},
		{
			"OFFChoDau",
			delegate
			{
				AutoPean.isAutoDonatePean = false;
				GameScr.info1.addInfo("Cho Đậu: OFF");
			}
		},
		{
			"ONThuDau",
			delegate
			{
				AutoPean.isAutoHarvestPean = true;
				GameScr.info1.addInfo("Thu Đậu: ON");
			}
		},
		{
			"OFFThuDau",
			delegate
			{
				AutoPean.isAutoHarvestPean = false;
				GameScr.info1.addInfo("Thu Đậu: OFF");
			}
		}
	};

	private static void SafeExecute(Action action)
	{
		try
		{
			action();
		}
		catch
		{
			GameScr.info1.addInfo("|1|Lỗi nhận dữ liệu QLTK");
		}
	}

	public static void handlerXamp(string message)
	{
		if (!message.StartsWith("xmap|"))
		{
			return;
		}
		SafeExecute(delegate
		{
			int num = int.Parse(message.Split('|')[1]);
			if (num == -99)
			{
				num = Char.myCharz().cgender + 21;
			}
			MainXmapCL.StartGoToMap(num);
		});
	}

	public static void handlerItem(string message)
	{
		if (message.StartsWith("item|"))
		{
			SafeExecute(delegate
			{
				ModProCL.useItem(int.Parse(message.Split('|')[1]));
			});
		}
	}

	public static void handlerConfigTagNameBoss(string message)
	{
		if (!message.StartsWith("TagNameAutoBoss|"))
		{
			return;
		}
		SafeExecute(delegate
		{
			string[] array = message.Split('|')[1].Split(',');
			AutoBossCL.targetBossNames.Clear();
			List<string> list = new List<string>();
			string[] array2 = array;
			string[] array3 = array2;
			string[] array4 = array3;
			string[] array5 = array4;
			string[] array6 = array5;
			string[] array7 = array6;
			foreach (string text in array7)
			{
				string text2 = text.Trim();
				if (!string.IsNullOrEmpty(text2))
				{
					AutoBossCL.targetBossNames.Add(text2);
					list.Add(text2);
				}
			}
			string text3 = string.Join(", ", list.ToArray());
			if (list.Count == 0)
			{
				GameScr.info1.addInfo("Cấu hình rỗng — hệ thống sẽ dò và tấn công tất cả boss.");
			}
			else
			{
				GameScr.info1.addInfo("Đã cấu hình thành công: " + text3);
			}
		});
	}

	public static void handlerChat(string message)
	{
		if (message.StartsWith("chat|"))
		{
			SafeExecute(delegate
			{
				string[] array = message.Split('|');
				Service.gI().chat(array[1]);
			});
		}
	}

	public static void handlerZone(string message)
	{
		if (message.StartsWith("khu|"))
		{
			SafeExecute(delegate
			{
				string[] array = message.Split('|');
				Service.gI().requestChangeZone(int.Parse(array[1]), -1);
			});
		}
	}

	public static void handlerNhapCodeLive(string message)
	{
		if (message.StartsWith("NhapCodeLive|"))
		{
			SafeExecute(delegate
			{
				string[] array = message.Split('|');
				GameScr.info1.addInfo(message);
				NhapCodeLive.getInstance().code = array[1];
				NhapCodeLive.getInstance().isEnable = true;
				NhapCodeLive.getInstance().isGoBack = false;
				NhapCodeLive.getInstance().isTrain = false;
			});
		}
	}

	public static void handlerFPS(string message)
	{
		SafeExecute(delegate
		{
			string[] array = message.Split('|');
			if (array.Length >= 2)
			{
				string text = array[0];
				int num = 30;
				if (array.Length > 1 && int.TryParse(array[1], out var result))
				{
					num = result;
				}
				string text2 = text;
				string text3 = text2;
				if (!(text3 == "ONreduceCPU"))
				{
					if (text3 == "OFFreduceCPU")
					{
						MainMod.toiUuCPU = false;
						GameScr.info1.addInfo("Đã tắt tối ưu CPU");
						QualitySettings.vSyncCount = 0;
						Application.targetFrameRate = 140;
					}
				}
				else
				{
					MainMod.toiUuCPU = true;
					GameScr.info1.addInfo($"Tối ưu CPU, FPS = {num}");
					QualitySettings.vSyncCount = 0;
					Application.targetFrameRate = num;
				}
			}
		});
	}

	public static void handlerTeleNpc(string message)
	{
		if (message.StartsWith("teleIdNpc|"))
		{
			SafeExecute(delegate
			{
				ModProCL.teleNPC(int.Parse(message.Split('|')[1]));
			});
		}
	}

	public static void handlerFarmBossNappa(string message)
	{
		foreach (KeyValuePair<string, Action<string>> kv in farmBossNappaActions)
		{
			if (message.StartsWith(kv.Key))
			{
				SafeExecute(delegate
				{
					kv.Value(message);
				});
				break;
			}
		}
	}

	public static void handlerAutoBoMong(string message)
	{
		if (message.StartsWith("ONBoMong"))
		{
			SafeExecute(delegate
			{
				string[] array = message.Split('|');
				AutoboMongCL.level = array[1];
				AutoboMongCL.chooseTypeGod = array[2] == "1";
				AutoboMongCL.nextnvVang = array[3].ToLower() == "true";
				AutoboMongCL.nextnvQuai = array[4].ToLower() == "true";
				AutoboMongCL.nextnvNguoi = array[5].ToLower() == "true";
				AutoboMongCL.autoboMong = true;
				AutoboMongCL.StartAuto();
			});
		}
		else if (message.StartsWith("OFFBoMong"))
		{
			SafeExecute(delegate
			{
				MainXmapCL.FinishXmap();
				AutoboMongCL.autoboMong = false;
				AutoboMongCL.trainVang = false;
				AutoboMongCL.killCharing = false;
				AutoboMongCL.trainning = false;
				AutoTrainCL.isGoBack = false;
				InfoMe.FinishBoMong = false;
				MainXmapCL.isEatChicken = true;
				AutoPick.isAutoPick = false;
				AutoPick.pickByList = 0;
				AutoboMongCL.getInstance().currentState = AutoboMongCL.AutoState.Idle;
			});
		}
	}

	public static void handlerAutoPet(string message)
	{
		Action value;
		if (message.StartsWith("ONdeTTNL"))
		{
			SafeExecute(delegate
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
		else if (autoPetActions.TryGetValue(message, out value))
		{
			SafeExecute(value);
		}
	}

	public static void XuLyDuLieu(string message)
	{
		handlerChat(message);
		handlerItem(message);
		handlerXamp(message);
		handlerZone(message);
		handlerTeleNpc(message);
		handlerAutoBoMong(message);
		handlerAutoPet(message);
		handlerFarmBossNappa(message);
		handlerNhapCodeLive(message);
		handlerFPS(message);
		handlerConfigTagNameBoss(message);
		switch (message)
		{
		case "bongtai":
			SafeExecute(delegate
			{
				MainMod.useHopThe();
			});
			break;
		case "Boom":
			SafeExecute(delegate
			{
				Skill skill = null;
				Skill[] keySkill = GameScr.keySkill;
				Skill[] array = keySkill;
				Skill[] array2 = array;
				Skill[] array3 = array2;
				Skill[] array4 = array3;
				Skill[] array5 = array4;
				Skill[] array6 = array5;
				Skill[] array7 = array6;
				foreach (Skill skill2 in array7)
				{
					if (skill2 != null && !skill2.paintCanNotUseSkill && skill2.template.id == 14)
					{
						int num = (int)((skill2.template.manaUseType == 2) ? 1 : ((skill2.template.manaUseType == 1) ? (skill2.manaUse * Char.myCharz().cMPFull / 100) : skill2.manaUse));
						if (Char.myCharz().cMP >= num && (skill == null || skill.coolDown < skill2.coolDown))
						{
							skill = skill2;
						}
					}
				}
				if (skill != null)
				{
					GameScr.gI().doSelectSkill(skill, isShortcut: true);
					GameScr.gI().doSelectSkill(skill, isShortcut: true);
				}
			});
			break;
		case "BatCoDen":
			SafeExecute(delegate
			{
				Service.gI().getFlag(1, 8);
			});
			break;
		case "TatCo":
			SafeExecute(delegate
			{
				Service.gI().getFlag(1, 0);
			});
			break;
		case "ONfindBoss":
			SafeExecute(delegate
			{
				AutoBossCL.aGimBoss = true;
				GameScr.info1.addInfo("Auto gim boss: ON");
			});
			break;
		case "OFFfindBoss":
			SafeExecute(delegate
			{
				AutoBossCL.aGimBoss = false;
				GameScr.info1.addInfo("Auto gim boss: OFF");
			});
			break;
		case "teleBoss":
			SafeExecute(delegate
			{
				for (int i = 0; i < GameScr.vCharInMap.size(); i++)
				{
					Char obj = (Char)GameScr.vCharInMap.elementAt(i);
					if (MainMod.isBoss(obj) && !Char.myCharz().meDead)
					{
						Char.myCharz().charFocus = obj;
						MainXmapCL.TeleportTo(obj.cx, obj.cy - 1);
						break;
					}
				}
			});
			break;
		case "ONacttackBoss":
			SafeExecute(delegate
			{
				ModProCL.tieuDietNguoiBatCo = false;
				AutoBossCL.tanCongBoss = true;
				Char.myCharz().mobFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().npcFocus = null;
				AutoBossCL.AutoteleBoss = true;
				GameScr.info1.addInfo("Tấn công Boss: ON");
			});
			break;
		case "OFFacttackBoss":
			SafeExecute(delegate
			{
				AutoBossCL.listBossTrongKhu.Clear();
				AutoBossCL.AutoteleBoss = false;
				AutoBossCL.tanCongBoss = false;
				GameScr.info1.addInfo("Tấn công Boss: OFF");
			});
			break;
		case "ONdoBoss":
			SafeExecute(delegate
			{
				AutoBossCL.DoBoss = true;
				GameScr.info1.addInfo("Dò boss: ON");
			});
			break;
		case "OFFdoBoss":
			SafeExecute(delegate
			{
				AutoBossCL.DoBoss = false;
				AutoBossCL.StopAutoDoBoss();
				GameScr.info1.addInfo("Dò boss: OFF");
			});
			break;
		case "ONautoWhis":
			SafeExecute(delegate
			{
				AutoBossCL.aWhis = true;
			});
			break;
		case "OFFautoWhis":
			SafeExecute(delegate
			{
				AutoBossCL.aWhis = false;
				AutoBossCL.StopAutoWhis();
			});
			break;
		case "ONfindBossTrungMabu":
			SafeExecute(delegate
			{
				AutoBossCL.findBossMod = true;
				GameScr.info1.addInfo("|0|Auto tìm boss Hirde: Bật");
			});
			break;
		case "OFFfindBossTrungMabu":
			SafeExecute(delegate
			{
				AutoBossCL.findBossMod = false;
				GameScr.info1.addInfo("|0|Auto tìm boss Hirde: Tắt");
			});
			break;
		case "ONtrainMob":
			SafeExecute(delegate
			{
				AutoTrainCL.getInstance().perform(2, null);
			});
			break;
		case "OFFtrainMob":
			SafeExecute(delegate
			{
				AutoTrainCL.getInstance().perform(8, null);
			});
			break;
		case "ONgoBack":
			SafeExecute(delegate
			{
				AutoTrainCL.isGobackCoordinate = false;
				AutoTrainCL.isGoBack = true;
				AutoTrainCL.gobackMapID = TileMap.mapID;
				AutoTrainCL.gobackZoneID = TileMap.zoneID;
				GameScr.info1.addInfo($"Goback\n[{TileMap.mapNames[AutoTrainCL.gobackMapID]}]\n[{AutoTrainCL.gobackZoneID}]");
			});
			break;
		case "OFFgoBack":
			SafeExecute(delegate
			{
				AutoTrainCL.isGoBack = false;
				GameScr.info1.addInfo("Goback [STATUS: OFF]");
			});
			break;
		case "OFFautoNeSieuQuai":
			SafeExecute(delegate
			{
				AutoTrainCL.isAvoidSuperMob = false;
				GameScr.info1.addInfo("né siêu quái [STATUS: OFF]");
			});
			break;
		case "ONautoNeSieuQuai":
			SafeExecute(delegate
			{
				AutoTrainCL.isAvoidSuperMob = true;
				GameScr.info1.addInfo("né siêu quái [STATUS: ON]");
			});
			break;
		case "OFFtrainAkDame":
			SafeExecute(delegate
			{
				AutoTrainCL.TYPEAK = false;
				GameScr.info1.addInfo("Loại train ak dame [STATUS: OFF]");
			});
			break;
		case "ONtrainAkDame":
			SafeExecute(delegate
			{
				AutoTrainCL.TYPEAK = true;
				GameScr.info1.addInfo("Loại train ak dame [STATUS: ON]");
			});
			break;
		case "ONAutoNhat":
			SafeExecute(delegate
			{
				AutoPick.isAutoPick = true;
				GameScr.info1.addInfo("Auto nhặt [STATUS: ON]");
			});
			break;
		case "OFFAutoNhat":
			SafeExecute(delegate
			{
				AutoPick.isAutoPick = false;
				GameScr.info1.addInfo("Auto nhặt [STATUS: OFF]");
			});
			break;
		case "ONautoNeBoss":
			SafeExecute(delegate
			{
				AutoTrainCL.autoNeBoss = true;
				AutoTrainCL.autoChangeZone = false;
				AutoTrainCL.SpamChangeZone = false;
				GameScr.info1.addInfo("Auto né boss [STATUS: ON]");
			});
			break;
		case "OFFautoNeBoss":
			SafeExecute(delegate
			{
				AutoTrainCL.autoNeBoss = false;
				GameScr.info1.addInfo("Auto né boss [STATUS: OFF]");
			});
			break;
		case "ONautoHopThe":
			SafeExecute(delegate
			{
				if (ModProCL.ExistPotara() != -1 && TileMap.mapID != Char.myCharz().cgender + 21)
				{
					AutoTrainCL.autoHopThe = true;
				}
				GameScr.info1.addInfo("Auto Hợp thể [STATUS: ON]");
			});
			break;
		case "OFFautoHopThe":
			SafeExecute(delegate
			{
				AutoTrainCL.autoHopThe = false;
				GameScr.info1.addInfo("Auto Hợp thể [STATUS: OFF]");
			});
			break;
		case "ONspamZoneIt":
			SafeExecute(delegate
			{
				AutoTrainCL.SpamChangeZone = true;
				GameScr.info1.addInfo("Auto spam khu ít [STATUS: ON]");
			});
			break;
		case "OFFspamZoneIt":
			SafeExecute(delegate
			{
				AutoTrainCL.SpamChangeZone = false;
				GameScr.info1.addInfo("Auto spam khu ít [STATUS: OFF]");
			});
			break;
		case "ONautoZoneIt":
			SafeExecute(delegate
			{
				AutoTrainCL.autoChangeZone = true;
				GameScr.info1.addInfo("Auto khu ít [STATUS: ON]");
			});
			break;
		case "OFFautoZoneIt":
			SafeExecute(delegate
			{
				AutoTrainCL.autoChangeZone = false;
				GameScr.info1.addInfo("Auto khu ít [STATUS: OFF]");
			});
			break;
		case "ONgoBackToaDo":
			SafeExecute(delegate
			{
				AutoTrainCL.isGobackCoordinate = true;
				AutoTrainCL.isGoBack = true;
				AutoTrainCL.gobackMapID = TileMap.mapID;
				AutoTrainCL.gobackZoneID = TileMap.zoneID;
				AutoTrainCL.gobackX = Char.myCharz().cx;
				AutoTrainCL.gobackY = Char.myCharz().cy;
				GameScr.info1.addInfo($"Goback Tọa Độ\n[{AutoTrainCL.gobackX}-{AutoTrainCL.gobackY}]");
			});
			break;
		case "OFFgoBackToaDo":
			SafeExecute(delegate
			{
				AutoTrainCL.isGoBack = false;
				GameScr.info1.addInfo("Goback [STATUS: OFF]");
			});
			break;
		}
	}
}
