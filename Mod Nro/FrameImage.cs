using System;

public class FrameImage
{
	public int frameWidth;

	public int frameHeight;

	public int nFrame;

	public Image imgFrame;

	public int Id = -1;

	public int numWidth;

	public int numHeight;

	public FrameImage(int ID)
	{
		Id = ID;
		Image image = Effect_End.getImage(ID);
		if (image != null)
		{
			imgFrame = image;
			frameWidth = Effect_End.arrInfoEff[ID][0];
			frameHeight = Effect_End.arrInfoEff[ID][1] / Effect_End.arrInfoEff[ID][2];
			nFrame = Effect_End.arrInfoEff[ID][2];
		}
	}

	public FrameImage(Image img, int width, int height)
	{
		if (img != null)
		{
			imgFrame = img;
			frameWidth = width;
			frameHeight = height;
			nFrame = img.getHeight() / height;
			if (nFrame < 1)
			{
				nFrame = 1;
			}
		}
	}

	public FrameImage(Image img, int numW, int numH, int numNull)
	{
		if (img != null)
		{
			imgFrame = img;
			numWidth = numW;
			numHeight = numH;
			frameWidth = imgFrame.getWidth() / numW;
			frameHeight = imgFrame.getHeight() / numH;
			nFrame = numW * numH - numNull;
		}
	}

	public void drawFrame(int idx, int x, int y, int trans, int anchor, mGraphics g)
	{
		try
		{
			if (imgFrame != null)
			{
				if (idx > nFrame)
				{
					idx = nFrame;
				}
				int num = idx * frameHeight;
				if (num > frameHeight * (nFrame - 1) || num < 0)
				{
					num = frameHeight * (nFrame - 1);
				}
				g.drawRegion(imgFrame, 0, num, frameWidth, frameHeight, trans, x, y, anchor);
			}
		}
		catch (Exception)
		{
		}
	}
}
