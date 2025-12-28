using System;
using Assets.src.g;

namespace Assets.src.f;

internal class Controller2
{
	public static void readMessage(Message msg)
	{
		try
		{
			switch (msg.command)
			{
			case sbyte.MinValue:
				readInfoEffChar(msg);
				break;
			case sbyte.MaxValue:
				readInfoRada(msg);
				break;
			case 114:
				try
				{
					string text = msg.reader().readUTF();
					mSystem.curINAPP = msg.reader().readByte();
					mSystem.maxINAPP = msg.reader().readByte();
					break;
				}
				catch (Exception)
				{
					break;
				}
			case 113:
			{
				int loop = 0;
				int layer = 0;
				int id2 = 0;
				short x = 0;
				short y = 0;
				short loopCount = -1;
				try
				{
					loop = msg.reader().readByte();
					layer = msg.reader().readByte();
					id2 = msg.reader().readShort();
					x = msg.reader().readShort();
					y = msg.reader().readShort();
					loopCount = msg.reader().readShort();
				}
				catch (Exception)
				{
				}
				EffecMn.addEff(new Effect(id2, x, y, layer, loop, loopCount));
				break;
			}
			case 48:
			{
				sbyte b29 = msg.reader().readByte();
				GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
				Session_ME.gI().close();
				GameCanvas.endDlg();
				ServerListScreen.waitToLogin = true;
				break;
			}
			case 31:
			{
				int num20 = msg.reader().readInt();
				sbyte b19 = msg.reader().readByte();
				if (b19 == 1)
				{
					short smallID = msg.reader().readShort();
					sbyte b20 = -1;
					int[] array7 = null;
					short wimg = 0;
					short himg = 0;
					try
					{
						b20 = msg.reader().readByte();
						if (b20 > 0)
						{
							sbyte b21 = msg.reader().readByte();
							array7 = new int[b21];
							for (int num21 = 0; num21 < b21; num21++)
							{
								array7[num21] = msg.reader().readByte();
							}
							wimg = msg.reader().readShort();
							himg = msg.reader().readShort();
						}
					}
					catch (Exception)
					{
					}
					if (num20 == Char.myCharz().charID)
					{
						Char.myCharz().petFollow = new PetFollow();
						Char.myCharz().petFollow.smallID = smallID;
						if (b20 > 0)
						{
							Char.myCharz().petFollow.SetImg(b20, array7, wimg, himg);
						}
						break;
					}
					Char obj2 = GameScr.findCharInMap(num20);
					obj2.petFollow = new PetFollow();
					obj2.petFollow.smallID = smallID;
					if (b20 > 0)
					{
						obj2.petFollow.SetImg(b20, array7, wimg, himg);
					}
				}
				else if (num20 == Char.myCharz().charID)
				{
					Char.myCharz().petFollow.remove();
					Char.myCharz().petFollow = null;
				}
				else
				{
					Char obj3 = GameScr.findCharInMap(num20);
					obj3.petFollow.remove();
					obj3.petFollow = null;
				}
				break;
			}
			case -89:
				GameCanvas.open3Hour = msg.reader().readByte() == 1;
				break;
			case 42:
			{
				GameCanvas.endDlg();
				LoginScr.isContinueToLogin = false;
				Char.isLoadingMap = false;
				sbyte haveName = msg.reader().readByte();
				if (GameCanvas.registerScr == null)
				{
					GameCanvas.registerScr = new RegisterScreen(haveName);
				}
				GameCanvas.registerScr.switchToMe();
				break;
			}
			case 52:
			{
				sbyte b28 = msg.reader().readByte();
				if (b28 == 1)
				{
					int num32 = msg.reader().readInt();
					if (num32 == Char.myCharz().charID)
					{
						Char.myCharz().setMabuHold(m: true);
						Char.myCharz().cx = msg.reader().readShort();
						Char.myCharz().cy = msg.reader().readShort();
					}
					else
					{
						Char obj5 = GameScr.findCharInMap(num32);
						if (obj5 != null)
						{
							obj5.setMabuHold(m: true);
							obj5.cx = msg.reader().readShort();
							obj5.cy = msg.reader().readShort();
						}
					}
				}
				if (b28 == 0)
				{
					int num33 = msg.reader().readInt();
					if (num33 == Char.myCharz().charID)
					{
						Char.myCharz().setMabuHold(m: false);
					}
					else
					{
						GameScr.findCharInMap(num33)?.setMabuHold(m: false);
					}
				}
				if (b28 == 2)
				{
					int charId3 = msg.reader().readInt();
					int id5 = msg.reader().readInt();
					Mabu mabu2 = (Mabu)GameScr.findCharInMap(charId3);
					mabu2.eat(id5);
				}
				if (b28 == 3)
				{
					GameScr.mabuPercent = msg.reader().readByte();
				}
				break;
			}
			case 51:
			{
				int charId2 = msg.reader().readInt();
				Mabu mabu = (Mabu)GameScr.findCharInMap(charId2);
				sbyte id4 = msg.reader().readByte();
				short x2 = msg.reader().readShort();
				short y2 = msg.reader().readShort();
				sbyte b27 = msg.reader().readByte();
				Char[] array8 = new Char[b27];
				long[] array9 = new long[b27];
				for (int num30 = 0; num30 < b27; num30++)
				{
					int num31 = msg.reader().readInt();
					Res.outz("char ID=" + num31);
					array8[num30] = null;
					if (num31 != Char.myCharz().charID)
					{
						array8[num30] = GameScr.findCharInMap(num31);
					}
					else
					{
						array8[num30] = Char.myCharz();
					}
					array9[num30] = msg.reader().readLong();
				}
				mabu.setSkill(id4, x2, y2, array8, array9);
				break;
			}
			case -127:
				readLuckyRound(msg);
				break;
			case -126:
			{
				sbyte b13 = msg.reader().readByte();
				Res.outz("type quay= " + b13);
				if (b13 == 1)
				{
					sbyte b14 = msg.reader().readByte();
					string num13 = msg.reader().readUTF();
					string finish = msg.reader().readUTF();
					GameScr.gI().showWinNumber(num13, finish);
				}
				if (b13 == 0)
				{
					GameScr.gI().showYourNumber(msg.reader().readUTF());
				}
				break;
			}
			case -122:
			{
				short id = msg.reader().readShort();
				Npc npc = GameScr.findNPCInMap(id);
				sbyte b4 = msg.reader().readByte();
				npc.duahau = new int[b4];
				Res.outz("N DUA HAU= " + b4);
				for (int k = 0; k < b4; k++)
				{
					npc.duahau[k] = msg.reader().readShort();
				}
				npc.setStatus(msg.reader().readByte(), msg.reader().readInt());
				break;
			}
			case 102:
			{
				sbyte b15 = msg.reader().readByte();
				if (b15 == 0 || b15 == 1 || b15 == 2 || b15 == 6)
				{
					BigBoss2 bigBoss = Mob.getBigBoss2();
					if (bigBoss == null)
					{
						break;
					}
					if (b15 == 6)
					{
						bigBoss.x = (bigBoss.y = (bigBoss.xTo = (bigBoss.yTo = (bigBoss.xFirst = (bigBoss.yFirst = -1000)))));
						break;
					}
					sbyte b16 = msg.reader().readByte();
					Char[] array2 = new Char[b16];
					long[] array3 = new long[b16];
					for (int num14 = 0; num14 < b16; num14++)
					{
						int num15 = msg.reader().readInt();
						array2[num14] = null;
						if (num15 != Char.myCharz().charID)
						{
							array2[num14] = GameScr.findCharInMap(num15);
						}
						else
						{
							array2[num14] = Char.myCharz();
						}
						array3[num14] = msg.reader().readLong();
					}
					bigBoss.setAttack(array2, array3, b15);
				}
				if (b15 == 3 || b15 == 4 || b15 == 5 || b15 == 7)
				{
					BachTuoc bachTuoc = Mob.getBachTuoc();
					if (bachTuoc == null)
					{
						break;
					}
					if (b15 == 7)
					{
						bachTuoc.x = (bachTuoc.y = (bachTuoc.xTo = (bachTuoc.yTo = (bachTuoc.xFirst = (bachTuoc.yFirst = -1000)))));
						break;
					}
					if (b15 == 3 || b15 == 4)
					{
						sbyte b17 = msg.reader().readByte();
						Char[] array4 = new Char[b17];
						long[] array5 = new long[b17];
						for (int num16 = 0; num16 < b17; num16++)
						{
							int num17 = msg.reader().readInt();
							array4[num16] = null;
							if (num17 != Char.myCharz().charID)
							{
								array4[num16] = GameScr.findCharInMap(num17);
							}
							else
							{
								array4[num16] = Char.myCharz();
							}
							array5[num16] = msg.reader().readLong();
						}
						bachTuoc.setAttack(array4, array5, b15);
					}
					if (b15 == 5)
					{
						short xMoveTo = msg.reader().readShort();
						bachTuoc.move(xMoveTo);
					}
				}
				if (b15 > 9 && b15 < 30)
				{
					readActionBoss(msg, b15);
				}
				break;
			}
			case 101:
			{
				Res.outz("big boss--------------------------------------------------");
				BigBoss bigBoss2 = Mob.getBigBoss();
				if (bigBoss2 == null)
				{
					break;
				}
				sbyte b30 = msg.reader().readByte();
				if (b30 == 0 || b30 == 1 || b30 == 2 || b30 == 4 || b30 == 3)
				{
					if (b30 == 3)
					{
						bigBoss2.xTo = (bigBoss2.xFirst = msg.reader().readShort());
						bigBoss2.yTo = (bigBoss2.yFirst = msg.reader().readShort());
						bigBoss2.setFly();
					}
					else
					{
						sbyte b31 = msg.reader().readByte();
						Res.outz("CHUONG nChar= " + b31);
						Char[] array10 = new Char[b31];
						long[] array11 = new long[b31];
						for (int num38 = 0; num38 < b31; num38++)
						{
							int num39 = msg.reader().readInt();
							Res.outz("char ID=" + num39);
							array10[num38] = null;
							if (num39 != Char.myCharz().charID)
							{
								array10[num38] = GameScr.findCharInMap(num39);
							}
							else
							{
								array10[num38] = Char.myCharz();
							}
							array11[num38] = msg.reader().readLong();
						}
						bigBoss2.setAttack(array10, array11, b30);
					}
				}
				if (b30 == 5)
				{
					bigBoss2.haftBody = true;
					bigBoss2.status = 2;
				}
				if (b30 == 6)
				{
					bigBoss2.getDataB2();
					bigBoss2.x = msg.reader().readShort();
					bigBoss2.y = msg.reader().readShort();
				}
				if (b30 == 7)
				{
					bigBoss2.setAttack(null, null, b30);
				}
				if (b30 == 8)
				{
					bigBoss2.xTo = (bigBoss2.xFirst = msg.reader().readShort());
					bigBoss2.yTo = (bigBoss2.yFirst = msg.reader().readShort());
					bigBoss2.status = 2;
				}
				if (b30 == 9)
				{
					bigBoss2.x = (bigBoss2.y = (bigBoss2.xTo = (bigBoss2.yTo = (bigBoss2.xFirst = (bigBoss2.yFirst = -1000)))));
				}
				break;
			}
			case -120:
			{
				long num3 = mSystem.currentTimeMillis();
				Service.logController = num3 - Service.curCheckController;
				Service.gI().sendCheckController();
				break;
			}
			case -121:
			{
				long num36 = mSystem.currentTimeMillis();
				Service.logMap = num36 - Service.curCheckMap;
				Service.gI().sendCheckMap();
				break;
			}
			case 100:
			{
				sbyte b6 = msg.reader().readByte();
				sbyte b7 = msg.reader().readByte();
				Item item = null;
				if (b6 == 0)
				{
					item = Char.myCharz().arrItemBody[b7];
				}
				if (b6 == 1)
				{
					item = Char.myCharz().arrItemBag[b7];
				}
				short num4 = msg.reader().readShort();
				if (num4 == -1)
				{
					break;
				}
				item.template = ItemTemplates.get(num4);
				item.quantity = msg.reader().readInt();
				item.info = msg.reader().readUTF();
				item.content = msg.reader().readUTF();
				sbyte b8 = msg.reader().readByte();
				if (b8 != 0)
				{
					item.itemOption = new ItemOption[b8];
					for (int m = 0; m < item.itemOption.Length; m++)
					{
						ItemOption itemOption2 = Controller.gI().readItemOption(msg);
						if (itemOption2 != null)
						{
							item.itemOption[m] = itemOption2;
						}
					}
				}
				if (item.quantity <= 0)
				{
					item = null;
				}
				break;
			}
			case -123:
			{
				int charId = msg.reader().readInt();
				if (GameScr.findCharInMap(charId) != null)
				{
					GameScr.findCharInMap(charId).perCentMp = msg.reader().readByte();
				}
				break;
			}
			case -119:
				Char.myCharz().rank = msg.reader().readInt();
				break;
			case -117:
				GameScr.gI().tMabuEff = 0;
				GameScr.gI().percentMabu = msg.reader().readByte();
				if (GameScr.gI().percentMabu == 100)
				{
					GameScr.gI().mabuEff = true;
				}
				if (GameScr.gI().percentMabu == 101)
				{
					Npc.mabuEff = true;
				}
				break;
			case -116:
				GameScr.canAutoPlay = msg.reader().readByte() == 1;
				break;
			case -115:
				Char.myCharz().setPowerInfo(msg.reader().readUTF(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
				break;
			case -113:
			{
				sbyte[] array = new sbyte[10];
				for (int l = 0; l < 10; l++)
				{
					array[l] = msg.reader().readByte();
					Res.outz("vlue i= " + array[l]);
				}
				GameScr.gI().onKSkill(array);
				GameScr.gI().onOSkill(array);
				GameScr.gI().onCSkill(array);
				break;
			}
			case -111:
			{
				short num34 = msg.reader().readShort();
				ImageSource.vSource = new MyVector();
				for (int num35 = 0; num35 < num34; num35++)
				{
					string iD = msg.reader().readUTF();
					sbyte version = msg.reader().readByte();
					ImageSource.vSource.addElement(new ImageSource(iD, version));
				}
				ImageSource.checkRMS();
				ImageSource.saveRMS();
				break;
			}
			case 125:
			{
				sbyte fusion = msg.reader().readByte();
				int num37 = msg.reader().readInt();
				if (num37 == Char.myCharz().charID)
				{
					Char.myCharz().setFusion(fusion);
				}
				else if (GameScr.findCharInMap(num37) != null)
				{
					GameScr.findCharInMap(num37).setFusion(fusion);
				}
				break;
			}
			case 124:
			{
				short id6 = msg.reader().readShort();
				string text4 = msg.reader().readUTF();
				Res.outz("noi chuyen = " + text4 + "npc ID= " + id6);
				GameScr.findNPCInMap(id6)?.addInfo(text4);
				break;
			}
			case 123:
			{
				Res.outz("SET POSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss");
				int num23 = msg.reader().readInt();
				short xPos = msg.reader().readShort();
				short yPos = msg.reader().readShort();
				sbyte b24 = msg.reader().readByte();
				Char obj4 = null;
				if (num23 == Char.myCharz().charID)
				{
					obj4 = Char.myCharz();
				}
				else if (GameScr.findCharInMap(num23) != null)
				{
					obj4 = GameScr.findCharInMap(num23);
				}
				if (obj4 != null)
				{
					ServerEffect.addServerEffect((b24 != 0) ? 173 : 60, obj4, 1);
					obj4.setPos(xPos, yPos, b24);
				}
				break;
			}
			case 122:
			{
				short timeLogin = msg.reader().readShort();
				Res.outz("second login = " + timeLogin);
                        LoginScr.timeLogin = timeLogin;
				LoginScr.currTimeLogin = (LoginScr.lastTimeLogin = mSystem.currentTimeMillis());
				GameCanvas.endDlg();
				break;
			}
			case 121:
				mSystem.publicID = msg.reader().readUTF();
				mSystem.strAdmob = msg.reader().readUTF();
				Res.outz("SHOW AD public ID= " + mSystem.publicID);
				mSystem.createAdmob();
				break;
			case -124:
			{
				sbyte b25 = msg.reader().readByte();
				sbyte b26 = msg.reader().readByte();
				if (b26 == 0)
				{
					if (b25 == 2)
					{
						int num24 = msg.reader().readInt();
						if (num24 == Char.myCharz().charID)
						{
							Char.myCharz().removeEffect();
						}
						else if (GameScr.findCharInMap(num24) != null)
						{
							GameScr.findCharInMap(num24).removeEffect();
						}
					}
					int num25 = msg.reader().readUnsignedByte();
					int num26 = msg.reader().readInt();
					if (num25 == 32)
					{
						if (b25 == 1)
						{
							int num27 = msg.reader().readInt();
							if (num26 == Char.myCharz().charID)
							{
								Char.myCharz().holdEffID = num25;
								GameScr.findCharInMap(num27).setHoldChar(Char.myCharz());
							}
							else if (GameScr.findCharInMap(num26) != null && num27 != Char.myCharz().charID)
							{
								GameScr.findCharInMap(num26).holdEffID = num25;
								GameScr.findCharInMap(num27).setHoldChar(GameScr.findCharInMap(num26));
							}
							else if (GameScr.findCharInMap(num26) != null && num27 == Char.myCharz().charID)
							{
								GameScr.findCharInMap(num26).holdEffID = num25;
								Char.myCharz().setHoldChar(GameScr.findCharInMap(num26));
							}
						}
						else if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().removeHoleEff();
						}
						else if (GameScr.findCharInMap(num26) != null)
						{
							GameScr.findCharInMap(num26).removeHoleEff();
						}
					}
					if (num25 == 33)
					{
						if (b25 == 1)
						{
							if (num26 == Char.myCharz().charID)
							{
								Char.myCharz().protectEff = true;
							}
							else if (GameScr.findCharInMap(num26) != null)
							{
								GameScr.findCharInMap(num26).protectEff = true;
							}
						}
						else if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().removeProtectEff();
						}
						else if (GameScr.findCharInMap(num26) != null)
						{
							GameScr.findCharInMap(num26).removeProtectEff();
						}
					}
					if (num25 == 39)
					{
						if (b25 == 1)
						{
							if (num26 == Char.myCharz().charID)
							{
								Char.myCharz().huytSao = true;
							}
							else if (GameScr.findCharInMap(num26) != null)
							{
								GameScr.findCharInMap(num26).huytSao = true;
							}
						}
						else if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().removeHuytSao();
						}
						else if (GameScr.findCharInMap(num26) != null)
						{
							GameScr.findCharInMap(num26).removeHuytSao();
						}
					}
					if (num25 == 40)
					{
						if (b25 == 1)
						{
							if (num26 == Char.myCharz().charID)
							{
								Char.myCharz().blindEff = true;
							}
							else if (GameScr.findCharInMap(num26) != null)
							{
								GameScr.findCharInMap(num26).blindEff = true;
							}
						}
						else if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().removeBlindEff();
						}
						else if (GameScr.findCharInMap(num26) != null)
						{
							GameScr.findCharInMap(num26).removeBlindEff();
						}
					}
					if (num25 == 41)
					{
						if (b25 == 1)
						{
							if (num26 == Char.myCharz().charID)
							{
								Char.myCharz().sleepEff = true;
							}
							else if (GameScr.findCharInMap(num26) != null)
							{
								GameScr.findCharInMap(num26).sleepEff = true;
							}
						}
						else if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().removeSleepEff();
						}
						else if (GameScr.findCharInMap(num26) != null)
						{
							GameScr.findCharInMap(num26).removeSleepEff();
						}
					}
					if (num25 == 42)
					{
						if (b25 == 1)
						{
							if (num26 == Char.myCharz().charID)
							{
								Char.myCharz().stone = true;
							}
						}
						else if (num26 == Char.myCharz().charID)
						{
							Char.myCharz().stone = false;
						}
					}
				}
				if (b26 != 1)
				{
					break;
				}
				int num28 = msg.reader().readUnsignedByte();
				sbyte mobIndex = msg.reader().readByte();
				Res.outz("modbHoldID= " + mobIndex + " skillID= " + num28 + "eff ID= " + b25);
				if (num28 == 32)
				{
					if (b25 == 1)
					{
						int num29 = msg.reader().readInt();
						if (num29 == Char.myCharz().charID)
						{
							GameScr.findMobInMap(mobIndex).holdEffID = num28;
							Char.myCharz().setHoldMob(GameScr.findMobInMap(mobIndex));
						}
						else if (GameScr.findCharInMap(num29) != null)
						{
							GameScr.findMobInMap(mobIndex).holdEffID = num28;
							GameScr.findCharInMap(num29).setHoldMob(GameScr.findMobInMap(mobIndex));
						}
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeHoldEff();
					}
				}
				if (num28 == 40)
				{
					if (b25 == 1)
					{
						GameScr.findMobInMap(mobIndex).blindEff = true;
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeBlindEff();
					}
				}
				if (num28 == 41)
				{
					if (b25 == 1)
					{
						GameScr.findMobInMap(mobIndex).sleepEff = true;
					}
					else
					{
						GameScr.findMobInMap(mobIndex).removeSleepEff();
					}
				}
				break;
			}
			case -125:
			{
				ChatTextField.gI().isShow = false;
				string text3 = msg.reader().readUTF();
				Res.outz("titile= " + text3);
				sbyte b22 = msg.reader().readByte();
				ClientInput.gI().setInput(b22, text3);
				for (int num22 = 0; num22 < b22; num22++)
				{
					ClientInput.gI().tf[num22].name = msg.reader().readUTF();
					sbyte b23 = msg.reader().readByte();
					if (b23 == 0)
					{
						ClientInput.gI().tf[num22].setIputType(TField.INPUT_TYPE_NUMERIC);
					}
					if (b23 == 1)
					{
						ClientInput.gI().tf[num22].setIputType(TField.INPUT_TYPE_ANY);
					}
					if (b23 == 2)
					{
						ClientInput.gI().tf[num22].setIputType(TField.INPUT_TYPE_PASSWORD);
					}
				}
				break;
			}
			case -110:
			{
				sbyte b18 = msg.reader().readByte();
				if (b18 == 1)
				{
					int id3 = msg.reader().readInt();
					sbyte[] array6 = Rms.loadRMS(id3 + string.Empty);
					if (array6 == null)
					{
						Service.gI().sendServerData(1, -1, null);
					}
					else
					{
						Service.gI().sendServerData(1, id3, array6);
					}
				}
				if (b18 == 0)
				{
					int num18 = msg.reader().readInt();
					short num19 = msg.reader().readShort();
					sbyte[] data = new sbyte[num19];
					msg.reader().read(ref data, 0, num19);
					Rms.saveRMS(num18 + string.Empty, data);
				}
				break;
			}
			case 93:
			{
				string str = msg.reader().readUTF();
				str = Res.changeString(str);
				if (!str.ToLower().Contains("trò chơi dành cho người chơi trên 18 tuổi"))
				{
					GameScr.gI().chatVip(str);
				}
				break;
			}
			case -106:
			{
				short num11 = msg.reader().readShort();
				int num12 = msg.reader().readShort();
				if (ItemTime.isExistItem(num11))
				{
					ItemTime.getItemById(num11).initTime(num12);
					break;
				}
				ItemTime o = new ItemTime(num11, num12);
				Char.vItemTime.addElement(o);
				break;
			}
			case -105:
				TransportScr.gI().time = 0;
				TransportScr.gI().maxTime = msg.reader().readShort();
				TransportScr.gI().last = (TransportScr.gI().curr = mSystem.currentTimeMillis());
				TransportScr.gI().type = msg.reader().readByte();
				TransportScr.gI().switchToMe();
				break;
			case -103:
				switch (msg.reader().readByte())
				{
				case 0:
				{
					GameCanvas.panel.vFlag.removeAllElements();
					sbyte b10 = msg.reader().readByte();
					for (int num7 = 0; num7 < b10; num7++)
					{
						Item item2 = new Item();
						short num8 = msg.reader().readShort();
						if (num8 != -1)
						{
							item2.template = ItemTemplates.get(num8);
							sbyte b11 = msg.reader().readByte();
							if (b11 != -1)
							{
								item2.itemOption = new ItemOption[b11];
								for (int num9 = 0; num9 < item2.itemOption.Length; num9++)
								{
									ItemOption itemOption3 = Controller.gI().readItemOption(msg);
									if (itemOption3 != null)
									{
										item2.itemOption[num9] = itemOption3;
									}
								}
							}
						}
						GameCanvas.panel.vFlag.addElement(item2);
					}
					GameCanvas.panel.setTypeFlag();
					GameCanvas.panel.show();
					break;
				}
				case 1:
				{
					int num10 = msg.reader().readInt();
					sbyte b12 = msg.reader().readByte();
					Res.outz("---------------actionFlag1:  " + num10 + " : " + b12);
					if (num10 == Char.myCharz().charID)
					{
						Char.myCharz().cFlag = b12;
					}
					else if (GameScr.findCharInMap(num10) != null)
					{
						GameScr.findCharInMap(num10).cFlag = b12;
					}
					GameScr.gI().getFlagImage(num10, b12);
					break;
				}
				case 2:
				{
					sbyte b9 = msg.reader().readByte();
					int num5 = msg.reader().readShort();
					PKFlag pKFlag = new PKFlag();
					pKFlag.cflag = b9;
					pKFlag.IDimageFlag = num5;
					GameScr.vFlag.addElement(pKFlag);
					for (int n = 0; n < GameScr.vFlag.size(); n++)
					{
						PKFlag pKFlag2 = (PKFlag)GameScr.vFlag.elementAt(n);
						Res.outz("i: " + n + "  cflag: " + pKFlag2.cflag + "   IDimageFlag: " + pKFlag2.IDimageFlag);
					}
					for (int num6 = 0; num6 < GameScr.vCharInMap.size(); num6++)
					{
						Char obj = (Char)GameScr.vCharInMap.elementAt(num6);
						if (obj != null && obj.cFlag == b9)
						{
							obj.flagImage = num5;
						}
					}
					if (Char.myCharz().cFlag == b9)
					{
						Char.myCharz().flagImage = num5;
					}
					break;
				}
				}
				break;
			case -102:
			{
				sbyte b5 = msg.reader().readByte();
				if (b5 != 0 && b5 == 1)
				{
					GameCanvas.loginScr.isLogin2 = false;
					Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
					LoginScr.isLoggingIn = true;
				}
				break;
			}
			case -101:
			{
				GameCanvas.loginScr.isLogin2 = true;
				GameCanvas.connect();
				string text2 = msg.reader().readUTF();
				Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, text2);
				Service.gI().setClientType();
				Service.gI().login(text2, string.Empty, GameMidlet.VERSION, 1);
				break;
			}
			case -100:
			{
				InfoDlg.hide();
				bool flag = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
				{
					flag = true;
				}
				sbyte b = msg.reader().readByte();
				if (b < 0)
				{
					break;
				}
				Res.outz("t Indxe= " + b);
				GameCanvas.panel.maxPageShop[b] = msg.reader().readByte();
				GameCanvas.panel.currPageShop[b] = msg.reader().readByte();
				Res.outz("Max page= " + GameCanvas.panel.maxPageShop[b] + " curr page= " + GameCanvas.panel.currPageShop[b]);
				int num = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop[b] = new Item[num];
				for (int i = 0; i < num; i++)
				{
					short num2 = msg.reader().readShort();
					if (num2 == -1)
					{
						continue;
					}
					Res.outz("template id= " + num2);
					Char.myCharz().arrItemShop[b][i] = new Item();
					Char.myCharz().arrItemShop[b][i].template = ItemTemplates.get(num2);
					Char.myCharz().arrItemShop[b][i].itemId = msg.reader().readShort();
					Char.myCharz().arrItemShop[b][i].buyCoin = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].buyGold = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].buyType = msg.reader().readByte();
					Char.myCharz().arrItemShop[b][i].quantity = msg.reader().readInt();
					Char.myCharz().arrItemShop[b][i].isMe = msg.reader().readByte();
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					sbyte b2 = msg.reader().readByte();
					if (b2 != -1)
					{
						Char.myCharz().arrItemShop[b][i].itemOption = new ItemOption[b2];
						for (int j = 0; j < Char.myCharz().arrItemShop[b][i].itemOption.Length; j++)
						{
							ItemOption itemOption = Controller.gI().readItemOption(msg);
							if (itemOption != null)
							{
								Char.myCharz().arrItemShop[b][i].itemOption[j] = itemOption;
								Char.myCharz().arrItemShop[b][i].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[b][i]);
							}
						}
					}
					sbyte b3 = msg.reader().readByte();
					if (b3 == 1)
					{
						int headTemp = msg.reader().readShort();
						int bodyTemp = msg.reader().readShort();
						int legTemp = msg.reader().readShort();
						int bagTemp = msg.reader().readShort();
						Char.myCharz().arrItemShop[b][i].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
					}
					if (GameMidlet.intVERSION >= 237)
					{
						Char.myCharz().arrItemShop[b][i].nameNguoiKyGui = msg.reader().readUTF();
						Res.err("nguoi ki gui  " + Char.myCharz().arrItemShop[b][i].nameNguoiKyGui);
					}
				}
				if (flag)
				{
					GameCanvas.panel2.setTabKiGui();
				}
				GameCanvas.panel.setTabShop();
				GameCanvas.panel.cmy = (GameCanvas.panel.cmtoY = 0);
				break;
			}
			}
		}
		catch (Exception ex4)
		{
			Res.outz("=====> Controller2 " + ex4.StackTrace);
		}
	}

	private static void readLuckyRound(Message msg)
	{
		try
		{
			switch (msg.reader().readByte())
			{
			case 0:
			{
				sbyte b2 = msg.reader().readByte();
				short[] array2 = new short[b2];
				for (int j = 0; j < b2; j++)
				{
					array2[j] = msg.reader().readShort();
				}
				sbyte b3 = msg.reader().readByte();
				int price = msg.reader().readInt();
				short idTicket = msg.reader().readShort();
				CrackBallScr.gI().SetCrackBallScr(array2, (byte)b3, price, idTicket);
				break;
			}
			case 1:
			{
				sbyte b = msg.reader().readByte();
				short[] array = new short[b];
				for (int i = 0; i < b; i++)
				{
					array[i] = msg.reader().readShort();
				}
				CrackBallScr.gI().DoneCrackBallScr(array);
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoRada(Message msg)
	{
		try
		{
			switch (msg.reader().readByte())
			{
			case 0:
			{
				RadarScr.gI();
				MyVector myVector = new MyVector(string.Empty);
				short num2 = msg.reader().readShort();
				int num3 = 0;
				for (int i = 0; i < num2; i++)
				{
					Info_RadaScr info_RadaScr = new Info_RadaScr();
					int id = msg.reader().readShort();
					int no = i + 1;
					int idIcon = msg.reader().readShort();
					sbyte rank = msg.reader().readByte();
					sbyte amount = msg.reader().readByte();
					sbyte max_amount = msg.reader().readByte();
					short templateId = -1;
					Char charInfo = null;
					sbyte b = msg.reader().readByte();
					if (b == 0)
					{
						templateId = msg.reader().readShort();
					}
					else
					{
						int head = msg.reader().readShort();
						int body = msg.reader().readShort();
						int leg = msg.reader().readShort();
						int bag = msg.reader().readShort();
						charInfo = Info_RadaScr.SetCharInfo(head, body, leg, bag);
					}
					string name = msg.reader().readUTF();
					string info = msg.reader().readUTF();
					sbyte b2 = msg.reader().readByte();
					sbyte use = msg.reader().readByte();
					sbyte b3 = msg.reader().readByte();
					ItemOption[] array = null;
					if (b3 != 0)
					{
						array = new ItemOption[b3];
						for (int j = 0; j < array.Length; j++)
						{
							ItemOption itemOption = Controller.gI().readItemOption(msg);
							sbyte activeCard = msg.reader().readByte();
							if (itemOption != null)
							{
								array[j] = itemOption;
								array[j].activeCard = activeCard;
							}
						}
					}
					info_RadaScr.SetInfo(id, no, idIcon, rank, b, templateId, name, info, charInfo, array);
					info_RadaScr.SetLevel(b2);
					info_RadaScr.SetUse(use);
					info_RadaScr.SetAmount(amount, max_amount);
					myVector.addElement(info_RadaScr);
					if (b2 > 0)
					{
						num3++;
					}
				}
				RadarScr.gI().SetRadarScr(myVector, num3, num2);
				RadarScr.gI().switchToMe();
				break;
			}
			case 1:
			{
				int id3 = msg.reader().readShort();
				sbyte use2 = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id3) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.list, id3).SetUse(use2);
				}
				RadarScr.SetListUse();
				break;
			}
			case 2:
			{
				int num4 = msg.reader().readShort();
				sbyte level = msg.reader().readByte();
				int num5 = 0;
				for (int k = 0; k < RadarScr.list.size(); k++)
				{
					Info_RadaScr info_RadaScr2 = (Info_RadaScr)RadarScr.list.elementAt(k);
					if (info_RadaScr2 != null)
					{
						if (info_RadaScr2.id == num4)
						{
							info_RadaScr2.SetLevel(level);
						}
						if (info_RadaScr2.level > 0)
						{
							num5++;
						}
					}
				}
				RadarScr.SetNum(num5, RadarScr.list.size());
				if (Info_RadaScr.GetInfo(RadarScr.listUse, num4) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.listUse, num4).SetLevel(level);
				}
				break;
			}
			case 3:
			{
				int id2 = msg.reader().readShort();
				sbyte amount2 = msg.reader().readByte();
				sbyte max_amount2 = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id2) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.list, id2).SetAmount(amount2, max_amount2);
				}
				if (Info_RadaScr.GetInfo(RadarScr.listUse, id2) != null)
				{
					Info_RadaScr.GetInfo(RadarScr.listUse, id2).SetAmount(amount2, max_amount2);
				}
				break;
			}
			case 4:
			{
				int num = msg.reader().readInt();
				short idAuraEff = msg.reader().readShort();
				Char obj = null;
				obj = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
				if (obj != null)
				{
					obj.idAuraEff = idAuraEff;
					obj.idEff_Set_Item = msg.reader().readByte();
				}
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoEffChar(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			int num = msg.reader().readInt();
			Char obj = null;
			obj = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
			switch (b)
			{
			case 0:
			{
				int id = msg.reader().readShort();
				int layer = msg.reader().readByte();
				int loop = msg.reader().readByte();
				short loopCount = msg.reader().readShort();
				sbyte isStand = msg.reader().readByte();
				obj?.addEffChar(new Effect(id, obj, layer, loop, loopCount, isStand));
				break;
			}
			case 1:
			{
				int id2 = msg.reader().readShort();
				obj?.removeEffChar(0, id2);
				break;
			}
			case 2:
				obj?.removeEffChar(-1, 0);
				break;
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readActionBoss(Message msg, int actionBoss)
	{
		try
		{
			sbyte idBoss = msg.reader().readByte();
			NewBoss newBoss = Mob.getNewBoss(idBoss);
			if (newBoss == null)
			{
				return;
			}
			if (actionBoss == 10)
			{
				short xMoveTo = msg.reader().readShort();
				short yMoveTo = msg.reader().readShort();
				newBoss.move(xMoveTo, yMoveTo);
			}
			if (actionBoss >= 11 && actionBoss <= 20)
			{
				sbyte b = msg.reader().readByte();
				Char[] array = new Char[b];
				long[] array2 = new long[b];
				for (int i = 0; i < b; i++)
				{
					int num = msg.reader().readInt();
					array[i] = null;
					if (num != Char.myCharz().charID)
					{
						array[i] = GameScr.findCharInMap(num);
					}
					else
					{
						array[i] = Char.myCharz();
					}
					array2[i] = msg.reader().readLong();
				}
				sbyte dir = msg.reader().readByte();
				newBoss.setAttack(array, array2, (sbyte)(actionBoss - 10), dir);
			}
			if (actionBoss == 21)
			{
				newBoss.xTo = msg.reader().readShort();
				newBoss.yTo = msg.reader().readShort();
				newBoss.setFly();
			}
			if (actionBoss == 22)
			{
			}
			if (actionBoss == 23)
			{
				newBoss.setDie();
			}
		}
		catch (Exception)
		{
		}
	}
}
