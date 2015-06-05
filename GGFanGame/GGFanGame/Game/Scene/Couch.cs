using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Scene
{
    class Couch : SceneryObject
    {
        public Couch(GGGame game) : base(game)
        {
            spriteSheet = game.textureManager.getResource(@"Sprites\Couch");
            size = new Vector3(80, 16, 5);
            drawShadow = true;
            addAnimation(Level.ObjectState.Idle, new Animation(1, Point.Zero, new Point(81, 36), 100));
        }
    }
}