using System.Collections.Generic;
using Mod.CuongLe;

namespace Mod.community;

public class AutoPick : IActionListener, IChatable
{
	private static AutoPick _Instance;

	public static bool isAutoPick;

	public static long lastTimePickedItem;

	private static int maximumPickDistance = 50;

	private static bool isTeleportToItem;

	private static bool isPickAll;

	public static int pickByList;

	private static List<int> listItemAutoPick = new List<int>();

	private static readonly string[] inputMaximumPickDistance = new string[2] { "Nhập khoảng cách nhặt", "khoảng cách (>50)" };

	private static readonly string[] inputItemID = new string[2] { "Nhập ID của item", "ID" };

	public static AutoPick getInstance()
	{
		return _Instance ?? (_Instance = new AutoPick());
	}

	public static void Update()
	{
		if (!isAutoPick || (GameScr.isAutoPlay && !GameScr.canAutoPlay && !AutoTrainCL.isAutoTrain))
		{
			return;
		}
		long num = mSystem.currentTimeMillis();
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
			if (itemMap == null || itemMap.template == null)
			{
				continue;
			}
			bool flag = itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1;
			int num2 = Math.Abs(Char.myCharz().cx - itemMap.x);
			int num3 = Math.Abs(Char.myCharz().cy - itemMap.y);
			if (isNRDMap(TileMap.mapID))
			{
				if (flag && num2 <= 60 && num - lastTimePickedItem > 550 && isNRD(itemMap))
				{
					Service.gI().pickItem(itemMap.itemMapID);
					lastTimePickedItem = num;
					break;
				}
			}
			else if (isPickIt(itemMap) && num2 <= maximumPickDistance && num3 <= maximumPickDistance && num - lastTimePickedItem > 550)
			{
				if (isTeleportToItem && !Char.isLockKey)
				{
					TeleportTo(itemMap.x, itemMap.y);
				}
				Service.gI().pickItem(itemMap.itemMapID);
				lastTimePickedItem = num;
				break;
			}
		}
	}

	public void onChatFromMe(string text, string to)
	{
		string text2 = ChatTextField.gI().tfChat.getText();
		if (string.IsNullOrEmpty(text2))
		{
			ChatTextField.gI().isShow = false;
		}
		else if (ChatTextField.gI().strChat.Equals(inputMaximumPickDistance[0]))
		{
			if (int.TryParse(text2, out var result))
			{
				maximumPickDistance = result;
				GameScr.info1.addInfo("Khoảng Cách Nhặt: " + result);
			}
			else
			{
				GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputItemID[0]))
		{
			if (int.TryParse(text2, out var result2))
			{
				listItemAutoPick.Add(result2);
				GameScr.info1.addInfo("Đã Thêm Item " + result2);
			}
			else
			{
				GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!");
			}
			ResetChatTextField();
		}
		else
		{
			ResetChatTextField();
			Service.gI().chat(text);
		}
	}

	public void onCancelChat()
	{
		if (GameScr.isPaintMessage)
		{
			GameScr.isPaintMessage = false;
			ChatTextField.gI().center = null;
		}
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			isAutoPick = !isAutoPick;
			pickByList = 0;
			GameScr.info1.addInfo("Auto Nhặt\n" + (isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"));
			break;
		case 2:
			isPickAll = !isPickAll;
			GameScr.info1.addInfo("Nhặt Tất Cả\n" + (isPickAll ? "[STATUS: ON]" : "[STATUS: OFF]"));
			break;
		case 3:
			isAutoPick = !isAutoPick;
			pickByList = 1;
			GameScr.info1.addInfo("Nhặt Theo Danh Sách\n" + (isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"));
			break;
		case 4:
			isTeleportToItem = !isTeleportToItem;
			GameScr.info1.addInfo("Dịch Đến Item\n" + (isTeleportToItem ? "[STATUS: ON]" : "[STATUS: OFF]"));
			break;
		case 5:
			ChatTextField.gI().strChat = inputMaximumPickDistance[0];
			ChatTextField.gI().tfChat.name = inputMaximumPickDistance[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 6:
			if (listItemAutoPick.Count == 0)
			{
				GameScr.info1.addInfo("Danh Sách Trống!");
				break;
			}
			GameScr.info1.addInfo(string.Join(" ", listItemAutoPick.ConvertAll((int i) => i.ToString()).ToArray()));
			break;
		case 7:
			listItemAutoPick.Clear();
			GameScr.info1.addInfo("Đã Clear Danh Sách Nhặt!");
			break;
		case 8:
			ChatTextField.gI().strChat = inputItemID[0];
			ChatTextField.gI().tfChat.name = inputItemID[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 9:
			if (Char.myCharz().itemFocus != null)
			{
				listItemAutoPick.Add(Char.myCharz().itemFocus.template.id);
				GameScr.info1.addInfo("Đã thêm " + Char.myCharz().itemFocus.template.name + " [" + Char.myCharz().itemFocus.template.id + "]");
			}
			break;
		}
	}

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Auto Nhặt\n" + ((!isAutoPick || pickByList != 0) ? "[STATUS: OFF]" : "[STATUS: ON]"), getInstance(), 1, null));
		myVector.addElement(new Command("Nhặt Tất Cả\n" + (isPickAll ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 2, null));
		myVector.addElement(new Command("Nhặt Theo Danh Sách\n" + ((!isAutoPick || pickByList != 1) ? "[STATUS: OFF]" : "[STATUS: ON]"), getInstance(), 3, null));
		myVector.addElement(new Command("Dịch Đến Item\n" + (isTeleportToItem ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 4, null));
		myVector.addElement(new Command("Khoảng Cách Nhặt\n[" + maximumPickDistance + "]", getInstance(), 5, null));
		myVector.addElement(new Command("Xem Danh Sách Nhặt", getInstance(), 6, null));
		myVector.addElement(new Command("Clear Danh Sách Nhặt", getInstance(), 7, null));
		myVector.addElement(new Command("Thêm ItemID", getInstance(), 8, null));
		if (Char.myCharz().itemFocus != null)
		{
			myVector.addElement(new Command("Thêm: " + Char.myCharz().itemFocus.template.name + " [" + Char.myCharz().itemFocus.template.id + "]", getInstance(), 9, null));
		}
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	public static void FocusToNearestItem()
	{
		if (Char.myCharz().itemFocus != null)
		{
			return;
		}
		ItemMap itemMap = null;
		int num = int.MaxValue;
		for (int i = 0; i < GameScr.vItemMap.size(); i++)
		{
			ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(i);
			if (itemMap2 == null || !isPickIt(itemMap2))
			{
				continue;
			}
			int num2 = Math.Abs(Char.myCharz().cx - itemMap2.x);
			int num3 = Math.Abs(Char.myCharz().cy - itemMap2.y);
			if (num2 <= maximumPickDistance && num3 <= maximumPickDistance)
			{
				int num4 = num2 * num2 + num3 * num3;
				if (num4 < num)
				{
					num = num4;
					itemMap = itemMap2;
				}
			}
		}
		if (itemMap != null)
		{
			Char.myCharz().itemFocus = itemMap;
		}
	}

	public static void PickIt()
	{
		if (Char.myCharz().itemFocus == null)
		{
			return;
		}
		long num = mSystem.currentTimeMillis();
		if (num - lastTimePickedItem >= 550)
		{
			ItemMap itemFocus = Char.myCharz().itemFocus;
			if (isTeleportToItem && !Char.isLockKey)
			{
				TeleportTo(itemFocus.x, itemFocus.y);
			}
			int num2 = Math.Abs(Char.myCharz().cx - itemFocus.x);
			int num3 = Math.Abs(Char.myCharz().cy - itemFocus.y);
			if (num2 <= 40 && num3 <= 40)
			{
				Service.gI().pickItem(itemFocus.itemMapID);
				lastTimePickedItem = num;
				Char.myCharz().itemFocus = null;
			}
			else
			{
				Char.myCharz().currentMovePoint = new MovePoint(itemFocus.x, itemFocus.y);
				Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
			}
		}
	}

	private static void TeleportTo(int x, int y)
	{
		Char.myCharz().cx = x;
		Char.myCharz().cy = y;
		Service.gI().charMove();
		Char.myCharz().cy = y + 1;
		Service.gI().charMove();
		Char.myCharz().cy = y;
		Service.gI().charMove();
	}

	private static bool isPickIt(ItemMap item)
	{
		if (item == null || item.template == null)
		{
			return false;
		}
		if (isPickAll)
		{
			return true;
		}
		bool flag = item.playerId == Char.myCharz().charID || item.playerId == -1;
		if (pickByList == 0)
		{
			return flag;
		}
		return pickByList == 1 && listItemAutoPick.Contains(item.template.id) && flag;
	}

	private static bool isNRDMap(int mapID)
	{
		return mapID >= 85 && mapID <= 91;
	}

	private static bool isNRD(ItemMap item)
	{
		return item.template.id >= 372 && item.template.id <= 378;
	}
}
