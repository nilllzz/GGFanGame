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
    internal class NewGameScreen : Screen
    {
        private const ushort ALPHABET_WIDTH = 156; // half of approximated width of the alphabet.

        private MenuBackgroundRenderer _backgroundRenderer;
        private readonly Screen _preScreen;

        private static readonly char[]
            _upperCaseAlphabet =
                ASCIIProvider.GetChars(new[] { (65, 26), (48, 10) }),
            _lowerCaseAlphabet =
                ASCIIProvider.GetChars(new[] { (97, 26), (48, 10) });
        private static readonly char[][] _alphabets = { _upperCaseAlphabet, _lowerCaseAlphabet };
        private static readonly ushort[] _charsPerRow = { 10, 10, 6, 10 };
        private readonly SpriteFont _font;
        private int _alphabetIndex = 0;
        private double _charRotation = 0D;
        private ushort _cursorX = 0, _cursorY = 0;
        private float _selectedCharScale = 2f;
        private string _name = string.Empty;
        private readonly Texture2D _arinFace;
        private ushort _arinSad = 0;
        private SpriteBatch _batch, _fontBatch;

        private static readonly Color
            DefaultLetterColor = new Color(254, 242, 205),
            HighlightLetterColor = new Color(208, 67, 32);   

        public NewGameScreen(Screen preScreen, MenuBackgroundRenderer backgroundRenderer)
        {
            _preScreen = preScreen;
            _backgroundRenderer = backgroundRenderer;

            _font = Content.Load<SpriteFont>(Resources.Fonts.CartoonFont);
            _arinFace = Content.Load<Texture2D>(Resources.UI.HUD.Arin);

            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
            _fontBatch = new SpriteBatch(GameInstance.GraphicsDevice);
        }

        public override void Draw()
        {
            _batch.Begin(SpriteBatchUsage.Default);
            _fontBatch.Begin(SpriteBatchUsage.Font);

            _backgroundRenderer.Draw(_batch);
            DrawCharSelection();
            DrawNameField();

            _batch.End();
            _fontBatch.End();
        }

        private void DrawNameField()
        {
            Vector2 positionOffset = new Vector2(GameInstance.ClientRectangle.Width / 2 - ALPHABET_WIDTH, 350);

            ushort arinFaceIndex = 0;
            if (_arinSad > 0)
                arinFaceIndex = 1;
            else if (_name.Length == 0)
                arinFaceIndex = 2;

            _batch.Draw(_arinFace, new Rectangle((int)positionOffset.X - 60, (int)positionOffset.Y - 6, 48, 48),
                new Rectangle(arinFaceIndex * 48, 0, 48, 48), Color.White);

            _batch.DrawRectangle(new Rectangle((int)positionOffset.X - 10, (int)positionOffset.Y - 10, ALPHABET_WIDTH * 2 + 20, 56),
                new Color(0, 0, 0, 150));

            _fontBatch.DrawString(_font, _name, positionOffset, DefaultLetterColor);
        }

        private void DrawCharSelection()
        {
            Vector2 positionOffset = new Vector2(GameInstance.ClientRectangle.Width / 2 - ALPHABET_WIDTH, 140);

            _batch.DrawRectangle(new Rectangle((int)positionOffset.X - 10, (int)positionOffset.Y - 10, ALPHABET_WIDTH * 2 + 20, 150), 
                new Color(0, 0, 0, 150));
                
            var alphabet = _alphabets[_alphabetIndex];
            int charIndex = 0;
            int rowIndex = 0;

            foreach (var rowLength in _charsPerRow)
            {
                for (int i = 0; i < rowLength; i++)
                {
                    var c = alphabet[charIndex++];
                    var rotation = 0d;
                    var color = DefaultLetterColor;
                    var scale = 1f;

                    if (_cursorX == i && _cursorY == rowIndex)
                    {
                        rotation = Math.Sin(_charRotation) * 0.5f;
                        color = HighlightLetterColor;
                        scale = _selectedCharScale;
                    }

                    Vector2 charSize = _font.MeasureString(c.ToString());

                    _fontBatch.DrawString(_font, c.ToString(), new Vector2(i * 32, rowIndex * 32) + charSize / 2 + positionOffset,
                        color, (float)rotation, charSize / 2, scale, SpriteEffects.None, 0f);
                }
                rowIndex++;
            }
        }

        public override void Update()
        {
            _backgroundRenderer.Update();

            _charRotation += 0.1D;

            if (_selectedCharScale > 1f)
            {
                _selectedCharScale -= 0.2f;
                if (_selectedCharScale <= 1f)
                    _selectedCharScale = 1f;
            }

            bool movedCursor = false;
            if (GetComponent<ControlsHandler>().DownPressed(PlayerIndex.One))
            {
                do
                {
                    _cursorY++;
                    if (_cursorY == _charsPerRow.Length)
                        _cursorY = 0;
                } while (_charsPerRow[_cursorY] <= _cursorX);
                
                movedCursor = true;
            }
            if (GetComponent<ControlsHandler>().UpPressed(PlayerIndex.One))
            {
                do
                {
                    if (_cursorY == 0)
                        _cursorY = (ushort)(_charsPerRow.Length - 1);
                    else
                        _cursorY--;
                } while (_charsPerRow[_cursorY] <= _cursorX);

                movedCursor = true;
            }
            if (GetComponent<ControlsHandler>().RightPressed(PlayerIndex.One))
            {
                _cursorX++;
                if (_cursorX == _charsPerRow[_cursorY])
                    _cursorX = 0;

                movedCursor = true;
            }
            if (GetComponent<ControlsHandler>().LeftPressed(PlayerIndex.One))
            {
                if (_cursorX == 0)
                    _cursorX = (ushort)(_charsPerRow[_cursorY] - 1);
                else
                    _cursorX--;

                movedCursor = true;
            }

            if (movedCursor)
                _selectedCharScale = 2f;

            if (_name.Length < 10 && GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.A))
            {
                ushort cIndex = _cursorX;
                for (int i = 0; i < _cursorY; i++)
                    cIndex += _charsPerRow[i];

                _name += _alphabets[_alphabetIndex][cIndex];
            }

            if (_name.Length > 0 && GetComponent<GamePadHandler>().ButtonPressed(PlayerIndex.One, Buttons.B))
            {
                _name = _name.Substring(0, _name.Length - 1);
                _arinSad = 10;
            }

            if (_arinSad > 0)
                _arinSad--;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_batch != null && !_batch.IsDisposed) _batch.Dispose();
                    if (_fontBatch != null && !_fontBatch.IsDisposed) _fontBatch.Dispose();
                    if (_backgroundRenderer != null && !_backgroundRenderer.IsDisposed) _backgroundRenderer.Dispose();
                }

                _batch = null;
                _fontBatch = null;
                _backgroundRenderer = null;
            }

            base.Dispose(disposing);
        }
    }
}
