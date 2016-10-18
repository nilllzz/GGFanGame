using System;

namespace GGFanGame.Game
{
    /// <summary>
    /// An attribute to be added to a <see cref="StageObject"/> that can be loaded from a file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class StageObjectAttribute : Attribute
    {
        /// <summary>
        /// The formatted stage information string (worldId/stageId/name)
        /// </summary>
        public string StageInformation { get; }

        public StageObjectAttribute(string name, string worldId, string stageId)
        {
            StageInformation = $"{worldId}/{stageId}/{name}";
        }
    }
}
