using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Square_DX.BasicClasses
{
    public class PickUps
    {
        private Sprite Texture;
        private Vector2 Location;

        public PickUps(Texture2D texture, Vector2 location)
        {
            Texture = new Sprite(texture, 1, 1);
            Location = location;
        }
        public bool IsIntersectedBy(Vector2 location, int size)
        {
            Rectangle pastInRect = new Rectangle(new Point((int)location.X, (int)location.Y), new Point(size));
            Rectangle currentRect = new Rectangle(new Point((int)Location.X, (int)Location.Y), new Point(Texture.Texture.Height));

            if (currentRect.Intersects(pastInRect))
            {
                return true;
            }
            return false;
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Texture.Draw(spriteBatch, Location);
        }
    }
}
