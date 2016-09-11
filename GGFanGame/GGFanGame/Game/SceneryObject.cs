namespace GGFanGame.Game
{
    /// <summary>
    /// The generic class for scenery stuff.
    /// </summary>
    internal abstract class SceneryObject : InteractableStageObject
    {
        protected SceneryObject()
        {
            canLandOn = true;
            state = ObjectState.Idle;
            canInteract = false;
        }
    }
}