using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game
{
    /// <summary>
    /// This class displays action words on the stage screen.
    /// </summary>
    internal sealed class ActionWord : StageObject
    {
        private static Dictionary<ActionWordType, string[]> _wordGroups;

        /// <summary>
        /// This initializes the possible word arrays.
        /// </summary>
        private static void InitializeWords()
        {
            if (_wordGroups == null)
            {
                _wordGroups = new Dictionary<ActionWordType, string[]>
                {
                    {ActionWordType.HurtEnemy, new[] {"Uhh", "Ahh", "Arg", "Huu", "Ehh"}},
                    {ActionWordType.HurtPlayer, new[] {"Ech", "Shiet", "Damn", "Dammit", "Urg", "Ahh", "Garg"}},
                    {ActionWordType.Landing, new[] {"Tud"}}
                };
            }
        }

        /// <summary>
        /// Returns a random word string for a specific word type.
        /// </summary>
        public static string GetWordText(ActionWordType wordType)
        {
            InitializeWords();

            var words = _wordGroups[wordType];
            return words[GameInstance.Random.Next(0, words.Length)];
        }

        //one-time values:
        private readonly string _text = "";
        private readonly Color _color;

        private readonly float _targetSize;

        //changing:
        private float _size;
        private double _delay;
        private float _rotation = 0f;

        private readonly SpriteFont _grumpFont;

        public ActionWord(string text, Color color, float targetSize, Vector3 position)
        {
            _grumpFont = GameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFont");

            _text = text;
            _color = color;
            _targetSize = targetSize;
            this.Position = position;
        }

        public override void Draw()
        {
            var fontSize = _grumpFont.MeasureString(_text) * _size;

            GameInstance.SpriteBatch.DrawString(_grumpFont, _text, new Vector2(X - fontSize.X / 2f, Z - Y - fontSize.Y / 2f), _color, 0f, Vector2.Zero, _size, SpriteEffects.None, 0f);
        }

        public override Point GetDrawingSize()
        {
            var textSize = _grumpFont.MeasureString(_text) * _size;
            return textSize.ToPoint();
        }

        public override Vector3 GetFeetPosition()
        {
            var fontSize = _grumpFont.MeasureString(_text) * _size;
            return new Vector3(X + fontSize.X / 2f, Y, Z + fontSize.Y);
        }

        public override void Update()
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
                        CanBeRemoved = true;
                    }
                }
            }
            _rotation += 0.01f;
        }
    }
}