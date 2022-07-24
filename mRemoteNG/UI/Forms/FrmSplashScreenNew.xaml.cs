using System;
using mRemoteNG.App.Info;

namespace mRemoteNG.UI.Forms
{
    /// <summary>
    /// Interaction logic for FrmSplashScreenNew.xaml
    /// </summary>
    public partial class FrmSplashScreenNew
    {
        static FrmSplashScreenNew _instance = null;
        public FrmSplashScreenNew()
        {
            InitializeComponent();
            LoadFont();
            lblLogoPartD.Content = $@"v. {GeneralAppInfo.ApplicationVersion}";
        }
        public static FrmSplashScreenNew GetInstance()
        {
            if (_instance == null)
                _instance = new FrmSplashScreenNew();
            return _instance;
        }
        void LoadFont()
        {
            lblLogoPartA.FontFamily = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./UI/Font/#HandelGotDBol");
            lblLogoPartB.FontFamily = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/"), "./UI/Font/#HandelGotDBol");
        }
    }
}
