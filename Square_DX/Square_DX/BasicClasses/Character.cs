using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Square_DX.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Square_DX.BasicClasses
{
    public class Character : IInputSubscriber
    {
        private Sprite Texture { get; set; }
        public Vector2 Location { get; set; }
        private int characterSpeed = 50;
        private Vector2 Velocity = Vector2.Zero;
        private Vector2 Jump = Vector2.Zero;
        private Vector2 gravity = new Vector2(0, 9.8f);
        public Character(Vector2 location, Texture2D texture)
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
        public void Update()
        {

        }
        private void UpdateGravity()
        {
            if ((int)Location.Y < 300)
            {
                gravity += gravity;
            }
            else
            {
                gravity = new Vector2(0, 9.8f);
            }
        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture.Texture, Location, Color.White);
        }

        public void NotifyOfChange(KeyboardChangeState keyboardChangeState, GameTime gameTime)
        {
            moveCharacter(keyboardChangeState, gameTime);
        }

        private void moveCharacter(KeyboardChangeState keyboardChangeState, GameTime gameTime)
        {
            
            var keyDictonary = new Dictionary<Keys, Vector2>
                                            {
                                                 {Keys.Left , new Vector2(-1, 0) },
                                                 {Keys.Right , new Vector2(1, 0) },
                                                 {Keys.A , new Vector2(-1, 0) },
                                                 {Keys.D , new Vector2(1, 0) },
                                            };

            var velocity = Vector2.Zero;

            foreach (var key in keyDictonary)
            {
                if (keyboardChangeState.CurrentState.IsKeyDown(key.Key))
                {

                    velocity += key.Value;
                    if (velocity.X > 1) { velocity.X = 1; }
                    if (velocity.X < -1) { velocity.X = -1; }

                }

            }

            //TODO:
            //Update vertical here before the normalize is done



            //normalizes the vector so you don't move diagnal faster than other directions
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            Velocity = velocity * characterSpeed;

            Location += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
