using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace vChat.Module.VoIP
{
    /// <summary>
    /// Thông tin bộ mã (Codec) dùng cho ghi âm
    /// </summary>
    public class UncompressedPcmCodec : IDisposable
    {
        /// <summary>
        /// Khởi tạo thông tin cho bộ mã (Codec)
        /// </summary>
        public UncompressedPcmCodec()
        {
            this.RecordFormat = new WaveFormat(8000, 16, 1); //Định dạng PCM, 8kHz, 16 bit, không nén
        }
        
        /// <summary>
        /// Tên của bộ mã (Codec)
        /// </summary>
        public string Name { get { return "PCM 8kHz 16 bit uncompressed"; } }
        
        /// <summary>
        /// Định dạng dùng cho ghi âm
        /// </summary>
        public WaveFormat RecordFormat { get; private set; }
        
        //public byte[] Encode(byte[] data, int offset, int length)
        //{
        //    byte[] encoded = new byte[length];
        //    Array.Copy(data, offset, encoded, 0, length);
        //    return encoded;
        //}
        
        //public byte[] Decode(byte[] data, int offset, int length) 
        //{
        //    byte[] decoded = new byte[length];
        //    Array.Copy(data, offset, decoded, 0, length);
        //    return decoded;
        //}
        
        /// <summary>
        /// Tỉ lệ truyền tải mõi giây
        /// </summary>
        public int BitsPerSecond { get { return this.RecordFormat.AverageBytesPerSecond * 8; } }
        
        public void Dispose() { }
        
        //public bool IsAvailable { get { return true; } }
    }
}
