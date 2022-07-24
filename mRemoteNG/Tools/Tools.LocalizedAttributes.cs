using System;
using System.ComponentModel;
using mRemoteNG.Resources.Language;

// ReSharper disable ArrangeAccessorOwnerBody

namespace mRemoteNG.Tools
{
    public class LocalizedAttributes
    {
        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedCategoryAttribute : CategoryAttribute
        {
            private const int MAX_ORDER = 10;
            private int _order;

            public LocalizedCategoryAttribute(string value, int order = 1) : base(value)
            {
                this._order = order > MAX_ORDER ? MAX_ORDER : order;
            }

            protected override string GetLocalizedString(string value)
            {
                var orderPrefix = "";
                for (var x = 0; x <= MAX_ORDER - _order; x++)
                {
                    orderPrefix += Convert.ToString("\t");
                }

                return orderPrefix + Language.ResourceManager.GetString(value);
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameAttribute : DisplayNameAttribute
        {
            private bool _localized;

            public LocalizedDisplayNameAttribute(string value) : base(value)
            {
                _localized = false;
            }

            public override string DisplayName
            {
                get
                {
                    if (!_localized)
                    {
                        _localized = true;
                        DisplayNameValue = Language.ResourceManager.GetString(DisplayNameValue);
                    }

                    return base.DisplayName;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDescriptionAttribute : DescriptionAttribute
        {
            private bool _localized;

            public LocalizedDescriptionAttribute(string value) : base(value)
            {
                _localized = false;
            }

            public override string Description
            {
                get
                {
                    if (!_localized)
                    {
                        _localized = true;
                        DescriptionValue = Language.ResourceManager.GetString(DescriptionValue);
                    }

                    return base.Description;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDefaultValueAttribute : DefaultValueAttribute
        {
            public LocalizedDefaultValueAttribute(string name) : base(Language.ResourceManager.GetString(name))
            {
            }

            // This allows localized attributes in a derived class to override a matching
            // non-localized attribute inherited from its base class
            public override object TypeId => typeof(DefaultValueAttribute);
        }

        #region Special localization - with String.Format

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDisplayNameInheritAttribute : DisplayNameAttribute
        {
            private bool _localized;

            public LocalizedDisplayNameInheritAttribute(string value) : base(value)
            {
                _localized = false;
            }

            public override string DisplayName
            {
                get
                {
                    if (!_localized)
                    {
                        _localized = true;
                        DisplayNameValue = string.Format(Language.FormatInherit,
                                                         Language.ResourceManager.GetString(DisplayNameValue));
                    }

                    return base.DisplayName;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
        public class LocalizedDescriptionInheritAttribute : DescriptionAttribute
        {
            private bool _localized;

            public LocalizedDescriptionInheritAttribute(string value) : base(value)
            {
                _localized = false;
            }

            public override string Description
            {
                get
                {
                    if (!_localized)
                    {
                        _localized = true;
                        DescriptionValue = string.Format(Language.FormatInheritDescription,
                                                         Language.ResourceManager.GetString(DescriptionValue));
                    }

                    return base.Description;
                }
            }
        }

        #endregion
    }
}