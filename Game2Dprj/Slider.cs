using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    class Slider
    {
        Rectangle knobReachable;  //range of knob
        Vector2 basePosition;
        Vector2 knobPosition;
        Texture2D baseText;
        Texture2D knobText;

        public Slider(Point drawPosition, double defValue, Texture2D baseText, Texture2D knobText)
        {
            this.baseText = baseText;
            this.knobText = knobText;
            knobReachable = new Rectangle( drawPosition, new Point(baseText.Width, knobText.Height));
            knobPosition = new Vector2(knobReachable.X + (int)(defValue * knobReachable.Width), knobReachable.Y);
            basePosition = new Vector2(knobReachable.X + knobText.Width / 2, knobReachable.Y + knobText.Height / 2 - baseText.Height / 2);
        }

        public double Update(MouseState oldMouse, MouseState newMouse)
        {
            if (oldMouse.LeftButton == ButtonState.Pressed && newMouse.LeftButton == ButtonState.Pressed && Contains(newMouse.X, newMouse.Y))
            {
                knobPosition.X += newMouse.X - oldMouse.X;
                if (knobPosition.X > knobReachable.X + knobReachable.Width)
                    knobPosition.X = knobReachable.X + knobReachable.Width;
                if (knobPosition.X < knobReachable.X)
                    knobPosition.X = knobReachable.X;
            }
            return (knobPosition.X - knobReachable.X) / knobReachable.Width;    //value used is between 0 and 1 included
        }

        private bool Contains(int pointX, int pointY)
        {
            Vector2 center = new Vector2(knobPosition.X + knobText.Width / 2, knobPosition.Y + knobText.Width / 2);  //center of knob
            double dist = Math.Sqrt(Math.Pow(pointX - center.X, 2) + Math.Pow(pointY - center.Y, 2));   //pitagora, distance between the center and the mouse

            if (dist <= knobText.Width/2)
                return true;
            else
                return false;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(baseText, basePosition, Color.White);
            _spriteBatch.Draw(knobText, knobPosition, Color.White);
        }

    }
}
