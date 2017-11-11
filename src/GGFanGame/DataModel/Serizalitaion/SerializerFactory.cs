namespace GGFanGame.DataModel.Serizalitaion
{
    internal static class SerializerFactory<T> where T : DataModel<T>
    {
        /// <summary>
        /// Returns a data serializer based on the data.
        /// </summary>
        private static IDataSerializer<T> GetSerializer(string data)
        {
            // this method tries to identify the type of data, if it's either Xml or Json.
            // Json does not have "comment outside of model" support, so we can check if it either stars with the object/array notation:

            string trimmed = data.Trim();
            if (trimmed.StartsWith("[") || trimmed.StartsWith("{"))
            {
                return GetSerializer(DataType.Json);
            }
            // otherwise we assume it's Xml:
            else
            {
                return GetSerializer(DataType.Xml);
            }
        }
        
        /// <summary>
        /// Returns the appropriate data serializer.
        /// </summary>
        public static IDataSerializer<T> GetSerializer(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Json:
                    return new JsonDataSerializer<T>();
                case DataType.Xml:
                    return new XmlDataSerializer<T>();
                default:
                    return null;
            }
        }
    }
}
