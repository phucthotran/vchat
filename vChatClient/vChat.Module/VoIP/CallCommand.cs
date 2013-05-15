using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vChat.Module.VoIP
{
    /// <summary>
    /// Thông tin các command được gửi đến một user
    /// </summary>
    public enum CallCommand
    {
        Invite, //Yêu cầu cuộc gọi
        Bye,    //Kết thúc cuộc gọi
        Busy,   //Người dùng đang bận
        OK,     //Phản hồi lại "Yêu cầu cuộc gọi" (Chấp nhận cuộc gọi)
        Null,   //No command
    }
}
