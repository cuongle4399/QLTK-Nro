public class MoneyCharge : mScreen, IActionListener
{
	public static MoneyCharge instance;

	public TField tfSerial;

	public TField tfCode;

	private int x;

	private int y;

	private int w;

	private int h;

	private string[] strPaint;

	private int focus;

	private int yt;

	private int freeAreaHeight;

	private int yy = GameCanvas.hh - mScreen.ITEM_HEIGHT - 5;

	private int yP;

	public MoneyCharge()
	{
		w = GameCanvas.w - 20;
		if (w > 320)
		{
			w = 320;
		}
		strPaint = mFont.tahoma_7b_green2.splitFontArray(mResources.pay_card, w - 20);
		x = (GameCanvas.w - w) / 2;
		y = GameCanvas.h - 150 - (strPaint.Length - 1) * 20;
		h = 110 + (strPaint.Length - 1) * 20;
		yP = y;
		tfSerial = new TField();
		tfSerial.name = mResources.SERI_NUM;
		tfSerial.x = x + 10;
		tfSerial.y = y + 35 + (strPaint.Length - 1) * 20;
		yt = tfSerial.y;
		tfSerial.width = w - 20;
		tfSerial.height = mScreen.ITEM_HEIGHT + 2;
		if (GameCanvas.isTouch)
		{
			tfSerial.isFocus = false;
		}
		else
		{
			tfSerial.isFocus = true;
		}
		tfSerial.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			tfSerial.showSubTextField = false;
		}
		if (Main.isIPhone)
		{
			tfSerial.isPaintMouse = false;
		}
		if (!GameCanvas.isTouch)
		{
			right = tfSerial.cmdClear;
		}
		tfCode = new TField();
		tfCode.name = mResources.CARD_CODE;
		tfCode.x = x + 10;
		tfCode.y = tfSerial.y + 35;
		tfCode.width = w - 20;
		tfCode.height = mScreen.ITEM_HEIGHT + 2;
		tfCode.isFocus = false;
		tfCode.setIputType(TField.INPUT_TYPE_ANY);
		if (Main.isWindowsPhone)
		{
			tfCode.showSubTextField = false;
		}
		if (Main.isIPhone)
		{
			tfCode.isPaintMouse = false;
		}
		left = new Command(mResources.CLOSE, this, 1, null);
		center = new Command(mResources.pay_card2, this, 2, null);
		if (GameCanvas.isTouch)
		{
			center.x = GameCanvas.w / 2 + 18;
			left.x = GameCanvas.w / 2 - 85;
			center.y = (left.y = y + h + 5);
		}
		freeAreaHeight = tfSerial.y - (4 * tfSerial.height - 10);
		yP = tfSerial.y;
	}

	public static MoneyCharge gI()
	{
		if (instance == null)
		{
			instance = new MoneyCharge();
		}
		return instance;
	}

	public override void switchToMe()
	{
		focus = 0;
		base.switchToMe();
	}

	public void updateTfWhenOpenKb()
	{
	}

	public override void paint(mGraphics g)
	{
		GameScr.gI().paint(g);
		PopUp.paintPopUp(g, x, y, w, h, -1, isButton: true);
		for (int i = 0; i < strPaint.Length; i++)
		{
			mFont.tahoma_7b_green2.drawString(g, strPaint[i], GameCanvas.w / 2, y + 15 + i * 20, mFont.CENTER);
		}
		tfSerial.paint(g);
		tfCode.paint(g);
		base.paint(g);
	}

	public override void update()
	{
		GameScr.gI().update();
		tfSerial.update();
		tfCode.update();
		if (Main.isWindowsPhone)
		{
			updateTfWhenOpenKb();
		}
	}

	public override void keyPress(int keyCode)
	{
		if (tfSerial.isFocus)
		{
			tfSerial.keyPressed(keyCode);
		}
		else if (tfCode.isFocus)
		{
			tfCode.keyPressed(keyCode);
		}
		base.keyPress(keyCode);
	}

	public override void updateKey()
	{
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21])
		{
			focus--;
			if (focus < 0)
			{
				focus = 1;
			}
		}
		else if (GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
		{
			focus++;
			if (focus > 1)
			{
				focus = 1;
			}
		}
		if (GameCanvas.keyPressed[(!Main.isPC) ? 2 : 21] || GameCanvas.keyPressed[(!Main.isPC) ? 8 : 22])
		{
			GameCanvas.clearKeyPressed();
			if (focus == 1)
			{
				tfSerial.isFocus = false;
				tfCode.isFocus = true;
				if (!GameCanvas.isTouch)
				{
					right = tfCode.cmdClear;
				}
			}
			else if (focus == 0)
			{
				tfSerial.isFocus = true;
				tfCode.isFocus = false;
				if (!GameCanvas.isTouch)
				{
					right = tfSerial.cmdClear;
				}
			}
			else
			{
				tfSerial.isFocus = false;
				tfCode.isFocus = false;
			}
		}
		if (GameCanvas.isPointerJustRelease)
		{
			if (GameCanvas.isPointerHoldIn(tfSerial.x, tfSerial.y, tfSerial.width, tfSerial.height))
			{
				focus = 0;
			}
			else if (GameCanvas.isPointerHoldIn(tfCode.x, tfCode.y, tfCode.width, tfCode.height))
			{
				focus = 1;
			}
		}
		base.updateKey();
		GameCanvas.clearKeyPressed();
	}

	public void clearScreen()
	{
		instance = null;
	}

	public void perform(int idAction, object p)
	{
		if (idAction == 1)
		{
			GameScr.instance.switchToMe();
			clearScreen();
		}
		if (idAction == 2)
		{
			if (tfSerial.getText() == null || tfSerial.getText().Equals(string.Empty))
			{
				GameCanvas.startOKDlg(mResources.serial_blank);
				return;
			}
			if (tfCode.getText() == null || tfCode.getText().Equals(string.Empty))
			{
				GameCanvas.startOKDlg(mResources.card_code_blank);
				return;
			}
			Service.gI().sendCardInfo(tfSerial.getText(), tfCode.getText());
			GameScr.instance.switchToMe();
			clearScreen();
		}
	}
}
