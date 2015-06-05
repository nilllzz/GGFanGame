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
    class SceneryObject : Level.InteractableStageObject
    {
        //TODO: Currently, this is a couch. Yeah.
        //We need to generalize this and have a couch class.

        public SceneryObject(GGGame game) : base(game)
        {
            spriteSheet = game.textureManager.getResource(@"Sprites\Couch");
            size = new Vector3(80, 10, 5);
            canLandOn = true;
            state = Level.ObjectState.Idle;
            drawShadow = true;
            canInteract = false;

            addAnimation(Level.ObjectState.Idle, new Animation(1, Point.Zero, new Point(81, 36), 100));
        }
    }
}