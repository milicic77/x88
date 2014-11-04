using System;
using UnityEngine;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Game.Common
{
    public enum LOG_LEVEL
    {
        DEBUG,
        INFO,
        WARN, // warning
        ERROR,
    }
    public class LogWriter
    {
        [DllImport("kernel32")]
        static extern uint GetCurrentThreadId();


        public static void Write(string format, params object[] objs)
        {
            Write2(LOG_LEVEL.DEBUG, format, objs);
        }

        public static void Write2(LOG_LEVEL errorLevel, string format, params object[] objs)
        {
            uint threadId;
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                threadId = GetCurrentThreadId();
            else
                threadId = (uint)System.Threading.Thread.CurrentThread.ManagedThreadId;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(
                "{0:yyyyMMdd-HHmmss}<{1,-5}:{2:X8}>: ",
                DateTime.Now, errorLevel, threadId
                );

            sb.AppendFormat(format, objs);
            sb.Append("\n");

            Console.Write(sb.ToString());
        }

        public static void WriteException(Exception e)
        {
            Console.ShowErrorColor();
            Write2(LOG_LEVEL.ERROR, e.ToString());
            Console.RestoreColor();
        }

        public static void WriteError(string format, params object[] objs)
        {
            Console.ShowErrorColor();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(format, objs);

            // 调用栈
            StackTrace st = new StackTrace(true);

            foreach (var frame in st.GetFrames())
            {
                sb.AppendFormat(
                    "\n    {0} at {1}:{2}",
                    frame.GetMethod().ToString(),
                    frame.GetFileName(),
                    frame.GetFileLineNumber()
                    );
            }
            Write2(LOG_LEVEL.ERROR, sb.ToString());

            Console.RestoreColor();
        }

        public static void WriteWarning(string format, params object[] objs)
        {
            Console.ShowErrorColor(); // 统一警告色
            Write2(LOG_LEVEL.WARN, format, objs);
            Console.RestoreColor();
        }
    }
}
