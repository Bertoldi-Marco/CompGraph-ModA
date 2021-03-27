using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//BUG ANNOTATION:
//-launch from menu --> mouse not centered
namespace Game2Dprj
{
    public partial class Game1
    {
        //-------------------------------Internal variables
        //First Update bool
        private bool drawn;
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
        private int squareTargetModulusSpeed;
        private Vector2 targetActualSpeed;
        private Vector2 targetPos;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
        private Point effectiveDiff;
        //Random
        Random rand;
        //Time
        private double elapsedTime;
        private double totalElapsedTimePrev;
        private double totalElapsedTimeAct;
        private double totalElapsedTimeDiff;
        private double timeToSpeedChange;

        private void TrackerInitLoad()
        {
            drawn = false;

            //Load contents
            background = Content.Load<Texture2D>("landscape");
            cursor = Content.Load<Texture2D>("cursor");
            target = Content.Load<Texture2D>("sphere");

            //Initiate variables
            backgroundStart = new Point((background.Width-xScreenDim)/2, (background.Height-yScreenDim)/2); //view in the middle of background texture
            viewDest = new Rectangle(0, 0, xScreenDim, yScreenDim);
            viewSource = new Rectangle(backgroundStart.X, backgroundStart.Y, xScreenDim, yScreenDim);
            mouseDiff = new Point(0,0);
            effectiveDiff = new Point(0,0);
            cursorRect = new Rectangle((xScreenDim - cursor.Width) / 2, (yScreenDim - cursor.Height) / 2, cursor.Width, cursor.Height);
            targetRect = new Rectangle((xScreenDim - target.Width) / 2, (yScreenDim - target.Height) / 2, target.Width, target.Height);
            squareTargetModulusSpeed = (int)Math.Pow(200,2);  
            totalElapsedTimeAct = 0;
            totalElapsedTimePrev = 0;
            targetPos = new Vector2((xScreenDim - target.Width) / 2, (yScreenDim - target.Height) / 2);
            rand = new Random();

        }

        private void TrackerUpdate(GameTime gameTime)
        {
            elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            totalElapsedTimeAct = gameTime.TotalGameTime.TotalSeconds;
            totalElapsedTimeDiff = totalElapsedTimeAct - totalElapsedTimePrev;

            //Camera movements
            if (!drawn)  //protect from movements since in the first call the mouse is not in the center (monogame window not opened yet)
            {
                newMouse = new MouseState(middleScreen.X, middleScreen.Y, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
                oldMouse = newMouse;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            }
            else
                newMouse = Mouse.GetState();

            Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            mouseDiff.X = newMouse.X - middleScreen.X;
            mouseDiff.Y = newMouse.Y - middleScreen.Y;

            effectiveDiff.X = -viewSource.X;
            effectiveDiff.Y = -viewSource.Y;

            //Update background position
            viewSource = Game1_Methods.CameraMovement(viewSource, mouseDiff, xScreenDim, yScreenDim, background); //get the movement according to the background limits
            
            effectiveDiff.X += viewSource.X;    //effectiveDiff = newSource-oldSource
            effectiveDiff.Y += viewSource.Y;
           
            ////Update background position
            //viewSource.X += effectiveDiff.X;
            //viewSource.Y += effectiveDiff.Y;
            //used effectiveDiff(point) that store viewSourceActual-viewSourcePrevious

            //Update target position
            targetPos.X -= effectiveDiff.X;
            targetPos.Y -= effectiveDiff.Y;
            targetRect.X = (int)(targetPos.X);  //useful to change the color, needs to be improved
            targetRect.Y = (int)(targetPos.Y);

            //Target check
            if (targetRect.Contains(middleScreen))
                targetColor = Color.Red;
            else
                targetColor = Color.White;

            //---------Target movement logic
            if(timeToSpeedChange <= totalElapsedTimeDiff)
            {
                if (1 == rand.Next(2))
                    targetActualSpeed.X = (float)Math.Sqrt(rand.Next(100) * squareTargetModulusSpeed / 100);    //formulas to be double checked, actually speed modulus change
                else
                    targetActualSpeed.X = -(float)Math.Sqrt(rand.Next(100) * squareTargetModulusSpeed / 100);
                if (1 == rand.Next(2))
                    targetActualSpeed.Y = (float)Math.Sqrt(squareTargetModulusSpeed - Math.Pow(targetActualSpeed.X, 2));
                else
                    targetActualSpeed.Y = -(float)Math.Sqrt(squareTargetModulusSpeed - Math.Pow(targetActualSpeed.X, 2));
                totalElapsedTimePrev = totalElapsedTimeAct;
                timeToSpeedChange = rand.NextDouble() * 2;  //change after minimum 0.5s maximum 3s;
            }
            //Update target position in relation to speed
            targetPos.X += (float)(targetActualSpeed.X * elapsedTime);
            targetPos.Y += (float)(targetActualSpeed.Y * elapsedTime);


            oldMouse = newMouse;         
        }

        private void TrackerDraw()
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            _spriteBatch.Draw(target, targetPos, targetColor);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);

            if (!drawn) //monogame window available
                drawn = true;   
        }

    }
}
