using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// An argument for a stage object that adds additional data.
    /// </summary>
    [DataContract]
    internal class StageObjectArgumentModel : DataModel<StageObjectArgumentModel>
    {
        [DataMember(Name = "key")]
        public string Key;
        [DataMember(Name = "value")]
        public string Value;
    }
}
