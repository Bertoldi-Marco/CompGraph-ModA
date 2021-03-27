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

        public StartMenu(int xScreenDimension, int yScreenDimension)
        {
            rectDimensions = new Point(640,360);    //needs to be improved
            hittingRect = new Rectangle(xScreenDimension/3-rectDimensions.X/2, yScreenDimension/3-rectDimensions.Y/2, rectDimensions.X, rectDimensions.Y);
            trackerRect = new Rectangle(xScreenDimension * 2/3 - rectDimensions.X / 2, yScreenDimension * 2/3 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
        }
        public void Update(ref SelectMode mode)
        {

        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            _spriteBatch.Draw(rectText, hittingRect, Color.White);
            _spriteBatch.Draw(rectText, trackerRect, Color.White);
        }
    }
}
