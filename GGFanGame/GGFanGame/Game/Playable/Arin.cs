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
            spriteSheet = gameInstance.textureManager.load(@"Sprites\Arin");
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
            addAnimation(ObjectState.Jumping, new Animation(6, new Point(0, 192), new Point(64, 64), 4));
            addAnimation(ObjectState.Falling, new Animation(2, new Point(0, 256), new Point(64, 64), 4));
            addAnimation(ObjectState.Blocking, new Animation(1, new Point(576, 0), new Point(64, 64), 1));

            var B1 = new AttackCombo(new Animation(5, new Point(0, 320), new Point(64, 64), 5, 1), new Vector2(6f, 0f));
            B1.addAttack(2, new AttackDefinition(new Attack(this, false, 5, strength, new Vector3(15), new Vector3(20, 10, 0)), 2));

            var B2 = new AttackCombo(new Animation(3, new Point(320, 320), new Point(64, 64), 5), new Vector2(6f, 0f));
            B2.addAttack(1, new AttackDefinition(new Attack(this, true, 5, strength * 1.3f, new Vector3(15), new Vector3(20, 10, 0)), 2));

            addCombo("B", B1);
            addCombo("BB", B2);

            var A1 = new AttackCombo(new Animation(4, new Point(0, 384), new Point(64, 64), 5, 1), new Vector2(6f, 0f));
            A1.addAttack(2, new AttackDefinition(new Attack(this, false, 3, strength, new Vector3(15), new Vector3(20, 10, 0)), 1));

            var A2 = new AttackCombo(new Animation(4, new Point(256, 384), new Point(64, 64), 5, 1), new Vector2(6f, 7f));
            A2.addAttack(1, new AttackDefinition(new Attack(this, true, 3, strength * 1.3f, new Vector3(15), new Vector3(20, 10, 0)), 1));

            var A3 = new AttackCombo(new Animation(6, new Point(0, 448), new Point(64, 64), 9), new Vector2(-5f, 0f));
            A3.addAttack(3, new AttackDefinition(null, 0, new AttackDefinition.DAttackAction(throwBomb)));

            addCombo("A", A1);
            addCombo("AA", A2);
            addCombo("AAA", A3);

            health = 100;
            playerSpeed = 4f;
        }

        private void throwBomb(AttackDefinition attack)
        {
            float xDirection = 5;
            if (facing == ObjectFacing.Left)
                xDirection = -5;

            Stage.activeStage().addObject(new ArinBomb(gameInstance, new Vector3(xDirection, 12, 0), new Vector3(X, Y + 10, Z), facing));
        }
    }
}