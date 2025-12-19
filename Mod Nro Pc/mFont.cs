using System;
using System.Collections;
using UnityEngine;

public class mFont
{
	public static int LEFT;

	public static int RIGHT;

	public static int CENTER;

	public static int RED;

	public static int YELLOW;

	public static int GREEN;

	public static int FATAL;

	public static int MISS;

	public static int ORANGE;

	public static int ADDMONEY;

	public static int MISS_ME;

	public static int FATAL_ME;

	public static int HP;

	public static int MP;

	private int space;

	private Image imgFont;

	private string strFont;

	private int[][] fImages;

	public static int yAddFont;

	public static int[] colorJava;

	public static mFont gI;

	public static mFont tahoma_7b_red;

	public static mFont tahoma_7b_blue;

	public static mFont tahoma_7b_white;

	public static mFont tahoma_7b_yellow;

	public static mFont tahoma_7b_yellowSmall;

	public static mFont tahoma_7b_dark;

	public static mFont tahoma_7b_green2;

	public static mFont tahoma_7b_green;

	public static mFont tahoma_7b_focus;

	public static mFont tahoma_7b_unfocus;

	public static mFont tahoma_7;

	public static mFont tahoma_7_blue1;

	public static mFont tahoma_7_blue1Small;

	public static mFont tahoma_7_green2;

	public static mFont tahoma_7_yellow;

	public static mFont tahoma_7_grey;

	public static mFont tahoma_7_red;

	public static mFont tahoma_7_blue;

	public static mFont tahoma_7_green;

	public static mFont tahoma_7_white;

	public static mFont tahoma_8b;

	public static mFont number_yellow;

	public static mFont number_red;

	public static mFont number_green;

	public static mFont number_gray;

	public static mFont number_orange;

	public static mFont bigNumber_red;

	public static mFont bigNumber_While;

	public static mFont bigNumber_yellow;

	public static mFont bigNumber_green;

	public static mFont bigNumber_orange;

	public static mFont bigNumber_blue;

	public static mFont bigNumber_black;

	public static mFont nameFontRed;

	public static mFont nameFontYellow;

	public static mFont nameFontGreen;

	public static mFont tahoma_7_greySmall;

	public static mFont tahoma_7b_yellowSmall2;

	public static mFont tahoma_7b_green2Small;

	public static mFont tahoma_7_whiteSmall;

	public static mFont tahoma_7b_greenSmall;

	public Font myFont;

	private int height;

	private int wO;

	public Color color1 = Color.white;

	public Color color2 = Color.gray;

	public sbyte id;

	public int fstyle;

	public string st1 = "áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ";

	public string st2 = "\u00b8µ¶·¹\u00a8¾»¼½Æ©ÊÇÈÉËÐÌÎÏÑªÕÒÓÔÖÝ×ØÜÞãßáâä«èåæçé¬íêëìîóïñòô\u00adøõö÷ùýúûüþ®\u00b8µ¶·¹¡¾»¼½Æ¢ÊÇÈÉËÐÌÎÏÑ£ÕÒÓÔÖÝ×ØÜÞãßáâä¤èåæçé¥íêëìîóïñòô¦øõö÷ùýúûüþ§";

	public const string str = " 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW";

	private int yAdd;

	private string pathImage;

	public mFont(string strFont, string pathImage, string pathData, int space)
	{
		try
		{
			this.strFont = strFont;
			this.space = space;
			this.pathImage = pathImage;
			DataInputStream dataInputStream = null;
			reloadImage();
			try
			{
				dataInputStream = MyStream.readFile(pathData);
				fImages = new int[dataInputStream.readShort()][];
				for (int i = 0; i < fImages.Length; i++)
				{
					fImages[i] = new int[4];
					fImages[i][0] = dataInputStream.readShort();
					fImages[i][1] = dataInputStream.readShort();
					fImages[i][2] = dataInputStream.readShort();
					fImages[i][3] = dataInputStream.readShort();
					setHeight(fImages[i][3]);
				}
				dataInputStream.close();
			}
			catch (Exception)
			{
				try
				{
					dataInputStream.close();
				}
				catch (Exception ex2)
				{
					ex2.StackTrace.ToString();
				}
			}
		}
		catch (Exception ex3)
		{
			ex3.StackTrace.ToString();
		}
	}

	public mFont(sbyte id)
	{
		string text = "chelthm";
		if ((id > 0 && id < 10) || id == 19)
		{
			yAdd = 1;
			text = "barmeneb";
		}
		else if (id >= 10 && id <= 18)
		{
			text = "chelthm";
			yAdd = 2;
		}
		else if (id > 24)
		{
			text = "staccato";
		}
		this.id = id;
		text = "FontSys/x" + mGraphics.zoomLevel + "/" + text;
		myFont = (Font)Resources.Load(text);
		if (id < 25)
		{
			color1 = setColorFont(id);
			color2 = setColorFont(id);
		}
		else
		{
			color1 = bigColor(id);
			color2 = bigColor(id);
		}
		wO = getWidthExactOf("o");
	}

	public static void init()
	{
		if (mGraphics.zoomLevel == 1)
		{
			tahoma_7b_red = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_red.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_blue = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_blue.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_white = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_white.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_yellow = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_yellow.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_yellowSmall = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_yellow.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_dark = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_brown.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_green2 = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_green2.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_green = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_green.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_focus = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_focus.png", "/myfont/tahoma_7b", 0);
			tahoma_7b_unfocus = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7b_unfocus.png", "/myfont/tahoma_7b", 0);
			tahoma_7 = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7.png", "/myfont/tahoma_7", 0);
			tahoma_7_blue1 = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_blue1.png", "/myfont/tahoma_7", 0);
			tahoma_7_green2 = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_green2.png", "/myfont/tahoma_7", 0);
			tahoma_7_yellow = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_yellow.png", "/myfont/tahoma_7", 0);
			tahoma_7_grey = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_grey.png", "/myfont/tahoma_7", 0);
			tahoma_7_red = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_red.png", "/myfont/tahoma_7", 0);
			tahoma_7_blue = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_blue.png", "/myfont/tahoma_7", 0);
			tahoma_7_green = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_green.png", "/myfont/tahoma_7", 0);
			tahoma_7_white = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_7_white.png", "/myfont/tahoma_7", 0);
			tahoma_8b = new mFont(" 0123456789+-*='_?.,<>/[]{}!@#$%^&*():aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠĂẰẮẲẴẶÂẤẦẨẪẬBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW", "/myfont/tahoma_8b.png", "/myfont/tahoma_8b", -1);
			number_yellow = new mFont(" 0123456789+-", "/myfont/number_yellow.png", "/myfont/number", 0);
			number_red = new mFont(" 0123456789+-", "/myfont/number_red.png", "/myfont/number", 0);
			number_green = new mFont(" 0123456789+-", "/myfont/number_green.png", "/myfont/number", 0);
			number_gray = new mFont(" 0123456789+-", "/myfont/number_gray.png", "/myfont/number", 0);
			number_orange = new mFont(" 0123456789+-", "/myfont/number_orange.png", "/myfont/number", 0);
			bigNumber_red = number_red;
			bigNumber_While = tahoma_7b_white;
			bigNumber_yellow = number_yellow;
			bigNumber_green = number_green;
			bigNumber_orange = number_orange;
			bigNumber_blue = tahoma_7_blue1;
			nameFontRed = tahoma_7_red;
			nameFontYellow = tahoma_7_yellow;
			nameFontGreen = tahoma_7_green;
			tahoma_7_greySmall = tahoma_7_grey;
			tahoma_7b_yellowSmall2 = tahoma_7_yellow;
			tahoma_7b_green2Small = tahoma_7b_green2;
			tahoma_7_whiteSmall = tahoma_7_white;
			tahoma_7b_greenSmall = tahoma_7b_green;
			tahoma_7_blue1Small = tahoma_7_blue1;
			return;
		}
		gI = new mFont(0);
		tahoma_7b_red = new mFont(1);
		tahoma_7b_blue = new mFont(2);
		tahoma_7b_white = new mFont(3);
		tahoma_7b_yellow = new mFont(4);
		tahoma_7b_yellowSmall = new mFont(4);
		tahoma_7b_dark = new mFont(5);
		tahoma_7b_green2 = new mFont(6);
		tahoma_7b_green = new mFont(7);
		tahoma_7b_focus = new mFont(8);
		tahoma_7b_unfocus = new mFont(9);
		tahoma_7 = new mFont(10);
		tahoma_7_blue1 = new mFont(11);
		tahoma_7_blue1Small = tahoma_7_blue1;
		tahoma_7_green2 = new mFont(12);
		tahoma_7_yellow = new mFont(13);
		tahoma_7_grey = new mFont(14);
		tahoma_7_red = new mFont(15);
		tahoma_7_blue = new mFont(16);
		tahoma_7_green = new mFont(17);
		tahoma_7_white = new mFont(18);
		tahoma_8b = new mFont(19);
		number_yellow = new mFont(20);
		number_red = new mFont(21);
		number_green = new mFont(22);
		number_gray = new mFont(23);
		number_orange = new mFont(24);
		bigNumber_red = new mFont(25);
		bigNumber_yellow = new mFont(26);
		bigNumber_green = new mFont(27);
		bigNumber_While = new mFont(28);
		bigNumber_blue = new mFont(29);
		bigNumber_orange = new mFont(30);
		bigNumber_black = new mFont(31);
		nameFontRed = tahoma_7b_red;
		nameFontYellow = tahoma_7_yellow;
		nameFontGreen = tahoma_7_green;
		tahoma_7_greySmall = tahoma_7_grey;
		tahoma_7b_yellowSmall2 = tahoma_7_yellow;
		tahoma_7b_green2Small = tahoma_7b_green2;
		tahoma_7_whiteSmall = tahoma_7_white;
		tahoma_7b_greenSmall = tahoma_7b_green;
		yAddFont = 1;
		if (mGraphics.zoomLevel == 1)
		{
			yAddFont = -3;
		}
	}

	public void setHeight(int height)
	{
		this.height = height;
	}

	public Color setColor(int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		float num3 = (rgb >> 16) & 0xFF;
		float b = (float)num / 256f;
		float g = (float)num2 / 256f;
		return new Color(num3 / 256f, g, b);
	}

	public Color bigColor(int id)
	{
		return (new Color[7]
		{
			Color.red,
			Color.yellow,
			Color.green,
			Color.white,
			setColor(40404),
			Color.red,
			Color.black
		})[id - 25];
	}

	public void setColorByID(int ID)
	{
		color1 = setColor(colorJava[ID]);
		color2 = setColor(colorJava[ID]);
	}

	public void setTypePaint(mGraphics g, string st, int x, int y, int align, sbyte idFont)
	{
		sbyte colorByID = id;
		if (idFont > 0)
		{
			colorByID = idFont;
		}
		x--;
		if (id > 24)
		{
			Color[] array = new Color[6]
			{
				setColor(6029312),
				setColor(7169025),
				setColor(7680),
				setColor(0),
				setColor(9264),
				setColor(6029312)
			};
			color1 = array[id - 25];
			color2 = array[id - 25];
			_drawString(g, st, x + 1, y, align);
			_drawString(g, st, x - 1, y, align);
			_drawString(g, st, x, y - 1, align);
			_drawString(g, st, x, y + 1, align);
			_drawString(g, st, x + 1, y + 1, align);
			_drawString(g, st, x + 1, y - 1, align);
			_drawString(g, st, x - 1, y - 1, align);
			_drawString(g, st, x - 1, y + 1, align);
			color1 = bigColor(id);
			color2 = bigColor(id);
		}
		else
		{
			setColorByID(colorByID);
		}
		_drawString(g, st, x, y - yAdd, align);
	}

	public Color setColorFont(sbyte id)
	{
		return setColor(colorJava[id]);
	}

	public void drawString(mGraphics g, string st, int x, int y, int align)
	{
		if (mGraphics.zoomLevel == 1)
		{
			int length = st.Length;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			if (1 == 0)
			{
			}
			int num = align switch
			{
				1 => x - getWidth(st), 
				0 => x, 
				_ => x - (getWidth(st) >> 1), 
			};
			if (1 == 0)
			{
			}
			int num2 = num;
			bool flag12 = false;
			int num3 = num2;
			bool flag13 = false;
			int num4 = num3;
			bool flag14 = false;
			int num5 = num4;
			bool flag15 = false;
			int num6 = num5;
			bool flag16 = false;
			int num7 = num6;
			bool flag17 = false;
			int num8 = num7;
			bool flag18 = false;
			int num9 = num8;
			bool flag19 = false;
			int num10 = num9;
			bool flag20 = false;
			int num11 = num10;
			bool flag21 = false;
			int num12 = num11;
			bool flag22 = false;
			int num13 = num12;
			for (int i = 0; i < length; i++)
			{
				int num14 = strFont.IndexOf(st[i] + string.Empty);
				if (num14 == -1)
				{
					num14 = 0;
				}
				if (num14 > -1)
				{
					int x2 = fImages[num14][0];
					int num15 = fImages[num14][1];
					int w = fImages[num14][2];
					int num16 = fImages[num14][3];
					if (num15 + num16 > imgFont.texture.height)
					{
						num15 -= imgFont.texture.height;
						x2 = imgFont.texture.width / 2;
					}
					g.drawRegion(imgFont, x2, num15, w, num16, 0, num13, y, 20);
				}
				num13 += fImages[num14][2] + space;
			}
		}
		else
		{
			setTypePaint(g, st, x, y, align, 0);
		}
	}

	public void drawStringBorder(mGraphics g, string st, int x, int y, int align)
	{
		if (mGraphics.zoomLevel == 1)
		{
			drawString(g, st, x, y, align);
		}
		else
		{
			setTypePaint(g, st, x, y, align, 0);
		}
	}

	public void drawStringBorder(mGraphics g, string st, int x, int y, int align, mFont font2)
	{
		if (mGraphics.zoomLevel == 1)
		{
			drawString(g, st, x, y, align, font2);
		}
		else
		{
			drawStringBd(g, st, x, y, align, font2);
		}
	}

	public void drawStringBd(mGraphics g, string st, int x, int y, int align, mFont font)
	{
		setTypePaint(g, st, x - 1, y - 1, align, 0);
		setTypePaint(g, st, x - 1, y + 1, align, 0);
		setTypePaint(g, st, x + 1, y - 1, align, 0);
		setTypePaint(g, st, x + 1, y + 1, align, 0);
		setTypePaint(g, st, x, y - 1, align, 0);
		setTypePaint(g, st, x, y + 1, align, 0);
		setTypePaint(g, st, x + 1, y, align, 0);
		setTypePaint(g, st, x - 1, y, align, 0);
		setTypePaint(g, st, x, y, align, 0);
	}

	public void drawString(mGraphics g, string st, int x, int y, int align, mFont font)
	{
		if (mGraphics.zoomLevel == 1)
		{
			int length = st.Length;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			if (1 == 0)
			{
			}
			int num = align switch
			{
				1 => x - getWidth(st), 
				0 => x, 
				_ => x - (getWidth(st) >> 1), 
			};
			if (1 == 0)
			{
			}
			int num2 = num;
			bool flag12 = false;
			int num3 = num2;
			bool flag13 = false;
			int num4 = num3;
			bool flag14 = false;
			int num5 = num4;
			bool flag15 = false;
			int num6 = num5;
			bool flag16 = false;
			int num7 = num6;
			bool flag17 = false;
			int num8 = num7;
			bool flag18 = false;
			int num9 = num8;
			bool flag19 = false;
			int num10 = num9;
			bool flag20 = false;
			int num11 = num10;
			bool flag21 = false;
			int num12 = num11;
			bool flag22 = false;
			int num13 = num12;
			for (int i = 0; i < length; i++)
			{
				int num14 = strFont.IndexOf(st[i]);
				if (num14 == -1)
				{
					num14 = 0;
				}
				if (num14 > -1)
				{
					int x2 = fImages[num14][0];
					int num15 = fImages[num14][1];
					int w = fImages[num14][2];
					int num16 = fImages[num14][3];
					if (num15 + num16 > imgFont.texture.height)
					{
						num15 -= imgFont.texture.height;
						x2 = imgFont.texture.width / 2;
					}
					if (!GameCanvas.lowGraphic && font != null)
					{
						g.drawRegion(font.imgFont, x2, num15, w, num16, 0, num13 + 1, y, 20);
						g.drawRegion(font.imgFont, x2, num15, w, num16, 0, num13, y + 1, 20);
					}
					g.drawRegion(imgFont, x2, num15, w, num16, 0, num13, y, 20);
				}
				num13 += fImages[num14][2] + space;
			}
		}
		else
		{
			setTypePaint(g, st, x, y + 1, align, font.id);
			setTypePaint(g, st, x, y, align, 0);
		}
	}

	public MyVector splitFontVector(string src, int lineWidth)
	{
		MyVector myVector = new MyVector();
		string text = string.Empty;
		for (int i = 0; i < src.Length; i++)
		{
			if (src[i] == '\n' || src[i] == '\b')
			{
				myVector.addElement(text);
				text = string.Empty;
				continue;
			}
			text += src[i];
			if (getWidth(text) > lineWidth)
			{
				int num = text.Length - 1;
				while (num >= 0 && text[num] != ' ')
				{
					num--;
				}
				if (num < 0)
				{
					num = text.Length - 1;
				}
				myVector.addElement(text.Substring(0, num));
				i = i - (text.Length - num) + 1;
				text = string.Empty;
			}
			if (i == src.Length - 1 && !text.Trim().Equals(string.Empty))
			{
				myVector.addElement(text);
			}
		}
		return myVector;
	}

	public string splitFirst(string str)
	{
		string text = string.Empty;
		bool flag = false;
		for (int i = 0; i < str.Length; i++)
		{
			if (!flag)
			{
				string text2 = str.Substring(i);
				text = ((!compare(text2, " ")) ? (text + text2) : (text + str[i] + "-"));
				flag = true;
			}
			else if (str[i] == ' ')
			{
				flag = false;
			}
		}
		return text;
	}

	public string[] splitStrInLine(string src, int lineWidth)
	{
		ArrayList arrayList = splitStrInLineA(src, lineWidth);
		string[] array = new string[arrayList.Count];
		for (int i = 0; i < arrayList.Count; i++)
		{
			array[i] = (string)arrayList[i];
		}
		return array;
	}

	public ArrayList splitStrInLineA(string src, int lineWidth)
	{
		ArrayList arrayList = new ArrayList();
		int i = 0;
		int num = 0;
		int length = src.Length;
		if (length < 5)
		{
			arrayList.Add(src);
			return arrayList;
		}
		string text = string.Empty;
		try
		{
			while (true)
			{
				if (getWidthNotExactOf(text) < lineWidth)
				{
					text += src[num];
					num++;
					if (src[num] != '\n')
					{
						if (num < length - 1)
						{
							continue;
						}
						num = length - 1;
					}
				}
				if (num != length - 1 && src[num + 1] != ' ')
				{
					int num2 = num;
					while (src[num + 1] != '\n' && (src[num + 1] != ' ' || src[num] == ' ') && num != i)
					{
						num--;
					}
					if (num == i)
					{
						num = num2;
					}
				}
				string text2 = src.Substring(i, num + 1 - i);
				if (text2[0] == '\n')
				{
					text2 = text2.Substring(1, text2.Length - 1);
				}
				if (text2[text2.Length - 1] == '\n')
				{
					text2 = text2.Substring(0, text2.Length - 1);
				}
				arrayList.Add(text2);
				if (num != length - 1)
				{
					for (i = num + 1; i != length - 1 && src[i] == ' '; i++)
					{
					}
					if (i != length - 1)
					{
						num = i;
						text = string.Empty;
						continue;
					}
					break;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			Cout.LogWarning("EXCEPTION WHEN REAL SPLIT " + src + "\nend=" + num + "\n" + ex.Message + "\n" + ex.StackTrace);
			arrayList.Add(src);
		}
		return arrayList;
	}

	public string[] splitFontArray(string src, int lineWidth)
	{
		MyVector myVector = splitFontVector(src, lineWidth);
		string[] array = new string[myVector.size()];
		for (int i = 0; i < myVector.size(); i++)
		{
			array[i] = (string)myVector.elementAt(i);
		}
		return array;
	}

	public bool compare(string strSource, string str)
	{
		for (int i = 0; i < strSource.Length; i++)
		{
			if ((string.Empty + strSource[i]).Equals(str))
			{
				return true;
			}
		}
		return false;
	}

	public int getWidth(string s)
	{
		if (mGraphics.zoomLevel == 1)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				int num2 = strFont.IndexOf(s[i]);
				if (num2 == -1)
				{
					num2 = 0;
				}
				num += fImages[num2][2] + space;
			}
			return num;
		}
		return getWidthExactOf(s);
	}

	public int getWidthExactOf(string s)
	{
		try
		{
			return (int)new GUIStyle
			{
				font = myFont
			}.CalcSize(new GUIContent(s)).x / mGraphics.zoomLevel;
		}
		catch (Exception ex)
		{
			Cout.LogError("GET WIDTH OF " + s + " FAIL.\n" + ex.Message + "\n" + ex.StackTrace);
			return getWidthNotExactOf(s);
		}
	}

	public int getWidthNotExactOf(string s)
	{
		return s.Length * wO / mGraphics.zoomLevel;
	}

	public int getHeight()
	{
		if (mGraphics.zoomLevel == 1)
		{
			return height;
		}
		if (height > 0)
		{
			return height / mGraphics.zoomLevel;
		}
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.font = myFont;
		try
		{
			height = (int)gUIStyle.CalcSize(new GUIContent("Adg")).y + 2;
		}
		catch (Exception ex)
		{
			Cout.LogError("FAIL GET HEIGHT " + ex.StackTrace);
			height = 20;
		}
		return height / mGraphics.zoomLevel;
	}

	public void _drawString(mGraphics g, string st, int x0, int y0, int align)
	{
		y0 += yAddFont;
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.font = myFont;
		float num = 0f;
		float num2 = 0f;
		switch (align)
		{
		case 0:
			num = x0;
			num2 = y0;
			gUIStyle.alignment = TextAnchor.UpperLeft;
			break;
		case 1:
			num = x0 - GameCanvas.w;
			num2 = y0;
			gUIStyle.alignment = TextAnchor.UpperRight;
			break;
		case 2:
		case 3:
			num = x0 - GameCanvas.w / 2;
			num2 = y0;
			gUIStyle.alignment = TextAnchor.UpperCenter;
			break;
		}
		gUIStyle.normal.textColor = color1;
		g.drawString(st, (int)num, (int)num2, gUIStyle);
	}

	public static string[] splitStringSv(string _text, string _searchStr)
	{
		int num = 0;
		int startIndex = 0;
		int length = _searchStr.Length;
		int num2 = _text.IndexOf(_searchStr, startIndex);
		while (num2 != -1)
		{
			startIndex = num2 + length;
			num2 = _text.IndexOf(_searchStr, startIndex);
			num++;
		}
		string[] array = new string[num + 1];
		int num3 = _text.IndexOf(_searchStr);
		int num4 = 0;
		int num5 = 0;
		while (num3 != -1)
		{
			array[num5] = _text.Substring(num4, num3 - num4);
			num4 = num3 + length;
			num3 = _text.IndexOf(_searchStr, num4);
			num5++;
		}
		array[num5] = _text.Substring(num4, _text.Length - num4);
		return array;
	}

	public void reloadImage()
	{
		if (mGraphics.zoomLevel == 1)
		{
			imgFont = GameCanvas.loadImage(pathImage);
		}
	}

	public void freeImage()
	{
	}

	static mFont()
	{
		LEFT = 0;
		RIGHT = 1;
		CENTER = 2;
		RED = 0;
		YELLOW = 1;
		GREEN = 2;
		FATAL = 3;
		MISS = 4;
		ORANGE = 5;
		ADDMONEY = 6;
		MISS_ME = 7;
		FATAL_ME = 8;
		HP = 9;
		MP = 10;
		colorJava = new int[31]
		{
			0, 16711680, 6520319, 16777215, 16776960, 5449989, 21285, 52224, 7386228, 16771788,
			0, 65535, 21285, 16776960, 5592405, 16742263, 33023, 8701737, 15723503, 7999781,
			16776960, 14961237, 4124899, 4671303, 16096312, 16711680, 16776960, 52224, 16777215, 6520319,
			16096312
		};
	}

	public void drawStringSmaller(mGraphics g, string st, int x, int y, int align, mFont font)
	{
		if (mGraphics.zoomLevel == 1)
		{
			int length = st.Length;
			int num = ((align == LEFT) ? x : ((align != RIGHT) ? (x - (int)((float)getWidth(st) * 0.85f / 2f)) : (x - (int)((float)getWidth(st) * 0.85f))));
			for (int i = 0; i < length; i++)
			{
				int num2 = strFont.IndexOf(st[i].ToString());
				if (num2 == -1)
				{
					num2 = 0;
				}
				if (num2 > -1)
				{
					int x2 = fImages[num2][0];
					int num3 = fImages[num2][1];
					float num4 = fImages[num2][2];
					int num5 = fImages[num2][3];
					if (num3 + num5 > imgFont.texture.height)
					{
						num3 -= imgFont.texture.height;
						x2 = imgFont.texture.width / 2;
					}
					int w = (int)(num4 * 0.85f);
					int h = (int)((float)num5 * 0.85f);
					if (!GameCanvas.lowGraphic && font != null && font.imgFont != null)
					{
						g.drawRegion(font.imgFont, x2, num3, w, h, 0, (float)num + 0.85f, y, 20);
						g.drawRegion(font.imgFont, x2, num3, w, h, 0, num, (float)y + 0.85f, 20);
					}
					g.drawRegion(imgFont, x2, num3, w, h, 0, num, y, 20);
					num += (int)((float)fImages[num2][2] * 0.85f) + space;
				}
			}
		}
		else
		{
			Matrix4x4 matrix = GUI.matrix;
			GUIUtility.ScaleAroundPivot(new Vector2(0.85f, 0.85f), new Vector2(x, y));
			if (!GameCanvas.lowGraphic && font != null)
			{
				font.setTypePaint(g, st, (int)((float)x + 0.85f), (int)((float)y + 0.85f), align, font.id);
				font.setTypePaint(g, st, x, (int)((float)y + 0.85f), align, font.id);
			}
			setTypePaint(g, st, x, y, align, 0);
			GUI.matrix = matrix;
		}
	}

	public void drawStringSmaller(mGraphics g, string st, int x, int y, int align)
	{
		if (mGraphics.zoomLevel == 1)
		{
			int length = st.Length;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			if (1 == 0)
			{
			}
			int num = align switch
			{
				1 => x - (int)((float)getWidth(st) * 0.85f), 
				0 => x, 
				_ => x - (int)((float)getWidth(st) * 0.85f / 2f), 
			};
			if (1 == 0)
			{
			}
			int num2 = num;
			bool flag12 = false;
			int num3 = num2;
			bool flag13 = false;
			int num4 = num3;
			bool flag14 = false;
			int num5 = num4;
			bool flag15 = false;
			int num6 = num5;
			bool flag16 = false;
			int num7 = num6;
			bool flag17 = false;
			int num8 = num7;
			bool flag18 = false;
			int num9 = num8;
			bool flag19 = false;
			int num10 = num9;
			bool flag20 = false;
			int num11 = num10;
			bool flag21 = false;
			int num12 = num11;
			bool flag22 = false;
			int num13 = num12;
			for (int i = 0; i < length; i++)
			{
				int num14 = strFont.IndexOf(st[i] + string.Empty);
				if (num14 == -1)
				{
					num14 = 0;
				}
				if (num14 > -1)
				{
					int x2 = fImages[num14][0];
					int num15 = fImages[num14][1];
					int num16 = fImages[num14][2];
					int num17 = fImages[num14][3];
					if (num15 + num17 > imgFont.texture.height)
					{
						num15 -= imgFont.texture.height;
						x2 = imgFont.texture.width / 2;
					}
					int w = (int)((float)num16 * 0.85f);
					int h = (int)((float)num17 * 0.85f);
					g.drawRegion(imgFont, x2, num15, w, h, 0, num13, y, 20);
					num13 += (int)((float)fImages[num14][2] * 0.85f) + space;
				}
			}
		}
		else
		{
			Matrix4x4 matrix = GUI.matrix;
			GUIUtility.ScaleAroundPivot(new Vector2(0.85f, 0.85f), new Vector2(x, y));
			setTypePaint(g, st, x, y, align, 0);
			GUI.matrix = matrix;
		}
	}
}
