using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vChat.Model;
using MahApps.Metro.Controls;
using System.Windows;

namespace vChat.Module
{
    class Helper
    {
        public static void ShowMessage(MethodInvokeResult InvokeResult)
        {
            switch (InvokeResult.Status)
            {
                case MethodInvokeResult.RESULT.SUCCESS:
                case MethodInvokeResult.RESULT.FAIL:
                case MethodInvokeResult.RESULT.UNHANDLE_ERROR:
                    MessageBox.Show(InvokeResult.Message);
                    break;

                case MethodInvokeResult.RESULT.INPUT_ERROR:
                    String Errors = String.Join(",", InvokeResult.Errors.ToArray());
                    MessageBox.Show(String.Format("Please fix following errors: ", Errors));
                    break;
            }
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
