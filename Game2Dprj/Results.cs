using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class Results
    {

        private Button quitButton;
        private Button menuButton;

        private Rectangle quitRect;
        private Rectangle menuRect;
        private Point rectDimensions;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;
        //Statistics Hitting
        int targetsDestroyedH;
        int clicksH;
        int scoreH;
        float avgTimeToKillH;
        float accuracyH;
        //Statistics Tracker
        int scoreT;
        float accuracyT;
        float avgTimeOnT;
        //statistics Graphics
        Point graphicPos;
        //File reference
        string filePath;

        //Graphics variables
        Texture2D freccia;
        Texture2D pentagono;
        Texture2D triangolo;
        SpriteFont font;

        Statistics statistics;

        public Results(Point screenDim, GraphicsDevice graphicsDevice, Texture2D quitButtonText, Texture2D menuButtonText, Texture2D mouseMenuPointer, HittingGame hittingGame, TrackerGame trackerGame, Texture2D freccia, Texture2D pentagono, Texture2D triangolo, SpriteFont font)
        {
            newMouse = new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
            rectDimensions = new Point(480, 270);    //needs to be improved
            quitRect = new Rectangle(screenDim.X / 3 - rectDimensions.X / 2, screenDim.Y / 4 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            menuRect = new Rectangle(2 * (screenDim.X / 3) - rectDimensions.X / 2, screenDim.Y / 4 - rectDimensions.Y / 2, rectDimensions.X, rectDimensions.Y);
            graphicPos = new Point(screenDim.X / 2 - pentagono.Width / 2, (int)(screenDim.Y / 2.5f));

            quitButton = new Button(quitRect, quitButtonText, Color.Cyan);
            menuButton = new Button(menuRect, menuButtonText, Color.Cyan);

            filePath = @"SaveRecords.txt";

            Mouse.SetCursor(MouseCursor.FromTexture2D(mouseMenuPointer, mouseMenuPointer.Width / 2, mouseMenuPointer.Height / 2));
            hittingGame.endHittingGame += endHittingGameHandler;
            trackerGame.endTrackerGame += endTrackerGameHandler;
            this.freccia = freccia;
            this.pentagono = pentagono;
            this.triangolo = triangolo;
            this.font = font;
        }

        public void Update(ref SelectMode mode, MouseState prevMouse, Game1 game)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (menuButton.IsPressed(newMouse, oldMouse))
            {
                mode = SelectMode.menu;
            }

            if (quitButton.IsPressed(newMouse, oldMouse))
            {
                game.Exit();
            }

            if (statistics != null)
            {
                statistics.Update();
            }
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont font)
        {
            quitButton.Draw(_spriteBatch);
            menuButton.Draw(_spriteBatch);
            //_spriteBatch.DrawString(font, "Resume", new Vector2(resumeButton.rectangle.X + resumeButton.rectangle.Width / 2, resumeButton.rectangle.Y + resumeButton.rectangle.Height / 2), Color.Black);  //how to center respect to the string length?
            //_spriteBatch.DrawString(font, "Main Menu", new Vector2(menuButton.rectangle.X + menuButton.rectangle.Width / 2, menuButton.rectangle.Y + menuButton.rectangle.Height / 2), Color.Black);

            if (statistics!=null)
            {
                statistics.Draw(_spriteBatch);
            }
        }

        void endHittingGameHandler(object sender, HittingGameEventArgs e)            //this handler could be edited to be the handler of both games,using typeof sender object to determine which game is ended
        {
            int scoreRec = 0, targetsDestroyedRec = 0;
            float avgTimeToKillRec = 0, accuracyRec = 0;
            targetsDestroyedH = e.TargetsDestroyed;
            clicksH = e.Clicks;
            scoreH = e.Score;
            if (clicksH != 0) 
            {
                accuracyH = (100 * ((float)targetsDestroyedH / clicksH));
            }
            else
            {
                accuracyH = -1;
            }
            if (targetsDestroyedH != 0) 
            {
                //avgTimeToKill = string.Format("{0:0.00}", (float)e.TotalTime / targetsDestroyed);            //2 decimal
                avgTimeToKillH = (float)e.TotalTime / targetsDestroyedH;
            }
            else
            {
                avgTimeToKillH = -1;
            }
            ManageFile(filePath, ref scoreRec, ref targetsDestroyedRec, ref avgTimeToKillRec, ref accuracyRec);
            statistics = new Pentagon(pentagono, graphicPos, scoreH, targetsDestroyedH, avgTimeToKillH, 1, accuracyH, 0.8f, freccia, font);
        }

        void endTrackerGameHandler(object sender, TrackerGameEventArgs e)            //this handler could be edited to be the handler of both games,using typeof sender object to determine which game is ended
        {
            int scoreRec = 0;
            float avgTimeOnRec = 0, accuracyRec = 0;
            accuracyT = ((float)Math.Round(e.Accuracy,2));
            avgTimeOnT = ((float)Math.Round(e.AvgTimeOn, 2));
            scoreT = e.Score;
            ManageFile(filePath, ref scoreRec, ref avgTimeOnRec, ref accuracyRec);
            statistics = new Triangle(triangolo, graphicPos, scoreT, 1, accuracyT, 0.8f, freccia, font);  // aggiungere avgTimeOnT :D
        }

        // File format:
        // hittingGame -> "score targetsDestroyed avgTimeToKill accuracy"
        // trackerGame -> "score accuracy avgTimeOn"
        void ManageFile(string filePath, ref int scoreRec, ref float avgTimeOnRec, ref float accuracyRec)   //trackerGame version
        {
            string readFromFile = ReadFromFile(filePath);
            string[] gameValues = readFromFile.Split("\n");
            string[] hittingGameRecords = gameValues[0].Split(" ");
            string[] trackerGameRecords = gameValues[1].Split(" ");
            string updated = null;

            updated = hittingGameRecords[0] + " ";
            updated += hittingGameRecords[1] + " ";
            updated += hittingGameRecords[2] + " ";
            updated += hittingGameRecords[3] + "\n";

            if (trackerGameRecords[0] == "-" || trackerGameRecords[1] == "-" || trackerGameRecords[2] == "-")
            {
                updated += scoreT.ToString() + " ";
                updated += accuracyT.ToString() + " ";
                updated += avgTimeOnT.ToString();
                scoreRec = scoreT;
                avgTimeOnRec = avgTimeOnT;
                accuracyRec = accuracyT;
            }
            else
            {
                //check scoreT
                if (scoreT > int.Parse(trackerGameRecords[0]))
                {
                    updated += scoreT.ToString() + " ";
                    scoreRec = scoreT;
                }
                else
                {
                    updated += trackerGameRecords[0] + " ";
                    scoreRec = int.Parse(trackerGameRecords[0]);
                }

                //check accuracyT
                if (accuracyT > float.Parse(trackerGameRecords[1]))
                {
                    updated += accuracyT.ToString() + " ";
                    accuracyRec = accuracyT;
                }
                else
                {
                    updated += trackerGameRecords[1] + " ";
                    accuracyRec = float.Parse(trackerGameRecords[1]);
                }
                
                //check avgTimeOnT
                if (avgTimeOnT > float.Parse(trackerGameRecords[2]))
                {
                    updated += avgTimeOnT.ToString();
                    avgTimeOnRec = avgTimeOnT;
                }
                else
                {
                    updated += trackerGameRecords[2];
                    avgTimeOnRec = float.Parse(trackerGameRecords[2]);
                }
            }
            WriteToFile(filePath, updated);
        }

        void ManageFile(string filePath, ref int scoreRec, ref int targetsDestroyedRec, ref float avgTimeToKillRec, ref float accuracyRec)  //hittingGame version
        {
            string readFromFile = ReadFromFile(filePath);
            string[] gameValues = readFromFile.Split("\n");
            string[] hittingGameRecords = gameValues[0].Split(" ");
            string[] trackerGameRecords = gameValues[1].Split(" ");
            string updated = null;
           
            if (hittingGameRecords[0] == "-" || hittingGameRecords[1] == "-" || hittingGameRecords[2] == "-" || hittingGameRecords[3] == "-")
            {
                updated = scoreH.ToString() + " ";
                updated += targetsDestroyedH.ToString() + " ";
                updated += avgTimeToKillH.ToString() + " ";
                updated += accuracyH.ToString() + "\n";
                scoreRec = scoreH;
                targetsDestroyedRec = targetsDestroyedH;
                avgTimeToKillRec = avgTimeToKillH;
                accuracyRec = accuracyH;
            }
            else
            {
                //check scoreH
                if (scoreH > int.Parse(hittingGameRecords[0]))
                {
                    updated = scoreH.ToString() + " ";
                    scoreRec = scoreH;
                }
                else
                {
                    updated = hittingGameRecords[0] + " ";
                    scoreRec = int.Parse(hittingGameRecords[0]);
                }

                //check targetsDestroyedH
                if (targetsDestroyedH > int.Parse(hittingGameRecords[1]))
                {
                    updated += targetsDestroyedH.ToString() + " ";
                    targetsDestroyedRec = targetsDestroyedH;
                }
                else
                {
                    updated += hittingGameRecords[1] + " ";
                    targetsDestroyedRec = int.Parse(hittingGameRecords[1]);
                }
                
                //check avgTimeToKillH
                if (avgTimeToKillH < float.Parse(hittingGameRecords[2]))
                {
                    updated += avgTimeToKillH.ToString() + " ";
                    avgTimeToKillRec = avgTimeToKillH;
                }
                else
                {
                    updated += hittingGameRecords[2] + " ";
                    avgTimeToKillRec = float.Parse(hittingGameRecords[2]);
                }

                //check accuracyH
                if (accuracyH > float.Parse(hittingGameRecords[3]))
                {
                    updated += accuracyH.ToString() + "\n";
                    accuracyRec = accuracyH;
                }
                else
                {
                    updated += hittingGameRecords[3] + "\n";
                    accuracyRec = float.Parse(hittingGameRecords[3]);
                }
            }
            updated += trackerGameRecords[0] + " ";
            updated += trackerGameRecords[1] + " ";
            updated += trackerGameRecords[2];
            WriteToFile(filePath, updated);
        }

        void WriteToFile(string filePath, string toWrite)
        {
            StreamWriter file = null;
            try
            {
                file = new StreamWriter(filePath);
                file.Write(toWrite);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }

        string ReadFromFile(string filePath)
        {
            TextReader file = null;
            StreamWriter create = null;
            try
            {
                file = new StreamReader(filePath);
                string readed = file.ReadToEnd();
                return readed;
            }
            catch (FileNotFoundException e)
            {
                string defValues = "- - - -\n- - -";
                create = new StreamWriter(filePath);
                create.WriteLine(defValues);
                return defValues;
            }
            finally
            {
                if (file != null)
                    file.Close();
                if (create != null)
                    create.Close();
            }
        }
    }
}
