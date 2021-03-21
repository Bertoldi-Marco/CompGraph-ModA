using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public partial class Game1
    {
        //-------------------------------Internal variables
        //Background
        private Texture2D background;
        private Point backgroundStart;  
        private Rectangle viewSource;
        private Rectangle viewDest;
        //Cursor
        private Texture2D cursor;
        private Rectangle cursorRect;
        //Target
        private Texture2D target;
        private Color targetColor;
        private Point targetPosition;
        private Rectangle targetRect;
        private float targetScale;
        private float targetSpeed;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
     
        private void TrackerInitLoad()
        {
            IsMouseVisible = false;
            //load contents
            background = Content.Load<Texture2D>("landscape");
            cursor = Content.Load<Texture2D>("cursor");
            target = Content.Load<Texture2D>("sphere");

            backgroundStart = new Point((background.Width-xScreenDim)/2, (background.Height-yScreenDim)/2); //view in the middle of background texture
            viewDest = new Rectangle(0, 0, xScreenDim, yScreenDim);
            viewSource = new Rectangle(backgroundStart.X, backgroundStart.Y, xScreenDim, yScreenDim);
            mouseDiff = new Point(0,0);
            cursorRect = new Rectangle((xScreenDim - cursor.Width) / 2, (yScreenDim - cursor.Height) / 2, cursor.Width, cursor.Height);
            
            
        }

        private void TrackerUpdate()
        {

            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            oldMouse = Mouse.GetState();

            //Camera movement logic
            mouseDiff = newMouse.Position - oldMouse.Position;
            //Saturation on the left                            use a rectangle, useful for the target spawn and logic-difficult to bind with the background
            if (viewSource.X > 0 && mouseDiff.X < 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X < 0)
                    viewSource.X = 0;
            }
            //Saturation on the right
            if (viewSource.X < background.Width-xScreenDim && mouseDiff.X > 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X > background.Width-xScreenDim)
                    viewSource.X = background.Width-xScreenDim;
            }
            //Saturation on the top
            if (viewSource.Y > 0 && mouseDiff.Y < 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y < 0)
                    viewSource.Y = 0;
            }
            //Saturation on the bottom
            if (viewSource.Y < background.Height-yScreenDim && mouseDiff.Y > 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y > background.Height-yScreenDim)
                    viewSource.Y = background.Height-yScreenDim;
            }

            //Target check
            if (targetRect.Contains(middleScreen))
                targetColor = Color.Red;
            else
                targetColor = Color.White;

            //Target logic 

        }

        private void TrackerDraw()
        {
            // needs a universal reference for adjust the scene with fixed distance between the elements
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);

            _spriteBatch.Draw(cursor, cursorRect,Color.White);

        }

    }
}
