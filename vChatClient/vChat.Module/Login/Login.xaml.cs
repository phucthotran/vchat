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
using vChat.Model.Entities;
using vChat.Service.UserService;

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
            if (loginSuccessHandler != null)
                OnLoginSuccess += loginSuccessHandler;
            if (String.IsNullOrEmpty(RememberedUser()))
            {
                InitializeComponent();
            }
            else
            {
                DoConnect(Cookie.Instance["user"].ToString());
                DoLoginSuccess(Cookie.Instance["user"].ToString());
            }
        }
        
        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (DoLogin(tbUser.Text, tbPass.Password, Remember.IsChecked.Value))
            {
                DoLoginSuccess(tbUser.Text);
            }
            else
            {
                OnLoginFailed();
            }
        }

        private void btSignUp_Click(object sender, RoutedEventArgs e)
        {
            OnSignUpClicked();
        }
    }
}
