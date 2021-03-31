using System;
using System.Collections.Generic;
using System.Text;

namespace Game2Dprj
{
    public class HittingGameEventArgs : EventArgs
    {
        public HittingGameEventArgs(int targetsDestroyed, int clicks, int totalTime)
        {
            TargetsDestroyed = targetsDestroyed;
            Clicks = clicks;
            TotalTime = totalTime;
        }

        public int TargetsDestroyed { get; set; }
        public int Clicks { get; set; }
        public int TotalTime { get; set; }
    }

    public class TrackerGameEventArgs : EventArgs
    {
        public TrackerGameEventArgs(int accuracy)
        {
            Accuracy = accuracy;
        }

        public int Accuracy { get; set; }
    }
}
