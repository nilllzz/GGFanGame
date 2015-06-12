using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Level.Scene
{
    class GroundSplat : InteractableStageObject
    { 
        public GroundSplat(GGGame game, Color color) : base(game)
        {
            spriteSheet = game.textureManager.load(@"Levels\Splat");
            objectColor = color;
            sortLowest = true;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(128, 128), 100));
        }

        public override void draw()
        {
            Rectangle frame = getAnimation().getFrameRec(animationFrame);
            double stageScale = Stage.activeStage().scale;

            int shadowWidth = (int)(spriteSheet.Width);
            int shadowHeight = (int)(spriteSheet.Height * (1d / 4d));

            gameInstance.spriteBatch.Draw(spriteSheet, new Rectangle((int)((X - shadowWidth / 2d) * stageScale),
                            (int)((Z - shadowHeight / 2d - Stage.activeStage().getGround(position)) * stageScale),
                            (int)(shadowWidth * stageScale),
                            (int)(shadowHeight * stageScale)), null, objectColor);
        }
    }
}