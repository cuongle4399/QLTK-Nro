using System;

public class ipKeyboard
{
	private static TouchScreenKeyboard tk;

	public static int TEXT;

	public static int NUMBERIC = 1;

	public static int PASS = 2;

	private static Command act;

	public static void openKeyBoard(string caption, int type, string text, Command action)
	{
		act = action;
		TouchScreenKeyboardType t = ((type == 0 || type == 2) ? TouchScreenKeyboardType.ASCIICapable : TouchScreenKeyboardType.NumberPad);
		TouchScreenKeyboard.hideInput = false;
		tk = TouchScreenKeyboard.Open(text, t, b1: false, b2: false, type == 2, b3: false, caption);
	}

	public static void update()
	{
		try
		{
			if (tk != null && tk.done)
			{
				if (act != null)
				{
					act.perform(tk.text);
				}
				tk.text = string.Empty;
				tk = null;
			}
		}
		catch (Exception)
		{
		}
	}
}
