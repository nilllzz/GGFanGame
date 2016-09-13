using GGFanGame.DataModel.Json.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Stage1_1
{
    [StageObject("bridgeBottom", "1", "1")]
    internal class BridgeBottom : SceneryObject
    {
        public BridgeBottom()
        {
            size = new Vector3(128, 64, 64);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            sortLowest = true;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 64), 100));

        }

        public override void applyDataModel(StageObjectModel dataModel)
        {
            spriteSheet = content.Load<Texture2D>(@"Levels\Stage1_1\BridgeBottom");
        }
    }
}
