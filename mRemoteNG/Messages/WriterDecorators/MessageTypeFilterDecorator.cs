﻿using System;
using mRemoteNG.Messages.MessageFilteringOptions;
using mRemoteNG.Messages.MessageWriters;

namespace mRemoteNG.Messages.WriterDecorators
{
    public class MessageTypeFilterDecorator : IMessageWriter
    {
        private readonly IMessageTypeFilteringOptions _filter;
        private readonly IMessageWriter _decoratedWriter;

        public MessageTypeFilterDecorator(IMessageTypeFilteringOptions filter, IMessageWriter decoratedWriter)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _decoratedWriter = decoratedWriter ?? throw new ArgumentNullException(nameof(decoratedWriter));
        }

        public void Write(IMessage message)
        {
            if (WeShouldWrite(message))
                _decoratedWriter.Write(message);
        }

        private bool WeShouldWrite(IMessage message)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (message.Class)
            {
                case MessageClass.InformationMsg:
                    if (_filter.AllowInfoMessages) return true;
                    break;
                case MessageClass.WarningMsg:
                    if (_filter.AllowWarningMessages) return true;
                    break;
                case MessageClass.ErrorMsg:
                    if (_filter.AllowErrorMessages) return true;
                    break;
                case MessageClass.DebugMsg:
                    if (_filter.AllowDebugMessages) return true;
                    break;
            }

            return false;
        }
    }
}