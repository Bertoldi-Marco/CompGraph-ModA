using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class Button
    {
        public Rectangle rect;
        public Point rectDimensions;
        public Texture2D rectText;
        public Color buttonColor;
        public bool inc;

        public Button(Rectangle rect, Point rectDimensions, Texture2D rectText, Color buttonColor)
        {
            this.rect = rect;
            this.rectDimensions = rectDimensions;
            this.rectText = rectText;
            this.buttonColor = buttonColor;
            inc = false;
        }

        public bool IsPressed(MouseState newMouse, MouseState oldMouse)
        {
            //mouseRectangle = new Rectangle(newMouse.X, newMouse.Y, 1, 1);           //single point rectangle, forse si può scrivere meglio don (con il contain)
            //if (mouseRectangle.Intersects(buttonRectangle))

            if (rect.Contains(new Point(newMouse.X, newMouse.Y)))
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
            else
            {
                buttonColor.A = 255;
            }

            return false;

        }
    }
}
