namespace mRemoteNG.Themes
{
    using System.Drawing;
    using UI.Tabs;
    using WeifenLuo.WinFormsUI.Docking;
    using WeifenLuo.WinFormsUI.ThemeVS2015;


    /// <summary>
    /// Visual Studio 2015 Light theme.
    /// </summary>
    public class MremoteNgThemeBase : VS2015ThemeBase
    {
        public MremoteNgThemeBase(byte[] themeResource)
            : base(themeResource)
        {
            Measures.SplitterSize = 3;
            Measures.AutoHideSplitterSize = 3;
            Measures.DockPadding = 2;
            ShowAutoHideContentOnHover = false;
        }
    }

    public class MremoteDockPaneStripFactory : DockPanelExtender.IDockPaneStripFactory
    {
        public DockPaneStripBase CreateDockPaneStrip(DockPane pane)
        {
            return new DockPaneStripNg(pane);
        }
    }

    public class MremoteFloatWindowFactory : DockPanelExtender.IFloatWindowFactory
    {
        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
        {
            var activeDocumentBounds = (dockPanel?.ActiveDocument as ConnectionTab)?.Bounds;

            return new FloatWindowNg(dockPanel, pane, activeDocumentBounds ?? bounds);
        }

        public FloatWindow CreateFloatWindow(DockPanel dockPanel, DockPane pane)
        {
            return new FloatWindowNg(dockPanel, pane);
        }
    }
}