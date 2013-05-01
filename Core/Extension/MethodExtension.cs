using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.ComponentModel;

namespace System
{
    public static class MethodExtension
    {
        /// <summary>
        /// Chuyển từ một mảng byte sang kiểu dữ liệu mong muốn.
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu cần chuyển.</typeparam>
        /// <param name="bytes">Mảng byte truyền vào cần cho việc chuyển.</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0L;
                    return (T)new BinaryFormatter().Deserialize(ms);
                }
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// Chuyển từ một đối tượng sang một mảng byte.
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu cần chuyển.</typeparam>
        /// <param name="obj">Đối tượng cần chuyển đổi.</param>
        /// <returns></returns>
        public static byte[] ToBytes<T>(this T obj)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ms, obj);
                    return ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Chuyển từ một chuỗi số sang dạng integer.
        /// </summary>
        /// <param name="s">Chuỗi số cần chuyển đổi.</param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            return Convert.ToInt32(s);
        }

        public static T LoadModule<T>(this Panel panel)
        {
            return panel.LoadModule<T>(null);
        }

        public static T LoadModule<T>(this Panel panel, params object[] args)
        {
            IEnumerable<object> element = panel.Children.Cast<object>().Where(e => e.GetType() == typeof(T));
            if (element.Count() > 0)
            {
                return (T)element.ElementAt(0);
            }
            object moduleInstance;
            if (args == null)
                moduleInstance = Activator.CreateInstance(typeof(T));
            else  
                moduleInstance = Activator.CreateInstance(typeof(T), args);
            panel.Children.Clear();
            panel.Children.Add((UIElement)moduleInstance);
            return (T)moduleInstance;
        }

        public static T Get<T>(this UserControl uc)
        {
            return uc.Get<T>(typeof(T).Name);
        }

        public static T Get<T>(this Window w)
        {
            return w.Get<T>(typeof(T).Name);
        }

        public static T Get<T>(this UserControl uc, string Key)
        {
            return (T)Application.Current.FindResource(Key);
        }

        public static T Get<T>(this Window w, string Key)
        {
            return (T)Application.Current.FindResource(Key);
        }

        private static ResourceDictionary Theme = null;

        public static void InitTheme(this Window w)
        {
            if (Theme == null)
            {
                Theme = new ResourceDictionary();
                Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml", UriKind.RelativeOrAbsolute) });
                Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute) });
                Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute) });
                Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml", UriKind.RelativeOrAbsolute) });
                Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml", UriKind.RelativeOrAbsolute) });
                Theme.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/FlatButton.xaml", UriKind.RelativeOrAbsolute) });
            }
            w.Resources = Theme;
        }

        public static void CloseHandler(this Window w, bool isHandle)
        {
            w.CloseHandler(isHandle, "Bạn có muốn tắt ứng dụng không?");
        }

        public static void CloseHandler(this Window w, bool isHandle, string question)
        {
            if (isHandle)
            {
                if (MessageBox.Show(question, "vChat - Chat kiểu Việt", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    w.Close();
                }
            }
            else
            {
                w.Close();
            }
        }

        public static void CloseHandler(this Window w, bool isHandle, CancelEventArgs e)
        {
            w.CloseHandler(isHandle, "Bạn có muốn tắt ứng dụng không?", e);
        }

        public static void CloseHandler(this Window w, bool isHandle, string question, CancelEventArgs e)
        {
            if (isHandle)
            {
                if (MessageBox.Show(question, "vChat - Chat kiểu Việt", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    e.Cancel = false;
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        #region Flash Window Extension
        private const UInt32 FLASHW_STOP = 0; //Stop flashing. The system restores the window to its original state.
        private const UInt32 FLASHW_CAPTION = 1; //Flash the window caption.
        private const UInt32 FLASHW_TRAY = 2; //Flash the taskbar button.
        private const UInt32 FLASHW_ALL = 3; //Flash both the window caption and taskbar button.
        private const UInt32 FLASHW_TIMER = 4; //Flash continuously, until the FLASHW_STOP flag is set.
        private const UInt32 FLASHW_TIMERNOFG = 12; //Flash continuously until the window comes to the foreground.
 
        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize; //The size of the structure in bytes.
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.
            public UInt32 dwFlags; //The Flash Status.
            public UInt32 uCount; // number of times to flash the window
            public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
        }
 
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
 
        public static void FlashWindow(this Window win, UInt32 count = UInt32.MaxValue)
        {
            //Don't flash if the window is active
            if (win.IsActive) return;
 
            WindowInteropHelper h = new WindowInteropHelper(win);
 
            FLASHWINFO info = new FLASHWINFO
                                    {
                                        hwnd = h.Handle,
                                        dwFlags = FLASHW_ALL | FLASHW_TIMER,
                                        uCount = count,
                                        dwTimeout = 0
                                    };
 
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            FlashWindowEx(ref info);
        }
 
        public static void StopFlashingWindow(this Window win)
        {
            WindowInteropHelper h = new WindowInteropHelper(win);
 
            FLASHWINFO info = new FLASHWINFO();
            info.hwnd = h.Handle;
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.dwFlags = FLASHW_STOP;
            info.uCount = UInt32.MaxValue;
            info.dwTimeout = 0;
 
            FlashWindowEx(ref info);
        }
        #endregion
    }
}