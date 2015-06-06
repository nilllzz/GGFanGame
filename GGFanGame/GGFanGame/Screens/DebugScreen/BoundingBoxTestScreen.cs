using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GGFanGame.Game.Level;

namespace GGFanGame.Screens.Debug
{
    /// <summary>
    /// A screen to render bounding boxes in 3D to see how they work in 3D space.
    /// </summary>
    class BoundingBoxTestScreen : Screen
    {
        Vector3 camPos = Vector3.Zero;
        float _yaw, _pitch;

        Matrix _view;
        Matrix _projection;

        public BoundingBoxTestScreen(GGGame game) : base(Identification.InGame, game)
        {
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(80f), game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            createMatrix();
        }

        private void createMatrix()
        {
            Matrix rotation = Matrix.CreateRotationX(_pitch) * Matrix.CreateRotationY(_yaw);

            Vector3 transformed = Vector3.Transform(new Vector3(0, 0, -1), rotation);
            Vector3 lookAt = camPos + transformed;

            _view = Matrix.CreateLookAt(camPos, lookAt, Vector3.Up);
        }

        public override void draw()
        {
            foreach (StageObject obj in Stage.activeStage().getObjects())
            {
                BoundingBox[] boxes = obj.boundingBoxes;

                //When the object does not have defined bounding boxes, take the default bounding box.
                if (boxes.Length == 0)
                    boxes = new BoundingBox[] { obj.boundingBox };

                foreach (BoundingBox box in boxes)
                {
                    BoundingBoxRenderer.Render(box, gameInstance.GraphicsDevice, _view, _projection, obj.objectColor);
                }
            }
        }

        public override void update()
        {
            float inRight = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Right) * 0.1f;
            float inLeft = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Left) * 0.1f;
            float inUp = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Up) * 0.1f;
            float inDown = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Down) * 0.1f;

            _yaw += inLeft;
            _yaw -= inRight;
            _pitch += inUp;
            _pitch -= inDown;

            inRight = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadRight) ? 1f : 0f;
            inLeft = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadLeft) ? 1f : 0f;
            inUp = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadUp) ? 1f : 0f;
            inDown = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadDown) ? 1f : 0f;

            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0f);

            Vector3 forward = Vector3.Transform(Vector3.Forward, rotationMatrix) * inUp * 2f;
            Vector3 backward = Vector3.Transform(Vector3.Backward, rotationMatrix) * inDown * 2f;
            Vector3 left = Vector3.Transform(Vector3.Left, rotationMatrix) * inLeft * 2f;
            Vector3 right = Vector3.Transform(Vector3.Right, rotationMatrix) * inRight * 2f;

            camPos += forward;
            camPos += backward;
            camPos += left;
            camPos += right;

            createMatrix();

            Stage.activeStage().update();
        }
    }
}