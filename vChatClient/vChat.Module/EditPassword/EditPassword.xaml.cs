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
using System.Threading.Tasks;
using System.ComponentModel;

namespace vChat.Module.EditPassword
{
    /// <summary>
    /// Interaction logic for EditPassword.xaml
    /// </summary>
    public partial class EditPassword : UserControl
    {
        private Brush _valid = Brushes.Green;
        private Brush _invalid = Brushes.Red;

        private BackgroundWorker _SubmitWorker;

        public delegate void EditSuccessHandler();
        public event EditSuccessHandler OnEditSuccess = delegate { };

        public EditPassword()
        {
            InitializeComponent();
            InitSubmitWorker();
        }

        private void InitSubmitWorker()
        {
            _SubmitWorker = new BackgroundWorker();
            _SubmitWorker.DoWork += new DoWorkEventHandler(delegate(object sender, DoWorkEventArgs e)
            {
                e.Result = DoEditPassword((EditPasswordMetadata)e.Argument);
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

        private Task _passOldTask;
        private void tbPassOld_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassOld.Dispatcher.CheckAccess())
            {
                string value = tbPassOld.Password;
                if (PassOldWarner.Dispatcher.CheckAccess())
                {
                    PassOldWarner.Busy = true;
                    Action<object> work = obj =>
                    {
                        Action<object> callback = cb =>
                        {
                            PassOldWarner.Text = cb.ToString();
                            if (PassOldWarner.Text == "")
                                tbPassOld.BorderBrush = _valid;
                            else
                                tbPassOld.BorderBrush = _invalid;
                        };
                        string warningText = "";
                        try
                        {
                            warningText = validatePassOld(obj.ToString());
                        }
                        catch (System.ServiceModel.EndpointNotFoundException)
                        {
                            warningText = "Không thể kết nối đến server.";
                        }
                        finally
                        {
                            _passOldTask.ContinueWith(t =>
                            {
                                if (PassOldWarner.Dispatcher.CheckAccess())
                                    callback(warningText);
                                else
                                    PassOldWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                            });
                        }
                    };

                    if (_passOldTask == null || _passOldTask.IsCompleted)
                        _passOldTask = Task.Factory.StartNew(obj =>
                        {
                            work(obj);
                        }, value);
                    else
                        _passOldTask.ContinueWith(t => { work(value); });
                }
                else
                {
                    PassOldWarner.Dispatcher.Invoke(new Action(() => tbPassOld_LostFocus(sender, e)));
                }
            }
            else
            {
                tbPassOld.Dispatcher.Invoke(new Action(() => tbPassOld_LostFocus(sender, e)));
            }
        }

        private Task _passNewTask;
        private void tbPassNew_LostFocus(object sender, RoutedEventArgs e)
        {
            string value = tbPassNew.Password;
            if (PassNewWarner.Dispatcher.CheckAccess())
            {
                PassNewWarner.Busy = true;
                Action<object> work = obj =>
                {
                    Action<object> callback = cb =>
                    {
                        PassNewWarner.Text = cb.ToString();
                        if (PassNewWarner.Text == "")
                            tbPassNew.BorderBrush = _valid;
                        else
                            tbPassNew.BorderBrush = _invalid;
                    };
                    string warningText = "";
                    try
                    {
                        warningText = validatePassNew(obj.ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException)
                    {
                        warningText = "Không thể kết nối đến server.";
                    }
                    finally
                    {
                        _passNewTask.ContinueWith(t =>
                        {
                            if (PassNewWarner.Dispatcher.CheckAccess())
                                callback(warningText);
                            else
                                PassNewWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                        });
                    }
                };

                if (_passNewTask == null || _passNewTask.IsCompleted)
                    _passNewTask = Task.Factory.StartNew(obj =>
                    {
                        work(obj);
                    }, value);
                else
                    _passNewTask.ContinueWith(t => { work(value); });
            }
            else
            {
                PassOldWarner.Dispatcher.Invoke(new Action(() => tbPassNew_LostFocus(sender, e)));
            }
        }

        private Task _passNewAgainTask;
        private void tbPassNewAgain_LostFocus(object sender, RoutedEventArgs e)
        {
            string[] value = new string[2] { tbPassNew.Password, tbPassNewAgain.Password };
            if (PassNewAgainWarner.Dispatcher.CheckAccess())
            {
                PassNewAgainWarner.Busy = true;
                Action<object> work = obj =>
                {
                    Action<object> callback = cb =>
                    {
                        PassNewAgainWarner.Text = cb.ToString();
                        if (PassNewAgainWarner.Text == "")
                            tbPassNewAgain.BorderBrush = _valid;
                        else
                            tbPassNewAgain.BorderBrush = _invalid;
                    };
                    string warningText = "";
                    try
                    {
                        string[] values = (string[])obj;
                        warningText = validatePassNewAgain(values[0], values[1]);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException)
                    {
                        warningText = "Không thể kết nối đến server.";
                    }
                    finally
                    {
                        _passNewAgainTask.ContinueWith(t =>
                        {
                            if (PassNewWarner.Dispatcher.CheckAccess())
                                callback(warningText);
                            else
                                PassNewWarner.Dispatcher.Invoke(new Action(() => callback(warningText)));
                        });
                    }
                };

                if (_passNewAgainTask == null || _passNewAgainTask.IsCompleted)
                    _passNewAgainTask = Task.Factory.StartNew(obj =>
                    {
                        work(obj);
                    }, value);
                else
                    _passNewAgainTask.ContinueWith(t => { work(value); });
            }
            else
            {
                PassNewWarner.Dispatcher.Invoke(new Action(() => tbPassNewAgain_LostFocus(sender, e)));
            }
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            btSubmit.IsEnabled = false;
            SubmitProgress.State = Elysium.Controls.ProgressState.Indeterminate;
            SubmitProgress.Visibility = System.Windows.Visibility.Visible;
            _SubmitWorker.RunWorkerAsync(new EditPasswordMetadata(tbPassOld.Password, tbPassNew.Password, tbPassNewAgain.Password));
        }
    }
}
