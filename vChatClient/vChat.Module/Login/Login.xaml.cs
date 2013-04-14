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
            InitializeComponent();
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (DoLogin(tbUser.Text, tbPass.Text))
            {
                Users tmpUser = this.Get<UserServiceClient>().FindName(tbUser.Text);

                Users userLogged = new Users { UserID = tmpUser.UserID, Username = tmpUser.Username };

                OnLoginSuccess(userLogged);
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
