using Microsoft.Xna.Framework;

namespace Square_DX.Input
{
    public interface IInputSubscriber
    {
        void NotifyOfChange(KeyboardChangeState keyboardChangeState, GameTime gameTime);
    }
}
