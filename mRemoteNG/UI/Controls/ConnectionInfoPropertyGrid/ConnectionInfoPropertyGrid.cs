﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using mRemoteNG.App;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using mRemoteNG.Security;
using mRemoteNG.Tools;
using mRemoteNG.Tools.Attributes;
using mRemoteNG.Tree.Root;
using mRemoteNG.Resources.Language;

namespace mRemoteNG.UI.Controls.ConnectionInfoPropertyGrid
{
    public partial class ConnectionInfoPropertyGrid : FilteredPropertyGrid.FilteredPropertyGrid
    {
        private readonly Dictionary<Type, IEnumerable<PropertyInfo>> _propertyCache = new();
        private ConnectionInfo _selectedConnectionInfo;
        private PropertyMode _propertyMode;

        /// <summary>
        /// The <see cref="ConnectionInfo"/> currently being shown by this
        /// property grid.
        /// </summary>
        public ConnectionInfo SelectedConnectionInfo
        {
            get => _selectedConnectionInfo;
            set
            {
                if (_selectedConnectionInfo == value)
                    return;

                _selectedConnectionInfo = value;
                RootNodeSelected = SelectedConnectionInfo is RootNodeInfo;
                SetGridObject();
            }
        }

        /// <summary>
        /// Determines which set of properties this grid will display.
        /// </summary>
        public PropertyMode PropertyMode
        {
            get => _propertyMode;
            set
            {
                if (_propertyMode == value)
                    return;
                _propertyMode = value;
                SetGridObject();
            }
        }

        /// <summary>
        /// Is the property grid showing the selected connection's
        /// inheritance info? If false, the connection's normal
        /// properties are shown instead.
        /// </summary>
        public bool IsShowingInheritance => PropertyMode == PropertyMode.Inheritance ||
                                            PropertyMode == PropertyMode.DefaultInheritance;

        /// <summary>
        /// This indicates whether the current <see cref="SelectedConnectionInfo"/>
        /// is a <see cref="DefaultConnectionInfo"/>.
        /// </summary>
        public bool IsShowingDefaultProperties => PropertyMode == PropertyMode.DefaultConnection ||
                                                  PropertyMode == PropertyMode.DefaultInheritance;

        /// <summary>
        /// True when the <see cref="SelectedConnectionInfo"/> is
        /// of type <see cref="RootNodeInfo"/>.
        /// </summary>
        public bool RootNodeSelected { get; private set; }

        public ConnectionInfoPropertyGrid()
        {
            InitializeComponent();
            PropertyValueChanged += pGrid_PropertyValueChanged;
        }

        private void SetGridObject()
        {
            ClearFilters();

            switch (PropertyMode)
            {
                case PropertyMode.Connection:
                default:
                    SelectedObject = SelectedConnectionInfo;
                    break;
                case PropertyMode.Inheritance:
                    SelectedObject = SelectedConnectionInfo?.Inheritance;
                    break;
                case PropertyMode.DefaultConnection:
                    SelectedObject = DefaultConnectionInfo.Instance;
                    break;
                case PropertyMode.DefaultInheritance:
                    SelectedObject = DefaultConnectionInheritance.Instance;
                    break;
            }

            if (SelectedObject != null)
                ShowHideGridItems();
        }

        private void ShowHideGridItems()
        {
            try
            {
                if (SelectedConnectionInfo == null)
                    return;

                if (RootNodeSelected && PropertyMode == PropertyMode.Connection)
                {
                    BrowsableProperties = SelectedConnectionInfo switch
                    {
                        RootPuttySessionsNodeInfo => new[] {nameof(RootPuttySessionsNodeInfo.Name)},
                        RootNodeInfo => new[] {nameof(RootNodeInfo.Name), nameof(RootNodeInfo.Password)},
                        _ => BrowsableProperties
                    };

                    Refresh();
                    return;
                }

                // set all browsable properties valid for this connection's protocol
                BrowsableProperties =
                    GetPropertiesForGridObject(SelectedObject)
                    .Where(property =>
                        IsValidForProtocol(property, SelectedConnectionInfo.Protocol, IsShowingInheritance))
                    .Select(property => property.Name)
                    .ToArray();

                var strHide = new List<string>();

                if (PropertyMode == PropertyMode.Connection)
                {
                    // hide any inherited properties
                    strHide.AddRange(SelectedConnectionInfo.Inheritance.GetEnabledInheritanceProperties());

                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (SelectedConnectionInfo.Protocol)
                    {
                        case ProtocolType.Rdp:
                            strHide.AddRange(SpecialRdpExclusions());
                            break;
                        case ProtocolType.Vnc:
                            strHide.AddRange(SpecialVncExclusions());
                            break;
                    }

                    if (SelectedConnectionInfo.IsContainer)
                        strHide.Add(nameof(AbstractConnectionRecord.Hostname));

                    if (SelectedConnectionInfo is PuttySessionInfo)
                        strHide.Add(nameof(AbstractConnectionRecord.Favorite));
                }
                else if (PropertyMode == PropertyMode.DefaultConnection)
                {
                    strHide.Add(nameof(AbstractConnectionRecord.Hostname));
                    strHide.Add(nameof(AbstractConnectionRecord.Name));
                }

                HiddenProperties = strHide.ToArray();
                Refresh();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(
                    MessageClass.ErrorMsg,
                    Language.ConfigPropertyGridHideItemsFailed +
                    Environment.NewLine + ex.Message, true);
            }
        }

        private IEnumerable<PropertyInfo> GetPropertiesForGridObject(object currentGridObject)
        {
            if (_propertyCache.TryGetValue(currentGridObject.GetType(), out var properties))
                return properties;

            var type = currentGridObject.GetType();
            var props = type.GetProperties();
            _propertyCache.Add(type, props);

            return props;
        }

        private bool IsValidForProtocol(PropertyInfo property, ProtocolType protocol, bool skipProtocolCheck)
        {
            return
                property.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false &&
                (skipProtocolCheck || property.GetCustomAttribute<AttributeUsedInProtocol>()?
                    .SupportedProtocolTypes
                    .Contains(protocol) != false);
        }

        /// <summary>
        /// 
        /// </summary>
        private List<string> SpecialRdpExclusions()
        {
            var strHide = new List<string>();

            if (SelectedConnectionInfo.RdpMinutesToIdleTimeout <= 0)
            {
                strHide.Add(nameof(AbstractConnectionRecord.RdpAlertIdleTimeout));
            }

            if (SelectedConnectionInfo.RdGatewayUsageMethod == RdGatewayUsageMethod.Never)
            {
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayDomain));
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayHostname));
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayPassword));
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayUseConnectionCredentials));
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayUsername));
            }
            else if (SelectedConnectionInfo.RdGatewayUseConnectionCredentials ==
                     RdGatewayUseConnectionCredentials.Yes)
            {
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayDomain));
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayPassword));
                strHide.Add(nameof(AbstractConnectionRecord.RdGatewayUsername));
            }

            if (!(SelectedConnectionInfo.Resolution == RdpResolutions.FitToWindow ||
                  SelectedConnectionInfo.Resolution == RdpResolutions.Fullscreen))
            {
                strHide.Add(nameof(AbstractConnectionRecord.AutomaticResize));
            }

            if (SelectedConnectionInfo.RedirectSound != RdpSounds.BringToThisComputer)
            {
                strHide.Add(nameof(AbstractConnectionRecord.SoundQuality));
            }

            if (!SelectedConnectionInfo.UseVmId)
            {
                strHide.Add(nameof(AbstractConnectionRecord.VmId));
                strHide.Add(nameof(AbstractConnectionRecord.UseEnhancedMode));
            }

            return strHide;
        }

        private List<string> SpecialVncExclusions()
        {
            var strHide = new List<string>();
            if (SelectedConnectionInfo.VncAuthMode == ProtocolVnc.AuthMode.AuthVnc)
            {
                strHide.Add(nameof(AbstractConnectionRecord.Username));
                strHide.Add(nameof(AbstractConnectionRecord.Domain));
            }

            if (SelectedConnectionInfo.VncProxyType == ProtocolVnc.ProxyType.ProxyNone)
            {
                strHide.Add(nameof(AbstractConnectionRecord.VncProxyIp));
                strHide.Add(nameof(AbstractConnectionRecord.VncProxyPassword));
                strHide.Add(nameof(AbstractConnectionRecord.VncProxyPort));
                strHide.Add(nameof(AbstractConnectionRecord.VncProxyUsername));
            }

            return strHide;
        }

        private void UpdateConnectionInfoNode(PropertyValueChangedEventArgs e)
        {
            if (IsShowingInheritance)
                return;

            if (e.ChangedItem.Label == Language.Protocol)
            {
                SelectedConnectionInfo.SetDefaultPort();
            }
            else if (e.ChangedItem.Label == Language.Name)
            {
                if (Settings.Default.SetHostnameLikeDisplayName)
                {
                    if (!string.IsNullOrEmpty(SelectedConnectionInfo.Name))
                        SelectedConnectionInfo.Hostname = SelectedConnectionInfo.Name;
                }
            }

            if (IsShowingDefaultProperties)
                DefaultConnectionInfo.Instance.SaveTo(Settings.Default, a => "ConDefault" + a);
        }

        private void UpdateRootInfoNode(PropertyValueChangedEventArgs e)
        {
            if (!(SelectedObject is RootNodeInfo rootInfo))
                return;

            if (e.ChangedItem.PropertyDescriptor?.Name != "Password")
                return;

            if (rootInfo.Password)
            {
                var passwordName = OptionsDBsPage.Default.UseSQLServer ? Language.SQLServer.TrimEnd(':') : Path.GetFileName(Runtime.ConnectionsService.GetStartupConnectionFileName());
                var password = MiscTools.PasswordDialog(passwordName);

                // operation cancelled, dont set a password
                if (!password.Any() || password.First().Length == 0)
                {
                    rootInfo.Password = false;
                    return;
                }

                rootInfo.PasswordString = password.First().ConvertToUnsecureString();
            }
            else
            {
                rootInfo.PasswordString = "";
            }
        }

        private void UpdateInheritanceNode()
        {
            if (IsShowingDefaultProperties && IsShowingInheritance)
                DefaultConnectionInheritance.Instance.SaveTo(Settings.Default, a => "InhDefault" + a);
        }

        private void pGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                UpdateConnectionInfoNode(e);
                UpdateRootInfoNode(e);
                UpdateInheritanceNode();
                ShowHideGridItems();
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg,
                    Language.ConfigPropertyGridValueFailed + Environment.NewLine +
                    ex.Message, true);
            }
        }
    }
}
