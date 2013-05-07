using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using vChat.Service.UserService;
using vChat.Model.Entities;

namespace vChat.Module.RecoveryPassword
{
    public partial class RecoveryPassword : UserControl
    {
        #region Validate Box
        private string validateUser(string user)
        {
            if (_userService.UserExist(user).Status != Model.MethodInvokeResult.RESULT.SUCCESS)
                return "Tài khoản này không tồn tại.";
            return "";
        }

        private string validatePass(string pass)
        {
            if (String.IsNullOrWhiteSpace(pass))
            {
                return "Mật khẩu không được bỏ trống.";
            }
            else if (pass.IndexOf(' ') > -1)
            {
                return "Mật khẩu không được chứa khoảng trắng.";
            }
            else if (pass.Length < 8 || pass.Length > 45)
            {
                return "Độ dài mật khẩu phải nhiều hơn 8 ký tự và thấp hơn 45 ký tự.";
            }
            return "";
        }

        private string validateAnswer(string username, int questionID, string answer)
        {
            if (validateUser(username) != null)
            {
                tbUser_LostFocus(null, null);
                return "Tài khoản này không tồn tại.";
            }
            else
            {
                if (_userService.AnswerIsMatch(_userService.FindName(username).UserID, questionID, answer).Status == Model.MethodInvokeResult.RESULT.SUCCESS)
                    return "";
                else
                    return "Câu trả lời không trùng khớp ý zới pé!!";
            }
        }
        #endregion

        public string DoRecovery(string username, string newPassword)
        {
            tbPass_LostFocus(null, null);
            string result = "";
            try
            {
                if (_userService.ChangePassword(_userService.FindName(username).UserID, null, newPassword).Status == Model.MethodInvokeResult.RESULT.SUCCESS)
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
