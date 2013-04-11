using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Windows;

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
/*
        public static void Add(this Window wd, Control item, List<Control> list, bool beingChild)
        {
            if (beingChild)
                wd.Controls.Add(item);
            list.Add(item);
        }
        */
        public static void InvokeEx<T>(this T @this, Action<T> action)
          where T : Control
        {
            if (@this.InvokeRequired)
            {
                @this.Invoke(action, new object[] { @this });
            }
            else
            {
                if (!@this.IsHandleCreated)
                    return;
                if (@this.IsDisposed)
                    throw new ObjectDisposedException("@this is disposed.");

                action(@this);
            }
        }

        public static IAsyncResult BeginInvokeEx<T>(this T @this, Action<T> action)
          where T : Control
        {
            return @this.BeginInvoke((Action)delegate { @this.InvokeEx(action); });
        }

        public static void EndInvokeEx<T>(this T @this, IAsyncResult result)
          where T : Control
        {
            @this.EndInvoke(result);
        }
    }
}
