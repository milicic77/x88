using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;


namespace Game.Common
{
    class Console
    {
        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        [DllImport("kernel32")]
        private static extern bool FreeConsole();

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(uint stdHandle);

        [DllImport("kernel32")]
        private static extern bool GetConsoleMode(IntPtr hConsole, out uint consoleMode);
        [DllImport("kernel32")]
        private static extern bool SetConsoleMode(IntPtr hConsole, uint consoleMode);

        [DllImport("kernel32.dll")]
        static extern bool ReadConsole(IntPtr hConsoleInput,
            [Out] StringBuilder lpBuffer,
            uint nNumberOfCharsToRead,
            out uint lpNumberOfCharsRead,
            IntPtr lpReserved);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool WriteConsole(IntPtr hConsoleOutput,
            string msg,
            uint msgLength,
            out uint m_CharsWritten,
            int Reserved
            );

        [DllImport("kernel32.dll")]
        private static extern bool FlushConsoleInputBuffer(IntPtr hConsoleInput);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int SetConsoleTextAttribute(IntPtr hConsoleOutput, uint wAttributes);

        public static IntPtr m_StdOutHandle = (IntPtr)(-1); // INVALID_HANDLE_VALUE
        public static IntPtr m_StdInHandle = (IntPtr)(-1);

        //  var
        const int STD_OUTPUT_HANDLE = -11;
        const int STD_INPUT_HANDLE = -10;

        const uint ENABLE_MOUSE_INPUT = 0x0010;

        const uint FOREGROUND_BLUE = 0x0001; // text color contains blue.
        const uint FOREGROUND_GREEN = 0x0002; // text color contains green.
        const uint FOREGROUND_RED = 0x0004; // text color contains red.
        const uint FOREGROUND_INTENSITY = 0x0008; // text color is intensified.
        const uint BACKGROUND_BLUE = 0x0010; // background color contains blue.
        const uint BACKGROUND_GREEN = 0x0020; // background color contains green.
        const uint BACKGROUND_RED = 0x0040; // background color contains red.
        const uint BACKGROUND_INTENSITY = 0x0080; // background color is intensified.


        static private void HandleUnityConsole(string logString, string stackTrace, LogType logType)
        {
            var strMessage = string.Format("{0}\n{1}", logString, stackTrace);

            switch (logType)
            {
                case LogType.Warning:
                    LogWriter.WriteWarning(strMessage);
                    break;
                case LogType.Log:
                    LogWriter.Write(strMessage);
                    break;

                case LogType.Exception:
                case LogType.Assert:

                    Console.ShowErrorColor();
                    LogWriter.Write2(LOG_LEVEL.ERROR, strMessage);
                    Console.RestoreColor();

                    Debug.LogError(strMessage);

                    Debug.Break();
                    System.Diagnostics.Debugger.Break();
                    break;
                case LogType.Error:
                default:
                    Console.ShowErrorColor();
                    LogWriter.Write2(LOG_LEVEL.ERROR, strMessage);
                    Console.RestoreColor();

                    Debug.LogError(strMessage);
                    break;
            }
        }

        static public bool Init()
        {
            // try to open console window in all possible situations (including cross-debugging)
            if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer)
                return false;

            AllocConsole();
            Application.logMessageReceived += HandleUnityConsole;

            m_StdOutHandle = GetStdHandle(unchecked((uint)STD_OUTPUT_HANDLE));
            m_StdInHandle = GetStdHandle(unchecked((uint)STD_INPUT_HANDLE));

            // 允许控制台右键菜单
            uint dwConsoleMode = 0;
            GetConsoleMode(m_StdInHandle, out dwConsoleMode);
            SetConsoleMode(m_StdInHandle, ~ENABLE_MOUSE_INPUT & dwConsoleMode);

            return true;
        }

        static public bool UnInit()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer)
                return false;

            Application.logMessageReceived -= HandleUnityConsole;
            FreeConsole();
            return true;
        }

        static public void ClearConsole()
        {
            UnInit();
            Init();
        }

        public static void Write(string txt)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer)
            {
                UnityEngine.Debug.Log(txt); // 临时
            }
            else
            {
                uint charWritten = 0;
                WriteConsole(m_StdOutHandle, txt, (uint)txt.Length, out charWritten, 0);

                if (!txt.EndsWith("\n") && !txt.EndsWith("\r"))
                    WriteConsole(Console.m_StdOutHandle, "\n", 1, out charWritten, 0);
            }
        }

        public static void SetColor(uint color)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer)
            {
                return;
            }
            else
            {
                SetConsoleTextAttribute(m_StdOutHandle, color);
            }
            return;
        }

        public static void ShowWarningColor()
        {
            SetColor(FOREGROUND_RED | FOREGROUND_GREEN);
        }
        public static void ShowErrorColor()
        {
            SetColor(FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE | FOREGROUND_INTENSITY | BACKGROUND_RED);
        }
        public static void ShowInfoColor()
        {
            SetColor(FOREGROUND_GREEN);
        }
        public static void RestoreColor()
        {
            SetColor(FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        }
    }
}
