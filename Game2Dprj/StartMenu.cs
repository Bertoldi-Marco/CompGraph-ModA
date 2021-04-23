using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        private Texture2D blackBack;
        private int phase;
        private Song menuSong;
        private SoundEffect onButton;
        private SoundEffect clickButton;        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;

        public StartMenu(Point screenDim,GraphicsDevice graphicsDevice, Texture2D background, Texture2D hitButtonStart, Texture2D trackButtonStart, Texture2D mouseMenuPointer, Song menuSong)
        {
            this.background = background;
            this.screenDim = screenDim;
            this.menuSong = menuSong;
            rand = new Random();
            viewDest = new Rectangle(0, 0, screenDim.X, screenDim.Y);
            viewPos = new Vector2(rand.Next(background.Width - screenDim.X / 2 + 1), rand.Next(background.Height - screenDim.Y / 2 + 1));
            viewSource = new Rectangle((int)viewPos.X, (int)viewPos.Y, screenDim.X, screenDim.Y);
            direction = new Vector2(0, 0);
            RandDirection();
            speed = 100;
            steadyTime = 7; //if changed, change also in the reset part in move background
            color = Color.White;
            opacity = 0;  //by the use of a double variable we do not lost any little decrement
            transpCoeff = 255 / 3;    //variation of .A per second, defined as range/(timeToLive-steadyTime)
            blackBack = new Texture2D(graphicsDevice, 1, 1);
            blackBack.SetData(new[] { Color.White });
            phase = 0;

            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480,270);    //needs to be improved
            hittingRect = new Rectangle(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            trackerRect = new Rectangle(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            hitButton = new Button(hittingRect, hitButtonStart, Color.Cyan);
            trackButton = new Button(trackerRect, trackButtonStart, Color.Cyan);
            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));
            MediaPlayer.Play(menuSong);
            MediaPlayer.IsRepeating = true;
        }

        public void Update(ref SelectMode mode, Point middleScreen, double elapsedSeconds)
        {
            if (MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(menuSong);

            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (hitButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.hittingGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);  //set mouse in the middle before the game is started
                MediaPlayer.Stop();
            }

            if (trackButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.trackerGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);
                MediaPlayer.Stop();
            }
            MoveBackground(elapsedSeconds);
        }

        private void MoveBackground(double elapsedSeconds)
        {
            switch (phase)
            {
                case 0: //increment
                    if (opacity >= 255)
                    {
                        phase = 1;
                        opacity = 255;
                    }
                    else
                        opacity += elapsedSeconds * transpCoeff;
                    break;
                case 1: //steady
                    if (steadyTime <= 0)
                    {
                        steadyTime = 7;
                        phase = 2;
                    } 
                    else
                        steadyTime -= elapsedSeconds;
                    break;
                case 2: //decrement
                    if (opacity <= 0)
                    {
                        phase = 0;
                        opacity = 0;
                        RandDirection();
                        viewPos.X = rand.Next(background.Width - screenDim.X / 2 + 1);
                        viewPos.Y = rand.Next(background.Height - screenDim.Y / 2 + 1);
                    }
                    else
                        opacity -= elapsedSeconds * transpCoeff;
                    break;
                default:
                    break;
            }
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
            viewSource.X = (int)viewPos.X;
            viewSource.Y = (int)viewPos.Y;
            color.A = (byte)opacity;
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
            _spriteBatch.Draw(blackBack, viewDest, Color.Black);
            _spriteBatch.Draw(background, viewDest, viewSource, color);
            hitButton.Draw(_spriteBatch);
            trackButton.Draw(_spriteBatch);
        }
    }
}
