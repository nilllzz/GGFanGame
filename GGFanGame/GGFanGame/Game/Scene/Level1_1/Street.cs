using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using static GameProvider;

namespace GGFanGame.Game.Scene.Level1_1
{
    class Street : SceneryObject
    {
        public Street()
        {
            spriteSheet = gameInstance.textureManager.load(@"Levels\Stage1-1\Street");
            size = new Vector3(64, 1, 32);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 32), 100));
            groundRelation = GroundRelation.Flat;
            //sortLowest = true;
        }
    }
}
