using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public bool alreadyOnButton;

        private SoundEffect onButton;
        private SoundEffect clickButton;

        public Button(Rectangle rectangle, Texture2D texture, Color color, SoundEffect onButton, SoundEffect clickButton)
        {
            this.rectangle = rectangle;
            this.texture = texture;
            this.color = color;
            inc = false;
            this.onButton = onButton;
            this.clickButton = clickButton;
            alreadyOnButton = false;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, rectangle, color);
        }

        public bool IsPressed(MouseState newMouse, MouseState oldMouse, float volume)
        {
            if (rectangle.Contains(new Point(newMouse.X, newMouse.Y)))
            {
                if (!alreadyOnButton)
                {
                    //SoundEffectInstance onButtonInstance = onButton.CreateInstance();
                    //onButtonInstance.Volume = 0.5f;
                    //onButtonInstance.Play();
                    onButton.Play(volume/10, 0f, 0f);                //low volume
                    alreadyOnButton = true;
                }
                if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                {
                    clickButton.Play(volume, 0f, 0f);
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
                alreadyOnButton = false;
                color.A = 255;
            }

            return false;

        }
    }
}
