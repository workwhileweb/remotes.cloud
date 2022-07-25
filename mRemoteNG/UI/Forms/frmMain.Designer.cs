﻿namespace mRemoteNG.UI.Forms
{
	public partial class FrmMain : System.Windows.Forms.Form
	{
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
                    if(components != null)
                        components.Dispose();

                    if(_advancedWindowMenu != null)
                        _advancedWindowMenu.Dispose();

                }
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pnlDock = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new mRemoteNG.UI.Menu.FileMenu();
            this.toolsMenu = new mRemoteNG.UI.Menu.ToolsMenu();
            this.helpMenu = new mRemoteNG.UI.Menu.HelpMenu();
            this.mMenSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsContainer = new System.Windows.Forms.ToolStripContainer();
            this.tmrAutoSave = new System.Windows.Forms.Timer(this.components);
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.tsModeAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsModeUser = new System.Windows.Forms.ToolStripMenuItem();
            this.msMain.SuspendLayout();
            this.tsContainer.ContentPanel.SuspendLayout();
            this.tsContainer.TopToolStripPanel.SuspendLayout();
            this.tsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDock
            // 
            this.pnlDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDock.DockBackColor = System.Drawing.SystemColors.Control;
            this.pnlDock.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            this.pnlDock.Location = new System.Drawing.Point(0, 0);
            this.pnlDock.Name = "pnlDock";
            this.pnlDock.Size = new System.Drawing.Size(1129, 529);
            this.pnlDock.TabIndex = 13;
            this.pnlDock.ActiveDocumentChanged += new System.EventHandler(this.pnlDock_ActiveDocumentChanged);
            // 
            // msMain
            // 
            this.msMain.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.msMain.Dock = System.Windows.Forms.DockStyle.None;
            this.msMain.GripMargin = new System.Windows.Forms.Padding(2);
            this.msMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.msMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.toolsMenu,
            this.helpMenu});
            this.msMain.Location = new System.Drawing.Point(6, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.msMain.Size = new System.Drawing.Size(261, 42);
            this.msMain.Stretch = false;
            this.msMain.TabIndex = 0;
            this.msMain.Text = "Main Toolbar";
            // 
            // fileMenu
            // 
            this.fileMenu.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.fileMenu.Name = "mMenFile";
            this.fileMenu.Size = new System.Drawing.Size(71, 36);
            this.fileMenu.Text = "&File";
            this.fileMenu.TreeWindow = null;
            // 
            // toolsMenu
            // 
            this.toolsMenu.CredentialProviderCatalog = null;
            this.toolsMenu.MainForm = null;
            this.toolsMenu.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.toolsMenu.Name = "mMenTools";
            this.toolsMenu.Size = new System.Drawing.Size(89, 36);
            this.toolsMenu.Text = "&Tools";
            // 
            // helpMenu
            // 
            this.helpMenu.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.helpMenu.Name = "mMenInfo";
            this.helpMenu.Size = new System.Drawing.Size(84, 36);
            this.helpMenu.Text = "&Help";
            this.helpMenu.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // mMenSep3
            // 
            this.mMenSep3.Name = "mMenSep3";
            this.mMenSep3.Size = new System.Drawing.Size(211, 6);
            // 
            // tsContainer
            // 
            // 
            // tsContainer.ContentPanel
            // 
            this.tsContainer.ContentPanel.Controls.Add(this.pnlDock);
            this.tsContainer.ContentPanel.Size = new System.Drawing.Size(1129, 529);
            this.tsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsContainer.Location = new System.Drawing.Point(0, 0);
            this.tsContainer.Name = "tsContainer";
            this.tsContainer.Size = new System.Drawing.Size(1129, 571);
            this.tsContainer.TabIndex = 17;
            this.tsContainer.Text = "ToolStripContainer1";
            // 
            // tsContainer.TopToolStripPanel
            // 
            this.tsContainer.TopToolStripPanel.Controls.Add(this.msMain);
            this.tsContainer.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            // 
            // tmrAutoSave
            // 
            this.tmrAutoSave.Interval = 10000;
            this.tmrAutoSave.Tick += new System.EventHandler(this.tmrAutoSave_Tick);
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // tsModeAdmin
            // 
            this.tsModeAdmin.Name = "tsModeAdmin";
            this.tsModeAdmin.Size = new System.Drawing.Size(32, 19);
            // 
            // tsModeUser
            // 
            this.tsModeUser.Checked = true;
            this.tsModeUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsModeUser.Name = "tsModeUser";
            this.tsModeUser.Size = new System.Drawing.Size(180, 22);
            this.tsModeUser.Text = "UserMode";
            this.tsModeUser.Click += new System.EventHandler(this.tsModeUser_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1129, 571);
            this.Controls.Add(this.tsContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "FrmMain";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.ResizeBegin += new System.EventHandler(this.frmMain_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tsContainer.ContentPanel.ResumeLayout(false);
            this.tsContainer.TopToolStripPanel.ResumeLayout(false);
            this.tsContainer.TopToolStripPanel.PerformLayout();
            this.tsContainer.ResumeLayout(false);
            this.tsContainer.PerformLayout();
            this.ResumeLayout(false);

		}
		internal WeifenLuo.WinFormsUI.Docking.DockPanel pnlDock;
		internal System.Windows.Forms.MenuStrip msMain;
		internal System.Windows.Forms.ToolStripContainer tsContainer;
		internal System.Windows.Forms.Timer tmrAutoSave;
		internal System.Windows.Forms.ToolStripSeparator mMenSep3;
        private System.ComponentModel.IContainer components;
        private Menu.FileMenu fileMenu;
        private Menu.ViewMenu viewMenu;
        private Menu.ToolsMenu toolsMenu;
        private Menu.HelpMenu helpMenu;
        internal mRemoteNG.UI.Controls.QuickConnectToolStrip _quickConnectToolStrip;
        internal mRemoteNG.UI.Controls.ExternalToolsToolStrip _externalToolsToolStrip;
		internal mRemoteNG.UI.Controls.MultiSshToolStrip _multiSshToolStrip;
        //theming support
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private System.Windows.Forms.ToolStripMenuItem tsModeAdmin;
        private System.Windows.Forms.ToolStripMenuItem tsModeUser;
    }
}
