public class EPosition
{
	public int x;

	public int y;

	public int anchor;

	public sbyte follow;

	public sbyte count;

	public sbyte dir = 1;

	public short index = -1;

	public EPosition(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public EPosition(int x, int y, int fol)
	{
		this.x = x;
		this.y = y;
		follow = (sbyte)fol;
	}

	public EPosition()
	{
	}
}
