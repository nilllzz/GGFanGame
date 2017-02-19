using System;
using Microsoft.Xna.Framework.Graphics;
using static Core;

namespace GGFanGame.Rendering
{
    internal abstract class ObjectRenderer : IDisposable
    {
        protected BasicEffect _effect;
        protected PrimitiveType _primitiveType = PrimitiveType.TriangleList;

        internal bool IsDisposed { get; private set; }

        internal virtual void LoadContent()
        {
            _effect = new BasicEffect(GameInstance.GraphicsDevice)
            {
                TextureEnabled = true
            };
            _effect.EnableDefaultLighting();
        }

        internal void PrepareRender(Camera camera)
        {
            _effect.View = camera.View;
            _effect.Projection = camera.Projection;
        }

        internal void Render(I3DObject obj)
        {
            if (obj.IsVisible && obj.IsVisualObject)
            {
                if (obj.BlendState != null)
                    GameInstance.GraphicsDevice.BlendState = obj.BlendState;
                else
                    GameInstance.GraphicsDevice.BlendState = BlendState.AlphaBlend;

                RenderVertices(obj);
            }
        }

        private void RenderVertices(I3DObject obj)
        {
            _effect.Alpha = obj.Alpha;
            _effect.Texture = obj.Texture;

            _effect.World = obj.World;
            _effect.CurrentTechnique.Passes[0].Apply();

            GameInstance.GraphicsDevice.Indices = obj.IndexBuffer;
            GameInstance.GraphicsDevice.SetVertexBuffer(obj.VertexBuffer);
            GameInstance.GraphicsDevice.DrawIndexedPrimitives(_primitiveType, 0, 0, obj.IndexBuffer.IndexCount / 3);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ObjectRenderer()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_effect != null && !_effect.IsDisposed) _effect.Dispose();
                }

                _effect = null;

                IsDisposed = true;
            }
        }
    }
}
