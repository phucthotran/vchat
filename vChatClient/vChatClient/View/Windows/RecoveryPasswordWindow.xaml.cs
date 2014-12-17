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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using vChat.Module.RecoveryPassword;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for RecoveryPassword.xaml
    /// </summary>
    public partial class RecoveryPasswordWindow : MetroWindow
    {
        public RecoveryPasswordWindow()
        {
            InitializeComponent();
            this.InitTheme();
            Grid.LoadModule<RecoveryPassword>().OnRecoverySuccess += user =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        ((MainWindow)window).SetDefaultUser(user);
                        this.Close();
                        break;
                    }
                }
            };
        }
    }
}
