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
        //TrackerGame used:
        //Update the view in the background and the target position in relation to the mouse movements and background limits
        public static void CameraTargetMovement(ref Rectangle viewSource, ref Vector3 targetPos, ref Rectangle boundariesRect, Point mouseDiff, Point screenDim, Texture2D background) 
        {
            Point effectiveDiff = new Point(-viewSource.X, -viewSource.Y);

            //Saturation on the left
            if (viewSource.X > 0 && mouseDiff.X < 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X < 0)
                    viewSource.X = 0;
            }
            //Saturation on the right
            if (viewSource.X < background.Width - screenDim.X && mouseDiff.X > 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X > background.Width - screenDim.X)
                    viewSource.X = background.Width - screenDim.X;
            }
            //Saturation on the top
            if (viewSource.Y > 0 && mouseDiff.Y < 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y < 0)
                    viewSource.Y = 0;
            }
            //Saturation on the bottom
            if (viewSource.Y < background.Height - screenDim.Y && mouseDiff.Y > 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y > background.Height - screenDim.Y)
                    viewSource.Y = background.Height - screenDim.Y;
            }
            effectiveDiff.X += viewSource.X;
            effectiveDiff.Y += viewSource.Y;

            //Update target position
            targetPos.X -= effectiveDiff.X;
            targetPos.Y -= effectiveDiff.Y;

            //Update boundaries position
            boundariesRect.X -= effectiveDiff.X;
            boundariesRect.Y -= effectiveDiff.Y;
        }

        // HittingGame used:
        //Update the view in the background and the target position in relation to the mouse movements and background limits
        public static void CameraTargetMovement(ref Rectangle viewSource, ref Vector3 targetPosition, Point mouseDiff, Point screenDim, Texture2D background) 
        {
            //Saturation on the left
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
            if (viewSource.X < background.Width - screenDim.X && mouseDiff.X > 0)
            {
                viewSource.X += mouseDiff.X;
                if (viewSource.X > background.Width - screenDim.X)
                {
                    targetPosition.X -= mouseDiff.X - viewSource.X + background.Width - screenDim.X;
                    viewSource.X = background.Width - screenDim.X;
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
            if (viewSource.Y < background.Height - screenDim.Y && mouseDiff.Y > 0)
            {
                viewSource.Y += mouseDiff.Y;
                if (viewSource.Y > background.Height - screenDim.Y)
                {
                    targetPosition.Y -= mouseDiff.Y - viewSource.Y + background.Height - screenDim.Y;
                    viewSource.Y = background.Height - screenDim.Y;
                }
                else
                    targetPosition.Y -= mouseDiff.Y;
            }

        }


        public static Point TargetSaturation(Rectangle viewSource, Point targetPosition, Point targetDim, int xScreenDim, int yScreenDim, Texture2D background)
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

        public static void TargetMovement(ref Rectangle targetRect, ref Vector2 targetPos, ref Vector2 targetActualSpeed, Rectangle boundaries, double elapsedTime) //obsoleto
        {
            //Update target position in relation to speed
            targetPos.X += (float)(targetActualSpeed.X * elapsedTime);
            targetPos.Y += (float)(targetActualSpeed.Y * elapsedTime);

            //Bounce on the left
            if (targetPos.X <= boundaries.Left)
            {
                targetPos.X = boundaries.Left;
                targetActualSpeed.X = -targetActualSpeed.X;
            }
            //Bounce on the right
            if (targetPos.X + targetRect.Width >= boundaries.Right)
            {
                targetPos.X = boundaries.Right - targetRect.Width;
                targetActualSpeed.X = -targetActualSpeed.X;
            }
            //Bounce on the top
            if (targetPos.Y  <= boundaries.Top)
            {
                targetPos.Y = boundaries.Top;
                targetActualSpeed.Y = -targetActualSpeed.Y;
            }
            //Bounce on the bottom
            if (targetPos.Y + targetRect.Height >= boundaries.Bottom)
            {
                targetPos.Y = boundaries.Bottom - targetRect.Height;
                targetActualSpeed.Y = -targetActualSpeed.Y;
            }
        }
        
    }
}    

