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
        private Point backgroundStart;
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
        int point;
        int timeRemaining;        //[ms]

        public HittingGame(Point backgroundStart, Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, int xScreenDim, int yScreenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target)
        {
            timeRemaining = 60000;
            point = 0;
            rand = new Random();
            this.backgroundStart = backgroundStart;
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

        public void Update(GameTime gameTime)
        {
            timeRemaining -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (timeRemaining < 0)
                timeRemaining = 0;

            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);

            mouseDiff.X = mouseSens.X * (newMouse.X - middleScreen.X);
            mouseDiff.Y = mouseSens.Y * (newMouse.Y - middleScreen.Y);

			Game1_Methods.CameraTargetMovement(ref viewSource, mouseDiff, ref targetPosition, xScreenDim, yScreenDim, background);

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (targetRect.Contains(middleScreen))
                {
                    point++;
                    targetPosition.X = rand.Next(xScreenDim / 8, xScreenDim - xScreenDim / 8);
                    targetPosition.Y = rand.Next(yScreenDim / 8, yScreenDim - yScreenDim / 8);
                }
            }

            targetPosition = Game1_Methods.TargetSaturation(viewSource, targetPosition, targetDim, xScreenDim, yScreenDim, background);

            targetRect = new Rectangle(targetPosition, targetDim);
            oldMouse = newMouse;               //this is necessary to store the previous value of left button

        }

        public void Draw(GameTime gameTime,SpriteBatch _spriteBatch,SpriteFont font)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            _spriteBatch.Draw(target, targetRect, Color.White);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
            _spriteBatch.DrawString(font, "Bersagli presi: " + point, new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, "Tempo rimasto: " + timeRemaining/1000, new Vector2(800, 100), Color.Black);
        }
    }

}
