using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Screens.Game.Level.GrumpSpace
{
    class Danny : PlayerCharacter
    {
        public Danny(GGGame game) : base(game)
        {
            texture = gameInstance.Content.Load<Texture2D>(@"Sprites\Danny");

            addAnimation(PlayerState.Idle, new PlayerAnimation(6, 8, Vector2.Zero));
            addAnimation(PlayerState.Walking, new PlayerAnimation(4, 6, new Vector2(0, 64)));
            addAnimation(PlayerState.Jumping, new PlayerAnimation(1, 0, new Vector2(0, 128)));
            addAnimation(PlayerState.Falling, new PlayerAnimation(1, 0, new Vector2(0, 192)));

            speed = 5f;
            jumpHeight = 14;
        }
    }
}
