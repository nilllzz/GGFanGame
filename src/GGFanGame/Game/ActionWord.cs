﻿using System;
using System.Collections.Generic;
using GGFanGame.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Core;
using GameDevCommon.Drawing;
using GameDevCommon.Rendering.Composers;
using GameDevCommon.Rendering;

namespace GGFanGame.Game
{
    /// <summary>
    /// This class displays action words on the stage screen.
    /// </summary>
    internal sealed class ActionWord : StageObject
    {
        private static readonly Dictionary<ActionWordType, string[]> _wordGroups = new Dictionary<ActionWordType, string[]>
                {
                    {ActionWordType.HurtEnemy, new[] {"Uhh", "Ahh", "Arg", "Huu", "Ehh"}},
                    {ActionWordType.HurtPlayer, new[] {"Ech", "Shiet", "Damn", "Dammit", "Urg", "Ahh", "Garg"}},
                    {ActionWordType.Landing, new[] {"Tud"}}
                };

        private static readonly Random _wordRandomizer = new Random();
        
        /// <summary>
        /// Returns a random word string for a specific word type.
        /// </summary>
        private static string GetWordText(ActionWordType wordType)
        {
            var words = _wordGroups[wordType];
            return words[_wordRandomizer.Next(0, words.Length)];
        }

        // one-time values:
        private readonly string _text = "";
        private readonly Color _color;

        private readonly float _targetSize;

        // changing:
        private float _size;
        private double _delay;
        private float _rotation = 0f;

        private readonly SpriteFont _grumpFont;

        public ActionWord(ActionWordType wordType, Color color, float targetSize, Vector3 position)
            : this(GetWordText(wordType), color, targetSize, position)
        { }

        public ActionWord(string text, Color color, float targetSize, Vector3 position)
        {
            _grumpFont = GameInstance.Content.Load<SpriteFont>(Resources.Fonts.CartoonFont);

            _text = text;
            _color = color;
            _targetSize = targetSize;

            Position = position;
            IsOpaque = false;

            CreateTexture();
        }

        private void CreateTexture()
        {
            var textSize = _grumpFont.MeasureString(_text);
            var target = new RenderTarget2D(GameInstance.GraphicsDevice, (int)textSize.X, (int)textSize.Y);
            var batch = new SpriteBatch(GameInstance.GraphicsDevice);

            GameInstance.GraphicsDevice.SetRenderTarget(target);
            GameInstance.GraphicsDevice.Clear(Color.Transparent);

            batch.Begin(SpriteBatchUsage.Default);
            batch.DrawString(_grumpFont, _text, Vector2.Zero, _color);
            batch.End();

            GameInstance.GraphicsDevice.SetRenderTarget(null);

            Texture = target;

            batch.Dispose();
        }
        
        public override Vector3 GetFeetPosition()
        {
            var fontSize = _grumpFont.MeasureString(_text) * _size;
            return new Vector3(X + fontSize.X / 2f, Y, Z + fontSize.Y);
        }

        protected override void CreateGeometry()
        {
            var textSize = _grumpFont.MeasureString(_text);

            var vertices = RectangleComposer.Create(textSize.X, textSize.Y);
            VertexTransformer.Rotate(vertices, new Vector3(MathHelper.PiOver2, 0f, 0f));
            Geometry.AddVertices(vertices);
        }

        public override void Update()
        {
            if (_size < _targetSize)
            {
                var diff = _size - MathHelper.Lerp(_targetSize, _size, 0.8f);
                diff *= ParentStage.TimeDelta;

                _size -= diff;

                // +0.01 for inaccuracy
                if (_size + 0.01f >= _targetSize)
                {
                    _size = _targetSize;
                    _delay = 1d;
                }
            }
            else
            {
                if (_delay > 0d)
                {
                    _delay -= 0.05 * ParentStage.TimeDelta;
                    Y += 0.9f / 64f * ParentStage.TimeDelta;
                    Alpha = (float)_delay;

                    if (_delay <= 0d)
                    {
                        CanBeRemoved = true;
                    }
                }
            }
            _rotation += 0.01f * ParentStage.TimeDelta;

            World = Matrix.CreateScale(_size / 64f) * Matrix.CreateTranslation(Position);
        }
    }
}
