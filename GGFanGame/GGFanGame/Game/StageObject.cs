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
        private Vector3 _position;
        private Vector3 _size;
        private ObjectFacing _facing;
        private Color _objectColor = Color.Orange;
        private bool _collision;
        private int _health = 1; //1 is the default so every object has at least one health when spawned.
        private float _weigth = 0;
        private bool _canInteract = false;
        private bool _canBeRemoved = false;
        private bool _canLandOn = true; //TODO: set to false, just for testing, this is true for all.
        private bool _canCollideWith = true;
        private float _strength = 0;
        private GGGame _game;

        private static int _currentSortingPriority = 0; //Keeps track of all the sorting priorities added so that every object has a different one.
        private int _sortingPriority = 0;

        #region Properties

        /// <summary>
        /// The game instance to refer to.
        /// </summary>
        /// <returns></returns>
        protected GGGame gameInstance
        {
            get { return _game; }
        }

        public Color objectColor
        {
            get { return _objectColor; }
            protected set { _objectColor = value; }
        }

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

        /// <summary>
        /// The size of this object.
        /// </summary>
        /// <returns></returns>
        public Vector3 size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// The way this object is facing.
        /// </summary>
        /// <returns></returns>
        public ObjectFacing facing
        {
            get { return _facing; }
            set { _facing = value; }
        }

        /// <summary>
        /// If other objects can collide with this one.
        /// </summary>
        /// <returns></returns>
        public bool collision
        {
            get { return _collision; }
            set { _collision = value; }
        }

        /// <summary>
        /// The health of this object.
        /// </summary>
        /// <returns></returns>
        public int health
        {
            get { return _health; }
            set { _health = value; }
        }

        /// <summary>
        /// The strength of this object.
        /// </summary>
        /// <returns></returns>
        public float strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        /// <summary>
        /// The weigth of this object.
        /// </summary>
        /// <returns></returns>
        public float weigth
        {
            get { return _weigth; }
            set { _weigth = value; }
        }

        /// <summary>
        /// If anything can interact with this object.
        /// </summary>
        /// <returns></returns>
        public bool canInteract
        {
            get { return _canInteract; }
            protected set { _canInteract = value; }
        }

        /// <summary>
        /// A check to indicate wether this object can be removed from a stage.
        /// </summary>
        /// <returns></returns>
        public bool canBeRemoved
        {
            get { return _canBeRemoved; }
            set { _canBeRemoved = value; }
        }

        /// <summary>
        /// If an object can land on this object.
        /// </summary>
        /// <returns></returns>
        public bool canLandOn
        {
            get { return _canLandOn; }
            set { _canLandOn = value; }
        }

        #endregion

        public StageObject(GGGame game)
        {
            _game = game;

            _sortingPriority = _currentSortingPriority;
            _currentSortingPriority++;
        }

        /// <summary>
        /// The default bounding box of this object based on its position and size.
        /// </summary>
        /// <returns></returns>
        public virtual BoundingBox boundingBox
        {
            get { return new BoundingBox(new Vector3(_position.X - _size.X / 2f, _position.Y, _position.Z - _size.Z / 2),
                                         new Vector3(_position.X + _size.X / 2f, _position.Y + _size.Y, _position.Z + _size.Z / 2)); }
        }

        public abstract Point getDrawingSize();

        /// <summary>
        /// Update the object.
        /// </summary>
        public abstract void update();

        /// <summary>
        /// Draw the object.
        /// </summary>
        public abstract void draw();

        /// <summary>
        /// This object gets hit by an attack.
        /// </summary>
        public virtual void getHit(Attack attack)
        {
            //By default, nothing happens...
        }

        /// <summary>
        /// Returns the lower center of this object.
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 getFeetPosition()
        {
            return _position;
        }

        //Needed in order to sort the list of objects and arrange them in an order
        //so that the objects in the foreground are overlaying those in the background.
        public int CompareTo(StageObject obj)
        {
            if (Z < obj.Z)
            {
                return -1;
            }
            else if (Z > obj.Z)
            {
                return 1;
            }
            else
            {
                //When they are on the same Z plane, compare their sorting priority.
                //This is very unlikely to happen, but eh.
                if (_sortingPriority < obj._sortingPriority)
                {
                    return -1;
                }
                else if (_sortingPriority > obj._sortingPriority)
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
}