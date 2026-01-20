using main.Mod;
using System.Collections.Generic;
using Xmap;

namespace ModCak.Mod.CuongLe
{
    internal class AutoReceiveNRD
    {
        public static bool reciveNrd = false;
        private static StepAutoReceiveNRD _currentStep = StepAutoReceiveNRD.reciveNrd;
        private static long waitTime = 0;
        private static List<int> TimeWait = new List<int>();
        public enum StepAutoReceiveNRD
        {
            reciveNrd,
            getTimeWait,
            wait
        }

        public static void Update()
        {
            if (!reciveNrd || MainXmapCL.isXmaping || mSystem.currentTimeMillis() < waitTime) return;

            if (_currentStep == StepAutoReceiveNRD.reciveNrd)
            {
                int mapTramTau = Char.myCharz().cgender + 24;
                if (TileMap.mapID != mapTramTau)
                {
                    MainXmapCL.StartGoToMap(mapTramTau);
                }
                else
                {
                    Npc npcTramTau = GameScr.findNPCInMap(29);
                    if (npcTramTau != null)
                    {
                        MainMod.TeleportTo(npcTramTau.cx, npcTramTau.cy);
                    }
                    NextMap.startComfirmNpc(29, "nhận thưởng");
                    waitTime += mSystem.currentTimeMillis() + 2500;

                }
            }
            else if (_currentStep == StepAutoReceiveNRD.getTimeWait)
            {
                for (int i = 0; i < GameCanvas.menu.menuItems.size(); i++)
                {
                    try
                    {
                        object obj = GameCanvas.menu.menuItems.elementAt(i);
                        if (obj == null) continue;

                        string text = NextMap.NormalizeText(((Command)obj).caption ?? "");
                        if (text.Contains("nhận thưởng") && !text.Contains("1 sao") && !text.Contains("2 sao"))
                        {
                            // hmm nhận xong off luôn thì hay hơn, nhận vầy ko ổn


                        }
                    }
                    catch { }
                }

            }
            
        }
    }
}
