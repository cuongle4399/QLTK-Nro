public class InfoDlg
{
	public static bool isShow;

	private static string title;

	private static string subtitke;

	public static int delay;

	public static bool isLock;

	public static void show(string title, string subtitle, int delay)
	{
		if (title != null)
		{
			isShow = true;
			InfoDlg.title = title;
			subtitke = subtitle;
			InfoDlg.delay = delay;
		}
	}

	public static void showWait()
	{
		show(mResources.PLEASEWAIT, null, 1000);
		isLock = true;
	}

	public static void showWait(string str)
	{
		show(str, null, 700);
		isLock = true;
	}

	public static void paint(mGraphics g)
	{
		if (isShow && (!isLock || delay <= 4990) && !GameScr.isPaintAlert)
		{
			int num = 10;
			GameCanvas.paintz.paintPopUp(GameCanvas.hw - 75, num, 150, 55, g);
			if (isLock)
			{
				GameCanvas.paintShukiren(GameCanvas.hw - mFont.tahoma_8b.getWidth(title) / 2 - 10, num + 28, g);
				mFont.tahoma_8b.drawString(g, title, GameCanvas.hw + 5, num + 21, 2);
			}
			else if (subtitke != null)
			{
				mFont.tahoma_8b.drawString(g, title, GameCanvas.hw, num + 13, 2);
				mFont.tahoma_7_green2.drawString(g, subtitke, GameCanvas.hw, num + 30, 2);
			}
			else
			{
				mFont.tahoma_8b.drawString(g, title, GameCanvas.hw, num + 21, 2);
			}
		}
	}

	public static void update()
	{
		if (isShow && delay > 0)
		{
			delay--;
			if (delay <= 0)
			{
				hide();
			}
		}
	}

	public static void hide()
	{
		title = string.Empty;
		subtitke = null;
		isLock = false;
		delay = 0;
		isShow = false;
	}
}
