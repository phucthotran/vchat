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
    /// <summary>
    /// Interaction logic for OnlineStatus.xaml
    /// </summary>
    public partial class OnlineStatus : UserControl
    {
        public static readonly DependencyProperty IsOnlineProperty = DependencyProperty.Register("IsOnline", typeof(bool), typeof(OnlineStatus), new UIPropertyMetadata(new PropertyChangedCallback(OnOnlineStatusChanged)));

        private static Brush ONLINE_COLOR = Brushes.Green;
        private static Brush OFFLINE_COLOR = Brushes.Transparent;

        /// <summary>
        /// Lấy/gán trạng thái online/offline
        /// </summary>
        public bool IsOnline
        {
            get { return (bool)GetValue(IsOnlineProperty); }
            set { SetValue(IsOnlineProperty, value); }
        }

        public OnlineStatus()
        {
            InitializeComponent();
        }

        private void SetOnlineStatus(bool IsOnline)
        {
            epseOnlineStatus.Fill = IsOnline ? ONLINE_COLOR : OFFLINE_COLOR;
        }

        private static void OnOnlineStatusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var uc = obj as OnlineStatus;

            if (uc != null && e.NewValue != e.OldValue) //Nếu giá trị của IsOnline đã thay đổi
            {
                bool newValue = (bool)e.NewValue;
                uc.SetOnlineStatus(newValue); //Thay đổi màu cho "epseOnlineStatus" dự theo giá trị của IsOnline
            }
        }
    }
}
