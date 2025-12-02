using System.Collections.Generic;
using System.Threading;

namespace Mod.community;

public class AutoItem : IActionListener, IChatable
{
	public class Item
	{
		public int Id;

		public string Name;

		public int Quantity;

		public short Index;

		public bool IsGold;

		public bool IsSell;

		public int Delay;

		public long LastTimeUse;

		public Item()
		{
		}

		public Item(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public Item(int id, short isGold, bool index, bool isSell)
		{
			Id = id;
			IsGold = index;
			Index = isGold;
			IsSell = isSell;
		}
	}

	private static AutoItem _Instance;

	private static List<Item> listItemAuto = new List<Item>();

	private static Item itemToAuto;

	public static List<string> set1 = new List<string>();

	public static List<string> set2 = new List<string>();

	private static bool isChangingSet;

	private static string[] inputDelay = new string[2] { "Nhập delay", "giây" };

	private static string[] inputSellQuantity = new string[2] { "Nhập số lượng bán", "số lượng" };

	private static string[] inputBuyQuantity = new string[2] { "Nhập số lượng mua", "số lượng" };

	public static AutoItem getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new AutoItem();
		}
		return _Instance;
	}

	public static void Update()
	{
		if (listItemAuto.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < listItemAuto.Count; i++)
		{
			Item item = listItemAuto[i];
			if (mSystem.currentTimeMillis() - item.LastTimeUse > item.Delay * 1000)
			{
				item.LastTimeUse = mSystem.currentTimeMillis();
				Service.gI().useItem(0, 1, -1, (short)item.Id);
				break;
			}
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
		{
			ChatTextField.gI().isShow = false;
		}
		else if (ChatTextField.gI().strChat.Equals(inputDelay[0]))
		{
			try
			{
				int delay = int.Parse(ChatTextField.gI().tfChat.getText());
				itemToAuto.Delay = delay;
				GameScr.info1.addInfo("Auto " + itemToAuto.Name + ": " + delay + " giây");
				listItemAuto.Add(itemToAuto);
			}
			catch
			{
				GameScr.info1.addInfo("Delay Không Hợp Lệ, Vui Lòng Nhập Lại!");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputBuyQuantity[0]))
		{
			try
			{
				int quantity = int.Parse(ChatTextField.gI().tfChat.getText());
				itemToAuto.Quantity = quantity;
				new Thread((ThreadStart)delegate
				{
					AutoBuy(itemToAuto);
				}).Start();
			}
			catch
			{
				GameScr.info1.addInfo("Số Lượng Không Hợp Lệ, Vui Lòng Nhập Lại!");
			}
			ResetChatTextField();
		}
		else
		{
			if (!ChatTextField.gI().strChat.Equals(inputSellQuantity[0]))
			{
				return;
			}
			try
			{
				int quantity2 = int.Parse(ChatTextField.gI().tfChat.getText());
				itemToAuto.Quantity = quantity2;
				new Thread((ThreadStart)delegate
				{
					AutoSell(itemToAuto);
				}).Start();
			}
			catch
			{
				GameScr.info1.addInfo("Số Lượng Không Hợp Lệ, Vui Lòng Nhập Lại!");
			}
			ResetChatTextField();
		}
	}

	public void onCancelChat()
	{
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			OpenTFAutoUseItem((Item)p);
			break;
		case 2:
			RemoveItemAuto((int)p);
			break;
		case 3:
			OpenTFAutoTradeItem((Item)p);
			break;
		case 4:
			set1.Add(((global::Item)p).getFullName());
			break;
		case 5:
			set2.Add(((global::Item)p).getFullName());
			break;
		case 6:
			set1.Remove(((global::Item)p).getFullName());
			break;
		case 7:
			set2.Remove(((global::Item)p).getFullName());
			break;
		}
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	public static bool isAutoUse(int templateId)
	{
		for (int i = 0; i < listItemAuto.Count; i++)
		{
			if (listItemAuto[i].Id == templateId)
			{
				return true;
			}
		}
		return false;
	}

	private static void RemoveItemAuto(int templateId)
	{
		for (int i = 0; i < listItemAuto.Count; i++)
		{
			if (listItemAuto[i].Id == templateId)
			{
				listItemAuto.RemoveAt(i);
				break;
			}
		}
	}

	private static void OpenTFAutoUseItem(Item item)
	{
		itemToAuto = item;
		ChatTextField.gI().strChat = inputDelay[0];
		ChatTextField.gI().tfChat.name = inputDelay[1];
		GameCanvas.panel.isShow = false;
		ChatTextField.gI().startChat2(getInstance(), string.Empty);
	}

	private static void OpenTFAutoTradeItem(Item item)
	{
		itemToAuto = item;
		GameCanvas.panel.isShow = false;
		if (item.IsSell)
		{
			ChatTextField.gI().strChat = inputSellQuantity[0];
			ChatTextField.gI().tfChat.name = inputSellQuantity[1];
		}
		else
		{
			ChatTextField.gI().strChat = inputBuyQuantity[0];
			ChatTextField.gI().tfChat.name = inputBuyQuantity[1];
		}
		ChatTextField.gI().startChat2(getInstance(), string.Empty);
	}

	private static void AutoSell(Item item)
	{
		Thread.Sleep(100);
		short index = item.Index;
		while (item.Quantity > 0)
		{
			if (Char.myCharz().arrItemBag[index] == null || (Char.myCharz().arrItemBag[index] != null && Char.myCharz().arrItemBag[index].template.id != (short)item.Id))
			{
				GameScr.info1.addInfo("Không Tìm Thấy Item!");
				return;
			}
			Service.gI().saleItem(0, 1, (short)(index + 3));
			Thread.Sleep(100);
			Service.gI().saleItem(1, 1, index);
			Thread.Sleep(1000);
			item.Quantity--;
			if (Char.myCharz().xu > 1963100000)
			{
				GameScr.info1.addInfo("Xong!");
				return;
			}
		}
		GameScr.info1.addInfo("Xong!");
	}

	private void AutoBuy(Item item)
	{
		while (item.Quantity > 0 && !GameScr.gI().isBagFull())
		{
			Service.gI().buyItem((!item.IsGold) ? ((sbyte)1) : ((sbyte)0), item.Id, 0);
			item.Quantity--;
			Thread.Sleep(1000);
		}
		GameScr.info1.addInfo("Xong!");
	}

	public static void useSet(int type)
	{
		if (isChangingSet)
		{
			GameScr.info1.addInfo("Đang Mặc Đồ!");
			return;
		}
		new Thread((ThreadStart)delegate
		{
			if (type == 0)
			{
				ChangeSet(set1);
			}
			if (type == 1)
			{
				ChangeSet(set2);
			}
		}).Start();
	}

	private static void ChangeSet(List<string> set)
	{
		if (isChangingSet)
		{
			GameScr.info1.addInfo("Đang Mặc Đồ!");
			return;
		}
		isChangingSet = true;
		for (int num = Char.myCharz().arrItemBag.Length - 1; num >= 0; num--)
		{
			global::Item item = Char.myCharz().arrItemBag[num];
			if (item != null && set.Contains(item.getFullName()))
			{
				Service.gI().getItem(4, (sbyte)num);
				Thread.Sleep(100);
			}
		}
		isChangingSet = false;
	}
}
