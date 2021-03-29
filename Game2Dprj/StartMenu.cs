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
        private Texture2D rectText;     //monocrome at the moment
        private Color hittingButtonColor;
        private Color trackerButtonColor;
        private bool hitInc;
        private bool trackInc;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        //private Rectangle mouseRectangle;

        public StartMenu(int xScreenDimension, int yScreenDimension, GraphicsDevice graphicsDevice, Texture2D background)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480,270);    //needs to be improved
            hittingRect = new Rectangle(xScreenDimension / 3 - rectDimensions.X / 2, yScreenDimension / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            trackerRect = new Rectangle(2 * (xScreenDimension / 3) - rectDimensions.X / 2, yScreenDimension / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);

            hittingButtonColor = new Color(255, 255, 255, 255);              //color in r,g,b + transparence 
            trackerButtonColor = new Color(255, 255, 255, 255);              //color in r,g,b + transparence 
            trackInc = false;
            hitInc = false;

            //Define a single pixel buttonColor texture that will be scaled in the draw
            rectText = new Texture2D(graphicsDevice, 1, 1);
            rectText.SetData(new[] { Color.Yellow });
            //rectText = background;

        }

        public void Update(ref SelectMode mode, Point middleScreen)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (IsPressed(hittingRect, ref hittingButtonColor,ref hitInc))
            {
                mode = SelectMode.hittingGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);  //set mouse in the middle before the game is started
            }

            if (IsPressed(trackerRect, ref trackerButtonColor, ref trackInc))
            {
                mode = SelectMode.trackerGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(rectText, hittingRect, hittingButtonColor);
            _spriteBatch.Draw(rectText, trackerRect, trackerButtonColor);
            _spriteBatch.DrawString(font, "Hitting Game", new Vector2(hittingRect.X + hittingRect.Width / 2, hittingRect.Y + hittingRect.Height / 2), Color.Black);  //how to center respect to the string length?
            _spriteBatch.DrawString(font, "Tracker Game", new Vector2(trackerRect.X + trackerRect.Width / 2, trackerRect.Y + trackerRect.Height / 2), Color.Black);
        }

        private bool IsPressed(Rectangle buttonRectangle, ref Color buttonColor, ref bool inc)
        {
            //mouseRectangle = new Rectangle(newMouse.X, newMouse.Y, 1, 1);           //single point rectangle, forse si può scrivere meglio don (con il contain)
            //if (mouseRectangle.Intersects(buttonRectangle))

            if(buttonRectangle.Contains(new Point(newMouse.X, newMouse.Y)))
            {
                if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                {
                    return true;
                }
                
                if (buttonColor.A == 255) inc = false;              //maxtransparence reached
                if (buttonColor.A == 0) inc = true;
                if (inc)
                {
                    buttonColor.A += 5;                 //change transparence
                }
                else
                {
                    buttonColor.A -= 5;
                }

            }

            return false;

        }
    }
}
