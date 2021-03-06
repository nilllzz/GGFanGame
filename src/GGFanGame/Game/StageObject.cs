﻿using System;
using System.Collections.Generic;
using GGFanGame.DataModel.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameDevCommon.Rendering;

namespace GGFanGame.Game
{
    /// <summary>
    /// The base object for all things that appear in a stage.
    /// </summary>
    internal abstract class StageObject : Base3DObject<VertexPositionNormalTexture>, IComparable<StageObject>
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
        /// <summary>
        /// Event that fires when this object gets hit and takes damage.
        /// </summary>
        public event Action<StageObject> OnGettingHit;

        private Vector3 _position;
        private readonly List<BoundingBox> _boundingBoxes = new List<BoundingBox>();

        private static int _instanceCount = 0;
        private int _instanceNum;
        private int _maxHealth;
        private bool _loadedContent;

        #region Properties

        /// <summary>
        /// The parent stage this StageObject belongs to.
        /// </summary>
        public Stage ParentStage { protected get; set; }

        /// <summary>
        /// The main color associated with this object.
        /// </summary>
        public Color ObjectColor { get; protected set; } = Color.Orange;

        /// <summary>
        /// The absolute position of this object in the level.
        /// </summary>
        public Vector3 Position
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
        protected float ZSortingOffset { get; set; } = 0f;

        /// <summary>
        /// The size of this object.
        /// </summary>
        public Vector3 Size { get; set; }
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// The way this object is facing.
        /// </summary>
        public ObjectFacing Facing { get; set; } = ObjectFacing.Right;

        /// <summary>
        /// If other objects can collide with this one.
        /// </summary>
        public bool Collision { get; set; } = false;

        /// <summary>
        /// The health of this object.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// The maximum amount of the health of this object.
        /// </summary>
        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                Health = value;
            }
        }

        /// <summary>
        /// The strength of this object.
        /// </summary>
        public float Strength { get; set; } = 0;

        /// <summary>
        /// The weight of this object.
        /// </summary>
        public float Weight { get; set; } = 0;

        /// <summary>
        /// A check to indicate wether this object can be removed from a stage.
        /// </summary>
        public bool CanBeRemoved { get; protected set; } = false;

        /// <summary>
        /// If anything can interact with this object.
        /// </summary>
        public bool CanInteract { get; protected set; } = false;

        /// <summary>
        /// If an object can land on this object.
        /// </summary>
        public bool CanLandOn { get; set; } = false;

        /// <summary>
        /// If the player can use a button on the gamepad to interact with this object.
        /// </summary>
        public bool CanClick { get; set; } = false;

        internal float CameraDistance => -Z; // Vector3.Distance(ParentStage.Camera.Position, Position);

        #endregion

        protected StageObject()
        {
            _instanceNum = _instanceCount;
            _instanceCount++;
            MaxHealth = 1; // 1 is the default so every object has at least one health when spawned.
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_loadedContent)
            {
                LoadContentInternal();
                _loadedContent = true;
            }
        }

        /// <summary>
        /// Loads the content of this StageObject.
        /// </summary>
        protected virtual void LoadContentInternal() { }

        /// <summary>
        /// Applies the data from a passed in data model to the object (default implementation contains position set).
        /// </summary>
        public virtual void ApplyDataModel(StageObjectModel dataModel)
        {
            Position = dataModel.Position;
        }

        /// <summary>
        /// The default bounding box of this object based on its position and size.
        /// </summary>
        public virtual BoundingBox BoundingBox
            => new BoundingBox(new Vector3(_position.X - Size.X / 2f, _position.Y, _position.Z - Size.Z / 2f),
                               new Vector3(_position.X + Size.X / 2f, _position.Y + Size.Y, _position.Z + Size.Z / 2f));

        /// <summary>
        /// This object gets hit by an attack.
        /// </summary>
        public virtual void GetHit(Attack attack) => OnGettingHit?.Invoke(this);

        /// <summary>
        /// This objects gets hit and moves by a certain amount.
        /// </summary>
        public virtual void GetHit(StageObject origin, Vector3 movement, int health, bool knockback) => OnGettingHit?.Invoke(this);

        /// <summary>
        /// Returns the lower center of this object.
        /// </summary>
        public virtual Vector3 GetFeetPosition() => _position;

        /// <summary>
        /// The player clicked on this object.
        /// </summary>
        public virtual void OnPlayerClick() { }

        protected void SetWorld(Vector3 position)
        {
            World = Matrix.CreateScale(1f) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(position);
        }

        //Needed in order to sort the list of objects and arrange them in an order
        //so that the objects in the foreground are overlaying those in the background.
        public virtual int CompareTo(StageObject obj)
        {
            if (!IsOpaque && !obj.IsOpaque)
            {
                return CameraDistance < obj.CameraDistance ?
                    1 : -1;
            }

            return _instanceNum < obj._instanceNum ?
                1 : -1;
        }
    }
}
