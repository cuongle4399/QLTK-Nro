using System;

public class MyRandom
{
	public Random r;

	public MyRandom()
	{
		r = new Random();
	}

	public int nextInt()
	{
		return r.Next();
	}

	public int nextInt(int a)
	{
		return r.Next(a);
	}

	public int nextInt(int a, int b)
	{
		return r.Next(a, b);
	}
}
