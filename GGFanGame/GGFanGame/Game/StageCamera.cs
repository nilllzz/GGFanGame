using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game
{
    class StageCamera
    {
        private double _scale = 2d;
        private Vector2 _offset = Vector2.Zero;

        /// <summary>
        /// The zoom of the camera.
        /// </summary>
        public double scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// The offset of the camera.
        /// </summary>
        public Vector2 offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public void update(Stage stage)
        {
            //For one player, the camera always follows that player with the level specific scale.
            //By default this scale is 2.
            //For more than one player, the camera focuses on the center of those players, making the scale larger if needed.
            //The scale for more than one player does not go below 2.

            Vector3[] playerPositions = new Vector3[] { stage.onePlayer.position, stage.twoPlayer.position, stage.threePlayer.position, stage.fourPlayer.position }.OrderBy(x => x.X).ToArray();
            Vector3 leftPosition = playerPositions.First();
            Vector3 rightPosition = playerPositions.Last();

            _scale = 2d;

            double left = (leftPosition.X + offset.X) * _scale;
            double right = (rightPosition.X + offset.X + 100) * _scale;

            while (right > 1280)
            {
                _scale -= 0.001f;
                right = (rightPosition.X + offset.X + 100) * _scale;
            }

            //System.Diagnostics.Debug.Print(right.ToString());
        }
    }
}