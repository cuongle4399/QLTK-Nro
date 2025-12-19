public class Item
{
	public const int OPT_STAR = 34;

	public const int OPT_MOON = 35;

	public const int OPT_SUN = 36;

	public const int OPT_COLORNAME = 41;

	public const int OPT_LVITEM = 72;

	public const int OPT_STARSLOT = 102;

	public const int OPT_MAXSTARSLOT = 107;

	public const int TYPE_BODY_MIN = 0;

	public const int TYPE_BODY_MAX = 6;

	public const int TYPE_AO = 0;

	public const int TYPE_QUAN = 1;

	public const int TYPE_GANGTAY = 2;

	public const int TYPE_GIAY = 3;

	public const int TYPE_RADA = 4;

	public const int TYPE_HAIR = 5;

	public const int TYPE_DAUTHAN = 6;

	public const int TYPE_NGOCRONG = 12;

	public const int TYPE_SACH = 7;

	public const int TYPE_NHIEMVU = 8;

	public const int TYPE_GOLD = 9;

	public const int TYPE_DIAMOND = 10;

	public const int TYPE_BALO = 11;

	public const int TYPE_MOUNT = 23;

	public const int TYPE_MOUNT_VIP = 24;

	public const int TYPE_DIAMOND_LOCK = 34;

	public const int TYPE_TRAINSUIT = 32;

	public const int TYPE_HAT = 35;

	public const sbyte UI_WEAPON = 2;

	public const sbyte UI_BAG = 3;

	public const sbyte UI_BOX = 4;

	public const sbyte UI_BODY = 5;

	public const sbyte UI_STACK = 6;

	public const sbyte UI_STACK_LOCK = 7;

	public const sbyte UI_GROCERY = 8;

	public const sbyte UI_GROCERY_LOCK = 9;

	public const sbyte UI_UPGRADE = 10;

	public const sbyte UI_UPPEARL = 11;

	public const sbyte UI_UPPEARL_LOCK = 12;

	public const sbyte UI_SPLIT = 13;

	public const sbyte UI_STORE = 14;

	public const sbyte UI_BOOK = 15;

	public const sbyte UI_LIEN = 16;

	public const sbyte UI_NHAN = 17;

	public const sbyte UI_NGOCBOI = 18;

	public const sbyte UI_PHU = 19;

	public const sbyte UI_NONNAM = 20;

	public const sbyte UI_NONNU = 21;

	public const sbyte UI_AONAM = 22;

	public const sbyte UI_AONU = 23;

	public const sbyte UI_GANGTAYNAM = 24;

	public const sbyte UI_GANGTAYNU = 25;

	public const sbyte UI_QUANNAM = 26;

	public const sbyte UI_QUANNU = 27;

	public const sbyte UI_GIAYNAM = 28;

	public const sbyte UI_GIAYNU = 29;

	public const sbyte UI_TRADE = 30;

	public const sbyte UI_UPGRADE_GOLD = 31;

	public const sbyte UI_FASHION = 32;

	public const sbyte UI_CONVERT = 33;

	public ItemOption[] itemOption;

	public ItemTemplate template;

	public MyVector options;

	public int itemId;

	public int playerId;

	public bool isSelect;

	public int indexUI;

	public int quantity;

	public int quantilyToBuy;

	public long powerRequire;

	public bool isLock;

	public int sys;

	public int upgrade;

	public int buyCoin;

	public int buyCoinLock;

	public int buyGold;

	public int buyGoldLock;

	public int saleCoinLock;

	public int buySpec;

	public int buyRuby;

	public short iconSpec = -1;

	public sbyte buyType = -1;

	public int typeUI;

	public bool isExpires;

	public bool isBuySpec;

	public EffectCharPaint eff;

	public int indexEff;

	public Image img;

	public string info;

	public string content;

	public string reason = string.Empty;

	public int compare;

	public sbyte isMe;

	public bool newItem;

	public int headTemp = -1;

	public int bodyTemp = -1;

	public int legTemp = -1;

	public int bagTemp = -1;

	public int wpTemp = -1;

	public string nameNguoiKyGui = string.Empty;

	private int[] color = new int[18]
	{
		0, 0, 0, 0, 600841, 600841, 667658, 667658, 3346944, 3346688,
		4199680, 5052928, 3276851, 3932211, 4587571, 5046280, 6684682, 3359744
	};

	private int[][] colorBorder = new int[5][]
	{
		new int[6] { 18687, 16869, 15052, 13235, 11161, 9344 },
		new int[6] { 45824, 39168, 32768, 26112, 19712, 13056 },
		new int[6] { 16744192, 15037184, 13395456, 11753728, 10046464, 8404992 },
		new int[6] { 13500671, 12058853, 10682572, 9371827, 7995545, 6684800 },
		new int[6] { 16711705, 15007767, 13369364, 11730962, 10027023, 8388621 }
	};

	private int[] size = new int[6] { 2, 1, 1, 1, 1, 1 };

	public void getCompare()
	{
		compare = GameCanvas.panel.getCompare(this);
	}

	public string getPrice()
	{
		string result = string.Empty;
		if (buyCoin <= 0 && buyGold <= 0)
		{
			return null;
		}
		if (buyCoin > 0 && buyGold <= 0)
		{
			result = buyCoin + mResources.XU;
		}
		else if (buyGold > 0 && buyCoin <= 0)
		{
			result = buyGold + mResources.LUONG;
		}
		else if (buyCoin > 0 && buyGold > 0)
		{
			result = buyCoin + mResources.XU + "/" + buyGold + mResources.LUONG;
		}
		return result;
	}

	public void paintUpgradeEffect(int x, int y, int upgrade, mGraphics g)
	{
		int num = GameScr.indexSize - 2;
		int num2 = 0;
		int num3 = ((upgrade >= 4) ? ((upgrade < 8) ? 1 : ((upgrade < 12) ? 2 : ((upgrade > 14) ? 4 : 3))) : 0);
		for (int i = num2; i < size.Length; i++)
		{
			int num4 = x - num / 2 + upgradeEffectX(GameCanvas.gameTick - i * 4);
			int num5 = y - num / 2 + upgradeEffectY(GameCanvas.gameTick - i * 4);
			g.setColor(colorBorder[num3][i]);
			g.fillRect(num4 - size[i] / 2, num5 - size[i] / 2, size[i], size[i]);
		}
		if (upgrade == 4 || upgrade == 8)
		{
			for (int j = num2; j < size.Length; j++)
			{
				int num6 = x - num / 2 + upgradeEffectX(GameCanvas.gameTick - num * 2 - j * 4);
				int num7 = y - num / 2 + upgradeEffectY(GameCanvas.gameTick - num * 2 - j * 4);
				g.setColor(colorBorder[num3 - 1][j]);
				g.fillRect(num6 - size[j] / 2, num7 - size[j] / 2, size[j], size[j]);
			}
		}
		if (upgrade != 1 && upgrade != 4 && upgrade != 8)
		{
			for (int k = num2; k < size.Length; k++)
			{
				int num8 = x - num / 2 + upgradeEffectX(GameCanvas.gameTick - num * 2 - k * 4);
				int num9 = y - num / 2 + upgradeEffectY(GameCanvas.gameTick - num * 2 - k * 4);
				g.setColor(colorBorder[num3][k]);
				g.fillRect(num8 - size[k] / 2, num9 - size[k] / 2, size[k], size[k]);
			}
		}
		if (upgrade != 1 && upgrade != 4 && upgrade != 8 && upgrade != 12 && upgrade != 2 && upgrade != 5 && upgrade != 9)
		{
			for (int l = num2; l < size.Length; l++)
			{
				int num10 = x - num / 2 + upgradeEffectX(GameCanvas.gameTick - num - l * 4);
				int num11 = y - num / 2 + upgradeEffectY(GameCanvas.gameTick - num - l * 4);
				g.setColor(colorBorder[num3][l]);
				g.fillRect(num10 - size[l] / 2, num11 - size[l] / 2, size[l], size[l]);
			}
		}
		if (upgrade != 1 && upgrade != 4 && upgrade != 8 && upgrade != 12 && upgrade != 2 && upgrade != 5 && upgrade != 9 && upgrade != 13 && upgrade != 3 && upgrade != 6 && upgrade != 10 && upgrade != 15)
		{
			for (int m = num2; m < size.Length; m++)
			{
				int num12 = x - num / 2 + upgradeEffectX(GameCanvas.gameTick - num * 3 - m * 4);
				int num13 = y - num / 2 + upgradeEffectY(GameCanvas.gameTick - num * 3 - m * 4);
				g.setColor(colorBorder[num3][m]);
				g.fillRect(num12 - size[m] / 2, num13 - size[m] / 2, size[m], size[m]);
			}
		}
	}

	private int upgradeEffectY(int tick)
	{
		int num = GameScr.indexSize - 2;
		int num2 = tick % (4 * num);
		if (0 <= num2 && num2 < num)
		{
			return 0;
		}
		if (num <= num2 && num2 < num * 2)
		{
			return num2 % num;
		}
		if (num * 2 <= num2 && num2 < num * 3)
		{
			return num;
		}
		return num - num2 % num;
	}

	private int upgradeEffectX(int tick)
	{
		int num = GameScr.indexSize - 2;
		int num2 = tick % (4 * num);
		if (0 <= num2 && num2 < num)
		{
			return num2 % num;
		}
		if (num <= num2 && num2 < num * 2)
		{
			return num;
		}
		if (num * 2 <= num2 && num2 < num * 3)
		{
			return num - num2 % num;
		}
		return 0;
	}

	public bool isHaveOption(int id)
	{
		for (int i = 0; i < this.itemOption.Length; i++)
		{
			ItemOption itemOption = this.itemOption[i];
			if (itemOption != null && itemOption.optionTemplate.id == id)
			{
				return true;
			}
		}
		return false;
	}

	public Item clone()
	{
		Item item = new Item();
		item.template = template;
		if (options != null)
		{
			item.options = new MyVector();
			for (int i = 0; i < options.size(); i++)
			{
				ItemOption itemOption = new ItemOption();
				itemOption.optionTemplate = ((ItemOption)options.elementAt(i)).optionTemplate;
				itemOption.param = ((ItemOption)options.elementAt(i)).param;
				item.options.addElement(itemOption);
			}
		}
		item.itemId = itemId;
		item.playerId = playerId;
		item.indexUI = indexUI;
		item.quantity = quantity;
		item.isLock = isLock;
		item.sys = sys;
		item.upgrade = upgrade;
		item.buyCoin = buyCoin;
		item.buyCoinLock = buyCoinLock;
		item.buyGold = buyGold;
		item.buyGoldLock = buyGoldLock;
		item.saleCoinLock = saleCoinLock;
		item.typeUI = typeUI;
		item.isExpires = isExpires;
		return item;
	}

	public bool isTypeBody()
	{
		if ((0 > template.type || template.type >= 6) && template.type != 32 && template.type != 35 && template.type != 11)
		{
			return template.type == 23;
		}
		return true;
	}

	public string getLockstring()
	{
		if (isLock)
		{
			return mResources.LOCKED;
		}
		return mResources.NOLOCK;
	}

	public string getUpgradestring()
	{
		if (template.level < 10 || template.type >= 10)
		{
			return mResources.NOTUPGRADE;
		}
		if (upgrade == 0)
		{
			return mResources.NOUPGRADE;
		}
		return null;
	}

	public bool isTypeUIMe()
	{
		if (typeUI != 5 && typeUI != 3)
		{
			return typeUI == 4;
		}
		return true;
	}

	public bool isTypeUIShopView()
	{
		if (!isTypeUIShop())
		{
			return isTypeUIStore() || isTypeUIBook() || isTypeUIFashion();
		}
		return true;
	}

	public bool isTypeUIShop()
	{
		if (typeUI != 20 && typeUI != 21 && typeUI != 22 && typeUI != 23 && typeUI != 24 && typeUI != 25 && typeUI != 26 && typeUI != 27 && typeUI != 28 && typeUI != 29 && typeUI != 16 && typeUI != 17 && typeUI != 18 && typeUI != 19 && typeUI != 2 && typeUI != 6)
		{
			return typeUI == 8;
		}
		return true;
	}

	public bool isTypeUIShopLock()
	{
		if (typeUI != 7)
		{
			return typeUI == 9;
		}
		return true;
	}

	public bool isTypeUIStore()
	{
		return typeUI == 14;
	}

	public bool isTypeUIBook()
	{
		return typeUI == 15;
	}

	public bool isTypeUIFashion()
	{
		return typeUI == 32;
	}

	public bool isUpMax()
	{
		return getUpMax() == upgrade;
	}

	public int getUpMax()
	{
		if (template.level >= 1 && template.level < 20)
		{
			return 4;
		}
		if (template.level >= 20 && template.level < 40)
		{
			return 8;
		}
		if (template.level >= 40 && template.level < 50)
		{
			return 12;
		}
		if (template.level >= 50 && template.level < 60)
		{
			return 14;
		}
		return 16;
	}

	public void setPartTemp(int headTemp, int bodyTemp, int legTemp, int bagTemp)
	{
		this.headTemp = headTemp;
		this.bodyTemp = bodyTemp;
		this.legTemp = legTemp;
		this.bagTemp = bagTemp;
	}

	public string getFullName()
	{
		string text = template.name;
		if (itemOption != null)
		{
			for (int i = 0; i < itemOption.Length; i++)
			{
				if (itemOption[i].optionTemplate.id == 72)
				{
					text = text + " [+" + itemOption[i].param + "]";
					break;
				}
			}
		}
		if (itemOption != null)
		{
			for (int j = 0; j < itemOption.Length; j++)
			{
				if (itemOption[j].optionTemplate.name.StartsWith("$"))
				{
					string optiongColor = itemOption[j].getOptiongColor();
					if (itemOption[j].param == 1)
					{
						text = text + "\n" + optiongColor;
					}
					if (itemOption[j].param == 0)
					{
						text = text + "\n" + optiongColor;
					}
				}
				else
				{
					string optionString = itemOption[j].getOptionString();
					if (!optionString.Equals(string.Empty) && itemOption[j].optionTemplate.id != 72)
					{
						text = text + "\n" + optionString;
					}
				}
			}
		}
		if (template.strRequire > 1)
		{
			text = text + "\n" + mResources.pow_request + ": " + template.strRequire;
		}
		return text + "\n" + template.description;
	}

	private int method_0(int int_0)
	{
		int num = 32;
		int num2 = int_0 % 128;
		if (0 <= num2 && num2 < num)
		{
			return num2 % num;
		}
		if (num <= num2 && num2 < num * 2)
		{
			return num;
		}
		if (num * 2 <= num2 && num2 < num * 3)
		{
			return num - num2 % num;
		}
		return 0;
	}

	private int method_1(int int_0)
	{
		int num = 22;
		int num2 = int_0 % 88;
		if (0 <= num2 && num2 < num)
		{
			return 0;
		}
		if (num <= num2 && num2 < num * 2)
		{
			return num2 % num;
		}
		if (num * 2 <= num2 && num2 < num * 3)
		{
			return num;
		}
		return num - num2 % num;
	}

	public void paintItemEffect(mGraphics g, int x, int y, int index)
	{
		if (index > 7)
		{
			index = 7;
		}
		if (index < 0)
		{
			index = 0;
		}
		int[] array = new int[7] { 0, 0, 1, 1, 2, 3, 4 };
		int[] array2 = new int[8] { 0, 0, 0, 0, 600841, 3346944, 3932211, 6684682 };
		int[] array3 = new int[4] { 0, 68, 68, 0 };
		int[] array4 = new int[4] { 0, 48, 0, 48 };
		if (index >= 4)
		{
			g.setColor(array2[index]);
			g.fillRect(x - 18, y - 12, 34, 23);
		}
		if (index < 4)
		{
			switch (index)
			{
			case 1:
			{
				for (int m = 0; m < 2; m++)
				{
					for (int n = 0; n < size.Length; n++)
					{
						int num5 = x - 17 + method_0(GameCanvas.gameTick + array3[m] - n * 4);
						int num6 = y - 12 + method_1(GameCanvas.gameTick + array4[m] - n * 4);
						g.setColor(colorBorder[array[index - 1]][n]);
						g.fillRect(num5 - size[n] / 2, num6 - size[n] / 2, size[n], size[n]);
					}
				}
				return;
			}
			default:
			{
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < size.Length; j++)
					{
						int num = x - 17 + method_0(GameCanvas.gameTick + array3[i] - j * 4);
						int num2 = y - 12 + method_1(GameCanvas.gameTick + array4[i] - j * 4);
						g.setColor(colorBorder[0][j]);
						g.fillRect(num - size[j] / 2, num2 - size[j] / 2, size[j], size[j]);
					}
				}
				for (int k = 2; k < 4; k++)
				{
					for (int l = 0; l < size.Length; l++)
					{
						int num3 = x - 17 + method_0(GameCanvas.gameTick + array3[k] - l * 4);
						int num4 = y - 12 + method_1(GameCanvas.gameTick + array4[k] - l * 4);
						g.setColor(colorBorder[1][l]);
						g.fillRect(num3 - size[l] / 2, num4 - size[l] / 2, size[l], size[l]);
					}
				}
				return;
			}
			case 2:
				break;
			}
		}
		for (int num7 = 0; num7 < 4; num7++)
		{
			for (int num8 = 0; num8 < size.Length; num8++)
			{
				int num9 = x - 17 + method_0(GameCanvas.gameTick + array3[num7] - num8 * 4);
				int num10 = y - 12 + method_1(GameCanvas.gameTick + array4[num7] - num8 * 4);
				g.setColor(colorBorder[array[index - 1]][num8]);
				g.fillRect(num9 - size[num8] / 2, num10 - size[num8] / 2, size[num8], size[num8]);
			}
		}
	}
}
