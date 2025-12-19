using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Mod_nro.MenuDataGame;

public class DataItem : IChatable
{
	public static bool IsShow;

	public static List<AllItemRef> AllItems;

	private static List<AllItemRef> _OriginalAllItems;

	public static int SelectedIndex;

	public const int Columns = 3;

	private static int PanelWidth;

	private static int PanelHeight;

	private static int PanelX;

	private static int PanelY;

	private static int ContentHeight;

	private static int Rows;

	private static int ItemWidth;

	private static float scrollPixelOffset;

	private static float scrollVelocity;

	private static readonly Queue<float> velocityHistory;

	private static long lastFrameTime;

	private static int hoveredIndex;

	private static bool isScrollbarHovered;

	private static bool isSearchButtonHovered;

	private static bool isResetButtonHovered;

	private static bool isDragging;

	private static int dragStartY;

	public static string strFindItem;

	private static string[] titleInput;

	private static DataItem _Instance;

	private static bool _isInitialized;

	static DataItem()
	{
		IsShow = false;
		SelectedIndex = -1;
		scrollPixelOffset = 0f;
		scrollVelocity = 0f;
		velocityHistory = new Queue<float>();
		lastFrameTime = 0L;
		hoveredIndex = -1;
		isScrollbarHovered = false;
		isSearchButtonHovered = false;
		isResetButtonHovered = false;
		isDragging = false;
		dragStartY = 0;
		AllItems = new List<AllItemRef>();
		_OriginalAllItems = new List<AllItemRef>();
		strFindItem = "";
		titleInput = new string[1] { "Nhập tên hoặc id item cần tìm kiếm" };
		_isInitialized = false;
	}

	public static DataItem getInstance()
	{
		if (_Instance == null)
		{
			_Instance = new DataItem();
		}
		return _Instance;
	}

	private static void CalculatePanelMetrics()
	{
		PanelX = 1;
		PanelY = 1;
		PanelWidth = GameCanvas.w - PanelX * 2;
		PanelHeight = GameCanvas.h - PanelY * 2;
		ContentHeight = PanelHeight - 18 - 8 - 20;
		Rows = System.Math.Max(1, ContentHeight / 45);
		ItemWidth = (PanelWidth - 12 - 2) / 3;
	}

	public static void LoadAllItemsToOriginalList()
	{
		_OriginalAllItems.Clear();
		List<AllItemRef> list = new List<AllItemRef>();
		try
		{
			if (ItemTemplates.itemTemplates == null)
			{
				return;
			}
			IDictionaryEnumerator enumerator = ItemTemplates.itemTemplates.GetEnumerator();
			while (enumerator.MoveNext())
			{
				try
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
					if (dictionaryEntry.Key != null)
					{
						short id = ((dictionaryEntry.Key is short num) ? num : ((short)(int)dictionaryEntry.Key));
						ItemTemplate itemTemplate = ItemTemplates.get(id);
						if (itemTemplate != null && !string.IsNullOrEmpty(itemTemplate.name) && itemTemplate.id >= 0)
						{
							list.Add(new AllItemRef
							{
								Name = (itemTemplate.name ?? "Unknown"),
								Id = id,
								IconId = itemTemplate.iconID
							});
						}
					}
				}
				catch
				{
				}
			}
			list.Sort((AllItemRef a, AllItemRef b) => b.Id.CompareTo(a.Id));
			_OriginalAllItems = list;
		}
		catch
		{
			_OriginalAllItems = new List<AllItemRef>();
		}
	}

	private static string RemoveDiacritics(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		string text2 = text.Normalize(NormalizationForm.FormD);
		StringBuilder stringBuilder = new StringBuilder();
		string text3 = text2;
		string text4 = text3;
		string text5 = text4;
		foreach (char c in text5)
		{
			if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString().Normalize(NormalizationForm.FormC).Replace("đ", "d")
			.Replace("Đ", "D");
	}

	public static void FilterItems()
	{
		SelectedIndex = -1;
		scrollPixelOffset = 0f;
		scrollVelocity = 0f;
		velocityHistory.Clear();
		if (_OriginalAllItems == null || _OriginalAllItems.Count == 0)
		{
			LoadAllItemsToOriginalList();
		}
		if (_OriginalAllItems == null)
		{
			_OriginalAllItems = new List<AllItemRef>();
		}
		if (string.IsNullOrEmpty(strFindItem))
		{
			AllItems = _OriginalAllItems.Where((AllItemRef x) => x != null).ToList();
		}
		else
		{
			string searchLower = strFindItem.ToLower();
			string searchUnsigned = RemoveDiacritics(searchLower);
			if (int.TryParse(strFindItem, out var searchId))
			{
				AllItems = _OriginalAllItems.Where((AllItemRef x) => x != null && x.Id == searchId).ToList();
			}
			else
			{
				AllItems = _OriginalAllItems.Where(delegate(AllItemRef item)
				{
					if (item == null || string.IsNullOrEmpty(item.Name))
					{
						return false;
					}
					string text = item.Name.ToLower();
					string text2 = RemoveDiacritics(text);
					return text2.Contains(searchUnsigned) || text.Contains(searchLower);
				}).ToList();
			}
		}
		if (AllItems == null)
		{
			AllItems = new List<AllItemRef>();
		}
		ClampScrollOffset();
	}

	public static void ResetFilter()
	{
		strFindItem = "";
		FilterItems();
	}

	private static float GetMaxScrollPixels()
	{
		if (AllItems == null || AllItems.Count == 0)
		{
			return 0f;
		}
		int num = (AllItems.Count + 3 - 1) / 3;
		return System.Math.Max(0f, num * 45 - ContentHeight);
	}

	private static void ClampScrollOffset()
	{
		float maxScrollPixels = GetMaxScrollPixels();
		scrollPixelOffset = System.Math.Max(0f, System.Math.Min(scrollPixelOffset, maxScrollPixels));
	}

	private static void UpdateScrollPhysics()
	{
		long num = mSystem.currentTimeMillis();
		float num2 = ((lastFrameTime == 0L) ? 0.016f : ((float)(num - lastFrameTime) / 1000f));
		lastFrameTime = num;
		if (isDragging)
		{
			scrollVelocity *= 0.97f;
		}
		else if (System.Math.Abs(scrollVelocity) > 0.3f)
		{
			scrollPixelOffset += scrollVelocity * num2 * 60f;
			scrollVelocity *= (float)System.Math.Pow(0.9599999785423279, num2 * 60f);
			ClampScrollOffset();
		}
		else
		{
			scrollVelocity = 0f;
		}
	}

    public static void Paint(mGraphics g)
    {
        if (!IsShow) return;

        // Init
        if (!_isInitialized)
        {
            CalculatePanelMetrics();
            if (_OriginalAllItems == null || _OriginalAllItems.Count == 0)
                LoadAllItemsToOriginalList();

            if (AllItems == null || AllItems.Count == 0)
                FilterItems();

            _isInitialized = true;
        }

        if (PanelWidth == 0 || PanelHeight == 0)
            CalculatePanelMetrics();

        AllItems ??= new List<AllItemRef>();
        _OriginalAllItems ??= new List<AllItemRef>();

        UpdateScrollPhysics();

        // ===== PANEL =====
        try
        {
            g.setColor(1973790);
            g.fillRoundRect(PanelX, PanelY, PanelWidth, PanelHeight, 12, 12);
            g.setColor(54527);
            g.drawRoundRect(PanelX, PanelY, PanelWidth, PanelHeight, 12, 12);

            g.setColor(2434341);
            g.fillRoundRect(PanelX + 2, PanelY + 2, PanelWidth - 4, 22, 10, 10);

            string title = $"DANH SÁCH VẬT PHẨM ({AllItems.Count}/{_OriginalAllItems.Count})";
            mFont.tahoma_7b_white.drawString(g, title, PanelX + PanelWidth / 2, PanelY + 7, mFont.CENTER);
        }
        catch { }

        // ===== BUTTON =====
        int btnY = PanelY + 18 + 4;
        int btnX = PanelX + 4;
        int resetX = btnX;
        int searchX = btnX + 60 + 5;

        bool canHover = !isDragging;
        isResetButtonHovered = canHover && GameCanvas.isPointer(resetX, btnY, 60, 20);
        isSearchButtonHovered = canHover && GameCanvas.isPointer(searchX, btnY, 60, 20);

        // Reset
        g.setColor(isResetButtonHovered ? 54527 : 43212);
        g.fillRoundRect(resetX, btnY, 60, 20, 5, 5);
        g.setColor(16777215);
        g.drawRoundRect(resetX, btnY, 60, 20, 5, 5);
        mFont.tahoma_7b_white.drawString(g, "Xem Tất cả", resetX + 30, btnY + 5, mFont.CENTER);

        // Search
        g.setColor(isSearchButtonHovered ? 54527 : 43212);
        g.fillRoundRect(searchX, btnY, 60, 20, 5, 5);
        g.setColor(16777215);
        g.drawRoundRect(searchX, btnY, 60, 20, 5, 5);
        mFont.tahoma_7b_white.drawString(g, "Tìm kiếm", searchX + 30, btnY + 5, mFont.CENTER);

        // ===== STATUS TEXT =====
        string status;
        if (string.IsNullOrEmpty(strFindItem))
        {
            status = "Đang xem: Tất cả";
        }
        else
        {
            string t = strFindItem.Trim();
            if (t.Length > 15) t = t.Substring(0, 12) + "...";
            status = $"Đã lọc: \"{t}\"";
        }
        mFont.tahoma_7_white.drawString(
            g,
            status,
            PanelX + PanelWidth - 8,
            btnY + 5,
            mFont.RIGHT
        );

        // ===== CONTENT =====
        int contentY = btnY + 20 + 4;
        int count = AllItems.Count;

        g.setClip(PanelX + 2, contentY, PanelWidth - 4, ContentHeight);

        if (count == 0)
        {
            // ⚠️ KHÔNG RETURN → để vẽ Close
            mFont.tahoma_7_white.drawString(
                g,
                "Không có vật phẩm nào",
                PanelX + PanelWidth / 2,
                contentY + ContentHeight / 2,
                mFont.CENTER
            );
        }
        else
        {
            int totalRows = (count + 3 - 1) / 3;
            int firstRow = (int)(scrollPixelOffset / 45f);
            int lastRow = Math.Min(totalRows - 1, firstRow + Rows);
            int baseY = contentY - (int)scrollPixelOffset;

            for (int i = firstRow; i <= lastRow; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int index = i * 3 + j;
                    if (index >= count) break;

                    AllItemRef item = AllItems[index];
                    if (item == null) continue;

                    int xItem = PanelX + j * ItemWidth + 2;
                    int yItem = baseY + i * 45;

                    int bgColor =
                        index == SelectedIndex ? 675182 :
                        index == hoveredIndex ? 3487029 : 2763306;

                    g.setColor(bgColor);
                    g.fillRoundRect(xItem, yItem + 1, ItemWidth - 4, 43, 6, 6);

                    if (index == SelectedIndex)
                    {
                        g.setColor(54527);
                        g.drawRoundRect(xItem, yItem + 1, ItemWidth - 4, 43, 6, 6);
                    }

                    if (item.IconId != -1)
                    {
                        try
                        {
                            SmallImage.drawSmallImage(
                                g,
                                item.IconId,
                                xItem + (ItemWidth - 4) / 2,
                                yItem + 10,
                                0,
                                3
                            );
                        }
                        catch { }
                    }

                    string name = string.IsNullOrEmpty(item.Name) ? "Unknown" : item.Name;
                    string label = $"{name} [{item.Id}]";
                    mFont font = index == SelectedIndex ? mFont.tahoma_7b_white : mFont.tahoma_7_white;

                    try
                    {
                        font.drawString(
                            g,
                            label,
                            xItem + (ItemWidth - 4) / 2,
                            yItem + 30,
                            mFont.CENTER
                        );
                    }
                    catch { }
                }
            }
        }

        g.setClip(0, 0, GameCanvas.w, GameCanvas.h);

        // ===== SCROLLBAR & CLOSE =====
        DrawScrollBar(g, PanelX, contentY);

        int closeX = PanelX + PanelWidth - GameCanvas.panel.cmdClose.img.w - 4;
        g.drawImage(GameCanvas.panel.cmdClose.img, closeX, PanelY + 2, 0);
    }


    private static void DrawScrollBar(mGraphics g, int panelX, int contentY)
	{
		float maxScrollPixels = GetMaxScrollPixels();
		if (!(maxScrollPixels <= 0f))
		{
			int num = panelX + PanelWidth - 12 - 4;
			int num2 = contentY + 2;
			int num3 = ContentHeight - 4;
			g.setColor(1381653);
			g.fillRoundRect(num, num2, 12, num3, 6, 6);
			int num4 = (AllItems.Count + 3 - 1) / 3;
			float num5 = (float)ContentHeight / (float)(num4 * 45);
			int num6 = System.Math.Max(30, System.Math.Min((int)((float)num3 * num5), num3 - 4));
			int num7 = num3 - num6;
			int num8 = num2 + (int)((float)num7 * (scrollPixelOffset / maxScrollPixels));
			int color = (isScrollbarHovered ? 54527 : 43212);
			g.setColor(color);
			g.fillRoundRect(num + 1, num8, 10, num6 - 2, 5, 5);
			g.setColor(16777215);
			g.fillRect(num + 3, num8 + (num6 - 2) / 2 - 1, 6, 2);
		}
	}

	public static void HandleInput()
	{
		if (!IsShow || ChatTextField.gI().isShow)
		{
			return;
		}
		if (PanelWidth == 0 || PanelHeight == 0)
		{
			CalculatePanelMetrics();
		}
		int num = PanelY + 18 + 4;
		int num2 = num + 20 + 4;
		int num3 = PanelX + 4;
		int x = num3;
		int x2 = num3 + 60 + 5;
		if (GameCanvas.isPointerDown && !isDragging && GameCanvas.isPointer(PanelX, num2, PanelWidth, ContentHeight))
		{
			isDragging = true;
			dragStartY = GameCanvas.py;
			scrollVelocity = 0f;
			velocityHistory.Clear();
		}
		if (isDragging)
		{
			if (GameCanvas.isPointerDown)
			{
				int num4 = GameCanvas.py - GameCanvas.pyLast;
				if (num4 != 0)
				{
					velocityHistory.Enqueue(num4);
					if (velocityHistory.Count > 3)
					{
						velocityHistory.Dequeue();
					}
					float num5 = velocityHistory.Average();
					float num6 = scrollPixelOffset;
					scrollPixelOffset -= num5 * 1.2f;
					ClampScrollOffset();
					if (System.Math.Abs(num6 - scrollPixelOffset) > 0.5f)
					{
						scrollVelocity = (0f - num5) * 1.2f * 0.4f;
					}
					GameCanvas.pyLast = GameCanvas.py;
				}
				hoveredIndex = GetFocusedItemIndex(PanelX, num2);
			}
			else
			{
				isDragging = false;
			}
		}
		else
		{
			hoveredIndex = (GameCanvas.isPointer(PanelX, num2, PanelWidth, ContentHeight) ? GetFocusedItemIndex(PanelX, num2) : (-1));
		}
		int x3 = PanelX + PanelWidth - 12 - 4;
		isScrollbarHovered = GameCanvas.isPointer(x3, num2, 12, ContentHeight);
		if (!GameCanvas.isPointerJustRelease)
		{
			return;
		}
		int x4 = PanelX + PanelWidth - GameCanvas.panel.cmdClose.img.w - 4;
		if (GameCanvas.isPointer(x4, PanelY + 2, GameCanvas.panel.cmdClose.img.w, GameCanvas.panel.cmdClose.img.h))
		{
			IsShow = false;
			ResetInputState();
			return;
		}
		if (System.Math.Abs(scrollVelocity) < 4f)
		{
			if (GameCanvas.isPointer(x, num, 60, 20))
			{
				ResetFilter();
				ResetInputState();
				return;
			}
			if (GameCanvas.isPointer(x2, num, 60, 20))
			{
				openFormChat();
				isDragging = false;
				velocityHistory.Clear();
				scrollVelocity = 0f;
				hoveredIndex = -1;
				isScrollbarHovered = false;
				return;
			}
		}
		if (System.Math.Abs(scrollVelocity) < 4f && GameCanvas.isPointer(PanelX, num2, PanelWidth, ContentHeight))
		{
			int focusedItemIndex = GetFocusedItemIndex(PanelX, num2);
			if (focusedItemIndex != -1)
			{
				SelectedIndex = focusedItemIndex;
			}
		}
		ResetInputState();
	}

	private static void ResetInputState()
	{
		isDragging = false;
		velocityHistory.Clear();
		GameCanvas.clearAllPointerEvent();
	}

	private static int GetFocusedItemIndex(int panelX, int contentY)
	{
		int px = GameCanvas.px;
		int py = GameCanvas.py;
		int num = PanelWidth - 12 - 2;
		if (px < panelX + 2 || px > panelX + num || py < contentY || py > contentY + ContentHeight)
		{
			return -1;
		}
		int num2 = py - contentY + (int)scrollPixelOffset;
		int num3 = num2 / 45;
		int num4 = (px - panelX - 2) / ItemWidth;
		int num5 = num3 * 3 + num4;
		if (AllItems == null || num5 < 0 || num5 >= AllItems.Count)
		{
			return -1;
		}
		if (AllItems[num5] == null)
		{
			return -1;
		}
		int num6 = contentY + num3 * 45 - (int)scrollPixelOffset;
		int num7 = panelX + num4 * ItemWidth + 2;
		if (py >= num6 && py < num6 + 45 && px >= num7 && px < num7 + ItemWidth)
		{
			return num5;
		}
		return -1;
	}

	public static void ToggleMenu()
	{
		IsShow = !IsShow;
		if (IsShow)
		{
			CalculatePanelMetrics();
			if (_OriginalAllItems.Count == 0)
			{
				LoadAllItemsToOriginalList();
			}
			strFindItem = "";
			FilterItems();
			SelectedIndex = -1;
			_isInitialized = true;
		}
		ResetInputState();
	}

	public void onChatFromMe(string text, string to)
	{
		if (text == null || text.Trim() == "")
		{
			ChatTextField.gI().isShow = false;
			ResetChatTextField();
			return;
		}
		if (ChatTextField.gI().strChat.Equals(titleInput[0]))
		{
			strFindItem = text.Trim();
			FilterItems();
		}
		else
		{
			ResetChatTextField();
			Service.gI().chat(text);
		}
		ResetChatTextField();
	}

	private static void ResetChatTextField()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().isShow = false;
	}

	public void onCancelChat()
	{
		if (GameScr.isPaintMessage)
		{
			GameScr.isPaintMessage = false;
			ChatTextField.gI().center = null;
		}
	}

	public static void openFormChat()
	{
		ChatTextField.gI().strChat = titleInput[0];
		ChatTextField.gI().tfChat.name = "Nhập tên hoặc id item cần tìm kiếm";
		ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_ANY);
		ChatTextField.gI().tfChat.setText(strFindItem);
		ChatTextField.gI().startChat2(getInstance(), string.Empty);
	}
}
