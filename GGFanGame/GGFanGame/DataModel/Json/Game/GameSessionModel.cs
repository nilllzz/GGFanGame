using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GGFanGame.DataModel.Json.Game
{
    [DataContract]
    class GameSessionModel : JsonDataModel
    {
        [DataMember]
        public string name;

        [DataMember]
        public decimal progress;

        [DataMember]
        public string lastGrump;
    }
}