using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Square_DX.BasicClasses;
using System.Collections.Generic;
using Square_DX.Input;

namespace Square_DX
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGameLoop : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameState CurrentState { get; set; } 

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
            spriteBatch.Begin();

            CurrentState.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// Start Menu
        /// </summary>
        public class StartState : GameState, IInputSubscriber
        {
            private SpriteFont font;
            private Sprite background;
            private Options[] MenuOptions = new Options[3]
            {
                Options.StartGame,
                Options.Help,
                Options.Credits
            };
            private Vector2 MenuLocation = new Vector2(100, 100);
            private int currentOption = 1;
            private TimeSpan elapseTime = TimeSpan.Zero;
            private int menuSpeed = 100;
            private int currentColor = 0;
            public StartState(MainGameLoop game)
                : base(game)
            {
                init();
            }
            private void init()
            {
                font = game.Content.Load<SpriteFont>("startMenuFont");
                Listener = new KeyboardListener();
                Listener.AddSubscriber(this);
            }

            public override void Update(GameTime gameTime)
            {
                Listener.Update(Keyboard.GetState(), gameTime);
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                Color color = Color.Black;
                Color topColor = Color.Black;
                currentColor += 1;
                if (currentColor % 1 == 0)
                {
                    topColor = Color.Black;
                }
                if (currentColor % 2 == 0)
                {
                    topColor = Color.Pink;
                }
                if (currentColor % 3 == 0)
                {
                    topColor = Color.Blue;
                }
                if (currentColor >= 1000)
                {
                    currentColor = 0;
                }


                Vector2 menu = MenuLocation;
                spriteBatch.DrawString(font, "Press \"A\" to select an option.", new Vector2(50, 40), topColor);
                spriteBatch.DrawString(font, "Navigate Menu with W,S,UP,Down.", new Vector2(50, 60), topColor);

                for (int i = 0; i < MenuOptions.Length; i++)
                {
                    if (currentOption == i + 1)
                    {
                        color = Color.Red;
                    }
                    else
                    {
                        color = Color.Black;
                    }
                    spriteBatch.DrawString(font, MenuOptions[i].ToString(), menu += new Vector2(0, font.LineSpacing), color);
                }
                
            }
            /// <summary>
            /// Contains the MainMenu
            /// </summary>
            /// <param name="keyboardChangeState"></param>
            /// <param name="gameTime"></param>
            public void NotifyOfChange(KeyboardChangeState keyboardChangeState, GameTime gameTime)
            {

                elapseTime += gameTime.ElapsedGameTime;
                //needs a lot of work to make the transitions ok speed wise
                if ((keyboardChangeState.CurrentState.IsKeyDown(Keys.W) || keyboardChangeState.CurrentState.IsKeyDown(Keys.Up)) && elapseTime >= TimeSpan.FromMilliseconds(menuSpeed))
                {
                    currentOption = currentOption - 1;
                    if (currentOption < 1) currentOption = 1;

                }
                if ((keyboardChangeState.CurrentState.IsKeyDown(Keys.S) || keyboardChangeState.CurrentState.IsKeyDown(Keys.Down)) && elapseTime >= TimeSpan.FromMilliseconds(menuSpeed))
                {
                    currentOption = currentOption + 1;
                    if (currentOption > 3) currentOption = 3;
                }
                if(keyboardChangeState.CurrentState.IsKeyDown(Keys.A)
                    && MenuOptions[currentOption - 1].Equals(Options.StartGame))
                {
                    game.CurrentState = new PlayState(game);
                }
                if (elapseTime > TimeSpan.FromMilliseconds(menuSpeed))
                {
                    elapseTime = TimeSpan.Zero;
                }
            }

            private enum Options
            {
                StartGame,
                Help,
                Credits
            }
        }
        /// <summary>
        /// Contains the Encapslated Game Logic
        /// </summary>
        public class PlayState : GameState, IInputSubscriber
        {
            private SpriteFont font;
            private Character player;
            private Texture2D playerTexture;
            private Texture2D blockTexture;
            private List<Block> blocks = new List<Block>();
            public PlayState(MainGameLoop game)
                : base(game)
            {
                init();
            }
            private void init()
            {
                font = game.Content.Load<SpriteFont>("startMenuFont");
                playerTexture = game.Content.Load<Texture2D>("Blue_Square");
                blockTexture = game.Content.Load<Texture2D>("Block_Brown");
                player = new Character(new Vector2(100, 300 - playerTexture.Height), playerTexture, font, blocks);
                for (int i = 0; i < 50; i++)
                {
                    blocks.Add(new Block(new Vector2(blockTexture.Width * i, 300), blockTexture));
                }
                for (int i = 0; i < 20; i++)
                {
                    blocks.Add(new Block(new Vector2(100 + (blockTexture.Width * i), 250), blockTexture));
                }
                for (int i = 0; i < 20; i++)
                {
                    blocks.Add(new Block(new Vector2(300 + (blockTexture.Width * i), 210), blockTexture));
                }
                Listener = new KeyboardListener();
                Listener.AddSubscriber(this);
                Listener.AddSubscriber(player);
            }

            public override void Update(GameTime gameTime)
            {
                Listener.Update(Keyboard.GetState(), gameTime);
                player.Update(gameTime);
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                foreach (var block in blocks)
                {
                    block.Draw(spriteBatch);
                }
                player.Draw(spriteBatch);
                spriteBatch.DrawString(font, "Game Running", new Vector2(200, 200), Color.BurlyWood);
            }
            public void NotifyOfChange(KeyboardChangeState keyboardChangeState, GameTime gameTime)
            {
                
            }
        }

    }
}
