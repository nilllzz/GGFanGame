using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// Model describing a <see cref="GGFanGame.Game.Stage"/>.
    /// </summary>
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
