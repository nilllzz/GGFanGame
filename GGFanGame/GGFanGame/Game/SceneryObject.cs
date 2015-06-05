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
    abstract class SceneryObject : Level.InteractableStageObject
    {
        public SceneryObject(GGGame game) : base(game)
        {
            canLandOn = true;
            state = Level.ObjectState.Idle;
            canInteract = false;
        }
    }
}