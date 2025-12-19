public class ClanImage
{
	public int ID;

	public string name;

	public short[] idImage;

	public int xu;

	public int luong;

	public static MyVector vClanImage = new MyVector();

	public static MyHashTable idImages = new MyHashTable();

	public static void addClanImage(ClanImage cm)
	{
		Service.gI().clanImage((sbyte)cm.ID);
		vClanImage.addElement(cm);
	}

	public static ClanImage getClanImage(short ID)
	{
		for (int i = 0; i < vClanImage.size(); i++)
		{
			ClanImage clanImage = (ClanImage)vClanImage.elementAt(i);
			if (clanImage.ID == ID)
			{
				return clanImage;
			}
		}
		return null;
	}

	public static bool isExistClanImage(int ID)
	{
		for (int i = 0; i < vClanImage.size(); i++)
		{
			ClanImage clanImage = (ClanImage)vClanImage.elementAt(i);
			if (clanImage.ID == ID)
			{
				return true;
			}
		}
		return false;
	}
}
