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
        private int characterSpeed = 100;
        private Vector2 Velocity = Vector2.Zero;
        private Vector2 Jump = Vector2.Zero;
        private Vector2 gravity = Vector2.Zero;
        private SpriteFont font;
        private bool JumpWasPressed = false;
        private TimeSpan JumpTime = TimeSpan.Zero;
        private bool JumpIsPressed = false;
        private CollisionManager collisionManager;
        private SpriteBatch view;

        public Character(Vector2 location, Texture2D texture, SpriteFont font, CollisionManager manager, SpriteBatch view)
        {
            this.font = font;
            Texture = new Sprite(texture, 1, 1);
            Location = location;
            collisionManager = manager;
            this.view = view;
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
            Tuple<bool, PickUps> collision = collisionManager.PickUpsCollision(new Rectangle(new Vector2(Location.X, Location.Y + 1).ToPoint(), new Point(Texture.Texture.Height)));
            if (collision.Item1)
            {
                characterSpeed += 10;
            }
            UpdateGravity(gameTime);
            if (JumpWasPressed)
            {
                JumpTime += gameTime.ElapsedGameTime;

                if (JumpTime < TimeSpan.FromMilliseconds(125))
                {
                    if (JumpIsPressed)
                    {
                        Jump += new Vector2(0, -40);
                        JumpIsPressed = false;
                    }
                    
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
            Tuple<bool,Block> collision = collisionManager.BlockCollision(new Rectangle(new Vector2(Location.X, Location.Y + 1).ToPoint(), new Point(Texture.Texture.Height)));

            if(collision.Item1)
            {
                Location = new Vector2(Location.X , collision.Item2.Location.Y - Texture.Texture.Height);
                Jump = Vector2.Zero;
                    
                canFall = false;
                
            }
            

            if (canFall)
            {
                if (gravity.Equals(Vector2.Zero))
                {
                    gravity = new Vector2(0, 4f);
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
        public Vector2 UpdateViewPort(SpriteBatch spritebatch)
        {
            float centerX = (spritebatch.GraphicsDevice.Viewport.X) + Location.X;
            float centerY = (spritebatch.GraphicsDevice.Viewport.Y) + Location.Y;
            return new Vector2(centerX, centerY);
           
            
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
                JumpIsPressed = true;
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

            var tempVector = (Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            Location += tempVector;
            
        }
    }
}
