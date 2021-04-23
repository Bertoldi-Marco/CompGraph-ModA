using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Game2Dprj
{
    public class HittingGame
    {
        //-------------------------------Internal variables
        //Background
        private Texture2D background;
        private Rectangle viewSource;
        private Rectangle viewDest;
        //Cursor
        private Rectangle cursorRect;
        private Texture2D cursor;
        //Target
        private Target target;
        private int zLimits;
        private Texture2D sphereAtlas;
        private Texture2D explosionAtlas;
        private List<Target> explodingTargets;
        private List<Target> toBeRemoved;
        private Texture2D targetText;
        private double elapsedTime;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
        //Screen
        private Point screenDim;
        private Point middleScreen;

        //Stats
        private const int totalTime = 20;
        int targetsDestroyed;
        int timeRemaining;        //[ms]        
        int clicks;
		double score;

        //Sound
        List<SoundEffectInstance> soundEffectInstancesList;
        SoundEffect[] glassBreak;
        Random rand;

        //Event
        public event EventHandler<HittingGameEventArgs> endHittingGame;

        public HittingGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Point screenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target, Texture2D sphereAtlas, Texture2D explosionAtlas, SoundEffect[] glassBreak)
        {
            rand = new Random();
            score = 0;
            this.explosionAtlas = explosionAtlas;
            targetText = target;
            clicks = 0;
            timeRemaining = totalTime * 1000;
            targetsDestroyed = 0;
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            oldMouse = Mouse.GetState();
            this.screenDim = screenDim;
            this.middleScreen = middleScreen;
            zLimits = 1000;
            this.background = background;
            this.cursor = cursor;
            mouseDiff = new Point(0, 0);
            this.target = new Target(target, viewSource, new Point(background.Width, background.Height), screenDim, zLimits, target.Width / 2, Color.White, sphereAtlas, explosionAtlas);       //lasciato target.width della sfera vecchia, dimesioni coincidenti ma schifezza di codice, solo per tornare agevolemente alla pallina statica
            this.sphereAtlas = sphereAtlas;
            explodingTargets = new List<Target>();
            soundEffectInstancesList = new List<SoundEffectInstance>();
            this.glassBreak = glassBreak;
        }


        public void Update(GameTime gameTime, ref SelectMode mode, double mouseSens, float volume)
        {
            timeRemaining -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds);
            elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            if (timeRemaining < 0)
            {
                mode = SelectMode.results;

                //normalize score to be in percentage
                score = 100*score/((target.cameraDistance + target.zRange)*60);     //60 = empyrichal limits for targetsDestroyed

                HittingGameEnded(new HittingGameEventArgs(targetsDestroyed, clicks, totalTime, (int)score));
            }

            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);

            mouseDiff.X = (int)Math.Round(mouseSens * (newMouse.X - middleScreen.X));
            mouseDiff.Y = (int)Math.Round(mouseSens * (newMouse.Y - middleScreen.Y));

            Game1_Methods.CameraMovement(ref viewSource, mouseDiff, screenDim, new Point(background.Width, background.Height));

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                clicks++;
                if (target.Contains(middleScreen))
                {
                    score += target.distance;
                    targetsDestroyed++;
                    //target.sphere.isExploding = true;           //little trick to set up explosion for target in list
                    explodingTargets.Add(target.CloneTarget());
                    //target.sphere.isExploding = false;           //little trick to set up explosion for target in list      
                    target.SpawnMove(screenDim);

                    SoundEffectInstance newSound = glassBreak[rand.Next(glassBreak.Length)].CreateInstance();
                    newSound.Play();
                    soundEffectInstancesList.Add(newSound);                   
                }
            }
            oldMouse = newMouse;               //this is necessary to store the previous value of left button

            List<SoundEffectInstance> soundToRemove = new List<SoundEffectInstance>();

            foreach (SoundEffectInstance sound in soundEffectInstancesList)
            {
                if (sound.State == SoundState.Stopped)
                    soundToRemove.Add(sound);
            }

            foreach (SoundEffectInstance sound in soundToRemove)
            {
                soundEffectInstancesList.Remove(sound);
            }

            foreach (SoundEffectInstance sound in soundEffectInstancesList)
            {
                sound.Volume = volume;
                if (sound.State == SoundState.Paused)
                    sound.Play();
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font, GameTime gameTime)
        {
            toBeRemoved = new List<Target>();
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            target.Draw(_spriteBatch, middleScreen, viewSource, elapsedTime);

            foreach (Target trgt in explodingTargets)
            {
                if(trgt.sphere.isExploding == false)           //animation ended
                {
                    toBeRemoved.Add(trgt);
                }
                else
                {
                    trgt.Draw(_spriteBatch, middleScreen, viewSource, elapsedTime);
                }
            }

            foreach(Target trgt in toBeRemoved)     //the double foreach is necessary to avoid exception removing target from the list
            {
                explodingTargets.Remove(trgt);
            }
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
            _spriteBatch.DrawString(font, "Bersagli presi: " + targetsDestroyed, new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, "Tempo rimasto: " + timeRemaining / 1000, new Vector2(800, 100), Color.Black);
        }

        protected virtual void HittingGameEnded(HittingGameEventArgs e)
        {
            endHittingGame?.Invoke(this, e);
        }

        public void PauseSound()
        {
            foreach(SoundEffectInstance sound in soundEffectInstancesList)
            {
                if (sound.State == SoundState.Playing)
                    sound.Pause();
            }
        }
    }
}
