using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace GGFanGame.DataModel.Base
{
    [DataContract]
    internal class ColorModel : DataModel<ColorModel>
    {
        [DataMember(Name = "r", Order = 0)]
        public int r;
        [DataMember(Name = "g", Order = 1)]
        public int g;
        [DataMember(Name = "b", Order = 2)]
        public int b;

        internal Color toColor() => new Color(r, g, b);
    }
}
