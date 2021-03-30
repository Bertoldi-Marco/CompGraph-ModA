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
        private Target target;
        private Point targetDim;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
        private Point mouseSens;
        //screen
        private Point screenDim;
        private Point middleScreen;
        //game status
		Random rand;
        int targetsDestroyed;
        int timeRemaining;        //[ms]        
        int clicks;
        //event
        public event EventHandler<CustomEventArgs> endHittingGame;

        public HittingGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Point screenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target)
        {
            clicks = 0;
            timeRemaining = 10000;
            targetsDestroyed = 0;
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            oldMouse = Mouse.GetState();
            this.screenDim = screenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            mouseDiff = new Point(0, 0);
            mouseSens = new Point(5, 5);            //change this in the menu

            targetDim = new Point(screenDim.X / 20, screenDim.X / 20);
            this.target = new Target(target, targetDim, Color.White, target.Width / 2, background, screenDim, viewSource);            
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

			Game1_Methods.CameraTargetMovement(ref viewSource, ref target.position, mouseDiff, screenDim, background);

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
					clicks++;
                if (target.Contains(middleScreen))                {
                    targetsDestroyed++;
                    target.SpawnMove(screenDim, background, viewSource);
                }
            }
            oldMouse = newMouse;               //this is necessary to store the previous value of left button

        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            target.Draw(_spriteBatch);
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
