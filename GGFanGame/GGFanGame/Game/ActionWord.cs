using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game
{
    /// <summary>
    /// This class displays action words on the stage screen.
    /// </summary>
    sealed class ActionWord : Level.StageObject
    {
        private static readonly string[] hurtWords = new string[] { "Ouch!", "Shiet", "Ahh!", "Uhh!", "ECH" };
        private static Random wordRnd = new Random();

        public static string getHurtWord()
        {
            return hurtWords[wordRnd.Next(0, hurtWords.Length)];
        }

        //one-time values:
        private string _text = "";
        private Color _color;

        private float _targetSize;

        //changing:
        private float _size;
        private double _delay;
        private float _rotation = 0f;

        private bool visible = true;
        private SpriteFont _grumpFont; 

        public ActionWord(GGGame game, string text, Color color, float targetSize, Vector3 position) : base(game)
        {
            _grumpFont = gameInstance.Content.Load<SpriteFont>("CartoonFont");

            _text = text;
            _color = color;
            _targetSize = targetSize;
            this.position = position;
        }

        public override void draw()
        {
            if (visible)
            {
                Vector2 fontSize = _grumpFont.MeasureString(_text) * _size;

                gameInstance.spriteBatch.DrawString(_grumpFont, _text, new Vector2(X - fontSize.X / 2f, Z - Y - fontSize.Y / 2f), _color, 0f,Vector2.Zero, _size, SpriteEffects.None, 0f);
            }
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
                        visible = false;
                    }
                }
            }

            if (visible)
            {
                _rotation += 0.01f;
            }
        }
    }
}