using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace Core.Data
{
    [Serializable()]
    public class CommandMetadata
    {
        public int Count
        {
            get
            {
                return this.Datas.Length;
            }
        }

        public object[] Datas { get; private set; }

        public CommandMetadata(params object[] objs)
        {
            this.Datas = objs;
        }
        /*
                public object[] GetAll()
                {
                    object[] objs = new object[this.Count];
                    for (int i = 0; i < this.Count; i++)
                    {
                        objs[i] = GetData<object>(i);
                    }
                    return objs;
                }

                public T GetData<T>(int index)
                {
                    byte[] bytes = Convert.FromBase64String(m_Datas[index]);
                    MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;
                    return (T)new BinaryFormatter().Deserialize(ms);
                }
                */
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < this.Count; i++)
            {
                result += this.Datas[i].ToString() + " :: ";
            }
            return result.Substring(0, result.Length - 4);
        }
    }
}
