using System;
using System.Security;

namespace Game.Common
{
    public class ExceptionTool
    {
        /** 抛异常
         */
        public static void ThrowException(string errMsg)
        {
            //throw new Exception(errMsg);
            Exception tmp = new Exception(errMsg);
            LogWriter.WriteWarning(tmp.Message + "  " + tmp.GetType().ToString());
            throw tmp;
        }

        /** 处理异常，记录下详细出错信息
         * 
         */
        public static void ProcessException(Exception e)
        {
            LogWriter.WriteException(e);
            //SubmitException(e);
        }

        /** 往堆栈的上一级函数传递异常
         */
        public static void SubmitException(Exception e)
        {
            throw e;
        }
        public static void Assert(bool expr)
        {
            if (!expr)
                throw new Exception();
        }
    }
}