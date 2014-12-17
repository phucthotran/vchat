using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace vChat.Lib.Serialize
{
    /// <summary>
    /// Serialize an object to Xml text
    /// </summary>
    /// <typeparam name="T">Object type to be serialize</typeparam>
    public class ObjectSerialize<T> where T : class
    {
        /// <summary>
        /// Convert an object to XmlTextObject
        /// </summary>
        /// <param name="obj">Object need to be convert</param>
        /// <returns></returns>        
        public static XmlTextObject ParseToXml(T obj)
        {
            MemoryStream mmStream = new MemoryStream();
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            ser.WriteObject(mmStream, obj);
            mmStream.Position = 0;
            StreamReader sReader = new StreamReader(mmStream);

            return new XmlTextObject (XmlSerializableObj : sReader.ReadToEnd());
        }

        /// <summary>
        /// Convert XmlTextObject to real Object
        /// </summary>
        /// <param name="obj">XmlTextObject to be convert</param>
        /// <returns></returns>
        public static T ParseToObject(XmlTextObject obj)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            StringReader stringReader = new StringReader(obj.XmlSerialize);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            return ser.ReadObject(xmlReader) as T;
        }
    }

    [DataContract(IsReference = true)]
    public class XmlTextObject
    {
        [DataMember]
        public String XmlSerialize { get; private set; }

        public XmlTextObject(String XmlSerializableObj)
        {
            this.XmlSerialize = XmlSerializableObj;
        }

        public override string ToString()
        {
            return XmlSerialize;
        }
    }
}
