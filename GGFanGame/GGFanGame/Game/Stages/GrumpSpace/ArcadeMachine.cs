using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    [StageObject("arcadeMachine", "0", "1")]
    internal class ArcadeMachine : InteractableStageObject
    {
        private ArcadeType _arcadeType;

        public ArcadeMachine()
        {
            Size = new Vector3(30, 60, 11);
            DrawShadow = false;
            Collision = true;
            CanLandOn = true;
            CanInteract = true;
            Weight = 20;
            FaceAttack = false;

            AddBoundingBox(new Vector3(30, 58, 10), new Vector3(0, 29, -2));
            ZSortingOffset = -7;   // -(boundingBox.Z / 2) - offset.Z

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(30, 65), 100));
            AddAnimation(ObjectState.Hurt, new Animation(1, Point.Zero, new Point(30, 65), 100));
            AddAnimation(ObjectState.HurtFalling, new Animation(1, Point.Zero, new Point(30, 65), 100));
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet = ParentStage.Content.Load<Texture2D>($@"Levels\GrumpSpace\Arcade{(int)_arcadeType}");
        }

        public override void ApplyDataModel(StageObjectModel dataModel)
        {
            base.ApplyDataModel(dataModel);

            _arcadeType = ParseArcadeType(dataModel.GetArgValue("game"));
        }

        private static ArcadeType ParseArcadeType(string input)
        {
            switch (input.ToLowerInvariant())
            {
                case "ninja":
                    return ArcadeType.Ninja;
                default:
                    return ArcadeType.Ninja;
            }
        }
    }
}
