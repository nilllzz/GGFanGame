using GameDevCommon.Drawing;
using GameDevCommon.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace GGFanGame.Screens.Menu
{
    internal class TestScreen : Screen
    {
        private float _ggoffsetX, _ggoffsetY, _stoffsetX, _stoffsetY;
        private SpriteBatch _batch;

        public TestScreen()
        {
            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
        }

        public override void Draw(GameTime time)
        {
            _batch.Begin(SpriteBatchUsage.Default);

            //TESTS FOR THE DRAWING CLASS:
            //Game grumps:

            //Steam train:

            if (_fadeLeft >= 0.5)
            {
                DrawSteamTrain();
                DrawGrumps();
            }
            else
            {
                DrawGrumps();
                DrawSteamTrain();
            }

            _batch.End();
        }

        private void DrawGrumps()
        {
            var extraOffsetX = (int)((100 * _fadeLeft) - (100 * _fadeRight));

            _batch.DrawGradient(new Rectangle(0, 0, 400 + extraOffsetX, 480), new Color(244, 131, 55), new Color(244, 170, 73), false);

            for (var x = -6; x < 19; x++)
            {
                for (var y = 0; y < 25; y++)
                {
                    var posX = (int)(x * 24 + y * 8 + _ggoffsetX);
                    var posY = (int)(y * 24 - (x * 8) + _ggoffsetY);

                    var colorShift = (double)posY / 480;
                    var cR = 4 * colorShift;
                    var cG = 35 * colorShift;
                    var cB = 20 * colorShift;

                    double cA = 255;
                    if (posX > 270 + extraOffsetX)
                    {
                        cA = 255 - ((posX - (270 + extraOffsetX)) * 2);
                        if (cA < 0)
                        {
                            cA = 0;
                        }
                    }

                    if (posX + 16 >= 0 && posX < 400 + extraOffsetX && posY + 16 >= 0 && posY < 480)
                    {
                        _batch.DrawCircle(new Vector2(posX, posY), 16, new Color((int)(241 + cR), (int)(118 + cG), (int)(50 + cB), (int)cA));
                    }
                }
            }
            _batch.Draw(GameInstance.Content.Load<Texture2D>(@"UI\Logos\GameGrumps"), new Rectangle(0 + extraOffsetX / 2, 100, 400, 225), Color.White);
            _batch.DrawRectangle(new Rectangle(0, 0, 400 + extraOffsetX, 480), new Color(0, 0, 0, (int)(130 * _fadeRight)));
            _batch.DrawRectangle(new Rectangle(388 + extraOffsetX, 0, 12, 480), new Color(0, 0, 0, (int)(100 * _fadeRight)));
        }

        private void DrawSteamTrain()
        {
            var extraOffsetX = (int)((100 * _fadeLeft) - (100 * _fadeRight));

            _batch.DrawGradient(new Rectangle(400 + extraOffsetX, 0, 400 - extraOffsetX, 480), new Color(78, 143, 249), new Color(151, 186, 251), false);

            for (var x = -9; x < 17; x++)
            {
                for (var y = -1; y < 24; y++)
                {
                    var posX = (int)(x * 24 + y * 8 + _stoffsetX) + 400;
                    var posY = (int)(y * 24 - (x * 8) + _stoffsetY);

                    var colorShift = (double)posY / 480;
                    var cR = 79 * colorShift;
                    var cG = 50 * colorShift;
                    var cB = 6 * colorShift;

                    double cA = 255;

                    if (posX < 400 + extraOffsetX + 90)
                    {
                        cA = 255 - ((400 + extraOffsetX + 90 - posX) * 3);
                        if (cA < 0)
                        {
                            cA = 0;
                        }
                    }

                    if (posX + 16 >= 400 + extraOffsetX && posX < 800  && posY + 16 >= 0 && posY < 480)
                    {
                        _batch.DrawCircle(new Vector2(posX, posY), 16, new Color((int)(71 + cR), (int)(133 + cG), (int)(244 + cB), (int)cA));
                    }
                }
            }
            _batch.DrawCircle(new Vector2(620 + extraOffsetX / 2, 345), 150, new Color(255, 255, 255, 130));

            _batch.DrawCircle(new Vector2(400 + extraOffsetX / 2, 350), 400, Color.White);


            _batch.DrawCircle(new Vector2(430 + extraOffsetX / 2, 360), 90, Color.White, 1d);
            _batch.DrawCircle(new Vector2(415 + extraOffsetX / 2, 345), 120, new Color(255, 255, 255, 130));
            _batch.DrawCircle(new Vector2(445 + extraOffsetX / 2, 375), 60, new Color(204, 218, 247));
            _batch.DrawCircle(new Vector2(452 + extraOffsetX / 2, 385), 40, Color.White);

            _batch.DrawCircle(new Vector2(630 + extraOffsetX / 2, 410), 20, new Color(204, 218, 247));

            _batch.DrawCircle(new Vector2(670 + extraOffsetX / 2, 390), 30, new Color(204, 218, 247));

            _batch.DrawCircle(new Vector2(540 + extraOffsetX / 2, 375), 40, new Color(204, 218, 247));

            _batch.DrawCircle(new Vector2(580 + extraOffsetX / 2, 405), 60, new Color(204, 218, 247));
            _batch.DrawCircle(new Vector2(588 + extraOffsetX / 2, 415), 40, Color.White);

            _batch.Draw(GameInstance.Content.Load<Texture2D>(@"UI\Logos\SteamTrain"), new Rectangle(400 + extraOffsetX / 2, 100, 400, 225), Color.White);
            _batch.DrawRectangle(new Rectangle(400 + extraOffsetX, 0, 400 - extraOffsetX, 480), new Color(0, 0, 0, (int)(130 * _fadeLeft)));
            _batch.DrawRectangle(new Rectangle(400 + extraOffsetX, 0, 12, 480), new Color(0, 0, 0, (int)(100 * _fadeLeft)));
        }

        private float _fadeLeft = 1f;
        private float _fadeRight;
        private bool _selection = true;

        public override void Update(GameTime time)
        {
            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.DPadLeft) || GetComponent<KeyboardHandler>().KeyPressed(Keys.Left))
            {
                _selection = true;
            }
            if (GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.DPadRight) || GetComponent<KeyboardHandler>().KeyPressed(Keys.Right))
            {
                _selection = false;
            }

            if (_selection)
            {
                _ggoffsetX -= 0.9f;
                _ggoffsetY += 0.3f;

                if (_ggoffsetX <= -24f)
                {
                    _ggoffsetX = 0;
                    _ggoffsetY = 0;
                }

                if (_fadeRight > 0f)
                {
                    _fadeRight = MathHelper.Lerp(0f, _fadeRight, 0.9f);
                }
                if (_fadeLeft < 1f)
                {
                    _fadeLeft = MathHelper.Lerp(1f, _fadeLeft, 0.9f);
                }
            }
            else
            {
                _stoffsetX -= 0.9f;
                _stoffsetY += 0.3f;

                if (_stoffsetX <= -24f)
                {
                    _stoffsetX = 0;
                    _stoffsetY = 0;
                }
                if (_fadeLeft > 0f)
                {
                    _fadeLeft = MathHelper.Lerp(0f, _fadeLeft, 0.9f);
                }
                if (_fadeRight < 1f)
                {
                    _fadeRight = MathHelper.Lerp(1f, _fadeRight, 0.9f);
                }
            }
        }
    }
}
