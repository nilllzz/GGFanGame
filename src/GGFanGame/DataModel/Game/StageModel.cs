﻿using System.Runtime.Serialization;
using GGFanGame.DataModel.Base;

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
        [DataMember(Name = "name")]
        public string Name;
        [DataMember(Name = "worldId")]
        public string WorldId;
        [DataMember(Name = "stageId")]
        public string StageId;
        [DataMember(Name = "backColor")]
        public ColorModel BackColor;
        [DataMember(Name = "light")]
        public StageLightModel Light;
        [DataMember(Name = "scenes")]
        public StageSceneModel[] Scenes;
    }
}
