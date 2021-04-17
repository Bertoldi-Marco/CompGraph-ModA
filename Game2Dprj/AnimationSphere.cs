using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class AnimationSphere
    {
        public bool isExploding;
        private Texture2D explosionAtlas;
        private Texture2D textureAtlas;
        private int frameRate;   //frame/second
        private double timeRef;
        private Point currentExplosionFrame;
        private Point currentFrame;
        private Point firstFrame;
        private Point lastFrame;
        private Point lastExplosionFrame;
        private Point distanceFromFrames;
        private Point distanceFromExplosionFrames;
        int irow;           //first row has index = 0
        int icol;           //first col has index = 0

        int irowexp;           //first row has index = 0
        int icolexp;

        public AnimationSphere(Texture2D textureAtlas,Texture2D explosionAtlas, int frameRate)
        {
            isExploding = false;
            this.explosionAtlas = explosionAtlas;
            this.textureAtlas = textureAtlas;
            this.frameRate = frameRate;
            timeRef = 0;
            distanceFromFrames = new Point(textureAtlas.Width/10, textureAtlas.Height/10);          //from texture atlas information
            distanceFromExplosionFrames = new Point(explosionAtlas.Width / 4, explosionAtlas.Height / 3);
            //setup first frame
            irow = 0;
            icol = 0;

            irowexp = -1;
            icolexp = 0;
            firstFrame = new Point(0, 0);
            currentFrame = firstFrame;
            //setup last frame
            lastFrame = new Point(5 * distanceFromFrames.X, 7 * distanceFromFrames.Y);

            //explosion frame
            currentExplosionFrame = new Point(0, 0);
            lastExplosionFrame = new Point(0, 2 * distanceFromExplosionFrames.Y);
        }


        public void Draw(SpriteBatch _spriteBatch, Vector2 positionOnScreen, Color color, float scale, double elapsedTime)
        {
            if (isExploding)
            {
                UpdateExplosionFramePos(elapsedTime);                                          //rivedibile il +80 per raddrizzare l'immagine
                _spriteBatch.Draw(explosionAtlas, new Vector2(positionOnScreen.X + 80 - distanceFromExplosionFrames.X / 2 , positionOnScreen.Y + 80 - distanceFromExplosionFrames.Y / 2), new Rectangle(currentExplosionFrame.X, currentExplosionFrame.Y, distanceFromExplosionFrames.X, distanceFromExplosionFrames.Y), color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            }
            else
            {
                UpdateFramePos(elapsedTime);
                _spriteBatch.Draw(textureAtlas, positionOnScreen, new Rectangle(currentFrame.X, currentFrame.Y, distanceFromFrames.X, distanceFromFrames.Y), color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
            }
        }

        private void UpdateFramePos(double elapsedTime)
        {
            timeRef += elapsedTime;
            int frames = (int)Math.Round(frameRate * timeRef);
            for (int i = 0; i < frames; i++)
            {
                if (currentFrame == lastFrame)
                {
                    currentFrame = firstFrame;
                    irow = 0;
                    icol = 0;
                }
                else
                {
                    irow++;
                    if (irow == 10)             //out of boundaries
                    {
                        irow = 0;
                        icol++;
                    }
                    currentFrame = new Point(irow * distanceFromFrames.X, icol * distanceFromFrames.Y);
                }
                timeRef = 0;
            }
        }

        private void UpdateExplosionFramePos(double elapsedTime)
        {
            timeRef += elapsedTime;
            int frames = (int)Math.Round(frameRate * timeRef);
            for (int i = 0; i < frames; i++)
            {
                irowexp++;
                if (irowexp == 3)             //out of boundaries
                {
                    irowexp = 0;
                    icolexp++;
                }
                currentExplosionFrame = new Point(irowexp * distanceFromExplosionFrames.X, icolexp * distanceFromExplosionFrames.Y);
                if (icolexp == 2)       //last frame
                {
                    isExploding = false;
                }
                timeRef = 0;
            }
        }

        public AnimationSphere CloneSphere()
        {
            AnimationSphere copy = (AnimationSphere)this.MemberwiseClone();
            copy.isExploding = true;
            return copy;
        }
    }
}
