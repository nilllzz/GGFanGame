using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game
{
    /// <summary>
    /// This class displays action words on the stage screen.
    /// </summary>
    sealed class ActionWord : StageObject
    {
        private static Random wordRnd = new Random();
        private static Dictionary<ActionWordType, string[]> _wordGroups;

        /// <summary>
        /// This initializes the possible word arrays.
        /// </summary>
        private static void initializeWords()
        {
            if (_wordGroups == null)
            {
                _wordGroups = new Dictionary<ActionWordType, string[]>();

                _wordGroups.Add(ActionWordType.HurtEnemy, new string[] { "Uhh", "Ahh", "Arg", "Huu", "Ehh" });
                _wordGroups.Add(ActionWordType.HurtPlayer, new string[] { "Ech", "Shiet", "Damn", "Dammit", "Urg", "Ahh", "Garg" });
                _wordGroups.Add(ActionWordType.Landing, new string[] { "Tud" });
            }
        }

        /// <summary>
        /// Returns a random word string for a specific word type.
        /// </summary>
        public static string getWordText(ActionWordType wordType)
        {
            initializeWords();

            string[] words = _wordGroups[wordType];
            return words[wordRnd.Next(0, words.Length)];
        }

        //one-time values:
        private string _text = "";
        private Color _color;

        private float _targetSize;

        //changing:
        private float _size;
        private double _delay;
        private float _rotation = 0f;

        private SpriteFont _grumpFont;

        public ActionWord(string text, Color color, float targetSize, Vector3 position)
        {
            _grumpFont = gameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFont");

            _text = text;
            _color = color;
            _targetSize = targetSize;
            this.position = position;
        }

        public override void draw()
        {
            Vector2 fontSize = _grumpFont.MeasureString(_text) * _size;

            gameInstance.spriteBatch.DrawString(_grumpFont, _text, new Vector2(X - fontSize.X / 2f, Z - Y - fontSize.Y / 2f), _color, 0f, Vector2.Zero, _size, SpriteEffects.None, 0f);
        }

        public override Point getDrawingSize()
        {
            Vector2 textSize = _grumpFont.MeasureString(_text) * _size;
            return textSize.ToPoint();
        }

        public override Vector3 getFeetPosition()
        {
            Vector2 fontSize = _grumpFont.MeasureString(_text) * _size;
            return new Vector3(X + fontSize.X / 2f, Y, Z + fontSize.Y);
        }

        public override void update()
        {
            if (_size < _targetSize)
            {
                _size = MathHelper.Lerp(_targetSize, _size, 0.8f);

                if (_size + 0.01f >= _targetSize)
                {
                    _size = _targetSize;
                    _delay = 10d;
                }
            }
            else
            {
                if (_delay > 0d)
                {
                    _delay--;
                    Y = Y + 0.9f;
                    if (_delay <= 0d)
                    {
                        canBeRemoved = true;
                    }
                }
            }
            _rotation += 0.01f;
        }
    }
}