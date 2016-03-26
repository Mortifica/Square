using Square_DX.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Square_DX.Input;
using Square_DX.BasicClasses;
using Microsoft.Xna.Framework.Input;

namespace Square_DX.Screens
{
    class MainMenuScreen : IScreen, IInputSubscriber
    {
        private MainGameLoop.StartState containingState;
        private SpriteFont font;
        private Sprite background;
        private Options[] MenuOptions = new Options[3]
        {
                Options.StartGame,
                Options.Help,
                Options.Credits
        };
        private Vector2 MenuLocation = new Vector2(100, 100);
        private int currentOption = 1;
        private TimeSpan elapseTime = TimeSpan.Zero;
        private int menuSpeed = 100;
        private int currentColor = 0;

        public MainMenuScreen(MainGameLoop.StartState state, SpriteFont spriteFont, Sprite sprite)
        {
            containingState = state;
            font = spriteFont;
            background = sprite;
        }
        public void NotifyOfChange(KeyboardChangeState keyboardChangeState, GameTime gameTime)
        {
            elapseTime += gameTime.ElapsedGameTime;
            //needs a lot of work to make the transitions ok speed wise
            if ((keyboardChangeState.CurrentState.IsKeyDown(Keys.W) || keyboardChangeState.CurrentState.IsKeyDown(Keys.Up)) && elapseTime >= TimeSpan.FromMilliseconds(menuSpeed))
            {
                currentOption = currentOption - 1;
                if (currentOption < 1) currentOption = 1;

            }
            if ((keyboardChangeState.CurrentState.IsKeyDown(Keys.S) || keyboardChangeState.CurrentState.IsKeyDown(Keys.Down)) && elapseTime >= TimeSpan.FromMilliseconds(menuSpeed))
            {
                currentOption = currentOption + 1;
                if (currentOption > 3) currentOption = 3;
            }
            if (keyboardChangeState.CurrentState.IsKeyDown(Keys.A)
                && MenuOptions[currentOption - 1].Equals(Options.StartGame))
            {
                containingState.NextState();
            }
            if (elapseTime > TimeSpan.FromMilliseconds(menuSpeed))
            {
                elapseTime = TimeSpan.Zero;
            }
        }

        public void Update(GameTime gametime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.Black;
            Color topColor = Color.Black;
            currentColor += 1;
            if (currentColor % 1 == 0)
            {
                topColor = Color.Black;
            }
            if (currentColor % 2 == 0)
            {
                topColor = Color.Pink;
            }
            if (currentColor % 3 == 0)
            {
                topColor = Color.Blue;
            }
            if (currentColor >= 1000)
            {
                currentColor = 0;
            }


            Vector2 menu = MenuLocation;
            spriteBatch.DrawString(font, "Press \"A\" to select an option.", new Vector2(50, 40), topColor);
            spriteBatch.DrawString(font, "Navigate Menu with W,S,UP,Down.", new Vector2(50, 60), topColor);

            for (int i = 0; i < MenuOptions.Length; i++)
            {
                if (currentOption == i + 1)
                {
                    color = Color.Red;
                }
                else
                {
                    color = Color.Black;
                }
                spriteBatch.DrawString(font, MenuOptions[i].ToString(), menu += new Vector2(0, font.LineSpacing), color);
            }
        }

        public ICameraFocus GetFocus()
        {
            throw new NotImplementedException();
        }

        private enum Options
        {
            StartGame,
            Help,
            Credits
        }
    }
}
