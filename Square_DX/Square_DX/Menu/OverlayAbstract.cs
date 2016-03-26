using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Square_DX.Menu
{
    public abstract class OverlayAbstract
    {
        protected IScreen Screen { get; set; }
        public OverlayAbstract(IScreen screen)
        {
            Screen = screen;
        }
        public IScreen GetCurrentScreen()
        {
            return Screen;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
