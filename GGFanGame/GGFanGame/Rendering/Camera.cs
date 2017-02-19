using System;
using Microsoft.Xna.Framework;
using static Core;

namespace GGFanGame.Rendering
{
    internal abstract class Camera
    {
        private float _fov = 90f;

        public event Action FOVChanged;

        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }
        public Vector3 Position { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public float FOV
        {
            get { return _fov; }
            set
            {
                _fov = value;
                FOVChanged?.Invoke();
            }
        }
        
        protected virtual void CreateView()
        {
            var up = Vector3.Up;
            var forward = Vector3.Forward;

            // yaw:
            {
                forward.Normalize();
                forward = Vector3.Transform(forward, Matrix.CreateFromAxisAngle(up, Yaw));
            }

            // pitch:
            {
                forward.Normalize();
                var left = Vector3.Cross(up, forward);
                left.Normalize();

                forward = Vector3.Transform(forward, Matrix.CreateFromAxisAngle(left, -Pitch));
                up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(left, -Pitch));
            }

            // roll:
            {
                up.Normalize();
                var left = Vector3.Cross(up, forward);
                left.Normalize();
                up = Vector3.Transform(up, Matrix.CreateFromAxisAngle(forward, Roll));
            }

            View = Matrix.CreateLookAt(Position, forward + Position, up);
        }

        protected virtual void CreateProjection()
        {
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(_fov), GameInstance.GraphicsDevice.Viewport.AspectRatio, 0.001f, 10000f);
        }

        public abstract void Update();
    }
}
