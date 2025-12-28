using System.Collections.Generic;

namespace Mod.CuongLe;

public class AutoVutDoCL : IActionListener, IChatable
{
	private static AutoVutDoCL _Instance;

	public static List<int> listVutDo;

	private static string[] titleInput;

	public static bool autoVut;

	private static int vutIndex;

	private static long vutTimer;

	public static AutoVutDoCL getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new AutoVutDoCL();
		}
		return _Instance;
	}

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Auto vứt: " + (autoVut ? "Bật" : "Tắt"), getInstance(), 5, null));
		myVector.addElement(new Command("Thêm id vật phẩm", getInstance(), 1, null));
		myVector.addElement(new Command("Xóa tất cả danh sách", getInstance(), 2, null));
		myVector.addElement(new Command("Xóa id vật phẩm", getInstance(), 3, null));
		myVector.addElement(new Command("Xem danh sách vật phẩm", getInstance(), 4, null));
		myVector.addElement(new Command("Lưu danh sách", getInstance(), 6, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			ChatTextField.gI().strChat = titleInput[0];
			ChatTextField.gI().tfChat.name = "idItem hoặc _ giữa các idItem";
			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 2:
			listVutDo.Clear();
			GameScr.info1.addInfo("Đã xóa toàn bộ danh sách vật phẩm");
			break;
		case 3:
			ChatTextField.gI().strChat = titleInput[1];
			ChatTextField.gI().tfChat.name = "idItem hoặc _ giữa các idItem";
			ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 4:
		{
			if (listVutDo.Count == 0)
			{
				ChatPopup.addChatPopupMultiLineGameline("Danh sách vật phẩm trống!");
				break;
			}
			string text = "";
			for (int j = 0; j < listVutDo.Count; j++)
			{
				string nameItem = GetNameItem((short)listVutDo[j]);
				text = text + listVutDo[j] + ": " + (string.IsNullOrEmpty(nameItem) ? "Unknown" : nameItem);
				if (j < listVutDo.Count - 1)
				{
					text += "\n";
				}
			}
			ChatPopup.addChatPopupMultiLineGameline(text);
			break;
		}
		case 5:
			autoVut = !autoVut;
			GameScr.info1.addInfo("Auto vứt item: " + (autoVut ? "Bật" : "Tắt"));
			break;
		case 6:
		{
			string[] array = new string[listVutDo.Count];
			for (int i = 0; i < listVutDo.Count; i++)
			{
				array[i] = listVutDo[i].ToString();
			}
			string data = string.Join(",", array);
			Rms.saveRMSString("listVutDo", data);
			GameScr.info1.addInfo("Đã lưu danh sách vật phẩm");
			break;
		}
		case 7:
			AddListVutDo((int)p);
			break;
		case 8:
			DeleteListVutDo((int)p);
			break;
		}
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().isShow = false;
	}

	static AutoVutDoCL()
	{
		vutIndex = 0;
		vutTimer = 0L;
		listVutDo = new List<int>();
		titleInput = new string[2] { "Nhập id Item cần thêm", "Nhập id Item cần xóa" };
	}

	public void onChatFromMe(string text, string to)
	{
		if (text == null || text.Trim().Length == 0 || ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Trim().Length == 0)
		{
			ChatTextField.gI().isShow = false;
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(titleInput[0]))
		{
			HandleAddItems(text);
		}
		else if (ChatTextField.gI().strChat.Equals(titleInput[1]))
		{
			HandleDeleteItems(text);
		}
		else
		{
			ResetChatTextField();
			Service.gI().chat(text);
		}
	}

	private void HandleAddItems(string text)
	{
		try
		{
			string[] array = text.Split('_');
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string[] array2 = array;
			string[] array3 = array2;
			string[] array4 = array3;
			foreach (string text2 in array4)
			{
				string text3 = text2.Trim();
				if (text3.Length == 0)
				{
					continue;
				}
				if (int.TryParse(text3, out var result) && result >= 0)
				{
					string nameItem = GetNameItem((short)result);
					string text4 = result + " (" + ((nameItem.Length == 0) ? "Unknown" : nameItem) + ")";
					if (!listVutDo.Contains(result))
					{
						listVutDo.Add(result);
						list.Add(text4);
					}
					else
					{
						list2.Add(text4 + ": đã tồn tại");
					}
				}
				else
				{
					list2.Add(text3 + ": không hợp lệ");
				}
			}
			string text5 = "";
			if (list.Count > 0)
			{
				text5 = text5 + "Đã thêm: " + string.Join(", ", list.ToArray());
			}
			if (list2.Count > 0)
			{
				text5 = text5 + ((text5.Length > 0) ? ". " : "") + "Thất bại: " + string.Join(", ", list2.ToArray());
			}
			if (text5.Length == 0)
			{
				text5 = "Không thêm được id Item nào";
			}
			GameScr.info1.addInfo(text5);
		}
		catch
		{
			GameScr.info1.addInfo("Vui lòng nhập đúng định dạng: số hoặc _ giữa các số");
		}
		ResetChatTextField();
	}

	private void HandleDeleteItems(string text)
	{
		try
		{
			string[] array = text.Split('_');
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string[] array2 = array;
			string[] array3 = array2;
			string[] array4 = array3;
			foreach (string text2 in array4)
			{
				string text3 = text2.Trim();
				if (text3.Length == 0)
				{
					continue;
				}
				if (int.TryParse(text3, out var result) && result >= 0)
				{
					string nameItem = GetNameItem((short)result);
					string text4 = result + " (" + ((nameItem.Length == 0) ? "Unknown" : nameItem) + ")";
					if (listVutDo.Contains(result))
					{
						listVutDo.Remove(result);
						list.Add(text4);
					}
					else
					{
						list2.Add(text4 + ": không tồn tại");
					}
				}
				else
				{
					list2.Add(text3 + ": không hợp lệ");
				}
			}
			string text5 = "";
			if (list.Count > 0)
			{
				text5 = text5 + "Đã xóa: " + string.Join(", ", list.ToArray());
			}
			if (list2.Count > 0)
			{
				text5 = text5 + ((text5.Length > 0) ? ". " : "") + "Thất bại: " + string.Join(", ", list2.ToArray());
			}
			if (text5.Length == 0)
			{
				text5 = "Không xóa được id Item nào";
			}
			GameScr.info1.addInfo(text5);
		}
		catch
		{
			GameScr.info1.addInfo("Vui lòng nhập đúng định dạng: số hoặc _ giữa các số");
		}
		ResetChatTextField();
	}

	public void onCancelChat()
	{
	}

	public static void update()
	{
		UpdateAutoVut();
	}

	public static void UpdateAutoVut()
	{
		if (!autoVut || Char.myCharz().isWaitMonkey || mSystem.currentTimeMillis() < vutTimer || listVutDo.Count <= 0)
		{
			return;
		}
		int num = listVutDo[vutIndex];
		for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = Char.myCharz().arrItemBag[i];
			if (item != null && item.template.id == num)
			{
				Service.gI().useItem(2, 1, (sbyte)i, -1);
				vutTimer = mSystem.currentTimeMillis() + 500;
				return;
			}
		}
		vutIndex++;
		if (vutIndex >= listVutDo.Count)
		{
			vutIndex = 0;
			vutTimer = mSystem.currentTimeMillis() + 1500;
		}
	}

	public static string GetNameItem(short idItem)
	{
		return ItemTemplates.get(idItem).name;
	}

	public static void loadData()
	{
		string text = Rms.loadRMSString("listVutDo");
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		try
		{
			listVutDo.Clear();
			string[] array = text.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (int.TryParse(array[i].Trim(), out var result) && result >= 0)
				{
					listVutDo.Add(result);
				}
			}
			GameScr.info1.addInfo((listVutDo.Count > 0) ? "Tải danh sách vật phẩm thành công" : "Danh sách vật phẩm trống");
		}
		catch
		{
			GameScr.info1.addInfo("Lỗi tải danh sách vật phẩm");
		}
	}

	private static void AddListVutDo(int idItem)
	{
		listVutDo.Add(idItem);
		GameScr.info1.addInfo("Đã thêm " + GetNameItem((short)idItem) + " vào danh sách vứt. Vui lòng mở auto");
	}

	private static void DeleteListVutDo(int idItem)
	{
		listVutDo.Remove(idItem);
		GameScr.info1.addInfo("Đã xóa " + GetNameItem((short)idItem) + " trong danh sách vứt");
	}
}
