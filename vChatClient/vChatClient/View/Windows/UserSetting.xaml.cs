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
using vChat.Module.EditPassword;
using Core.Client;

namespace vChat.View.Windows
{
    /// <summary>
    /// Interaction logic for UserSetting.xaml
    /// </summary>
    public partial class UserSetting : MetroWindow
    {
        public UserSetting()
        {
            InitializeComponent();
            this.InitTheme();
            ChangePasswordContent.Content = new EditPassword();
        }
    }
}
