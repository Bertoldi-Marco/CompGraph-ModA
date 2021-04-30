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
    public class Pause
    {
        private Button resumeButton;
        private Button menuButton;
        private Button exitButton;

        private Rectangle resumeRect;
        private Rectangle menuRect;
        private Point rectDimensions;
        private Texture2D screenFreezed;
        private int[] backBuffer;
        private Slider volumeSlide;
        private Slider sensSlide;
        private int mouseScale;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;

        public Pause (Point screenDim, GraphicsDevice graphicsDevice, Texture2D resumeButtonText, Texture2D menuButtonText, Texture2D exit, Texture2D mouseMenuPointer, Texture2D knobText, Texture2D slideText, SpriteFont font, float mouseSens, float volume, SoundEffect onButton, SoundEffect clickButton)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(menuButtonText.Width, menuButtonText.Height);
            resumeRect = new Rectangle(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            menuRect = new Rectangle(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            backBuffer = new int[screenDim.X * screenDim.Y];
            mouseScale = 10;    //max reachable value, for the volume is 1
            volumeSlide = new Slider(new Point(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 2 + rectDimensions.Y), volume, slideText, knobText, font, "Volume: ", Color.White);
            sensSlide = new Slider(new Point(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 2 + rectDimensions.Y), mouseSens/mouseScale, slideText, knobText, font, "Mouse sensibility: ", Color.White);

            exitButton = new Button(new Rectangle(screenDim.X - exit.Width / 4, 0, exit.Width / 4, exit.Height / 4), exit, Color.White, onButton, clickButton);
            resumeButton = new Button(resumeRect, resumeButtonText, Color.White, onButton, clickButton);
            menuButton = new Button(menuRect, menuButtonText, Color.White, onButton, clickButton);

            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));
        }

        public void Update(ref SelectMode mode, ref float mouseSens, ref float volume,  SelectMode prevMode, MouseState prevMouse, Game1 game)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            volume = (float)(volumeSlide.Update(newMouse, volume));
            mouseSens = (mouseScale * sensSlide.Update(newMouse, mouseSens/mouseScale));

            if (resumeButton.IsPressed(newMouse,oldMouse, volume))
            {
                if(prevMode == SelectMode.hittingGame)
                    mode = SelectMode.hittingGame;
                else
                    mode = SelectMode.trackerGame;
                Mouse.SetPosition(prevMouse.X, prevMouse.Y);  //set mouse where it was when 'p' was pressed
                MediaPlayer.Resume();           //resume game song
            }

            if (menuButton.IsPressed(newMouse, oldMouse, volume))
            {
                mode = SelectMode.menu;
                MediaPlayer.Stop();             //stop game song to start menu song
            }

            if (exitButton.IsPressed(newMouse, oldMouse, volume))
            {
                game.CheckExit();   //mediaplayer.stop() is necessary??
            }
        }

        public void FreezeScreen(GraphicsDevice graphicsDevice, Point screenDim)
        {
            graphicsDevice.GetBackBufferData(backBuffer);
            screenFreezed = new Texture2D(graphicsDevice, screenDim.X, screenDim.Y, false, graphicsDevice.PresentationParameters.BackBufferFormat);
            screenFreezed.SetData(backBuffer);
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(screenFreezed, new Vector2(0, 0), Color.Gray);
            resumeButton.Draw(_spriteBatch);
            menuButton.Draw(_spriteBatch);
            exitButton.Draw(_spriteBatch);
            sensSlide.Draw(_spriteBatch);
            volumeSlide.Draw(_spriteBatch);
            //_spriteBatch.DrawString(font, "Resume", new Vector2(resumeButton.rectangle.X + resumeButton.rectangle.Width / 2, resumeButton.rectangle.Y + resumeButton.rectangle.Height / 2), Color.Black);  //how to center respect to the string length?
            //_spriteBatch.DrawString(font, "Main Menu", new Vector2(menuButton.rectangle.X + menuButton.rectangle.Width / 2, menuButton.rectangle.Y + menuButton.rectangle.Height / 2), Color.Black);
        }
    }
}
