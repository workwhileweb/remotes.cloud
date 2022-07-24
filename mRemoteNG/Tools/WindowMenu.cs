﻿using System;
using System.Drawing;
using mRemoteNG.App;
using Microsoft.Win32.SafeHandles;

// ReSharper disable MemberCanBeMadeStatic.Global

namespace mRemoteNG.Tools
{
    public sealed class WindowMenu : SafeHandleZeroOrMinusOneIsInvalid, IDisposable
    {
        [Flags]
        public enum Flags
        {
            MfString = NativeMethods.MF_STRING,
            MfSeparator = NativeMethods.MF_SEPARATOR,
            MfBycommand = NativeMethods.MF_BYCOMMAND,
            MfByposition = NativeMethods.MF_BYPOSITION,
            MfPopup = NativeMethods.MF_POPUP,
            WmSyscommand = NativeMethods.WM_SYSCOMMAND
        }

        private bool _disposed;
        internal IntPtr SystemMenuHandle;
        private readonly IntPtr _formHandle;

        public WindowMenu(IntPtr handle) : base(true)
        {
            _formHandle = handle;
            SystemMenuHandle = NativeMethods.GetSystemMenu(_formHandle, false);
            SetHandle(SystemMenuHandle);
        }

        public void Reset()
        {
            SystemMenuHandle = NativeMethods.GetSystemMenu(_formHandle, true);
        }

        public void AppendMenuItem(IntPtr parentMenu, Flags flags, IntPtr id, string text)
        {
            NativeMethods.AppendMenu(parentMenu, (int)flags, id, text);
        }

        public IntPtr CreatePopupMenuItem()
        {
            return NativeMethods.CreatePopupMenu();
        }

        public bool InsertMenuItem(IntPtr sysMenu, int position, Flags flags, IntPtr subMenu, string text)
        {
            return NativeMethods.InsertMenu(sysMenu, position, (int)flags, subMenu, text);
        }

        public IntPtr SetBitmap(IntPtr menu, int position, Flags flags, Bitmap bitmap)
        {
            return new IntPtr(Convert.ToInt32(NativeMethods.SetMenuItemBitmaps(menu, position, (int)flags,
                                                                               bitmap.GetHbitmap(),
                                                                               bitmap.GetHbitmap())));
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(SystemMenuHandle);
        }


        /* If we don't have the finalizer, then we get this warning: https://msdn.microsoft.com/library/ms182329.aspx (CA2216: Disposable types should declare finalizer)
         * If we DO have the finalizer, then we get this warning: https://msdn.microsoft.com/library/ms244737.aspx (CA1063: Implement IDisposable correctly)
         * 
         * Since the handle is likely going to be in use for the entierty of the process, the finalizer isn't very important since when we're calling it
         * the process is likely exiting. Leaks would be moot once it exits. CA2216 is the lesser of 2 evils as far as I can tell. Suppress.
        ~SystemMenu()
        {
            Dispose(false);
        }
        */

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2216:DisposableTypesShouldDeclareFinalizer")]
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) return;

            ReleaseHandle();

            _disposed = true;
        }
    }
}