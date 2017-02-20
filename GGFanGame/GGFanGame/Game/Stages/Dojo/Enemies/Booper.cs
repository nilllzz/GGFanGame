using GGFanGame.Content;
using GGFanGame.Rendering;
using GGFanGame.Rendering.Composers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Stages.Dojo.Enemies
{
    /// <summary>
    /// An enemy for a dojo stage.
    /// </summary>
    [StageObject("booper", "grumpSpace", "dojo")]
    internal class Booper : Enemy
    {
        public override int Score => 100;

        public Booper()
        {
            DrawShadow = true;
            ShadowSize = 0.6d;
            Strength = 0f;
            Weight = 4f;
            State = ObjectState.Idle;
            Size = new Vector3(40, 48, 10);
            MaxHealth = 50;

            AddAnimation(ObjectState.Idle, new Animation(6, Point.Zero, new Point(64, 64), 6));
            AddAnimation(ObjectState.Hurt, new Animation(7, new Point(0, 64), new Point(64, 64), 4));
            AddAnimation(ObjectState.HurtFalling, new Animation(7, new Point(0, 64), new Point(64, 64), 4));
            AddAnimation(ObjectState.Dead, new Animation(6, new Point(0, 128), new Point(64, 64), 4));

            OnDeath += OnDeathHandler;
        }

        protected override void LoadContentInternal()
        {
            SpriteSheet1 = new SpriteSheet(ParentStage.Content.Load<Texture2D>(Resources.Sprites.Booper));
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(64f, 64f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            Geometry.AddVertices(vertices);
        }

        public override void Update()
        {
            base.Update();

            //This enemy always faces the player:
            if (State == ObjectState.Idle)
            {
                if (ParentStage.OnePlayer.X < X)
                {
                    Facing = ObjectFacing.Left;
                }
                else
                {
                    Facing = ObjectFacing.Right;
                }
            }
        }

        private void OnDeathHandler(StageObject obj)
        {
            ParentStage.AddObject(new GroundSplat(new Color(255, 128, 255)) { Position = Position });

            for (var i = 0; i < 3; i++)
            {
                float xMovement = ParentStage.Random.Next(5, 10);
                float zMovement = ParentStage.Random.Next(-3, 4);
                if (Facing == ObjectFacing.Right)
                {
                    xMovement *= -1;
                }

                var G = 128;
                if (ParentStage.Random.Next(0, 2) == 0)
                {
                    G = 174;
                }

                ParentStage.AddObject(new SplatBall(new Color(255, G, 255), new Vector3(xMovement, 3f, zMovement)) { Position = Position });
            }
        }
    }
}
