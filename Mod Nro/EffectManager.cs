public class EffectManager : MyVector
{
	public static EffectManager lowEffects = new EffectManager();

	public static EffectManager mid_2Effects = new EffectManager();

	public static EffectManager midEffects = new EffectManager();

	public static EffectManager hiEffects = new EffectManager();

	public void updateAll()
	{
		for (int num = size() - 1; num >= 0; num--)
		{
			Effect_End effect_End = (Effect_End)elementAt(num);
			if (effect_End != null)
			{
				effect_End.update();
				if (effect_End.isRemove)
				{
					removeElementAt(num);
				}
			}
		}
	}

	public static void update()
	{
		hiEffects.updateAll();
		mid_2Effects.updateAll();
		midEffects.updateAll();
		lowEffects.updateAll();
	}

	public void paintAll(mGraphics g)
	{
		for (int i = 0; i < size(); i++)
		{
			Effect_End effect_End = (Effect_End)elementAt(i);
			if (effect_End != null && !effect_End.isRemove)
			{
				((Effect_End)elementAt(i)).paint(g);
			}
		}
	}

	public void removeAll()
	{
		for (int num = size() - 1; num >= 0; num--)
		{
			Effect_End effect_End = (Effect_End)elementAt(num);
			if (effect_End != null)
			{
				effect_End.isRemove = true;
				removeElementAt(num);
			}
		}
	}

	public static void remove()
	{
		hiEffects.removeAll();
		lowEffects.removeAll();
		midEffects.removeAll();
		mid_2Effects.removeAll();
	}

	public static void addHiEffect(Effect_End eff)
	{
		hiEffects.addElement(eff);
	}

	public static void addMidEffects(Effect_End eff)
	{
		midEffects.addElement(eff);
	}

	public static void addMid_2Effects(Effect_End eff)
	{
		mid_2Effects.addElement(eff);
	}

	public static void addLowEffect(Effect_End eff)
	{
		lowEffects.addElement(eff);
	}
}
