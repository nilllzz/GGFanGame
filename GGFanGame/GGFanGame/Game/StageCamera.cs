using System.Linq;
using Microsoft.Xna.Framework;

namespace GGFanGame.Game
{
    internal class StageCamera
    {
        /// <summary>
        /// The zoom of the camera.
        /// </summary>
        public double scale { get; set; } = 2d;

        /// <summary>
        /// The offset of the camera.
        /// </summary>
        public Vector2 offset { get; set; }

        public void update(Stage stage)
        {
            //For one player, the camera always follows that player with the level specific scale.
            //By default this scale is 2.
            //For more than one player, the camera focuses on the center of those players, making the scale larger if needed.
            //The scale for more than one player does not go below 2.

            var playerPositions = new Vector3[] { stage.onePlayer.position, stage.twoPlayer.position, stage.threePlayer.position, stage.fourPlayer.position }.OrderBy(x => x.X).ToArray();
            var leftPosition = playerPositions.First();
            var rightPosition = playerPositions.Last();

            scale = 2d;

            var left = (leftPosition.X + offset.X) * scale;
            var right = (rightPosition.X + offset.X + 100) * scale;

            while (right > 1280)
            {
                scale -= 0.001f;
                right = (rightPosition.X + offset.X + 100) * scale;
            }

            //System.Diagnostics.Debug.Print(right.ToString());
        }
    }
}