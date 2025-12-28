using System;
using System.Collections.Generic;
using Assets.src.g;
using Xmap;
using DoHoa;
using Mod.CuongLe;

public class AutoBuyItemCL : IActionListener, IChatable
{
	public class itemBuy
	{
		public int idItem;

		public int idIconItem;

		public int quantityBuy;

		public int mapBuyItem;

		public int sellPrice;

		public short idNpcBuyItem;

		public string nameCofirm;

		public string nameConfirm2;

		public string nameConfirm3;

		public bool itemTDLT;

		public itemBuy(int idItem, int idIconItem, int quantityBuy, int mapBuyItem, short idNpcBuyItem, int sellPrice, string nameCofirm, string nameConfirm2 = "", string nameConfirm3 = "")
		{
			this.idItem = idItem;
			this.idIconItem = idIconItem;
			this.quantityBuy = quantityBuy;
			this.sellPrice = sellPrice;
			this.mapBuyItem = mapBuyItem;
			this.idNpcBuyItem = idNpcBuyItem;
			this.nameCofirm = nameCofirm;
			this.nameConfirm2 = nameConfirm2;
			this.nameConfirm3 = nameConfirm3;
			itemTDLT = ((idItem == 521 || (uint)(idItem - 1523) <= 1u) ? true : false);
		}
	}

	public static bool AutoMuaTDLT;

	public static bool AutoMuaKhauTrang;

	public static bool AutoMuaCo;

	public static bool AutoMuaBuaX2Detu;

	public static bool CheckGoBack;

	public static bool checkTRain;

	public static bool neBoss;

	public static bool SpamZoneItNguoi;

	public static bool BlockPaintShop;

	private static readonly string[] titleInput = new string[1] { "Nhập số lượng item cần mua" };

	private static itemBuy pendingItem;

	private static itemBuy currentBuyingItem;

	private static AutoBuyItemCL _Instance;

	private static int retryDelayCounter;

	private static int retryBuyCounter;

	private static int delayReturnCounter;

	private static int waitConfirm = 1;

	private const int retryDelayTime = 30;

	private const int maxRetryBuy = 10;

	private const int delayReturnTime = 100;

	public static List<itemBuy> listItemBuy = new List<itemBuy>();

	public static AutoBuyItemCL getInstance()
	{
		return _Instance ?? (_Instance = new AutoBuyItemCL());
	}

	public void perform(int IdAction, object p)
	{
		if (IdAction != 2 && !AutoTrainCL.isGoBack)
		{
			ChatPopup.addChatPopupMultiLineGameline("Vui lòng bật Goback để biết đường quay lại sau khi mua item!");
			return;
		}
		switch (IdAction)
		{
		case 1:
			AddItemToList(p as itemBuy);
			break;
		case 2:
			RemoveItemFromList(p as itemBuy);
			break;
		case 3:
			ToggleAutoTDLT();
			break;
		case 4:
			ToggleAutoItem(ref AutoMuaCo, 1635, 13618, 10, "cửa hàng", "Auto Mua Cỏ");
			break;
		case 5:
			ToggleAutoItem(ref AutoMuaKhauTrang, 764, 7149, 2, "cửa hàng", "Auto Mua Khẩu Trang");
			break;
		case 6:
			ToggleAutoItem(ref AutoMuaBuaX2Detu, 1628, 13540, 5, "cửa hàng", "Auto Mua Bùa x2 tnsm đệ");
			break;
		}
	}

	private static void AddItemToList(itemBuy newItem)
	{
		if (newItem == null)
		{
			return;
		}
		int idItem = newItem.idItem;
		if (idItem == 1628 || idItem == 1635)
		{
			pendingItem = newItem;
			ChatTextField.gI().strChat = titleInput[0];
			ChatTextField.gI().tfChat.name = "Số lượng";
			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
		}
		else
		{
			listItemBuy.RemoveAll((itemBuy x) => x.idItem == newItem.idItem);
			listItemBuy.Add(newItem);
			GameScr.info1.addInfo($"Đã thêm item {newItem.idItem} vào danh sách mua.");
		}
	}

	private static void RemoveItemFromList(itemBuy removeItem)
	{
		if (removeItem != null)
		{
			listItemBuy.RemoveAll((itemBuy x) => x.idItem == removeItem.idItem);
			GameScr.info1.addInfo($"Đã xóa item {removeItem.idItem} khỏi danh sách mua.");
		}
	}

	private void ToggleAutoTDLT()
	{
		AutoMuaTDLT = !AutoMuaTDLT;
		int idItem = 521;
		int sellPrice = 1;
		int num = Char.myCharz().checkLuong();
		if (num >= 22)
		{
			idItem = 1524;
			sellPrice = 22;
		}
		else if (num >= 9)
		{
			idItem = 1523;
			sellPrice = 9;
		}
		itemBuy p = new itemBuy(idItem, 4387, 1, 5, 39, sellPrice, "cửa hàng");
		perform(AutoMuaTDLT ? 1 : 2, p);
		GameScr.info1.addInfo("Auto Mua TDLT: " + (AutoMuaTDLT ? "ON" : "OFF"));
	}

	private void ToggleAutoItem(ref bool flag, int id, int icon, int price, string shop, string msg)
	{
		flag = !flag;
		itemBuy p = new itemBuy(id, icon, 1, 5, 39, price, shop);
		perform(flag ? 1 : 2, p);
		GameScr.info1.addInfo(msg + ": " + (flag ? "ON" : "OFF"));
	}

	public void onChatFromMe(string text, string to)
	{
		if (text == null || text.Trim().Length == 0)
		{
			ResetChatTextField();
			pendingItem = null;
		}
		else if (ChatTextField.gI().strChat == titleInput[0] && pendingItem != null)
		{
			if (int.TryParse(text.Trim(), out var result) && result > 0)
			{
				pendingItem.quantityBuy = result;
				listItemBuy.RemoveAll((itemBuy x) => x.idItem == pendingItem.idItem);
				listItemBuy.Add(pendingItem);
				GameScr.info1.addInfo($"Đã thêm item {pendingItem.idItem} với số lượng {result}.");
			}
			else
			{
				GameScr.info1.addInfo("Số lượng không hợp lệ!");
			}
			ResetChatTextField();
			pendingItem = null;
		}
		else
		{
			ResetChatTextField();
			Service.gI().chat(text);
		}
	}

	public void onCancelChat()
	{
		pendingItem = null;
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().isShow = false;
	}

	public static void update()
	{
		UpdateBuyItem();
	}

	public static void UpdateBuyItem()
	{
		if (MainXmapCL.isXmaping || GameScr.gI().mobCapcha != null || NextMap.confirming || listItemBuy.Count == 0 || GameCanvas.gameTick % 5 != 0 || HandleReturnDelay() || (retryDelayCounter > 0 && --retryDelayCounter >= 0) || HandleRetry())
		{
			return;
		}
		foreach (itemBuy item in listItemBuy)
		{
			if (!NeedToBuy(item))
			{
				continue;
			}
			if (!NavigateToShop(item) && !HandleConfirmNpc(item))
			{
				DoPurchase(item);
			}
			return;
		}
		FinishBuying();
	}

	private static bool HandleReturnDelay()
	{
		if (delayReturnCounter <= 0)
		{
			return false;
		}
		if (--delayReturnCounter == 0)
		{
			ResumeTrainGoBack("Hoàn tất mua item, kích hoạt goback để quay lại.");
		}
		return true;
	}

	private static bool HandleRetry()
	{
		if (currentBuyingItem == null)
		{
			return false;
		}
		return RetryPurchase(currentBuyingItem);
	}

	private static void ResumeTrainGoBack(string msg)
	{
		AutoTrainCL.isGoBack = true;
		if (checkTRain)
		{
			checkTRain = false;
			AutoTrainCL.isAutoTrain = true;
			AutoTrainCL.TuMoTDLT();
		}
		if (neBoss)
		{
			AutoTrainCL.autoNeBoss = true;
			neBoss = false;
		}
		if (SpamZoneItNguoi)
		{
			AutoTrainCL.autoChangeZone = true;
			SpamZoneItNguoi = false;
		}
		CheckGoBack = false;
	}

	private static void FinishBuying()
	{
		ResumeTrainGoBack("Không có item nào để mua, kích hoạt goback để quay lại.");
		delayReturnCounter = 100;
	}

	public static bool ExistItemBag(int IdItem)
	{
		return CountItemInBag(IdItem) > 0;
	}

	public static int CountItemInBag(int IdItem)
	{
		int num = 0;
		Item[] arrItemBag = Char.myCharz().arrItemBag;
		Item[] array = arrItemBag;
		foreach (Item item in array)
		{
			if (item?.template.id == IdItem)
			{
				num += item.quantity;
			}
		}
		return num;
	}

	private static bool NeedToBuy(itemBuy item)
	{
		if (ModProCL.checkItemTime(item.idIconItem))
		{
			return false;
		}
		if (Char.myCharz().checkLuong() < item.sellPrice)
		{
			return false;
		}
		return item.itemTDLT ? (timeItemDatBiet(521) <= 0) : (!ExistItemBag(item.idItem));
	}

	private static bool NavigateToShop(itemBuy item)
	{
		if (TileMap.mapID == item.mapBuyItem)
		{
			return false;
		}
		if (AutoTrainCL.isAutoTrain)
		{
			checkTRain = true;
			AutoTrainCL.isAutoTrain = false;
		}
		if (AutoTrainCL.autoNeBoss)
		{
			neBoss = true;
			AutoTrainCL.autoNeBoss = false;
		}
		if (AutoTrainCL.autoChangeZone)
		{
			SpamZoneItNguoi = true;
			AutoTrainCL.autoChangeZone = false;
		}
		if (AutoTrainCL.isGoBack)
		{
			CheckGoBack = true;
			AutoTrainCL.isGoBack = false;
		}
		MainXmapCL.StartGoToMap(item.mapBuyItem);
		return true;
	}

	private static bool HandleConfirmNpc(itemBuy item)
	{
		if (BlockPaintShop && waitConfirm == -1)
		{
			return false;
		}
		switch (waitConfirm)
		{
		case 1:
			ModProCL.teleNPC(item.idNpcBuyItem);
			NextMap.startComfirmNpc(item.idNpcBuyItem, item.nameCofirm);
			waitConfirm = (string.IsNullOrEmpty(item.nameConfirm2) ? (-1) : 2);
			break;
		case 2:
			NextMap.startComfirmNpc(item.idNpcBuyItem, item.nameConfirm2);
			waitConfirm = (string.IsNullOrEmpty(item.nameConfirm3) ? (-1) : 3);
			break;
		case 3:
                NextMap.startComfirmNpc(item.idNpcBuyItem, item.nameConfirm3);
			waitConfirm = -1;
			break;
		}
		return true;
	}

	private static void DoPurchase(itemBuy item)
	{
		int idItem = item.idItem;
		if (idItem == 1628 || idItem == 1635)
		{
			int num = CountItemInBag(item.idItem);
			if (num < item.quantityBuy)
			{
				Service.gI().buyItem(1, item.idItem, 1);
				TField tField = new TField();
				tField.setText((item.quantityBuy - num).ToString());
				Service.gI().sendClientInput(new TField[1] { tField });
				ClientInput.gI().perform(1, null);
				GameScr.info1.addInfo($"Đang mua item {item.idItem} (còn thiếu {item.quantityBuy - num}/{item.quantityBuy})...");
			}
		}
		else
		{
			Service.gI().buyItem(1, item.idItem, 1);
			GameScr.info1.addInfo($"Đang mua item {item.idItem}...");
		}
		currentBuyingItem = item;
		retryBuyCounter = 1;
		retryDelayCounter = 30;
		waitConfirm = 1;
		BlockPaintShop = false;
	}

	private static bool RetryPurchase(itemBuy item)
	{
		bool flag;
		if (!item.itemTDLT)
		{
			if (!ExistItemBag(item.idItem))
			{
				flag = ModProCL.checkItemTime(item.idIconItem);
				goto IL_0055;
			}
		}
		else if (timeItemDatBiet(521) <= 0)
		{
			flag = GameScr.canAutoPlay;
			goto IL_0055;
		}
		goto IL_0202;
		IL_0055:
		if (!flag)
		{
			if (retryBuyCounter >= 10)
			{
				GameScr.info1.addInfo($"Mua item {item.idItem} thất bại, mở lại menu.");
				waitConfirm = 1;
				ResetRetry();
				return true;
			}
			int idItem = item.idItem;
			if (idItem == 1628 || idItem == 1635)
			{
				int num = CountItemInBag(item.idItem);
				if (num < item.quantityBuy)
				{
					Service.gI().buyItem(1, item.idItem, 1);
					TField tField = new TField();
					tField.setText((item.quantityBuy - num).ToString());
					Service.gI().sendClientInput(new TField[1] { tField });
					ClientInput.gI().perform(1, null);
					GameScr.info1.addInfo($"Thử lại mua item {item.idItem} (còn thiếu {item.quantityBuy - num}, lần {retryBuyCounter}/{10}).");
				}
			}
			else
			{
				Service.gI().buyItem(1, item.idItem, 1);
				GameScr.info1.addInfo($"Thử lại mua item {item.idItem} (lần {retryBuyCounter}/{10}).");
			}
			retryBuyCounter++;
			retryDelayCounter = 30;
			return true;
		}
		goto IL_0202;
		IL_0202:
		GameScr.info1.addInfo($"Mua thành công item {item.idItem}.");
		ResetCurrentItem();
		delayReturnCounter = 100;
		return true;
	}

	private static void ResetCurrentItem()
	{
		currentBuyingItem = null;
		ResetRetry();
	}

	private static void ResetRetry()
	{
		retryBuyCounter = 0;
		retryDelayCounter = 0;
	}

	public static int timeItemDatBiet(int idItem)
	{
		try
		{
			Item[] arrItemBag = Char.myCharz().arrItemBag;
			Item[] array = arrItemBag;
			Item[] array2 = array;
			foreach (Item item in array2)
			{
				if (item?.template.id == idItem)
				{
					return item.itemOption[0].param;
				}
			}
		}
		catch (Exception ex)
		{
			GameScr.info1.addInfo($"Lỗi kiểm tra item {idItem}: {ex.Message}");
		}
		return 0;
	}

    public static void showMenu()
    {
        MyVector myVector = new MyVector();
        myVector.addElement(new Command("Auto Mua TDLT: " + (AutoMuaTDLT ? "ON" : "OFF"), getInstance(), 3, null));
        myVector.addElement(new Command("Auto Mua Cỏ: " + (AutoMuaCo ? "ON" : "OFF"), getInstance(), 4, null));
        myVector.addElement(new Command("Auto Mua Khẩu Trang: " + (AutoMuaKhauTrang ? "ON" : "OFF"), getInstance(), 5, null));
        myVector.addElement(new Command("Auto Mua Bùa x2 tnsm đệ: " + (AutoMuaBuaX2Detu ? "ON" : "OFF"), getInstance(), 6, null));

        // Hiển thị menu trước
        GameCanvas.menu.startAt(myVector, 3);
        GameCanvas.menu.setMenuHeaderText("Vui lòng bật GoBack để sử dụng chức năng này");

    }

    public static void Paint(mGraphics g, ref int y, int spaceY, int x = 10)
	{
		if (AutoMuaTDLT)
		{
			GraphicsManagement.DrawFont.drawString(g, "Tự động mua TDLT khi hết", 10, y, 0);
			y += spaceY;
		}
		if (AutoMuaCo)
		{
			GraphicsManagement.DrawFont.drawString(g, "Tự động mua Cỏ khi hết", 10, y, 0);
			y += spaceY;
		}
		if (AutoMuaKhauTrang)
		{
			GraphicsManagement.DrawFont.drawString(g, "Tự động mua khẩu trang khi hết", 10, y, 0);
			y += spaceY;
		}
		if (AutoMuaBuaX2Detu)
		{
			GraphicsManagement.DrawFont.drawString(g, "Tự động mua Bùa đệ x2 khi hết", 10, y, 0);
			y += spaceY;
		}
	}
}
