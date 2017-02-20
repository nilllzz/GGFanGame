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
    }
}
