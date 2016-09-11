using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using static GameProvider;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// The bomb Arin throws in his AAA combo.
    /// </summary>
    class ArinBomb : InteractableStageObject
    {
        private Vector3 _movement;

        public ArinBomb(Vector3 movement, Vector3 startPosition, ObjectFacing facing)
        {
            spriteSheet = gameInstance.textureManager.load(@"Sprites\ArinBomb");
            shadowSize = 0.8f;
            this.facing = facing;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(32, 32), 20));

            position = startPosition;
            canInteract = false;
            _movement = movement;
        }

        public override void update()
        {
            float groundY = Stage.activeStage().getGround(getFeetPosition());

            X += _movement.X;
            Z += _movement.Z;

            //Check if the bomb hit something, then explode.
            if (Stage.activeStage().intersects(this, position))
            {
                explode();
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
                    explode();
                }
            }
        }

        private void explode()
        {
            canBeRemoved = true;
            Stage.activeStage().applyExplosion(this, position, 50f, 10, 9f);
        }
    }
}