﻿using System;
using System.Collections.Generic;

namespace mRemoteNG.Tools.CustomCollections
{
    public class CollectionUpdatedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> ChangedItems { get; }
        public ActionType Action { get; }

        public CollectionUpdatedEventArgs(ActionType action, IEnumerable<T> changedItems)
        {
            Action = action;
            ChangedItems = changedItems ?? throw new ArgumentNullException(nameof(changedItems));
        }
    }

    public enum ActionType
    {
        Added,
        Removed,
        Updated
    }
}