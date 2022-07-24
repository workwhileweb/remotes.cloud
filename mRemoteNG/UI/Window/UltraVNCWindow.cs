using System;
using mRemoteNG.App;
using WeifenLuo.WinFormsUI.Docking;
using mRemoteNG.Resources.Language;


namespace mRemoteNG.UI.Window
{
    public class UltraVncWindow : BaseWindow
    {
        #region Form Init

        internal System.Windows.Forms.ToolStrip TsMain;
        internal System.Windows.Forms.Panel PnlContainer;
        internal System.Windows.Forms.ToolStripButton BtnDisconnect;

        private void InitializeComponent()
        {
            var resources =
                new System.ComponentModel.ComponentResourceManager(typeof(UltraVncWindow));
            TsMain = new System.Windows.Forms.ToolStrip();
            BtnDisconnect = new System.Windows.Forms.ToolStripButton();
            PnlContainer = new System.Windows.Forms.Panel();
            TsMain.SuspendLayout();
            SuspendLayout();
            // 
            // tsMain
            // 
            TsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            TsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                BtnDisconnect
            });
            TsMain.Location = new System.Drawing.Point(0, 0);
            TsMain.Name = "TsMain";
            TsMain.Size = new System.Drawing.Size(446, 25);
            TsMain.TabIndex = 0;
            TsMain.Text = "ToolStrip1";
            // 
            // btnDisconnect
            // 
            BtnDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            BtnDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("btnDisconnect.Image")));
            BtnDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            BtnDisconnect.Name = "BtnDisconnect";
            BtnDisconnect.Size = new System.Drawing.Size(70, 22);
            BtnDisconnect.Text = "Disconnect";
            BtnDisconnect.Click += new EventHandler(btnDisconnect_Click);
            // 
            // pnlContainer
            // 
            PnlContainer.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom)
                                                     | System.Windows.Forms.AnchorStyles.Left)
                                                    | System.Windows.Forms.AnchorStyles.Right)));
            PnlContainer.Location = new System.Drawing.Point(0, 27);
            PnlContainer.Name = "PnlContainer";
            PnlContainer.Size = new System.Drawing.Size(446, 335);
            PnlContainer.TabIndex = 1;
            // 
            // UltraVNCWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(446, 362);
            Controls.Add(PnlContainer);
            Controls.Add(TsMain);
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                                                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Name = "UltraVncWindow";
            TabText = "UltraVNC SC";
            Text = "UltraVNC SC";
            Load += new EventHandler(UltraVNCSC_Load);
            TsMain.ResumeLayout(false);
            TsMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        #region Declarations

        //Private WithEvents vnc As AxCSC_ViewerXControl

        #endregion

        #region Public Methods

        public UltraVncWindow()
        {
            WindowType = WindowType.UltraVncsc;
            DockPnl = new DockContent();
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void UltraVNCSC_Load(object sender, EventArgs e)
        {
            ApplyLanguage();

            StartListening();
        }

        private void ApplyLanguage()
        {
            BtnDisconnect.Text = Language.Disconnect;
        }

        private void StartListening()
        {
            try
            {
                //If vnc IsNot Nothing Then
                //    vnc.Dispose()
                //    vnc = Nothing
                //End If

                //vnc = New AxCSC_ViewerXControl()
                //SetupLicense()

                //vnc.Parent = pnlContainer
                //vnc.Dock = DockStyle.Fill
                //vnc.Show()

                //vnc.StretchMode = ViewerX.ScreenStretchMode.SSM_ASPECT
                //vnc.ListeningText = Language.InheritListeningForIncomingVNCConnections & " " & Settings.UVNCSCPort

                //vnc.ListenEx(Settings.UVNCSCPort)
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg,
                                                    "StartListening (UI.Window.UltraVNCSC) failed" +
                                                    Environment.NewLine + ex.Message);
                Close();
            }
        }

#if false
        private void SetupLicense()
		{
			try
			{
				//Dim f As System.Reflection.FieldInfo
				//f = GetType(AxHost).GetField("licenseKey", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
				//f.SetValue(vnc, "{072169039103041044176252035252117103057101225235137221179204110241121074}")
			}
			catch (Exception ex)
			{
				Runtime.MessageCollector.AddMessage(Messages.MessageClass.ErrorMsg, "VNC SetupLicense failed (UI.Window.UltraVNCSC)" + Environment.NewLine + ex.Message, true);
			}
		}
#endif

        //Private Sub vnc_ConnectionAccepted(ByVal sender As Object, ByVal e As AxViewerX._ISmartCodeVNCViewerEvents_ConnectionAcceptedEvent) Handles vnc.ConnectionAccepted
        //    mC.AddMessage(Messages.MessageClass.InformationMsg, e.bstrServerAddress & " is now connected to your UltraVNC SingleClick panel!")
        //End Sub

        //Private Sub vnc_Disconnected(ByVal sender As Object, ByVal e As System.EventArgs) Handles vnc.Disconnected
        //    StartListening()
        //End Sub

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            //vnc.Dispose()
            Dispose();
            Windows.Show(WindowType.UltraVncsc);
        }

        #endregion
    }
}