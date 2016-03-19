using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Square_DX.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Square_DX.BasicClasses
{
    public abstract class GameState
    {
        protected KeyboardListener Listener { get; set; }

        protected MainGameLoop game { get; set; }

        public GameState(MainGameLoop game)
        {
            this.game = game;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void UpdateViewPort();
    }
}
