using mRemoteNG.App;
using mRemoteNG.Tools;
using System;
using System.Threading;
using System.Windows.Forms;
using mRemoteNG.UI.Tabs;

// ReSharper disable UnusedMember.Local

namespace mRemoteNG.Connection.Protocol
{
    public abstract class ProtocolBase : IDisposable
    {
        #region Private Variables

        private ConnectionTab _connectionTab;
        private InterfaceControl _interfaceControl;
        private ConnectingEventHandler _connectingEvent;
        private ConnectedEventHandler _connectedEvent;
        private DisconnectedEventHandler _disconnectedEvent;
        private ErrorOccuredEventHandler _errorOccuredEvent;
        private ClosingEventHandler _closingEvent;
        private ClosedEventHandler _closedEvent;

        #endregion

        #region Public Properties

        #region Control

        private string Name { get; }

        private ConnectionTab ConnectionTab
        {
            get => _connectionTab;
            set
            {
                _connectionTab = value;
                _connectionTab.ResizeBegin += ResizeBegin;
                _connectionTab.Resize += Resize;
                _connectionTab.ResizeEnd += ResizeEnd;
            }
        }

        public InterfaceControl InterfaceControl
        {
            get => _interfaceControl;
            set
            {
                _interfaceControl = value;

                if (_interfaceControl.Parent is ConnectionTab ct)
                    ConnectionTab = ct;
            }
        }

        protected Control Control { get; set; }

        #endregion

        public ConnectionInfo.Force Force { get; set; }

        public readonly System.Timers.Timer TmrReconnect = new(2000);
        protected ReconnectGroup ReconnectGroup;

        protected ProtocolBase(string name)
        {
            Name = name;
        }

        protected ProtocolBase()
        {
        }

        #endregion

        #region Methods

        //public abstract int GetDefaultPort();

        public virtual void Focus()
        {
            try
            {
                Control.Focus();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Couldn't focus Control (Connection.Protocol.Base)",
                                                                ex);
            }
        }

        public virtual void ResizeBegin(object sender, EventArgs e)
        {
        }

        public virtual void Resize(object sender, EventArgs e)
        {
        }

        public virtual void ResizeEnd(object sender, EventArgs e)
        {
        }

        public virtual bool Initialize()
        {
            try
            {
                _interfaceControl.Parent.Tag = _interfaceControl;
                _interfaceControl.Show();

                if (Control == null)
                    return true;


                Control.Name = Name;
                Control.Location = _interfaceControl.Location;
                Control.Size = InterfaceControl.Size;
                Control.Anchor = _interfaceControl.Anchor;
                _interfaceControl.Controls.Add(Control);

                return true;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Couldn't SetProps (Connection.Protocol.Base)", ex);
                return false;
            }
        }

        public virtual bool Connect()
        {
            if (InterfaceControl.Info.Protocol == ProtocolType.Rdp) return false;
            if (_connectedEvent == null) return false;
            _connectedEvent(this);
            return true;
        }

        public virtual void Disconnect()
        {
            Close();
        }

        public virtual void Close()
        {
            var t = new Thread(CloseBg);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        private void CloseBg()
        {
            _closedEvent?.Invoke(this);
            try
            {
                TmrReconnect.Enabled = false;

                if (Control != null)
                {
                    try
                    {
                        DisposeControl();
                    }
                    catch (Exception ex)
                    {
                        Runtime.MessageCollector?.AddExceptionStackTrace(
                            "Couldn't dispose control, probably form is already closed (Connection.Protocol.Base)", ex);
                    }
                }

                if (_interfaceControl == null) return;

                try
                {
                    if (_interfaceControl.Parent == null) return;

                    if (_interfaceControl.Parent.Tag != null)
                    {
                        SetTagToNothing();
                    }

                    DisposeInterface();
                }
                catch (Exception ex)
                {
                    Runtime.MessageCollector?.AddExceptionStackTrace(
                        "Couldn't set InterfaceControl.Parent.Tag or Dispose Interface, " +
                        "probably form is already closed (Connection.Protocol.Base)", ex);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector?.AddExceptionStackTrace(
                    "Couldn't Close InterfaceControl BG (Connection.Protocol.Base)", ex);
            }
        }

        private delegate void DisposeInterfaceCb();

        private void DisposeInterface()
        {
            if (_interfaceControl.InvokeRequired)
            {
                var s = new DisposeInterfaceCb(DisposeInterface);
                _interfaceControl.Invoke(s);
            }
            else
            {
                _interfaceControl.Dispose();
            }
        }

        private delegate void SetTagToNothingCb();

        private void SetTagToNothing()
        {
            if (_interfaceControl.Parent.InvokeRequired)
            {
                var s = new SetTagToNothingCb(SetTagToNothing);
                _interfaceControl.Parent.Invoke(s);
            }
            else
            {
                _interfaceControl.Parent.Tag = null;
            }
        }

        private delegate void DisposeControlCb();

        private void DisposeControl()
        {
            if (Control.InvokeRequired)
            {
                var s = new DisposeControlCb(DisposeControl);
                Control.Invoke(s);
            }
            else
            {
                Control.Dispose();
            }
        }

        #endregion

        #region Events

        public delegate void ConnectingEventHandler(object sender);

        public event ConnectingEventHandler Connecting
        {
            add => _connectingEvent = (ConnectingEventHandler)Delegate.Combine(_connectingEvent, value);
            remove => _connectingEvent = (ConnectingEventHandler)Delegate.Remove(_connectingEvent, value);
        }

        public delegate void ConnectedEventHandler(object sender);

        public event ConnectedEventHandler Connected
        {
            add => _connectedEvent = (ConnectedEventHandler)Delegate.Combine(_connectedEvent, value);
            remove => _connectedEvent = (ConnectedEventHandler)Delegate.Remove(_connectedEvent, value);
        }

        public delegate void DisconnectedEventHandler(object sender, string disconnectedMessage, int? reasonCode);

        public event DisconnectedEventHandler Disconnected
        {
            add => _disconnectedEvent = (DisconnectedEventHandler)Delegate.Combine(_disconnectedEvent, value);
            remove => _disconnectedEvent = (DisconnectedEventHandler)Delegate.Remove(_disconnectedEvent, value);
        }

        public delegate void ErrorOccuredEventHandler(object sender, string errorMessage, int? errorCode);

        public event ErrorOccuredEventHandler ErrorOccured
        {
            add => _errorOccuredEvent = (ErrorOccuredEventHandler)Delegate.Combine(_errorOccuredEvent, value);
            remove => _errorOccuredEvent = (ErrorOccuredEventHandler)Delegate.Remove(_errorOccuredEvent, value);
        }

        public delegate void ClosingEventHandler(object sender);

        public event ClosingEventHandler Closing
        {
            add => _closingEvent = (ClosingEventHandler)Delegate.Combine(_closingEvent, value);
            remove => _closingEvent = (ClosingEventHandler)Delegate.Remove(_closingEvent, value);
        }

        public delegate void ClosedEventHandler(object sender);

        public event ClosedEventHandler Closed
        {
            add => _closedEvent = (ClosedEventHandler)Delegate.Combine(_closedEvent, value);
            remove => _closedEvent = (ClosedEventHandler)Delegate.Remove(_closedEvent, value);
        }


        public void Event_Closing(object sender)
        {
            _closingEvent?.Invoke(sender);
        }

        protected void Event_Closed(object sender)
        {
            _closedEvent?.Invoke(sender);
        }

        protected void Event_Connecting(object sender)
        {
            _connectingEvent?.Invoke(sender);
        }

        protected void Event_Connected(object sender)
        {
            _connectedEvent?.Invoke(sender);
        }

        protected void Event_Disconnected(object sender, string disconnectedMessage, int? reasonCode)
        {
            _disconnectedEvent?.Invoke(sender, disconnectedMessage, reasonCode);
        }

        protected void Event_ErrorOccured(object sender, string errorMsg, int? errorCode)
        {
            _errorOccuredEvent?.Invoke(sender, errorMsg, errorCode);
        }

        protected void Event_ReconnectGroupCloseClicked()
        {
            Close();
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing) return;
            TmrReconnect?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}