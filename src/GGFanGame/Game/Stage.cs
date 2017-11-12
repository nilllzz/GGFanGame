using GameDevCommon.Drawing;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using GGFanGame.Drawing;
using GGFanGame.Game.Playable;
using GGFanGame.Screens.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core;

namespace GGFanGame.Game
{
    /// <summary>
    /// Represents a level in the game.
    /// </summary>
    internal class Stage : IDisposable
    {
        #region ActiveStage

        /// <summary>
        /// The currently active stage.
        /// </summary>
        public static Stage ActiveStage { get; private set; }

        /// <summary>
        /// Sets the active stage to this instance.
        /// </summary>
        public void SetActiveStage()
        {
            ActiveStage = this;
        }

        #endregion

        private StageModel _dataModel;
        private readonly float _yDefaultKillPlane = float.MinValue;
        private StageShader _shader;
        private StageObjectCollection _objects;

        public IEnumerable<StageObject> Objects => _objects;
        /// <summary>
        /// How fast the time in the game moves. 1 is default, 0 is not moving.
        /// </summary>
        internal float TimeDelta { get; set; } = 1f;
        internal ContentManager Content { get; private set; }
        internal Random Random { get; private set; } = new Random();

        public string Name => _dataModel.Name;
        public string WorldId => _dataModel.WorldId;
        public string StageId => _dataModel.StageId;
        public Color BackColor => _dataModel.BackColor.ToColor();

        public PlayerCharacter OnePlayer { get; set; }
        public PlayerCharacter TwoPlayer { get; set; }
        public PlayerCharacter ThreePlayer { get; set; }
        public PlayerCharacter FourPlayer { get; set; }

        internal bool IsDisposed { get; private set; }

        /// <summary>
        /// Creates a new instance of the Stage class.
        /// </summary>
        public Stage(ContentManager content, IEnumerable<StageObject> objects, StageModel dataModel)
        {
            Content = content;

            _dataModel = dataModel;
            _objects = new StageObjectCollection(objects.ToArray());
        }

        public void LoadContent()
        {
            SetActiveStage();

            _shader = new StageShader(Content);
            _shader.SetDirectionalLight(_dataModel.Light.Config);
            _shader.AddPointLight(new PointLight(position: new Vector3(-3, 2, -4), color: Color.White, radius: 3, intensity: 2));

            OnePlayer = new Arin(PlayerIndex.One) { X = -4, Y = 0.1f, Z = -4 };
            //TwoPlayer = new Arin(PlayerIndex.Two) { X = 0, Y = 0.1f, Z = 0 };
            //ThreePlayer = new Arin(PlayerIndex.Three) { X = 50, Z = 230 };
            //FourPlayer = new Arin(PlayerIndex.Four) { X = 50, Z = 200 };

            AddObject(OnePlayer);
            //AddObject(TwoPlayer);
            //_objects.Add(ThreePlayer);
            //_objects.Add(FourPlayer);

            _objects.ForEach(o =>
            {
                o.ParentStage = this;
                o.LoadContent();
            });
        }

        /// <summary>
        /// Renders the objects in this stage.
        /// </summary>
        public void Render(StageCamera camera)
        {
            GameInstance.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, BackColor, 1.0f, 0);
            GameInstance.GraphicsDevice.ResetFull();

            _shader.Prepare(camera);

            GameInstance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (var obj in _objects.OpaqueObjects)
                _shader.Render(obj);

            foreach (var obj in _objects.TransparentObjects)
                _shader.Render(obj);
        }

        /// <summary>
        /// Draws the objects in this stage.
        /// </summary>
        public void Draw(SpriteBatch batch)
        {
            //TEST: Object counter.
            batch.DrawString(Content.Load<SpriteFont>(Resources.Fonts.CartoonFontSmall), _objects.Count.ToString(), Vector2.Zero, Color.White);
        }

        /// <summary>
        /// Updates and sorts the objects in this stage.
        /// </summary>
        public void Update()
        {
            _objects.Sort();

            for (var i = 0; i < _objects.Count; i++)
            {
                if (i <= _objects.Count - 1)
                {
                    if (_objects[i].CanBeRemoved)
                    {
                        _objects.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        _objects[i].Update();
                    }
                }
            }
        }

        /// <summary>
        /// Adds an object the stage's object list.
        /// </summary>
        public void AddObject(StageObject obj)
        {
            obj.ParentStage = this;
            obj.LoadContent();
            _objects.Add(obj);
        }

        internal void AddPointLight(PointLight light)
        {
            _shader.AddPointLight(light);
        }

        /// <summary>
        /// A single hit that targets all objects on the screen gets issued.
        /// </summary>
        /// <param name="maxHitCount">The maximum amount of objects this attack can hit.</param>
        /// <returns>This returns the amount of objects the attack hit.</returns>
        public int ApplyAttack(Attack attack, Vector3 relPosition, int maxHitCount)
        {
            var objIndex = 0;
            var hitCount = 0;

            var attackHitbox = attack.GetHitbox(relPosition);

            while (objIndex < _objects.Count && hitCount < maxHitCount)
            {
                var obj = _objects[objIndex];

                if (obj != attack.Origin && obj.CanInteract)
                {
                    if (attackHitbox.Intersects(obj.BoundingBox))
                    {
                        hitCount++;
                        obj.GetHit(attack);

                        var wordPosition = obj.GetFeetPosition();
                        wordPosition.Y += (float)(obj.Size.Y / 2d);

                        AddObject(new ActionWord(ActionWordType.HurtEnemy, obj.ObjectColor, 1f, wordPosition));
                    }
                }

                objIndex++;
            }

            return hitCount;
        }

        const bool TYPE1_EXPLOSION = false; //For testing, to easily switch methods of calculating explosion effects.

        /// <summary>
        /// Applies an explosion effect to the objects in the level.
        /// </summary>
        /// <param name="origin">The object where the explosion effect originated from.</param>
        /// <param name="center">The center of the explosion.</param>
        /// <param name="radius">The radius of the explosion.</param>
        /// <param name="health">The health to take away from hit targets.</param>
        /// <param name="strength">The strength of the explosion.</param>
        public void ApplyExplosion(StageObject origin, Vector3 center, float radius, int health, float strength)
        {
            //For explosions, we are using a sphere to detect collision because why not.
            var explosionSphere = new BoundingSphere(center, radius);

            foreach (var obj in Objects)
            {
                if (obj != origin && obj.CanInteract)
                {
                    if (explosionSphere.Contains(obj.Position) != ContainmentType.Disjoint)
                    {
                        if (TYPE1_EXPLOSION)
                        {
                            //TODO: refine calculations
                            strength = Vector3.Distance(center, obj.Position);

                            var xAffection = radius - Math.Abs(center.X - obj.X);
                            var zAffection = radius - Math.Abs(center.Z - obj.Z);

                            if (center.X > obj.X)
                            {
                                xAffection *= -1f;
                            }
                            if (center.Z > obj.Z)
                            {
                                zAffection *= -1f;
                            }

                            obj.GetHit(origin, new Vector3(xAffection / 10f, 5, zAffection / 25f), health, true);
                        }
                        else
                        {
                            var xAffection = strength;
                            if (center.X > obj.X)
                            {
                                xAffection *= -1f;
                            }

                            obj.GetHit(origin, new Vector3(xAffection * 1.3f, strength, 0f), health, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the supporting object and its Y height for a position.
        /// </summary>
        /// <param name="position">The position to check the supporting object for.</param>
        public (StageObject supportingObject, float objY) GetSupporting(StageObject chkObj)
        {
            var returnY = _yDefaultKillPlane;
            StageObject returnObj = null;
            var chkBox = chkObj.BoundingBox;
            var minY = chkBox.Min.Y;

            foreach (var obj in Objects)
            {
                if (obj.CanLandOn)
                {
                    var box = obj.BoundingBox;
                    var topY = box.Max.Y;

                    if (topY <= minY && topY > returnY)
                    {
                        if (chkBox.Min.X < box.Max.X &&
                            box.Min.X < chkBox.Max.X &&
                            chkBox.Min.Z < box.Max.Z &&
                            box.Min.Z < chkBox.Max.Z)
                        {
                            returnY = topY;
                            returnObj = obj;
                        }
                    }
                }
            }

            //The first item is the actual object, the second item the Y position:
            //The object is null when there's no object to stand on.
            return (returnObj, returnY);
        }

        /// <summary>
        /// Checks if an object intersects with something in the stage.
        /// </summary>
        public bool Intersects(StageObject chkObj, Vector3 desiredPosition)
        {
            return
                CheckCollision(chkObj, desiredPosition) != null;
        }

        /// <summary>
        /// Checks if a desired position for an object collides with space occupied by another object.
        /// </summary>
        public StageObject CheckCollision(StageObject chkObj, Vector3 desiredPosition)
        {
            var destinationBox = chkObj.BoundingBox.Offset(desiredPosition - chkObj.Position + new Vector3(0, 0.1f, 0));
            foreach (var obj in Objects)
            {
                if (obj != chkObj && obj.Collision)
                {
                    var box = obj.BoundingBox;

                    if (box.Intersects(destinationBox))
                    {
                        return obj;
                    }
                }
            }

            return null;
        }

        internal IEnumerable<PlayerCharacter> GetPlayers()
        {
            var players = new List<PlayerCharacter>();
            if (OnePlayer != null)
                players.Add(OnePlayer);
            if (TwoPlayer != null)
                players.Add(TwoPlayer);
            if (ThreePlayer != null)
                players.Add(ThreePlayer);
            if (FourPlayer != null)
                players.Add(FourPlayer);
            return players;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~Stage()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_shader != null && !_shader.IsDisposed) _shader.Dispose();
                    if (_objects != null)
                    {
                        foreach (var obj in _objects)
                        {
                            if (obj != null && !obj.IsDisposed)
                                obj.Dispose();
                        }
                    }
                }

                _objects = null;
                Content = null;
                Random = null;
                _dataModel = null;
                _shader = null;
                IsDisposed = true;
            }
        }
    }
}
