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
        private float scale;
        private int modulusSpeed;
        private Vector3 vectSpeed;
        public Vector3 position;
        private Vector2 positionOnScreen;
        private Random rand;
        private int radius;
        private int apparentRadius;
        private double distance;    //between cameraOrig and the 3d center of the target
        private Point cameraOrig;       //we assume that is constant
        private double cameraDistance;   //we assume that is constant
        private int zRange;
        private Vector3 p1;
        private Vector3 p2;     //used in multi safe point
        private Vector3 p3;     //used in multi safe point
        private Vector3 p4;     //used in multi safe point
        private double alpha;
        private double beta;
        private double gamma;
        private double delta;
        private double gammaDeltaOff;
        private float gammaMax;   //used in multi safe point
        public AnimationSphere sphere;
        private Texture2D sphereAtlas;
        private Texture2D explosionAtlas;

        //Time related
        private double totalElapsedTimePrev;
        private double totalElapsedTimeDiff;
        private double timeToSpeedChange;

        public int Radius
        {
            get;
        }


        //OK
        public Target(Texture2D texture, Rectangle viewSource, Point background, Point screenDim, int zRange, int radius, int speed, Color color, Texture2D sphereAtlas,Texture2D explosionAtlas)   //trackerGame
        {
            this.texture = texture;
            this.zRange = zRange;
            this.color = color;
            this.radius = radius;
            this.modulusSpeed = speed;
            this.sphereAtlas = sphereAtlas;
            Radius = radius;

            sphere = new AnimationSphere(sphereAtlas,explosionAtlas);
            cameraDistance = (background.X - screenDim.X) / Math.PI;
            cameraOrig = new Point((int)cameraDistance + zRange, (int)cameraDistance + zRange);
            position = new Vector3(zRange + (float)cameraDistance - radius, zRange + (float)cameraDistance - radius, (float)cameraDistance + zRange / 2 - radius);
            rand = new Random();
            vectSpeed = new Vector3(0, 0, 0);
            timeToSpeedChange = 0;
            totalElapsedTimePrev = 0;
            totalElapsedTimeDiff = 0;
            gammaMax = (background.Y - screenDim.Y) / (float)cameraDistance;
            gammaDeltaOff = 0.5 * (Math.PI - (background.Y - screenDim.Y) / cameraDistance);
            CalculateSafePoints();
            UpdateScale();
            UpdateAngles(viewSource);
        }
        //OK
        public Target(Texture2D texture, Rectangle viewSource, Point background, Point screenDim, int zRange, int radius, Color color, Texture2D sphereAtlas, Texture2D explosionAtlas)  //hittingGame
        {
            this.texture = texture;
            this.zRange = zRange;
            this.color = color;
            this.radius = radius;
            this.sphereAtlas = sphereAtlas;
            Radius = radius;

            sphere = new AnimationSphere(sphereAtlas,explosionAtlas);
            cameraDistance = (background.X - screenDim.X) / Math.PI;
            cameraOrig = new Point((int)cameraDistance + zRange, (int)cameraDistance + zRange);
            position = new Vector3(zRange + (float)cameraDistance - radius, zRange + (float)cameraDistance - radius, (float)cameraDistance + zRange / 2 - radius);
            rand = new Random();
            vectSpeed = new Vector3(0, 0, 0);
            modulusSpeed = 0;
            timeToSpeedChange = 0;
            totalElapsedTimePrev = 0;
            totalElapsedTimeDiff = 0;
            gammaMax = (background.Y - screenDim.Y) / (float)cameraDistance;
            gammaDeltaOff = 0.5 * (Math.PI - (background.Y - screenDim.Y) / cameraDistance);
            CalculateSafePoints();
            UpdateScale();
            UpdateAngles(viewSource);
        }

        //OK
        public bool Contains(Point pointToCheck)
        {
            Vector2 center = new Vector2(positionOnScreen.X + apparentRadius, positionOnScreen.Y + apparentRadius);  //center of the target on screen
            double dist = Math.Sqrt(Math.Pow(pointToCheck.X - center.X, 2) + Math.Pow(pointToCheck.Y - center.Y, 2));   //pitagora, distance between the center and the position

            if (dist <= apparentRadius)
                return true;
            else
                return false;
        }
        //OK
        public void ContinuousMove(double elapsedTime, double totalElapsedTime)  //used in TrackerGame
        {
            //Update target position in relation to speed
            position.X += (float)(vectSpeed.X * elapsedTime);
            position.Y += (float)(vectSpeed.Y * elapsedTime);
            position.Z += (float)(vectSpeed.Z * elapsedTime);
            UpdateScale();

            //true = over the limit
            bool zLimit = position.Z <= 0;
            bool farLimit = distance + radius >= cameraDistance + zRange;
            bool nearLimit = distance - radius <= cameraDistance;
            bool upLimit = position.Y >= position.Z * Math.Tan(Math.PI / 2 - gammaDeltaOff) + cameraOrig.Y;
            bool downLimit = position.Y <= -position.Z * Math.Tan(Math.PI / 2 - gammaDeltaOff) + cameraOrig.Y;

            if (zLimit || farLimit || nearLimit || upLimit || downLimit)
            {
                vectSpeed = VectDiff(p1, position, modulusSpeed);   //single safe point
                ////move towards the safe points
                //if (position.X <= cameraOrig.X) //separation made not respect the 3d center of the target, probably is not necessary that complication
                //{
                //    if (position.Y <= cameraOrig.Y)
                //        vectSpeed = VectDiff(p1, position, modulusSpeed);
                //    else
                //        vectSpeed = VectDiff(p3, position, modulusSpeed);
                //}
                //else
                //{
                //    if (position.Y <= cameraOrig.Y)
                //        vectSpeed = VectDiff(p2, position, modulusSpeed);
                //    else
                //        vectSpeed = VectDiff(p4, position, modulusSpeed);
                //}
            }
            else
                UpdateVectSpeed(totalElapsedTime);
        }
        //OK
        public void SpawnMove(Point screenDim)  //used in HittingGame
        {
                    int deltaModulus = rand.Next(2 * radius, screenDim.Y / 2);     //modulus of the shift, upper limit TO BE IMPROVED
            Vector3 futurePosition = position + deltaModulus * RandomDirection();

            Vector3 centerPosition = new Vector3(futurePosition.X + radius, futurePosition.Y + radius, futurePosition.Z + radius);    //target position referred to its 3d center
            Vector3 newReference = new Vector3(centerPosition.X - cameraOrig.X, centerPosition.Y - cameraOrig.Y, centerPosition.Z);       //new coordinate system with cameraOrig as (0, 0, 0)
            double futureDistance = Math.Sqrt(Math.Pow(newReference.X, 2) + Math.Pow(newReference.Y, 2) + Math.Pow(newReference.Z, 2));  //distance between camera and point

            //true = over the limit
            bool zLimit = futurePosition.Z <= 0;
            bool farLimit = futureDistance + radius >= cameraDistance + zRange;
            bool nearLimit = futureDistance - radius <= cameraDistance;
            bool upLimit = futurePosition.Y >= futurePosition.Z * Math.Tan(Math.PI / 2 - gammaDeltaOff) + cameraOrig.Y;
            bool downLimit = futurePosition.Y <= -futurePosition.Z * Math.Tan(Math.PI / 2 - gammaDeltaOff) + cameraOrig.Y;

            if (zLimit || farLimit || nearLimit || upLimit || downLimit)    //if the futurePosition exceed the limits, the next position is in direction of the "safe points"
            {
                position += VectDiff(p1, position, deltaModulus);    //single safe point
                //if (position.X <= cameraOrig.X) //separation made not respect the 3d center of the target, probably is not necessary that complication
                //{
                //    if (position.Y <= cameraOrig.Y)
                //        position = VectDiff(p1, position, deltaModulus);
                //    else
                //        position = VectDiff(p3, position, deltaModulus);
                //}
                //else
                //{
                //    if (position.Y <= cameraOrig.Y)
                //        position = VectDiff(p2, position, deltaModulus);
                //    else
                //        position = VectDiff(p4, position, deltaModulus);
                //}
            }
            else
                position = futurePosition;
            UpdateScale();
        }

        //OK
        public void Draw(SpriteBatch _spriteBatch, Point middleScreen, Rectangle viewSource)
        {
            UpdateAngles(viewSource);
            ProjectOnScreen(middleScreen);
            //_spriteBatch.Draw(texture, positionOnScreen, new Rectangle(0,0, texture.Width, texture.Height), color, 0, new Vector2(0,0), scale, SpriteEffects.None, 0);  //with origin in middle target is not working, the target is drawn in a different position respect the position on screen vector
            sphere.Draw(_spriteBatch, positionOnScreen, color, scale);
        }
        //OK
        private void UpdateScale()
        {
            Vector3 centerPosition = new Vector3(position.X + radius, position.Y + radius, position.Z + radius);    //target position referred to its 3d center
            Vector3 newReference = new Vector3(centerPosition.X - cameraOrig.X, centerPosition.Y - cameraOrig.Y, centerPosition.Z);       //new coordinate system with cameraOrig as (0, 0, 0)

            distance = Math.Sqrt(Math.Pow(newReference.X, 2) + Math.Pow(newReference.Y, 2) + Math.Pow(newReference.Z, 2));  //distance between camera and point
            apparentRadius = (int)(radius * (cameraDistance / distance));
            scale = (float)apparentRadius / (float)radius;
        }

        //OK
        private void ProjectOnScreen(Point middleScreen)
        {
            positionOnScreen.X = (float)(middleScreen.X + distance * Math.Sin(beta - alpha) - apparentRadius);
            positionOnScreen.Y = (float)(middleScreen.Y + distance * Math.Sin(delta - gamma) - apparentRadius);
        }
        //OK
        private Vector3 VectDiff(Vector3 posPoint, Vector3 minusPoint, float modulus)
        {
            double norm = Math.Sqrt(Math.Pow(posPoint.X - minusPoint.X, 2) + Math.Pow(posPoint.Y - minusPoint.Y, 2) + Math.Pow(posPoint.Z - minusPoint.Z, 2));    //norm 2 of posPoint-minusPoint
            float x = (float)(modulus * (posPoint.X - minusPoint.X) / norm);
            float y = (float)(modulus * (posPoint.Y - minusPoint.Y) / norm);
            float z = (float)(modulus * (posPoint.Z - minusPoint.Z) / norm);

            return new Vector3(x, y, z);
        }
        //OK
        private void UpdateVectSpeed(double totalElapsedTime)
        {
            totalElapsedTimeDiff = totalElapsedTime - totalElapsedTimePrev;

            if (timeToSpeedChange <= totalElapsedTimeDiff)
            {
                vectSpeed = modulusSpeed * RandomDirection();
                totalElapsedTimePrev = totalElapsedTime;
                timeToSpeedChange = rand.NextDouble() * 3;
            }
        }
        //OK
        private Vector3 RandomDirection()
        {
            Vector3 randomVector;

            if (1 == rand.Next(2))
                randomVector.X = rand.Next(0, 999);
            else
                randomVector.X = -rand.Next(0, 999);

            if (1 == rand.Next(2))
                randomVector.Y = rand.Next(0, 999);
            else
                randomVector.Y = -rand.Next(0, 999);

            if (1 == rand.Next(2))
                randomVector.Z = rand.Next(0, 999);
            else
                randomVector.Z = -rand.Next(0, 999);

            double norm = Math.Sqrt(Math.Pow(randomVector.X, 2) + Math.Pow(randomVector.Y, 2) + Math.Pow(randomVector.Z, 2));
            randomVector.X = (float)(randomVector.X / norm);
            randomVector.Y = (float)(randomVector.Y / norm);
            randomVector.Z = (float)(randomVector.Z / norm);

            return randomVector;
        }
        //OK
        private void UpdateAngles(Rectangle viewSource)
        {
            alpha = viewSource.X / cameraDistance;
            gamma = (viewSource.Y / cameraDistance) + gammaDeltaOff;
            beta = Math.Acos((cameraOrig.X - position.X - radius) / distance);
            delta = Math.Acos((cameraOrig.Y - position.Y - radius) / distance);
        }

        private void CalculateSafePoints()
        {
            float halfDistance = (float)cameraDistance + zRange / 2;
            p1 = new Vector3(cameraOrig.X - radius, cameraOrig.Y - radius, halfDistance + radius);     //single safe point

            //safe points at halfdistance and at anglemax/3
            //p2 = new Vector3(cameraOrig.X + halfDistance * 0.5f, cameraOrig.Y - halfDistance * (float)Math.Cos(gammaDeltaOff + gammaMax / 3), halfDistance * (float)Math.Sin(Math.PI / 3));
            //p4 = new Vector3(cameraOrig.X + halfDistance * 0.5f, cameraOrig.Y + halfDistance * (float)Math.Cos(gammaDeltaOff + gammaMax / 3), halfDistance * (float)Math.Sin(Math.PI / 3));
            //p1 = new Vector3(cameraOrig.X - halfDistance * 0.5f, cameraOrig.Y - halfDistance * (float)Math.Cos(gammaDeltaOff + gammaMax / 3), halfDistance * (float)Math.Sin(Math.PI / 3));
            //p3 = new Vector3(cameraOrig.X - halfDistance * 0.5f, cameraOrig.Y + halfDistance * (float)Math.Cos(gammaDeltaOff + gammaMax / 3), halfDistance * (float)Math.Sin(Math.PI / 3));

            //fixed safe point like the sheets
            //p1 = new Vector3(zRange, zRange, (float)cameraDistance + (zRange / 2));
            //p2 = new Vector3(zRange + 2 * (float)cameraDistance, zRange, (float)cameraDistance + (zRange / 2));
            //p3 = new Vector3(zRange, zRange + 2 * (float)cameraDistance, (float)cameraDistance + (zRange / 2));
            //p4 = new Vector3(zRange + 2 * (float)cameraDistance, zRange + 2 * (float)cameraDistance, (float)cameraDistance + (zRange / 2));
        }

        public Target CloneTarget()
        {
            Target copy = (Target)this.MemberwiseClone();
            copy.sphere = sphere.CloneSphere();
            return copy;
        }

    }
}
