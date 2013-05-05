using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Core.Data
{
    [Serializable()]
    public enum CommandType
    {
        LogIn,
        LogOut,
        Chat,
        SendFileAccept,
        SendFileReject,
        SendFileRequest,
        SendFileProcess,
        ReceiveFileProcess,
        SendFileSuccess,
        ReceiveFileSuccess,
        CheckIP,
        CheckOnline,
        FriendRequest,
        FriendReject,
        FriendAccept
    }
}