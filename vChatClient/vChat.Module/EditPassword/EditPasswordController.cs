using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using vChat.Service.UserService;
using Core.Client;
using vChat.Model;

namespace vChat.Module.EditPassword
{
    public class EditPasswordMetadata
    {
        public string PassOld { get; set; }
        public string PassNew { get; set; }
        public string PassNewAgain { get; set; }
        public EditPasswordMetadata(string PassOld, string PassNew, string PassNewAgain)
        {
            this.PassOld = PassOld;
            this.PassNew = PassNew;
            this.PassNewAgain = PassNewAgain;
        }
    }

    public partial class EditPassword : UserControl
    {
        #region Validate Box
        private string validatePassOld(string value)
        {
            if (this.Get<UserServiceClient>().Login(this.Get<Client>().Name, value).Status != MethodInvokeResult.RESULT.SUCCESS)
                return "Mật khẩu cũ không trùng khớp.";
            return "";
        }

        private string validatePassNew(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return "Mật khẩu không được bỏ trống.";
            }
            else if (value.IndexOf(' ') > -1)
            {
                return "Mật khẩu không được chứa khoảng trắng.";
            }
            else if (value.Length < 8 || value.Length > 45)
            {
                return "Độ dài mật khẩu phải nhiều hơn 8 ký tự và thấp hơn 45 ký tự.";
            }
            return "";
        }

        private string validatePassNewAgain(string value, string valueAgain)
        {
            if (!value.Equals(valueAgain))
                return "Vui lòng nhập lại mật khẩu trùng khớp với mật khẩu mới.";
            return "";
        }
        #endregion

        public string DoEditPassword(EditPasswordMetadata data)
        {
            tbPassOld_LostFocus(null, null);
            tbPassNew_LostFocus(null, null);
            tbPassNewAgain_LostFocus(null, null);
            string result = "";
            try
            {
                MethodInvokeResult signUpResult = this.Get<UserServiceClient>().ChangePassword(this.Get<Client>().ID, data.PassOld, data.PassNew);
                if (signUpResult.Status == MethodInvokeResult.RESULT.SUCCESS)
                    result = "";
                else
                    result = "Vui lòng kiểm tra lại thông tin.";
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                result = "Không thể kết nối đến server.";
            }
            catch (Exception e)
            {
                result = String.Format("Đã có lỗi xảy ra ({0})", e.GetType().ToString());
            }
            return result;
        }
    }
}
