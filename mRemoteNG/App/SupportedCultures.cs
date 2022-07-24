using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using mRemoteNG.Properties;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.App
{
    [Serializable]
    public sealed class SupportedCultures : Dictionary<string, string>
    {
        private static SupportedCultures _instance;

        private static SupportedCultures SingletonInstance
        {
            get { return _instance ?? (_instance = new SupportedCultures()); }
        }


        private SupportedCultures()
        {
            foreach (var cultureName in AppUI.Default.SupportedUICultures.Split(','))
            {
                try
                {
                    var cultureInfo = new CultureInfo(cultureName.Trim());
                    Add(cultureInfo.Name, cultureInfo.TextInfo.ToTitleCase(cultureInfo.NativeName));
                }
                catch (Exception ex)
                {
                    Debug.Print(
                                $"An exception occurred while adding the culture {cultureName} to the list of supported cultures. {ex.StackTrace}");
                }
            }
        }

        // fix CA2229 - https://docs.microsoft.com/en-us/visualstudio/code-quality/ca2229-implement-serialization-constructors?view=vs-2017
        private SupportedCultures(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public static bool IsNameSupported(string cultureName)
        {
            return SingletonInstance.ContainsKey(cultureName);
        }

        public static bool IsNativeNameSupported(string cultureNativeName)
        {
            return SingletonInstance.ContainsValue(cultureNativeName);
        }

        public static string get_CultureName(string cultureNativeName)
        {
            var names = new string[SingletonInstance.Count + 1];
            var nativeNames = new string[SingletonInstance.Count + 1];

            SingletonInstance.Keys.CopyTo(names, 0);
            SingletonInstance.Values.CopyTo(nativeNames, 0);

            for (var index = 0; index <= SingletonInstance.Count; index++)
            {
                if (nativeNames[index] == cultureNativeName)
                {
                    return names[index];
                }
            }

            throw (new KeyNotFoundException());
        }

        public static string get_CultureNativeName(string cultureName)
        {
            return SingletonInstance[cultureName];
        }

        public static List<string> CultureNativeNames
        {
            get
            {
                var valueList = new List<string>();
                foreach (var value in SingletonInstance.Values)
                {
                    valueList.Add(value);
                }

                return valueList;
            }
        }
    }
}