using GGFanGame.Drawing;
using GGFanGame.Rendering.Composers;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game.Stages
{
    /// <summary>
    /// A splat on the ground.
    /// </summary>
    internal class GroundSplat : InteractableStageObject
    {
        private int _alpha = 255;

        public GroundSplat(Color color)
        {
            ObjectColor = color;
            IsOpaque = false;
            
            CanInteract = false;
            
            AddAnimation(ObjectState.Idle, new Animation(1, Point.Zero, new Point(64, 64), 100));
        }

        protected override void LoadContentInternal()
        {
            const int ellipseAmount = 8;
            var ellipses = new (Rectangle Bounds, Color FillColor)[ellipseAmount];
            var random = ParentStage.Random;

            for (var i = 0; i < ellipseAmount; i++)
            {
                var width = random.Next(8, 31);
                var height = random.Next(8, 31);
                var x = random.Next(0, 64 - width);
                var y = random.Next(0, 64 - height);

                ellipses[i].Bounds = new Rectangle(x, y, width, height);
                ellipses[i].FillColor = ObjectColor;
            }

            SpriteSheet1 = new SpriteSheet(EllipseConfiguration.CreateJoinedEllipse(
                64,
                64,
                ellipses
            ));

            if (random.Next(0, 2) == 0)
            {
                Facing = ObjectFacing.Left;
            }
        }

        protected override void CreateGeometry()
        {
            var vertices = RectangleComposer.Create(1f, 1f);
            Geometry.AddVertices(vertices);
        }
        
        public override void Update()
        {
            base.Update();

            _alpha--;
            if (_alpha < 0)
            {
                _alpha = 0;
                CanBeRemoved = true;
            }

            Alpha = _alpha / 255f;
        }
    }
}
