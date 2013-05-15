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
        /// <summary>
        /// Hiển thị hộp thoại thông báo các thông tin từ trên service gửi về (lỗi, thông tin thành công/thất bại, lỗi ràng buộc,..)
        /// </summary>
        /// <param name="InvokeResult">Đối tượng chứa kết quả trả về từ service</param>
        public static void ShowMessage(MethodInvokeResult InvokeResult)
        {
            if (InvokeResult.Message != null && InvokeResult.Errors != null)
                MessageBox.Show(InvokeResult.Message + "\n" + String.Join(",", InvokeResult.Errors.ToArray()));
            else if (InvokeResult.Message != null)
                MessageBox.Show(InvokeResult.Message);
            else if (InvokeResult.Errors != null)
                MessageBox.Show(String.Join(",", InvokeResult.Errors.ToArray()));            
        }

        /// <summary>
        /// Khởi tạo một đối tượng MetroWindow
        /// </summary>
        /// <param name="NewWindow">Đối tượng chứa kết quả trả về</param>
        /// <param name="Title">Tiêu đề của cửa sổ</param>
        /// <param name="Content">Nội dung phía bên trong cửa số (các control)</param>
        /// <returns></returns>
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
