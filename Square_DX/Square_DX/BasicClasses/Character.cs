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
        public Sprite Texture { get; set; }
        public Vector2 Location { get; set; }
        private int characterSpeed = 50;
        private Vector2 Velocity = Vector2.Zero;
        private Vector2 Jump = Vector2.Zero;
        private Vector2 gravity = Vector2.Zero;
        private SpriteFont font;
        private List<Block> blocks;
        private bool JumpWasPressed = false;
        private TimeSpan JumpTime = TimeSpan.Zero;
        public Character(Vector2 location, Texture2D texture, SpriteFont font, List<Block> blocks)
        {
            this.font = font;
            this.blocks = blocks;
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
            UpdateGravity(gameTime);
            if (JumpWasPressed)
            {
                JumpTime += gameTime.ElapsedGameTime;

                if (JumpTime < TimeSpan.FromMilliseconds(100))
                {
                    Jump += new Vector2(0, -30);
                }
                else
                {
                    JumpWasPressed = false;
                }
            }
            else
            {
                
            }

            Jump += gravity;
        }
        
        private void UpdateGravity(GameTime gameTime)
        {
            bool canFall = true;

            foreach (var block in blocks)
            {
                 if(block.IsIntersectedBy(new Vector2(Location.X , Location.Y + 1), Texture.Texture.Height))
                {
                    Location = new Vector2(Location.X , block.Location.Y - Texture.Texture.Height);
                    Jump = Vector2.Zero;
                    
                    canFall = false;
                    break;
                }
            }

            if (canFall)
            {
                if (gravity.Equals(Vector2.Zero))
                {
                    gravity = new Vector2(0, 2f);
                }
                gravity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            else
            {
                gravity = Vector2.Zero;
                JumpTime = TimeSpan.Zero;

                
            }

        }
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Texture.Texture, Location, Color.White);
            spritebatch.DrawString(font, "Jump Y Vector" + Jump.Y + ", Gravity Y Vector" + gravity.Y, new Vector2(10, 10), Color.Black);
            spritebatch.DrawString(font, "Jump Button was pressed: " + JumpWasPressed, new Vector2(10, 25), Color.Black);
            spritebatch.DrawString(font, "Jump elapsed time" + JumpTime, new Vector2(10, 40), Color.Black);
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
            if (keyboardChangeState.CurrentState.IsKeyDown(Keys.Space))
            {

                if (!JumpWasPressed)
                {
                    JumpWasPressed = true;

                }
            }

            Jump += gravity;
            //normalizes the vector so you don't move diagnal faster than other directions
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }
            Velocity = velocity * characterSpeed;
            Velocity += Jump;
            Location += (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
