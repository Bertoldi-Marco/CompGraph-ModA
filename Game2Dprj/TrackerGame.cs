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

        //From Game1
        private Point screenDim;
        private Point middleScreen;

        //Background
        private Texture2D background;
        private Rectangle viewSource;
        private Rectangle viewDest;
        //private Rectangle boundariesRect;

        //Cursor
        private Texture2D cursor;
        private Rectangle cursorRect;

        //Target
        private int modulusSpeed;
        private int zLimits;
        private Target target;

        //Mouse
        private MouseState newMouse;
        private Point mouseDiff;

        //Time
        private double elapsedTime;
        private double totalElapsedTime;
        private double timeRemaining;        //[s]
        private double timeOn;

        //Font
        private SpriteFont font;

        //Stats
        private double precision;
        private const double gameTotalTime = 60;
        private int score;

        //Event
        public event EventHandler<TrackerGameEventArgs> endTrackerGame;

        public TrackerGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Point screenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target, SpriteFont font)
        {
            //Initiate variables
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            this.screenDim = screenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            this.font = font;
            zLimits = 1000;
            precision = 100;
            timeOn = 0;
            timeRemaining = gameTotalTime;
            mouseDiff = new Point(0,0);
            modulusSpeed = 400;
            this.target = new Target(target, viewSource, new Point(background.Width, background.Height), screenDim, zLimits, target.Width / 2, modulusSpeed, Color.White);
        }

        public void Update(GameTime gameTime,ref SelectMode mode)
        {
            elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            totalElapsedTime = gameTime.TotalGameTime.TotalSeconds;
            timeRemaining -= elapsedTime;

            if (timeRemaining < 0)
            {
                mode = SelectMode.results;
                score = (int)precision;
                endTrackerGame?.Invoke(this, new TrackerGameEventArgs(precision, score));
            }

            //Camera movements
            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            mouseDiff.X = newMouse.X - middleScreen.X;
            mouseDiff.Y = newMouse.Y - middleScreen.Y;

            //Update background position in relation to mouse movement
            Game1_Methods.CameraMovement(ref viewSource, mouseDiff, screenDim, new Point(background.Width, background.Height)); 
            
            //Target check
            if (target.Contains(middleScreen))
            {
                if (target.color == Color.Red)
                    timeOn += elapsedTime;
                target.color = Color.Red;
            }
            else
            {
                if(target.color == Color.Red)
                    timeOn += elapsedTime;
                target.color = Color.White;
            }
                            //Target movement logic
            target.ContinuousMove(elapsedTime, totalElapsedTime);

            //Stats
            precision = (timeOn / (gameTotalTime - timeRemaining)) * 100;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            target.Draw(_spriteBatch, middleScreen, viewSource);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
            _spriteBatch.DrawString(font, "Precisione: " + Math.Round(precision,2) +"%", new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, "Tempo rimasto: " + Math.Round(timeRemaining, 0), new Vector2(800, 100), Color.Black);
        }

    }
}
