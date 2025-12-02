public class InputStream : myReader
{
	public InputStream()
	{
	}

	public InputStream(sbyte[] data)
	{
		buffer = data;
	}

	public InputStream(string filename)
		: base(filename)
	{
	}
}
