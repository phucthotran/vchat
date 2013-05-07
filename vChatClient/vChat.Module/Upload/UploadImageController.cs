using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model;
using vChat.Service.UserService;

namespace vChat.Module.Upload
{
    public partial class UploadImage
    {
        public bool ChangeProfilePicture(int UserID, byte[] ImageBytes)
        {
            MethodInvokeResult result = this.Get<UserServiceClient>().ChangeAvatar(UserID, ImageBytes);

            Helper.ShowMessage(result);

            if (result.Status == MethodInvokeResult.RESULT.SUCCESS)
                return true;

            return false;
        }
    }
}
