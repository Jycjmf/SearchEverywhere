using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace SearchEverywhere.Utility
{
    public class HotKeyUtility
    {
        [Flags]
        public enum Modifiers
        {
            NoMod = 0x0000,
            Alt = 0x0001,
            Ctrl = 0x0002,
            Shift = 0x0004,
            Win = 0x0008
        }

        private const int MYACTION_HOTKEY_ID = 1;
        private static readonly Timer timer = new Timer(TimeElapse, null, 0, Timeout.Infinite);
        private static int counter;

        public HotKeyUtility()
        {
            if (Application.Current.MainWindow != null)
            {
                var source = PresentationSource.FromVisual(Application.Current.MainWindow) as HwndSource;
                if (source == null)
                    throw new Exception("Could not create hWnd source from window.");
                source.AddHook(WndProc);
            }

            if (Application.Current.MainWindow != null)
                RegisterHotKey(new WindowInteropHelper(Application.Current.MainWindow).Handle, MYACTION_HOTKEY_ID,
                    (int) Modifiers.Shift, 0);
        }


        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0312)
            {
                if (counter == 0)
                {
                    counter++;
                    timer.Change(500, Timeout.Infinite);
                }
                else
                {
                    counter++;
                }
            }

            return IntPtr.Zero;
        }

        private static void TimeElapse(object state)
        {
            if (counter >= 2) MainWindow.ChangeWindowStateAction(true);
            counter = 0;
        }
    }
}