using System.Linq;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    internal class StageCamera
    {
        /// <summary>
        /// The zoom of the camera.
        /// </summary>
        public double Scale { get; set; } = 2d;

        /// <summary>
        /// The offset of the camera.
        /// </summary>
        public Vector2 Offset { get; set; }

        public void Update(Stage stage)
        {
            //For one player, the camera always follows that player with the level specific scale.
            //By default this scale is 2.
            //For more than one player, the camera focuses on the center of those players, making the scale larger if needed.
            //The scale for more than one player does not go below 2.
            
            var playerPositions = stage.GetPlayers().Select(p => p.Position).OrderBy(x => x.X);
            var leftPosition = playerPositions.First();
            var rightPosition = playerPositions.Last();

            Scale = 2d;

            var left = (leftPosition.X + Offset.X) * Scale;
            var right = (rightPosition.X + Offset.X + 100) * Scale;

            while (right > 1280)
            {
                Scale -= 0.001f;
                right = (rightPosition.X + Offset.X + 100) * Scale;
            }

            //System.Diagnostics.Debug.Print(right.ToString());
        }
    }
}