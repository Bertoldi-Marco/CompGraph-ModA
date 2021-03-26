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
        private Color targetColor;
        private Point targetPosition;
        private Rectangle targetRect;
        Point targetDim;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
        //screen
        private int xScreenDim;
        private int yScreenDim;
        private Point middleScreen;

        Random rand;

        public HittingGame(Point backgroundStart, Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Color targetColor, int xScreenDim, int yScreenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target)
        {
            rand = new Random();
            targetDim = new Point(10, 10);
            this.backgroundStart = backgroundStart;
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            this.targetColor = targetColor;
            targetPosition = new Point(rand.Next(xScreenDim / 8, xScreenDim - xScreenDim / 8), targetPosition.Y = rand.Next(yScreenDim / 8, yScreenDim - yScreenDim / 8));
            targetRect = new Rectangle(targetPosition, targetDim);
            oldMouse = Mouse.GetState();
            this.xScreenDim = xScreenDim;
            this.yScreenDim = yScreenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            this.target = target;
        }

        public void Update(GameTime gameTime)
        {
            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);

            mouseDiff.X = newMouse.X - middleScreen.X;
            mouseDiff.Y = newMouse.Y - middleScreen.Y;

           // viewSource = Game1_Methods.BackgroundCameraMovement(viewSource, mouseDiff, xScreenDim, yScreenDim, background);

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (targetRect.Contains(middleScreen))
                {
                    targetPosition.X = rand.Next(xScreenDim / 8, xScreenDim - xScreenDim / 8);
                    targetPosition.Y = rand.Next(yScreenDim / 8, yScreenDim - yScreenDim / 8);
                }
            }
            else
            {
                targetPosition.X -= mouseDiff.X;
                targetPosition.Y -= mouseDiff.Y;
            }

            targetRect = new Rectangle(targetPosition, targetDim);
            oldMouse = newMouse;               //this is necessary to store the previous value of left button
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(target, targetRect, Color.White);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
        }
    }

}
