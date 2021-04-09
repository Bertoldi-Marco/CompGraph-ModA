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
        //Statistics Hitting
        int targetsDestroyed;
        int clicks;
        int score;
        float avgTimeToKill;
        //statitstics tracker
        float accuracy;
        //statistics Graphics
        Point graphicPos;

        //Graphics variables
        Texture2D freccia;
        Texture2D pentagono;
        Texture2D triangolo;
        SpriteFont font;

        Statistics statistics;

        public Results(Point screenDim, GraphicsDevice graphicsDevice, Texture2D quitButtonText, Texture2D menuButtonText, Texture2D mouseMenuPointer, HittingGame hittingGame, TrackerGame trackerGame, Texture2D freccia, Texture2D pentagono, Texture2D triangolo, SpriteFont font)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480, 270);    //needs to be improved
            quitRect = new Rectangle(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 4 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            menuRect = new Rectangle(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 4 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            graphicPos = new Point(screenDim.X / 2 - pentagono.Width / 2, (int)(screenDim.Y / 2.5f));

            quitButton = new Button(quitRect, quitButtonText, Color.Cyan);
            menuButton = new Button(menuRect, menuButtonText, Color.Cyan);

            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));
            hittingGame.endHittingGame += endHittingGameHandler;
            trackerGame.endTrackerGame += endTrackerGameHandler;
            this.freccia = freccia;
            this.pentagono = pentagono;
            this.triangolo = triangolo;
            this.font = font;
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

            if (statistics != null)
            {
                statistics.Update();
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            quitButton.Draw(_spriteBatch);
            menuButton.Draw(_spriteBatch);
            //_spriteBatch.DrawString(font, "Resume", new Vector2(resumeButton.rectangle.X + resumeButton.rectangle.Width / 2, resumeButton.rectangle.Y + resumeButton.rectangle.Height / 2), Color.Black);  //how to center respect to the string length?
            //_spriteBatch.DrawString(font, "Main Menu", new Vector2(menuButton.rectangle.X + menuButton.rectangle.Width / 2, menuButton.rectangle.Y + menuButton.rectangle.Height / 2), Color.Black);

            if (statistics!=null)
            {
                statistics.Draw(_spriteBatch);
            }
        }

        void endHittingGameHandler(object sender, HittingGameEventArgs e)            //this handler could be edited to be the handler of both games,using typeof sender object to determine which game is ended
        {
            targetsDestroyed = e.TargetsDestroyed;
            clicks = e.Clicks;
            score = e.Score;
            if (clicks != 0) 
            {
                accuracy = (100 * ((float)targetsDestroyed / clicks));
            }
            else
            {
                accuracy = -1;
            }
            if (targetsDestroyed != 0) 
            {
                //avgTimeToKill = string.Format("{0:0.00}", (float)e.TotalTime / targetsDestroyed);            //2 decimal
                avgTimeToKill = (float)e.TotalTime / targetsDestroyed;
            }
            else
            {
                avgTimeToKill = -1;
            }
            statistics = new Pentagon(pentagono, graphicPos, score, targetsDestroyed, avgTimeToKill, 1, accuracy, 0.8f, freccia, font);
        }

        void endTrackerGameHandler(object sender, TrackerGameEventArgs e)            //this handler could be edited to be the handler of both games,using typeof sender object to determine which game is ended
        {
            accuracy = ((float)Math.Round(e.Accuracy,2));
            score = e.Score;
            statistics = new Triangle(triangolo, graphicPos, score, 1, accuracy, 0.8f, freccia, font);
        }

    }
}
