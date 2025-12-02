public class Line
{
	public int x0;

	public int y0;

	public int x1;

	public int y1;

	public int vx;

	public int vy;

	public int f;

	public int fRe;

	public int idColor;

	public int type;

	public bool is2Line;

	public FrameImage fraImgEff;

	public int[] frame;

	public void setLine(int x0, int y0, int x1, int y1, int vx, int vy, bool is2Line)
	{
		this.x0 = x0;
		this.y0 = y0;
		this.x1 = x1;
		this.y1 = y1;
		this.vx = vx;
		this.vy = vy;
		this.is2Line = is2Line;
	}

	public void update()
	{
		x0 += vx;
		x1 += vx;
		y0 += vy;
		y1 += vy;
		f++;
	}

	public void update_not_F()
	{
		x0 += vx;
		x1 += vx;
		y0 += vy;
		y1 += vy;
	}
}
