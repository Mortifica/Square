using Microsoft.Xna.Framework.Input;

namespace Square_DX.Input
{
    public class KeyboardChangeState
    {
        public KeyboardState PreviousState { get; private set; }
        public KeyboardState CurrentState { get; private set; }

        public void SetState(KeyboardState keyboardState)
        {
            PreviousState = CurrentState;
            CurrentState = keyboardState;
        }

        public bool HasChanged()
        {
            if (!PreviousState.GetPressedKeys().Equals(CurrentState.GetPressedKeys()))
            {
                return true;
            }
            return false;
        }
    }
}
