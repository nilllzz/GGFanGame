using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game.Stages.GrumpSpace
{
    internal class Poster : SceneryObject
    {
        static readonly string[] _posters = { "Brian", "Goose", "Larry", "NoUse", "UpDog" };

        public Poster()
        {
            size = new Vector3(23, 30, 2);

            drawShadow = true;
            collision = false;
            canInteract = false;
            canLandOn = false;

            addAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(23, 30), 100));
        }

        protected override void loadInternal()
        {
            setRandomPoster();
        }

        private void setRandomPoster()
        {
            spriteSheet = content.Load<Texture2D>(@"Levels\GrumpSpace\Posters\" + 
                _posters[gameInstance.random.Next(0, _posters.Length)]);
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