using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using static GameProvider;

namespace GGFanGame.Game.Scene.GrumpSpace
{
    class Couch : SceneryObject
    {
        public Couch() : base()
        {
            spriteSheet = gameInstance.textureManager.load(@"Levels\GrumpSpace\Couch");
            size = new Vector3(80, 16, 5);
            drawShadow = true;
            collision = true;
            canLandOn = true;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(81, 36), 100));

            addBoundingBox(new Vector3(12, 28, 12), new Vector3(-34, 14, 0)); //Left arm
            addBoundingBox(new Vector3(57, 16, 6), new Vector3(0, 8, 3)); //Center seat
            addBoundingBox(new Vector3(57, 28, 6), new Vector3(0, 14, -3)); //Back arm
            addBoundingBox(new Vector3(12, 28, 12), new Vector3(34, 14, 0)); //Right arm 

            zSortingOffset = -6; // -(boundingBox.Z / 2) - offset.Z
        }
    }
}