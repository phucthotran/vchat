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

namespace vChat.Control
{
    public class UserDefined : Run
    {
        static UserDefined()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UserDefined), new FrameworkPropertyMetadata(typeof(UserDefined)));
        }

        public UserDefined SetText(string user, bool owner)
        {
            this.FontWeight = FontWeights.Bold;
            this.FontSize = 14;
            if (owner)
                this.Foreground = Brushes.Black;
            else
                this.Foreground = Brushes.DarkGreen;
            this.Text = user + ": ";
            return this;
        }
    }
}
