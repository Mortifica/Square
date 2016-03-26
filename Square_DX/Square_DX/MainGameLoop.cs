using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Square_DX.BasicClasses;
using System.Collections.Generic;
using Square_DX.Input;
using Square_DX.Menu;
using Square_DX.Screens;

namespace Square_DX
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>a
    public class MainGameLoop : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameState CurrentState { get; set; }
        private Camera2D Camera;
        public MainGameLoop()
        {
            graphics = new GraphicsDeviceManager(this);
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
            CurrentState = new StartState(this);
            Camera = new Camera2D(CurrentState);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            // TODO: Add your update logic here
            CurrentState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        Camera.GetMatrixTransformation(GraphicsDevice));

            CurrentState.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// Start State
        /// </summary>
        public class StartState : GameState, IInputSubscriber 
        {
            private OverLayNone testOverlay;
            private OverlayMainMenu mainMenu;
            private OverlayAbstract currentOverlay;

            public StartState(MainGameLoop game)
                : base(game)
            {
                init();
            }
            private void init()
            {
                var font = game.Content.Load<SpriteFont>("startMenuFont");
                    
                var menuScreen = new MainMenuScreen(this, font, null);
                Listener = new KeyboardListener();
                Listener.AddSubscriber(this);
                testOverlay = new OverLayNone(menuScreen);

                currentOverlay = testOverlay;
            }

            public override void Update(GameTime gameTime)
            {
                Listener.Update(Keyboard.GetState(), gameTime);
                currentOverlay.Update(gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                currentOverlay.Draw(spriteBatch);
            }
            public void NextState()
            {
                game.CurrentState = new PlayState(game);
            }

            public void NotifyOfChange(KeyboardChangeState keyboardChangeState, GameTime gameTime)
            {
                if (keyboardChangeState.CurrentState.IsKeyDown(Keys.Tab))
                {
                    var tempScreen = currentOverlay.GetCurrentScreen();
                    Listener.AddSubscriber((MainMenuScreen)tempScreen);
                    Listener.RemoveSubscriber(this);
                    mainMenu = new OverlayMainMenu(tempScreen);
                    currentOverlay = mainMenu;
                }
            }
        }
        /// <summary>
        /// Contains the Encapslated Game Logic
        /// </summary>
        public class PlayState : GameState, IInputSubscriber
        {

            private OverLayNone level;
            private OverlayDebug debugLevel;
            private OverlayAbstract currentOverlay;
            public PlayState(MainGameLoop game)
                : base(game)
            {
                init();
            }
            private void init()
            {
                XOffset = .25f;
                YOffset = .75f;
                Listener = new KeyboardListener();
                var screen = new TestLevelScreen(game.Content, Listener);
                level = new OverLayNone(screen);
                currentOverlay = level;
                
                Listener.AddSubscriber(this);
                game.Camera.Focus = screen.GetFocus();
                game.Camera.FocusOffest = new Vector3((float)(game.GraphicsDevice.Viewport.Width / 2),(float)(game.GraphicsDevice.Viewport.Height / 2), 0);
            }

            public override void Update(GameTime gameTime)
            {
                Listener.Update(Keyboard.GetState(), gameTime);
                currentOverlay.Update(gameTime);
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                currentOverlay.Draw(spriteBatch);
            }
            public void NotifyOfChange(KeyboardChangeState keyboardChangeState, GameTime gameTime)
            {
                
            }
        }

    }
}
