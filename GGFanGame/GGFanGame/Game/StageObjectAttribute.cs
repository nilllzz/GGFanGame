using System;

namespace GGFanGame.Game
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class StageObjectAttribute : Attribute
    {
        public string stageInformation { get; }

        public StageObjectAttribute(string name, string worldId, string stageId)
        {
            stageInformation = $"{worldId}/{stageId}/{name}";
        }
    }
}
