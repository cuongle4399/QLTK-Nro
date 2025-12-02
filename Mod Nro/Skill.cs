public class Skill
{
	public const sbyte ATT_STAND = 0;

	public const sbyte ATT_FLY = 1;

	public const sbyte SKILL_AUTO_USE = 0;

	public const sbyte SKILL_CLICK_USE_ATTACK = 1;

	public const sbyte SKILL_CLICK_USE_BUFF = 2;

	public const sbyte SKILL_CLICK_NPC = 3;

	public const sbyte SKILL_CLICK_LIVE = 4;

	public SkillTemplate template;

	public short skillId;

	public int point;

	public long powRequire;

	public int coolDown;

	public long lastTimeUseThisSkill;

	public int dx;

	public int dy;

	public int maxFight;

	public int manaUse;

	public SkillOption[] options;

	public bool paintCanNotUseSkill;

	public short damage;

	public string moreInfo;

	public short price;

	public short curExp;

	public string strCurExp()
	{
		if (curExp / 10 >= 100)
		{
			return "MAX";
		}
		if (curExp % 10 == 0)
		{
			return curExp / 10 + "%";
		}
		int num = curExp % 10;
		return curExp / 10 + "." + num % 10 + "%";
	}

	public string strTimeReplay()
	{
		if (coolDown % 1000 == 0)
		{
			return coolDown / 1000 + string.Empty;
		}
		int num = coolDown % 1000;
		return coolDown / 1000 + "." + ((num % 100 != 0) ? (num / 10) : (num / 100));
	}

	public void paint(int x, int y, mGraphics g)
	{
		SmallImage.drawSmallImage(g, template.iconId, x, y, 0, StaticObj.VCENTER_HCENTER);
		long num = mSystem.currentTimeMillis();
		long num2 = num - lastTimeUseThisSkill;
		if (num2 < coolDown)
		{
			g.setColor(2721889, 0.7f);
			if (paintCanNotUseSkill && GameCanvas.gameTick % 6 > 2)
			{
				g.setColor(876862);
			}
			int num3 = (int)(num2 * 20 / coolDown);
			g.fillRect(x - 10, y - 10 + num3, 20, 20 - num3);
		}
		else
		{
			paintCanNotUseSkill = false;
		}
	}
}
