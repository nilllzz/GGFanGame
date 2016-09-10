using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GGFanGame.Content;
using System;

namespace GGFanGame
{
    /// <summary>
    /// The main game type.
    /// </summary>
    class GGGame : Microsoft.Xna.Framework.Game
    {
        public const int RENDER_WIDTH = 1280;
        public const int RENDER_HEIGHT = 720;
        public const string GAME_TITLE = "Hard Dudes";

        private static GGGame _instance;

        /// <summary>
        /// The active game instance.
        /// </summary>
        public static GGGame getInstance()
        {
            if (_instance == null)
                _instance = new GGGame();

            return _instance;
        }

        private RenderTarget2D _target; //The target each frame renders to.

        /// <summary>
        /// The global randomizer of the game.
        /// </summary>
        public Random random { get; private set; } = new Random();

        /// <summary>
        /// The texture manager for this game.
        /// </summary>
        public TextureManager textureManager { get; private set; }

        /// <summary>
        /// The music manager for this game.
        /// </summary>
        public MusicManager musicManager { get; private set; }

        /// <summary>
        /// The font manager for this game.
        /// </summary>
        public FontManager fontManager { get; private set; }

        /// <summary>
        /// The active main sprite batch of the game.
        /// </summary>
        public SpriteBatch spriteBatch { get; private set; }

        /// <summary>
        /// The active font sprite batch of the game.
        /// </summary>
        public SpriteBatch fontBatch { get; private set; }

        /// <summary>
        /// The video card manager.
        /// </summary>
        public GraphicsDeviceManager graphics { get; private set; }

        /// <summary>
        /// Returns a rectangle representing the game's drawing area relative to the window position.
        /// </summary>
        public Rectangle clientRectangle
        {
            get { return new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); }
        }

        private GGGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            musicManager = new MusicManager();
            textureManager = new TextureManager();
            fontManager = new FontManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Just testing the screen manager here and setting the main menu as first screen.
            //I guess we will implement a splash screen of some sort later.
            Screens.ScreenManager.getInstance().setScreen(new Screens.Menu.TitleScreen());

            graphics.PreferredBackBufferWidth = RENDER_WIDTH;
            graphics.PreferredBackBufferHeight = RENDER_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontBatch = new SpriteBatch(GraphicsDevice);

            Drawing.Graphics.initialize(GraphicsDevice, spriteBatch);
            _target = new RenderTarget2D(GraphicsDevice, RENDER_WIDTH, RENDER_HEIGHT);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.ControlsHandler.update();

            Screens.ScreenManager.getInstance().updateScreen(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(_target);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            fontBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            Screens.ScreenManager.getInstance().drawScreen(gameTime);

            spriteBatch.End();
            fontBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            spriteBatch.Draw(_target, clientRectangle, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Resets the render target to the default.
        /// </summary>
        public void resetRenderTarget()
        {
            GraphicsDevice.SetRenderTarget(_target);
        }

        /// <summary>
        /// Begins to render the screen to a render target.
        /// </summary>
        public RenderTarget2D beginRenderScreenToTarget()
        {
            RenderTarget2D target = new RenderTarget2D(GraphicsDevice, RENDER_WIDTH, RENDER_HEIGHT);

            //End the sprite batch, render to current target.
            //Then, set to new render target and begin the batch.
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(target);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            return target;
        }

        /// <summary>
        /// Ends rendering the screen to a render target.
        /// </summary>
        public void endRenderScreenToTarget()
        {
            //Ends the sprite batch for the current target, resets the target, and starts the sprite batch for the default target.
            spriteBatch.End();
            resetRenderTarget();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
        }
    }
}