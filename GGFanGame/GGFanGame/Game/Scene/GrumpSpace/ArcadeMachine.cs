using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game.Scene.GrumpSpace
{
    class ArcadeMachine : InteractableStageObject
    {
        private ArcadeType _arcadeType;

        public ArcadeMachine(ArcadeType arcadeType)
        {
            _arcadeType = arcadeType;

            spriteSheet = gameInstance.Content.Load<Texture2D>(@"Levels\GrumpSpace\Arcade" + ((int)_arcadeType).ToString());

            size = new Vector3(30, 60, 11);
            drawShadow = false;
            collision = true;
            canLandOn = true;
            canInteract = true;
            weight = 20;
            faceAttack = false;

            addBoundingBox(new Vector3(30, 58, 10), new Vector3(0, 29, -2));
            zSortingOffset = -7;   // -(boundingBox.Z / 2) - offset.Z

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(30, 65), 100));
            addAnimation(ObjectState.Hurt, new Animation(1, Point.Zero, new Point(30, 65), 100));
            addAnimation(ObjectState.HurtFalling, new Animation(1, Point.Zero, new Point(30, 65), 100));
        }
    }
}