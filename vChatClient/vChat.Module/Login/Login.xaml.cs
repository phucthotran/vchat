using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using vChat.Control;
using vChat.Service.UserService;
using System.ComponentModel;
using vChat.Model.Entities;

namespace vChat.Module.Login
{
    /// <summary>
    /// Interaction logic for LoginUC.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public delegate void LoginSuccessHandler(Users UserLogged);
        public event LoginSuccessHandler OnLoginSuccess = delegate { };

        public delegate void LoginFailedHandler();
        public event LoginFailedHandler OnLoginFailed = delegate { };

        public delegate void SignUpClickHandler();
        public event SignUpClickHandler OnSignUpClicked = delegate { };

        public delegate void RecoveryPasswordHandler();
        public event RecoveryPasswordHandler OnRecovery = delegate { };

        private BackgroundWorker _LoggingWorker = new BackgroundWorker();

        public Login()
        {
            InitLogin(null);
        }

        public Login(LoginSuccessHandler loginSuccessHandler)
        {
            InitLogin(loginSuccessHandler);
        }

        private void InitLogin(LoginSuccessHandler loginSuccessHandler)
        {
            _LoggingWorker.DoWork += new DoWorkEventHandler(LoggingWorker_DoWork);
            _LoggingWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(LoggingWorker_RunWorkerCompleted);
            if (loginSuccessHandler != null)
                OnLoginSuccess += loginSuccessHandler;
            InitializeComponent();
            tbUser.Focus();
            if (IsExistCookie())
            {
                Cookie cookie = Cookie.Instance;
                LoginPanel.Visibility = System.Windows.Visibility.Collapsed;
                ProcessPanel.Visibility = System.Windows.Visibility.Visible;
                UserLogging.Text = cookie["user"].ToString();
                _LoggingWorker.RunWorkerAsync(new LoginMetadata(true, cookie["user"].ToString(), cookie["pass"].ToString(), true));
            }
        }

        public void SetUser(string user)
        {
            tbUser.Text = user;
        }

        void LoggingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessPanel.Visibility = System.Windows.Visibility.Collapsed;
            LoggingResponse res = e.Result as LoggingResponse;
            if (res.Success)
            {
                this.Get<Core.Client.Client>().ID = res.Users.UserID;
                this.Get<Core.Client.Client>().Name = res.Users.Username;
                OnLoginSuccess(res.Users);
            }
            else
            {
                LoginPanel.Visibility = System.Windows.Visibility.Visible;
                Message.Text = res.Message;
            }
        }

        void LoggingWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            LoginMetadata packed = e.Argument as LoginMetadata;
            bool result = false;
            LoggingResponse res = new LoggingResponse(null);
            try
            {
                if (packed.isHashed)
                {
                    result = (this.Get<UserServiceClient>().LoginHash(packed.User, packed.Pass).Status == Model.MethodInvokeResult.RESULT.SUCCESS);
                }
                else
                {
                    result = (this.Get<UserServiceClient>().Login(packed.User, packed.Pass).Status == Model.MethodInvokeResult.RESULT.SUCCESS);
                    if (result)
                    {
                        RememberAccount(packed.isRemember, packed.User, packed.Pass);
                    }
                }
                res = new LoggingResponse(this.Get<UserServiceClient>().FindName(packed.User));
                if (result)
                {
                    DoConnect(packed.User);
                    res.Success = true;
                }
                else
                {
                    if (packed.isCookie)
                    {
                        res.Message = "Thông tin lưu trữ tài khoản đã gặp lỗi. Vui lòng đăng nhập lại.";
                    }
                    else
                    {
                        res.Message = "Tài khoản và mật khẩu không trùng khớp.";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is System.ServiceModel.EndpointNotFoundException
                    || ex is System.Net.Sockets.SocketException)
                    res.Message = "Không thể kết nối đến server.";
                else
                    res.Message = ex.Message;
            }
            finally
            {
                e.Result = res;
            }
        }
        
        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = System.Windows.Visibility.Collapsed;
            ProcessPanel.Visibility = System.Windows.Visibility.Visible;
            UserLogging.Text = tbUser.Text;
            _LoggingWorker.RunWorkerAsync(new LoginMetadata(false, tbUser.Text, tbPass.Password, false, Remember.IsChecked.Value));
        }

        private void btSignUp_Click(object sender, RoutedEventArgs e)
        {
            OnSignUpClicked();
        }

        private void btRecoveryPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnRecovery();
        }
    }

    public class LoginMetadata
    {
        public string User { get; private set; }
        public string Pass { get; private set; }
        public bool isCookie { get; private set; }
        public bool isHashed { get; private set; }
        public bool isRemember { get; private set; }
        public LoginMetadata(bool isCookie, string user, string pass, bool isHashed) : this(isCookie, user, pass, isHashed, true)
        {
        }
        public LoginMetadata(bool isCookie, string user, string pass, bool isHashed, bool isRemember)
        {
            this.isCookie = isCookie;
            this.User = user;
            this.Pass = pass;
            this.isHashed = isHashed;
            this.isRemember = isRemember;
        }
    }

    public class LoggingResponse
    {
        public Users Users { get; private set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public LoggingResponse(Users users)
        {
            this.Users = users;
        }
    }
}
