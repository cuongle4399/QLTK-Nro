using System;
using System.Text;
using UnityEngine;

public class myReader
{
	public sbyte[] buffer;

	private int posRead;

	private int posMark;

	private static string fileName;

	private static int status;

	public myReader()
	{
	}

	public myReader(sbyte[] data)
	{
		buffer = data;
	}

	public myReader(string filename)
	{
		TextAsset textAsset = (TextAsset)Resources.Load(filename, typeof(TextAsset));
		buffer = mSystem.convertToSbyte(textAsset.bytes);
	}

	public sbyte readSByte()
	{
		if (posRead < buffer.Length)
		{
			return buffer[posRead++];
		}
		posRead = buffer.Length;
		throw new Exception(" loi doc sbyte eof ");
	}

	public sbyte readByte()
	{
		return readSByte();
	}

	public void mark(int readlimit)
	{
		posMark = posRead;
	}

	public void reset()
	{
		posRead = posMark;
	}

	public byte readUnsignedByte()
	{
		return convertSbyteToByte(readSByte());
	}

	public short readShort()
	{
		short num = 0;
		for (int i = 0; i < 2; i++)
		{
			num <<= 8;
			num |= (short)(0xFF & buffer[posRead++]);
		}
		return num;
	}

	public ushort readUnsignedShort()
	{
		ushort num = 0;
		for (int i = 0; i < 2; i++)
		{
			num <<= 8;
			num |= (ushort)(0xFF & buffer[posRead++]);
		}
		return num;
	}

	public int readInt()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			num <<= 8;
			num |= 0xFF & buffer[posRead++];
		}
		return num;
	}

	public long readLong()
	{
		long num = 0L;
		for (int i = 0; i < 8; i++)
		{
			num <<= 8;
			num |= 0xFF & buffer[posRead++];
		}
		return num;
	}

	public bool readBool()
	{
		return readSByte() > 0;
	}

	public bool readBoolean()
	{
		return readSByte() > 0;
	}

	public string readString()
	{
		short num = readShort();
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = convertSbyteToByte(readSByte());
		}
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(array);
	}

	public string readStringUTF()
	{
		short num = readShort();
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = convertSbyteToByte(readSByte());
		}
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(array);
	}

	public string readUTF()
	{
		return readStringUTF();
	}

	public int read()
	{
		if (posRead < buffer.Length)
		{
			return readSByte();
		}
		return -1;
	}

	public int read(ref sbyte[] data)
	{
		if (data == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = readSByte();
			if (posRead > buffer.Length)
			{
				return -1;
			}
			num++;
		}
		return num;
	}

	public void readFully(ref sbyte[] data)
	{
		if (data != null && data.Length + posRead <= buffer.Length)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = readSByte();
			}
		}
	}

	public int available()
	{
		return buffer.Length - posRead;
	}

	public static byte convertSbyteToByte(sbyte var)
	{
		if (var > 0)
		{
			return (byte)var;
		}
		return (byte)(var + 256);
	}

	public static byte[] convertSbyteToByte(sbyte[] var)
	{
		byte[] array = new byte[var.Length];
		for (int i = 0; i < var.Length; i++)
		{
			if (var[i] > 0)
			{
				array[i] = (byte)var[i];
			}
			else
			{
				array[i] = (byte)(var[i] + 256);
			}
		}
		return array;
	}

	public void Close()
	{
		buffer = null;
	}

	public void close()
	{
		buffer = null;
	}

	public void read(ref sbyte[] data, int arg1, int arg2)
	{
		if (data == null)
		{
			return;
		}
		for (int i = 0; i < arg2; i++)
		{
			data[i + arg1] = readSByte();
			if (posRead > buffer.Length)
			{
				break;
			}
		}
	}
}
