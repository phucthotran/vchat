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
using System.ComponentModel;

namespace vChat.Module.SignUp
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : UserControl
    {
        public delegate void SignUpSuccessHandler();
        public event SignUpSuccessHandler OnSignUpSuccess = delegate { };

        public delegate void SignUpFailedHander();
        public event SignUpFailedHander OnSignUpFailed = delegate { };

        public delegate void UserChangedHandler(string user);
        public event UserChangedHandler OnUserChanged = delegate { };

        private BackgroundWorker _UserWarningWorker;
        private BackgroundWorker _SubmitWorker;

        public string ct;
        public Visibility vi;

        public SignUp()
        {
            InitializeComponent();
            InitUserWarningWorker();
            InitSubmitWorker();
        }

        private void InitUserWarningWorker()
        {
            _UserWarningWorker = new BackgroundWorker();
            _UserWarningWorker.WorkerSupportsCancellation = true;
            _UserWarningWorker.DoWork += new DoWorkEventHandler(delegate(object sender, DoWorkEventArgs e)
            {
                string user = (string)e.Argument;
                string warningText = "";
                try
                {
                    if (IsUserExist(user))
                    {
                        warningText = "Đã có người sử dụng tên tài khoản này.";
                    }
                    else
                    {
                        warningText = "";
                    }
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    warningText = "Không thể kết nối đến server.";
                }
                finally
                {
                    e.Result = warningText;
                }
            });
            _UserWarningWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                UserWarner.Text = (string)e.Result;
            });
        }

        private void InitSubmitWorker()
        {
            _SubmitWorker = new BackgroundWorker();
            _SubmitWorker.DoWork += new DoWorkEventHandler(delegate(object sender, DoWorkEventArgs e)
            {
                e.Result = DoSignUp((SignUpMetadata)e.Argument);
            });
            _SubmitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                if (((SignUpResponse)e.Result).Success)
                {
                    OnSignUpSuccess();
                }
                else
                {
                    OnSignUpFailed();
                    SubmitWarning.Text = ((SignUpResponse)e.Result).ServiceMessage;
                    if (String.IsNullOrWhiteSpace(SubmitWarning.Text))
                    {
                        SubmitWarning.Text = "asdsad asd sd asdsadisadn asiud nas dnasu dasnud san usan uan sad usna";
                    }
                    SubmitWarning.Visibility = System.Windows.Visibility.Visible;
                }
                SubmitProgress.State = Elysium.Controls.ProgressState.Normal;
                SubmitProgress.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            SubmitProgress.State = Elysium.Controls.ProgressState.Indeterminate;
            SubmitProgress.Visibility = System.Windows.Visibility.Visible;
            SubmitWarning.Visibility = System.Windows.Visibility.Collapsed;
            _SubmitWorker.RunWorkerAsync(new SignUpMetadata(tbUser.Text, tbPass.Text, tbFname.Text, tbLname.Text, tbDob.Text));
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbUser_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_UserWarningWorker.IsBusy)
            {
                _UserWarningWorker.CancelAsync();
                InitUserWarningWorker();
            }
            UserWarner.Busy = true;
            _UserWarningWorker.RunWorkerAsync(tbUser.Text);
        }
    }
}
