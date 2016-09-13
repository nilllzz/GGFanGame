using System.IO;
using GGFanGame.DataModel;
using Microsoft.Xna.Framework.Content;

namespace GGFanGame // no "Content" to have it be accessible anywhere.
{
    static class DataModelContentExtension
    {
        public static T Load<T>(this ContentManager content, string assetName, DataType dataType) where T : DataModel<T>
        {
            // TODO: fix json file name hardcoded and add Xml serializer

            var assetPath = Path.Combine(content.RootDirectory, assetName);

            if (!assetPath.EndsWith(".json"))
            {
                assetPath = assetPath + ".json";
            }

            var source = File.ReadAllText(assetPath);

            return DataModel<T>.fromString(source, dataType);
        }
    }
}
