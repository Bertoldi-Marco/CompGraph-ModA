using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class TrackerGame
    {
        //-------------------------------Internal variables
        //First Update bool
        //private bool drawn;
        //From Game1
        private int xScreenDim;
        private int yScreenDim;
        private Point middleScreen;
        //Background
        private Texture2D background;
        private Rectangle viewSource;
        private Rectangle viewDest;
        private Rectangle boundariesRect;
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
        private Point mouseDiff;
        //Random
        Random rand;
        //Time
        private double elapsedTime;
        private double totalElapsedTimePrev;
        private double totalElapsedTimeAct;
        private double totalElapsedTimeDiff;
        private double timeToSpeedChange;
        //Font
        private SpriteFont font;
        public TrackerGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, int xScreenDim, int yScreenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target, SpriteFont font)
        {
            //drawn = false;

            //Initiate variables
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            this.xScreenDim = xScreenDim;
            this.yScreenDim = yScreenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            this.target = target;
            this.font = font;

            boundariesRect = new Rectangle((2 * xScreenDim - background.Width) / 2, (2 * yScreenDim - background.Height) / 2, background.Width - xScreenDim, background.Height - yScreenDim);
            mouseDiff = new Point(0,0);
            targetRect = new Rectangle((xScreenDim - target.Width) / 2, (yScreenDim - target.Height) / 2, target.Width, target.Height);
            squareTargetModulusSpeed = (int)Math.Pow(300,2);  
            totalElapsedTimeAct = 0;
            totalElapsedTimePrev = 0;
            targetPos = new Vector2((xScreenDim - target.Width) / 2, (yScreenDim - target.Height) / 2);
            rand = new Random();

        }

        public void Update(GameTime gameTime)
        {
            elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            totalElapsedTimeAct = gameTime.TotalGameTime.TotalSeconds;
            totalElapsedTimeDiff = totalElapsedTimeAct - totalElapsedTimePrev;

            //Camera movements
            //#Protection not needed while menu is used    //if (!drawn)  //protect from movements since in the first call the mouse is not in the center (monogame window not opened yet)
                                                            //{
                                                            //    newMouse = new MouseState(middleScreen.X, middleScreen.Y, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
                                                            //    oldMouse = newMouse;
                                                            //}
                                                            //else
            newMouse = Mouse.GetState();

            Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            mouseDiff.X = newMouse.X - middleScreen.X;
            mouseDiff.Y = newMouse.Y - middleScreen.Y;

            //Update background and target position in relation to mouse movement
            Game1_Methods.CameraTargetMovement(ref viewSource, ref targetPos, ref boundariesRect, mouseDiff, xScreenDim, yScreenDim, background); 
            
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
                timeToSpeedChange = rand.NextDouble() * 2;
            }

            Game1_Methods.TargetMovement(ref targetRect, ref targetPos, ref targetActualSpeed, boundariesRect, elapsedTime);

            //Update target rectangle position
            targetRect.X = (int)(targetPos.X);  //useful to change the color, needs to be improved
            targetRect.Y = (int)(targetPos.Y);         
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            _spriteBatch.Draw(target, targetPos, targetColor);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);

            //#Protection component
            //if (!drawn) //monogame window available
            //    drawn = true;   
        }

    }
}
