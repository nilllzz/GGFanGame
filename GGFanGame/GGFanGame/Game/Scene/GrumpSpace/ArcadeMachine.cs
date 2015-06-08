using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Level.Scene.GrumpSpace
{
    public enum ArcadeType
    {
        Ninja = 0
    }

    class ArcadeMachine : InteractableStageObject
    {
        private ArcadeType _arcadeType;

        public ArcadeMachine(GGGame game, ArcadeType arcadeType) : base(game)
        {
            _arcadeType = arcadeType;

            spriteSheet = game.textureManager.load(@"Levels\GrumpSpace\Arcade" + ((int)_arcadeType).ToString());

            size = new Vector3(31, 59, 8);
            drawShadow = true;
            collision = true;
            canLandOn = true;
            canInteract = true;
            weight = 20;
            faceAttack = false;
            facing = ObjectFacing.Right;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(31, 59), 100));
            addAnimation(ObjectState.Hurt, new Animation(1, Point.Zero, new Point(31, 59), 100));
            addAnimation(ObjectState.HurtFalling, new Animation(1, Point.Zero, new Point(31, 59), 100));
        }
    }
}