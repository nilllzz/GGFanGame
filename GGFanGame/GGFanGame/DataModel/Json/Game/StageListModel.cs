using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Json.Game
{
    [DataContract]
    internal class StageListModel : DataModel<StageListModel>
    {
        [DataMember]
        public StageListMapEntryModel[] stages;
    }
}
