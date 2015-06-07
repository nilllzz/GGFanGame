using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Level.Playable
{
    /// <summary>
    /// The bomb Arin throws in his AAA combo.
    /// </summary>
    class ArinBomb : InteractableStageObject
    {
        private Vector3 _movement;

        public ArinBomb(GGGame game, Vector3 movement, Vector3 startPosition) : base(game)
        {
            spriteSheet = game.textureManager.load(@"Sprites\ArinBomb");
            shadowSize = 0.8f;

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

            if (Y > groundY)
            {
                _movement.Y -= 0.8f;
            }

            Y += _movement.Y;

            if (Y < groundY)
            {
                Y = groundY;
                canBeRemoved = true;
            }
        }
    }
}