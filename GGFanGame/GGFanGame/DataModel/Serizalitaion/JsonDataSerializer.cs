using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace GGFanGame.DataModel.Serizalitaion
{
    /// <summary>
    /// Serializes and deserializes json data.
    /// </summary>
    internal class JsonDataSerializer<T> : DataSerializer<T> where T : DataModel<T>
    {
        public T FromString(string data)
        {
            // We create a new Json serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings() { SerializeReadOnlyTypes = true });
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
                // Exception occurs while loading the object due to malformed Json.
                // Throw exception and move up to handler class.
                throw new DataLoadException(data, typeof(T), ex, DataType.Json);
            }
        }

        public string ToString(DataModel<T> dataModel)
        {
            // We create a new Json serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractJsonSerializer(dataModel.GetType(), new DataContractJsonSerializerSettings() { SerializeReadOnlyTypes = true });
            var memStream = new MemoryStream();

            // Write the data to the stream.
            serializer.WriteObject(memStream, dataModel);

            // Reset the stream's position to the beginning:
            memStream.Position = 0;

            // Create stream reader, read string and return it.
            var sr = new StreamReader(memStream);
            string returnJson = sr.ReadToEnd();

            return returnJson;
        }
    }
}
