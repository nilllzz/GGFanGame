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
        private readonly Color fourUpColor = new Color(215, 71, 213, 180);

        private int[] _selections = new int[4]; //These save the selected grump (index) for each player select display.
        private bool[] _activatedPlayers = new bool[4]; //These save if a specific player select display has choosen a character.
        private Texture2D[] _menuElements = new Texture2D[4]; //Stores the "1Up" etc. textures.
        private int[] _selectedAnimations = new int[4]; //Stores the animation progress for when the player selects a character.
        private int[] _switchedAnimations = new int[4]; //Stores the animation progress for when the player switches characters.

        //These store the grumps, grump overlays and grump name textures:
        //The grump overlays are white copies of the normal textures and used to overlay with an alpha over the normal grump texture when not selected.
        private Texture2D[] _grumps = new Texture2D[5];
        private Texture2D[] _grumps_overlay = new Texture2D[5];
        private Texture2D[] _grumps_names = new Texture2D[5];

        public PlayerSelectScreen(GGGame game) : base(Identification.PlayerSelect, game)
        {
            loadGrumpTexture(0, "Arin");
            loadGrumpTexture(1, "Danny");
            loadGrumpTexture(2, "Barry");
            loadGrumpTexture(3, "Ross");
            loadGrumpTexture(4, "Suzy");

            for (int i = 0; i < 4; i++)
            {
                _menuElements[i] = gameInstance.Content.Load<Texture2D>(@"GrumpCade\" + (i + 1).ToString() + "Up");
            }

            _selections[1] = 1;
            _selections[2] = 2;
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
            drawGrumpSelect(0, 30, oneUpColor);
            drawGrumpSelect(1, 290, twoUpColor);
            drawGrumpSelect(2, 550, threeUpColor);
        }

        /// <summary>
        /// Draws a single grump selection.
        /// </summary>
        /// <param name="index">The index of that selection.</param>
        /// <param name="posX">The horizontal start position.</param>
        /// <param name="color">The accent color.</param>
        private void drawGrumpSelect(int index, int posX, Color color)
        {
            //This value stores the added Y position of the sprite when switching characters.
            //It calculates from a sinus function and the switchedAnimations array.
            float addedY = (float)Math.Sin(_switchedAnimations[index]) * 4f;

            //Only when the player has chosen a character, draw the dash line and a shadow.
            if (_activatedPlayers[index])
            {
                UI.Graphics.drawLine(new Vector2(posX - 90 + _selectedAnimations[index] / 4.8f,
                                                480 - _selectedAnimations[index] + 480),
                                     new Vector2(posX + 30 + _selectedAnimations[index] / 4.8f,
                                                480 - _selectedAnimations[index]),
                                     new Color(color.R, color.G, color.B, 255), 120);
                gameInstance.spriteBatch.Draw(_grumps[_selections[index]], new Rectangle(posX + 30, 190, (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), new Color(0, 0, 0, 100));
            }

            //Draw a black overlay in the back over the dash line to hide it behind the UI on the top:
            UI.Graphics.drawRectangle(new Rectangle(posX - 10, 0, 270, 160), Color.Black);
            //Draws the UI element on top:
            gameInstance.spriteBatch.Draw(_menuElements[index], new Rectangle(posX, 30, 220, 120), Color.White);

            //Draws the grump character:
            gameInstance.spriteBatch.Draw(_grumps[_selections[index]], new Rectangle(posX + 20, (int)(180 + addedY), (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), Color.White);

            //When the player has NOT chosen a character, draw a colored overlay:
            if (!_activatedPlayers[index])
            {
                gameInstance.spriteBatch.Draw(_grumps_overlay[_selections[index]], new Rectangle(posX + 20, (int)(180 + addedY), (int)(_grumps[_selections[index]].Width * 0.5), (int)(_grumps[_selections[index]].Height * 0.5)), color);
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
                        new Rectangle(posX + 20 - size / 2,
                                     180 - size / 2,
                                     (int)(_grumps[_selections[index]].Width * 0.5) + size,
                                     (int)(_grumps[_selections[index]].Height * 0.5) + size), new Color(255, 255, 255, 255 - alpha));
                }

                //Draw the name of the selected grump in the UI:
                gameInstance.spriteBatch.Draw(_grumps_names[_selections[index]], new Rectangle(posX + 20, 90, (int)(_grumps_names[_selections[index]].Width / 1.6), (int)(_grumps_names[_selections[index]].Height / 1.6)), new Color(color.R, color.G, color.B, alpha));
                if (_selectedAnimations[index] < 480)
                    gameInstance.spriteBatch.Draw(_grumps_names[_selections[index]], new Rectangle(posX + 20 - size / 2, 90 - size / 4, (int)(_grumps_names[_selections[index]].Width / 1.6) + size, (int)(_grumps_names[_selections[index]].Height / 1.6) + size / 2), new Color(color.R, color.G, color.B, 255 - alpha));
            }
        }

        public override void update(GameTime gameTime)
        {
            //Cycle through the connected GamePads:
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex playerIndex = (PlayerIndex)i;

                //Only when the player has not selected a character, they can select a different one:
                if (!_activatedPlayers[i])
                {
                    if (Input.GamePadHandler.buttonPressed(Buttons.DPadUp, playerIndex))
                    {
                        _selections[i]--;
                        _switchedAnimations[i] = 10;
                    }
                    if (Input.GamePadHandler.buttonPressed(Buttons.DPadDown, playerIndex))
                    {
                        _selections[i]++;
                        _switchedAnimations[i] = 10;
                    }

                    if (_selections[i] < 0)
                        _selections[i] = _grumps.Length - 1;
                    else if (_selections[i] > _grumps.Length - 1)
                        _selections[i] = 0;

                    //When A is pressed, select a character:
                    if (Input.GamePadHandler.buttonPressed(Buttons.A, playerIndex) || (Input.KeyboardHandler.keyPressed(Keys.Enter) && i == 0))
                    {
                        _activatedPlayers[i] = true;
                        _selectedAnimations[i] = 0;
                    }
                }
                else
                {
                    //When B is pressed, revoke the selection:
                    if (Input.GamePadHandler.buttonPressed(Buttons.B, playerIndex))
                    {
                        _activatedPlayers[i] = false;
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