using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Rendering
{
    internal interface I3DObject
    {
        VertexBuffer VertexBuffer { get; set; }
        IndexBuffer IndexBuffer { get; set; }
        Matrix World { get; set; }
        bool IsVisible { get; set; }
        BlendState BlendState { get; }
        bool IsVisualObject { get; set; }
        float Alpha { get; set; }
        Texture2D Texture { get; set; }

        void Update();
    }
}
