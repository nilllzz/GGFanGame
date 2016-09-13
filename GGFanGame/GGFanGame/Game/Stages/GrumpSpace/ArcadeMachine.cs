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
            size = new Vector3(30, 60, 11);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            canInteract = true;
            weight = 20;
            faceAttack = false;

            addBoundingBox(new Vector3(30, 58, 10), new Vector3(0, 29, -2));
            zSortingOffset = -7;   // -(boundingBox.Z / 2) - offset.Z

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(30, 65), 100));
            addAnimation(ObjectState.Hurt, new Animation(1, Point.Zero, new Point(30, 65), 100));
            addAnimation(ObjectState.HurtFalling, new Animation(1, Point.Zero, new Point(30, 65), 100));
        }

        protected override void loadInternal()
        {
            spriteSheet = content.Load<Texture2D>($@"Levels\GrumpSpace\Arcade{(int)_arcadeType}");
        }

        public override void applyDataModel(StageObjectModel dataModel)
        {
            base.applyDataModel(dataModel);

            _arcadeType = parseArcadeType(dataModel.arguments[0]);
        }

        private static ArcadeType parseArcadeType(string input)
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