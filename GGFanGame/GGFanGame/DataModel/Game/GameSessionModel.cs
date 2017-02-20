using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// The data model for a <see cref="GameSession"/>.
    /// </summary>
    [DataContract]
    internal class GameSessionModel : DataModel<GameSessionModel>
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "progress")]
        public decimal Progress;

        [DataMember(Name = "lastGrump")]
        public string LastGrump;
    }
}
