using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Json.Game
{
    /// <summary>
    /// The data model for a <see cref="GameSession"/>.
    /// </summary>
    [DataContract]
    internal class GameSessionModel : JsonDataModel
    {
        [DataMember]
        public string name;

        [DataMember]
        public decimal progress;

        [DataMember]
        public string lastGrump;
    }
}