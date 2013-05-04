using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Module.VoIP
{
    public class RecordEventArgs
    {
        public byte[] RecordedData { get; set; }
        public int BytesRecorded { get; set; }

        public RecordEventArgs(byte[] RecordedData, int BytesRecorded)
        {
            this.RecordedData = RecordedData;
            this.BytesRecorded = BytesRecorded;
        }
    }
}
