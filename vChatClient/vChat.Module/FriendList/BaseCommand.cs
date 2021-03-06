﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace vChat.Module.FriendList
{
    /// <summary>
    /// Class nền tảng dùng để tạo các Command
    /// </summary>
    public class BaseCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public virtual void Execute(object parameter)
        {            
        }
    }
}
