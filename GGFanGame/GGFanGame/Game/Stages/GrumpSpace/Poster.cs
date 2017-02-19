using GGFanGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;

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

        protected override void LoadContentInternal()
        {
            SetRandomPoster();
        }

        private void SetRandomPoster()
        {
            SpriteSheet = ParentStage.Content.Load<Texture2D>(@"Levels\GrumpSpace\Posters\" + 
                _posters[ParentStage.Random.Next(0, _posters.Length)]);
        }

        public override void Update()
        {
            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Microsoft.Xna.Framework.Input.Buttons.A))
            {
                SetRandomPoster();
            }
        }
    }
}