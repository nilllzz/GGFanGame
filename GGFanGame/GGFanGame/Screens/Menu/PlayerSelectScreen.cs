using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Screens.Menu
{
    /// <summary>
    /// Enables the player(s) to select a character.
    /// </summary>
    class PlayerSelectScreen : Screen
    {
        //The colors used for the four player select displays:
        //We add an alpha value of 180 because that is the default which we use to draw the overlay with.
        private readonly Color oneUpColor = new Color(103, 204, 252, 180);
        private readonly Color twoUpColor = new Color(245, 204, 43, 180);
        private readonly Color threeUpColor = new Color(215, 71, 213, 180);
        private readonly Color fourUpColor = new Color(215, 67, 110, 180);

        private int[] _selections = new int[4]; //These save the selected grump (index) for each player select display.
        private bool[] _activatedPlayers = new bool[4]; //These save if a specific player select display has choosen a character.
        private Texture2D[] _menuElements = new Texture2D[4]; //Stores the "1Up" etc. textures.
        private int[] _selectedAnimations = new int[4]; //Stores the animation progress for when the player selects a character.
        private int[] _switchedAnimations = new int[4]; //Stores the animation progress for when the player switches characters.
        private int[] _randomCharacters = new int[4]; //When the player selects "random character", this will count down until they get their character.

        //These store the grumps, grump overlays and grump name textures:
        //The grump overlays are white copies of the normal textures and used to overlay with an alpha over the normal grump texture when not selected.
        private Texture2D[] _grumps = new Texture2D[5];
        private Texture2D[] _grumps_overlay = new Texture2D[5];
        private Texture2D[] _grumps_names = new Texture2D[5];

        private SpriteFont _grumpFont = null;

        public PlayerSelectScreen(GGGame game) : base(Identification.PlayerSelect, game)
        {
            _grumpFont = game.Content.Load<SpriteFont>("CartoonFont");

            loadGrumpTexture(0, "Arin");
            loadGrumpTexture(1, "Danny");
            loadGrumpTexture(2, "Barry");
            loadGrumpTexture(3, "Ross");
            loadGrumpTexture(4, "Suzy");

            for (int i = 0; i < 4; i++)
            {
                _menuElements[i] = gameInstance.Content.Load<Texture2D>(@"GrumpCade\" + (i + 1).ToString() + "Up");
            }

            //Set start selections:
            for (int i = 0; i < 4; i++)
            {
                _selections[i] = i;
            }
        }

        /// <summary>
        /// Loads the textures for a grump.
        /// </summary>
        /// <param name="index">The index the grump appears at in the selection.</param>
        /// <param name="name">The name of the grump.</param>
        private void loadGrumpTexture(int index, string name)
        {
            _grumps[index] = gameInstance.Content.Load<Texture2D>(@"GrumpCade\" + name);
            _grumps_overlay[index] = gameInstance.Content.Load<Texture2D>(@"GrumpCade\" + name + "_Overlay");
            _grumps_names[index] = gameInstance.Content.Load<Texture2D>(@"Names\" + name + "_name");
        }

        public override void draw(GameTime gameTime)
        {
            UI.Graphics.drawRectangle(gameInstance.clientRectangle, Color.Black);

            //We draw the selections here:

            //Drawing the selections at the center of the screen:
            int startX = (gameInstance.clientRectangle.Width - 1030) / 2;
            int startY = 50;

            drawGrumpSelect(0, new Vector2(startX, startY), oneUpColor);
            drawGrumpSelect(1, new Vector2(startX + 260, startY), twoUpColor);
            drawGrumpSelect(2, new Vector2(startX + 520, startY), threeUpColor);
            drawGrumpSelect(3, new Vector2(startX + 780, startY), fourUpColor);

            //Now, draw a black overlay on the bottom to hide the incoming dash lines:
            UI.Graphics.drawRectangle(new Rectangle(0, startY + 480, gameInstance.clientRectangle.Width, gameInstance.clientRectangle.Height - 480 - startY), Color.Black);

            if (_activatedPlayers[0])
            {
                string text = "Press START to Begin!";
                Vector2 textSize = _grumpFont.MeasureString(text);

                gameInstance.fontBatch.DrawString(_grumpFont, text,
                    new Vector2(gameInstance.clientRectangle.Width / 2 - textSize.X / 2,
                                startY + 480 + 50), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draws a single grump selection.
        /// </summary>
        /// <param name="index">The index of this selection.</param>
        /// <param name="offset">The offset applied to this selection.</param>
        /// <param name="color">The accent color.</param>
        private void drawGrumpSelect(int index, Vector2 offset, Color color)
        {
            //This value stores the added Y position of the sprite when switching characters.
            //It calculates from a sinus function and the switchedAnimations array.
            float addedY = (float)Math.Sin(_switchedAnimations[index]) * 4f;

            //Only when the player has chosen a character, draw the dash line and a shadow.
            if (_activatedPlayers[index])
            {
                UI.Graphics.drawLine(new Vector2(offset.X - 90 + _selectedAnimations[index] / 4.8f,
                                                 offset.Y + 480 - _selectedAnimations[index] + 480),
                                     new Vector2(offset.X + 30 + _selectedAnimations[index] / 4.8f,
                                                 offset.Y + 480 - _selectedAnimations[index]),
                                     new Color(color.R, color.G, color.B, 255), 120);
                gameInstance.spriteBatch.Draw(_grumps[_selections[index]], new Rectangle((int)offset.X + 30, (int)offset.Y + 190, (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), new Color(0, 0, 0, 100));
            }

            //Draw a black overlay in the back over the dash line to hide it behind the UI on the top:
            UI.Graphics.drawRectangle(new Rectangle((int)offset.X - 10, (int)offset.Y, 270, 160), Color.Black);
            //Draws the UI element on top:
            gameInstance.spriteBatch.Draw(_menuElements[index], new Rectangle((int)offset.X, (int)offset.Y + 30, 220, 120), Color.White);

            //Draws the grump character:
            gameInstance.spriteBatch.Draw(_grumps[_selections[index]], new Rectangle((int)offset.X + 20, (int)(offset.Y + 180 + addedY), (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), Color.White);

            //When the player has NOT chosen a character, draw a colored overlay:
            if (!_activatedPlayers[index])
            {
                gameInstance.spriteBatch.Draw(_grumps_overlay[_selections[index]], new Rectangle((int)offset.X + 20, (int)(offset.Y + 180 + addedY), (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), color);
            }
            else //otherwise...
            {
                int alpha = 255;
                int size = 0;

                //While the animation is going on
                if (_selectedAnimations[index] < 480)
                {
                    //Get an alpha and size for a quick animation that plays when selecting a character:
                    alpha = (int)(255 * (double)_selectedAnimations[index] / 480);
                    size = (int)(80 * (double)_selectedAnimations[index] / 480);

                    //Draw a white transparent overlay when selecting a grump:
                    gameInstance.spriteBatch.Draw(_grumps_overlay[_selections[index]],
                        new Rectangle((int)offset.X + 20 - size / 2,
                                      (int)offset.Y + 180 - size / 2,
                                      (int)(_grumps[_selections[index]].Width * 0.5) + size,
                                      (int)(_grumps[_selections[index]].Height * 0.5) + size), new Color(255, 255, 255, 255 - alpha));
                }

                //Draw the name of the selected grump in the UI:
                gameInstance.fontBatch.Draw(_grumps_names[_selections[index]], new Rectangle((int)offset.X + 20, (int)offset.Y + 90, (int)(_grumps_names[_selections[index]].Width / 1.6), (int)(_grumps_names[_selections[index]].Height / 1.6)), new Color(color.R, color.G, color.B, alpha));
                if (_selectedAnimations[index] < 480)
                    gameInstance.fontBatch.Draw(_grumps_names[_selections[index]], new Rectangle((int)offset.X + 20 - size / 2, (int)offset.Y + 90 - size / 4, (int)(_grumps_names[_selections[index]].Width / 1.6) + size, (int)(_grumps_names[_selections[index]].Height / 1.6) + size / 2), new Color(color.R, color.G, color.B, 255 - alpha));
            }
        }

        public override void update(GameTime gameTime)
        {
            //Cycle through the connected GamePads:
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex playerIndex = (PlayerIndex)i;

                //Only when the player has not selected a character, they can select a different one:
                if (!_activatedPlayers[i] && _randomCharacters[i] == 0)
                {
                    if (Input.GamePadHandler.buttonPressed(playerIndex, Buttons.Y))
                    {
                        _randomCharacters[i] = new Random().Next(5, 12);
                    }
                    else
                    {
                        if (Input.ControlsHandler.upPressed(playerIndex, new Input.InputDirectionType[] { Input.InputDirectionType.All }))
                        {
                            _selections[i]--;
                            _switchedAnimations[i] = 10;
                        }
                        if (Input.ControlsHandler.downPressed(playerIndex, new Input.InputDirectionType[] { Input.InputDirectionType.All }))
                        {
                            _selections[i]++;
                            _switchedAnimations[i] = 10;
                        }

                        if (_selections[i] < 0)
                            _selections[i] = _grumps.Length - 1;
                        else if (_selections[i] > _grumps.Length - 1)
                            _selections[i] = 0;

                        //When A is pressed, select a character:
                        if (Input.GamePadHandler.buttonPressed(playerIndex, Buttons.A) || (Input.KeyboardHandler.keyPressed(Keys.Enter) && i == 0))
                        {
                            _activatedPlayers[i] = true;
                            _selectedAnimations[i] = 0;
                        }
                    }
                }
                else if (_activatedPlayers[i])
                {
                    //When B is pressed, revoke the selection:
                    if (Input.GamePadHandler.buttonPressed(playerIndex, Buttons.B) || !Input.GamePadHandler.isConnected(playerIndex))
                    {
                        _activatedPlayers[i] = false;
                    }
                }
                else if (_randomCharacters[i] > 0)
                {
                    //When the randomizer is running, update it:
                    if (!Input.GamePadHandler.isConnected(playerIndex))
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
    }
}