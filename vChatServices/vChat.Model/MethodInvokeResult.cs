using System;
using System.Runtime.Serialization;
using vChat.Lib;
using vChat.Lib.Serialize;

namespace vChat.Model
{
    [Serializable]
    public class MethodInvokeResult
    {
        public enum RESULT { SUCCESS, FAIL, UNHANDLE_ERROR, INPUT_ERROR }

        public virtual RESULT Status { get; set; }

        public virtual String Message { get; set; }

        public virtual ExceptionInfo Exception { get; set; }
    }

    [Serializable]
    public class ExceptionInfo
    {
        public String Message { get; private set; }

        public String StackTrace { get; private set; }

        public String Source { get; private set; }

        public String ExceptionType { get; private set; }

        public ExceptionInfo(Exception ex)
        {
            Message = ex.Message;
            StackTrace = ex.StackTrace;
            Source = ex.Source;
            ExceptionType = ex.GetType().ToString();
        }
    }
}