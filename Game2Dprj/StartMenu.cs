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
        private Texture2D rectText;     //to be defined
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        private Rectangle mouseRectangle;

        Color buttonColor = new Color(255, 255, 255, 255);              //color in r,g,b + transparence
        bool inc = false;

        public StartMenu(int xScreenDimension, int yScreenDimension)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(640,360);    //needs to be improved
            hittingRect = new Rectangle(xScreenDimension/3-rectDimensions.X/2, yScreenDimension/3-rectDimensions.Y/2, rectDimensions.X, rectDimensions.Y);
            trackerRect = new Rectangle(xScreenDimension * 2/3 - rectDimensions.X / 2, yScreenDimension * 2/3 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
        }

        public void Update(ref SelectMode mode)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (IsPressed(hittingRect))
            {
                mode = SelectMode.hittingGame;
            }

            if (IsPressed(trackerRect))
            {
                mode = SelectMode.trackerGame;
            }

        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(rectText, hittingRect, Color.White);
            _spriteBatch.Draw(rectText, trackerRect, Color.White);
        }

        public bool IsPressed(Rectangle buttonRectangle)
        {
            mouseRectangle = new Rectangle(newMouse.X, newMouse.Y, 1, 1);           //single point rectangle, forse si può scrivere meglio don (con il contain)

            if (mouseRectangle.Intersects(buttonRectangle))
            {
                if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                {
                    return true;
                }

                if (buttonColor.A == 255) inc = false;              //maxtransparence reached
                if (buttonColor.A == 0) inc = true;
                if (inc)
                {
                    buttonColor.A += 4;                 //change transparence
                }
                else
                {
                    buttonColor.A -= 4;
                }

            }

            return false;

        }
    }
}
