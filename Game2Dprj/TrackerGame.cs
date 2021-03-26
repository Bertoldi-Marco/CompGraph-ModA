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
        //First Update bool
        private bool first;
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
        private Rectangle targetRect;
        private float targetScale;
        private float targetSpeed;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;        //could be replaced with a fixed reference to middle screen
        private Point mouseDiff;
        private Point effectiveDiff;
     
        private void TrackerInitLoad()
        {
            IsMouseVisible = false;
            first = true;
            //Load contents
            background = Content.Load<Texture2D>("landscape");
            cursor = Content.Load<Texture2D>("cursor");
            target = Content.Load<Texture2D>("sphere");

            backgroundStart = new Point((background.Width-xScreenDim)/2, (background.Height-yScreenDim)/2); //view in the middle of background texture
            viewDest = new Rectangle(0, 0, xScreenDim, yScreenDim);
            viewSource = new Rectangle(backgroundStart.X, backgroundStart.Y, xScreenDim, yScreenDim);
            mouseDiff = new Point(0,0);
            effectiveDiff = new Point(0,0);
            cursorRect = new Rectangle((xScreenDim - cursor.Width) / 2, (yScreenDim - cursor.Height) / 2, cursor.Width, cursor.Height);
            targetRect = new Rectangle((xScreenDim - target.Width) / 2, (yScreenDim - target.Height) / 2, target.Width, target.Height);
        }

        private void TrackerUpdate()
        {
            //Camera movements
            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);

            mouseDiff.X = newMouse.X - middleScreen.X;
            mouseDiff.Y = newMouse.Y - middleScreen.Y;
            if (first)  //protect from movements since in the first call the mouse is not in the center (why?...)
            {
                first = false;
                newMouse = new MouseState(middleScreen.X, middleScreen.Y, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
                oldMouse = newMouse;
            }
            
            effectiveDiff = Game1_Methods.CameraMovement(viewSource, mouseDiff, xScreenDim, yScreenDim, background); //get the movement according to the background limits
            


            //Update background
            viewSource.X += effectiveDiff.X;
            viewSource.Y += effectiveDiff.Y;

            //Update target
            targetRect.X -= effectiveDiff.X;
            targetRect.Y -= effectiveDiff.Y;

            //Target check
            if (targetRect.Contains(middleScreen))
                targetColor = Color.Red;
            else
                targetColor = Color.White;

            //Target logic 
            newMouse = Mouse.GetState();
        }

        private void TrackerDraw()
        {
            // needs a universal reference to adjust the scene with fixed distance between the elements
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            _spriteBatch.Draw(target, targetRect, targetColor);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);

        }

    }
}
