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
        [DataMember]
        public double x;
        [DataMember]
        public double y;
        [DataMember]
        public double z;
        [DataMember(Name = "t")]
        public string type;
        [DataMember(Name = "args")]
        public string[] arguments;

        public Vector3 position => new Vector3((float)x, (float)y, (float)z);
    }
}
