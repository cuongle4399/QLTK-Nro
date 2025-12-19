public class Npc : Char
{
	public const sbyte BINH_KHI = 0;

	public const sbyte PHONG_CU = 1;

	public const sbyte TRANG_SUC = 2;

	public const sbyte DUOC_PHAM = 3;

	public const sbyte TAP_HOA = 4;

	public const sbyte THU_KHO = 5;

	public const sbyte DA_LUYEN = 6;

	public NpcTemplate template;

	public int npcId;

	public bool isFocus = true;

	public static NpcTemplate[] arrNpcTemplate;

	public int sys;

	public new bool isHide;

	private int duaHauIndex;

	private int dyEff;

	public static bool mabuEff;

	public static int tMabuEff;

	private static int[] shock_x;

	private static int[] shock_y;

	public static int shock_scr;

	public int[] duahau;

	public new int seconds;

	public new long last;

	public new long cur;

	public int idItem;

	private string nameIdNPC = "";

	public Npc(int npcId, int status, int cx, int cy, int templateId, int avatar)
	{
		isShadown = true;
		this.npcId = npcId;
		base.avatar = avatar;
		base.cx = cx;
		base.cy = cy;
		xSd = cx;
		ySd = cy;
		statusMe = status;
		if (npcId != -1)
		{
			template = arrNpcTemplate[templateId];
		}
		if (templateId == 23 || templateId == 42)
		{
			ch = 45;
		}
		if (templateId == 51)
		{
			isShadown = false;
			duaHauIndex = status;
		}
		if (template != null)
		{
			if (template.name == null)
			{
				template.name = string.Empty;
			}
			template.name = Res.changeString(template.name);
		}
	}

	public void setStatus(sbyte s, int sc)
	{
		duaHauIndex = s;
		last = (cur = mSystem.currentTimeMillis());
		seconds = sc;
	}

	public static void clearEffTask()
	{
		for (int i = 0; i < GameScr.vNpc.size(); i++)
		{
			Npc npc = (Npc)GameScr.vNpc.elementAt(i);
			npc.effTask = null;
			npc.indexEffTask = -1;
		}
	}

	public override void update()
	{
		if (template.npcTemplateId == 51)
		{
			cur = mSystem.currentTimeMillis();
			if (cur - last >= 1000)
			{
				seconds--;
				last = cur;
				if (seconds < 0)
				{
					seconds = 0;
				}
			}
		}
		if (isShadown)
		{
			updateShadown();
		}
		if (effTask == null)
		{
			sbyte[] array = new sbyte[7] { -1, 9, 9, 10, 10, 11, 11 };
			if (Char.myCharz().ctaskId >= 9 && Char.myCharz().ctaskId <= 10 && Char.myCharz().nClass.classId > 0 && array[Char.myCharz().nClass.classId] == template.npcTemplateId)
			{
				if (Char.myCharz().taskMaint == null)
				{
					effTask = GameScr.efs[57];
					indexEffTask = 0;
				}
				else if (Char.myCharz().taskMaint != null && Char.myCharz().taskMaint.index + 1 == Char.myCharz().taskMaint.subNames.Length)
				{
					effTask = GameScr.efs[62];
					indexEffTask = 0;
				}
			}
			else
			{
				sbyte taskNpcId = GameScr.getTaskNpcId();
				if (Char.myCharz().taskMaint == null && taskNpcId == template.npcTemplateId)
				{
					indexEffTask = 0;
				}
				else if (Char.myCharz().taskMaint != null && taskNpcId == template.npcTemplateId)
				{
					if (Char.myCharz().taskMaint.index + 1 == Char.myCharz().taskMaint.subNames.Length)
					{
						effTask = GameScr.efs[98];
					}
					else
					{
						effTask = GameScr.efs[98];
					}
					indexEffTask = 0;
				}
			}
		}
		base.update();
		if (TileMap.mapID != 51)
		{
			return;
		}
		if (cx > Char.myCharz().cx)
		{
			cdir = -1;
		}
		else
		{
			cdir = 1;
		}
		if (template.npcTemplateId % 2 == 0)
		{
			if (cf == 1)
			{
				cf = 0;
			}
			else
			{
				cf = 1;
			}
		}
	}

	public void paintHead(mGraphics g, int xStart, int yStart)
	{
		Part part = GameScr.parts[template.headId];
		if (cdir == 1)
		{
			SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[cf][0][0]].id, GameCanvas.w - 31 - g.getTranslateX(), 2 - g.getTranslateY(), 0, 0);
		}
		else
		{
			SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[cf][0][0]].id, GameCanvas.w - 31 - g.getTranslateX(), 2 - g.getTranslateY(), 2, 24);
		}
	}

	public override void paint(mGraphics g)
	{
		if (Char.isLoadingMap || isHide || !isPaint() || statusMe == 15)
		{
			return;
		}
		if (cTypePk != 0)
		{
			base.paint(g);
		}
		else
		{
			if (template == null)
			{
				return;
			}
			if (template.npcTemplateId != 4 && template.npcTemplateId != 51 && template.npcTemplateId != 50)
			{
				g.drawImage(TileMap.bong, cx, cy, 3);
			}
			if (template.npcTemplateId == 3)
			{
				SmallImage.drawSmallImage(g, 265, cx, cy, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
				if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this) && ChatPopup.currChatPopup == null)
				{
					g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, cx, cy - ch + 4, mGraphics.BOTTOM | mGraphics.HCENTER);
				}
				dyEff = 60;
			}
			else if (template.npcTemplateId != 4)
			{
				if (template.npcTemplateId == 50 || template.npcTemplateId == 51)
				{
					if (duahau != null)
					{
						if (template.npcTemplateId == 50 && mabuEff)
						{
							tMabuEff++;
							if (GameCanvas.gameTick % 3 == 0)
							{
								EffecMn.addEff(new Effect(19, cx + Res.random(-50, 50), cy, 2, 1, -1));
							}
							if (GameCanvas.gameTick % 15 == 0)
							{
								EffecMn.addEff(new Effect(18, cx + Res.random(-5, 5), cy + Res.random(-90, 0), 2, 1, -1));
							}
							if (tMabuEff == 100)
							{
								GameScr.gI().activeSuperPower(cx, cy);
							}
							if (tMabuEff == 110)
							{
								mabuEff = false;
								template.npcTemplateId = 4;
							}
						}
						int num = 0;
						if (SmallImage.imgNew[duahau[duaHauIndex]] != null && SmallImage.imgNew[duahau[duaHauIndex]].img != null)
						{
							num = mGraphics.getImageHeight(SmallImage.imgNew[duahau[duaHauIndex]].img);
						}
						SmallImage.drawSmallImage(g, duahau[duaHauIndex], cx + Res.random(-1, 1), cy, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
						if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this))
						{
							if (ChatPopup.currChatPopup == null)
							{
								g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, cx, cy - ch - 9 + 16 - num, mGraphics.BOTTOM | mGraphics.HCENTER);
							}
							mFont.tahoma_7b_white.drawString(g, NinjaUtil.getTime(seconds), cx, cy - ch - 16 - mFont.tahoma_7.getHeight() - 20 - num + 16, mFont.CENTER, mFont.tahoma_7b_dark);
						}
						else
						{
							mFont.tahoma_7b_white.drawString(g, NinjaUtil.getTime(seconds), cx, cy - ch - 8 - mFont.tahoma_7.getHeight() - 20 - num + 16, mFont.CENTER, mFont.tahoma_7b_dark);
						}
					}
				}
				else if (template.npcTemplateId == 6)
				{
					SmallImage.drawSmallImage(g, 545, cx, cy + 5, 0, mGraphics.BOTTOM | mGraphics.HCENTER);
					if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this) && ChatPopup.currChatPopup == null)
					{
						g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, cx, cy - ch - 9, mGraphics.BOTTOM | mGraphics.HCENTER);
					}
					mFont.tahoma_7b_white.drawString(g, TileMap.zoneID + string.Empty, cx, cy - ch + 19 - mFont.tahoma_7.getHeight(), mFont.CENTER);
				}
				else
				{
					int headId = template.headId;
					int legId = template.legId;
					int bodyId = template.bodyId;
					Part part = GameScr.parts[headId];
					Part part2 = GameScr.parts[legId];
					Part part3 = GameScr.parts[bodyId];
					if (cdir == 1)
					{
						SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[cf][0][0]].id, cx + Char.CharInfo[cf][0][1] + part.pi[Char.CharInfo[cf][0][0]].dx, cy - Char.CharInfo[cf][0][2] + part.pi[Char.CharInfo[cf][0][0]].dy, 0, 0);
						SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[cf][1][0]].id, cx + Char.CharInfo[cf][1][1] + part2.pi[Char.CharInfo[cf][1][0]].dx, cy - Char.CharInfo[cf][1][2] + part2.pi[Char.CharInfo[cf][1][0]].dy, 0, 0);
						SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[cf][2][0]].id, cx + Char.CharInfo[cf][2][1] + part3.pi[Char.CharInfo[cf][2][0]].dx, cy - Char.CharInfo[cf][2][2] + part3.pi[Char.CharInfo[cf][2][0]].dy, 0, 0);
					}
					else
					{
						SmallImage.drawSmallImage(g, part.pi[Char.CharInfo[cf][0][0]].id, cx - Char.CharInfo[cf][0][1] - part.pi[Char.CharInfo[cf][0][0]].dx, cy - Char.CharInfo[cf][0][2] + part.pi[Char.CharInfo[cf][0][0]].dy, 2, 24);
						SmallImage.drawSmallImage(g, part2.pi[Char.CharInfo[cf][1][0]].id, cx - Char.CharInfo[cf][1][1] - part2.pi[Char.CharInfo[cf][1][0]].dx, cy - Char.CharInfo[cf][1][2] + part2.pi[Char.CharInfo[cf][1][0]].dy, 2, 24);
						SmallImage.drawSmallImage(g, part3.pi[Char.CharInfo[cf][2][0]].id, cx - Char.CharInfo[cf][2][1] - part3.pi[Char.CharInfo[cf][2][0]].dx, cy - Char.CharInfo[cf][2][2] + part3.pi[Char.CharInfo[cf][2][0]].dy, 2, 24);
					}
					if (TileMap.mapID != 51)
					{
						int num2 = 15;
						if (template.npcTemplateId == 47)
						{
							num2 = 47;
						}
						if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this))
						{
							if (ChatPopup.currChatPopup == null)
							{
								int num3 = 0;
								int num4 = 0;
								if (Char.myCharz().npcFocus.template.npcTemplateId == 28 || Char.myCharz().npcFocus.template.npcTemplateId == 41)
								{
									num3 = 3;
									num4 = -12;
								}
								g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, cx + num3, cy - ch - (num2 - 8) + num4, mGraphics.BOTTOM | mGraphics.HCENTER);
							}
						}
						else
						{
							_ = template.npcTemplateId;
						}
					}
					dyEff = 65;
				}
			}
			if (indexEffTask < 0 || effTask == null || cTypePk != 0)
			{
				return;
			}
			SmallImage.drawSmallImage(g, effTask.arrEfInfo[indexEffTask].idImg, cx + effTask.arrEfInfo[indexEffTask].dx, cy + effTask.arrEfInfo[indexEffTask].dy - dyEff, 0, mGraphics.VCENTER | mGraphics.HCENTER);
			if (GameCanvas.gameTick % 2 == 0)
			{
				indexEffTask++;
				if (indexEffTask >= effTask.arrEfInfo.Length)
				{
					indexEffTask = 0;
				}
			}
		}
	}

	public new void paintName(mGraphics g)
	{
		nameIdNPC = template.name + " [" + template.npcTemplateId + "]";
		if (Char.isLoadingMap || isHide || !isPaint() || statusMe == 15 || template == null)
		{
			return;
		}
		if (template.npcTemplateId == 3)
		{
			if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this))
			{
				mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx, cy - ch - mFont.tahoma_7.getHeight() - 5, mFont.CENTER, mFont.tahoma_7_grey);
			}
			else
			{
				mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx, cy - ch - 3 - mFont.tahoma_7.getHeight(), mFont.CENTER, mFont.tahoma_7_grey);
			}
			dyEff = 60;
		}
		else
		{
			if (template.npcTemplateId == 4)
			{
				return;
			}
			if (template.npcTemplateId == 50 || template.npcTemplateId == 51)
			{
				if (duahau != null)
				{
					int num = 0;
					if (SmallImage.imgNew[duahau[duaHauIndex]] != null && SmallImage.imgNew[duahau[duaHauIndex]].img != null)
					{
						num = mGraphics.getImageHeight(SmallImage.imgNew[duahau[duaHauIndex]].img);
					}
					if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this))
					{
						mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx, cy - ch - mFont.tahoma_7.getHeight() - num, mFont.CENTER, mFont.tahoma_7_grey);
					}
					else
					{
						mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx, cy - ch - 8 - mFont.tahoma_7.getHeight() - num + 16, mFont.CENTER, mFont.tahoma_7_grey);
					}
				}
				return;
			}
			if (template.npcTemplateId == 6)
			{
				if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this))
				{
					mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx, cy - ch - mFont.tahoma_7.getHeight() - 16, mFont.CENTER, mFont.tahoma_7_grey);
				}
				else
				{
					mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx, cy - ch - 8 - mFont.tahoma_7.getHeight(), mFont.CENTER, mFont.tahoma_7_grey);
				}
				return;
			}
			if (TileMap.mapID != 51)
			{
				int num2 = 15;
				if (template.npcTemplateId == 47)
				{
					num2 = 47;
				}
				if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus.Equals(this))
				{
					if (TileMap.mapID != 113)
					{
						int num3 = 0;
						int num4 = 0;
						if (Char.myCharz().npcFocus.template.npcTemplateId == 28 || Char.myCharz().npcFocus.template.npcTemplateId == 41)
						{
							num3 = 3;
							num4 = -12;
						}
						mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx + num3, cy - ch - mFont.tahoma_7.getHeight() - num2 + num4, mFont.CENTER, mFont.tahoma_7_grey);
					}
				}
				else
				{
					num2 = 8;
					if (template.npcTemplateId == 47)
					{
						num2 = 40;
					}
					if (TileMap.mapID != 113)
					{
						int num5 = 0;
						int num6 = 0;
						if (template.npcTemplateId == 28 || template.npcTemplateId == 41)
						{
							num5 = 3;
							num6 = -12;
						}
						mFont.tahoma_7b_yellow.drawString(g, nameIdNPC, cx + num5, cy - ch - num2 - mFont.tahoma_7.getHeight() + num6, mFont.CENTER, mFont.tahoma_7_grey);
					}
				}
			}
			dyEff = 65;
		}
	}

	public new void hide()
	{
		statusMe = 15;
		Char.chatPopup = null;
	}

	static Npc()
	{
		shock_x = new int[4] { 1, -1, 1, -1 };
		shock_y = new int[4] { 1, -1, -1, 1 };
	}
}
