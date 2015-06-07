﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GGFanGame.Game.Level.Playable;
using GGFanGame.Game.Level.HUD;

namespace GGFanGame.Game.Level
{
    /// <summary>
    /// Represents a level in the game.
    /// </summary>
    class Stage
    {
        //We store a single stage as active so that objects can easily access the current stage.
        private static Stage _activeStage = null;

        /// <summary>
        /// The currently active stage.
        /// </summary>
        /// <returns></returns>
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

        private GGGame _gameInstance;
        private List<StageObject> _objects;
        private double _scale = 2d;
        private Color _ambientColor = new Color(0, 0, 0, 100); //Used for shadow color

        /// <summary>
        /// The scale of objects in this stage.
        /// </summary>
        /// <returns></returns>
        public double scale
        {
            get { return _scale; }
        }

        /// <summary>
        /// The ambient shadow color in this stage.
        /// </summary>
        /// <returns></returns>
        public Color ambientColor
        {
            get { return _ambientColor; }
        }

        private PlayerCharacter _onePlayer;
        private PlayerCharacter _twoPlayer;
        private PlayerCharacter _threePlayer;
        private PlayerCharacter _fourPlayer;

        private PlayerStatus _oneStatus;
        private PlayerStatus _twoStatus;
        private PlayerStatus _threeStatus;
        private PlayerStatus _fourStatus;

        public PlayerCharacter onePlayer
        {
            get { return _onePlayer; }
        }

        public Stage(GGGame game)
        {
            _gameInstance = game;

            _objects = new List<StageObject>();

            _onePlayer = new Arin(game, PlayerIndex.One) { X = 35, Z = 115 };
            _twoPlayer = new Arin(game, PlayerIndex.Two) { X = 320, Z = 280 };
            _threePlayer = new Arin(game, PlayerIndex.Three) { X = 500, Z = 330 };
            _fourPlayer = new Arin(game, PlayerIndex.Four) { X = 900, Z = 100 };

            _objects.Add(_onePlayer);
            _objects.Add(_twoPlayer);
            _objects.Add(_threePlayer);
            _objects.Add(_fourPlayer);

            _oneStatus = new PlayerStatus(game, _onePlayer, PlayerIndex.One);
            _twoStatus = new PlayerStatus(game, _twoPlayer, PlayerIndex.Two);
            _threeStatus = new PlayerStatus(game, _threePlayer, PlayerIndex.Three);
            _fourStatus = new PlayerStatus(game, _fourPlayer, PlayerIndex.Four);

            _objects.Add(new Enemies.Booper(game) { X = 100, Z = 100 });
            _objects.Add(new Scene.Couch(game) { X = 300, Z = 300 });
            _objects.Add(new Enemies.Booper(game) { X = 300, Z = 200 });
            _objects.Add(new Enemies.Booper(game) { X = 500, Z = 150 });
            _objects.Add(new Enemies.Booper(game) { X = 800, Z = 400 });
        }

        public void draw()
        {
            foreach (StageObject obj in _objects)
            {
                obj.draw();
            }

            _oneStatus.draw();
            _twoStatus.draw();
            _threeStatus.draw();
            _fourStatus.draw();
        }

        public StageObject[] getObjects()
        {
            return _objects.ToArray();
        }

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
        }

        /// <summary>
        /// A single hit that targets all objects on the screen gets issued.
        /// </summary>
        /// <param name="maxHitCount">The maximum amount of objects this attack can hit.</param>
        public void applyAttack(Attack attack, Vector3 relPosition, int maxHitCount)
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
                        wordPosition.Y += (float)(obj.size.Y / 2d * _scale);

                        _objects.Add(new ActionWord(_gameInstance, ActionWord.getWordText(ActionWord.WordType.HurtEnemy), obj.objectColor, 1f, wordPosition));
                    }
                }

                objIndex++;
            }
        }

        public void addObject(StageObject obj)
        {
            _objects.Add(obj);
        }

        /// <summary>
        /// Returns the altitute of the ground for a specific position.
        /// </summary>
        /// <returns></returns>
        public float getGround(Vector3 position)
        {
            float returnY = 0f;

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
                            }
                        }
                    }
                }
            }

            return returnY;
        }

        /// <summary>
        /// Checks if a desired position for an object collides with space occupied by another object.
        /// </summary>
        /// <param name="chkObj"></param>
        /// <param name="desiredPosition"></param>
        /// <returns></returns>
        public bool checkCollision(StageObject chkObj, Vector3 desiredPosition)
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
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}