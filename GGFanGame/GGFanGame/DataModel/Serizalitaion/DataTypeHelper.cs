using System;

namespace GGFanGame.DataModel.Serizalitaion
{
    /// <summary>
    /// Contains helper methods for the <see cref="DataType"/> enum.
    /// </summary>
    internal static class DataTypeHelper
    {
        /// <summary>
        /// Returns the file extension associated with a DataType.
        /// </summary>
        public static string getFileExtension(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Json:
                    return "json";
                case DataType.Xml:
                    return "xml";
                default:
                    throw new InvalidOperationException(dataType.ToString());
            }
        }
    }
}
