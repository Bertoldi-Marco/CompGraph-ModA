using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class Pause
    {
        private Button resumeButton;
        private Button menuButton;

        private Rectangle resumeRect;
        private Rectangle menuRect;
        private Point rectDimensions;
        private Texture2D rectText;     //monocrome at the moment
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        //private Rectangle mouseRectangle;

        public Pause(int xScreenDimension, int yScreenDimension, GraphicsDevice graphicsDevice)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480, 270);    //needs to be improved
            resumeRect = new Rectangle(xScreenDimension / 3 - rectDimensions.X / 2, yScreenDimension / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            menuRect = new Rectangle(2 * (xScreenDimension / 3) - rectDimensions.X / 2, yScreenDimension / 2 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);

            //buttonColor = new Color(255, 255, 255, 255);              //color in r,g,b + transparence

            //Define a single pixel buttonColor texture that will be scaled in the draw
            rectText = new Texture2D(graphicsDevice, 1, 1);
            rectText.SetData(new[] { Color.Yellow });

            resumeButton = new Button(resumeRect, rectDimensions, rectText, new Color(255, 255, 255, 255));
            menuButton = new Button(menuRect, rectDimensions, rectText, new Color(255, 255, 255, 255));

        }

        public void Update(ref SelectMode mode, Point middleScreen)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            //if (IsPressed(ref resumeButton))
            if (resumeButton.IsPressed(newMouse,oldMouse))
            {
                mode = SelectMode.hittingGame;
                Mouse.SetPosition(middleScreen.X, middleScreen.Y);  //set mouse in the middle before the game is started
            }

            //if (IsPressed(ref menuButton))
            if (menuButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.menu;
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(resumeButton.rectText, resumeButton.rect, resumeButton.buttonColor);
            _spriteBatch.Draw(menuButton.rectText, menuButton.rect, menuButton.buttonColor);
            _spriteBatch.DrawString(font, "Resume", new Vector2(resumeButton.rect.X + resumeButton.rect.Width / 2, resumeButton.rect.Y + resumeButton.rect.Height / 2), Color.Black);  //how to center respect to the string length?
            _spriteBatch.DrawString(font, "Main Menu", new Vector2(menuButton.rect.X + menuButton.rect.Width / 2, menuButton.rect.Y + menuButton.rect.Height / 2), Color.Black);
        }

        private bool IsPressed(ref Button button)                   //obsoleto
        {
            //mouseRectangle = new Rectangle(newMouse.X, newMouse.Y, 1, 1);           //single point rectangle, forse si può scrivere meglio don (con il contain)
            //if (mouseRectangle.Intersects(buttonRectangle))

            if (button.rect.Contains(new Point(newMouse.X, newMouse.Y)))
            {
                if (newMouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                {
                    return true;
                }

                if (button.buttonColor.A == 255) button.inc = false;              //maxtransparence reached
                if (button.buttonColor.A == 0) button.inc = true;
                if (button.inc)
                {
                    button.buttonColor.A += 5;                 //change transparence
                }
                else
                {
                    button.buttonColor.A -= 5;
                }

            }
            /*else
            {
                buttonColor.A = 255;
            }*/

            return false;

        }
    }
}
