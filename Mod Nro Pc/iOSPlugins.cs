using System.Runtime.InteropServices;
using UnityEngine;

public class iOSPlugins
{
	public static string devide;

	public static string Myname;

	[DllImport("__Internal")]
	private static extern void _SMSsend(string tophone, string withtext, int n);

	[DllImport("__Internal")]
	private static extern int _unpause();

	[DllImport("__Internal")]
	private static extern int _checkRotation();

	[DllImport("__Internal")]
	private static extern int _back();

	[DllImport("__Internal")]
	private static extern int _Send();

	[DllImport("__Internal")]
	private static extern void _purchaseItem(string itemID, string userName, string gameID);

	public static int Check()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return checkCanSendSMS();
		}
		devide = iPhoneSettings.generation.ToString();
		string text = string.Empty + devide[2];
		if (text == "h" && devide.Length > 6)
		{
			Myname = SystemInfo.operatingSystem.ToString();
			string text2 = string.Empty + Myname[10];
			if (text2 != "2" && text2 != "3")
			{
				return 0;
			}
			return 1;
		}
		Cout.println(devide + "  loai");
		if (devide == "Unknown" && ScaleGUI.WIDTH * ScaleGUI.HEIGHT < 786432f)
		{
			return 0;
		}
		return -1;
	}

	public static int checkCanSendSMS()
	{
		if (iPhoneSettings.generation == iPhoneGeneration.iPhone3GS || iPhoneSettings.generation == iPhoneGeneration.iPhone4 || iPhoneSettings.generation == iPhoneGeneration.iPhone4S || iPhoneSettings.generation == iPhoneGeneration.iPhone5)
		{
			return 0;
		}
		return -1;
	}

	public static void SMSsend(string phonenumber, string bodytext, int n)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			_SMSsend(phonenumber, bodytext, n);
		}
	}

	public static void back()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			_back();
		}
	}

	public static void Send()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			_Send();
		}
	}

	public static int unpause()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			return _unpause();
		}
		return 0;
	}

	public static int checkRotation()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			return _checkRotation();
		}
		return 0;
	}

	public static void purchaseItem(string itemID, string userName, string gameID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			_purchaseItem(itemID, userName, gameID);
		}
	}
}
