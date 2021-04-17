using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game2Dprj
{
    public abstract class Statistics
    {
        public Texture2D freccia;
        public SpriteFont font;

        public Color red = new Color(200, 0, 0, 200);

        public Vector2 origin;
        public Vector2[] statsPos;
        public Vector2[] recordPos;

        public abstract void Draw(SpriteBatch _spriteBatch);
        public abstract void Update();

    }



    public class Pentagon : Statistics
    {
        Texture2D pentagono;

        Rectangle pentagonoRect;
        Point centroPentagono;
        float[] scale;
        float tempScale;

        float pentagonoScale;
        float maxScale;

        int score;
        int targetsDestroyed;
        string accuracy;
        string avgTimeToKill;
        string reactionTime;

        public Pentagon(Texture2D pentagono,Point pentagonoPos, float score, int targetsDestroyed, float timeToKill, float reactionTime, float accuracy, float pentagonoScale, Texture2D freccia, SpriteFont font)
        {
            if (accuracy == -1)
            {
                this.accuracy = "N / D";
            }
            else
            {
                this.accuracy = accuracy.ToString() + " %";
            }

            if (timeToKill == -1)
            {
                avgTimeToKill = "N / D";
            }
            else
            {
                avgTimeToKill = timeToKill.ToString() + " sec";
            }

            if (reactionTime == -1)
            {
                this.reactionTime = "N / D";
            }
            else
            {
                this.reactionTime = reactionTime.ToString() + " sec";
            }


            this.targetsDestroyed = targetsDestroyed;
            this.score = (int)score;
            this.pentagono = pentagono;
            this.pentagonoRect = new Rectangle(pentagonoPos.X, pentagonoPos.Y, (int)(pentagonoScale * pentagono.Width), (int)(pentagonoScale * pentagono.Height));
            this.centroPentagono = new Point((int)(503 * pentagonoScale) + pentagonoRect.X, (int)(350 * pentagonoScale) + pentagonoRect.Y);
            this.tempScale = 0.1f;
            this.pentagonoScale = pentagonoScale;
            this.maxScale = 1.15f * pentagonoScale;
            this.scale = new float[5] { maxScale * score / 100, maxScale * targetsDestroyed / 60, maxScale * 0.5f / timeToKill, maxScale * 0.01f / reactionTime, maxScale * accuracy / 100 };
            this.freccia = freccia;
            this.font = font;
            this.origin = new Vector2(freccia.Width / 2, freccia.Height / 2);

            //measured good position, based on the pentagon scale and position
            statsPos = new Vector2[5]
            {   new Vector2(110 * pentagonoScale + pentagonoRect.X, 232 * pentagonoScale + pentagonoRect.Y),
                new Vector2(471 * pentagonoScale + pentagonoRect.X, 86 * pentagonoScale + pentagonoRect.Y),
                new Vector2(760 * pentagonoScale + pentagonoRect.X, 232 * pentagonoScale + pentagonoRect.Y),
                new Vector2(170 * pentagonoScale + pentagonoRect.X, 540 * pentagonoScale + pentagonoRect.Y),
                new Vector2(660 * pentagonoScale + pentagonoRect.X, 540 * pentagonoScale + pentagonoRect.Y)
            };
            recordPos = new Vector2[5]
            {   new Vector2(23 * pentagonoScale + pentagonoRect.X, 345 * pentagonoScale + pentagonoRect.Y),
                new Vector2(777 * pentagonoScale + pentagonoRect.X, 45 * pentagonoScale + pentagonoRect.Y),
                new Vector2(900 * pentagonoScale + pentagonoRect.X, 340 * pentagonoScale + pentagonoRect.Y),
                new Vector2(200 * pentagonoScale + pentagonoRect.X, 470 * pentagonoScale + pentagonoRect.Y),
                new Vector2(890 * pentagonoScale + pentagonoRect.X, 470 * pentagonoScale + pentagonoRect.Y)
            };
        }

        public override void Update()
        {
            if (tempScale < 1)
            {
                tempScale += 0.008f;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(pentagono, pentagonoRect, Color.White);

            for (int i = 0; i < 5; i++)
            {
                _spriteBatch.Draw(freccia, new Rectangle(centroPentagono.X, centroPentagono.Y, (int)(0.9 * freccia.Width), (int)(tempScale * maxScale * freccia.Height)), null, red, (float)((2 * i * Math.PI) / 5), origin, SpriteEffects.None, 0f);
                //_spriteBatch.Draw(freccia, new Vector2(centroPentagono.X, centroPentagono.Y), null, Color.White, (float)((2 * i * Math.PI) / 5), origin, scale[i], SpriteEffects.None, 0f);
            }

            for (int i = 0; i < 5; i++)
            {
                _spriteBatch.Draw(freccia, new Rectangle(centroPentagono.X, centroPentagono.Y, (int)(0.9 * freccia.Width / 2), (int)(tempScale * scale[i] * freccia.Height)), null, Color.Blue, (float)((2 * i * Math.PI) / 5), origin, SpriteEffects.None, 0f);
                //_spriteBatch.Draw(freccia, new Vector2(centroPentagono.X, centroPentagono.Y), null, Color.White, (float)((2 * i * Math.PI) / 5), origin, scale[i], SpriteEffects.None, 0f);
            }

            _spriteBatch.DrawString(font, accuracy, statsPos[0], Color.Blue, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, score.ToString(), statsPos[1], Color.Blue, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, targetsDestroyed.ToString(), statsPos[2], Color.Blue, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, reactionTime, statsPos[3], Color.Blue, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, avgTimeToKill, statsPos[4], Color.Blue, 0f, new Vector2(0, 0), 1.4f, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(font, "90.2%", recordPos[0], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "42", recordPos[1], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "33", recordPos[2], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "1.1 sec", recordPos[3], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "1.5 sec", recordPos[4], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

        }
    }

    public class Triangle : Statistics
    {
        Texture2D triangolo;

        Rectangle TriangoloRect;
        Point centroTriangolo;
        float[] scale;
        float tempScale;

        float maxScale;

        int score;
        string reactionTime;
        string accuracy;

        public Triangle(Texture2D triangolo, Point triangoloPos, int score, float reactionTime, float accuracy, float pentagonoScale, Texture2D freccia, SpriteFont font)
        {
            this.score = score;
            this.reactionTime = reactionTime.ToString() + " sec";
            this.accuracy = accuracy.ToString() + " %";
            this.triangolo = triangolo;
            this.TriangoloRect = new Rectangle(triangoloPos.X, triangoloPos.Y, (int)(pentagonoScale * triangolo.Width), (int)(pentagonoScale * triangolo.Height));
            this.centroTriangolo = new Point((int)(503 * pentagonoScale) + TriangoloRect.X, (int)(370 * pentagonoScale) + TriangoloRect.Y);
            this.tempScale = 0.1f;
            this.maxScale = 1.15f * pentagonoScale;
            this.scale = new float[3] { maxScale * score / 100, maxScale * accuracy / 100, maxScale * 1 / reactionTime };
            this.freccia = freccia;
            this.font = font;
            this.origin = new Vector2(freccia.Width / 2, freccia.Height/2);

            //measured good position, based on the pentagon scale and position
            statsPos = new Vector2[3]
            {   new Vector2(471 * pentagonoScale + TriangoloRect.X, 86 * pentagonoScale + TriangoloRect.Y),
                new Vector2(792 * pentagonoScale + TriangoloRect.X, 540 * pentagonoScale + TriangoloRect.Y),
                new Vector2(70 * pentagonoScale + TriangoloRect.X, 540 * pentagonoScale + TriangoloRect.Y),
            };
            recordPos = new Vector2[3]
            {   new Vector2(800 * pentagonoScale + TriangoloRect.X, 45 * pentagonoScale + TriangoloRect.Y),
                new Vector2(800 * pentagonoScale + TriangoloRect.X, 613 * pentagonoScale + TriangoloRect.Y),
                new Vector2(225 * pentagonoScale + TriangoloRect.X, 613 * pentagonoScale + TriangoloRect.Y)
            };
        }

        public override void Update()
        {
            if (tempScale < 1)
            {
                tempScale += 0.008f;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(triangolo, TriangoloRect, Color.White);

            for (int i = 0; i < 3; i++)
            {
                _spriteBatch.Draw(freccia, new Rectangle(centroTriangolo.X, centroTriangolo.Y, (int)(0.9 * freccia.Width), (int)(tempScale * maxScale * freccia.Height)), null, red, (float)((2 * i * Math.PI) / 3), origin, SpriteEffects.None, 0f);
                //_spriteBatch.Draw(freccia, new Vector2(centroPentagono.X, centroPentagono.Y), null, Color.White, (float)((2 * i * Math.PI) / 5), origin, scale[i], SpriteEffects.None, 0f);
            }

            for (int i = 0; i < 3; i++)
            {
                _spriteBatch.Draw(freccia, new Rectangle(centroTriangolo.X, centroTriangolo.Y, (int)(0.9 * freccia.Width / 2), (int)(tempScale * scale[i] * freccia.Height)), null, Color.Blue, (float)((2 * i * Math.PI) / 3), origin, SpriteEffects.None, 0f);
                //_spriteBatch.Draw(freccia, new Vector2(centroPentagono.X, centroPentagono.Y), null, Color.White, (float)((2 * i * Math.PI) / 5), origin, scale[i], SpriteEffects.None, 0f);
            }

            _spriteBatch.DrawString(font, score.ToString(), statsPos[0], Color.Blue, 0f, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, accuracy, statsPos[1], Color.Blue, 0f, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, reactionTime, statsPos[2], Color.Blue, 0f, new Vector2(0, 0), 1.2f, SpriteEffects.None, 0f);

            _spriteBatch.DrawString(font, "80", recordPos[0], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "90 %", recordPos[1], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(font, "1.5 sec", recordPos[2], red, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

        }
    }
}
