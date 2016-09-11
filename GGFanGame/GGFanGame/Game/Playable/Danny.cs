using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Playable
{
    /// <summary>
    /// Playable Dan character.
    /// </summary>
    class Danny : PlayerCharacter
    {
        public override string name => "Danny";
        public override int maxGrumpPower => 100;

        public Danny(PlayerIndex playerIndex) : base(playerIndex)
        {
            //texture = gameInstance.textureManager.getResource(@"Sprites\Danny");

            //addAnimation(PlayerState.Idle, new PlayerAnimation(6, 8, Vector2.Zero, new Vector2(32, 48)));
            //addAnimation(PlayerState.Walking, new PlayerAnimation(4, 6, new Vector2(0, 64), new Vector2(32, 48)));
            //addAnimation(PlayerState.Jumping, new PlayerAnimation(1, 0, new Vector2(0, 128), new Vector2(32, 48)));
            //addAnimation(PlayerState.Falling, new PlayerAnimation(1, 0, new Vector2(0, 192), new Vector2(32, 48)));

            //speed = 5f;
            //jumpHeight = 14;

            //TODO: Finish animation once the sprite sheet is there.
            //TODO: Set values.
        }
    }
}
