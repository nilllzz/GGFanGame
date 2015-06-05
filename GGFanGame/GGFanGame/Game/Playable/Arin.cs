using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Level.Playable
{
    /// <summary>
    /// Playable Arin character.
    /// </summary>
    class Arin : PlayerCharacter
    {
        public Arin(GGGame game, PlayerIndex playerIndex) : base(game, playerIndex, "Arin")
        {
            spriteSheet = gameInstance.textureManager.getResource(@"Sprites\Arin");
            drawShadow = true;
            shadowSize = 0.5d;
            strength = 4;
            weigth = 4;
            size = new Vector3(32, 50, 5);

            addAnimation(ObjectState.Idle, new Animation(8, Point.Zero, new Point(64, 64), 7));
            addAnimation(ObjectState.Walking, new Animation(6, new Point(0, 64), new Point(64, 64), 5));
            addAnimation(ObjectState.Hurt, new Animation(1, new Point(0, 128), new Point(64, 64), 5, 3));
            addAnimation(ObjectState.HurtFalling, new Animation(5, new Point(0, 128), new Point(64, 64), 7, 5));
            addAnimation(ObjectState.StandingUp, new Animation(5, new Point(256, 128), new Point(64, 64), 9));
            addAnimation(ObjectState.Dead, new Animation(1, new Point(256, 128), new Point(64, 64), 1));
            addAnimation(ObjectState.Jumping, new Animation(6, new Point(0, 192), new Point(64,64), 4));
            addAnimation(ObjectState.Falling, new Animation(2, new Point(0, 256), new Point(64, 64), 4));
            addAnimation(ObjectState.Blocking, new Animation(1, new Point(576, 0), new Point(64, 64), 1));

            addComboAnimation("B", new Animation(5, new Point(0, 320), new Point(64, 64), 5, 1));
            addComboAnimation("BB", new Animation(3, new Point(320, 320), new Point(64, 64), 5));

            addAttack("B", 2, new Attack(this, false, 5, strength, new Vector3(15), new Vector3(20, 10, 0)));
            addAttack("BB", 1, new Attack(this, true, 5, strength * 1.3f, new Vector3(15), new Vector3(20, 10, 0)));

            health = 100;
            playerSpeed = 4f;
        }
    }
}