﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Level.Scene.Level1_1
{
    class Street : SceneryObject
    {
        public Street(GGGame game) : base(game)
        {
            spriteSheet = game.textureManager.load(@"Levels\Stage1-1\Street");
            size = new Vector3(64, 32, 0);
            drawShadow = false;
            collision = false;
            canLandOn = false;
            gravityAffected = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 32), 100));
            sortLowest = true;
        }
    }
}
