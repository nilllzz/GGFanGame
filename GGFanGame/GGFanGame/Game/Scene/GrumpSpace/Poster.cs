using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Level.Scene.GrumpSpace
{
    class Poster : SceneryObject
    {
        static string[] posters = new string[] { "Brian", "Goose", "Larry", "NoUse", "UpDog" };

        public Poster(GGGame game) : base(game)
        {
            setRandomPoster();
            size = new Vector3(23, 30, 2);

            drawShadow = true;
            collision = false;
            canInteract = false;
            canLandOn = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(23, 30), 100));
        }

        private void setRandomPoster()
        {
            spriteSheet = gameInstance.textureManager.load(@"Levels\GrumpSpace\Posters\" + 
                posters[gameInstance.random.Next(0, posters.Length)]);
        }

        public override void update()
        {
            if (Input.GamePadHandler.buttonPressed(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.A))
            {
                setRandomPoster();
            }
        }
    }
}