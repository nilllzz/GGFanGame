using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Screens.Debug
{
    /// <summary>
    /// A class to render a bounding box outline in 3D space.
    /// </summary>
    internal static class BoundingBoxRenderer
    {
        private static readonly VertexPositionColor[] _verts = new VertexPositionColor[8];
        private static readonly short[] _indices = { 0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4 };
        private static BasicEffect _effect;

        /// <summary>
        /// Renders a bounding box as line list.
        /// </summary>
        public static void Render(BoundingBox box, GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Color color)
        {
            if (_effect == null)
            {
                _effect = new BasicEffect(graphicsDevice)
                {
                    VertexColorEnabled = true,
                    LightingEnabled = false
                };
            }

            var corners = box.GetCorners();

            for (var i = 0; i < 8; i++)
            {
                _verts[i].Position = corners[i];
                _verts[i].Color = color;
            }

            _effect.View = view;
            _effect.Projection = projection;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, _verts, 0, 8, _indices, 0, _indices.Length / 2);
            }
        }
    }
}
