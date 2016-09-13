using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Json.Game
{
    [DataContract]
    internal class StageListMapEntryModel : DataModel<StageListMapEntryModel>
    {
        [DataMember]
        public string worldId;
        [DataMember]
        public string stageId;
        [DataMember]
        public string path;
    }
}
