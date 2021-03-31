using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    class Target
    {
        private Texture2D texture;
        public Color color;
        private Point dimensions;
        private float scale;  
        private int squareSpeed;
        private Vector3 vectSpeed;
        public Vector3 position;
        private Random rand;
        private int radius;

        //Time related
        private double totalElapsedTimePrev;
        private double totalElapsedTimeDiff;
        private double timeToSpeedChange;



        public Target(Texture2D texture, Point dimensions, Point zLimits, Vector3 position, Color color, int radius, int speed)
        {
            this.texture = texture;
            this.dimensions = dimensions;
            this.squareSpeed = speed;
            this.position = position;
            this.radius = radius;
            this.color = color;
            vectSpeed = new Vector3(0, 0, 0);
            timeToSpeedChange = 0;
            totalElapsedTimePrev = 0;
            totalElapsedTimeDiff = 0;
            rand = new Random();
            scale = 1;

        }
        public Target(Texture2D texture, Point dimensions, Point zLimits, Color color, int radius, Texture2D background, Point screenDim, Rectangle viewSource)
        {
            this.texture = texture;
            this.dimensions = dimensions;
            this.radius = radius;
            this.color = color;
            rand = new Random();
            SpawnMove (screenDim,zLimits, background, viewSource);
            squareSpeed = 0;
            vectSpeed = new Vector3(0, 0, 0);
            timeToSpeedChange = 0;
            totalElapsedTimePrev = 0;
            totalElapsedTimeDiff = 0;
            scale = 1;
        }

        public bool Contains(Point position)
        {
            Vector2 center = new Vector2(this.position.X + (dimensions.X / 2), this.position.Y + (dimensions.Y / 2));  //center of the target
            double distance = Math.Sqrt(Math.Pow(position.X - center.X, 2) + Math.Pow(position.Y - center.Y, 2));   //pitagora, distance between the center and the position

            if (distance <= radius)
                return true;
            else
                return false;
        }

        public void ContinuousMove(Rectangle boundaries, Point zLimits, double elapsedTime)  //used in TrackerGame
        {
            //Update target position in relation to speed
            position.X += (float)(vectSpeed.X * elapsedTime);
            position.Y += (float)(vectSpeed.Y * elapsedTime);
            position.Z += (float)(vectSpeed.Z * elapsedTime);

            //Bounce on the left
            if (position.X <= boundaries.Left)
            {
                position.X = boundaries.Left;
                vectSpeed.X = -vectSpeed.X;
            }
            //Bounce on the right
            if (position.X + dimensions.X >= boundaries.Right)
            {
                position.X = boundaries.Right - dimensions.X;
                vectSpeed.X = -vectSpeed.X;
            }
            //Bounce on the top
            if (position.Y <= boundaries.Top)
            {
                position.Y = boundaries.Top;
                vectSpeed.Y = -vectSpeed.Y;
            }
            //Bounce on the bottom
            if (position.Y + dimensions.Y >= boundaries.Bottom)
            {
                position.Y = boundaries.Bottom - dimensions.Y;
                vectSpeed.Y = -vectSpeed.Y;
            }
            //Bounce on the back
            if (position.Z >= zLimits.Y + 2 * radius)    //we assume .y like upper limit 
            {
                position.Z = zLimits.Y + 2 * radius;
                vectSpeed.Z = -vectSpeed.Z;
            }
            //Bounce on the front
            if (position.Z <= zLimits.X)    //we assume .x like inferior limit
            {
                position.Z = zLimits.X;
                vectSpeed.Z = -vectSpeed.Z;
            }
        }

        public void UpdateVectSpeed(double totalElapsedTime)
        {
            totalElapsedTimeDiff = totalElapsedTime - totalElapsedTimePrev;
            
            //v^2 = x^2+y^2+z^2;    vi^2|max = v^2/2; 

            if (timeToSpeedChange <= totalElapsedTimeDiff)
            {
                if (1 == rand.Next(2))
                    vectSpeed.X = (float)Math.Sqrt(rand.Next(100) * squareSpeed / 200);    
                else
                    vectSpeed.X = -(float)Math.Sqrt(rand.Next(100) * squareSpeed / 200);

                if (1 == rand.Next(2))
                    vectSpeed.Y = (float)Math.Sqrt(rand.Next(100) * squareSpeed / 200);
                else
                    vectSpeed.Y = -(float)Math.Sqrt(rand.Next(100) * squareSpeed / 200);

                if (1 == rand.Next(2))
                    vectSpeed.Z = (float)Math.Sqrt(squareSpeed - Math.Pow(vectSpeed.X, 2) - Math.Pow(vectSpeed.Y, 2));
                else
                    vectSpeed.Z = -(float)Math.Sqrt(squareSpeed - Math.Pow(vectSpeed.X, 2) - Math.Pow(vectSpeed.Y, 2));

                totalElapsedTimePrev = totalElapsedTime;
                timeToSpeedChange = rand.NextDouble() * 4;
            }
        }

        public void SpawnMove(Point screenDim, Point zLimits, Texture2D background, Rectangle viewSource)  //used in HittingGame
        {
            position.X = rand.Next(screenDim.X / 8, screenDim.X - screenDim.X / 8);
            position.Y = rand.Next(screenDim.Y / 8, screenDim.Y - screenDim.Y / 8);
            position.Z = rand.Next(zLimits.X, zLimits.Y- 2* radius);    //no need of saturation

            //Saturation on the left
            if (viewSource.X + position.X - screenDim.X / 2 < 0)
            {
                position.X = screenDim.X / 2 - viewSource.X;
            }
            //Saturation on the right
            if (background.Width - viewSource.X - dimensions.X / 2 - position.X - screenDim.X / 2 < 0)
            {
                position.X = background.Width - dimensions.X / 2 - viewSource.X - screenDim.X / 2;
            }
            //Saturation on the top
            if (viewSource.Y + position.Y - screenDim.Y / 2 < 0)
            {
                position.Y = screenDim.Y / 2 - viewSource.Y;
            }
            //Saturation on the bottom
            if (background.Height - viewSource.Y - dimensions.Y / 2 - position.Y - screenDim.Y / 2 < 0)
            {
                position.Y = background.Height - dimensions.Y / 2 - viewSource.Y - screenDim.Y / 2;
            }
        }


        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, color, 0, new Vector2(position.X, position.Y), scale, SpriteEffects.None,1);
        }

        public void UpdateScale(Point cameraOrigin, float cameraDistance)
        {
            // we assume that the target original dimensions are referred to the reference "sphere", wich center is in cameraOrig and his radius is cameraDistance

            Vector3 centerPosition = new Vector3(position.X + radius, position.Y + radius, position.Z + radius);    //target position referred to its center
            Vector3 newReference = new Vector3(centerPosition.X-cameraOrigin.X, centerPosition.Y-cameraOrigin.Y, centerPosition.Z+ cameraDistance);       //new coordinate system with cameraOrig as (0, 0, -cameraDistance)
            double distance = Math.Sqrt(Math.Pow(newReference.X,2)+Math.Pow(newReference.Y,2)+Math.Pow(newReference.Z,2));  //distance between camera and point

            double halfAngle = Math.Atan(radius / distance);    //angle between the center of the target and one edge
            double apparentRadius = cameraDistance * Math.Tan(halfAngle);

            scale = (float)(apparentRadius / radius);
        }
    }
}
