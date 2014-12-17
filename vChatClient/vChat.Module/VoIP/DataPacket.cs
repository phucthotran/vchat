using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Module.VoIP
{
    /// <summary>
    /// Gói thông tin chứa command và tên của người gửi/nhận
    /// </summary>
    public class DataPacket
    {

        #region PROPERTY

        /// <summary>
        /// Lấy/gán tên người gửi/nhận
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Thông tin command được gửi đi hoặc nhận về
        /// </summary>
        public CallCommand Command { get; set; }

        /// <summary>
        /// Khởi tạo gói thông tin rỗng
        /// </summary>
        public DataPacket()
        {
            this.Name = null;
            this.Command = CallCommand.Null;
        }

        #endregion

        #region MAIN METHOD

        /// <summary>
        /// Khởi tạo gói thông tin với dữ liệu đã chuyển sang mảng byte
        /// </summary>
        /// <param name="Data"></param>
        public DataPacket(byte[] Data)
        {
            //Lấy 4 byte đầu tiên chứa thông tin Command (Dựa theo Start Index)
            this.Command = (CallCommand)BitConverter.ToInt32(Data, 0);

            //Lấy 4 byte tiếp theo chứa chiều dài của tên người nhận/gửi (Dựa theo Start Index)
            int nameLen = BitConverter.ToInt32(Data, 4);

            if (nameLen > 0) //Trường hợp tên người nhận/gửi không bị trống thì sẽ lấy tên người gửi/nhận ở 4 byte tiếp theo (Dựa theo Start Index)
                this.Name = Encoding.UTF8.GetString(Data, 8, nameLen);
            else
                this.Name = null;
        }

        /// <summary>
        /// Chuyển gói thông tin sang byte để gửi đi
        /// </summary>
        /// <returns></returns>
        public byte[] ToByte()
        {
            List<Byte> byteData = new List<Byte>(); //Mảng chứa dữ liệu byte dùng cho gửi gói thông tin đi

            //Chuyển thông tin Command sang mảng byte và lưu vào 4 byte đầu trên "byteData" (Start Index: 0)
            byteData.AddRange(BitConverter.GetBytes((int)Command));

            if (Name != null) //Nếu tên không bỏ trống thì thực hiện chuyển thông tin chiều dài của tên sang mảng byte vào lưu vào 4 byte tiếp theo trên "byteData" (Start Index: 4)
                byteData.AddRange(BitConverter.GetBytes(Name.Length));
            else
                byteData.AddRange(BitConverter.GetBytes(0));

            //Chuyển tên sang mảng byte và lưu vào 4 byte tiếp theo trên "byteData" (Start Index: 8)
            if (Name != null)
                byteData.AddRange(Encoding.UTF8.GetBytes(Name));

            return byteData.ToArray();
        }

        #endregion

    }
}
