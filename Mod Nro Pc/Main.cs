using main.Mod;
using ModCak.main.Mod;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public class Main : MonoBehaviour
{
	public static Main main;

	public static mGraphics g;

	public static GameMidlet midlet;

	public static string res;

	public static string mainThreadName;

	public static bool started;

	public static bool isIpod;

	public static bool isIphone4;

	public static bool isPC;

	public static bool isWindowsPhone;

	public static bool isIPhone;

	public static bool IphoneVersionApp;

	public static string IMEI;

	public static int versionIp;

	public static int numberQuit;

	public static int typeClient;

	public const sbyte PC_VERSION = 4;

	public const sbyte IP_APPSTORE = 5;

	public const sbyte WINDOWSPHONE = 6;

	private int level;

	public const sbyte IP_JB = 3;

	private int updateCount;

	private int paintCount;

	private int count;

	private int fps;

	private int max;

	private int up;

	private int upmax;

	private long timefps;

	private long timeup;

	private bool isRun;

	public static int waitTick;

	public static int f;

	public static bool isResume;

	public static bool isMiniApp;

	public static bool isQuitApp;

	private Vector2 lastMousePos;

	public static int a;

	public static bool isCompactDevice;

	private void Start()
	{
		if (started)
		{
			return;
		}
		Application.runInBackground = true;
		Time.timeScale = ModProCL.timeScale;
		if (Thread.CurrentThread.Name != "Main")
		{
			Thread.CurrentThread.Name = "Main";
		}
		mainThreadName = Thread.CurrentThread.Name;
		isPC = Application.platform == RuntimePlatform.WindowsPlayer;
		started = true;
		if (isPC)
		{
			bool fullscreen = false;
			string[] array = File.ReadAllText("Data\\size.ini").Split('|');
			if (!array[2].Equals("0"))
			{
				fullscreen = true;
			}
			level = Rms.loadRMSInt("levelScreenKN");
			if (level == 1)
			{
				Screen.SetResolution(int.Parse(array[0]), int.Parse(array[1]), fullscreen);
			}
			else
			{
				Screen.SetResolution(int.Parse(array[0]), int.Parse(array[1]), fullscreen);
			}
		}
	}

	private void SetInit()
	{
		base.enabled = true;
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	private void OnGUI()
	{
		if (count >= 10)
		{
			if (fps == 0)
			{
				timefps = mSystem.currentTimeMillis();
			}
			else if (mSystem.currentTimeMillis() - timefps > 1000)
			{
				max = fps;
				fps = 0;
				timefps = mSystem.currentTimeMillis();
			}
			fps++;
			checkInput();
			Session_ME.update();
			Session_ME2.update();
			if (Event.current.type.Equals(EventType.Repaint) && paintCount <= updateCount)
			{
				GameMidlet.gameCanvas.paint(g);
				paintCount++;
				g.reset();
			}
		}
	}

	public void setsizeChange()
	{
		if (!isRun)
		{
            Screen.orientation = ScreenOrientation.LandscapeLeft;
			Application.runInBackground = true;
			base.useGUILayout = false;
			isCompactDevice = detectCompactDevice();
			if (main == null)
			{
				main = this;
			}
			isRun = true;
			ScaleGUI.initScaleGUI();
			if (isPC)
			{
				IMEI = SystemInfo.deviceUniqueIdentifier;
			}
			else
			{
				IMEI = GetMacAddress();
			}
			if (isPC)
			{
				Screen.fullScreen = false;
			}
			if (isWindowsPhone)
			{
				typeClient = 6;
			}
			if (isPC)
			{
				typeClient = 4;
			}
			if (IphoneVersionApp)
			{
				typeClient = 5;
			}
			if (iPhoneSettings.generation == iPhoneGeneration.iPodTouch4Gen)
			{
				isIpod = true;
			}
			if (iPhoneSettings.generation == iPhoneGeneration.iPhone4)
			{
				isIphone4 = true;
			}
			g = new mGraphics();
			midlet = new GameMidlet();
			TileMap.loadBg();
			Paint.loadbg();
			PopUp.loadBg();
			GameScr.loadBg();
			InfoMe.gI().loadCharId();
			Panel.loadBg();
			Menu.loadBg();
			Key.mapKeyPC();
			SoundMn.gI().loadSound(TileMap.mapID);
			g.CreateLineMaterial();
			if (File.Exists("admin.ini"))
			{
                WinConsole.Init();
                WinConsole.WriteLine("CHẾ ĐỘ NHÀ PHÁT TRIỂN");
            }
        }
	}

	public static void setBackupIcloud(string path)
	{
	}

	public string GetMacAddress()
	{
		_ = string.Empty;
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		for (int i = 0; i < allNetworkInterfaces.Length; i++)
		{
			PhysicalAddress physicalAddress = allNetworkInterfaces[i].GetPhysicalAddress();
			if (physicalAddress.ToString() != string.Empty)
			{
				return physicalAddress.ToString();
			}
		}
		return string.Empty;
	}

	public void doClearRMS()
	{
		if (isPC && Rms.loadRMSInt("lastZoomlevel") != mGraphics.zoomLevel)
		{
			Rms.clearAll();
			Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
			Rms.saveRMSInt("levelScreenKN", level);
		}
	}

	public static void closeKeyBoard()
	{
		if (TouchScreenKeyboard.visible)
		{
			TField.kb.active = false;
			TField.kb = null;
		}
	}

	private void FixedUpdate()
	{
		Rms.update();
		count++;
		if (count >= 10)
		{
			if (up == 0)
			{
				timeup = mSystem.currentTimeMillis();
			}
			else if (mSystem.currentTimeMillis() - timeup > 1000)
			{
				upmax = up;
				up = 0;
				timeup = mSystem.currentTimeMillis();
			}
			up++;
			setsizeChange();
			updateCount++;
			ipKeyboard.update();
			GameMidlet.gameCanvas.update();
			Image.update();
			DataInputStream.update();
			SMS.update();
			Net.update();
			f++;
			if (f > 8)
			{
				f = 0;
			}
			if (!isPC)
			{
				_ = 1 / a;
			}
		}
	}

	private void checkInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePosition = Input.mousePosition;
			GameMidlet.gameCanvas.pointerPressed((int)(mousePosition.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition.y) / (float)mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard);
			lastMousePos.x = mousePosition.x / (float)mGraphics.zoomLevel;
			lastMousePos.y = mousePosition.y / (float)mGraphics.zoomLevel + (float)mGraphics.addYWhenOpenKeyBoard;
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition2 = Input.mousePosition;
			GameMidlet.gameCanvas.pointerDragged((int)(mousePosition2.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition2.y) / (float)mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard);
			lastMousePos.x = mousePosition2.x / (float)mGraphics.zoomLevel;
			lastMousePos.y = mousePosition2.y / (float)mGraphics.zoomLevel + (float)mGraphics.addYWhenOpenKeyBoard;
		}
		if (Input.GetMouseButtonUp(0))
		{
			Vector3 mousePosition3 = Input.mousePosition;
			lastMousePos.x = mousePosition3.x / (float)mGraphics.zoomLevel;
			lastMousePos.y = mousePosition3.y / (float)mGraphics.zoomLevel + (float)mGraphics.addYWhenOpenKeyBoard;
			GameMidlet.gameCanvas.pointerReleased((int)(mousePosition3.x / (float)mGraphics.zoomLevel), (int)(((float)Screen.height - mousePosition3.y) / (float)mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard);
		}
		if (Input.anyKeyDown && Event.current.type == EventType.KeyDown)
		{
			int num = MyKeyMap.map(Event.current.keyCode);
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				MainMod.checkInputSHIFT(ref num);
				switch (Event.current.keyCode)
				{
				case KeyCode.Alpha2:
					num = 64;
					break;
				case KeyCode.Minus:
					num = 95;
					break;
				}
			}
			if (num != 0)
			{
				GameMidlet.gameCanvas.keyPressedz(num);
			}
		}
		if (Event.current.type == EventType.KeyUp)
		{
			int num2 = MyKeyMap.map(Event.current.keyCode);
			if (num2 != 0)
			{
				GameMidlet.gameCanvas.keyReleasedz(num2);
			}
		}
		if (isPC)
		{
			GameMidlet.gameCanvas.scrollMouse((int)(Input.GetAxis("Mouse ScrollWheel") * 10f));
			int num3 = (int)Input.mousePosition.x;
			float y = Input.mousePosition.y;
			int x = num3 / mGraphics.zoomLevel;
			int y2 = (Screen.height - (int)y) / mGraphics.zoomLevel;
			GameMidlet.gameCanvas.pointerMouse(x, y2);
		}
	}

	private void OnApplicationQuit()
	{
		if (SocketGame.IsRunning)
		{
			SocketGame.Disconnect();
		}
		Debug.LogWarning("APP QUIT");
		GameCanvas.bRun = false;
		Session_ME.gI().close();
		Session_ME2.gI().close();
		if (isPC)
		{
			Application.Quit();
		}
	}

	private void OnApplicationPause(bool paused)
	{
		isResume = false;
		if (paused)
		{
			if (GameCanvas.isWaiting())
			{
				isQuitApp = true;
			}
		}
		else
		{
			isResume = true;
		}
		if (TouchScreenKeyboard.visible)
		{
			TField.kb.active = false;
			TField.kb = null;
		}
		if (isQuitApp)
		{
			Application.Quit();
		}
	}

	public static void exit()
	{
		if (isPC)
		{
			main.OnApplicationQuit();
		}
		else
		{
			a = 0;
		}
	}

	public static bool detectCompactDevice()
	{
		if (iPhoneSettings.generation != iPhoneGeneration.iPhone && iPhoneSettings.generation != iPhoneGeneration.iPhone3G && iPhoneSettings.generation != iPhoneGeneration.iPodTouch1Gen)
		{
			return iPhoneSettings.generation != iPhoneGeneration.iPodTouch2Gen;
		}
		return false;
	}

	public static bool checkCanSendSMS()
	{
		if (iPhoneSettings.generation != iPhoneGeneration.iPhone3GS && iPhoneSettings.generation != iPhoneGeneration.iPhone4)
		{
			return iPhoneSettings.generation > iPhoneGeneration.iPodTouch4Gen;
		}
		return true;
	}

	static Main()
	{
		res = "res";
		numberQuit = 1;
		typeClient = 4;
		isMiniApp = true;
		a = 1;
		isCompactDevice = true;
	}
}
