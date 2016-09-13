using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Json.Game
{
    [DataContract]
    internal class StageModel : DataModel<StageModel>
    {
        [DataMember]
        public string name;
        [DataMember]
        public string worldId;
        [DataMember]
        public string stageId;
        [DataMember]
        public StageObjectModel[] objects;
    }
}
