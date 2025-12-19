public class SkillTemplate
{
	public sbyte id;

	public int classId;

	public string name;

	public int maxPoint;

	public int manaUseType;

	public int type;

	public int iconId;

	public string[] description;

	public Skill[] skills;

	public string damInfo;

	public bool isBuffToPlayer()
	{
		if (type == 2)
		{
			return true;
		}
		return false;
	}

	public bool isUseAlone()
	{
		if (type == 3)
		{
			return true;
		}
		return false;
	}

	public bool isAttackSkill()
	{
		if (type == 1)
		{
			return true;
		}
		return false;
	}

	public bool isSkillSpec()
	{
		if (type == 4)
		{
			return true;
		}
		return false;
	}
}
