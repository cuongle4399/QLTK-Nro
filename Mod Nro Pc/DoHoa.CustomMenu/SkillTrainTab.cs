using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DoHoa.CustomMenu.Shared;
using Mod.CuongLe;

namespace DoHoa.CustomMenu;

public static class SkillTrainTab
{
	public static SkillTrain[] SkillTrains;

	public static int ScrollOffset;

	private static string planet;

	static SkillTrainTab()
	{
		ScrollOffset = 0;
		planet = Char.myCharz().getGender();
		int num = Char.myCharz().vSkill.size();
		SkillTrains = new SkillTrain[num];
		for (int i = 0; i < num; i++)
		{
			Skill skill = (Skill)Char.myCharz().vSkill.elementAt(i);
			SkillTrains[i] = new SkillTrain
			{
				Name = MenuHelper.NormalizeSkillName(skill.template.name),
				Id = skill.template.id,
				AutoFlag = !MenuConstants.ExcludedSkillIds.Contains(skill.template.id)
			};
		}
		LoadConfig();
	}

	public static void Paint(mGraphics g, int panelX, int contentY)
	{
		bool flag = mGraphics.zoomLevel <= 1;
		int focusedIndex = GetFocusedIndex(panelX, contentY);
		for (int i = ScrollOffset; i < System.Math.Min(SkillTrains.Length, ScrollOffset + MenuHelper.Rows); i++)
		{
			int num = i - ScrollOffset;
			int num2 = contentY + num * 32;
			bool flag2 = i == focusedIndex;
			Skill skill = (Skill)Char.myCharz().vSkill.elementAt(i);
			SkillTemplate template = skill.template;
			int num3 = panelX + 8;
			int num4 = MenuHelper.PanelWidth - 16;
			int h = 31;
			g.setColor(flag2 ? 6052956 : 3815994);
			int num5 = num3;
			int num6 = num4;
			if (flag)
			{
				num5 = num3;
				num6 = num4;
				g.fillRect(num5, num2, num6, h);
				g.setColor(flag2 ? 33679 : 4934475);
				g.fillRect(num3, num2, 40, h);
			}
			else
			{
				num5 = num3 + 40;
				num6 = num4 - 40;
				g.setColor(flag2 ? 33679 : 4934475);
				g.fillRect(num3, num2, 40, h);
				g.setColor(flag2 ? 6052956 : 3815994);
				g.fillRect(num5, num2, num6, h);
			}
			if (flag2)
			{
				g.setColor(49151);
				g.drawRect(num3, num2, num4, h);
			}
			int x = num3 + 20;
			int y = num2 + 16;
			SmallImage.drawSmallImage(g, template.iconId, x, y, 0, 3);
			if (!flag)
			{
				int num7 = num3 + 45;
				int y2 = num2 + 3;
				int num8 = num2 + 15;
				int x2 = panelX + MenuHelper.PanelWidth - 28;
				mFont tahoma_7b_white = mFont.tahoma_7b_white;
				tahoma_7b_white.drawString(g, template.name, num7, y2, 0);
				mFont.tahoma_7_white.drawString(g, "Lv: " + skill.point, x2, y2, mFont.RIGHT);
				mFont tahoma_7_white = mFont.tahoma_7_white;
				if (skill.point == template.maxPoint)
				{
					mFont.tahoma_7_blue.drawString(g, mResources.max_level_reach, num7, num8, 0);
				}
				else if (template.isSkillSpec())
				{
					string text = "TN: ";
					tahoma_7_white.drawString(g, text, num7, num8, 0);
					int num9 = num7 + tahoma_7_white.getWidthExactOf(text) + 2;
					int y3 = num8 + 4;
					g.setColor(5066061);
					g.fillRect(num9, y3, 50, 5);
					int w = skill.curExp * 50 / 1000;
					g.setColor(16754470);
					g.fillRect(num9, y3, w, 5);
					int x3 = num9 + 50 + 5;
					tahoma_7_white.drawString(g, skill.strCurExp(), x3, num8, 0);
				}
				else
				{
					Skill skill2 = template.skills[skill.point];
					string st = "Lv " + (skill.point + 1) + " (CN: " + Res.formatNumber2(skill2.powRequire) + ")";
					tahoma_7_white.drawString(g, st, num7, num8, 0);
				}
			}
			int x4 = panelX + MenuHelper.PanelWidth - 18 - 8;
			int y4 = num2 + 7;
			GameCanvas.paintz.paintCheckPass(g, x4, y4, SkillTrains[i].AutoFlag, focus: false);
		}
		int maxScrollOffset = MenuHelper.CalculateMaxScrollOffset(SkillTrains.Length);
		MenuHelper.DrawScrollBar(g, panelX, contentY, SkillTrains.Length, ScrollOffset, maxScrollOffset);
		DrawControlButtons(g, panelX, contentY + MenuHelper.ContentHeight + 8);
	}

	private static void DrawControlButtons(mGraphics g, int panelX, int btnY_Start)
	{
		int num = MenuHelper.PanelWidth - 16;
		mFont tahoma_7b_white = mFont.tahoma_7b_white;
		int num2 = (num - 8) / 2;
		int num3 = panelX + 8;
		int num4 = num3 + num2 + 8;
		g.setColor(6732650);
		g.fillRect(num3, btnY_Start, num2, 18);
		tahoma_7b_white.drawString(g, "LƯU", num3 + num2 / 2, btnY_Start + 4, 2);
		string st = ((mGraphics.zoomLevel <= 1) ? "RESET" : "MẶC ĐỊNH");
		g.setColor(15684432);
		g.fillRect(num4, btnY_Start, num2, 18);
		tahoma_7b_white.drawString(g, st, num4 + num2 / 2, btnY_Start + 4, 2);
	}

	public static void HandleClick(int panelX, int contentY)
	{
		int x = panelX + MenuHelper.PanelWidth - 28 - 8;
		for (int i = ScrollOffset; i < System.Math.Min(ScrollOffset + MenuHelper.Rows, SkillTrains.Length); i++)
		{
			int num = i - ScrollOffset;
			int num2 = num % MenuHelper.Rows;
			int y = contentY + num2 * 32;
			if (!GameCanvas.isPointerHoldIn(x, y, 36, 32))
			{
				continue;
			}
			SkillTrains[i].AutoFlag = !SkillTrains[i].AutoFlag;
			string text = (SkillTrains[i].AutoFlag ? "BẬT" : "TẮT");
			GameScr.info1.addInfo("Auto Skill " + SkillTrains[i].Name + ": " + text);
			if (SkillTrains[i].Id == 7 && planet.Equals("NM"))
			{
				AutoPetCL.HealingPower = SkillTrains[i].AutoFlag;
				if (SkillTrains[i].AutoFlag)
				{
					Thread thread = new Thread(ModProCL.AutoHealingPower2);
					thread.IsBackground = true;
					thread.Start();
				}
			}
			GameCanvas.clearAllPointerEvent();
			return;
		}
		int y2 = contentY + MenuHelper.ContentHeight + 8;
		int num3 = MenuHelper.PanelWidth - 16;
		int num4 = (num3 - 8) / 2;
		int num5 = panelX + 8;
		int x2 = num5 + num4 + 8;
		if (GameCanvas.isPointerHoldIn(num5, y2, num4, 18))
		{
			SaveConfig();
			GameScr.info1.addInfo("Đã lưu cấu hình Auto Skill!");
			GameCanvas.clearAllPointerEvent();
		}
		else if (GameCanvas.isPointerHoldIn(x2, y2, num4, 18))
		{
			ResetConfig();
			GameScr.info1.addInfo("Đã khôi phục cài đặt mặc định!");
			GameCanvas.clearAllPointerEvent();
		}
	}

	public static void HandleScroll(int deltaY)
	{
		int num = -deltaY / 4;
		int val = MenuHelper.CalculateMaxScrollOffset(SkillTrains.Length);
		int val2 = ScrollOffset + num;
		val2 = System.Math.Max(0, System.Math.Min(val2, val));
		if (val2 != ScrollOffset)
		{
			ScrollOffset = val2;
			GameCanvas.pyLast = GameCanvas.py;
		}
	}

	private static int GetFocusedIndex(int panelX, int contentY)
	{
		if (!GameCanvas.isPointerDown || !GameCanvas.isPointerJustRelease)
		{
			return -1;
		}
		int px = GameCanvas.px;
		int py = GameCanvas.py;
		int num = panelX + 8;
		int num2 = panelX + MenuHelper.PanelWidth - 8;
		for (int i = ScrollOffset; i < System.Math.Min(ScrollOffset + MenuHelper.Rows, SkillTrains.Length); i++)
		{
			int num3 = i - ScrollOffset;
			int num4 = num3 % MenuHelper.Rows;
			int num5 = contentY + num4 * 32;
			if (px >= num && px <= num2 && py >= num5 && py <= num5 + 32)
			{
				return i;
			}
		}
		return -1;
	}

	public static void SaveConfig()
	{
		string file = "AutoSkillConfig_Shield_" + planet;
		string filename = "AutoSkillConfig_" + planet;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < SkillTrains.Length; i++)
		{
			if (SkillTrains[i].Id == 19)
			{
				Rms.saveRMSInt(file, SkillTrains[i].AutoFlag ? 1 : 0);
				continue;
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.AppendFormat("{0}|{1}", SkillTrains[i].Id, SkillTrains[i].AutoFlag ? "1" : "0");
		}
		Rms.saveRMSString(filename, stringBuilder.ToString());
	}

	public static void LoadConfig()
	{
		string file = "AutoSkillConfig_Shield_" + planet;
		string fileName = "AutoSkillConfig_" + planet;
		int num = Rms.loadRMSInt(file);
		string text = Rms.loadRMSString(fileName);
		if (string.IsNullOrEmpty(text) && num == 0)
		{
			return;
		}
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		if (!string.IsNullOrEmpty(text))
		{
			string[] array = text.Split(',');
			string[] array2 = array;
			string[] array3 = array2;
			string[] array4 = array3;
			foreach (string text2 in array4)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					string[] array5 = text2.Split('|');
					if (array5.Length == 2 && int.TryParse(array5[0], out var result))
					{
						bool value = array5[1] == "1";
						dictionary[result] = value;
					}
				}
			}
		}
		for (int j = 0; j < SkillTrains.Length; j++)
		{
			if (SkillTrains[j].Id == 19)
			{
				SkillTrains[j].AutoFlag = num == 1;
			}
			else if (dictionary.ContainsKey(SkillTrains[j].Id))
			{
				SkillTrains[j].AutoFlag = dictionary[SkillTrains[j].Id];
			}
		}
	}

	public static void ResetConfig()
	{
		for (int i = 0; i < SkillTrains.Length; i++)
		{
			SkillTrains[i].AutoFlag = !MenuConstants.ExcludedSkillIds.Contains(SkillTrains[i].Id);
		}
	}
}
