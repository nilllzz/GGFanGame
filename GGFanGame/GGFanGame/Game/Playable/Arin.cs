using GGFanGame.Content;
using GGFanGame.Rendering;
using GGFanGame.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// Playable Arin character.
    /// </summary>
    internal class Arin : PlayerCharacter
    {
        public override string Name => "Arin";
        public override int MaxGrumpPower => 100;

        public Arin(PlayerIndex playerIndex)
            : base(playerIndex)
        {
            SpriteSheet1 = new SpriteSheet(GameInstance.Content.Load<Texture2D>(Resources.Sprites.Arin));
            DrawShadow = true;
            ShadowSize = 0.48d;
            Strength = 4;
            Weight = 4;
            Size = new Vector3(32, 50, 5);
            MaxHealth = 100;
            PlayerSpeed = 4f;

            AddAnimation(ObjectState.Idle, new Animation(8, Point.Zero, new Point(64, 64), 7));
            AddAnimation(ObjectState.Walking, new Animation(6, new Point(0, 64), new Point(64, 64), 5));
            AddAnimation(ObjectState.Hurt, new Animation(1, new Point(0, 128), new Point(64, 64), 5, 3));
            AddAnimation(ObjectState.HurtFalling, new Animation(5, new Point(0, 128), new Point(64, 64), 7, 5));
            AddAnimation(ObjectState.StandingUp, new Animation(5, new Point(256, 128), new Point(64, 64), 9));
            AddAnimation(ObjectState.Dead, new Animation(1, new Point(256, 128), new Point(64, 64), 1));
            AddAnimation(ObjectState.Jumping, new Animation(6, new Point(0, 192), new Point(64, 64), 4));
            AddAnimation(ObjectState.Falling, new Animation(2, new Point(0, 256), new Point(64, 64), 4));
            AddAnimation(ObjectState.Blocking, new Animation(1, new Point(576, 0), new Point(64, 64), 1));
            AddAnimation(ObjectState.Dashing, new Animation(6, new Point(0, 576), new Point(64, 64), 3));
            AddAnimation(ObjectState.JumpAttacking, new Animation(3, new Point(448, 192), new Point(64, 64), 3));

            var B1 = new PlayerAttack(new Animation(5, new Point(0, 320), new Point(64, 64), 5, 1), new Vector2(6f, 0f));
            B1.AddAttack(2, new AttackDefinition(new Attack(this, false, 5, Strength, new Vector3(15), new Vector3(20, 10, 0)), 2));

            var B2 = new PlayerAttack(new Animation(3, new Point(320, 320), new Point(64, 64), 5), new Vector2(6f, 0f));
            B2.AddAttack(1, new AttackDefinition(new Attack(this, true, 5, Strength * 1.3f, new Vector3(15), new Vector3(20, 10, 0)), 2));

            AddAttack("B", B1);
            AddAttack("BB", B2);

            var A1 = new PlayerAttack(new Animation(4, new Point(0, 384), new Point(64, 64), 5, 1), new Vector2(6f, 0f));
            A1.AddAttack(2, new AttackDefinition(new Attack(this, false, 3, Strength, new Vector3(15), new Vector3(20, 10, 0)), 1));

            var A2 = new PlayerAttack(new Animation(4, new Point(256, 384), new Point(64, 64), 5, 1), new Vector2(6f, 7f));
            A2.AddAttack(1, new AttackDefinition(new Attack(this, true, 3, Strength * 1.3f, new Vector3(15), new Vector3(20, 10, 0)), 1));

            var A3 = new PlayerAttack(new Animation(6, new Point(0, 448), new Point(64, 64), 9), new Vector2(-5f, 0f));
            A3.AddAttack(3, new AttackDefinition(null, 0, ThrowBomb));

            AddAttack("A", A1);
            AddAttack("AA", A2);
            AddAttack("AAA", A3);

            var B4 = new PlayerAttack(new Animation(4, new Point(0, 512), new Point(64, 64), 6, 1), new Vector2(3f, 0f));
            B4.AddAttack(2, new AttackDefinition(new Attack(this, true, 5, Strength, new Vector3(15), new Vector3(24, 10, 0)), 1));

            var A4 = new PlayerAttack(new Animation(1, new Point(320, 512), new Point(64, 64), 5, 3), new Vector2(-3f, 0f));
            A4.AddAttack(1, new AttackDefinition(null, 0, ThrowLemon));

            AddAttack("AB", B4);
            AddAttack("ABA", A4);
        }
        
        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(1f, 1f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            Geometry.AddVertices(vertices);
        }

        /// <summary>
        /// Callback void to throw a bomb in the AAA combo.
        /// </summary>
        private void ThrowBomb(AttackDefinition attack)
        {
            float xDirection = 5;
            if (Facing == ObjectFacing.Left)
                xDirection = -5;

            ParentStage.AddObject(new ArinBomb(new Vector3(xDirection, 12, 0), new Vector3(X, Y + 10, Z), Facing));
        }

        /// <summary>
        /// Callback void to throw a lemon in the ABA combo.
        /// </summary>
        private void ThrowLemon(AttackDefinition attack)
        {
            float xOffset = 16;
            if (Facing == ObjectFacing.Left)
            {
                xOffset = -16;
            }

            ParentStage.AddObject(new ArinLemon(new Vector3(X + xOffset, Y + 27, Z), Facing));
        }
    }
}
