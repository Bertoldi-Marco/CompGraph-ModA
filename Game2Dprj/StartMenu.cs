using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class StartMenu
    {
        private Rectangle hittingRect;
        private Rectangle trackerRect;
        private Point rectDimensions;
        private Button hitButton;
        private Button trackButton;
        private Texture2D background;
        private Point screenDim;
        private Rectangle viewSource;
        private Rectangle viewDest;
        private Vector2 viewPos;
        private Random rand;
        private Vector2 direction;
        private int speed;  //pixel per seconds
        private Color color;
        private double opacity;
        private double transpCoeff;
        private double steadyTime;
       
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;

        public StartMenu(Point screenDim, Texture2D background, Texture2D hitButtonStart, Texture2D trackButtonStart, Texture2D mouseMenuPointer)
        {
            this.background = background;
            this.screenDim = screenDim;
            rand = new Random();
            viewDest = new Rectangle(0, 0, screenDim.X, screenDim.Y);
            viewPos = new Vector2(rand.Next(background.Width - screenDim.X / 2 + 1), rand.Next(background.Height - screenDim.Y / 2 + 1));
            viewSource = new Rectangle((int)viewPos.X, (int)viewPos.Y, screenDim.X, screenDim.Y);
            direction = new Vector2(0, 0);
            RandDirection();
            speed = 100;
            steadyTime = 5; //if changed, change also in the reset part in move background
            color = Color.White;
            opacity = 255;  //by the use of a double variable we do not lost any little decrement
            transpCoeff = 255 / 5;    //variation of .A per second, defined as range/(timeToLive-steadyTime)

            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480,270);    //needs to be improved
            hittingRect = new Rectangle(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            trackerRect = new Rectangle(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            hitButton = new Button(hittingRect, hitButtonStart, Color.Cyan);
            trackButton = new Button(trackerRect, trackButtonStart, Color.Cyan);
            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));
        }

        public void Update(ref SelectMode mode, Point middleScreen, double elapsedSeconds)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (hitButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.hittingGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);  //set mouse in the middle before the game is started
            }

            if (trackButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.trackerGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            }
            MoveBackground(elapsedSeconds);
        }

        private void MoveBackground(double elapsedSeconds)
        {
            if (color.A > 0)
            {
                viewPos.X += (float)(elapsedSeconds * speed * direction.X);     //we use a vector because the rounding of the point lost little variations 
                viewPos.Y += (float)(elapsedSeconds * speed * direction.Y);
                
                //Saturation on the left
                if (viewPos.X <= 0)
                {
                    viewPos.X = 0;
                    direction.X = -direction.X;
                }
                //Saturation on the right
                if (viewPos.X >= background.Width - screenDim.X)
                {
                    viewPos.X = background.Width - screenDim.X;
                    direction.X = -direction.X;
                }
                //Saturation on the top
                if (viewPos.Y <= 0)
                {
                    viewPos.Y = 0;
                    direction.Y = -direction.Y;
                }
                //Saturation on the bottom
                if (viewPos.Y >= background.Height - screenDim.Y)
                {
                    viewPos.Y = background.Height - screenDim.Y;
                    direction.Y = -direction.Y;
                }

                if (steadyTime <= 0)
                    opacity -= elapsedSeconds * transpCoeff;
                else
                    steadyTime -= elapsedSeconds;
            }
            else
            {
                opacity = 255;
                steadyTime = 5;
                //modulus = 1
                RandDirection();
                viewPos.X = rand.Next(background.Width - screenDim.X / 2 + 1);
                viewPos.Y = rand.Next(background.Height - screenDim.Y / 2 + 1);
            }

            color.A = (byte)opacity;
            viewSource.X = (int)viewPos.X;
            viewSource.Y = (int)viewPos.Y;
        }
        private void RandDirection()
        {
            if (rand.Next(2) == 1)
                direction.X = (float)rand.NextDouble();
            else
                direction.X = -(float)rand.NextDouble();
            if (rand.Next(2) == 1)
                direction.Y = (float)Math.Sqrt(1 - Math.Pow(direction.X, 2));
            else
                direction.Y = -(float)Math.Sqrt(1 - Math.Pow(direction.X, 2));
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, color);
            hitButton.Draw(_spriteBatch);
            trackButton.Draw(_spriteBatch);
        }
    }
}
