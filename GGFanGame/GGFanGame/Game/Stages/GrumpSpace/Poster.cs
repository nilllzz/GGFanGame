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
            Size = new Vector3(23, 30, 2);

            DrawShadow = true;
            Collision = false;
            CanInteract = false;
            CanLandOn = false;

            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(23, 30), 100));
        }

        protected override void LoadInternal()
        {
            SetRandomPoster();
        }

        private void SetRandomPoster()
        {
            SpriteSheet = Content.Load<Texture2D>(@"Levels\GrumpSpace\Posters\" + 
                _posters[GameInstance.Random.Next(0, _posters.Length)]);
        }

        public override void Update()
        {
            if (Input.GamePadHandler.ButtonPressed(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.A))
            {
                SetRandomPoster();
            }
        }
    }
}