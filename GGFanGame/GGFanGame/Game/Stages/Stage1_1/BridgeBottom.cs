using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Stage1_1
{
    [StageObject("bridgeBottom", "1", "1")]
    internal class BridgeBottom : SceneryObject
    {
        public BridgeBottom()
        {
            Size = new Vector3(128, 64, 64);
            DrawShadow = false;
            Collision = true;
            CanLandOn = true;
            SortLowest = true;
            GravityAffected = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 64), 100));

        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            SpriteSheet = ParentStage.Content.Load<Texture2D>(Resources.Levels.Stage1_1.BridgeBottom);
        }
    }
}
