using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

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
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0L;
                return (T)new BinaryFormatter().Deserialize(ms);
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
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
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
            try
            {
                object moduleInstance = Activator.CreateInstance(typeof(T));
                panel.Children.Clear();
                panel.Children.Add((UIElement)moduleInstance);
                return (T)moduleInstance;
            }
            catch
            {
                throw new ModuleCannotInitException(typeof(T));
            }
        }

        public static T LoadModule<T>(this Panel panel, params object[] args)
        {
            try
            {
                object moduleInstance = Activator.CreateInstance(typeof(T), args);
                panel.Children.Clear();
                panel.Children.Add((UIElement)moduleInstance);
                return (T)moduleInstance;
            }
            catch
            {
                throw new ModuleCannotInitException(typeof(T));
            }
        }

        public static T Get<T>(this UserControl uc)
        {
            return (T)Application.Current.FindResource(typeof(T).Name);
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

        public static void Show(this Window w, UserControl uc)
        {
            w.Content = uc;
            w.InitTheme();
            w.Show();
        }

        public static void Show(this Window w, Panel panel, UserControl uc)
        {
            panel.Children.Clear();
            panel.Children.Add(uc);
            w.InitTheme();
            w.Show();
        }

        public static Window Show(this UserControl uc)
        {
            Window nw = new Window()
            {
                Content = uc
            };
            nw.InitTheme();
            nw.Show();
            return nw;
        }

        public static Window ShowDialog(this UserControl uc)
        {
            Window nw = new Window()
            {
                Content = uc
            };
            nw.InitTheme();
            nw.ShowDialog();
            return nw;
        }
    }

    public class ModuleCannotInitException : Exception
    {
        public string ModuleName { get; set; }
        public override string Message
        {
            get
            {
                return String.Format("Không thể khởi tạo module <{0}>.", ModuleName); 
            }
        }
        public ModuleCannotInitException(Type module)
        {
            this.ModuleName = module.FullName;
        }
    }
}
