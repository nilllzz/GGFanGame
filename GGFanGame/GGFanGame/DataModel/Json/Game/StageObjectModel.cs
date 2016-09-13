using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace GGFanGame.DataModel.Json.Game
{
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
