using System;
using System.Windows.Forms;
using QLTK_Nro_Pro.Presenter.Socket;
using QLTK_Nro_Pro.Presenter;

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
            BindEventHandlers();
        }

        /// <summary>
        /// Bind tất cả event handlers vào Form
        /// </summary>
        private void BindEventHandlers()
        {
            if (form == null) return;

            // Home Chat & Items
            BindButtonEvent(form, "btnChat", (s, e) => OnBtnChatClick());
            BindButtonEvent(form, "button1", (s, e) => OnButton1Click());
            BindButtonEvent(form, "button2", (s, e) => OnButton2Click());

            // Auto Boss
            BindButtonEvent(form, "button3", (s, e) => OnButton3Click());
            BindButtonEvent(form, "button4", (s, e) => OnButton4Click());
            BindButtonEvent(form, "button5", (s, e) => OnButton5Click());
            BindButtonEvent(form, "button7", (s, e) => OnButton7Click());
            BindButtonEvent(form, "button8", (s, e) => OnButton8Click());
            BindButtonEvent(form, "button13", (s, e) => OnButton13Click());
            BindButtonEvent(form, "button9", (s, e) => OnButton9Click());
            BindButtonEvent(form, "button14", (s, e) => OnButton14Click());

            // Train Mob
            BindButtonEvent(form, "button6", (s, e) => OnButton6Click());
            BindButtonEvent(form, "button16", (s, e) => OnButton16Click());
            BindButtonEvent(form, "button17", (s, e) => OnButton17Click());

            // Auto Bo Mong
            BindButtonEvent(form, "materialButton20", (s, e) => OnMaterialButton20Click());

            // Auto Pet
            BindButtonEvent(form, "button18", (s, e) => OnButton18Click());
            BindButtonEvent(form, "button19", (s, e) => OnButton19Click());
            BindButtonEvent(form, "button20", (s, e) => OnButton20Click());
            BindButtonEvent(form, "button21", (s, e) => OnButton21Click());
            BindButtonEvent(form, "button22", (s, e) => OnButton22Click());
            BindButtonEvent(form, "button23", (s, e) => OnButton23Click());
            BindButtonEvent(form, "materialButton131", (s, e) => OnMaterialButton131Click());
            BindButtonEvent(form, "materialButton132", (s, e) => OnMaterialButton132Click());
            BindButtonEvent(form, "materialButton133", (s, e) => OnMaterialButton133Click());

            // NPC & Teleport
            BindButtonEvent(form, "btnTeleNPC", (s, e) => OnBtnTeleNPCClick());

            // Utilities
            BindButtonEvent(form, "btnReduceCPU", (s, e) => OnBtnReduceCPUClick());
            BindButtonEvent(form, "btnNhapCodeLive", (s, e) => OnBtnNhapCodeLiveClick());
            BindButtonEvent(form, "materialButton77", (s, e) => OnMaterialButton77Click());

            // Train Commands
            BindButtonEvent(form, "btnNeSieuQuai", (s, e) => OnBtnNeSieuQuaiClick());
            BindButtonEvent(form, "btnAkDame", (s, e) => OnBtnAkDameClick());
            BindButtonEvent(form, "btnAutoNhat", (s, e) => OnBtnAutoNhatClick());
            BindButtonEvent(form, "btnNeBoss", (s, e) => OnBtnNeBossClick());
            BindButtonEvent(form, "btnAutoHopThe", (s, e) => OnBtnAutoHopTheClick());
            BindButtonEvent(form, "btnSpamZoneIt", (s, e) => OnBtnSpamZoneItClick());
            BindButtonEvent(form, "btnAutoZoneIt", (s, e) => OnBtnAutoZoneItClick());
            BindButtonEvent(form, "btnApDungVut", (s, e) => OnBtnApDungVutClick());
        }

        /// <summary>
        /// Helper method để bind event với button
        /// </summary>
        private void BindButtonEvent(Form form, string buttonName, EventHandler handler)
        {
            try
            {
                var button = form.Controls[buttonName] as Button;
                if (button != null)
                {
                    button.Click += handler;
                }
            }
            catch { }
        }

        #region Socket Command Methods

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
        /// Đổi trạng thái ON/OFF của button
        /// </summary>
        public void ChangeStatus(Button btn)
        {
            if (btn == null) return;

            if (btn.InvokeRequired)
            {
                btn.Invoke((MethodInvoker)(() =>
                {
                    if (btn.Text.Contains("ON"))
                        btn.Text = btn.Text.Replace("ON", "OFF");
                    else
                        btn.Text = btn.Text.Replace("OFF", "ON");
                }));
            }
            else
            {
                if (btn.Text.Contains("ON"))
                    btn.Text = btn.Text.Replace("ON", "OFF");
                else
                    btn.Text = btn.Text.Replace("OFF", "ON");
            }
        }

        #endregion

        #region Socket Home Commands

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

        #endregion

        #region Auto Boss Commands

        public void SendBossCommand(Button btn, string command)
        {
            SendCommand(btn, command);
        }

        public void SendFarmNappaCommand(Button btn, int selectedIndex)
        {
            SendCommand(btn, "farmNappa|" + selectedIndex);
        }

        #endregion

        #region Train & Mob Commands

        public void SendTrainMobCommand(Button btn)
        {
            SendCommand(btn, "trainMob");
        }

        public void SendGoBackCommand(Button btn)
        {
            SendCommand(btn, "goBack");
        }

        public void SendGoBackToDo(Button btn)
        {
            SendCommand(btn, "goBackToaDo");
        }

        #endregion

        #region Auto Bo Mong Command

        public void SendBoMongCommand(Button btn, string typeNV, int typeNVGold, bool nextGold, bool nextMob, bool nextHuman)
        {
            string cmd = "BoMong|" + typeNV.ToLower().Trim() + "|" + typeNVGold + "|"
                         + nextGold + "|" + nextMob + "|" + nextHuman;
            SendCommand(btn, cmd);
        }

        #endregion

        #region Pet Commands

        public void SendPetCommand(Button btn, string command)
        {
            SendCommand(btn, command);
        }

        public void SendTTNLCommand(Button btn, int percenHP)
        {
            SendCommand(btn, $"deTTNL|{percenHP}");
        }

        #endregion

        #region Utility Commands

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

        public void SendListVutCommand(Button btn, string vutList)
        {
            SendCommand(btn, "listvut|" + vutList);
        }

        #endregion

        #region Special Commands

        public void SendBoomCommand(Button btn) => SendCommand(btn, "Boom");
        public void SendFindBossCommand(Button btn) => SendCommand(btn, "findBoss");
        public void SendTeleBossCommand(Button btn) => SendCommand(btn, "teleBoss");
        public void SendAttackBossCommand(Button btn) => SendCommand(btn, "acttackBoss");
        public void SendDoBossCommand(Button btn) => SendCommand(btn, "doBoss");
        public void SendAutoWhisCommand(Button btn) => SendCommand(btn, "autoWhis");
        public void SendFindBossTrungMabuCommand(Button btn) => SendCommand(btn, "findBossTrungMabu");

        public void SendBongTaiCommand() { TCPSocket.send("bongtai"); Thread.Sleep(delaySocket); }
        public void SendBatCoDenCommand() { TCPSocket.send("BatCoDen"); Thread.Sleep(delaySocket); }
        public void SendTatCoCommand() { TCPSocket.send("TatCo"); Thread.Sleep(delaySocket); }

        public void SendAutoNeSieuQuaiCommand(Button btn) => SendCommand(btn, "autoNeSieuQuai");
        public void SendTrainAkDameCommand(Button btn) => SendCommand(btn, "trainAkDame");
        public void SendAutoNhatCommand(Button btn) => SendCommand(btn, "AutoNhat");
        public void SendAutoNeBossCommand(Button btn) => SendCommand(btn, "autoNeBoss");
        public void SendAutoHopTheCommand(Button btn) => SendCommand(btn, "autoHopThe");
        public void SendSpamZoneItCommand(Button btn) => SendCommand(btn, "spamZoneIt");
        public void SendAutoZoneItCommand(Button btn) => SendCommand(btn, "autoZoneIt");

        public void SendXinDauCommand(Button btn) => SendCommand(btn, "xinDau");
        public void SendThuDauCommand(Button btn) => SendCommand(btn, "ThuDau");
        public void SendChoDauCommand(Button btn) => SendCommand(btn, "ChoDau");

        #endregion

        #region Event Handlers - Home Chat & Items

        private void OnBtnChatClick()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendChatMessage(form1.txtChatGame.Text);
            form1.txtChatGame.Text = "";
        }

        private void OnButton1Click()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendKhuCommand(form1.txtKHU.Value.ToString());
        }

        private void OnButton2Click()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendItemCommand(form1.txtIdItem.Value.ToString());
        }

        #endregion

        #region Event Handlers - NPC & Teleport

        private void OnBtnTeleNPCClick()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendTeleNpcCommand((int)form1.txtIdNpc.Value);
        }

        #endregion

        #region Event Handlers - Auto Boss

        private void OnButton3Click()
        {
            SendBoomCommand(GetButtonFromForm("button3"));
        }

        private void OnButton4Click()
        {
            SendFindBossCommand(GetButtonFromForm("button4"));
        }

        private void OnButton5Click()
        {
            SendTeleBossCommand(GetButtonFromForm("button5"));
        }

        private void OnButton7Click()
        {
            SendAttackBossCommand(GetButtonFromForm("button7"));
        }

        private void OnButton8Click()
        {
            SendDoBossCommand(GetButtonFromForm("button8"));
        }

        private void OnButton13Click()
        {
            SendAutoWhisCommand(GetButtonFromForm("button13"));
        }

        private void OnButton9Click()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendFarmNappaCommand(GetButtonFromForm("button9"), form1.cbbBossNappa.SelectedIndex);
        }

        private void OnButton14Click()
        {
            SendFindBossTrungMabuCommand(GetButtonFromForm("button14"));
        }

        #endregion

        #region Event Handlers - Train Mob

        private void OnButton6Click()
        {
            SendTrainMobCommand(GetButtonFromForm("button6"));
        }

        private void OnButton16Click()
        {
            SendGoBackCommand(GetButtonFromForm("button16"));
        }

        private void OnButton17Click()
        {
            SendGoBackToDo(GetButtonFromForm("button17"));
        }

        #endregion

        #region Event Handlers - Auto Bo Mong

        private void OnMaterialButton20Click()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendBoMongCommand(GetButtonFromForm("materialButton20"), form1.TypeNV.Text,
                form1.cbbTypeNVGold.SelectedIndex,
                form1.chkNextGold.Checked,
                form1.chkNextMob.Checked,
                form1.chkNextHuman.Checked);
        }

        #endregion

        #region Event Handlers - Auto Pet

        private void OnButton18Click()
        {
            SendPetCommand(GetButtonFromForm("button18"), "deSua");
        }

        private void OnButton19Click()
        {
            SendPetCommand(GetButtonFromForm("button19"), "deKOK");
        }

        private void OnButton20Click()
        {
            SendPetCommand(GetButtonFromForm("button20"), "deCoDen");
        }

        private void OnButton21Click()
        {
            SendPetCommand(GetButtonFromForm("button21"), "deAutoNhat");
        }

        private void OnButton22Click()
        {
            SendPetCommand(GetButtonFromForm("button22"), "deGim");
        }

        private void OnButton23Click()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendTTNLCommand(GetButtonFromForm("button23"), (int)form1.txtPercenHP.Value);
        }

        private void OnMaterialButton131Click()
        {
            SendXinDauCommand(GetButtonFromForm("materialButton131"));
        }

        private void OnMaterialButton132Click()
        {
            SendThuDauCommand(GetButtonFromForm("materialButton132"));
        }

        private void OnMaterialButton133Click()
        {
            SendChoDauCommand(GetButtonFromForm("materialButton133"));
        }

        #endregion

        #region Event Handlers - Utilities

        private void OnBtnReduceCPUClick()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendReduceCPUCommand(GetButtonFromForm("btnReduceCPU"), (int)form1.txtFps.Value);
        }

        private void OnBtnNhapCodeLiveClick()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendCodeLiveCommand(GetButtonFromForm("btnNhapCodeLive"), form1.txtCodeLive.Text);
        }

        private void OnMaterialButton77Click()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendTagNameBossCommand(GetButtonFromForm("materialButton77"), form1.txtTagNameBoss.Text);
        }

        #endregion

        #region Event Handlers - Train Commands

        private void OnBtnNeSieuQuaiClick()
        {
            SendAutoNeSieuQuaiCommand(GetButtonFromForm("btnNeSieuQuai"));
        }

        private void OnBtnAkDameClick()
        {
            SendTrainAkDameCommand(GetButtonFromForm("btnAkDame"));
        }

        private void OnBtnAutoNhatClick()
        {
            SendAutoNhatCommand(GetButtonFromForm("btnAutoNhat"));
        }

        private void OnBtnNeBossClick()
        {
            SendAutoNeBossCommand(GetButtonFromForm("btnNeBoss"));
        }

        private void OnBtnAutoHopTheClick()
        {
            SendAutoHopTheCommand(GetButtonFromForm("btnAutoHopThe"));
        }

        private void OnBtnSpamZoneItClick()
        {
            SendSpamZoneItCommand(GetButtonFromForm("btnSpamZoneIt"));
        }

        private void OnBtnAutoZoneItClick()
        {
            SendAutoZoneItCommand(GetButtonFromForm("btnAutoZoneIt"));
        }

        private void OnBtnApDungVutClick()
        {
            var form1 = form as Form1;
            if (form1 == null) return;
            SendListVutCommand(GetButtonFromForm("btnApDungVut"), form1.txtVut.Text);
        }

        #endregion

        #region Helper Methods

        private Button GetButtonFromForm(string buttonName)
        {
            return form?.Controls[buttonName] as Button;
        }

        #endregion
    }
}