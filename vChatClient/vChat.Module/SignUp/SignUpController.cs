using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Controls;
using vChat.Service.UserService;
using vChat.Model;
using System.Text.RegularExpressions;

namespace vChat.Module.SignUp
{
    public class SignUpMetadata
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Question { get; set; }
        public string Answer { get; set; }
        public DateTime DateOfBirth { get; set; }
        public SignUpMetadata(string user, string pass, string firstName, string lastName, int question, string answer, string dateOfBirth)
        {
            this.User = user;
            this.Pass = pass;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Question = question;
            this.Answer = answer;
            this.DateOfBirth = DateTime.ParseExact(dateOfBirth, "dd/MM/yyyy", null);
        }
    }

    public partial class SignUp : UserControl
    {
        #region Validate Box
        private string validateUser(string user)
        {
            if (String.IsNullOrWhiteSpace(user))
            {
                return "Người vô danh không bao giờ ngồi chat.";
            }
            else if (!Regex.IsMatch(user, "^[a-zA-Z]+([._]?[a-zA-Z0-9]+)*$"))
            {
                return "Tên tài khoản không hợp lệ. Chỉ được dùng các ký tự trong phạm vi \"a-z\", \"A-Z\", \"0-9\", \".\" và \"_\"";
            }
            else if (user.Length < 6 || user.Length > 45)
            {
                return "Độ dài tên tài khoản phải nhiều hơn 6 ký tự và thấp hơn 45 ký tự.";
            }
            else if (this.Get<UserServiceClient>().UserExist(user).Status == MethodInvokeResult.RESULT.SUCCESS)
            {
                return "Tài khoản này đã có người sử dụng. Vui lòng chọn tên đăng nhập khác.";
            }
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

        public string DoSignUp(SignUpMetadata data)
        {
            tbUser_LostFocus(null, null);
            tbPass_LostFocus(null, null);
            tbFname_LostFocus(null, null);
            tbLname_LostFocus(null, null);
            tbAnswer_LostFocus(null, null);
            string result = "";
            try
            {
                MethodInvokeResult signUpResult = this.Get<UserServiceClient>().Signup(data.User, data.Pass, data.FirstName, data.LastName, data.Question, data.Answer, data.DateOfBirth);
                if (signUpResult.Status == MethodInvokeResult.RESULT.SUCCESS)
                    result = "";
                else
                    result = "Vui lòng kiểm tra lại thông tin đăng ký.";
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
