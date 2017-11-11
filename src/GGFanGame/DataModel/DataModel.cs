using System;
using System.Runtime.Serialization;
using GGFanGame.DataModel.Serizalitaion;

namespace GGFanGame.DataModel
{
    /// <summary>
    /// The DataModel is the base class for all data models in the game.
    /// </summary>
    [DataContract]
    internal abstract class DataModel<T> where T : DataModel<T>
    {
        /// <summary>
        /// Creates a data model of a specific type.
        /// </summary>
        /// <typeparam name="T">The data model type.</typeparam>
        /// <param name="input">The input Json string.</param>
        public static T FromString(string input, DataType dataType)
        {
            return SerializerFactory<T>.GetSerializer(dataType).FromString(input);
        }

        /// <summary>
        /// Returns the data representation of this object.
        /// </summary>
        public string ToString(DataType dataType)
        {
            return SerializerFactory<T>.GetSerializer(dataType).ToString(this);
        }
    }

    /// <summary>
    /// The exception that occurs when the serialization of data failed.
    /// </summary>
    internal sealed class DataLoadException : Exception
    {
        private const string MESSAGE = "An exception occured trying to read Json data into an internal format. Please check that the input data is correct.";

        /// <summary>
        /// Creates a new instance of the DataLoadException class.
        /// </summary>
        /// <param name="data">The data that caused the problem.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="inner">The inner exception thrown.</param>
        /// <param name="dataType">The type of data that was being serialized.</param>
        public DataLoadException(string data, Type targetType, Exception inner, DataType dataType) : base(MESSAGE, inner)
        {
            Data.Add("Target type", targetType.Name);
            Data.Add("Data", data);
            Data.Add("Data Type", dataType);
        }
    }
}
