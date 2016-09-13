using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static GameProvider;

namespace GGFanGame.Game.Stages
{
    /// <summary>
    /// A ball shot by a gun that splats on the ground upon contact.
    /// </summary>
    internal class SplatBall : InteractableStageObject
    {
        public SplatBall(Color color, Vector3 movement)
        {
            initialize(color, movement);
        }

        public SplatBall(Color color, ObjectFacing setFacing)
        {
            float xMovement = gameInstance.random.Next(10, 20);
            float yMovement = gameInstance.random.Next(0, 10);
            float zMovement = gameInstance.random.Next(-5, 5);
            if (setFacing == ObjectFacing.Left)
            {
                autoMovement.X = -xMovement;
            }
            else
            {
                autoMovement.X = xMovement;
            }

            initialize(color, new Vector3(xMovement, yMovement, zMovement));
        }

        private void initialize(Color color, Vector3 movement)
        {
            var ellipses = new List<Rectangle>();
            var colors = new List<Color>();

            for (var i = 0; i < 3; i++)
            {
                var width = gameInstance.random.Next(4, 8);
                var height = gameInstance.random.Next(4, 8);
                var x = gameInstance.random.Next(0, 16 - width);
                var y = gameInstance.random.Next(0, 16 - height);

                ellipses.Add(new Rectangle(x, y, width, height));
                colors.Add(color);
            }

            spriteSheet = Drawing.Graphics.createJoinedEllipse(
                16,
                16,
                ellipses.ToArray(),
                colors.ToArray()
            );

            objectColor = color;
            canInteract = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(16, 16), 100));

            autoMovement = movement;
        }

        public override void update()
        {
            base.update();

            var groundY = Stage.activeStage.getGround(position);

            if (Y <= groundY)
            {
                canBeRemoved = true;
                Stage.activeStage.addObject(new GroundSplat(objectColor) { position = position });
            }
        }
    }
}