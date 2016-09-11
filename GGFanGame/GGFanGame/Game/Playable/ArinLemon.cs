using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using static GameProvider;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// The Lemon projectile Arin throws in his ABA combo.
    /// </summary>
    class ArinLemon : InteractableStageObject
    {
        private int _ticksAlive = 0;

        public ArinLemon(Vector3 startPosition, ObjectFacing facing)
        {
            spriteSheet = gameInstance.textureManager.load(@"Sprites\ArinLemon");
            this.facing = facing;
            size = new Vector3(4f, 4f, 8f);

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(7, 6), 20));

            position = startPosition;
            canInteract = false;
        }

        public override void update()
        {
            if (facing == ObjectFacing.Left)
            {
                X -= 5f;
            }
            else
            {
                X += 5f;
            }

            int hits = Stage.activeStage().applyAttack(new Attack(this, false, 8, 4f, size, Vector3.Zero, facing), position, 1);

            if (hits > 0)
            {
                canBeRemoved = true;
            }
            else
            {
                //When this didn't hit anything, check if it collides with things.
                //If it does, remove it from the stage:
                if (Stage.activeStage().intersects(this, position))
                {
                    canBeRemoved = true;
                }
                else
                {
                    if (_ticksAlive == 70) //If this didnt hit anything after 70 ticks, destroy it.
                    {
                        canBeRemoved = true;
                    }
                }
            }
            _ticksAlive++;
        }
    }
}