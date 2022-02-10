using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WaifuAssistant
{
    public class WinApi
    {
        #region winApi

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll")]
        private static extern long SetWindowLongPtrA(IntPtr hWnd, int nIndex, long dwNewLong);



        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


        [DllImport("user32.dll")]
        static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);


        [DllImport("User32.dll")]
        static extern int SetActiveWindow(IntPtr point);

        [DllImport("User32.dll")]
        static extern int ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("User32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("User32.dll")]
        static extern IntPtr SetFocus(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("Dwmapi.dll")]
        private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

        const int GWL_EXSTYLE = -20;

        const int WS_EX_LAYERED = 0x00080000;
        const int WS_EX_TRANSPARENT = 0x00000020;

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const uint LWA_COLORKEY = 0x00000001;

        private IntPtr hWnd;

        #endregion winApi

        public WinApi(IntPtr ptr)
        {
            this.hWnd = ptr;
        }

        #region winApiFunc
        public async void runInBackground()
        {
            while (false) //true
            {
                /*
                //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //supprime les bordures
                //RECT rect = new RECT();
                //GetWindowRect(GetForegroundWindow(), out rect);

                SetClickthrough(true);
                SetActiveWindow(windowspointer);
                SetForegroundWindow(windowspointer);
                ShowWindowAsync(windowspointer, 2);
                await Task.Delay(10);
                */


                //RECT rect = new RECT();
                //GetWindowRect(GetForegroundWindow(), out rect);
                //SetClickthrough(true);
                await Task.Delay(10);
            }
        }

        public void invisible()
        {
            /*
            hWnd = GetActiveWindow();
            MARGINS margins = new MARGINS { cxLeftWidth = -1 };
            DwmExtendFrameIntoClientArea(hWnd, ref margins);
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
            */
        }

        public void SetClickthrough(bool clickthrough)
        {
            if (clickthrough)
            {
                SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
            }
            else
            {
                SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
            }
        }

        public void MySetActiveWindow()
        {
            SetActiveWindow(hWnd);
        }
        public void MySetForegroundWindow()
        {
            SetForegroundWindow(hWnd);
        }
        public void MyShowWindowAsync(int mode)
        {
            ShowWindowAsync(hWnd, mode);
        }

        public void MySetFocus()
        {
            Console.WriteLine("on met le focus:");
            MyShowWindowAsync(1);
            SetFocus(hWnd);
            SetActiveWindow(hWnd);
            SetForegroundWindow(hWnd);
            //SetFocus(hWnd);
        }
        public IntPtr MyGetActiveWindow()
        {
            return GetActiveWindow();
        }
        public IntPtr MyFindWindow(string windowName)
        {
            return (IntPtr)FindWindow(windowName, null);
        }
        public IntPtr MyGetForegroundWindow()
        {
            return GetForegroundWindow();
        }
        public IntPtr GetWinPointer()
        {
            return this.hWnd;
        }
        #endregion WinApiFunc
    }
}
