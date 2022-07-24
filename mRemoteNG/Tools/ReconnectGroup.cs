﻿using System;
using System.Drawing;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.Tools
{
    public partial class ReconnectGroup
    {
        public ReconnectGroup()
        {
            InitializeComponent();
        }

        private bool _serverReady;

        public bool ServerReady
        {
            get => _serverReady;
            set
            {
                SetStatusImage(value ? Properties.Resources.HostStatus_On : Properties.Resources.HostStatus_Off);

                _serverReady = value;
            }
        }

        private delegate void SetStatusImageCb(Image img);

        private void SetStatusImage(Image img)
        {
            if (pbServerStatus.InvokeRequired)
            {
                var d = new SetStatusImageCb(SetStatusImage);
                ParentForm?.Invoke(d, new object[] {img});
            }
            else
            {
                pbServerStatus.Image = img;
            }
        }

        private void chkReconnectWhenReady_CheckedChanged(object sender, EventArgs e)
        {
            _reconnectWhenReady = chkReconnectWhenReady.Checked;
        }

        private bool _reconnectWhenReady;

        public bool ReconnectWhenReady
        {
            get => _reconnectWhenReady;
            set
            {
                _reconnectWhenReady = value;
                SetCheckbox(value);
            }
        }

        private delegate void SetCheckboxCb(bool val);

        private void SetCheckbox(bool val)
        {
            if (chkReconnectWhenReady.InvokeRequired)
            {
                var d = new SetCheckboxCb(SetCheckbox);
                ParentForm?.Invoke(d, new object[] {val});
            }
            else
            {
                chkReconnectWhenReady.Checked = val;
            }
        }

        public delegate void CloseClickedEventHandler();

        private CloseClickedEventHandler _closeClickedEvent;

        public event CloseClickedEventHandler CloseClicked
        {
            add => _closeClickedEvent = (CloseClickedEventHandler)Delegate.Combine(_closeClickedEvent, value);
            remove => _closeClickedEvent = (CloseClickedEventHandler)Delegate.Remove(_closeClickedEvent, value);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            _closeClickedEvent?.Invoke();
        }

        private void tmrAnimation_Tick(object sender, EventArgs e)
        {
            switch (lblAnimation.Text)
            {
                case "":
                    lblAnimation.Text = "»";
                    break;
                case "»":
                    lblAnimation.Text = "»»";
                    break;
                case "»»":
                    lblAnimation.Text = "»»»";
                    break;
                case "»»»":
                    lblAnimation.Text = "";
                    break;
            }
        }

        private delegate void DisposeReconnectGroupCb();

        public void DisposeReconnectGroup()
        {
            if (InvokeRequired)
            {
                var d = new DisposeReconnectGroupCb(DisposeReconnectGroup);
                ParentForm?.Invoke(d);
            }
            else
            {
                Dispose();
            }
        }

        public void ReconnectGroup_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            grpAutomaticReconnect.Text = Language.GroupboxAutomaticReconnect;
            btnClose.Text = Language._Close;
            lblServerStatus.Text = Language.ServerStatus;
            chkReconnectWhenReady.Text = Language.CheckboxReconnectWhenReady;
        }
    }
}