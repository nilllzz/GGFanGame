using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Base
{
    [DataContract]
    internal class ColorModel : DataModel<ColorModel>
    {
        [DataMember(Name = "r", Order = 0)]
        public int R;
        [DataMember(Name = "g", Order = 1)]
        public int G;
        [DataMember(Name = "b", Order = 2)]
        public int B;

        internal Color ToColor() => new Color(R, G, B);
    }
}
