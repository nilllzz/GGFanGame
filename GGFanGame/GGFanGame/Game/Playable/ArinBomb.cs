using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// The bomb Arin throws in his AAA combo.
    /// </summary>
    internal class ArinBomb : InteractableStageObject
    {
        private Vector3 _movement;

        public ArinBomb(Vector3 movement, Vector3 startPosition, ObjectFacing facing)
        {
            Facing = facing;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(32, 32), 20));

            Position = startPosition;
            CanInteract = false;
            _movement = movement;
        }

        protected override void LoadContentInternal()
        {
            //SpriteSheet = GameInstance.Content.Load<Texture2D>(Resources.Sprites.ArinBomb);
        }

        public override void Update()
        {
            var groundY = ParentStage.GetSupporting(this).objY;

            X += _movement.X;
            Z += _movement.Z;

            //Check if the bomb hit something, then explode.
            if (ParentStage.Intersects(this, Position))
            {
                Explode();
            }
            else
            {
                if (Y > groundY)
                {
                    _movement.Y -= 0.8f;
                }

                Y += _movement.Y;

                if (Y < groundY)
                {
                    Y = groundY;
                    Explode();
                }
            }
        }

        private void Explode()
        {
            CanBeRemoved = true;
            ParentStage.ApplyExplosion(this, Position, 50f, 10, 9f);
        }
    }
}
