using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Screens.Game
{
    internal abstract class StageScreen : Screen
    {
        internal abstract void RenderStage();
        internal abstract void DrawStage(SpriteBatch batch = null);
        internal abstract void DrawHUD(SpriteBatch batch = null);
    }
}