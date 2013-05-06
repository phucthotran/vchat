using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using vChat.Model;
using MahApps.Metro.Controls;

namespace vChat.Module
{
    class Helper
    {
        public static void ShowMessage(MethodInvokeResult InvokeResult)
        {
            if (InvokeResult.Message != null && InvokeResult.Errors != null)
                MessageBox.Show(InvokeResult.Message + "\n" + String.Join(",", InvokeResult.Errors.ToArray()));
            else if (InvokeResult.Message != null)
                MessageBox.Show(InvokeResult.Message);
            else if (InvokeResult.Errors != null)
                MessageBox.Show(String.Join(",", InvokeResult.Errors.ToArray()));            
        }

        public static Window CreateWindow(ref MetroWindow NewWindow, String Title, System.Windows.Controls.ContentControl Content)
        {
            NewWindow = new MetroWindow();
            NewWindow.Title = Title;
            NewWindow.SizeToContent = SizeToContent.WidthAndHeight;
            NewWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            NewWindow.ResizeMode = ResizeMode.NoResize;
            NewWindow.ShowInTaskbar = false;
            NewWindow.Content = Content;
            NewWindow.InitTheme();

            return NewWindow;
        }
    }
}
