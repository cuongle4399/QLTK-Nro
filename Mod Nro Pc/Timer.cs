using System;

public class Timer
{
	public static IActionListener timeListener;

	public static int idAction;

	public static long timeExecute;

	public static bool isON;

	public static void setTimer(IActionListener actionListener, int action, long timeEllapse)
	{
		timeListener = actionListener;
		idAction = action;
		timeExecute = mSystem.currentTimeMillis() + timeEllapse;
		isON = true;
	}

	public static void update()
	{
		long num = mSystem.currentTimeMillis();
		if (!isON || num <= timeExecute)
		{
			return;
		}
		isON = false;
		try
		{
			if (idAction > 0)
			{
				GameScr.gI().actionPerform(idAction, null);
			}
		}
		catch (Exception)
		{
		}
	}
}
