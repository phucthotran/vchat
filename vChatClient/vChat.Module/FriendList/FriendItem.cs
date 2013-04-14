using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace vChat.Module.FriendList
{
    class FriendItem : StackPanel
    {
        public FriendItem() { }

        public static readonly DependencyProperty IDProperty = DependencyProperty.Register("UserID", typeof(int), typeof(FriendItem), new UIPropertyMetadata(0));
        public int UserID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(String), typeof(FriendItem), new UIPropertyMetadata(""));
        public String Username
        {
            get { return GetValue(UsernameProperty).ToString(); }
            set { SetValue(UsernameProperty, value); }
        }
    }
}
