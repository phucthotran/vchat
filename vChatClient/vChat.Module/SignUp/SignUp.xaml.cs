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
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

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

        public delegate void LoginClickHandler();
        public event LoginClickHandler OnLoginClicked = delegate { };

        private BackgroundWorker _SubmitWorker;

        public string ct { get; set; }
        public Visibility vi { get; set; }

        private Brush _valid = Brushes.Green;
        private Brush _invalid = Brushes.Red;

        public SignUp()
        {
            InitializeComponent();
            InitSubmitWorker();
            tbDob.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            tbDob.DisplayDateStart = DateTime.Parse("1/1/1900");
            tbDob.DisplayDateEnd = DateTime.Today;
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name); 
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci; 
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
            _SubmitWorker.RunWorkerAsync(new SignUpMetadata(tbUser.Text, tbPass.Password, tbFname.Text, tbLname.Text, tbDob.Text));
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            OnLoginClicked();
        }

        private Task _userTask;
        private void tbUser_LostFocus(object sender, RoutedEventArgs e)
        {
            UserWarner.Busy = true;
            if (_userTask == null || _userTask.IsCompleted)
                _userTask = Task.Factory.StartNew(obj =>
                {
                    string user = obj.ToString();
                    string warningText = "";
                    try
                    {
                        warningText = validateUser(user);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException)
                    {
                        warningText = "Không thể kết nối đến server.";
                    }
                    finally
                    {
                        Action<object> action = res =>
                        {
                            UserWarner.Text = res.ToString();
                            if (UserWarner.Text == "")
                                tbUser.BorderBrush = _valid;
                            else
                                tbUser.BorderBrush = _invalid;
                        };
                        _userTask.ContinueWith(t =>
                        {
                            if (UserWarner.Dispatcher.CheckAccess())
                                action(warningText);
                            else
                                UserWarner.Dispatcher.Invoke(new Action(() => action(warningText)));
                        });
                    }
                }, tbUser.Text);
        }

        private Task _passTask;
        private void tbPass_LostFocus(object sender, RoutedEventArgs e)
        {
      /*      PassWarner.Busy = true;
            Action<object> action = res =>
            {
                UserWarner.Text = res.ToString();
                if (UserWarner.Text == "")
                    tbUser.BorderBrush = _valid;
                else
                    tbUser.BorderBrush = _invalid;
            };
            Action<Task> callback = task =>
            {
                if (UserWarner.Dispatcher.CheckAccess())
                    action(tbPass.Passwod);
                else
                    UserWarner.Dispatcher.Invoke(new Action(() => action(tbP)));
            };
            if (_passTask == null || _passTask.IsCompleted)
                _passTask = Task.Factory.StartNew(obj =>
                {
                    string warningText = validatePass(tbPass.Password);
                    _passTask.ContinueWith(t =>
                    {
                        if (UserWarner.Dispatcher.CheckAccess())
                            action(warningText);
                        else
                            UserWarner.Dispatcher.Invoke(new Action(() => action(warningText)));
                    });
                }, tbPass.Password);
            _passTask.ContinueWith(t =>
            {

            }); */
        }

        private void tbFname_LostFocus(object sender, RoutedEventArgs e)
        {
            FNameWarner.Busy = true;
            FNameWarner.Text = validateFirstName(tbFname.Text);
            if (FNameWarner.Text != "")
                tbFname.BorderBrush = _invalid;
            else
                tbFname.BorderBrush = _valid;
        }

        private void tbLname_LostFocus(object sender, RoutedEventArgs e)
        {
            LNameWarner.Busy = true;
            LNameWarner.Text = validateLastName(tbLname.Text);
            if (LNameWarner.Text != "")
                tbLname.BorderBrush = _invalid;
            else
                tbLname.BorderBrush = _valid;
        }

        private void tbDob_LostFocus(object sender, RoutedEventArgs e)
        {
            tbDob.BorderBrush = _valid;
        }

        private void cbQuestion_LostFocus(object sender, RoutedEventArgs e)
        {
            cbQuestion.BorderBrush = _valid;
        }

        private void tbAnswer_LostFocus(object sender, RoutedEventArgs e)
        {
            AnswerWarner.Busy = true;
            AnswerWarner.Text = validateAnswer(tbAnswer.Text);
            if (AnswerWarner.Text != "")
                tbAnswer.BorderBrush = _invalid;
            else
                tbAnswer.BorderBrush = _valid;
        }
    }
}
