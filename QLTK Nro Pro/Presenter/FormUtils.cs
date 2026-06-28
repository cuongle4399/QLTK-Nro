using System.Windows.Forms;

namespace QLTK_Nro_Pro.Presenter
{
    public static class FormUtils
    {
        /// <summary>
        /// Tìm kiếm control đệ quy theo tên trong container
        /// </summary>
        public static Control FindControlRecursive(Control parent, string name)
        {
            if (parent == null) return null;
            if (parent.Name == name) return parent;

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
    }
}
