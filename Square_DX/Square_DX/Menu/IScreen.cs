using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Square_DX.BasicClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Square_DX.Menu
{
    /// <summary>
    /// An Interface that gives both Access to Draw and Update Methods
    /// </summary>
    public interface IScreen
    {
        ICameraFocus GetFocus();
        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
