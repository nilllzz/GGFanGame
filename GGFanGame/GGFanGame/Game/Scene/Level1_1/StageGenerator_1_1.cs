using System.Collections.Generic;
using GGFanGame.Game.Scene.GrumpSpace.Enemies;

namespace GGFanGame.Game.Scene.Level1_1
{
    internal class StageGenerator_1_1 : StageGenerator
    {
        internal override List<StageObject> generate(Stage stage)
        {
            return new List<StageObject>
            {
                new GrumpSpace.Couch {X = 110, Y = 0, Z = 320},
                new GrumpSpace.ArcadeMachine(GrumpSpace.ArcadeType.Ninja) {X = 310, Y = 0, Z = 320},
                new BridgeRailing {X = 64, Y = 0, Z = 158},
                new BridgeRailing {X = 64, Y = 0, Z = 190},
                new Street {X = 64, Y = 0, Z = 190},
                new BridgeRailing {X = 128, Y = 10, Z = 158},
                new BridgeRailing {X = 128, Y = 10, Z = 190},
                new Street {X = 128, Y = 10, Z = 190},
                new BridgeRailing {X = 192, Y = 20, Z = 158},
                new BridgeRailing {X = 192, Y = 20, Z = 190},
                new Street {X = 192, Y = 20, Z = 190},
                new BridgeRailing {X = 256, Y = 30, Z = 158},
                new BridgeRailing {X = 256, Y = 30, Z = 190},
                new Street {X = 256, Y = 30, Z = 190},
                new BridgeRailing {X = 320, Y = 40, Z = 158},
                new BridgeRailing {X = 320, Y = 40, Z = 190},
                new Street {X = 320, Y = 40, Z = 190},
                new Booper {X = 280, Y = 0, Z = 250}
            };
        }
    }
}
