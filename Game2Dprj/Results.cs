using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class Results
    {
        private Button quitButton;
        private Button menuButton;

        private Rectangle quitRect;
        private Rectangle menuRect;
        private Point rectDimensions;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        //Statistics
        int targetsDestroyed;
        int clicks;
        float accuracy;
        float avgTimeToKill;


        public Results(Point screenDim, GraphicsDevice graphicsDevice, Texture2D quitButtonText, Texture2D menuButtonText, Texture2D mouseMenuPointer, HittingGame hittingGame)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480, 270);    //needs to be improved
            quitRect = new Rectangle(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            menuRect = new Rectangle(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);

            quitButton = new Button(quitRect, quitButtonText, Color.Cyan);
            menuButton = new Button(menuRect, menuButtonText, Color.Cyan);

            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));
            hittingGame.endHittingGame += endHittingGameHandler;
        }

        public void Update(ref SelectMode mode, MouseState prevMouse, Game1 game)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (menuButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.menu;
            }

            if (quitButton.IsPressed(newMouse, oldMouse))
            {
                game.Exit();
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            quitButton.Draw(_spriteBatch);
            menuButton.Draw(_spriteBatch);
            //_spriteBatch.DrawString(font, "Resume", new Vector2(resumeButton.rectangle.X + resumeButton.rectangle.Width / 2, resumeButton.rectangle.Y + resumeButton.rectangle.Height / 2), Color.Black);  //how to center respect to the string length?
            //_spriteBatch.DrawString(font, "Main Menu", new Vector2(menuButton.rectangle.X + menuButton.rectangle.Width / 2, menuButton.rectangle.Y + menuButton.rectangle.Height / 2), Color.Black);

            _spriteBatch.DrawString(font, "accuracy: " + (int)accuracy + "%", new Vector2(600, 700), Color.Black);  //how to center respect to the string length?
            _spriteBatch.DrawString(font, "averageTimeToKill" + avgTimeToKill + "sec", new Vector2(1200, 700), Color.Black);
            _spriteBatch.DrawString(font, "kills" + targetsDestroyed , new Vector2(1200, 900), Color.Black);
            _spriteBatch.DrawString(font, "clicks" + clicks, new Vector2(600, 900), Color.Black);
        }

        void endHittingGameHandler(object sender, CustomEventArgs e)            //this handler could be edited to be the handler of both games,using typeof sender object to determine which game is ended
        {
            targetsDestroyed = e.TargetsDestroyed;
            clicks = e.Clicks;
            accuracy = 100*((float)targetsDestroyed / (float)clicks);
            avgTimeToKill = 60 / targetsDestroyed;
        }
    }
}
