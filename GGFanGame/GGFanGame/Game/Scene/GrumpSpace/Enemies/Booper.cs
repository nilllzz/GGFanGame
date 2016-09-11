using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game.Scene.GrumpSpace.Enemies
{
    /// <summary>
    /// An enemy for a dojo stage.
    /// </summary>
    internal class Booper : Enemy
    {
        public Booper()
        {
            spriteSheet = content.Load<Texture2D>(@"Sprites\Booper");
            drawShadow = true;
            shadowSize = 0.6d;
            strength = 0f;
            weight = 4f;
            state = ObjectState.Idle;
            size = new Vector3(40, 48, 10);
            maxHealth = 50;

            addAnimation(ObjectState.Idle, new Animation(6, Point.Zero, new Point(64, 64), 6));
            addAnimation(ObjectState.Hurt, new Animation(7, new Point(0, 64), new Point(64, 64), 4));
            addAnimation(ObjectState.HurtFalling, new Animation(7, new Point(0, 64), new Point(64, 64), 4));
            addAnimation(ObjectState.Dead, new Animation(6, new Point(0, 128), new Point(64, 64), 4));
            
            OnDeath += onDeath;
        }

        public override void update()
        {
            base.update();

            //This enemy always faces the player:
            if (state == ObjectState.Idle)
            {
                if (Stage.activeStage.onePlayer.X < X)
                {
                    facing = ObjectFacing.Left;
                }
                else
                {
                    facing = ObjectFacing.Right;
                }
            }
        }

        private void onDeath(StageObject obj)
        {
            Stage.activeStage.addObject(new GroundSplat(new Color(255, 128, 255)) { position = position });

            for (var i = 0; i < 3; i++)
            {
                float xMovement = gameInstance.random.Next(5, 10);
                float zMovement = gameInstance.random.Next(-3, 4);
                if (facing == ObjectFacing.Right)
                {
                    xMovement *= -1;
                }

                var G = 128;
                if (gameInstance.random.Next(0, 2) == 0)
                {
                    G = 174;
                }

                Stage.activeStage.addObject(new SplatBall(new Color(255, G, 255), new Vector3(xMovement, 3f, zMovement)) { position = position });
            }
        }
    }
}
