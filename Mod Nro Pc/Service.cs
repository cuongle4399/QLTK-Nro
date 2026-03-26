using System;
using Assets.src.g;

public class Service
{
	private ISession session = Session_ME.gI();

	protected static Service instance;

	public static long curCheckController;

	public static long curCheckMap;

	public static long logController;

	public static long logMap;

	public int demGui;

	public static bool reciveFromMainSession;

	public static Service gI()
	{
		if (instance == null)
		{
			instance = new Service();
		}
		return instance;
	}

	public void gotoPlayer(int id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)18);
			message.writer().writeInt(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void androidPack()
	{
		if (mSystem.android_pack == null)
		{
			return;
		}
		Message message = null;
		try
		{
			message = new Message((sbyte)126);
			message.writer().writeUTF(mSystem.android_pack);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void charInfo(string day, string month, string year, string address, string cmnd, string dayCmnd, string noiCapCmnd, string sdt, string name)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)42);
			message.writer().writeUTF(day);
			message.writer().writeUTF(month);
			message.writer().writeUTF(year);
			message.writer().writeUTF(address);
			message.writer().writeUTF(cmnd);
			message.writer().writeUTF(dayCmnd);
			message.writer().writeUTF(noiCapCmnd);
			message.writer().writeUTF(sdt);
			message.writer().writeUTF(name);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void androidPack2()
	{
		if (mSystem.android_pack == null)
		{
			return;
		}
		Message message = null;
		try
		{
			message = new Message((sbyte)126);
			message.writer().writeUTF(mSystem.android_pack);
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void checkAd(sbyte status)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-44));
			message.writer().writeByte(status);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void combine(sbyte action, MyVector id)
	{
		Res.outz("combine");
		Message message = null;
		try
		{
			message = new Message((sbyte)(-81));
			message.writer().writeByte(action);
			if (action == 1)
			{
				message.writer().writeByte(id.size());
				for (int i = 0; i < id.size(); i++)
				{
					message.writer().writeByte(((Item)id.elementAt(i)).indexUI);
					Res.outz("gui id " + ((Item)id.elementAt(i)).indexUI);
				}
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void giaodich(sbyte action, int playerID, sbyte index, int num)
	{
		Res.outz2("giao dich action = " + action);
		Message message = null;
		try
		{
			message = new Message((sbyte)(-86));
			message.writer().writeByte(action);
			if (action == 0 || action == 1)
			{
				Res.outz2(">>>> len playerID =" + playerID);
				message.writer().writeInt(playerID);
			}
			if (action == 2)
			{
				Res.outz2("gui len index =" + index + " num= " + num);
				message.writer().writeByte(index);
				message.writer().writeInt(num);
			}
			if (action == 4)
			{
				Res.outz2(">>>> len index =" + index);
				message.writer().writeByte(index);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendClientInput(TField[] t)
	{
		Message message = null;
		try
		{
			Res.outz(" gui input ");
			message = new Message((sbyte)(-125));
			Res.outz("byte lent = " + t.Length);
			message.writer().writeByte(t.Length);
			for (int i = 0; i < t.Length; i++)
			{
				message.writer().writeUTF(t[i].getText());
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void speacialSkill(sbyte index)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)112);
			message.writer().writeByte(index);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void test(short x, short y)
	{
		Res.outz("gui x= " + x + " y= " + y);
		Message message = null;
		try
		{
			message = new Message(0);
			message.writer().writeShort(x);
			message.writer().writeShort(y);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void test2()
	{
		Res.outz("gui test1");
		Message message = null;
		try
		{
			message = new Message(1);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void testJoint()
	{
	}

	public void mobCapcha(char ch)
	{
		Res.outz("cap char c= " + ch);
		Message message = null;
		try
		{
			message = new Message((sbyte)(-85));
			message.writer().writeChar(ch);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void friend(sbyte action, int playerId)
	{
		Res.outz("add friend");
		Message message = null;
		try
		{
			message = new Message((sbyte)(-80));
			message.writer().writeByte(action);
			if (playerId != -1)
			{
				message.writer().writeInt(playerId);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getArchivemnt(int index)
	{
		Res.outz("get ngoc");
		Message message = null;
		try
		{
			message = new Message((sbyte)(-76));
			message.writer().writeByte(index);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getPlayerMenu(int playerID)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-79));
			message.writer().writeInt(playerID);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clanImage(sbyte id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-62));
			message.writer().writeByte(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void skill_not_focus(sbyte status)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-45));
			message.writer().writeByte(status);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clanDonate(int id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-54));
			message.writer().writeInt(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clanMessage(int type, string text, int clanID)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-51));
			message.writer().writeByte(type);
			if (type == 0)
			{
				message.writer().writeUTF(text);
			}
			if (type == 2)
			{
				message.writer().writeInt(clanID);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void useItem(sbyte type, sbyte where, sbyte index, short template)
	{
		Cout.println("USE ITEM! " + type);
		if (Char.myCharz().statusMe == 14)
		{
			return;
		}
		Message message = null;
		try
		{
			message = new Message((sbyte)(-43));
			message.writer().writeByte(type);
			message.writer().writeByte(where);
			message.writer().writeByte(index);
			if (index == -1)
			{
				message.writer().writeShort(template);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void joinClan(int id, sbyte action)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-49));
			message.writer().writeInt(id);
			message.writer().writeByte(action);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clanMember(int id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-50));
			message.writer().writeInt(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void searchClan(string text)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-47));
			message.writer().writeUTF(text);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestClan(short id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-53));
			message.writer().writeShort(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clanRemote(int id, sbyte role)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-56));
			message.writer().writeInt(id);
			message.writer().writeByte(role);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void leaveClan()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-55));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clanInvite(sbyte action, int playerID, int clanID, int code)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-57));
			message.writer().writeByte(action);
			if (action == 0)
			{
				message.writer().writeInt(playerID);
			}
			if (action == 1 || action == 2)
			{
				message.writer().writeInt(clanID);
				message.writer().writeInt(code);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getClan(sbyte action, int id, string text)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-46));
			message.writer().writeByte(action);
			if (action == 2 || action == 4)
			{
				message.writer().writeShort((short)id);
				message.writer().writeUTF(text);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void updateCaption(sbyte gender)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-41));
			message.writer().writeByte(gender);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getItem(sbyte type, sbyte id)
	{
		Res.outz("get item type= " + type + " id= " + id);
        Message message = null;
		try
		{
			message = new Message((sbyte)(-40));
			message.writer().writeByte(type);
			message.writer().writeByte(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getTask(int npcTemplateId, int menuId, int optionId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)40);
			message.writer().writeByte(npcTemplateId);
			message.writer().writeByte(menuId);
			if (optionId >= 0)
			{
				message.writer().writeByte(optionId);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public Message messageNotLogin(sbyte command)
	{
		Message message = new Message((sbyte)(-29));
		message.writer().writeByte(command);
		return message;
	}

	public Message messageNotMap(sbyte command)
	{
		Message message = new Message((sbyte)(-28));
		message.writer().writeByte(command);
		return message;
	}

	public static Message messageSubCommand(sbyte command)
	{
		Message message = new Message((sbyte)(-30));
		message.writer().writeByte(command);
		return message;
	}

	public void setClientType()
	{
		if (Rms.loadRMSInt("clienttype") != -1)
		{
			Main.typeClient = Rms.loadRMSInt("clienttype");
		}
		try
		{
			Res.outz(">>send ClientType1");
			Message message = messageNotLogin(2);
			message.writer().writeByte(Main.typeClient);
			message.writer().writeByte(mGraphics.zoomLevel);
			message.writer().writeBoolean(value: false);
			message.writer().writeInt(GameCanvas.w);
			message.writer().writeInt(GameCanvas.h);
			message.writer().writeBoolean(TField.isQwerty);
			message.writer().writeBoolean(GameCanvas.isTouch);
			message.writer().writeUTF(GameCanvas.getPlatformName() + "|" + GameMidlet.VERSION);
			DataInputStream dataInputStream = MyStream.readFile("/info");
			if (dataInputStream != null)
			{
				sbyte[] data = new sbyte[dataInputStream.r.buffer.Length];
				dataInputStream.read(ref data);
				if (data != null)
				{
					message.writer().writeShort(data.Length);
					message.writer().write(data);
				}
			}
			session.sendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
	}

	public void setClientType2()
	{
		Res.outz("SET CLIENT TYPEAK");
		if (Rms.loadRMSInt("clienttype") != -1)
		{
			mSystem.clientType = Rms.loadRMSInt("clienttype");
		}
		try
		{
			Res.outz(">>send ClientType2");
			Message message = messageNotLogin(2);
			message.writer().writeByte(mSystem.clientType);
			message.writer().writeByte(mGraphics.zoomLevel);
			Res.outz("gui zoomlevel = " + mGraphics.zoomLevel);
			message.writer().writeBoolean(value: false);
			message.writer().writeInt(GameCanvas.w);
			message.writer().writeInt(GameCanvas.h);
			message.writer().writeBoolean(TField.isQwerty);
			message.writer().writeBoolean(GameCanvas.isTouch);
			message.writer().writeUTF(GameCanvas.getPlatformName() + "|" + GameMidlet.VERSION);
			DataInputStream dataInputStream = MyStream.readFile("/info");
			if (dataInputStream != null)
			{
				sbyte[] data = new sbyte[dataInputStream.r.buffer.Length];
				dataInputStream.read(ref data);
				if (data != null)
				{
					message.writer().writeShort(data.Length);
					message.writer().write(data);
				}
			}
			session = Session_ME2.gI();
			session.sendMessage(message);
			session = Session_ME.gI();
			message.cleanup();
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
	}

	public void sendCheckController()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-120));
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			curCheckController = mSystem.currentTimeMillis();
			message.cleanup();
		}
	}

	public void sendCheckMap()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-121));
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			curCheckMap = mSystem.currentTimeMillis();
			message.cleanup();
		}
	}

	public void login(string username, string pass, string version, sbyte type)
	{
		Res.outz("Login " + username + " " + pass + " " + version);
		try
		{
			Message message = messageNotLogin(0);
			if (!string.IsNullOrEmpty(AutoLoginCL.Account) && !string.IsNullOrEmpty(AutoLoginCL.Password))
			{
				message.writer().writeUTF(AutoLoginCL.Account);
				message.writer().writeUTF(AutoLoginCL.Password);
			}
			else
			{
				message.writer().writeUTF(username);
				message.writer().writeUTF(pass);
			}
			message.writer().writeUTF(version);
			message.writer().writeByte(type);
			session.sendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
	}

	public void requestRegister(string username, string pass, string usernameAo, string passAo, string version)
	{
		try
		{
			Message message = messageNotLogin(1);
			message.writer().writeUTF(username);
			message.writer().writeUTF(pass);
			if (usernameAo != null && !usernameAo.Equals(string.Empty))
			{
				message.writer().writeUTF(usernameAo);
				message.writer().writeUTF("a");
			}
			session.sendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
	}

	public void requestChangeMap()
	{
		Message message = new Message((sbyte)(-23));
		session.sendMessage(message);
		message.cleanup();
	}

	public void magicTree(sbyte type)
	{
		Message message = new Message((sbyte)(-34));
		try
		{
			message.writer().writeByte(type);
			session.sendMessage(message);
			message.cleanup();
		}
		catch (Exception)
		{
		}
	}

	public void requestChangeZone(int zoneId, int indexUI)
	{
		Message message = new Message((sbyte)21);
		try
		{
			message.writer().writeByte(zoneId);
			session.sendMessage(message);
			message.cleanup();
		}
		catch (Exception)
		{
		}
	}

	public void checkMMove(int second)
	{
		Message message = new Message((sbyte)(-78));
		try
		{
			message.writer().writeInt(second);
			session.sendMessage(message);
			message.cleanup();
		}
		catch (Exception)
		{
		}
	}

	public void charMove()
	{
		int num = Char.myCharz().cx - Char.myCharz().cxSend;
		int num2 = Char.myCharz().cy - Char.myCharz().cySend;
		int cx = Char.myCharz().cx;
		int cy = Char.myCharz().cy;
		if (Char.ischangingMap || (num == 0 && num2 == 0) || Controller.isStopReadMessage || Char.myCharz().isTeleport || Char.myCharz().cy <= 0 || Char.myCharz().telePortSkill)
		{
			return;
		}
		try
		{
			Message message = new Message((sbyte)(-7));
			Char.myCharz().cxSend = Char.myCharz().cx;
			Char.myCharz().cySend = Char.myCharz().cy;
			Char.myCharz().cdirSend = Char.myCharz().cdir;
			Char.myCharz().cactFirst = Char.myCharz().statusMe;
			if (TileMap.tileTypeAt(Char.myCharz().cx / TileMap.size, Char.myCharz().cy / TileMap.size) == 0)
			{
				message.writer().writeByte((sbyte)1);
			}
			else
			{
				message.writer().writeByte((sbyte)0);
			}
			message.writer().writeShort(Char.myCharz().cx);
			if (num2 != 0)
			{
				message.writer().writeShort(Char.myCharz().cy);
			}
			session.sendMessage(message);
			GameScr.tickMove++;
			message.cleanup();
		}
		catch (Exception ex)
		{
			Char.myCharz().cxSend = cx;
			Char.myCharz().cySend = cy;
			Char.myCharz().cx = cx;
			Char.myCharz().cy = cy;
			Cout.LogError("LOI CHAR MOVE " + ex.ToString());
		}
	}

	public void selectCharToPlay(string charname)
	{
		Message message = new Message((sbyte)(-28));
		try
		{
			message.writer().writeByte((sbyte)1);
			message.writer().writeUTF(charname);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		session.sendMessage(message);
	}

	public void selectZone(sbyte sub, int value)
	{
	}

	public void createChar(string name, int gender, int hair)
	{
		Message message = new Message((sbyte)(-28));
		try
		{
			message.writer().writeByte((sbyte)2);
			message.writer().writeUTF(name);
			message.writer().writeByte(gender);
			message.writer().writeByte(hair);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		session.sendMessage(message);
	}

	public void requestModTemplate(int modTemplateId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)11);
			message.writer().writeShort(modTemplateId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestSkill(int skillId)
	{
		Message message = null;
		try
		{
			message = messageNotMap(9);
			message.writer().writeShort(skillId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestItemInfo(int typeUI, int indexUI)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)35);
			message.writer().writeByte(typeUI);
			message.writer().writeByte(indexUI);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestItemPlayer(int charId, int indexUI)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)90);
			message.writer().writeInt(charId);
			message.writer().writeByte(indexUI);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void upSkill(int skillTemplateId, int point)
	{
		Message message = null;
		try
		{
			message = messageSubCommand(17);
			message.writer().writeShort(skillTemplateId);
			message.writer().writeByte(point);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void saleItem(sbyte action, sbyte type, short id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)7);
			message.writer().writeByte(action);
			message.writer().writeByte(type);
			message.writer().writeShort(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void buyItem(sbyte type, int id, int quantity)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)6);
			message.writer().writeByte(type);
			message.writer().writeShort(id);
			if (quantity > 1)
			{
				message.writer().writeShort(quantity);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void selectSkill(int skillTemplateId)
	{
		Cout.println(Char.myCharz().cName + " SELECT SKILL " + skillTemplateId);
		Message message = null;
		try
		{
			message = new Message((sbyte)34);
			message.writer().writeShort(skillTemplateId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getEffData(short id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-66));
			message.writer().writeShort(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void openUIZone()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)29);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void confirmMenu(short npcID, sbyte select)
	{
		Res.outz("confirme menu ID: "  + npcID + "index select" + select);
		Message message = null;
		try
		{
			message = new Message((sbyte)32);
			message.writer().writeShort(npcID);
			message.writer().writeByte(select);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void openMenu(int npcId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)33);
			message.writer().writeShort(npcId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void menu(int npcId, int menuId, int optionId)
	{
		Cout.println("menuid: " + menuId);
		Message message = null;
		try
		{
			message = new Message((sbyte)22);
			message.writer().writeByte(npcId);
			message.writer().writeByte(menuId);
			message.writer().writeByte(optionId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void menuId(short menuId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)27);
			message.writer().writeShort(menuId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void textBoxId(short menuId, string str)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)88);
			message.writer().writeShort(menuId);
			message.writer().writeUTF(str);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestItem(int typeUI)
	{
		Message message = null;
		try
		{
			message = messageSubCommand(22);
			message.writer().writeByte(typeUI);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void boxSort()
	{
		Message message = null;
		try
		{
			message = messageSubCommand(19);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void boxCoinOut(int coinOut)
	{
		Message message = null;
		try
		{
			message = messageSubCommand(21);
			message.writer().writeInt(coinOut);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void upgradeItem(Item item, Item[] items, bool isGold)
	{
		GameCanvas.msgdlg.pleasewait();
		Message message = null;
		try
		{
			message = new Message((sbyte)14);
			message.writer().writeBoolean(isGold);
			message.writer().writeByte(item.indexUI);
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] != null)
				{
					message.writer().writeByte(items[i].indexUI);
				}
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void crystalCollectLock(Item[] items)
	{
		GameCanvas.msgdlg.pleasewait();
		Message message = null;
		try
		{
			message = new Message((sbyte)13);
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] != null)
				{
					message.writer().writeByte(items[i].indexUI);
				}
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void acceptInviteTrade(int playerMapId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)37);
			message.writer().writeInt(playerMapId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void cancelInviteTrade()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)50);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void tradeAccept()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)39);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void tradeItemLock(int coin, Item[] items)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)38);
			message.writer().writeInt(coin);
			int num = 0;
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] != null)
				{
					num++;
				}
			}
			message.writer().writeByte(num);
			for (int j = 0; j < items.Length; j++)
			{
				if (items[j] != null)
				{
					message.writer().writeByte(items[j].indexUI);
				}
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendPlayerAttack(MyVector vMob, MyVector vChar, int type)
	{
		try
		{
			Res.outz(">>SEND ATTACT  vMob=" + vMob.size() + "  vChar=" + vChar.size());
			Message message = null;
			if (type == 0)
			{
				return;
			}
			if (vMob.size() > 0 && vChar.size() > 0)
			{
				switch (type)
				{
				case 1:
					message = new Message((sbyte)(-4));
					break;
				case 2:
					message = new Message((sbyte)67);
					break;
				}
				message.writer().writeByte(vMob.size());
				for (int i = 0; i < vMob.size(); i++)
				{
					Mob mob = (Mob)vMob.elementAt(i);
					message.writer().writeByte(mob.mobId);
				}
				for (int j = 0; j < vChar.size(); j++)
				{
					Char obj = (Char)vChar.elementAt(j);
					if (obj != null)
					{
						message.writer().writeInt(obj.charID);
					}
					else
					{
						message.writer().writeInt(-1);
					}
				}
			}
			else if (vMob.size() > 0)
			{
				message = new Message((sbyte)54);
				for (int k = 0; k < vMob.size(); k++)
				{
					Mob mob2 = (Mob)vMob.elementAt(k);
					if (!mob2.isMobMe)
					{
						message.writer().writeByte(mob2.mobId);
						continue;
					}
					message.writer().writeByte((sbyte)(-1));
					message.writer().writeInt(mob2.mobId);
				}
			}
			else if (vChar.size() > 0)
			{
				message = new Message((sbyte)(-60));
				for (int l = 0; l < vChar.size(); l++)
				{
					Char obj2 = (Char)vChar.elementAt(l);
					message.writer().writeInt(obj2.charID);
				}
			}
			message.writer().writeSByte((sbyte)Char.myCharz().cdir);
			if (message != null)
			{
				session.sendMessage(message);
			}
		}
		catch (Exception)
		{
			Res.err(">>err ATTACT  vMob=" + vMob.size() + "  vChar=" + vChar.size());
		}
	}

	public void pickItem(int itemMapId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-20));
			message.writer().writeShort(itemMapId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void throwItem(int index)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-18));
			message.writer().writeByte(index);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void returnTownFromDead()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-15));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void wakeUpFromDead()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-16));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void chat(string text)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)44);
			message.writer().writeUTF(text);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void updateData()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-87));
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void updateMap()
	{
		Message message = null;
		try
		{
			message = messageNotMap(6);
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void updateSkill()
	{
		Message message = null;
		try
		{
			message = messageNotMap(7);
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void updateItem()
	{
		Message message = null;
		try
		{
			message = messageNotMap(8);
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clientOk()
	{
		Message message = null;
		try
		{
			message = messageNotMap(13);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void tradeInvite(int charId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)36);
			message.writer().writeInt(charId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void addFriend(string name)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)53);
			message.writer().writeUTF(name);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void addPartyAccept(int charId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)76);
			message.writer().writeInt(charId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void addPartyCancel(int charId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)77);
			message.writer().writeInt(charId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void testInvite(int charId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)59);
			message.writer().writeInt(charId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void addCuuSat(int charId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)62);
			message.writer().writeInt(charId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void addParty(string name)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)75);
			message.writer().writeUTF(name);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void player_vs_player(sbyte action, sbyte type, int playerId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-59));
			message.writer().writeByte(action);
			message.writer().writeByte(type);
			message.writer().writeInt(playerId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestMaptemplate(int maptemplateId)
	{
		Message message = null;
		try
		{
			message = messageNotMap(10);
			message.writer().writeByte(maptemplateId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void outParty()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)79);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestPlayerInfo(MyVector chars)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)18);
			message.writer().writeByte(chars.size());
			for (int i = 0; i < chars.size(); i++)
			{
				Char obj = (Char)chars.elementAt(i);
				message.writer().writeInt(obj.charID);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void pleaseInputParty(string str)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)16);
			message.writer().writeUTF(str);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void acceptPleaseParty(string str)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)17);
			message.writer().writeUTF(str);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void chatPlayer(string text, int id)
	{
		Res.outz("chat player text = " + text);
		Message message = null;
		try
		{
			message = new Message((sbyte)(-72));
			message.writer().writeInt(id);
			message.writer().writeUTF(text);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void chatGlobal(string text)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-71));
			message.writer().writeUTF(text);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void chatPrivate(string to, string text)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)91);
			message.writer().writeUTF(to);
			message.writer().writeUTF(text);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendCardInfo(string NAP, string PIN)
	{
		Message message = null;
		try
		{
			message = messageNotMap(16);
			message.writer().writeUTF(NAP);
			message.writer().writeUTF(PIN);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void saveRms(string key, sbyte[] data)
	{
		Message message = null;
		try
		{
			message = messageSubCommand(60);
			message.writer().writeUTF(key);
			message.writer().writeInt(data.Length);
			message.writer().write(data);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void loadRMS(string key)
	{
		Cout.println("REQUEST RMS");
		Message message = null;
		try
		{
			message = messageSubCommand(61);
			message.writer().writeUTF(key);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clearTask()
	{
		Message message = null;
		try
		{
			message = messageNotMap(17);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void changeName(string name, int id)
	{
		Message message = null;
		try
		{
			message = messageNotMap(18);
			message.writer().writeInt(id);
			message.writer().writeUTF(name);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestIcon(int id)
	{
		GameCanvas.connect();
		Message message = null;
		try
		{
			message = new Message((sbyte)(-67));
			message.writer().writeInt(id);
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			Res.outz(">>>>>>>>>>>>>REQUEST ICON " + id + "  isConnected:" + Controller.isGet_CLIENT_INFO);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void doConvertUpgrade(int index1, int index2, int index3)
	{
		Message message = null;
		try
		{
			message = messageNotMap(33);
			message.writer().writeByte(index1);
			message.writer().writeByte(index2);
			message.writer().writeByte(index3);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void inviteClanDun(string name)
	{
		Message message = null;
		try
		{
			message = messageNotMap(34);
			message.writer().writeUTF(name);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void inputNumSplit(int indexItem, int numSplit)
	{
		Message message = null;
		try
		{
			message = messageNotMap(40);
			message.writer().writeByte(indexItem);
			message.writer().writeInt(numSplit);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void activeAccProtect(int pass)
	{
		Message message = null;
		try
		{
			message = messageNotMap(37);
			message.writer().writeInt(pass);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void clearAccProtect(int pass)
	{
		Message message = null;
		try
		{
			message = messageNotMap(41);
			message.writer().writeInt(pass);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void updateActive(int passOld, int passNew)
	{
		Message message = null;
		try
		{
			message = messageNotMap(38);
			message.writer().writeInt(passOld);
			message.writer().writeInt(passNew);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void openLockAccProtect(int pass2)
	{
		Message message = null;
		try
		{
			message = messageNotMap(39);
			message.writer().writeInt(pass2);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getBgTemplate(short id)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-32));
			message.writer().writeShort(id);
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getMapOffline()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-33));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void finishUpdate()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-38));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void finishUpdate(int playerID)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-38));
			message.writer().writeInt(playerID);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void finishLoadMap()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-39));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getChest(sbyte action)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-35));
			message.writer().writeByte(action);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestBagImage(int ID)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-63));
			message.writer().writeShort(ID);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getBag(sbyte action)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-36));
			message.writer().writeByte(action);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getBody(sbyte action)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-37));
			message.writer().writeByte(action);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void login2(string user)
	{
		Res.outz("Login 2:  " + user);
		Message message = null;
		try
		{
			message = new Message((sbyte)(-101));
			message.writer().writeUTF(user);
			message.writer().writeByte(1);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getMagicTree(sbyte action)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-34));
			message.writer().writeByte(action);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void upPotential(int typePotential, int num)
	{
		Message message = null;
		try
		{
			message = messageSubCommand(16);
			message.writer().writeByte(typePotential);
			message.writer().writeShort(num);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getResource(sbyte action, MyVector vResourceIndex)
	{
		Res.outz("request resource action= " + action);
		Message message = null;
		try
		{
			message = new Message((sbyte)(-74));
			message.writer().writeByte(action);
			if (action == 2 && vResourceIndex != null)
			{
				message.writer().writeShort(vResourceIndex.size());
				for (int i = 0; i < vResourceIndex.size(); i++)
				{
					message.writer().writeShort(short.Parse((string)vResourceIndex.elementAt(i)));
				}
			}
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				reciveFromMainSession = true;
				session = Session_ME.gI();
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestMapSelect(int selected)
	{
		Res.outz("request magic tree");
		Message message = null;
		try
		{
			message = new Message((sbyte)(-91));
			message.writer().writeByte(selected);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void petInfo()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-107));
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendTop(string topName, sbyte selected)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-96));
			message.writer().writeUTF(topName);
			message.writer().writeByte(selected);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void enemy(sbyte b, int charID)
	{
		Message message = null;
		Res.outz("add enemy");
		try
		{
			message = new Message((sbyte)(-99));
			message.writer().writeByte(b);
			if (b == 1 || b == 2)
			{
				message.writer().writeInt(charID);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void kigui(sbyte action, int itemId, sbyte moneyType, int money, int quaintly)
	{
		Message message = null;
		try
		{
			Res.outz("ki gui action= " + action);
			message = new Message((sbyte)(-100));
			message.writer().writeByte(action);
			if (action == 0)
			{
				message.writer().writeShort(itemId);
				message.writer().writeByte(moneyType);
				message.writer().writeInt(money);
				message.writer().writeInt(quaintly);
			}
			if (action == 1 || action == 2)
			{
				message.writer().writeShort(itemId);
			}
			if (action == 3)
			{
				message.writer().writeShort(itemId);
				message.writer().writeByte(moneyType);
				message.writer().writeInt(money);
			}
			if (action == 4)
			{
				message.writer().writeByte(moneyType);
				message.writer().writeByte(money);
				Res.outz("currTab= " + moneyType + " page= " + money);
			}
			if (action == 5)
			{
				message.writer().writeShort(itemId);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getFlag(sbyte action, sbyte flagType)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-103));
			message.writer().writeByte(action);
			Res.outz("------------service--  " + action + "   " + flagType);
			if (action != 0)
			{
				message.writer().writeByte(flagType);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void setLockInventory(int pass)
	{
		Message message = null;
		try
		{
			Res.outz("------------setLockInventory:     " + pass);
			message = new Message((sbyte)(-104));
			message.writer().writeInt(pass);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void petStatus(sbyte status)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-108));
			message.writer().writeByte(status);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void transportNow()
	{
		Message message = null;
		try
		{
			Res.outz("------------transportNow  ");
			message = new Message((sbyte)(-105));
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void funsion(sbyte type)
	{
		Message message = null;
		try
		{
			Res.outz("FUNSION");
			message = new Message((sbyte)125);
			message.writer().writeByte(type);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void imageSource(MyVector vID)
	{
		Message message = null;
		try
		{
			Res.outz("IMAGE SOURCE size= " + vID.size());
			message = new Message((sbyte)(-111));
			message.writer().writeShort(vID.size());
			if (vID.size() > 0)
			{
				for (int i = 0; i < vID.size(); i++)
				{
					Res.outz("gui len str " + ((ImageSource)vID.elementAt(i)).id);
					message.writer().writeUTF(((ImageSource)vID.elementAt(i)).id);
				}
			}
			if (Session_ME2.gI().isConnected() && !Session_ME2.connecting)
			{
				session = Session_ME2.gI();
			}
			else
			{
				session = Session_ME.gI();
				reciveFromMainSession = true;
			}
			session.sendMessage(message);
			session = Session_ME.gI();
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getQuayso()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-126));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendServerData(sbyte action, int id, sbyte[] data)
	{
		Message message = null;
		try
		{
			Res.outz("SERVER DATA");
			message = new Message((sbyte)(-110));
			message.writer().writeByte(action);
			if (action == 1)
			{
				message.writer().writeInt(id);
				if (data != null)
				{
					int num = data.Length;
					message.writer().writeShort(num);
					message.writer().write(ref data, 0, num);
				}
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void changeOnKeyScr(sbyte[] skill)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-113));
			for (int i = 0; i < GameScr.onScreenSkill.Length; i++)
			{
				message.writer().writeByte(skill[i]);
			}
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void requestPean()
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-114));
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendThachDau(int id)
	{
		Res.outz("GUI THACH DAU");
		Message message = null;
		try
		{
			message = new Message((sbyte)(-118));
			message.writer().writeInt(id);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void messagePlayerMenu(int charId)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-30));
			message.writer().writeByte((sbyte)63);
			message.writer().writeInt(charId);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void playerMenuAction(int charId, short select)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-30));
			message.writer().writeByte((sbyte)64);
			message.writer().writeInt(charId);
			message.writer().writeShort(select);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			ex.StackTrace.ToString();
		}
		finally
		{
			message.cleanup();
		}
	}

	public void getImgByName(string nameImg)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)66);
			message.writer().writeUTF(nameImg);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void SendCrackBall(byte type, byte soluong)
	{
		Message message = new Message((sbyte)(-127));
		try
		{
			message.writer().writeByte(type);
			if (soluong > 0)
			{
				message.writer().writeByte(soluong);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void SendRada(int i, int id)
	{
		Message message = new Message(sbyte.MaxValue);
		try
		{
			message.writer().writeByte(i);
			if (id != -1)
			{
				message.writer().writeShort(id);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendOptHat(sbyte action)
	{
		Message message = new Message((sbyte)24);
		try
		{
			if (action == 1)
			{
				sbyte[] array = Res.TakeSnapShot();
				message.writer().writeByte(1);
				message.writer().writeShort(array.Length);
				message.writer().write(array);
			}
			else
			{
				message.writer().writeByte((Char.myCharz().idHat != -1) ? (-1) : 0);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendDelAcc()
	{
		Message message = new Message((sbyte)69);
		try
		{
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void new_skill_not_focus(sbyte idTemplateSkill, sbyte dir, short x, short y)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)(-45));
			message.writer().writeSByte(20);
			message.writer().writeSByte(idTemplateSkill);
			message.writer().writeShort(Char.myCharz().cx);
			message.writer().writeShort(Char.myCharz().cy);
			message.writer().writeSByte(dir);
			message.writer().writeShort(x);
			message.writer().writeShort(y);
			session.sendMessage(message);
		}
		catch (Exception ex)
		{
			Cout.println(ex.Message + ex.StackTrace);
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendCT_ready(sbyte sub, sbyte sub_sub)
	{
		Message message = null;
		try
		{
			message = new Message((sbyte)24);
			message.writer().writeByte(sub);
			message.writer().writeByte(sub_sub);
			Res.err(" =====> SEND OPTION_HAT " + sub + "_" + sub_sub);
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}

	public void sendCmdExtra(sbyte sub, string user, string pass)
	{
		Message message = new Message((sbyte)24);
		try
		{
			message.writer().writeByte(sub);
			if (sub == sbyte.MaxValue)
			{
				message.writer().writeUTF(user);
				message.writer().writeUTF(pass);
				Controller.isEXTRA_LINK = false;
				Res.err(" =====> SEND EXTRA_LINK " + sub + " user:" + user + " pass:" + pass);
			}
			session.sendMessage(message);
		}
		catch (Exception)
		{
		}
		finally
		{
			message.cleanup();
		}
	}
}
