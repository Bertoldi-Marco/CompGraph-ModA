using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game2Dprj
{
    public class StartMenu
    {
        private Rectangle hittingRect;
        private Rectangle trackerRect;
        private Rectangle helpButtonRect;
        private Button hitButton;
        private Button trackButton;
        private Button helpButton;
        private Button exitButton;
        private Texture2D background;
        private Texture2D board;
        private Rectangle boardRect;
        private Texture2D help_info;
        private Rectangle help_info_Rect;
        private bool help_info_on;
        private Texture2D title;
        private Rectangle titleRect;
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
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        //Slider
        private Slider volumeSlide;

        public StartMenu(Point screenDim, GraphicsDevice graphicsDevice, Texture2D background, Texture2D hitButtonStart, Texture2D trackButtonStart, Texture2D mouseMenuPointer, Texture2D knobText, Texture2D slideText, SpriteFont font, float volume, Song menuSong, SoundEffect onButton, SoundEffect clickButton, Texture2D help, Texture2D help_info, Texture2D title, Texture2D board, Texture2D exit)
        {
            this.background = background;
            this.screenDim = screenDim;
            this.menuSong = menuSong;
            this.help_info = help_info;
            this.board = board;
            help_info_on = false;
            this.title = title;
            rand = new Random();
            help_info_Rect = new Rectangle(-150, 250, help_info.Width, help_info.Height);
            titleRect = new Rectangle(0, 0, title.Width, title.Height);
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
            boardRect = new Rectangle(screenDim.X - board.Width, (screenDim.Y - board.Height) / 2, board.Width, board.Height);

            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            hittingRect = new Rectangle(boardRect.X + 143, boardRect.Y + 174, hitButtonStart.Width, hitButtonStart.Height);
            trackerRect = new Rectangle(boardRect.X + 143, boardRect.Y + 372, trackButtonStart.Width, trackButtonStart.Height);
            helpButtonRect = new Rectangle(50, screenDim.Y - 3 * help.Height / 4 - 20, 3 * help.Width / 4, 3 * help.Height / 4);
            hitButton = new Button(hittingRect, hitButtonStart, Color.White, onButton, clickButton);
            trackButton = new Button(trackerRect, trackButtonStart, Color.White, onButton, clickButton);
            helpButton = new Button(helpButtonRect, help, Color.White, onButton, clickButton);
            exitButton = new Button(new Rectangle(screenDim.X - exit.Width / 4, 0, exit.Width / 4, exit.Height / 4), exit, Color.White, onButton, clickButton);
            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));
            MediaPlayer.Play(menuSong);
            MediaPlayer.IsRepeating = true;

            volumeSlide = new Slider(new Point(screenDim.X - 320 - slideText.Width/2, boardRect.Y + 592), volume, slideText, knobText, font, "Volume: ", Color.Black);
        }

        public void Update(ref SelectMode mode, Point middleScreen, double elapsedSeconds, ref float volume)
        {
            volume = (float)(volumeSlide.Update(newMouse, volume));

            MediaPlayer.Volume = volume;
            if (MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(menuSong);

            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if(helpButton.IsPressed(newMouse, oldMouse, volume))
            {
                help_info_on = !help_info_on;               //activate or deactivate help info
            }

            if (hitButton.IsPressed(newMouse, oldMouse, volume))
            {
                mode = SelectMode.hittingGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);  //set mouse in the middle before the game is started
                MediaPlayer.Stop();
            }

            if (trackButton.IsPressed(newMouse, oldMouse, volume))
            {
                mode = SelectMode.trackerGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);
                MediaPlayer.Stop();
            }

            if (exitButton.IsPressed(newMouse, oldMouse, volume))
            {
                mode = SelectMode.exiting;          //mediaplayer.stop() is necessary??
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
            _spriteBatch.Draw(title, titleRect, Color.White);
            helpButton.Draw(_spriteBatch);
            _spriteBatch.Draw(board, boardRect, Color.White);
            hitButton.Draw(_spriteBatch);
            exitButton.Draw(_spriteBatch);
            trackButton.Draw(_spriteBatch);
            volumeSlide.Draw(_spriteBatch);

            if (help_info_on)
                _spriteBatch.Draw(help_info, help_info_Rect, Color.White);
        }
    }
}
