using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Level.Enemies
{
    /// <summary>
    /// An enemy for a dojo stage.
    /// </summary>
    class Booper : Enemy
    {
        public Booper(GGGame game) : base(game)
        {
            spriteSheet = game.textureManager.getResource(@"Sprites\Booper");
            drawShadow = true;
            shadowSize = 0.6d;
            strength = 0f;
            weigth = 12f;
            state = ObjectState.Idle;
            size = new Vector3(120, 120, 20);

            addAnimation(ObjectState.Idle, new Animation(10, Point.Zero, new Point(64, 64), 6));
            addAnimation(ObjectState.Hurt, new Animation(1, new Point(0, 64), new Point(64, 64), 20, 1));
            addAnimation(ObjectState.HurtFalling, new Animation(1, new Point(0, 64), new Point(64, 64), 20, 1));
            addAnimation(ObjectState.Dead, new Animation(10, new Point(0, 128), new Point(64, 64), 3));

            health = 50;
        }

        public override void update()
        {
            base.update();

            if (state == ObjectState.Idle)
            {
                if (Stage.activeStage().onePlayer.X < X)
                {
                    facing = ObjectFacing.Left;
                }
                else
                {
                    facing = ObjectFacing.Right;
                }
            }
        }
    }
}