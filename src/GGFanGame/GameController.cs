using GameDevCommon;
using GameDevCommon.Drawing;
using GameDevCommon.Input;
using GGFanGame.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGFanGame
{
    /// <summary>
    /// The main game type.
    /// </summary>
    internal class GameController : Microsoft.Xna.Framework.Game, IGame
    {
        public const int RENDER_WIDTH = 1280;
        public const int RENDER_HEIGHT = 720;
        public const string GAME_TITLE = "Hard Dudes";

        private SpriteBatch _batch;
        private ComponentManager _componentManager;

        /// <summary>
        /// The video card manager.
        /// </summary>
        internal GraphicsDeviceManager Graphics { get; }

        /// <summary>
        /// Returns a rectangle representing the game's drawing area relative to the window position.
        /// </summary>
        internal Rectangle ClientRectangle => new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        internal GameController()
        {
            GameInstanceProvider.SetInstance(this);

            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = RENDER_WIDTH,
                PreferredBackBufferHeight = RENDER_HEIGHT,
                GraphicsProfile = GraphicsProfile.HiDef,
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };
            Window.Position = new Point(0, 100);
            Window.Title = $"Game Grumps: {GAME_TITLE}";
        }

        public Microsoft.Xna.Framework.Game GetGame()
        {
            return this;
        }

        public ComponentManager GetComponentManager()
        {
            return _componentManager;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _componentManager = new ComponentManager();
            _componentManager.LoadComponents();

            base.Initialize();

            _componentManager.GetComponent<ScreenManager>().SetScreen(new Screens.Menu.TitleScreen());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _batch = new SpriteBatch(GraphicsDevice);
            RenderTargetManager.Initialize(RENDER_WIDTH, RENDER_HEIGHT);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _componentManager.GetComponent<ControlsHandler>().Update();
            _componentManager.GetComponent<ScreenManager>().UpdateScreen(gameTime);

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

            _componentManager.GetComponent<ScreenManager>().DrawScreen(gameTime);

            GraphicsDevice.SetRenderTarget(null);

            _batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise);
            _batch.Draw(RenderTargetManager.DefaultTarget, ClientRectangle, Color.White);
            _batch.End();

            base.Draw(gameTime);
        }
    }
}
