using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// The Lemon projectile Arin throws in his ABA combo.
    /// </summary>
    internal class ArinLemon : InteractableStageObject
    {
        private int _ticksAlive = 0;

        public ArinLemon(Vector3 startPosition, ObjectFacing facing)
        {
            Facing = facing;
            Size = new Vector3(4f, 4f, 8f);

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(7, 6), 20));

            Position = startPosition;
            CanInteract = false;
        }

        protected override void LoadContentInternal()
        {
            //SpriteSheet = GameInstance.Content.Load<Texture2D>(Resources.Sprites.ArinLemon);
        }

        public override void Update()
        {
            if (Facing == ObjectFacing.Left)
            {
                X -= 5f;
            }
            else
            {
                X += 5f;
            }

            var hits = ParentStage.ApplyAttack(new Attack(this, false, 8, 4f, Size, Vector3.Zero, Facing), Position, 1);

            if (hits > 0)
            {
                CanBeRemoved = true;
            }
            else
            {
                //When this didn't hit anything, check if it collides with things.
                //If it does, remove it from the stage:
                if (ParentStage.Intersects(this, Position))
                {
                    CanBeRemoved = true;
                }
                else
                {
                    if (_ticksAlive == 70) //If this didnt hit anything after 70 ticks, destroy it.
                    {
                        CanBeRemoved = true;
                    }
                }
            }
            _ticksAlive++;
        }
    }
}
