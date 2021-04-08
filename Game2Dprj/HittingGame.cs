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
        private int zLimits;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
        private Point mouseSens;
        //Screen
        private Point screenDim;
        private Point middleScreen;

        //Stats
        private const int totalTime = 20;
        int targetsDestroyed;
        int timeRemaining;        //[ms]        
        int clicks;
		int score;
		//Event
        public event EventHandler<HittingGameEventArgs> endHittingGame;

        public HittingGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Point screenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target)
        {
            clicks = 0;
            timeRemaining = totalTime * 1000;
            targetsDestroyed = 0;
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            oldMouse = Mouse.GetState();
            this.screenDim = screenDim;
            this.middleScreen = middleScreen;
			zLimits = 1000;
            this.background = background;
            this.cursor = cursor;
            mouseDiff = new Point(0, 0);
            mouseSens = new Point(5, 5);            //change this in the menu
            this.target = new Target(target, viewSource, new Point(background.Width, background.Height), screenDim, zLimits, target.Width / 2, Color.White);
        }


        public void Update(GameTime gameTime, ref SelectMode mode)
        {
            timeRemaining -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (timeRemaining < 0)
            {
                mode = SelectMode.results;

                score = targetsDestroyed;           //tapullo momentaneo

                HittingGameEnded(new HittingGameEventArgs(targetsDestroyed, clicks, totalTime, score));
            }

            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);

            mouseDiff.X = mouseSens.X * (newMouse.X - middleScreen.X);
            mouseDiff.Y = mouseSens.Y * (newMouse.Y - middleScreen.Y);

            Game1_Methods.CameraMovement(ref viewSource, mouseDiff, screenDim, new Point(background.Width, background.Height));

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                clicks++;
                if (target.Contains(middleScreen))
                {
                    targetsDestroyed++;
                    target.SpawnMove(screenDim);
                }
            }
            oldMouse = newMouse;               //this is necessary to store the previous value of left button
        }

        public void Draw (SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            target.Draw(_spriteBatch, middleScreen, viewSource);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
            _spriteBatch.DrawString(font, "Bersagli presi: " + targetsDestroyed, new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, "Tempo rimasto: " + timeRemaining / 1000, new Vector2(800, 100), Color.Black);
        }

        protected virtual void HittingGameEnded(HittingGameEventArgs e)
        {
            endHittingGame?.Invoke(this, e);
        }
    }
}
