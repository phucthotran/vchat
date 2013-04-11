using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data
{

    [AttributeUsage(AttributeTargets.Method)]
    public class InvokeAttribute : Attribute
    {
        /// <summary>
        /// Kiểu command cần thực thi
        /// </summary>
        public CommandType CommandType { get; private set; }
        /// <summary>
        /// Xác nhận chuyển tiếp command đến đối tượng cần truyền. Mặc định là <code>true</code>.
        /// </summary>
        public bool Forward { get; set; }
        /// <summary>
        /// Thực thi command truyền đến
        /// </summary>
        /// <param name="cmdType">Kiểu command cần thực thi</param>
        public InvokeAttribute(CommandType cmdType)
        {
            this.Forward = true;
            this.CommandType = cmdType;
        }
    }
}
