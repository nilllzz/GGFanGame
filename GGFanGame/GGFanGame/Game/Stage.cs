using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GGFanGame.Game.Level.Playable;

namespace GGFanGame.Game.Level
{
    class Stage
    {
        private static Stage _activeStage = null;

        public static Stage activeStage()
        {
            return _activeStage;
        }

        public void setActiveStage()
        {
            _activeStage = this;
        }

        private GGGame _gameInstance;
        private List<StageObject> _objects;

        private PlayerCharacter _onePlayer;
        private PlayerCharacter _twoPlayer;
        private PlayerCharacter _threePlayer;
        private PlayerCharacter _fourPlayer;

        private HUD.PlayerStatus _oneStatus;
        private HUD.PlayerStatus _twoStatus;
        private HUD.PlayerStatus _threeStatus;
        private HUD.PlayerStatus _fourStatus;

        public PlayerCharacter onePlayer
        {
            get { return _onePlayer; }
        }

        public Stage(GGGame game)
        {
            _gameInstance = game;

            _objects = new List<StageObject>();

            _onePlayer = new Arin(game, PlayerIndex.One);
            _twoPlayer = new Arin(game, PlayerIndex.Two) { X = 300, Z = 300 };
            _threePlayer = new Arin(game, PlayerIndex.Three) { X = 500, Z = 400 };
            _fourPlayer = new Arin(game, PlayerIndex.Four) { X = 900, Z = 100 };

            _objects.Add(_onePlayer);
            _objects.Add(_twoPlayer);
            _objects.Add(_threePlayer);
            _objects.Add(_fourPlayer);

            _oneStatus = new HUD.PlayerStatus(game, _onePlayer, PlayerIndex.One);
            _twoStatus = new HUD.PlayerStatus(game, _twoPlayer, PlayerIndex.Two);
            _threeStatus = new HUD.PlayerStatus(game, _threePlayer, PlayerIndex.Three);
            _fourStatus = new HUD.PlayerStatus(game, _fourPlayer, PlayerIndex.Four);

            _objects.Add(new Enemies.Dummy(game) { X = 100, Z = 100 });
            _objects.Add(new Enemies.Dummy(game) { X = 300, Z = 200 });
            _objects.Add(new Enemies.Dummy(game) { X = 500, Z = 150 });
            _objects.Add(new Enemies.Dummy(game) { X = 800, Z = 400 });
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
                        wordPosition.Y += obj.size.Y;

                        _objects.Add(new ActionWord(_gameInstance, ActionWord.getWordText(ActionWord.WordType.HurtEnemy), obj.objectColor, 1f, wordPosition));
                    }
                }

                objIndex++;
            }
        }

        public void addActionWord(ActionWord word)
        {
            _objects.Add(word);
        }
    }
}