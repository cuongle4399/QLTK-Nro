using QLTK_Nro_Pro.Presenter.Socket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace QLTK_Nro_Pro.Handlers
{
    internal class XmapHandler
    {
        public static void MapButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int mapId)
            {
                NextMap(mapId);
            }
        }

        private static void NextMap(int mapId)
        {
            TCPSocket.send("xmap|" + mapId);
        }
        private static Control FindControlRecursive(Control parent, string name)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Name == name)
                    return ctrl;

                var found = FindControlRecursive(ctrl, name);
                if (found != null)
                    return found;
            }
            return null;
        }

        public static void BindMapButtons(Form form)
        {
            var mapConfig = new Dictionary<string, int>
            {
                { "materialButton1", 44 },
                { "materialButton2", 14 },
                { "materialButton3", 15 },
                { "materialButton6", 16 },
                { "materialButton5", 17 },
                { "materialButton4", 18 },
                { "materialButton9", 20 },
                { "materialButton8", 19 },
                { "materialButton7", 35 },
                { "materialButton12", 36 },
                { "materialButton11", 37 },
                { "materialButton10", 38 },
                { "materialButton15", 52 },
                { "materialButton14", 26 },
                { "materialButton18", 113 },
                { "materialButton13", 129 },
                { "materialButton37", 127 },
                { "materialButton36", 42 },
                { "materialButton35", 0 },
                { "materialButton34", 1 },
                { "materialButton33", 2 },
                { "materialButton32", 3 },
                { "materialButton31", 4 },
                { "materialButton30", 5 },
                { "materialButton29", 6 },
                { "materialButton28", 27 },
                { "materialButton27", 28 },
                { "materialButton26", 29 },
                { "materialButton25", 30 },
                { "materialButton24", 47 },
                { "materialButton23", 24 },
                { "materialButton22", 46 },
                { "materialButton21", 45 },
                { "materialButton54", 43 },
                { "materialButton53", 7 },
                { "materialButton52", 8 },
                { "materialButton51", 9 },
                { "materialButton50", 12 },
                { "materialButton43", 11 },
                { "materialButton49", 13 },
                { "materialButton42", 10 },
                { "materialButton47", 31 },
                { "materialButton48", 32 },
                { "materialButton46", 33 },
                { "materialButton45", 34 },
                { "materialButton44", 25 },
                { "materialButton72", 68 },
                { "materialButton71", 69 },
                { "materialButton70", 70 },
                { "materialButton69", 71 },
                { "materialButton68", 72 },
                { "materialButton67", 64 },
                { "materialButton66", 65 },
                { "materialButton65", 63 },
                { "materialButton64", 66 },
                { "materialButton63", 67 },
                { "materialButton62", 73 },
                { "materialButton61", 74 },
                { "materialButton60", 75 },
                { "materialButton59", 76 },
                { "materialButton58", 77 },
                { "materialButton57", 81 },
                { "materialButton56", 82 },
                { "materialButton55", 83 },
                { "materialButton19", 79 },
                { "materialButton17", 80 },
                { "materialButton126", 131 },
                { "materialButton125", 132 },
                { "materialButton124", 133 },
                { "materialButton90", 102 },
                { "materialButton89", 92 },
                { "materialButton88", 93 },
                { "materialButton87", 94 },
                { "materialButton86", 96 },
                { "materialButton85", 97 },
                { "materialButton84", 98 },
                { "materialButton83", 99 },
                { "materialButton82", 100 },
                { "materialButton81", 103 },
                { "materialButton108", 109 },
                { "materialButton107", 108 },
                { "materialButton106", 107 },
                { "materialButton105", 110 },
                { "materialButton104", 106 },
                { "materialButton103", 105 },
                { "materialButton123", 53 },
                { "materialButton122", 58 },
                { "materialButton121", 59 },
                { "materialButton120", 60 },
                { "materialButton119", 61 },
                { "materialButton118", 62 },
                { "materialButton117", 55 },
                { "materialButton116", 56 },
                { "materialButton115", 54 },
                { "materialButton114", 57 },
                { "materialButton113", 84 },
                { "materialButton16", -99 },
                { "materialButton102", 153 },
                { "materialButton93", 48 },
                { "materialButton94", 154 },
                { "materialButton95", 155 },
                { "materialButton97", 50 },
                { "materialButton96", 166 },
                { "materialButton101", 156 },
                { "materialButton100", 157 },
                { "materialButton99", 158 },
                { "materialButton98", 159 },
                { "materialButton127", 149 },
                { "materialButton112", 147 },
                { "materialButton111", 152 },
                { "materialButton110", 151 },
                { "materialButton109", 148 },
                { "materialButton73", 181 },
                { "materialButton74", 139 },
                { "materialButton75", 140 },
            };

            foreach (var kv in mapConfig)
            {
                var btn = FindControlRecursive(form, kv.Key) as Button;
                if (btn != null)
                {
                    btn.Tag = kv.Value;
                    btn.Click += MapButton_Click;
                }
                else
                {
                    Console.WriteLine($"⚠️ Không tìm thấy {kv.Key}");
                }
            }
        }
    }
}
