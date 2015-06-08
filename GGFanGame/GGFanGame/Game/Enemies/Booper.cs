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
            spriteSheet = game.textureManager.load(@"Sprites\Booper");
            drawShadow = true;
            shadowSize = 0.6d;
            strength = 0f;
            weight = 4f;
            state = ObjectState.Idle;
            size = new Vector3(40, 48, 10);

            addAnimation(ObjectState.Idle, new Animation(6, Point.Zero, new Point(64, 64), 6));
            addAnimation(ObjectState.Hurt, new Animation(7, new Point(0, 64), new Point(64, 64), 4));
            addAnimation(ObjectState.HurtFalling, new Animation(7, new Point(0, 64), new Point(64, 64), 4));
            addAnimation(ObjectState.Dead, new Animation(6, new Point(0, 128), new Point(64, 64), 4));

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