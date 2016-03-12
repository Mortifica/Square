using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Square_DX.BasicClasses
{
    class Block
    {
        private Sprite Texture { get; set; }
        public Vector2 Location { get; set; }

        public Block(Vector2 location, Texture2D texture)
        {
            Texture = new Sprite(texture,1,1);
            Location = location;
        }
        public bool IsIntersectedBy(Vector2 location, int size)
        {
            Rectangle pastInRect = new Rectangle(new Point((int)location.X, (int)location.Y),new Point(size));
            Rectangle currentRect = new Rectangle(new Point((int)Location.X, (int)Location.Y), new Point(Texture.Texture.Height));

            if (currentRect.Intersects(pastInRect))
            {
                return true;
            }
            return false;
        }
        public void Update()
        {

        }
        
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture.Texture, Location, Color.White);
        }

    }
}
