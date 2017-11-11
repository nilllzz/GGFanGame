using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Game
{
    [DataContract]
    internal class StageSceneModel : DataModel<StageSceneModel>
    {
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "objects")]
        public StageObjectModel[] Objects;
    }
}
