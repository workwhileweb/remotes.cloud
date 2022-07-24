using System.ComponentModel;
using System.Drawing;
using BrightIdeasSoftware;
using mRemoteNG.Themes;

namespace mRemoteNG.UI.Controls
{
    //Simple coloring of ObjectListView
    //This is subclassed to avoid repeating the code in multiple places
    internal class MrngListView : ObjectListView
    {
        private CellBorderDecoration _deco;

        //Control if the gridlines are styled, must be set before the OnCreateControl is fired
        public bool DecorateLines { get; set; } = true;

        public MrngListView()
        {
            InitializeComponent();
            ThemeManager.GetInstance().ThemeChanged += OnCreateControl;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            var themeManager = ThemeManager.GetInstance();
            if (!themeManager.ActiveAndExtended) return;
            //List back color
            BackColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("List_Background");
            ForeColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("List_Item_Foreground");
            //Selected item
            SelectedBackColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("List_Item_Selected_Background");
            SelectedForeColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("List_Item_Selected_Foreground");

            //Header style
            HeaderUsesThemes = false;
            var headerStylo = new HeaderFormatStyle
            {
                Normal =
                {
                    BackColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("List_Header_Background"),
                    ForeColor = themeManager.ActiveTheme.ExtendedPalette.GetColor("List_Header_Foreground")
                }
            };
            HeaderFormatStyle = headerStylo;
            //Border style
            if (DecorateLines)
            {
                UseCellFormatEvents = true;
                GridLines = false;
                _deco = new CellBorderDecoration
                {
                    BorderPen = new Pen(themeManager.ActiveTheme.ExtendedPalette.GetColor("List_Item_Border")),
                    FillBrush = null,
                    BoundsPadding = Size.Empty,
                    CornerRounding = 0
                };
                FormatCell += NGListView_FormatCell;
            }

            if (Items != null && Items.Count != 0)
                BuildList();
            Invalidate();
        }

        private void NGListView_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.Column.IsVisible)
            {
                e.SubItem.Decoration = _deco;
            }
        }

        private void InitializeComponent()
        {
            ((ISupportInitialize)(this)).BeginInit();
            SuspendLayout();
            // 
            // NGListView
            // 
            Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ((ISupportInitialize)(this)).EndInit();
            ResumeLayout(false);
        }
    }
}