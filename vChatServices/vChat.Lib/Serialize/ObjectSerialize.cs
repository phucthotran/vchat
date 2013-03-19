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
            MemoryStream mStream = new MemoryStream();
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            ser.WriteObject(mStream, obj);
            mStream.Position = 0;
            StreamReader sReader = new StreamReader(mStream);

            String xmlString = sReader.ReadToEnd();

            return new XmlTextObject(xmlString);
        }

        /// <summary>
        /// Convert XmlTextObject to real Object
        /// </summary>
        /// <param name="obj">XmlTextObject to be convert</param>
        /// <returns></returns>
        public static T ParseToObject(XmlTextObject obj)
        {
            DataContractSerializer dataContractSer = new DataContractSerializer(typeof(T));
            StringReader stringReader = new StringReader(obj.SerializableObjectXml);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            return dataContractSer.ReadObject(xmlReader) as T;
        }
    }

    [DataContract(IsReference = true)]
    public class XmlTextObject
    {
        [DataMember]
        public String SerializableObjectXml { get; private set; }

        public XmlTextObject(String XmlSerializableObj)
        {
            this.SerializableObjectXml = XmlSerializableObj;
        }

        public override string ToString()
        {
            return SerializableObjectXml;
        }
    }
}
