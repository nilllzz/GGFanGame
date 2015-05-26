using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame
{
    /// <summary>
    /// The main game type.
    /// </summary>
    public class GGGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteBatch _fontBatch;

        /// <summary>
        /// The active main sprite batch of the game.
        /// </summary>
        /// <returns></returns>
        public SpriteBatch spriteBatch
        {
            get { return _spriteBatch;  }
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

        public Rectangle clientRectangle
        {
            get { return new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); }
        }

        public GGGame() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            Screens.ScreenManager.getInstance().setScreen(new Screens.Menu.MainMenuScreen(this));

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 720;
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

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            _fontBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            Screens.ScreenManager.getInstance().drawScreen(gameTime);

            _spriteBatch.End();
            _fontBatch.End();

            base.Draw(gameTime);
        }
    }
}