public class BallInfo
{
	public int x;

	public int y;

	public int xTo = -999;

	public int yTo = -999;

	public int count;

	public int vy;

	public int vx;

	public int dir;

	public int idImg;

	public bool isPaint = true;

	public bool isDone;

	public bool isSetImg;

	public Char cFocus;

	public void SetChar()
	{
		cFocus = new Char();
		cFocus.charID = Res.random(-999, -800);
		cFocus.head = -1;
		cFocus.body = -1;
		cFocus.leg = -1;
		cFocus.bag = -1;
		cFocus.cName = string.Empty;
		cFocus.cHP = (cFocus.cHPFull = 20L);
	}

	public void UpdChar()
	{
		cFocus.cx = x;
		cFocus.cy = y;
	}
}
