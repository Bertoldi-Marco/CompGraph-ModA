using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class StartMenu
    {
        private Rectangle hittingRect;
        private Rectangle trackerRect;
        private Point rectDimensions;
        private Button hitButton;
        private Button trackButton;
        
       
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        //private Rectangle mouseRectangle;

        public StartMenu(int xScreenDimension, int yScreenDimension, GraphicsDevice graphicsDevice, Texture2D background, Texture2D hitButtonStart, Texture2D trackButtonStart)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480,270);    //needs to be improved
            hittingRect = new Rectangle(xScreenDimension / 3 - rectDimensions.X / 2, yScreenDimension / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            trackerRect = new Rectangle(2 * (xScreenDimension / 3) - rectDimensions.X / 2, yScreenDimension / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            hitButton = new Button(hittingRect, hitButtonStart, Color.Cyan);
            trackButton = new Button(trackerRect, trackButtonStart, Color.Cyan);
        }

        public void Update(ref SelectMode mode, Point middleScreen)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (hitButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.hittingGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);  //set mouse in the middle before the game is started
            }

            if (trackButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.trackerGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            hitButton.Draw(_spriteBatch);
            trackButton.Draw(_spriteBatch);
        }
    }
}
