using System.Collections.Generic;
using System.IO;

namespace Mod.community;

public class AutoChat : IActionListener, IChatable
{
	private static AutoChat _Instance;

	private static bool isAutoChatPublic;

	private static long delayAutoChatPublic;

	private static long lastTimeChatPublic;

	private static string autoChatPublicContent;

	private static bool isAutoChatGlobal;

    private static bool isAutoSpamChatGlobal;

    private static long delayAutoChatGlobal;

	private static long lastTimeChatGlobal;

	private static string autoChatGlobalContent;

	private static int countChatGlobal;

	private static int gems;

	private static bool zon;

	private static int spammedChatGlobalTimes;

	private static int timesSpamChatGlobal;

	public static bool isAutoInbox;

	public static List<int> listCharInbox;

	private static string autoInboxContent;

	private static long lastTimeInbox;

	private static int countChatInbox;

	private static bool isSaveData;

	private static string[] inputDelayChatPublic;

	private static string[] inputDelayChatGlobal;

	private static string[] inputSpamChatGlobal;

	public static AutoChat getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new AutoChat();
		}
		return _Instance;
	}

	public static void Update()
	{
		if (isAutoChatPublic)
		{
			ChatPublic();
		}
		if (isAutoChatGlobal)
		{
			ChatGlobal();
		}
		if (isAutoSpamChatGlobal)
		{
			SpamChatGlobal();
		}
		if (isAutoInbox)
		{
			ChatInbox();
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
		{
			ChatTextField.gI().isShow = false;
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputDelayChatPublic[0]))
		{
			int num = 0;
			try
			{
				num = int.Parse(ChatTextField.gI().tfChat.getText());
				if (num > 0)
				{
					try
					{
						autoChatPublicContent = File.ReadAllText("Data\\TextChatPublic.ini");
						delayAutoChatPublic = num;
						isAutoChatPublic = true;
						GameScr.info1.addInfo("Auto Chat: " + ((delayAutoChatPublic >= 5000) ? NinjaUtil.getMoneys(delayAutoChatPublic) : "5.000") + " mili giây");
						if (isSaveData)
						{
							Rms.saveRMSInt("AutoChatIsAutoChatPublic", 1);
							Rms.saveRMSString("AutoChatDelayChatPublic", delayAutoChatPublic.ToString());
						}
					}
					catch
					{
						GameScr.info1.addInfo("Lỗi đọc File!");
					}
				}
				if (num <= 0)
				{
					GameCanvas.startOKDlg("Delay không hợp lệ!");
				}
			}
			catch
			{
				GameCanvas.startOKDlg("Delay không hợp lệ!");
			}
			ResetChatTextField();
		}
		else if (ChatTextField.gI().strChat.Equals(inputDelayChatGlobal[0]))
		{
			int num2 = 0;
			try
			{
				num2 = int.Parse(ChatTextField.gI().tfChat.getText());
				if (num2 > 0)
				{
					try
					{
						autoChatGlobalContent = File.ReadAllText("Data\\TextChatGlobal.ini");
						delayAutoChatGlobal = num2;
						isAutoChatGlobal = true;
						isAutoSpamChatGlobal = false;
						GameScr.info1.addInfo("Auto Chat Thế Giới: " + NinjaUtil.getMoneys(delayAutoChatGlobal) + " mili giây");
						if (isSaveData)
						{
							Rms.saveRMSInt("AutoChatIsAutoChatGlobal", 1);
							Rms.saveRMSString("AutoChatDelayChatGlobal", delayAutoChatGlobal.ToString());
						}
					}
					catch
					{
						GameScr.info1.addInfo("Lỗi đọc File!");
					}
				}
				if (num2 <= 0)
				{
					GameCanvas.startOKDlg("Delay không hợp lệ!");
				}
			}
			catch
			{
				GameCanvas.startOKDlg("Delay không hợp lệ!");
			}
			ResetChatTextField();
		}
		else
		{
			if (!ChatTextField.gI().strChat.Equals(inputSpamChatGlobal[0]))
			{
				return;
			}
			int num3 = 0;
			try
			{
				num3 = int.Parse(ChatTextField.gI().tfChat.getText());
				if (num3 > 0)
				{
					try
					{
						autoChatGlobalContent = File.ReadAllText("Data\\TextChatGlobal.ini");
						timesSpamChatGlobal = num3;
						spammedChatGlobalTimes = 0;
						gems = Char.myCharz().luong + Char.myCharz().luongKhoa;
						isAutoChatGlobal = false;
						isAutoSpamChatGlobal = true;
						GameScr.info1.addInfo("Spam Chat Thế Giới: " + timesSpamChatGlobal + " lần");
					}
					catch
					{
						GameScr.info1.addInfo("Lỗi đọc File!");
					}
				}
				if (num3 <= 0)
				{
					GameCanvas.startOKDlg("Số lần không hợp lệ!");
				}
			}
			catch
			{
				GameCanvas.startOKDlg("Số lần không hợp lệ!");
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
			if (!isAutoChatPublic)
			{
				ChatTextField.gI().strChat = inputDelayChatPublic[0];
				ChatTextField.gI().tfChat.name = inputDelayChatPublic[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
			}
			if (isAutoChatPublic)
			{
				isAutoChatPublic = false;
				GameScr.info1.addInfo("Auto Chat\n[STATUS: OFF]");
				if (isSaveData)
				{
					Rms.saveRMSInt("AutoChatIsAutoChatPublic", 0);
				}
			}
			break;
		case 2:
			if (!isAutoChatGlobal)
			{
				ChatTextField.gI().strChat = inputDelayChatGlobal[0];
				ChatTextField.gI().tfChat.name = inputDelayChatGlobal[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
			}
			if (isAutoChatGlobal)
			{
				isAutoChatGlobal = false;
				GameScr.info1.addInfo("Auto Chat Thế Giới\n[STATUS: OFF]");
				if (isSaveData)
				{
					Rms.saveRMSInt("AutoChatIsAutoChatGlobal", 0);
				}
			}
			break;
		case 3:
			if (!isAutoSpamChatGlobal)
			{
				ChatTextField.gI().strChat = inputSpamChatGlobal[0];
				ChatTextField.gI().tfChat.name = inputSpamChatGlobal[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
			}
			if (isAutoSpamChatGlobal)
			{
				isAutoSpamChatGlobal = false;
				GameScr.info1.addInfo("Auto Spam Chat Thế Giới\n[STATUS: OFF]");
			}
			break;
		case 4:
			if (isAutoInbox)
			{
				isAutoInbox = false;
				GameScr.info1.addInfo("Auto Inbox\n[STATUS: OFF]");
			}
			else
			{
				try
				{
					autoInboxContent = File.ReadAllText("Data\\TextChatInbox.ini");
					isAutoInbox = true;
					GameScr.info1.addInfo("Auto Inbox\n[STATUS: ON]");
				}
				catch
				{
					GameScr.info1.addInfo("Lỗi đọc File!");
				}
			}
			if (isSaveData)
			{
				Rms.saveRMSInt("AutoChatIsAutoChatInbox", isAutoInbox ? 1 : 0);
			}
			break;
		case 5:
			isSaveData = !isSaveData;
			GameScr.info1.addInfo("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"));
			Rms.saveRMSInt("AutoChatIsSaveRms", isSaveData ? 1 : 0);
			if (isSaveData)
			{
				SaveData();
			}
			break;
		}
	}

	public static void ShowMenu()
	{
		LoadSavedData();
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Auto Chat\n" + (isAutoChatPublic ? ("[" + NinjaUtil.getMoneys(delayAutoChatPublic) + " mili giây]") : "[STATUS: OFF]"), getInstance(), 1, null));
		myVector.addElement(new Command("Auto Chat Thế Giới\n" + (isAutoChatGlobal ? ("[" + NinjaUtil.getMoneys(delayAutoChatGlobal) + " mili giây]") : "[STATUS: OFF]"), getInstance(), 2, null));
		myVector.addElement(new Command("Auto Spam Chat Thế Giới\n" + (isAutoSpamChatGlobal ? ("[" + spammedChatGlobalTimes + "/" + timesSpamChatGlobal + "]") : "[STATUS: OFF]"), getInstance(), 3, null));
		myVector.addElement(new Command("Auto Inbox\n" + (isAutoInbox ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 4, null));
		myVector.addElement(new Command("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 5, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	private static void LoadSavedData()
	{
		isSaveData = Rms.loadRMSInt("AutoChatIsSaveRms") == 1;
		if (isSaveData)
		{
			isAutoChatPublic = Rms.loadRMSInt("AutoChatIsAutoChatPublic") == 1;
			isAutoChatGlobal = Rms.loadRMSInt("AutoChatIsAutoChatGlobal") == 1;
			isAutoInbox = Rms.loadRMSInt("AutoChatIsAutoChatInbox") == 1;
			if (isAutoChatPublic)
			{
				delayAutoChatPublic = long.Parse(Rms.loadRMSString("AutoChatDelayChatPublic"));
			}
			if (isAutoChatGlobal)
			{
				delayAutoChatGlobal = long.Parse(Rms.loadRMSString("AutoChatDelayChatGlobal"));
			}
		}
	}

	private static void SaveData()
	{
		Rms.saveRMSInt("AutoChatIsAutoChatPublic", isAutoChatPublic ? 1 : 0);
		Rms.saveRMSInt("AutoChatIsAutoChatGlobal", isAutoChatGlobal ? 1 : 0);
		Rms.saveRMSInt("AutoChatIsAutoChatInbox", isAutoInbox ? 1 : 0);
		if (isAutoChatPublic)
		{
			Rms.saveRMSString("AutoChatDelayChatPublic", delayAutoChatPublic.ToString());
		}
		if (isAutoChatGlobal)
		{
			Rms.saveRMSString("AutoChatDelayChatGlobal", delayAutoChatGlobal.ToString());
		}
	}

	private static void ChatPublic()
	{
		if (delayAutoChatPublic < 5000)
		{
			delayAutoChatPublic = 5000L;
		}
		if (autoChatPublicContent == null || autoChatPublicContent.Equals(""))
		{
			try
			{
				autoChatPublicContent = File.ReadAllText("Data\\TextChatPublic.ini");
			}
			catch
			{
				autoChatPublicContent = "Cường đẹp trai!";
			}
		}
		if (mSystem.currentTimeMillis() - lastTimeChatPublic > delayAutoChatPublic)
		{
			lastTimeChatPublic = mSystem.currentTimeMillis();
			Service.gI().chat(autoChatPublicContent);
		}
	}

	private static void ChatGlobal()
	{
		int num = Char.myCharz().luong + Char.myCharz().luongKhoa;
		if (num < 5)
		{
			isAutoChatGlobal = false;
			GameScr.info1.addInfo("Bạn không đủ ngọc để chat!");
			return;
		}
		if (delayAutoChatGlobal <= 0)
		{
			delayAutoChatGlobal = 5000L;
		}
		if (autoChatGlobalContent == null || autoChatGlobalContent.Equals(""))
		{
			try
			{
				autoChatGlobalContent = File.ReadAllText("Data\\TextChatGlobal.ini");
			}
			catch
			{
				autoChatGlobalContent = "Cường đẹp trai!";
			}
		}
		if (gems == num && mSystem.currentTimeMillis() - lastTimeChatGlobal > 1000)
		{
			lastTimeChatGlobal = mSystem.currentTimeMillis() - delayAutoChatGlobal - 500;
		}
		if (mSystem.currentTimeMillis() - lastTimeChatGlobal > delayAutoChatGlobal)
		{
			lastTimeChatGlobal = mSystem.currentTimeMillis();
			countChatGlobal++;
			gems = num;
			Service.gI().chatGlobal(autoChatGlobalContent);
		}
	}

	private static void SpamChatGlobal()
	{
		int num = Char.myCharz().luong + Char.myCharz().luongKhoa;
		if (num < 5 || timesSpamChatGlobal <= 0 || spammedChatGlobalTimes >= timesSpamChatGlobal)
		{
			isAutoSpamChatGlobal = false;
		}
		if (autoChatGlobalContent == null || autoChatGlobalContent.Equals(""))
		{
			try
			{
				autoChatGlobalContent = File.ReadAllText("Data\\TextChatGlobal.ini");
			}
			catch
			{
				autoChatGlobalContent = "Cường đẹp trai!";
			}
		}
		if (gems != num)
		{
			gems = num;
			spammedChatGlobalTimes++;
		}
		if (mSystem.currentTimeMillis() - lastTimeChatGlobal >= 150)
		{
			lastTimeChatGlobal = mSystem.currentTimeMillis();
			countChatGlobal++;
			gems = num;
			Service.gI().chatGlobal(autoChatGlobalContent);
		}
	}

	private static void ChatInbox()
	{
		if (listCharInbox == null)
		{
			listCharInbox = new List<int>();
		}
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			Char obj = (Char)GameScr.vCharInMap.elementAt(i);
			if (isInboxable(obj) && !listCharInbox.Contains(obj.charID))
			{
				listCharInbox.Add(obj.charID);
			}
		}
		if (autoInboxContent == null || autoInboxContent.Equals(""))
		{
			try
			{
				autoInboxContent = File.ReadAllText("Data\\TextChatInbox.ini");
			}
			catch
			{
				autoInboxContent = "Cường đẹp trai!";
			}
		}
		if (listCharInbox.Count > 0 && mSystem.currentTimeMillis() - lastTimeInbox > 2000)
		{
			lastTimeInbox = mSystem.currentTimeMillis();
			countChatInbox++;
			Service.gI().chatPlayer(autoInboxContent, listCharInbox[0]);
			listCharInbox.RemoveAt(0);
		}
	}

	private static bool isInboxable(Char ch)
	{
		if (ch != null && ch.cName != null && !ch.cName.Equals("") && !char.IsUpper(char.Parse(ch.cName.Substring(0, 1))) && !ch.isPet && !ch.isMiniPet && !ch.cName.StartsWith("#"))
		{
			return !ch.cName.StartsWith("$");
		}
		return false;
	}

	static AutoChat()
	{
		listCharInbox = new List<int>();
		inputDelayChatPublic = new string[2] { "Nhập delay chat:", "Mili giây (>=5000ms)" };
		inputDelayChatGlobal = new string[2] { "Nhập delay chat thế giới:", "Mili giây (>=5000ms)" };
		inputSpamChatGlobal = new string[2] { "Nhập số lần spam", "Số lần" };
		LoadSavedData();
	}
}
