using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square_DX.Menu
{
    public class OverlayMainMenu : OverlayAbstract
    { 
        public OverlayMainMenu(IScreen screen)
            :base(screen)
        {

        }

        public override void Update(GameTime gameTime)
        {
            Screen.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Screen.Draw(spriteBatch);
        }
    }
}
