using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square_DX.Menu
{
    public class OverLayNone : OverlayAbstract
    {
        public OverLayNone(IScreen screen)
            : base(screen)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Screen.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Screen.Update(gameTime);
        }
    }
}
