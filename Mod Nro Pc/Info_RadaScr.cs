public class Info_RadaScr
{
	public const sbyte TYPE_MONSTER = 0;

	public const sbyte TYPE_CHARPART = 1;

	public sbyte rank;

	public sbyte amount;

	public sbyte max_amount;

	public sbyte typeMonster;

	public int id;

	public int no;

	public int idIcon;

	public string name;

	public string info;

	public sbyte level;

	public sbyte isUse;

	public Char charInfo;

	public Mob mobInfo;

	public ItemOption[] itemOption;

	private int[] f = new int[10] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };

	private int count;

	private long timeRequest;

	public ChatPopup cp;

	public MyVector eff = new MyVector(string.Empty);

	public void SetInfo(int id, int no, int idIcon, sbyte rank, sbyte typeMonster, short templateId, string name, string info, Char charInfo, ItemOption[] itemOption)
	{
		this.id = id;
		this.no = no;
		this.idIcon = idIcon;
		this.rank = rank;
		this.typeMonster = typeMonster;
		if (templateId != -1)
		{
			mobInfo = new Mob();
			mobInfo.templateId = templateId;
		}
		this.name = name;
		this.info = info;
		this.charInfo = charInfo;
		this.itemOption = itemOption;
		addItemDetail();
	}

	public void SetAmount(sbyte amount, sbyte max_amount)
	{
		this.amount = amount;
		this.max_amount = max_amount;
	}

	public void SetLevel(sbyte level)
	{
		this.level = level;
		addItemDetail();
	}

	public void SetUse(sbyte isUse)
	{
		this.isUse = isUse;
		addItemDetail();
	}

	public static Char SetCharInfo(int head, int body, int leg, int bag)
	{
		Char obj = new Char();
		obj.head = head;
		obj.body = body;
		obj.leg = leg;
		obj.bag = bag;
		return obj;
	}

	public static Info_RadaScr GetInfo(MyVector vec, int id)
	{
		if (vec != null)
		{
			for (int i = 0; i < vec.size(); i++)
			{
				Info_RadaScr info_RadaScr = (Info_RadaScr)vec.elementAt(i);
				if (info_RadaScr != null && info_RadaScr.id == id)
				{
					return info_RadaScr;
				}
			}
		}
		return null;
	}

	public void paintInfo(mGraphics g, int x, int y)
	{
		count++;
		if (count > f.Length - 1)
		{
			count = 0;
		}
		if (typeMonster == 0)
		{
			if (Mob.arrMobTemplate[mobInfo.templateId] != null)
			{
				if (Mob.arrMobTemplate[mobInfo.templateId].data != null)
				{
					Mob.arrMobTemplate[mobInfo.templateId].data.paintFrame(g, f[count], x, y, 0, 0);
				}
				else if (timeRequest - GameCanvas.timeNow < 0)
				{
					timeRequest = GameCanvas.timeNow + 1500;
					mobInfo.getData();
				}
			}
		}
		else if (charInfo != null)
		{
			charInfo.paintCharBody(g, x, y, 1, f[count], isPaintBag: true);
		}
	}

	public void addItemDetail()
	{
		cp = new ChatPopup();
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty2 = empty2 + "\n|6|" + info;
		empty2 += "\n--";
		if (itemOption != null)
		{
			int num = 0;
			bool flag = true;
			while (flag)
			{
				int num2 = 0;
				for (int i = 0; i < itemOption.Length; i++)
				{
					empty = itemOption[i].getOptionString();
					if (!empty.Equals(string.Empty) && num == itemOption[i].activeCard)
					{
						num2++;
						break;
					}
				}
				if (num2 == 0)
				{
					flag = false;
					break;
				}
				if (num == 0)
				{
					empty2 = empty2 + "\n|6|2|--" + mResources.unlock + "--";
				}
				else
				{
					string text = empty2;
					empty2 = text + "\n|6|2|--" + mResources.equip + " Lv." + num + "--";
				}
				for (int j = 0; j < itemOption.Length; j++)
				{
					empty = itemOption[j].getOptionString();
					if (empty.Equals(string.Empty) || num != itemOption[j].activeCard)
					{
						continue;
					}
					string text2 = "1";
					if (level == 0)
					{
						text2 = "2";
					}
					else if (itemOption[j].activeCard != 0)
					{
						if (isUse == 0)
						{
							text2 = "2";
						}
						else if (level < itemOption[j].activeCard)
						{
							text2 = "2";
						}
					}
					string text3 = empty2;
					empty2 = text3 + "\n|" + text2 + "|1|" + empty;
				}
				if (num2 != 0)
				{
					num++;
				}
			}
		}
		popUpDetailInit(cp, empty2);
	}

	public void popUpDetailInit(ChatPopup cp, string chat)
	{
		cp.sayWidth = RadarScr.wText;
		cp.cx = RadarScr.xText;
		cp.says = mFont.tahoma_7.splitFontArray(chat, cp.sayWidth - 8);
		cp.delay = 10000000;
		cp.c = null;
		cp.ch = cp.says.Length * 12;
		cp.cy = RadarScr.yText;
		cp.strY = 10;
		cp.lim = cp.ch - RadarScr.hText;
		if (cp.lim < 0)
		{
			cp.lim = 0;
		}
	}

	public void SetEff()
	{
		if (amount == max_amount && eff.size() == 0)
		{
			int num = Res.random(1, 5);
			for (int i = 0; i < num; i++)
			{
				Position position = new Position();
				position.x = Res.random(5, 25);
				position.y = Res.random(5, 25);
				position.v = i * Res.random(0, 8);
				position.w = 0;
				position.anchor = -1;
				eff.addElement(position);
			}
		}
	}

	public void paintEff(mGraphics g, int x, int y)
	{
		SetEff();
		for (int i = 0; i < eff.size(); i++)
		{
			Position position = (Position)eff.elementAt(i);
			if (position == null)
			{
				continue;
			}
			if (position.w < position.v)
			{
				position.w++;
			}
			if (position.w >= position.v)
			{
				position.anchor = GameCanvas.gameTick / 3 % (RadarScr.fraEff.nFrame + 1);
				if (position.anchor >= RadarScr.fraEff.nFrame)
				{
					eff.removeElementAt(i);
					i--;
				}
				else
				{
					RadarScr.fraEff.drawFrame(position.anchor, x + position.x, y + position.y, 0, 3, g);
				}
			}
		}
	}
}
