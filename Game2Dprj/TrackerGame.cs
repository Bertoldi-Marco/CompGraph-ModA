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
    public class TrackerGame
    {
        //-------------------------------Internal variables

        //From Game1
        private Point screenDim;
        private Point middleScreen;

        //Background
        private Texture2D background;
        private Texture2D backgroundStatsInGame;
        private Rectangle viewSource;
        private Rectangle viewDest;
        private Rectangle oldViewSource;
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
        public MouseState oldMouse;
        private Point mouseDiff;
        private Point effectiveDiff;

        //Time
        private double elapsedTime;
        private double totalElapsedTime;
        private double timeRemaining;        //[s]
        private double timeOn;

        //Font
        private SpriteFont font;

        //Stats
        private double precision;
        private double avgTimeOn;
        private int numberOfTimesOn;
        private const double gameTotalTime = 50;
        private int score;

        //Start Game
        private bool go;
        private Button goButton;
        private Rectangle goButtonRectangle;

        //Event
        public event EventHandler<TrackerGameEventArgs> endTrackerGame;

        //Sound
        private SoundEffectInstance ticking;
        private Song gameSong;

        public TrackerGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Point screenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target, SpriteFont font, Texture2D sphereAtlas, Texture2D explosionAtlas, SoundEffect tick, Texture2D goText, SoundEffect onButton, SoundEffect clickButton, Song gameSong, Texture2D backgroundStatsInGame)
        {
            //Initiate variables
            goButtonRectangle = new Rectangle(middleScreen.X - goText.Width / 2, middleScreen.Y - goText.Height / 2, goText.Width, goText.Height);
            goButton = new Button(goButtonRectangle, goText, Color.White, onButton, clickButton);
            go = false;
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            this.screenDim = screenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            this.font = font;
            effectiveDiff = new Point(viewSource.X, viewSource.Y);
            oldViewSource = viewSource;
            oldMouse = Mouse.GetState();
            zLimits = 1000;
            avgTimeOn = 0;
            numberOfTimesOn = 0;
            precision = 100;
            timeOn = 0;
            timeRemaining = gameTotalTime;
            mouseDiff = new Point(0, 0);
            modulusSpeed = 300;
            this.target = new Target(target, viewSource, new Point(background.Width, background.Height), screenDim, zLimits, target.Width / 2, modulusSpeed, Color.White, sphereAtlas, explosionAtlas);
            ticking = tick.CreateInstance();
            ticking.IsLooped = true;
            this.gameSong = gameSong;
            this.backgroundStatsInGame = backgroundStatsInGame;
        }

        public void Update(GameTime gameTime,ref SelectMode mode, float mouseSens, float volume)
        {
            //Camera movements
            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            mouseDiff.X = (int)Math.Round((newMouse.X - middleScreen.X) * mouseSens);
            mouseDiff.Y = (int)Math.Round((newMouse.Y - middleScreen.Y) * mouseSens);

            //Update background position in relation to mouse movement
            Game1_Methods.CameraMovement(ref viewSource, mouseDiff, screenDim, new Point(background.Width, background.Height));

            if (go)
            {
                elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
                totalElapsedTime = gameTime.TotalGameTime.TotalSeconds;
                timeRemaining -= elapsedTime;
                ticking.Volume = volume * 0.75f;

                MediaPlayer.Volume = volume;
                if (MediaPlayer.State == MediaState.Stopped)
                    MediaPlayer.Play(gameSong);

                //Target check
                if (target.Contains(middleScreen))
                {
                    if (target.color == Color.Yellow)
                        timeOn += elapsedTime;
                    else
                        numberOfTimesOn++;
                    target.color = Color.Yellow;
   					if (ticking.State != SoundState.Playing)
                    	ticking.Play();
                }
                else
			    {
                    if(target.color == Color.Yellow)
                        timeOn += elapsedTime;
                    target.color = Color.White;
                    if (ticking.State == SoundState.Playing)
                        ticking.Stop();
                }
			    //Target movement logic
                target.ContinuousMove(elapsedTime, totalElapsedTime);

                //Stats
                precision = (timeOn / (gameTotalTime - timeRemaining)) * 100;
                avgTimeOn = timeOn / numberOfTimesOn;

                if (timeRemaining < 0)
                {
                    MediaPlayer.Stop();
                    mode = SelectMode.results;
                    score = (int)precision;
                    ticking.Stop();
                    endTrackerGame?.Invoke(this, new TrackerGameEventArgs(precision, avgTimeOn, score));
                }
            }
            else
            {
                effectiveDiff.X = viewSource.X - oldViewSource.X;
                effectiveDiff.Y = viewSource.Y - oldViewSource.Y;
                oldViewSource = viewSource;
                goButtonRectangle.X = goButtonRectangle.X - effectiveDiff.X;
                goButtonRectangle.Y = goButtonRectangle.Y - effectiveDiff.Y;
                goButton.rectangle = goButtonRectangle;

                if (goButton.IsPressed(newMouse, oldMouse,volume))
                {
                    go = true;
                }

                oldMouse = newMouse;        //difference between hittingGame: not necessary to update oldMouse after game start,small optimization
            }
        }

        public void PauseSound()
        {
            ticking.Pause();
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);

            if (go)
            {
                target.Draw(_spriteBatch, middleScreen, viewSource, elapsedTime);
                _spriteBatch.Draw(backgroundStatsInGame, new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(font, "Precisione: " + Math.Round(precision, 2) + "%", new Vector2(50, 30), Color.Black);
                _spriteBatch.DrawString(font, "Tempo rimasto: " + Math.Round(timeRemaining, 0), new Vector2(400, 30), Color.Black);
            }
            else
            {
                goButton.Draw(_spriteBatch);
            }
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
        }

    }
}
