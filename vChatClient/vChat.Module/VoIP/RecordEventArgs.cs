using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Module.VoIP
{
    /// <summary>
    /// Chứa các dữ liệu ghi âm
    /// </summary>
    public class RecordEventArgs
    {
        /// <summary>
        /// Dữ liệu âm thanh đã ghi
        /// </summary>
        public byte[] RecordedData { get; set; }

        /// <summary>
        /// Số byte đã ghi âm được
        /// </summary>
        public int BytesRecorded { get; set; }

        /// <summary>
        /// Khởi tạo thông tin đã ghi được
        /// </summary>
        /// <param name="RecordedData">Dữ liệu âm thanh đã ghi</param>
        /// <param name="BytesRecorded">Số byte đã ghi âm được</param>
        public RecordEventArgs(byte[] RecordedData, int BytesRecorded)
        {
            this.RecordedData = RecordedData;
            this.BytesRecorded = BytesRecorded;
        }
    }
}
