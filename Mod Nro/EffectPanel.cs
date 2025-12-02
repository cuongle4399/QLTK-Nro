public class EffectPanel : Effect2
{
	public EffectCharPaint eff;

	private int i0;

	private int dx0;

	private int dy0;

	private int x;

	private int y;

	private Char c;

	private Mob m;

	private short loopCount;

	private long endTime;

	private int trans;

	public static void addServerEffect(int id, int cx, int cy, int loopCount)
	{
		EffectPanel effectPanel = new EffectPanel();
		effectPanel.eff = GameScr.efs[id - 1];
		effectPanel.x = cx;
		effectPanel.y = cy;
		effectPanel.loopCount = (short)loopCount;
		Effect2.vEffect3.addElement(effectPanel);
	}

	public override void paint(mGraphics g)
	{
		if (mGraphics.zoomLevel == 1)
		{
			GameScr.countEff++;
		}
		if (GameScr.countEff < 8)
		{
			if (c != null)
			{
				x = c.cx;
				y = c.cy + GameCanvas.transY;
			}
			if (m != null)
			{
				x = m.x;
				y = m.y + GameCanvas.transY;
			}
			int num = x + dx0 + eff.arrEfInfo[i0].dx;
			int num2 = y + dy0 + eff.arrEfInfo[i0].dy;
			SmallImage.drawSmallImage(g, eff.arrEfInfo[i0].idImg, num, num2, trans, mGraphics.VCENTER | mGraphics.HCENTER);
		}
	}

	public override void update()
	{
		if (endTime != 0)
		{
			i0++;
			if (i0 >= eff.arrEfInfo.Length)
			{
				i0 = 0;
			}
			if (mSystem.currentTimeMillis() - endTime > 0)
			{
				Effect2.vEffect3.removeElement(this);
			}
		}
		else
		{
			i0++;
			if (i0 >= eff.arrEfInfo.Length)
			{
				loopCount--;
				if (loopCount <= 0)
				{
					Effect2.vEffect3.removeElement(this);
				}
				else
				{
					i0 = 0;
				}
			}
		}
		if (GameCanvas.gameTick % 11 == 0 && c != null && c != Char.myCharz() && !GameScr.vCharInMap.contains(c))
		{
			Effect2.vEffect3.removeElement(this);
		}
	}
}
