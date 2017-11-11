using GGFanGame.DataModel.Base;
using GGFanGame.Drawing;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace GGFanGame.DataModel.Game
{
    [DataContract]
    internal class StageLightModel : DataModel<StageLightModel>
    {
        [DataMember(Name = "direction", Order = 0)]
        Vector3Model Direction;
        [DataMember(Name = "color", Order = 1)]
        ColorModel Color;
        [DataMember(Name = "intensity", Order = 2)]
        float Intensity;

        internal DirectionalLightConfiguration Config => new DirectionalLightConfiguration
        {
            Color = Color.ToColor(),
            Direction = Direction.ToVector3(),
            Intensity = Intensity
        };
    }
}
