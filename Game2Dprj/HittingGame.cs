using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private Point targetDim;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Point mouseDiff;
        private Point mouseSens;
        //screen
        private Point screenDim;
        private Point middleScreen;
        //game status
        private Random rand;
        private int point;
        private int timeRemaining;        //[ms]

        public HittingGame(Rectangle viewSource, Rectangle viewDest, Rectangle cursorRect, Point screenDim, Point middleScreen, Texture2D background, Texture2D cursor, Texture2D target)
        {
            timeRemaining = 60000;
            point = 0;
            this.viewSource = viewSource;
            this.viewDest = viewDest;
            this.cursorRect = cursorRect;
            oldMouse = Mouse.GetState();
            this.screenDim = screenDim;
            this.middleScreen = middleScreen;
            this.background = background;
            this.cursor = cursor;
            mouseDiff = new Point(0, 0);
            mouseSens = new Point(5, 5);            //change this in the menu

            targetDim = new Point(screenDim.X / 20, screenDim.X / 20);
            this.target = new Target(target, targetDim, Color.White, target.Width / 2, background, screenDim, viewSource);            
        }

        public void Update(GameTime gameTime)
        {
            timeRemaining -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds);

            if (timeRemaining < 0)
                timeRemaining = 0;

            newMouse = Mouse.GetState();
            Mouse.SetPosition(middleScreen.X, middleScreen.Y);

            mouseDiff.X = mouseSens.X * (newMouse.X - middleScreen.X);
            mouseDiff.Y = mouseSens.Y * (newMouse.Y - middleScreen.Y);

			Game1_Methods.CameraTargetMovement(ref viewSource, ref target.position, mouseDiff, screenDim, background);

            if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
            {
                if (target.Contains(middleScreen))
                {
                    point++;
                    target.SpawnMove(screenDim, background, viewSource);
                }
            }
            oldMouse = newMouse;               //this is necessary to store the previous value of left button

        }

        public void Draw(GameTime gameTime,SpriteBatch _spriteBatch,SpriteFont font)
        {
            _spriteBatch.Draw(background, viewDest, viewSource, Color.White);
            target.Draw(_spriteBatch);
            _spriteBatch.Draw(cursor, cursorRect, Color.White);
            _spriteBatch.DrawString(font, "Bersagli presi: " + point, new Vector2(100, 100), Color.Black);
            _spriteBatch.DrawString(font, "Tempo rimasto: " + timeRemaining/1000, new Vector2(800, 100), Color.Black);
        }
    }

}
