using UnityEngine;

internal class Net
{
	public static WWW www;

	public static Command h;

	public static void update()
	{
		if (www != null && www.isDone)
		{
			string str = string.Empty;
			if (www.error == null || www.error.Equals(string.Empty))
			{
				str = www.text;
			}
			www = null;
			if (h != null)
			{
				h.perform(str);
			}
		}
	}

	public static void connectHTTP(string link, Command h)
	{
		if (www != null)
		{
			Cout.LogError("GET HTTP BUSY");
		}
		www = new WWW(link);
		Net.h = h;
	}

	public static void connectHTTP2(string link, Command h)
	{
		Net.h = h;
		if (link != null)
		{
			h.perform(link);
		}
	}
}
