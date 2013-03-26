using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using vChat.Lib;
using vChat.Lib.Serialize;

namespace vChat.Model
{
    [DataContract]
    public class MethodInvokeResult
    {
        [DataContract]
        public enum RESULT { [EnumMember] SUCCESS, [EnumMember] FAIL, [EnumMember] UNHANDLE_ERROR, [EnumMember] INPUT_ERROR }

        [DataMember]
        public virtual RESULT Status { get; set; }

        [DataMember]
        public virtual List<String> Errors { get; set; }

        [DataMember]
        public virtual String Message { get; set; }

        [DataMember]
        public virtual ExceptionInfo Exception { get; set; }
    }

    [DataContract]
    public class ExceptionInfo
    {
        [DataMember]
        public String Message { get; private set; }

        [DataMember]
        public String StackTrace { get; private set; }

        [DataMember]
        public String Source { get; private set; }

        [DataMember]
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