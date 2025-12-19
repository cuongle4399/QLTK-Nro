namespace Assets.src.e;

public class Small
{
	public Image img;

	public int id;

	public int timePaint;

	public int timeUpdate;

	public Small(Image img, int id)
	{
		this.img = img;
		this.id = id;
		timePaint = 0;
		timeUpdate = 0;
	}

	public void paint(mGraphics g, int transform, int x, int y, int anchor)
	{
		g.drawRegion(img, 0, 0, mGraphics.getImageWidth(img), mGraphics.getImageHeight(img), transform, x, y, anchor);
		if (GameCanvas.gameTick % 1000 == 0)
		{
			timePaint++;
			timeUpdate = timePaint;
		}
	}

	public void paint(mGraphics g, int transform, int f, int x, int y, int w, int h, int anchor)
	{
		paint(g, transform, f, x, y, w, h, anchor, isClip: false);
	}

	public void paint(mGraphics g, int transform, int f, int x, int y, int w, int h, int anchor, bool isClip)
	{
		if (mGraphics.getImageWidth(img) != 1)
		{
			g.drawRegion(img, 0, f * w, w, h, transform, x, y, anchor, isClip);
			if (GameCanvas.gameTick % 1000 == 0)
			{
				timePaint++;
				timeUpdate = timePaint;
			}
		}
	}

	public void update()
	{
		timeUpdate++;
		if (timeUpdate - timePaint > 1 && !Char.myCharz().isCharBodyImageID(id))
		{
			SmallImage.imgNew[id] = null;
		}
	}
}
