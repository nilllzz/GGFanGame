using System.IO;
using GGFanGame.DataModel;
using GGFanGame.DataModel.Serizalitaion;
using Microsoft.Xna.Framework.Content;

namespace GGFanGame // no "Content" to have it be accessible anywhere.
{
    /// <summary>
    /// Contains an extension method to load data models through a <see cref="ContentManager"/> directly.
    /// </summary>
    internal static class DataModelContentExtension
    {
        public static T Load<T>(this ContentManager content, string assetName, DataType dataType) where T : DataModel<T>
        {
            var fileExtension = DataTypeHelper.GetFileExtension(dataType);
            var assetPath = Path.Combine(content.RootDirectory, assetName);

            if (!assetPath.ToLowerInvariant().EndsWith(fileExtension))
                assetPath = assetPath + "." +  fileExtension;

            var source = File.ReadAllText(assetPath);

            return DataModel<T>.FromString(source, dataType);
        }
    }
}
