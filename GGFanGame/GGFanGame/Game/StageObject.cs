using System;
using System.Collections.Generic;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GGFanGame.Game
{
    /// <summary>
    /// The base object for all things that appear in a stage.
    /// </summary>
    internal abstract class StageObject : IComparable<StageObject>
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

        private Vector3 _position;
        private readonly List<BoundingBox> _boundingBoxes = new List<BoundingBox>();

        private static int _currentSortingPriority = 0; //Keeps track of all the sorting priorities added so that every object has a different one.
        private readonly int _sortingPriority = 0;
        private int _maxHealth;
        private bool _loadedContent;

        #region Properties

        protected static ContentManager content => Stage.activeStage.content;

        /// <summary>
        /// The main color associated with this object.
        /// </summary>
        public Color objectColor { get; protected set; } = Color.Orange;

        /// <summary>
        /// The absolute position of this object in the level.
        /// </summary>
        public Vector3 position
        {
            get { return _position; }
            set
            {
                var prePosition = _position;
                _position = value;

                OnPositionChanged?.Invoke(this, prePosition);
            }
        }

        /// <summary>
        /// The X position.
        /// </summary>
        public float X
        {
            get { return _position.X; }
            set
            {
                var prePosition = _position;
                _position.X = value;

                OnPositionChanged?.Invoke(this, prePosition);
            }
        }

        /// <summary>
        /// The Y position.
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
            set
            {
                var prePosition = _position;
                _position.Y = value;

                OnPositionChanged?.Invoke(this, prePosition);
            }
        }

        /// <summary>
        /// The Z position.
        /// </summary>
        public float Z
        {
            get { return _position.Z; }
            set
            {
                var prePosition = _position;
                _position.Z = value;

                OnPositionChanged?.Invoke(this, prePosition);
            }
        }

        /// <summary>
        /// The Z offset position used in sorting - for isometrically drawn objects that have Z depth.
        /// </summary>
        protected float zSortingOffset { get; set; } = 0f;

        /// <summary>
        /// The size of this object.
        /// </summary>
        public Vector3 size { get; set; }

        /// <summary>
        /// The way this object is facing.
        /// </summary>
        public ObjectFacing facing { get; set; } = ObjectFacing.Right;

        /// <summary>
        /// If other objects can collide with this one.
        /// </summary>
        public bool collision { get; set; } = false;

        /// <summary>
        /// The health of this object.
        /// </summary>
        public int health { get; set; }

        /// <summary>
        /// The maximum amount of the health of this object.
        /// </summary>
        public int maxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                health = value;
            }
        }

        /// <summary>
        /// The strength of this object.
        /// </summary>
        public float strength { get; set; } = 0;

        /// <summary>
        /// The weight of this object.
        /// </summary>
        public float weight { get; set; } = 0;

        /// <summary>
        /// A check to indicate wether this object can be removed from a stage.
        /// </summary>
        public bool canBeRemoved { get; protected set; } = false;

        /// <summary>
        /// If anything can interact with this object.
        /// </summary>
        public bool canInteract { get; protected set; } = false;

        /// <summary>
        /// If an object can land on this object.
        /// </summary>
        public bool canLandOn { get; set; } = false;

        /// <summary>
        /// If this object should be sorted to appear as closest to the ground.
        /// </summary>
        protected bool sortLowest { get; set; } = false;

        /// <summary>
        /// If the player can use a button on the gamepad to interact with this object.
        /// </summary>
        public bool canClick { get; set; } = false;

        /// <summary>
        /// The relation of this object to the ground.
        /// </summary>
        protected GroundRelation groundRelation { get; set; } = GroundRelation.Upright;

        #endregion

        protected StageObject()
        {
            _sortingPriority = _currentSortingPriority;
            _currentSortingPriority++;

            maxHealth = 1; // 1 is the default so every object has at least one health when spawned.
        }

        public void load()
        {
            if (!_loadedContent)
            {
                loadInternal();
                _loadedContent = true;
            }
        }

        /// <summary>
        /// Loads the content of this StageObject.
        /// </summary>
        protected virtual void loadInternal() { }

        /// <summary>
        /// Applies the data from a passed in data model to the object (default implementation contains position set).
        /// </summary>
        public virtual void applyDataModel(StageObjectModel dataModel) => position = dataModel.position;

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
        public BoundingBox[] boundingBoxes
        {
            get
            {
                var boxes = new BoundingBox[_boundingBoxes.Count];

                //Add this object's position to each bounding box as offset:
                for (var i = 0; i < _boundingBoxes.Count; i++)
                    boxes[i] = _boundingBoxes[i].Offset(_position);

                return boxes;
            }
        }

        /// <summary>
        /// The default bounding box of this object based on its position and size.
        /// </summary>
        public virtual BoundingBox boundingBox
        {
            get
            {
                if (groundRelation == GroundRelation.Upright)
                {
                    return new BoundingBox(new Vector3(_position.X - size.X / 2f, _position.Y, _position.Z - size.Z / 2f),
                                           new Vector3(_position.X + size.X / 2f, _position.Y + size.Y, _position.Z + size.Z / 2f));
                }
                else
                {
                    return new BoundingBox(new Vector3(_position.X - size.X / 2f, _position.Y, _position.Z - size.Z),
                                           new Vector3(_position.X + size.X / 2f, _position.Y + size.Y, _position.Z));
                }
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
        public virtual Vector3 getFeetPosition() => _position;

        /// <summary>
        /// The player clicked on this object.
        /// </summary>
        public virtual void onPlayerClick() { }

        //Needed in order to sort the list of objects and arrange them in an order
        //so that the objects in the foreground are overlaying those in the background.
        public virtual int CompareTo(StageObject obj)
        {
            var obj1Z = this.Z + this.zSortingOffset;
            var obj2Z = obj.Z + obj.zSortingOffset;

            // if the object is laying flat on the ground, have other objects on top of it
            // until the object is behind the first object (I hope no one needs to read this ever).

            if (this.groundRelation == GroundRelation.Flat)
                obj1Z -= this.size.Z;
            if (obj.groundRelation == GroundRelation.Flat)
                obj2Z -= obj.size.Z;

            if (obj1Z > obj2Z)
                return 1;
            else if (obj1Z < obj2Z)
                return -1;
            else
            {
                if (this._sortingPriority > obj._sortingPriority)
                    return 1;
                else if (this._sortingPriority < obj._sortingPriority)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
