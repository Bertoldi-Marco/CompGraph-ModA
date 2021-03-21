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
        private Point backgroundOffset;
        private Rectangle viewSource;
        private Rectangle viewDest;
        //Aim
        private Texture2D aim;
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
            //load contents
            background = Content.Load<Texture2D>("landscape");

            backgroundStart = new Point((background.Width-viewSource.Width)/2, (background.Height-viewSource.Width)/2); //view in the middle of background texture
            backgroundOffset = new Point(0, 0);
            viewDest = new Rectangle(0, 0, xScreenDim, yScreenDim);
            viewSource = new Rectangle(backgroundStart.X, backgroundStart.Y, xScreenDim, yScreenDim);
            oldMouse = Mouse.GetState();
            mouseDiff = new Point(0,0);

            
        }

        private void TrackerUpdate()
        {

            newMouse = Mouse.GetState();
            Mouse.SetPosition(xScreenDim / 2, yScreenDim / 2);

            //Camera movement logic------------NOT WORKING
            mouseDiff = oldMouse.Position - newMouse.Position;
            //Saturation on the left
            if (viewSource.X > 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X < 0)
                    viewSource.X = 0;
            }
            //Saturation on the right
            if (viewSource.X < background.Width)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X > background.Width)
                    viewSource.X = background.Width;
            }
            //Saturation on the top
            if (viewSource.Y > 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y < 0)
                    viewSource.Y = 0;
            }
            //Saturation on the bottom
            if (viewSource.Y < background.Height)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y > background.Height)
                    viewSource.Y = background.Height;
            }

            //Target check
            if (targetRect.Contains(xScreenDim / 2, yScreenDim / 2))
                targetColor = Color.Red;
            else
                targetColor = Color.White;

            //Target logic 

        }

        private void TrackerDraw()
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
        }

    }
}
