using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGFanGame
{
    /// <summary>
    /// The main game type.
    /// </summary>
    internal class GameController : Microsoft.Xna.Framework.Game
    {
        public const int RENDER_WIDTH = 1280;
        public const int RENDER_HEIGHT = 720;
        public const string GAME_TITLE = "Hard Dudes";

        private static GameController _instance;

        /// <summary>
        /// The active game instance.
        /// </summary>
        internal static GameController GetInstance() => _instance ?? (_instance = new GameController());

        /// <summary>
        /// The global randomizer of the game.
        /// </summary>
        internal Random Random { get; private set; } = new Random();
        
        /// <summary>
        /// The active main sprite batch of the game.
        /// </summary>
        internal SpriteBatch SpriteBatch { get; private set; }

        /// <summary>
        /// The active font sprite batch of the game.
        /// </summary>
        internal SpriteBatch FontBatch { get; private set; }

        /// <summary>
        /// The video card manager.
        /// </summary>
        internal GraphicsDeviceManager Graphics { get; }

        /// <summary>
        /// Returns a rectangle representing the game's drawing area relative to the window position.
        /// </summary>
        internal Rectangle ClientRectangle => new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        private GameController()
        {
            Graphics = new GraphicsDeviceManager(this);
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
            //Just testing the screen manager here and setting the main menu as first screen.
            //I guess we will implement a splash screen of some sort later.
            Screens.ScreenManager.GetInstance().SetScreen(new Screens.Menu.TitleScreen());
            Window.Title = $"Game Grumps: {GAME_TITLE}";

            Graphics.PreferredBackBufferWidth = RENDER_WIDTH;
            Graphics.PreferredBackBufferHeight = RENDER_HEIGHT;
            Graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            FontBatch = new SpriteBatch(GraphicsDevice);
            
            Drawing.Graphics.Initialize(GraphicsDevice, SpriteBatch);
            RenderTargetManager.initialize();
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

            Input.ControlsHandler.Update();

            Screens.ScreenManager.GetInstance().UpdateScreen(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(RenderTargetManager.DefaultTarget);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            FontBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            Screens.ScreenManager.GetInstance().DrawScreen(gameTime);

            SpriteBatch.End();
            FontBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            SpriteBatch.Draw(RenderTargetManager.DefaultTarget, ClientRectangle, Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}