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
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using vChat.Service.UserService;
using Core.Client;
using vChat.Model.Entities;
using System.Threading.Tasks;

namespace vChat.Module.EditInfo
{
    /// <summary>
    /// Interaction logic for EditInfo.xaml
    /// </summary>
    public partial class EditInfo : UserControl
    {
        private BackgroundWorker _SubmitWorker;
        private Brush _valid = Brushes.Green;
        private Brush _invalid = Brushes.Red;

        public delegate void EditSuccessHandler();
        public event EditSuccessHandler OnEditSuccess = delegate { };

        public EditInfo()
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
            cbQuestion.DisplayMemberPath = "Content";
            cbQuestion.SelectedValuePath = "QuestionID";
            cbQuestion.SelectedValue = "1";
            tbUser.Text = this.Get<Client>().Name;
            btRefresh_Click(null, null);
        }

        private void InitSubmitWorker()
        {
            _SubmitWorker = new BackgroundWorker();
            _SubmitWorker.DoWork += new DoWorkEventHandler(delegate(object sender, DoWorkEventArgs e)
            {
                e.Result = DoEditInfo((EditInfoMetadata)e.Argument);
            });
            _SubmitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Result.ToString().Equals(""))
                    OnEditSuccess();
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
            _SubmitWorker.RunWorkerAsync(new EditInfoMetadata(tbFname.Text, tbLname.Text, cbQuestion.SelectedIndex, tbAnswer.Text, tbDob.Text));
        }

        private void btRefresh_Click(object sender, RoutedEventArgs e)
        {
            Users user = this.Get<UserServiceClient>().FindName(this.Get<Client>().Name);
            tbFname.Text = user.FirstName;
            tbLname.Text = user.LastName;
            tbAnswer.Text = user.Answer;
            tbDob.Text = user.Birthdate.ToString("dd/MM/yyyy");
      //      cbQuestion.SelectedValue = user.Question.QuestionID;
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
