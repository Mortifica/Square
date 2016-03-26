using Square_DX.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Square_DX.BasicClasses;
using Square_DX.Input;

namespace Square_DX.Screens
{
    class TestLevelScreen : IScreen
    {

        private SpriteFont font;
        private Character player;
        private Texture2D playerTexture;
        private Texture2D blockTexture;
        private Texture2D pickupTexture;
        private List<Block> blocks = new List<Block>();
        private List<PickUps> pickups = new List<PickUps>();
        private CollisionManager collisionManager;
        private ContentManager Content;
        private KeyboardListener Listener;
        public TestLevelScreen(ContentManager content, KeyboardListener listener)
        {
            Content = content;
            Listener = listener;
            init();
        }
        public void init()
        {
            collisionManager = new CollisionManager(blocks, pickups);
            font = Content.Load<SpriteFont>("startMenuFont");
            playerTexture = Content.Load<Texture2D>("Blue_Square");
            blockTexture = Content.Load<Texture2D>("Block_Brown");
            pickupTexture = Content.Load<Texture2D>("Ball_Purple");
            player = new Character(new Vector2(100, 300 - playerTexture.Height), playerTexture, font, collisionManager);
            for (int i = 0; i < 500; i++)
            {
                if (i > 10 && i % 3 == 0)
                {
                    pickups.Add(new PickUps(pickupTexture, new Vector2(pickupTexture.Width * i, 300 - pickupTexture.Height)));
                }
                blocks.Add(new Block(new Vector2(blockTexture.Width * i, 300), blockTexture));
            }
            for (int i = 0; i < 20; i++)
            {
                if (i % 3 == 0)
                {
                    pickups.Add(new PickUps(pickupTexture, new Vector2(100 + pickupTexture.Width * i, 250 - pickupTexture.Height)));
                }
                blocks.Add(new Block(new Vector2(100 + (blockTexture.Width * i), 250), blockTexture));
            }
            for (int i = 0; i < 20; i++)
            {
                if (i % 3 == 0)
                {
                    pickups.Add(new PickUps(pickupTexture, new Vector2(300 + pickupTexture.Width * i, 200 - pickupTexture.Height)));
                }
                blocks.Add(new Block(new Vector2(300 + (blockTexture.Width * i), 200), blockTexture));
            }
            Listener.AddSubscriber(player);

        }
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var block in blocks)
            {
                block.Draw(spriteBatch);
            }
            foreach (var pickup in pickups)
            {
                pickup.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Game Running",new Vector2(200, 200), Color.BurlyWood);
        }

        public ICameraFocus GetFocus()
        {
            return (ICameraFocus)player;
        }
    }
}
