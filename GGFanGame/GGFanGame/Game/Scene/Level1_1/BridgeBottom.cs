using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using static GameProvider;

namespace GGFanGame.Game.Scene.Level1_1
{
    class BridgeBottom : SceneryObject
    {
        public BridgeBottom()
        {
            spriteSheet = gameInstance.textureManager.load(@"Levels\Stage1-1\BridgeBottom");
            size = new Vector3(128, 64, 64);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            sortLowest = true;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 64), 100));

        }
    }
}
