﻿using GGFanGame.Game;
using GameDevCommon.Input;
using Microsoft.Xna.Framework;
using static Core;

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
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(80f), GameInstance.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);
            CreateMatrix();
        }

        private void CreateMatrix()
        {
            var rotation = Matrix.CreateRotationX(_pitch) * Matrix.CreateRotationY(_yaw);

            var transformed = Vector3.Transform(new Vector3(0, 0, -1), rotation);
            var lookAt = camPos + transformed;

            _view = Matrix.CreateLookAt(camPos, lookAt, Vector3.Up);
        }

        public override void Draw(GameTime time)
        {
            foreach (var obj in Stage.ActiveStage.Objects)
            {
                BoundingBoxRenderer.Render(obj.BoundingBox, GameInstance.GraphicsDevice, _view, _projection, obj.ObjectColor);
            }
        }

        public override void Update(GameTime time)
        {
            var gamePadHandler = GetComponent<GamePadHandler>();

            var inRight = gamePadHandler.GetThumbStickDirection(PlayerIndex.One, ThumbStick.Right, InputDirection.Right) * 0.1f;
            var inLeft = gamePadHandler.GetThumbStickDirection(PlayerIndex.One, ThumbStick.Right, InputDirection.Left) * 0.1f;
            var inUp = gamePadHandler.GetThumbStickDirection(PlayerIndex.One, ThumbStick.Right, InputDirection.Up) * 0.1f;
            var inDown = gamePadHandler.GetThumbStickDirection(PlayerIndex.One, ThumbStick.Right, InputDirection.Down) * 0.1f;

            _yaw += inLeft;
            _yaw -= inRight;
            _pitch += inUp;
            _pitch -= inDown;

            inRight = gamePadHandler.ButtonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadRight) ? 1f : 0f;
            inLeft = gamePadHandler.ButtonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadLeft) ? 1f : 0f;
            inUp = gamePadHandler.ButtonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadUp) ? 1f : 0f;
            inDown = gamePadHandler.ButtonDown(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.DPadDown) ? 1f : 0f;

            var rotationMatrix = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0f);

            var forward = Vector3.Transform(Vector3.Forward, rotationMatrix) * inUp * 2f;
            var backward = Vector3.Transform(Vector3.Backward, rotationMatrix) * inDown * 2f;
            var left = Vector3.Transform(Vector3.Left, rotationMatrix) * inLeft * 2f;
            var right = Vector3.Transform(Vector3.Right, rotationMatrix) * inRight * 2f;

            camPos += forward;
            camPos += backward;
            camPos += left;
            camPos += right;

            CreateMatrix();

            Stage.ActiveStage.Update();
        }
    }
}
