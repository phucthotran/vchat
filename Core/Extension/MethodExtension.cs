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
                    return (T)new SoapFormatter().Deserialize(ms);
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
                    new SoapFormatter().Serialize(ms, obj);
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
    }
}