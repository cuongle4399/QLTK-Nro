using System;
using System.Threading;
using System.Windows.Forms;
using QLTK_Nro_Pro.Presenter.Socket;

namespace QLTK_Nro_Pro.HandlerSocket
{
    internal class SocketClientUpdater
    {
        private readonly Form form;
        private readonly Button btnAutoOpenTabError;
        private int delaySocket = 50;

        public SocketClientUpdater(Form form, Button btnAutoOpenTabError)
        {
            this.form = form;
            this.btnAutoOpenTabError = btnAutoOpenTabError;
        }

        #region Socket Command Core Methods

        /// <summary>
        /// Gửi lệnh socket và xử lý trạng thái button
        /// </summary>
        public void SendCommand(Button btn, string command)
        {
            if (btn == null) return;

            string cmdToSend;

            if (btn.Text.Contains("ON"))
            {
                cmdToSend = "OFF" + command;
                TCPSocket.send(cmdToSend);
                ChangeStatus(btn);
            }
            else if (btn.Text.Contains("OFF"))
            {
                cmdToSend = "ON" + command;
                TCPSocket.send(cmdToSend);
                ChangeStatus(btn);
            }
            else
            {
                cmdToSend = command;
                TCPSocket.send(cmdToSend);
            }

            Thread.Sleep(delaySocket);
        }

        /// <summary>
        /// Đổi trạng thái ON/OFF của button (hỗ trợ thread-safe)
        /// </summary>
        public void ChangeStatus(Button btn)
        {
            if (btn == null) return;

            if (btn.InvokeRequired)
            {
                btn.Invoke((MethodInvoker)(() => ToggleButtonText(btn)));
            }
            else
            {
                ToggleButtonText(btn);
            }
        }

        private void ToggleButtonText(Button btn)
        {
            if (btn.Text.Contains("ON"))
                btn.Text = btn.Text.Replace("ON", "OFF");
            else if (btn.Text.Contains("OFF"))
                btn.Text = btn.Text.Replace("OFF", "ON");
        }

        #endregion

        #region Socket Basic Commands

        public void SendChatMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                TCPSocket.send("chat|" + message);
                Thread.Sleep(delaySocket);
            }
        }

        public void SendKhuCommand(string khuValue)
        {
            TCPSocket.send("khu|" + khuValue);
            Thread.Sleep(delaySocket);
        }

        public void SendItemCommand(string itemId)
        {
            TCPSocket.send("item|" + itemId);
            Thread.Sleep(delaySocket);
        }

        public void SendTeleNpcCommand(int npcId)
        {
            TCPSocket.send("teleIdNpc|" + npcId);
            Thread.Sleep(delaySocket);
        }

        public void SendBongTaiCommand() { TCPSocket.send("bongtai"); Thread.Sleep(delaySocket); }
        public void SendBatCoDenCommand() { TCPSocket.send("BatCoDen"); Thread.Sleep(delaySocket); }
        public void SendTatCoCommand() { TCPSocket.send("TatCo"); Thread.Sleep(delaySocket); }

        #endregion

        #region Dynamic Parameterized Commands

        public void SendFarmNappaCommand(Button btn, int selectedIndex)
        {
            SendCommand(btn, "farmNappa|" + selectedIndex);
        }

        public void SendBoMongCommand(Button btn, string typeNV, int typeNVGold, bool nextGold, bool nextMob, bool nextHuman)
        {
            string cmd = "BoMong|" + typeNV.ToLower().Trim() + "|" + typeNVGold + "|"
                         + nextGold + "|" + nextMob + "|" + nextHuman;
            SendCommand(btn, cmd);
        }

        public void SendTTNLCommand(Button btn, int percenHP)
        {
            SendCommand(btn, $"deTTNL|{percenHP}");
        }

        public void SendReduceCPUCommand(Button btn, int fps)
        {
            SendCommand(btn, $"reduceCPU|{fps}");
        }

        public void SendCodeLiveCommand(Button btn, string codeLive)
        {
            SendCommand(btn, $"NhapCodeLive|{codeLive.Trim()}");
        }

        public void SendTagNameBossCommand(Button btn, string tagName)
        {
            SendCommand(btn, $"TagNameAutoBoss|{tagName.Trim().ToLower()}");
        }

        #endregion
    }
}