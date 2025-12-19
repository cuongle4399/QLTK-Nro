public class PlayerInfo
{
	public string name;

	public string showName;

	public string status;

	public int IDDB;

	private int exp;

	public bool isReady;

	public int xu;

	public int gold;

	public string strMoney = string.Empty;

	public sbyte finishPosition;

	public bool isMaster;

	public static Image[] imgStart;

	public sbyte[] indexLv;

	public int onlineTime;

	public string getName()
	{
		return name;
	}

	public void setMoney(int m)
	{
		xu = m;
		strMoney = GameCanvas.getMoneys(xu);
	}

	public void setName(string name)
	{
		this.name = name;
		if (name.Length > 9)
		{
			showName = name.Substring(0, 8);
		}
		else
		{
			showName = name;
		}
	}

	public void paint(mGraphics g, int x, int y)
	{
	}

	public int getExp()
	{
		return exp;
	}
}
