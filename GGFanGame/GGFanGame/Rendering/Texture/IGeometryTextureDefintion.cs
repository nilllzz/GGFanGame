using Microsoft.Xna.Framework;

namespace GGFanGame.Rendering.Texture
{
    public interface IGeometryTextureDefintion
    {
        void NextElement();
        Vector2 Transform(Vector2 normalVector);
    }
}
