﻿using System;
using mRemoteNG.Connection;

namespace mRemoteNG.Tree.ClickHandlers
{
    public class OpenConnectionClickHandler : ITreeNodeClickHandler<ConnectionInfo>
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public OpenConnectionClickHandler(IConnectionInitiator connectionInitiator)
        {
            _connectionInitiator = connectionInitiator ?? throw new ArgumentNullException(nameof(connectionInitiator));
        }

        public void Execute(ConnectionInfo clickedNode)
        {
            if (clickedNode == null)
                throw new ArgumentNullException(nameof(clickedNode));
            if (clickedNode.GetTreeNodeType() != TreeNodeType.Connection &&
                clickedNode.GetTreeNodeType() != TreeNodeType.PuttySession) return;
            _connectionInitiator.OpenConnection(clickedNode);
        }
    }
}