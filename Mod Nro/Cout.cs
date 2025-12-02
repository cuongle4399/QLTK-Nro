using UnityEngine;

public class Cout
{
	public static int count;

	public static void println(string s)
	{
		if (mSystem.isTest)
		{
			Debug.Log(((count % 2 != 0) ? "***--- " : ">>>--- ") + s);
			count++;
		}
	}

	public static void Log(string str)
	{
		if (mSystem.isTest)
		{
			Debug.Log(str);
		}
	}

	public static void LogError(string str)
	{
		if (mSystem.isTest)
		{
			Debug.LogError(str);
		}
	}

	public static void LogError2(string str)
	{
		if (!mSystem.isTest)
		{
		}
	}

	public static void LogError3(string str)
	{
		if (!mSystem.isTest)
		{
		}
	}

	public static void LogWarning(string str)
	{
		if (mSystem.isTest)
		{
			Debug.LogWarning(str);
		}
	}
}
