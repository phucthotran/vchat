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
using vChat.Service.UserService;
using System.Threading.Tasks;
using System.ComponentModel;

namespace vChat.Module.RecoveryPassword
{
    /// <summary>
    /// Interaction logic for RecoveryPassword.xaml
    /// </summary>
    public partial class RecoveryPassword : UserControl
    {
        private UserServiceClient _userService;
        private BackgroundWorker _checkWorker;
        private BackgroundWorker _submitWorker;

        public delegate void RecoverySuccessHandler(string username);
        public event RecoverySuccessHandler OnRecoverySuccess = delegate { };
        
        public RecoveryPassword()
        {
            InitializeComponent();
            InitCheckWorker();
            InitSubmitWorker();
            _userService = this.Get<UserServiceClient>();
            cbQuestion.ItemsSource = _userService.GetAllQuestion();
            cbQuestion.DisplayMemberPath = "Content";
            cbQuestion.SelectedValuePath = "QuestionID";
            cbQuestion.SelectedIndex = 0;
        }

        private void InitCheckWorker()
        {
            _checkWorker = new BackgroundWorker();
            _checkWorker.DoWork += new DoWorkEventHandler((sender, e) =>
            {
                object[] data = (object[])e.Argument;
                e.Result = validateAnswer(data[0].ToString(), Convert.ToInt32(data[1]), data[2].ToString());
            });
            _checkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) =>
            {
                RecoveryMessage.Text = e.Result.ToString();
                if (RecoveryMessage.Text == "")
                {
                    RecoveryMessage.Text = "Câu trả lời hoàn toàn chính xác!!";
                    tbAnswer.IsEnabled = false;
                    cbQuestion.IsEnabled = false;
                    GridNewPassword.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    btCheck.Visibility = System.Windows.Visibility.Visible;
                    GridMessage.Visibility = System.Windows.Visibility.Collapsed;
                }
            });
        }

        private void InitSubmitWorker()
        {
            _submitWorker = new BackgroundWorker();
            _submitWorker.DoWork += new DoWorkEventHandler((sender, e) =>
            {
                object[] data = (object[])e.Argument;
                string result = DoRecovery(data[0].ToString(), data[1].ToString());
                if (result == "")
                    result = "Đã khôi phục mật khẩu thành công.";
                e.Result = result;
            });
            _submitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) =>
            {
                MessageBox.Show(e.Result.ToString());
                OnRecoverySuccess(tbUser.Text);
            });
        }

        private void btCheck_Click(object sender, RoutedEventArgs e)
        {
            btCheck.Visibility = System.Windows.Visibility.Collapsed;
            GridMessage.Visibility = System.Windows.Visibility.Visible;
            RecoveryMessage.Busy = true;
            _checkWorker.RunWorkerAsync(new object[] { tbUser.Text, cbQuestion.SelectedValue, tbAnswer.Password });
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            _submitWorker.RunWorkerAsync(new object[] { tbUser.Text, tbPass.Password });
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
                                tbUser.BorderBrush = Brushes.Green;
                            else
                                tbUser.BorderBrush = Brushes.Red;
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
                            tbPass.BorderBrush = Brushes.Green;
                        else
                            tbPass.BorderBrush = Brushes.Red;
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
    }
}
