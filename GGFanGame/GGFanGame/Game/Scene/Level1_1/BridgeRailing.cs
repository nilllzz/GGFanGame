using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Level.Scene.Level1_1
{
    class BridgeRailing : SceneryObject
    {
        public BridgeRailing(GGGame game) : base(game)
        {
            spriteSheet = game.textureManager.load(@"Levels\Stage1-1\BridgeRailing");
            size = new Vector3(64, 32, 5);
            drawShadow = false;
            collision = true;
            canLandOn = true;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 32), 100));

        }
    }
}
