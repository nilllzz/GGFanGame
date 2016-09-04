﻿using System;
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
        public SceneryObject() : base()
        {
            canLandOn = true;
            state = ObjectState.Idle;
            canInteract = false;
        }
    }
}