using System;
using System.IO;
using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Serizalitaion
{
    /// <summary>
    /// Serializes and deserializes xml data.
    /// </summary>
    internal class XmlDataSerializer<T> : DataSerializer<T> where T : DataModel<T>
    {
        private const string XML_SCHEMA_INSTANCE = " xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"";

        public T fromString(string data)
        {
            // We create a new Xml serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractSerializer(typeof(T), new DataContractSerializerSettings() { SerializeReadOnlyTypes = true });
            var memStream = new MemoryStream();

            // Create StreamWriter to the memory stream, which writes the input string to the stream.
            var sw = new StreamWriter(memStream);
            sw.Write(data);
            sw.Flush();

            // Reset the stream's position to the beginning:
            memStream.Position = 0;

            try
            {
                // Create and return the object:
                T returnObj = (T)serializer.ReadObject(memStream);
                return returnObj;
            }
            catch (Exception ex)
            {
                // Exception occurs while loading the object due to malformed Xml.
                // Throw exception and move up to handler class.
                throw new DataLoadException(data, typeof(T), ex, DataType.Xml);
            }
        }

        public string toString(DataModel<T> dataModel)
        {
            // We create a new Xml serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractSerializer(dataModel.GetType(), new DataContractSerializerSettings() { SerializeReadOnlyTypes = true });
            var memStream = new MemoryStream();

            // Write the data to the stream.
            serializer.WriteObject(memStream, dataModel);

            // Reset the stream's position to the beginning:
            memStream.Position = 0;

            // Create stream reader, read string and return it.
            var sr = new StreamReader(memStream);
            string returnXml = sr.ReadToEnd();

            // we want to remove the instance note on this, so we do this the dirty way, as there is no other way...
            return returnXml.Replace(XML_SCHEMA_INSTANCE, "");
        }
    }
}
