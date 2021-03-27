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
		public static Rectangle Camera_movement(Rectangle viewSource, Point mouseDiff, int xScreenDim, int yScreenDim, Texture2D background)
        {
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

			 return viewSource;
        }

        public static Point Target_saturation(Rectangle viewSource, Point targetPosition, Point targetDim, int xScreenDim, int yScreenDim, Texture2D background)
        {
            //saturation on the left
            if (viewSource.X + targetPosition.X - xScreenDim / 2 < 0) 
            {
                targetPosition.X = xScreenDim / 2 - viewSource.X;
            }
            //Saturation on the right
            if (background.Width - viewSource.X - targetDim.X / 2 - targetPosition.X - xScreenDim / 2 < 0)
            {
                targetPosition.X = background.Width - targetDim.X / 2 - viewSource.X - xScreenDim / 2;
            }
            //Saturation on the top
            if (viewSource.Y + targetPosition.Y - yScreenDim / 2 < 0)
            {
                targetPosition.Y = yScreenDim / 2 - viewSource.Y;
            }
            //Saturation on the bottom
            if (background.Height - viewSource.Y - targetDim.Y / 2 - targetPosition.Y - yScreenDim / 2 < 0) 
            {
                targetPosition.Y = background.Height - targetDim.Y / 2 - viewSource.Y - yScreenDim / 2;
            }

            return targetPosition;
        }

        public static void Camera_target_movement(ref Rectangle viewSource, Point mouseDiff, ref Point targetPosition, int xScreenDim, int yScreenDim, Texture2D background)
        {
            if (viewSource.X > 0 && mouseDiff.X < 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X < 0)
                {
                    targetPosition.X -= mouseDiff.X - viewSource.X;
                    viewSource.X = 0;
                }
                else
                    targetPosition.X -= mouseDiff.X;
            }
            //Saturation on the right
            if (viewSource.X < background.Width - xScreenDim && mouseDiff.X > 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X > background.Width - xScreenDim)
                {
                    targetPosition.X -= mouseDiff.X - viewSource.X + background.Width - xScreenDim;
                    viewSource.X = background.Width - xScreenDim;
                }
                else
                    targetPosition.X -= mouseDiff.X;
            }
            //Saturation on the top
            if (viewSource.Y > 0 && mouseDiff.Y < 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y < 0)
                {
                    targetPosition.Y -= mouseDiff.Y - viewSource.Y;
                    viewSource.Y = 0;
                }
                else
                    targetPosition.Y -= mouseDiff.Y;
            }
            //Saturation on the bottom
            if (viewSource.Y < background.Height - yScreenDim && mouseDiff.Y > 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y > background.Height - yScreenDim)
                {
                    targetPosition.Y -= mouseDiff.Y - viewSource.Y + background.Height - yScreenDim;
                    viewSource.Y = background.Height - yScreenDim;
                }
                else
                    targetPosition.Y -= mouseDiff.Y;
            }

        }
    }
}
