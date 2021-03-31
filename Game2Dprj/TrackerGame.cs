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
        //First Update bool
        //private bool drawn;
        //From Game1
        private Point screenDim;
        private Point middleScreen;
        //Background
        private Texture2D background;
        private Rectangle viewSource;
        private Rectangle viewDest;
        private Rectangle boundariesRect;
        //Cursor
        private Texture2D cursor;
        private Rectangle cursorRect;
        //Target
        private Vector2 targetPos;
        private int squareTargetModulusSpeed;
        Target target;
        //private float targetScale;
        //Mouse
        private MouseState newMouse;
        private Point mouseDiff;
        //Time
        private double elapsedTime;
        private double timeRemaining;        //[s]
        private double timeOn;
        //Font
        private SpriteFont font;
        //Stats
        private double precision;
        private const double totalTime = 20;
        //event
        public event EventHandler<TrackerGameEventArgs> endTrackerGame;

        public TrackerGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Point screenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target, SpriteFont font)
        {
            //drawn = false;

            //Initiate variables
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            this.screenDim = screenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            this.font = font;
            precision = 100;
            timeRemaining = totalTime;
            timeOn = 0;
            boundariesRect = new Rectangle((2 * screenDim.X - background.Width) / 2, (2 * screenDim.Y - background.Height) / 2, background.Width - screenDim.X, background.Height - screenDim.Y);
            mouseDiff = new Point(0,0);
            targetPos = new Vector2((screenDim.X - target.Width) / 2, (screenDim.Y - target.Height) / 2);
            squareTargetModulusSpeed = (int)Math.Pow(300, 2);
            this.target = new Target(target, new Point(target.Width, target.Height), targetPos, Color.White, target.Width / 2, squareTargetModulusSpeed);

        }

        public void Update(GameTime gameTime,ref SelectMode mode)
        {
            elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            timeRemaining -= elapsedTime;

            if (timeRemaining < 0)
            {
                mode = SelectMode.results;
                endTrackerGame?.Invoke(this, new TrackerGameEventArgs((int)precision));
            }

            //#Protection not needed while menu is used    //if (!drawn)  //protect from movements since in the first call the mouse is not in the center (monogame window not opened yet)
            //{
            //    newMouse = new MouseState(middleScreen.X, middleScreen.Y, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            //    oldMouse = newMouse;
            //}
            //else

            //Camera movements
            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            mouseDiff.X = newMouse.X - middleScreen.X;
            mouseDiff.Y = newMouse.Y - middleScreen.Y;

            //Update background and target position in relation to mouse movement
            Game1_Methods.CameraTargetMovement(ref viewSource, ref target.position, ref boundariesRect, mouseDiff, screenDim, background); 
            
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
            target.UpdateVectSpeed(gameTime.TotalGameTime.TotalSeconds);
            target.ContinuousMove(boundariesRect, elapsedTime);

            //Stats
            precision = (timeOn / (totalTime - timeRemaining)) * 100;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            target.Draw(_spriteBatch);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
            _spriteBatch.DrawString(font, "Precisione: " + precision +"%", new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, "Tempo rimasto: " + timeRemaining, new Vector2(800, 100), Color.Black);

            //#Protection component
            //if (!drawn) //monogame window available
            //    drawn = true;   
        }

    }
}
