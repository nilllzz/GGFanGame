using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GGFanGame.Game.Playable;
using GGFanGame.Game.HUD;
using static GameProvider;

namespace GGFanGame.Game
{
    /// <summary>
    /// Represents a level in the game.
    /// </summary>
    class Stage
    {
        #region ActiveStage

        //We store a single stage as active so that objects can easily access the current stage.
        private static Stage _activeStage = null;

        /// <summary>
        /// The currently active stage.
        /// </summary>
        public static Stage activeStage()
        {
            return _activeStage;
        }

        /// <summary>
        /// Sets the active stage to this instance.
        /// </summary>
        public void setActiveStage()
        {
            _activeStage = this;
        }

        #endregion
        
        private List<StageObject> _objects;
        private Color _ambientColor = new Color(0, 0, 0, 100); //Used for shadow color
        private float _yDefaultKillPlane = -0f;

        /// <summary>
        /// The ambient shadow color in this stage.
        /// </summary>
        public Color ambientColor
        {
            get { return _ambientColor; }
        }

        /// <summary>
        /// The camera of the level.
        /// </summary>
        public StageCamera camera { get; private set; }
        
        public PlayerCharacter onePlayer { get; set; }
        public PlayerCharacter twoPlayer { get; set; }
        public PlayerCharacter threePlayer { get; set; }
        public PlayerCharacter fourPlayer { get; set; }

        /// <summary>
        /// Creates a new instance of the Stage class.
        /// </summary>
        public Stage()
        {
            camera = new StageCamera();

            _objects = new List<StageObject>();

            onePlayer = new Arin(PlayerIndex.One) { X = 320, Z = 200 };
            twoPlayer = new Arin(PlayerIndex.Two) { X = 320, Z = 230 };
            threePlayer = new Arin(PlayerIndex.Three) { X = 50, Z = 230 };
            fourPlayer = new Arin(PlayerIndex.Four) { X = 50, Z = 200 };

            _objects.Add(onePlayer);
            _objects.Add(twoPlayer);
            _objects.Add(threePlayer);
            _objects.Add(fourPlayer);

            _objects.Add(new Scene.GrumpSpace.Couch() { X = 110, Y = 0, Z = 320 });
            _objects.Add(new Scene.GrumpSpace.ArcadeMachine(Scene.GrumpSpace.ArcadeType.Ninja) { X = 310, Y = 0, Z = 320 });

            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 64, Y = 0, Z = 158 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 64, Y = 0, Z = 190 });
            _objects.Add(new Scene.Level1_1.Street() { X = 64, Y = 0, Z = 190 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 128, Y = 10, Z = 158 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 128, Y = 10, Z = 190 });
            _objects.Add(new Scene.Level1_1.Street() { X = 128, Y = 10, Z = 190 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 192, Y = 20, Z = 158 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 192, Y = 20, Z = 190 });
            _objects.Add(new Scene.Level1_1.Street() { X = 192, Y = 20, Z = 190 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 256, Y = 30, Z = 158 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 256, Y = 30, Z = 190 });
            _objects.Add(new Scene.Level1_1.Street() { X = 256, Y = 30, Z = 190 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 320, Y = 40, Z = 158 });
            _objects.Add(new Scene.Level1_1.BridgeRailing() { X = 320, Y = 40, Z = 190 });
            _objects.Add(new Scene.Level1_1.Street() { X = 320, Y = 40, Z = 190 });
        }

        /// <summary>
        /// Renders the objects in this stage.
        /// </summary>
        public void draw()
        {
            foreach (StageObject obj in _objects)
            {
                obj.draw();
            }
            
            //TEST: Object counter.
            gameInstance.spriteBatch.DrawString(gameInstance.Content.Load<SpriteFont>(@"Fonts\CartoonFontSmall"), _objects.Count.ToString(), Vector2.Zero, Color.White);
        }

        /// <summary>
        /// Returns the list of objects in this stage.
        /// </summary>
        public StageObject[] getObjects()
        {
            return _objects.ToArray();
        }
        
        /// <summary>
        /// Updates and sorts the objects in this stage.
        /// </summary>
        public void update()
        {
            _objects.Sort();
            
            for (int i = 0; i < _objects.Count; i++)
            {
                if (i <= _objects.Count - 1)
                {
                    if (_objects[i].canBeRemoved)
                    {
                        _objects.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        _objects[i].update();
                    }
                }
            }

            camera.update(this);
        }

        /// <summary>
        /// Adds an object the stage's object list.
        /// </summary>
        public void addObject(StageObject obj)
        {
            _objects.Add(obj);
        }

        /// <summary>
        /// A single hit that targets all objects on the screen gets issued.
        /// </summary>
        /// <param name="maxHitCount">The maximum amount of objects this attack can hit.</param>
        /// <returns>This returns the amount of objects the attack hit.</returns>
        public int applyAttack(Attack attack, Vector3 relPosition, int maxHitCount)
        {
            int objIndex = 0;
            int hitCount = 0;

            var attackHitbox = attack.getHitbox(relPosition);

            while (objIndex < _objects.Count && hitCount < maxHitCount)
            {
                StageObject obj = _objects[objIndex];

                if (obj != attack.origin && obj.canInteract)
                {
                    if (attackHitbox.Intersects(obj.boundingBox))
                    {
                        hitCount++;
                        obj.getHit(attack);

                        Vector3 wordPosition = obj.getFeetPosition();
                        wordPosition.Y += (float)(obj.size.Y / 2d * camera.scale);

                        _objects.Add(new ActionWord(ActionWord.getWordText(ActionWordType.HurtEnemy), obj.objectColor, 1f, wordPosition));
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
        public void applyExplosion(StageObject origin, Vector3 center, float radius, int health, float strength)
        {
            //For explosions, we are using a sphere to detect collision because why not.
            var explosionSphere = new BoundingSphere(center, radius);

            foreach (StageObject obj in _objects)
            {
                if (obj != origin && obj.canInteract)
                {
                    if (explosionSphere.Contains(obj.position) != ContainmentType.Disjoint)
                    {
                        if (TYPE1_EXPLOSION)
                        {
                            //TODO: refine calculations
                            strength = Vector3.Distance(center, obj.position);

                            float xAffection = radius - Math.Abs(center.X - obj.X);
                            float zAffection = radius - Math.Abs(center.Z - obj.Z);

                            if (center.X > obj.X)
                            {
                                xAffection *= -1f;
                            }
                            if (center.Z > obj.Z)
                            {
                                zAffection *= -1f;
                            }

                            obj.getHit(new Vector3(xAffection / 10f, 5, zAffection / 25f), health, true);
                        }
                        else
                        {
                            float xAffection = strength;
                            if (center.X > obj.X)
                            {
                                xAffection *= -1f;
                            }

                            obj.getHit(new Vector3(xAffection * 1.3f, strength, 0f), health, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the altitute of the ground for a specific position.
        /// </summary>
        public float getGround(Vector3 position)
        {
            var supporting = getSupporting(position);
            return supporting.Item2;
        }

        /// <summary>
        /// Returns the supporting object and its Y height for a position.
        /// </summary>
        /// <param name="position">The position to check the supporting object for.</param>
        public Tuple<StageObject, float> getSupporting(Vector3 position)
        {
            float returnY = _yDefaultKillPlane;
            StageObject returnObj = null;

            if (position.Y > 0f)
            {
                Vector2 twoDimPoint = new Vector2(position.X, position.Z);

                foreach (StageObject obj in _objects)
                {
                    if (obj.canLandOn)
                    {
                        BoundingBox[] boxes = obj.boundingBoxes;

                        //When the object does not have defined bounding boxes, take the default bounding box.
                        if (boxes.Length == 0)
                            boxes = new BoundingBox[] { obj.boundingBox };

                        foreach (BoundingBox box in boxes)
                        {
                            float topY = box.Max.Y;
                            Rectangle twoDimPlane = new Rectangle((int)box.Min.X, (int)box.Min.Z, (int)(box.Max.X - box.Min.X), (int)(box.Max.Z - box.Min.Z));

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
            return new Tuple<StageObject, float>(returnObj, returnY);
        }

        /// <summary>
        /// Checks if an object intersects with something in the stage.
        /// </summary>
        public bool intersects(StageObject chkObj, Vector3 desiredPosition)
        {
            return
                checkCollision(chkObj, desiredPosition) != null;
        }

        /// <summary>
        /// Checks if a desired position for an object collides with space occupied by another object.
        /// </summary>
        public StageObject checkCollision(StageObject chkObj, Vector3 desiredPosition)
        {
            //Create bounding box out of the desired position to check for collision with other bounding boxes.
            //We add 0.1 to the Y position so we don't get stuck in the floor.
            //The destination box is basically a line from the feet of the object with the height of its own height.
            //This way, it is consistent with the way we check for ground.
            BoundingBox destinationBox = new BoundingBox(desiredPosition + new Vector3(0, 0.1f, 0), desiredPosition + new Vector3(0, 0.1f + (chkObj.boundingBox.Max.Y - chkObj.boundingBox.Min.Y), 0));

            foreach (StageObject obj in _objects)
            {
                if (obj != chkObj && obj.collision)
                {
                    BoundingBox[] boxes = obj.boundingBoxes;

                    //When the object does not have defined bounding boxes, take the default bounding box.
                    if (boxes.Length == 0)
                        boxes = new BoundingBox[] { obj.boundingBox };

                    foreach (BoundingBox box in boxes)
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