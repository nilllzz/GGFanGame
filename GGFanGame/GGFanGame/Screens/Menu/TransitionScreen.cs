using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// The screen to display a Game Grumps transition for switching between screens.
    /// </summary>
    class TransitionScreen : Screen
    {

        private Texture2D _gg_overlay = null;
        private float _overlaySize = 80f;
        private float _rotation = 0f;

        //If the screen outro is playing, or the intro.
        private bool _outro = true;

        //out screen is the current one, inscreen the new one.
        private Screen _outScreen, _inScreen;

        public TransitionScreen(Screen outScreen, Screen inScreen)
        {
            _gg_overlay = gameInstance.Content.Load<Texture2D>(@"UI\Logos\GameGrumpsTransition");
            _outScreen = outScreen;
            _inScreen = inScreen;
        }

        public override void draw()
        {
            if (_outro)
                _outScreen.draw();
            else
                _inScreen.draw();

            if (_overlaySize > 0)
            {
                //Render the rotating logo:
                gameInstance.spriteBatch.Draw(_gg_overlay, new Rectangle(GameController.RENDER_WIDTH / 2,
                                                                        GameController.RENDER_HEIGHT / 2,
                                                                        (int)(_gg_overlay.Width * _overlaySize),
                                                                        (int)(_gg_overlay.Height * _overlaySize)),
                    null, Color.White, _rotation, new Vector2(_gg_overlay.Width / 2, _gg_overlay.Height / 2), SpriteEffects.None, 0f);

                //Get the space between the edges of the screen and the logo.
                float diffX = GameController.RENDER_WIDTH - (_gg_overlay.Width * _overlaySize);
                float diffY = GameController.RENDER_HEIGHT - (_gg_overlay.Height * _overlaySize);

                int addSide = (int)(160 * _overlaySize);

                //When needed, draw black rectangles at the side:
                if (diffX + 50 > 0)
                {
                    Drawing.Graphics.drawRectangle(new Rectangle(0, 0, (int)(diffX * 0.5f) + addSide, GameController.RENDER_HEIGHT), Color.Black);
                    Drawing.Graphics.drawRectangle(new Rectangle(GameController.RENDER_WIDTH - (int)Math.Floor(diffX / 2) - 2 - addSide, 0, (int)Math.Ceiling(diffX / 2) + 2 + addSide, GameController.RENDER_HEIGHT), Color.Black);
                    Drawing.Graphics.drawRectangle(new Rectangle((int)(diffX / 2), 0, (int)(GameController.RENDER_WIDTH - diffX) + 1, (int)(diffY / 2) + 1 + addSide), Color.Black);
                    Drawing.Graphics.drawRectangle(new Rectangle((int)(diffX / 2), GameController.RENDER_HEIGHT - (int)Math.Floor(diffY / 2) - 2 - addSide, (int)(GameController.RENDER_WIDTH - diffX), (int)(diffY / 2) + 2 + addSide), Color.Black);
                }

                //Draw slightly fading rectangle.
                Drawing.Graphics.drawRectangle(gameInstance.clientRectangle, new Color(0, 0, 0, (int)(255 * (1f - _overlaySize / 2f)))); 
            }
            else
            {
                Drawing.Graphics.drawRectangle(gameInstance.clientRectangle, Color.Black);
            }
        }

        public override void update()
        {
            if (_outro)
            {
                _overlaySize = MathHelper.Lerp(0f, _overlaySize, 0.9f);
                _rotation -= 0.08f;
                _outScreen.update();

                if (_overlaySize - 0.01f <= 0f)
                {
                    _overlaySize = 0.01f;
                    _outro = false;
                }
            }
            else
            {
                _overlaySize += MathHelper.Lerp(0f, 80f, 0.92f * (_overlaySize / 600f)); //It works, dont question why.
                _rotation += 0.08f;
                _inScreen.update();

                //Once the intro animation is done, switch to the new screen.
                if (_overlaySize + 0.01f >= 80f)
                {
                    ScreenManager.getInstance().setScreen(_inScreen);
                }
            }
        }
    }
}