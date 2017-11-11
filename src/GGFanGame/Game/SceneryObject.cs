using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    /// <summary>
    /// The generic class for scenery stuff.
    /// </summary>
    internal abstract class SceneryObject : InteractableStageObject
    {
        protected SceneryObject()
        {
            CanLandOn = true;
            State = ObjectState.Idle;
            CanInteract = false;
        }

        protected void AddStaticAnimation(int x, int y, int textureWidth, int textureHeight)
        {
            AddAnimation(ObjectState.Idle, new Animation(1, new Point(x, y), new Point(textureWidth, textureHeight), 100));
        }
    }
}
