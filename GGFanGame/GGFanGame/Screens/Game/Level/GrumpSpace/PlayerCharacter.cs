using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame.Screens.Game.Level.GrumpSpace
{
    /// <summary>
    /// A playable character in the grump space.
    /// </summary>
    abstract class PlayerCharacter : LevelObject
    {
        /// <summary>
        /// The different states a player can have at a given time.
        /// </summary>
        public enum PlayerState
        {
            Idle,
            Walking,
            Jumping,
            Falling
        }

        /// <summary>
        /// An animation for a player state.
        /// </summary>
        protected struct PlayerAnimation
        {
            public int frameCount;
            public int maxDelay;
            public Vector2 startPosition;

            public PlayerAnimation(int frameCount, int maxDelay, Vector2 startPosition)
            {
                this.frameCount = frameCount;
                this.maxDelay = maxDelay;
                this.startPosition = startPosition;
            }
        }

        private PlayerState _state = PlayerState.Idle;
        /// <summary>
        /// The current player state.
        /// </summary>
        /// <returns></returns>
        protected PlayerState playerState
        {
            get { return _state; }
            set { _state = value; }
        }

        private Texture2D _texture = null;
        /// <summary>
        /// The loaded sprite sheet.
        /// </summary>
        /// <returns></returns>
        protected Texture2D texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private float _speed = 5f;
        /// <summary>
        /// The speed at which this character moves.
        /// </summary>
        /// <returns></returns>
        protected float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private float _jumpHeight = 14f;
        /// <summary>
        /// The height the character can jump.
        /// </summary>
        /// <returns></returns>
        protected float jumpHeight
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private Texture2D _shadow = null; //The shadow texture that displays below the sprite.
        private Dictionary<PlayerState, PlayerAnimation> _animations = new Dictionary<PlayerState, PlayerAnimation>(); //The loaded animations matching their player states.

        //Animation variables.
        private int _animationFrame = 0;
        private int _animationDelay = 8;
        private float _upwardsTrajectory = 0f;

        /// <summary>
        /// Creates a new instance of the player character class.
        /// </summary>
        protected PlayerCharacter(GGGame game) : base(game)
        {
            _shadow = gameInstance.textureManager.getResource(@"Misc\Shadow");
        }

        /// <summary>
        /// Adds an animation to the animation list.
        /// </summary>
        protected void addAnimation(PlayerState state, PlayerAnimation animation)
        {
            _animations.Add(state, animation);
        }

        /// <summary>
        /// Draws the player character and related things like the shadow.
        /// </summary>
        public override void draw()
        {
            SpriteEffects effect = SpriteEffects.None;
            if (facing == ObjectFacing.Left)
            {
                effect = SpriteEffects.FlipHorizontally;
            }

            gameInstance.spriteBatch.Draw(_shadow, new Rectangle((int)position.X + 16, (int)(position.Z) + 160, 96, 48), new Color(0, 0, 0, 100));
            gameInstance.spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)(position.Z - position.Y), 128, 192), new Rectangle((int)getAnimation().startPosition.X + _animationFrame * 32, (int)getAnimation().startPosition.Y, 32, 48), Color.White, 0f, Vector2.Zero, effect, 0f);
        }

        /// <summary>
        /// Updates the player character.
        /// </summary>
        public override void update()
        {
            updateMovement();

            if (getAnimation().frameCount > 1)
            {
                _animationDelay--;
                if (_animationDelay == 0)
                {
                    _animationDelay = getAnimation().maxDelay;
                    _animationFrame++;
                    if (_animationFrame == getAnimation().frameCount)
                    {
                        _animationFrame = 0;
                    }
                }
            }
        }

        private void updateMovement()
        {
            //We update the movement here.
            //If any movement occurs, then don't reset the state to Idle.
            //This is kept track in the isMoving variable.

            bool isMoving = false;

            /*For each direction, check if the thumbstick is pressed down, and then multiply the amount the thumbstick is pressed
            in that direction to multiply it with a default speed. */
            if (Input.GamePadHandler.buttonDown(PlayerIndex.One, Buttons.LeftThumbstickRight))
            {
                isMoving = true;
                if (_state == PlayerState.Idle)
                    setState(PlayerState.Walking);
                X += _speed * Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Left, Input.InputDirection.Right);
                facing = ObjectFacing.Right;
            }
            if (Input.GamePadHandler.buttonDown(PlayerIndex.One, Buttons.LeftThumbstickLeft))
            {
                isMoving = true;
                if (_state == PlayerState.Idle)
                    setState(PlayerState.Walking);
                X -= _speed * Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Left, Input.InputDirection.Left);
                facing = ObjectFacing.Left;
            }
            if (Input.GamePadHandler.buttonDown(PlayerIndex.One, Buttons.LeftThumbstickUp))
            {
                isMoving = true;
                if (_state == PlayerState.Idle)
                    setState(PlayerState.Walking);
                Z -= _speed * 0.8f * Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Left, Input.InputDirection.Up);
            }
            if (Input.GamePadHandler.buttonDown(PlayerIndex.One, Buttons.LeftThumbstickDown))
            {
                isMoving = true;
                if (_state == PlayerState.Idle)
                    setState(PlayerState.Walking);
                Z += _speed * 0.8f * Input.GamePadHandler.thumbStickDirection(PlayerIndex.One, Input.ThumbStick.Left, Input.InputDirection.Down);
            }

            //When the player is standing on the floor (Y = 0) and A is pressed, jump.
            if (Y == 0f && Input.GamePadHandler.buttonPressed(PlayerIndex.One, Buttons.A))
            {
                _upwardsTrajectory = _jumpHeight;
                setState(PlayerState.Jumping);
                isMoving = true;
            }

            if (_state == PlayerState.Jumping)
            {
                Y += _upwardsTrajectory;
                _upwardsTrajectory -= 1f;
                if (_upwardsTrajectory <= 0f)
                {
                    _upwardsTrajectory = 0f;
                    setState(PlayerState.Falling);
                }
            }
            else if (_state == PlayerState.Falling)
            {
                Y -= _upwardsTrajectory;
                _upwardsTrajectory += 1f;
                if (Y <= 0f)
                {
                    Y = 0f;
                    _upwardsTrajectory = 0f;
                    setState(PlayerState.Idle);
                }
            }
            else if (!isMoving)
            {
                //No movement occurs, set state to Idle.
                setState(PlayerState.Idle);
            }
        }

        /// <summary>
        /// Sets the state of the player.
        /// </summary>
        protected void setState(PlayerState state)
        {
            if (state != _state)
            {
                _state = state;
                _animationFrame = 0;
                _animationDelay = getAnimation().maxDelay;
            }
        }

        /// <summary>
        /// Returns the current animation.
        /// </summary>
        /// <returns></returns>
        private PlayerAnimation getAnimation()
        {
            return _animations[_state];
        }
    }
}