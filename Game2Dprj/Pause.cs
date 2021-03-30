using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class Pause
    {
        private Button resumeButton;
        private Button menuButton;

        private Rectangle resumeRect;
        private Rectangle menuRect;
        private Point rectDimensions;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;

        public Pause (Point screenDim, GraphicsDevice graphicsDevice, Texture2D resumeButtonText, Texture2D menuButtonText, Texture2D mouseMenuPointer)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480, 270);    //needs to be improved
            resumeRect = new Rectangle(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            menuRect = new Rectangle(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);

            resumeButton = new Button(resumeRect, resumeButtonText, Color.Cyan);
            menuButton = new Button(menuRect, menuButtonText, Color.Cyan);

            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));

        }

        public void Update(ref SelectMode mode, SelectMode prevMode, MouseState prevMouse)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (resumeButton.IsPressed(newMouse,oldMouse))
            {
                if(prevMode == SelectMode.hittingGame)
                    mode = SelectMode.hittingGame;
                else
                    mode = SelectMode.trackerGame;
                Mouse.SetPosition(prevMouse.X, prevMouse.Y);  //set mouse where it was when 'p' was pressed
            }

            if (menuButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.menu;
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            resumeButton.Draw(_spriteBatch);
            menuButton.Draw(_spriteBatch);
            //_spriteBatch.DrawString(font, "Resume", new Vector2(resumeButton.rectangle.X + resumeButton.rectangle.Width / 2, resumeButton.rectangle.Y + resumeButton.rectangle.Height / 2), Color.Black);  //how to center respect to the string length?
            //_spriteBatch.DrawString(font, "Main Menu", new Vector2(menuButton.rectangle.X + menuButton.rectangle.Width / 2, menuButton.rectangle.Y + menuButton.rectangle.Height / 2), Color.Black);
        }
    }
}
