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
        SpriteFont font;
        string title;
        double value;

        public Slider(Point drawPosition, double defValue, Texture2D baseText, Texture2D knobText, SpriteFont font, string quantity)
        {
            this.baseText = baseText;
            this.knobText = knobText;
            this.font = font;
            this.title = quantity;
            knobReachable = new Rectangle(new Point(drawPosition.X - knobText.Width/2, drawPosition.Y), new Point(baseText.Width + knobText.Width/2, knobText.Height));
            knobPosition = new Vector2(knobReachable.X + (int)(defValue * baseText.Width), knobReachable.Y);
            basePosition = new Vector2(drawPosition.X, knobReachable.Y + knobText.Height / 2 - baseText.Height / 2);
        }

        public double Update(MouseState newMouse)
        {
            if (newMouse.LeftButton == ButtonState.Pressed && knobReachable.Contains(new Point(newMouse.X, newMouse.Y)))
            {
                knobPosition.X = newMouse.X - knobText.Width / 2;
                if (knobPosition.X > knobReachable.X + knobReachable.Width)
                    knobPosition.X = knobReachable.X + knobReachable.Width;
                if (knobPosition.X < knobReachable.X)
                    knobPosition.X = knobReachable.X;
            }
            value = (knobPosition.X - knobReachable.X) / baseText.Width;    //value used is between 0 and 1 included
            return value;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(baseText, basePosition, Color.White);
            _spriteBatch.Draw(knobText, knobPosition, Color.White);
            _spriteBatch.DrawString(font, title + Math.Round(value * 100) + " %", new Vector2(basePosition.X, knobReachable.Y - knobText.Height / 2), Color.White);
        }

    }
}
