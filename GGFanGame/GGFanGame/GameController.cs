using System;
using System.Collections.Generic;
using System.Linq;
using GGFanGame.Input;
using GGFanGame.Screens;
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
        
        private SpriteBatch _batch;
        
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
            LoadComponents();

            Window.Title = $"Game Grumps: {GAME_TITLE}";

            Graphics.PreferredBackBufferWidth = RENDER_WIDTH;
            Graphics.PreferredBackBufferHeight = RENDER_HEIGHT;
            Graphics.ApplyChanges();

            base.Initialize();

            GetComponent<ScreenManager>().SetScreen(new Screens.Menu.TitleScreen());
        }

        private void LoadComponents()
        {
            var componentInterfaceType = typeof(IGameComponent);
            foreach (var t in typeof(GameController).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(componentInterfaceType)))
                Components.Add(Activator.CreateInstance(t) as IGameComponent);
        }

        private readonly Dictionary<Type, IGameComponent> _componentCache = new Dictionary<Type, IGameComponent>();

        internal T GetComponent<T>() where T : IGameComponent
        {
            IGameComponent component;
            var tType = typeof(T);

            if (!_componentCache.TryGetValue(tType, out component))
            {
                component = Components.FirstOrDefault(c => c.GetType() == tType);
                _componentCache.Add(tType, component);
            }

            return (T)component;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _batch = new SpriteBatch(GraphicsDevice);
            RenderTargetManager.initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GetComponent<ControlsHandler>().Update();
            GetComponent<ScreenManager>().UpdateScreen(gameTime);

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
            
            GetComponent<ScreenManager>().DrawScreen(gameTime);
            
            GraphicsDevice.SetRenderTarget(null);

            _batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            _batch.Draw(RenderTargetManager.DefaultTarget, ClientRectangle, Color.White);
            _batch.End();

            base.Draw(gameTime);
        }

    }
}