using System.Collections.Generic;
using GGFanGame.Rendering.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Rendering.Composers
{
    public static class CylinderComposer
    {
        public static VertexPositionNormalTexture[] Create(float radius, float height, int edgeCount)
            => Create(radius, height, edgeCount, DefaultGeometryTextureDefinition.Instance, DefaultGeometryTextureDefinition.Instance);

        public static VertexPositionNormalTexture[] Create(float radius, float height, int edgeCount, 
            IGeometryTextureDefintion sideTexture, IGeometryTextureDefintion endTexture)
        {
            var vertices = new List<VertexPositionNormalTexture>();

            var sides = TubeComposer.Create(radius, height, edgeCount, sideTexture);
            var end1 = CircleComposer.Create(radius, edgeCount, endTexture);
            var end2 = CircleComposer.Create(radius, edgeCount, endTexture);

            VertexTransformer.Rotate(end1, new Vector3(0, 0, MathHelper.PiOver2));
            VertexTransformer.Offset(end1, new Vector3(height / 2f, 0, 0));
            VertexTransformer.Rotate(end2, new Vector3(0, 0, MathHelper.PiOver2));
            VertexTransformer.Offset(end2, new Vector3(-height / 2f, 0, 0));

            vertices.AddRange(sides);
            vertices.AddRange(end1);
            vertices.AddRange(end2);

            return vertices.ToArray();
        }
    }
}
