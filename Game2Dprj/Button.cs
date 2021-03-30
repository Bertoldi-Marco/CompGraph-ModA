using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    class Button
    {
        public Rectangle rectangle;
        public Texture2D texture;
        public Color color;
        public bool inc;

        public Button(Rectangle rectangle, Texture2D texture, Color color)
        {
            this.rectangle = rectangle;
            this.texture = texture;
            this.color = color;
            inc = false;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, rectangle, color);
        }

        public bool IsPressed(MouseState newMouse, MouseState oldMouse)
        {
            if (rectangle.Contains(new Point(newMouse.X, newMouse.Y)))
            {
                if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                {
                    return true;
                }

                if (color.A == 255) inc = false;              //max opacity reached
                if (color.A == 0) inc = true;
                if (inc)
                {
                    color.A += 5;                 //change transparence
                }
                else
                {
                    color.A -= 5;
                }

            }
            else
            {
                color.A = 255;
            }

            return false;

        }
    }
}
