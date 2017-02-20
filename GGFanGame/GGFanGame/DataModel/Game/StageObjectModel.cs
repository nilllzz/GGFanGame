using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// Model describing a <see cref="GGFanGame.Game.StageObject"/>.
    /// </summary>
    [DataContract]
    internal class StageObjectModel : DataModel<StageObjectModel>
    {
        [DataMember(Name = "x")]
        public double X;
        [DataMember(Name = "y")]
        public double Y;
        [DataMember(Name = "z")]
        public double Z;
        [DataMember(Name = "t")]
        public string Type;
        [DataMember(Name = "args")]
        public string[] Arguments;

        public Vector3 Position => new Vector3((float)X, (float)Y, (float)Z);

        internal bool HasArg(string arg)
            => Arguments != null && Arguments.Contains(arg);
    }
}
