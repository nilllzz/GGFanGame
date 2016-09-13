using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// Model that contains a map of all stage ids with their respective file paths.
    /// </summary>
    [DataContract]
    internal class StageListModel : DataModel<StageListModel>
    {
        [DataMember]
        public StageListMapEntryModel[] stages;
    }
}
