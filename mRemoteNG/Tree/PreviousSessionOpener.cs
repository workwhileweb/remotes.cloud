﻿using mRemoteNG.Connection;
using mRemoteNG.Container;
using System;
using System.Linq;
using mRemoteNG.UI.Controls.ConnectionTree;


namespace mRemoteNG.Tree
{
    public class PreviousSessionOpener : IConnectionTreeDelegate
    {
        private readonly IConnectionInitiator _connectionInitiator;

        public PreviousSessionOpener(IConnectionInitiator connectionInitiator)
        {
            _connectionInitiator = connectionInitiator ?? throw new ArgumentNullException(nameof(connectionInitiator));
        }

        public void Execute(IConnectionTree connectionTree)
        {
            var connectionInfoList = connectionTree.GetRootConnectionNode().GetRecursiveChildList()
                                                   .Where(node => !(node is ContainerInfo));
            var previouslyOpenedConnections = connectionInfoList
                .Where(item =>
                           item.PleaseConnect &&
                           //ignore items that have already connected
                           !_connectionInitiator.ActiveConnections.Contains(item.ConstantId));

            foreach (var connectionInfo in previouslyOpenedConnections)
            {
                _connectionInitiator.OpenConnection(connectionInfo);
            }
        }
    }
}