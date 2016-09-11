using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGFanGame.Game.Scene.Level1_1
{
    class StageGenerator_1_1 : StageGenerator
    {
        internal override List<StageObject> generate(Stage stage)
        {
            var objects = new List<StageObject>();

            objects.Add(new GrumpSpace.Couch() { X = 110, Y = 0, Z = 320 });
            objects.Add(new GrumpSpace.ArcadeMachine(GrumpSpace.ArcadeType.Ninja) { X = 310, Y = 0, Z = 320 });

            objects.Add(new BridgeRailing() { X = 64, Y = 0, Z = 158 });
            objects.Add(new BridgeRailing() { X = 64, Y = 0, Z = 190 });
            objects.Add(new Street() { X = 64, Y = 0, Z = 190 });
            objects.Add(new BridgeRailing() { X = 128, Y = 10, Z = 158 });
            objects.Add(new BridgeRailing() { X = 128, Y = 10, Z = 190 });
            objects.Add(new Street() { X = 128, Y = 10, Z = 190 });
            objects.Add(new BridgeRailing() { X = 192, Y = 20, Z = 158 });
            objects.Add(new BridgeRailing() { X = 192, Y = 20, Z = 190 });
            objects.Add(new Street() { X = 192, Y = 20, Z = 190 });
            objects.Add(new BridgeRailing() { X = 256, Y = 30, Z = 158 });
            objects.Add(new BridgeRailing() { X = 256, Y = 30, Z = 190 });
            objects.Add(new Street() { X = 256, Y = 30, Z = 190 });
            objects.Add(new BridgeRailing() { X = 320, Y = 40, Z = 158 });
            objects.Add(new BridgeRailing() { X = 320, Y = 40, Z = 190 });
            objects.Add(new Street() { X = 320, Y = 40, Z = 190 });

            return objects;
        }
    }
}
