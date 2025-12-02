public class ItemMap : IMapObject
{
	public int x;

	public int y;

	public int xEnd;

	public int yEnd;

	public int f;

	public int vx;

	public int vy;

	public int playerId;

	public int itemMapID;

	public int IdCharMove;

	public ItemTemplate template;

	public sbyte status;

	public bool isHintFocus;

	public int rO;

	public int xO;

	public int yO;

	public int angle;

	public int iAngle;

	public int iDot;

	public int[] xArg;

	public int[] yArg;

	public int[] xDot;

	public int[] yDot;

	public int count;

	public int countAura;

	public static Image imageFlare = GameCanvas.loadImage("/mainImage/myTexture2dflare.png");

	public static Image imageAuraItem1 = GameCanvas.loadImage("/mainImage/myTexture2ditemaura1.png");

	public static Image imageAuraItem2 = GameCanvas.loadImage("/mainImage/myTexture2ditemaura2.png");

	public static Image imageAuraItem3 = GameCanvas.loadImage("/mainImage/myTexture2ditemaura3.png");

	public ItemMap(short itemMapID, short itemTemplateID, int x, int y, int xEnd, int yEnd)
	{
		this.itemMapID = itemMapID;
		template = ItemTemplates.get(itemTemplateID);
		this.x = xEnd;
		this.y = y;
		this.xEnd = xEnd;
		this.yEnd = yEnd;
		vx = xEnd - x >> 2;
		vy = 5;
		Res.outz("playerid=  " + playerId + " myid= " + Char.myCharz().charID);
	}

	public ItemMap(int playerId, short itemMapID, short itemTemplateID, int x, int y, short r)
	{
		Res.outz("item map item= " + itemMapID + " template= " + itemTemplateID + " x= " + x + " y= " + y);
		this.itemMapID = itemMapID;
		template = ItemTemplates.get(itemTemplateID);
		Res.outz("playerid=  " + playerId + " myid= " + Char.myCharz().charID);
		this.x = (xEnd = x);
		this.y = (yEnd = y);
		status = 1;
		this.playerId = playerId;
		if (isAuraItem())
		{
			rO = r;
			setAuraItem();
		}
	}

	public void setPoint(int xEnd, int yEnd)
	{
		this.xEnd = xEnd;
		this.yEnd = yEnd;
		vx = xEnd - x >> 2;
		vy = yEnd - y >> 2;
		status = 2;
	}

	public void update()
	{
		if (status == 2 && x == xEnd && y == yEnd)
		{
			GameScr.vItemMap.removeElement(this);
			if (Char.myCharz().itemFocus != null && Char.myCharz().itemFocus.Equals(this))
			{
				Char.myCharz().itemFocus = null;
			}
			return;
		}
		if (status > 0)
		{
			if (vx == 0)
			{
				x = xEnd;
			}
			if (vy == 0)
			{
				y = yEnd;
			}
			if (x != xEnd)
			{
				x += vx;
				if ((vx > 0 && x > xEnd) || (vx < 0 && x < xEnd))
				{
					x = xEnd;
				}
			}
			if (y != yEnd)
			{
				y += vy;
				if ((vy > 0 && y > yEnd) || (vy < 0 && y < yEnd))
				{
					y = yEnd;
				}
			}
		}
		else
		{
			status -= 4;
			if (status < -12)
			{
				y -= 12;
				status = 1;
			}
		}
		if (isAuraItem())
		{
			updateAuraItemEff();
		}
	}

	public void paint(mGraphics g)
	{
		if (isAuraItem())
		{
			g.drawImage(TileMap.bong, x + 3, y, mGraphics.VCENTER | mGraphics.HCENTER);
			if (status <= 0)
			{
				if (countAura < 10)
				{
					g.drawImage(imageAuraItem1, x, y + status + 3, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
				else
				{
					g.drawImage(imageAuraItem2, x, y + status + 3, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
			}
			else if (countAura < 10)
			{
				g.drawImage(imageAuraItem1, x, y + 3, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			else
			{
				g.drawImage(imageAuraItem2, x, y + 3, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
		}
		else if (!isAuraItem())
		{
			if (GameCanvas.gameTick % 4 == 0)
			{
				g.drawImage(imageFlare, x, y + status + 13, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			if (status <= 0)
			{
				SmallImage.drawSmallImage(g, template.iconID, x, y + status + 3, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			else
			{
				SmallImage.drawSmallImage(g, template.iconID, x, y + 3, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
			}
			if (Char.myCharz().itemFocus != null && Char.myCharz().itemFocus.Equals(this) && status != 2)
			{
				g.drawRegion(Mob.imgHP, 0, 24, 9, 6, 0, x, y - 17, 3);
			}
		}
	}

	private bool isAuraItem()
	{
		bool flag = false;
		if (template.type == 22)
		{
			return true;
		}
		return false;
	}

	private void setAuraItem()
	{
		xO = x;
		yO = y;
		iDot = 120;
		angle = 0;
		if (!GameCanvas.lowGraphic)
		{
			iAngle = 360 / iDot;
			xArg = new int[iDot];
			yArg = new int[iDot];
			xDot = new int[iDot];
			yDot = new int[iDot];
			setDotPosition();
		}
	}

	private void updateAuraItemEff()
	{
		count++;
		countAura++;
		if (countAura >= 40)
		{
			countAura = 0;
		}
		if (count >= iDot)
		{
			count = 0;
		}
		if (count % 10 == 0 && !GameCanvas.lowGraphic)
		{
			ServerEffect.addServerEffect(114, x - 5, y - 30, 1);
		}
	}

	public void paintAuraItemEff(mGraphics g)
	{
		if (GameCanvas.lowGraphic || !isAuraItem())
		{
			return;
		}
		for (int i = 0; i < yArg.Length; i++)
		{
			if (count == i)
			{
				if (countAura <= 20)
				{
					g.drawImage(imageAuraItem3, xDot[i], yDot[i] + 3, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
				else
				{
					SmallImage.drawSmallImage(g, template.iconID, xDot[i], yDot[i] + 3, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
			}
		}
	}

	private void setDotPosition()
	{
		if (GameCanvas.lowGraphic)
		{
			return;
		}
		for (int i = 0; i < yArg.Length; i++)
		{
			yArg[i] = Res.abs(rO * Res.sin(angle) / 1024);
			xArg[i] = Res.abs(rO * Res.cos(angle) / 1024);
			if (angle < 90)
			{
				xDot[i] = xO + xArg[i];
				yDot[i] = yO - yArg[i];
			}
			else if (angle >= 90 && angle < 180)
			{
				xDot[i] = xO - xArg[i];
				yDot[i] = yO - yArg[i];
			}
			else if (angle >= 180 && angle < 270)
			{
				xDot[i] = xO - xArg[i];
				yDot[i] = yO + yArg[i];
			}
			else
			{
				xDot[i] = xO + xArg[i];
				yDot[i] = yO + yArg[i];
			}
			angle += iAngle;
		}
	}

	public int getX()
	{
		return x;
	}

	public int getY()
	{
		return y;
	}

	public int getH()
	{
		return 20;
	}

	public int getW()
	{
		return 20;
	}

	public void stopMoving()
	{
	}

	public bool isInvisible()
	{
		return false;
	}
}
