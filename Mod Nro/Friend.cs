public class Friend
{
	public string friendName;

	public sbyte type;

	public Friend(string friendName, sbyte type)
	{
		this.friendName = friendName;
		this.type = type;
	}

	public Friend(string friendName)
	{
		this.friendName = friendName;
		type = 2;
	}
}
