using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Base
{
    [DataContract]
    internal class Vector3Model : DataModel<Vector3Model>
    {
        [DataMember(Name = "x", Order = 0)]
        public int X;
        [DataMember(Name = "y", Order = 1)]
        public int Y;
        [DataMember(Name = "z", Order = 2)]
        public int Z;

        internal Vector3 ToVector3() => new Vector3(X, Y, Z);
    }
}
