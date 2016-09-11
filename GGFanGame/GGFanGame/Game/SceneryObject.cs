using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game
{
    /// <summary>
    /// The generic class for scenery stuff.
    /// </summary>
    abstract class SceneryObject : InteractableStageObject
    {
        public SceneryObject()
        {
            canLandOn = true;
            state = ObjectState.Idle;
            canInteract = false;
        }
    }
}