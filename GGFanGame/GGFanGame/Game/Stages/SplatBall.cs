using System.Collections.Generic;
using GGFanGame.Drawing;
using Microsoft.Xna.Framework;
using static Core;

namespace GGFanGame.Game.Stages
{
    /// <summary>
    /// A ball shot by a gun that splats on the ground upon contact.
    /// </summary>
    internal class SplatBall : InteractableStageObject
    {
        public SplatBall(Color color, Vector3 movement)
        {
            Initialize(color, movement);
        }

        public SplatBall(Color color, ObjectFacing setFacing)
        {
            float xMovement = ParentStage.Random.Next(10, 20);
            float yMovement = ParentStage.Random.Next(0, 10);
            float zMovement = ParentStage.Random.Next(-5, 5);
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
            var ellipses = new List<Rectangle>();
            var colors = new List<Color>();

            for (var i = 0; i < 3; i++)
            {
                var width = ParentStage.Random.Next(4, 8);
                var height = ParentStage.Random.Next(4, 8);
                var x = ParentStage.Random.Next(0, 16 - width);
                var y = ParentStage.Random.Next(0, 16 - height);

                ellipses.Add(new Rectangle(x, y, width, height));
                colors.Add(color);
            }

            SpriteSheet = SpriteBatchExtensions.CreateJoinedEllipse(
                16,
                16,
                ellipses.ToArray(),
                colors.ToArray()
            );

            ObjectColor = color;
            CanInteract = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(16, 16), 100));

            AutoMovement = movement;
        }

        public override void Update()
        {
            base.Update();

            var groundY = ParentStage.GetGround(Position);

            if (Y <= groundY)
            {
                CanBeRemoved = true;
                ParentStage.AddObject(new GroundSplat(ObjectColor) { Position = Position });
            }
        }
    }
}