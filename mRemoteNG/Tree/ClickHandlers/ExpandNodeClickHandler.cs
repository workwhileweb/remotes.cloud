using System;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.UI.Controls.ConnectionTree;

namespace mRemoteNG.Tree.ClickHandlers
{
    public class ExpandNodeClickHandler : ITreeNodeClickHandler<ConnectionInfo>
    {
        private readonly IConnectionTree _connectionTree;

        public ExpandNodeClickHandler(IConnectionTree connectionTree)
        {
            _connectionTree = connectionTree ?? throw new ArgumentNullException(nameof(connectionTree));
        }

        public void Execute(ConnectionInfo clickedNode)
        {
            if (clickedNode is not ContainerInfo clickedNodeAsContainer) return;
            _connectionTree.ToggleExpansion(clickedNodeAsContainer);
        }
    }
}