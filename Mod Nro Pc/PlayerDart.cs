public class PlayerDart
{
	public Char charBelong;

	public DartInfo info;

	public MyVector darts = new MyVector();

	public int angle;

	public int vx;

	public int vy;

	public int va;

	public int x;

	public int y;

	public int z;

	private int life;

	private int dx;

	private int dy;

	public bool isActive = true;

	public bool isSpeedUp;

	public SkillPaint skillPaint;

	public PlayerDart(Char charBelong, int dartType, SkillPaint sp, int x, int y)
	{
		skillPaint = sp;
		this.charBelong = charBelong;
		info = GameScr.darts[dartType];
		va = info.va;
		this.x = x;
		this.y = y;
		object obj;
		if (charBelong.mobFocus == null)
		{
			IMapObject charFocus = charBelong.charFocus;
			obj = charFocus;
		}
		else
		{
			obj = charBelong.mobFocus;
		}
		IMapObject mapObject = (IMapObject)obj;
		setAngle(Res.angle(mapObject.getX() - x, mapObject.getY() - y));
	}

	public void setAngle(int angle)
	{
		this.angle = angle;
		vx = va * Res.cos(angle) >> 10;
		vy = va * Res.sin(angle) >> 10;
	}

	public void update()
	{
		if (!isActive)
		{
			return;
		}
		if (charBelong.mobFocus == null && charBelong.charFocus == null)
		{
			endMe();
			return;
		}
		object obj;
		if (charBelong.mobFocus == null)
		{
			IMapObject charFocus = charBelong.charFocus;
			obj = charFocus;
		}
		else
		{
			obj = charBelong.mobFocus;
		}
		IMapObject mapObject = (IMapObject)obj;
		for (int i = 0; i < info.nUpdate; i++)
		{
			if (info.tail.Length != 0)
			{
				darts.addElement(new SmallDart(x, y));
			}
			int num = ((charBelong.getX() <= mapObject.getX()) ? (-10) : 10);
			dx = mapObject.getX() + num - x;
			dy = mapObject.getY() - mapObject.getH() / 2 - y;
			life++;
			if (Res.abs(dx) < 20 && Res.abs(dy) < 20)
			{
				if (charBelong.charFocus != null && charBelong.charFocus.me)
				{
					charBelong.charFocus.doInjure(charBelong.charFocus.damHP, 0L, charBelong.charFocus.isCrit, charBelong.charFocus.isMob);
				}
				endMe();
				return;
			}
			int num2 = Res.angle(dx, dy);
			if (Math.Abs(num2 - angle) < 90 || dx * dx + dy * dy > 4096)
			{
				if (Math.Abs(num2 - angle) < 15)
				{
					angle = num2;
				}
				else if ((num2 - angle >= 0 && num2 - angle < 180) || num2 - angle < -180)
				{
					angle = Res.fixangle(angle + 15);
				}
				else
				{
					angle = Res.fixangle(angle - 15);
				}
			}
			if (!isSpeedUp && va < 8192)
			{
				va += 1024;
			}
			vx = va * Res.cos(angle) >> 10;
			vy = va * Res.sin(angle) >> 10;
			dx += vx;
			int num3 = dx >> 10;
			x += num3;
			dx &= 1023;
			dy += vy;
			int num4 = dy >> 10;
			y += num4;
			dy &= 1023;
		}
		for (int j = 0; j < darts.size(); j++)
		{
			SmallDart smallDart = (SmallDart)darts.elementAt(j);
			smallDart.index++;
			if (smallDart.index >= info.tail.Length)
			{
				darts.removeElementAt(j);
			}
		}
	}

	private void endMe()
	{
		if (!charBelong.isUseSkillAfterCharge && x >= GameScr.cmx && x <= GameScr.cmx + GameCanvas.w)
		{
			SoundMn.gI().explode_1();
		}
		charBelong.setAttack();
		if (charBelong.me)
		{
			charBelong.saveLoadPreviousSkill();
		}
		if (charBelong.isUseSkillAfterCharge)
		{
			charBelong.isUseSkillAfterCharge = false;
			if (charBelong.isLockMove && charBelong.me && charBelong.statusMe != 14 && charBelong.statusMe != 5)
			{
				charBelong.isLockMove = false;
			}
			if (charBelong.cgender == 2)
			{
				int num = -1;
				num = ((!charBelong.me) ? charBelong.skillTemplateId : Char.myCharz().myskill.skillId);
				if (num < 77 || num > 83)
				{
					GameScr.gI().activeSuperPower(x, y);
				}
			}
			else
			{
				GameScr.gI().activeSuperPower(x, y);
			}
		}
		charBelong.dart = null;
		charBelong.isCreateDark = false;
		charBelong.skillPaint = null;
		charBelong.skillPaintRandomPaint = null;
		charBelong.stopUseChargeSkill();
	}

	public void paint(mGraphics g)
	{
		if (!isActive)
		{
			return;
		}
		int num = MonsterDart.findDirIndexFromAngle(360 - angle);
		int num2 = MonsterDart.FRAME[num];
		int transform = MonsterDart.TRANSFORM[num];
		for (int i = darts.size() / 2; i < darts.size(); i++)
		{
			SmallDart smallDart = (SmallDart)darts.elementAt(i);
			SmallImage.drawSmallImage(g, info.tailBorder[smallDart.index], smallDart.x, smallDart.y, 0, 3);
		}
		int num3 = GameCanvas.gameTick % info.headBorder.Length;
		SmallImage.drawSmallImage(g, info.headBorder[num3][num2], x, y, transform, 3);
		for (int j = 0; j < darts.size(); j++)
		{
			SmallDart smallDart2 = (SmallDart)darts.elementAt(j);
			SmallImage.drawSmallImage(g, info.tail[smallDart2.index], smallDart2.x, smallDart2.y, 0, 3);
		}
		SmallImage.drawSmallImage(g, info.head[num3][num2], x, y, transform, 3);
		for (int k = 0; k < darts.size(); k++)
		{
			SmallDart smallDart3 = (SmallDart)darts.elementAt(k);
			if (Res.abs(MonsterDart.r.nextInt(100)) < info.xdPercent)
			{
				SmallImage.drawSmallImage(g, (GameCanvas.gameTick % 2 != 0) ? info.xd2[smallDart3.index] : info.xd1[smallDart3.index], smallDart3.x, smallDart3.y, 0, 3);
			}
		}
		g.setColor(16711680);
	}
}
