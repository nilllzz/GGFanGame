using System;
using GGFanGame.Content;
using GGFanGame.Drawing;
using GGFanGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Core;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// Enables the player(s) to select a character.
    /// </summary>
    internal class PlayerSelectScreen : Screen
    {
        private readonly int[] _selections = new int[4]; //These save the selected grump (index) for each player select display.
        private readonly bool[] _activatedPlayers = new bool[4]; //These save if a specific player select display has choosen a character.
        private readonly Texture2D[] _menuElements = new Texture2D[4]; //Stores the "1Up" etc. textures.
        private readonly int[] _selectedAnimations = new int[4]; //Stores the animation progress for when the player selects a character.
        private readonly int[] _switchedAnimations = new int[4]; //Stores the animation progress for when the player switches characters.
        private readonly int[] _randomCharacters = new int[4]; //When the player selects "random character", this will count down until they get their character.

        //These store the grumps, grump overlays and grump name textures:
        //The grump overlays are white copies of the normal textures and used to overlay with an alpha over the normal grump texture when not selected.
        private readonly Texture2D[] _grumps = new Texture2D[5];
        private readonly Texture2D[] _grumps_overlay = new Texture2D[5];
        private readonly Texture2D[] _grumps_names = new Texture2D[5];

        private readonly SpriteFont _grumpFont = null;

        private SpriteBatch _batch, _fontBatch;

        public PlayerSelectScreen()
        {
            _grumpFont = GameInstance.Content.Load<SpriteFont>(Resources.Fonts.CartoonFont);
            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
            _fontBatch = new SpriteBatch(GameInstance.GraphicsDevice);

            LoadGrumpTexture(0, "Arin");
            LoadGrumpTexture(1, "Danny");
            LoadGrumpTexture(2, "Barry");
            LoadGrumpTexture(3, "Ross");
            LoadGrumpTexture(4, "Suzy");

            for (var i = 0; i < 4; i++)
            {
                _menuElements[i] = GameInstance.Content.Load<Texture2D>(@"UI\GrumpCade\" + (i + 1).ToString() + "Up");
            }

            //Set start selections:
            for (var i = 0; i < 4; i++)
            {
                _selections[i] = i;
            }
        }

        /// <summary>
        /// Loads the textures for a grump.
        /// </summary>
        /// <param name="index">The index the grump appears at in the selection.</param>
        /// <param name="name">The name of the grump.</param>
        private void LoadGrumpTexture(int index, string name)
        {
            _grumps[index] = GameInstance.Content.Load<Texture2D>(@"UI\GrumpCade\" + name);
            _grumps_overlay[index] = GameInstance.Content.Load<Texture2D>(@"UI\GrumpCade\" + name + "_Overlay");
            _grumps_names[index] = GameInstance.Content.Load<Texture2D>(@"UI\Names\" + name + "_name");
        }

        public override void Draw()
        {
            _batch.Begin(SpriteBatchUsage.Default);
            _fontBatch.Begin(SpriteBatchUsage.Font);

            _batch.DrawRectangle(GameInstance.ClientRectangle, Color.Black);

            //We draw the selections here:

            //Drawing the selections at the center of the screen:
            var startX = (GameInstance.ClientRectangle.Width - 1030) / 2;
            var startY = 50;

            DrawGrumpSelect(0, new Vector2(startX, startY), Drawing.Colors.oneUpColor);
            DrawGrumpSelect(1, new Vector2(startX + 260, startY), Drawing.Colors.twoUpColor);
            DrawGrumpSelect(2, new Vector2(startX + 520, startY), Drawing.Colors.threeUpColor);
            DrawGrumpSelect(3, new Vector2(startX + 780, startY), Drawing.Colors.fourUpColor);

            //Now, draw a black overlay on the bottom to hide the incoming dash lines:
            _batch.DrawRectangle(new Rectangle(0, startY + 480, GameInstance.ClientRectangle.Width, GameInstance.ClientRectangle.Height - 480 - startY), Color.Black);

            if (_activatedPlayers[0])
            {
                const string text = "Press START to Begin!";
                var textSize = _grumpFont.MeasureString(text);

                _fontBatch.DrawString(_grumpFont, text,
                    new Vector2(GameInstance.ClientRectangle.Width / 2 - textSize.X / 2,
                                startY + 480 + 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            _batch.End();
            _fontBatch.End();
        }

        /// <summary>
        /// Draws a single grump selection.
        /// </summary>
        /// <param name="index">The index of this selection.</param>
        /// <param name="offset">The offset applied to this selection.</param>
        /// <param name="color">The accent color.</param>
        private void DrawGrumpSelect(int index, Vector2 offset, Color color)
        {
            //This value stores the added Y position of the sprite when switching characters.
            //It calculates from a sinus function and the switchedAnimations array.
            var addedY = (float)Math.Sin(_switchedAnimations[index]) * 4f;

            //Only when the player has chosen a character, draw the dash line and a shadow.
            if (_activatedPlayers[index])
            {
                _batch.DrawLine(new Vector2(offset.X - 90 + _selectedAnimations[index] / 4.8f,
                                                 offset.Y + 480 - _selectedAnimations[index] + 480),
                                     new Vector2(offset.X + 30 + _selectedAnimations[index] / 4.8f,
                                                 offset.Y + 480 - _selectedAnimations[index]),
                                     color, 120);
                _batch.Draw(_grumps[_selections[index]], new Rectangle((int)offset.X + 30, (int)offset.Y + 190, (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), new Color(0, 0, 0, 100));
            }

            //Draw a black overlay in the back over the dash line to hide it behind the UI on the top:
            _batch.DrawRectangle(new Rectangle((int)offset.X - 10, (int)offset.Y, 270, 160), Color.Black);
            //Draws the UI element on top:
            _batch.Draw(_menuElements[index], new Rectangle((int)offset.X, (int)offset.Y + 30, 220, 120), Color.White);

            //Draws the grump character:
            _batch.Draw(_grumps[_selections[index]], new Rectangle((int)offset.X + 20, (int)(offset.Y + 180 + addedY), (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), Color.White);

            //When the player has NOT chosen a character, draw a colored overlay:
            if (!_activatedPlayers[index])
            {
                _batch.Draw(_grumps_overlay[_selections[index]], new Rectangle((int)offset.X + 20, (int)(offset.Y + 180 + addedY), (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), new Color(color.R, color.G, color.B, 180));
            }
            else //otherwise...
            {
                var alpha = 255;
                var size = 0;

                //While the animation is going on
                if (_selectedAnimations[index] < 480)
                {
                    //Get an alpha and size for a quick animation that plays when selecting a character:
                    alpha = (int)(255 * (double)_selectedAnimations[index] / 480);
                    size = (int)(80 * (double)_selectedAnimations[index] / 480);

                    //Draw a white transparent overlay when selecting a grump:
                    _batch.Draw(_grumps_overlay[_selections[index]],
                        new Rectangle((int)offset.X + 20 - size / 2,
                                      (int)offset.Y + 180 - size / 2,
                                      (int)(_grumps[_selections[index]].Width * 0.5) + size,
                                      (int)(_grumps[_selections[index]].Height * 0.5) + size), new Color(255, 255, 255, 255 - alpha));
                }

                //Draw the name of the selected grump in the UI:
                _fontBatch.Draw(_grumps_names[_selections[index]], new Rectangle((int)offset.X + 20, (int)offset.Y + 90, (int)(_grumps_names[_selections[index]].Width / 1.6), (int)(_grumps_names[_selections[index]].Height / 1.6)), new Color(color.R, color.G, color.B, alpha));
                if (_selectedAnimations[index] < 480)
                    _fontBatch.Draw(_grumps_names[_selections[index]], new Rectangle((int)offset.X + 20 - size / 2, (int)offset.Y + 90 - size / 4, (int)(_grumps_names[_selections[index]].Width / 1.6) + size, (int)(_grumps_names[_selections[index]].Height / 1.6) + size / 2), new Color(color.R, color.G, color.B, 255 - alpha));
            }
        }

        public override void Update()
        {
            //Cycle through the connected GamePads:
            for (var i = 0; i < 4; i++)
            {
                var playerIndex = (PlayerIndex)i;

                //Only when the player has not selected a character, they can select a different one:
                if (!_activatedPlayers[i] && _randomCharacters[i] == 0)
                {
                    if (GetComponent<GamePadHandler>().ButtonPressed(playerIndex, Buttons.Y))
                    {
                        _randomCharacters[i] = new Random().Next(5, 12);
                    }
                    else
                    {
                        if (GetComponent<ControlsHandler>().UpPressed(playerIndex, new Input.InputDirectionType[] { Input.InputDirectionType.All }))
                        {
                            _selections[i]--;
                            _switchedAnimations[i] = 10;
                        }
                        if (GetComponent<ControlsHandler>().DownPressed(playerIndex, new Input.InputDirectionType[] { Input.InputDirectionType.All }))
                        {
                            _selections[i]++;
                            _switchedAnimations[i] = 10;
                        }

                        if (_selections[i] < 0)
                            _selections[i] = _grumps.Length - 1;
                        else if (_selections[i] > _grumps.Length - 1)
                            _selections[i] = 0;

                        //When A is pressed, select a character:
                        if (GetComponent<GamePadHandler>().ButtonPressed(playerIndex, Buttons.A) || (GetComponent<KeyboardHandler>().KeyPressed(Keys.Enter) && i == 0))
                        {
                            _activatedPlayers[i] = true;
                            _selectedAnimations[i] = 0;
                        }
                    }
                }
                else if (_activatedPlayers[i])
                {
                    //When B is pressed, revoke the selection:
                    if (GetComponent<GamePadHandler>().ButtonPressed(playerIndex, Buttons.B) || !GetComponent<GamePadHandler>().IsConnected(playerIndex))
                    {
                        _activatedPlayers[i] = false;
                    }
                }
                else if (_randomCharacters[i] > 0)
                {
                    //When the randomizer is running, update it:
                    if (!GetComponent<GamePadHandler>().IsConnected(playerIndex))
                    {
                        //When the GamePad is no longer connected, stop animation:
                        _randomCharacters[i] = 0;
                        _activatedPlayers[i] = false;
                        _switchedAnimations[i] = 0;
                    }
                    else
                    {
                        if (_switchedAnimations[i] == 0)
                        {
                            _selections[i]++;
                            if (_selections[i] < 0)
                                _selections[i] = _grumps.Length - 1;
                            else if (_selections[i] > _grumps.Length - 1)
                                _selections[i] = 0;

                            _switchedAnimations[i] = 10;
                            _randomCharacters[i]--;

                            if (_randomCharacters[i] == 0)
                            {
                                _activatedPlayers[i] = true;
                                _selectedAnimations[i] = 0;
                            }
                        }
                    }
                }

                //Update the animations:
                if (_activatedPlayers[i] && _selectedAnimations[i] < 480)
                {
                    _selectedAnimations[i] += 40;
                }
                if (_switchedAnimations[i] > 0)
                {
                    _switchedAnimations[i]--;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_batch != null && !_batch.IsDisposed) _batch.Dispose();
                    if (_fontBatch != null && !_fontBatch.IsDisposed) _fontBatch.Dispose();
                }

                _batch = null;
                _fontBatch = null;
            }

            base.Dispose(disposing);
        }
    }
}