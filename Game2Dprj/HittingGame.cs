using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class HittingGame
    {
        //-------------------------------Internal variables
        //Background
        private Texture2D background;
        private Rectangle viewSource;
        private Rectangle viewDest;
        //Cursor
        private Rectangle cursorRect;
        private Texture2D cursor;
        //Target
        private Texture2D target;
        private Point targetPosition;
        private Rectangle targetRect;
        Point targetDim;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
        Point mouseSens;
        //screen
        private int xScreenDim;
        private int yScreenDim;
        private Point middleScreen;
        //game status
        Random rand;
        int targetsDestroyed;
        int timeRemaining;        //[ms]
        int clicks;
        //event
        public event EventHandler<CustomEventArgs> endHittingGame;

        public HittingGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, int xScreenDim, int yScreenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target)
        {
            clicks = 0;
            timeRemaining = 10000;
            targetsDestroyed = 0;
            rand = new Random();
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            targetPosition = new Point(rand.Next(xScreenDim / 8, xScreenDim - xScreenDim / 8), rand.Next(yScreenDim / 8, yScreenDim - yScreenDim / 8));
            targetRect = new Rectangle(targetPosition, targetDim);
            oldMouse = Mouse.GetState();
            this.xScreenDim = xScreenDim;
            this.yScreenDim = yScreenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            this.target = target;
            mouseDiff = new Point(0, 0);
            targetDim = new Point(xScreenDim/20, xScreenDim/20);
            mouseSens = new Point(5, 5);            //change this in the menu
        }


        public void Update(GameTime gameTime, ref SelectMode mode)
        {
            timeRemaining -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (timeRemaining < 0)
            {
                mode = SelectMode.results;
                HittingGameEnded(new CustomEventArgs(targetsDestroyed, clicks));
            }

            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);

            mouseDiff.X = mouseSens.X * (newMouse.X - middleScreen.X);
            mouseDiff.Y = mouseSens.Y * (newMouse.Y - middleScreen.Y);

			Game1_Methods.CameraTargetMovement(ref viewSource, ref targetPosition, mouseDiff, xScreenDim, yScreenDim, background);

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                clicks++;
                if (targetRect.Contains(middleScreen))
                {
                    targetsDestroyed++;
                    targetPosition.X = rand.Next(xScreenDim / 8, xScreenDim - xScreenDim / 8);
                    targetPosition.Y = rand.Next(yScreenDim / 8, yScreenDim - yScreenDim / 8);
                }
            }

            targetPosition = Game1_Methods.TargetSaturation(viewSource, targetPosition, targetDim, xScreenDim, yScreenDim, background);

            targetRect = new Rectangle(targetPosition, targetDim);
            oldMouse = newMouse;               //this is necessary to store the previous value of left button

        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            _spriteBatch.Draw(target, targetRect, Color.White);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
            _spriteBatch.DrawString(font, "Bersagli presi: " + targetsDestroyed, new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, "Tempo rimasto: " + timeRemaining/1000, new Vector2(800, 100), Color.Black);
        }

        protected virtual void HittingGameEnded(CustomEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> endHittingEvent = endHittingGame;

            // Event will be null if there are no subscribers
            if (endHittingEvent != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                //e.TargetsDestroyed = targetsDestroyed;
                //e.Clicks = clicks;

                // Call to raise the event.
                endHittingEvent(this, e);
            }
        }
    }
}
