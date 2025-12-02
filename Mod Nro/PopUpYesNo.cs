public class PopUpYesNo : IActionListener
{
	public Command cmdYes;

	public Command cmdNo;

	public string[] info;

	private int X;

	private int Y;

	private int W = 120;

	private int H;

	private int dem;

	private long last;

	private long curr;

	public void setPopUp(string info, Command cmdYes, Command cmdNo)
	{
		this.info = new string[1] { info };
		H = 29;
		this.cmdYes = cmdYes;
		this.cmdNo = cmdNo;
		this.cmdYes.img = (this.cmdNo.img = GameScr.imgNut);
		this.cmdYes.imgFocus = (this.cmdNo.imgFocus = GameScr.imgNutF);
		this.cmdYes.w = mGraphics.getImageWidth(cmdYes.img);
		this.cmdNo.w = mGraphics.getImageWidth(cmdYes.img);
		this.cmdYes.h = mGraphics.getImageHeight(cmdYes.img);
		this.cmdNo.h = mGraphics.getImageHeight(cmdYes.img);
		last = mSystem.currentTimeMillis();
		dem = this.info[0].Length / 3;
		if (dem < 15)
		{
			dem = 15;
		}
		TextInfo.reset();
	}

	public void paint(mGraphics g)
	{
		PopUp.paintPopUp(g, X, Y, W, H + ((!GameCanvas.isTouch) ? 10 : 0), 16777215, isButton: false);
		if (info != null)
		{
			TextInfo.paint(g, info[0], X + 5, Y + H / 2 - ((!GameCanvas.isTouch) ? 6 : 4), W - 10, H, mFont.tahoma_7);
			if (GameCanvas.isTouch)
			{
				cmdYes.paint(g);
				mFont.tahoma_7_yellow.drawString(g, dem + string.Empty, cmdYes.x + cmdYes.w / 2, cmdYes.y + cmdYes.h + 5, 2, mFont.tahoma_7_grey);
			}
			else if (TField.isQwerty)
			{
				mFont.tahoma_7b_blue.drawString(g, mResources.do_accept_qwerty + dem + ")", X + W / 2, Y + H - 6, 2);
			}
			else
			{
				mFont.tahoma_7b_blue.drawString(g, mResources.do_accept + dem + ")", X + W / 2, Y + H - 6, 2);
			}
		}
	}

	public void update()
	{
		if (info != null)
		{
			X = GameCanvas.w - 5 - W;
			Y = 45;
			if (GameCanvas.w - 50 > 155 + W)
			{
				X = GameCanvas.w - 55 - W;
				Y = 5;
			}
			cmdYes.x = X - 35;
			cmdYes.y = Y;
			curr = mSystem.currentTimeMillis();
			Res.outz("curr - last= " + (curr - last));
			if (curr - last >= 1000)
			{
				last = mSystem.currentTimeMillis();
				dem--;
			}
			if (dem == 0)
			{
				GameScr.gI().popUpYesNo = null;
			}
		}
	}

	public void perform(int idAction, object p)
	{
	}
}
