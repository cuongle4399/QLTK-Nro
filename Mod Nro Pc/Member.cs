public class Member
{
	public int ID;

	public short head;

	public short headICON = -1;

	public short leg;

	public short body;

	public string name;

	public sbyte role;

	public string powerPoint;

	public int donate;

	public int receive_donate;

	public int curClanPoint;

	public int clanPoint;

	public int lastRequest;

	public string joinTime;

	public static string getRole(int r)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (1 == 0)
		{
		}
		string text = r switch
		{
			0 => mResources.clan_leader, 
			1 => mResources.clan_coleader, 
			2 => mResources.member, 
			_ => string.Empty, 
		};
		if (1 == 0)
		{
		}
		string text2 = text;
		bool flag4 = false;
		string text3 = text2;
		bool flag5 = false;
		string result = text3;
		bool flag6 = false;
		return result;
	}
}
