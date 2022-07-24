using mRemoteNG.App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using mRemoteNG.Messages;
using mRemoteNG.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace mRemoteNG.Themes
{
    /// <summary>
    /// Main class of the theming component. Centralizes creation, loading and deletion of themes
    /// Implemented as a singleton
    /// </summary>
    public class ThemeManager
    {
        #region Private Variables

        private ThemeInfo _activeTheme;
        private Hashtable _themes;
        private bool _themeActive;
        private static ThemeManager _themeInstance;
        private readonly string _themePath = App.Info.SettingsFileInfo.ThemeFolder;

        #endregion

        #region Constructors

        private ThemeManager()
        {
            LoadThemes();
            SetActive();
            _themeActive = true;
        }

        private void SetActive()
        {
            if (_themes[OptionsThemePage.Default.ThemeName] != null)
                ActiveTheme = (ThemeInfo)_themes[OptionsThemePage.Default.ThemeName];
            else
            {
                ActiveTheme = DefaultTheme;
                if (string.IsNullOrEmpty(OptionsThemePage.Default.ThemeName)) return;

                //too early for logging to be enabled...
                Debug.WriteLine("Detected invalid Theme in settings file. Resetting to default.");
                // if we got here, then there's an invalid theme name in use, so just empty it out...
                OptionsThemePage.Default.ThemeName = "";
                OptionsThemePage.Default.Save();
            }
        }

        #endregion

        #region Public Methods

        public static ThemeManager GetInstance()
        {
            return _themeInstance ?? (_themeInstance = new ThemeManager());
        }


        public ThemeInfo GetTheme(string themeName)
        {
            if (_themes[themeName] != null)
                return (ThemeInfo)_themes[themeName];
            return null;
        }

        private bool ThemeDirExists()
        {
            //Load the files in theme folder first, to include vstheme light as default
            if (_themePath == null) return false;
            try
            {
                //In install mode first time is necessary to copy the themes folder
                if (!Directory.Exists(_themePath))
                {
                    Directory.CreateDirectory(_themePath);
                }

                var orig = new DirectoryInfo(App.Info.SettingsFileInfo.InstalledThemeFolder);
                var files = orig.GetFiles();
                foreach (var file in files)
                {
                    if (!File.Exists(Path.Combine(_themePath, file.Name)))
                        file.CopyTo(Path.Combine(_themePath, file.Name), true);
                }

                return Directory.Exists(_themePath);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error loading theme directory", ex);
            }

            return false;
        }

        private ThemeInfo LoadDefaultTheme()
        {
            try
            {
                if (ThemeDirExists())
                {
                    var defaultThemeUrl = $"{_themePath}\\vs2015light.vstheme";

                    if (!File.Exists($"{_themePath}\\vs2015light.vstheme"))
                    {
                        Runtime.MessageCollector.AddMessage(MessageClass.ErrorMsg, "Could not find default theme file.",
                                                            true);
                        return null;
                    }

                    //First we load the default base theme, its vs2015lightNG
                    //the true "default" in DockPanelSuite built-in VS2015LightTheme named "vs2015Light"
                    //hence the *NG suffix for this one...
                    var defaultTheme = ThemeSerializer.LoadFromXmlFile(defaultThemeUrl);
                    defaultTheme.Name = $"{defaultTheme.Name}NG";
                    return defaultTheme;
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error loading default theme", ex);
            }

            return null;
        }

        //The manager precharges all the themes at once
        public List<ThemeInfo> LoadThemes()
        {
            if (_themes != null) return _themes.Values.OfType<ThemeInfo>().ToList();
            _themes = new Hashtable();

            if (_themePath == null) return _themes.Values.OfType<ThemeInfo>().ToList();
            try
            {
                //Check that theme folder exist before trying to load themes
                if (ThemeDirExists())
                {
                    var themeFiles = Directory.GetFiles(_themePath, "*.vstheme");

                    //First we load the default base theme, its vs2015lightNG
                    var defaultTheme = LoadDefaultTheme();
                    _themes.Add(defaultTheme.Name, defaultTheme);
                    //Then the rest
                    foreach (var themeFile in themeFiles)
                    {
                        // Skip the default theme here, since it will get loaded again without the *NG below...
                        if (themeFile.Contains("vs2015light.vstheme")) continue;
                        //filter default one
                        var extTheme = ThemeSerializer.LoadFromXmlFile(themeFile, defaultTheme);
                        if (extTheme.Theme == null || _themes.ContainsKey(extTheme.Name)) continue;

                        if (extTheme.Name.Equals("darcula") || extTheme.Name.Equals("vs2015blue") ||
                            extTheme.Name.Equals("vs2015dark"))
                            extTheme.Name = $"{extTheme.Name}NG";

                        _themes.Add(extTheme.Name, extTheme);
                    }

                    //Load the embedded themes, extended palettes are taken from the vs2015 themes, trying to match the color theme

                    // 2012
                    var vs2012Light = new ThemeInfo("vs2012Light", new VS2012LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)_themes["vs2015lightNG"]).ExtendedPalette);
                    _themes.Add(vs2012Light.Name, vs2012Light);
                    var vs2012Dark = new ThemeInfo("vs2012Dark", new VS2012DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)_themes["vs2015darkNG"]).ExtendedPalette);
                    _themes.Add(vs2012Dark.Name, vs2012Dark);
                    var vs2012Blue = new ThemeInfo("vs2012Blue", new VS2012BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2012, ((ThemeInfo)_themes["vs2015blueNG"]).ExtendedPalette);
                    _themes.Add(vs2012Blue.Name, vs2012Blue);

                    // 2013
                    var vs2013Light = new ThemeInfo("vs2013Light", new VS2013LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)_themes["vs2015lightNG"]).ExtendedPalette);
                    _themes.Add(vs2013Light.Name, vs2013Light);
                    var vs2013Dark = new ThemeInfo("vs2013Dark", new VS2013DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)_themes["vs2015darkNG"]).ExtendedPalette);
                    _themes.Add(vs2013Dark.Name, vs2013Dark);
                    var vs2013Blue = new ThemeInfo("vs2013Blue", new VS2013BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2013, ((ThemeInfo)_themes["vs2015blueNG"]).ExtendedPalette);
                    _themes.Add(vs2013Blue.Name, vs2013Blue);

                    // 2015
                    var vs2015Light = new ThemeInfo("vs2015Light", new VS2015LightTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, ((ThemeInfo)_themes["vs2015lightNG"]).ExtendedPalette);
                    _themes.Add(vs2015Light.Name, vs2015Light);
                    var vs2015Dark = new ThemeInfo("vs2015Dark", new VS2015DarkTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, ((ThemeInfo)_themes["vs2015darkNG"]).ExtendedPalette);
                    _themes.Add(vs2015Dark.Name, vs2015Dark);
                    var vs2015Blue = new ThemeInfo("vs2015Blue", new VS2015BlueTheme(), "", VisualStudioToolStripExtender.VsVersion.Vs2015, ((ThemeInfo)_themes["vs2015blueNG"]).ExtendedPalette);
                    _themes.Add(vs2015Blue.Name, vs2015Blue);
                }
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddExceptionStackTrace("Error loading themes", ex);
            }

            return _themes.Values.OfType<ThemeInfo>().ToList();
        }

        /// <summary>
        /// Add a new theme based on an existing one by cloning and renaming, the theme is saved to disk
        /// </summary>
        /// <param name="baseTheme"></param>
        /// <param name="newThemeName"></param>
        /// <returns></returns>
        public ThemeInfo AddTheme(ThemeInfo baseTheme, string newThemeName)
        {
            if (_themes.Contains(newThemeName)) return null;
            var modifiedTheme = (ThemeInfo)baseTheme.Clone();
            modifiedTheme.Name = newThemeName;
            modifiedTheme.IsExtendable = true;
            modifiedTheme.IsThemeBase = false;
            ThemeSerializer.SaveToXmlFile(modifiedTheme, baseTheme);
            _themes.Add(newThemeName, modifiedTheme);
            return modifiedTheme;
        }

        //Delete a theme from memory and disk
        public void DeleteTheme(ThemeInfo themeToDelete)
        {
            if (!_themes.Contains(themeToDelete.Name)) return;
            if (ActiveTheme == themeToDelete)
                ActiveTheme = DefaultTheme;
            _themes.Remove(themeToDelete.Name);
            ThemeSerializer.DeleteFile(themeToDelete);
        }

        //Synchronize the theme XML values from memory to disk
        public void UpdateTheme(ThemeInfo themeToUpdate)
        {
            ThemeSerializer.UpdateThemeXmlValues(themeToUpdate);
        }

        //refresh the ui controls to reflect a theme change
        public void RefreshUi()
        {
            NotifyThemeChanged(this, new PropertyChangedEventArgs(""));
        }

        //Verify if theme name is repeated or if the name is a valid file  name
        public bool IsThemeNameOk(string name)
        {
            if (_themes.Contains(name))
                return false;
            var badChars = Path.GetInvalidFileNameChars();
            return name.IndexOfAny(badChars) == -1;
        }

        #endregion

        #region Events

        public delegate void ThemeChangedEventHandler();

        private ThemeChangedEventHandler _themeChangedEvent;

        public event ThemeChangedEventHandler ThemeChanged
        {
            add => _themeChangedEvent = (ThemeChangedEventHandler)Delegate.Combine(_themeChangedEvent, value);
            remove => _themeChangedEvent = (ThemeChangedEventHandler)Delegate.Remove(_themeChangedEvent, value);
        }

        // ReSharper disable once UnusedParameter.Local
        private void NotifyThemeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                return;
            }

            _themeChangedEvent?.Invoke();
        }

        #endregion

        #region Properties

        public bool ThemingActive
        {
            get => _themeActive;
            set
            {
                if (_themes.Count == 0) return;
                _themeActive = value;
                OptionsThemePage.Default.ThemingActive = value;
                NotifyThemeChanged(this, new PropertyChangedEventArgs(""));
            }
        }

        public ThemeInfo DefaultTheme =>
            _themes != null && ThemesCount > 0
                ? (ThemeInfo)_themes["vs2015Light"]
                : new ThemeInfo("vs2015Light", new VS2015LightTheme(), "",
                                VisualStudioToolStripExtender.VsVersion.Vs2015);

        public ThemeInfo ActiveTheme
        {
            // default if themes are not enabled
            get => ThemingActive == false ? DefaultTheme : _activeTheme;
            set
            {
                // You can only enable theming if there are themes loaded
                // Default accordingly...
                if (value == null)
                {
                    var changed = !OptionsThemePage.Default.ThemeName.Equals(DefaultTheme.Name);

                    OptionsThemePage.Default.ThemeName = DefaultTheme.Name;
                    _activeTheme = DefaultTheme;

                    if (changed)
                        NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));

                    OptionsThemePage.Default.Save();
                    return;
                }

                _activeTheme = value;
                OptionsThemePage.Default.ThemeName = value.Name;
                NotifyThemeChanged(this, new PropertyChangedEventArgs("theme"));
            }
        }

        public bool ActiveAndExtended => ThemingActive && ActiveTheme.IsExtended;

        public int ThemesCount => _themes.Count;

        #endregion
    }
}