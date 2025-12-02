using System.Collections.Generic;

namespace DoHoa.CustomMenu.Shared;

public static class MenuConstants
{
	private const int RED = 16711680;

	private const int DARK_RED = 12986408;

	public const int PANEL_BG_COLOR = 2894892;

	public const int PANEL_BORDER_COLOR = 49151;

	public const int SELECTED_TAB_COLOR = 49151;

	public const int UNSELECTED_TAB_COLOR = 4079166;

	public const int ITEM_BG_UNSELECTED = 3815994;

	public const int ITEM_BG_SELECTED = 6052956;

	public const int ICON_BG_UNSELECTED = 4934475;

	public const int ICON_BG_SELECTED = 33679;

	public const int SCROLL_TRACK_COLOR = 5066061;

	public const int SCROLL_HANDLE_COLOR = 49151;

	public const int SAVE_BUTTON_COLOR = 6732650;

	public const int RESET_BUTTON_COLOR = 15684432;

	public const int CHONHET_BUTTON_COLOR = 9268835;

	public const int TRAIN_DISABLED_COLOR = 7697781;

	public const int NEBOSS_ON_COLOR = 16754470;

	public const int NEBOSS_OFF_COLOR = 8026746;

	public const int TRAIN_READY_COLOR = 5025616;

	public const int TRAIN_OFF_COLOR = 16757504;

	public const int TRAIN_ON_COLOR_BLINK1 = 16711680;

	public const int TRAIN_ON_COLOR_BLINK2 = 12986408;

	public const int AVOID_ON_COLOR = 15684432;

	public const int AVOID_OFF_COLOR = 8026746;

	public const int FUSION_ON_COLOR = 6732650;

	public const int FUSION_OFF_COLOR = 8026746;

	public const int TDLT_ON_COLOR = 14172949;

	public const int TDLT_OFF_COLOR = 8026746;

	public const int SELECTBYTYPE_COLOR = 10586239;

	public const int GOBACK_ON_COLOR = 11225020;

	public const int GOBACK_OFF_COLOR = 8026746;

	public const int ItemHeight = 32;

	public const int CheckBoxSize = 18;

	public const int BasePanelWidth = 120;

	public const int ToggleButtonHeight = 18;

	public const int ItemPadding = 8;

	public const int HEADER_HEIGHT = 30;

	public const int CONTENT_START_MARGIN = 8;

	public const int CONTENT_END_MARGIN = 8;

	public const int SCROLL_WIDTH = 2;

	public const int DRAG_THRESHOLD = 10;

	public static readonly HashSet<int> ExcludedSkillIds = new HashSet<int>(new int[15]
	{
		1, 3, 5, 8, 9, 10, 11, 14, 18, 20,
		22, 23, 24, 25, 26
	});
}
