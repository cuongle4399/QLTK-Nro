public class EffecMn
{
	public static MyVector vEff = new MyVector();

	public static void addEff(Effect me)
	{
		vEff.addElement(me);
	}

	public static void removeEff(int id)
	{
		if (getEffById(id) != null)
		{
			vEff.removeElement(getEffById(id));
		}
	}

	public static Effect getEffById(int id)
	{
		for (int i = 0; i < vEff.size(); i++)
		{
			Effect effect = (Effect)vEff.elementAt(i);
			if (effect.effId == id)
			{
				return effect;
			}
		}
		return null;
	}

	public static void paintBackGroundUnderLayer(mGraphics g, int x, int y, int layer)
	{
		for (int i = 0; i < vEff.size(); i++)
		{
			if (((Effect)vEff.elementAt(i)).layer == -layer)
			{
				((Effect)vEff.elementAt(i)).paintUnderBackground(g, x, y);
			}
		}
	}

	public static void paintLayer1(mGraphics g)
	{
		for (int i = 0; i < vEff.size(); i++)
		{
			if (((Effect)vEff.elementAt(i)).layer == 1)
			{
				((Effect)vEff.elementAt(i)).paint(g);
			}
		}
	}

	public static void paintLayer2(mGraphics g)
	{
		for (int i = 0; i < vEff.size(); i++)
		{
			if (((Effect)vEff.elementAt(i)).layer == 2)
			{
				((Effect)vEff.elementAt(i)).paint(g);
			}
		}
	}

	public static void paintLayer3(mGraphics g)
	{
		for (int i = 0; i < vEff.size(); i++)
		{
			if (((Effect)vEff.elementAt(i)).layer == 3)
			{
				((Effect)vEff.elementAt(i)).paint(g);
			}
		}
	}

	public static void paintLayer4(mGraphics g)
	{
		for (int i = 0; i < vEff.size(); i++)
		{
			if (((Effect)vEff.elementAt(i)).layer == 4)
			{
				((Effect)vEff.elementAt(i)).paint(g);
			}
		}
	}

	public static void update()
	{
		for (int i = 0; i < vEff.size(); i++)
		{
			((Effect)vEff.elementAt(i)).update();
		}
	}
}
