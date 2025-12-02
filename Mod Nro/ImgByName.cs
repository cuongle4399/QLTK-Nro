using System;
using System.Collections;
using UnityEngine;

public class ImgByName
{
	public static MyHashTable hashImagePath = new MyHashTable();

	public static void SetImage(string name, Image img, sbyte nFrame)
	{
		hashImagePath.put(string.Empty + name, new MainImage(img, nFrame));
	}

	public static MainImage getImagePath(string nameImg, MyHashTable hash)
	{
		MainImage mainImage = (MainImage)hash.get(string.Empty + nameImg);
		if (mainImage == null)
		{
			mainImage = new MainImage();
			MainImage fromRms = getFromRms(nameImg);
			if (fromRms != null)
			{
				mainImage.img = fromRms.img;
				mainImage.nFrame = fromRms.nFrame;
			}
			hash.put(string.Empty + nameImg, mainImage);
		}
		mainImage.count = GameCanvas.timeNow / 1000;
		if (mainImage.img == null)
		{
			mainImage.timeImageNull--;
			if (mainImage.timeImageNull <= 0)
			{
				Service.gI().getImgByName(nameImg);
				mainImage.timeImageNull = 200;
			}
		}
		return mainImage;
	}

	public static MainImage getFromRms(string nameImg)
	{
		string text = mGraphics.zoomLevel + "ImgByName_" + nameImg;
		MainImage result = null;
		sbyte[] array = null;
		array = Rms.loadRMS(text);
		if (array == null)
		{
			return result;
		}
		try
		{
			result = new MainImage();
			result.nFrame = array[0];
			result.img = Image.createImage(array, 1, array.Length - 1);
			if (result.img != null)
			{
			}
		}
		catch (Exception)
		{
			Debug.LogError(text + ">>>>>getFromRms: nulllllllllll 2222");
			return null;
		}
		return result;
	}

	public static void saveRMS(string nameImg, sbyte nFrame, sbyte[] data)
	{
		string text = mGraphics.zoomLevel + "ImgByName_" + nameImg;
		DataOutputStream dataOutputStream = new DataOutputStream(data.Length + 1);
		int i = 0;
		try
		{
			dataOutputStream.writeByte(nFrame);
			for (i = 0; i < data.Length; i++)
			{
				dataOutputStream.writeByte(data[i]);
			}
			Rms.saveRMS(text, dataOutputStream.toByteArray());
			dataOutputStream.close();
		}
		catch (Exception ex)
		{
			Debug.LogError(i + ">>Errr save rms: " + text + "  " + ex.ToString());
		}
	}

	public static void checkDelHash(MyHashTable hash, int minute, bool isTrue)
	{
		MyVector myVector = new MyVector("checkDelHash");
		if (isTrue)
		{
			hash.clear();
			return;
		}
		IDictionaryEnumerator enumerator = hash.GetEnumerator();
		while (enumerator.MoveNext())
		{
			MainImage mainImage = (MainImage)enumerator.Value;
			if (GameCanvas.timeNow / 1000 - mainImage.count > minute * 60)
			{
				string o = (string)enumerator.Key;
				myVector.addElement(o);
			}
		}
		for (int i = 0; i < myVector.size(); i++)
		{
			hash.remove((string)myVector.elementAt(i));
		}
	}
}
