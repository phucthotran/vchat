using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChatClient;
using vChat.UserService;
using System.Reflection;
using vChat.Model;

namespace vChat.Controllers
{
    public class SignUpMetadata
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public SignUpMetadata(string user, string pass, string firstName, string lastName, string dateOfBirth)
        {
            this.User = user;
            this.Pass = pass;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
        }
    }

    public class SignUpResponse
    {
        public string UserMessage { get; set; }
        public string PassMessage { get; set; }
        public string FirstNameMessage { get; set; }
        public string LastNameMessage { get; set; }
        public string DateOfBirthMessage { get; set; }
        public string ServiceMessage { get; set; }
        public bool Success
        {
            get
            {
                // kiểm tra trong đối tượng các field thuộc kiểu string và đã được thiết lập message hay chưa
                // kết quả trả về là 1 mảng đối tượng fieldinfo, nếu số lượng trong mảng lớn hơn 1 thì tức là signup có lỗi
                return ((this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Where(f => f.FieldType == typeof(string) && !String.IsNullOrWhiteSpace((string)f.GetValue(this)))).Count() == 0);
            }
        }
    }

    public class SignUpController
    {
        public bool IsUserExist(string user)
        {
            return (App.UserService.UserExist(user).Status == MethodInvokeResult.RESULT.SUCCESS);
        }

        public SignUpResponse SignUp(SignUpMetadata data)
        {
            SignUpResponse res = new SignUpResponse();
            if (String.IsNullOrWhiteSpace(data.User))
                res.UserMessage = "Tên tài khoản không được để trống.";
            if (String.IsNullOrWhiteSpace(data.Pass))
                res.PassMessage = "Mật khẩu không được để trống.";
            if (String.IsNullOrWhiteSpace(data.FirstName))
                res.FirstNameMessage = "Họ không được để trống.";
            if (String.IsNullOrWhiteSpace(data.LastName))
                res.LastNameMessage = "Tên không được để trống.";
            if (String.IsNullOrWhiteSpace(data.DateOfBirth))
                res.DateOfBirthMessage = "Ngày sinh không được để trống.";
            if (res.Success)
            {
                try
                {
                    MethodInvokeResult signUpResult = App.UserService.Signup(data.User, data.Pass, data.FirstName, data.LastName, 1, "abac", DateTime.Parse(data.DateOfBirth));
                    if (signUpResult.Status == MethodInvokeResult.RESULT.SUCCESS)
                        res.ServiceMessage = "";
                    else
                        res.ServiceMessage = signUpResult.Message;
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    res.ServiceMessage = "Không thể kết nối đến server.";
                }
                catch (Exception)
                {
                    res.ServiceMessage = "Đã có lỗi xảy ra.";
                }
            }
            return res;
        }
    }
}
