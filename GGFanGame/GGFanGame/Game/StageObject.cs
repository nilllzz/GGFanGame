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

        Attacking,
        JumpAttacking,
        Dashing
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
        /// <summary>
        /// An event that occures when the position of the object changed.
        /// </summary>
        public event OnPositionChangedEventHandler OnPositionChanged;
        /// <summary>
        /// The event handler for the position changed event.
        /// </summary>
        /// <param name="previousPosition">The position before the event occured.</param>
        public delegate void OnPositionChangedEventHandler(StageObject obj, Vector3 previousPosition);

        private GGGame _game;
        private Vector3 _position;
        private Vector3 _size;
        private ObjectFacing _facing;
        private Color _objectColor = Color.Orange;
        private bool _collision;
        private int _health = 1; //1 is the default so every object has at least one health when spawned.
        private float _weight = 0;
        private bool _canInteract = false;
        private bool _canBeRemoved = false;
        private bool _canLandOn = false;
        private float _strength = 0;
        private List<BoundingBox> _boundingBoxes = new List<BoundingBox>();

        private static int _currentSortingPriority = 0; //Keeps track of all the sorting priorities added so that every object has a different one.
        private int _sortingPriority = 0;
        private bool _sortLowest = false;

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
            set
            {
                Vector3 prePosition = _position;
                _position = value;

                if (OnPositionChanged != null)
                    OnPositionChanged(this, prePosition);
            }
        }

        /// <summary>
        /// The X position.
        /// </summary>
        /// <returns></returns>
        public float X
        {
            get { return _position.X; }
            set
            {
                Vector3 prePosition = _position;
                _position.X = value;

                if (OnPositionChanged != null)
                    OnPositionChanged(this, prePosition);
            }
        }

        /// <summary>
        /// The Y position.
        /// </summary>
        /// <returns></returns>
        public float Y
        {
            get { return _position.Y; }
            set
            {
                Vector3 prePosition = _position;
                _position.Y = value;

                if (OnPositionChanged != null)
                    OnPositionChanged(this, prePosition);
            }
        }

        /// <summary>
        /// The Z position.
        /// </summary>
        /// <returns></returns>
        public float Z
        {
            get { return _position.Z; }
            set
            {
                Vector3 prePosition = _position;
                _position.Z = value;

                if (OnPositionChanged != null)
                    OnPositionChanged(this, prePosition);
            }
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
        /// The weight of this object.
        /// </summary>
        /// <returns></returns>
        public float weight
        {
            get { return _weight; }
            set { _weight = value; }
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

        /// <summary>
        /// If this object should be sorted to appear as closest to the ground.
        /// </summary>
        /// <returns></returns>
        protected bool sortLowest
        {
            get { return _sortLowest; }
            set { _sortLowest = value; }
        }

        #endregion

        public StageObject(GGGame game)
        {
            _game = game;

            _sortingPriority = _currentSortingPriority;
            _currentSortingPriority++;
        }

        /// <summary>
        /// Adds a new bounding box to the array of defined bounding boxes for this object.
        /// </summary>
        /// <param name="size">The size of the box.</param>
        /// <param name="offset">The offset of this box relative to the position of this object.</param>
        protected void addBoundingBox(Vector3 size, Vector3 offset)
        {
            _boundingBoxes.Add(
                new BoundingBox(new Vector3(-size.X / 2f + offset.X, -size.Y / 2f + offset.Y, -size.Z / 2f + offset.Z),
                                               new Vector3(size.X / 2f + offset.X, size.Y / 2f + offset.Y, size.Z / 2f + offset.Z))
                              );

        }

        /// <summary>
        /// Returns the array of defined bounding boxes for this object.
        /// </summary>
        /// <returns></returns>
        public BoundingBox[] boundingBoxes
        {
            get
            {
                BoundingBox[] boxes = new BoundingBox[_boundingBoxes.Count];

                //Add this object's position to each bounding box as offset:
                for (int i = 0; i < _boundingBoxes.Count; i++)
                    boxes[i] = _boundingBoxes[i].Offset(_position);

                return boxes;
            }
        }

        /// <summary>
        /// The default bounding box of this object based on its position and size.
        /// </summary>
        /// <returns></returns>
        public virtual BoundingBox boundingBox
        {
            get
            {
                return new BoundingBox(new Vector3(_position.X - _size.X / 2f, _position.Y, _position.Z - _size.Z / 2),
                                       new Vector3(_position.X + _size.X / 2f, _position.Y + _size.Y, _position.Z + _size.Z / 2));
            }
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
        public virtual void getHit(Attack attack) { }

        /// <summary>
        /// This objects gets hit and moves by a certain amount.
        /// </summary>
        public virtual void getHit(Vector3 movement, int health, bool knockback) { }

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
        public virtual int CompareTo(StageObject obj)
        {
            //When something should be rendered lowest (most likely a floor tile), we put it at the end:
            //When both are lowest, return that with the lowest sorting priority:
            if (_sortLowest && !obj.sortLowest)
            {
                return -1;
            }
            else if (!_sortLowest && obj.sortLowest)
            {
                return 1;
            }
            else if (_sortLowest && obj.sortLowest)
            {
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
            else
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
}