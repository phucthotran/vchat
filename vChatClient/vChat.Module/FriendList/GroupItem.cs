using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace vChat.Module.FriendList
{
    class GroupItem : StackPanel
    {
        public GroupItem() { }

        public static readonly DependencyProperty GroupIDProperty = DependencyProperty.Register("GroupID", typeof(int), typeof(GroupItem), new UIPropertyMetadata(0));
        public int GroupID
        {
            get { return (int)GetValue(GroupIDProperty); }
            set { SetValue(GroupIDProperty, value); }
        }

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(String), typeof(GroupItem), new UIPropertyMetadata(""));
        public String GroupName
        {
            get { return GetValue(GroupNameProperty).ToString(); }
            set { SetValue(GroupNameProperty, value); }
        }        
    }
}
