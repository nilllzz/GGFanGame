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
            spriteSheet = game.textureManager.load(@"Levels\GrumpSpace\Couch");
            size = new Vector3(80, 16, 5);
            drawShadow = true;
            collision = true;
            addAnimation(Level.ObjectState.Idle, new Animation(1, Point.Zero, new Point(81, 36), 100));

            addBoundingBox(new Vector3(11, 29, 8), new Vector3(-33, 14.5f, 4)); //Left arm
            addBoundingBox(new Vector3(57, 16, 8), new Vector3(0, 8, 4)); //Center area
            addBoundingBox(new Vector3(57, 16, 8), new Vector3(0, 21, -4)); //Back arm
            addBoundingBox(new Vector3(11, 29, 8), new Vector3(33, 14.5f, 4)); //Right arm 
        }
    }
}