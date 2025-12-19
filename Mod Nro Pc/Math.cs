using System;

public class Math
{
	public const double PI = System.Math.PI;

	public static int Abs(int i)
	{
		return (i <= 0) ? (-i) : i;
	}

	public static int Min(int x, int y)
	{
		return (x >= y) ? y : x;
	}

	public static int Max(int x, int y)
	{
		return (x <= y) ? y : x;
	}

	public static int Pow(int data, int x)
	{
		int num = 1;
		for (int i = 0; i < x; i++)
		{
			num *= data;
		}
		return num;
	}
}
