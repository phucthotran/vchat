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

        public static T Get<T>(this Window w)
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
        /*
        public static string ExportXaml(this RichTextBox rt)
        {
            
            TextRange range = new TextRange(rt.Document.find, rt.Document.ContentEnd);
            using (MemoryStream stream = new MemoryStream())
            {
                range.Save(stream, DataFormats.Xaml);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static void ImportXaml(this RichTextBox rt, string xaml)
        {
            rt.Document = new FlowDocument();
            rt.AppendXaml(xaml);
        }

        public static void AppendXaml(this RichTextBox rt, string xaml)
        {
            
            StringReader stringReader = new StringReader(xaml);
            using (XmlReader xmlReader = XmlReader.Create(stringReader))
            {
                Section sec = System.Windows.Markup.XamlReader.Load(xmlReader) as Section;
                while (sec.Blocks.Count > 0)
                    rt.Document.Blocks.Add(sec.Blocks.FirstBlock);
            }
        } */
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