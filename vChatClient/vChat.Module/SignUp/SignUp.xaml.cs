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
using vChat.Service.UserService;

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
            cbQuestion.ItemsSource = this.Get<UserServiceClient>().GetAllQuestion();
            cbQuestion.DisplayMemberPath="Content";
            cbQuestion.SelectedValuePath="QuestionID";
            cbQuestion.SelectedValue = "1";
            this.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
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
                if (e.Result.ToString().Equals(""))
                    OnSignUpSuccess();
                else
                {
                    OnSignUpFailed();
                    SubmitWarning.Text = e.Result.ToString();
                    SubmitWarning.Visibility = System.Windows.Visibility.Visible;
                }
                SubmitProgress.State = Elysium.Controls.ProgressState.Normal;
                SubmitProgress.Visibility = System.Windows.Visibility.Collapsed;
                btSubmit.IsEnabled = true;
            });
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            btSubmit.IsEnabled = false;
            SubmitProgress.State = Elysium.Controls.ProgressState.Indeterminate;
            SubmitProgress.Visibility = System.Windows.Visibility.Visible;
            SubmitWarning.Visibility = System.Windows.Visibility.Collapsed;
            _SubmitWorker.RunWorkerAsync(new SignUpMetadata(tbUser.Text, tbPass.Password, tbFname.Text, tbLname.Text, cbQuestion.SelectedIndex, tbAnswer.Text, tbDob.Text));
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            OnLoginClicked();
        }

        private Task _userTask;
        private void tbUser_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbUser.Dispatcher.CheckAccess())
            {
                string user = tbUser.Text;
                if (UserWarner.Dispatcher.CheckAccess())
                {
                    UserWarner.Busy = true;
                    Action<object> work = obj =>
                    {
                        Action<object> callback = cb =>
                        {
                            UserWarner.Text = cb.ToString();
                            if (UserWarner.Text == "")
                                tbUser.BorderBrush = _valid;
                            else
                                tbUser.BorderBrush = _invalid;
                        };
                        string warningText = "";
                        try
                        {
                            warningText = validateUser(obj.ToString());
                        }
                        catch (System.ServiceModel.EndpointNotFoundException)
                        {
                            warningText = "Không thể kết nối đến server.";
                        }
                        finally
                        {
                            _userTask.ContinueWith(t =>
                            {
                                if (UserWarner.Dispatcher.CheckAccess())
                                    callback(warningText);
                                else
                                    UserWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                            });
                        }
                    };

                    if (_userTask == null || _userTask.IsCompleted)
                        _userTask = Task.Factory.StartNew(obj =>
                        {
                            work(obj);
                        }, user);
                    else
                        _userTask.ContinueWith(t => { work(user); });
                }
                else
                {
                    UserWarner.Dispatcher.Invoke(new Action(() => tbUser_LostFocus(sender, e)));
                }
            }
            else
            {
                tbUser.Dispatcher.Invoke(new Action(() => tbUser_LostFocus(sender, e)));
            }
        }

        private Task _passTask;
        private void tbPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PassWarner.Dispatcher.CheckAccess())
            {
                PassWarner.Busy = true;
                Action<object> work = obj =>
                {
                    Action<object> callback = cb =>
                    {
                        PassWarner.Text = cb.ToString();
                        if (PassWarner.Text == "")
                            tbPass.BorderBrush = _valid;
                        else
                            tbPass.BorderBrush = _invalid;
                    };
                    string warningText = validatePass(obj.ToString());
                    _passTask.ContinueWith(t =>
                    {
                        if (PassWarner.Dispatcher.CheckAccess())
                            callback(warningText);
                        else
                            PassWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                    });
                };

                if (_passTask == null || _passTask.IsCompleted)
                    _passTask = Task.Factory.StartNew(obj =>
                    {
                        work(obj);
                    }, tbPass.Password);
                else
                    _passTask.ContinueWith(t => { work(tbPass.Password); });
            }
            else
            {
                PassWarner.Dispatcher.Invoke(new Action(() => tbPass_LostFocus(sender, e)));
            }
        }

        private Task _fNameTask;
        private void tbFname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FNameWarner.Dispatcher.CheckAccess())
            {
                FNameWarner.Busy = true;
                Action<object> work = obj =>
                {
                    Action<object> callback = cb =>
                    {
                        FNameWarner.Text = cb.ToString();
                        if (FNameWarner.Text != "")
                            tbFname.BorderBrush = _invalid;
                        else
                            tbFname.BorderBrush = _valid;
                    };
                    string warningText = validateFirstName(obj.ToString());
                    _fNameTask.ContinueWith(t =>
                    {
                        if (FNameWarner.Dispatcher.CheckAccess())
                            callback(warningText);
                        else
                            FNameWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                    });
                };

                if (_fNameTask == null || _fNameTask.IsCompleted)
                    _fNameTask = Task.Factory.StartNew(obj =>
                    {
                        work(obj);
                    }, tbFname.Text);
                else
                    _fNameTask.ContinueWith(t => { work(tbFname.Text); });
            }
            else
            {
                FNameWarner.Dispatcher.Invoke(new Action(() => tbFname_LostFocus(sender, e)));
            }
        }

        private Task _lNameTask;
        private void tbLname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LNameWarner.Dispatcher.CheckAccess())
            {
                LNameWarner.Busy = true;

                Action<object> work = obj =>
                {
                    Action<object> callback = cb =>
                    {
                        LNameWarner.Text = cb.ToString();
                        if (LNameWarner.Text != "")
                            tbLname.BorderBrush = _invalid;
                        else
                            tbLname.BorderBrush = _valid;
                    };
                    string warningText = validateLastName(obj.ToString());
                    _lNameTask.ContinueWith(t =>
                    {
                        if (LNameWarner.Dispatcher.CheckAccess())
                            callback(warningText);
                        else
                            LNameWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                    });
                };

                if (_lNameTask == null || _lNameTask.IsCompleted)
                    _lNameTask = Task.Factory.StartNew(obj =>
                    {
                        work(obj);
                    }, tbLname.Text);
                else
                    _lNameTask.ContinueWith(t => { work(tbLname.Text); });
            }
            else
            {
                LNameWarner.Dispatcher.Invoke(new Action(() => tbLname_LostFocus(sender, e)));
            }
        }

        private void tbDob_LostFocus(object sender, RoutedEventArgs e)
        {
            tbDob.BorderBrush = _valid;
        }

        private void cbQuestion_LostFocus(object sender, RoutedEventArgs e)
        {
            cbQuestion.BorderBrush = _valid;
        }

        private Task _answerTask;
        private void tbAnswer_LostFocus(object sender, RoutedEventArgs e)
        {

            if (AnswerWarner.Dispatcher.CheckAccess())
            {
                AnswerWarner.Busy = true;

                Action<object> work = obj =>
                {
                    Action<object> callback = cb =>
                    {
                        AnswerWarner.Text = cb.ToString();
                        if (AnswerWarner.Text != "")
                            tbAnswer.BorderBrush = _invalid;
                        else
                            tbAnswer.BorderBrush = _valid;
                    };
                    string warningText = validateLastName(obj.ToString());
                    _answerTask.ContinueWith(t =>
                    {
                        if (AnswerWarner.Dispatcher.CheckAccess())
                            callback(warningText);
                        else
                            AnswerWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                    });
                };

                if (_answerTask == null || _answerTask.IsCompleted)
                    _answerTask = Task.Factory.StartNew(obj =>
                    {
                        work(obj);
                    }, tbAnswer.Text);
                else
                    _answerTask.ContinueWith(t => { work(tbAnswer.Text); });
            }
            else
            {
                AnswerWarner.Dispatcher.Invoke(new Action(() => tbAnswer_LostFocus(sender, e)));
            }
        }
    }
}
