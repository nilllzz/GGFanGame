using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Level.Scene
{
    /// <summary>
    /// A ball shot by a gun that splats on the ground upon contact.
    /// </summary>
    class SplatBall : InteractableStageObject
    {
        public SplatBall(GGGame game, Color color, Vector3 movement) : base(game)
        {
            initialize(color, movement);
        }

        public SplatBall(GGGame game, Color color, ObjectFacing setFacing) : base(game) 
        {
            float xMovement = (float)game.random.Next(10, 20);
            float yMovement = game.random.Next(0, 10);
            float zMovement = game.random.Next(-5, 5);
            if (setFacing == ObjectFacing.Left)
            {
                _autoMovement.X = -xMovement;
            }
            else
            {
                _autoMovement.X = xMovement;
            }

            initialize(color, new Vector3(xMovement, yMovement, zMovement));
        }

        private void initialize( Color color, Vector3 movement)
        {
            List<Rectangle> ellipses = new List<Rectangle>();
            List<Color> colors = new List<Color>();

            for (int i = 0; i < 3; i++)
            {
                int width = gameInstance.random.Next(4, 8);
                int height = gameInstance.random.Next(4, 8);
                int x = gameInstance.random.Next(0, 16 - width);
                int y = gameInstance.random.Next(0, 16 - height);

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

            _autoMovement = movement;
        }

        public override void update()
        {
            base.update();

            var groundY = Stage.activeStage().getGround(position);

            if (Y <= groundY)
            {
                canBeRemoved = true;
                Stage.activeStage().addObject(new GroundSplat(gameInstance, objectColor) { position = position });
            }
        }
    }
}