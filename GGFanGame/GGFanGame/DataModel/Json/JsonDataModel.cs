using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace GGFanGame.DataModel.Json
{
    /// <summary>
    /// The JsonDataModel is the base class for all data models in the game.
    /// </summary>
    [DataContract]
    abstract class JsonDataModel
    {
        //This class will manage the Json data models for this game.
        //Levels, saves etc. will be saved in the Json format.

        protected JsonDataModel()
        { /*Empty constructor*/ }

        /// <summary>
        /// Creates a data model of a specific type.
        /// </summary>
        /// <typeparam name="T">The data model type.</typeparam>
        /// <param name="input">The input Json string.</param>
        public static T fromString<T>(string input)
        {
            //We create a new Json serializer of the given type and a corresponding memory stream here.
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream memStream = new MemoryStream();

            //Create StreamWriter to the memory stream, which writes the input string to the stream.
            StreamWriter sw = new StreamWriter(memStream);
            sw.Write(input);
            sw.Flush();

            //Reset the stream's position to the beginning:
            memStream.Position = 0;

            try
            {
                //Create and return the object:
                T returnObj = (T)serializer.ReadObject(memStream);
                return returnObj;
            }
            catch (Exception ex)
            {
                throw new JsonDataLoadException(input, typeof(T), ex);
            }
        }

        /// <summary>
        /// Returns the Json representation of this object.
        /// </summary>
        public override string ToString()
        {
            //We create a new Json serializer of the given type and a corresponding memory stream:
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(this.GetType());
            MemoryStream memStream = new MemoryStream();

            //Write the data to the stream:
            serializer.WriteObject(memStream, this);

            //Reset the stream's position to the beginning:
            memStream.Position = 0;

            //Create a stream reader, read string and return it:
            StreamReader sr = new StreamReader(memStream);
            string returnJson = sr.ReadToEnd();

            return returnJson;
        }
    }

    /// <summary>
    /// The exception that occurs when the serialization of Json data failed.
    /// </summary>
    class JsonDataLoadException : Exception
    {
        private const string MESSAGE = "An exception occured trying to read Json data into an internal format. Please check that the input data is correct.";

        /// <summary>
        /// Creates a new instance of the JsonDataLoadException class.
        /// </summary>
        /// <param name="jsonData">The json data that caused the problem.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="inner">The inner exception thrown.</param>
        public JsonDataLoadException(string jsonData, Type targetType, Exception inner) : base(MESSAGE, inner)
        {
            Data.Add("Target type", targetType.Name);
            Data.Add("Json data", jsonData);
        }
    }
}