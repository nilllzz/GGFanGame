using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame.Game.Level
{

    /// <summary>
    /// The states of an object.
    /// </summary>
    public enum ObjectState
    {
        Idle,
        Walking,
        Jumping,
        Falling,

        Blocking,

        Hurt,
        HurtFalling,
        OnBack,
        StandingUp,
        Dead,

        Attacking
    }


    /// <summary>
    /// The ways an object can face.
    /// </summary>
    enum ObjectFacing
    {
        Left,
        Right
    }

    /// <summary>
    /// The base object for all things that appear in a stage.
    /// </summary>
    abstract class StageObject : IComparable<StageObject>
    {
        private GGGame _game; 
        /// <summary>
        /// The game instance to refer to.
        /// </summary>
        /// <returns></returns>
        protected GGGame gameInstance
        {
            get { return _game; }
        }

        private Vector3 _position;
        /// <summary>
        /// The absolute position of this object in the level.
        /// </summary>
        /// <returns></returns>
        public Vector3 position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// The X position.
        /// </summary>
        /// <returns></returns>
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        /// <summary>
        /// The Y position.
        /// </summary>
        /// <returns></returns>
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        /// <summary>
        /// The Z position.
        /// </summary>
        /// <returns></returns>
        public float Z
        {
            get { return _position.Z; }
            set { _position.Z = value; }
        }

        private Vector3 _size; 
        /// <summary>
        /// The size of this object.
        /// </summary>
        /// <returns></returns>
        public Vector3 size
        {
            get { return _size; }
            set { _size = value; }
        }

        public virtual Vector3 getFeetPosition()
        {
            return _position;
        }

        private ObjectFacing _facing; 
        /// <summary>
        /// The way this object is facing.
        /// </summary>
        /// <returns></returns>
        public ObjectFacing facing
        {
            get { return _facing; }
            set { _facing = value; }
        }

        private bool _collision;
        /// <summary>
        /// If other objects can collide with this one.
        /// </summary>
        /// <returns></returns>
        public bool collision
        {
            get { return _collision; }
            set { _collision = value; }
        }

        private int _health = 1; //1 is the default so every object has at least one health when spawned.
        /// <summary>
        /// The health of this object.
        /// </summary>
        /// <returns></returns>
        public int health
        {
            get { return _health; }
            set { _health = value; }
        }

        private float _strength = 0;
        /// <summary>
        /// The strength of this object.
        /// </summary>
        /// <returns></returns>
        public float strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        private float _weigth = 0;
        /// <summary>
        /// The weigth of this object.
        /// </summary>
        /// <returns></returns>
        public float weigth
        {
            get { return _weigth; }
            set { _weigth = value; }
        }

        public StageObject(GGGame game)
        {
            _game = game;
        }

        /// <summary>
        /// The default bounding box of this object based on its position and size.
        /// </summary>
        /// <returns></returns>
        public virtual BoundingBox boundingBox
        {
            get { return new BoundingBox(new Vector3(_position.X - _size.X / 2, _position.Y, _position.Z - _size.Z / 2),
                                         new Vector3(_position.X + _size.X / 2, _position.Y + _size.Y, _position.Z + _size.Z / 2)); }
        }

        public abstract void update();

        public abstract void draw();

        public virtual void getHit(Attack attack)
        {
            //By default, nothing happens...
        }

        private bool _canInteract = false;

        public bool canInteract
        {
            get { return _canInteract; }
            protected set { _canInteract = value; }
        }

        public int CompareTo(StageObject obj)
        {
            Vector3 ownFeet = getFeetPosition();
            Vector3 objFeet = obj.getFeetPosition();

            if (ownFeet.Z < objFeet.Z)
            {
                return -1;
            }
            else if (ownFeet.Z > objFeet.Z)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}