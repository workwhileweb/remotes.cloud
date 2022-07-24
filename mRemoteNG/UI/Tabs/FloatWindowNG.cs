using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.UI.Tabs
{
    class FloatWindowNg : FloatWindow
    {
        public FloatWindowNg(DockPanel dockPanel, DockPane pane)
            : base(dockPanel, pane)
        {
        }

        public FloatWindowNg(DockPanel dockPanel, DockPane pane, Rectangle bounds)
            : base(dockPanel, pane, bounds)
        {
        }
    }
}