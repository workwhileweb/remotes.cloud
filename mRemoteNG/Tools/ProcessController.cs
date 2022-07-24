using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Text;
using mRemoteNG.App;
using mRemoteNG.Properties;
using mRemoteNG.Tools.Cmdline;

namespace mRemoteNG.Tools
{
    public class ProcessController : IDisposable
    {
        #region Public Methods

        public bool Start(string fileName, CommandLineArguments arguments = null)
        {
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.FileName = fileName;
            if (arguments != null)
                _process.StartInfo.Arguments = arguments.ToString();

            if (!_process.Start())
                return false;
            GetMainWindowHandle();

            return true;
        }

        public bool SetControlVisible(string className, string text, bool visible = true)
        {
            if (_process == null || _process.HasExited)
                return false;
            if (_handle == IntPtr.Zero)
                return false;

            var controlHandle = GetControlHandle(className, text);
            if (controlHandle == IntPtr.Zero)
                return false;

            var nCmdShow = visible ? NativeMethods.SW_SHOW : NativeMethods.SW_HIDE;
            NativeMethods.ShowWindow(controlHandle, (int)nCmdShow);
            return true;
        }

        public bool SetControlText(string className, string oldText, string newText)
        {
            if (_process == null || _process.HasExited || _handle == IntPtr.Zero)
                return false;

            var controlHandle = GetControlHandle(className, oldText);
            if (controlHandle == IntPtr.Zero)
                return false;

            var result = NativeMethods.SendMessage(controlHandle, NativeMethods.WM_SETTEXT, (IntPtr)0,
                                                   new StringBuilder(newText));
            return result.ToInt32() == NativeMethods.TRUE;
        }

        public bool SelectListBoxItem(string itemText)
        {
            if (_process == null || _process.HasExited || _handle == IntPtr.Zero)
                return false;

            var listBoxHandle = GetControlHandle("ListBox");
            if (listBoxHandle == IntPtr.Zero)
                return false;

            var result = NativeMethods.SendMessage(listBoxHandle, NativeMethods.LB_SELECTSTRING, (IntPtr)(-1),
                                                   new StringBuilder(itemText));
            return result.ToInt32() != NativeMethods.LB_ERR;
        }

        public bool ClickButton(string text)
        {
            if (_process == null || _process.HasExited || _handle == IntPtr.Zero)
                return false;

            var buttonHandle = GetControlHandle("Button", text);
            if (buttonHandle == IntPtr.Zero)
                return false;

            var buttonControlId = NativeMethods.GetDlgCtrlID(buttonHandle);
            NativeMethods.SendMessage(_handle, NativeMethods.WM_COMMAND, (IntPtr)buttonControlId, buttonHandle);

            return true;
        }

        public void WaitForExit()
        {
            if (_process == null || _process.HasExited)
                return;
            _process.WaitForExit();
        }

        #endregion

        #region Protected Fields

        private readonly Process _process = new();
        private IntPtr _handle = IntPtr.Zero;
        private List<IntPtr> _controls = new();

        #endregion

        #region Protected Methods

        // ReSharper disable once UnusedMethodReturnValue.Local
        private IntPtr GetMainWindowHandle()
        {
            if (_process == null || _process.HasExited)
                return IntPtr.Zero;

            _process.WaitForInputIdle(OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000);

            _handle = IntPtr.Zero;
            var startTicks = Environment.TickCount;
            while (_handle == IntPtr.Zero &&
                   Environment.TickCount < startTicks + (OptionsAdvancedPage.Default.MaxPuttyWaitTime * 1000))
            {
                _process.Refresh();
                _handle = _process.MainWindowHandle;
                if (_handle == IntPtr.Zero)
                {
                    System.Threading.Thread.Sleep(0);
                }
            }

            return _handle;
        }

        private IntPtr GetControlHandle(string className, string text = "")
        {
            if (_process == null || _process.HasExited || _handle == IntPtr.Zero)
                return IntPtr.Zero;

            if (_controls.Count == 0)
            {
                var windowEnumerator = new EnumWindows();
                _controls = windowEnumerator.EnumChildWindows(_handle);
            }

            var stringBuilder = new StringBuilder();
            var controlHandle = IntPtr.Zero;
            foreach (var control in _controls)
            {
                NativeMethods.GetClassName(control, stringBuilder, stringBuilder.Capacity);
                if (stringBuilder.ToString() != className) continue;
                if (string.IsNullOrEmpty(text))
                {
                    controlHandle = control;
                    break;
                }
                else
                {
                    NativeMethods.SendMessage(control, NativeMethods.WM_GETTEXT, new IntPtr(stringBuilder.Capacity), stringBuilder);
                    if (stringBuilder.ToString() != text) continue;
                    controlHandle = control;
                    break;
                }
            }

            return controlHandle;
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            if(_process != null)
                _process.Dispose();

            _handle = IntPtr.Zero;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}