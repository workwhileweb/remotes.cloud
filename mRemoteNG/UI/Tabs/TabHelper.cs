using mRemoteNG.App;
using mRemoteNG.UI.Window;
using System;

namespace mRemoteNG.UI.Tabs
{
    class TabHelper
    {
        private static readonly Lazy<TabHelper> LazyHelper = new(() => new TabHelper());

        public static TabHelper Instance => LazyHelper.Value;

        private TabHelper()
        {
        }

        private ConnectionTab _currentTab;

        public ConnectionTab CurrentTab
        {
            get => _currentTab;
            set
            {
                _currentTab = value;
                FindCurrentPanel();
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                    "Tab got focused: " + _currentTab.TabText);
            }
        }

        private void FindCurrentPanel()
        {
            var currentForm = _currentTab.Parent;
            while (currentForm != null && !(currentForm is ConnectionWindow))
            {
                currentForm = currentForm.Parent;
            }

            if (currentForm != null)
                CurrentPanel = (ConnectionWindow)currentForm;
        }

        private ConnectionWindow _currentPanel;

        public ConnectionWindow CurrentPanel
        {
            get => _currentPanel;
            set
            {
                _currentPanel = value;
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                                                    "Panel got focused: " + _currentPanel.TabText);
            }
        }
    }
}