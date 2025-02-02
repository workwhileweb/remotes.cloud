﻿using System;
using System.Collections.Generic;
using System.Linq;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.Http;
using mRemoteNG.Connection.Protocol.RDP;
using mRemoteNG.Connection.Protocol.VNC;
using mRemoteNG.Container;
using mRemoteNG.Tree;
using mRemoteNG.Tree.Root;

namespace mRemoteNG.Config.Serializers.ConnectionSerializers.Csv
{
    public class CsvConnectionsDeserializerMremotengFormat : IDeserializer<string, ConnectionTreeModel>
    {
        public ConnectionTreeModel Deserialize(string serializedData)
        {
            var lines = serializedData.Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            var csvHeaders = new List<string>();
            // used to map a connectioninfo to it's parent's GUID
            var parentMapping = new Dictionary<ConnectionInfo, string>();

            for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
            {
                var line = lines[lineNumber].Split(';');
                if (lineNumber == 0)
                    csvHeaders = line.ToList();
                else
                {
                    var connectionInfo = ParseConnectionInfo(csvHeaders, line);
                    parentMapping.Add(connectionInfo, line[csvHeaders.IndexOf("Parent")]);
                }
            }

            var root = CreateTreeStructure(parentMapping);
            var connectionTreeModel = new ConnectionTreeModel();
            connectionTreeModel.AddRootNode(root);
            return connectionTreeModel;
        }

        private RootNodeInfo CreateTreeStructure(Dictionary<ConnectionInfo, string> parentMapping)
        {
            var root = new RootNodeInfo(RootNodeType.Connection);

            foreach (var node in parentMapping)
            {
                // no parent mapped, add to root
                if (string.IsNullOrEmpty(node.Value))
                {
                    root.AddChild(node.Key);
                    continue;
                }

                // search for parent in the list by GUID
                var parent = parentMapping
                             .Keys
                             .OfType<ContainerInfo>()
                             .FirstOrDefault(info => info.ConstantId == node.Value);

                if (parent != null)
                {
                    parent.AddChild(node.Key);
                }
                else
                {
                    root.AddChild(node.Key);
                }
            }

            return root;
        }

        private ConnectionInfo ParseConnectionInfo(IList<string> headers, string[] connectionCsv)
        {
            var nodeType = headers.Contains("NodeType")
                ? (TreeNodeType)Enum.Parse(typeof(TreeNodeType), connectionCsv[headers.IndexOf("NodeType")], true)
                : TreeNodeType.Connection;

            var nodeId = headers.Contains("Id")
                ? connectionCsv[headers.IndexOf("Id")]
                : Guid.NewGuid().ToString();

            var connectionRecord = nodeType == TreeNodeType.Connection
                ? new ConnectionInfo(nodeId)
                : new ContainerInfo(nodeId);

            connectionRecord.Name = headers.Contains("Name")
                ? connectionCsv[headers.IndexOf("Name")]
                : "";

            connectionRecord.Description = headers.Contains("Description")
                ? connectionCsv[headers.IndexOf("Description")]
                : "";

            connectionRecord.Icon = headers.Contains("Icon")
                ? connectionCsv[headers.IndexOf("Icon")]
                : "";

            connectionRecord.Panel = headers.Contains("Panel")
                ? connectionCsv[headers.IndexOf("Panel")]
                : "";

            connectionRecord.Username = headers.Contains("UserViaAPI")
                ? connectionCsv[headers.IndexOf("UserViaAPI")]
                : "";

            connectionRecord.Username = headers.Contains("Username")
                ? connectionCsv[headers.IndexOf("Username")]
                : "";

            connectionRecord.Password = headers.Contains("Password")
                ? connectionCsv[headers.IndexOf("Password")]
                : "";

            connectionRecord.Domain = headers.Contains("Domain")
                ? connectionCsv[headers.IndexOf("Domain")]
                : "";

            connectionRecord.Hostname = headers.Contains("Hostname")
                ? connectionCsv[headers.IndexOf("Hostname")]
                : "";

            connectionRecord.VmId = headers.Contains("VmId")
                ? connectionCsv[headers.IndexOf("VmId")] : "";

            connectionRecord.SshOptions =headers.Contains("SSHOptions")
                ? connectionCsv[headers.IndexOf("SSHOptions")]
                : "";

            connectionRecord.SshTunnelConnectionName = headers.Contains("SSHTunnelConnectionName")
                ? connectionCsv[headers.IndexOf("SSHTunnelConnectionName")]
                : "";

            connectionRecord.PuttySession = headers.Contains("PuttySession")
                ? connectionCsv[headers.IndexOf("PuttySession")]
                : "";

            connectionRecord.LoadBalanceInfo = headers.Contains("LoadBalanceInfo")
                ? connectionCsv[headers.IndexOf("LoadBalanceInfo")]
                : "";

            connectionRecord.OpeningCommand = headers.Contains("OpeningCommand")
                ? connectionCsv[headers.IndexOf("OpeningCommand")]
                : "";

            connectionRecord.PreExtApp = headers.Contains("PreExtApp")
                ? connectionCsv[headers.IndexOf("PreExtApp")]
                : "";

            connectionRecord.PostExtApp =
                headers.Contains("PostExtApp")
                ? connectionCsv[headers.IndexOf("PostExtApp")]
                : "";

            connectionRecord.MacAddress =
                headers.Contains("MacAddress")
                ? connectionCsv[headers.IndexOf("MacAddress")]
                : "";

            connectionRecord.UserField =
                headers.Contains("UserField")
                ? connectionCsv[headers.IndexOf("UserField")]
                : "";

            connectionRecord.ExtApp = headers.Contains("ExtApp")
                ? connectionCsv[headers.IndexOf("ExtApp")] : "";

            connectionRecord.VncProxyUsername = headers.Contains("VNCProxyUsername")
                ? connectionCsv[headers.IndexOf("VNCProxyUsername")]
                : "";

            connectionRecord.VncProxyPassword = headers.Contains("VNCProxyPassword")
                ? connectionCsv[headers.IndexOf("VNCProxyPassword")]
                : "";

            connectionRecord.RdGatewayUsername = headers.Contains("RDGatewayUsername")
                ? connectionCsv[headers.IndexOf("RDGatewayUsername")]
                : "";

            connectionRecord.RdGatewayPassword = headers.Contains("RDGatewayPassword")
                ? connectionCsv[headers.IndexOf("RDGatewayPassword")]
                : "";

            connectionRecord.RdGatewayDomain = headers.Contains("RDGatewayDomain")
                ? connectionCsv[headers.IndexOf("RDGatewayDomain")]
                : "";

            connectionRecord.VncProxyIp = headers.Contains("VNCProxyIP")
                ? connectionCsv[headers.IndexOf("VNCProxyIP")]
                : "";

            connectionRecord.RdGatewayHostname = headers.Contains("RDGatewayHostname")
                ? connectionCsv[headers.IndexOf("RDGatewayHostname")]
                : "";

            connectionRecord.RdpStartProgram = headers.Contains("RDPStartProgram")
                ? connectionCsv[headers.IndexOf("RDPStartProgram")]
                : "";

            connectionRecord.RdpStartProgramWorkDir = headers.Contains("RDPStartProgramWorkDir")
                ? connectionCsv[headers.IndexOf("RDPStartProgramWorkDir")]
                : "";

            if (headers.Contains("Protocol"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Protocol")], out ProtocolType protocolType))
                    connectionRecord.Protocol = protocolType;
            }

            if (headers.Contains("Port"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("Port")], out var port))
                    connectionRecord.Port = port;
            }

            if (headers.Contains("ConnectToConsole"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("ConnectToConsole")], out var useConsoleSession))
                    connectionRecord.UseConsoleSession = useConsoleSession;
            }

            if (headers.Contains("UseCredSsp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseCredSsp")], out var value))
                    connectionRecord.UseCredSsp = value;
            }

            if (headers.Contains("UseRestrictedAdmin"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseRestrictedAdmin")], out var value))
                    connectionRecord.UseRestrictedAdmin = value;
            }
            if (headers.Contains("UseRCG"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseRCG")], out var value))
                    connectionRecord.UseRcg = value;
            }


            if (headers.Contains("UseVmId"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseVmId")], out var value))
                    connectionRecord.UseVmId = value;
            }

            if (headers.Contains("UseEnhancedMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("UseEnhancedMode")], out var value))
                    connectionRecord.UseEnhancedMode = value;
            }

            if (headers.Contains("RenderingEngine"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RenderingEngine")], out HttpBase.RenderingEngine value))
                    connectionRecord.RenderingEngine = value;
            }

            if (headers.Contains("RDPAuthenticationLevel"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDPAuthenticationLevel")], out AuthenticationLevel value))
                    connectionRecord.RdpAuthenticationLevel = value;
            }

            if (headers.Contains("Colors"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Colors")], out RdpColors value))
                    connectionRecord.Colors = value;
            }

            if (headers.Contains("Resolution"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("Resolution")], out RdpResolutions value))
                    connectionRecord.Resolution = value;
            }

            if (headers.Contains("AutomaticResize"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("AutomaticResize")], out var value))
                    connectionRecord.AutomaticResize = value;
            }

            if (headers.Contains("DisplayWallpaper"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayWallpaper")], out var value))
                    connectionRecord.DisplayWallpaper = value;
            }

            if (headers.Contains("DisplayThemes"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisplayThemes")], out var value))
                    connectionRecord.DisplayThemes = value;
            }

            if (headers.Contains("EnableFontSmoothing"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableFontSmoothing")], out var value))
                    connectionRecord.EnableFontSmoothing = value;
            }

            if (headers.Contains("EnableDesktopComposition"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("EnableDesktopComposition")], out var value))
                    connectionRecord.EnableDesktopComposition = value;
            }

            if (headers.Contains("DisableFullWindowDrag"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableFullWindowDrag")], out var value))
                    connectionRecord.DisableFullWindowDrag = value;
            }

            if (headers.Contains("DisableMenuAnimations"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableMenuAnimations")], out var value))
                    connectionRecord.DisableMenuAnimations = value;
            }

            if (headers.Contains("DisableCursorShadow"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableCursorShadow")], out var value))
                    connectionRecord.DisableCursorShadow = value;
            }

            if (headers.Contains("DisableCursorBlinking"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("DisableCursorBlinking")], out var value))
                    connectionRecord.DisableCursorBlinking = value;
            }

            if (headers.Contains("CacheBitmaps"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("CacheBitmaps")], out var value))
                    connectionRecord.CacheBitmaps = value;
            }

            if (headers.Contains("RedirectDiskDrives"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectDiskDrives")], out var value))
                    connectionRecord.RedirectDiskDrives = value;
            }

            if (headers.Contains("RedirectPorts"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPorts")], out var value))
                    connectionRecord.RedirectPorts = value;
            }

            if (headers.Contains("RedirectPrinters"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectPrinters")], out var value))
                    connectionRecord.RedirectPrinters = value;
            }

            if (headers.Contains("RedirectClipboard"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectClipboard")], out var value))
                    connectionRecord.RedirectClipboard = value;
            }

            if (headers.Contains("RedirectSmartCards"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectSmartCards")], out var value))
                    connectionRecord.RedirectSmartCards = value;
            }

            if (headers.Contains("RedirectSound"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RedirectSound")], out RdpSounds value))
                    connectionRecord.RedirectSound = value;
            }

            if (headers.Contains("RedirectAudioCapture"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectAudioCapture")], out var value))
                    connectionRecord.RedirectAudioCapture = value;
            }

            if (headers.Contains("RedirectKeys"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("RedirectKeys")], out var value))
                    connectionRecord.RedirectKeys = value;
            }

            if (headers.Contains("VNCCompression"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCCompression")], out ProtocolVnc.Compression value))
                    connectionRecord.VncCompression = value;
            }

            if (headers.Contains("VNCEncoding"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCEncoding")], out ProtocolVnc.Encoding value))
                    connectionRecord.VncEncoding = value;
            }

            if (headers.Contains("VNCAuthMode"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCAuthMode")], out ProtocolVnc.AuthMode value))
                    connectionRecord.VncAuthMode = value;
            }

            if (headers.Contains("VNCProxyType"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCProxyType")], out ProtocolVnc.ProxyType value))
                    connectionRecord.VncProxyType = value;
            }

            if (headers.Contains("VNCProxyPort"))
            {
                if (int.TryParse(connectionCsv[headers.IndexOf("VNCProxyPort")], out var value))
                    connectionRecord.VncProxyPort = value;
            }

            if (headers.Contains("VNCColors"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCColors")], out ProtocolVnc.Colors value))
                    connectionRecord.VncColors = value;
            }

            if (headers.Contains("VNCSmartSizeMode"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("VNCSmartSizeMode")], out ProtocolVnc.SmartSizeMode value))
                    connectionRecord.VncSmartSizeMode = value;
            }

            if (headers.Contains("VNCViewOnly"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("VNCViewOnly")], out var value))
                    connectionRecord.VncViewOnly = value;
            }

            if (headers.Contains("RDGatewayUsageMethod"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUsageMethod")], out RdGatewayUsageMethod value))
                    connectionRecord.RdGatewayUsageMethod = value;
            }

            if (headers.Contains("RDGatewayUseConnectionCredentials"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RDGatewayUseConnectionCredentials")], out RdGatewayUseConnectionCredentials value))
                    connectionRecord.RdGatewayUseConnectionCredentials = value;
            }

            if (headers.Contains("Favorite"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("Favorite")], out var value))
                    connectionRecord.Favorite = value;
            }

            if (headers.Contains("RdpVersion"))
            {
                if (Enum.TryParse(connectionCsv[headers.IndexOf("RdpVersion")], true, out RdpVersion version))
                    connectionRecord.RdpVersion = version;
            }

            #region Inheritance

            if (headers.Contains("InheritCacheBitmaps"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritCacheBitmaps")], out var value))
                    connectionRecord.Inheritance.CacheBitmaps = value;
            }

            if (headers.Contains("InheritColors"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritColors")], out var value))
                    connectionRecord.Inheritance.Colors = value;
            }

            if (headers.Contains("InheritDescription"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDescription")], out var value))
                    connectionRecord.Inheritance.Description = value;
            }

            if (headers.Contains("InheritDisplayThemes"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayThemes")], out var value))
                    connectionRecord.Inheritance.DisplayThemes = value;
            }

            if (headers.Contains("InheritDisplayWallpaper"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisplayWallpaper")], out var value))
                    connectionRecord.Inheritance.DisplayWallpaper = value;
            }

            if (headers.Contains("InheritEnableFontSmoothing"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableFontSmoothing")], out var value))
                    connectionRecord.Inheritance.EnableFontSmoothing = value;
            }

            if (headers.Contains("InheritEnableDesktopComposition"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritEnableDesktopComposition")], out var value))
                    connectionRecord.Inheritance.EnableDesktopComposition = value;
            }

            if (headers.Contains("InheritDisableFullWindowDrag"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableFullWindowDrag")], out var value))
                    connectionRecord.Inheritance.DisableFullWindowDrag = value;
            }

            if (headers.Contains("InheritDisableMenuAnimations"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableMenuAnimations")], out var value))
                    connectionRecord.Inheritance.DisableMenuAnimations = value;
            }

            if (headers.Contains("InheritDisableCursorShadow"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableCursorShadow")], out var value))
                    connectionRecord.Inheritance.DisableCursorShadow = value;
            }

            if (headers.Contains("InheritDisableCursorBlinking"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDisableCursorBlinking")], out var value))
                    connectionRecord.Inheritance.DisableCursorBlinking = value;
            }

            if (headers.Contains("InheritDomain"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritDomain")], out var value))
                    connectionRecord.Inheritance.Domain = value;
            }

            if (headers.Contains("InheritIcon"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritIcon")], out var value))
                    connectionRecord.Inheritance.Icon = value;
            }

            if (headers.Contains("InheritPanel"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPanel")], out var value))
                    connectionRecord.Inheritance.Panel = value;
            }

            if (headers.Contains("InheritPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPassword")], out var value))
                    connectionRecord.Inheritance.Password = value;
            }

            if (headers.Contains("InheritPort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPort")], out var value))
                    connectionRecord.Inheritance.Port = value;
            }

            if (headers.Contains("InheritProtocol"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritProtocol")], out var value))
                    connectionRecord.Inheritance.Protocol = value;
            }

            if (headers.Contains("InheritSSHTunnelConnectionName"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSSHTunnelConnectionName")], out var value))
                    connectionRecord.Inheritance.SshTunnelConnectionName = value;
            }

            if (headers.Contains("InheritOpeningCommand"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritOpeningCommand")], out var value))
                    connectionRecord.Inheritance.OpeningCommand = value;
            }

            if (headers.Contains("InheritSSHOptions"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSSHOptions")], out var value))
                    connectionRecord.Inheritance.SshOptions = value;
            }

            if (headers.Contains("InheritPuttySession"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPuttySession")], out var value))
                    connectionRecord.Inheritance.PuttySession = value;
            }

            if (headers.Contains("InheritRedirectDiskDrives"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectDiskDrives")], out var value))
                    connectionRecord.Inheritance.RedirectDiskDrives = value;
            }

            if (headers.Contains("InheritRedirectKeys"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectKeys")], out var value))
                    connectionRecord.Inheritance.RedirectKeys = value;
            }

            if (headers.Contains("InheritRedirectPorts"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPorts")], out var value))
                    connectionRecord.Inheritance.RedirectPorts = value;
            }

            if (headers.Contains("InheritRedirectPrinters"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectPrinters")], out var value))
                    connectionRecord.Inheritance.RedirectPrinters = value;
            }

            if (headers.Contains("InheritRedirectClipboard"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectClipboard")], out var value))
                    connectionRecord.Inheritance.RedirectClipboard = value;
            }

            if (headers.Contains("InheritRedirectSmartCards"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSmartCards")], out var value))
                    connectionRecord.Inheritance.RedirectSmartCards = value;
            }

            if (headers.Contains("InheritRedirectSound"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectSound")], out var value))
                    connectionRecord.Inheritance.RedirectSound = value;
            }

            if (headers.Contains("InheritResolution"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritResolution")], out var value))
                    connectionRecord.Inheritance.Resolution = value;
            }

            if (headers.Contains("InheritAutomaticResize"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritAutomaticResize")], out var value))
                    connectionRecord.Inheritance.AutomaticResize = value;
            }

            if (headers.Contains("InheritUseConsoleSession"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseConsoleSession")], out var value))
                    connectionRecord.Inheritance.UseConsoleSession = value;
            }

            if (headers.Contains("InheritUseCredSsp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseCredSsp")], out var value))
                    connectionRecord.Inheritance.UseCredSsp = value;
            }

            if (headers.Contains("InheritUseRestrictedAdmin"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseRestrictedAdmin")], out var value))
                    connectionRecord.Inheritance.UseRestrictedAdmin = value;
            }

            if (headers.Contains("InheritUseRCG"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseRCG")], out var value))
                    connectionRecord.Inheritance.UseRcg = value;
            }


            if (headers.Contains("InheritUseVmId"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseVmId")], out var value))
                    connectionRecord.Inheritance.UseVmId = value;
            }

            if (headers.Contains("InheritUseEnhancedMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUseEnhancedMode")], out var value))
                    connectionRecord.Inheritance.UseEnhancedMode = value;
            }

            if (headers.Contains("InheritRenderingEngine"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRenderingEngine")], out var value))
                    connectionRecord.Inheritance.RenderingEngine = value;
            }

            if (headers.Contains("InheritUserViaAPI"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUserViaAPI")], out var value))
                    connectionRecord.Inheritance.UserViaApi = value;
            }

            if (headers.Contains("InheritUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUsername")], out var value))
                    connectionRecord.Inheritance.Username = value;
            }

            if (headers.Contains("InheritVmId"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVmId")], out var value))
                    connectionRecord.Inheritance.VmId = value;
            }

            if (headers.Contains("InheritRDPAuthenticationLevel"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAuthenticationLevel")], out var value))
                    connectionRecord.Inheritance.RdpAuthenticationLevel = value;
            }

            if (headers.Contains("InheritLoadBalanceInfo"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritLoadBalanceInfo")], out var value))
                    connectionRecord.Inheritance.LoadBalanceInfo = value;
            }

            if (headers.Contains("InheritOpeningCommand"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritOpeningCommand")], out var value))
                    connectionRecord.Inheritance.OpeningCommand = value;
            }

            if (headers.Contains("InheritPreExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPreExtApp")], out var value))
                    connectionRecord.Inheritance.PreExtApp = value;
            }

            if (headers.Contains("InheritPostExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritPostExtApp")], out var value))
                    connectionRecord.Inheritance.PostExtApp = value;
            }

            if (headers.Contains("InheritMacAddress"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritMacAddress")], out var value))
                    connectionRecord.Inheritance.MacAddress = value;
            }

            if (headers.Contains("InheritUserField"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritUserField")], out var value))
                    connectionRecord.Inheritance.UserField = value;
            }

            if (headers.Contains("InheritFavorite"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritFavorite")], out var value))
                    connectionRecord.Inheritance.Favorite = value;
            }

            if (headers.Contains("InheritExtApp"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritExtApp")], out var value))
                    connectionRecord.Inheritance.ExtApp = value;
            }

            if (headers.Contains("InheritVNCCompression"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCCompression")], out var value))
                    connectionRecord.Inheritance.VncCompression = value;
            }

            if (headers.Contains("InheritVNCEncoding"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCEncoding")], out var value))
                    connectionRecord.Inheritance.VncEncoding = value;
            }

            if (headers.Contains("InheritVNCAuthMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCAuthMode")], out var value))
                    connectionRecord.Inheritance.VncAuthMode = value;
            }

            if (headers.Contains("InheritVNCProxyType"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyType")], out var value))
                    connectionRecord.Inheritance.VncProxyType = value;
            }

            if (headers.Contains("InheritVNCProxyIP"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyIP")], out var value))
                    connectionRecord.Inheritance.VncProxyIp = value;
            }

            if (headers.Contains("InheritVNCProxyPort"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPort")], out var value))
                    connectionRecord.Inheritance.VncProxyPort = value;
            }

            if (headers.Contains("InheritVNCProxyUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyUsername")], out var value))
                    connectionRecord.Inheritance.VncProxyUsername = value;
            }

            if (headers.Contains("InheritVNCProxyPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCProxyPassword")], out var value))
                    connectionRecord.Inheritance.VncProxyPassword = value;
            }

            if (headers.Contains("InheritVNCColors"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCColors")], out var value))
                    connectionRecord.Inheritance.VncColors = value;
            }

            if (headers.Contains("InheritVNCSmartSizeMode"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCSmartSizeMode")], out var value))
                    connectionRecord.Inheritance.VncSmartSizeMode = value;
            }

            if (headers.Contains("InheritVNCViewOnly"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritVNCViewOnly")], out var value))
                    connectionRecord.Inheritance.VncViewOnly = value;
            }

            if (headers.Contains("InheritRDGatewayUsageMethod"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsageMethod")], out var value))
                    connectionRecord.Inheritance.RdGatewayUsageMethod = value;
            }

            if (headers.Contains("InheritRDGatewayHostname"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayHostname")], out var value))
                    connectionRecord.Inheritance.RdGatewayHostname = value;
            }

            if (headers.Contains("InheritRDGatewayUseConnectionCredentials"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUseConnectionCredentials")],
                                  out var value))
                    connectionRecord.Inheritance.RdGatewayUseConnectionCredentials = value;
            }

            if (headers.Contains("InheritRDGatewayUsername"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayUsername")], out var value))
                    connectionRecord.Inheritance.RdGatewayUsername = value;
            }

            if (headers.Contains("InheritRDGatewayPassword"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayPassword")], out var value))
                    connectionRecord.Inheritance.RdGatewayPassword = value;
            }

            if (headers.Contains("InheritRDGatewayDomain"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDGatewayDomain")], out var value))
                    connectionRecord.Inheritance.RdGatewayDomain = value;
            }

            if (headers.Contains("InheritRDPAlertIdleTimeout"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPAlertIdleTimeout")], out var value))
                    connectionRecord.Inheritance.RdpAlertIdleTimeout = value;
            }

            if (headers.Contains("InheritRDPMinutesToIdleTimeout"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRDPMinutesToIdleTimeout")], out var value))
                    connectionRecord.Inheritance.RdpMinutesToIdleTimeout = value;
            }

            if (headers.Contains("InheritSoundQuality"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritSoundQuality")], out var value))
                    connectionRecord.Inheritance.SoundQuality = value;
            }

            if (headers.Contains("InheritRedirectAudioCapture"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRedirectAudioCapture")], out var value))
                    connectionRecord.Inheritance.RedirectAudioCapture = value;
            }

            if (headers.Contains("InheritRdpVersion"))
            {
                if (bool.TryParse(connectionCsv[headers.IndexOf("InheritRdpVersion")], out var value))
                    connectionRecord.Inheritance.RdpVersion = value;
            }

            #endregion

            return connectionRecord;
        }
    }
}