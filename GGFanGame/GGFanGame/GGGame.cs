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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _fontBatch;

        private MusicManager _musicManager;
        private TextureManager _textureManager;
        private FontManager _fontManager;

        private Random _random = new Random();

        private RenderTarget2D _target; //The target each frame renders to.

        public Random random
        {
            get { return _random; }
        }
        
        /// <summary>
        /// The texture manager for this game.
        /// </summary>
        /// <returns></returns>
        public TextureManager textureManager
        {
            get { return _textureManager; }
        }

        /// <summary>
        /// The music manager for this game.
        /// </summary>
        /// <returns></returns>
        public MusicManager musicManager
        {
            get { return _musicManager; }
        }

        /// <summary>
        /// The font manager for this game.
        /// </summary>
        /// <returns></returns>
        public FontManager fontManager
        {
            get { return _fontManager; }
        }

        /// <summary>
        /// The active main sprite batch of the game.
        /// </summary>
        /// <returns></returns>
        public SpriteBatch spriteBatch
        {
            get { return _spriteBatch; }
        }

        /// <summary>
        /// The active font sprite batch of the game.
        /// </summary>
        /// <returns></returns>
        public SpriteBatch fontBatch
        {
            get { return _fontBatch; }
        }

        /// <summary>
        /// The video card manager.
        /// </summary>
        /// <returns></returns>
        public GraphicsDeviceManager graphics
        {
            get { return _graphics; }
        }

        /// <summary>
        /// Returns a rectangle representing the game's drawing area relative to the window position.
        /// </summary>
        /// <returns></returns>
        public Rectangle clientRectangle
        {
            get { return new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); }
        }
        
        public GGGame() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _musicManager = new MusicManager(this);
            _textureManager = new TextureManager(this);
            _fontManager = new FontManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Just testing the screen manager here and setting the main menu as first screen.
            //I guess we will implement a splash screen of some sort later.
            Screens.ScreenManager.getInstance().setScreen(new Screens.Menu.TitleScreen(this));

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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fontBatch = new SpriteBatch(GraphicsDevice);

            Drawing.Graphics.initialize(GraphicsDevice, _spriteBatch);
            _target = new RenderTarget2D(GraphicsDevice, RENDER_WIDTH, RENDER_HEIGHT);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            _fontBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            Screens.ScreenManager.getInstance().drawScreen(gameTime);

            _spriteBatch.End();
            _fontBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            _spriteBatch.Draw(_target, clientRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}