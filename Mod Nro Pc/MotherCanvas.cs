using System.IO;
using main.Mod;

public class MotherCanvas
{
	public static MotherCanvas instance;

	public GameCanvas tCanvas;

	public int zoomLevel = 1;

	public Image imgCache;

	private int[] imgRGBCache;

	private int newWidth;

	private int newHeight;

	private int[] output;

	private int OUTPUTSIZE = 20;

	public MotherCanvas()
	{
		checkZoomLevel(getWidth(), getHeight());
	}

	public void checkZoomLevel(int w, int h)
	{
		if (Main.isWindowsPhone)
		{
			mGraphics.zoomLevel = 2;
			if (w * h >= 2073600)
			{
				mGraphics.zoomLevel = 4;
			}
			else if (w * h > 384000)
			{
				mGraphics.zoomLevel = 3;
			}
		}
		else if (!Main.isPC)
		{
			try
			{
				MainMod.listBackgroundImages.Add(Image.__createImage(File.ReadAllBytes("Dragonboy_vn_v206_Data\\Resources\\unity_bgr")));
				MainMod.listBackgroundImages.Add(Image.__createImage(File.ReadAllBytes("Dragonboy_vn_v206_Data\\Resources\\unity_default_bg")));
				FileInfo[] files = new DirectoryInfo("background_image").GetFiles();
				FileInfo[] array = files;
				FileInfo[] array2 = array;
				FileInfo[] array3 = array2;
				foreach (FileInfo fileInfo in array3)
				{
					MainMod.listBackgroundImages.Add(Image.__createImage(File.ReadAllBytes("background_image\\" + fileInfo.Name)));
				}
			}
			catch
			{
			}
			if (Main.isIpod)
			{
				mGraphics.zoomLevel = 2;
			}
			else if (w * h >= 2073600)
			{
				mGraphics.zoomLevel = 4;
			}
			else if (w * h >= 691200)
			{
				mGraphics.zoomLevel = 3;
			}
			else if (w * h > 153600)
			{
				mGraphics.zoomLevel = 2;
			}
		}
		else
		{
			mGraphics.zoomLevel = 2;
			if (w * h < 480000)
			{
				mGraphics.zoomLevel = 1;
			}
		}
	}

	public int getWidth()
	{
		return (int)ScaleGUI.WIDTH;
	}

	public int getHeight()
	{
		return (int)ScaleGUI.HEIGHT;
	}

	public void setChildCanvas(GameCanvas tCanvas)
	{
		this.tCanvas = tCanvas;
	}

	protected void paint(mGraphics g)
	{
		tCanvas.paint(g);
	}

	protected void keyPressed(int keyCode)
	{
		tCanvas.keyPressedz(keyCode);
	}

	protected void keyReleased(int keyCode)
	{
		tCanvas.keyReleasedz(keyCode);
	}

	protected void pointerDragged(int x, int y)
	{
		x /= mGraphics.zoomLevel;
		y /= mGraphics.zoomLevel;
		tCanvas.pointerDragged(x, y);
	}

	protected void pointerPressed(int x, int y)
	{
		x /= mGraphics.zoomLevel;
		y /= mGraphics.zoomLevel;
		tCanvas.pointerPressed(x, y);
	}

	protected void pointerReleased(int x, int y)
	{
		x /= mGraphics.zoomLevel;
		y /= mGraphics.zoomLevel;
		tCanvas.pointerReleased(x, y);
	}

	public int getWidthz()
	{
		int width = getWidth();
		return width / mGraphics.zoomLevel + width % mGraphics.zoomLevel;
	}

	public int getHeightz()
	{
		int height = getHeight();
		return height / mGraphics.zoomLevel + height % mGraphics.zoomLevel;
	}
}
