using System;
using System.Linq;
using Xmap;
using Mod.community;
using UnityEngine;

namespace Mod.CuongLe;

public class AutoboMongCL : IActionListener, IChatable
{
	public enum AutoState
	{
		Idle,
		NavigateToMap,
		TeleConfirmNV,
		TeleConfirmNV_Wait,
		WaitForSignal,
		WaitForSignal_retry,
		CompleteTask,
		HuyNV_Confirm,
		HuyNV_Wait,
		DoneNV_Navigate,
		DoneNV_Wait,
		TrainQuai_WaitNameMap,
		TrainQuai_Navigate,
		TrainQuai_Execute,
		TrainQuai_Wait,
		PemNguoi_Navigate,
		PemNguoi_Execute,
		PemNguoi_Wait,
		TrainVang_Navigate,
		TrainVang_Execute,
		TrainVang_Wait,
		TrainVang2_Navigate,
		TrainVang2_Execute,
		LumVang_Wait
	}

	private static AutoboMongCL _Instance;

	private static readonly object _lock;

	public static bool autoboMong;

	public static bool waitAuto;

	public static string level;

	public static string checkInfo;

	public static bool trainning;

	public static string checknv;

	public static MobInfoCL[] arr;

	public static bool killCharing;

	public static bool trainVang;

	public static bool nextnvVang;

	public static bool nextnvNguoi;

	public static bool nextnvQuai;

	public static bool chooseTypeGod;

	public static bool PickGoding;

	public static int IdMapTrainVang;

	public static int InputMapTrainVang;

	public static bool bomongTuSat;

	public static string StatusBoMong;

	public static int completedTasks;

	public static int cancekTasks;

	public AutoState currentState = AutoState.Idle;

	private float stateTimer = 0f;

	private float delayDuration = 0f;

	private int trainQuaiMapId;

	private int trainQuaiMobId;

	private int pemNguoiMapId;

	private int trainVangMapId;

	private int trainVang2MapId;

	private static int MapNvNguoi;

	private static int ZoneMapNVNguoi;

	public static bool nickKepBoMong;

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
		{
			return;
		}
		switch (currentState)
		{
		case AutoState.Idle:
			break;
		case AutoState.NavigateToMap:
			if (TileMap.mapID != 47)
			{
				if (!MainXmapCL.isXmaping)
				{
					MainXmapCL.StartGoToMap(47);
				}
			}
			else if (!MainXmapCL.isXmaping)
			{
				TransitionTo(AutoState.TeleConfirmNV, 0.15f);
			}
			else
			{
				SetDelay(1f);
			}
			break;
		case AutoState.TeleConfirmNV:
			ModProCL.teleNPC(17);
			ModProCL.startComfirmNpc(17, "nhiệm vụ hàng ngày", level, "", "", "chi tiết nhiệm vụ", "", "", "nhận thưởng");
			TransitionTo(AutoState.TeleConfirmNV_Wait, 0.15f);
			break;
		case AutoState.TeleConfirmNV_Wait:
			if (!ModProCL.confirming)
			{
				TransitionTo(AutoState.WaitForSignal, 1f);
			}
			break;
		case AutoState.WaitForSignal:
			if (!InfoMe.NhanTinHieu && !trainning && !killCharing && !trainVang)
			{
				ModProCL.teleNPC(17);
				ModProCL.startComfirmNpc(17, "nhiệm vụ hàng ngày", level, "", "", "chi tiết nhiệm vụ", "", "", "nhận thưởng");
				TransitionTo(AutoState.WaitForSignal_retry, 0.2f);
			}
			break;
		case AutoState.WaitForSignal_retry:
			if (!ModProCL.confirming)
			{
				TransitionTo(AutoState.WaitForSignal, 1.7f);
			}
			break;
		case AutoState.CompleteTask:
			DoneNV();
			TransitionTo(AutoState.DoneNV_Navigate, 0f);
			break;
		case AutoState.HuyNV_Confirm:
			ModProCL.startComfirmNpc(17, "nhiệm vụ hàng ngày", "hủy nhiệm vụ");
			TransitionTo(AutoState.HuyNV_Wait, 1.7f);
			break;
		case AutoState.HuyNV_Wait:
			if (!ModProCL.confirming)
			{
				cancekTasks++;
				TransitionTo(AutoState.NavigateToMap, 0f);
			}
			break;
		case AutoState.DoneNV_Navigate:
			if (Char.myCharz().meDead)
			{
				Service.gI().returnTownFromDead();
				SetDelay(0.5f);
			}
			else if (TileMap.mapID != 47)
			{
				if (!MainXmapCL.isXmaping)
				{
					MainXmapCL.StartGoToMap(47);
				}
				SetDelay(1.2f);
			}
			else if (!MainXmapCL.isXmaping)
			{
				ModProCL.teleNPC(17);
				ModProCL.startComfirmNpc(17, "nhiệm vụ hàng ngày", "nhận thưởng");
				TransitionTo(AutoState.DoneNV_Wait, 0.15f);
			}
			break;
		case AutoState.DoneNV_Wait:
			if (!ModProCL.confirming)
			{
				trainVang = false;
				killCharing = false;
				trainning = false;
				AutoTrainCL.isGoBack = false;
				InfoMe.FinishBoMong = false;
				InfoMe.NhanTinHieu = false;
				completedTasks++;
				TransitionTo(AutoState.NavigateToMap, 0f);
			}
			break;
		case AutoState.TrainQuai_WaitNameMap:
			if (!string.IsNullOrEmpty(InfoMe.nameMap))
			{
				nvTrainQuai();
			}
			break;
		case AutoState.TrainQuai_Navigate:
			if (TileMap.mapID != trainQuaiMapId)
			{
				if (!MainXmapCL.isXmaping)
				{
					MainXmapCL.StartGoToMap(trainQuaiMapId);
				}
				SetDelay(1f);
			}
			else if (!MainXmapCL.isXmaping)
			{
				int num3 = ((trainQuaiMapId >= 63 && trainQuaiMapId <= 83) ? 1 : RandomZoneFrom2To14());
				if (TileMap.zoneID != num3 && TileMap.zoneID <= 1)
				{
					Service.gI().requestChangeZone(num3, -1);
					SetDelay(1.2f);
				}
				else
				{
					TransitionTo(AutoState.TrainQuai_Execute, 0f);
				}
			}
			break;
		case AutoState.TrainQuai_Execute:
			AutoTrainCL.isGobackCoordinate = false;
			AutoTrainCL.isGoBack = true;
			AutoTrainCL.gobackMapID = TileMap.mapID;
			AutoTrainCL.gobackZoneID = TileMap.zoneID;
			AutoTrainCL.getInstance().perform(1, trainQuaiMobId);
			TransitionTo(AutoState.TrainQuai_Wait, 1.2f);
			break;
		case AutoState.TrainQuai_Wait:
			if (InfoMe.FinishBoMong)
			{
				TransitionTo(AutoState.CompleteTask, 0f);
				StatusBoMong = "Nhận thưởng";
			}
			break;
		case AutoState.PemNguoi_Navigate:
			if (Char.myCharz().isDie)
			{
				break;
			}
			if (TileMap.mapID != pemNguoiMapId)
			{
				if (!MainXmapCL.isXmaping)
				{
					MainXmapCL.StartGoToMap(pemNguoiMapId);
				}
				SetDelay(1f);
			}
			else if (!MainXmapCL.isXmaping)
			{
				int num = 0;
				if (ZoneMapNVNguoi != -1)
				{
					num = ZoneMapNVNguoi;
				}
				if (TileMap.zoneID != num)
				{
					Service.gI().requestChangeZone(num, -1);
					SetDelay(1.2f);
					break;
				}
				AutoTrainCL.isGobackCoordinate = false;
				AutoTrainCL.isGoBack = true;
				AutoTrainCL.gobackMapID = TileMap.mapID;
				AutoTrainCL.gobackZoneID = TileMap.zoneID;
				TransitionTo(AutoState.PemNguoi_Execute, 0f);
			}
			break;
		case AutoState.PemNguoi_Execute:
		{
			int num5 = 0;
			if (ZoneMapNVNguoi != -1)
			{
				num5 = ZoneMapNVNguoi;
			}
			if (InfoMe.FinishBoMong)
			{
				ModProCL.tieuDietNguoiBatCo = false;
				AutoBossCL.tanCongBoss = false;
				ModProCL.listNguoiCoDen.Clear();
				GameScr.info1.addInfo("Chế độ đồ sát người: " + (ModProCL.tieuDietNguoiBatCo ? " ON " : "OFF"));
				TransitionTo(AutoState.CompleteTask, 0f);
				StatusBoMong = "Nhận thưởng";
			}
			else if (Char.myCharz().isDie || GameCanvas.gameTick % 5 != 0)
			{
				SetDelay(0.5f);
			}
			else if (TileMap.mapID != pemNguoiMapId)
			{
				SetDelay(1.5f);
			}
			else if (TileMap.zoneID != num5)
			{
				Service.gI().requestChangeZone(num5, -1);
				SetDelay(1.2f);
			}
			else if (!ModProCL.tieuDietNguoiBatCo)
			{
				AutoBossCL.tanCongBoss = false;
				ModProCL.tieuDietNguoiBatCo = true;
				Char.myCharz().mobFocus = null;
				Char.myCharz().itemFocus = null;
				Char.myCharz().npcFocus = null;
				GameScr.info1.addInfo("Chế độ đồ sát người: " + (ModProCL.tieuDietNguoiBatCo ? " ON " : "OFF"));
				SetDelay(1.5f);
			}
			break;
		}
		case AutoState.TrainVang_Navigate:
			if (TileMap.mapID != trainVangMapId)
			{
				if (!MainXmapCL.isXmaping)
				{
					MainXmapCL.StartGoToMap(trainVangMapId);
				}
				SetDelay(1f);
			}
			else if (!MainXmapCL.isXmaping)
			{
				int num4 = ((trainVangMapId == 10 || trainVangMapId == 68 || trainVangMapId == 77) ? 1 : RandomZoneFrom2To14());
				if (TileMap.zoneID < num4 && TileMap.zoneID < 1)
				{
					Service.gI().requestChangeZone(num4, -1);
					SetDelay(1.2f);
				}
				else
				{
					TransitionTo(AutoState.TrainVang_Execute, 0f);
				}
			}
			break;
		case AutoState.TrainVang_Execute:
		{
			AutoTrainCL.isGobackCoordinate = false;
			AutoTrainCL.isGoBack = true;
			AutoTrainCL.gobackMapID = TileMap.mapID;
			AutoTrainCL.gobackZoneID = TileMap.zoneID;
			AutoPick.isAutoPick = true;
			MainXmapCL.isEatChicken = true;
			AutoPick.pickByList = 0;
			int num2 = ((trainVangMapId == 77) ? 53 : ((trainVangMapId == 68) ? 39 : (-1)));
			AutoTrainCL.getInstance().perform((num2 != -1) ? 1 : 2, (num2 == -1) ? null : ((object)num2));
			TransitionTo(AutoState.TrainVang_Wait, 1.2f);
			break;
		}
		case AutoState.TrainVang_Wait:
			if (InfoMe.FinishBoMong)
			{
				TransitionTo(AutoState.CompleteTask, 0f);
				StatusBoMong = "Nhận thưởng";
			}
			break;
		case AutoState.TrainVang2_Navigate:
			if (Char.myCharz().isDie || ModProCL.suicide)
			{
				break;
			}
			if (TileMap.mapID != trainVang2MapId)
			{
				if (!MainXmapCL.isXmaping)
				{
					MainXmapCL.StartGoToMap(trainVang2MapId);
				}
				SetDelay(1f);
			}
			else if (!MainXmapCL.isXmaping)
			{
				int zoneId2 = RandomZoneFrom2To14();
				if (TileMap.zoneID <= 1)
				{
					Service.gI().requestChangeZone(zoneId2, -1);
					SetDelay(1.2f);
					break;
				}
				AutoTrainCL.isGobackCoordinate = false;
				AutoTrainCL.isGoBack = true;
				AutoTrainCL.gobackMapID = TileMap.mapID;
				AutoTrainCL.gobackZoneID = TileMap.zoneID;
				TransitionTo(AutoState.TrainVang2_Execute, 0f);
			}
			break;
		case AutoState.TrainVang2_Execute:
			if (InfoMe.FinishBoMong)
			{
				ModProCL.suicide = false;
				TransitionTo(AutoState.CompleteTask, 0f);
				StatusBoMong = "Nhận thưởng";
				break;
			}
			if (ModProCL.suicide || Char.myCharz().isDie || GameCanvas.gameTick % 5 != 0)
			{
				SetDelay(0.5f);
				break;
			}
			if (TileMap.mapID != trainVang2MapId)
			{
				SetDelay(1.7f);
				break;
			}
			if (TileMap.zoneID <= 1)
			{
				int zoneId = RandomZoneFrom2To14();
				Service.gI().requestChangeZone(zoneId, -1);
				SetDelay(1.2f);
				break;
			}
			LumVang();
			if (!PickGoding)
			{
				ModProCL.startTuDamBanThan();
				SetDelay(3f);
			}
			SetDelay(1f);
			break;
		case AutoState.LumVang_Wait:
			LumVang();
			break;
		case AutoState.PemNguoi_Wait:
			break;
		}
	}

	private void TransitionTo(AutoState newState, float delay)
	{
		currentState = newState;
		SetDelay(delay);
	}

	private void SetDelay(float delay)
	{
		delayDuration = delay;
		stateTimer = 0f;
	}

	public static void Paint(mGraphics g)
	{
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			autoboMong = !autoboMong;
			if (!autoboMong)
			{
				MainXmapCL.FinishXmap();
				trainVang = false;
				killCharing = false;
				trainning = false;
				AutoTrainCL.isGoBack = false;
				InfoMe.FinishBoMong = false;
				MainXmapCL.isEatChicken = true;
				AutoPick.isAutoPick = false;
				AutoPick.pickByList = 0;
				currentState = AutoState.Idle;
			}
			else if (nickKepBoMong)
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
			break;
		case 2:
			if (level == "siêu khó")
			{
				level = "dễ";
			}
			else if (level == "dễ")
			{
				level = "khó";
			}
			else
			{
				level = "siêu khó";
			}
			ShowMenu();
			break;
		case 3:
			nextnvVang = !nextnvVang;
			ShowMenu();
			break;
		case 5:
			nextnvNguoi = !nextnvNguoi;
			ShowMenu();
			break;
		case 6:
			nextnvQuai = !nextnvQuai;
			ShowMenu();
			break;
		case 7:
			Rms.saveRMSInt("nextnvVang", nextnvVang ? 1 : 0);
			Rms.saveRMSInt("nextnvNguoi", nextnvNguoi ? 1 : 0);
			Rms.saveRMSInt("nextnvQuai", nextnvQuai ? 1 : 0);
			Rms.saveRMSInt("ChooseTypeGod", chooseTypeGod ? 1 : 0);
			Rms.saveRMSString("level", level);
			ChatPopup.addChatPopupMultiLineGame("Đã lưu cài đặt", 0, null);
			break;
		case 8:
			chooseTypeGod = !chooseTypeGod;
			ChatPopup.addChatPopupMultiLineGame(chooseTypeGod ? "Đã chọn tự pem lụm vàng bản thân" : "Đã chọn train quái lụm vàng", 0, null);
			break;
		case 9:
			ChatTextField.gI().strChat = "idMapVang";
			ChatTextField.gI().tfChat.name = "Nhập ID Map làm nv up vàng";
			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 10:
			ChatTextField.gI().strChat = "idMapNvNguoi";
			ChatTextField.gI().tfChat.name = "Nhập ID Map làm nv pem người";
			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 11:
			ChatTextField.gI().strChat = "zoneMapNvNguoi";
			ChatTextField.gI().tfChat.name = "Nhập Khu Map làm nv pem Người";
			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 12:
			nickKepBoMong = !nickKepBoMong;
			if (nickKepBoMong)
			{
				Service.gI().getFlag(1, 8);
				if (Char.myCharz().cFlag == 0)
				{
					nickKepBoMong = false;
					ChatPopup.addChatPopupMultiLineGameline("Cờ đen chưa được bật, mở auto kẹp thất bại. Mở lại đi");
					break;
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
			GameScr.info1.addInfo("Auto nick Kẹp :" + (nickKepBoMong ? "ON" : "OFF"));
			ShowMenu();
			break;
		case 4:
			break;
		}
	}

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command((!autoboMong) ? "Bắt đầu" : "Dừng", getInstance(), 1, null));
		myVector.addElement(new Command("Đổi mức độ\nHiện tại: " + level, getInstance(), 2, null));
		myVector.addElement(new Command("Bỏ nv nhặt vàng: " + (nextnvVang ? "ON" : "OFF"), getInstance(), 3, null));
		myVector.addElement(new Command("Bỏ nv pem người: " + (nextnvNguoi ? "ON" : "OFF"), getInstance(), 5, null));
		myVector.addElement(new Command("Bỏ nv pem quái: " + (nextnvQuai ? "ON" : "OFF"), getInstance(), 6, null));
		myVector.addElement(new Command("Chọn kiểu NV vàng: " + (chooseTypeGod ? "Nhặt vàng" : "Up Quái"), getInstance(), 8, null));
		int num = ((IdMapTrainVang != -1) ? IdMapTrainVang : GetDefaultTrainVangMapId());
		myVector.addElement(new Command("Đổi map up vàng: " + TileMap.mapNames[num], getInstance(), 9, null));
		int num2 = ((MapNvNguoi == -1) ? (Char.myCharz().cgender + 42) : MapNvNguoi);
		int num3 = ((ZoneMapNVNguoi != -1) ? ZoneMapNVNguoi : 0);
		myVector.addElement(new Command("Đổi map nv pem người: " + TileMap.mapNames[num2], getInstance(), 10, null));
		myVector.addElement(new Command("Đổi khu nv pem người: " + num3, getInstance(), 11, null));
		myVector.addElement(new Command("Auto Kẹp nv Pem người: " + (nickKepBoMong ? "ON" : "OFF"), getInstance(), 12, null));
		myVector.addElement(new Command("Lưu cài đặt", getInstance(), 7, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static int GetDefaultTrainVangMapId()
	{
		int taskId = Char.myCharz().taskMaint.taskId;
		long cPower = Char.myCharz().cPower;
		return (taskId < 22) ? 10 : ((taskId == 22 || taskId == 23) ? 68 : ((taskId <= 25) ? 77 : ((taskId >= 33 && cPower >= 60000000000L) ? 155 : 80)));
	}

	public static void loadData()
	{
		nextnvVang = Rms.loadRMSInt("nextnvVang") == 1;
		nextnvNguoi = Rms.loadRMSInt("nextnvNguoi") == 1;
		nextnvQuai = Rms.loadRMSInt("nextnvQuai") == 1;
		chooseTypeGod = Rms.loadRMSInt("ChooseTypeGod") == 1;
		string value = Rms.loadRMSString("level");
		if (!string.IsNullOrEmpty(value))
		{
			level = value;
		}
		else
		{
			level = "siêu khó";
		}
	}

	public static void StartAuto()
	{
		trainVang = false;
		killCharing = false;
		trainning = false;
		AutoTrainCL.isGoBack = false;
		InfoMe.FinishBoMong = false;
		InfoMe.NhanTinHieu = false;
		MainXmapCL.isEatChicken = true;
		AutoPick.isAutoPick = false;
		AutoPick.pickByList = 0;
		AutoTrainCL.TuMoTDLT();
		getInstance().TransitionTo(AutoState.NavigateToMap, 0f);
		StatusBoMong = "nhận nv";
	}

	static AutoboMongCL()
	{
		MapNvNguoi = -1;
		ZoneMapNVNguoi = -1;
		_lock = new object();
		completedTasks = 0;
		cancekTasks = 0;
		StatusBoMong = "nhận nv";
		IdMapTrainVang = -1;
		arr = new MobInfoCL[95]
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
			new MobInfoCL("Thằn lằn bay 2", -1, 28),
			new MobInfoCL("Phi long 2", -1, 29),
			new MobInfoCL("Quỷ bay 2", -1, 30),
			new MobInfoCL("Không tặc", 29, 31),
			new MobInfoCL("Quỷ đầu to", 33, 32),
			new MobInfoCL("Quỷ địa ngục", 37, 33),
			new MobInfoCL("Lính độc nhãn", -1, 34),
			new MobInfoCL("Lính độc nhãn", -1, 35),
			new MobInfoCL("Sói xám", -1, 36),
			new MobInfoCL("Robot bay", -1, 37),
			new MobInfoCL("Robot thép", -1, 38),
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
			new MobInfoCL("Hirudegarn", -1, 70),
			new MobInfoCL("Vua Bạch Tuộc", -1, 71),
			new MobInfoCL("Rôbốt bảo vệ", -1, 72),
			new MobInfoCL("Kawazu", -1, 73),
			new MobInfoCL("Kinkarn", -1, 74),
			new MobInfoCL("Arbee", -1, 75),
			new MobInfoCL("Cỗ máy hủy diệt", -1, 76),
			new MobInfoCL("Gấu tướng cướp", -1, 77),
			new MobInfoCL("Khỉ lông xanh", 155, 78),
			new MobInfoCL("Taburine Đỏ", 155, 79),
			new MobInfoCL("Cabira", -1, 80),
			new MobInfoCL("Tobi", -1, 81),
			new MobInfoCL("Voi Chín Ngà", -1, 82),
			new MobInfoCL("Gà Chín Cựa", -1, 83),
			new MobInfoCL("Ngựa Chín Lmao", -1, 84),
			new MobInfoCL("Piano", -1, 85),
			new MobInfoCL("Ếch mặt đỏ", 166, 86),
			new MobInfoCL("Jinai", 166, 87),
			new MobInfoCL("Quỷ đỏ", -1, 88),
			new MobInfoCL("Quỷ xanh", -1, 89),
			new MobInfoCL("Quỷ xanh lá", -1, 90),
			new MobInfoCL("Quỷ vàng", -1, 91),
			new MobInfoCL("Godzilla", -1, 92),
			new MobInfoCL("Kong", -1, 93),
			new MobInfoCL("Máy đo sức mạnh", 42, 94)
		};
		waitAuto = true;
		checkInfo = "thời gian nhận nhiệm vụ";
		checknv = "đã hết nhiệm vụ cho hôm nay, hãy chờ đến ngày mai";
	}

	public static int typeNV(string x)
	{
		if (x.Contains("địa điểm"))
		{
			return 0;
		}
		if (x.Contains("vàng"))
		{
			return 1;
		}
		if (x.Contains("người"))
		{
			return 2;
		}
		if (x.Contains("ăn trộm"))
		{
			return 3;
		}
		return 4;
	}

	public static void huyNV()
	{
		getInstance().TransitionTo(AutoState.HuyNV_Confirm, 0.15f);
	}

	public static void DoneNV()
	{
		if (InfoMe.FinishBoMong)
		{
			if (AutoPick.isAutoPick)
			{
				AutoPick.isAutoPick = false;
				AutoPick.pickByList = 0;
			}
			if (!MainXmapCL.isEatChicken)
			{
				MainXmapCL.isEatChicken = true;
			}
			AutoTrainCL.isGoBack = false;
			ModProCL.suicide = false;
			InfoMe.NhanTinHieu = false;
			AutoTrainCL.getInstance().perform(8, null);
			getInstance().TransitionTo(AutoState.DoneNV_Navigate, 0f);
			if (ModProCL.tieuDietNguoiBatCo)
			{
				ModProCL.tieuDietNguoiBatCo = false;
				AutoBossCL.tanCongBoss = false;
				ModProCL.listNguoiCoDen.Clear();
				GameScr.info1.addInfo("Chế độ đồ sát người: " + (ModProCL.tieuDietNguoiBatCo ? " ON " : "OFF"));
			}
			StatusBoMong = "Nhận thưởng";
		}
	}

	public void nvTrainQuai()
	{
		try
		{
			trainning = true;
			if (string.IsNullOrEmpty(InfoMe.nameMap))
			{
				TransitionTo(AutoState.TrainQuai_WaitNameMap, 1f);
				return;
			}
			string mapModID = GetMapModID(InfoMe.nameMap);
			if (mapModID == "-1|-1")
			{
				HandleTrainError("Lỗi tìm map quái");
				return;
			}
			string[] array = mapModID.Split('|');
			if (!int.TryParse(array[0], out trainQuaiMapId) || !int.TryParse(array[1], out trainQuaiMobId))
			{
				HandleTrainError("Lỗi tìm map quái");
			}
			else
			{
				TransitionTo(AutoState.TrainQuai_Navigate, 0f);
			}
		}
		catch (Exception)
		{
			HandleTrainError("Lỗi tìm map quái");
		}
	}

	private void HandleTrainError(string message)
	{
		autoboMong = false;
		trainning = false;
		ChatPopup.addChatPopupMultiLineGame(message, 0, null);
		currentState = AutoState.Idle;
	}

	public static string GetMapModID(string NameMob)
	{
		if (NameMob == null || NameMob.Trim() == "")
		{
			return "-1|-1";
		}
		string text = NameMob.ToLower().Trim();
		int num = -1;
		string result = "-1|-1";
		MobInfoCL[] array = arr;
		MobInfoCL[] array2 = array;
		MobInfoCL[] array3 = array2;
		foreach (MobInfoCL mobInfoCL in array3)
		{
			string text2 = mobInfoCL.NameMob.ToLower().Trim();
			if (text.Contains(text2) && text2.Length > num)
			{
				num = text2.Length;
				result = $"{mobInfoCL.IdMap}|{mobInfoCL.IdMob}";
			}
		}
		return result;
	}

	public void nvPemNguoi()
	{
		killCharing = true;
		MainXmapCL.isEatChicken = false;
		pemNguoiMapId = Char.myCharz().cgender + 42;
		if (MapNvNguoi != -1)
		{
			pemNguoiMapId = MapNvNguoi;
		}
		TransitionTo(AutoState.PemNguoi_Navigate, 0f);
	}

	public void nvTrainVang()
	{
		trainVang = true;
		trainVangMapId = ((IdMapTrainVang != -1) ? IdMapTrainVang : GetDefaultTrainVangMapId());
		TransitionTo(AutoState.TrainVang_Navigate, 0f);
	}

	public void nvTrainVang2()
	{
		trainVang = true;
		MainXmapCL.isEatChicken = false;
		trainVang2MapId = Char.myCharz().cgender + 42;
		TransitionTo(AutoState.TrainVang2_Navigate, 0f);
	}

	public void LumVang()
	{
		bool flag = false;
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			if (itemMap != null && itemMap.playerId == Char.myCharz().charID && (itemMap.template.id == 76 || itemMap.template.id == 188 || itemMap.template.id == 189 || itemMap.template.id == 190))
			{
				flag = true;
				MainXmapCL.TeleportTo(itemMap.x, itemMap.y);
				Service.gI().pickItem(itemMap.itemMapID);
			}
		}
		PickGoding = flag;
		if (flag)
		{
			TransitionTo(AutoState.LumVang_Wait, 0.5f);
		}
		else
		{
			TransitionTo(AutoState.TrainVang2_Execute, 0f);
		}
	}

	public static void selectSkill()
	{
		int[] source = new int[4] { 0, 2, 4, 17 };
		Skill selectedSkill = null;
		Skill[] keySkill = GameScr.keySkill;
		foreach (Skill s in keySkill)
		{
			if (s != null && source.Contains(s.template.id))
			{
				if (s.template.id == 17)
				{
					selectedSkill = s;
					break;
				}
				if (selectedSkill == null)
				{
					selectedSkill = s;
				}
			}
		}
		if (selectedSkill != null && selectedSkill != Char.myCharz().myskill)
		{
			GameScr.gI().doSelectSkill(selectedSkill, isShortcut: true);
		}
	}

	public static int RandomZoneFrom2To14()
	{
		return UnityEngine.Random.Range(2, 15);
	}

	public void onChatFromMe(string text, string to)
	{
		if (string.IsNullOrEmpty(text) || ChatTextField.gI().tfChat.getText() == null)
		{
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals("idMapVang"))
		{
			try
			{
				if (string.IsNullOrEmpty(text.Trim()))
				{
					GameScr.info1.addInfo("Nhập đi chứ dm!");
					return;
				}
				if (int.TryParse(text.Trim(), out var result) && result >= 0 && result < TileMap.mapNames.Length && TileMap.mapNames[result] != null)
				{
					IdMapTrainVang = result;
					GameScr.info1.addInfo("Đã đổi map up vàng thành " + TileMap.mapNames[IdMapTrainVang]);
				}
				else
				{
					GameScr.info1.addInfo("Id Map up vàng không tồn tại !");
				}
			}
			catch
			{
				GameScr.info1.addInfo("Vui lòng nhập đúng định dạng Id Map");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals("idMapNvNguoi"))
		{
			try
			{
				if (string.IsNullOrEmpty(text.Trim()))
				{
					GameScr.info1.addInfo("Nhập đi chứ dm!");
					return;
				}
				if (int.TryParse(text.Trim(), out var result2) && result2 >= 0 && result2 < TileMap.mapNames.Length && TileMap.mapNames[result2] != null)
				{
					MapNvNguoi = result2;
					GameScr.info1.addInfo("Đã đổi map up pem người thành " + TileMap.mapNames[MapNvNguoi]);
				}
				else
				{
					GameScr.info1.addInfo("Id Map pem người không tồn tại !");
				}
			}
			catch
			{
				GameScr.info1.addInfo("Vui lòng nhập đúng định dạng Id Map");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals("zoneMapNvNguoi"))
		{
			try
			{
				if (string.IsNullOrEmpty(text.Trim()))
				{
					GameScr.info1.addInfo("Nhập đi chứ dm!");
					return;
				}
				if (int.TryParse(text.Trim(), out var result3) && result3 >= 0 && result3 < TileMap.mapNames.Length && TileMap.mapNames[result3] != null)
				{
					ZoneMapNVNguoi = result3;
					GameScr.info1.addInfo("Đã đổi khu pem người thành " + ZoneMapNVNguoi);
				}
				else
				{
					GameScr.info1.addInfo("Khu pem người không tồn tại !");
				}
			}
			catch
			{
				GameScr.info1.addInfo("Vui lòng nhập đúng định dạng khu");
			}
			ResetChatTextField();
		}
		else
		{
			Service.gI().chat(text);
			ChatTextField.gI().isShow = false;
		}
	}

	public void onCancelChat()
	{
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().isShow = false;
	}
}
