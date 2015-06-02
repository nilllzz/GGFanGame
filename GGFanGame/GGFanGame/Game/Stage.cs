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
        private HUD.PlayerStatus _oneStatus;

        public PlayerCharacter onePlayer
        {
            get { return _onePlayer; }
        }

        public Stage(GGGame game)
        {
            _gameInstance = game;

            _objects = new List<StageObject>();

            _onePlayer = new Arin(game, PlayerIndex.One);
            _oneStatus = new HUD.PlayerStatus(game, _onePlayer, PlayerIndex.One);

            _objects.Add(_onePlayer);
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
        }

        public void update()
        {
            _objects.Sort();

            for (int i = 0; i < _objects.Count; i++)
            {
                if (i <= _objects.Count - 1)
                {
                    _objects[i].update();
                }
            }
        }

        /// <summary>
        /// A single hit that targets all objects on the screen gets issued.
        /// </summary>
        public void singleHitAll(Attack attack, Vector3 relPosition)
        {
            System.Diagnostics.Debug.Print("ATTACK");

            int objIndex = 0;
            bool hit = false;

            var attackHitbox = attack.getHitbox(relPosition);

            while (objIndex < _objects.Count && !hit)
            {
                StageObject obj = _objects[objIndex];

                if (obj != attack.origin && obj.canInteract)
                {
                    if (attackHitbox.Intersects(obj.boundingBox))
                    {
                        hit = true;
                        obj.getHit(attack);


                        Vector3 wordPosition = obj.getFeetPosition();
                        wordPosition.Y += obj.size.Y * 2;

                        _objects.Add(new ActionWord(_gameInstance, ActionWord.getHurtWord(), Color.Orange, 1f, wordPosition));   
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