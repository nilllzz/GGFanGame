using System.Collections.Generic;

namespace GGFanGame.Game.Scene
{
    internal abstract class StageGenerator
    {
        internal abstract List<StageObject> generate(Stage stage);
    }
}
