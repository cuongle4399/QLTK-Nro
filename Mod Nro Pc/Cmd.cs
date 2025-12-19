public class Cmd
{
	public const sbyte CMD_EXTRA_BIG = 12;

	public const sbyte CMD_EXTRA = 24;

	public const sbyte EXTRA_LINK = sbyte.MaxValue;

	public const sbyte LOGIN = 0;

	public const sbyte REGISTER = 1;

	public const sbyte CLIENT_INFO = 2;

	public const sbyte SEND_SMS = 3;

	public const sbyte REGISTER_IMAGE = 4;

	public const sbyte MESSAGE_TIME = 65;

	public const sbyte LOGOUT = 0;

	public const sbyte SELECT_PLAYER = 1;

	public const sbyte CREATE_PLAYER = 2;

	public const sbyte DELETE_PLAYER = 3;

	public const sbyte UPDATE_VERSION = 4;

	public const sbyte UPDATE_MAP = 6;

	public const sbyte UPDATE_SKILL = 7;

	public const sbyte UPDATE_ITEM = 8;

	public const sbyte REQUEST_SKILL = 9;

	public const sbyte REQUEST_MAPTEMPLATE = 10;

	public const sbyte REQUEST_MOB_TEMPLATE = 11;

	public const sbyte UPDATE_TYPE_PK = 35;

	public const sbyte PLAYER_ATTACK_PLAYER = -60;

	public const sbyte PLAYER_VS_PLAYER = -59;

	public const sbyte CLAN_PARTY = -58;

	public const sbyte CLAN_INVITE = -57;

	public const sbyte CLAN_REMOTE = -56;

	public const sbyte CLAN_LEAVE = -55;

	public const sbyte CLAN_DONATE = -54;

	public const sbyte CLAN_MESSAGE = -51;

	public const sbyte CLAN_UPDATE = -52;

	public const sbyte CLAN_INFO = -53;

	public const sbyte CLAN_JOIN = -49;

	public const sbyte CLAN_MEMBER = -50;

	public const sbyte CLAN_SEARCH = -47;

	public const sbyte CLAN_CREATE_INFO = -46;

	public const sbyte CLIENT_OK = 13;

	public const sbyte CLIENT_OK_INMAP = 14;

	public const sbyte UPDATE_VERSION_OK = 15;

	public const sbyte INPUT_CARD = 16;

	public const sbyte CLEAR_TASK = 17;

	public const sbyte CHANGE_NAME = 18;

	public const sbyte UPDATE_PK = 20;

	public const sbyte CREATE_CLAN = 21;

	public const sbyte CONVERT_UPGRADE = 33;

	public const sbyte INVITE_CLANDUN = 34;

	public const sbyte NOT_USEACC = 35;

	public const sbyte ME_LOAD_ACTIVE = 36;

	public const sbyte ME_ACTIVE = 37;

	public const sbyte ME_UPDATE_ACTIVE = 38;

	public const sbyte ME_OPEN_LOCK = 39;

	public const sbyte ITEM_SPLIT = 40;

	public const sbyte ME_CLEAR_LOCK = 41;

	public const sbyte GET_IMG_BY_NAME = 66;

	public const sbyte ME_LOAD_ALL = 0;

	public const sbyte ME_LOAD_CLASS = 1;

	public const sbyte ME_LOAD_SKILL = 2;

	public const sbyte ME_LOAD_INFO = 4;

	public const sbyte ME_LOAD_HP = 5;

	public const sbyte ME_LOAD_MP = 6;

	public const sbyte PLAYER_LOAD_ALL = 7;

	public const sbyte PLAYER_SPEED = 8;

	public const sbyte PLAYER_LOAD_LEVEL = 9;

	public const sbyte PLAYER_LOAD_VUKHI = 10;

	public const sbyte PLAYER_LOAD_AO = 11;

	public const sbyte PLAYER_LOAD_QUAN = 12;

	public const sbyte PLAYER_LOAD_BODY = 13;

	public const sbyte PLAYER_LOAD_HP = 14;

	public const sbyte PLAYER_LOAD_LIVE = 15;

	public const sbyte GOTO_PLAYER = 18;

	public const sbyte POTENTIAL_UP = 16;

	public const sbyte SKILL_UP = 17;

	public const sbyte BAG_SORT = 18;

	public const sbyte BOX_SORT = 19;

	public const sbyte BOX_COIN_OUT = 21;

	public const sbyte REQUEST_ITEM = 22;

	public const sbyte ME_ADD_SKILL = 23;

	public const sbyte ME_UPDATE_SKILL = 62;

	public const sbyte GET_PLAYER_MENU = 63;

	public const sbyte PLAYER_MENU_ACTION = 64;

	public const sbyte SAVE_RMS = 60;

	public const sbyte LOAD_RMS = 61;

	public const sbyte USE_BOOK_SKILL = 43;

	public const sbyte LOCK_INVENTORY = -104;

	public const sbyte CHANGE_FLAG = -103;

	public const sbyte LOGINFAIL = -102;

	public const sbyte LOGIN2 = -101;

	public const sbyte KIGUI = -100;

	public const sbyte ENEMY_LIST = -99;

	public const sbyte ANDROID_IAP = -98;

	public const sbyte UPDATE_ACTIVEPOINT = -97;

	public const sbyte TOP = -96;

	public const sbyte MOB_ME_UPDATE = -95;

	public const sbyte UPDATE_COOLDOWN = -94;

	public const sbyte BGITEM_VERSION = -93;

	public const sbyte SET_CLIENTTYPE = -92;

	public const sbyte MAP_TRASPORT = -91;

	public const sbyte UPDATE_BODY = -90;

	public const sbyte SERVERSCREEN = -88;

	public const sbyte UPDATE_DATA = -87;

	public const sbyte GIAO_DICH = -86;

	public const sbyte MOB_CAPCHA = -85;

	public const sbyte MOB_MAX_HP = -84;

	public const sbyte CALL_DRAGON = -83;

	public const sbyte TILE_SET = -82;

	public const sbyte COMBINNE = -81;

	public const sbyte FRIEND = -80;

	public const sbyte PLAYER_MENU = -79;

	public const sbyte CHECK_MOVE = -78;

	public const sbyte SMALLIMAGE_VERSION = -77;

	public const sbyte ARCHIVEMENT = -76;

	public const sbyte NPC_BOSS = -75;

	public const sbyte GET_IMAGE_SOURCE = -74;

	public const sbyte NPC_ADD_REMOVE = -73;

	public const sbyte CHAT_PLAYER = -72;

	public const sbyte CHAT_THEGIOI_CLIENT = -71;

	public const sbyte BIG_MESSAGE = -70;

	public const sbyte MAXSTAMINA = -69;

	public const sbyte STAMINA = -68;

	public const sbyte REQUEST_ICON = -67;

	public const sbyte GET_EFFDATA = -66;

	public const sbyte TELEPORT = -65;

	public const sbyte UPDATE_BAG = -64;

	public const sbyte GET_BAG = -63;

	public const sbyte CLAN_IMAGE = -62;

	public const sbyte UPDATE_CLANID = -61;

	public const sbyte SKILL_NOT_FOCUS = -45;

	public const sbyte SHOP = -44;

	public const sbyte USE_ITEM = -43;

	public const sbyte ME_LOAD_POINT = -42;

	public const sbyte UPDATE_CAPTION = -41;

	public const sbyte GET_ITEM = -40;

	public const sbyte FINISH_LOADMAP = -39;

	public const sbyte FINISH_UPDATE = -38;

	public const sbyte BODY = -37;

	public const sbyte BAG = -36;

	public const sbyte BOX = -35;

	public const sbyte MAGIC_TREE = -34;

	public const sbyte MAP_OFFLINE = -33;

	public const sbyte BACKGROUND_TEMPLATE = -32;

	public const sbyte ITEM_BACKGROUND = -31;

	public const sbyte SUB_COMMAND = -30;

	public const sbyte NOT_LOGIN = -29;

	public const sbyte NOT_MAP = -28;

	public const sbyte GET_SESSION_ID = -27;

	public const sbyte DIALOG_MESSAGE = -26;

	public const sbyte SERVER_MESSAGE = -25;

	public const sbyte MAP_INFO = -24;

	public const sbyte MAP_CHANGE = -23;

	public const sbyte MAP_CLEAR = -22;

	public const sbyte ITEMMAP_REMOVE = -21;

	public const sbyte ITEMMAP_MYPICK = -20;

	public const sbyte ITEMMAP_PLAYERPICK = -19;

	public const sbyte ME_THROW = -18;

	public const sbyte ME_DIE = -17;

	public const sbyte ME_LIVE = -16;

	public const sbyte ME_BACK = -15;

	public const sbyte PLAYER_THROW = -14;

	public const sbyte NPC_LIVE = -13;

	public const sbyte NPC_DIE = -12;

	public const sbyte NPC_ATTACK_ME = -11;

	public const sbyte NPC_ATTACK_PLAYER = -10;

	public const sbyte MOB_HP = -9;

	public const sbyte PLAYER_DIE = -8;

	public const sbyte PLAYER_MOVE = -7;

	public const sbyte PLAYER_REMOVE = -6;

	public const sbyte PLAYER_ADD = -5;

	public const sbyte PLAYER_ATTACK_N_P = -4;

	public const sbyte PLAYER_UP_EXP = -3;

	public const sbyte ME_UP_COIN_LOCK = -2;

	public const sbyte ME_CHANGE_COIN = -1;

	public const sbyte ITEM_BUY = 6;

	public const sbyte ITEM_SALE = 7;

	public const sbyte UPPEARL_LOCK = 13;

	public const sbyte UPGRADE = 14;

	public const sbyte PLEASE_INPUT_PARTY = 16;

	public const sbyte ACCEPT_PLEASE_PARTY = 17;

	public const sbyte REQUEST_PLAYERS = 18;

	public const sbyte UPDATE_ACHIEVEMENT = 19;

	public const sbyte PHUBANG_INFO = 20;

	public const sbyte ZONE_CHANGE = 21;

	public const sbyte MENU = 22;

	public const sbyte OPEN_UI = 23;

	public const sbyte OPEN_UI_PT = 25;

	public const sbyte OPEN_UI_SHOP = 26;

	public const sbyte OPEN_MENU_ID = 27;

	public const sbyte OPEN_UI_COLLECT = 28;

	public const sbyte OPEN_UI_ZONE = 29;

	public const sbyte OPEN_UI_TRADE = 30;

	public const sbyte OPEN_UI_SAY = 38;

	public const sbyte OPEN_UI_CONFIRM = 32;

	public const sbyte OPEN_UI_MENU = 33;

	public const sbyte SKILL_SELECT = 34;

	public const sbyte REQUEST_ITEM_INFO = 35;

	public const sbyte TRADE_INVITE = 36;

	public const sbyte TRADE_INVITE_ACCEPT = 37;

	public const sbyte TRADE_LOCK_ITEM = 38;

	public const sbyte TRADE_ACCEPT = 39;

	public const sbyte TASK_GET = 40;

	public const sbyte TASK_NEXT = 41;

	public const sbyte GAME_INFO = 50;

	public const sbyte TASK_UPDATE = 43;

	public const sbyte CHAT_MAP = 44;

	public const sbyte NPC_MISS = 45;

	public const sbyte RESET_POINT = 46;

	public const sbyte ALERT_MESSAGE = 47;

	public const sbyte AUTO_SERVER = 48;

	public const sbyte ALERT_SEND_SMS = 49;

	public const sbyte TRADE_INVITE_CANCEL = 50;

	public const sbyte BOSS_SKILL = 51;

	public const sbyte MABU_HOLD = 52;

	public const sbyte FRIEND_INVITE = 53;

	public const sbyte PLAYER_ATTACK_NPC = 54;

	public const sbyte HAVE_ATTACK_PLAYER = 56;

	public const sbyte OPEN_UI_NEWMENU = 57;

	public const sbyte MOVE_FAST = 58;

	public const sbyte TEST_INVITE = 59;

	public const sbyte ADD_CUU_SAT = 62;

	public const sbyte ME_CUU_SAT = 63;

	public const sbyte CLEAR_CUU_SAT = 64;

	public const sbyte PLAYER_UP_EXPDOWN = 65;

	public const sbyte ME_DIE_EXP_DOWN = 66;

	public const sbyte PLAYER_ATTACK_P_N = 67;

	public const sbyte ITEMMAP_ADD = 68;

	public const sbyte DEL_ACC = 69;

	public const sbyte USE_SKILL_MY_BUFF = 70;

	public const sbyte NPC_CHANGE = 74;

	public const sbyte PARTY_INVITE = 75;

	public const sbyte PARTY_ACCEPT = 76;

	public const sbyte PARTY_CANCEL = 77;

	public const sbyte PLAYER_IN_PARTY = 78;

	public const sbyte PARTY_OUT = 79;

	public const sbyte FRIEND_ADD = 80;

	public const sbyte NPC_IS_DISABLE = 81;

	public const sbyte NPC_IS_MOVE = 82;

	public const sbyte SUMON_ATTACK = 83;

	public const sbyte RETURN_POINT_MAP = 84;

	public const sbyte NPC_IS_FIRE = 85;

	public const sbyte NPC_IS_ICE = 86;

	public const sbyte NPC_IS_WIND = 87;

	public const sbyte OPEN_TEXT_BOX_ID = 88;

	public const sbyte REQUEST_ITEM_PLAYER = 90;

	public const sbyte CHAT_PRIVATE = 91;

	public const sbyte CHAT_THEGIOI_SERVER = 92;

	public const sbyte CHAT_VIP = 93;

	public const sbyte SERVER_ALERT = 94;

	public const sbyte ME_UP_COIN_BAG = 95;

	public const sbyte GET_TASK_ORDER = 96;

	public const sbyte GET_TASK_UPDATE = 97;

	public const sbyte CLEAR_TASK_ORDER = 98;

	public const sbyte ADD_ITEM_MAP = 99;

	public const sbyte TRANSPORT = -105;

	public const sbyte ITEM_TIME = -106;

	public const sbyte PET_INFO = -107;

	public const sbyte PET_STATUS = -108;

	public const sbyte SERVER_DATA = -110;

	public const sbyte CLIENT_INPUT = -125;

	public const sbyte HOLD = -124;

	public const sbyte SHOW_ADS = 121;

	public const sbyte LOGIN_DE = 122;

	public const sbyte SET_POS = 123;

	public const sbyte NPC_CHAT = 124;

	public const sbyte FUSION = 125;

	public const sbyte ANDROID_PACK = 126;

	public const sbyte GET_IMAGE_SOURCE2 = -111;

	public const sbyte CHAGE_MOD_BODY = -112;

	public const sbyte CHANGE_ONSKILL = -113;

	public const sbyte REQUEST_PEAN = -114;

	public const sbyte POWER_INFO = -115;

	public const sbyte AUTOPLAY = -116;

	public const sbyte MABU = -117;

	public const sbyte THACHDAU = -118;

	public const sbyte THELUC = -119;

	public const sbyte UPDATECHAR_MP = -123;

	public const sbyte REFRESH_ITEM = 100;

	public const sbyte CHECK_CONTROLLER = -120;

	public const sbyte CHECK_MAP = -121;

	public const sbyte BIG_BOSS = 101;

	public const sbyte BIG_BOSS_2 = 102;

	public const sbyte DUAHAU = -122;

	public const sbyte QUAYSO = -126;

	public const sbyte USER_INFO = 42;

	public const sbyte OPEN3HOUR = -89;

	public const sbyte STATUS_PET = 31;

	public const sbyte SPEACIAL_SKILL = 112;

	public const sbyte SERVER_EFFECT = 113;

	public const sbyte INAPP = 114;

	public const sbyte LUCKY_ROUND = -127;

	public const sbyte RADA_CARD = sbyte.MaxValue;

	public const sbyte CHAR_EFFECT = sbyte.MinValue;
}
