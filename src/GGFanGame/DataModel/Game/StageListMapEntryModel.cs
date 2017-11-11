using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// Model of an entry for the <see cref="StageListModel"/>.
    /// </summary>
    [DataContract]
    internal class StageListMapEntryModel : DataModel<StageListMapEntryModel>
    {
        [DataMember(Name = "worldId")]
        public string WorldId;
        [DataMember(Name = "stageId")]
        public string StageId;
        [DataMember(Name = "path")]
        public string Path;
    }
}
