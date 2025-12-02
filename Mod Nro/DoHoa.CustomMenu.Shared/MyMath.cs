namespace DoHoa.CustomMenu.Shared;

public static class MyMath
{
	public static int Min(int a, int b)
	{
		return (a < b) ? a : b;
	}

	public static int Max(int a, int b)
	{
		return (a > b) ? a : b;
	}

	public static int Abs(int x)
	{
		return (x < 0) ? (-x) : x;
	}

	public static int Clamp(int value, int min, int max)
	{
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}
}
