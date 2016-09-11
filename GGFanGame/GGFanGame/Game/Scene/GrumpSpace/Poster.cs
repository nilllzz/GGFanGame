using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game.Scene.GrumpSpace
{
    class Poster : SceneryObject
    {
        static string[] posters = new string[] { "Brian", "Goose", "Larry", "NoUse", "UpDog" };

        public Poster()
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
            spriteSheet = gameInstance.Content.Load<Texture2D>(@"Levels\GrumpSpace\Posters\" + 
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