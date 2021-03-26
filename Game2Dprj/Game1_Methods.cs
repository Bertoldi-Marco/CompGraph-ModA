using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game2Dprj
{
    public static class Game1_Methods
    {
        public static Point CameraMovement(Rectangle viewSource, Point mouseDiff, int xScreenDim, int yScreenDim, Texture2D background)     //Returns the relative camera movement according to the background limits
        {
            Point origPos = new Point(viewSource.X, viewSource.Y);

            //Saturation on the left
            if (viewSource.X > 0 && mouseDiff.X < 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X < 0)
                    viewSource.X = 0;
            }
            //Saturation on the right
            if (viewSource.X < background.Width - xScreenDim && mouseDiff.X > 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X > background.Width - xScreenDim)
                    viewSource.X = background.Width - xScreenDim;
            }
            //Saturation on the top
            if (viewSource.Y > 0 && mouseDiff.Y < 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y < 0)
                    viewSource.Y = 0;
            }
            //Saturation on the bottom
            if (viewSource.Y < background.Height - yScreenDim && mouseDiff.Y > 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y > background.Height - yScreenDim)
                    viewSource.Y = background.Height - yScreenDim;
            }
            return new Point(viewSource.X-origPos.X, viewSource.Y-origPos.Y);
        }
    }
}
