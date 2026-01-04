using Assets.src.e;
using Assets.src.f;
using Assets.src.g;
using DoHoa.CustomMenu;
using Mod.CuongLe;
using Mod_nro.MenuDataGame;
using System;
using System.Text;
using UnityEngine;

public class Controller : IMessageHandler
{
    protected static Controller me;

    protected static Controller me2;

    public Message messWait;

    public static bool isLoadingData = false;

    public static bool isConnectOK;

    public static bool isConnectionFail;

    public static bool isDisconnected;

    public static bool isMain;

    private float demCount;

    private int move;

    private int total;

    public static bool isStopReadMessage;

    public static bool isGet_CLIENT_INFO = false;

    public static MyHashTable frameHT_NEWBOSS = new MyHashTable();

    public const sbyte PHUBAN_TYPE_CHIENTRUONGNAMEK = 0;

    public const sbyte PHUBAN_START = 0;

    public const sbyte PHUBAN_UPDATE_POINT = 1;

    public const sbyte PHUBAN_END = 2;

    public const sbyte PHUBAN_LIFE = 4;

    public const sbyte PHUBAN_INFO = 5;

    public static bool isEXTRA_LINK = false;

    public static Controller gI()
    {
        if (me == null)
        {
            me = new Controller();
        }
        return me;
    }

    public static Controller gI2()
    {
        if (me2 == null)
        {
            me2 = new Controller();
        }
        return me2;
    }

    public void onConnectOK(bool isMain1)
    {
        isMain = isMain1;
        mSystem.onConnectOK();
    }

    public void onConnectionFail(bool isMain1)
    {
        isMain = isMain1;
        mSystem.onConnectionFail();
    }

    public void onDisconnected(bool isMain1)
    {
        isMain = isMain1;
        mSystem.onDisconnected();
    }

    public void requestItemPlayer(Message msg)
    {
        try
        {
            int num = msg.reader().readUnsignedByte();
            Item item = GameScr.currentCharViewInfo.arrItemBody[num];
            item.saleCoinLock = msg.reader().readInt();
            item.sys = msg.reader().readByte();
            item.options = new MyVector();
            try
            {
                while (true)
                {
                    ItemOption itemOption = readItemOption(msg);
                    if (itemOption != null)
                    {
                        item.options.addElement(itemOption);
                    }
                }
            }
            catch (Exception ex)
            {
                Cout.println("Loi tairequestItemPlayer 1" + ex.ToString());
            }
        }
        catch (Exception ex2)
        {
            Cout.println("Loi tairequestItemPlayer 2" + ex2.ToString());
        }
    }

    public void onMessage(Message msg)
    {
        GameCanvas.debugSession.removeAllElements();
        GameCanvas.debug("SA1", 2);
        try
        {
            if (msg.command != -74)
            {
                Res.outz("=========> [READ] cmd= " + msg.command);
            }
            Char obj = null;
            Mob mob = null;
            MyVector myVector = new MyVector();
            int num = 0;
            GameCanvas.timeLoading = 15;
            Controller2.readMessage(msg);
            switch (msg.command)
            {
                case 12:
                    read_cmdExtraBig(msg);
                    break;
                case 0:
                    readLogin(msg);
                    break;
                case 24:
                    read_cmdExtra(msg);
                    break;
                case 20:
                    phuban_Info(msg);
                    break;
                case 66:
                    readGetImgByName(msg);
                    break;
                case 65:
                    {
                        sbyte id3 = msg.reader().readSByte();
                        string text5 = msg.reader().readUTF();
                        short num245 = msg.reader().readShort();
                        if (ItemTime.isExistMessage(id3))
                        {
                            if (num245 != 0)
                            {
                                ItemTime.getMessageById(id3).initTimeText(id3, text5, num245);
                            }
                            else
                            {
                                GameScr.textTime.removeElement(ItemTime.getMessageById(id3));
                            }
                        }
                        else
                        {
                            ItemTime itemTime = new ItemTime();
                            itemTime.initTimeText(id3, text5, num245);
                            GameScr.textTime.addElement(itemTime);
                        }
                        break;
                    }
                case 112:
                    {
                        sbyte b83 = msg.reader().readByte();
                        Res.outz("spec type= " + b83);
                        switch (b83)
                        {
                            case 0:
                                Panel.spearcialImage = msg.reader().readShort();
                                Panel.specialInfo = msg.reader().readUTF();
                                break;
                            case 1:
                                {
                                    sbyte b84 = msg.reader().readByte();
                                    Char.myCharz().infoSpeacialSkill = new string[b84][];
                                    Char.myCharz().imgSpeacialSkill = new short[b84][];
                                    GameCanvas.panel.speacialTabName = new string[b84][];
                                    for (int num226 = 0; num226 < b84; num226++)
                                    {
                                        GameCanvas.panel.speacialTabName[num226] = new string[2];
                                        string[] array11 = Res.split(msg.reader().readUTF(), "\n", 0);
                                        if (array11.Length == 2)
                                        {
                                            GameCanvas.panel.speacialTabName[num226] = array11;
                                        }
                                        if (array11.Length == 1)
                                        {
                                            GameCanvas.panel.speacialTabName[num226][0] = array11[0];
                                            GameCanvas.panel.speacialTabName[num226][1] = string.Empty;
                                        }
                                        int num227 = msg.reader().readByte();
                                        Char.myCharz().infoSpeacialSkill[num226] = new string[num227];
                                        Char.myCharz().imgSpeacialSkill[num226] = new short[num227];
                                        for (int num228 = 0; num228 < num227; num228++)
                                        {
                                            Char.myCharz().imgSpeacialSkill[num226][num228] = msg.reader().readShort();
                                            Char.myCharz().infoSpeacialSkill[num226][num228] = msg.reader().readUTF();
                                        }
                                    }
                                    GameCanvas.panel.tabName[25] = GameCanvas.panel.speacialTabName;
                                    GameCanvas.panel.setTypeSpeacialSkill();
                                    GameCanvas.panel.show();
                                    break;
                                }
                        }
                        break;
                    }
                case -98:
                    {
                        sbyte b94 = msg.reader().readByte();
                        GameCanvas.menu.showMenu = false;
                        if (b94 == 0)
                        {
                            GameCanvas.startYesNoDlg(msg.reader().readUTF(), new Command(mResources.YES, GameCanvas.instance, 888397, msg.reader().readUTF()), new Command(mResources.NO, GameCanvas.instance, 888396, null));
                        }
                        break;
                    }
                case -97:
                    Char.myCharz().cNangdong = msg.reader().readInt();
                    break;
                case -96:
                    {
                        sbyte typeTop = msg.reader().readByte();
                        GameCanvas.panel.vTop.removeAllElements();
                        string topName = msg.reader().readUTF();
                        sbyte b99 = msg.reader().readByte();
                        for (int num280 = 0; num280 < b99; num280++)
                        {
                            int rank = msg.reader().readInt();
                            int pId = msg.reader().readInt();
                            short headID = msg.reader().readShort();
                            short headICON = msg.reader().readShort();
                            short body = msg.reader().readShort();
                            short leg = msg.reader().readShort();
                            string name = msg.reader().readUTF();
                            string info4 = msg.reader().readUTF();
                            TopInfo topInfo = new TopInfo();
                            topInfo.rank = rank;
                            topInfo.headID = headID;
                            topInfo.headICON = headICON;
                            topInfo.body = body;
                            topInfo.leg = leg;
                            topInfo.name = name;
                            topInfo.info = info4;
                            topInfo.info2 = msg.reader().readUTF();
                            topInfo.pId = pId;
                            GameCanvas.panel.vTop.addElement(topInfo);
                        }
                        GameCanvas.panel.topName = topName;
                        GameCanvas.panel.setTypeTop(typeTop);
                        GameCanvas.panel.show();
                        break;
                    }
                case -94:
                    while (msg.reader().available() > 0)
                    {
                        short num317 = msg.reader().readShort();
                        int num318 = msg.reader().readInt();
                        for (int num319 = 0; num319 < Char.myCharz().vSkill.size(); num319++)
                        {
                            Skill skill = (Skill)Char.myCharz().vSkill.elementAt(num319);
                            if (skill != null && skill.skillId == num317)
                            {
                                if (num318 < skill.coolDown)
                                {
                                    skill.lastTimeUseThisSkill = mSystem.currentTimeMillis() - (skill.coolDown - num318);
                                }
                                Res.outz("1 chieu id= " + skill.template.id + " cooldown= " + num318 + "curr cool down= " + skill.coolDown);
                            }
                        }
                    }
                    break;
                case -95:
                    {
                        sbyte b97 = msg.reader().readByte();
                        Res.outz("type= " + b97);
                        if (b97 == 0)
                        {
                            int num263 = msg.reader().readInt();
                            short templateId = msg.reader().readShort();
                            long num264 = msg.reader().readLong();
                            SoundMn.gI().explode_1();
                            if (num263 == Char.myCharz().charID)
                            {
                                Char.myCharz().mobMe = new Mob(num263, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num264, 0, num264, (short)(Char.myCharz().cx + ((Char.myCharz().cdir != 1) ? (-40) : 40)), (short)Char.myCharz().cy, 4, 0);
                                Char.myCharz().mobMe.isMobMe = true;
                                EffecMn.addEff(new Effect(18, Char.myCharz().mobMe.x, Char.myCharz().mobMe.y, 2, 10, -1));
                                Char.myCharz().tMobMeBorn = 30;
                                GameScr.vMob.addElement(Char.myCharz().mobMe);
                            }
                            else
                            {
                                obj = GameScr.findCharInMap(num263);
                                if (obj != null)
                                {
                                    Mob mob10 = new Mob(num263, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num264, 0, num264, (short)obj.cx, (short)obj.cy, 4, 0);
                                    mob10.isMobMe = true;
                                    obj.mobMe = mob10;
                                    GameScr.vMob.addElement(obj.mobMe);
                                }
                                else
                                {
                                    Mob mob11 = GameScr.findMobInMap(num263);
                                    if (mob11 == null)
                                    {
                                        mob11 = new Mob(num263, isDisable: false, isDontMove: false, isFire: false, isIce: false, isWind: false, templateId, 1, num264, 0, num264, -100, -100, 4, 0);
                                        mob11.isMobMe = true;
                                        GameScr.vMob.addElement(mob11);
                                    }
                                }
                            }
                        }
                        if (b97 == 1)
                        {
                            int num265 = msg.reader().readInt();
                            int mobId = msg.reader().readByte();
                            Res.outz("mod attack id= " + num265);
                            if (num265 == Char.myCharz().charID)
                            {
                                if (GameScr.findMobInMap(mobId) != null)
                                {
                                    Char.myCharz().mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
                                }
                            }
                            else
                            {
                                obj = GameScr.findCharInMap(num265);
                                if (obj != null && GameScr.findMobInMap(mobId) != null)
                                {
                                    obj.mobMe.attackOtherMob(GameScr.findMobInMap(mobId));
                                }
                            }
                        }
                        if (b97 == 2)
                        {
                            int num266 = msg.reader().readInt();
                            int num267 = msg.reader().readInt();
                            long num268 = msg.reader().readLong();
                            long cHPNew = msg.reader().readLong();
                            if (num266 == Char.myCharz().charID)
                            {
                                Res.outz("mob dame= " + num268);
                                obj = GameScr.findCharInMap(num267);
                                if (obj != null)
                                {
                                    obj.cHPNew = cHPNew;
                                    if (Char.myCharz().mobMe.isBusyAttackSomeOne)
                                    {
                                        obj.doInjure(num268, 0L, isCrit: false, isMob: true);
                                    }
                                    else
                                    {
                                        Char.myCharz().mobMe.dame = num268;
                                        Char.myCharz().mobMe.setAttack(obj);
                                    }
                                }
                            }
                            else
                            {
                                mob = GameScr.findMobInMap(num266);
                                if (mob != null)
                                {
                                    if (num267 == Char.myCharz().charID)
                                    {
                                        Char.myCharz().cHPNew = cHPNew;
                                        if (mob.isBusyAttackSomeOne)
                                        {
                                            Char.myCharz().doInjure(num268, 0L, isCrit: false, isMob: true);
                                        }
                                        else
                                        {
                                            mob.dame = num268;
                                            mob.setAttack(Char.myCharz());
                                        }
                                    }
                                    else
                                    {
                                        obj = GameScr.findCharInMap(num267);
                                        if (obj != null)
                                        {
                                            obj.cHPNew = cHPNew;
                                            if (mob.isBusyAttackSomeOne)
                                            {
                                                obj.doInjure(num268, 0L, isCrit: false, isMob: true);
                                            }
                                            else
                                            {
                                                mob.dame = num268;
                                                mob.setAttack(obj);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (b97 == 3)
                        {
                            int num269 = msg.reader().readInt();
                            int mobId2 = msg.reader().readInt();
                            long hp = msg.reader().readLong();
                            long num270 = msg.reader().readLong();
                            obj = null;
                            obj = ((Char.myCharz().charID != num269) ? GameScr.findCharInMap(num269) : Char.myCharz());
                            if (obj != null)
                            {
                                mob = GameScr.findMobInMap(mobId2);
                                if (obj.mobMe != null)
                                {
                                    obj.mobMe.attackOtherMob(mob);
                                }
                                if (mob != null)
                                {
                                    mob.hp = hp;
                                    mob.updateHp_bar();
                                    if (num270 == 0)
                                    {
                                        mob.x = mob.xFirst;
                                        mob.y = mob.yFirst;
                                        GameScr.startFlyText(mResources.miss, mob.x, mob.y - mob.h, 0, -2, mFont.MISS);
                                    }
                                    else
                                    {
                                        GameScr.startFlyText("-" + num270, mob.x, mob.y - mob.h, 0, -2, mFont.ORANGE);
                                    }
                                }
                            }
                        }
                        if (b97 == 4)
                        {
                        }
                        if (b97 == 5)
                        {
                            int num271 = msg.reader().readInt();
                            sbyte b98 = msg.reader().readByte();
                            int mobId3 = msg.reader().readInt();
                            long num272 = msg.reader().readLong();
                            long hp2 = msg.reader().readLong();
                            obj = null;
                            obj = ((num271 != Char.myCharz().charID) ? GameScr.findCharInMap(num271) : Char.myCharz());
                            if (obj == null)
                            {
                                return;
                            }
                            if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
                            {
                                obj.setSkillPaint(GameScr.sks[b98], 0);
                            }
                            else
                            {
                                obj.setSkillPaint(GameScr.sks[b98], 1);
                            }
                            Mob mob12 = GameScr.findMobInMap(mobId3);
                            if (obj.cx <= mob12.x)
                            {
                                obj.cdir = 1;
                            }
                            else
                            {
                                obj.cdir = -1;
                            }
                            obj.mobFocus = mob12;
                            mob12.hp = hp2;
                            mob12.updateHp_bar();
                            GameCanvas.debug("SA83v2", 2);
                            if (num272 == 0)
                            {
                                mob12.x = mob12.xFirst;
                                mob12.y = mob12.yFirst;
                                GameScr.startFlyText(mResources.miss, mob12.x, mob12.y - mob12.h, 0, -2, mFont.MISS);
                            }
                            else
                            {
                                GameScr.startFlyText("-" + num272, mob12.x, mob12.y - mob12.h, 0, -2, mFont.ORANGE);
                            }
                        }
                        if (b97 == 6)
                        {
                            int num273 = msg.reader().readInt();
                            if (num273 == Char.myCharz().charID)
                            {
                                Char.myCharz().mobMe.startDie();
                            }
                            else
                            {
                                GameScr.findCharInMap(num273)?.mobMe.startDie();
                            }
                        }
                        if (b97 != 7)
                        {
                            break;
                        }
                        int num274 = msg.reader().readInt();
                        if (num274 == Char.myCharz().charID)
                        {
                            Char.myCharz().mobMe = null;
                            for (int num275 = 0; num275 < GameScr.vMob.size(); num275++)
                            {
                                if (((Mob)GameScr.vMob.elementAt(num275)).mobId == num274)
                                {
                                    GameScr.vMob.removeElementAt(num275);
                                }
                            }
                            break;
                        }
                        obj = GameScr.findCharInMap(num274);
                        for (int num276 = 0; num276 < GameScr.vMob.size(); num276++)
                        {
                            if (((Mob)GameScr.vMob.elementAt(num276)).mobId == num274)
                            {
                                GameScr.vMob.removeElementAt(num276);
                            }
                        }
                        if (obj != null)
                        {
                            obj.mobMe = null;
                        }
                        break;
                    }
                case -92:
                    Main.typeClient = msg.reader().readByte();
                    if (Rms.loadRMSString("ResVersion") == null)
                    {
                        Rms.clearAll();
                    }
                    Rms.saveRMSInt("clienttype", Main.typeClient);
                    Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
                    if (Rms.loadRMSString("ResVersion") == null)
                    {
                        GameCanvas.startOK(mResources.plsRestartGame, 8885, null);
                    }
                    break;
                case -91:
                    {
                        sbyte b70 = msg.reader().readByte();
                        GameCanvas.panel.mapNames = new string[b70];
                        GameCanvas.panel.planetNames = new string[b70];
                        for (int num181 = 0; num181 < b70; num181++)
                        {
                            GameCanvas.panel.mapNames[num181] = msg.reader().readUTF();
                            GameCanvas.panel.planetNames[num181] = msg.reader().readUTF();
                        }
                        GameCanvas.panel.setTypeMapTrans();
                        GameCanvas.panel.show();
                        break;
                    }
                case -90:
                    {
                        sbyte b106 = msg.reader().readByte();
                        int num295 = msg.reader().readInt();
                        Res.outz("===> UPDATE_BODY:    type = " + b106);
                        obj = ((Char.myCharz().charID != num295) ? GameScr.findCharInMap(num295) : Char.myCharz());
                        if (b106 != -1)
                        {
                            short num296 = msg.reader().readShort();
                            short num297 = msg.reader().readShort();
                            short num298 = msg.reader().readShort();
                            sbyte isMonkey = msg.reader().readByte();
                            if (obj != null)
                            {
                                if (obj.charID == num295)
                                {
                                    obj.isMask = true;
                                    obj.isMonkey = isMonkey;
                                    if (obj.isMonkey != 0)
                                    {
                                        obj.isWaitMonkey = false;
                                        obj.isLockMove = false;
                                    }
                                }
                                else if (obj != null)
                                {
                                    obj.isMask = true;
                                    obj.isMonkey = isMonkey;
                                }
                                if (num296 != -1)
                                {
                                    obj.head = num296;
                                }
                                if (num297 != -1)
                                {
                                    obj.body = num297;
                                }
                                if (num298 != -1)
                                {
                                    obj.leg = num298;
                                }
                            }
                        }
                        if (b106 == -1 && obj != null)
                        {
                            obj.isMask = false;
                            obj.isMonkey = 0;
                        }
                        if (obj == null)
                        {
                            break;
                        }
                        for (int num299 = 0; num299 < 54; num299++)
                        {
                            obj.removeEffChar(0, 201 + num299);
                        }
                        if (obj.bag >= 201 && obj.bag < 255)
                        {
                            Effect effect2 = new Effect(obj.bag, obj, 2, -1, 10, 1);
                            effect2.typeEff = 5;
                            obj.addEffChar(effect2);
                        }
                        if (obj.bag == 30 && obj.me)
                        {
                            GameScr.isPickNgocRong = true;
                        }
                        if (!obj.me)
                        {
                            break;
                        }
                        GameScr.isudungCapsun4 = false;
                        GameScr.isudungCapsun3 = false;
                        for (int num300 = 0; num300 < Char.myCharz().arrItemBag.Length; num300++)
                        {
                            Item item5 = Char.myCharz().arrItemBag[num300];
                            if (item5 == null)
                            {
                                continue;
                            }
                            if (item5.template.id == 194)
                            {
                                GameScr.isudungCapsun4 = item5.quantity > 0;
                                if (GameScr.isudungCapsun4)
                                {
                                    break;
                                }
                            }
                            else if (item5.template.id == 193)
                            {
                                GameScr.isudungCapsun3 = item5.quantity > 0;
                            }
                        }
                        break;
                    }
                case -88:
                    GameCanvas.endDlg();
                    GameCanvas.serverScreen.switchToMe();
                    break;
                case -87:
                    {
                        Res.outz("GET UPDATE_DATA " + msg.reader().available() + " bytes");
                        msg.reader().mark(500000);
                        createData(msg.reader(), isSaveRMS: true);
                        msg.reader().reset();
                        sbyte[] data4 = new sbyte[msg.reader().available()];
                        msg.reader().readFully(ref data4);
                        sbyte[] data5 = new sbyte[1] { GameScr.vcData };
                        Rms.saveRMS("NRdataVersion", data5);
                        LoginScr.isUpdateData = false;
                        GameScr.gI().readOk();


                        break;
                    }
                case -86:
                    {
                        sbyte b107 = msg.reader().readByte();
                        Res.outz("server gui ve giao dich action = " + b107);
                        if (b107 == 0)
                        {
                            int playerID = msg.reader().readInt();
                            GameScr.gI().giaodich(playerID);
                        }
                        if (b107 == 1)
                        {
                            int num301 = msg.reader().readInt();
                            Char obj47 = GameScr.findCharInMap(num301);
                            if (obj47 == null)
                            {
                                return;
                            }
                            GameCanvas.panel.setTypeGiaoDich(obj47);
                            GameCanvas.panel.show();
                            Service.gI().getPlayerMenu(num301);
                        }
                        if (b107 == 2)
                        {
                            sbyte b108 = msg.reader().readByte();
                            for (int num302 = 0; num302 < GameCanvas.panel.vMyGD.size(); num302++)
                            {
                                Item item6 = (Item)GameCanvas.panel.vMyGD.elementAt(num302);
                                if (item6.indexUI == b108)
                                {
                                    GameCanvas.panel.vMyGD.removeElement(item6);
                                    break;
                                }
                            }
                        }
                        if (b107 == 5)
                        {
                        }
                        if (b107 == 6)
                        {
                            GameCanvas.panel.isFriendLock = true;
                            if (GameCanvas.panel2 != null)
                            {
                                GameCanvas.panel2.isFriendLock = true;
                            }
                            GameCanvas.panel.vFriendGD.removeAllElements();
                            if (GameCanvas.panel2 != null)
                            {
                                GameCanvas.panel2.vFriendGD.removeAllElements();
                            }
                            int friendMoneyGD = msg.reader().readInt();
                            sbyte b109 = msg.reader().readByte();
                            Res.outz("item size = " + b109);
                            for (int num303 = 0; num303 < b109; num303++)
                            {
                                Item item7 = new Item();
                                item7.template = ItemTemplates.get(msg.reader().readShort());
                                item7.quantity = msg.reader().readInt();
                                int num304 = msg.reader().readUnsignedByte();
                                if (num304 != 0)
                                {
                                    item7.itemOption = new ItemOption[num304];
                                    for (int num305 = 0; num305 < item7.itemOption.Length; num305++)
                                    {
                                        ItemOption itemOption5 = readItemOption(msg);
                                        if (itemOption5 != null)
                                        {
                                            item7.itemOption[num305] = itemOption5;
                                            item7.compare = GameCanvas.panel.getCompare(item7);
                                        }
                                    }
                                }
                                if (GameCanvas.panel2 != null)
                                {
                                    GameCanvas.panel2.vFriendGD.addElement(item7);
                                }
                                else
                                {
                                    GameCanvas.panel.vFriendGD.addElement(item7);
                                }
                            }
                            if (GameCanvas.panel2 != null)
                            {
                                GameCanvas.panel2.setTabGiaoDich(isMe: false);
                                GameCanvas.panel2.friendMoneyGD = friendMoneyGD;
                            }
                            else
                            {
                                GameCanvas.panel.friendMoneyGD = friendMoneyGD;
                                if (GameCanvas.panel.currentTabIndex == 2)
                                {
                                    GameCanvas.panel.setTabGiaoDich(isMe: false);
                                }
                            }
                        }
                        if (b107 == 7)
                        {
                            InfoDlg.hide();
                            if (GameCanvas.panel.isShow)
                            {
                                GameCanvas.panel.hide();
                            }
                        }
                        break;
                    }
                case -85:
                    {
                        Res.outz("CAP CHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                        sbyte b76 = msg.reader().readByte();
                        if (b76 == 0)
                        {
                            int num200 = msg.reader().readUnsignedShort();
                            Res.outz("lent =" + num200);
                            sbyte[] data2 = new sbyte[num200];
                            msg.reader().read(ref data2, 0, num200);
                            GameScr.imgCapcha = Image.createImage(data2, 0, num200);
                            GameScr.gI().keyInput = "-----";
                            GameScr.gI().strCapcha = msg.reader().readUTF();
                            GameScr.gI().keyCapcha = new int[GameScr.gI().strCapcha.Length];
                            GameScr.gI().mobCapcha = new Mob();
                            GameScr.gI().right = null;
                        }
                        if (b76 == 1)
                        {
                            MobCapcha.isAttack = true;
                        }
                        if (b76 == 2)
                        {
                            MobCapcha.explode = true;
                            GameScr.gI().right = GameScr.gI().cmdFocus;
                        }
                        break;
                    }
                case -112:
                    {
                        sbyte b64 = msg.reader().readByte();
                        if (b64 == 0)
                        {
                            sbyte mobIndex = msg.reader().readByte();
                            GameScr.findMobInMap(mobIndex).clearBody();
                        }
                        if (b64 == 1)
                        {
                            sbyte mobIndex2 = msg.reader().readByte();
                            GameScr.findMobInMap(mobIndex2).setBody(msg.reader().readShort());
                        }
                        break;
                    }
                case -84:
                    {
                        int index5 = msg.reader().readUnsignedByte();
                        Mob mob13 = null;
                        try
                        {
                            mob13 = (Mob)GameScr.vMob.elementAt(index5);
                        }
                        catch (Exception)
                        {
                        }
                        if (mob13 != null)
                        {
                            mob13.maxHp = msg.reader().readLong();
                        }
                        break;
                    }
                case -83:
                    {
                        sbyte b90 = msg.reader().readByte();
                        if (b90 == 0)
                        {
                            int num249 = msg.reader().readShort();
                            int bgRID = msg.reader().readShort();
                            int num250 = msg.reader().readUnsignedByte();
                            int num251 = msg.reader().readInt();
                            string text6 = msg.reader().readUTF();
                            int xR = msg.reader().readShort();
                            int yR = msg.reader().readShort();
                            sbyte b91 = msg.reader().readByte();
                            if (b91 == 1)
                            {
                                GameScr.gI().isRongNamek = true;
                            }
                            else
                            {
                                GameScr.gI().isRongNamek = false;
                            }
                            GameScr.gI().xR = xR;
                            GameScr.gI().yR = yR;
                            Res.outz("xR= " + xR + " yR= " + yR + " +++++++++++++++++++++++++++++++++++++++");
                            if (Char.myCharz().charID == num251)
                            {
                                GameCanvas.panel.hideNow();
                                GameScr.gI().activeRongThanEff(isMe: true);
                            }
                            else if (TileMap.mapID == num249 && TileMap.zoneID == num250)
                            {
                                GameScr.gI().activeRongThanEff(isMe: false);
                            }
                            else if (mGraphics.zoomLevel > 1)
                            {
                                GameScr.gI().doiMauTroi();
                            }
                            GameScr.gI().mapRID = num249;
                            GameScr.gI().bgRID = bgRID;
                            GameScr.gI().zoneRID = num250;
                        }
                        if (b90 == 1)
                        {
                            Res.outz("map RID = " + GameScr.gI().mapRID + " zone RID= " + GameScr.gI().zoneRID);
                            Res.outz("map ID = " + TileMap.mapID + " zone ID= " + TileMap.zoneID);
                            if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
                            {
                                GameScr.gI().hideRongThanEff();
                            }
                            else
                            {
                                GameScr.gI().isRongThanXuatHien = false;
                                if (GameScr.gI().isRongNamek)
                                {
                                    GameScr.gI().isRongNamek = false;
                                }
                            }
                        }
                        if (b90 != 2)
                        {
                        }
                        break;
                    }
                case -82:
                    {
                        sbyte b7 = msg.reader().readByte();
                        TileMap.tileIndex = new int[b7][][];
                        TileMap.tileType = new int[b7][];
                        for (int j = 0; j < b7; j++)
                        {
                            sbyte b8 = msg.reader().readByte();
                            TileMap.tileType[j] = new int[b8];
                            TileMap.tileIndex[j] = new int[b8][];
                            for (int k = 0; k < b8; k++)
                            {
                                TileMap.tileType[j][k] = msg.reader().readInt();
                                sbyte b9 = msg.reader().readByte();
                                TileMap.tileIndex[j][k] = new int[b9];
                                for (int l = 0; l < b9; l++)
                                {
                                    TileMap.tileIndex[j][k][l] = msg.reader().readByte();
                                }
                            }
                        }
                        break;
                    }
                case -81:
                    {
                        sbyte b61 = msg.reader().readByte();
                        if (b61 == 0)
                        {
                            string src = msg.reader().readUTF();
                            string src2 = msg.reader().readUTF();
                            GameCanvas.panel.setTypeCombine();
                            GameCanvas.panel.combineInfo = mFont.tahoma_7b_blue.splitFontArray(src, Panel.WIDTH_PANEL);
                            GameCanvas.panel.combineTopInfo = mFont.tahoma_7.splitFontArray(src2, Panel.WIDTH_PANEL);
                            GameCanvas.panel.show();
                        }
                        if (b61 == 1)
                        {
                            GameCanvas.panel.vItemCombine.removeAllElements();
                            sbyte b62 = msg.reader().readByte();
                            for (int m = 0; m < b62; m++)
                            {
                                sbyte b63 = msg.reader().readByte();
                                for (int n = 0; n < Char.myCharz().arrItemBag.Length; n++)
                                {
                                    Item item4 = Char.myCharz().arrItemBag[n];
                                    if (item4 != null && item4.indexUI == b63)
                                    {
                                        item4.isSelect = true;
                                        GameCanvas.panel.vItemCombine.addElement(item4);
                                    }
                                }
                            }
                            if (GameCanvas.panel.isShow)
                            {
                                GameCanvas.panel.setTabCombine();
                            }
                        }
                        if (b61 == 2)
                        {
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(0);
                        }
                        if (b61 == 3)
                        {
                            GameCanvas.panel.combineSuccess = 1;
                            GameCanvas.panel.setCombineEff(0);
                        }
                        if (b61 == 4)
                        {
                            short iconID = msg.reader().readShort();
                            GameCanvas.panel.iconID3 = iconID;
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(1);
                        }
                        if (b61 == 5)
                        {
                            short iconID2 = msg.reader().readShort();
                            GameCanvas.panel.iconID3 = iconID2;
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(2);
                        }
                        if (b61 == 6)
                        {
                            short iconID3 = msg.reader().readShort();
                            short iconID4 = msg.reader().readShort();
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(3);
                            GameCanvas.panel.iconID1 = iconID3;
                            GameCanvas.panel.iconID3 = iconID4;
                        }
                        if (b61 == 7)
                        {
                            short iconID5 = msg.reader().readShort();
                            GameCanvas.panel.iconID3 = iconID5;
                            GameCanvas.panel.combineSuccess = 0;
                            GameCanvas.panel.setCombineEff(4);
                        }
                        if (b61 == 8)
                        {
                            GameCanvas.panel.iconID3 = -1;
                            GameCanvas.panel.combineSuccess = 1;
                            GameCanvas.panel.setCombineEff(4);
                        }
                        short num164 = 21;
                        int num165 = 0;
                        int num166 = 0;
                        try
                        {
                            num164 = msg.reader().readShort();
                            num165 = msg.reader().readShort();
                            num166 = msg.reader().readShort();
                            GameCanvas.panel.xS = num165 - GameScr.cmx;
                            GameCanvas.panel.yS = num166 - GameScr.cmy;
                        }
                        catch (Exception)
                        {
                        }
                        for (int num167 = 0; num167 < GameScr.vNpc.size(); num167++)
                        {
                            Npc npc6 = (Npc)GameScr.vNpc.elementAt(num167);
                            if (npc6.template.npcTemplateId == num164)
                            {
                                GameCanvas.panel.xS = npc6.cx - GameScr.cmx;
                                GameCanvas.panel.yS = npc6.cy - GameScr.cmy;
                                GameCanvas.panel.idNPC = num164;
                                break;
                            }
                        }
                        break;
                    }
                case -80:
                    {
                        sbyte b95 = msg.reader().readByte();
                        InfoDlg.hide();
                        if (b95 == 0)
                        {
                            GameCanvas.panel.vFriend.removeAllElements();
                            int num255 = msg.reader().readUnsignedByte();
                            for (int num256 = 0; num256 < num255; num256++)
                            {
                                Char obj10 = new Char();
                                obj10.charID = msg.reader().readInt();
                                obj10.head = msg.reader().readShort();
                                obj10.headICON = msg.reader().readShort();
                                obj10.body = msg.reader().readShort();
                                obj10.leg = msg.reader().readShort();
                                obj10.bag = msg.reader().readShort();
                                obj10.cName = msg.reader().readUTF();
                                bool isOnline2 = msg.reader().readBoolean();
                                InfoItem infoItem2 = new InfoItem(mResources.power + ": " + msg.reader().readUTF());
                                infoItem2.charInfo = obj10;
                                infoItem2.isOnline = isOnline2;
                                GameCanvas.panel.vFriend.addElement(infoItem2);
                            }
                            GameCanvas.panel.setTypeFriend();
                            GameCanvas.panel.show();
                        }
                        if (b95 == 3)
                        {
                            MyVector vFriend = GameCanvas.panel.vFriend;
                            int num257 = msg.reader().readInt();
                            Res.outz("online offline id=" + num257);
                            for (int num258 = 0; num258 < vFriend.size(); num258++)
                            {
                                InfoItem infoItem3 = (InfoItem)vFriend.elementAt(num258);
                                if (infoItem3.charInfo != null && infoItem3.charInfo.charID == num257)
                                {
                                    Res.outz("online= " + infoItem3.isOnline);
                                    infoItem3.isOnline = msg.reader().readBoolean();
                                    break;
                                }
                            }
                        }
                        if (b95 != 2)
                        {
                            break;
                        }
                        MyVector vFriend2 = GameCanvas.panel.vFriend;
                        int num259 = msg.reader().readInt();
                        for (int num260 = 0; num260 < vFriend2.size(); num260++)
                        {
                            InfoItem infoItem4 = (InfoItem)vFriend2.elementAt(num260);
                            if (infoItem4.charInfo != null && infoItem4.charInfo.charID == num259)
                            {
                                vFriend2.removeElement(infoItem4);
                                break;
                            }
                        }
                        if (GameCanvas.panel.isShow)
                        {
                            GameCanvas.panel.setTabFriend();
                        }
                        break;
                    }
                case -99:
                    InfoDlg.hide();
                    if (msg.reader().readByte() == 0)
                    {
                        GameCanvas.panel.vEnemy.removeAllElements();
                        int num243 = msg.reader().readUnsignedByte();
                        for (int num244 = 0; num244 < num243; num244++)
                        {
                            Char obj7 = new Char();
                            obj7.charID = msg.reader().readInt();
                            obj7.head = msg.reader().readShort();
                            obj7.headICON = msg.reader().readShort();
                            obj7.body = msg.reader().readShort();
                            obj7.leg = msg.reader().readShort();
                            obj7.bag = msg.reader().readShort();
                            obj7.cName = msg.reader().readUTF();
                            InfoItem infoItem = new InfoItem(msg.reader().readUTF());
                            bool isOnline = msg.reader().readBoolean();
                            infoItem.charInfo = obj7;
                            infoItem.isOnline = isOnline;
                            Res.outz("isonline = " + isOnline);
                            GameCanvas.panel.vEnemy.addElement(infoItem);
                        }
                        GameCanvas.panel.setTypeEnemy();
                        GameCanvas.panel.show();
                    }
                    break;
                case -79:
                    {
                        InfoDlg.hide();
                        int num242 = msg.reader().readInt();
                        Char charMenu = GameCanvas.panel.charMenu;
                        if (charMenu == null)
                        {
                            return;
                        }
                        charMenu.cPower = msg.reader().readLong();
                        charMenu.currStrLevel = msg.reader().readUTF();
                        break;
                    }
                case -93:
                    {
                        short num236 = msg.reader().readShort();
                        BgItem.newSmallVersion = new sbyte[num236];
                        for (int num237 = 0; num237 < num236; num237++)
                        {
                            BgItem.newSmallVersion[num237] = msg.reader().readByte();
                        }
                        break;
                    }
                case -77:
                    {
                        short num220 = msg.reader().readShort();
                        SmallImage.newSmallVersion = new sbyte[num220];
                        SmallImage.maxSmall = num220;
                        SmallImage.imgNew = new Small[num220];
                        for (int num221 = 0; num221 < num220; num221++)
                        {
                            SmallImage.newSmallVersion[num221] = msg.reader().readByte();
                        }
                        break;
                    }
                case -76:
                    switch (msg.reader().readByte())
                    {
                        case 0:
                            {
                                sbyte b92 = msg.reader().readByte();
                                if (b92 <= 0)
                                {
                                    return;
                                }
                                Char.myCharz().arrArchive = new Archivement[b92];
                                for (int num253 = 0; num253 < b92; num253++)
                                {
                                    Char.myCharz().arrArchive[num253] = new Archivement();
                                    Char.myCharz().arrArchive[num253].info1 = num253 + 1 + ". " + msg.reader().readUTF();
                                    Char.myCharz().arrArchive[num253].info2 = msg.reader().readUTF();
                                    Char.myCharz().arrArchive[num253].money = msg.reader().readShort();
                                    Char.myCharz().arrArchive[num253].isFinish = msg.reader().readBoolean();
                                    Char.myCharz().arrArchive[num253].isRecieve = msg.reader().readBoolean();
                                }
                                GameCanvas.panel.setTypeArchivement();
                                GameCanvas.panel.show();
                                break;
                            }
                        case 1:
                            {
                                int num252 = msg.reader().readUnsignedByte();
                                if (Char.myCharz().arrArchive[num252] != null)
                                {
                                    Char.myCharz().arrArchive[num252].isRecieve = true;
                                }
                                break;
                            }
                    }
                    break;
                case -74:
                    {
                        if (ServerListScreen.stopDownload)
                        {
                            return;
                        }
                        if (!GameCanvas.isGetResourceFromServer())
                        {
                            Service.gI().getResource(3, null);
                            SmallImage.loadBigRMS();
                            SplashScr.imgLogo = null;
                            if (Rms.loadRMSString("acc") != null || Rms.loadRMSString("userAo" + ServerListScreen.ipSelect) != null)
                            {
                                LoginScr.isContinueToLogin = true;
                            }
                            GameCanvas.loginScr = new LoginScr();
                            GameCanvas.loginScr.switchToMe();
                            return;
                        }
                        bool flag6 = true;
                        Res.outz("1>>GET_IMAGE_SOURCE = " + msg.reader().available());
                        sbyte b87 = msg.reader().readByte();
                        Res.outz("2>GET_IMAGE_SOURCE = " + b87);
                        if (b87 == 0)
                        {
                            int num231 = msg.reader().readInt();
                            Res.outz("3>GET_IMAGE_SOURCE serverVersion = " + num231);
                            string text2 = Rms.loadRMSString("ResVersion");
                            int num232 = ((text2 == null || !(text2 != string.Empty)) ? (-1) : int.Parse(text2));
                            Res.outz("4>>>GET_IMAGE_SOURCE: version>> " + text2 + " <> " + num232 + "!=" + num231);
                            if (num232 == -1 || num232 != num231)
                            {
                                GameCanvas.serverScreen.show2();
                            }
                            else
                            {
                                SmallImage.loadBigRMS();
                                SplashScr.imgLogo = null;
                                ServerListScreen.loadScreen = true;
                                Res.outz(">>>vo ne: " + GameCanvas.currentScreen);
                                if (GameCanvas.currentScreen != GameCanvas.loginScr)
                                {
                                    if (GameCanvas.serverScreen == null)
                                    {
                                        GameCanvas.serverScreen = new ServerListScreen();
                                    }
                                    GameCanvas.serverScreen.switchToMe();
                                }
                                else
                                {
                                    if (GameCanvas.loginScr == null)
                                    {
                                        GameCanvas.loginScr = new LoginScr();
                                    }
                                    GameCanvas.loginScr.doLogin();
                                }
                            }
                        }
                        if (b87 == 1)
                        {
                            ServerListScreen.strWait = mResources.downloading_data;
                            short nBig = msg.reader().readShort();
                            ServerListScreen.nBig = nBig;
                            Service.gI().getResource(2, null);
                        }
                        if (b87 == 2)
                        {
                            try
                            {
                                isLoadingData = true;
                                GameCanvas.endDlg();
                                ServerListScreen.demPercent++;
                                ServerListScreen.percent = ServerListScreen.demPercent * 100 / ServerListScreen.nBig;
                                string text3 = msg.reader().readUTF();
                                Res.outz(">>>vo serverPath: " + text3);
                                string[] array12 = Res.split(text3, "/", 0);
                                string filename = "x" + mGraphics.zoomLevel + array12[array12.Length - 1];
                                int num233 = msg.reader().readInt();
                                sbyte[] data3 = new sbyte[num233];
                                msg.reader().read(ref data3, 0, num233);
                                Rms.saveRMS(filename, data3);
                            }
                            catch (Exception)
                            {
                                GameCanvas.startOK(mResources.pls_restart_game_error, 8885, null);
                            }
                        }
                        if (b87 == 3 && flag6)
                        {
                            isLoadingData = false;
                            int num234 = msg.reader().readInt();
                            Res.outz(">>>GET_IMAGE_SOURCE: lastVersion>> " + num234);
                            Rms.saveRMSString("ResVersion", num234 + string.Empty);
                            Service.gI().getResource(3, null);
                            GameCanvas.endDlg();
                            SplashScr.imgLogo = null;
                            SmallImage.loadBigRMS();
                            mSystem.gcc();
                            ServerListScreen.bigOk = true;
                            ServerListScreen.loadScreen = true;
                            GameScr.gI().loadGameScr();
                            GameScr.isLoadAllData = false;
                            Service.gI().updateData();
                            if (GameCanvas.currentScreen != GameCanvas.loginScr)
                            {
                                GameCanvas.serverScreen.switchToMe();
                            }
                        }
                        break;
                    }
                case -43:
                    {
                        sbyte itemAction = msg.reader().readByte();
                        sbyte b86 = msg.reader().readByte();
                        sbyte index = msg.reader().readByte();
                        string info3 = msg.reader().readUTF();
                        GameCanvas.panel.itemRequest(itemAction, info3, b86, index);
                        break;
                    }
                case -59:
                    {
                        sbyte typePK = msg.reader().readByte();
                        GameScr.gI().player_vs_player(msg.reader().readInt(), msg.reader().readInt(), msg.reader().readUTF(), typePK);
                        break;
                    }
                case -62:
                    {
                        int num229 = msg.reader().readUnsignedByte();
                        sbyte b85 = msg.reader().readByte();
                        if (b85 <= 0)
                        {
                            break;
                        }
                        ClanImage clanImage2 = ClanImage.getClanImage((short)num229);
                        if (clanImage2 == null)
                        {
                            break;
                        }
                        clanImage2.idImage = new short[b85];
                        for (int num230 = 0; num230 < b85; num230++)
                        {
                            clanImage2.idImage[num230] = msg.reader().readShort();
                            if (clanImage2.idImage[num230] > 0)
                            {
                                SmallImage.vKeys.addElement(clanImage2.idImage[num230] + string.Empty);
                            }
                        }
                        break;
                    }
                case -65:
                    {
                        InfoDlg.hide();
                        int num246 = msg.reader().readInt();
                        sbyte b89 = msg.reader().readByte();
                        if (b89 == 0)
                        {
                            break;
                        }
                        if (Char.myCharz().charID == num246)
                        {
                            GameScr.gI().center = null;
                            if (b89 == 0 || b89 == 1 || b89 == 3)
                            {
                                Teleport teleport = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 0, isMe: true, (b89 != 1) ? b89 : Char.myCharz().cgender);
                                //Teleport.addTeleport(teleport);
                            }
                            //if (b89 == 2)
                            //{
                            //	Char.myCharz().hide();
                            //}
                            break;
                        }
                        Char obj9 = GameScr.findCharInMap(num246);
                        if (obj9 != null)
                        {
                            if (b89 == 0 || b89 == 1 || b89 == 3)
                            {
                                obj9.isUsePlane = true;
                                Teleport teleport2 = new Teleport(obj9.cx, obj9.cy, obj9.head, obj9.cdir, 0, isMe: false, (b89 != 1) ? b89 : obj9.cgender);
                                teleport2.id = num246;
                                teleport2.Char = obj9;
                                Teleport.addTeleport(teleport2);
                            }
                            if (b89 == 2)
                            {
                                obj9.hide();
                            }
                        }
                        break;
                    }
                case -64:
                    {
                        int num186 = msg.reader().readInt();
                        int num187 = msg.reader().readShort();
                        obj = null;
                        obj = ((num186 != Char.myCharz().charID) ? GameScr.findCharInMap(num186) : Char.myCharz());
                        if (obj == null)
                        {
                            return;
                        }
                        obj.bag = num187;
                        for (int num188 = 0; num188 < 54; num188++)
                        {
                            obj.removeEffChar(0, 201 + num188);
                        }
                        if (obj.bag >= 201 && obj.bag < 255)
                        {
                            Effect effect = new Effect(obj.bag, obj, 2, -1, 10, 1);
                            effect.typeEff = 5;
                            obj.addEffChar(effect);
                        }
                        Res.outz("cmd:-64 UPDATE BAG PLAER = " + ((obj != null) ? obj.cName : string.Empty) + num186 + " BAG ID= " + num187);
                        if (num187 == 30 && obj.me)
                        {
                            GameScr.isPickNgocRong = true;
                        }
                        break;
                    }
                case -63:
                    {
                        Res.outz("GET BAG");
                        int iD = msg.reader().readShort();
                        sbyte b67 = msg.reader().readByte();
                        ClanImage clanImage = new ClanImage();
                        clanImage.ID = iD;
                        if (b67 > 0)
                        {
                            clanImage.idImage = new short[b67];
                            for (int num174 = 0; num174 < b67; num174++)
                            {
                                clanImage.idImage[num174] = msg.reader().readShort();
                                Res.outz("ID=  " + iD + " frame= " + clanImage.idImage[num174]);
                            }
                            ClanImage.idImages.put(iD + string.Empty, clanImage);
                        }
                        break;
                    }
                case -57:
                    {
                        string strInvite = msg.reader().readUTF();
                        int clanID = msg.reader().readInt();
                        int code = msg.reader().readInt();
                        GameScr.gI().clanInvite(strInvite, clanID, code);
                        break;
                    }
                case -51:
                    InfoDlg.hide();
                    readClanMsg(msg, 0);
                    if (GameCanvas.panel.isMessage && GameCanvas.panel.type == 5)
                    {
                        GameCanvas.panel.initTabClans();
                    }
                    break;
                case -53:
                    {
                        InfoDlg.hide();
                        bool flag7 = false;
                        int num238 = msg.reader().readInt();
                        Res.outz("clanId= " + num238);
                        if (num238 == -1)
                        {
                            flag7 = true;
                            Char.myCharz().clan = null;
                            ClanMessage.vMessage.removeAllElements();
                            if (GameCanvas.panel.member != null)
                            {
                                GameCanvas.panel.member.removeAllElements();
                            }
                            if (GameCanvas.panel.myMember != null)
                            {
                                GameCanvas.panel.myMember.removeAllElements();
                            }
                            if (GameCanvas.currentScreen == GameScr.gI())
                            {
                                GameCanvas.panel.setTabClans();
                            }
                            return;
                        }
                        GameCanvas.panel.tabIcon = null;
                        if (Char.myCharz().clan == null)
                        {
                            Char.myCharz().clan = new Clan();
                        }
                        Char.myCharz().clan.ID = num238;
                        Char.myCharz().clan.name = msg.reader().readUTF();
                        Char.myCharz().clan.slogan = msg.reader().readUTF();
                        Char.myCharz().clan.imgID = msg.reader().readShort();
                        Char.myCharz().clan.powerPoint = msg.reader().readUTF();
                        Char.myCharz().clan.leaderName = msg.reader().readUTF();
                        Char.myCharz().clan.currMember = msg.reader().readUnsignedByte();
                        Char.myCharz().clan.maxMember = msg.reader().readUnsignedByte();
                        Char.myCharz().role = msg.reader().readByte();
                        Char.myCharz().clan.clanPoint = msg.reader().readInt();
                        Char.myCharz().clan.level = msg.reader().readByte();
                        GameCanvas.panel.myMember = new MyVector();
                        for (int num239 = 0; num239 < Char.myCharz().clan.currMember; num239++)
                        {
                            Member member5 = new Member();
                            member5.ID = msg.reader().readInt();
                            member5.head = msg.reader().readShort();
                            member5.headICON = msg.reader().readShort();
                            member5.leg = msg.reader().readShort();
                            member5.body = msg.reader().readShort();
                            member5.name = msg.reader().readUTF();
                            member5.role = msg.reader().readByte();
                            member5.powerPoint = msg.reader().readUTF();
                            member5.donate = msg.reader().readInt();
                            member5.receive_donate = msg.reader().readInt();
                            member5.clanPoint = msg.reader().readInt();
                            member5.curClanPoint = msg.reader().readInt();
                            member5.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            GameCanvas.panel.myMember.addElement(member5);
                        }
                        int num240 = msg.reader().readUnsignedByte();
                        for (int num241 = 0; num241 < num240; num241++)
                        {
                            readClanMsg(msg, -1);
                        }
                        if (GameCanvas.panel.isSearchClan || GameCanvas.panel.isViewMember || GameCanvas.panel.isMessage)
                        {
                            GameCanvas.panel.setTabClans();
                        }
                        if (flag7)
                        {
                            GameCanvas.panel.setTabClans();
                        }
                        Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -537 MY CLAN INFO");
                        break;
                    }
                case -52:
                    {
                        sbyte b88 = msg.reader().readByte();
                        if (b88 == 0)
                        {
                            Member member2 = new Member();
                            member2.ID = msg.reader().readInt();
                            member2.head = msg.reader().readShort();
                            member2.headICON = msg.reader().readShort();
                            member2.leg = msg.reader().readShort();
                            member2.body = msg.reader().readShort();
                            member2.name = msg.reader().readUTF();
                            member2.role = msg.reader().readByte();
                            member2.powerPoint = msg.reader().readUTF();
                            member2.donate = msg.reader().readInt();
                            member2.receive_donate = msg.reader().readInt();
                            member2.clanPoint = msg.reader().readInt();
                            member2.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            if (GameCanvas.panel.myMember == null)
                            {
                                GameCanvas.panel.myMember = new MyVector();
                            }
                            GameCanvas.panel.myMember.addElement(member2);
                            GameCanvas.panel.initTabClans();
                        }
                        if (b88 == 1)
                        {
                            GameCanvas.panel.myMember.removeElementAt(msg.reader().readByte());
                            Panel panel = GameCanvas.panel;
                            Panel panel2 = panel;
                            Panel panel3 = panel2;
                            Panel panel4 = panel3;
                            Panel panel5 = panel4;
                            Panel panel6 = panel5;
                            Panel panel7 = panel6;
                            Panel panel8 = panel7;
                            Panel panel9 = panel8;
                            Panel panel10 = panel9;
                            Panel panel11 = panel10;
                            Panel panel12 = panel11;
                            panel12.currentListLength--;
                            GameCanvas.panel.initTabClans();
                        }
                        if (b88 == 2)
                        {
                            Member member3 = new Member();
                            member3.ID = msg.reader().readInt();
                            member3.head = msg.reader().readShort();
                            member3.headICON = msg.reader().readShort();
                            member3.leg = msg.reader().readShort();
                            member3.body = msg.reader().readShort();
                            member3.name = msg.reader().readUTF();
                            member3.role = msg.reader().readByte();
                            member3.powerPoint = msg.reader().readUTF();
                            member3.donate = msg.reader().readInt();
                            member3.receive_donate = msg.reader().readInt();
                            member3.clanPoint = msg.reader().readInt();
                            member3.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            for (int num235 = 0; num235 < GameCanvas.panel.myMember.size(); num235++)
                            {
                                Member member4 = (Member)GameCanvas.panel.myMember.elementAt(num235);
                                if (member4.ID == member3.ID)
                                {
                                    if (Char.myCharz().charID == member3.ID)
                                    {
                                        Char.myCharz().role = member3.role;
                                    }
                                    Member o = member3;
                                    GameCanvas.panel.myMember.removeElement(member4);
                                    GameCanvas.panel.myMember.insertElementAt(o, num235);
                                    return;
                                }
                            }
                        }
                        Res.outz("=>>>>>>>>>>>>>>>>>>>>>> -52  MY CLAN UPDSTE");
                        break;
                    }
                case -50:
                    {
                        InfoDlg.hide();
                        GameCanvas.panel.member = new MyVector();
                        sbyte b71 = msg.reader().readByte();
                        for (int num185 = 0; num185 < b71; num185++)
                        {
                            Member member = new Member();
                            member.ID = msg.reader().readInt();
                            member.head = msg.reader().readShort();
                            member.headICON = msg.reader().readShort();
                            member.leg = msg.reader().readShort();
                            member.body = msg.reader().readShort();
                            member.name = msg.reader().readUTF();
                            member.role = msg.reader().readByte();
                            member.powerPoint = msg.reader().readUTF();
                            member.donate = msg.reader().readInt();
                            member.receive_donate = msg.reader().readInt();
                            member.clanPoint = msg.reader().readInt();
                            member.joinTime = NinjaUtil.getDate(msg.reader().readInt());
                            GameCanvas.panel.member.addElement(member);
                        }
                        GameCanvas.panel.isViewMember = true;
                        GameCanvas.panel.isSearchClan = false;
                        GameCanvas.panel.isMessage = false;
                        GameCanvas.panel.currentListLength = GameCanvas.panel.member.size() + 2;
                        GameCanvas.panel.initTabClans();
                        break;
                    }
                case -47:
                    {
                        InfoDlg.hide();
                        sbyte b72 = msg.reader().readByte();
                        Res.outz("clan = " + b72);
                        if (b72 == 0)
                        {
                            GameCanvas.panel.clanReport = mResources.cannot_find_clan;
                            GameCanvas.panel.clans = null;
                        }
                        else
                        {
                            GameCanvas.panel.clans = new Clan[b72];
                            Res.outz("clan search lent= " + GameCanvas.panel.clans.Length);
                            for (int num189 = 0; num189 < GameCanvas.panel.clans.Length; num189++)
                            {
                                GameCanvas.panel.clans[num189] = new Clan();
                                GameCanvas.panel.clans[num189].ID = msg.reader().readInt();
                                GameCanvas.panel.clans[num189].name = msg.reader().readUTF();
                                GameCanvas.panel.clans[num189].slogan = msg.reader().readUTF();
                                GameCanvas.panel.clans[num189].imgID = msg.reader().readShort();
                                GameCanvas.panel.clans[num189].powerPoint = msg.reader().readUTF();
                                GameCanvas.panel.clans[num189].leaderName = msg.reader().readUTF();
                                GameCanvas.panel.clans[num189].currMember = msg.reader().readUnsignedByte();
                                GameCanvas.panel.clans[num189].maxMember = msg.reader().readUnsignedByte();
                                GameCanvas.panel.clans[num189].date = msg.reader().readInt();
                            }
                        }
                        GameCanvas.panel.isSearchClan = true;
                        GameCanvas.panel.isViewMember = false;
                        GameCanvas.panel.isMessage = false;
                        if (GameCanvas.panel.isSearchClan)
                        {
                            GameCanvas.panel.initTabClans();
                        }
                        break;
                    }
                case -46:
                    {
                        InfoDlg.hide();
                        sbyte b113 = msg.reader().readByte();
                        if (b113 == 1 || b113 == 3)
                        {
                            GameCanvas.endDlg();
                            ClanImage.vClanImage.removeAllElements();
                            int num313 = msg.reader().readShort();
                            for (int num314 = 0; num314 < num313; num314++)
                            {
                                ClanImage clanImage3 = new ClanImage();
                                clanImage3.ID = msg.reader().readShort();
                                clanImage3.name = msg.reader().readUTF();
                                clanImage3.xu = msg.reader().readInt();
                                clanImage3.luong = msg.reader().readInt();
                                if (!ClanImage.isExistClanImage(clanImage3.ID))
                                {
                                    ClanImage.addClanImage(clanImage3);
                                    continue;
                                }
                                ClanImage.getClanImage((short)clanImage3.ID).name = clanImage3.name;
                                ClanImage.getClanImage((short)clanImage3.ID).xu = clanImage3.xu;
                                ClanImage.getClanImage((short)clanImage3.ID).luong = clanImage3.luong;
                            }
                            if (Char.myCharz().clan != null)
                            {
                                GameCanvas.panel.changeIcon();
                            }
                        }
                        if (b113 == 4)
                        {
                            Char.myCharz().clan.imgID = msg.reader().readShort();
                            Char.myCharz().clan.slogan = msg.reader().readUTF();
                        }
                        break;
                    }
                case -61:
                    {
                        int num315 = msg.reader().readInt();
                        if (num315 != Char.myCharz().charID)
                        {
                            if (GameScr.findCharInMap(num315) != null)
                            {
                                GameScr.findCharInMap(num315).clanID = msg.reader().readInt();
                                if (GameScr.findCharInMap(num315).clanID == -2)
                                {
                                    GameScr.findCharInMap(num315).isCopy = true;
                                }
                            }
                        }
                        else if (Char.myCharz().clan != null)
                        {
                            Char.myCharz().clan.ID = msg.reader().readInt();
                        }
                        break;
                    }
                case -42:
                    Char.myCharz().cHPGoc = msg.readInt3Byte();
                    Char.myCharz().cMPGoc = msg.readInt3Byte();
                    Char.myCharz().cDamGoc = msg.reader().readInt();
                    Char.myCharz().cHPFull = msg.reader().readLong();
                    Char.myCharz().cMPFull = msg.reader().readLong();
                    Char.myCharz().cHP = msg.reader().readLong();
                    Char.myCharz().cMP = msg.reader().readLong();
                    Char.myCharz().cspeed = msg.reader().readByte();
                    Char.myCharz().hpFrom1000TiemNang = msg.reader().readByte();
                    Char.myCharz().mpFrom1000TiemNang = msg.reader().readByte();
                    Char.myCharz().damFrom1000TiemNang = msg.reader().readByte();
                    Char.myCharz().cDamFull = msg.reader().readLong();
                    Char.myCharz().cDefull = msg.reader().readLong();
                    Char.myCharz().cCriticalFull = msg.reader().readByte();
                    Char.myCharz().cTiemNang = msg.reader().readLong();
                    Char.myCharz().expForOneAdd = msg.reader().readShort();
                    Char.myCharz().cDefGoc = msg.reader().readInt();
                    Char.myCharz().cCriticalGoc = msg.reader().readByte();
                    Char.myCharz().cGiamST = msg.reader().readByte();
                    Char.myCharz().cCritDameFull = msg.reader().readShort();
                    InfoDlg.hide();
                    break;
                case 1:
                    {
                        bool flag9 = msg.reader().readBool();
                        Res.outz("isRes= " + flag9);
                        if (!flag9)
                        {
                            GameCanvas.startOKDlg(msg.reader().readUTF());
                            break;
                        }
                        GameCanvas.loginScr.isLogin2 = false;
                        Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
                        GameCanvas.endDlg();
                        GameCanvas.loginScr.doLogin();
                        break;
                    }
                case 2:
                    Char.isLoadingMap = false;
                    LoginScr.isLoggingIn = false;
                    if (!GameScr.isLoadAllData)
                    {
                        GameScr.gI().initSelectChar();
                    }
                    BgItem.clearHashTable();
                    GameCanvas.endDlg();
                    CreateCharScr.isCreateChar = true;
                    CreateCharScr.gI().switchToMe();
                    break;
                case -107:
                    {
                        sbyte b77 = msg.reader().readByte();
                        if (b77 == 0)
                        {
                            Char.myCharz().havePet = false;
                        }
                        if (b77 == 1)
                        {
                            Char.myCharz().havePet = true;
                        }
                        if (b77 != 2)
                        {
                            break;
                        }
                        InfoDlg.hide();
                        Char.myPetz().head = msg.reader().readShort();
                        Char.myPetz().setDefaultPart();
                        int num201 = msg.reader().readUnsignedByte();
                        Res.outz("num body = " + num201);
                        Char.myPetz().arrItemBody = new Item[num201];
                        for (int num202 = 0; num202 < num201; num202++)
                        {
                            short num203 = msg.reader().readShort();
                            Res.outz("template id= " + num203);
                            if (num203 == -1)
                            {
                                continue;
                            }
                            Res.outz("1");
                            Char.myPetz().arrItemBody[num202] = new Item();
                            Char.myPetz().arrItemBody[num202].template = ItemTemplates.get(num203);
                            int type2 = Char.myPetz().arrItemBody[num202].template.type;
                            Char.myPetz().arrItemBody[num202].quantity = msg.reader().readInt();
                            Res.outz("3");
                            Char.myPetz().arrItemBody[num202].info = msg.reader().readUTF();
                            Char.myPetz().arrItemBody[num202].content = msg.reader().readUTF();
                            int num204 = msg.reader().readUnsignedByte();
                            Res.outz("option size= " + num204);
                            if (num204 != 0)
                            {
                                Char.myPetz().arrItemBody[num202].itemOption = new ItemOption[num204];
                                for (int num205 = 0; num205 < Char.myPetz().arrItemBody[num202].itemOption.Length; num205++)
                                {
                                    ItemOption itemOption3 = readItemOption(msg);
                                    if (itemOption3 != null)
                                    {
                                        Char.myPetz().arrItemBody[num202].itemOption[num205] = itemOption3;
                                    }
                                }
                            }
                            switch (type2)
                            {
                                case 0:
                                    Char.myPetz().body = Char.myPetz().arrItemBody[num202].template.part;
                                    break;
                                case 1:
                                    Char.myPetz().leg = Char.myPetz().arrItemBody[num202].template.part;
                                    break;
                            }
                        }
                        Char.myPetz().cHP = msg.reader().readLong();
                        Char.myPetz().cHPFull = msg.reader().readLong();
                        Char.myPetz().cMP = msg.reader().readLong();
                        Char.myPetz().cMPFull = msg.reader().readLong();
                        Char.myPetz().cDamFull = msg.reader().readLong();
                        Char.myPetz().cName = msg.reader().readUTF();
                        Char.myPetz().currStrLevel = msg.reader().readUTF();
                        Char.myPetz().cPower = msg.reader().readLong();
                        Char.myPetz().cTiemNang = msg.reader().readLong();
                        Char.myPetz().petStatus = msg.reader().readByte();
                        Char.myPetz().cStamina = msg.reader().readShort();
                        Char.myPetz().cMaxStamina = msg.reader().readShort();
                        Char.myPetz().cCriticalFull = msg.reader().readByte();
                        Char.myPetz().cDefull = msg.reader().readLong();
                        Char.myPetz().arrPetSkill = new Skill[msg.reader().readByte()];
                        Res.outz("SKILLENT = " + Char.myPetz().arrPetSkill);
                        for (int num206 = 0; num206 < Char.myPetz().arrPetSkill.Length; num206++)
                        {
                            short num207 = msg.reader().readShort();
                            if (num207 != -1)
                            {
                                Char.myPetz().arrPetSkill[num206] = Skills.get(num207);
                                continue;
                            }
                            Char.myPetz().arrPetSkill[num206] = new Skill();
                            Char.myPetz().arrPetSkill[num206].template = null;
                            Char.myPetz().arrPetSkill[num206].moreInfo = msg.reader().readUTF();
                        }
                        Char.myPetz().cGiamST = msg.reader().readByte();
                        Char.myPetz().cCritDameFull = msg.reader().readShort();
                        if (!ModProCL.petw)
                        {
                            if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
                            {
                                GameCanvas.panel2 = new Panel();
                                GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
                                GameCanvas.panel2.setTypeBodyOnly();
                                GameCanvas.panel2.show();
                                GameCanvas.panel.setTypePetMain();
                                GameCanvas.panel.show();
                            }
                            else
                            {
                                GameCanvas.panel.tabName[21] = mResources.petMainTab;
                                GameCanvas.panel.setTypePetMain();
                                GameCanvas.panel.show();
                            }
                        }
                        break;
                    }
                case -37:
                    {
                        sbyte b75 = msg.reader().readByte();
                        Res.outz("cAction= " + b75);
                        if (b75 != 0)
                        {
                            break;
                        }
                        Char.myCharz().head = msg.reader().readShort();
                        Char.myCharz().setDefaultPart();
                        int num195 = msg.reader().readUnsignedByte();
                        Res.outz("num body = " + num195);
                        Char.myCharz().arrItemBody = new Item[num195];
                        for (int num196 = 0; num196 < num195; num196++)
                        {
                            short num197 = msg.reader().readShort();
                            if (num197 == -1)
                            {
                                continue;
                            }
                            Char.myCharz().arrItemBody[num196] = new Item();
                            Char.myCharz().arrItemBody[num196].template = ItemTemplates.get(num197);
                            int type = Char.myCharz().arrItemBody[num196].template.type;
                            Char.myCharz().arrItemBody[num196].quantity = msg.reader().readInt();
                            Char.myCharz().arrItemBody[num196].info = msg.reader().readUTF();
                            Char.myCharz().arrItemBody[num196].content = msg.reader().readUTF();
                            int num198 = msg.reader().readUnsignedByte();
                            if (num198 != 0)
                            {
                                Char.myCharz().arrItemBody[num196].itemOption = new ItemOption[num198];
                                for (int num199 = 0; num199 < Char.myCharz().arrItemBody[num196].itemOption.Length; num199++)
                                {
                                    ItemOption itemOption2 = readItemOption(msg);
                                    if (itemOption2 != null)
                                    {
                                        Char.myCharz().arrItemBody[num196].itemOption[num199] = itemOption2;
                                    }
                                }
                            }
                            switch (type)
                            {
                                case 0:
                                    Char.myCharz().body = Char.myCharz().arrItemBody[num196].template.part;
                                    break;
                                case 1:
                                    Char.myCharz().leg = Char.myCharz().arrItemBody[num196].template.part;
                                    break;
                            }
                        }
                        break;
                    }
                case -36:
                    {
                        sbyte b65 = msg.reader().readByte();
                        Res.outz("cAction= " + b65);
                        GameScr.isudungCapsun4 = false;
                        GameScr.isudungCapsun3 = false;
                        if (b65 == 0)
                        {
                            int num168 = msg.reader().readUnsignedByte();
                            Char.myCharz().arrItemBag = new Item[num168];
                            GameScr.hpPotion = 0;
                            Res.outz("numC=" + num168);
                            for (int num169 = 0; num169 < num168; num169++)
                            {
                                short num170 = msg.reader().readShort();
                                if (num170 == -1)
                                {
                                    continue;
                                }
                                Char.myCharz().arrItemBag[num169] = new Item();
                                Char.myCharz().arrItemBag[num169].template = ItemTemplates.get(num170);
                                Char.myCharz().arrItemBag[num169].quantity = msg.reader().readInt();
                                Char.myCharz().arrItemBag[num169].info = msg.reader().readUTF();
                                Char.myCharz().arrItemBag[num169].content = msg.reader().readUTF();
                                Char.myCharz().arrItemBag[num169].indexUI = num169;
                                int num171 = msg.reader().readUnsignedByte();
                                if (num171 != 0)
                                {
                                    Char.myCharz().arrItemBag[num169].itemOption = new ItemOption[num171];
                                    for (int num172 = 0; num172 < Char.myCharz().arrItemBag[num169].itemOption.Length; num172++)
                                    {
                                        ItemOption itemOption = readItemOption(msg);
                                        if (itemOption != null)
                                        {
                                            Char.myCharz().arrItemBag[num169].itemOption[num172] = itemOption;
                                        }
                                    }
                                    Char.myCharz().arrItemBag[num169].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemBag[num169]);
                                }
                                if (Char.myCharz().arrItemBag[num169].template.type == 11)
                                {
                                }
                                if (Char.myCharz().arrItemBag[num169].template.type == 6)
                                {
                                    GameScr.hpPotion += Char.myCharz().arrItemBag[num169].quantity;
                                }
                                if (Char.myCharz().arrItemBag[num169].template.id == 194)
                                {
                                    GameScr.isudungCapsun4 = Char.myCharz().arrItemBag[num169].quantity > 0;
                                }
                                else if (Char.myCharz().arrItemBag[num169].template.id == 193 && !GameScr.isudungCapsun4)
                                {
                                    GameScr.isudungCapsun3 = Char.myCharz().arrItemBag[num169].quantity > 0;
                                }
                            }
                        }
                        if (b65 == 2)
                        {
                            sbyte b66 = msg.reader().readByte();
                            int num173 = msg.reader().readInt();
                            int quantity = Char.myCharz().arrItemBag[b66].quantity;
                            int id = Char.myCharz().arrItemBag[b66].template.id;
                            Char.myCharz().arrItemBag[b66].quantity = num173;
                            if (Char.myCharz().arrItemBag[b66].quantity < quantity && Char.myCharz().arrItemBag[b66].template.type == 6)
                            {
                                GameScr.hpPotion -= quantity - Char.myCharz().arrItemBag[b66].quantity;
                            }
                            if (Char.myCharz().arrItemBag[b66].quantity == 0)
                            {
                                Char.myCharz().arrItemBag[b66] = null;
                            }
                            switch (id)
                            {
                                case 194:
                                    GameScr.isudungCapsun4 = num173 > 0;
                                    break;
                                case 193:
                                    GameScr.isudungCapsun3 = num173 > 0;
                                    break;
                            }
                        }
                        break;
                    }
                case -35:
                    {
                        sbyte b110 = msg.reader().readByte();
                        Res.outz("cAction= " + b110);
                        if (b110 == 0)
                        {
                            int num308 = msg.reader().readUnsignedByte();
                            Char.myCharz().arrItemBox = new Item[num308];
                            GameCanvas.panel.hasUse = 0;
                            for (int num309 = 0; num309 < num308; num309++)
                            {
                                short num310 = msg.reader().readShort();
                                if (num310 == -1)
                                {
                                    continue;
                                }
                                Char.myCharz().arrItemBox[num309] = new Item();
                                Char.myCharz().arrItemBox[num309].template = ItemTemplates.get(num310);
                                Char.myCharz().arrItemBox[num309].quantity = msg.reader().readInt();
                                Char.myCharz().arrItemBox[num309].info = msg.reader().readUTF();
                                Char.myCharz().arrItemBox[num309].content = msg.reader().readUTF();
                                int num311 = msg.reader().readUnsignedByte();
                                if (num311 != 0)
                                {
                                    Char.myCharz().arrItemBox[num309].itemOption = new ItemOption[num311];
                                    for (int num312 = 0; num312 < Char.myCharz().arrItemBox[num309].itemOption.Length; num312++)
                                    {
                                        ItemOption itemOption6 = readItemOption(msg);
                                        if (itemOption6 != null)
                                        {
                                            Char.myCharz().arrItemBox[num309].itemOption[num312] = itemOption6;
                                        }
                                    }
                                }
                                Panel panel13 = GameCanvas.panel;
                                panel13.hasUse++;
                            }
                        }
                        if (b110 == 1)
                        {
                            bool isBoxClan = false;
                            try
                            {
                                sbyte b111 = msg.reader().readByte();
                                if (b111 == 1)
                                {
                                    isBoxClan = true;
                                }
                            }
                            catch (Exception)
                            {
                            }
                            GameCanvas.panel.setTypeBox();
                            GameCanvas.panel.isBoxClan = isBoxClan;
                            GameCanvas.panel.show();
                        }
                        if (b110 == 2)
                        {
                            sbyte b112 = msg.reader().readByte();
                            int quantity2 = msg.reader().readInt();
                            Char.myCharz().arrItemBox[b112].quantity = quantity2;
                            if (Char.myCharz().arrItemBox[b112].quantity == 0)
                            {
                                Char.myCharz().arrItemBox[b112] = null;
                            }
                        }
                        break;
                    }
                case -45:
                    {
                        sbyte b100 = msg.reader().readByte();
                        int num283 = msg.reader().readInt();
                        short num284 = msg.reader().readShort();
                        Res.outz(">.SKILL_NOT_FOCUS      skillNotFocusID: " + num284 + " skill type= " + b100 + "   player use= " + num283);
                        if (b100 == 20)
                        {
                            sbyte typeFrame = msg.reader().readByte();
                            sbyte dir = msg.reader().readByte();
                            short timeGong = msg.reader().readShort();
                            bool isFly = ((msg.reader().readByte() != 0) ? true : false);
                            sbyte typePaint = msg.reader().readByte();
                            sbyte typeItem = -1;
                            try
                            {
                                typeItem = msg.reader().readByte();
                            }
                            catch (Exception)
                            {
                            }
                            Res.outz(">.SKILL_NOT_FOCUS  skill typeFrame= " + typeFrame);
                            obj = ((Char.myCharz().charID != num283) ? GameScr.findCharInMap(num283) : Char.myCharz());
                            obj.SetSkillPaint_NEW(num284, isFly, typeFrame, typePaint, dir, timeGong, typeItem);
                        }
                        if (b100 == 21)
                        {
                            Point point = new Point();
                            point.x = msg.reader().readShort();
                            point.y = msg.reader().readShort();
                            short timeDame = msg.reader().readShort();
                            short rangeDame = msg.reader().readShort();
                            sbyte typePaint2 = 0;
                            sbyte typeItem2 = -1;
                            Point[] array13 = null;
                            obj = ((Char.myCharz().charID != num283) ? GameScr.findCharInMap(num283) : Char.myCharz());
                            try
                            {
                                typePaint2 = msg.reader().readByte();
                                sbyte b101 = msg.reader().readByte();
                                if (b101 > 0)
                                {
                                    array13 = new Point[b101];
                                    for (int num285 = 0; num285 < array13.Length; num285++)
                                    {
                                        array13[num285] = new Point();
                                        array13[num285].type = msg.reader().readByte();
                                        if (array13[num285].type == 0)
                                        {
                                            array13[num285].id = msg.reader().readByte();
                                        }
                                        else
                                        {
                                            array13[num285].id = msg.reader().readInt();
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                            try
                            {
                                typeItem2 = msg.reader().readByte();
                            }
                            catch (Exception)
                            {
                            }
                            Res.outz(">.SKILL_NOT_FOCUS  skill targetDame= " + point.x + ":" + point.y + "    c:" + obj.cx + ":" + obj.cy + "   cdir:" + obj.cdir);
                            obj.SetSkillPaint_STT(1, num284, point, timeDame, rangeDame, typePaint2, array13, typeItem2);
                        }
                        if (b100 == 0)
                        {
                            Res.outz("id use= " + num283);
                            if (Char.myCharz().charID != num283)
                            {
                                obj = GameScr.findCharInMap(num283);
                                if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
                                {
                                    obj.setSkillPaint(GameScr.sks[num284], 0);
                                }
                                else
                                {
                                    obj.setSkillPaint(GameScr.sks[num284], 1);
                                    obj.delayFall = 20;
                                }
                            }
                            else
                            {
                                Char.myCharz().saveLoadPreviousSkill();
                                Res.outz("LOAD LAST SKILL");
                            }
                            sbyte b102 = msg.reader().readByte();
                            Res.outz("npc size= " + b102);
                            for (int num286 = 0; num286 < b102; num286++)
                            {
                                sbyte index3 = msg.reader().readByte();
                                sbyte seconds = msg.reader().readByte();
                                Res.outz("index= " + index3);
                                if (num284 >= 42 && num284 <= 48)
                                {
                                    ((Mob)GameScr.vMob.elementAt(index3)).isFreez = true;
                                    ((Mob)GameScr.vMob.elementAt(index3)).seconds = seconds;
                                    ((Mob)GameScr.vMob.elementAt(index3)).last = (((Mob)GameScr.vMob.elementAt(index3)).cur = mSystem.currentTimeMillis());
                                }
                            }
                            sbyte b103 = msg.reader().readByte();
                            for (int num287 = 0; num287 < b103; num287++)
                            {
                                int num288 = msg.reader().readInt();
                                sbyte b104 = msg.reader().readByte();
                                Res.outz("player ID= " + num288 + " my ID= " + Char.myCharz().charID);
                                if (num284 < 42 || num284 > 48)
                                {
                                    continue;
                                }
                                if (num288 == Char.myCharz().charID)
                                {
                                    if (!Char.myCharz().isFlyAndCharge && !Char.myCharz().isStandAndCharge)
                                    {
                                        GameScr.gI().isFreez = true;
                                        Char.myCharz().isFreez = true;
                                        Char.myCharz().freezSeconds = b104;
                                        Char.myCharz().lastFreez = (Char.myCharz().currFreez = mSystem.currentTimeMillis());
                                        Char.myCharz().isLockMove = true;
                                    }
                                }
                                else
                                {
                                    obj = GameScr.findCharInMap(num288);
                                    if (obj != null && !obj.isFlyAndCharge && !obj.isStandAndCharge)
                                    {
                                        obj.isFreez = true;
                                        obj.seconds = b104;
                                        obj.freezSeconds = b104;
                                        obj.lastFreez = (GameScr.findCharInMap(num288).currFreez = mSystem.currentTimeMillis());
                                    }
                                }
                            }
                        }
                        if (b100 == 1 && num283 != Char.myCharz().charID)
                        {
                            try
                            {
                                GameScr.findCharInMap(num283).isCharge = true;
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (b100 == 3)
                        {
                            if (num283 == Char.myCharz().charID)
                            {
                                Char.myCharz().isCharge = false;
                                SoundMn.gI().taitaoPause();
                                Char.myCharz().saveLoadPreviousSkill();
                            }
                            else
                            {
                                GameScr.findCharInMap(num283).isCharge = false;
                            }
                        }
                        if (b100 == 4)
                        {
                            if (num283 == Char.myCharz().charID)
                            {
                                Char.myCharz().seconds = msg.reader().readShort() - 1000;
                                Char.myCharz().last = mSystem.currentTimeMillis();
                                Res.outz("second= " + Char.myCharz().seconds + " last= " + Char.myCharz().last);
                            }
                            else if (GameScr.findCharInMap(num283) != null)
                            {
                                Char obj46 = GameScr.findCharInMap(num283);
                                switch (obj46.cgender)
                                {
                                    case 0:
                                        if (TileMap.mapID != 170)
                                        {
                                            obj.useChargeSkill(isGround: false);
                                            break;
                                        }
                                        if (num284 >= 77 && num284 <= 83)
                                        {
                                            obj.useChargeSkill(isGround: true);
                                        }
                                        if (num284 >= 70 && num284 <= 76)
                                        {
                                            obj.useChargeSkill(isGround: false);
                                        }
                                        break;
                                    case 1:
                                        {
                                            if (TileMap.mapID != 170)
                                            {
                                                obj.useChargeSkill(isGround: true);
                                                break;
                                            }
                                            bool isGround2 = true;
                                            if (num284 >= 70 && num284 <= 76)
                                            {
                                                isGround2 = false;
                                            }
                                            if (num284 >= 77 && num284 <= 83)
                                            {
                                                isGround2 = true;
                                            }
                                            obj.useChargeSkill(isGround2);
                                            break;
                                        }
                                    default:
                                        if (TileMap.mapID == 170)
                                        {
                                            bool isGround = true;
                                            if (num284 >= 70 && num284 <= 76)
                                            {
                                                isGround = false;
                                            }
                                            if (num284 >= 77 && num284 <= 83)
                                            {
                                                isGround = true;
                                            }
                                            obj.useChargeSkill(isGround);
                                        }
                                        break;
                                }
                                obj.skillTemplateId = num284;
                                if (num284 >= 70 && num284 <= 76)
                                {
                                    obj.isUseSkillAfterCharge = true;
                                }
                                obj.seconds = msg.reader().readShort();
                                obj.last = mSystem.currentTimeMillis();
                            }
                        }
                        if (b100 == 5)
                        {
                            if (num283 == Char.myCharz().charID)
                            {
                                Char.myCharz().stopUseChargeSkill();
                            }
                            else if (GameScr.findCharInMap(num283) != null)
                            {
                                GameScr.findCharInMap(num283).stopUseChargeSkill();
                            }
                        }
                        if (b100 == 6)
                        {
                            if (num283 == Char.myCharz().charID)
                            {
                                Char.myCharz().setAutoSkillPaint(GameScr.sks[num284], 0);
                            }
                            else if (GameScr.findCharInMap(num283) != null)
                            {
                                GameScr.findCharInMap(num283).setAutoSkillPaint(GameScr.sks[num284], 0);
                                SoundMn.gI().gong();
                            }
                        }
                        if (b100 == 7)
                        {
                            if (num283 == Char.myCharz().charID)
                            {
                                Char.myCharz().seconds = msg.reader().readShort();
                                Res.outz("second = " + Char.myCharz().seconds);
                                Char.myCharz().last = mSystem.currentTimeMillis();
                            }
                            else if (GameScr.findCharInMap(num283) != null)
                            {
                                GameScr.findCharInMap(num283).useChargeSkill(isGround: true);
                                GameScr.findCharInMap(num283).seconds = msg.reader().readShort();
                                GameScr.findCharInMap(num283).last = mSystem.currentTimeMillis();
                                SoundMn.gI().gong();
                            }
                        }
                        if (b100 == 8 && num283 != Char.myCharz().charID && GameScr.findCharInMap(num283) != null)
                        {
                            GameScr.findCharInMap(num283).setAutoSkillPaint(GameScr.sks[num284], 0);
                        }
                        break;
                    }
                case -44:
                    {
                        bool flag5 = false;
                        if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
                        {
                            flag5 = true;
                        }
                        sbyte b80 = msg.reader().readByte();
                        int num212 = msg.reader().readUnsignedByte();
                        Char.myCharz().arrItemShop = new Item[num212][];
                        GameCanvas.panel.shopTabName = new string[num212 + ((!flag5) ? 1 : 0)][];
                        for (int num213 = 0; num213 < GameCanvas.panel.shopTabName.Length; num213++)
                        {
                            GameCanvas.panel.shopTabName[num213] = new string[2];
                        }
                        if (b80 == 2)
                        {
                            GameCanvas.panel.maxPageShop = new int[num212];
                            GameCanvas.panel.currPageShop = new int[num212];
                        }
                        if (!flag5)
                        {
                            GameCanvas.panel.shopTabName[num212] = mResources.inventory;
                        }
                        for (int num214 = 0; num214 < num212; num214++)
                        {
                            string[] array5 = Res.split(msg.reader().readUTF(), "\n", 0);
                            if (b80 == 2)
                            {
                                GameCanvas.panel.maxPageShop[num214] = msg.reader().readUnsignedByte();
                            }
                            if (array5.Length == 2)
                            {
                                GameCanvas.panel.shopTabName[num214] = array5;
                            }
                            if (array5.Length == 1)
                            {
                                GameCanvas.panel.shopTabName[num214][0] = array5[0];
                                GameCanvas.panel.shopTabName[num214][1] = string.Empty;
                            }
                            int num215 = msg.reader().readUnsignedByte();
                            Char.myCharz().arrItemShop[num214] = new Item[num215];
                            Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
                            if (b80 == 1)
                            {
                                Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy2;
                            }
                            for (int num216 = 0; num216 < num215; num216++)
                            {
                                short num217 = msg.reader().readShort();
                                if (num217 == -1)
                                {
                                    continue;
                                }
                                Char.myCharz().arrItemShop[num214][num216] = new Item();
                                Char.myCharz().arrItemShop[num214][num216].template = ItemTemplates.get(num217);
                                switch (b80)
                                {
                                    case 8:
                                        Char.myCharz().arrItemShop[num214][num216].buyCoin = msg.reader().readInt();
                                        Char.myCharz().arrItemShop[num214][num216].buyGold = msg.reader().readInt();
                                        Char.myCharz().arrItemShop[num214][num216].quantity = msg.reader().readInt();
                                        break;
                                    case 4:
                                        Char.myCharz().arrItemShop[num214][num216].reason = msg.reader().readUTF();
                                        break;
                                    case 0:
                                        Char.myCharz().arrItemShop[num214][num216].buyCoin = msg.reader().readInt();
                                        Char.myCharz().arrItemShop[num214][num216].buyGold = msg.reader().readInt();
                                        break;
                                    case 1:
                                        Char.myCharz().arrItemShop[num214][num216].powerRequire = msg.reader().readLong();
                                        break;
                                    case 2:
                                        Char.myCharz().arrItemShop[num214][num216].itemId = msg.reader().readShort();
                                        Char.myCharz().arrItemShop[num214][num216].buyCoin = msg.reader().readInt();
                                        Char.myCharz().arrItemShop[num214][num216].buyGold = msg.reader().readInt();
                                        Char.myCharz().arrItemShop[num214][num216].buyType = msg.reader().readByte();
                                        Char.myCharz().arrItemShop[num214][num216].quantity = msg.reader().readInt();
                                        Char.myCharz().arrItemShop[num214][num216].isMe = msg.reader().readByte();
                                        break;
                                    case 3:
                                        Char.myCharz().arrItemShop[num214][num216].isBuySpec = true;
                                        Char.myCharz().arrItemShop[num214][num216].iconSpec = msg.reader().readShort();
                                        Char.myCharz().arrItemShop[num214][num216].buySpec = msg.reader().readInt();
                                        break;
                                }
                                int num218 = msg.reader().readUnsignedByte();
                                if (num218 != 0)
                                {
                                    Char.myCharz().arrItemShop[num214][num216].itemOption = new ItemOption[num218];
                                    for (int num219 = 0; num219 < Char.myCharz().arrItemShop[num214][num216].itemOption.Length; num219++)
                                    {
                                        ItemOption itemOption4 = readItemOption(msg);
                                        if (itemOption4 != null)
                                        {
                                            Char.myCharz().arrItemShop[num214][num216].itemOption[num219] = itemOption4;
                                            Char.myCharz().arrItemShop[num214][num216].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[num214][num216]);
                                        }
                                    }
                                }
                                sbyte b81 = msg.reader().readByte();
                                Char.myCharz().arrItemShop[num214][num216].newItem = ((b81 != 0) ? true : false);
                                sbyte b82 = msg.reader().readByte();
                                if (b82 == 1)
                                {
                                    int headTemp = msg.reader().readShort();
                                    int bodyTemp = msg.reader().readShort();
                                    int legTemp = msg.reader().readShort();
                                    int bagTemp = msg.reader().readShort();
                                    Char.myCharz().arrItemShop[num214][num216].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
                                }
                                if (b80 == 2 && GameMidlet.intVERSION >= 237)
                                {
                                    Char.myCharz().arrItemShop[num214][num216].nameNguoiKyGui = msg.reader().readUTF();
                                    Res.err("nguoi ki gui  " + Char.myCharz().arrItemShop[num214][num216].nameNguoiKyGui);
                                }
                            }
                        }
                        if (flag5)
                        {
                            if (b80 != 2)
                            {
                                GameCanvas.panel2 = new Panel();
                                GameCanvas.panel2.tabName[7] = new string[1][] { new string[1] { string.Empty } };
                                GameCanvas.panel2.setTypeBodyOnly();
                                GameCanvas.panel2.show();
                            }
                            else
                            {
                                GameCanvas.panel2 = new Panel();
                                GameCanvas.panel2.setTypeKiGuiOnly();
                                GameCanvas.panel2.show();
                            }
                        }
                        GameCanvas.panel.tabName[1] = GameCanvas.panel.shopTabName;
                        if (b80 == 2)
                        {
                            string[][] array6 = GameCanvas.panel.tabName[1];
                            if (flag5)
                            {
                                GameCanvas.panel.tabName[1] = new string[4][]
                                {
                            array6[0],
                            array6[1],
                            array6[2],
                            array6[3]
                                };
                            }
                            else
                            {
                                GameCanvas.panel.tabName[1] = new string[5][]
                                {
                            array6[0],
                            array6[1],
                            array6[2],
                            array6[3],
                            array6[4]
                                };
                            }
                        }
                        GameCanvas.panel.setTypeShop(b80);
                        GameCanvas.panel.show();
                        if (AutoBuyItemCL.listItemBuy.Count > 0)
                        {
                            AutoBuyItemCL.BlockPaintShop = true;
                        }
                        break;
                    }
                case -41:
                    {
                        sbyte b78 = msg.reader().readByte();
                        Char.myCharz().strLevel = new string[b78];
                        for (int num208 = 0; num208 < b78; num208++)
                        {
                            string text = msg.reader().readUTF();
                            Char.myCharz().strLevel[num208] = text;
                        }
                        Res.outz("---   xong  level caption cmd : " + msg.command);
                        break;
                    }
                case -34:
                    {
                        sbyte b68 = msg.reader().readByte();
                        Res.outz("act= " + b68);
                        if (b68 == 0 && GameScr.gI().magicTree != null)
                        {
                            Res.outz("toi duoc day");
                            MagicTree magicTree = GameScr.gI().magicTree;
                            magicTree.id = msg.reader().readShort();
                            magicTree.name = msg.reader().readUTF();
                            magicTree.name = Res.changeString(magicTree.name);
                            magicTree.x = msg.reader().readShort();
                            magicTree.y = msg.reader().readShort();
                            magicTree.level = msg.reader().readByte();
                            magicTree.currPeas = msg.reader().readShort();
                            magicTree.maxPeas = msg.reader().readShort();
                            Res.outz("curr Peas= " + magicTree.currPeas);
                            magicTree.strInfo = msg.reader().readUTF();
                            magicTree.seconds = msg.reader().readInt();
                            magicTree.timeToRecieve = magicTree.seconds;
                            sbyte b69 = msg.reader().readByte();
                            magicTree.peaPostionX = new int[b69];
                            magicTree.peaPostionY = new int[b69];
                            for (int num180 = 0; num180 < b69; num180++)
                            {
                                magicTree.peaPostionX[num180] = msg.reader().readByte();
                                magicTree.peaPostionY[num180] = msg.reader().readByte();
                            }
                            magicTree.isUpdate = msg.reader().readBool();
                            magicTree.last = (magicTree.cur = mSystem.currentTimeMillis());
                            GameScr.gI().magicTree.isUpdateTree = true;
                        }
                        if (b68 == 1)
                        {
                            myVector = new MyVector();
                            try
                            {
                                while (msg.reader().available() > 0)
                                {
                                    string caption = msg.reader().readUTF();
                                    myVector.addElement(new Command(caption, GameCanvas.instance, 888392, null));
                                }
                            }
                            catch (Exception ex6)
                            {
                                Cout.println("Loi MAGIC_TREE " + ex6.ToString());
                            }
                            GameCanvas.menu.startAt(myVector, 3);
                        }
                        if (b68 == 2)
                        {
                            GameScr.gI().magicTree.remainPeas = msg.reader().readShort();
                            GameScr.gI().magicTree.seconds = msg.reader().readInt();
                            GameScr.gI().magicTree.last = (GameScr.gI().magicTree.cur = mSystem.currentTimeMillis());
                            GameScr.gI().magicTree.isUpdateTree = true;
                            GameScr.gI().magicTree.isPeasEffect = true;
                        }
                        break;
                    }
                case 11:
                    {
                        GameCanvas.debug("SA9", 2);
                        int num9 = msg.reader().readShort();
                        sbyte b10 = msg.reader().readByte();
                        if (b10 != 0)
                        {
                            Mob.arrMobTemplate[num9].data.readDataNewBoss(NinjaUtil.readByteArray(msg), b10);
                        }
                        else
                        {
                            Mob.arrMobTemplate[num9].data.readData(NinjaUtil.readByteArray(msg));
                        }
                        for (int i = 0; i < GameScr.vMob.size(); i++)
                        {
                            mob = (Mob)GameScr.vMob.elementAt(i);
                            if (mob.templateId == num9)
                            {
                                mob.w = Mob.arrMobTemplate[num9].data.width;
                                mob.h = Mob.arrMobTemplate[num9].data.height;
                            }
                        }
                        sbyte[] array2 = NinjaUtil.readByteArray(msg);
                        Image img = Image.createImage(array2, 0, array2.Length);
                        Mob.arrMobTemplate[num9].data.img = img;
                        int num10 = msg.reader().readByte();
                        Mob.arrMobTemplate[num9].data.typeData = num10;
                        if (num10 == 1 || num10 == 2)
                        {
                            readFrameBoss(msg, num9);
                        }
                        break;
                    }
                case -69:
                    Char.myCharz().cMaxStamina = msg.reader().readShort();
                    break;
                case -68:
                    Char.myCharz().cStamina = msg.reader().readShort();
                    break;
                case -67:
                    {
                        demCount += 1f;
                        int num320 = msg.reader().readInt();
                        Res.outz("RECIEVE  hinh small: " + num320);
                        sbyte[] array20 = null;
                        try
                        {
                            array20 = NinjaUtil.readByteArray(msg);
                            Res.outz(">SIZE CHECK= " + array20.Length);
                            if (num320 == 3896)
                            {
                            }
                            SmallImage.imgNew[num320].img = createImage(array20);
                        }
                        catch (Exception)
                        {
                            array20 = null;
                            SmallImage.imgNew[num320].img = Image.createRGBImage(new int[1], 1, 1, bl: true);
                        }
                        if (array20 != null)
                        {
                            Rms.saveRMS(mGraphics.zoomLevel + "Small" + num320, array20);
                        }
                        break;
                    }
                case -66:
                    {
                        short id5 = msg.reader().readShort();
                        sbyte[] data6 = NinjaUtil.readByteArray(msg);
                        EffectData effDataById = Effect.getEffDataById(id5);
                        sbyte b114 = msg.reader().readSByte();
                        if (b114 == 0)
                        {
                            effDataById.readData(data6);
                        }
                        else
                        {
                            effDataById.readDataNewBoss(data6, b114);
                        }
                        sbyte[] array18 = NinjaUtil.readByteArray(msg);
                        effDataById.img = Image.createImage(array18, 0, array18.Length);
                        break;
                    }
                case -32:
                    {
                        short id4 = msg.reader().readShort();
                        int num293 = msg.reader().readInt();
                        sbyte[] array14 = null;
                        Image image = null;
                        try
                        {
                            array14 = new sbyte[num293];
                            for (int num294 = 0; num294 < num293; num294++)
                            {
                                array14[num294] = msg.reader().readByte();
                            }
                            image = Image.createImage(array14, 0, num293);
                            BgItem.imgNew.put(id4 + string.Empty, image);
                        }
                        catch (Exception)
                        {
                            array14 = null;
                            BgItem.imgNew.put(id4 + string.Empty, Image.createRGBImage(new int[1], 1, 1, bl: true));
                        }
                        if (array14 != null)
                        {
                            if (mGraphics.zoomLevel > 1)
                            {
                                Rms.saveRMS(mGraphics.zoomLevel + "bgItem" + id4, array14);
                            }
                            BgItemMn.blendcurrBg(id4, image);
                        }
                        break;
                    }
                case 92:
                    {
                        if (GameCanvas.currentScreen == GameScr.instance)
                        {
                            GameCanvas.endDlg();
                        }
                        string text8 = msg.reader().readUTF();
                        string str2 = msg.reader().readUTF();
                        str2 = Res.changeString(str2);
                        string empty = string.Empty;
                        Char obj11 = null;
                        sbyte b96 = 0;
                        if (!text8.Equals(string.Empty))
                        {
                            obj11 = new Char();
                            obj11.charID = msg.reader().readInt();
                            obj11.head = msg.reader().readShort();
                            obj11.headICON = msg.reader().readShort();
                            obj11.body = msg.reader().readShort();
                            obj11.bag = msg.reader().readShort();
                            obj11.leg = msg.reader().readShort();
                            b96 = msg.reader().readByte();
                            obj11.cName = text8;
                        }
                        empty += str2;
                        InfoDlg.hide();
                        if (text8.Equals(string.Empty))
                        {
                            GameScr.info1.addInfo(empty);
                            break;
                        }
                        GameScr.info2.addInfoWithChar(empty, obj11, b96 == 0);
                        if (GameCanvas.panel.isShow && GameCanvas.panel.type == 8)
                        {
                            GameCanvas.panel.initLogMessage();
                        }
                        break;
                    }
                case -26:
                    {
                        ServerListScreen.testConnect = 2;
                        GameCanvas.debug("SA2", 2);
                        string text7 = msg.reader().readUTF();
                        GameCanvas.startOKDlg(text7);
                        InfoDlg.hide();
                        LoginScr.isContinueToLogin = false;
                        Char.isLoadingMap = false;
                        if (text7.Trim().Contains("Thông tin tài khoản hoặc mật khẩu không chính xác"))
                        {
                            AutoLoginCL.IsEnabled = false;
                        }
                        if (GameCanvas.currentScreen == GameCanvas.loginScr)
                        {
                            GameCanvas.serverScreen.switchToMe();
                        }
                        break;
                    }
                case -25:
                    GameCanvas.debug("SA3", 2);
                    GameScr.info1.addInfo(msg.reader().readUTF());
                    break;
                case 94:
                    GameCanvas.debug("SA3", 2);
                    GameScr.info1.addInfo(msg.reader().readUTF());
                    break;
                case 47:
                    GameCanvas.debug("SA4", 2);
                    GameScr.gI().resetButton();
                    break;
                case 81:
                    {
                        GameCanvas.debug("SXX4", 2);
                        Mob mob8 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob8.isDisable = msg.reader().readBool();
                        break;
                    }
                case 82:
                    {
                        GameCanvas.debug("SXX5", 2);
                        Mob mob6 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob6.isDontMove = msg.reader().readBool();
                        break;
                    }
                case 85:
                    {
                        GameCanvas.debug("SXX5", 2);
                        Mob mob7 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob7.isFire = msg.reader().readBool();
                        break;
                    }
                case 86:
                    {
                        GameCanvas.debug("SXX5", 2);
                        Mob mob4 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob4.isIce = msg.reader().readBool();
                        if (!mob4.isIce)
                        {
                            ServerEffect.addServerEffect(77, mob4.x, mob4.y - 9, 1);
                        }
                        break;
                    }
                case 87:
                    {
                        GameCanvas.debug("SXX5", 2);
                        Mob mob5 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        mob5.isWind = msg.reader().readBool();
                        break;
                    }
                case 56:
                    {
                        GameCanvas.debug("SXX6", 2);
                        obj = null;
                        int num190 = msg.reader().readInt();
                        if (num190 == Char.myCharz().charID)
                        {
                            bool flag3 = false;
                            obj = Char.myCharz();
                            obj.cHP = msg.reader().readLong();
                            long num191 = msg.reader().readLong();
                            Res.outz("dame hit = " + num191);
                            if (num191 != 0)
                            {
                                obj.doInjure();
                            }
                            int num192 = 0;
                            try
                            {
                                flag3 = msg.reader().readBoolean();
                                sbyte b73 = msg.reader().readByte();
                                if (b73 != -1)
                                {
                                    Res.outz("hit eff= " + b73);
                                    EffecMn.addEff(new Effect(b73, obj.cx, obj.cy, 3, 1, -1));
                                }
                            }
                            catch (Exception)
                            {
                            }
                            num191 += num192;
                            if (Char.myCharz().cTypePk != 4)
                            {
                                if (num191 == 0)
                                {
                                    GameScr.startFlyText(mResources.miss, obj.cx, obj.cy - obj.ch, 0, -3, mFont.MISS_ME);
                                }
                                else
                                {
                                    GameScr.startFlyText("-" + num191, obj.cx, obj.cy - obj.ch, 0, -3, flag3 ? mFont.FATAL : mFont.RED);
                                }
                            }
                            break;
                        }
                        obj = GameScr.findCharInMap(num190);
                        if (obj == null)
                        {
                            return;
                        }
                        obj.cHP = msg.reader().readLong();
                        bool flag4 = false;
                        long num193 = msg.reader().readLong();
                        if (num193 != 0)
                        {
                            obj.doInjure();
                        }
                        int num194 = 0;
                        try
                        {
                            flag4 = msg.reader().readBoolean();
                            sbyte b74 = msg.reader().readByte();
                            if (b74 != -1)
                            {
                                Res.outz("hit eff= " + b74);
                                EffecMn.addEff(new Effect(b74, obj.cx, obj.cy, 3, 1, -1));
                            }
                        }
                        catch (Exception)
                        {
                        }
                        num193 += num194;
                        if (obj.cTypePk != 4)
                        {
                            if (num193 == 0)
                            {
                                GameScr.startFlyText(mResources.miss, obj.cx, obj.cy - obj.ch, 0, -3, mFont.MISS);
                            }
                            else
                            {
                                GameScr.startFlyText("-" + num193, obj.cx, obj.cy - obj.ch, 0, -3, flag4 ? mFont.FATAL : mFont.ORANGE);
                            }
                        }
                        break;
                    }
                case 83:
                    {
                        GameCanvas.debug("SXX8", 2);
                        int num182 = msg.reader().readInt();
                        obj = ((num182 != Char.myCharz().charID) ? GameScr.findCharInMap(num182) : Char.myCharz());
                        if (obj == null)
                        {
                            return;
                        }
                        Mob mobToAttack = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        if (obj.mobMe != null)
                        {
                            obj.mobMe.attackOtherMob(mobToAttack);
                        }
                        break;
                    }
                case 84:
                    {
                        int num179 = msg.reader().readInt();
                        if (num179 == Char.myCharz().charID)
                        {
                            obj = Char.myCharz();
                        }
                        else
                        {
                            obj = GameScr.findCharInMap(num179);
                            if (obj == null)
                            {
                                return;
                            }
                        }
                        obj.cHP = obj.cHPFull;
                        obj.cMP = obj.cMPFull;
                        obj.cx = msg.reader().readShort();
                        obj.cy = msg.reader().readShort();
                        obj.liveFromDead();
                        break;
                    }
                case 46:
                    GameCanvas.debug("SA5", 2);
                    Cout.LogWarning("Controler RESET_POINT  " + Char.ischangingMap);
                    Char.isLockKey = false;
                    int x1 = msg.reader().readShort();
                    int y2 = msg.reader().readShort();
                    Char.myCharz().setResetPoint(x1, y2);
                    Char.myCharz().cx = x1;
                    Char.myCharz().cy = y2;
                    break;
                case -29:
                    messageNotLogin(msg);
                    break;
                case -28:
                    messageNotMap(msg);
                    break;
                case -30:
                    messageSubCommand(msg);
                    break;
                case 62:
                    GameCanvas.debug("SZ3", 2);
                    obj = GameScr.findCharInMap(msg.reader().readInt());
                    if (obj != null)
                    {
                        obj.killCharId = Char.myCharz().charID;
                        Char.myCharz().npcFocus = null;
                        Char.myCharz().mobFocus = null;
                        Char.myCharz().itemFocus = null;
                        Char.myCharz().charFocus = obj;
                        Char.isManualFocus = true;
                        GameScr.info1.addInfo(obj.cName + mResources.CUU_SAT);
                    }
                    break;
                case 63:
                    GameCanvas.debug("SZ4", 2);
                    Char.myCharz().killCharId = msg.reader().readInt();
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().charFocus = GameScr.findCharInMap(Char.myCharz().killCharId);
                    Char.isManualFocus = true;
                    break;
                case 64:
                    GameCanvas.debug("SZ5", 2);
                    obj = Char.myCharz();
                    try
                    {
                        obj = GameScr.findCharInMap(msg.reader().readInt());
                    }
                    catch (Exception ex3)
                    {
                        Cout.println("Loi CLEAR_CUU_SAT " + ex3.ToString());
                    }
                    obj.killCharId = -9999;
                    break;
                case 39:
                    GameCanvas.debug("SA49", 2);
                    GameScr.gI().typeTradeOrder = 2;
                    if (GameScr.gI().typeTrade >= 2 && GameScr.gI().typeTradeOrder >= 2)
                    {
                        InfoDlg.showWait();
                    }
                    break;
                case 57:
                    {
                        GameCanvas.debug("SZ6", 2);
                        MyVector myVector2 = new MyVector();
                        myVector2.addElement(new Command(msg.reader().readUTF(), GameCanvas.instance, 88817, null));
                        GameCanvas.menu.startAt(myVector2, 3);
                        break;
                    }
                case 58:
                    {
                        GameCanvas.debug("SZ7", 2);
                        int num175 = msg.reader().readInt();
                        Char obj4 = ((num175 != Char.myCharz().charID) ? GameScr.findCharInMap(num175) : Char.myCharz());
                        obj4.moveFast = new short[3];
                        obj4.moveFast[0] = 0;
                        short num176 = msg.reader().readShort();
                        short num177 = msg.reader().readShort();
                        obj4.moveFast[1] = num176;
                        obj4.moveFast[2] = num177;
                        try
                        {
                            num175 = msg.reader().readInt();
                            Char obj5 = ((num175 != Char.myCharz().charID) ? GameScr.findCharInMap(num175) : Char.myCharz());
                            obj5.cx = num176;
                            obj5.cy = num177;
                        }
                        catch (Exception ex4)
                        {
                            Cout.println("Loi MOVE_FAST " + ex4.ToString());
                        }
                        break;
                    }
                case 88:
                    {
                        string info5 = msg.reader().readUTF();
                        short num324 = msg.reader().readShort();
                        GameCanvas.inputDlg.show(info5, new Command(mResources.ACCEPT, GameCanvas.instance, 88818, num324), TField.INPUT_TYPE_ANY);
                        break;
                    }
                case 27:
                    {
                        myVector = new MyVector();
                        string text10 = msg.reader().readUTF();
                        int num321 = msg.reader().readByte();
                        for (int num322 = 0; num322 < num321; num322++)
                        {
                            string caption4 = msg.reader().readUTF();
                            short num323 = msg.reader().readShort();
                            myVector.addElement(new Command(caption4, GameCanvas.instance, 88819, num323));
                        }
                        GameCanvas.menu.startWithoutCloseButton(myVector, 3);
                        break;
                    }
                case 33:
                    {
                        GameCanvas.debug("SA51", 2);
                        InfoDlg.hide();
                        GameCanvas.clearKeyHold();
                        GameCanvas.clearKeyPressed();
                        myVector = new MyVector();
                        try
                        {
                            while (true)
                            {
                                string caption3 = msg.reader().readUTF();
                                myVector.addElement(new Command(caption3, GameCanvas.instance, 88822, null));
                            }
                        }
                        catch (Exception ex24)
                        {
                            Cout.println("Loi OPEN_UI_MENU " + ex24.ToString());
                        }
                        if (Char.myCharz().npcFocus == null)
                        {
                            return;
                        }
                        for (int num316 = 0; num316 < Char.myCharz().npcFocus.template.menu.Length; num316++)
                        {
                            string[] array19 = Char.myCharz().npcFocus.template.menu[num316];
                            myVector.addElement(new Command(array19[0], GameCanvas.instance, 88820, array19));
                        }
                        GameCanvas.menu.startAt(myVector, 3);
                        break;
                    }
                case 40:
                    {
                        GameCanvas.debug("SA52", 2);
                        GameCanvas.taskTick = 150;
                        short taskId = msg.reader().readShort();
                        sbyte index4 = msg.reader().readByte();
                        string str3 = msg.reader().readUTF();
                        str3 = Res.changeString(str3);
                        string str4 = msg.reader().readUTF();
                        str4 = Res.changeString(str4);
                        string[] array15 = new string[msg.reader().readByte()];
                        string[] array16 = new string[array15.Length];
                        GameScr.tasks = new int[array15.Length];
                        GameScr.mapTasks = new int[array15.Length];
                        short[] array17 = new short[array15.Length];
                        short count = -1;
                        for (int num306 = 0; num306 < array15.Length; num306++)
                        {
                            string str5 = msg.reader().readUTF();
                            str5 = Res.changeString(str5);
                            GameScr.tasks[num306] = msg.reader().readByte();
                            GameScr.mapTasks[num306] = msg.reader().readShort();
                            string str6 = msg.reader().readUTF();
                            str6 = Res.changeString(str6);
                            array17[num306] = -1;
                            array15[num306] = str5;
                            if (!str6.Equals(string.Empty))
                            {
                                array16[num306] = str6;
                            }
                        }
                        try
                        {
                            count = msg.reader().readShort();
                            Cout.println(" TASK_GET count:" + count);
                            for (int num307 = 0; num307 < array15.Length; num307++)
                            {
                                array17[num307] = msg.reader().readShort();
                                Cout.println(num307 + " i TASK_GET   counts[i]:" + array17[num307]);
                            }
                        }
                        catch (Exception ex22)
                        {
                            Cout.println("Loi TASK_GET " + ex22.ToString());
                        }
                        Char.myCharz().taskMaint = new Task(taskId, index4, str3, str4, array15, array17, count, array16);
                        if (Char.myCharz().npcFocus != null)
                        {
                            Npc.clearEffTask();
                        }
                        Char.taskAction(isNextStep: true);
                        break;
                    }
                case 41:
                    {
                        GameCanvas.debug("SA53", 2);
                        GameCanvas.taskTick = 100;
                        Res.outz("TASK NEXT");
                        Task taskMaint = Char.myCharz().taskMaint;
                        Task task = taskMaint;
                        task.index++;
                        Char.myCharz().taskMaint.count = 0;
                        Npc.clearEffTask();
                        Char.taskAction(isNextStep: true);
                        break;
                    }
                case 50:
                    {
                        sbyte b105 = msg.reader().readByte();
                        Panel.vGameInfo.removeAllElements();
                        GameInfo gameInfo = new GameInfo();
                        gameInfo.id = 100;
                        gameInfo.main = "Hướng dẫn sử dụng Mod";
                        gameInfo.content = "Lệnh Chat:\r\nk_x: Đổi khu\r\nfakename_x: x là tên muốn fake\r\ncheat_x: Thay đổi tốc độ của game\r\ns_x: Điều chỉnh tốc độ chạy của nhân vật\r\n\r\nTích hợp sẵn\r\nHiển thị nhân vật trong game\r\nHiển thị thông báo boss\r\nTự cho đậu khi đệ kêu\r\nHiển thị thời gian hồi chiêu\r\nTốc độ game " + ModProCL.timeScale + "\r\n\r\nPhím Tắt:\r\nx: Menu chức năng\r\na: Auto đánh\r\ns: Auto gim boss\r\nd: Đóng băng skill\r\nz: Tự mở/tắt cờ đen\r\nc: Mở Capsunbay\r\nh: Auto phù NRD\r\nj: Load map Trái\r\nk: Load map Giữa\r\nl: Load map Phải\r\nm: Mở tab khu\r\nn: Menu nhặt đồ\r\nb: Danh sách bạn\r\ny: Chat thế giới\r\ne: Auto hồi sinh\r\nt: Bật/tắt TDLT\r\ni: Khóa ID\r\no: Auto NRO đen\r\nf: Bông tai/Hợp thể\r\ng: Gửi giao dịch\r\nv: Chức năng Boss\r\nSHIFT+C: Siêu tối ưu CPU\r\nSHIFT+A: Mặc nhanh set 1\r\nSHIFT+Z: Mặc nhanh set 2\r\nSHIFT+U: Dừng 1 số auto\r\nSHIFT+P: Thoát game\r\np: Kết nối QLTK\r\n\r\n";
                        gameInfo.hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1;
                        Panel.vGameInfo.addElement(gameInfo);
                        bool flag10 = (gameInfo.hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1);
                        gameInfo = new GameInfo();
                        gameInfo.id = 101;
                        gameInfo.main = "Lịch sử thông báo của hệ thống Game";
                        gameInfo.content = "";
                        Panel.vGameInfo.addElement(gameInfo);
                        GameInfo gameInfo2 = new GameInfo();
                        gameInfo2.id = 102;
                        gameInfo2.main = "Xem id map của game";
                        StringBuilder stringBuilder = new StringBuilder();
                        for (int num291 = 0; num291 < TileMap.mapNames.Length; num291++)
                        {
                            stringBuilder.Append(num291).Append(". ").Append(TileMap.mapNames[num291])
                                .Append("\r\n");
                        }
                        gameInfo2.content = stringBuilder.ToString();
                        gameInfo2.hasRead = Rms.loadRMSInt(gameInfo2.id + string.Empty) != -1;
                        Panel.vGameInfo.addElement(gameInfo2);
                        flag10 = (gameInfo.hasRead = Rms.loadRMSInt(gameInfo.id + string.Empty) != -1);
                        for (int num292 = 0; num292 < b105; num292++)
                        {
                            GameInfo gameInfo3 = new GameInfo();
                            gameInfo3.id = msg.reader().readShort();
                            gameInfo3.main = msg.reader().readUTF();
                            gameInfo3.content = msg.reader().readUTF();
                            Panel.vGameInfo.addElement(gameInfo3);
                            bool flag11 = (gameInfo3.hasRead = Rms.loadRMSInt(gameInfo3.id + string.Empty) != -1);
                        }
                        break;
                    }
                case 43:
                    GameCanvas.taskTick = 50;
                    GameCanvas.debug("SA55", 2);
                    Char.myCharz().taskMaint.count = msg.reader().readShort();
                    if (Char.myCharz().npcFocus != null)
                    {
                        Npc.clearEffTask();
                    }
                    try
                    {
                        short x_hint = msg.reader().readShort();
                        short y_hint = msg.reader().readShort();
                        Char.myCharz().x_hint = x_hint;
                        Char.myCharz().y_hint = y_hint;
                    }
                    catch (Exception)
                    {
                    }
                    break;
                case 90:
                    GameCanvas.debug("SA577", 2);
                    requestItemPlayer(msg);
                    break;
                case 29:
                    GameCanvas.debug("SA58", 2);
                    GameScr.gI().openUIZone(msg);
                    AutoBossCL.offPaintZone = false;
                    break;
                case -21:
                    {
                        GameCanvas.debug("SA60", 2);
                        short num289 = msg.reader().readShort();
                        for (int num290 = 0; num290 < GameScr.vItemMap.size(); num290++)
                        {
                            if (((ItemMap)GameScr.vItemMap.elementAt(num290)).itemMapID == num289)
                            {
                                GameScr.vItemMap.removeElementAt(num290);
                                break;
                            }
                        }
                        break;
                    }
                case -20:
                    {
                        GameCanvas.debug("SA61", 2);
                        Char.myCharz().itemFocus = null;
                        short num281 = msg.reader().readShort();
                        for (int num282 = 0; num282 < GameScr.vItemMap.size(); num282++)
                        {
                            ItemMap itemMap4 = (ItemMap)GameScr.vItemMap.elementAt(num282);
                            if (itemMap4.itemMapID != num281)
                            {
                                continue;
                            }
                            itemMap4.setPoint(Char.myCharz().cx, Char.myCharz().cy - 10);
                            string text9 = msg.reader().readUTF();
                            num = 0;
                            try
                            {
                                num = msg.reader().readShort();
                                if (itemMap4.template.type == 9)
                                {
                                    num = msg.reader().readShort();
                                    Char obj12 = Char.myCharz();
                                    Char obj13 = obj12;
                                    Char obj14 = obj13;
                                    Char obj15 = obj14;
                                    Char obj16 = obj15;
                                    Char obj17 = obj16;
                                    Char obj18 = obj17;
                                    Char obj19 = obj18;
                                    Char obj20 = obj19;
                                    Char obj21 = obj20;
                                    Char obj22 = obj21;
                                    Char obj23 = obj22;
                                    obj23.xu += num;
                                    Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
                                }
                                else if (itemMap4.template.type == 10)
                                {
                                    num = msg.reader().readShort();
                                    Char obj24 = Char.myCharz();
                                    Char obj25 = obj24;
                                    Char obj26 = obj25;
                                    Char obj27 = obj26;
                                    Char obj28 = obj27;
                                    Char obj29 = obj28;
                                    Char obj30 = obj29;
                                    Char obj31 = obj30;
                                    Char obj32 = obj31;
                                    Char obj33 = obj32;
                                    Char obj34 = obj33;
                                    Char obj23 = obj34;
                                    obj23.luong += num;
                                    Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                                }
                                else if (itemMap4.template.type == 34)
                                {
                                    num = msg.reader().readShort();
                                    Char obj35 = Char.myCharz();
                                    Char obj36 = obj35;
                                    Char obj37 = obj36;
                                    Char obj38 = obj37;
                                    Char obj39 = obj38;
                                    Char obj40 = obj39;
                                    Char obj41 = obj40;
                                    Char obj42 = obj41;
                                    Char obj43 = obj42;
                                    Char obj44 = obj43;
                                    Char obj45 = obj44;
                                    Char obj23 = obj45;
                                    obj23.luongKhoa += num;
                                    Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                                }
                            }
                            catch (Exception)
                            {
                            }
                            if (text9.Equals(string.Empty))
                            {
                                if (itemMap4.template.type == 9)
                                {
                                    GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.YELLOW);
                                    SoundMn.gI().getItem();
                                }
                                else if (itemMap4.template.type == 10)
                                {
                                    GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.GREEN);
                                    SoundMn.gI().getItem();
                                }
                                else if (itemMap4.template.type == 34)
                                {
                                    GameScr.startFlyText(((num >= 0) ? "+" : string.Empty) + num, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -2, mFont.RED);
                                    SoundMn.gI().getItem();
                                }
                                else
                                {
                                    GameScr.info1.addInfo(mResources.you_receive + " " + ((num <= 0) ? string.Empty : (num + " ")) + itemMap4.template.name);
                                    SoundMn.gI().getItem();
                                }
                                if (num > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 4683)
                                {
                                    ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
                                    ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
                                }
                            }
                            else if (text9.Length == 1)
                            {
                                Cout.LogError3("strInf.Length =1:  " + text9);
                            }
                            else
                            {
                                GameScr.info1.addInfo(text9);
                            }
                            break;
                        }
                        break;
                    }
                case -19:
                    {
                        GameCanvas.debug("SA62", 2);
                        short num278 = msg.reader().readShort();
                        obj = GameScr.findCharInMap(msg.reader().readInt());
                        for (int num279 = 0; num279 < GameScr.vItemMap.size(); num279++)
                        {
                            ItemMap itemMap3 = (ItemMap)GameScr.vItemMap.elementAt(num279);
                            if (itemMap3.itemMapID != num278)
                            {
                                continue;
                            }
                            if (obj == null)
                            {
                                return;
                            }
                            itemMap3.setPoint(obj.cx, obj.cy - 10);
                            if (itemMap3.x < obj.cx)
                            {
                                obj.cdir = -1;
                            }
                            else if (itemMap3.x > obj.cx)
                            {
                                obj.cdir = 1;
                            }
                            break;
                        }
                        break;
                    }
                case -18:
                    {
                        GameCanvas.debug("SA63", 2);
                        int num277 = msg.reader().readByte();
                        GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), Char.myCharz().arrItemBag[num277].template.id, Char.myCharz().cx, Char.myCharz().cy, msg.reader().readShort(), msg.reader().readShort()));
                        Char.myCharz().arrItemBag[num277] = null;
                        break;
                    }
                case 68:
                    {
                        Res.outz("ADD ITEM TO MAP --------------------------------------");
                        GameCanvas.debug("SA6333", 2);
                        short itemMapID = msg.reader().readShort();
                        short itemTemplateID = msg.reader().readShort();
                        int x = msg.reader().readShort();
                        int y = msg.reader().readShort();
                        int num261 = msg.reader().readInt();
                        short r = 0;
                        if (num261 == -2)
                        {
                            r = msg.reader().readShort();
                        }
                        ItemMap itemMap = new ItemMap(num261, itemMapID, itemTemplateID, x, y, r);
                        bool flag8 = false;
                        for (int num262 = 0; num262 < GameScr.vItemMap.size(); num262++)
                        {
                            ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(num262);
                            if (itemMap2.itemMapID == itemMap.itemMapID)
                            {
                                flag8 = true;
                                break;
                            }
                        }
                        if (!flag8)
                        {
                            GameScr.vItemMap.addElement(itemMap);
                        }
                        break;
                    }
                case 69:
                    SoundMn.IsDelAcc = ((msg.reader().readByte() != 0) ? true : false);
                    break;
                case -14:
                    GameCanvas.debug("SA64", 2);
                    obj = GameScr.findCharInMap(msg.reader().readInt());
                    if (obj == null)
                    {
                        return;
                    }
                    GameScr.vItemMap.addElement(new ItemMap(msg.reader().readShort(), msg.reader().readShort(), obj.cx, obj.cy, msg.reader().readShort(), msg.reader().readShort()));
                    break;
                case -22:
                    GameCanvas.debug("SA65", 2);
                    Char.ischangingMap = true;
                    GameScr.gI().timeStartMap = 0;
                    GameScr.gI().timeLengthMap = 0;
                    Char.myCharz().mobFocus = null;
                    Char.myCharz().npcFocus = null;
                    Char.myCharz().charFocus = null;
                    Char.myCharz().itemFocus = null;
                    Char.myCharz().focus.removeAllElements();
                    Char.myCharz().testCharId = -9999;
                    Char.myCharz().killCharId = -9999;
                    GameCanvas.resetBg();
                    GameScr.gI().resetButton();
                    GameScr.gI().center = null;
                    if (Effect.vEffData.size() > 15)
                    {
                        for (int num254 = 0; num254 < 5; num254++)
                        {
                            Effect.vEffData.removeElementAt(0);
                        }
                    }
                    break;
                case -70:
                    {
                        Res.outz("BIG MESSAGE .......................................");
                        GameCanvas.endDlg();
                        int avatar2 = msg.reader().readShort();
                        string chat3 = msg.reader().readUTF();
                        Npc npc11 = new Npc(-1, 0, 0, 0, 0, 0);
                        npc11.avatar = avatar2;
                        ChatPopup.addBigMessage(chat3, 100000, npc11);
                        sbyte b93 = msg.reader().readByte();
                        if (b93 == 0)
                        {
                            ChatPopup.serverChatPopUp.cmdMsg1 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
                            ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 35;
                            ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
                        }
                        if (b93 == 1)
                        {
                            string p = msg.reader().readUTF();
                            string caption2 = msg.reader().readUTF();
                            ChatPopup.serverChatPopUp.cmdMsg1 = new Command(caption2, ChatPopup.serverChatPopUp, 1000, p);
                            ChatPopup.serverChatPopUp.cmdMsg1.x = GameCanvas.w / 2 - 75;
                            ChatPopup.serverChatPopUp.cmdMsg1.y = GameCanvas.h - 35;
                            ChatPopup.serverChatPopUp.cmdMsg2 = new Command(mResources.CLOSE, ChatPopup.serverChatPopUp, 1001, null);
                            ChatPopup.serverChatPopUp.cmdMsg2.x = GameCanvas.w / 2 + 11;
                            ChatPopup.serverChatPopUp.cmdMsg2.y = GameCanvas.h - 35;
                        }
                        break;
                    }
                case 38:
                    {
                        GameCanvas.debug("SA67", 2);
                        InfoDlg.hide();
                        int num247 = msg.reader().readShort();
                        Res.outz("OPEN_UI_SAY ID= " + num247);
                        string str = msg.reader().readUTF();
                        str = Res.changeString(str);
                        for (int num248 = 0; num248 < GameScr.vNpc.size(); num248++)
                        {
                            Npc npc9 = (Npc)GameScr.vNpc.elementAt(num248);
                            Res.outz("npc id= " + npc9.template.npcTemplateId);
                            if (npc9.template.npcTemplateId == num247)
                            {
                                ChatPopup.addChatPopupMultiLine(str, 100000, npc9);
                                GameCanvas.panel.hideNow();
                                return;
                            }
                        }
                        Npc npc10 = new Npc(num247, 0, 0, 0, num247, GameScr.info1.charId[Char.myCharz().cgender][2]);
                        if (npc10.template.npcTemplateId == 5)
                        {
                            npc10.charID = 5;
                        }
                        try
                        {
                            npc10.avatar = msg.reader().readShort();
                        }
                        catch (Exception)
                        {
                        }
                        ChatPopup.addChatPopupMultiLine(str, 100000, npc10);
                        GameCanvas.panel.hideNow();
                        break;
                    }
                case 32:
                    {
                        GameCanvas.debug("SA68", 2);
                        int num222 = msg.reader().readShort();
                        for (int num223 = 0; num223 < GameScr.vNpc.size(); num223++)
                        {
                            Npc npc7 = (Npc)GameScr.vNpc.elementAt(num223);
                            if (npc7.template.npcTemplateId == num222 && npc7.Equals(Char.myCharz().npcFocus))
                            {
                                string chat = msg.reader().readUTF();
                                string[] array9 = new string[msg.reader().readByte()];
                                for (int num224 = 0; num224 < array9.Length; num224++)
                                {
                                    array9[num224] = msg.reader().readUTF();
                                }
                                GameScr.gI().createMenu(array9, npc7);
                                ChatPopup.addChatPopup(chat, 100000, npc7);
                                return;
                            }
                        }
                        Npc npc8 = new Npc(num222, 0, -100, 100, num222, GameScr.info1.charId[Char.myCharz().cgender][2]);
                        Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
                        string chat2 = msg.reader().readUTF();
                        string[] array10 = new string[msg.reader().readByte()];
                        for (int num225 = 0; num225 < array10.Length; num225++)
                        {
                            array10[num225] = msg.reader().readUTF();
                        }
                        try
                        {
                            short avatar = msg.reader().readShort();
                            npc8.avatar = avatar;
                        }
                        catch (Exception)
                        {
                        }
                        Res.outz((Char.myCharz().npcFocus == null) ? "null" : "!null");
                        GameScr.gI().createMenu(array10, npc8);
                        ChatPopup.addChatPopup(chat2, 100000, npc8);
                        break;
                    }
                case 7:
                    {
                        sbyte type3 = msg.reader().readByte();
                        short id2 = msg.reader().readShort();
                        string info2 = msg.reader().readUTF();
                        GameCanvas.panel.saleRequest(type3, info2, id2);
                        break;
                    }
                case 6:
                    GameCanvas.debug("SA70", 2);
                    Char.myCharz().xu = msg.reader().readLong();
                    Char.myCharz().luong = msg.reader().readInt();
                    Char.myCharz().luongKhoa = msg.reader().readInt();
                    Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
                    Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                    Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                    GameCanvas.endDlg();
                    break;
                case -24:
                    Res.outz("***************MAP_INFO**************");
                    GameScr.isPickNgocRong = false;
                    Char.isLoadingMap = true;
                    Cout.println("GET MAP INFO");
                    GameScr.gI().magicTree = null;
                    GameCanvas.isLoading = true;
                    GameCanvas.debug("SA75", 2);
                    GameScr.resetAllvector();
                    GameCanvas.endDlg();
                    TileMap.vGo.removeAllElements();
                    PopUp.vPopups.removeAllElements();
                    mSystem.gcc();
                    TileMap.mapID = msg.reader().readUnsignedByte();
                    TileMap.planetID = msg.reader().readByte();
                    TileMap.tileID = msg.reader().readByte();
                    TileMap.bgID = msg.reader().readByte();
                    GameScr.isPaint_CT = TileMap.mapID != 170;
                    Cout.println("load planet from server: " + TileMap.planetID + "bgType= " + TileMap.bgType + ".............................");
                    TileMap.typeMap = msg.reader().readByte();
                    TileMap.mapName = msg.reader().readUTF();
                    TileMap.zoneID = msg.reader().readByte();
                    GameCanvas.debug("SA75x1", 2);
                    try
                    {
                        TileMap.loadMapFromResource(TileMap.mapID);
                    }
                    catch (Exception)
                    {
                        Service.gI().requestMaptemplate(TileMap.mapID);
                        messWait = msg;
                        break;
                    }
                    loadInfoMap(msg);
                    try
                    {
                        TileMap.isMapDouble = ((msg.reader().readByte() != 0) ? true : false);
                    }
                    catch (Exception)
                    {
                    }
                    GameScr.cmx = GameScr.cmtoX;
                    GameScr.cmy = GameScr.cmtoY;
                    GameCanvas.isRequestMapID = 2;
                    GameCanvas.waitingTimeChangeMap = mSystem.currentTimeMillis() + 1000;
                    break;
                case -31:
                    {
                        TileMap.vItemBg.removeAllElements();
                        short num209 = msg.reader().readShort();
                        Res.err("[ITEM_BACKGROUND] nItem= " + num209);
                        for (int num210 = 0; num210 < num209; num210++)
                        {
                            BgItem bgItem = new BgItem();
                            bgItem.id = num210;
                            bgItem.idImage = msg.reader().readShort();
                            bgItem.layer = msg.reader().readByte();
                            bgItem.dx = msg.reader().readShort();
                            bgItem.dy = msg.reader().readShort();
                            sbyte b79 = msg.reader().readByte();
                            bgItem.tileX = new int[b79];
                            bgItem.tileY = new int[b79];
                            for (int num211 = 0; num211 < b79; num211++)
                            {
                                bgItem.tileX[num210] = msg.reader().readByte();
                                bgItem.tileY[num210] = msg.reader().readByte();
                            }
                            TileMap.vItemBg.addElement(bgItem);
                        }
                        break;
                    }
                case -4:
                    {
                        GameCanvas.debug("SA76", 2);
                        obj = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj == null)
                        {
                            return;
                        }
                        GameCanvas.debug("SA76v1", 2);
                        if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
                        {
                            obj.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 0);
                        }
                        else
                        {
                            obj.setSkillPaint(GameScr.sks[msg.reader().readUnsignedByte()], 1);
                        }
                        GameCanvas.debug("SA76v2", 2);
                        obj.attMobs = new Mob[msg.reader().readByte()];
                        for (int num183 = 0; num183 < obj.attMobs.Length; num183++)
                        {
                            Mob mob3 = (Mob)GameScr.vMob.elementAt(msg.reader().readByte());
                            obj.attMobs[num183] = mob3;
                            if (num183 == 0)
                            {
                                if (obj.cx <= mob3.x)
                                {
                                    obj.cdir = 1;
                                }
                                else
                                {
                                    obj.cdir = -1;
                                }
                            }
                        }
                        GameCanvas.debug("SA76v3", 2);
                        obj.charFocus = null;
                        obj.mobFocus = obj.attMobs[0];
                        Char[] array4 = new Char[10];
                        num = 0;
                        try
                        {
                            for (num = 0; num < array4.Length; num++)
                            {
                                int num184 = msg.reader().readInt();
                                Char obj6 = (array4[num] = ((num184 != Char.myCharz().charID) ? GameScr.findCharInMap(num184) : Char.myCharz()));
                                if (num == 0)
                                {
                                    if (obj.cx <= obj6.cx)
                                    {
                                        obj.cdir = 1;
                                    }
                                    else
                                    {
                                        obj.cdir = -1;
                                    }
                                }
                            }
                        }
                        catch (Exception ex7)
                        {
                            Cout.println("Loi PLAYER_ATTACK_N_P " + ex7.ToString());
                        }
                        GameCanvas.debug("SA76v4", 2);
                        if (num > 0)
                        {
                            obj.attChars = new Char[num];
                            for (num = 0; num < obj.attChars.Length; num++)
                            {
                                obj.attChars[num] = array4[num];
                            }
                            obj.charFocus = obj.attChars[0];
                            obj.mobFocus = null;
                        }
                        GameCanvas.debug("SA76v5", 2);
                        break;
                    }
                case 54:
                    {
                        obj = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj == null)
                        {
                            return;
                        }
                        int num178 = msg.reader().readUnsignedByte();
                        if ((TileMap.tileTypeAtPixel(obj.cx, obj.cy) & 2) == 2)
                        {
                            obj.setSkillPaint(GameScr.sks[num178], 0);
                        }
                        else
                        {
                            obj.setSkillPaint(GameScr.sks[num178], 1);
                        }
                        Mob[] array3 = new Mob[10];
                        num = 0;
                        try
                        {
                            for (num = 0; num < array3.Length; num++)
                            {
                                Mob mob2 = (array3[num] = (Mob)GameScr.vMob.elementAt(msg.reader().readByte()));
                                if (num == 0)
                                {
                                    if (obj.cx <= mob2.x)
                                    {
                                        obj.cdir = 1;
                                    }
                                    else
                                    {
                                        obj.cdir = -1;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (num > 0)
                        {
                            obj.attMobs = new Mob[num];
                            for (num = 0; num < obj.attMobs.Length; num++)
                            {
                                obj.attMobs[num] = array3[num];
                            }
                            obj.charFocus = null;
                            obj.mobFocus = obj.attMobs[0];
                        }
                        break;
                    }
                case -60:
                    {
                        GameCanvas.debug("SA7666", 2);
                        int num2 = msg.reader().readInt();
                        int num3 = -1;
                        if (num2 != Char.myCharz().charID)
                        {
                            Char obj2 = GameScr.findCharInMap(num2);
                            if (obj2 == null)
                            {
                                return;
                            }
                            if (obj2.currentMovePoint != null)
                            {
                                obj2.createShadow(obj2.cx, obj2.cy, 10);
                                obj2.cx = obj2.currentMovePoint.xEnd;
                                obj2.cy = obj2.currentMovePoint.yEnd;
                            }
                            int num4 = msg.reader().readUnsignedByte();
                            if ((TileMap.tileTypeAtPixel(obj2.cx, obj2.cy) & 2) == 2)
                            {
                                obj2.setSkillPaint(GameScr.sks[num4], 0);
                            }
                            else
                            {
                                obj2.setSkillPaint(GameScr.sks[num4], 1);
                            }
                            sbyte b = msg.reader().readByte();
                            Char[] array = new Char[b];
                            for (num = 0; num < array.Length; num++)
                            {
                                num3 = msg.reader().readInt();
                                Char obj3 = (array[num] = ((num3 != Char.myCharz().charID) ? GameScr.findCharInMap(num3) : Char.myCharz()));
                                if (num == 0)
                                {
                                    if (obj2.cx <= obj3.cx)
                                    {
                                        obj2.cdir = 1;
                                    }
                                    else
                                    {
                                        obj2.cdir = -1;
                                    }
                                }
                            }
                            if (num > 0)
                            {
                                obj2.attChars = new Char[num];
                                for (num = 0; num < obj2.attChars.Length; num++)
                                {
                                    obj2.attChars[num] = array[num];
                                }
                                obj2.mobFocus = null;
                                obj2.charFocus = obj2.attChars[0];
                            }
                        }
                        else
                        {
                            sbyte b2 = msg.reader().readByte();
                            sbyte b3 = msg.reader().readByte();
                            num3 = msg.reader().readInt();
                        }
                        try
                        {
                            sbyte b4 = msg.reader().readByte();
                            Res.outz("isRead continue = " + b4);
                            if (b4 != 1)
                            {
                                break;
                            }
                            sbyte b5 = msg.reader().readByte();
                            Res.outz("type skill = " + b5);
                            if (num3 == Char.myCharz().charID)
                            {
                                bool flag = false;
                                obj = Char.myCharz();
                                long num5 = msg.reader().readLong();
                                Res.outz("dame hit = " + num5);
                                obj.isDie = msg.reader().readBoolean();
                                if (obj.isDie)
                                {
                                    Char.isLockKey = true;
                                }
                                Res.outz("isDie=" + obj.isDie + "---------------------------------------");
                                int num6 = 0;
                                flag = (obj.isCrit = msg.reader().readBoolean());
                                obj.isMob = false;
                                num5 = (obj.damHP = num5 + num6);
                                if (b5 == 0)
                                {
                                    obj.doInjure(num5, 0L, flag, isMob: false);
                                }
                            }
                            else
                            {
                                obj = GameScr.findCharInMap(num3);
                                if (obj == null)
                                {
                                    return;
                                }
                                bool flag2 = false;
                                long num7 = msg.reader().readLong();
                                Res.outz("dame hit= " + num7);
                                obj.isDie = msg.reader().readBoolean();
                                Res.outz("isDie=" + obj.isDie + "---------------------------------------");
                                int num8 = 0;
                                flag2 = (obj.isCrit = msg.reader().readBoolean());
                                obj.isMob = false;
                                num7 = (obj.damHP = num7 + num8);
                                if (b5 == 0)
                                {
                                    obj.doInjure(num7, 0L, flag2, isMob: false);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    }
            }
            switch (msg.command)
            {
                case -2:
                    {
                        GameCanvas.debug("SA77", 22);
                        int num335 = msg.reader().readInt();
                        Char obj62 = Char.myCharz();
                        Char obj63 = obj62;
                        Char obj64 = obj63;
                        Char obj65 = obj64;
                        Char obj66 = obj65;
                        Char obj67 = obj66;
                        Char obj68 = obj67;
                        Char obj69 = obj68;
                        Char obj70 = obj69;
                        Char obj71 = obj70;
                        Char obj72 = obj71;
                        Char obj23 = obj72;
                        obj23.yen += num335;
                        GameScr.startFlyText((num335 <= 0) ? (string.Empty + num335) : ("+" + num335), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
                        break;
                    }
                case 95:
                    {
                        GameCanvas.debug("SA77", 22);
                        int num347 = msg.reader().readInt();
                        Char obj107 = Char.myCharz();
                        Char obj108 = obj107;
                        Char obj109 = obj108;
                        Char obj110 = obj109;
                        Char obj111 = obj110;
                        Char obj112 = obj111;
                        Char obj113 = obj112;
                        Char obj114 = obj113;
                        Char obj115 = obj114;
                        Char obj116 = obj115;
                        Char obj117 = obj116;
                        Char obj23 = obj117;
                        obj23.xu += num347;
                        Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
                        GameScr.startFlyText((num347 <= 0) ? (string.Empty + num347) : ("+" + num347), Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
                        break;
                    }
                case 96:
                    GameCanvas.debug("SA77a", 22);
                    Char.myCharz().taskOrders.addElement(new TaskOrder(msg.reader().readByte(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readUTF(), msg.reader().readUTF(), msg.reader().readByte(), msg.reader().readByte()));
                    break;
                case 97:
                    {
                        sbyte b120 = msg.reader().readByte();
                        for (int num348 = 0; num348 < Char.myCharz().taskOrders.size(); num348++)
                        {
                            TaskOrder taskOrder = (TaskOrder)Char.myCharz().taskOrders.elementAt(num348);
                            if (taskOrder.taskId == b120)
                            {
                                taskOrder.count = msg.reader().readShort();
                                break;
                            }
                        }
                        break;
                    }
                case -1:
                    {
                        GameCanvas.debug("SA77", 222);
                        int num331 = msg.reader().readInt();
                        Char obj50 = Char.myCharz();
                        Char obj51 = obj50;
                        Char obj52 = obj51;
                        Char obj53 = obj52;
                        Char obj54 = obj53;
                        Char obj55 = obj54;
                        Char obj56 = obj55;
                        Char obj57 = obj56;
                        Char obj58 = obj57;
                        Char obj59 = obj58;
                        Char obj60 = obj59;
                        Char obj23 = obj60;
                        obj23.xu += num331;
                        Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
                        obj50 = Char.myCharz();
                        obj51 = obj50;
                        obj52 = obj51;
                        obj53 = obj52;
                        obj54 = obj53;
                        obj55 = obj54;
                        obj56 = obj55;
                        obj57 = obj56;
                        obj58 = obj57;
                        obj59 = obj58;
                        obj60 = obj59;
                        obj23 = obj60;
                        obj23.yen -= num331;
                        GameScr.startFlyText("+" + num331, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 10, 0, -2, mFont.YELLOW);
                        break;
                    }
                case -3:
                    {
                        GameCanvas.debug("SA78", 2);
                        sbyte b118 = msg.reader().readByte();
                        int num342 = msg.reader().readInt();
                        if (b118 == 0)
                        {
                            Char obj74 = Char.myCharz();
                            Char obj75 = obj74;
                            Char obj76 = obj75;
                            Char obj77 = obj76;
                            Char obj78 = obj77;
                            Char obj79 = obj78;
                            Char obj80 = obj79;
                            Char obj81 = obj80;
                            Char obj82 = obj81;
                            Char obj83 = obj82;
                            Char obj84 = obj83;
                            Char obj23 = obj84;
                            obj23.cPower += num342;
                        }
                        if (b118 == 1)
                        {
                            Char obj85 = Char.myCharz();
                            Char obj86 = obj85;
                            Char obj87 = obj86;
                            Char obj88 = obj87;
                            Char obj89 = obj88;
                            Char obj90 = obj89;
                            Char obj91 = obj90;
                            Char obj92 = obj91;
                            Char obj93 = obj92;
                            Char obj94 = obj93;
                            Char obj95 = obj94;
                            Char obj23 = obj95;
                            obj23.cTiemNang += num342;
                        }
                        if (b118 == 2)
                        {
                            Char obj96 = Char.myCharz();
                            Char obj97 = obj96;
                            Char obj98 = obj97;
                            Char obj99 = obj98;
                            Char obj100 = obj99;
                            Char obj101 = obj100;
                            Char obj102 = obj101;
                            Char obj103 = obj102;
                            Char obj104 = obj103;
                            Char obj105 = obj104;
                            Char obj106 = obj105;
                            Char obj23 = obj106;
                            obj23.cPower += num342;
                            obj96 = Char.myCharz();
                            obj97 = obj96;
                            obj98 = obj97;
                            obj99 = obj98;
                            obj100 = obj99;
                            obj101 = obj100;
                            obj102 = obj101;
                            obj103 = obj102;
                            obj104 = obj103;
                            obj105 = obj104;
                            obj106 = obj105;
                            obj23 = obj106;
                            obj23.cTiemNang += num342;
                        }
                        Char.myCharz().applyCharLevelPercent();
                        if (Char.myCharz().cTypePk != 3)
                        {
                            GameScr.startFlyText(((num342 <= 0) ? string.Empty : "+") + num342, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch, 0, -4, mFont.GREEN);
                            if (num342 > 0 && Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5002)
                            {
                                ServerEffect.addServerEffect(55, Char.myCharz().petFollow.cmx, Char.myCharz().petFollow.cmy, 1);
                                ServerEffect.addServerEffect(55, Char.myCharz().cx, Char.myCharz().cy, 1);
                            }
                        }
                        break;
                    }
                case -73:
                    {
                        sbyte b121 = msg.reader().readByte();
                        for (int num349 = 0; num349 < GameScr.vNpc.size(); num349++)
                        {
                            Npc npc12 = (Npc)GameScr.vNpc.elementAt(num349);
                            if (npc12.template.npcTemplateId == b121)
                            {
                                if (msg.reader().readByte() == 0)
                                {
                                    npc12.isHide = true;
                                }
                                else
                                {
                                    npc12.isHide = false;
                                }
                                break;
                            }
                        }
                        break;
                    }
                case -5:
                    {
                        GameCanvas.debug("SA79", 2);
                        int num337 = msg.reader().readInt();
                        int num338 = msg.reader().readInt();
                        Char obj73;
                        if (num338 != -100)
                        {
                            obj73 = new Char();
                            obj73.charID = num337;
                            obj73.clanID = num338;
                        }
                        else
                        {
                            obj73 = new Mabu();
                            obj73.charID = num337;
                            obj73.clanID = num338;
                        }
                        if (obj73.clanID == -2)
                        {
                            obj73.isCopy = true;
                        }
                        if (readCharInfo(obj73, msg))
                        {
                            sbyte b117 = msg.reader().readByte();
                            if (obj73.cy <= 10 && b117 != 0 && b117 != 2)
                            {
                                Res.outz("nhân vật bay trên trời xuống x= " + obj73.cx + " y= " + obj73.cy);
                                Teleport teleport3 = new Teleport(obj73.cx, obj73.cy, obj73.head, obj73.cdir, 1, isMe: false, (b117 != 1) ? b117 : obj73.cgender);
                                teleport3.id = obj73.charID;
                                obj73.isTeleport = true;
                                Teleport.addTeleport(teleport3);
                            }
                            if (b117 == 2)
                            {
                                obj73.show();
                            }
                            for (int num339 = 0; num339 < GameScr.vMob.size(); num339++)
                            {
                                Mob mob20 = (Mob)GameScr.vMob.elementAt(num339);
                                if (mob20 != null && mob20.isMobMe && mob20.mobId == obj73.charID)
                                {
                                    Res.outz("co 1 con quai");
                                    obj73.mobMe = mob20;
                                    obj73.mobMe.x = obj73.cx;
                                    obj73.mobMe.y = obj73.cy - 40;
                                    break;
                                }
                            }
                            GameScr.RemovefindCharInMap(num337);
                            GameScr.vCharInMap.addElement(obj73);
                            obj73.isMonkey = msg.reader().readByte();
                            short num340 = msg.reader().readShort();
                            Res.outz("mount id= " + num340 + "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                            if (num340 != -1)
                            {
                                obj73.isHaveMount = true;
                                switch (num340)
                                {
                                    case 346:
                                    case 347:
                                    case 348:
                                        obj73.isMountVip = false;
                                        break;
                                    case 349:
                                    case 350:
                                    case 351:
                                        obj73.isMountVip = true;
                                        break;
                                    case 396:
                                        obj73.isEventMount = true;
                                        break;
                                    case 532:
                                        obj73.isSpeacialMount = true;
                                        break;
                                    default:
                                        if (num340 >= Char.ID_NEW_MOUNT)
                                        {
                                            obj73.idMount = num340;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                obj73.isHaveMount = false;
                            }
                        }
                        sbyte cFlag = msg.reader().readByte();
                        Res.outz("addplayer:   " + cFlag);
                        obj73.cFlag = cFlag;
                        obj73.isNhapThe = msg.reader().readByte() == 1;
                        try
                        {
                            obj73.idAuraEff = msg.reader().readShort();
                            obj73.idEff_Set_Item = msg.reader().readSByte();
                            obj73.idHat = msg.reader().readShort();
                            if (obj73.bag >= 201 && obj73.bag < 255)
                            {
                                Effect effect3 = new Effect(obj73.bag, obj73, 2, -1, 10, 1);
                                effect3.typeEff = 5;
                                obj73.addEffChar(effect3);
                            }
                            else
                            {
                                for (int num341 = 0; num341 < 54; num341++)
                                {
                                    obj73.removeEffChar(0, 201 + num341);
                                }
                            }
                        }
                        catch (Exception ex38)
                        {
                            Res.outz("cmd: -5 err: " + ex38.StackTrace);
                        }
                        GameScr.gI().getFlagImage(obj73.charID, obj73.cFlag);
                        break;
                    }
                case -7:
                    {
                        GameCanvas.debug("SA80", 2);
                        int num332 = msg.reader().readInt();
                        for (int num333 = 0; num333 < GameScr.vCharInMap.size(); num333++)
                        {
                            Char obj61 = null;
                            try
                            {
                                obj61 = (Char)GameScr.vCharInMap.elementAt(num333);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            if (obj61 == null || obj61.charID != num332)
                            {
                                continue;
                            }
                            GameCanvas.debug("SA8x2y" + num333, 2);
                            obj61.moveTo(msg.reader().readShort(), msg.reader().readShort(), 0);
                            obj61.lastUpdateTime = mSystem.currentTimeMillis();
                            break;
                        }
                        GameCanvas.debug("SA80x3", 2);
                        break;
                    }
                case -6:
                    {
                        GameCanvas.debug("SA81", 2);
                        int num329 = msg.reader().readInt();
                        for (int num330 = 0; num330 < GameScr.vCharInMap.size(); num330++)
                        {
                            Char obj49 = (Char)GameScr.vCharInMap.elementAt(num330);
                            if (obj49 != null && obj49.charID == num329)
                            {
                                if (!obj49.isInvisiblez && !obj49.isUsePlane)
                                {
                                    ServerEffect.addServerEffect(60, obj49.cx, obj49.cy, 1);
                                }
                                if (!obj49.isUsePlane)
                                {
                                    GameScr.vCharInMap.removeElementAt(num330);
                                }
                                return;
                            }
                        }
                        break;
                    }
                case -13:
                    {
                        GameCanvas.debug("SA82", 2);
                        int num343 = msg.reader().readUnsignedByte();
                        if (num343 > GameScr.vMob.size() - 1 || num343 < 0)
                        {
                            return;
                        }
                        Mob mob21 = (Mob)GameScr.vMob.elementAt(num343);
                        mob21.sys = msg.reader().readByte();
                        mob21.levelBoss = msg.reader().readByte();
                        if (mob21.levelBoss != 0)
                        {
                            mob21.typeSuperEff = Res.random(0, 3);
                        }
                        mob21.x = mob21.xFirst;
                        mob21.y = mob21.yFirst;
                        mob21.status = 5;
                        mob21.injureThenDie = false;
                        mob21.hp = msg.reader().readLong();
                        mob21.maxHp = mob21.hp;
                        mob21.updateHp_bar();
                        ServerEffect.addServerEffect(60, mob21.x, mob21.y, 1);
                        break;
                    }
                case -75:
                    {
                        Mob mob18 = null;
                        try
                        {
                            mob18 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        if (mob18 != null)
                        {
                            mob18.levelBoss = msg.reader().readByte();
                            if (mob18.levelBoss > 0)
                            {
                                mob18.typeSuperEff = Res.random(0, 3);
                            }
                        }
                        break;
                    }
                case -9:
                    {
                        GameCanvas.debug("SA83", 2);
                        Mob mob17 = null;
                        try
                        {
                            mob17 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        GameCanvas.debug("SA83v1", 2);
                        if (mob17 != null)
                        {
                            mob17.hp = msg.reader().readLong();
                            mob17.updateHp_bar();
                            long num334 = msg.reader().readLong();
                            if (num334 == 1)
                            {
                                return;
                            }
                            if (num334 > 1)
                            {
                                mob17.setInjure();
                            }
                            bool flag12 = false;
                            try
                            {
                                flag12 = msg.reader().readBoolean();
                            }
                            catch (Exception)
                            {
                            }
                            sbyte b116 = msg.reader().readByte();
                            if (b116 != -1)
                            {
                                EffecMn.addEff(new Effect(b116, mob17.x, mob17.getY(), 3, 1, -1));
                            }
                            GameCanvas.debug("SA83v2", 2);
                            if (flag12)
                            {
                                GameScr.startFlyText("-" + num334, mob17.x, mob17.getY() - mob17.getH(), 0, -2, mFont.FATAL);
                            }
                            else if (num334 == 0)
                            {
                                mob17.x = mob17.xFirst;
                                mob17.y = mob17.yFirst;
                                GameScr.startFlyText(mResources.miss, mob17.x, mob17.getY() - mob17.getH(), 0, -2, mFont.MISS);
                            }
                            else if (num334 > 1)
                            {
                                GameScr.startFlyText("-" + num334, mob17.x, mob17.getY() - mob17.getH(), 0, -2, mFont.ORANGE);
                            }
                        }
                        GameCanvas.debug("SA83v3", 2);
                        break;
                    }
                case 45:
                    {
                        GameCanvas.debug("SA84", 2);
                        Mob mob15 = null;
                        try
                        {
                            mob15 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception ex29)
                        {
                            Cout.println("Loi tai NPC_MISS  " + ex29.ToString());
                        }
                        if (mob15 != null)
                        {
                            mob15.hp = msg.reader().readLong();
                            mob15.updateHp_bar();
                            GameScr.startFlyText(mResources.miss, mob15.x, mob15.y - mob15.h, 0, -2, mFont.MISS);
                        }
                        break;
                    }
                case -12:
                    {
                        Res.outz("SERVER SEND MOB DIE");
                        GameCanvas.debug("SA85", 2);
                        Mob mob22 = null;
                        try
                        {
                            mob22 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                            Cout.println("LOi tai NPC_DIE cmd " + msg.command);
                        }
                        if (mob22 == null || mob22.status == 0 || mob22.status == 0)
                        {
                            break;
                        }
                        mob22.startDie();
                        try
                        {
                            long num344 = msg.reader().readLong();
                            if (msg.reader().readBool())
                            {
                                GameScr.startFlyText("-" + num344, mob22.x, mob22.y - mob22.h, 0, -2, mFont.FATAL);
                            }
                            else
                            {
                                GameScr.startFlyText("-" + num344, mob22.x, mob22.y - mob22.h, 0, -2, mFont.ORANGE);
                            }
                            sbyte b119 = msg.reader().readByte();
                            for (int num345 = 0; num345 < b119; num345++)
                            {
                                ItemMap itemMap6 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob22.x, mob22.y, msg.reader().readShort(), msg.reader().readShort());
                                int num346 = (itemMap6.playerId = msg.reader().readInt());
                                Res.outz("playerid= " + num346 + " my id= " + Char.myCharz().charID);
                                GameScr.vItemMap.addElement(itemMap6);
                                if (Res.abs(itemMap6.y - Char.myCharz().cy) < 24 && Res.abs(itemMap6.x - Char.myCharz().cx) < 24)
                                {
                                    Char.myCharz().charFocus = null;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    }
                case 74:
                    {
                        GameCanvas.debug("SA85", 2);
                        Mob mob16 = null;
                        try
                        {
                            mob16 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                            Cout.println("Loi tai NPC CHANGE " + msg.command);
                        }
                        if (mob16 != null && mob16.status != 0 && mob16.status != 0)
                        {
                            mob16.status = 0;
                            ServerEffect.addServerEffect(60, mob16.x, mob16.y, 1);
                            ItemMap itemMap5 = new ItemMap(msg.reader().readShort(), msg.reader().readShort(), mob16.x, mob16.y, msg.reader().readShort(), msg.reader().readShort());
                            GameScr.vItemMap.addElement(itemMap5);
                            if (Res.abs(itemMap5.y - Char.myCharz().cy) < 24 && Res.abs(itemMap5.x - Char.myCharz().cx) < 24)
                            {
                                Char.myCharz().charFocus = null;
                            }
                        }
                        break;
                    }
                case -11:
                    {
                        GameCanvas.debug("SA86", 2);
                        Mob mob14 = null;
                        try
                        {
                            int index6 = msg.reader().readUnsignedByte();
                            mob14 = (Mob)GameScr.vMob.elementAt(index6);
                        }
                        catch (Exception ex27)
                        {
                            Res.outz("Loi tai NPC_ATTACK_ME " + msg.command + " err= " + ex27.StackTrace);
                        }
                        if (mob14 != null)
                        {
                            Char.myCharz().isDie = false;
                            Char.isLockKey = false;
                            long num326 = msg.reader().readLong();
                            long num327;
                            try
                            {
                                num327 = msg.reader().readLong();
                            }
                            catch (Exception)
                            {
                                num327 = 0L;
                            }
                            if (mob14.isBusyAttackSomeOne)
                            {
                                Char.myCharz().doInjure(num326, num327, isCrit: false, isMob: true);
                                break;
                            }
                            mob14.dame = num326;
                            mob14.dameMp = num327;
                            mob14.setAttack(Char.myCharz());
                        }
                        break;
                    }
                case -10:
                    {
                        GameCanvas.debug("SA87", 2);
                        Mob mob19 = null;
                        try
                        {
                            mob19 = (Mob)GameScr.vMob.elementAt(msg.reader().readUnsignedByte());
                        }
                        catch (Exception)
                        {
                        }
                        GameCanvas.debug("SA87x1", 2);
                        if (mob19 != null)
                        {
                            GameCanvas.debug("SA87x2", 2);
                            obj = GameScr.findCharInMap(msg.reader().readInt());
                            if (obj == null)
                            {
                                return;
                            }
                            GameCanvas.debug("SA87x3", 2);
                            long num336 = msg.reader().readLong();
                            mob19.dame = obj.cHP - num336;
                            obj.cHPNew = num336;
                            GameCanvas.debug("SA87x4", 2);
                            try
                            {
                                obj.cMP = msg.reader().readLong();
                            }
                            catch (Exception)
                            {
                            }
                            GameCanvas.debug("SA87x5", 2);
                            if (mob19.isBusyAttackSomeOne)
                            {
                                obj.doInjure(mob19.dame, 0L, isCrit: false, isMob: true);
                            }
                            else
                            {
                                mob19.setAttack(obj);
                            }
                            GameCanvas.debug("SA87x6", 2);
                        }
                        break;
                    }
                case -17:
                    GameCanvas.debug("SA88", 2);
                    Char.myCharz().meDead = true;
                    Char.myCharz().cPk = msg.reader().readByte();
                    Char.myCharz().startDie(msg.reader().readShort(), msg.reader().readShort());
                    try
                    {
                        Char.myCharz().cPower = msg.reader().readLong();
                        Char.myCharz().applyCharLevelPercent();
                    }
                    catch (Exception)
                    {
                        Cout.println("Loi tai ME_DIE " + msg.command);
                    }
                    Char.myCharz().countKill = 0;
                    break;
                case 66:
                    Res.outz("ME DIE XP DOWN NOT IMPLEMENT YET!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    break;
                case -8:
                    GameCanvas.debug("SA89", 2);
                    obj = GameScr.findCharInMap(msg.reader().readInt());
                    if (obj == null)
                    {
                        return;
                    }
                    obj.cPk = msg.reader().readByte();
                    obj.waitToDie(msg.reader().readShort(), msg.reader().readShort());
                    break;
                case -16:
                    GameCanvas.debug("SA90", 2);
                    if (Char.myCharz().wdx != 0 || Char.myCharz().wdy != 0)
                    {
                        Char.myCharz().cx = Char.myCharz().wdx;
                        Char.myCharz().cy = Char.myCharz().wdy;
                        Char.myCharz().wdx = (Char.myCharz().wdy = 0);
                    }
                    Char.myCharz().liveFromDead();
                    Char.myCharz().isLockMove = false;
                    Char.myCharz().meDead = false;
                    break;
                case 44:
                    {
                        GameCanvas.debug("SA91", 2);
                        int num328 = msg.reader().readInt();
                        string text11 = msg.reader().readUTF();
                        Res.outz("user id= " + num328 + " text= " + text11);
                        obj = ((Char.myCharz().charID != num328) ? GameScr.findCharInMap(num328) : Char.myCharz());
                        if (obj == null)
                        {
                            return;
                        }
                        obj.addInfo(text11);
                        break;
                    }
                case 18:
                    {
                        sbyte b115 = msg.reader().readByte();
                        for (int num325 = 0; num325 < b115; num325++)
                        {
                            int charId = msg.reader().readInt();
                            int cx = msg.reader().readShort();
                            int cy = msg.reader().readShort();
                            long cHPShow = msg.reader().readLong();
                            Char obj48 = GameScr.findCharInMap(charId);
                            if (obj48 != null)
                            {
                                obj48.cx = cx;
                                obj48.cy = cy;
                                obj48.cHP = (obj48.cHPShow = cHPShow);
                                obj48.lastUpdateTime = mSystem.currentTimeMillis();
                            }
                        }
                        break;
                    }
                case 19:
                    Char.myCharz().countKill = msg.reader().readUnsignedShort();
                    Char.myCharz().countKillMax = msg.reader().readUnsignedShort();
                    break;
            }
            GameCanvas.debug("SA92", 2);
        }
        catch (Exception ex41)
        {
            Res.err("[Controller] [error] " + ex41.StackTrace + " msg: " + ex41.Message + " cause " + ex41.Data);
        }
        finally
        {
            msg?.cleanup();
        }
    }

    private void readLogin(Message msg)
    {
        sbyte b = msg.reader().readByte();
        ChooseCharScr.playerData = new PlayerData[b];
        Res.outz("[LEN] sl nguoi choi " + b);
        for (int i = 0; i < b; i++)
        {
            int playerID = msg.reader().readInt();
            string name = msg.reader().readUTF();
            short head = msg.reader().readShort();
            short body = msg.reader().readShort();
            short leg = msg.reader().readShort();
            long ppoint = msg.reader().readLong();
            ChooseCharScr.playerData[i] = new PlayerData(playerID, name, head, body, leg, ppoint);
        }
        GameCanvas.chooseCharScr.switchToMe();
        GameCanvas.chooseCharScr.updateChooseCharacter((byte)b);
    }

    private void createSkill(myReader d)
    {
        GameScr.vcSkill = d.readByte();
        GameScr.gI().sOptionTemplates = new SkillOptionTemplate[d.readByte()];
        for (int i = 0; i < GameScr.gI().sOptionTemplates.Length; i++)
        {
            GameScr.gI().sOptionTemplates[i] = new SkillOptionTemplate();
            GameScr.gI().sOptionTemplates[i].id = i;
            GameScr.gI().sOptionTemplates[i].name = d.readUTF();
        }
        GameScr.nClasss = new NClass[d.readByte()];
        for (int j = 0; j < GameScr.nClasss.Length; j++)
        {
            GameScr.nClasss[j] = new NClass();
            GameScr.nClasss[j].classId = j;
            GameScr.nClasss[j].name = d.readUTF();
            GameScr.nClasss[j].skillTemplates = new SkillTemplate[d.readByte()];
            for (int k = 0; k < GameScr.nClasss[j].skillTemplates.Length; k++)
            {
                GameScr.nClasss[j].skillTemplates[k] = new SkillTemplate();
                GameScr.nClasss[j].skillTemplates[k].id = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].name = d.readUTF();
                GameScr.nClasss[j].skillTemplates[k].maxPoint = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].manaUseType = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].type = d.readByte();
                GameScr.nClasss[j].skillTemplates[k].iconId = d.readShort();
                GameScr.nClasss[j].skillTemplates[k].damInfo = d.readUTF();
                int lineWidth = 130;
                if (GameCanvas.w == 128 || GameCanvas.h <= 208)
                {
                    lineWidth = 100;
                }
                GameScr.nClasss[j].skillTemplates[k].description = mFont.tahoma_7_green2.splitFontArray(d.readUTF(), lineWidth);
                GameScr.nClasss[j].skillTemplates[k].skills = new Skill[d.readByte()];
                for (int l = 0; l < GameScr.nClasss[j].skillTemplates[k].skills.Length; l++)
                {
                    GameScr.nClasss[j].skillTemplates[k].skills[l] = new Skill();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].skillId = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].template = GameScr.nClasss[j].skillTemplates[k];
                    GameScr.nClasss[j].skillTemplates[k].skills[l].point = d.readByte();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].powRequire = d.readLong();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].manaUse = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].coolDown = d.readInt();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].dx = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].dy = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].maxFight = d.readByte();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].damage = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].price = d.readShort();
                    GameScr.nClasss[j].skillTemplates[k].skills[l].moreInfo = d.readUTF();
                    Skills.add(GameScr.nClasss[j].skillTemplates[k].skills[l]);
                }
            }
        }
    }

    private void createMap(myReader d)
    {
        GameScr.vcMap = d.readByte();
        TileMap.mapNames = new string[d.readShort()];
        for (int i = 0; i < TileMap.mapNames.Length; i++)
        {
            TileMap.mapNames[i] = d.readUTF();
        }
        Npc.arrNpcTemplate = new NpcTemplate[d.readByte()];
        for (sbyte b = 0; b < Npc.arrNpcTemplate.Length; b++)
        {
            Npc.arrNpcTemplate[b] = new NpcTemplate();
            Npc.arrNpcTemplate[b].npcTemplateId = b;
            Npc.arrNpcTemplate[b].name = d.readUTF();
            Npc.arrNpcTemplate[b].headId = d.readShort();
            Npc.arrNpcTemplate[b].bodyId = d.readShort();
            Npc.arrNpcTemplate[b].legId = d.readShort();
            Npc.arrNpcTemplate[b].menu = new string[d.readByte()][];
            for (int j = 0; j < Npc.arrNpcTemplate[b].menu.Length; j++)
            {
                Npc.arrNpcTemplate[b].menu[j] = new string[d.readByte()];
                for (int k = 0; k < Npc.arrNpcTemplate[b].menu[j].Length; k++)
                {
                    Npc.arrNpcTemplate[b].menu[j][k] = d.readUTF();
                }
            }
        }
        Mob.arrMobTemplate = new MobTemplate[d.readShort()];
        for (int l = 0; l < Mob.arrMobTemplate.Length; l++)
        {
            Mob.arrMobTemplate[l] = new MobTemplate();
            Mob.arrMobTemplate[l].mobTemplateId = l;
            Mob.arrMobTemplate[l].type = d.readByte();
            Mob.arrMobTemplate[l].name = d.readUTF();
            Mob.arrMobTemplate[l].hp = d.readLong();
            Mob.arrMobTemplate[l].rangeMove = d.readByte();
            Mob.arrMobTemplate[l].speed = d.readByte();
            Mob.arrMobTemplate[l].dartType = d.readByte();
        }
    }

    private void createData(myReader d, bool isSaveRMS)
    {
        GameScr.vcData = d.readByte();
        if (isSaveRMS)
        {
            Rms.saveRMS("NR_dart", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_arrow", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_effect", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_image", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_part", NinjaUtil.readByteArray(d));
            Rms.saveRMS("NR_skill", NinjaUtil.readByteArray(d));
            Rms.DeleteStorage("NRdata");
        }
    }

    private Image createImage(sbyte[] arr)
    {
        try
        {
            return Image.createImage(arr, 0, arr.Length);
        }
        catch (Exception)
        {
        }
        return null;
    }

    public int[] arrayByte2Int(sbyte[] b)
    {
        int[] array = new int[b.Length];
        for (int i = 0; i < b.Length; i++)
        {
            int num = b[i];
            if (num < 0)
            {
                num += 256;
            }
            array[i] = num;
        }
        return array;
    }

    public void readClanMsg(Message msg, int index)
    {
        try
        {
            ClanMessage clanMessage = new ClanMessage();
            sbyte b = (sbyte)(clanMessage.type = msg.reader().readByte());
            clanMessage.id = msg.reader().readInt();
            clanMessage.playerId = msg.reader().readInt();
            clanMessage.playerName = msg.reader().readUTF();
            clanMessage.role = msg.reader().readByte();
            clanMessage.time = msg.reader().readInt() + 1000000000;
            bool flag = false;
            GameScr.isNewClanMessage = false;
            switch (b)
            {
                case 0:
                    {
                        string text = msg.reader().readUTF();
                        GameScr.isNewClanMessage = true;
                        if (mFont.tahoma_7.getWidth(text) > Panel.WIDTH_PANEL - 60)
                        {
                            clanMessage.chat = mFont.tahoma_7.splitFontArray(text, Panel.WIDTH_PANEL - 10);
                        }
                        else
                        {
                            clanMessage.chat = new string[1];
                            clanMessage.chat[0] = text;
                        }
                        clanMessage.color = msg.reader().readByte();
                        break;
                    }
                case 1:
                    clanMessage.recieve = msg.reader().readByte();
                    clanMessage.maxCap = msg.reader().readByte();
                    flag = msg.reader().readByte() == 1;
                    if (flag)
                    {
                        GameScr.isNewClanMessage = true;
                    }
                    if (clanMessage.playerId != Char.myCharz().charID)
                    {
                        if (clanMessage.recieve < clanMessage.maxCap)
                        {
                            clanMessage.option = new string[1] { mResources.donate };
                        }
                        else
                        {
                            clanMessage.option = null;
                        }
                    }
                    if (GameCanvas.panel.cp != null)
                    {
                        GameCanvas.panel.updateRequest(clanMessage.recieve, clanMessage.maxCap);
                    }
                    break;
                case 2:
                    if (Char.myCharz().role == 0)
                    {
                        GameScr.isNewClanMessage = true;
                        clanMessage.option = new string[2]
                        {
                        mResources.CANCEL,
                        mResources.receive
                        };
                    }
                    break;
            }
            if (GameCanvas.currentScreen != GameScr.instance)
            {
                GameScr.isNewClanMessage = false;
            }
            else if (GameCanvas.panel.isShow && GameCanvas.panel.type == 0 && GameCanvas.panel.currentTabIndex == 3)
            {
                GameScr.isNewClanMessage = false;
            }
            ClanMessage.addMessage(clanMessage, index, flag);
        }
        catch (Exception)
        {
            Cout.println("LOI TAI CMD -= " + msg.command);
        }
    }

    public void loadCurrMap(sbyte teleport3)
    {
        Res.outz("[CONTROLER] start load map " + teleport3);
        GameScr.gI().auto = 0;
        GameScr.isChangeZone = false;
        CreateCharScr.instance = null;
        GameScr.info1.isUpdate = false;
        GameScr.info2.isUpdate = false;
        GameScr.lockTick = 0;
        GameCanvas.panel.isShow = false;
        DataItem.IsShow = false;
        MainMenu.ToggleMenu(show: false);
        SoundMn.gI().stopAll();
        if (!GameScr.isLoadAllData && !CreateCharScr.isCreateChar)
        {
            GameScr.gI().initSelectChar();
        }
        GameScr.loadCamera(fullmScreen: false, (teleport3 != 1) ? (-1) : Char.myCharz().cx, (teleport3 == 0) ? (-1) : 0);
        TileMap.loadMainTile();
        TileMap.loadMap(TileMap.tileID);
        Res.outz("LOAD GAMESCR 2");
        Char.myCharz().cvx = 0;
        Char.myCharz().statusMe = 4;
        Char.myCharz().currentMovePoint = null;
        Char.myCharz().mobFocus = null;
        Char.myCharz().charFocus = null;
        Char.myCharz().npcFocus = null;
        Char.myCharz().itemFocus = null;
        Char.myCharz().skillPaint = null;
        Char.myCharz().setMabuHold(m: false);
        Char.myCharz().skillPaintRandomPaint = null;
        GameCanvas.clearAllPointerEvent();
        if (Char.myCharz().cy >= TileMap.pxh - 100)
        {
            Char.myCharz().isFlyUp = true;
            Char.myCharz().cx += Res.abs(Res.random(0, 80));
            Service.gI().charMove();
        }
        GameScr.gI().loadGameScr();
        GameCanvas.loadBG(TileMap.bgID);
        Char.isLockKey = false;
        Res.outz("cy= " + Char.myCharz().cy + "---------------------------------------------");
        for (int i = 0; i < Char.myCharz().vEff.size(); i++)
        {
            EffectChar effectChar = (EffectChar)Char.myCharz().vEff.elementAt(i);
            if (effectChar.template.type == 10)
            {
                Char.isLockKey = true;
                break;
            }
        }
        GameCanvas.clearKeyHold();
        GameCanvas.clearKeyPressed();
        GameScr.gI().dHP = Char.myCharz().cHP;
        GameScr.gI().dMP = Char.myCharz().cMP;
        Char.ischangingMap = false;
        GameScr.gI().switchToMe();
        if (Char.myCharz().cy <= 10 && teleport3 != 0 && teleport3 != 2)
        {
            Teleport p = new Teleport(Char.myCharz().cx, Char.myCharz().cy, Char.myCharz().head, Char.myCharz().cdir, 1, isMe: true, (teleport3 != 1) ? teleport3 : Char.myCharz().cgender);
            Teleport.addTeleport(p);
            Char.myCharz().isTeleport = true;
        }
        if (teleport3 == 2)
        {
            Char.myCharz().show();
        }
        if (GameScr.gI().isRongThanXuatHien)
        {
            if (TileMap.mapID == GameScr.gI().mapRID && TileMap.zoneID == GameScr.gI().zoneRID)
            {
                GameScr.gI().callRongThan(GameScr.gI().xR, GameScr.gI().yR);
            }
            if (mGraphics.zoomLevel > 1)
            {
                GameScr.gI().doiMauTroi();
            }
        }
        InfoDlg.hide();
        InfoDlg.show(TileMap.mapName, mResources.zone + " " + TileMap.zoneID, 30);
        GameCanvas.endDlg();
        GameCanvas.isLoading = false;
        Hint.clickMob();
        Hint.clickNpc();
        GameCanvas.debug("SA75x9", 2);
        GameCanvas.isRequestMapID = 2;
        GameCanvas.waitingTimeChangeMap = mSystem.currentTimeMillis() + 1000;
        Res.outz("[CONTROLLER] loadMap DONE!!!!!!!!!");
    }

    public void loadInfoMap(Message msg)
    {
        try
        {
            if (mGraphics.zoomLevel == 1)
            {
                SmallImage.clearHastable();
            }
            Char.myCharz().cx = (Char.myCharz().cxSend = (Char.myCharz().cxFocus = msg.reader().readShort()));
            Char.myCharz().cy = (Char.myCharz().cySend = (Char.myCharz().cyFocus = msg.reader().readShort()));
            Char.myCharz().xSd = Char.myCharz().cx;
            Char.myCharz().ySd = Char.myCharz().cy;
            Res.outz("head= " + Char.myCharz().head + " body= " + Char.myCharz().body + " left= " + Char.myCharz().leg + " x= " + Char.myCharz().cx + " y= " + Char.myCharz().cy + " chung toc= " + Char.myCharz().cgender);
            if (Char.myCharz().cx >= 0 && Char.myCharz().cx <= 100)
            {
                Char.myCharz().cdir = 1;
            }
            else if (Char.myCharz().cx >= TileMap.tmw - 100 && Char.myCharz().cx <= TileMap.tmw)
            {
                Char.myCharz().cdir = -1;
            }
            GameCanvas.debug("SA75x4", 2);
            int num = msg.reader().readByte();
            Res.outz("vGo size= " + num);
            if (!GameScr.info1.isDone)
            {
                GameScr.info1.cmx = Char.myCharz().cx - GameScr.cmx;
                GameScr.info1.cmy = Char.myCharz().cy - GameScr.cmy;
            }
            for (int i = 0; i < num; i++)
            {
                Waypoint waypoint = new Waypoint(msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readUTF());
                if ((TileMap.mapID != 21 && TileMap.mapID != 22 && TileMap.mapID != 23) || waypoint.minX < 0 || waypoint.minX <= 24)
                {
                }
            }
            Resources.UnloadUnusedAssets();
            GC.Collect();
            GameCanvas.debug("SA75x5", 2);
            num = msg.reader().readByte();
            Mob.newMob.removeAllElements();
            for (sbyte b = 0; b < num; b++)
            {
                Mob mob = new Mob(b, msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readBoolean(), msg.reader().readShort(), msg.reader().readByte(), msg.reader().readLong(), msg.reader().readByte(), msg.reader().readLong(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readByte(), msg.reader().readByte());
                mob.xSd = mob.x;
                mob.ySd = mob.y;
                mob.isBoss = msg.reader().readBoolean();
                if (Mob.arrMobTemplate[mob.templateId].type != 0)
                {
                    if (b % 3 == 0)
                    {
                        mob.dir = -1;
                    }
                    else
                    {
                        mob.dir = 1;
                    }
                    mob.x += 10 - b % 20;
                }
                mob.isMobMe = false;
                BigBoss bigBoss = null;
                BachTuoc bachTuoc = null;
                BigBoss2 bigBoss2 = null;
                NewBoss newBoss = null;
                if (mob.templateId == 70)
                {
                    bigBoss = new BigBoss(b, (short)mob.x, (short)mob.y, 70, mob.hp, mob.maxHp, mob.sys);
                }
                if (mob.templateId == 71)
                {
                    bachTuoc = new BachTuoc(b, (short)mob.x, (short)mob.y, 71, mob.hp, mob.maxHp, mob.sys);
                }
                if (mob.templateId == 72)
                {
                    bigBoss2 = new BigBoss2(b, (short)mob.x, (short)mob.y, 72, mob.hp, mob.maxHp, 3);
                }
                if (mob.isBoss)
                {
                    newBoss = new NewBoss(b, (short)mob.x, (short)mob.y, mob.templateId, mob.hp, mob.maxHp, mob.sys);
                }
                if (newBoss != null)
                {
                    GameScr.vMob.addElement(newBoss);
                }
                else if (bigBoss != null)
                {
                    GameScr.vMob.addElement(bigBoss);
                }
                else if (bachTuoc != null)
                {
                    GameScr.vMob.addElement(bachTuoc);
                }
                else if (bigBoss2 != null)
                {
                    GameScr.vMob.addElement(bigBoss2);
                }
                else
                {
                    GameScr.vMob.addElement(mob);
                }
            }
            if (Char.myCharz().mobMe != null && GameScr.findMobInMap(Char.myCharz().mobMe.mobId) == null)
            {
                Char.myCharz().mobMe.getData();
                Char.myCharz().mobMe.x = Char.myCharz().cx;
                Char.myCharz().mobMe.y = Char.myCharz().cy - 40;
                GameScr.vMob.addElement(Char.myCharz().mobMe);
            }
            num = msg.reader().readByte();
            for (byte b2 = 0; b2 < num; b2++)
            {
            }
            GameCanvas.debug("SA75x6", 2);
            num = msg.reader().readByte();
            Res.outz("NPC size= " + num);
            for (int j = 0; j < num; j++)
            {
                sbyte status = msg.reader().readByte();
                short cx = msg.reader().readShort();
                short num2 = msg.reader().readShort();
                sbyte b3 = msg.reader().readByte();
                short num3 = msg.reader().readShort();
                if (b3 != 6 && ((Char.myCharz().taskMaint.taskId >= 7 && (Char.myCharz().taskMaint.taskId != 7 || Char.myCharz().taskMaint.index > 1)) || (b3 != 7 && b3 != 8 && b3 != 9)) && (Char.myCharz().taskMaint.taskId >= 6 || b3 != 16))
                {
                    if (b3 == 4)
                    {
                        GameScr.gI().magicTree = new MagicTree(j, status, cx, num2, b3, num3);
                        Service.gI().magicTree(2);
                        GameScr.vNpc.addElement(GameScr.gI().magicTree);
                    }
                    else
                    {
                        Npc o = new Npc(j, status, cx, num2 + 3, b3, num3);
                        GameScr.vNpc.addElement(o);
                    }
                }
            }
            GameCanvas.debug("SA75x7", 2);
            num = msg.reader().readByte();
            string empty = string.Empty;
            Res.outz("item size = " + num);
            empty = empty + "item: " + num;
            for (int k = 0; k < num; k++)
            {
                short itemMapID = msg.reader().readShort();
                short itemTemplateID = msg.reader().readShort();
                int x = msg.reader().readShort();
                int y = msg.reader().readShort();
                int num4 = msg.reader().readInt();
                short r = 0;
                if (num4 == -2)
                {
                    r = msg.reader().readShort();
                }
                ItemMap itemMap = new ItemMap(num4, itemMapID, itemTemplateID, x, y, r);
                bool flag = false;
                for (int l = 0; l < GameScr.vItemMap.size(); l++)
                {
                    ItemMap itemMap2 = (ItemMap)GameScr.vItemMap.elementAt(l);
                    if (itemMap2.itemMapID == itemMap.itemMapID)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    GameScr.vItemMap.addElement(itemMap);
                }
                empty = empty + itemTemplateID + ",";
            }
            Res.err("sl item on map " + empty + "\n");
            TileMap.vCurrItem.removeAllElements();
            if (mGraphics.zoomLevel == 1)
            {
                BgItem.clearHashTable();
            }
            BgItem.vKeysNew.removeAllElements();
            if (!GameCanvas.lowGraphic || (GameCanvas.lowGraphic && TileMap.isVoDaiMap()) || TileMap.mapID == 45 || TileMap.mapID == 46 || TileMap.mapID == 47 || TileMap.mapID == 48 || TileMap.mapID == 120 || TileMap.mapID == 128 || TileMap.mapID == 170 || TileMap.mapID == 49)
            {
                short num5 = msg.reader().readShort();
                empty = "item high graphic: ";
                for (int m = 0; m < num5; m++)
                {
                    short id = msg.reader().readShort();
                    short num6 = msg.reader().readShort();
                    short num7 = msg.reader().readShort();
                    if (TileMap.getBIById(id) != null)
                    {
                        BgItem bIById = TileMap.getBIById(id);
                        BgItem bgItem = new BgItem();
                        bgItem.id = id;
                        bgItem.idImage = bIById.idImage;
                        bgItem.dx = bIById.dx;
                        bgItem.dy = bIById.dy;
                        bgItem.x = num6 * TileMap.size;
                        bgItem.y = num7 * TileMap.size;
                        bgItem.layer = bIById.layer;
                        if (TileMap.isExistMoreOne(bgItem.id))
                        {
                            bgItem.trans = ((m % 2 != 0) ? 2 : 0);
                            if (TileMap.mapID == 45)
                            {
                                bgItem.trans = 0;
                            }
                        }
                        Image image = null;
                        if (!BgItem.imgNew.containsKey(bgItem.idImage + string.Empty))
                        {
                            if (mGraphics.zoomLevel == 1)
                            {
                                image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
                                if (image == null)
                                {
                                    image = Image.createRGBImage(new int[1], 1, 1, bl: true);
                                    Service.gI().getBgTemplate(bgItem.idImage);
                                }
                                BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
                            }
                            else
                            {
                                bool flag2 = false;
                                sbyte[] array = Rms.loadRMS(mGraphics.zoomLevel + "bgItem" + bgItem.idImage);
                                if (array != null)
                                {
                                    if (BgItem.newSmallVersion != null)
                                    {
                                        Res.outz("Small  last= " + array.Length % 127 + "new Version= " + BgItem.newSmallVersion[bgItem.idImage]);
                                        if (array.Length % 127 != BgItem.newSmallVersion[bgItem.idImage])
                                        {
                                            flag2 = true;
                                        }
                                    }
                                    if (!flag2)
                                    {
                                        image = Image.createImage(array, 0, array.Length);
                                        if (image != null)
                                        {
                                            BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
                                        }
                                        else
                                        {
                                            flag2 = true;
                                        }
                                    }
                                }
                                else
                                {
                                    flag2 = true;
                                }
                                if (flag2)
                                {
                                    image = GameCanvas.loadImage("/mapBackGround/" + bgItem.idImage + ".png");
                                    if (image == null)
                                    {
                                        image = Image.createRGBImage(new int[1], 1, 1, bl: true);
                                        Service.gI().getBgTemplate(bgItem.idImage);
                                    }
                                    BgItem.imgNew.put(bgItem.idImage + string.Empty, image);
                                }
                            }
                            BgItem.vKeysLast.addElement(bgItem.idImage + string.Empty);
                        }
                        if (!BgItem.isExistKeyNews(bgItem.idImage + string.Empty))
                        {
                            BgItem.vKeysNew.addElement(bgItem.idImage + string.Empty);
                        }
                        bgItem.changeColor();
                        TileMap.vCurrItem.addElement(bgItem);
                    }
                    empty = empty + id + ",";
                }
                Res.err("item High Graphics: " + empty);
                for (int n = 0; n < BgItem.vKeysLast.size(); n++)
                {
                    string text = (string)BgItem.vKeysLast.elementAt(n);
                    if (!BgItem.isExistKeyNews(text))
                    {
                        BgItem.imgNew.remove(text);
                        if (BgItem.imgNew.containsKey(text + "blend" + 1))
                        {
                            BgItem.imgNew.remove(text + "blend" + 1);
                        }
                        if (BgItem.imgNew.containsKey(text + "blend" + 3))
                        {
                            BgItem.imgNew.remove(text + "blend" + 3);
                        }
                        BgItem.vKeysLast.removeElementAt(n);
                        n--;
                    }
                }
                BackgroudEffect.isFog = false;
                BackgroudEffect.nCloud = 0;
                EffecMn.vEff.removeAllElements();
                BackgroudEffect.vBgEffect.removeAllElements();
                Effect.newEff.removeAllElements();
                short num8 = msg.reader().readShort();
                for (int num9 = 0; num9 < num8; num9++)
                {
                    string key = msg.reader().readUTF();
                    string value = msg.reader().readUTF();
                    keyValueAction(key, value);
                }
            }
            else
            {
                short num10 = msg.reader().readShort();
                for (int num11 = 0; num11 < num10; num11++)
                {
                    short num12 = msg.reader().readShort();
                    short num13 = msg.reader().readShort();
                    short num14 = msg.reader().readShort();
                }
                short num15 = msg.reader().readShort();
                for (int num16 = 0; num16 < num15; num16++)
                {
                    string text2 = msg.reader().readUTF();
                    string text3 = msg.reader().readUTF();
                }
            }
            TileMap.bgType = msg.reader().readByte();
            sbyte teleport = msg.reader().readByte();
            Res.err(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> LoadmapMap tele:" + teleport.ToString());
            loadCurrMap(teleport);
            GameCanvas.debug("SA75x8", 2);
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        catch (Exception)
        {
            Res.err(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Loadmap khong thanh cong");
            GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
            ServerListScreen.waitToLogin = true;
            GameCanvas.endDlg();
        }
        GameCanvas.isLoading = false;
        Res.err(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Loadmap thanh cong");
    }

    public void keyValueAction(string key, string value)
    {
        if (key.Equals("eff"))
        {
            if (Panel.graphics > 0)
            {
                return;
            }
            string[] array = Res.split(value, ".", 0);
            int id = int.Parse(array[0]);
            int layer = int.Parse(array[1]);
            int x = int.Parse(array[2]);
            int y = int.Parse(array[3]);
            int loop;
            int loopCount;
            if (array.Length <= 4)
            {
                loop = -1;
                loopCount = 1;
            }
            else
            {
                loop = int.Parse(array[4]);
                loopCount = int.Parse(array[5]);
            }
            Effect effect = new Effect(id, x, y, layer, loop, loopCount);
            if (array.Length > 6)
            {
                effect.typeEff = int.Parse(array[6]);
                if (array.Length > 7)
                {
                    effect.indexFrom = int.Parse(array[7]);
                    effect.indexTo = int.Parse(array[8]);
                }
            }
            EffecMn.addEff(effect);
        }
        else if (key.Equals("beff") && Panel.graphics <= 1)
        {
            BackgroudEffect.addEffect(int.Parse(value));
        }
    }

    public void messageNotMap(Message msg)
    {
        GameCanvas.debug("SA6", 2);
        try
        {
            sbyte b = msg.reader().readByte();
            Res.outz("---messageNotMap : " + b);
            switch (b)
            {
                case 16:
                    MoneyCharge.gI().switchToMe();
                    break;
                case 17:
                    GameCanvas.debug("SYB123", 2);
                    Char.myCharz().clearTask();
                    break;
                case 18:
                    {
                        GameCanvas.isLoading = false;
                        GameCanvas.endDlg();
                        int num2 = msg.reader().readInt();
                        GameCanvas.inputDlg.show(mResources.changeNameChar, new Command(mResources.OK, GameCanvas.instance, 88829, num2), TField.INPUT_TYPE_ANY);
                        break;
                    }
                case 20:
                    Char.myCharz().cPk = msg.reader().readByte();
                    GameScr.info1.addInfo(mResources.PK_NOW + " " + Char.myCharz().cPk);
                    break;
                case 35:
                    GameCanvas.endDlg();
                    GameScr.gI().resetButton();
                    GameScr.info1.addInfo(msg.reader().readUTF());
                    break;
                case 36:
                    GameScr.typeActive = msg.reader().readByte();
                    Res.outz("load Me Active: " + GameScr.typeActive);
                    break;
                case 4:
                    {
                        GameCanvas.debug("SA8", 2);
                        GameCanvas.loginScr.savePass();
                        GameScr.isAutoPlay = false;
                        GameScr.canAutoPlay = false;
                        LoginScr.isUpdateAll = true;
                        LoginScr.isUpdateData = true;
                        LoginScr.isUpdateMap = true;
                        LoginScr.isUpdateSkill = true;
                        LoginScr.isUpdateItem = true;
                        GameScr.vsData = msg.reader().readByte();
                        GameScr.vsMap = msg.reader().readByte();
                        GameScr.vsSkill = msg.reader().readByte();
                        GameScr.vsItem = msg.reader().readByte();
                        sbyte b2 = msg.reader().readByte();
                        if (GameCanvas.loginScr.isLogin2)
                        {
                            Rms.saveRMSString("acc", string.Empty);
                            Rms.saveRMSString("pass", string.Empty);
                        }
                        else
                        {
                            Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, string.Empty);
                        }
                        if (GameScr.vsData != GameScr.vcData)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateData();
                        }
                        else
                        {
                            try
                            {
                                LoginScr.isUpdateData = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcData = -1;
                                Service.gI().updateData();
                            }
                        }
                        if (GameScr.vsMap != GameScr.vcMap)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateMap();
                        }
                        else
                        {
                            try
                            {
                                if (!GameScr.isLoadAllData)
                                {
                                    DataInputStream dataInputStream = new DataInputStream(Rms.loadRMS("NRmap"));
                                    createMap(dataInputStream.r);
                                }
                                LoginScr.isUpdateMap = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcMap = -1;
                                Service.gI().updateMap();
                            }
                        }
                        if (GameScr.vsSkill != GameScr.vcSkill)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateSkill();
                        }
                        else
                        {
                            try
                            {
                                if (!GameScr.isLoadAllData)
                                {
                                    DataInputStream dataInputStream2 = new DataInputStream(Rms.loadRMS("NRskill"));
                                    createSkill(dataInputStream2.r);
                                }
                                LoginScr.isUpdateSkill = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcSkill = -1;
                                Service.gI().updateSkill();
                            }
                        }
                        if (GameScr.vsItem != GameScr.vcItem)
                        {
                            GameScr.isLoadAllData = false;
                            Service.gI().updateItem();
                        }
                        else
                        {
                            try
                            {
                                DataInputStream dataInputStream3 = new DataInputStream(Rms.loadRMS("NRitem0"));
                                loadItemNew(dataInputStream3.r, 0, isSave: false);
                                DataInputStream dataInputStream4 = new DataInputStream(Rms.loadRMS("NRitem1"));
                                loadItemNew(dataInputStream4.r, 1, isSave: false);
                                DataInputStream dataInputStream5 = new DataInputStream(Rms.loadRMS("NRitem100"));
                                loadItemNew(dataInputStream5.r, 100, isSave: false);
                                LoginScr.isUpdateItem = false;
                            }
                            catch (Exception)
                            {
                                GameScr.vcItem = -1;
                                Service.gI().updateItem();
                            }
                            try
                            {
                                DataInputStream dataInputStream6 = new DataInputStream(Rms.loadRMS("NRitem101"));
                                loadItemNew(dataInputStream6.r, 101, isSave: false);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (!GameScr.isLoadAllData)
                        {
                            GameScr.gI().readOk();
                        }
                        else
                        {
                            Service.gI().clientOk();
                        }
                        sbyte b3 = msg.reader().readByte();
                        Res.outz("CAPTION LENT= " + b3);
                        GameScr.exps = new long[b3];
                        for (int j = 0; j < GameScr.exps.Length; j++)
                        {
                            GameScr.exps[j] = msg.reader().readLong();
                        }
                        break;
                    }
                case 6:
                    {
                        Res.outz("GET UPDATE_MAP " + msg.reader().available() + " bytes");
                        msg.reader().mark(500000);
                        createMap(msg.reader());
                        msg.reader().reset();
                        sbyte[] data3 = new sbyte[msg.reader().available()];
                        msg.reader().readFully(ref data3);
                        Rms.saveRMS("NRmap", data3);
                        sbyte[] data4 = new sbyte[1] { GameScr.vcMap };
                        Rms.saveRMS("NRmapVersion", data4);
                        LoginScr.isUpdateMap = false;
                        GameScr.gI().readOk();
                        break;
                    }
                case 7:
                    {
                        Res.outz("GET UPDATE_SKILL " + msg.reader().available() + " bytes");
                        msg.reader().mark(500000);
                        createSkill(msg.reader());
                        msg.reader().reset();
                        sbyte[] data = new sbyte[msg.reader().available()];
                        msg.reader().readFully(ref data);
                        Rms.saveRMS("NRskill", data);
                        sbyte[] data2 = new sbyte[1] { GameScr.vcSkill };
                        Rms.saveRMS("NRskillVersion", data2);
                        LoginScr.isUpdateSkill = false;
                        GameScr.gI().readOk();
                        break;
                    }
                case 8:
                    Res.outz("GET UPDATE_ITEM " + msg.reader().available() + " bytes");
                    createItemNew(msg.reader());
                    break;
                case 10:
                    try
                    {
                        Char.isLoadingMap = true;
                        Res.outz("REQUEST MAP TEMPLATE");
                        GameCanvas.isLoading = true;
                        TileMap.maps = null;
                        TileMap.types = null;
                        mSystem.gcc();
                        GameCanvas.debug("SA99", 2);
                        TileMap.tmw = msg.reader().readByte();
                        TileMap.tmh = msg.reader().readByte();
                        TileMap.maps = new int[TileMap.tmw * TileMap.tmh];
                        Res.err("   M apsize= " + TileMap.tmw * TileMap.tmh);
                        for (int i = 0; i < TileMap.maps.Length; i++)
                        {
                            int num = msg.reader().readByte();
                            if (num < 0)
                            {
                                num += 256;
                            }
                            TileMap.maps[i] = (ushort)num;
                        }
                        TileMap.types = new int[TileMap.maps.Length];
                        msg = messWait;
                        loadInfoMap(msg);
                        try
                        {
                            TileMap.isMapDouble = ((msg.reader().readByte() != 0) ? true : false);
                        }
                        catch (Exception ex)
                        {
                            Res.err(" 1 LOI TAI CASE REQUEST_MAPTEMPLATE " + ex.ToString());
                        }
                    }
                    catch (Exception ex2)
                    {
                        Res.err("2 LOI TAI CASE REQUEST_MAPTEMPLATE " + ex2.ToString());
                    }
                    msg.cleanup();
                    messWait.cleanup();
                    msg = (messWait = null);
                    GameScr.gI().switchToMe();
                    break;
                case 9:
                    GameCanvas.debug("SA11", 2);
                    break;
            }
        }
        catch (Exception ex8)
        {
            Cout.LogError("LOI TAI messageNotMap=== " + msg.command + "  >>" + ex8.StackTrace);
        }
        finally
        {
            msg?.cleanup();
        }
    }

    public void messageNotLogin(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            Res.outz("---messageNotLogin : " + b);
            if (b == 2)
            {
                string linkDefault = msg.reader().readUTF();
                Res.outz(">>Get CLIENT_INFO");
                ServerListScreen.linkDefault = linkDefault;
                mSystem.AddIpTest();
                ServerListScreen.getServerList(ServerListScreen.linkDefault);
                try
                {
                    sbyte b2 = msg.reader().readByte();
                    Panel.CanNapTien = b2 == 1;
                }
                catch (Exception)
                {
                }
                isGet_CLIENT_INFO = true;
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            msg?.cleanup();
        }
    }

    public void messageSubCommand(Message msg)
    {
        try
        {
            GameCanvas.debug("SA12", 2);
            sbyte b = msg.reader().readByte();
            Res.outz("---messageSubCommand : " + b);
            switch (b)
            {
                case 63:
                    {
                        sbyte b4 = msg.reader().readByte();
                        if (b4 > 0)
                        {
                            GameCanvas.panel.vPlayerMenu_id.removeAllElements();
                            InfoDlg.showWait();
                            MyVector vPlayerMenu = GameCanvas.panel.vPlayerMenu;
                            for (int i = 0; i < b4; i++)
                            {
                                string caption = msg.reader().readUTF();
                                string caption2 = msg.reader().readUTF();
                                short menuSelect = msg.reader().readShort();
                                GameCanvas.panel.vPlayerMenu_id.addElement(menuSelect + string.Empty);
                                Char.myCharz().charFocus.menuSelect = menuSelect;
                                Command command = new Command(caption, 11115, Char.myCharz().charFocus);
                                command.caption2 = caption2;
                                vPlayerMenu.addElement(command);
                            }
                            InfoDlg.hide();
                            GameCanvas.panel.setTabPlayerMenu();
                        }
                        break;
                    }
                case 1:
                    GameCanvas.debug("SA13", 2);
                    Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
                    Char.myCharz().cTiemNang = msg.reader().readLong();
                    Char.myCharz().vSkill.removeAllElements();
                    Char.myCharz().vSkillFight.removeAllElements();
                    Char.myCharz().myskill = null;
                    break;
                case 2:
                    {
                        GameCanvas.debug("SA14", 2);
                        if (Char.myCharz().statusMe != 14 && Char.myCharz().statusMe != 5)
                        {
                            Char.myCharz().cHP = Char.myCharz().cHPFull;
                            Char.myCharz().cMP = Char.myCharz().cMPFull;
                            Cout.LogError2(" ME_LOAD_SKILL");
                        }
                        Char.myCharz().vSkill.removeAllElements();
                        Char.myCharz().vSkillFight.removeAllElements();
                        sbyte b2 = msg.reader().readByte();
                        for (sbyte b3 = 0; b3 < b2; b3++)
                        {
                            short skillId = msg.reader().readShort();
                            Skill skill2 = Skills.get(skillId);
                            useSkill(skill2);
                        }
                        GameScr.gI().sortSkill();
                        if (GameScr.isPaintInfoMe)
                        {
                            GameScr.indexRow = -1;
                            GameScr.gI().left = (GameScr.gI().center = null);
                        }
                        break;
                    }
                case 19:
                    GameCanvas.debug("SA17", 2);
                    Char.myCharz().boxSort();
                    break;
                case 21:
                    {
                        GameCanvas.debug("SA19", 2);
                        int num4 = msg.reader().readInt();
                        Char.myCharz().xuInBox -= num4;
                        Char.myCharz().xu += num4;
                        Char.myCharz().xuStr = mSystem.numberTostring(Char.myCharz().xu);
                        break;
                    }
                case 0:
                    {
                        GameCanvas.debug("SA21", 2);
                        RadarScr.list = new MyVector();
                        Teleport.vTeleport.removeAllElements();
                        GameScr.vCharInMap.removeAllElements();
                        GameScr.vItemMap.removeAllElements();
                        Char.vItemTime.removeAllElements();
                        GameScr.loadImg();
                        GameScr.currentCharViewInfo = Char.myCharz();
                        Char.myCharz().charID = msg.reader().readInt();
                        Char.myCharz().ctaskId = msg.reader().readByte();
                        Char.myCharz().cgender = msg.reader().readByte();
                        Char.myCharz().head = msg.reader().readShort();
                        Char.myCharz().cName = msg.reader().readUTF();
                        Char.myCharz().cPk = msg.reader().readByte();
                        Char.myCharz().cTypePk = msg.reader().readByte();
                        Char.myCharz().cPower = msg.reader().readLong();
                        Char.myCharz().applyCharLevelPercent();
                        Char.myCharz().eff5BuffHp = msg.reader().readShort();
                        Char.myCharz().eff5BuffMp = msg.reader().readShort();
                        Char.myCharz().nClass = GameScr.nClasss[msg.reader().readByte()];
                        Char.myCharz().vSkill.removeAllElements();
                        Char.myCharz().vSkillFight.removeAllElements();
                        GameScr.gI().dHP = Char.myCharz().cHP;
                        GameScr.gI().dMP = Char.myCharz().cMP;
                        sbyte b6 = msg.reader().readByte();
                        for (sbyte b7 = 0; b7 < b6; b7++)
                        {
                            Skill skill3 = Skills.get(msg.reader().readShort());
                            useSkill(skill3);
                        }
                        GameScr.gI().sortSkill();
                        GameScr.gI().loadSkillShortcut();
                        Char.myCharz().xu = msg.reader().readLong();
                        Char.myCharz().luongKhoa = msg.reader().readInt();
                        Char.myCharz().luong = msg.reader().readInt();
                        Char.myCharz().xuStr = Res.formatNumber(Char.myCharz().xu);
                        Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                        Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                        Char.myCharz().arrItemBody = new Item[msg.reader().readByte()];
                        try
                        {
                            Char.myCharz().setDefaultPart();
                            for (int k = 0; k < Char.myCharz().arrItemBody.Length; k++)
                            {
                                short num5 = msg.reader().readShort();
                                if (num5 == -1)
                                {
                                    continue;
                                }
                                ItemTemplate itemTemplate = ItemTemplates.get(num5);
                                int type = itemTemplate.type;
                                Char.myCharz().arrItemBody[k] = new Item();
                                Char.myCharz().arrItemBody[k].template = itemTemplate;
                                Char.myCharz().arrItemBody[k].quantity = msg.reader().readInt();
                                Char.myCharz().arrItemBody[k].info = msg.reader().readUTF();
                                Char.myCharz().arrItemBody[k].content = msg.reader().readUTF();
                                int num6 = msg.reader().readUnsignedByte();
                                if (num6 != 0)
                                {
                                    Char.myCharz().arrItemBody[k].itemOption = new ItemOption[num6];
                                    for (int l = 0; l < Char.myCharz().arrItemBody[k].itemOption.Length; l++)
                                    {
                                        ItemOption itemOption = readItemOption(msg);
                                        if (itemOption != null)
                                        {
                                            Char.myCharz().arrItemBody[k].itemOption[l] = itemOption;
                                        }
                                    }
                                }
                                switch (type)
                                {
                                    case 0:
                                        Res.outz("toi day =======================================" + Char.myCharz().body);
                                        Char.myCharz().body = Char.myCharz().arrItemBody[k].template.part;
                                        break;
                                    case 1:
                                        Char.myCharz().leg = Char.myCharz().arrItemBody[k].template.part;
                                        Res.outz("toi day =======================================" + Char.myCharz().leg);
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Char.myCharz().arrItemBag = new Item[msg.reader().readByte()];
                        GameScr.hpPotion = 0;
                        GameScr.isudungCapsun4 = false;
                        GameScr.isudungCapsun3 = false;
                        for (int m = 0; m < Char.myCharz().arrItemBag.Length; m++)
                        {
                            short num7 = msg.reader().readShort();
                            if (num7 == -1)
                            {
                                continue;
                            }
                            Char.myCharz().arrItemBag[m] = new Item();
                            Char.myCharz().arrItemBag[m].template = ItemTemplates.get(num7);
                            Char.myCharz().arrItemBag[m].quantity = msg.reader().readInt();
                            Char.myCharz().arrItemBag[m].info = msg.reader().readUTF();
                            Char.myCharz().arrItemBag[m].content = msg.reader().readUTF();
                            Char.myCharz().arrItemBag[m].indexUI = m;
                            sbyte b8 = msg.reader().readByte();
                            if (b8 != 0)
                            {
                                Char.myCharz().arrItemBag[m].itemOption = new ItemOption[b8];
                                for (int n = 0; n < Char.myCharz().arrItemBag[m].itemOption.Length; n++)
                                {
                                    ItemOption itemOption2 = readItemOption(msg);
                                    if (itemOption2 != null)
                                    {
                                        Char.myCharz().arrItemBag[m].itemOption[n] = itemOption2;
                                        Char.myCharz().arrItemBag[m].getCompare();
                                    }
                                }
                            }
                            if (Char.myCharz().arrItemBag[m].template.type == 6)
                            {
                                GameScr.hpPotion += Char.myCharz().arrItemBag[m].quantity;
                            }
                            switch (num7)
                            {
                                case 194:
                                    GameScr.isudungCapsun4 = Char.myCharz().arrItemBag[m].quantity > 0;
                                    break;
                                case 193:
                                    if (!GameScr.isudungCapsun4)
                                    {
                                        GameScr.isudungCapsun3 = Char.myCharz().arrItemBag[m].quantity > 0;
                                    }
                                    break;
                            }
                        }
                        Char.myCharz().arrItemBox = new Item[msg.reader().readByte()];
                        GameCanvas.panel.hasUse = 0;
                        for (int num8 = 0; num8 < Char.myCharz().arrItemBox.Length; num8++)
                        {
                            short num9 = msg.reader().readShort();
                            if (num9 == -1)
                            {
                                continue;
                            }
                            Char.myCharz().arrItemBox[num8] = new Item();
                            Char.myCharz().arrItemBox[num8].template = ItemTemplates.get(num9);
                            Char.myCharz().arrItemBox[num8].quantity = msg.reader().readInt();
                            Char.myCharz().arrItemBox[num8].info = msg.reader().readUTF();
                            Char.myCharz().arrItemBox[num8].content = msg.reader().readUTF();
                            Char.myCharz().arrItemBox[num8].itemOption = new ItemOption[msg.reader().readByte()];
                            for (int num10 = 0; num10 < Char.myCharz().arrItemBox[num8].itemOption.Length; num10++)
                            {
                                ItemOption itemOption3 = readItemOption(msg);
                                if (itemOption3 != null)
                                {
                                    Char.myCharz().arrItemBox[num8].itemOption[num10] = itemOption3;
                                    Char.myCharz().arrItemBox[num8].getCompare();
                                }
                            }
                            GameCanvas.panel.hasUse++;
                        }
                        Char.myCharz().statusMe = 4;
                        int num11 = Rms.loadRMSInt(Char.myCharz().cName + "vci");
                        if (num11 < 1)
                        {
                            GameScr.isViewClanInvite = false;
                        }
                        else
                        {
                            GameScr.isViewClanInvite = true;
                        }
                        short num12 = msg.reader().readShort();
                        Char.idHead = new short[num12];
                        Char.idAvatar = new short[num12];
                        for (int num13 = 0; num13 < num12; num13++)
                        {
                            Char.idHead[num13] = msg.reader().readShort();
                            Char.idAvatar[num13] = msg.reader().readShort();
                        }
                        for (int num14 = 0; num14 < GameScr.info1.charId.Length; num14++)
                        {
                            GameScr.info1.charId[num14] = new int[3];
                        }
                        GameScr.info1.charId[Char.myCharz().cgender][0] = msg.reader().readShort();
                        GameScr.info1.charId[Char.myCharz().cgender][1] = msg.reader().readShort();
                        GameScr.info1.charId[Char.myCharz().cgender][2] = msg.reader().readShort();
                        Char.myCharz().isNhapThe = msg.reader().readByte() == 1;
                        Res.outz("NHAP THE= " + Char.myCharz().isNhapThe);
                        GameScr.deltaTime = mSystem.currentTimeMillis() - (long)msg.reader().readInt() * 1000L;
                        GameScr.isNewMember = msg.reader().readByte();
                        Service.gI().updateCaption((sbyte)Char.myCharz().cgender);
                        Service.gI().androidPack();
                        try
                        {
                            Char.myCharz().idAuraEff = msg.reader().readShort();
                            Char.myCharz().idEff_Set_Item = msg.reader().readSByte();
                            Char.myCharz().idHat = msg.reader().readShort();
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                case 4:
                    GameCanvas.debug("SA23", 2);
                    Char.myCharz().xu = msg.reader().readLong();
                    Char.myCharz().luong = msg.reader().readInt();
                    Char.myCharz().cHP = msg.reader().readLong();
                    Char.myCharz().cMP = msg.reader().readLong();
                    Char.myCharz().luongKhoa = msg.reader().readInt();
                    Char.myCharz().xuStr = Res.formatNumber2(Char.myCharz().xu);
                    Char.myCharz().luongStr = mSystem.numberTostring(Char.myCharz().luong);
                    Char.myCharz().luongKhoaStr = mSystem.numberTostring(Char.myCharz().luongKhoa);
                    break;
                case 5:
                    {
                        GameCanvas.debug("SA24", 2);
                        long cHP = Char.myCharz().cHP;
                        Char.myCharz().cHP = msg.reader().readLong();
                        if (Char.myCharz().cHP > cHP && Char.myCharz().cTypePk != 4)
                        {
                            GameScr.startFlyText("+" + (Char.myCharz().cHP - cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
                            SoundMn.gI().HP_MPup();
                            if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5003)
                            {
                                MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1L, -1L, Char.myCharz(), 29);
                            }
                        }
                        if (Char.myCharz().cHP < cHP)
                        {
                            GameScr.startFlyText("-" + (cHP - Char.myCharz().cHP) + " " + mResources.HP, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 20, 0, -1, mFont.HP);
                        }
                        GameScr.gI().dHP = Char.myCharz().cHP;
                        if (GameScr.isPaintInfoMe)
                        {
                        }
                        break;
                    }
                case 6:
                    {
                        GameCanvas.debug("SA25", 2);
                        if (Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
                        {
                            break;
                        }
                        long cMP = Char.myCharz().cMP;
                        Char.myCharz().cMP = msg.reader().readLong();
                        if (Char.myCharz().cMP > cMP)
                        {
                            GameScr.startFlyText("+" + (Char.myCharz().cMP - cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
                            SoundMn.gI().HP_MPup();
                            if (Char.myCharz().petFollow != null && Char.myCharz().petFollow.smallID == 5001)
                            {
                                MonsterDart.addMonsterDart(Char.myCharz().petFollow.cmx + ((Char.myCharz().petFollow.dir != 1) ? (-10) : 10), Char.myCharz().petFollow.cmy + 10, isBoss: true, -1L, -1L, Char.myCharz(), 29);
                            }
                        }
                        if (Char.myCharz().cMP < cMP)
                        {
                            GameScr.startFlyText("-" + (cMP - Char.myCharz().cMP) + " " + mResources.KI, Char.myCharz().cx, Char.myCharz().cy - Char.myCharz().ch - 23, 0, -2, mFont.MP);
                        }
                        Res.outz("curr MP= " + Char.myCharz().cMP);
                        GameScr.gI().dMP = Char.myCharz().cMP;
                        if (GameScr.isPaintInfoMe)
                        {
                        }
                        break;
                    }
                case 7:
                    {
                        Char obj9 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj9 == null)
                        {
                            break;
                        }
                        obj9.clanID = msg.reader().readInt();
                        if (obj9.clanID == -2)
                        {
                            obj9.isCopy = true;
                        }
                        readCharInfo(obj9, msg);
                        try
                        {
                            obj9.idAuraEff = msg.reader().readShort();
                            obj9.idEff_Set_Item = msg.reader().readSByte();
                            obj9.idHat = msg.reader().readShort();
                            if (obj9.bag >= 201)
                            {
                                Effect effect = new Effect(obj9.bag, obj9, 2, -1, 10, 1);
                                effect.typeEff = 5;
                                obj9.addEffChar(effect);
                            }
                            else
                            {
                                obj9.removeEffChar(0, 201);
                            }
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                case 8:
                    {
                        GameCanvas.debug("SA26", 2);
                        Char obj10 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj10 != null)
                        {
                            obj10.cspeed = msg.reader().readByte();
                        }
                        break;
                    }
                case 9:
                    {
                        GameCanvas.debug("SA27", 2);
                        Char obj8 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj8 != null)
                        {
                            obj8.cHP = msg.reader().readLong();
                            obj8.cHPFull = msg.reader().readLong();
                        }
                        break;
                    }
                case 10:
                    {
                        GameCanvas.debug("SA28", 2);
                        Char obj5 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj5 != null)
                        {
                            obj5.cHP = msg.reader().readLong();
                            obj5.cHPFull = msg.reader().readLong();
                            obj5.eff5BuffHp = msg.reader().readShort();
                            obj5.eff5BuffMp = msg.reader().readShort();
                            obj5.wp = msg.reader().readShort();
                            if (obj5.wp == -1)
                            {
                                obj5.setDefaultWeapon();
                            }
                        }
                        break;
                    }
                case 11:
                    {
                        GameCanvas.debug("SA29", 2);
                        Char obj2 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj2 != null)
                        {
                            obj2.cHP = msg.reader().readLong();
                            obj2.cHPFull = msg.reader().readLong();
                            obj2.eff5BuffHp = msg.reader().readShort();
                            obj2.eff5BuffMp = msg.reader().readShort();
                            obj2.body = msg.reader().readShort();
                            if (obj2.body == -1)
                            {
                                obj2.setDefaultBody();
                            }
                        }
                        break;
                    }
                case 12:
                    {
                        GameCanvas.debug("SA30", 2);
                        Char obj11 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj11 != null)
                        {
                            obj11.cHP = msg.reader().readLong();
                            obj11.cHPFull = msg.reader().readLong();
                            obj11.eff5BuffHp = msg.reader().readShort();
                            obj11.eff5BuffMp = msg.reader().readShort();
                            obj11.leg = msg.reader().readShort();
                            if (obj11.leg == -1)
                            {
                                obj11.setDefaultLeg();
                            }
                        }
                        break;
                    }
                case 13:
                    {
                        GameCanvas.debug("SA31", 2);
                        int num2 = msg.reader().readInt();
                        Char obj = ((num2 != Char.myCharz().charID) ? GameScr.findCharInMap(num2) : Char.myCharz());
                        if (obj != null)
                        {
                            obj.cHP = msg.reader().readLong();
                            obj.cHPFull = msg.reader().readLong();
                            obj.eff5BuffHp = msg.reader().readShort();
                            obj.eff5BuffMp = msg.reader().readShort();
                        }
                        break;
                    }
                case 14:
                    {
                        GameCanvas.debug("SA32", 2);
                        Char obj4 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj4 == null)
                        {
                            break;
                        }
                        obj4.cHP = msg.reader().readLong();
                        sbyte b5 = msg.reader().readByte();
                        Res.outz("player load hp type= " + b5);
                        if (b5 == 1)
                        {
                            ServerEffect.addServerEffect(11, obj4, 5);
                            ServerEffect.addServerEffect(104, obj4, 4);
                        }
                        if (b5 == 2)
                        {
                            obj4.doInjure();
                        }
                        try
                        {
                            obj4.cHPFull = msg.reader().readLong();
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                case 15:
                    {
                        GameCanvas.debug("SA33", 2);
                        Char obj3 = GameScr.findCharInMap(msg.reader().readInt());
                        if (obj3 != null)
                        {
                            obj3.cHP = msg.reader().readLong();
                            obj3.cHPFull = msg.reader().readLong();
                            obj3.cx = msg.reader().readShort();
                            obj3.cy = msg.reader().readShort();
                            obj3.statusMe = 1;
                            obj3.cp3 = 3;
                            ServerEffect.addServerEffect(109, obj3, 2);
                        }
                        break;
                    }
                case 35:
                    {
                        GameCanvas.debug("SY3", 2);
                        int num3 = msg.reader().readInt();
                        Res.outz("CID = " + num3);
                        if (TileMap.mapID == 130)
                        {
                            GameScr.gI().starVS();
                        }
                        if (num3 == Char.myCharz().charID)
                        {
                            Char.myCharz().cTypePk = msg.reader().readByte();
                            if (GameScr.gI().isVS() && Char.myCharz().cTypePk != 0)
                            {
                                GameScr.gI().starVS();
                            }
                            Res.outz("type pk= " + Char.myCharz().cTypePk);
                            Char.myCharz().npcFocus = null;
                            if (!GameScr.gI().isMeCanAttackMob(Char.myCharz().mobFocus))
                            {
                                Char.myCharz().mobFocus = null;
                            }
                            Char.myCharz().itemFocus = null;
                        }
                        else
                        {
                            Char obj6 = GameScr.findCharInMap(num3);
                            if (obj6 != null)
                            {
                                Res.outz("type pk= " + obj6.cTypePk);
                                obj6.cTypePk = msg.reader().readByte();
                                if (obj6.isAttacPlayerStatus())
                                {
                                    Char.myCharz().charFocus = obj6;
                                }
                            }
                        }
                        for (int j = 0; j < GameScr.vCharInMap.size(); j++)
                        {
                            Char obj7 = GameScr.findCharInMap(j);
                            if (obj7 != null && obj7.cTypePk != 0 && obj7.cTypePk == Char.myCharz().cTypePk)
                            {
                                if (!Char.myCharz().mobFocus.isMobMe)
                                {
                                    Char.myCharz().mobFocus = null;
                                }
                                Char.myCharz().npcFocus = null;
                                Char.myCharz().itemFocus = null;
                                break;
                            }
                        }
                        Res.outz("update type pk= ");
                        break;
                    }
                case 61:
                    {
                        string text = msg.reader().readUTF();
                        sbyte[] data = new sbyte[msg.reader().readInt()];
                        msg.reader().read(ref data);
                        if (data.Length == 0)
                        {
                            data = null;
                        }
                        if (text.Equals("KSkill"))
                        {
                            GameScr.gI().onKSkill(data);
                        }
                        else if (text.Equals("OSkill"))
                        {
                            GameScr.gI().onOSkill(data);
                        }
                        else if (text.Equals("CSkill"))
                        {
                            GameScr.gI().onCSkill(data);
                        }
                        break;
                    }
                case 23:
                    {
                        short num = msg.reader().readShort();
                        Skill skill = Skills.get(num);
                        useSkill(skill);
                        if (num != 0 && num != 14 && num != 28)
                        {
                            GameScr.info1.addInfo(mResources.LEARN_SKILL + " " + skill.template.name);
                        }
                        break;
                    }
                case 62:
                    Res.outz("ME UPDATE SKILL");
                    read_UpdateSkill(msg);
                    break;
            }
        }
        catch (Exception ex5)
        {
            Cout.println("Loi tai Sub : " + ex5.ToString());
        }
        finally
        {
            msg?.cleanup();
        }
    }

    private void useSkill(Skill skill)
    {
        if (Char.myCharz().myskill == null)
        {
            Char.myCharz().myskill = skill;
        }
        else if (skill.template.Equals(Char.myCharz().myskill.template))
        {
            Char.myCharz().myskill = skill;
        }
        Char.myCharz().vSkill.addElement(skill);
        if ((skill.template.type == 1 || skill.template.type == 4 || skill.template.type == 2 || skill.template.type == 3) && (skill.template.maxPoint == 0 || (skill.template.maxPoint > 0 && skill.point > 0)))
        {
            if (skill.template.id == Char.myCharz().skillTemplateId)
            {
                Service.gI().selectSkill(Char.myCharz().skillTemplateId);
            }
            Char.myCharz().vSkillFight.addElement(skill);
        }
    }

    public bool readCharInfo(Char c, Message msg)
    {
        try
        {
            c.clevel = msg.reader().readByte();
            c.isInvisiblez = msg.reader().readBoolean();
            c.cTypePk = msg.reader().readByte();
            Res.outz("ADD TYPEAK PK= " + c.cTypePk + " to player " + c.charID + " @@ " + c.cName);
            c.nClass = GameScr.nClasss[msg.reader().readByte()];
            c.cgender = msg.reader().readByte();
            c.head = msg.reader().readShort();
            c.cName = msg.reader().readUTF();
            c.cHP = msg.reader().readLong();
            c.dHP = c.cHP;
            if (c.cHP == 0)
            {
                c.statusMe = 14;
            }
            c.cHPFull = msg.reader().readLong();
            if (c.cy >= TileMap.pxh - 100)
            {
                c.isFlyUp = true;
            }
            c.body = msg.reader().readShort();
            c.leg = msg.reader().readShort();
            c.bag = msg.reader().readShort();
            Res.outz(" body= " + c.body + " leg= " + c.leg + " bag=" + c.bag + "BAG ==" + c.bag + "*********************************");
            c.isShadown = true;
            sbyte b = msg.reader().readByte();
            if (c.wp == -1)
            {
                c.setDefaultWeapon();
            }
            if (c.body == -1)
            {
                c.setDefaultBody();
            }
            if (c.leg == -1)
            {
                c.setDefaultLeg();
            }
            c.cx = msg.reader().readShort();
            c.cy = msg.reader().readShort();
            c.xSd = c.cx;
            c.ySd = c.cy;
            c.eff5BuffHp = msg.reader().readShort();
            c.eff5BuffMp = msg.reader().readShort();
            int num = msg.reader().readByte();
            for (int i = 0; i < num; i++)
            {
                EffectChar effectChar = new EffectChar(msg.reader().readByte(), msg.reader().readInt(), msg.reader().readInt(), msg.reader().readShort());
                c.vEff.addElement(effectChar);
                if (effectChar.template.type == 12 || effectChar.template.type == 11)
                {
                    c.isInvisiblez = true;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            ex.StackTrace.ToString();
        }
        return false;
    }

    private void readGetImgByName(Message msg)
    {
        try
        {
            string name = msg.reader().readUTF();
            sbyte nFrame = msg.reader().readByte();
            sbyte[] array = null;
            array = NinjaUtil.readByteArray(msg);
            Image img = createImage(array);
            ImgByName.SetImage(name, img, nFrame);
            if (array == null)
            {
            }
        }
        catch (Exception)
        {
        }
    }

    private void createItemNew(myReader d)
    {
        try
        {
            loadItemNew(d, -1, isSave: true);
        }
        catch (Exception)
        {
        }
    }

    private void loadItemNew(myReader d, sbyte type, bool isSave)
    {
        try
        {
            d.mark(1000000);
            GameScr.vcItem = d.readByte();
            type = d.readByte();
            Res.err(GameScr.vcItem + ":<<GameScr.vcItem >>>>>>loadItemNew: " + type + "  isSave:" + isSave);
            switch (type)
            {
                case 0:
                    {
                        GameScr.gI().iOptionTemplates = new ItemOptionTemplate[d.readShort()];
                        for (int j = 0; j < GameScr.gI().iOptionTemplates.Length; j++)
                        {
                            GameScr.gI().iOptionTemplates[j] = new ItemOptionTemplate();
                            GameScr.gI().iOptionTemplates[j].id = j;
                            GameScr.gI().iOptionTemplates[j].name = d.readUTF();
                            GameScr.gI().iOptionTemplates[j].type = d.readByte();
                        }
                        if (isSave)
                        {
                            d.reset();
                            sbyte[] data3 = new sbyte[d.available()];
                            d.readFully(ref data3);
                            Rms.saveRMS("NRitem0", data3);
                        }
                        break;
                    }
                case 1:
                    {
                        ItemTemplates.itemTemplates.clear();
                        int num3 = d.readShort();
                        for (int k = 0; k < num3; k++)
                        {
                            ItemTemplate it = new ItemTemplate((short)k, d.readByte(), d.readByte(), d.readUTF(), d.readUTF(), d.readByte(), d.readInt(), d.readShort(), d.readShort(), d.readBoolean());
                            ItemTemplates.add(it);
                        }
                        if (isSave)
                        {
                            d.reset();
                            sbyte[] data4 = new sbyte[d.available()];
                            d.readFully(ref data4);
                            Rms.saveRMS("NRitem1", data4);
                            sbyte[] data5 = new sbyte[1] { GameScr.vcItem };
                            Rms.saveRMS("NRitemVersion", data5);
                        }
                        LoginScr.isUpdateItem = false;
                        GameScr.gI().readOk();
                        break;
                    }
                case 100:
                    Char.Arr_Head_2Fr = readArrHead(d);
                    if (isSave)
                    {
                        d.reset();
                        sbyte[] data2 = new sbyte[d.available()];
                        d.readFully(ref data2);
                        Rms.saveRMS("NRitem100", data2);
                    }
                    break;
                case 101:
                    try
                    {
                        int num = d.readShort();
                        Char.Arr_Head_FlyMove = new short[num];
                        for (int i = 0; i < num; i++)
                        {
                            short num2 = d.readShort();
                            Char.Arr_Head_FlyMove[i] = num2;
                        }
                        if (isSave)
                        {
                            d.reset();
                            sbyte[] data = new sbyte[d.available()];
                            d.readFully(ref data);
                            Rms.saveRMS("NRitem101", data);
                        }
                        break;
                    }
                    catch (Exception)
                    {
                        Char.Arr_Head_FlyMove = new short[0];
                        break;
                    }
            }
        }
        catch (Exception ex2)
        {
            ex2.ToString();
        }
    }

    private void readFrameBoss(Message msg, int mobTemplateId)
    {
        try
        {
            int num = msg.reader().readByte();
            int[][] array = new int[num][];
            for (int i = 0; i < num; i++)
            {
                int num2 = msg.reader().readByte();
                array[i] = new int[num2];
                for (int j = 0; j < num2; j++)
                {
                    array[i][j] = msg.reader().readByte();
                }
            }
            frameHT_NEWBOSS.put(mobTemplateId + string.Empty, array);
        }
        catch (Exception)
        {
        }
    }

    private int[][] readArrHead(myReader d)
    {
        int[][] array = new int[1][] { new int[2] { 542, 543 } };
        try
        {
            int num = d.readShort();
            array = new int[num][];
            for (int i = 0; i < array.Length; i++)
            {
                int num2 = d.readByte();
                array[i] = new int[num2];
                for (int j = 0; j < num2; j++)
                {
                    array[i][j] = d.readShort();
                }
            }
        }
        catch (Exception)
        {
        }
        return array;
    }

    public void phuban_Info(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            if (b == 0)
            {
                readPhuBan_CHIENTRUONGNAMEK(msg, b);
            }
        }
        catch (Exception)
        {
        }
    }

    private void readPhuBan_CHIENTRUONGNAMEK(Message msg, int type_PB)
    {
        try
        {
            switch (msg.reader().readByte())
            {
                case 0:
                    {
                        short idmapPaint = msg.reader().readShort();
                        string nameTeam = msg.reader().readUTF();
                        string nameTeam2 = msg.reader().readUTF();
                        int maxPoint = msg.reader().readInt();
                        short timeSecond = msg.reader().readShort();
                        int maxLife = msg.reader().readByte();
                        GameScr.phuban_Info = new InfoPhuBan(type_PB, idmapPaint, nameTeam, nameTeam2, maxPoint, timeSecond);
                        GameScr.phuban_Info.maxLife = maxLife;
                        GameScr.phuban_Info.updateLife(type_PB, 0, 0);
                        break;
                    }
                case 1:
                    {
                        int pointTeam = msg.reader().readInt();
                        int pointTeam2 = msg.reader().readInt();
                        if (GameScr.phuban_Info != null)
                        {
                            GameScr.phuban_Info.updatePoint(type_PB, pointTeam, pointTeam2);
                        }
                        break;
                    }
                case 2:
                    {
                        sbyte b = msg.reader().readByte();
                        short type = 0;
                        short num = -1;
                        switch (b)
                        {
                            case 1:
                                type = 1;
                                num = 3;
                                break;
                            case 2:
                                type = 2;
                                break;
                        }
                        num = -1;
                        GameScr.phuban_Info = null;
                        GameScr.addEffectEnd(type, num, 0, GameCanvas.hw, GameCanvas.hh, 0, 0, -1, null);
                        break;
                    }
                case 5:
                    {
                        short timeSecond2 = msg.reader().readShort();
                        if (GameScr.phuban_Info != null)
                        {
                            GameScr.phuban_Info.updateTime(type_PB, timeSecond2);
                        }
                        break;
                    }
                case 4:
                    {
                        int lifeTeam = msg.reader().readByte();
                        int lifeTeam2 = msg.reader().readByte();
                        if (GameScr.phuban_Info != null)
                        {
                            GameScr.phuban_Info.updateLife(type_PB, lifeTeam, lifeTeam2);
                        }
                        break;
                    }
                case 3:
                    break;
            }
        }
        catch (Exception)
        {
        }
    }

    public void read_cmdExtra(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            mSystem.println(">>---read_cmdExtra-sub:" + b);
            switch (b)
            {
                case 0:
                    {
                        short idHat = msg.reader().readShort();
                        Char.myCharz().idHat = idHat;
                        SoundMn.gI().getStrOption();
                        break;
                    }
                case 2:
                    {
                        int num3 = msg.reader().readInt();
                        sbyte b5 = msg.reader().readByte();
                        short num4 = msg.reader().readShort();
                        string v = num4 + "," + b5;
                        MainImage imagePath = ImgByName.getImagePath("banner_" + num4, ImgByName.hashImagePath);
                        GameCanvas.danhHieu.put(num3 + string.Empty, v);
                        break;
                    }
                case 3:
                    {
                        short num2 = msg.reader().readShort();
                        SmallImage.createImage(num2);
                        BackgroudEffect.id_water1 = num2;
                        break;
                    }
                case 4:
                    {
                        string o = msg.reader().readUTF();
                        GameCanvas.messageServer.addElement(o);
                        break;
                    }
                case 5:
                    {
                        string text = "------------------|ChienTruong|Log: ";
                        text = "\n|ChienTruong|Log: ";
                        sbyte b2 = msg.reader().readByte();
                        switch (b2)
                        {
                            case 0:
                                {
                                    GameScr.nCT_team = msg.reader().readUTF();
                                    GameScr.nCT_TeamA = (GameScr.nCT_TeamB = msg.reader().readByte());
                                    GameScr.nCT_nBoyBaller = GameScr.nCT_TeamA * 2;
                                    GameScr.isPaint_CT = false;
                                    string text4 = text;
                                    text = text4 + "\tsub    0|  nCT_team= " + GameScr.nCT_team + "|nCT_TeamA =" + GameScr.nCT_TeamA + "  isPaint_CT=false \n";
                                    break;
                                }
                            case 1:
                                {
                                    int num = msg.reader().readInt();
                                    sbyte b4 = (GameScr.nCT_floor = msg.reader().readByte());
                                    GameScr.nCT_timeBallte = num * 1000 + mSystem.currentTimeMillis();
                                    GameScr.isPaint_CT = true;
                                    string text3 = text;
                                    text = text3 + "\tsub    1 floor= " + b4 + "|timeBallte= " + num + "isPaint_CT=true \n";
                                    break;
                                }
                            case 2:
                                {
                                    GameScr.nCT_TeamA = msg.reader().readByte();
                                    GameScr.nCT_TeamB = msg.reader().readByte();
                                    GameScr.res_CT.removeAllElements();
                                    sbyte b3 = msg.reader().readByte();
                                    for (int i = 0; i < b3; i++)
                                    {
                                        string empty = string.Empty;
                                        empty = empty + msg.reader().readByte() + "|";
                                        empty = empty + msg.reader().readUTF() + "|";
                                        empty = empty + msg.reader().readShort() + "|";
                                        empty += msg.reader().readInt();
                                        GameScr.res_CT.addElement(empty);
                                    }
                                    string text2 = text;
                                    text = text2 + "\tsub   2|  A= " + GameScr.nCT_TeamA + "|B =" + GameScr.nCT_TeamB + "  isPaint_CT=true \n";
                                    break;
                                }
                            case 3:
                                Service.gI().sendCT_ready(b, b2);
                                GameScr.nCT_floor = 0;
                                GameScr.nCT_timeBallte = 0L;
                                GameScr.isPaint_CT = false;
                                text += "\tsub    3|  isPaint_CT=false \n";
                                break;
                            case 4:
                                GameScr.nUSER_CT = msg.reader().readByte();
                                GameScr.nUSER_MAX_CT = msg.reader().readByte();
                                break;
                        }
                        text += "END LOG CT.";
                        Res.err(text);
                        break;
                    }
                default:
                    readExtra(b, msg);
                    break;
            }
        }
        catch (Exception)
        {
        }
    }

    public void read_UpdateSkill(Message msg)
    {
        try
        {
            short num = msg.reader().readShort();
            sbyte b = -1;
            try
            {
                b = msg.reader().readSByte();
            }
            catch (Exception)
            {
            }
            switch (b)
            {
                case 0:
                    {
                        short curExp = msg.reader().readShort();
                        for (int m = 0; m < Char.myCharz().vSkill.size(); m++)
                        {
                            Skill skill4 = (Skill)Char.myCharz().vSkill.elementAt(m);
                            if (skill4.skillId == num)
                            {
                                skill4.curExp = curExp;
                                break;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        sbyte b2 = msg.reader().readByte();
                        for (int n = 0; n < Char.myCharz().vSkill.size(); n++)
                        {
                            Skill skill5 = (Skill)Char.myCharz().vSkill.elementAt(n);
                            if (skill5.skillId == num)
                            {
                                for (int num2 = 0; num2 < 20; num2++)
                                {
                                    string nameImg = "Skills_" + skill5.template.id + "_" + b2 + "_" + num2;
                                    MainImage imagePath = ImgByName.getImagePath(nameImg, ImgByName.hashImagePath);
                                }
                                break;
                            }
                        }
                        break;
                    }
                case -1:
                    {
                        Skill skill = Skills.get(num);
                        for (int i = 0; i < Char.myCharz().vSkill.size(); i++)
                        {
                            Skill skill2 = (Skill)Char.myCharz().vSkill.elementAt(i);
                            if (skill2.template.id == skill.template.id)
                            {
                                Char.myCharz().vSkill.setElementAt(skill, i);
                                break;
                            }
                        }
                        for (int j = 0; j < Char.myCharz().vSkillFight.size(); j++)
                        {
                            Skill skill3 = (Skill)Char.myCharz().vSkillFight.elementAt(j);
                            if (skill3.template.id == skill.template.id)
                            {
                                Char.myCharz().vSkillFight.setElementAt(skill, j);
                                break;
                            }
                        }
                        for (int k = 0; k < GameScr.onScreenSkill.Length; k++)
                        {
                            if (GameScr.onScreenSkill[k] != null && GameScr.onScreenSkill[k].template.id == skill.template.id)
                            {
                                GameScr.onScreenSkill[k] = skill;
                                break;
                            }
                        }
                        for (int l = 0; l < GameScr.keySkill.Length; l++)
                        {
                            if (GameScr.keySkill[l] != null && GameScr.keySkill[l].template.id == skill.template.id)
                            {
                                GameScr.keySkill[l] = skill;
                                break;
                            }
                        }
                        if (Char.myCharz().myskill.template.id == skill.template.id)
                        {
                            Char.myCharz().myskill = skill;
                        }
                        GameScr.info1.addInfo(mResources.hasJustUpgrade1 + skill.template.name + mResources.hasJustUpgrade2 + skill.point);
                        break;
                    }
            }
        }
        catch (Exception)
        {
        }
    }

    public void readExtra(sbyte sub, Message msg)
    {
        try
        {
            if (sub != sbyte.MaxValue)
            {
                return;
            }
            GameCanvas.endDlg();
            try
            {
                string text = (ServerListScreen.linkDefault = msg.reader().readUTF());
                mSystem.AddIpTest();
                ServerListScreen.getServerList(ServerListScreen.linkDefault);
                Res.outz(">>>>read.isEXTRA_LINK " + text);
                sbyte b = msg.reader().readByte();
                if (b > 0)
                {
                    ServerListScreen.typeClass = new sbyte[b];
                    ServerListScreen.listChar = new Char[b];
                    for (int i = 0; i < b; i++)
                    {
                        ServerListScreen.typeClass[i] = msg.reader().readByte();
                        Res.outz(ServerListScreen.nameServer[i] + ">>>>read.isEXTRA_LINK  typeClass: " + ServerListScreen.typeClass[i]);
                        if (ServerListScreen.typeClass[i] > -1)
                        {
                            ServerListScreen.isHaveChar = true;
                            ServerListScreen.listChar[i] = new Char();
                            ServerListScreen.listChar[i].cgender = ServerListScreen.typeClass[i];
                            ServerListScreen.listChar[i].head = msg.reader().readShort();
                            ServerListScreen.listChar[i].body = msg.reader().readShort();
                            ServerListScreen.listChar[i].leg = msg.reader().readShort();
                            ServerListScreen.listChar[i].bag = msg.reader().readShort();
                            ServerListScreen.listChar[i].cName = msg.reader().readUTF();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            isEXTRA_LINK = true;
            ServerListScreen.saveRMS_ExtraLink();
            ServerListScreen.isWait = false;
            Char.isLoadingMap = false;
            LoginScr.isContinueToLogin = false;
            ServerListScreen.waitToLogin = false;
            bool flag = false;
            bool flag2 = false;
            try
            {
                if (!Rms.loadRMSString("acc").Equals(string.Empty))
                {
                    flag = true;
                }
                if (!Rms.loadRMSString("userAo" + ServerListScreen.ipSelect).Equals(string.Empty))
                {
                    flag2 = true;
                }
            }
            catch (Exception)
            {
            }
            if (!ServerListScreen.isHaveChar && !flag && !flag2)
            {
                GameCanvas.serverScreen.Login_New();
                return;
            }
            if (Rms.loadRMSInt(ServerListScreen.RMS_svselect) == -1)
            {
                ServerScr.isShowSv_HaveChar = false;
                GameCanvas.serverScr.switchToMe();
                return;
            }
            ServerListScreen.SetIpSelect(Rms.loadRMSInt(ServerListScreen.RMS_svselect), issave: false);
            if (ServerListScreen.listChar != null && ServerListScreen.listChar[ServerListScreen.ipSelect] != null)
            {
                GameCanvas._SelectCharScr.SetInfoChar(ServerListScreen.listChar[ServerListScreen.ipSelect]);
            }
            else
            {
                GameCanvas.serverScreen.Login_New();
            }
        }
        catch (Exception)
        {
            Res.outz(">>>>read.isEXTRA_LINK  errr:");
            GameCanvas.serverScr.switchToMe();
        }
    }

    public ItemOption readItemOption(Message msg)
    {
        ItemOption result = null;
        try
        {
            int num = msg.reader().readShort();
            int param = msg.reader().readInt();
            if (num != -1)
            {
                result = new ItemOption(num, param);
            }
        }
        catch (Exception)
        {
            Res.err(">>>>read.ItemOption  errr:");
        }
        return result;
    }

    public void read_cmdExtraBig(Message msg)
    {
        try
        {
            sbyte b = msg.reader().readByte();
            mSystem.println(">>---read_cmdExtraBig-sub:" + b);
            if (b == 0)
            {
                loadItemNew(msg.reader(), 1, isSave: true);
            }
        }
        catch (Exception)
        {
        }
    }
}
