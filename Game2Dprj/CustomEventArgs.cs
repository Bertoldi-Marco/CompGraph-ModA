using System;
using System.Collections.Generic;
using System.Text;

namespace Game2Dprj
{
    public class HittingGameEventArgs : EventArgs
    {
        public HittingGameEventArgs(int targetsDestroyed, int clicks, int totalTime, int score)
        {
            TargetsDestroyed = targetsDestroyed;
            Clicks = clicks;
            TotalTime = totalTime;
            Score = score;
        }

        public int TargetsDestroyed { get; set; }
        public int Clicks { get; set; }
        public int TotalTime { get; set; }

        public int Score { get; set; }
    }

    public class TrackerGameEventArgs : EventArgs
    {
        public TrackerGameEventArgs(double accuracy, double avgTimeOn, int score)
        {
            Accuracy = accuracy;
            Score = score;
            AvgTimeOn = avgTimeOn;
        }

        public double Accuracy { get; set; }
        public int Score { get; set; }
        public double AvgTimeOn { get; set; }
    }
}
