using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Game
{
    /// <summary>
    /// The data model for a <see cref="GameSession"/>.
    /// </summary>
    [DataContract]
    internal class GameSessionModel : DataModel<GameSessionModel>
    {
        [DataMember]
        public string name;

        [DataMember]
        public decimal progress;

        [DataMember]
        public string lastGrump;
    }
}