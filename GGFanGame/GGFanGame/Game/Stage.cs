using System;
using System.Collections.Generic;
using GGFanGame.Content;
using GGFanGame.DataModel.Game;
using GGFanGame.Game.Playable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static GameProvider;

namespace GGFanGame.Game
{
    /// <summary>
    /// Represents a level in the game.
    /// </summary>
    internal class Stage
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
        private List<StageObject> _objects;
        private readonly float _yDefaultKillPlane = -0f;

        internal ContentManager Content { get; }

        /// <summary>
        /// The ambient shadow color in this stage.
        /// </summary>
        public Color AmbientColor { get; private set; } = new Color(0, 0, 0, 100); //Used for shadow color

        /// <summary>
        /// The camera of the level.
        /// </summary>
        public StageCamera Camera { get; }

        public string Name => _dataModel.Name;
        public string WorldId => _dataModel.WorldId;
        public string StageId => _dataModel.StageId;
        public Color BackColor => _dataModel.BackColor.ToColor();

        public PlayerCharacter OnePlayer { get; set; }
        public PlayerCharacter TwoPlayer { get; set; }
        public PlayerCharacter ThreePlayer { get; set; }
        public PlayerCharacter FourPlayer { get; set; }

        /// <summary>
        /// Creates a new instance of the Stage class.
        /// </summary>
        public Stage(ContentManager content, IEnumerable<StageObject> objects, StageModel dataModel)
        {
            this.Content = content;

            _dataModel = dataModel;
            _objects = new List<StageObject>(objects);

            Camera = new StageCamera();
        }

        public void Load()
        {
            SetActiveStage();

            OnePlayer = new Arin(PlayerIndex.One) { X = 320, Z = 200 };
            TwoPlayer = new Arin(PlayerIndex.Two) { X = 320, Z = 230 };
            ThreePlayer = new Arin(PlayerIndex.Three) { X = 50, Z = 230 };
            FourPlayer = new Arin(PlayerIndex.Four) { X = 50, Z = 200 };

            _objects.Add(OnePlayer);
            _objects.Add(TwoPlayer);
            _objects.Add(ThreePlayer);
            _objects.Add(FourPlayer);

            _objects.ForEach(o => o.Load());
        }

        /// <summary>
        /// Renders the objects in this stage.
        /// </summary>
        public void Draw()
        {
            foreach (var obj in _objects)
            {
                obj.Draw();
            }
            
            //TEST: Object counter.
            GameInstance.SpriteBatch.DrawString(Content.Load<SpriteFont>(Resources.Fonts.CartoonFontSmall), _objects.Count.ToString(), Vector2.Zero, Color.White);
        }

        /// <summary>
        /// Returns the list of objects in this stage.
        /// </summary>
        public StageObject[] GetObjects()
        {
            return _objects.ToArray();
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

            Camera.Update(this);
        }

        /// <summary>
        /// Adds an object the stage's object list.
        /// </summary>
        public void AddObject(StageObject obj)
        {
            obj.Load();
            _objects.Add(obj);
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
                        wordPosition.Y += (float)(obj.Size.Y / 2d * Camera.Scale);

                        _objects.Add(new ActionWord(ActionWord.GetWordText(ActionWordType.HurtEnemy), obj.ObjectColor, 1f, wordPosition));
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

            foreach (var obj in _objects)
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
        /// Returns the altitute of the ground for a specific position.
        /// </summary>
        public float GetGround(Vector3 position)
            => GetSupporting(position).objY;

        /// <summary>
        /// Returns the supporting object and its Y height for a position.
        /// </summary>
        /// <param name="position">The position to check the supporting object for.</param>
        public (StageObject supportingObject, float objY) GetSupporting(Vector3 position)
        {
            var returnY = _yDefaultKillPlane;
            StageObject returnObj = null;

            if (position.Y > 0f)
            {
                var twoDimPoint = new Vector2(position.X, position.Z);

                foreach (var obj in _objects)
                {
                    if (obj.CanLandOn)
                    {
                        var boxes = obj.BoundingBoxes;

                        //When the object does not have defined bounding boxes, take the default bounding box.
                        if (boxes.Length == 0)
                            boxes = new BoundingBox[] { obj.BoundingBox };

                        foreach (var box in boxes)
                        {
                            var topY = box.Max.Y;
                            var twoDimPlane = new Rectangle((int)box.Min.X, (int)box.Min.Z, (int)(box.Max.X - box.Min.X), (int)(box.Max.Z - box.Min.Z));

                            if (topY <= position.Y && topY > returnY)
                            {
                                if (twoDimPlane.Contains(twoDimPoint))
                                {
                                    returnY = topY;
                                    returnObj = obj;
                                }
                            }
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
            //Create bounding box out of the desired position to check for collision with other bounding boxes.
            //We add 0.1 to the Y position so we don't get stuck in the floor.
            //The destination box is basically a line from the feet of the object with the height of its own height.
            //This way, it is consistent with the way we check for ground.
            var destinationBox = new BoundingBox(desiredPosition + new Vector3(0, 0.1f, 0), desiredPosition + new Vector3(0, 0.1f + (chkObj.BoundingBox.Max.Y - chkObj.BoundingBox.Min.Y), 0));

            foreach (var obj in _objects)
            {
                if (obj != chkObj && obj.Collision)
                {
                    var boxes = obj.BoundingBoxes;

                    //When the object does not have defined bounding boxes, take the default bounding box.
                    if (boxes.Length == 0)
                        boxes = new BoundingBox[] { obj.BoundingBox };

                    foreach (var box in boxes)
                    {
                        if (box.Intersects(destinationBox))
                        {
                            return obj;
                        }
                    }
                }
            }

            return null;
        }
    }
}