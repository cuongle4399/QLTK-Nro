using System;
using System.Threading;
using DoHoa.CustomMenu.Shared;

namespace DoHoa.CustomMenu;

public static class AutoItemTab
{
	public static AutoItemRef[] AutoItems;

	public static int ScrollOffset;

	static AutoItemTab()
	{
		AutoItems = new AutoItemRef[20]
		{
			new AutoItemRef
			{
				Name = "Cuồng Nộ",
				IdCon = 2754,
				Id = 381
			},
			new AutoItemRef
			{
				Name = "Khẩu Trang",
				IdCon = 7149,
				Id = 764
			},
			new AutoItemRef
			{
				Name = "Bổ Huyết",
				IdCon = 2755,
				Id = 382
			},
			new AutoItemRef
			{
				Name = "Giáp Xên",
				IdCon = 2757,
				Id = 384
			},
			new AutoItemRef
			{
				Name = "Búa x2 đệ",
				IdCon = 13540,
				Id = 1628
			},
			new AutoItemRef
			{
				Name = "Cỏ may mắn",
				IdCon = 13618,
				Id = 1635
			},
			new AutoItemRef
			{
				Name = "Kem dâu",
				IdCon = 6326,
				Id = 665
			},
			new AutoItemRef
			{
				Name = "Mì ly",
				IdCon = 6327,
				Id = 666
			},
			new AutoItemRef
			{
				Name = "Xúc xích",
				IdCon = 6325,
				Id = 664
			},
			new AutoItemRef
			{
				Name = "Sushi",
				IdCon = 6328,
				Id = 667
			},
			new AutoItemRef
			{
				Name = "Bánh Pudding",
				IdCon = 6324,
				Id = 663
			},
			new AutoItemRef
			{
				Name = "Cuồng Nộ cấp 2",
				IdCon = 10716,
				Id = 1150
			},
			new AutoItemRef
			{
				Name = "Bổ Huyết cấp 2",
				IdCon = 10714,
				Id = 1152
			},
			new AutoItemRef
			{
				Name = "Giáp Xên cấp 2",
				IdCon = 10712,
				Id = 1153
			},
			new AutoItemRef
			{
				Name = "Bổ khí",
				IdCon = 2756,
				Id = 383
			},
			new AutoItemRef
			{
				Name = "Bổ khí cấp 2",
				IdCon = 10715,
				Id = 1151
			},
			new AutoItemRef
			{
				Name = "Cua rang me",
				IdCon = 8060,
				Id = 880
			},
			new AutoItemRef
			{
				Name = "Bạch tuộc nướng",
				IdCon = 8061,
				Id = 881
			},
			new AutoItemRef
			{
				Name = "Tôm tẩm bột chiên xù",
				IdCon = 8062,
				Id = 882
			},

            new AutoItemRef
            {
                Name = "Máy dò capsun",
                IdCon = 2758,
                Id = 379
            }
        };
		ScrollOffset = 0;
	}

	public static void AutoItemVIP(AutoItemRef item)
	{
		try
		{
			while (item.AutoFlag)
			{
				if (ModProCL.ExistItemBag(item.Id))
				{
					if (ItemTime.getItemTimeInSeconds(item.IdCon) <= 0)
					{
						ModProCL.useItem(item.Id);
						Thread.Sleep(500);
					}
					int itemTimeInSeconds = ItemTime.getItemTimeInSeconds(item.IdCon);
					while (itemTimeInSeconds > 0 && item.AutoFlag)
					{
						Thread.Sleep(1000);
						itemTimeInSeconds = ItemTime.getItemTimeInSeconds(item.IdCon);
					}
				}
				else
				{
					Thread.Sleep(1000);
				}
			}
		}
		finally
		{
			item.Thread = null;
		}
	}

	public static void Paint(mGraphics g, int panelX, int contentY)
	{
		bool flag = mGraphics.zoomLevel <= 1;
		int focusedIndex = GetFocusedIndex(panelX, contentY);
		int num = System.Math.Min(ScrollOffset + MenuHelper.Rows, AutoItems.Length);
		for (int i = ScrollOffset; i < num; i++)
		{
			AutoItemRef autoItemRef = AutoItems[i];
			int num2 = i - ScrollOffset;
			int num3 = contentY + num2 * 32;
			bool flag2 = i == focusedIndex;
			bool flag3 = ModProCL.ExistItemBag(autoItemRef.Id);
			bool autoFlag = autoItemRef.AutoFlag;
			int color = (flag2 ? 6052956 : 3815994);
			int x = panelX + 4;
			int w = MenuHelper.PanelWidth - 8;
			int x2 = panelX + 4 + 4;
			if (!flag)
			{
				int color2 = (flag2 ? 33679 : 4934475);
				x = panelX + 4 + 34;
				w = MenuHelper.PanelWidth - 8 - 34;
				x2 = panelX + 4 + 34 + 4;
				g.setColor(color2);
				g.fillRect(panelX + 4, num3, 34, 30);
				ItemTemplate itemTemplate = ItemTemplates.get((short)autoItemRef.Id);
				if (itemTemplate != null)
				{
					int x3 = panelX + 4 + 17;
					int y = num3 + 16;
					SmallImage.drawSmallImage(g, itemTemplate.iconID, x3, y, 0, 3);
				}
			}
			g.setColor(color);
			g.fillRect(x, num3, w, 30);
			mFont mFont = (flag2 ? mFont.tahoma_7b_white : (autoFlag ? mFont.tahoma_7b_yellow : mFont.tahoma_7b_white));
			int itemTimeInSeconds = ItemTime.getItemTimeInSeconds(autoItemRef.IdCon);
			if (flag)
			{
				string text = ((!autoFlag) ? "(Tắt)" : ((itemTimeInSeconds > 0) ? $"({itemTimeInSeconds}s)" : (flag3 ? "(Dùng)" : "(Chờ)")));
				mFont.drawString(g, autoItemRef.Name + " " + text, x2, num3 + 8, 0);
			}
			else
			{
				mFont.drawString(g, autoItemRef.Name, x2, num3 + 4, 0);
				string st;
				mFont mFont2;
				if (autoFlag)
				{
					if (itemTimeInSeconds > 0)
					{
						st = $"(Buff còn {itemTimeInSeconds}s)";
						mFont2 = mFont.tahoma_7_white;
					}
					else if (flag3)
					{
						st = "(Đang chờ dùng...)";
						mFont2 = mFont.tahoma_7_yellow;
					}
					else
					{
						st = "(Chờ có item!)";
						mFont2 = mFont.tahoma_7_red;
					}
				}
				else
				{
					st = (flag3 ? "(Đang có item)" : "(Chưa chọn auto)");
					mFont2 = (flag3 ? mFont.tahoma_7_white : mFont.tahoma_7_blue1);
				}
				mFont2.drawString(g, st, x2, num3 + 14, 0);
			}
			GameCanvas.paintz.paintCheckPass(g, panelX + MenuHelper.PanelWidth - 26, num3 + 4, autoFlag, focus: false);
		}
		int maxScrollOffset = MenuHelper.CalculateMaxScrollOffset(AutoItems.Length);
		MenuHelper.DrawScrollBar(g, panelX, contentY, AutoItems.Length, ScrollOffset, maxScrollOffset);
	}

	public static void HandleClick(int panelX, int contentY)
	{
		int x = panelX + MenuHelper.PanelWidth - 28 - 4;
		for (int i = ScrollOffset; i < System.Math.Min(ScrollOffset + MenuHelper.Rows, AutoItems.Length); i++)
		{
			int num = i - ScrollOffset;
			int num2 = num % MenuHelper.Rows;
			int y = contentY + num2 * 32;
			if (!GameCanvas.isPointerHoldIn(x, y, 32, 32))
			{
				continue;
			}
			AutoItemRef item = AutoItems[i];
			item.AutoFlag = !item.AutoFlag;
			if (item.AutoFlag && (item.Thread == null || !item.Thread.IsAlive))
			{
				item.Thread = new Thread((ThreadStart)delegate
				{
					AutoItemVIP(item);
				})
				{
					IsBackground = true
				};
				item.Thread.Start();
			}
			GameCanvas.clearAllPointerEvent();
			break;
		}
	}

	public static void HandleScroll(int deltaY)
	{
		int num = deltaY / 18;
		if (num == 0 && deltaY != 0)
		{
			num = ((deltaY > 0) ? 1 : (-1));
		}
		int val = MenuHelper.CalculateMaxScrollOffset(AutoItems.Length);
		int val2 = ScrollOffset - num;
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
		for (int i = ScrollOffset; i < System.Math.Min(ScrollOffset + MenuHelper.Rows, AutoItems.Length); i++)
		{
			int num = i - ScrollOffset;
			int num2 = num % MenuHelper.Rows;
			int num3 = contentY + num2 * 32;
			if (px >= panelX + 4 && px <= panelX + MenuHelper.PanelWidth - 4 && py >= num3 && py <= num3 + 32)
			{
				return i;
			}
		}
		return -1;
	}
}
