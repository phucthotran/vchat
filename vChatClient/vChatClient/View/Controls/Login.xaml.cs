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
using vChatClient;
using vChat.Templates;

namespace vChat.View.Controls
{
    /// <summary>
    /// Interaction logic for LoginUC.xaml
    /// </summary>
    public partial class Login : vChatController
    {
        public delegate void LoginSuccessHandler(string userLogged);
        public event LoginSuccessHandler OnLoginSuccess = delegate { };

        public delegate void LoginFailedHandler();
        public event LoginFailedHandler OnLoginFailed = delegate { };

        public delegate void SignUpClickHandler();
        public event SignUpClickHandler OnSignUpClicked = delegate { };

        public Login() : base()
        {
            InitializeComponent();
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (this.Controller.Login(tbUser.Text, tbPass.Text))
            {
                OnLoginSuccess(this.tbUser.Text);
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
