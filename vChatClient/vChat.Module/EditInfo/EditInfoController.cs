using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using vChat.Service.UserService;
using Core.Client;
using vChat.Model;
using System.Text.RegularExpressions;

namespace vChat.Module.EditInfo
{
    public class EditInfoMetadata
    {
        public string PassOld { get; set; }
        public string PassNew { get; set; }
        public string PassNewAgain { get; set; }
        public EditInfoMetadata(string PassOld, string PassNew, string PassNewAgain)
        {
            this.PassOld = PassOld;
            this.PassNew = PassNew;
            this.PassNewAgain = PassNewAgain;
        }
    }

    public partial class EditInfo : UserControl
    {
        #region Validate Box
        private string validateAnswer(string answer)
        {
            if (String.IsNullOrWhiteSpace(answer))
            {
                return "Hỏi mà không trả lời là bất lịch sự!!";
            }
            else if (answer.Length < 2 || answer.Length > 50)
            {
                return "Câu trả lời phải ít nhất 2 ký tự và không được vượt quá 50 ký tự.";
            }
            return "";
        }

        private string validateFirstName(string fname)
        {
            if (String.IsNullOrWhiteSpace(fname))
            {
                return "Họ đệm không được bỏ trống.";
            }
            else if (!Regex.IsMatch(fname, "^[a-zA-Z]+([ ]?[a-zA-Z]+)*$"))
            {
                return "Chắc đây không phải là họ đệm trong giấy khai sinh?";
            }
            else if (fname.Length < 2 || fname.Length > 45)
            {
                return "Họ đệm phải có ít nhất 2 ký tự và không được nhiều hơn 45 ký tự.";
            }
            return "";
        }

        private string validateLastName(string lname)
        {
            if (String.IsNullOrWhiteSpace(lname))
            {
                return "Tên thật không được bỏ trống.";
            }
            else if (!Regex.IsMatch(lname, "^[a-zA-Z]+$"))
            {
                return "Chắc đây không phải là tên thật trong giấy khai sinh?";
            }
            else if (lname.Length < 2 || lname.Length > 45)
            {
                return "Tên thật phải có ít nhất 2 ký tự và không được nhiều hơn 45 ký tự.";
            }
            return "";
        }
        #endregion

   /*     public string DoEditInfo(EditInfoMetadata data)
        {
            tbPassOld_LostFocus(null, null);
            tbPassNew_LostFocus(null, null);
            tbPassNewAgain_LostFocus(null, null);
            string result = "";
            try
            {
                MethodInvokeResult signUpResult = this.Get<UserServiceClient>().ChangePassword(this.Get<Client>().ID, tbPassOld.Password, tbPassNew.Password);
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
        } */
    }
}
