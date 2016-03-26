using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Square_DX.Input
{
    public class KeyboardListener
    {
        private KeyboardChangeState KeyboardCurrentState { get; set; }
        private List<IInputSubscriber> Subscribers { get; set; }
        private List<IInputSubscriber> NewSubscribers { get; set; }
        private double elapsedTime = 0;
        private static readonly int MILLISECONDS_PER_SECOND = 1000;
        private static readonly int UPDATES_PER_SECOND = 70;
        private int timeSpanMilliseconds = MILLISECONDS_PER_SECOND / UPDATES_PER_SECOND;


        public KeyboardListener()
        {
            KeyboardCurrentState = new KeyboardChangeState();
            Subscribers = new List<IInputSubscriber>();
            NewSubscribers = new List<IInputSubscriber>();
        }

        public void Update(KeyboardState currentState, GameTime gameTime)
        {

            Subscribers = NewSubscribers;

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            KeyboardCurrentState.SetState(currentState);
            //TODO: rework listener so that it notifys for more than just changes in keyboard state
            //lots of problems with this way
            if (KeyboardCurrentState.HasChanged() && (int)elapsedTime >= TimeSpan.FromMilliseconds(10).TotalMilliseconds)
            {
                notifySubscribers(gameTime);
            }

            if (elapsedTime > 1000) { elapsedTime = 0; }
        }
        public void AddSubscriber(IInputSubscriber subscriber)
        {
            if(subscriber != null && !Subscribers.Contains(subscriber))
            {
                NewSubscribers.Add(subscriber);
            }
        }
        public void RemoveSubscriber(IInputSubscriber subscriber)
        {
            NewSubscribers.Remove(subscriber);
        } 
        public void notifySubscribers(GameTime gameTime)
        {
            IInputSubscriber[] temp = new IInputSubscriber[Subscribers.Count];
            Subscribers.CopyTo(temp);
            foreach (var subscriber in temp)
            {
                subscriber.NotifyOfChange(KeyboardCurrentState, gameTime);
            }
            
        }
    
    }
}
