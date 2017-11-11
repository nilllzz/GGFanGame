using GameDevCommon.Drawing;
using GameDevCommon.Rendering;
using GameDevCommon.Rendering.Composers;
using Microsoft.Xna.Framework;
using System;

namespace GGFanGame.Game.Stages
{
    /// <summary>
    /// A ball shot by a gun that splats on the ground upon contact.
    /// </summary>
    internal class SplatBall : InteractableStageObject
    {
        private readonly Random _random = new Random();

        public SplatBall(Color color, Vector3 movement)
        {
            IsOpaque = false;

            Initialize(color, movement);
        }

        public SplatBall(Color color, ObjectFacing setFacing)
        {
            float xMovement = _random.Next(10, 20);
            float yMovement = _random.Next(0, 10);
            float zMovement = _random.Next(-5, 5);
            if (setFacing == ObjectFacing.Left)
            {
                AutoMovement.X = -xMovement;
            }
            else
            {
                AutoMovement.X = xMovement;
            }

            Initialize(color, new Vector3(xMovement, yMovement, zMovement));
        }

        private void Initialize(Color color, Vector3 movement)
        {
            const int ellipseAmount = 3;
            var ellipses = new EllipseHelper.SimpleEllipse[ellipseAmount];

            for (var i = 0; i < ellipseAmount; i++)
            {
                var width = _random.Next(4, 8);
                var height = _random.Next(4, 8);
                var x = _random.Next(0, 16 - width);
                var y = _random.Next(0, 16 - height);

                ellipses[i].Bounds = new Rectangle(x, y, width, height);
                ellipses[i].FillColor = color;
            }

            SpriteSheet = new SpriteSheet(EllipseHelper.CreateJoined(
                16,
                16,
                ellipses
            ));

            ObjectColor = color;
            CanInteract = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(16, 16), 100));

            AutoMovement = movement;
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(0.25f, 0.25f);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            Geometry.AddVertices(vertices);
        }

        public override void Update()
        {
            base.Update();

            var groundY = ParentStage.GetSupporting(this).objY;

            if (Y <= groundY)
            {
                CanBeRemoved = true;
                ParentStage.AddObject(new GroundSplat(ObjectColor) { Position = Position });
            }
        }
    }
}
