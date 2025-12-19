namespace Mod.community;

public class AutoPoint : IActionListener, IChatable
{
	private static AutoPoint _Instance;

	public static int typePotential;

	public static bool isAutoPoint;

	public static int damageToAuto;

	public static int hpToAuto;

	public static int mpToAuto;

	public static string[] inputDamageAuto = new string[2] { "Nhập Sức Đánh Mà Bạn Muốn Auto", "Sức Đánh" };

	public static string[] inputHPAuto = new string[2] { "Nhập HP Mà Bạn Muốn Auto", "HP" };

	public static string[] inputMPAuto = new string[2] { "Nhập MP Mà Bạn Muốn Auto", "MP" };

	public static string[] inputPointToAdd = new string[2] { "Nhập Chỉ Số Mà Bạn Muốn Cộng Thêm", "Chỉ Số" };

	public static string[] inputPointAddTo = new string[2] { "Nhập Chỉ Số Mà Bạn Muốn Cộng Tới", "Chỉ Số" };

	public static AutoPoint getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new AutoPoint();
		}
		return _Instance;
	}

	public static void Update()
	{
		if (isAutoPoint)
		{
			DoIt();
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
		{
			if (ChatTextField.gI().strChat.Equals(inputPointToAdd[0]))
			{
				try
				{
					int num = int.Parse(ChatTextField.gI().tfChat.getText());
					if ((typePotential == 0 || typePotential == 1) && num % 20 != 0)
					{
						GameScr.info1.addInfo("Chỉ Số HP, MP Phải chia hết cho 20. Vui Lòng Nhập Lại!");
						return;
					}
					if (typePotential == 0 || typePotential == 1)
					{
						num /= 20;
					}
					Service.gI().upPotential(typePotential, num);
					GameScr.info1.addInfo("Đã Cộng Xong!");
				}
				catch
				{
					GameScr.info1.addInfo("Chỉ Số Không Hợp Lệ, Vui Lòng Nhập Lại!");
				}
				ResetChatTextField();
			}
			else if (ChatTextField.gI().strChat.Equals(inputPointAddTo[0]))
			{
				try
				{
					int num2 = int.Parse(ChatTextField.gI().tfChat.getText());
					if ((typePotential == 0 || typePotential == 1) && num2 % 20 != 0)
					{
						GameScr.info1.addInfo("Chỉ Số HP, MP Phải chia hết cho 20. Vui Lòng Nhập Lại!");
						return;
					}
					if (typePotential == 0 || typePotential == 1)
					{
						num2 /= 20;
					}
					int num3 = Char.myCharz().cHPGoc / 20;
					if (typePotential == 1)
					{
						num3 = Char.myCharz().cMPGoc / 20;
					}
					if (typePotential == 2)
					{
						num3 = Char.myCharz().cDamGoc;
					}
					if (typePotential == 3)
					{
						num3 = Char.myCharz().cDefGoc;
					}
					if (typePotential == 4)
					{
						num3 = Char.myCharz().cCriticalGoc;
					}
					if (num2 <= num3)
					{
						GameScr.info1.addInfo("Chỉ Số Không Hợp Lệ, Vui Lòng Nhập Lại!");
						return;
					}
					Service.gI().upPotential(typePotential, num2 - num3);
					GameScr.info1.addInfo("Đã Cộng Xong!");
				}
				catch
				{
					GameScr.info1.addInfo("Chỉ Số Không Hợp Lệ, Vui Lòng Nhập Lại!");
				}
				ResetChatTextField();
			}
			else if (ChatTextField.gI().strChat.Equals(inputDamageAuto[0]))
			{
				try
				{
					int num4 = (damageToAuto = int.Parse(ChatTextField.gI().tfChat.getText()));
					GameScr.info1.addInfo("Auto Cộng Sức Đánh: " + NinjaUtil.getMoneys(num4));
				}
				catch
				{
					GameScr.info1.addInfo("Sức Đánh Không Hợp Lệ, Vui Lòng Nhập Lại!");
				}
				ResetChatTextField();
			}
			else if (ChatTextField.gI().strChat.Equals(inputHPAuto[0]))
			{
				try
				{
					int num5 = (hpToAuto = int.Parse(ChatTextField.gI().tfChat.getText()));
					GameScr.info1.addInfo("Auto Cộng HP: " + NinjaUtil.getMoneys(num5));
				}
				catch
				{
					GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!");
				}
				ResetChatTextField();
			}
			else if (ChatTextField.gI().strChat.Equals(inputMPAuto[0]))
			{
				try
				{
					int num6 = (mpToAuto = int.Parse(ChatTextField.gI().tfChat.getText()));
					GameScr.info1.addInfo("Auto Cộng MP: " + NinjaUtil.getMoneys(num6));
				}
				catch
				{
					GameScr.info1.addInfo("MP Không Hợp Lệ, Vui Lòng Nhập Lại!");
				}
				ResetChatTextField();
			}
		}
		else
		{
			Service.gI().chat(text);
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
			break;
		case 2:
			break;
		case 3:
			isAutoPoint = !isAutoPoint;
			GameScr.info1.addInfo("Auto\n" + (isAutoPoint ? "[STATUS: ON]" : "[STATUS: OFF]"));
			break;
		case 4:
			ChatTextField.gI().strChat = inputDamageAuto[0];
			ChatTextField.gI().tfChat.name = inputDamageAuto[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 5:
			ChatTextField.gI().strChat = inputHPAuto[0];
			ChatTextField.gI().tfChat.name = inputHPAuto[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 6:
			ChatTextField.gI().strChat = inputMPAuto[0];
			ChatTextField.gI().tfChat.name = inputMPAuto[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 7:
			ShowMenuAddPoint(0);
			break;
		case 8:
			ShowMenuAddPoint(1);
			break;
		case 9:
			ShowMenuAddPoint(2);
			break;
		case 10:
			ShowMenuAddPoint(3);
			break;
		case 11:
			ShowMenuAddPoint(4);
			break;
		case 12:
			typePotential = (int)p;
			GameCanvas.panel.isShow = false;
			ChatTextField.gI().strChat = inputPointToAdd[0];
			ChatTextField.gI().tfChat.name = inputPointToAdd[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		case 13:
			typePotential = (int)p;
			GameCanvas.panel.isShow = false;
			ChatTextField.gI().strChat = inputPointAddTo[0];
			ChatTextField.gI().tfChat.name = inputPointAddTo[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		}
	}

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Auto\n" + (isAutoPoint ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 3, null));
		myVector.addElement(new Command("Sức Đánh\n[" + NinjaUtil.getMoneys(damageToAuto) + "]", getInstance(), 4, null));
		myVector.addElement(new Command("HP\n[" + NinjaUtil.getMoneys(hpToAuto) + "]", getInstance(), 5, null));
		myVector.addElement(new Command("MP\n[" + NinjaUtil.getMoneys(mpToAuto) + "]", getInstance(), 6, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	public static void ShowMenuAddPoint(int typePotential)
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Cộng", getInstance(), 12, typePotential));
		myVector.addElement(new Command("Cộng\nTới Mức", getInstance(), 13, typePotential));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	public static void DoIt()
	{
		if (Char.myCharz().cDamGoc < damageToAuto)
		{
			if (Char.myCharz().cTiemNang > Char.myCharz().cDamGoc * 100 && GameCanvas.gameTick % 20 == 0)
			{
				Service.gI().upPotential(2, 1);
			}
		}
		else if (Char.myCharz().cHPGoc < hpToAuto)
		{
			if (Char.myCharz().cTiemNang > Char.myCharz().cHPGoc + 1000 && GameCanvas.gameTick % 20 == 0)
			{
				Service.gI().upPotential(0, 1);
			}
		}
		else if (Char.myCharz().cMPGoc < mpToAuto && Char.myCharz().cTiemNang > Char.myCharz().cMPGoc + 1000 && GameCanvas.gameTick % 20 == 0)
		{
			Service.gI().upPotential(1, 1);
		}
	}
}
