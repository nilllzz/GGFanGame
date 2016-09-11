using Microsoft.Xna.Framework;
using GGFanGame.Game;
using static GameProvider;

namespace GGFanGame.Screens.Debug
{
    /// <summary>
    /// A screen to render bounding boxes in 3D to see how they work in 3D space.
    /// </summary>
    internal class BoundingBoxTestScreen : Screen
    {
        Vector3 camPos = Vector3.Zero;
        private float _yaw, _pitch;

        private Matrix _view;
        private readonly Matrix _projection;

        public BoundingBoxTestScreen()
        {
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(80f), gameInstance.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            createMatrix();
        }

        private void createMatrix()
        {
            var rotation = Matrix.CreateRotationX(_pitch) * Matrix.CreateRotationY(_yaw);

            var transformed = Vector3.Transform(new Vector3(0, 0, -1), rotation);
            var lookAt = camPos + transformed;

            _view = Matrix.CreateLookAt(camPos, lookAt, Vector3.Up);
        }

        public override void draw()
        {
            foreach (var obj in Stage.activeStage.getObjects())
            {
                var boxes = obj.boundingBoxes;

                //When the object does not have defined bounding boxes, take the default bounding box.
                if (boxes.Length == 0)
                    boxes = new BoundingBox[] { obj.boundingBox };

                foreach (var box in boxes)
                {
                    BoundingBoxRenderer.Render(box, gameInstance.GraphicsDevice, _view, _projection, obj.objectColor);
                }
            }
        }

        public override void update()
        {
            var inRight = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Right) * 0.1f;
            var inLeft = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Left) * 0.1f;
            var inUp = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Up) * 0.1f;
            var inDown = Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Right, Input.InputDirection.Down) * 0.1f;

            _yaw += inLeft;
            _yaw -= inRight;
            _pitch += inUp;
            _pitch -= inDown;

            inRight = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadRight) ? 1f : 0f;
            inLeft = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadLeft) ? 1f : 0f;
            inUp = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadUp) ? 1f : 0f;
            inDown = Input.GamePadHandler.buttonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadDown) ? 1f : 0f;

            var rotationMatrix = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0f);

            var forward = Vector3.Transform(Vector3.Forward, rotationMatrix) * inUp * 2f;
            var backward = Vector3.Transform(Vector3.Backward, rotationMatrix) * inDown * 2f;
            var left = Vector3.Transform(Vector3.Left, rotationMatrix) * inLeft * 2f;
            var right = Vector3.Transform(Vector3.Right, rotationMatrix) * inRight * 2f;

            camPos += forward;
            camPos += backward;
            camPos += left;
            camPos += right;

            createMatrix();

            Stage.activeStage.update();
        }
    }
}