using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Screens.Menu
{
    internal class NewGameScreen : Screen
    {
        private readonly MenuBackgroundRenderer _backgroundRenderer;
        private readonly Screen _preScreen;

        private static readonly char[]
            _upperCaseAlphabet =
                ASCIIProvider.GetChars(new[] { new Tuple<int, int>(65, 26), new Tuple<int, int>(48, 10) }),
            _lowerCaseAlphabet =
                ASCIIProvider.GetChars(new[] { new Tuple<int, int>(97, 26), new Tuple<int, int>(48, 10) });
        private static readonly char[][] _alphabets = { _upperCaseAlphabet, _lowerCaseAlphabet };
        private static readonly int[]
            _charsPerRow =
                { 10, 10, 6, 10 };
        private readonly SpriteFont _font;
        private int _alphabetIndex = 0;

        public NewGameScreen(Screen preScreen, MenuBackgroundRenderer backgroundRenderer)
        {
            _preScreen = preScreen;
            _backgroundRenderer = backgroundRenderer;

            _font = Content.Load<SpriteFont>("Fonts\\CartoonFont");
        }

        public override void Draw()
        {
            _backgroundRenderer.Draw();
            DrawNameSelection();
        }

        private void DrawNameSelection()
        {
            var alphabet = _alphabets[_alphabetIndex];
            int charIndex = 0;
            int rowIndex = 0;

            foreach (var row in _charsPerRow)
            {
                for (int i = 0; i < row; i++)
                {
                    var c = alphabet[charIndex++];
                    GameInstance.SpriteBatch.DrawString(_font, c.ToString(), new Vector2(i * 32, rowIndex * 32), Color.White);
                }
                rowIndex++;
            }
        }

        public override void Update()
        {
            _backgroundRenderer.Update();
        }
    }
}
